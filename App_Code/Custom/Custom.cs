using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using WebComponent;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Configuration;

/// <summary>
/// Summary description for Custom
/// </summary>
public enum QuotationFor
{
    NewAmc = 1,
    Complaint = 2,
    Sales = 3,
    AMCService = 4,
    AMCRenewal = 5
}
public enum ProjectStatus
{
    Open = 1,
    InProgress = 2,
    Completed = 3,
    Closed = 4,
    Hold = 5
}
public enum Currency
{
    USDINR = 1,
    EURINR = 2,
    GBPINR = 3,
    JPYINR = 4,
    EURUSD = 5,
    GBPUSD = 6,
    USDJPY = 7
}
public enum EnumFinstationHistoryType
{
    SpotRate = 1,
    RBIRefRate = 2,
    MonthlyAvg = 3,
    CashSpot = 4
}

public static class Custom
{
    public static string AppUrl
    {
        get
        {
            return ConfigurationManager.AppSettings["AppUrl"].ToString();
        }
    }
    public static string FormatAmounWithComma(object amount)
    {
        double dblAmount = Convert.ToDouble(amount);
        string strAmount = Convert.ToString(GlobalUtilities.ConvertToInt(amount));
        if (strAmount.Length < 4) return GlobalUtilities.ConvertToInt(amount).ToString();
        if (strAmount.Length == 4)
        {
            strAmount = strAmount.Insert(1, ",");
        }
        else if (strAmount.Length == 5)
        {
            strAmount = strAmount.Insert(2, ",");
        }
        else if (strAmount.Length == 6)
        {
            strAmount = strAmount.Insert(1, ",");
            strAmount = strAmount.Insert(4, ",");
        }
        else if (strAmount.Length == 7)
        {
            strAmount = strAmount.Insert(2, ",");
            strAmount = strAmount.Insert(5, ",");
        }
        else if (strAmount.Length == 8)
        {
            strAmount = strAmount.Insert(1, ",");
            strAmount = strAmount.Insert(4, ",");
            strAmount = strAmount.Insert(7, ",");
        }
        return strAmount;
    }
    public static string RemoveSpecialCharsForUrl(string data)
    {
        //data = data.ToLower().Replace("'", "").Replace(" ", "-").Replace("?", "").Replace(".", "").Replace("/", "-").Replace("--", "-").Replace("_", "-");

        data = Regex.Replace(data, "[^a-zA-Z0-9_. ]+", "", RegexOptions.Compiled);
        data = data.Replace(" ", "-").Replace("--", "-");
        if (data.EndsWith("-")) data = data.Substring(0, data.Length - 1);
        return data;
    }
    public static void SaveDbConstant(string name, string value)
    {
        InsertUpdate obj = new InsertUpdate();
        string query = "update tbl_dbconstants set dbconstants_value='" + value + "' WHERE dbconstants_name='" + name + "'";
        obj.ExecuteQuery(query);
    }
    public static void SaveDbConstantFromTable(string name, string table, string columnName)
    {
        DataTable dttbl = DbTable.GetOneTableData(table);
        StringBuilder data = new StringBuilder();
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string color = GlobalUtilities.ConvertToString(dttbl.Rows[i][columnName]);
            if (i == 0)
            {
                data.Append(color);
            }
            else
            {
                data.Append("~" + color);
            }
        }
        SaveDbConstant(name, data.ToString());
    }
    public static void UpdateOpportunityStatus(int id)
    {
        DataRow dr = DbTable.GetOneRow("tbl_opportunity", id);
        int status = GlobalUtilities.ConvertToInt(dr["opportunity_opportunitystatusid"]);
        int clientId = GlobalUtilities.ConvertToInt(dr["opportunity_clientid"]);

        int statusId = status;
        int opportunityStatusId = 0;
        if (status == 6)//trial 
        {
           
                statusId = 3;
                opportunityStatusId = 7;
           
        }
        else if (status == 7)//trial expired
        {
            
                statusId = 1;
                opportunityStatusId = 6;
            
        }
        else if (status == 2)//subscribed
        {  
                //subscribed expired
                statusId = 4;
                opportunityStatusId = 8;
            
        }
        else if (status == 8)//subscribed expired
        {      
                //subscribed
                statusId = 2;
                opportunityStatusId = 2;
           
        }
        if (statusId == 0) return;

        //update client status
        string query = "update tbl_client set client_subscriptionstatusid=" + statusId + " where client_clientid=" + clientId;
        DbTable.ExecuteQuery(query);
        //update opportunity status
        if (opportunityStatusId > 0)
        {
            query = "update tbl_opportunity set opportunity_opportunitystatusid=" + statusId + " where opportunity_opportunityid=" + id;
            DbTable.ExecuteQuery(query);
        }
    }
    public static void UpdateSubscriptionStatus(int id)
    {
        UpdateSubscriptionStatus(id, false);//for subscription
    }
    public static void UpdateSubscriptionStatus(int id, bool istrial)
    {
        string module = "subscription";
        if (istrial)
        {
            module = "trial";
        }
        DataRow dr = DbTable.GetOneRow("tbl_"+module, id);
        int status = GlobalUtilities.ConvertToInt(dr[module+"_subscriptionstatusid"]);
        int clientId = GlobalUtilities.ConvertToInt(dr[module+"_clientid"]);
        DateTime dtEndDate = Convert.ToDateTime(dr[module+"_enddate"]);
        int statusId = status;
        if (statusId == 0) statusId = 1;
        if (status == 1)//trial 
        {
            if (dtEndDate < DateTime.Today)
            {
                //trial expired
                statusId = 3;
            }
        }
        else if (status == 3)//trial expired
        {
            if (dtEndDate >= DateTime.Today)
            {
                //trial
                statusId = 1;
            }
        }
        else if (status == 2)//subscribed
        {
            if (dtEndDate < DateTime.Today)
            {
                //subscribed expired
                statusId = 4;
            }
        }
        else if (status == 4)//subscribed expired
        {
            if (dtEndDate >= DateTime.Today)
            {
                //subscribed
                statusId = 2;
            }
        }
        else
        {
            if (module == "subscription")
            {
                if (status == 6)
                {
                    if (dtEndDate < DateTime.Today)
                    {
                        //subscribed expired
                        statusId = 4;
                    }
                    else
                    {
                        statusId = 2;
                    }
                }
            }
        }
        if (statusId == 0) return;

        //update subscription status
        string query = "update tbl_"+module+" set "+module+"_subscriptionstatusid=" + statusId + 
                        " where "+module+"_"+module+"id=" + id;
        DbTable.ExecuteQuery(query);
    }
    public static void UpdateSubscriptionOnClient(int id, bool istrial)
    {
        string m = "subscription";
        if (istrial) m = "trial";
        string query = "update tbl_client set client_employeeid=" + m + "_employeeid,client_startdate=" + m + "_startdate," +
                     "client_enddate=" + m + "_enddate,client_whatsappenddate=" + m + "_whatsappenddate," +
                     "client_subscriptionstatusid=" + m + "_subscriptionstatusid,client_subscriptionid=" + id + " " +
                     "FROM tbl_client JOIN tbl_" + m + " ON client_clientid=" + m + "_clientid " +
                     "WHERE " + m + "_" + m + "id=" + id;
        DbTable.ExecuteQuery(query);
        if (m == "subscription")
        {
            DataRow drs = DbTable.GetOneRow("tbl_subscription", id);
            int clientId = GlobalUtilities.ConvertToInt(drs["subscription_clientid"]);
            query = "select * from tbl_client where client_clientid=" + clientId;
            DataRow drc = DbTable.ExecuteSelectRow(query);
            int servicePlanId = 0;
            servicePlanId = GlobalUtilities.ConvertToInt(drs["subscription_serviceplanid"]);
            if (GlobalUtilities.ConvertToInt(drc["client_serviceplanid"]) == 0)
            {
                query = "update tbl_client set client_serviceplanid=" + servicePlanId + " where client_clientid=" + clientId;
                DbTable.ExecuteQuery(query);
            }
        }
    }
    public static void SaveFutureFeedback(int clientId)
    {
        string query = "select * from tbl_client where client_clientid=" + clientId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        int contactType = GlobalUtilities.ConvertToInt(dr["client_contacttypeid"]);
        int status = GlobalUtilities.ConvertToInt(dr["client_subscriptionstatusid"]);
        int employeeId = GlobalUtilities.ConvertToInt(dr["client_employeeid"]);
        if (status != 2) return;//return if not subscribed
        if (contactType == 0) return;

        int months = 0;
        if (contactType == 1)
        {
            months = 3;
        }
        else if (contactType == 2)
        {
            months = 6;
        }
        else if (contactType == 3)
        {
            months = 12;
        }
        query = "delete from tbl_feedback WHERE feedback_followupstatusid=1 AND feedback_clientid=" + clientId;
        DbTable.ExecuteQuery(query);

        DateTime startDate = Convert.ToDateTime(dr["client_startdate"]);
        DateTime endDate = Convert.ToDateTime(dr["client_enddate"]);
        DateTime dt = startDate.AddMonths(months);
        while (dt <= endDate)
        {
            query = "select * from tbl_feedback WHERE feedback_clientid=" + clientId +
                    " AND MONTH(feedback_date)=" + dt.Month + " AND YEAR(feedback_date)=" + dt.Year;
            DataRow drFeedback = DbTable.ExecuteSelectRow(query);
            if (drFeedback == null)
            {
                string date = GlobalUtilities.ConvertToDate(dt);
                Hashtable hstbl = new Hashtable();
                hstbl.Add("clientid",clientId);
                hstbl.Add("date", date);
                hstbl.Add("followupstatusid", 1);
                hstbl.Add("employeeid", employeeId);
                InsertUpdate obj = new InsertUpdate();
                obj.InsertData(hstbl, "tbl_feedback");
            }
            dt = dt.AddMonths(months);
        }
    }
    public static int SaveClient(string customerName, string emailId, string contactPerson, string landlineNo,
        string mobileNo, string website, int designationId, int campaignId, int stateId, int areaId, int exposureId, int businessId, int industrytypeId)
    {
        int maxid = GlobalUtilities.GetMaxId("client");
        string Customercode = "C-" + maxid;
        Hashtable hstbl = new Hashtable();
        InsertUpdate obj = new InsertUpdate();
        hstbl.Add("customername", customerName);
        hstbl.Add("customercode", Customercode);
        //hstbl.Add("billingname", customerName);
        hstbl.Add("emailid", emailId);
        hstbl.Add("contactperson", contactPerson);
        hstbl.Add("landlineno", landlineNo);
        hstbl.Add("mobileno", mobileNo);
        hstbl.Add("website", website);
        hstbl.Add("designationid", designationId);
        hstbl.Add("campaignid", campaignId);
        hstbl.Add("stateid", stateId);
        hstbl.Add("areaid", areaId);
        hstbl.Add("exposureid", exposureId);
        hstbl.Add("businessid", businessId);
        hstbl.Add("industrytypesid", industrytypeId);
        hstbl.Add("contacttypeid", 1);
        int id = obj.InsertData(hstbl, "tbl_client");
        if (id > 0)
        {
            int ledgerId = Accounts.SaveLedger(customerName, LedgerGroup.SundryDebtor, "", "", 0,
                            customerName, LedgerType.Customer);
            Contact.SaveMainContact(id, contactPerson, designationId, emailId, mobileNo, landlineNo);
        }
        return id;
    }
    public static string RoleWhere(string module, bool isAddAND)
    {
        string where = "";
        if (Common.RoleId > 1)
        {
            int userId = Common.UserId;
            int employeeId = Common.EmployeeId;
            int managerId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ManagerId"));
            int backupPersondId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_BackupPersonId"));
            where = "(" + module + "_createdby=" + userId;

            where += " OR " + module + "_employeeid=" + employeeId;
            string childEmployees = CustomSession.Session("Login_ChildEmployees");
            if (childEmployees != "" && childEmployees != null)
            {
                where += " OR " + module + "_employeeid IN(" + childEmployees + ")";
            }
            where += ")";
        }
        if (isAddAND)
        {
            if (where != "")
            {
                where = " AND " + where;
            }
        }
        return where;
    }
    public static double GetSportRate(int currency, bool isExport)
    {
        int liverateId = 0;
        if (currency == 1)//USDINR
        {
            liverateId = 1;
        }
        else if (currency == 2)//EURINR
        {
            liverateId = 10;
        }
        else if (currency == 3)//GBPINR
        {
            liverateId = 19;
        }
        else if (currency == 4)//JPYINR
        {
            liverateId = 28;
        }
        else if (currency == 5)//EURUSD
        {
            liverateId = 37;
        }
        else if (currency == 6)//GBPUSD
        {
            liverateId = 46;
        }
        else if (currency == 7)//USDJPY
        {
            liverateId = 55;
        }
        if (isExport == false)
        {
            liverateId = liverateId + 1;
        }
        string query = "select * from tbl_liverate where liverate_liverateid=" + liverateId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return 0;
        return GlobalUtilities.ConvertToDouble(dr["liverate_currentrate"]);
    }
    public static void CheckSubscriptionAccess()
    {
        if (GlobalUtilities.ConvertToInt(CustomSession.Session("Login_SubscriptionStatusId")) != 2)
        {
            HttpContext.Current.Response.Redirect("~/noaccessfortrial.aspx");
        }
    }
    
    public static double GetLiveRate(int rateId)
    {
        string query = "select * from tbl_liverate WHERE liverate_liverateid=" + rateId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return 0;
        return GlobalUtilities.ConvertToDouble(dr["liverate_currentrate"]);
    }
    public static void CreateAllTableColumns()
    {
        string query = @"select * from tbl_module
                        where ";
        query += @" module_moduleid in(
                    select columns_moduleid from tbl_columns
                    where isnull(columns_submoduleid,0)=0 and 
                    columns_control in('Text Box','Auto Complete','Email Id','Mobile No','Phone No',
                    'Amount','Number','Date','Date Time','Multi Line','Html Editor','Dropdown','Checkbox',
                    'File Upload')
                    and columns_columnname not in(select name from syscolumns)
                    )";

        DataTable dttbl = DbTable.ExecuteSelect(query);
        //HttpContext.Current.Response.Write(dttbl.Rows.Count);
        //HttpContext.Current.Response.End();
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            DataRow dr = dttbl.Rows[i];
            string moduleName = Convert.ToString(dr["module_modulename"]);
            string tableName = Convert.ToString(dr["module_tablename"]);
            int moduleId = GlobalUtilities.ConvertToInt(dr["module_moduleid"]);
            //database changes
            CP objCp = new CP();
            objCp.CheckAndCreateTable(tableName);
            objCp.CreateTableColumns(tableName, moduleId);
            objCp.AddCreatedDate(tableName);
        }
    }
    public static bool IsPasswordStrength(string password)
    {
        if (password.Length < 6) return false;
        char[] arr = {'!', '@', '#', '$', '%', '^', '&', '*', '(', ')'};
        int isSymbol = password.IndexOfAny(arr);
        bool result = password.Any(c => char.IsLetter(c)) && password.Any(c => char.IsDigit(c)) && (isSymbol >= 0);
        return result;
    }
    public static void UpdateClientProspects(string m, int id, int clientId, int servicePlanId)
    {
        string query = "";
        string prospectmodule = "";
        string servicemodule = "";
        int userId = Common.UserId;
        if (m == "invoice")
        {
            //if its not latest invoice then dont update
            query = "select top 1 * from tbl_invoice where invoice_clientid=" + clientId +
                    " order by invoice_invoiceid desc";
            DataRow drinvoice = DbTable.ExecuteSelectRow(query);
            if (drinvoice != null)
            {
                if (GlobalUtilities.ConvertToInt(drinvoice["invoice_invoiceid"]) != id) return;
            }
        }
        else if (m == "proformainvoice")
        {
            //if its not latest invoice then dont update
            query = "select top 1 * from tbl_proformainvoice where proformainvoice_clientid=" + clientId +
                    " order by proformainvoice_proformainvoiceid desc";
            DataRow drproinvoice = DbTable.ExecuteSelectRow(query);
            if (drproinvoice != null)
            {
                if (GlobalUtilities.ConvertToInt(drproinvoice["proformainvoice_proformainvoiceid"]) != id) return;
            }
        }
        if (m == "trial")
        {
            prospectmodule = "trialprospect";
        }
        else
        {
            prospectmodule = m + "prospects";
            servicemodule = m + "services";
        }
        query = "delete from tbl_clientprospects where clientprospects_clientid=" + clientId +
                " and clientprospects_prospectid not in(select " + prospectmodule + "_prospectid from tbl_" + prospectmodule +
                            " where " + prospectmodule + "_" + m + "id=" + id + ")";
        DbTable.ExecuteQuery(query);
        query = "insert into tbl_clientprospects(clientprospects_clientid,clientprospects_prospectid,clientprospects_createddate,clientprospects_createdby)" +
                "select " + clientId + "," + prospectmodule + "_prospectid,getdate()," + userId + "  from tbl_" + prospectmodule +
                            " where " + prospectmodule + "_" + m + "id=" + id +
                            " and " + prospectmodule + "_prospectid not in(select clientprospects_prospectid from tbl_clientprospects where clientprospects_clientid=" + clientId + ")";
        DbTable.ExecuteQuery(query);
        if (m != "trial")
        {
            query = "delete from tbl_clientservices where clientservices_clientid=" + clientId +
                    " and clientservices_serviceid not in(select " + servicemodule + "_serviceid from tbl_" + servicemodule +
                                " where " + servicemodule + "_" + m + "id=" + id + ")";
            DbTable.ExecuteQuery(query);
            query = "insert into tbl_clientservices(clientservices_clientid,clientservices_serviceid,clientservices_createddate,clientservices_createdby)" +
                    "select " + clientId + "," + servicemodule + "_serviceid,getdate()," + userId + "  from tbl_" + servicemodule +
                                " where " + servicemodule + "_" + m + "id=" + id+
                                " and " + servicemodule + "_serviceid not in(select clientservices_serviceid from tbl_clientservices where clientservices_clientid=" + clientId + ")";
            DbTable.ExecuteQuery(query);
        }
        if (m == "invoice")
        {
            query = "select top 1 * from tbl_subscription where subscription_clientid=" + clientId + " order by subscription_subscriptionid desc";
            DataRow drsubs = DbTable.ExecuteSelectRow(query);
            if (drsubs != null)
            {
                int subscriptionId = GlobalUtilities.ConvertToInt(drsubs["subscription_subscriptionid"]);
                query = "update tbl_subscription set subscription_serviceplanid=" + servicePlanId + " where subscription_subscriptionid=" + subscriptionId;
                DbTable.ExecuteQuery(query);

                query = "delete from tbl_subscriptionprospects where subscriptionprospects_subscriptionid=" + subscriptionId +
                " and subscriptionprospects_prospectid not in(select " + prospectmodule + "_prospectid from tbl_" + prospectmodule +
                            " where " + prospectmodule + "_" + m + "id=" + id + ")";
                DbTable.ExecuteQuery(query);
                query = "insert into tbl_subscriptionprospects(subscriptionprospects_subscriptionid,subscriptionprospects_prospectid,subscriptionprospects_createddate,subscriptionprospects_createdby)" +
                        "select " + subscriptionId + "," + prospectmodule + "_prospectid,getdate()," + userId + "  from tbl_" + prospectmodule +
                                    " where " + prospectmodule + "_" + m + "id=" + id+
                                    " and " + prospectmodule + "_prospectid not in(select subscriptionprospects_prospectid from tbl_subscriptionprospects where subscriptionprospects_subscriptionid=" + subscriptionId + ")";
                DbTable.ExecuteQuery(query);

                query = "delete from tbl_subscriptionservices where subscriptionservices_subscriptionid=" + subscriptionId +
                    " and subscriptionservices_serviceid not in(select " + servicemodule + "_serviceid from tbl_" + servicemodule +
                                " where " + servicemodule + "_" + m + "id=" + id + ")";
                DbTable.ExecuteQuery(query);
                query = "insert into tbl_subscriptionservices(subscriptionservices_subscriptionid,subscriptionservices_serviceid,subscriptionservices_createddate,subscriptionservices_createdby)" +
                        "select " + subscriptionId + "," + servicemodule + "_serviceid,getdate()," + userId + "  from tbl_" + servicemodule +
                                    " where " + servicemodule + "_" + m + "id=" + id+
                                    " and " + servicemodule + "_serviceid not in(select subscriptionservices_serviceid from tbl_subscriptionservices where subscriptionservices_subscriptionid=" + subscriptionId + ")";
                DbTable.ExecuteQuery(query);
            }
        }
        //query = "update tbl_client set client_serviceplanid=" + servicePlanId + " where client_clientid=" + clientId;
        //DbTable.ExecuteQuery(query);
    }
    public static DataRow GetEnquiryData(int id)
    {
        string query = "select * from tbl_enquiry " +
                       "JOIN tbl_client ON client_clientid=enquiry_clientid " +
                       "WHERE enquiry_enquiryid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        return dr;
    }
    public static DataRow GetTrialData(int id)
    {
        string query = "select * from tbl_trial " +
                       "JOIN tbl_client ON client_clientid=trial_clientid " +
                       "WHERE trial_trialid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        return dr;
    }
    public static DataRow GetProformaInvoiceData(int id)
    {
        string query = @"select * from tbl_proformainvoice
                         join tbl_client on client_clientid=proformainvoice_clientid
                         where proformainvoice_proformainvoiceid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        return dr;
    }
    public static DataRow GetInvoiceData(int id)
    {
        string query = @"select * from tbl_invoice
                         join tbl_client on client_clientid=invoice_clientid
                         where invoice_invoiceid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        return dr;
    }
    public static DataRow GetContactsClientUserData(int id)
    {
        string query = @"select * from tbl_contacts
                        join tbl_clientuser on clientuser_contactsid=contacts_contactsid
                        join tbl_client on client_clientid=contacts_clientid
                         where contacts_contactsid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        return dr;
    }
}