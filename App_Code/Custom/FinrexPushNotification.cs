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
using WebComponent;
using System.Text;
using System.IO;
using System.Collections;


/// <summary>
/// Summary description for FinrexPushNotification
/// </summary>
public class FinrexPushNotification
{
    public Enum_PushnotificationApplicationType ApplicationType = Enum_PushnotificationApplicationType.Finstation;
    public DataRow _drclientuser = null;
    public void ProcessRequest()
    {
        string m = Common.GetQueryString("m");
        string action = Common.GetQueryString("a");
        string app = Common.GetQueryString("app");
        
        if (app == "finwatch")
        {
            ApplicationType = Enum_PushnotificationApplicationType.Finwatch;
        }
        else if (app == "finicon")
        {
            ApplicationType = Enum_PushnotificationApplicationType.FinIcon;
        }
        SetClientUser();
        if (ClientUserId == 0) return;
        if (action == "finstation-homemsgcount")
        {
            GetNotificationCount(Enum_PushnotificationApplicationType.Finstation);
        }
        else if (action == "finstation-topmessages")
        {
            string html = GetTopNotifications(Enum_PushnotificationApplicationType.Finstation);
            HttpContext.Current.Response.Write(html.Replace("\"", "'"));
        }
        else if (action == "finwatch-homemsgcount")
        {
            GetNotificationCount(Enum_PushnotificationApplicationType.Finwatch);
        }
        else if (action == "finwatch-topmessages")
        {
            string html = GetTopNotifications(Enum_PushnotificationApplicationType.Finwatch);
            HttpContext.Current.Response.Write(html.Replace("\"", "'"));
        }
        else if (action == "finicon-homemsgcount")
        {
            GetNotificationCount(Enum_PushnotificationApplicationType.FinIcon);
        }
        else if (action == "finicon-notificationlist")
        {
            string html = "";
            if (Common.GetQueryStringBool("isall"))
            {
                html = GetAllNotifications(0, Enum_PushnotificationApplicationType.FinIcon);
            }
            else
            {
                html = GetTopNotifications(Enum_PushnotificationApplicationType.FinIcon, 100);
            }
            //html = html.Replace("<div class='notification-row-viewall'>View All</div>", "");
            HttpContext.Current.Response.Write(html.Replace("\"", "'"));
        }
    }
    private int ClientId
    {
        get
        {
            if (ApplicationType == Enum_PushnotificationApplicationType.Finwatch || ApplicationType == Enum_PushnotificationApplicationType.FinIcon)
            {
                if (_drclientuser == null) return 0;
                int clientId = GlobalUtilities.ConvertToInt(_drclientuser["clientuser_clientid"]);
                return clientId;
            }
            return Common.ClientId;
        }
    }
    private int ClientUserId
    {
        get
        {
            if (ApplicationType == Enum_PushnotificationApplicationType.Finwatch || ApplicationType == Enum_PushnotificationApplicationType.FinIcon)
            {
                if (_drclientuser == null) return 0;
                int clientUserId = GlobalUtilities.ConvertToInt(_drclientuser["clientuser_clientuserid"]);
                return clientUserId;
            }
            return Common.ClientUserId;
        }
    }
    public void SetClientUser()
    {
        if (ApplicationType == Enum_PushnotificationApplicationType.Finstation) return;
        string sessionId = Common.GetQueryString("sid");
        if(sessionId == "") sessionId = Common.GetQueryString("sessionid");
        if (sessionId == "") return;
        string query = "";
        if (ApplicationType == Enum_PushnotificationApplicationType.Finwatch)
        {
            query = "select * from tbl_clientuser WHERE clientuser_isactive=1 and clientuser_exesessionid=@sessionId";
        }
        else if (ApplicationType == Enum_PushnotificationApplicationType.FinIcon)
        {
            query = "select * from tbl_clientuser WHERE clientuser_isactive=1 and clientuser_mobilesessionid=@sessionId";
        }
        Hashtable hstblp = new Hashtable();
        hstblp.Add("sessionid", sessionId);
        _drclientuser = DbTable.ExecuteSelectRow(query, hstblp);
    }
    public void GetNotificationCount(Enum_PushnotificationApplicationType applicationType)
    {
        if (ClientUserId == 0) return;
        string query = "";
        int lastPushNotificationId = GlobalUtilities.ConvertToInt(DbTable.GetOneColumnData("tbl_clientuser", "clientuser_lastpushnotificationid", ClientUserId));
        query = "select count(*) as c from tbl_pushnotification where pushnotification_pushnotificationid>" + lastPushNotificationId;
        query += GetNotificationWhere(applicationType);
        query += " order by 1 desc";
        //HttpContext.Current.Response.Write(query);
        //HttpContext.Current.Response.End();
        //return;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        int count = GlobalUtilities.ConvertToInt(dr["c"]);
        HttpContext.Current.Response.Write(count.ToString());
    }
    private string GetNotificationWhere(Enum_PushnotificationApplicationType applicationType)
    {
        string query = "";
        string where = "";
        int clientId=ClientId;
        query = "select * from tbl_client where client_clientid=" + clientId;
        if (clientId == 0) return "1=2";
        DataRow drclient = DbTable.ExecuteSelectRow(query);

        int statusId = GlobalUtilities.ConvertToInt(drclient["client_subscriptionstatusid"]);
        int groupId = 0;
        int industryTypeId = GlobalUtilities.ConvertToInt(drclient["client_industrytypesid"]);
        int servicePlanId = GlobalUtilities.ConvertToInt(drclient["client_serviceplanid"]);
        string businessIntoIds = GlobalUtilities.ConvertToString(drclient["client_businessintoids"]);

        where += "and (pushnotification_clientids='' or ','+pushnotification_clientids+',' like '%," + clientId + ",%')";
        where += "and (pushnotification_pushnotificationindustrytypeids='' or ','+pushnotification_pushnotificationindustrytypeids+',' like '%," + industryTypeId + ",%')";

        if (statusId == 1 || statusId == 6) //trial, open
        {
            groupId = 2;//Prospect
        }
        else if (statusId == 2) //Subscribed
        {
            groupId = 1;//Client
        }
        else if (statusId == 3) //Trial Expired
        {
            groupId = 3;//Expired Prospect
        }
        else if (statusId == 4) //Subscription Expired
        {
            groupId = 4;//Expired Client
        }
        else if (statusId == 5) //Declined
        {
            groupId = 6;//Competitor
        }

        where += "and (pushnotification_pushnotificationclientgroupsids='' OR ','+pushnotification_pushnotificationclientgroupsids+',' like '%," + groupId + ",%')";
        
        where += " and (pushnotification_pushnotificationindustrytypeids='' OR ','+pushnotification_pushnotificationindustrytypeids+',' like '%," + industryTypeId + ",%')";

        where += " and (pushnotification_pushnotificationserviceplanids='' OR ','+pushnotification_pushnotificationserviceplanids+',' like '%," + servicePlanId + ",%')";

        where += " and (pushnotification_pushnotificationapplicationids='' OR ','+pushnotification_pushnotificationapplicationids+',' like '%," + (int)applicationType + ",%')";

        if (businessIntoIds == "")
        {
            where += " and pushnotification_pushnotificationbusinessintoids=''";
        }
        else
        {
            where += " and pushnotification_pushnotificationbusinessintoids='' OR (";
            Array arrbusiness = businessIntoIds.Split(',');
            for (int i = 0; i < arrbusiness.Length; i++)
            {
                int businessId = GlobalUtilities.ConvertToInt(arrbusiness.GetValue(i));
                if (i > 0) where += " OR ";
                where += "','+pushnotification_pushnotificationbusinessintoids+',' like '%," + businessId + ",%'";
            }
            where += ")";
        }


        return where;
    }
    public string GetTopNotifications(Enum_PushnotificationApplicationType applicationType)
    {
        return GetTopNotifications(applicationType, 10);
    }
    public string GetTopNotifications(Enum_PushnotificationApplicationType applicationType, int top)
    {
        string query = "";
        int lastPushNotificationId = GlobalUtilities.ConvertToInt(DbTable.GetOneColumnData("tbl_clientuser", "clientuser_lastpushnotificationid", ClientUserId));
        query = "select top " + top + " * from tbl_pushnotification where pushnotification_pushnotificationid>" + lastPushNotificationId;
        query += GetNotificationWhere(applicationType);
        query += " order by 1 desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        if (GlobalUtilities.IsValidaTable(dttbl))
        {
            lastPushNotificationId = GlobalUtilities.ConvertToInt(dttbl.Rows[0]["pushnotification_pushnotificationid"]);
            query = "update tbl_clientuser set clientuser_lastpushnotificationid=" + lastPushNotificationId + " where clientuser_clientuserid=" + ClientUserId;
            DbTable.ExecuteQuery(query);
        }
        string html = GetNotificationHtml(dttbl, 0, true, false);
        return html;
    }
    public string GetAllNotifications(int notificationId, Enum_PushnotificationApplicationType applicationType)
    {
        string query = "";
        query = "select top 100 * from tbl_pushnotification where 1=1";
        if (notificationId > 0) query += " and pushnotification_pushnotificationid=" + notificationId;
        query += GetNotificationWhere(applicationType);
        query += " order by 1 desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        string html = GetNotificationHtml(dttbl, notificationId, false, true);
        return html;
    }
    public string GetNotificationHtml(DataTable dttbl, int notificationId, bool istopnotification, bool isAll)
    {
        StringBuilder html = new StringBuilder();
        if (GlobalUtilities.IsValidaTable(dttbl))
        {
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                string title = GlobalUtilities.ConvertToString(dttbl.Rows[i]["pushnotification_title"]);
                string notification = GetNotificationMessage(dttbl.Rows[i], notificationId);
                string time = GetTime(Convert.ToDateTime(dttbl.Rows[i]["pushnotification_date"]));
                int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["pushnotification_pushnotificationid"]);
                html.Append("<div class='notification-row jq-notification-row' nid='" + id + "'><div class='notification-box'>");
                if (title != "") html.Append("<div class='notification-title'>" + title + "</div>");
                html.Append("<div>" + notification + "</div><div class='notification-time'>" + time + "</div>");
                html.Append("</div></div>");
            }
        }
        else
        {
            html.Append("<div class='notification-row'>No new notification found!</div>");
        }
        if (notificationId == 0 && !isAll)
        {
            html.Append("<div class='notification-row-viewall'>View All</div>");
        }
        return html.ToString();
    }
    public string GetNotificationMessage(DataRow dr, int notificationId)
    {
        StringBuilder html = new StringBuilder();
        string msg = "";
        if (notificationId > 0)
        {
            msg = GlobalUtilities.ConvertToString(dr["pushnotification_message"]);
        }
        else
        {
            msg = GlobalUtilities.ConvertToString(dr["pushnotification_shortmessage"]);
        }
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
                    html.Append("<div><a class='lnk1' target='_blank' href='https://finstation.in/upload/pushnotification/" + fileName + "'>" + arr1.GetValue(1).ToString() + "</a></div>");
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
