using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections;

public partial class change_password : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string query="select * from tbl_clientuser "+
                            "WHERE clientuser_clientuserid=" + GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (dr == null) return;
            username.Text = GlobalUtilities.ConvertToString(dr["clientuser_username"]);
        }
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
        int clientuserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
        //DataRow dr = Common.GetOneRowData("tbl_user", intUserId);
        DataRow dr = Common.GetOneRowData("tbl_clientuser", clientuserId);
        InsertUpdate obj = new InsertUpdate();
        if (dr != null)
        {
            CurrentPassword = Convert.ToString(dr["clientuser_password"]);
            CurrentEntredPassword = global.CheckInputData(txtcurrentpassword.Text);
            if (CurrentPassword == CurrentEntredPassword)
            {
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
                if (txtnewpassword.Text == txtconfirmpassword.Text)
                {
                    NewPassword = txtconfirmpassword.Text;
                    query = @"UPDATE tbl_clientuser SET clientuser_password='" + NewPassword + @"',clientuser_isfirstlogin=0,
                            clientuser_lastpasswordchange=getdate(),clientuser_mobilesessionid=null,clientuser_sessionid=null 
                            WHERE clientuser_clientuserid=" + clientuserId;
                    if (obj.ExecuteQuery(query))
                    {
                        query = "update tbl_clientuser set ";
                        lblMessage.Text = "Password Changed Sucessfully";
                        lblMessage.Visible = true;
                    }
                }
                else
                {
                    lblMessage.Text = "New Password is not matching with Confirm Password";
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
            Response.Redirect("~/customerlogin.aspx");

        }
    }
}
