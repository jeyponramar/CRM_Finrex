using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class demo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDemoUser();
        }
    }
    private void BindDemoUser()
    {
        CustomSession.Session("Login_IsRefuxLoggedIn", true);
        CustomSession.Session("Login_FullName", "Demo");
        CustomSession.Session("Login_UserName", "demo@rpluscrm.com");
        CustomSession.Session("Login_UserId", "1");
        CustomSession.Session("Login_RoleId", "1");
        //Response.Redirect("admin.aspx");
    }
}
