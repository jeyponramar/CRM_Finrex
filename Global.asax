<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        BulkEmail_SendClean._apiUrl = "https://api.us1-mta1.sendclean.net/v2/mail/send";
        BulkEmail_SendClean._authKey = "SC.7ebc866f01bcf85c.170d0aad46a7b77ea4ed5ef0";
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        Exception ex = Server.GetLastError();
        WebComponent.ErrorLog.WriteLog(ex.Message);
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
        if (Request["_sid"] == null)
        {
            HttpCookie cookie = new HttpCookie("_sid", Guid.NewGuid().ToString());
            cookie.Expires = DateTime.Now.AddDays(365);
            Response.Cookies.Add(cookie);
        }
        string url = Request.Url.ToString().ToLower();

    }
    void Application_BeginRequest(object sender, EventArgs e)
    {
        string url = Request.Url.ToString().ToLower();
        if (url.Contains("/mobile-phonegap/") || url.Contains("/exe/"))
        {
            HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", "*");
        }
        if (url.Contains("/upload/client/"))
        {
            
        }
        if (!url.Contains("localhost:"))
        {
            if (ConfigurationManager.AppSettings["EnableHTTPS"] == "true")
            {
                if (url.Contains("http://"))
                {
                    url = url.Replace("http://", "https://");
                    HttpContext.Current.Response.Redirect(url);
                    return;
                }
            }
            else if (ConfigurationManager.AppSettings["EnableHTTPS"] == "false")
            {
                if (url.Contains("https://"))
                {
                    url = url.Replace("https://", "http://");
                    HttpContext.Current.Response.Redirect(url);
                    return;
                }
            }
        }
        if (url.Contains("images/user/thumb/"))
        {
            Array arr = url.Split('/');
            if (!System.IO.File.Exists(Server.MapPath("~/images/user/thumb/" + arr.GetValue(arr.Length - 1).ToString())))
            {
                Server.Transfer("~/images/user/thumb/default.jpg");
            }
        }
        else if (url.Contains("images/user/"))
        {
            Array arr = url.Split('/');
            if (!System.IO.File.Exists(Server.MapPath("~/images/user/" + arr.GetValue(arr.Length - 1).ToString())))
            {
                Server.Transfer("~/images/user/default.jpg");
            }
        }
        //if (url.Contains("/upload/"))
        //{
        //    if (GlobalUtilities.ConvertToBool(Session["Login_IsLoggedIn"])
        //        || GlobalUtilities.ConvertToBool(CustomSession.Session("Login_IsRefuxLoggedIn")))
        //    {
        //    }
        //    else
        //    {
        //        Response.Write("Invalid request");
        //        Response.End();
        //    }
        //}
    }
   
    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }
           
</script>
