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

public partial class utilities_viewreport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int reportid = GlobalUtilities.ConvertToInt(Request.QueryString["rid"]);
        if (reportid == 1)
        {
            Session["ew"] = "subscription_subscriptionstatusid IN(1,2) AND subscription_lastcalllogsent is null OR cast(subscription_lastcalllogsent as date)<>cast(getdate() as date)";
            Response.Redirect("~/utilities/view.aspx?m=subscription&title=Call Log Pending Today");
        }
    }
}
