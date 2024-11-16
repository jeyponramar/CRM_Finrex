using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WebComponent;

public partial class trailreminders : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblPageTitle.Text = "Trail Reminders";
            ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
            grid.EnableAddLink = false;
        }
        int ReminderBefore = 7;
        string query = @"SELECT ISNULL(setting_settingvalue,7) AS ReminderBefore
			            FROM tbl_setting WHERE setting_settingname = 'TrailReminder'";
        InsertUpdate obj = new InsertUpdate();
        DataRow dr = obj.ExecuteSelectRow(query);
        if (dr != null)
        {
            int AutoReminderBefore = GlobalUtilities.ConvertToInt(dr["ReminderBefore"]);
            if (AutoReminderBefore > 0)
            {
                ReminderBefore = AutoReminderBefore;
            }
        }

        grid.GridQuery = @"SELECT DISTINCT (DATEDIFF(day,GETDATE(),trial_enddate)+1) AS trial_DaysRemaining,
			DATEADD(day,1,trial_enddate)AS trial_NextRenewalDate,
			*
			FROM tbl_trial a 			
			INNER JOIN tbl_client c ON client_clientid = trial_ClientId
			WHERE trial_subscriptionstatusid=1	
			AND DATEDIFF(day,GETDATE(),trial_enddate)<=" + ReminderBefore + Custom.RoleWhere("trial", true);
        //lbltest.Text = grid.GridQuery;
        grid.BindReport();
    }
}
