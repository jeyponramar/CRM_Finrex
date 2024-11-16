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

public partial class usercontrols_PrintHeader : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblCompanyName.Text = CustomSettings.CompanyName;
        lblAddress.Text = Common.GetSetting("CompanyAddress");
        lblemail.Text = Common.GetSetting("Company EmailId");
        lblphone.Text = Common.GetSetting("Company Telephone No");
        lblmobile.Text = Common.GetSetting("Company MobileNo");
        lblwebsite.Text = Common.GetSetting("Company Website");
        lblstate.Text = Common.GetSetting("state");
        lblgstno.Text = Common.GetSetting("GST IN");
        
    }
}
