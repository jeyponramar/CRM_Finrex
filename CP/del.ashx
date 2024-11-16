<%@ WebHandler Language="C#" Class="del" %>

using System;
using System.Web;
using WebComponent;

public class del : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        int id = Convert.ToInt32(context.Request.QueryString["id"]);
        string m = Convert.ToString(context.Request.QueryString["m"]);
        if (id != 0)
        {
            string query = "delete from tbl_" + m + " where " + m + "_" + m + "id=" + id;
            InsertUpdate obj = new InsertUpdate();
            obj.ExecuteQuery(query);
            if (context.Request.QueryString["t"] == "column")
            {
                string cn = context.Request.QueryString["cn"];
                string tableName = "tbl_" + cn.Substring(0, cn.IndexOf("_"));
                query = "alter table " + tableName + " drop column " + cn;
                InsertUpdate obj1 = new InsertUpdate();
                obj1.ExecuteQuery(query);
            }
            context.Response.Write("1");            
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}