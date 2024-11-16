<%@ WebHandler Language="C#" Class="blurpop" %>

using System;
using System.Web;
using WebComponent;

public class blurpop : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}