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
using WebComponent;
using System.Text;

public partial class crosscurrencyrate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //ltRate.Text = GetRateHtml(2, "", "", "", 6, "","");
            FinstationPortal obj = new FinstationPortal();
            //obj.BindRate(2, ltRate, "", "", "", 0, "", "");
            ltRate.Text = obj.GetRateHtml(2, "", "", "", 7, "", "", true);
        }
    }
    
}