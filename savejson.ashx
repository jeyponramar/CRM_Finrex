<%@ WebHandler Language="C#" Class="savejson" %>

using System;
using System.Web;
using System.Collections;
using WebComponent;

public class savejson : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        Common.ValidateAjaxRequest();    
        context.Response.ContentType = "text/plain";
        Hashtable hstbl = new Hashtable();
        try
        {
            string u = context.Request.Url.ToString();
            int id = 0;
            for (int i = 0; i < context.Request.Form.Keys.Count; i++)
            {
                string name = context.Request.Form.Keys[i];
                string val = context.Request.Form[i];
                name = name.Replace("~", "");
                name = name.Replace("txt", "").Replace("ddl", "").Replace("chk", "").ToLower();
                if (name.Contains("$"))
                {
                    name = name.Substring(name.LastIndexOf("$") + 1);
                }
                hstbl.Add(name, val);
            }
            string m = context.Request.QueryString["m"];
            InsertUpdate obj = new InsertUpdate();
            //hstbl.Add("createdby", CustomSession.Session("Login_UserId"));
            id = obj.InsertData(hstbl, "tbl_" + m);
            context.Response.Write(id);
        }
        catch (Exception e)
        {
            context.Response.Write("Error : " + e.Message);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}