<%@ WebHandler Language="C#" Class="bagrid" %>

using System;
using System.Web;
using WebComponent;
using System.Data;
using System.Collections;

public class bagrid : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
            Calculate(clientId);
            if (newid > 0)
            {
                string query = "select " + m + "_" + m + "id as id,* from tbl_" + m + " where " + m + "_" + m + "id=" + newid;
                DataRow dr = DbTable.ExecuteSelectRow(query);
                string data = JSON.Convert(dr, "", true);
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
            if (bookingNo == "") return false;
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
    private void Calculate(int clientId)
    {
        string m = Common.GetQueryString("m");
        string query = "";
        string orderNo = HttpContext.Current.Request.Form["txtba_exportorderno"];
        if (orderNo == "") return;
        if (m == "forwardcontract")
        {
            query = "select * from tbl_forwardcontract where forwardcontract_exportorderno='" + orderNo + "' AND forwardcontract_clientid="+clientId;
            DataTable dttbl = DbTable.ExecuteSelect(query);
            double forwardBookingAmount = 0;
            double sumSold = 0;
            double sumMulSold = 0;
            double weightRate = 0;   
            //calculate on forward contract
            if (GlobalUtilities.IsValidaTable(dttbl))
            {
                for (int i = 0; i < dttbl.Rows.Count; i++)
                {
                    forwardBookingAmount += GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["forwardcontract_balancesold"]);
                    double sold = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["forwardcontract_sold"]);
                    double rate = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["forwardcontract_rate"]);
                    sumMulSold += sold * rate;
                    sumSold += sold;
                }
                if (sumSold > 0)
                {
                    weightRate = sumMulSold / sumSold;
                }
                
            }
            query = "update tbl_exportorder set exportorder_forwardbookingamount=" + forwardBookingAmount + "," +
                         "exportorder_forwardbookingrate=" + weightRate + " where exportorder_exportorderno='" + orderNo + "' AND exportorder_clientid="+clientId;
            DbTable.ExecuteQuery(query);
        }
        else if (m == "pcfc")
        {
            query = "select * from tbl_pcfc where pcfc_exportorderno='" + orderNo + "' AND pcfc_clientid="+clientId;
            DataTable dttbl = DbTable.ExecuteSelect(query);
            double fcamountBalanceSum = 0;
            double sumMulSold = 0;
            double weightRate = 0;
            //calculate on pcfc
            if (GlobalUtilities.IsValidaTable(dttbl))
            {
                for (int i = 0; i < dttbl.Rows.Count; i++)
                {
                    double fcamountbalance = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["pcfc_fcamountbalance"]);
                    fcamountBalanceSum += fcamountbalance;
                    double rate = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["pcfc_spotrate"]);
                    sumMulSold += fcamountbalance * rate;
                }
                if (fcamountBalanceSum > 0)
                {
                    weightRate = sumMulSold / fcamountBalanceSum;
                }
            }
            query = "update tbl_exportorder set exportorder_pcfcamount=" + fcamountBalanceSum + "," +
                         "exportorder_pcfcrate=" + weightRate + " where exportorder_exportorderno='" + orderNo + "' AND exportorder_clientid="+clientId;
            DbTable.ExecuteQuery(query);
        }
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
            double spotrate = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_sellingspotrate-dbl"]);
            product = fcamountbal * spotrate;
            hstbl.Add("product", product);
            
            //pcfc interest amount
            double pcfcinterestamount = 0;
            double interestrate = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_interestrate-dbl"]);
            pcfcinterestamount = interestrate * fcamount * days / 360;
            hstbl.Add("pcfcinterestamount", pcfcinterestamount);
            
            
        }
        else if (m == "forwardcontract")
        {
            //balance sold
            double balancesold = 0;
            double sold = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_sold-dbl"]);
            double utilised = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_utilised-dbl"]);
            if (utilised <= sold)
            {
                balancesold = sold - utilised;
            }
            hstbl.Add("balancesold", balancesold);
            
            //sold amount in rs
            double soldAmountInRs = 0;
            double rate = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_rate-dbl"]);
            soldAmountInRs = rate * balancesold;
            hstbl.Add("soldamountinrs", soldAmountInRs);
            
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
    public bool IsReusable {
        get {
            return false;
        }
    }

}