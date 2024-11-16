<%@ WebHandler Language="C#" Class="bagridexportexposure" %>

using System;
using System.Web;
using WebComponent;
using System.Data;
using System.Collections;

public class bagridexportexposure : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        try
        {
            if (CustomSession.Session("Login_ClientId") == null)
            {
                context.Response.Write("Session Expired");
                return;
            }
            int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
            string m = Common.GetQueryString("m");
            if (context.Request.QueryString["a"] == "d")
            {
                DeleteData(m, clientId);
                return;
            }
            int id = GlobalUtilities.ConvertToInt(context.Request.Form["txtba_hdnid"]);
            if (!IsValid(m, id, clientId)) return;
            Hashtable hstbl = new Hashtable();
            AddCalculatedColumns(hstbl, clientId);
            
            for (int i = 0; i < context.Request.Form.Keys.Count; i++)
            {
                string key = context.Request.Form.Keys[i];
                if (key.StartsWith("txtba_") && key != "txtba_hdnid")
                {
                    string columnName = key.Replace("txtba_", "");
                    string val = global.CheckInputData(context.Request.Form[key]);
                    if (columnName.EndsWith("-dbl"))
                    {
                        columnName = columnName.Replace("-dbl", "");
                        val = GlobalUtilities.ConvertToDouble(val).ToString();
                    }
                    else if (columnName.EndsWith("-i"))
                    {
                        columnName = columnName.Replace("-i", "");
                        val = GlobalUtilities.ConvertToInt(val).ToString();
                    }
                    else if (columnName.EndsWith("-dt"))
                    {
                        columnName = columnName.Replace("-dt", "");
                        //if (val != "")
                        //{
                        //    val = GlobalUtilities.ConvertMMDateToDD(val);
                        //}
                    }
                    if (!hstbl.Contains(columnName))
                    {
                        hstbl.Add(columnName, val);
                    }
                }
            }
            hstbl.Add("clientid", clientId);
            InsertUpdate obj = new InsertUpdate();
            int newid = 0;
            if (id == 0)
            {
                newid = obj.InsertData(hstbl, "tbl_" + m);
            }
            else
            {
                newid = obj.UpdateData(hstbl, "tbl_" + m, id);
            }
            //
            if (newid > 0)
            {
                m = Common.GetQueryString("m");
                string orderNo = HttpContext.Current.Request.Form["txtba_exportorderno"];
                CalculateAfterSave(clientId, m, newid);
                UpdateExportOrder(orderNo);
                
                string query = "select " + m + "_" + m + "id as id,* from tbl_" + m + " where " + m + "_" + m + "id=" + newid;
                DataRow dr = DbTable.ExecuteSelectRow(query);
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    if (dr.Table.Columns[i].DataType == typeof(System.Decimal))
                    {
                        string val = GlobalUtilities.ConvertToString(dr[i]);
                        dr[i] = Common.FormatAmountComma(val);
                    }
                }
                string data = JSON.ConvertAmountComma(dr, "", true);
                context.Response.Write(data);
            }
            else
            {
                context.Response.Write("Error");
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("Error : " + ex.Message);
        }
    }
    private void DeleteData(string m, int clientId)
    {
        string query = "";
        int id = Common.GetQueryStringValue("id");
        query = "select * from tbl_" + m + " where " + m + "_" + m + "id=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return;
        
        query = "delete from tbl_" + m + " where " + m + "_" + m + "id=" + id + " AND " + m + "_clientid=" + clientId;
        DbTable.ExecuteQuery(query);
        
        if (m == "exportorder")
        {
        }
        else
        {
            string orderNo = GlobalUtilities.ConvertToString(dr[m+"_exportorderno"]);
            UpdateExportOrder(orderNo);
        }
        
        HttpContext.Current.Response.Write("Ok");
    }
    private bool IsValid(string m, int id, int clientId)
    {
        string query = "";
        if (m == "exportorder")
        {
            string orderNo = HttpContext.Current.Request.Form["txtba_exportorderno"];
            query = "select * from tbl_exportorder WHERE exportorder_exportorderno='" + orderNo + "' AND exportorder_clientid=" + clientId;
            if (id > 0)
            {
                query += " AND exportorder_exportorderid<>" + id;
            }
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (dr != null)
            {
                HttpContext.Current.Response.Write("Error : Order number already exists!");
                return false;
            }
        }
        else if (m == "forwardcontract")
        {
            string bookingNo = HttpContext.Current.Request.Form["txtba_bookingno"].Trim();
            if (bookingNo == "") return true;
            query = "select * from tbl_forwardcontract WHERE forwardcontract_bookingno='" + bookingNo + "' AND forwardcontract_clientid=" + clientId;
            if (id > 0)
            {
                query += " AND forwardcontract_forwardcontractid<>" + id;
            }
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (dr != null)
            {
                HttpContext.Current.Response.Write("Error : Booking number already exists!");
                return false;
            }
        }
        else if (m == "pcfc")
        {
            string pcfcNo = HttpContext.Current.Request.Form["txtba_pcfcno"].Trim();
            if (pcfcNo == "") return false;
            query = "select * from tbl_pcfc WHERE pcfc_pcfcno='" + pcfcNo + "' AND pcfc_clientid=" + clientId;
            if (id > 0)
            {
                query += " AND pcfc_pcfcid<>" + id;
            }
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (dr != null)
            {
                HttpContext.Current.Response.Write("Error : PCFC number already exists!");
                return false;
            }
        }
        return true;
    }
    private void CalculateAfterSave(int clientId, string m, int id)
    {
        string query = "";
        if (m == "forwardcontract")
        {
            query = "select * from tbl_forwardcontract where forwardcontract_forwardcontractid=" + id;
            DataRow dr = DbTable.ExecuteSelectRow(query);
            DateTime todate = Convert.ToDateTime(dr["forwardcontract_to"]);
            //get spot date
            int currency = GlobalUtilities.ConvertToInt(dr["forwardcontract_exposurecurrencymasterid"]);
            int liverateId = 0;
            int spotrateLiveRateId = 0;
            if (currency == 1)
            {
                liverateId = 194; spotrateLiveRateId = 1;
            }
            else if (currency == 2)
            {
                liverateId = 304; spotrateLiveRateId = 10;
            }
            else if (currency == 3)
            {
                liverateId = 409; spotrateLiveRateId = 19;
            }
            else if (currency == 4)
            {
                liverateId = 513; spotrateLiveRateId = 28;
            }
            query = "select * from tbl_liverate where liverate_liverateid=" + liverateId;
            DataRow drliverate = DbTable.ExecuteSelectRow(query);
            DateTime spotDate = todate;
            if (drliverate != null)
            {
                spotDate = Convert.ToDateTime(drliverate["liverate_currentrate"]);
            }
            TimeSpan sp = spotDate - todate;
            int daydiff = sp.Days;
            double mtmrate = 0;
            double spotrate = 0;
            double PandLamount = 0;
            //get spotrate
            query = "select * from tbl_liverate where liverate_liverateid=" + spotrateLiveRateId;
            DataRow drliverate_spotrate = DbTable.ExecuteSelectRow(query);
            if (drliverate_spotrate != null)
            {
                spotrate = GlobalUtilities.ConvertToDouble(drliverate_spotrate["liverate_currentrate"]);
            }
            double dblPremium = FindPremiumRate(spotDate, todate, currency, true);
            if (daydiff > 0)
            {
                mtmrate = (spotrate + dblPremium) / 100;
            }
            if (mtmrate > 0)
            {
                double balanceSold = GlobalUtilities.ConvertToDouble(dr["forwardcontract_balancesold"]);
                double rate = GlobalUtilities.ConvertToDouble(dr["forwardcontract_rate"]);
                PandLamount = balanceSold * (rate * mtmrate);
            }
            query = "update tbl_forwardcontract set forwardcontract_mtmrate=" + mtmrate + ",forwardcontract_profitandlossamount="+PandLamount+
                    " where forwardcontract_forwardcontractid=" + id;
            DbTable.ExecuteQuery(query);
            
        }
    }
    
    private void UpdateExportOrder(string orderNo)
    {
        if (orderNo == "") return;
        Hashtable hstbl = new Hashtable();
        string query = "";
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        query = "select * from tbl_exportorder where exportorder_exportorderno='" + orderNo + "' AND exportorder_clientid=" + clientId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return;
        
        double netamount = 0;
        int orderId = GlobalUtilities.ConvertToInt(dr["exportorder_exportorderid"]);
        double amountReceived = GlobalUtilities.ConvertToDouble(dr["exportorder_amountreceived"]);
        double exportOrderAmount = GlobalUtilities.ConvertToDouble(dr["exportorder_exportorderamount"]);
        if (amountReceived <= exportOrderAmount)
        {
            netamount = exportOrderAmount - amountReceived;
        }
        hstbl.Add("netamount", netamount);

        //value
        double value = 0;
        double costing = GlobalUtilities.ConvertToDouble(dr["exportorder_costing"]);
        value = costing * netamount;
        hstbl.Add("value", value);

        //forwardbooking
        //string orderNo = GlobalUtilities.ConvertToString(dr["exportorder_exportorderno"]);
        double forwardBookingAmount = 0;
        query = "select * from tbl_forwardcontract where forwardcontract_exportorderno='" + orderNo + "' AND forwardcontract_clientid=" + clientId;
        DataTable dttblForward = DbTable.ExecuteSelect(query);
        double sumSold = 0;
        double sumMulSold = 0;
        double forwardBookingRate = 0;
        if (GlobalUtilities.IsValidaTable(dttblForward))
        {
            for (int i = 0; i < dttblForward.Rows.Count; i++)
            {
                double balanceSold = GlobalUtilities.ConvertToDouble(dttblForward.Rows[i]["forwardcontract_balancesold"]);
                forwardBookingAmount += balanceSold;
                //double sold = GlobalUtilities.ConvertToDouble(dttblForward.Rows[i]["forwardcontract_sold"]);
                double rate = GlobalUtilities.ConvertToDouble(dttblForward.Rows[i]["forwardcontract_rate"]);
                sumMulSold += balanceSold * rate;
                sumSold += balanceSold;
            }
            if (sumSold > 0)
            {
                forwardBookingRate = sumMulSold / sumSold;
            }
        }
        hstbl.Add("forwardbookingamount", forwardBookingAmount);
        hstbl.Add("forwardbookingrate", forwardBookingRate);
        //pcfc
        query = "select * from tbl_pcfc where pcfc_exportorderno='" + orderNo + "' AND pcfc_clientid=" + clientId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        double pcfcAmount = 0;
        double sumMulSold_pcfc = 0;
        double pcfcRate = 0;
        //calculate on pcfc
        if (GlobalUtilities.IsValidaTable(dttbl))
        {
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                double fcamountbalance = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["pcfc_fcamountbalance"]);
                pcfcAmount += fcamountbalance;
                double rate = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["pcfc_spotrate"]);
                sumMulSold_pcfc += fcamountbalance * rate;
            }
            if (pcfcAmount > 0)
            {
                pcfcRate = sumMulSold_pcfc / pcfcAmount;
            }
        }
        hstbl.Add("pcfcamount", pcfcAmount);
        hstbl.Add("pcfcrate", pcfcRate);

        //unhedged amount
        double unhedgedamount = 0;
        unhedgedamount = netamount - forwardBookingAmount - pcfcAmount;
        hstbl.Add("unhedgedamount", unhedgedamount);

        //effective rate
        double effectiveRate = 0;
        double div = forwardBookingAmount + pcfcAmount;
        if (div > 0)
        {
            effectiveRate = (forwardBookingAmount * forwardBookingRate + pcfcAmount * pcfcRate) / div;
        }
        hstbl.Add("effectiverate", effectiveRate);
        InsertUpdate obj = new InsertUpdate();
        int id = obj.UpdateData(hstbl, "tbl_exportorder", orderId);
    }
    private void AddCalculatedColumns(Hashtable hstbl, int clientId)
    {
        string m = Common.GetQueryString("m");
        if (m == "pcfc")
        {
            //pfc due date
            string pcfcdate = HttpContext.Current.Request.Form["txtba_pcfcdate-dt"];
            int days = GlobalUtilities.ConvertToInt(HttpContext.Current.Request.Form["txtba_days-i"]);
            if (pcfcdate.Trim() != "")
            {
                Array arr = pcfcdate.Split('-');
                int day = Convert.ToInt32(arr.GetValue(0));
                int month = Convert.ToInt32(arr.GetValue(1));
                int year = Convert.ToInt32(arr.GetValue(2));
                DateTime dtpcdueDate = new DateTime(year, month, day);
                
                dtpcdueDate = dtpcdueDate.AddDays(days);
                day = dtpcdueDate.Day;
                month = dtpcdueDate.Month;
                year = dtpcdueDate.Year;
                string strday = day.ToString();
                string strmonth = month.ToString();
                if (strday.Length == 1) strday = "0" + strday;
                if (strmonth.Length == 1) strmonth = "0" + strmonth;
                string duedate = strday + "-" + strmonth + "-" + year;
                hstbl.Add("pcduedate", duedate);
            }
            //fc balance amount
            double fcamount = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_fcamount-dbl"]);
            double repayment = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_repayment-dbl"]);
            double fcamountbal = fcamount - repayment;
            hstbl.Add("fcamountbalance", fcamountbal);
            
            //product
            double product = 0;
            double spotrate = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_spotrate-dbl"]);
            product = fcamountbal * spotrate;
            hstbl.Add("product", product);
            
            //pcfc interest amount
            double pcfcinterestamount = 0;
            double interestrate = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_interestrate-dbl"]);
            pcfcinterestamount = interestrate * fcamount * days / 360.0;
            pcfcinterestamount = pcfcinterestamount / 100.0;
            hstbl.Add("pcfcinterestamount", pcfcinterestamount);
            
            
        }
        else if (m == "forwardcontract")
        {
            //balance sold
            double balancesold = 0;
            double sold = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_sold-dbl"]);
            double utilised = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_utilised-dbl"]);
            double cancellation = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_cancellation-dbl"]);
            if (utilised <= sold)
            {
                balancesold = sold - utilised - cancellation;
            }
            hstbl.Add("balancesold", balancesold);
            
            //sold amount in rs
            double soldAmountInRs = 0;
            double rate = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_rate-dbl"]);
            soldAmountInRs = rate * balancesold;
            hstbl.Add("soldamountinrs", soldAmountInRs);
            
            //profit loss utilisation
            double spotRateUtilized = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_spotrateutilisedcancel-dbl"]);
            double PLUtilisation = utilised * (rate - spotRateUtilized);
            hstbl.Add("profitandlossonutilisation", PLUtilisation);
            
            //profit loss on utilization cancel
            //double cancelationAmount = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_cancellationamount-dbl"]);
            double spotRateCancel = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_spotratecancellation-dbl"]);
            //double PLOnCancel = cancelationAmount * (rate - utilizedcancel);
            double PLOnCancel = cancellation * (rate - spotRateCancel);
            hstbl.Add("profitandlossoncancellation", PLOnCancel);
            
        }
        else if (m == "exportorder")
        {
            double netamount = 0;
            double amountReceived = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_amountreceived-dbl"]);
            double exportOrderAmount = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_exportorderamount-dbl"]);
            if (amountReceived <= exportOrderAmount)
            {
                netamount = exportOrderAmount - amountReceived;
            }
            hstbl.Add("netamount", netamount);
            
            //value
            double value = 0;
            double costing = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_costing-dbl"]);
            value = costing * netamount;
            hstbl.Add("value", value);

            //forwardbooking
            string orderNo = HttpContext.Current.Request.Form["txtba_exportorderno"];
            double forwardBookingAmount = 0;
            string query = "select * from tbl_forwardcontract where forwardcontract_exportorderno='" + orderNo + "' AND forwardcontract_clientid="+clientId;
            DataTable dttblForward = DbTable.ExecuteSelect(query);
            double sumSold = 0;
            double sumMulSold = 0;
            double forwardBookingRate = 0;
            if (GlobalUtilities.IsValidaTable(dttblForward))
            {
                for (int i = 0; i < dttblForward.Rows.Count; i++)
                {
                    forwardBookingAmount += GlobalUtilities.ConvertToDouble(dttblForward.Rows[i]["forwardcontract_balancesold"]);
                    double sold = GlobalUtilities.ConvertToDouble(dttblForward.Rows[i]["forwardcontract_sold"]);
                    double rate = GlobalUtilities.ConvertToDouble(dttblForward.Rows[i]["forwardcontract_rate"]);
                    sumMulSold += sold * rate;
                    sumSold += sold;
                }
                if (sumSold > 0)
                {
                    forwardBookingRate = sumMulSold / sumSold;
                }
            }
            hstbl.Add("forwardbookingamount", forwardBookingAmount);
            hstbl.Add("forwardbookingrate", forwardBookingRate);
            //pcfc
            query = "select * from tbl_pcfc where pcfc_exportorderno='" + orderNo + "' AND pcfc_clientid="+clientId;
            DataTable dttbl = DbTable.ExecuteSelect(query);
            double pcfcAmount = 0;
            double sumMulSold_pcfc = 0;
            double pcfcRate = 0;
            //calculate on pcfc
            if (GlobalUtilities.IsValidaTable(dttbl))
            {
                for (int i = 0; i < dttbl.Rows.Count; i++)
                {
                    double fcamountbalance = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["pcfc_fcamountbalance"]);
                    pcfcAmount += fcamountbalance;
                    double rate = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["pcfc_spotrate"]);
                    sumMulSold_pcfc += fcamountbalance * rate;
                }
                if (pcfcAmount > 0)
                {
                    pcfcRate = sumMulSold_pcfc / pcfcAmount;
                }
            }
            hstbl.Add("pcfcamount", pcfcAmount);
            hstbl.Add("pcfcrate", pcfcRate);
            
            //unhedged amount
            double unhedgedamount = 0;
            unhedgedamount = netamount - forwardBookingAmount - pcfcAmount;
            hstbl.Add("unhedgedamount", unhedgedamount);
            
            //effective rate
            double effectiveRate = 0;
            double div = forwardBookingAmount + pcfcAmount;
            if (div > 0)
            {
                effectiveRate = (forwardBookingAmount * forwardBookingRate + pcfcAmount * pcfcRate)/div;
            }
            hstbl.Add("effectiverate", effectiveRate);
            
        }
    }
    private double FindPremiumRate(DateTime spotDate, DateTime dtBrokenDate, int currency, bool isExport)
    {
        int endDateRateId_Start = 0;
        int endDateRateId_End = 0;
        int rateId_Start = 0;
        int rateId_End = 0;
        double dblDivisionFactor = 0;
        double dblForwardRateDivFactor = 1;

        try
        {
            //validate the date first
            //if (!IsValidDate(dtBrokenDate, currency))
            //{
            //    return;
            //}

            if (currency == 1)//USDINR
            {
                endDateRateId_Start = 195;
                if (isExport)
                {
                    rateId_Start = 208;
                }
                else
                {
                    rateId_Start = 221;
                }
                dblDivisionFactor = 100;
            }
            else if (currency == 2)//EURINR
            {
                endDateRateId_Start = 305;
                if (isExport)
                {
                    rateId_Start = 318;
                }
                else
                {
                    rateId_Start = 331;
                }
                dblDivisionFactor = 100;
            }
            else if (currency == 3)//GBPINR
            {
                endDateRateId_Start = 410;
                if (isExport)
                {
                    rateId_Start = 423;
                }
                else
                {
                    rateId_Start = 436;
                }
                dblDivisionFactor = 100;
                dblForwardRateDivFactor = 100;
            }
            else if (currency == 4)//JPYINR
            {
                endDateRateId_Start = 514;
                if (isExport)
                {
                    rateId_Start = 527;
                }
                else
                {
                    rateId_Start = 540;
                }
                dblDivisionFactor = 100;
                dblForwardRateDivFactor = 100;
            }
            else if (currency == 5)//EURUSD
            {
                endDateRateId_Start = 618;
                if (isExport)
                {
                    rateId_Start = 631;
                }
                else
                {
                    rateId_Start = 644;
                }
                dblDivisionFactor = 10000;
            }
            else if (currency == 6)//GBPUSD
            {
                endDateRateId_Start = 722;
                if (isExport)
                {
                    rateId_Start = 735;
                }
                else
                {
                    rateId_Start = 748;
                }
                dblDivisionFactor = 10000;
                dblForwardRateDivFactor = 10;
            }
            else if (currency == 7)//USDJPY
            {
                endDateRateId_Start = 826;
                if (isExport)
                {
                    rateId_Start = 839;
                }
                else
                {
                    rateId_Start = 852;
                }
                dblDivisionFactor = 0;
                dblForwardRateDivFactor = 100;
            }
            rateId_End = rateId_Start + 11;

            DataTable dttblMonthEndDate = GetLiveRatesBetween(endDateRateId_Start, endDateRateId_Start + 11);

            //find forward date
            int brokenDateMonth = dtBrokenDate.Month;
            int brokenDateYear = dtBrokenDate.Year;
            int forwardDateIndex = -1;
            DateTime dtForwardDate = DateTime.MinValue;

            for (int i = 0; i < dttblMonthEndDate.Rows.Count; i++)
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(dttblMonthEndDate.Rows[i]["liverate_currentrate"]);
                    TimeSpan sp1 = dt - dtBrokenDate;
                    if (sp1.Days >= 0)
                    {
                        forwardDateIndex = i;
                        dtForwardDate = dt;
                        break;
                    }
                }
                catch { }
            }
            if (forwardDateIndex < 0)
            {
                return 0;
            }
            //find liverate import/export value which is near to broken date
            DataTable dttblRates = GetLiveRatesBetween(rateId_Start, rateId_End);
            double forwardRate = GlobalUtilities.ConvertToDouble(dttblRates.Rows[forwardDateIndex]["liverate_currentrate"]);

            forwardRate = forwardRate / dblForwardRateDivFactor;

            //find days = Forward Date - Spot Date
            TimeSpan sp = dtBrokenDate - spotDate;
            int days = sp.Days;

            //find premium = SpotRate * (SpotRate / (Broken Date - Spot Date))
            //first find the Broken Date - Spot Date
            sp = dtForwardDate - spotDate;
            int brokenDateDiff = sp.Days;

            //per day
            double dblPerday = forwardRate / brokenDateDiff;

            double dblPremium = dblPerday * days;

            return dblPremium;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
    private DataTable GetLiveRatesBetween(int start, int end)
    {
        string query = "select * from tbl_liverate where liverate_liverateid between " + start + " and " + end + " order by liverate_liverateid";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        return dttbl;
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}