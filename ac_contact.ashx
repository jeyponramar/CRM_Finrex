<%@ WebHandler Language="C#" Class="ac" %>

using System;
using System.Web;
using System.Data;
using System.Text;
using WebComponent;

public class ac : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        Common.ValidateAjaxRequest();
        context.Response.ContentType = "text/plain";
        string q = context.Request.QueryString["q"].Replace(",","");
        string m = context.Request.QueryString["m"];
        string cn = context.Request.QueryString["cn"];
        StringBuilder data = new StringBuilder();
        string tableName = "tbl_" + m.ToLower();
        int index = tableName.IndexOf('_');
        string prefix = tableName.Substring(index + 1);
        string columnName = prefix + "_" + cn;
        string query = "SELECT TOP 20 * FROM tbl_" + m + " WHERE " + columnName + " LIKE '%" + q + "%'";
        InsertUpdate obj = new InsertUpdate();
        DataTable dttbl = obj.ExecuteSelect(query);
        if (global.IsValidTable(dttbl))
        {
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                data.Append(Convert.ToString(dttbl.Rows[i][columnName]) + "|" + Convert.ToString(dttbl.Rows[i][0]) + "\n");
            }
        }
        context.Response.Write(data.ToString());
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}