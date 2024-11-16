<%@ WebHandler Language="C#" Class="detail" %>

using System;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;
using WebComponent;
using System.Text;

public class DetailData
{
    public string column;
    public string value;
}
public class detail : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        Common.ValidateAjaxRequest();    
        context.Response.ContentType = "application/json";
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        DropDown ddlBL = new DropDown();
        string m = context.Request.QueryString["m"];
        string jt = context.Request.QueryString["jt"];
        int id = 0;
        id = GlobalUtilities.ConvertToInt(context.Request.QueryString["id"]);
        string where = context.Request.QueryString["w"];
        string joinTables = "";
        string query = "";
        if (m == "popcompanyzone")
        {
            int bid = GlobalUtilities.ConvertToInt(context.Request.QueryString["bid"]);
            int cid = GlobalUtilities.ConvertToInt(context.Request.QueryString["cid"]);
            query = @"select * from tbl_branch 
                    JOIN tbl_customerbranch ON customerbranch_branchid=branch_branchid 
                    LEFT JOIN tbl_companyzone  ON companyzone_companyzoneid =  customerbranch_companyzoneid
                    LEFT JOIN tbl_companysubzone ON   companysubzone_companysubzoneid = customerbranch_companysubzoneid                           
                    where branch_branchid=" + bid + " and customerbranch_customerid=" + cid;
        }
        else if (m == "taskdetail")
        {
            int taskId = GlobalUtilities.ConvertToInt(context.Request.QueryString["tid"]);
            query = "SELECT * FROM tbl_task JOIN tbl_tasktype ON tasktype_tasktypeid=task_tasktypeid " +
                    "LEFT JOIN tbl_employee ON employee_employeeid=task_employeeid "+
                    "JOIN tbl_status ON status_statusid=task_statusid " +
                    "LEFT JOIN tbl_client ON client_clientid=task_clientid "+
                    "WHERE task_taskid=" + taskId;
        }
        else if (m == "trackordernumber")
        {
            string Order = GlobalUtilities.ConvertToString(context.Request.QueryString["orno"]);
            query = "SELECT *,order_orderid AS OrderId FROM tbl_order WHERE order_ordernumber='" + Order + "'";
        }
        else if (m == "get-client-serviceplan-details")
        {
            GetClientServicePlanDetails();
            return;
        }
        else if (m == "get-serviceplan-details")
        {
            GetServiceplanDetails();
            return;
        }
        else
        {
            if (where == null || where == "")
            {
                where = m + "_" + m + "id=" + id;
            }
            if (jt != null && jt != "")
            {
                Array arrjt = jt.Split(',');
                for (int i = 0; i < arrjt.Length; i++)
                {
                    joinTables += " LEFT JOIN tbl_" + arrjt.GetValue(i).ToString() + " ON " + arrjt.GetValue(i).ToString() + "_" + arrjt.GetValue(i).ToString()
                                    + "id=" + m + "_" + arrjt.GetValue(i).ToString() + "id";
                }
            }
            query = "select * from tbl_" + m + joinTables + " where " + where;
        }
        InsertUpdate obj = new InsertUpdate();
        DataTable dttbl = obj.ExecuteSelect(query);
        string module = Common.GetQueryString("module");
        if (GlobalUtilities.IsValidaTable(dttbl))
        {
            if (m == "client")
            {
                dttbl.Columns.Add("directorname"); dttbl.Columns.Add("directormobile"); dttbl.Columns.Add("directoremailid");
                dttbl.Columns.Add("financename"); dttbl.Columns.Add("financemobile"); dttbl.Columns.Add("financeemailid");
                dttbl.Columns.Add("officetelno"); dttbl.Columns.Add("officeemailid");
                dttbl.Columns.Add("billingaddress"); dttbl.Columns.Add("courieraddress");
                query = "select top 1 * from tbl_contacts where contacts_clientid=" + id + " and contacts_designationid=1";
                DataRow dro = DbTable.ExecuteSelectRow(query);
                if (dro != null)
                {
                    dttbl.Rows[0]["directorname"] = GlobalUtilities.ConvertToString(dro["contacts_contactperson"]);
                    dttbl.Rows[0]["directormobile"] = GlobalUtilities.ConvertToString(dro["contacts_mobileno"]);
                    dttbl.Rows[0]["directoremailid"] = GlobalUtilities.ConvertToString(dro["contacts_emailid"]);

                }
                query = "select top 1 * from tbl_contacts where contacts_clientid=" + id + " and contacts_designationid=4";
                DataRow drf = DbTable.ExecuteSelectRow(query);
                if (drf != null)
                {
                    dttbl.Rows[0]["financename"] = GlobalUtilities.ConvertToString(drf["contacts_contactperson"]);
                    dttbl.Rows[0]["financemobile"] = GlobalUtilities.ConvertToString(drf["contacts_mobileno"]);
                    dttbl.Rows[0]["financeemailid"] = GlobalUtilities.ConvertToString(drf["contacts_emailid"]);
                }
                dttbl.Rows[0]["officetelno"] = GlobalUtilities.ConvertToString(dttbl.Rows[0]["client_landlineno"]);
                dttbl.Rows[0]["officeemailid"] = GlobalUtilities.ConvertToString(dttbl.Rows[0]["client_officeaccountsemailid"]);
                dttbl.Rows[0]["billingaddress"] = GlobalUtilities.ConvertToString(dttbl.Rows[0]["client_address"]);
                dttbl.Rows[0]["courieraddress"] = GlobalUtilities.ConvertToString(dttbl.Rows[0]["client_address"]);
                if (module == "proformainvoice" || module == "invoice")
                {
                    query = "select top 1 * from tbl_" + module + " where +" + module + "_clientid=" + id + " order by " + module + "_" + module + "id desc";
                    DataRow drinvoice = DbTable.ExecuteSelectRow(query);
                    if (drinvoice != null)
                    {
                        double taxableAmount = GlobalUtilities.ConvertToDouble(drinvoice[module + "_taxableamount"]);
                        dttbl.Columns.Add("taxableamount");
                        dttbl.Rows[0]["taxableamount"] = taxableAmount;
                        int invoiceId = GlobalUtilities.ConvertToInt(drinvoice[module + "_" + module + "id"]);
                        query = "select * from tbl_" + module + "services where " + module + "services_" + module + "id=" + invoiceId;
                        DataTable dttblservices = DbTable.ExecuteSelect(query);
                        string serviceIds = GlobalUtilities.CommaSeparatedFromDataTable(dttblservices, module + "services_serviceid");
                        dttbl.Columns.Add("serviceids"); dttbl.Columns.Add("serviceplanid");
                        dttbl.Rows[0]["serviceids"] = serviceIds;
                        int servicePlanId = GlobalUtilities.ConvertToInt(drinvoice[module + "_serviceplanid"]);
                        dttbl.Rows[0]["serviceplanid"] = servicePlanId;
                        if (servicePlanId > 0)
                        {
                            DataRow drserviceplan = DbTable.GetOneRow("tbl_serviceplan", servicePlanId);
                            string servicePlan = "";
                            if (drserviceplan != null) servicePlan = GlobalUtilities.ConvertToString(drserviceplan["serviceplan_planname"]);
                            query = @"select * from tbl_serviceplanprospects where serviceplanprospects_serviceplanid=" + servicePlanId;
                            DataTable dttblprospects = DbTable.ExecuteSelect(query);
                            string prospectIds = GlobalUtilities.CommaSeparatedFromDataTable(dttblprospects, "serviceplanprospects_prospectid");
                            dttbl.Columns.Add("serviceplan"); dttbl.Columns.Add("prospectids");
                            dttbl.Rows[0]["serviceplan"] = servicePlan;
                            dttbl.Rows[0]["prospectids"] = prospectIds;
                        }
                    }
                }
            }
        }
        var results = new System.Collections.Generic.List<object>();

        if (dttbl.Rows.Count > 0)
        {
            for (int i = 0; i < dttbl.Columns.Count; i++)
            {
                var item = new System.Collections.Generic.Dictionary<string, string>();
                string val = Convert.ToString(dttbl.Rows[0][i]).Replace("\"", "&dquot;").Replace("'","&quot;");
                DetailData data = new DetailData();
                data.column = dttbl.Columns[i].ColumnName;
                data.value = val;
                results.Add(data);
            }
        }
        context.Response.Write(serializer.Serialize(results));
    }
    private void GetClientServicePlanDetails()
    {
        string query = "";
        int clientId = Common.GetQueryStringValue("id");
        query = @"select * from tbl_client
                 join tbl_serviceplan on serviceplan_serviceplanid=client_serviceplanid
                 where client_clientid=" + clientId;
        DataRow drc = DbTable.ExecuteSelectRow(query);
        int serviceplanId = 0;
        string servicePlan = "";
        if (drc != null)
        {
            serviceplanId = GlobalUtilities.ConvertToInt(drc["serviceplan_serviceplanid"]);
            servicePlan = GlobalUtilities.ConvertToString(drc["serviceplan_planname"]);
        }
        query = @"select * from tbl_clientprospects where clientprospects_clientid="+clientId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        string prospectIds  = GlobalUtilities.CommaSeparatedFromDataTable(dttbl, "clientprospects_prospectid");

        query = @"select * from tbl_clientservices where clientservices_clientid=" + clientId;
        dttbl = DbTable.ExecuteSelect(query);
        string serviceIds = GlobalUtilities.CommaSeparatedFromDataTable(dttbl, "clientservices_serviceid");
        string json = "{\"serviceplanid\":\"" + serviceplanId + "\",\"serviceplan\":\"" + servicePlan + "\"," +
                        "\"prospectids\":\"" + prospectIds + "\",\"serviceids\":\"" + serviceIds + "\"}";
        HttpContext.Current.Response.Write(json);
    }
    private void GetServiceplanDetails()
    {
        string query = "";
        int servicePlanId = Common.GetQueryStringValue("id");

        query = @"select * from tbl_serviceplanprospects where serviceplanprospects_serviceplanid=" + servicePlanId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        string prospectIds = GlobalUtilities.CommaSeparatedFromDataTable(dttbl, "serviceplanprospects_prospectid");

        query = @"select * from tbl_serviceplanservices where serviceplanservices_serviceplanid=" + servicePlanId;
        dttbl = DbTable.ExecuteSelect(query);
        string serviceIds = GlobalUtilities.CommaSeparatedFromDataTable(dttbl, "serviceplanservices_serviceid");
        string json = "{\"prospectids\":\"" + prospectIds + "\",\"serviceids\":\"" + serviceIds + "\"}";
        HttpContext.Current.Response.Write(json);
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}