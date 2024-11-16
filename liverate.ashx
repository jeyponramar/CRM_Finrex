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
                Commodity com = new Commodity();
                com.UpdateRate();
                //string rtds = global.CheckInputData(context.Request["rtd"]).Replace("___","#");
                //string values = global.CheckInputData(context.Request["v"]);

                ////GlobalUtilities.WriteFile(context.Server.MapPath("~/doc/log/liverate.txt"), rtds + Environment.NewLine + values);
                
                ////rtd = "17#1#1#6069#USDINRCOMP#Bid";
                //Array arrRtd = rtds.Split(',');
                //Array arrValues = values.Split(',');
                
                //for (int i = 0; i < arrRtd.Length; i++)
                //{
                //    string rtd = arrRtd.GetValue(i).ToString();
                //    string value = arrValues.GetValue(i).ToString();
                //    if (value != "")
                //    {
                //        query = "UPDATE tbl_liverate SET liverate_prevrate=liverate_currentrate,liverate_currentrate='" + value + "',liverate_modifieddate=GETDATE() WHERE liverate_rtdcode='" + rtd + "'";

                //        DbTable.ExecuteQuery(query);
                //    }
                //}
                //context.Response.Write("Success");
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
                query = @"select liverate_liverateid,liverate_liveratesectionid,liverate_decimalplaces,
                        liverate_liverateid as rid,liverate_currentrate as cr 
                        from tbl_liverate where liverate_istick=1";
                if (HttpContext.Current.Request.QueryString["all"] == "1")
                {
                }
                else
                {
                    query += " and DATEDIFF(s,liverate_modifieddate,getdate()) < 50";
                }
                DataTable dttbl = DbTable.ExecuteSelect(query);
                dttbl = Finstation.CorrectLiveRateValues(dttbl);
                context.Response.Write(JSON.Convert(dttbl));
            }
            else if (action == "exe")
            {
                string lids = Common.GetQueryString("lids");
                if (lids == "" || !GlobalUtilities.IsValidCommaSepValues(lids)) return;
//                query = @"select liverate_liverateid as rid,liverate_currentrate as cr from tbl_liverate 
//                          where liverate_liverateid IN(1,4,5,10,13,14,19,22,23,37,40,41,46,76,77)";
                query = @"select liverate_liverateid as rid,liverate_currentrate as cr from tbl_liverate 
                          where liverate_liverateid IN("+lids+")";
                
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
                context.Response.Write(JSON.Convert(dttbl));
            }
            else if (action == "mobile-liverate")
            {
                context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
                string type = Common.GetQueryString("type");
                string liveRateIds = "";
                string sessionId = Common.GetQueryString("sessionid");
                if (Common.GetQueryStringValue("counter") == 0 && sessionId != "")
                {
                    query = "select * from tbl_clientuser where clientuser_mobilesessionid='" + sessionId + "'";
                    DataRow drclientuser = DbTable.ExecuteSelectRow(query);
                    if (drclientuser == null)
                    {
                        context.Response.Write("session expired");
                        return;
                    }
                }
                if (type == "lme3mforward")
                {
                    query = @"select MetalLiveRate_metalid as mid, MetalLiveRate_bid as bid,MetalLiveRate_ask as ask,MetalLiveRate_open as [open],
                    MetalLiveRate_high as high,MetalLiveRate_low as low,MetalLiveRate_change as change,metalliverate_changeper as changeper
                    from tbl_MetalLiveRate 
                    ";//5seconds
                    //where DATEDIFF(ss, MetalLiveRate_modifieddate,GETDATE())<=5
                    DataTable dttblmetal = DbTable.ExecuteSelect(query);
                    string jsonmetal = JSON.Convert(dttblmetal);
                    HttpContext.Current.Response.Write(jsonmetal);
                    return;
                }
                else
                {
                    if (type == "spotrate")
                    {
                        liveRateIds = "1048,1,2,4,5,10,11,13,15,19,20,22,23,28,29,31,32,37,38,40,41,46,47,49,50,55,56,58,59,64,65,67,68,73,74,76,77";
                    }
                    if (Common.GetQueryString("lrids") != "")
                    {
                        liveRateIds = Common.GetQueryString("lrids");
                    }
                    query = @"select liverate_liverateid as rid,liverate_currentrate as cr from tbl_liverate 
                          where liverate_liverateid IN(" + liveRateIds + ")";
                    if (!IsLocal)
                    {
                        //query += " AND DATEDIFF(s,liverate_modifieddate,getdate()) < 50";
                    }
                }
                //context.Response.Write(query);
                DataTable dttbl = DbTable.ExecuteSelect(query);
                Random rnd = new Random();
                if (IsLocal)
                {
                    dttbl.Rows[0]["cr"] = rnd.Next(1, 100);
                }
                string json = JSON.Convert(dttbl);
                HttpContext.Current.Response.Write(json);
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
            else if (action == "ratehtml")
            {
                string html = Finstation.GetLiverateHtml();
                HttpContext.Current.Response.Write(html);
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("Error:"+ex.Message); 
        }
    }
    private DataTable CorrectLiveRateValues(DataTable dttbl)
    {
        return dttbl;
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string currentRate = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_currentrate"]);
            if (currentRate.Contains("/"))
            {
                Array arr = currentRate.Split('/');
                currentRate = arr.GetValue(1).ToString() + "-" + arr.GetValue(0).ToString() + "-" + arr.GetValue(2).ToString();
                dttbl.Rows[i]["liverate_currentrate"] = currentRate;
            }
        }
        return dttbl;
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