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

public partial class usercontrols_Dashboard : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblcompanyname.Text = CustomSettings.CompanyName;
            gridFeedbackReminder.Report();
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        //gridMyTask.ExtraWhere = Common.GetViewRightsQuery("followups");
    }
}
