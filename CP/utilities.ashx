<%@ WebHandler Language="C#" Class="utilities" %>
using WebComponent;
using System;
using System.Web;

public class utilities : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string m = context.Request.QueryString["m"];
        if (m == "column_sequence")
        {
            //update column sequence
            int sequence = Convert.ToInt32(context.Request.QueryString["s"]);
            int moduleid = Convert.ToInt32(context.Request.QueryString["mid"]);
            string query = "update tbl_columns set columns_sequence = columns_sequence+1 where columns_moduleid=" + moduleid + " and columns_sequence>=" + sequence;
            InsertUpdate obj = new InsertUpdate();
            obj.ExecuteQuery(query);            
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}