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

public partial class CompetitorsRenewalReminder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblPageTitle.Text = "Competitors Renewal Reminder";
            ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
            grid.EnableAddLink = false;
        }
        int ReminderBefore = 7;
        string query = @"SELECT ISNULL(setting_settingvalue,7) AS ReminderBefore
			            FROM tbl_setting WHERE setting_settingname = 'CompetitorsRenewalReminder'";
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

        grid.GridQuery = query = @"SELECT DISTINCT (DATEDIFF(day,GETDATE(),clientcompetitor_expirydate)+1) AS clientcompetitor_DaysRemaining,
        			*
        			FROM tbl_clientcompetitor a 			
        			INNER JOIN tbl_client c ON client_clientid = clientcompetitor_clientid
        			WHERE  DATEDIFF(day,GETDATE(),clientcompetitor_expirydate)<=" + ReminderBefore;
        grid.BindReport();
    }
}
