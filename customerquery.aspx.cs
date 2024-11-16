using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;
using System.Collections;
using System.Configuration;

public partial class customerquery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Custom.CheckSubscriptionAccess();
        Finstation.CheckFullFinstationAccess();
        if (!IsPostBack)
        {
            BindQueryDetail();
        }
    }
   
    private int Id
    {
        get
        {
            return Common.GetQueryStringValue("id");
        }
    }
    private int ClientUserId
    {
        get
        {
            return GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
        }
    }
    private void BindQueryDetail()
    {
        DataRow dr = DbTable.GetOneRow("tbl_querytopic", Id);
        if (dr == null)
        {
            Response.End();
        }
        lbltopic.Text = GlobalUtilities.ConvertToString(dr["querytopic_topicname"]);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string uniqueId = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
        uniqueId = uniqueId.Replace("-", "");
        Hashtable hstbl = new Hashtable();
        hstbl.Add("subject", txtsubject.Text);
        hstbl.Add("query", txtquery.Text);
        hstbl.Add("clientuserid", ClientUserId);
        hstbl.Add("querystatusid", 1);
        hstbl.Add("querytopicid", Id);
        hstbl.Add("date", "getdate()");
        hstbl.Add("lastupdateddate", "getdate()");
        hstbl.Add("IsAdvisorReminderSent", 0);
        hstbl.Add("uniqueid", uniqueId);
        InsertUpdate obj = new InsertUpdate();
        int id = obj.InsertData(hstbl, "tbl_customerquery");
        if (id > 0)
        {
            SendEmail(id, uniqueId);
            mfuattachments.SaveFileNameInDBByModule("customerquery", id);
            Response.Redirect("~/viewcustomerquery.aspx");
        }
        else
        {
            lblMessage.Text = "Error occurred, please try again.";
        }
    }
    private void SendEmail(int id, string uniqueId)
    {
        string query = "";
        query = @"select * from tbl_customerquery
                join tbl_clientuser on clientuser_clientuserid=customerquery_clientuserid
                join tbl_client on client_clientid=clientuser_clientid
                where customerquery_customerqueryid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        DataRow drt = DbTable.GetOneRow("tbl_querytopic", Id);
        string type = "FEMA";
        string appurl = ConfigurationManager.AppSettings["AppUrl"];
        string toEmailId = "";
        string bccEmailid = "";
        if (GlobalUtilities.ConvertToInt(drt["querytopic_querytypeid"]) == 1)//exim
        {
            type = "EXIM";
            toEmailId = Common.GetSetting("Advisor_EximToEmailId");
            bccEmailid = Common.GetSetting("Advisor_EximBccEmailId");
        }
        else
        {
            toEmailId = Common.GetSetting("Advisor_FemaToEmailId");
            bccEmailid = Common.GetSetting("Advisor_FemaBccEmailId");
        }
        string url = appurl + "/adminlogin.aspx?url=advisor/querydetail.aspx?qid=" + uniqueId;
        string subject = "Resolving the Query - " + txtsubject.Text;
        string body = Common.GetSetting("CustomerQuery_NewQueryToAdvisor");
        body = body.Replace("$url$", url);
        body = body.Replace("$querytype_querytype$", type);
        body = body.Replace("$customerquery_subject$", txtsubject.Text);
        body = body.Replace("$customerquery_query$", txtquery.Text);
        //DataRow drc = DbTable.GetOneRow("tbl_client", Common.ClientId);
        //body = body.Replace("$client_customername$", GlobalUtilities.ConvertToString(drc["client_customername"]));
        body = Common.GetFormattedSettingForEmail(body, dr, false);

        BulkEmail.SendMail_Alert(toEmailId, "", bccEmailid, subject, body, "");
    }
    public string VersionNo
    {
        get
        {
            return ConfigurationManager.AppSettings["VersionNo"].ToString();
        }
    }
}
