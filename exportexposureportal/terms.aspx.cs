using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;

public partial class exportexposureportal_terms : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblterms.Text = Common.GetSetting("Portal Terms and Conditions");
        }
    }
    protected void btnAgree_Click(object sender, EventArgs e)
    {
        int clientuserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
        string query = "update tbl_clientuser set clientuser_isportalfirstlogin=0 where clientuser_clientuserid=" + clientuserId;
        DbTable.ExecuteQuery(query);
        Response.Redirect("~/exportexposureportal/portal.aspx?pt=" + Request.QueryString["pt"]);
    }
}
