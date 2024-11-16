<%@ WebHandler Language="C#" Class="liverate" %>

using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using WebComponent;

public class liverate : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        
        string action = global.CheckInputData(context.Request.QueryString["a"]);
        try
        {
            string query = "";
            if (action == "u")
            {
                string rtds = global.CheckInputData(context.Request["rtd"]).Replace("___","#");
                string values = global.CheckInputData(context.Request["v"]);

                //GlobalUtilities.WriteFile(context.Server.MapPath("~/doc/log/liverate.txt"), rtds + Environment.NewLine + values);
                
                //rtd = "17#1#1#6069#USDINRCOMP#Bid";
                Array arrRtd = rtds.Split(',');
                Array arrValues = values.Split(',');
                
                for (int i = 0; i < arrRtd.Length; i++)
                {
                    string rtd = arrRtd.GetValue(i).ToString();
                    string value = arrValues.GetValue(i).ToString();
                    //if (rtd != "")
                    {
                        query = "UPDATE tbl_liverate SET liverate_prevrate=liverate_currentrate,liverate_currentrate='" + value + "',liverate_modifieddate=GETDATE() WHERE liverate_rtdcode='" + rtd + "'";

                        DbTable.ExecuteQuery(query);
                    }
                }
                context.Response.Write("Success");
            }
            else if (action == "s")
            {
                query = "select * from tbl_session where SessionName='Login_IsLoggedIn' AND SessionID='" + CustomSession.SessionID + "'";
                DataRow drSession = DbTable.ExecuteSelectRow(query);
                if (drSession == null)
                {
                    context.Response.Write("Session Expired");
                    return;
                }
                query = "select liverate_liverateid as rid,liverate_currentrate as cr from tbl_liverate where DATEDIFF(s,liverate_modifieddate,getdate()) < 50";
                DataTable dttbl = DbTable.ExecuteSelect(query);
                context.Response.Write(JSON.Convert(dttbl));
            }
            else if (action == "exe" || action == "mobile-liverate")
            {
                string source = Common.GetQueryString("source");
                query = @"select liverate_liverateid as rid,liverate_currentrate as cr from tbl_liverate 
                          where liverate_liverateid IN(1,4,5,10,13,14,19,22,23,37,40,41,46,76,77)";
//                query = @"select liverate_liverateid as rid,liverate_currentrate as cr from tbl_liverate 
////                          where liverate_liverateid IN(1,4,5,10,13,14,19,22,23,37,40,41,46,76,77) AND DATEDIFF(s,liverate_modifieddate,getdate()) < 50";
                if (!IsLocal)
                {
                    query += " AND DATEDIFF(s,liverate_modifieddate,getdate()) < 50";
                }
                DataTable dttbl = DbTable.ExecuteSelect(query);
                Random rnd = new Random();
                if (IsLocal)
                {
                    dttbl.Rows[0]["cr"] = rnd.Next(1, 100);
                }
                if (action == "mobile-liverate" && source == "phonegap")
                {
                    context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
                    if (source == "phonegap")
                    {
                        context.Response.ContentType = "text/javascript";
                    }
                    string callback = Common.GetQueryString("callback");
                    string json = JSON.Convert(dttbl);
                    string response = json.Replace("\n", "__NEWLINE__").Replace("\r", "__NEWLINER__");
                    if (callback != "")
                    {
                        response = callback + "('" + response + "');";
                    }
                    HttpContext.Current.Response.Write(response);
                }
                else
                {
                    context.Response.Write(JSON.Convert(dttbl));
                }
            }
            else if (action == "exe-heartbeat")
            {
                string sessionId = global.CheckInputData(Common.GetQueryString("sid"));
                query = "select * from tbl_clientuser where clientuser_exesessionid='" + sessionId + "'";
                DataRow dr = DbTable.ExecuteSelectRow(query);
                if (dr == null)
                {
                    context.Response.Write("Session Expired");
                    //context.Response.End();
                    return;
                }
                query = "update tbl_clientuser set clientuser_lastexeheartbeat=getdate() where clientuser_exesessionid='" + sessionId + "'";
                DbTable.ExecuteQuery(query);
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("Error:"+ex.Message); 
        }
    }
    private bool IsLocal
    {
        get
        {
            if (HttpContext.Current.Request.Url.ToString().Contains("localhost") || HttpContext.Current.Request.Url.ToString().Contains(":8085")) return true;
            return false;
        }
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}