<%@ WebHandler Language="C#" Class="ddl" %>

using System;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;
using WebComponent;
public class ddl : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        Common.ValidateAjaxRequest();    
        context.Response.ContentType = "application/json";
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        DropDown ddlBL = new DropDown();
        string m = context.Request.QueryString["m"];
        string cn = context.Request.QueryString["cn"];
        string id = context.Request.QueryString["id"];
        DataTable dttbl = ddlBL.GetData(m,cn,id);

        var results = new System.Collections.Generic.List<object>();

        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            var item = new System.Collections.Generic.Dictionary<string, string>();
            DropDownData data = new DropDownData();
            data.id = Convert.ToInt32(dttbl.Rows[i][0]);
            data.Name = Convert.ToString(dttbl.Rows[i][1]);
            results.Add(data);
        }

        context.Response.Write(serializer.Serialize(results));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}