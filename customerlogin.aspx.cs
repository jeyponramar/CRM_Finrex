using System.Collections.Generic;
using WebComponent;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;


public partial class customerlogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            System.IO.Directory.CreateDirectory(Server.MapPath("~/.well-known"));

            CustomSession.InitSession(12);

            try
            {
                if (Request.Cookies["rm_u"] != null)
                    txtUserName.Text = Encription.Decrypt(Request.Cookies["rm_u"].Value);
                if (Request.Cookies["rm_p"] != null)
                    txtPassword.Attributes.Add("value", Encription.Decrypt(Request.Cookies["rm_p"].Value));
                if (Request.Cookies["rm_u"] != null && Request.Cookies["rm_p"] != null)
                    chkRememberMe.Checked = true;
            }
            catch { }
        }
        //tdchangepassword.Visible = true;
        //tdLogin.Visible = false;
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
        InsertUpdate obj = new InsertUpdate();
        string query = "select * from tbl_clientuser WHERE clientuser_isactive=1 and clientuser_iswebuser=1 and clientuser_username='" + global.CheckData(txtUserName.Text) + "'" +
                    " and clientuser_password='" + global.CheckData(txtPassword.Text) + "'";
        DataRow dr = obj.ExecuteSelectRow(query);
        if (dr == null)
        {
            lblMessage.Text = "Invalid User Name / Password";
            lblMessage.Visible = true;
        }
        else
        {
            bool isexpired = true;
            int clientId = GlobalUtilities.ConvertToInt(dr["clientuser_clientid"]);
            query = "select * from tbl_client where client_clientid=" + clientId;
            DataRow drClient = DbTable.ExecuteSelectRow(query);
            int subscriptionStatus = 0;
            if (drClient != null)
            {
                subscriptionStatus = GlobalUtilities.ConvertToInt(drClient["client_subscriptionstatusid"]);
                if (subscriptionStatus == 1 || subscriptionStatus == 2)//1-trial,2-subscribed
                {
                    if (Convert.ToDateTime(drClient["client_enddate"]) < DateTime.Today)
                    {
                    }
                    else
                    {
                        isexpired = false;
                    }
                }
            }
            if (isexpired)
            {
                //lblMessage.Text = "Your account has been Expired, please contact administrator to activate your account!";
                lblMessage.Text = "Your subscription has expired. Kindly contact your advisor.<br/>" +
                                  "Or Contact No. :  022-26820633/34 and Email ID : <a href='mailto:info@finrex.in'>info@finrex.in</a>";
                lblMessage.Visible = true;
                return;
            }
            int clientUserId = GlobalUtilities.ConvertToInt(dr["clientuser_clientuserid"]);
            Session["ClientUserId_Temp"] = clientUserId;
            Session["ClientId_Temp"] = clientId;
            bool isPasswordchange = false;
            if (GlobalUtilities.ConvertToBool(dr["clientuser_isfirstlogin"]))
            {
                isPasswordchange = true;
            }
            if (dr["clientuser_lastpasswordchange"] == DBNull.Value
                || (DateTime.Now - Convert.ToDateTime(dr["clientuser_lastpasswordchange"])).Days > 90)
            {
                isPasswordchange = true;
            }
            //DeleteLiverateHistory();
            if (isPasswordchange)
            {
                tdchangepassword.Visible = true;
                tdLogin.Visible = false;
            }
            else
            {
                Login();
            }
        }

    }
    private void DeleteLiverateHistory()
    {
        string query = "";
        query = "select top 1 * from tbl_liveratehistory order by liveratehistory_date desc";
        DataRow dr1 = DbTable.ExecuteSelectRow(query);
        if (dr1 == null) return;
        string sqldate = GlobalUtilities.ConvertToSqlDate(Convert.ToDateTime(dr1["liveratehistory_date"]));
        query = "delete from tbl_liveratehistory where cast(liveratehistory_date as date)<cast('" + sqldate + @"' as date)";
        DbTable.ExecuteQuery(query);
    }
    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        lblMessage2.Text = "";
        int clientUserId = GlobalUtilities.ConvertToInt(Session["ClientUserId_Temp"]);
        if (txtNewPassword.Text.Trim() == "")
        {
            lblMessage2.Text = "Please enter the Password!";
            return;
        }

        if (clientUserId == 0)
        {
            Response.Redirect("~/customerlogin.aspx");
        }
        string query = "select * from tbl_clientuser WHERE clientuser_clientuserid=" + clientUserId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (txtNewPassword.Text != txtConfirmPassword.Text)
        {
            lblMessage2.Text = "New Password is not matching with Confirm Password!";
            return;
        }
        if (GlobalUtilities.ConvertToString(dr["clientuser_password"]) == txtNewPassword.Text)
        {
            lblMessage2.Text = "Password can not be same as old password.";
            return;
        }
        if (!Custom.IsPasswordStrength(txtNewPassword.Text))
        {
            lblMessage2.Text = "Minimum length of the Password should be 6. <br/>Also Password should contains atlease one alphabet, one number and one special character!";
            return;
        }
        
        Hashtable hstbl = new Hashtable();
        hstbl.Add("password", txtNewPassword.Text);
        hstbl.Add("lastpasswordchange", "getdate()");
        hstbl.Add("isfirstlogin", "0");
        InsertUpdate obj = new InsertUpdate();
        obj.UpdateData(hstbl, "tbl_clientuser", clientUserId);
        Login();
    }
    private void Login()
    {
        int clientUserId = GlobalUtilities.ConvertToInt(Session["ClientUserId_Temp"]);
        if (clientUserId == 0)
        {
            Response.Redirect("~/customerlogin.aspx");
        }
        int clientId = GlobalUtilities.ConvertToInt(Session["ClientId_Temp"]);
        string query = "select * from tbl_clientuser WHERE clientuser_clientuserid="+clientUserId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        query = "select * from tbl_client where client_clientid=" + clientId;
        DataRow drClient = DbTable.ExecuteSelectRow(query);
        int subscriptionStatus = GlobalUtilities.ConvertToInt(drClient["client_subscriptionstatusid"]);
        string oldSessionId = GlobalUtilities.ConvertToString(dr["clientuser_sessionid"]);
        int subscriptionId = GlobalUtilities.ConvertToInt(drClient["client_subscriptionid"]);
        if (oldSessionId != "")
        {
            query = "delete from tbl_session where SessionId='" + oldSessionId + "'";
            DbTable.ExecuteQuery(query);
        }

        query = "update tbl_clientuser set clientuser_isloggedin=1,clientuser_sessionid='" + CustomSession.SessionID + "' " +
                "where clientuser_clientuserid=" + clientUserId;
        DbTable.ExecuteQuery(query);

        //UpdateLoginHistory(clientId, clientUserId);
        Common.SaveClientUserHistory(clientUserId, 1);

        CustomSession.Session("Login_IsLoggedIn", true);
        CustomSession.Session("Login_Name", dr["clientuser_name"]);
        CustomSession.Session("Login_ClientUserId", clientUserId);
        CustomSession.Session("Login_SubscriptionId", subscriptionId);
        //CustomSession.Session("Login_TrialId", trialId);
        CustomSession.Session("Login_ClientId", clientId);
        CustomSession.Session("Login_SubscriptionStatusId", subscriptionStatus);
        CustomSession.Session("Login_IsAdmin", GlobalUtilities.ConvertToBool(dr["clientuser_isadmin"]));

        if (chkRememberMe.Checked)
        {
            Response.Cookies["rm_u"].Value = Encription.Encrypt(GlobalUtilities.ConvertToString(dr["clientuser_username"]));
            Response.Cookies["rm_p"].Value = Encription.Encrypt(GlobalUtilities.ConvertToString(dr["clientuser_password"]));
            Response.Cookies["rm_u"].Expires = DateTime.Now.AddDays(365);
            Response.Cookies["rm_p"].Expires = DateTime.Now.AddDays(365);
        }
        else
        {
            Response.Cookies["rm_u"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["rm_p"].Expires = DateTime.Now.AddDays(-1);
        }
        Session.Remove("ClientUserId_Temp");
        Session.Remove("ClientId_Temp");
        Finstation.SetFinstationProspects(clientId);
        if (Request.QueryString["url"] == null)
        {
            if (Finstation.IsFinstationEnabled() || Finstation.IsMiniFinstationEnabled())
            {
                Response.Redirect("~/index.aspx");
            }
            else if (Finstation.IsBankScanEnabled())
            {
                Response.Redirect("~/viewbankaudit.aspx");
            }
            else if (Commodity.IsCommodityEnabled())
            {
                Response.Redirect("~/commodity-metal.aspx");
            }
            else
            {
                CustomSession.Delete();
                Session.Abandon();
                Response.Redirect("~/customerlogin.aspx");
            }
        }
        else
        {
            Response.Redirect("~/" + Request.QueryString["url"]);
        }
    }
    private void UpdateLoginHistory(int clientId,int clientUserId)
    {
        Hashtable hstbl = new Hashtable();
        InsertUpdate obj = new InsertUpdate();
        hstbl.Add("clientid", clientId);
        hstbl.Add("clientuserid", clientUserId);
        hstbl.Add("logintime", "getdate()");
        obj.InsertData(hstbl, "tbl_loginhistory");
    }
    protected void lnkforgotpassword_Click(object sender, EventArgs e)
    {
        tdLogin.Visible = false;
        tdforgotpassword.Visible = true;
    }
    protected void btnForgotPassword_Click(object sender, EventArgs e)
    {
        string strEmailId = global.CheckInputData(txtemailid_fp.Text);
        string query = "select * from tbl_clientuser " +
                        "where clientuser_username=@username";
        Hashtable hstbl = new Hashtable();
        hstbl.Add("username", strEmailId);
        DataRow dr = DbTable.ExecuteSelectRow(query,hstbl);
        if (dr != null)
        {
            string struseremailid = GlobalUtilities.ConvertToString(dr["clientuser_username"]);
            string strMessage = "<table cellpadding=0 cellspacing=0 width='800'><tr><td><table width='100%'>" +
                                "<tr><td style='border-bottom:solid 5px #17365d;background-color:#17365d;color:#ffffff;font-size:15px;padding:10px 0px 10px 10px;font-family:Trebuchet MS;'>Forgot Password</td></tr>" +
                                "<tr><td style='background-color:#ebebeb;border-bottom:solid 1px #dad8d8;border-right:solid 1px #dad8d8;border-left:solid 1px #dad8d8;color:#484848;font-size:14px;padding-left:15px;padding-bottom:15px;'><table>" +
                                "<tr><td>Hi <b>" + Convert.ToString(dr["clientuser_name"]) + "</b>,</td></tr>" +
                                "<tr><td>&nbsp;</td></tr>" +
                                "<tr><td>Your UserName is <b>" + Convert.ToString(dr["clientuser_username"]) + "</td></tr>" +
                                "<tr><td>&nbsp;</td></tr>" +
                                 "<tr><td>Your Password is <b>" + Convert.ToString(dr["clientuser_password"]) + "</td></tr>" +
                                "<tr><td>Best Regards,</td></tr>" +
                                "<tr><td><b>Finrex</b></td></tr>" +
                            "</table></td></tr></table></td></tr></table>";

            string CcEmailid = GlobalUtilities.GetAppSetting("AdminEmailId");
            if (txtUserName.Text != "")
            {
                BulkEmail.SendMail(struseremailid, "Forgot Password - Finrex", strMessage, "");

                lblMessage_fp.Text = "Password sent to your Email Id.";
            }

            txtUserName.Text = "";
            lblMessage_fp.Visible = true;
        }
        else
        {
            lblMessage_fp.Text = "Invalid Email Id";
            lblMessage_fp.Visible = true;
        }
    }
    protected void lnkbacktologin_Click(object sender, EventArgs e)
    {
        tdLogin.Visible = true;
        tdforgotpassword.Visible = false;
    }
}

