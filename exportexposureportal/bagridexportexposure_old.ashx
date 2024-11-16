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
        //try
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
            else if (context.Request.QueryString["a"] == "sum")
            {
                ExportExposurePortal objPortal = new ExportExposurePortal(clientId);
                context.Response.Write(objPortal.GetSummaryData(m, Common.GetQueryString("ew")));
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
                int parentId = Common.GetQueryStringValue("pid");
                string parentModule = Common.GetQueryString("pm");
                int orderId = 0;
                if (m == "femorderdetail")
                {
                    orderId = Common.GetQueryStringValue("pid");
                }
                if (orderId > 0)
                {
                    orderNo = GetOrderNo(orderId);
                }
                CalculateAfterSave(clientId, m, newid);
                UpdateExportOrder(orderNo);

                DataRow dr = GetDataRow(m, newid);
                
                ExportExposurePortal objPortal = new ExportExposurePortal(clientId);
                //dr.Table.Columns.Add("SummaryDetail");
                //dr["SummaryDetail"] = objPortal.GetSummaryData(Common.GetQueryString("m"));
                if (parentId > 0)
                {
                    CalculateAfterSave(clientId, parentModule, parentId);
                    dr.Table.Columns.Add("ParentDetail");
                    //dr.Table.Columns.Add("ParentSummaryDetail");
                    DataRow drParentDetail = GetDataRow(parentModule, parentId);
                    string orderDetailJSON = JSON.ConvertAmountComma(drParentDetail, "", true);
                    dr["ParentDetail"] = orderDetailJSON;
                    //dr["ParentSummaryDetail"] = objPortal.GetSummaryData(Common.GetQueryString("pm"));
                }
                string data = JSON.ConvertAmountComma(dr, "", true);
                context.Response.Write(data);
            }
            else
            {
                context.Response.Write("Error");
            }
        }
        //catch (Exception ex)
        {
            //context.Response.Write("Error : " + ex.Message);
        }
    }
    private string GetOrderNo(int orderId)
    {
        string orderNo = "";
        if (orderId > 0)
        {
            string query = "select * from tbl_exportorder where exportorder_exportorderid=" + orderId;
            DataRow drorder = DbTable.ExecuteSelectRow(query);
            orderNo = GlobalUtilities.ConvertToString(drorder["exportorder_exportorderno"]);
        }
        return orderNo;
    }
    private DataRow GetDataRow(string m, int id)
    {
        string query = "select " + m + "_" + m + "id as id,* from tbl_" + m + " where " + m + "_" + m + "id=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        for (int i = 0; i < dr.Table.Columns.Count; i++)
        {
            if (dr.Table.Columns[i].DataType == typeof(System.Decimal))
            {
                string val = GlobalUtilities.ConvertToString(dr[i]);
                if (val == "")
                {
                    dr[i] = "0.00";
                }
                else
                {
                    dr[i] = Common.FormatAmountComma(val);
                }
            }
        }
        return dr;
    }
    private void DeleteData(string m, int clientId)
    {
        try
        {
            string query = "";
            int id = Common.GetQueryStringValue("id");
            query = "select * from tbl_" + m + " where " + m + "_" + m + "id=" + id;
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (dr == null) return;

            query = "delete from tbl_" + m + " where " + m + "_" + m + "id=" + id + " AND " + m + "_clientid=" + clientId;
            DbTable.ExecuteQuery(query);
            int parentId = Common.GetQueryStringValue("pid");

            string parentModule = Common.GetQueryString("pm");
            if (parentModule == "")
            {
                HttpContext.Current.Response.Write("Ok");
                return;
            }
            if (m == "exportorder")
            {
            }
            else
            {
                CalculateAfterSave(clientId, m, 0);
                string orderNo = "";
                if (m == "femorderdetail")
                {
                    orderNo = GetOrderNo(parentId);
                    UpdateExportOrder(orderNo);
                }
            }
            
            if (parentModule != "")
            {
                //update the parent window detail
                CalculateAfterSave(clientId, parentModule, parentId);
                
                DataRow drParentDetail = GetDataRow(Common.GetQueryString("pm"), Common.GetQueryStringValue("pid"));
                string orderDetailJSON = JSON.ConvertAmountComma(drParentDetail, "", true);
                HttpContext.Current.Response.Write(orderDetailJSON);
            }
            else
            {
                HttpContext.Current.Response.Write("Ok");
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("Error");
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
            if (pcfcNo == "")
            {
                //HttpContext.Current.Response.Write("Error : Booking number already exists!");
                return true;
            }
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
        else if (m == "femepc")
        {
            string femepcNo = HttpContext.Current.Request.Form["txtba_epcno"].Trim();
            if (femepcNo == "")
            {
                //HttpContext.Current.Response.Write("Error : Booking number already exists!");
                return true;
            }
            query = "select * from tbl_femepc WHERE femepc_epcno='" + femepcNo + "' AND femepc_clientid=" + clientId;
            if (id > 0)
            {
                query += " AND femepc_femepcid<>" + id;
            }
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (dr != null)
            {
                HttpContext.Current.Response.Write("Error : EPC number already exists!");
                return false;
            }
        }
        else if (m == "femorderdetail")
        {
            int orderId = Common.GetQueryStringValue("pid");
            query = "select * from tbl_exportorder where exportorder_exportorderid=" + orderId;
            DataRow drorder = DbTable.ExecuteSelectRow(query);
            double exportOrderAmount = GlobalUtilities.ConvertToDouble(drorder["exportorder_exportorderamount"]);
            query = "select sum(femorderdetail_amountreceived) as total from tbl_femorderdetail where femorderdetail_exportorderid=" + orderId;
            if (id > 0)
            {
                query += " and femorderdetail_femorderdetailid<>" + id;
            }
            DataRow dr = DbTable.ExecuteSelectRow(query);
            double dblAmountReceived = GlobalUtilities.ConvertToDouble(dr["total"]);
            dblAmountReceived += GetFormData_dbl("txtba_amountreceived-dbl");
            if (dblAmountReceived > exportOrderAmount)
            {
                HttpContext.Current.Response.Write("Error : Sum of Received Amount can not exceed Export Order Amount!");
                return false;
            }
        }
        else if (m == "femfowardutilizationdetail")
        {
            int forwardcontractId = Common.GetQueryStringValue("pid");
            query = "select * from tbl_forwardcontract where forwardcontract_forwardcontractid=" + forwardcontractId;
            DataRow drorder = DbTable.ExecuteSelectRow(query);
            double forwardBookingAmount = GlobalUtilities.ConvertToDouble(drorder["forwardcontract_sold"]);
            query = "select sum(femfowardutilizationdetail_utilisationamount) as total from tbl_femfowardutilizationdetail where femfowardutilizationdetail_forwardcontractid=" + forwardcontractId;
            if (id > 0)
            {
                query += " and femfowardutilizationdetail_femfowardutilizationdetailid<>" + id;
            }
            DataRow dr = DbTable.ExecuteSelectRow(query);
            double dblUtilisationAmount = GlobalUtilities.ConvertToDouble(dr["total"]);
            dblUtilisationAmount += GetFormData_dbl("txtba_utilisationamount-dbl");
            if (dblUtilisationAmount > forwardBookingAmount)
            {
                HttpContext.Current.Response.Write("Error : Sum of Utilisation Amount can not exceed Forward Booking Amount!");
                return false;
            }
            query = @"select sum(femfowardcancellationdetail_cancellationamount) as total from tbl_femfowardcancellationdetail 
                    where femfowardcancellationdetail_forwardcontractid=" + forwardcontractId;
            DataRow drCancellation = DbTable.ExecuteSelectRow(query);
            double dblCancellationAmount = 0;
            if (drCancellation != null) dblCancellationAmount = GlobalUtilities.ConvertToDouble(drCancellation["total"]);
            double balance = forwardBookingAmount - dblUtilisationAmount - dblCancellationAmount;
            if (balance < 0)
            {
                HttpContext.Current.Response.Write("Error : Forward  Balance Amount can not be negative!");
                return false;
            }
        }
        else if (m == "femfowardcancellationdetail")
        {
            int forwardcontractId = Common.GetQueryStringValue("pid");
            query = "select * from tbl_forwardcontract where forwardcontract_forwardcontractid=" + forwardcontractId;
            DataRow drorder = DbTable.ExecuteSelectRow(query);
            double forwardBookingAmount = GlobalUtilities.ConvertToDouble(drorder["forwardcontract_sold"]);
            query = "select sum(femfowardcancellationdetail_cancellationamount) as total from tbl_femfowardcancellationdetail where femfowardcancellationdetail_forwardcontractid=" + forwardcontractId;
            if (id > 0)
            {
                query += " and femfowardcancellationdetail_femfowardcancellationdetailid<>" + id;
            }
            DataRow dr = DbTable.ExecuteSelectRow(query);
            double dblcancellationAmount = GlobalUtilities.ConvertToDouble(dr["total"]);
            dblcancellationAmount += GetFormData_dbl("txtba_cancellationamount-dbl");
            if (dblcancellationAmount > forwardBookingAmount)
            {
                HttpContext.Current.Response.Write("Error : Sum of Cancellation Amount can not exceed Forward Booking Amount!");
                return false;
            }
            query = @"select sum(femfowardutilizationdetail_utilisationamount) as total from tbl_femfowardutilizationdetail
                    where femfowardutilizationdetail_forwardcontractid=" + forwardcontractId;
            DataRow drUtilitzed = DbTable.ExecuteSelectRow(query);
            double dblUtilisationAmount = 0;
            if (drUtilitzed != null) dblUtilisationAmount = GlobalUtilities.ConvertToDouble(drUtilitzed["total"]);
            double balance = forwardBookingAmount - dblUtilisationAmount - dblcancellationAmount;
            if (balance < 0)
            {
                HttpContext.Current.Response.Write("Error : Forward  Balance Amount can not be negative!");
                return false;
            }
        }
        else if (m == "fempcfcdetail")
        {
            int pcfcId = Common.GetQueryStringValue("pid");
            query = "select * from tbl_pcfc where pcfc_pcfcid=" + pcfcId;
            DataRow drorder = DbTable.ExecuteSelectRow(query);
            double forwardBookingAmount = GlobalUtilities.ConvertToDouble(drorder["pcfc_fcamount"]);
            query = "select sum(fempcfcdetail_liquidationamount) as total from tbl_fempcfcdetail where fempcfcdetail_pcfcid=" + pcfcId;
            if (id > 0)
            {
                query += " and fempcfcdetail_fempcfcdetailid<>" + id;
            }
            DataRow dr = DbTable.ExecuteSelectRow(query);
            double dblcancellationAmount = GlobalUtilities.ConvertToDouble(dr["total"]);
            dblcancellationAmount += GetFormData_dbl("txtba_liquidationamount-dbl");
            if (dblcancellationAmount > forwardBookingAmount)
            {
                HttpContext.Current.Response.Write("Error : Sum of Liquidation Amount can not exceed PCFC Amount!");
                return false;
            }
        }
        else if (m == "femepcdetail")
        {
            int epcId = Common.GetQueryStringValue("pid");
            query = "select * from tbl_femepc where femepc_femepcid=" + epcId;
            DataRow drorder = DbTable.ExecuteSelectRow(query);
            double epcamountinrs = GlobalUtilities.ConvertToDouble(drorder["femepc_epcamountinrs"]);
            query = "select sum(femepcdetail_liquidationamountinrs) as total from tbl_femepcdetail where femepcdetail_femepcid=" + epcId;
            if (id > 0)
            {
                query += " and femepcdetail_femepcdetailid<>" + id;
            }
            DataRow dr = DbTable.ExecuteSelectRow(query);
            double dblRepayment = GlobalUtilities.ConvertToDouble(dr["total"]);
            dblRepayment += GetFormData_dbl("txtba_liquidationamountinrs-dbl");
            if (dblRepayment > epcamountinrs)
            {
                HttpContext.Current.Response.Write("Error : Sum of Liquidation Amount can not exceed EPC Amount!");
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
            query = "update tbl_forwardcontract set forwardcontract_balancesold=forwardcontract_sold - forwardcontract_utilised - forwardcontract_cancellation" +
                    " where forwardcontract_forwardcontractid=" + id;
            DbTable.ExecuteQuery(query);
            query = "update tbl_forwardcontract set forwardcontract_soldamountinrs=forwardcontract_balancesold * forwardcontract_rate" +
                    " where forwardcontract_forwardcontractid=" + id;
            DbTable.ExecuteQuery(query);
            
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
        else if (m == "femorderdetail")
        {
            int orderId = Common.GetQueryStringValue("pid");
            query = @"update tbl_exportorder set exportorder_amountreceived=(select sum(femorderdetail_amountreceived) 
                            from tbl_femorderdetail where femorderdetail_exportorderid=" + orderId + @"),
                            
                            exportorder_conversionrate=case when (select sum(femorderdetail_amountreceived) 
                            from tbl_femorderdetail where femorderdetail_exportorderid=" + orderId + @")> 0 then
                            (
                                (select SUM(femorderdetail_amountreceived*femorderdetail_conversionrate) from tbl_femorderdetail where femorderdetail_exportorderid=" + orderId + @")/
                                (select SUM(femorderdetail_amountreceived) from tbl_femorderdetail where femorderdetail_exportorderid=" + orderId + @")                                
                            )
                            else 0 END,

                            exportorder_profitlossfromcosting=(select sum(femorderdetail_profitlossfromcosting) 
                            from tbl_femorderdetail where femorderdetail_exportorderid=" + orderId + @"),
                            
                            exportorder_profitlossfromspotrateonremittancereceived=(select sum(femorderdetail_profitlossfromspotrate) 
                            from tbl_femorderdetail where femorderdetail_exportorderid=" + orderId + @")                                                       
                      where exportorder_exportorderid=" + orderId;
            DbTable.ExecuteQuery(query);
        }
        else if (m == "femfowardutilizationdetail")
        {
            int forwardcontractId = Common.GetQueryStringValue("pid");
            //query = "select * from tbl_forwardcontract where forwardcontract_forwardcontractid="+forwardcontractId;
            //DataRow drforwardcontract = DbTable.ExecuteSelectRow(query);
            //double forwardBookingRate = GlobalUtilities.ConvertToDouble(drforwardcontract["forwardcontract_rate"]);
            query = @"update tbl_forwardcontract set 
                            forwardcontract_utilised=(select SUM(femfowardutilizationdetail_utilisationamount) from tbl_femfowardutilizationdetail where femfowardutilizationdetail_forwardcontractid=" + forwardcontractId + @"),
                            forwardcontract_spotrateonutilisationdate=case when (select sum(femfowardutilizationdetail_utilisationamount) 
                            from tbl_femfowardutilizationdetail where femfowardutilizationdetail_forwardcontractid=" + forwardcontractId + @")> 0 then
                            (
                                (select SUM(femfowardutilizationdetail_utilisationamount*femfowardutilizationdetail_spotrateonutilisationdate) from tbl_femfowardutilizationdetail where femfowardutilizationdetail_forwardcontractid=" + forwardcontractId + @")/
                                (select SUM(femfowardutilizationdetail_utilisationamount) from tbl_femfowardutilizationdetail where femfowardutilizationdetail_forwardcontractid=" + forwardcontractId + @")                                
                            )
                            else 0 END,
                            forwardcontract_utilisationrate=case when (select sum(femfowardutilizationdetail_utilisationamount) 
                            from tbl_femfowardutilizationdetail where femfowardutilizationdetail_forwardcontractid=" + forwardcontractId + @")> 0 then
                            (
                                (select SUM(femfowardutilizationdetail_utilisationamount*femfowardutilizationdetail_utilizationrate) from tbl_femfowardutilizationdetail where femfowardutilizationdetail_forwardcontractid=" + forwardcontractId + @")/
                                (select SUM(femfowardutilizationdetail_utilisationamount) from tbl_femfowardutilizationdetail where femfowardutilizationdetail_forwardcontractid=" + forwardcontractId + @")                                
                            )
                            else 0 END,                            
                            forwardcontract_profitandlossonutilisation=(select SUM(femfowardutilizationdetail_profitandlossonutilisation) from tbl_femfowardutilizationdetail where femfowardutilizationdetail_forwardcontractid=" + forwardcontractId + @")
                            
                      where forwardcontract_forwardcontractid=" + forwardcontractId;
            DbTable.ExecuteQuery(query);
            query = @"update tbl_forwardcontract set 
                            forwardcontract_totalprofitandlossonforwardcontract=forwardcontract_profitandlossonutilisation+forwardcontract_profitandlossoncancellation
                      where forwardcontract_forwardcontractid=" + forwardcontractId;
            DbTable.ExecuteQuery(query);
        }
        else if (m == "femfowardcancellationdetail") 
        {
            int forwardcontractId = Common.GetQueryStringValue("pid");
            query = @"update tbl_forwardcontract set 
                            forwardcontract_cancellation=(select SUM(femfowardcancellationdetail_cancellationamount) from tbl_femfowardcancellationdetail where femfowardcancellationdetail_forwardcontractid=" + forwardcontractId + @"),
                            forwardcontract_spotrateoncancellationdate=case when (select sum(femfowardcancellationdetail_cancellationamount) 
                            from tbl_femfowardcancellationdetail where femfowardcancellationdetail_forwardcontractid=" + forwardcontractId + @")> 0 then
                            (
                                (select SUM(femfowardcancellationdetail_cancellationamount*femfowardcancellationdetail_spotrateoncancellationdate) from tbl_femfowardcancellationdetail where femfowardcancellationdetail_forwardcontractid=" + forwardcontractId + @")/
                                (select SUM(femfowardcancellationdetail_cancellationamount) from tbl_femfowardcancellationdetail where femfowardcancellationdetail_forwardcontractid=" + forwardcontractId + @")                                
                            )
                            else 0 END,
                            forwardcontract_cancellationrate = case when (select sum(femfowardcancellationdetail_cancellationamount) 
                            from tbl_femfowardcancellationdetail where femfowardcancellationdetail_forwardcontractid=" + forwardcontractId + @")> 0 then
                            (
                                (select SUM(femfowardcancellationdetail_cancellationamount*femfowardcancellationdetail_cancellationrate) from tbl_femfowardcancellationdetail where femfowardcancellationdetail_forwardcontractid=" + forwardcontractId + @")/
                                (select SUM(femfowardcancellationdetail_cancellationamount) from tbl_femfowardcancellationdetail where femfowardcancellationdetail_forwardcontractid=" + forwardcontractId + @")                                
                            )
                            else 0 END,
                            forwardcontract_profitandlossoncancellation=(select SUM(femfowardcancellationdetail_profitandlossoncancellation) from tbl_femfowardcancellationdetail where femfowardcancellationdetail_forwardcontractid=" + forwardcontractId + @")
                      where forwardcontract_forwardcontractid=" + forwardcontractId;
            DbTable.ExecuteQuery(query);
            query = @"update tbl_forwardcontract set 
                            forwardcontract_totalprofitandlossonforwardcontract=forwardcontract_profitandlossonutilisation+forwardcontract_profitandlossoncancellation
                      where forwardcontract_forwardcontractid=" + forwardcontractId;
            DbTable.ExecuteQuery(query);
        }
        else if (m == "fempcfcdetail")
        {
            int pcfcId = Common.GetQueryStringValue("pid");
            query = @"update tbl_pcfc set 
                        
                        pcfc_repayment=(select sum(fempcfcdetail_liquidationamount) from tbl_fempcfcdetail where fempcfcdetail_pcfcid=" + pcfcId + @"),
                        pcfc_spotrateonrepayment=case when (select sum(fempcfcdetail_liquidationamount) 
                            from tbl_fempcfcdetail where fempcfcdetail_pcfcid=" + pcfcId + @")> 0 then
                            (
                                (select SUM(fempcfcdetail_liquidationamount*fempcfcdetail_spotrateonrepayment) from tbl_fempcfcdetail where fempcfcdetail_pcfcid=" + pcfcId + @")/
                                (select SUM(fempcfcdetail_liquidationamount) from tbl_fempcfcdetail where fempcfcdetail_pcfcid=" + pcfcId + @")                                
                            )
                            else 0 END,
                        pcfc_profitandlossonpcfc=(select sum(fempcfcdetail_profitandlossonpcfc) from tbl_fempcfcdetail where fempcfcdetail_pcfcid=" + pcfcId + @")
                      where pcfc_pcfcid=" + pcfcId;
            DbTable.ExecuteQuery(query);
        }
        else if (m == "femepcdetail")
        {
            int femepcId = Common.GetQueryStringValue("pid");
            query = @"update tbl_femepc set femepc_repaymentinrs=(select sum(femepcdetail_liquidationamountinrs) from tbl_femepcdetail
                                                    where femepcdetail_femepcid=" + femepcId + @")
                      where femepc_femepcid=" + femepcId;
            DbTable.ExecuteQuery(query);
            query = @"update tbl_femepc set femepc_epcbalanceamountinrs = femepc_epcamountinrs - femepc_repaymentinrs
                      where femepc_femepcid=" + femepcId;
            DbTable.ExecuteQuery(query);
        }
        else if (m == "pcfc")
        {
            query = "update tbl_pcfc set pcfc_fcamountbalance=pcfc_fcamount-pcfc_repayment where pcfc_pcfcid=" + id;
            DbTable.ExecuteQuery(query);
            //HttpContext.Current.Response.Write(query);
//            query = @"update tbl_femepc set femepc_epcbalanceamountinrs = femepc_epcamountinrs - femepc_repaymentinrs
//                      where femepc_femepcid=" + id;
//            DbTable.ExecuteQuery(query);
        }
        else if (m == "femepc")
        {
            query = "update tbl_femepc set femepc_epcbalanceamountinrs = femepc_epcamountinrs - femepc_repaymentinrs where femepc_femepcid=" + id;
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
            /*
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
            */
            
            //profit loss utilisation
            //double spotRateUtilized = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_spotrateutilisedcancel-dbl"]);
            //double PLUtilisation = utilised * (rate - spotRateUtilized);
            //hstbl.Add("profitandlossonutilisation", PLUtilisation);//REMOVED WITH NEW CALC
            
            //profit loss on utilization cancel
            //double cancelationAmount = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_cancellationamount-dbl"]);
            //double spotRateCancel = GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form["txtba_spotratecancellation-dbl"]);
            //double PLOnCancel = cancelationAmount * (rate - utilizedcancel);
            //double PLOnCancel = cancellation * (rate - spotRateCancel);
            //hstbl.Add("profitandlossoncancellation", PLOnCancel);//REMOVED WITH NEW CALC
            
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
        else if (m == "femorderdetail")
        {
            double profitlossFromCosting = 0; double profitlossFromSpotRate = 0;
            double conversionRate = GetFormData_dbl("txtba_conversionrate-dbl");
            double amountReceived = GetFormData_dbl("txtba_amountreceived-dbl");
            double spotRate = GetFormData_dbl("txtba_spotrate-dbl");
            int orderId = Common.GetQueryStringValue("pid");
            DataRow drOrder = DbTable.GetOneRow("tbl_exportorder", orderId);
            double costing = GlobalUtilities.ConvertToDouble(drOrder["exportorder_costing"]);
            profitlossFromCosting = (conversionRate - costing) * amountReceived;

            profitlossFromSpotRate = (conversionRate - spotRate) * amountReceived;
            if (conversionRate == 0) profitlossFromCosting = 0;//NO CALC
            if (spotRate == 0) profitlossFromSpotRate = 0;//NO CALC
            hstbl.Add("profitlossfromcosting", profitlossFromCosting);
            hstbl.Add("profitlossfromspotrate", profitlossFromSpotRate);
        }
        else if (m == "femfowardutilizationdetail")
        {
            int forwardContractId = Common.GetQueryStringValue("pid");
            DataRow drForwardContract = DbTable.GetOneRow("tbl_forwardcontract", forwardContractId);
            double forwardBookingRate = GlobalUtilities.ConvertToDouble(drForwardContract["forwardcontract_rate"]);
            double premium = GetFormData_dbl("txtba_premium-dbl");
            double utilizationrate = forwardBookingRate - premium;
            hstbl.Add("utilizationrate", utilizationrate);
            
            double spotrateonutilisationdate = GetFormData_dbl("txtba_spotrateonutilisationdate-dbl");
            double utilizationamount = GetFormData_dbl("txtba_utilisationamount-dbl");
            double profitandlossonutilisation = (utilizationrate - spotrateonutilisationdate) * utilizationamount;
            if (spotrateonutilisationdate == 0) profitandlossonutilisation = 0;//NO CALC
            hstbl.Add("profitandlossonutilisation", profitandlossonutilisation);
        }
        else if (m == "femfowardcancellationdetail")
        {
            int forwardContractId = Common.GetQueryStringValue("pid");
            DataRow drForwardContract = DbTable.GetOneRow("tbl_forwardcontract", forwardContractId);
            double forwardBookingRate = GlobalUtilities.ConvertToDouble(drForwardContract["forwardcontract_rate"]);
            double premium = GetFormData_dbl("txtba_premium-dbl");
            double spotrateoncancellationdate = GetFormData_dbl("txtba_spotrateoncancellationdate-dbl");
            double cancellationrate = spotrateoncancellationdate + premium;
            hstbl.Add("cancellationrate", cancellationrate);
            
            double spotrateonutilisationdate = GetFormData_dbl("txtba_spotrateonutilisationdate-dbl");
            double cancellationamount = GetFormData_dbl("txtba_cancellationamount-dbl");
            double profitandlossoncancellation = (forwardBookingRate - cancellationrate) * cancellationamount;
            if (spotrateoncancellationdate == 0) profitandlossoncancellation = 0;//NO CALC
            hstbl.Add("profitandlossoncancellation", profitandlossoncancellation);
        }
        else if (m == "fempcfcdetail")
        {
            int pcfcId = Common.GetQueryStringValue("pid");
            DataRow drPcFc = DbTable.GetOneRow("tbl_pcfc", pcfcId);
            double pcfcConversionRate = GlobalUtilities.ConvertToDouble(drPcFc["pcfc_spotrate"]);
            double spotrateonrepayment = GetFormData_dbl("txtba_spotrateonrepayment-dbl");
            double liquidationamount=GetFormData_dbl("txtba_liquidationamount-dbl");
            double profitandlossonpcfc = (pcfcConversionRate-spotrateonrepayment) * liquidationamount;
            if (spotrateonrepayment == 0) profitandlossonpcfc = 0;//NO CALC
            hstbl.Add("profitandlossonpcfc", profitandlossonpcfc);
        }
        //else if (m == "femepc")
        //{
        //    int id = Common.GetQueryStringValue("id");
        //    DataRow drEPC = DbTable.GetOneRow("tbl_femepc", id);
        //    string epcdate = GetFormData("txtba_epcavaileddate-dt");
        //    string epdduedate = "";
        //    int creditPeriodDays = GetFormData_int("txtba_creditperioddays-i");
        //    if (epcdate != "")
        //    {
        //        DateTime dt = GlobalUtilities.ConvertToDateFromTextBox(epcdate);
        //        dt = dt.AddDays(creditPeriodDays);
        //        epdduedate = GlobalUtilities.ConvertToDate(dt);
        //    }
        //    hstbl.Add("epcduedate", epdduedate);
        //    double epcamountinrs = GetFormData_dbl("txtba_epcamountinrs-dbl");
        //    double repayment = GlobalUtilities.ConvertToDouble(drEPC["femepc_repaymentinrs"]);
        //    double epcbalanceamountinrs = epcamountinrs - repayment;
        //    hstbl.Add("epcbalanceamountinrs", epcbalanceamountinrs);
        //    double forwardbookingamount = GetFormData_dbl("txtba_forwardbookingamount-dbl");
        //    double forwardbookingrate = GetFormData_dbl("txtba_forwardbookingrate-dbl");
        //    double valueinrs = forwardbookingamount * forwardbookingrate;
        //    hstbl.Add("valueinrs", valueinrs);

        //    double interestrate = GetFormData_dbl("txtba_interestrate-dbl");
        //    double epcinterestamount = epcamountinrs * interestrate * creditPeriodDays / 365.0 / 100.0;
        //    hstbl.Add("epcinterestamount", epcinterestamount);
        //}
        else if (m == "femepc")
        {
            string epcdate = GetFormData("txtba_epcavaileddate-dt");
            string epdduedate = "";
            int creditPeriodDays = GetFormData_int("txtba_creditperioddays-i");
            if (epcdate != "")
            {
                DateTime dt = GlobalUtilities.ConvertToDateFromTextBox(epcdate);
                dt = dt.AddDays(creditPeriodDays);
                epdduedate = GlobalUtilities.ConvertToDate(dt);
            }
            hstbl.Add("epcduedate", epdduedate);
            double epcamountinrs = GetFormData_dbl("txtba_epcamountinrs-dbl");
            double forwardbookingamount = GetFormData_dbl("txtba_forwardbookingamount-dbl");
            double forwardbookingrate = GetFormData_dbl("txtba_forwardbookingrate-dbl");
            double valueinrs = forwardbookingamount * forwardbookingrate;
            hstbl.Add("valueinrs", valueinrs);

            double interestrate = GetFormData_dbl("txtba_interestrate-dbl");
            double epcinterestamount = epcamountinrs * interestrate * creditPeriodDays / 365.0 / 100.0;
            hstbl.Add("epcinterestamount", epcinterestamount);
        }
        else if (m == "femepcdetail")
        {
            double spotrateonrepayment = GetFormData_dbl("txtba_spotrateonrepayment-dbl");
            double liquidationamountinrs=GetFormData_dbl("txtba_liquidationamountinrs-dbl");
            double profitandlossonpcfc = (0 - spotrateonrepayment) * liquidationamountinrs;
            if (spotrateonrepayment == 0) profitandlossonpcfc = 0;//NO CALC
            hstbl.Add("profitandlossonpcfc", profitandlossonpcfc);
        }
        
        
        //add extra values
        if (Common.GetQueryString("pm") != "")
        {
            hstbl.Add(Common.GetQueryString("pm") + "id", Common.GetQueryStringValue("pid"));
        }
        //end
    }
    private DataRow GetOrderDetail(int orderId)
    {
        DataRow drOrder = DbTable.GetOneRow("tbl_exportorder", orderId);
        return drOrder;
    }
    private double GetFormData_dbl(string key)
    {
        return GlobalUtilities.ConvertToDouble(HttpContext.Current.Request.Form[key]);
    }
    private int GetFormData_int(string key)
    {
        return GlobalUtilities.ConvertToInt(HttpContext.Current.Request.Form[key]);
    }
    private string GetFormData(string key)
    {
        return GlobalUtilities.ConvertToString(HttpContext.Current.Request.Form[key]);
    }
    private double FindPremiumRate(DateTime spotDate, DateTime dtBrokenDate, int currency, bool isExport)
    {
        int endDateRateId_Start = 0;
        int endDateRateId_End = 0;
        int rateId_Start = 0;
        int rateId_End = 0;
        double dblDivisionFactor = 0;
        double dblForwardRateDivFactor = 1;

        //try
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
        //catch (Exception ex)
        {
            //return 0;
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