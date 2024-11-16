<%@ WebHandler Language="C#" Class="exportexposureschedular" %>

using System;
using System.Web;
using WebComponent;
using System.Text;
using System.Data;
using System.Collections;
public class exportexposureschedular : IHttpHandler {
    private DataTable _dttblEOCols = new DataTable();
    private DataTable _dttblFCCols = new DataTable();
    private DataTable _dttblPCFCCols = new DataTable();
    private DataTable _dttblImportOrderCols_Import = new DataTable();
    private DataTable _dttblForwardContractCols_Import = new DataTable();
    private DataTable _dttblTradeContractCols_Import = new DataTable();

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        try
        {
            if (context.Request.QueryString["istest"] == "true")
            {
                int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
                string query = "update tbl_exportexposurealerts set exportexposurealerts_duesentdate=getdate()-1,exportexposurealerts_dashboardsentdate=getdate()-1,exportexposurealerts_hedgingratiosent=getdate()-1 " +
                                "WHERE exportexposurealerts_clientid=" + clientId;
                DbTable.ExecuteQuery(query);
            }
            else
            {
                if (DateTime.Now.Hour > 8 || DateTime.Now.Hour < 7 || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    context.Response.Write("Ok");
                    return;
                }
            }
            SetColumns();
            SenReminders();
            context.Response.Write("Ok");
        }
        catch (Exception ex)
        {
            context.Response.Write("Error : " + ex.Message);
        }
    }
    private int ClientId
    {
        get { return GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId")); }
    }
    private void SenReminders()
    {
        //send dashboard mails
        string query = @"select * from tbl_client 
                        join tbl_subscription on subscription_clientid=client_clientid
                        where client_subscriptionstatusid=2";
        if (HttpContext.Current.Request.QueryString["istest"] == "true")
        {
            query += " and client_clientid=" + ClientId;
        }
        DataTable dttbl = DbTable.ExecuteSelect(query);
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int clientId = 0;
            try
            {
                clientId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["client_clientid"]);
                int subscriptionId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["subscription_subscriptionid"]);
                query = "select * from tbl_subscriptionprospects where subscriptionprospects_prospectid=4 AND subscriptionprospects_subscriptionid=" + subscriptionId;
                DataRow dr = DbTable.ExecuteSelectRow(query);
                if (dr != null)
                {
                    query = "select * from tbl_exportexposurealerts where exportexposurealerts_clientid=" + clientId;
                    DataRow drAlert = DbTable.ExecuteSelectRow(query);
                    if (drAlert != null)
                    {
                        SendDashboardMail(dttbl.Rows[i], drAlert);

                        //Send other dues
                        SendDueReminders(dttbl.Rows[i], drAlert);

                        //send hedging ration
                        SendHedgingRatio(dttbl.Rows[i], drAlert);
                    }
                }
                //import module dashboard
                query = "select * from tbl_subscriptionprospects where subscriptionprospects_prospectid=5 AND subscriptionprospects_subscriptionid=" + subscriptionId;
                dr = DbTable.ExecuteSelectRow(query);
                if (dr != null)
                {
                    query = "select * from tbl_importexposurealerts where importexposurealerts_clientid=" + clientId;
                    DataRow drAlert = DbTable.ExecuteSelectRow(query);
                    if (drAlert != null)
                    {
                        SendDashboardMail_Import(dttbl.Rows[i], drAlert);
                        //Send other dues
                        SendDueReminders_Import(dttbl.Rows[i], drAlert);

                        //send hedging ration
                        //SendHedgingRatio(dttbl.Rows[i], drAlert);
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("Error : " + " Client Id : " + clientId + " " + ex.Message);
            }
        }
        
    }

    private void SendDashboardMail(DataRow drClient, DataRow drAlert)
    {
        if (!GlobalUtilities.ConvertToBool(drAlert["exportexposurealerts_senddailydashboard"])) return;
        int clientId = GlobalUtilities.ConvertToInt(drClient["client_clientid"]);
        string emailIds = GlobalUtilities.ConvertToString(drAlert["exportexposurealerts_emailid"]);
        if (emailIds == "") return;
        if (drAlert["exportexposurealerts_dashboardsentdate"] != DBNull.Value)
        {
            TimeSpan sp = DateTime.Now - Convert.ToDateTime(drAlert["exportexposurealerts_dashboardsentdate"]);
            if (sp.Days == 0) return;//if today sent today then dont send
        }
        string query = "";
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%'>");
        query = "select * from tbl_customercurrency " +
                "join tbl_exposurecurrencymaster on exposurecurrencymaster_exposurecurrencymasterid=customercurrency_exposurecurrencymasterid " +
                "WHERE customercurrency_clientid=" + clientId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        if (!GlobalUtilities.IsValidaTable(dttbl)) return;
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int currencyId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["exposurecurrencymaster_exposurecurrencymasterid"]);
            string currency = GlobalUtilities.ConvertToString(dttbl.Rows[i]["exposurecurrencymaster_currency"]);
            ExportExposurePortal obj = new ExportExposurePortal(clientId);
            html.Append("<tr><td style='font-weight:bold;'>" + currency + "</td></tr>");
            DataTable dttbldashboard = new DataTable();
            html.Append("<tr><td>" + obj.GetDashboardData(1, currencyId, true, out dttbldashboard) + "</td></tr>");
        }
        html.Append("</table>");
        string subject = "Alerts : Dashboard (Export)";
        string message = Common.GetSetting("Export Exposure Dashboard Mail");
        message = GlobalUtilities.ReplaceString(message, "$data$", html.ToString());

        message = message.Replace("$subject$", subject);
        message = message.Replace("$client_customername$", GlobalUtilities.ConvertToString(drClient["client_customername"]));
        message = message.Replace("$date$", GlobalUtilities.GetCurrentDateDDMMYYYY());
        
        bool issent = BulkEmail.SendMail_Alert(emailIds, subject, message, "");
        if (issent)
        {
            query = "update tbl_exportexposurealerts set exportexposurealerts_dashboardsentdate=getdate() " +
                    "where exportexposurealerts_clientid=" + clientId;
            DbTable.ExecuteQuery(query);
        }

    }
    private void SendDashboardMail_Import(DataRow drClient, DataRow drAlert)
    {
        if (!GlobalUtilities.ConvertToBool(drAlert["importexposurealerts_senddailydashboard"])) return;
        int clientId = GlobalUtilities.ConvertToInt(drClient["client_clientid"]);
        string emailIds = GlobalUtilities.ConvertToString(drAlert["importexposurealerts_emailid"]);
        if (emailIds == "" || emailIds == null) return;
        if (drAlert["importexposurealerts_dashboardsentdate"] != DBNull.Value)
        {
            TimeSpan sp = DateTime.Now - Convert.ToDateTime(drAlert["importexposurealerts_dashboardsentdate"]);
            if (sp.Days == 0) return;//if today sent today then dont send
        }
        string query = "";
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%'>");
        query = "select * from tbl_customercurrency " +
                "join tbl_exposurecurrencymaster on exposurecurrencymaster_exposurecurrencymasterid=customercurrency_exposurecurrencymasterid " +
                "WHERE customercurrency_clientid=" + clientId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        if (!GlobalUtilities.IsValidaTable(dttbl)) return;
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int currencyId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["exposurecurrencymaster_exposurecurrencymasterid"]);
            string currency = GlobalUtilities.ConvertToString(dttbl.Rows[i]["exposurecurrencymaster_currency"]);
            ExportExposurePortal obj = new ExportExposurePortal(clientId);
            html.Append("<tr><td style='font-weight:bold;'>" + currency + "</td></tr>");
            DataTable dttbldashboard = new DataTable();
            html.Append("<tr><td>" + obj.GetDashboardData(2, currencyId, true, out dttbldashboard) + "</td></tr>");
        }
        html.Append("</table>");
        string subject = "Alerts : Dashboard (Import)";
        string message = Common.GetSetting("Export Exposure Dashboard Mail");
        message = GlobalUtilities.ReplaceString(message, "$data$", html.ToString());

        message = message.Replace("$subject$", subject);
        message = message.Replace("$client_customername$", GlobalUtilities.ConvertToString(drClient["client_customername"]));
        message = message.Replace("$date$", GlobalUtilities.GetCurrentDateDDMMYYYY());

        bool issent = BulkEmail.SendMail_Alert(emailIds, subject, message, "");
        if (issent)
        {
            query = "update tbl_importexposurealerts set importexposurealerts_dashboardsentdate=getdate() " +
                    "where importexposurealerts_clientid=" + clientId;
            DbTable.ExecuteQuery(query);
        }

    }
    private void SendHedgingRatio(DataRow drClient, DataRow drAlert)
    {
        string emailIds = GlobalUtilities.ConvertToString(drAlert["exportexposurealerts_emailid"]);
        if (drAlert["exportexposurealerts_hedgingratiosent"] != DBNull.Value)
        {
            TimeSpan sp = DateTime.Now - Convert.ToDateTime(drAlert["exportexposurealerts_hedgingratiosent"]);
            if (sp.Days == 0) return;//if today sent today then dont send
        }
        //get forward contract dues
        string query = "";
        int clientId = GlobalUtilities.ConvertToInt(drClient["client_clientid"]);

        if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
        {
            return;
        }
        int hedgingRatioLimit = GlobalUtilities.ConvertToInt(drAlert["exportexposurealerts_hedginglimit"]);
        query = "select * from tbl_customercurrency " +
                "join tbl_exposurecurrencymaster on exposurecurrencymaster_exposurecurrencymasterid=customercurrency_exposurecurrencymasterid " +
                "WHERE customercurrency_clientid=" + clientId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        if (!GlobalUtilities.IsValidaTable(dttbl)) return;
        dttbl.Columns.Add("HedgingRatio");
        bool isLimitCrossed = false;
        StringBuilder html = new StringBuilder();
        html.Append("<table width='600' cellspacing=0 cellpadding=5 border='1'><tr><td>&nbsp;</td>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string currency = GlobalUtilities.ConvertToString(dttbl.Rows[i]["exposurecurrencymaster_currency"]);
            html.Append("<td>" + currency + "INR</td>");
        }
        html.Append("</tr>");
        html.Append("<tr><td>Hedge Ratio as per Actual Position</td>");
        
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int currencyId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["exposurecurrencymaster_exposurecurrencymasterid"]);
            string currency = GlobalUtilities.ConvertToString(dttbl.Rows[i]["exposurecurrencymaster_currency"]);
            ExportExposurePortal obj = new ExportExposurePortal(clientId);
            DataTable dttbldashboard = new DataTable();
            obj.GetDashboardData(1, currencyId, true, out dttbldashboard);
            int hedgingRatio = GlobalUtilities.ConvertToInt(GlobalUtilities.ConvertToString(dttbldashboard.Rows[5]["Total"]).Replace("%", ""));
            dttbl.Rows[i]["HedgingRatio"] = hedgingRatio;
            string ratio = hedgingRatio.ToString();
            if (hedgingRatioLimit > 0)
            {
                if (hedgingRatio < hedgingRatioLimit && hedgingRatio > 0)
                {
                    isLimitCrossed = true;
                    ratio = "<span style='color:#ff0000'>" + hedgingRatio.ToString() + "</span>";
                }
            }
            html.Append("<td>" + ratio + "</td>");            
        }
        
        if (!isLimitCrossed) return;
        html.Append("</tr><tr><td>Hedge Ratio as per Limit Set</td>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            html.Append("<td>" + hedgingRatioLimit + "</td>");
        }
        html.Append("</tr></table>");

        string message = Common.GetSetting("Daily Reminders Mail");
        message = GlobalUtilities.ReplaceString(message, "$data$", html.ToString());
        string subject = "Alerts: Hedged Position below Limit set";
        
        message = message.Replace("$subject$", subject);
        message = message.Replace("$client_customername$", GlobalUtilities.ConvertToString(drClient["client_customername"]));
        message = message.Replace("$date$", GlobalUtilities.GetCurrentDateDDMMYYYY());
        
        bool issent = BulkEmail.SendMail_Alert(emailIds, subject, message, "");
        if (issent)
        {
            query = "update tbl_exportexposurealerts set exportexposurealerts_hedgingratiosent=getdate() " +
                    "where exportexposurealerts_clientid=" + clientId;
            DbTable.ExecuteQuery(query);
        }
    }
    private void SendDueReminders(DataRow drClient, DataRow drAlert)
    {
        string emailIds = GlobalUtilities.ConvertToString(drAlert["exportexposurealerts_emailid"]);
        if (drAlert["exportexposurealerts_duesentdate"] != DBNull.Value)
        {
            TimeSpan sp = DateTime.Now - Convert.ToDateTime(drAlert["exportexposurealerts_duesentdate"]);
            if (sp.Days == 0) return;//if today sent today then dont send
        }
        //get forward contract dues
        string query = "";
        int clientId = GlobalUtilities.ConvertToInt(drClient["client_clientid"]);
        DataTable dttblFc = new DataTable();
        DataTable dttblEO = new DataTable();
        DataTable dttblPCFC = new DataTable();
        
        //if today is saturday then send due for next week
        bool isSaturday = false;
        if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
        {
            isSaturday = true;
        }
        //forward contract due
        if (GlobalUtilities.ConvertToBool(drAlert["exportexposurealerts_sendforwardcontractdue"]))
        {
            query = @"select * from tbl_forwardcontract
                left join tbl_bank on bank_bankid=forwardcontract_bankid
                left join tbl_exposurecurrencymaster on exposurecurrencymaster_exposurecurrencymasterid=forwardcontract_exposurecurrencymasterid 
                where isnull(forwardcontract_balancesold,0)<>0 and forwardcontract_clientid=" + clientId;
            if (isSaturday)
            {
                query += " AND (DATEDIFF(day,GETDATE(),forwardcontract_to) >= 0 AND DATEDIFF(day,GETDATE(),forwardcontract_to) <= 7)";
            }
            else
            {
                query += " AND (DATEDIFF(day,GETDATE(),forwardcontract_to) = 0)";
            }
            dttblFc = DbTable.ExecuteSelect(query);
        }
        //get export order
        if (GlobalUtilities.ConvertToBool(drAlert["exportexposurealerts_sendexportorderdue"]))
        {
            query = @"select * from tbl_exportorder
                left join tbl_exposurecurrencymaster on exposurecurrencymaster_exposurecurrencymasterid=exportorder_exposurecurrencymasterid 
                where isnull(exportorder_netamount,0)<>0 and exportorder_clientid=" + clientId;
            if (isSaturday)
            {
                query += " AND (DATEDIFF(day,GETDATE(),exportorder_expectedduedate) >= 0 AND DATEDIFF(day,GETDATE(),exportorder_expectedduedate) <= 7)";
            }
            else
            {
                query += " AND DATEDIFF(day,GETDATE(),exportorder_expectedduedate) = 0";
            }
            dttblEO = DbTable.ExecuteSelect(query);
        }
        
        //get pcfc due
        if (GlobalUtilities.ConvertToBool(drAlert["exportexposurealerts_sendpcfcdue"]))
        {
            query = @"select * from tbl_pcfc
                left join tbl_bank on bank_bankid=pcfc_bankid
                left join tbl_exposurecurrencymaster on exposurecurrencymaster_exposurecurrencymasterid=pcfc_exposurecurrencymasterid 
                where isnull(pcfc_fcamountbalance,0)<>0 and pcfc_clientid=" + clientId;
            if (isSaturday)
            {
                query += " AND (DATEDIFF(day,GETDATE(),pcfc_pcduedate) >= 0 AND DATEDIFF(day,GETDATE(),pcfc_pcduedate) <= 7)";
            }
            else
            {
                query += " AND DATEDIFF(day,GETDATE(),pcfc_pcduedate) = 0";
            }
            dttblPCFC = DbTable.ExecuteSelect(query);
        }
        
        if (GlobalUtilities.IsValidaTable(dttblEO) == false && GlobalUtilities.IsValidaTable(dttblPCFC) == false &&
            GlobalUtilities.IsValidaTable(dttblFc) == false) return;

        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%'>");
        if (GlobalUtilities.IsValidaTable(dttblEO))
        {
            if (isSaturday)
            {
                html.Append("<tr><td style='font-weight:bold;'>Export Order > Due for Next Week</td></tr>");
            }
            else
            {
                html.Append("<tr><td style='font-weight:bold;'>Export Order > Due for Today</td></tr>");
            }
            html.Append("<tr><td><table width='100%' style='border-collapse: collapse;border:1px solid #000;' border='1' cellspacing='0'>");
            GetGridHeader(_dttblEOCols, html);
            GetData(dttblEO, _dttblEOCols, html);
            html.Append("</table></td></tr>");
        }  
        if (GlobalUtilities.IsValidaTable(dttblFc))
        {
            html.Append("<tr><td>&nbsp;</td></tr>");
            if (isSaturday)
            {
                html.Append("<tr><td style='font-weight:bold;'>Forward Contract > Due for Next Week</td></tr>");
            }
            else
            {
                html.Append("<tr><td style='font-weight:bold;'>Forward Contract > Due for Today</td></tr>");
            }

            html.Append("<tr><td><table width='100%' style='border-collapse: collapse;border:1px solid #000;' border='1' cellspacing='0'>");
            GetGridHeader(_dttblFCCols, html);
            GetData(dttblFc, _dttblFCCols, html);
            html.Append("</table></td></tr>");
        }
        if (GlobalUtilities.IsValidaTable(dttblPCFC))
        {
            html.Append("<tr><td>&nbsp;</td></tr>");
            if (isSaturday)
            {
                html.Append("<tr><td style='font-weight:bold;'>PCFC > Due for Next Week</td></tr>");
            }
            else
            {
                html.Append("<tr><td style='font-weight:bold;'>PCFC > Due for Today</td></tr>");
            }

            html.Append("<tr><td><table width='100%' style='border-collapse: collapse;border:1px solid #000;' border='1' cellspacing='0'>");
            GetGridHeader(_dttblPCFCCols, html);
            GetData(dttblPCFC, _dttblPCFCCols, html);
            html.Append("</table></td></tr>");
        }
        
        html.Append("</table>");

        string message = Common.GetSetting("Daily Reminders Mail");
        message = GlobalUtilities.ReplaceString(message, "$data$", html.ToString());
        string subject = "Alerts: Exposure Due date TODAY";
        if (isSaturday)
        {
            subject = "Alerts: Exposure Due date Next Week";
        }
        //bool issent = BulkEmail.SendMail_Alert(emailIds, "Due date mail (" + GlobalUtilities.GetCurrentDateDDMMYYYY() + ")", message, "");
        message = message.Replace("$subject$", subject);
        message = message.Replace("$client_customername$", GlobalUtilities.ConvertToString(drClient["client_customername"]));
        message = message.Replace("$date$", GlobalUtilities.GetCurrentDateDDMMYYYY());
        
        bool issent = BulkEmail.SendMail_Alert(emailIds, subject, message, "");
        if (issent)
        {
            query = "update tbl_exportexposurealerts set exportexposurealerts_duesentdate=getdate() " +
                    "where exportexposurealerts_clientid=" + clientId;
            DbTable.ExecuteQuery(query);
        }
    }
    
    private void SendDueReminders_Import(DataRow drClient, DataRow drAlert)
    {
        string emailIds = GlobalUtilities.ConvertToString(drAlert["importexposurealerts_emailid"]);
        if (emailIds == "" || emailIds == null) return;
        if (drAlert["importexposurealerts_duesentdate"] != DBNull.Value)
        {
            TimeSpan sp = DateTime.Now - Convert.ToDateTime(drAlert["importexposurealerts_duesentdate"]);
            if (sp.Days == 0) return;//if today sent today then dont send
        }
        //get forward contract dues
        string query = "";
        int clientId = GlobalUtilities.ConvertToInt(drClient["client_clientid"]);
        DataTable dttblTradeCredit = new DataTable();
        DataTable dttblForwardContract = new DataTable();
        DataTable dttblImportOrder = new DataTable();

        //if today is saturday then send due for next week
        bool isSaturday = false;
        if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
        {
            isSaturday = true;
        }
        //forward contract due
        if (GlobalUtilities.ConvertToBool(drAlert["importexposurealerts_sendforwardcontractduedatemail"]))
        {
            query = @"select * from tbl_fimforwardcontract
                left join tbl_bank on bank_bankid=fimforwardcontract_bankid
                left join tbl_exposurecurrencymaster on exposurecurrencymaster_exposurecurrencymasterid=fimforwardcontract_exposurecurrencymasterid 
                where isnull(fimforwardcontract_forwardbalanceamount,0)<>0 and fimforwardcontract_clientid=" + clientId;
            if (isSaturday)
            {
                query += " AND (DATEDIFF(day,GETDATE(),fimforwardcontract_todate) >= 0 AND DATEDIFF(day,GETDATE(),fimforwardcontract_todate) <= 7)";
            }
            else
            {
                query += " AND (DATEDIFF(day,GETDATE(),fimforwardcontract_todate) = 0)";
            }
            dttblForwardContract = DbTable.ExecuteSelect(query);
        }
        //get import order
        if (GlobalUtilities.ConvertToBool(drAlert["importexposurealerts_sendimportorderduedatemail"]))
        {
            query = @"select * from tbl_fimimportorder
                left join tbl_bank on bank_bankid=fimimportorder_bankid            
                left join tbl_exposurecurrencymaster on exposurecurrencymaster_exposurecurrencymasterid=fimimportorder_exposurecurrencymasterid 
                left join tbl_paymentterm on paymentterm_paymenttermid=fimimportorder_paymenttermid
                left join tbl_documenttype on documenttype_documenttypeid=fimimportorder_documenttypeid
                where isnull(fimimportorder_netimportorderamountpayable,0)<>0 and fimimportorder_clientid=" + clientId;
            if (isSaturday)
            {
                query += " AND (DATEDIFF(day,GETDATE(),fimimportorder_expectedduedate) >= 0 AND DATEDIFF(day,GETDATE(),fimimportorder_expectedduedate) <= 7)";
            }
            else
            {
                query += " AND DATEDIFF(day,GETDATE(),fimimportorder_expectedduedate) = 0";
            }
            dttblImportOrder = DbTable.ExecuteSelect(query);
        }

        //get trade credit due
        if (GlobalUtilities.ConvertToBool(drAlert["importexposurealerts_sendtradecreditduedatemail"]))
        {
            query = @"select * from tbl_fimtradecredit
                left join tbl_bank on bank_bankid=fimtradecredit_bankid
                left join tbl_exposurecurrencymaster on exposurecurrencymaster_exposurecurrencymasterid=fimtradecredit_exposurecurrencymasterid 
                left join tbl_tradecredittype on tradecredittype_tradecredittypeid=fimtradecredit_tradecredittypeid                
                where isnull(fimtradecredit_outstandingtradecreditamount,0)<>0 and fimtradecredit_clientid=" + clientId;
            if (isSaturday)
            {
                query += " AND (DATEDIFF(day,GETDATE(),fimtradecredit_tradecreditduedate) >= 0 AND DATEDIFF(day,GETDATE(),fimtradecredit_tradecreditduedate) <= 7)";
            }
            else
            {
                query += " AND DATEDIFF(day,GETDATE(),fimtradecredit_tradecreditduedate) = 0";
            }
            dttblTradeCredit = DbTable.ExecuteSelect(query);
        }

        if (GlobalUtilities.IsValidaTable(dttblForwardContract) == false && GlobalUtilities.IsValidaTable(dttblImportOrder) == false &&
            GlobalUtilities.IsValidaTable(dttblTradeCredit) == false) return;

        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%'>");
        if (GlobalUtilities.IsValidaTable(dttblImportOrder))
        {
            if (isSaturday)
            {
                html.Append("<tr><td style='font-weight:bold;'>Import Order > Due for Next Week</td></tr>");
            }
            else
            {
                html.Append("<tr><td style='font-weight:bold;'>Import Order > Due for Today</td></tr>");
            }
            html.Append("<tr><td><table width='100%' style='border-collapse: collapse;border:1px solid #000;' border='1' cellspacing='0'>");
            GetGridHeader(_dttblImportOrderCols_Import, html);
            GetData(dttblImportOrder, _dttblImportOrderCols_Import, html);
            html.Append("</table></td></tr>");
        }
        if (GlobalUtilities.IsValidaTable(dttblForwardContract))
        {
            html.Append("<tr><td>&nbsp;</td></tr>");
            if (isSaturday)
            {
                html.Append("<tr><td style='font-weight:bold;'>Import Forward Contract > Due for Next Week</td></tr>");
            }
            else
            {
                html.Append("<tr><td style='font-weight:bold;'>Import Forward Contract > Due for Today</td></tr>");
            }

            html.Append("<tr><td><table width='100%' style='border-collapse: collapse;border:1px solid #000;' border='1' cellspacing='0'>");
            GetGridHeader(_dttblForwardContractCols_Import, html);
            GetData(dttblForwardContract, _dttblForwardContractCols_Import, html);
            html.Append("</table></td></tr>");
        }
        if (GlobalUtilities.IsValidaTable(dttblTradeCredit))
        {
            html.Append("<tr><td>&nbsp;</td></tr>");
            if (isSaturday)
            {
                html.Append("<tr><td style='font-weight:bold;'>Import Trade Contract > Due for Next Week</td></tr>");
            }
            else
            {
                html.Append("<tr><td style='font-weight:bold;'>Import Trade Contract > Due for Today</td></tr>");
            }

            html.Append("<tr><td><table width='100%' style='border-collapse: collapse;border:1px solid #000;' border='1' cellspacing='0'>");
            GetGridHeader(_dttblTradeContractCols_Import, html);
            GetData(dttblTradeCredit, _dttblTradeContractCols_Import, html);
            html.Append("</table></td></tr>");
        }

        html.Append("</table>");

        string message = Common.GetSetting("Daily Reminders Mail");
        message = GlobalUtilities.ReplaceString(message, "$data$", html.ToString());
        string subject = "Alerts: Exposure Due date TODAY";
        if (isSaturday)
        {
            subject = "Alerts: Exposure Due date Next Week";
        }
        //bool issent = BulkEmail.SendMail_Alert(emailIds, "Due date mail (" + GlobalUtilities.GetCurrentDateDDMMYYYY() + ")", message, "");
        message = message.Replace("$subject$", subject);
        message = message.Replace("$client_customername$", GlobalUtilities.ConvertToString(drClient["client_customername"]));
        message = message.Replace("$date$", GlobalUtilities.GetCurrentDateDDMMYYYY());

        bool issent = BulkEmail.SendMail_Alert(emailIds, subject, message, "");
        if (issent)
        {
            query = "update tbl_importexposurealerts set importexposurealerts_duesentdate=getdate() " +
                    "where importexposurealerts_clientid=" + clientId;
            DbTable.ExecuteQuery(query);
        }

    }
    private void GetData(DataTable dttbl, DataTable dttblCol, StringBuilder html)
    {
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            html.Append("<tr>");
            for (int j = 0; j < dttblCol.Rows.Count; j++)
            {
                try
                {
                    string label = GlobalUtilities.ConvertToString(dttblCol.Rows[j]["columns_lbl"]);
                    string columnname = GlobalUtilities.ConvertToString(dttblCol.Rows[j]["columns_columnname"]);
                    string control = GlobalUtilities.ConvertToString(dttblCol.Rows[j]["columns_control"]);
                    string dropdowncolumn = GlobalUtilities.ConvertToString(dttblCol.Rows[j]["columns_dropdowncolumn"]);
                    if (dropdowncolumn != "") columnname = dropdowncolumn;
                    string val = "";
                    if (control == "Date")
                    {
                        val = GlobalUtilities.ConvertToDate(dttbl.Rows[i][columnname]);
                    }
                    else
                    {
                        val = GlobalUtilities.ConvertToString(dttbl.Rows[i][columnname]);
                    }
                    html.Append("<td>" + val + "</td>");
                }
                catch (Exception ex)
                {
                    
                }
            }
            html.Append("</tr>");
        }
    }
    private void GetGridHeader(DataTable dttbl, StringBuilder html)
    {
        html.Append("<tr style='font-weight:bold'>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string label = GlobalUtilities.ConvertToString(dttbl.Rows[i]["columns_lbl"]);
            html.Append("<td style='color:#fff;font-weight:bold;background-color:#17365d;'>" + label + "</td>");
        }
        html.Append("</tr>");
    }
    private void SetColumns()
    {
//        string query = @"select * from tbl_columns 
//                         join tbl_module on module_moduleid=columns_moduleid
//                         where columns_isgenerate=1 AND module_modulename='Export Order' order by columns_sequence";
//        _dttblEOCols = DbTable.ExecuteSelect(query);

//        query = @"select * from tbl_columns 
//                         join tbl_module on module_moduleid=columns_moduleid
//                         where columns_isgenerate=1 AND module_modulename='Forward Contract' order by columns_sequence";
//        _dttblFCCols = DbTable.ExecuteSelect(query);

//        query = @"select * from tbl_columns 
//                         join tbl_module on module_moduleid=columns_moduleid
//                         where columns_isgenerate=1 AND module_modulename='PCFC' order by columns_sequence";
//        _dttblPCFCCols = DbTable.ExecuteSelect(query);
        _dttblEOCols = GetColumns("Export Order");
        _dttblFCCols = GetColumns("Forward Contract");
        _dttblPCFCCols = GetColumns("PCFC");
        _dttblImportOrderCols_Import = GetColumns("fim import order");
        _dttblForwardContractCols_Import = GetColumns("fim forward contract");
        _dttblTradeContractCols_Import = GetColumns("fim trade credit");
    }
    private DataTable GetColumns(string m)
    {
        string query = @"select * from tbl_columns 
                         join tbl_module on module_moduleid=columns_moduleid
                         where columns_isgenerate=1 AND module_modulename='" + m + "' order by columns_sequence";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        return dttbl;
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}