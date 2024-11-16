<%@ WebHandler Language="C#" Class="mobile_handler" %>

using System;
using System.Web;

public class mobile_handler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        MobileHandler obj = new MobileHandler();
        obj.ProcessRequest();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}