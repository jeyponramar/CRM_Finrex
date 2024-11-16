<%@ WebHandler Language="C#" Class="adddata" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using WebComponent;

public class adddata : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        Common.ValidateAjaxRequest();    
        context.Response.ContentType = "text/plain";
        Hashtable hstbl = new Hashtable();
        try
        {
            int id = 0;
            for (int i = 0; i < context.Request.Form.Keys.Count; i++)
            {
                string name = context.Request.Form.Keys[i];
                if (name.StartsWith("@"))
                {
                    string val = context.Request.Form[i];
                    name = name.Replace("@", "");
                    name = name.Replace("~", "");
                    hstbl.Add(name, val);
                }
            }
            InsertUpdate obj = new InsertUpdate();
            id = obj.InsertData(hstbl,"tbl_complaint");
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