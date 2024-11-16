<%@ WebHandler Language="C#" Class="status" %>

using System;
using System.Web;
using WebComponent;
using System.Data;
using System.Data.SqlClient;

public class status : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {

        context.Response.ContentType = "text/plain";
        string m = context.Request.QueryString["m"].ToString().ToLower();
        int sid = GlobalUtilities.ConvertToInt(context.Request.QueryString["sid"]);
        int id = GlobalUtilities.ConvertToInt(context.Request.QueryString["id"]);
        string statuscol = context.Request.QueryString["sc"].ToString();
        string approvedbyCol = context.Request.QueryString["ab"].ToString();
        string approvedDateCol = context.Request.QueryString["ad"].ToString();
        string tableName = "tbl_" + m;
        if (statuscol == "")
        {
            statuscol = m + "_" + m + "statusid";
        }
        else
        {
            statuscol = m + "_" + statuscol;
        }
        string extraCols = "";
        if (approvedbyCol != "")
        {
            approvedbyCol = m + "_" + approvedbyCol;
            approvedDateCol = m + "_" + approvedDateCol;
            extraCols = "," + approvedbyCol + "=" + CustomSession.Session("Login_UserId") + "," + approvedDateCol + "='" + GlobalUtilities.GetCurrentDateTimeSQL() + "'";
            
        }
        InsertUpdate obj = new InsertUpdate();
        string query = "update " + tableName + " set " + statuscol + "=" + sid + extraCols + " where " + m + "_" + m + "id=" + id;
        obj.ExecuteQuery(query);
        context.Response.Write("1");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}