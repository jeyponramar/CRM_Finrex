using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;

public partial class advisorhome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CustomSession.Session("Login_IsRefuxLoggedIn") == null)
        {
            Response.Redirect("~/adminlogin.aspx");
        }
        if (!IsPostBack)
        {
        }
        lblLoginUserName.Text = GlobalUtilities.ConvertToString(CustomSession.Session("Login_FullName"));
    }
    
    
}
