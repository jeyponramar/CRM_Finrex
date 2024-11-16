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
        //try
        //{
            string query = "";
            if (action == "u")
            {
                string rtds = global.CheckInputData(context.Request["rtd"]).Replace("___","#");
                string values = global.CheckInputData(context.Request["v"]);

                //GlobalUtilities.WriteFile(context.Server.MapPath("~/doc/log/liverate.txt"), rtds + Environment.NewLine + values);
                
                //rtd = "17#1#1#6069#USDINRCOMP#Bid";
                Array arrRtd = rtds.Split(',');
                Array arrValues = values.Split(',');
                DataTable dttbl = LiverateData;
                for (int i = 0; i < arrRtd.Length; i++)
                {
                    string rtd = arrRtd.GetValue(i).ToString();
                    string value = arrValues.GetValue(i).ToString();
                    if (value != "")
                    {
                        for (int j = 0; j < dttbl.Rows.Count; j++)
                        {
                            if (GlobalUtilities.ConvertToString(dttbl.Rows[j]["liverate_rtdcode"]) == rtd)
                            {
                                dttbl.Rows[j]["liverate_prevrate"] = dttbl.Rows[j]["liverate_currentrate"];
                                dttbl.Rows[j]["liverate_currentrate"] = value;
                                dttbl.Rows[j]["liverate_modifieddate"] = DateTime.Now;
                            }
                        }
                        query = "UPDATE tbl_liverate SET liverate_prevrate=liverate_currentrate,liverate_currentrate='" + value + "',liverate_modifieddate=GETDATE() WHERE liverate_rtdcode='" + rtd + "'";
                        DbTable.ExecuteQuery(query);
                    }
                }
                LiverateData = dttbl;
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
                //DataTable dttbl = DbTable.ExecuteSelect(query);
                //dttbl = CorrectLiveRateValues(dttbl);
                DateTime dt = DateTime.Now.AddMinutes(-1);
                string m = dt.Month.ToString();
                string d = dt.Day.ToString();
                string min = dt.Minute.ToString();
                string h = dt.Hour.ToString();
                if (m.Length == 1) m = "0" + m;
                if (d.Length == 1) d = "0" + d;
                if (min.Length == 1) min = "0" + min;
                if (h.Length == 1) h = "0" + h;
                string date = dt.Year + "-" + m + "-" + d + " " + h + ":" + min + ":00.000";
                string where = "liverate_modifieddate>='" + date + "'";
                DataTable dttbl = GetLiverateData(where);
                
                context.Response.Write(JSON.Convert(dttbl));
            }
            else if (action == "exe")
            {
                query = @"select liverate_liverateid as rid,liverate_currentrate as cr from tbl_liverate 
                          where liverate_liverateid IN(1,4,5,10,13,14,19,22,23,37,40,41,46,76,77)";
                if (!IsLocal)
                {
                    query += " AND DATEDIFF(s,liverate_modifieddate,getdate()) < 50";
                }
                //DataTable dttbl = DbTable.ExecuteSelect(query);
                string where = "liverate_liverateid IN(1,4,5,10,13,14,19,22,23,37,40,41,46,76,77)";
                if (!IsLocal)
                {
                    where += " AND DATEDIFF(s,liverate_modifieddate,getdate()) < 50";
                }
                DataTable dttbl = GetLiverateData(where);
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
        //}
        //catch (Exception ex)
        //{
        //    //context.Response.Write("Error:"+ex.Message); 
        //    throw ex;
        //}
    }
    private DataTable LiverateData
    {
        get
        {
            DataTable dttbl = new DataTable();
            if (HttpContext.Current.Application["LiverateData"] == null)
            {
                SetLiveRateData();
            }
            else
            {
                DateTime dt = DateTime.Now;
                if (HttpContext.Current.Application["LastLiverateDataUpdate"] != null)
                {
                    dt = (DateTime)HttpContext.Current.Application["LastLiverateDataUpdate"];
                }
                TimeSpan ts = DateTime.Now - dt;
                if (ts.Days > 0)
                {
                    SetLiveRateData();
                }
                return (DataTable)HttpContext.Current.Application["LiverateData"];
            }
            return dttbl;
        }
        set
        {
            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application["LiverateData"] = value;
            HttpContext.Current.Application.UnLock();
        }
    }
    private void SetLiveRateData()
    {
        string query = "select * from tbl_liverate";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        HttpContext.Current.Application.Lock();
        HttpContext.Current.Application["LiverateData"] = dttbl;
        HttpContext.Current.Application["LastLiverateDataUpdate"] = DateTime.Now;
        HttpContext.Current.Application.UnLock();
    }
    private DataTable GetLiverateData(string where)
    {
        DataTable dttbl = LiverateData;
        DataView dv = dttbl.DefaultView;
        dv.RowFilter = "liverate_modifieddate>='2020-03-10 07:16:00.000'";// where;
        //HttpContext.Current.Response.Write(where);
        //HttpContext.Current.Response.End();
        dttbl = dv.ToTable();
        DataTable dttblnew = new DataTable();
        dttblnew.Columns.Add("rid");
        dttblnew.Columns.Add("cr");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            DataRow dr = dttblnew.NewRow();
            dr["rid"] = dttbl.Rows[i]["liverate_liverateid"];
            dr["cr"] = dttbl.Rows[i]["liverate_currentrate"];
            dttblnew.Rows.Add(dr);
        }
        return dttblnew;
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