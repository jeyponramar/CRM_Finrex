using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class ChangePassword_add : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblPageTitle.Text = "Change Password";
            username.Text = GlobalUtilities.ConvertToString(CustomSession.Session("Login_UserName"));
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        ValidateUserProfile();
    }       
    private void ValidateUserProfile()
    {
        string CurrentPassword = "";
        string CurrentEntredPassword = "";
        string NewPassword = "";
        string query = "";
        int intUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId"));
        //DataRow dr = Common.GetOneRowData("tbl_user", intUserId);
        DataRow dr = Common.GetOneRowData("tbl_user", intUserId);
        InsertUpdate obj = new InsertUpdate();
        if (dr != null)
        {
            CurrentPassword = Convert.ToString(dr["user_password"]);
            CurrentEntredPassword = global.CheckInputData(txtcurrentpassword.Text);
            if (CurrentPassword == txtnewpassword.Text)
            {
                lblMessage.Text = "Password can not be same as old password.";
                lblMessage.Visible = true;
                return;
            }
            if (!Custom.IsPasswordStrength(txtnewpassword.Text))
            {
                lblMessage.Text = "Minimum length of the Password should be 6. <br/>Also Password should contains atlease one alphabet, one number and one special character!";
                return;
            }
            if (CurrentPassword == CurrentEntredPassword)
            {
                NewPassword = global.CheckInputData(txtconfirmpassword.Text);
                query = "UPDATE tbl_user SET user_password='" + NewPassword + "',user_lastpasswordchange=getdate() WHERE user_userid=" + intUserId;
                if (obj.ExecuteQuery(query))
                {
                    lblMessage.Text = "Password Changed Sucessfully";
                    lblMessage.Visible = true;
                }
                
            }
            else
            {
                lblMessage.Text = "Invalid Current Password";
                lblMessage.Visible = true;
                
            }

        }
        else
        {
            //Response.Redirect("login.aspx");
            Response.Redirect("~/adminlogin.aspx");
            
        }
    }
    
}
