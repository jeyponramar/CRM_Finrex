using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Collections;

public partial class admin_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CustomSession.Session("Login_IsRefuxLoggedIn") == null || GlobalUtilities.ConvertToBool(Session["IsDemoSample"]))
        {
            Response.Redirect("~/adminlogin.aspx");
        }
        if (Common.RoleId == 10 || Common.RoleId == 11)
        {
            Response.Redirect("~/adminlogin.aspx");
        }
        if (!IsPostBack)
        {
            EnableDisableTab();
            //UpdateOpportunityStatus();
            UpdateSubscriptionStatus();
            Custom.CreateAllTableColumns();
        }
    }
    
    private void UpdateOpportunityStatus()
    {
        //trial expired
        string query = "update tbl_opportunity set opportunity_opportunitystatusid=7 "+
                       "where opportunity_opportunitystatusid = 6 AND cast(opportunity_enddate as date)<cast(getdate() as date)";
        DbTable.ExecuteQuery(query);
        //subscription expired
        query = "update tbl_opportunity set opportunity_opportunitystatusid=8 "+
                "where opportunity_opportunitystatusid = 2 AND cast(opportunity_enddate as date)<cast(getdate() as date)";
        DbTable.ExecuteQuery(query);
    }
    private void UpdateSubscriptionStatus()
    {
        string query = "";
        //trial expired
        query = "select * from tbl_trial where trial_subscriptionstatusid = 1 AND cast(trial_enddate as date)<cast(getdate() as date)";
        DataTable dttblTrial = DbTable.ExecuteSelect(query);
        for (int i = 0; i < dttblTrial.Rows.Count; i++)
        {
            int trialId = GlobalUtilities.ConvertToInt(dttblTrial.Rows[i]["trial_trialid"]);
            int clientId = GlobalUtilities.ConvertToInt(dttblTrial.Rows[i]["trial_clientid"]);
            query = "update tbl_trial set trial_subscriptionstatusid=3 where trial_trialid=" + trialId;
            DbTable.ExecuteQuery(query);
            query = "update tbl_client set client_subscriptionstatusid=3 where client_clientid=" + clientId;
            DbTable.ExecuteQuery(query);
        }
        //subscription expired
        query = "select * from tbl_subscription where subscription_subscriptionstatusid = 2 AND cast(subscription_enddate as date)<cast(getdate() as date)";
        DataTable dttblSubscription = DbTable.ExecuteSelect(query);
        for (int i = 0; i < dttblSubscription.Rows.Count; i++)
        {
            int subscriptionId = GlobalUtilities.ConvertToInt(dttblSubscription.Rows[i]["subscription_subscriptionid"]);
            int clientId = GlobalUtilities.ConvertToInt(dttblSubscription.Rows[i]["subscription_clientid"]);
            query = "update tbl_subscription set subscription_subscriptionstatusid=4 where subscription_subscriptionid=" + subscriptionId;
            DbTable.ExecuteQuery(query);
            query = "update tbl_client set client_subscriptionstatusid=4 where client_clientid=" + clientId;
            DbTable.ExecuteQuery(query);
        }
        query = "update tbl_client set client_subscriptionstatusid=2 " +
                "where client_clientid IN(select subscription_clientid from tbl_subscription where cast(subscription_enddate as date)>=cast(getdate() as date))";
        DbTable.ExecuteQuery(query);
    }
    private void EnableDisableTab()
    {
        int userId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId"));
        string query = "select user_ismultitab from tbl_user WHERE user_userid=" + userId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        bool isMultitabEnabled = GlobalUtilities.ConvertToBool(dr["user_ismultitab"]);
        if (isMultitabEnabled)
        {
            ltMultiTabFunction.Text = "<td class='tab-multi enable-tab' align='right'></td>";
        }
        else
        {
            ltMultiTabFunction.Text = "<td class='tab-multi disable-tab' align='right'></td>";
        }
    }
}
