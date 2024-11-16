<%@ WebHandler Language="C#" Class="webhook" %>

using System;
using System.Web;
using System.Net;
using WebComponent;
using System.IO;

public class webhook : IHttpHandler {

    string _token = "ajsdskSkJKdhLdjs@jdkaljdJdajd";
    public void ProcessRequest (HttpContext context) {
        
        if (HttpContext.Current.Request.HttpMethod == "GET")
        {
            ValidateWhatsAppToken(context);
        }
        else if (HttpContext.Current.Request.HttpMethod == "POST")
        {
            NotifyWhatsAppMessage(context);
        }
    }
    private void ValidateWhatsAppToken(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string node = context.Request.QueryString["hub.mode"];
        string challenge = context.Request.QueryString["hub.challenge"];
        string token = context.Request.QueryString["hub.verify_token"];
        if (node == "subscribe")
        {
            if (token == _token)
            {
                context.Response.Write(challenge);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
    private void NotifyWhatsAppMessage(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
        bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
        var bodyText = bodyStream.ReadToEnd();
        ErrorLog.WriteLog("NotifyWhatsAppMessage:"+ bodyText);
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}