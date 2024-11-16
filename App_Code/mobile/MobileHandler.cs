using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WebComponent;
using System.Text;
using System.Collections;
using System.IO;

/// <summary>
/// Summary description for MobileHandler
/// </summary>
public class MobileHandler
{
    private int _clientUserId = 0;
    public void ProcessRequest()
	{
        string m = Common.GetQueryString("m");
        string action = Common.GetQueryString("action");
        if (m == "login")
        {
            Login();
        }
        else if (m == "checkloginstatus")
        {
            CheckLogin();
        }
        else if (m == "pageload")
        {
            Page_Load();
        }
	}
    private void Login()
    {
        string username = global.CheckInputData(HttpContext.Current.Request["username"]);
        string password = global.CheckInputData(HttpContext.Current.Request["password"]);

        string query = "select * from tbl_clientuser where clientuser_isfinmessenger=1 and " +
                "clientuser_username='" + username + "' and clientuser_password='" + password + "'";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        string error = "";
        string data = "";
        string redirect = "";
        string msg = "";
        if (dr == null)
        {
            error = "Invalid User Name / Password";
        }
        else
        {
            MobileApp_PhoneGap obj = new MobileApp_PhoneGap();
            if (obj.IsClientActive(GlobalUtilities.ConvertToInt(dr["clientuser_clientid"])))
            {
                int clientUserId = GlobalUtilities.ConvertToInt(dr["clientuser_clientuserid"]);
                SubscribePushNotification(clientUserId);
                string sessionId = Common.GetQueryString("sessionid");
                string newsession = sessionId;
                if (newsession == "") newsession = Guid.NewGuid().ToString();
                query = "update tbl_clientuser set clientuser_finmessengersessionid='" + newsession + "' where clientuser_clientuserid=" + clientUserId;
                DbTable.ExecuteQuery(query);
                //string notification = "No notification found!";
                //query = "select top 1 * from tbl_pushnotification where cast(pushnotification_date as date)=cast(getdate() as date) order by 1 desc";
                //DataRow drmsg = DbTable.ExecuteSelectRow(query);
                //if (drmsg != null)
                //{
                //    notification = GlobalUtilities.ConvertToString(drmsg["pushnotification_message"]);
                //    string time=GlobalUtilities.ConvertToTime(drmsg["pushnotification_date"]);
                //    notification = "<div><div>" + notification + "</div><div class='notification-time'>" + time + "</div>";
                //}
                //notification = notification.Replace("\"", "'");
                data = "\"sessionid\":\"" + newsession + "\"";
                redirect = "latestnotification.html,FinMessgener";
            }
            else
            {
                error = "Your subscription has expired!";
            }
        }
        string response = "{\"error\":\"" + error + "\",\"data\":{" + data + "},\"redirect\":\"" + redirect + "\",\"msg\":\"" + msg + "\"}";
        HttpContext.Current.Response.Write(response);
    }
    private void CheckLogin()
    {
        string sessionId = Common.GetQueryString("sessionid");
        string query = "select * from tbl_clientuser where clientuser_isfinmessenger=1 and " +
                "clientuser_finmessengersessionid='" + sessionId + "'";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            SubscribePushNotification(GlobalUtilities.ConvertToInt(dr["clientuser_clientuserid"]));
            HttpContext.Current.Response.Write("ok");
        }
    }
    private bool IsUserLoggedIn()
    {
        string sessionId = Common.GetQueryString("sessionid").Trim();
        if (sessionId == "") return false;
        string query = "select * from tbl_clientuser where clientuser_isfinmessenger=1 and " +
                "clientuser_finmessengersessionid='" + sessionId + "'";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            return false;
        }
        else
        {
            _clientUserId = GlobalUtilities.ConvertToInt(dr["clientuser_clientuserid"]);
            return true;
        }
    }
    private void Page_Load()
    {
        string page = Common.GetQueryString("page");
        if (!IsUserLoggedIn())
        {
            HttpContext.Current.Response.Write("session expired");
            return;
        }
        if (page == "latestnotification")
        {
            GetAllNotifications(true);
            Common.SaveClientUserHistory(_clientUserId, 4);
        }
        else if (page == "viewmessages")
        {
            GetAllNotifications(false);
        }
    }
    private void GetLatestNotification()
    {
        string query = "";
        query = "select * from tbl_pushnotification where cast(pushnotification_date as date)=cast(getdate() as date) order by 1 desc";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        StringBuilder html = new StringBuilder();
        if (dr == null)
        {
            html.Append("<div>No notification found!</div>");
        }
        else
        {
            int notificationId = GlobalUtilities.ConvertToInt(dr["pushnotification_pushnotificationid"]);
            string title = GlobalUtilities.ConvertToString(dr["pushnotification_title"]);
            string notification = GetNotificationMessage(dr);
            string time = GetTime(Convert.ToDateTime(dr["pushnotification_date"]));
            html.Append("<div class='notification-row'><div class='notification-box'>");
            if (time != "")
            {
                html.Append("<div class='notification-title'>" + title + "</div>");
            }
            html.Append("<div><div>" + notification + "</div><div class='notification-time'>" + time + "</div>");
            html.Append("</div></div>");
            query = "update tbl_clientuser set clientuser_lastpushnotificationid=" + notificationId + " where clientuser_clientuserid=" + _clientUserId;
            DbTable.ExecuteQuery(query);
        }
        HttpContext.Current.Response.Write(html.Replace("\"", "'"));
    }
    private void GetAllNotifications(bool isToday)
    {
        string query = "";
        query = "select top 50 * from tbl_pushnotification order by 1 desc";
        if (isToday)
        {
            SubscribePushNotification(_clientUserId);
            query = "select top 10 * from tbl_pushnotification where cast(pushnotification_date as date)=cast(getdate() as date) order by 1 desc";
        }
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        if (GlobalUtilities.IsValidaTable(dttbl))
        {
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                string title = GlobalUtilities.ConvertToString(dttbl.Rows[i]["pushnotification_title"]);
                string notification = GetNotificationMessage(dttbl.Rows[i]);
                string time = GetTime(Convert.ToDateTime(dttbl.Rows[i]["pushnotification_date"]));
                html.Append("<div class='notification-row'><div class='notification-box'>");
                if (title != "") html.Append("<div class='notification-title'>" + title + "</div>");
                html.Append("<div>" + notification + "</div><div class='notification-time'>" + time + "</div>");
                html.Append("</div></div>");
            }
        }
        else
        {
            html.Append("<div>No notification found!</div>");
        }
        HttpContext.Current.Response.Write(html.Replace("\"", "'"));
    }
    private void SubscribePushNotification(int clientUserId)
    {
        string device = Common.GetQueryString("device").ToLower();
        string onesignalid = Common.GetQueryString("onesignalid");
        if (onesignalid != "" && device != "")
        {
            string colName = "androidonesignalid";
            if (device == "ios") colName = "iosonesignalid";
            Hashtable hstbl = new Hashtable();
            hstbl.Add(colName, onesignalid);
            InsertUpdate obj = new InsertUpdate();
            obj.UpdateData(hstbl, "tbl_clientuser", clientUserId);
        }
    }
    private string GetNotificationMessage(DataRow dr)
    {
        StringBuilder html = new StringBuilder();
        string msg = GlobalUtilities.ConvertToString(dr["pushnotification_message"]);
        msg = msg.Replace("../upload/", "https://finstation.in/upload/");
        int imgStartIndex = 0;
        int startIndex = -1;
        int endIndex = 0;
        int counter = 0;
        while (true)
        {
            if (counter > 20) break;
            imgStartIndex = startIndex = msg.IndexOf("<img ", startIndex + 1);
            if (imgStartIndex < 0) break;
            startIndex = msg.IndexOf("src=\"", startIndex + 1);
            endIndex = msg.IndexOf(">", startIndex + 1);
            if (startIndex > 0 && endIndex > startIndex)
            {
                string img = msg.Substring(startIndex, endIndex - startIndex);
                int index1 = img.IndexOf("\"");
                int index2 = img.IndexOf("\"", index1 + 1);
                if (index1 > 0 && index2 > index1)
                {
                    string url = img.Substring(index1 + 1, index2 - index1 - 1);
                    string href = "<a href='" + url + "'>";
                    msg = msg.Insert(endIndex + 1, "</a>");
                    msg = msg.Insert(imgStartIndex, href);
                    startIndex = endIndex + href.Length + 4;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
            counter++;
        }

        string attachment = GlobalUtilities.ConvertToString(dr["pushnotification_attachment"]);
        html.Append("<div>" + msg + "</div>");
        if (attachment != "")
        {
            html.Append("<div class='pushnoti-attach'>");
            Array arr = attachment.Split('|');
            for (int i = 0; i < arr.Length; i++)
            {
                string fileName = arr.GetValue(i).ToString();
                Array arr1 = fileName.Split('_');
                if (File.Exists(HttpContext.Current.Server.MapPath("~/upload/pushnotification/" + fileName)))
                {
                    html.Append("<div><a class='lnk1' href='https://finstation.in/upload/pushnotification/" + fileName + "'>" + arr1.GetValue(1).ToString() + "</a></div>");
                    //html.Append("<div><a class='lnk1' href='https://finstation.in/download.aspx?f=upload/pushnotification/" + fileName + "'>" + arr1.GetValue(1).ToString() + "</a></div>");
                    //html.Append("<div><a class='lnk' href='../upload/pushnotification/" + fileName + "'>" + arr1.GetValue(1).ToString() + "</a></div>");
                }
            }
            html.Append("</div>");
        }
        return html.ToString();
    }
    private string GetTime(DateTime date)
    {
        return GlobalUtilities.ConvertToDateTime(date);
        TimeSpan ts = DateTime.Now - date;
        if (ts.Days > 0)
        {
            return GlobalUtilities.ConvertToDateTime(date);
        }
        else if (ts.Hours < 24 && ts.Hours > 0)
        {
            return ts.Hours.ToString() + " hrs ago";
        }
        else if (ts.Minutes <= 1)
        {
            return "Just Now";
        }
        else if (ts.Minutes < 60)
        {
            return ts.Minutes.ToString() + " mins ago";
        }
        else
        {
            return GlobalUtilities.ConvertToDateTime(date);   
        }
    }
}
