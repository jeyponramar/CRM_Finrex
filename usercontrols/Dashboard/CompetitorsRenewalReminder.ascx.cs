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

public partial class usercontrols_Dashboard_CompetitorsRenewalReminder : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Bindamcreminder();
    }
    private void Bindamcreminder()
    {
        string query = @"SELECT ISNULL(setting_settingvalue,7) AS ReminderBefore
			            FROM tbl_setting WHERE setting_settingname = 'CompetitorsRenewalReminder'";
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
        query = @"SELECT DISTINCT Top 10(DATEDIFF(day,GETDATE(),clientcompetitor_expirydate)+1) AS clientcompetitor_DaysRemaining,
        			*
        			FROM tbl_clientcompetitor a 			
        			INNER JOIN tbl_client c ON client_clientid = clientcompetitor_clientid
        			WHERE  DATEDIFF(day,GETDATE(),clientcompetitor_expirydate)<=" + ReminderBefore + Custom.RoleWhere("client", true); ;
        DataTable dttblAmc = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%'");
        html.Append("<tr><td colspan='2' style='text-align:right;'><a href='#reminder/CompetitorsRenewalReminder.aspx' class='spage' title='Competitors Renewal Reminder'>View more..</a></td></tr>");
        for (int i = 0; i < dttblAmc.Rows.Count; i++)
        {
            DataRow dr = dttblAmc.Rows[i];
            string clientName = GlobalUtilities.ConvertToString(dr["client_customername"]);
            string competitorsDate = GlobalUtilities.ConvertToDate(dr["clientcompetitor_expirydate"]);
            string clientid = GlobalUtilities.ConvertToString(dr["client_clientid"]);
            string rowcss = "dashboard-alt";
            if (i % 2 == 0) rowcss = "dashboard-row";
            string expiryTime = "";
            int daysRemaining = GlobalUtilities.ConvertToInt(dr["clientcompetitor_DaysRemaining"]);
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
            html.Append("<td><table width='100%'><tr><td class='bold'>" + clientName + "</td></tr>");
            html.Append("<tr><td>" + competitorsDate + "</td><td align='right'>" + expiryTime + "</td></tr>");
            html.Append("</table></td>");
            html.Append("</td></tr>");
        }
        html.Append("</table>");
        ltCompetitorsRenewalReminders.Text = html.ToString();
    }
}
