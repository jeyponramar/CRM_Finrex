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

public partial class usercontrols_Dashboard_TrialReminder : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Bindamcreminder();
    }
    private void Bindamcreminder()
    {
        string query = @"SELECT ISNULL(setting_settingvalue,7) AS ReminderBefore
			            FROM tbl_setting WHERE setting_settingname = 'TrailReminder'";
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
        query = @"SELECT DISTINCT Top 10(DATEDIFF(day,GETDATE(),trial_enddate)+1) AS trial_DaysRemaining,
        			DATEADD(day,1,trial_enddate)AS trial_NextRenewalDate,
        			*
        			FROM tbl_trial a 			
        			INNER JOIN tbl_client c ON client_clientid = trial_ClientId
        			WHERE trial_subscriptionstatusid=1	
        			AND DATEDIFF(day,GETDATE(),trial_enddate)<=" + ReminderBefore + Custom.RoleWhere("trial", true); ;
        DataTable dttblAmc = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%'");
        html.Append("<tr><td colspan='2' style='text-align:right;'><a href='#reminder/TrailReminders.aspx' class='spage' title='Trial Reminder'>View more..</a></td></tr>");
        for (int i = 0; i < dttblAmc.Rows.Count; i++)
        {
            DataRow dr = dttblAmc.Rows[i];
            string clientName = GlobalUtilities.ConvertToString(dr["client_customername"]);
            string amcCode = GlobalUtilities.ConvertToString(dr["trial_trialcode"]);
            string clientid = GlobalUtilities.ConvertToString(dr["client_clientid"]);
            string rowcss = "dashboard-alt";
            if (i % 2 == 0) rowcss = "dashboard-row";
            string amcDateRange = GlobalUtilities.ConvertToDate(dr["trial_startdate"]) + " to " + GlobalUtilities.ConvertToDate(dr["trial_enddate"]);
            string expiryTime = "";
            int daysRemaining = GlobalUtilities.ConvertToInt(dr["trial_DaysRemaining"]);
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
        ltTrialReminders.Text = html.ToString();
    }
}
