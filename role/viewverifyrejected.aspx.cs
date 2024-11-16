using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class RoleVerifyRejected_view : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
			grid.SearchBy("Search By", "");
			grid.SearchBy("Role Name", "role_rolename");            
            lblPageTitle.Text = "View Role Verification Rejected";
            ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
        }
    }
    
}