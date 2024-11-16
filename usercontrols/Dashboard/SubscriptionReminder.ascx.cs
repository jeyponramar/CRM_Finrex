using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.Collections;
using System.Text;
using System.IO;

public partial class usercontrols_Dashboard_SubscriptionReminder : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Bindamcreminder();
    }
    private void Bindamcreminder()
    {
        string query = @"SELECT ISNULL(setting_settingvalue,7) AS ReminderBefore
			            FROM tbl_setting WHERE setting_settingname = 'SubscriptionReminder'";
        InsertUpdate obj = new InsertUpdate();
        DataRow dr1 = obj.ExecuteSelectRow(query);
        int ReminderBefore = 7;
        if (dr1 != null)
        {
            int AutoReminderBefore = GlobalUtilities.ConvertToInt(dr1["ReminderBefore"]);
            if (AutoReminderBefore > 0)
            {
                ReminderBefore = AutoReminderBefore;
            }
        }
        query = @"SELECT DISTINCT Top 10(DATEDIFF(day,GETDATE(),subscription_enddate)+1) AS subscription_DaysRemaining,
        			DATEADD(day,1,subscription_enddate)AS subscription_NextRenewalDate,
        			*
        			FROM tbl_subscription a 			
        			INNER JOIN tbl_client c ON client_clientid = subscription_ClientId
        			WHERE ISNULL(a.subscription_isdecline,0)=0 AND ISNULL(subscription_isrenew,0)=0	 And subscription_subscriptionstatusid=2	
        			AND DATEDIFF(day,GETDATE(),subscription_enddate)<=" + ReminderBefore + Custom.RoleWhere("subscription", true);
        DataTable dttblAmc = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%'");
        html.Append("<tr><td colspan='2' style='text-align:right;'><a href='#reminder/SubscriptionReminder.aspx' class='spage' title='Subscription Reminder'>View more..</a></td></tr>");
        for (int i = 0; i < dttblAmc.Rows.Count; i++)
        {
            DataRow dr = dttblAmc.Rows[i];
            string clientName = GlobalUtilities.ConvertToString(dr["client_customername"]);
            string amcCode = GlobalUtilities.ConvertToString(dr["subscription_subscriptioncode"]);
            string clientid = GlobalUtilities.ConvertToString(dr["client_clientid"]);
            string rowcss = "dashboard-alt";
            if (i % 2 == 0) rowcss = "dashboard-row";
            string amcDateRange = GlobalUtilities.ConvertToDate(dr["subscription_startdate"]) + " to " + GlobalUtilities.ConvertToDate(dr["subscription_enddate"]);
            string expiryTime = "";
            int daysRemaining = GlobalUtilities.ConvertToInt(dr["subscription_DaysRemaining"]);
            if (daysRemaining > 0)
            {
                expiryTime = "<div class='green'>" + daysRemaining + " Days Remaining</div>";
            }
            else if (daysRemaining == 0)
            {
                expiryTime = daysRemaining + " Today";
            }
            else
            {
                expiryTime = "<div class='red'>" + -1 * daysRemaining + " Days Overdue</div>";
            }
            if (!File.Exists(Server.MapPath("~/upload/client-logo/" + clientid + ".jpg"))) clientid = "default";
            html.Append("<tr class='" + rowcss + "'>");
            //html.Append("<td valign='middle' width='40'><div><img src='../upload/client-logo/" + clientid + ".jpg' width='40'/></div></td>" +
            html.Append("<td><table width='100%'><tr><td class='bold'>" + clientName + "</td><td align='right'>" + amcCode + "</td></tr>");
            html.Append("<tr><td>" + amcDateRange + "</td><td align='right'>" + expiryTime + "</td></tr>");
            html.Append("</table></td>");
            html.Append("</td></tr>");
        }
        html.Append("</table>");
        ltSubscriptionReminders.Text = html.ToString();
    }
}
