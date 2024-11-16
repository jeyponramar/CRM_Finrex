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

public partial class customerquerydetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["qid"] != null)
        {
            string query = "select * from tbl_customerquery where customerquery_uniqueid='" + Common.GetQueryString("qid") + "'";
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (dr != null)
            {
                Response.Redirect("~/advisor/querydetail.aspx?id=" + GlobalUtilities.ConvertToInt(dr["customerquery_customerqueryid"]));
            }
        }

        Custom.CheckSubscriptionAccess();
        Finstation.CheckFullFinstationAccess();
        CheckPermission();
        if (!IsPostBack)
        {
            BindQueryDetail();
            UpdateVisitedStatus();
            BindResponse();
        }
    }
    private void CheckPermission()
    {
        string query = @"select * from tbl_customerquery
                        where customerquery_clientuserid=" + ClientUserId + " and customerquery_customerqueryid=" + Id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            Response.End();
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
    private void UpdateVisitedStatus()
    {
        string query = "";
        query = @"update tbl_customerquery set customerquery_iscustomervisited=1 where customerquery_customerqueryid=" + Id;
        DbTable.ExecuteQuery(query);
    }
    private void BindQueryDetail()
    {
        string query = "";
        query = @"select * from tbl_customerquery
                  join tbl_querytopic on querytopic_querytopicid=customerquery_querytopicid
                  join tbl_querytype on querytype_querytypeid=querytopic_querytypeid
                  join tbl_querystatus on querystatus_querystatusid=customerquery_querystatusid
                  where customerquery_clientuserid=" + ClientUserId + " and customerquery_customerqueryid=" + Id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        lbltype.Text = GlobalUtilities.ConvertToString(dr["querytype_querytype"]);
        lbltopic.Text = GlobalUtilities.ConvertToString(dr["querytopic_topicname"]);
        lblsubject.Text = GlobalUtilities.ConvertToString(dr["customerquery_subject"]);
        lblquery.Text = GlobalUtilities.ConvertToString(dr["customerquery_query"]);
        lbldate.Text = GlobalUtilities.ConvertToDateTime(dr["customerquery_date"]);
        lblstatus.Text = GlobalUtilities.ConvertToString(dr["querystatus_status"]);
        string attachment = GlobalUtilities.ConvertToString(dr["customerquery_attachments"]);
        if (attachment != "")
        {
            Array arr = attachment.Split('|');
            StringBuilder html = new StringBuilder();
            html.Append("<table>");
            for (int i = 0; i < arr.Length; i++)
            {
                string filePath = arr.GetValue(i).ToString();
                string fileName = filePath.Split('_').GetValue(1).ToString();
                html.Append("<tr><td><a href='download-file.aspx?f=upload/customerquery/attachment/" + filePath + "' target='_blank'>" + fileName + "</a></td></tr>");
            }
            html.Append("</table>");
            ltattachment.Text = html.ToString();
        }
        int statusId = GlobalUtilities.ConvertToInt(dr["customerquery_querystatusid"]);
        if (statusId == 5)
        {
            btnResolve.Visible = false;
        }
        else
        {
            btnResolve.Visible = true;
        }
    }
    protected void btnreply_click(object sender, EventArgs e)
    {
        string query = "";
        Hashtable hstbl = new Hashtable();
        hstbl.Add("reply", txtresponse.Text);
        hstbl.Add("date", "getdate()");
        hstbl.Add("clientuserid", ClientUserId);
        hstbl.Add("customerqueryid", Id);
        hstbl.Add("iscustomerreply", 1);
        InsertUpdate obj = new InsertUpdate();
        int id = obj.InsertData(hstbl, "tbl_customerqueryreply");
        if (id > 0)
        {
            mfureplyattachment.SaveFileNameInDBByModule("customerqueryreply", id);
            query = @"update tbl_customerquery set customerquery_querystatusid=3,customerquery_lastupdateddate=getdate(),
                        customerquery_isadvisorvisited=0,customerquery_IsAdvisorReminderSent=0 
                       where customerquery_customerqueryid=" + Id;
            DbTable.ExecuteQuery(query);
            SendEmail();
            txtresponse.Text = "";
            BindResponse();
            //Response.Redirect("~/viewcustomerquery.aspx");
        }
    }
    private void SendEmail()
    {
        string query = @"select * from tbl_customerquery
                        join tbl_querytopic on querytopic_querytopicid=customerquery_querytopicid
                        where customerquery_customerqueryid=" + Id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        string type = "FEMA";
        string appurl = ConfigurationManager.AppSettings["AppUrl"];
        string toEmailId = "";
        string bccEmailid = "";
        if (GlobalUtilities.ConvertToInt(dr["querytopic_querytypeid"]) == 1)//exim
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
        string url = appurl + "/adminlogin.aspx?url=advisor/querydetail.aspx?qid=" + GlobalUtilities.ConvertToString(dr["customerquery_uniqueid"]);
        string subject = "Resolving the Query - " + GlobalUtilities.ConvertToString(dr["customerquery_subject"]);
        string body = Common.GetSetting("CustomerQuery_NewQueryToAdvisor");
        body = body.Replace("$url$", url);
        body = body.Replace("$querytype_querytype$", type);
        BulkEmail.SendMail_Alert(toEmailId, "", bccEmailid, subject, body, "");
    }
    private void BindResponse()
    {
        string query = @"select * from tbl_customerqueryreply
                        left join tbl_user on user_userid=customerqueryreply_userid
                        left join tbl_clientuser on clientuser_clientuserid=customerqueryreply_clientuserid
                        where customerqueryreply_customerqueryid=" + Id +
                        " order by customerqueryreply_customerqueryreplyid desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='50%' cellpadding='5'>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string message = GlobalUtilities.ConvertToString(dttbl.Rows[i]["customerqueryreply_reply"]);
            string attachment = GlobalUtilities.ConvertToString(dttbl.Rows[i]["customerqueryreply_replyattachment"]);
            bool iscustomerreply = GlobalUtilities.ConvertToBool(dttbl.Rows[i]["customerqueryreply_iscustomerreply"]);
            string date = GlobalUtilities.ConvertToDateTime(dttbl.Rows[i]["customerqueryreply_date"]);
            string name = "";
            html.Append("<tr><td>");
            string titlecss = "cust-query-title";
            html.Append("<div class='query-msg'>");
            if (iscustomerreply)
            {
                name = GlobalUtilities.ConvertToString(dttbl.Rows[i]["clientuser_name"]);
            }
            else
            {
                name = GlobalUtilities.ConvertToString(dttbl.Rows[i]["user_fullname"]);
                titlecss = "adv-query-title";
            }
            html.Append("<table width='100%' cellpadding='10' cellspacing=0>");
            html.Append("<tr class='" + titlecss + "'><td class='bold'>" + name + "</td><td align='right'>" + date + "</td></tr>");
            html.Append("<tr><td colspan='2' style='padding-bottom:20px;'>" + message + "</td></tr>");
            if (attachment != "")
            {
                Array arr = attachment.Split('|');
                html.Append("<tr><td><table>");
                for (int j = 0; j < arr.Length; j++)
                {
                    string fileFullName = arr.GetValue(j).ToString();
                    string fileName = fileFullName.Split('_').GetValue(1).ToString();
                    html.Append("<tr><td><a href='download-file.aspx?f=upload/customerqueryreply/" + fileFullName + "' target='_blank'>" + fileName + "</a></td></tr>");
                }
                html.Append("</table></td></tr>");
            }
            
            html.Append("</table></div>");
            html.Append("</td></tr>");
        }
        html.Append("</table>");
        ltreplies.Text = html.ToString();
    }
    protected void btnresolved_click(object sender, EventArgs e)
    {
        string query = "";
        query = @"update tbl_customerquery set customerquery_querystatusid=5,customerquery_lastupdateddate=getdate(),
                        customerquery_isadvisorvisited=1 where customerquery_customerqueryid=" + Id;
        DbTable.ExecuteQuery(query);
        lblMessage.Text = "Your query has been resolved, you can reply anytime to open this query again.";
        lblMessage.Visible = true;
        BindQueryDetail();
    }
    public string VersionNo
    {
        get
        {
            return ConfigurationManager.AppSettings["VersionNo"].ToString();
        }
    }
}
