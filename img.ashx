<%@ WebHandler Language="C#" Class="img" %>

using System;
using System.Web;
using System.IO;

public class img : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "image/jpeg";
        Common.ValidateAjaxRequest();
        string url = context.Request.QueryString["url"];
        if (File.Exists(context.Server.MapPath("~/" + url)))
        {
            context.Response.Redirect("~/" + url);
        }
        else
        {
            Array arr = url.Split('/');
            string folderPath = "";
            for (int i = 0; i < arr.Length - 1; i++)
            {
                folderPath += "/" + Convert.ToString(arr.GetValue(i));
            }
            context.Response.Redirect("~" + folderPath + "/default.jpg");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}