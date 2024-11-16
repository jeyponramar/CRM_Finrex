using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Collections;
using System.Data;

public partial class exportportal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        int clientUserId = 0;
        if (Request.QueryString["ia"] == null)
        {
            if (!GlobalUtilities.ConvertToBool(CustomSession.Session("Login_IsLoggedIn")))
            {
                Response.Redirect("~/customerlogin.aspx");
            }
            clientUserId = Convert.ToInt32(CustomSession.Session("Login_ClientUserId"));
        }
        else
        {
            //admin user
            if (GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId")) > 0)
            {
                int clientId = 0; string query = "";
                if (Request.QueryString["sid"] == null)
                {
                    int trialId = Common.GetQueryStringValue("tid");
                    query = "select * from tbl_trial where trial_trialid=" + trialId;
                    DataRow dr=DbTable.ExecuteSelectRow(query);
                    if (dr == null) return;
                    clientId = GlobalUtilities.ConvertToInt(dr["trial_clientid"]);
                }
                else
                {
                    int subscriptionId = Common.GetQueryStringValue("sid");
                    query = "select * from tbl_subscription where subscription_subscriptionid=" + subscriptionId;
                    DataRow dr = DbTable.ExecuteSelectRow(query);
                    if (dr == null) return;
                    clientId = GlobalUtilities.ConvertToInt(dr["subscription_clientid"]);
                }
                
                query = "select * from tbl_clientuser where clientuser_clientid=" + clientId;
                DataRow druser = DbTable.ExecuteSelectRow(query);
                if (druser == null)
                {
                    Response.Write("No user found!");
                    return;
                }
                clientUserId = GlobalUtilities.ConvertToInt(druser["clientuser_clientuserid"]);
                CustomSession.Session("Login_ProspectIds", "4,5");
            }
            else
            {
                Response.Redirect("~/adminlogin.aspx");
            }
        }
        string sessionId = Guid.NewGuid().ToString();
        Hashtable hstbl = new Hashtable();
        hstbl.Add("session", "cuserid~" + clientUserId.ToString());
        hstbl.Add("sessionid", sessionId);
        InsertUpdate obj = new InsertUpdate();
        int id=obj.InsertData(hstbl,"tbl_tempsession");
        if(id>0)
        {
            string url = "~/exportexposureportal/portalredirect.aspx?pt="+Request.QueryString["pt"];
            //if(AppConstants.IsLive)
            //{
            //    url = "http://portal.finstation.in/exportexposureportal/portalredirect.aspx";
            //}
            url += "&sid=" + sessionId;
            Response.Redirect(url);
        }
        
    }
}
