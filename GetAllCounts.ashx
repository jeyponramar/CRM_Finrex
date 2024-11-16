<%@ WebHandler Language="C#" Class="GetClients" %>

using System;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;

public class GetClients : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "application/text";
        //JavaScriptSerializer serializer = new JavaScriptSerializer();
        CommonDAO obj = new CommonDAO();
        DataTable dttbl = obj.GetAllCounts();

        //var results = new System.Collections.Generic.List<object>();
        string results = "";
        for (int i = 0; i < dttbl.Columns.Count; i++)
        {
            if (i == 0)
            {
                results = Convert.ToString(dttbl.Rows[0][i]);
            }
            else
            {
                results += "," + Convert.ToString(dttbl.Rows[0][i]);
            }
        }
        context.Response.Write(results);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}