using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Collections;

public partial class resetpassword_resetpassword : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (GlobalUtilities.ConvertToInt(CustomSession.Session("Login_RoleId")) != 1)
        {
            Session["S_Error"] = "You do not have access rights to perform this operation!";
            Response.Redirect("../error.aspx");
        }
        if (!IsPostBack)
        {
            gblData.FillDropdown(ddluserid, "tbl_user", "user_username", "user_userid", "", "user_username");
        }
        lblPageTitle.Text = "Reset Password";
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
        InsertUpdate obj = new InsertUpdate();
        Hashtable hstb = new Hashtable();
        hstb.Add("password", txtresetpassword.Text);
        obj.UpdateData(hstb, "tbl_user",GlobalUtilities.ConvertToInt(ddluserid.SelectedValue));
        gblData.ResetForm(form);
        lblMessage.Text = "Reset password done successfully";
        lblMessage.Visible = true;
    }
}
