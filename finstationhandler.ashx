<%@ WebHandler Language="C#" Class="finstationhandler" %>

using System;
using System.Web;

public class finstationhandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        if (!GlobalUtilities.ConvertToBool(CustomSession.Session("Login_IsLoggedIn")))
            return;    
        Finstation obj = new Finstation();
        obj.Process();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}