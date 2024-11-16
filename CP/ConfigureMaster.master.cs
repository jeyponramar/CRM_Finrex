using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls; 
using WebComponent;

public partial class ConfigureMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = Request.Url.ToString().ToLower();
        if (AppConstants.IsLive == true)
        {
            Response.Redirect("~/default.aspx");
        }
        if (url.Contains("login.aspx"))
        {
        }
        else
        {
            if (!Convert.ToBoolean(CustomSession.Session("Login_IsRefuxLoggedIn")))
            {
                Response.Redirect("../login.aspx");
            }
        }
        if (!IsPostBack)
        {
        }
    }
}
