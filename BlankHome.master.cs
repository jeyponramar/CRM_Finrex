using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BlankHome : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GlobalUtilities.ConvertToBool(Session["Login_IsLoggedIn"]))
        {
            Response.Redirect("~/customerlogin.aspx");
        }
    }
}
