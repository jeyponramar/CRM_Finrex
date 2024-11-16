<%@ WebHandler Language="C#" Class="commodityhandler" %>

using System;
using System.Web;

public class commodityhandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        Commodity com = new Commodity();
        com.Process();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}