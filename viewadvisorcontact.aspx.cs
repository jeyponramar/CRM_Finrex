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

public partial class viewadvisorcontact : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindAdvisorDetails();
        }
    }
    private void BindAdvisorDetails()
    {
        int subscriptionId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_SubscriptionId"));
        string query = @"select * from tbl_subscription
                        join tbl_employee on employee_employeeid=subscription_employeeid
                        where subscription_subscriptionid=" + subscriptionId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return;
        string emailId = GlobalUtilities.ConvertToString(dr["employee_emailid"]);
        lbladvisorname.Text = GlobalUtilities.ConvertToString(dr["employee_employeename"]);
        ltadvisoremailid.Text = "<a href='mailto:" + emailId + "'>" + emailId + "</a>";
        lbladvisormobileno.Text = GlobalUtilities.ConvertToString(dr["employee_mobileno"]);
    }
}
