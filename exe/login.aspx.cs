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
using WebComponent;

public partial class exe_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if (Request.QueryString["gid"] == null)
            //{
            //    Response.Redirect("~/exe/login.aspx?sid=" + Request.QueryString["sid"] + "&gid=" + Guid.NewGuid().ToString());
            //}
            CheckLogin();
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        InsertUpdate obj = new InsertUpdate();
        string query = "select * from tbl_clientuser WHERE clientuser_isactive=1 and clientuser_isexeuser=1 and clientuser_username='" + global.CheckData(txtusername.Text) + "'" +
                    " and clientuser_password='" + global.CheckData(txtpassword.Text) + "'";
        DataRow dr = obj.ExecuteSelectRow(query);
        if (dr == null)
        {
            lblMessage.Text = "Invalid User Name / Password";
            lblMessage.Visible = true;
        }
        else
        {
            string sessionId = Common.GetQueryString("sid");
            int userId = GlobalUtilities.ConvertToInt(dr["clientuser_clientuserid"]);

            if (!IsUserSubscriptionActive(userId))
            {
                return;
            }

            query = "update tbl_clientuser set clientuser_exesessionid='" + sessionId + "',clientuser_lastexeheartbeat=getdate() where clientuser_clientuserid=" + userId;
            DbTable.ExecuteQuery(query);
            //SaveLoginHistory(GlobalUtilities.ConvertToInt(dr["clientuser_clientuserid"]));
            Response.Redirect("~/exe/default.aspx?sid=" + sessionId + "&header=" + Request.QueryString["header"]);
        }
    }
    private void CheckLogin()
    {
        string sessionId = global.CheckInputData(Common.GetQueryString("sid"));
        if (sessionId == "") return;
        //string query = "select * from tbl_clientuser WHERE clientuser_exesessionid='" + sessionId + "' "+
        //                "and datediff(s,getdate(),clientuser_lastexeheartbeat) < 60";//<1 min heart beat
        string query = "select * from tbl_clientuser WHERE clientuser_exesessionid='" + sessionId + "'";//<1 min heart beat
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            //query = "select * from tbl_clientuser WHERE clientuser_exesessionid='" + sessionId + "' ";
            //dr = DbTable.ExecuteSelectRow(query);
            //int clientUserId = 0;
            //if (dr == null)
            //{
            //}
            //else
            //{
            //}
            //query = "update tbl_clientuser set clientuser_exesessionid='" + sessionId + "'";
            //DbTable.ExecuteQuery(query);
        }
        else
        {
            if (!IsUserSubscriptionActive(GlobalUtilities.ConvertToInt(dr["clientuser_clientuserid"])))
            {
                return;
            }
            if (GlobalUtilities.ConvertToBool(dr["clientuser_isexeuser"]))
            {
                //SaveLoginHistory(GlobalUtilities.ConvertToInt(dr["clientuser_clientuserid"]));
                Response.Redirect("~/exe/default.aspx?sid=" + sessionId + "&header=" + Request.QueryString["header"]);
            }
            else
            {

            }
        }
    }
    private bool IsUserSubscriptionActive(int clientUserId)
    {
        string query = "select * from tbl_clientuser where clientuser_clientuserid=" + clientUserId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        int subscriptionId = 0;
        int trialId = 0;
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
            lblMessage.Text = "Your subscription has expired. Kindly contact your advisor.<br/>" +
                              "Or Contact No. :  022-26820633/34 and Email ID : <a href='mailto:info@finrex.in'>info@finrex.in</a><br/><br/><br/><br/><br/>";
            lblMessage.Visible = true;
            tblLogin.Visible = false;
            return false;
        }
        return true;
    }
    private void SaveLoginHistory(int clientUserId)
    {
        string query = "select * from tbl_clientuser where clientuser_clientuserid=" + clientUserId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        Hashtable hstbl = new Hashtable();
        hstbl.Add("clientuserid", clientUserId);
        hstbl.Add("clientid", GlobalUtilities.ConvertToInt(dr["clientuser_clientid"]));
        hstbl.Add("logintime", "getdate()");
        hstbl.Add("loginsource", "1");//EXE
        InsertUpdate obj = new InsertUpdate();
        obj.InsertData(hstbl, "tbl_loginhistory");
    }
}
