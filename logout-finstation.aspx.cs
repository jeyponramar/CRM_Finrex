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

public partial class logout_finstation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CustomSession.Delete();
        Session.Abandon();
        Response.Redirect("~/customerlogin.aspx");
    }
}
