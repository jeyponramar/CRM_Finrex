using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class usercontrols_Dashboard_EnquirySummarySixMonths : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            iFrame.Attributes.Add("src", Common.BackSlashURL("../graph/MyEnquirySummaryLast6Months.aspx"));
        }
    }
}
