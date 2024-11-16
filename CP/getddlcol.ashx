<%@ WebHandler Language="C#" Class="detail" %>
using System.Data;
using System;
using System.Web;
using WebComponent;
using System.Text;
using System.Text.RegularExpressions;

public class detail : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        if (GlobalUtilities.ConvertToString(context.Request.QueryString["id"]) == "") return;
        int id = Convert.ToInt32(context.Request.QueryString["id"]);
        string m = context.Request.QueryString["m"];
        string query = "select top 1 * from tbl_columns where columns_moduleid=" + id + " order by columns_sequence";
        InsertUpdate obj = new InsertUpdate();
        DataTable dttbl = new DataTable();
        dttbl = obj.ExecuteSelect(query);
        string html = "";
        if (dttbl.Rows.Count > 0)
        {
            html = Convert.ToString(dttbl.Rows[0][1]);
        }
        context.Response.Write(html);
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}