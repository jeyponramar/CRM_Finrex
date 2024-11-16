<%@ WebHandler Language="C#" Class="mobile" %>

using System;
using System.Web;
using WebComponent;
using System.Data;

public class mobile : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        //context.Response.ContentType = "text/plain";
        context.Response.ContentType = "text/javascript";
        context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
        string callback = Common.GetQueryString("callback");
        string m = Common.GetQueryString("m");
        string query = "";
        string response = "";
        if (m == "login")
        {
            string username = global.CheckInputData(context.Request["username"]);
            string password = global.CheckInputData(context.Request["password"]);
            query = "select * from tbl_clientuser where clientuser_ismobileuser=1 and " +
                    "clientuser_username='" + username + "' and clientuser_password='" + password + "'";
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (dr == null)
            {
                response = "Error:Invalid User Name / Password";
            }
            else
            {
                string json = "Ok";//JSON.Convert(dr);
                response = json.Replace("\n", "__NEWLINE__").Replace("\r", "__NEWLINER__");
            }
        }
        if(callback!="")response = callback + "('" + response + "');";
        HttpContext.Current.Response.Write(response);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}