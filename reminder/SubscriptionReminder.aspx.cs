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

public partial class subscriptionreminder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblPageTitle.Text = "Subscription Reminders";
            ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
            grid.EnableAddLink = false;
        }
        int ReminderBefore = 7;
        string query = @"SELECT ISNULL(setting_settingvalue,7) AS ReminderBefore
			            FROM tbl_setting WHERE setting_settingname = 'SubscriptionReminder'";
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

        grid.GridQuery = @"SELECT DISTINCT (DATEDIFF(day,GETDATE(),subscription_enddate)+1) AS subscription_DaysRemaining,
			DATEADD(day,1,subscription_enddate)AS subscription_NextRenewalDate,
			*
			FROM tbl_subscription a 			
			INNER JOIN tbl_client c ON client_clientid = subscription_ClientId
			WHERE ISNULL(a.subscription_isdecline,0)=0 AND ISNULL(subscription_isrenew,0)=0	And subscription_subscriptionstatusid=2	
			AND DATEDIFF(day,GETDATE(),subscription_enddate)<=" + ReminderBefore + Custom.RoleWhere("subscription", true);

        lbltest.Text = grid.GridQuery;
        grid.BindReport();
    }
}
