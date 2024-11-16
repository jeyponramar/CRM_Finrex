<%@ WebHandler Language="C#" Class="mobile_handler" %>

using System;
using System.Web;

public class mobile_handler : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        //context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
        MobileApp_PhoneGap obj = new MobileApp_PhoneGap();
        obj.ProcessRequest(context);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}