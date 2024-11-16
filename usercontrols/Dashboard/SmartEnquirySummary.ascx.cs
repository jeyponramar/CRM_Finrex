using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class usercontrols_Dashboard_SmartEnquirySummary : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            imgAssigned.Attributes.Add("src", "~/images/pending-service.png");
            imgOpportunity.Attributes.Add("src", "~/images/pendingsales.png");
            imgQuoteCreated.Attributes.Add("src", "~/images/quote.png");
            imgWon.Attributes.Add("src", "~/images/thumbwon.png");
            imgHold.Attributes.Add("src", "~/images/hold.png");
            imgOpen.Attributes.Add("src", "~/images/open-services.png");
        }
    }
}
