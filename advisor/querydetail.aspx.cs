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
using System.Configuration;

public partial class querydetail : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_customerquery", "customerqueryid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string query = "";
            if (Request.QueryString["qid"] != null)
            {
                query = "select * from tbl_customerquery where customerquery_uniqueid='" + Common.GetQueryString("qid") + "'";
                DataRow dr = DbTable.ExecuteSelectRow(query);
                if (dr != null)
                {
                    Response.Redirect("~/advisor/querydetail.aspx?id=" + GlobalUtilities.ConvertToInt(dr["customerquery_customerqueryid"]));
                }
            }
            if (Request.QueryString["id"] == null)
            {
                Response.End();
            }
            else
            {
                
                int id = GetId();
                //if (CustomSession.Session("Login_IsRefuxLoggedIn") == null)
                //{
                //    Response.Redirect("~/adminlogin.aspx?url=advisor/querydetail.aspx?id=" + id);
                //}
                query = @"select * from tbl_customerquery
                         LEFT JOIN tbl_querytopic ON querytopic_querytopicid=customerquery_querytopicid
                         LEFT JOIN tbl_clientuser ON clientuser_clientuserid=customerquery_clientuserid
                         LEFT JOIN tbl_querystatus ON querystatus_querystatusid=customerquery_querystatusid
                         LEFT JOIN tbl_client on client_clientid=clientuser_clientid
                         where customerquery_customerqueryid=" + id;
                DataRow dr = DbTable.ExecuteSelectRow(query);
                gblData.PopulateForm(dr, form);
                int clientId = GlobalUtilities.ConvertToInt(dr["client_clientid"]);
                string userName = GlobalUtilities.ConvertToString(dr["clientuser_username"]);
                query = "select * from tbl_contacts where contacts_clientid=" + clientId + " and contacts_emailid='" + userName + "'";
                DataRow drcontact = DbTable.ExecuteSelectRow(query);
                if (drcontact != null)
                {
                    lblusermobileno.Text = GlobalUtilities.ConvertToString(drcontact["contacts_mobileno"]);
                }
                BindAttachment(GlobalUtilities.ConvertToString(dr["customerquery_attachments"]));
                BindResponse();
                if (!Finstation.IsAdvisor)
                {
                    trresponse.Visible = false;
                }
            }
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
    }
    private void BindAttachment(string attachment)
    {
        StringBuilder html = new StringBuilder();
        if (attachment != "")
        {
            Array arr = attachment.Split('|');
            html.Append("<table>");
            for (int j = 0; j < arr.Length; j++)
            {
                string fileFullName = arr.GetValue(j).ToString();
                string fileName = Common.GetFileName(fileFullName);
                html.Append("<tr><td><a href='../download.aspx?f=upload/customerquery/" + fileFullName + "' target='_blank'>" + fileName + "</a></td></tr>");
            }
            html.Append("</table>");
        }
        ltattachment.Text = html.ToString();
    }
    protected void btnreply_click(object sender, EventArgs e)
    {
        int queryId = GetId();
        string query = "";
        Hashtable hstbl = new Hashtable();
        hstbl.Add("reply", txtresponse.Text);
        hstbl.Add("date", "getdate()");
        hstbl.Add("userid", Common.UserId);
        hstbl.Add("customerqueryid", queryId);
        InsertUpdate obj = new InsertUpdate();
        int id = obj.InsertData(hstbl, "tbl_customerqueryreply");
        if (id > 0)
        {
            mfureplyattachment.SaveFileNameInDBByModule("customerqueryreply", id);
            query = @"update tbl_customerquery set customerquery_querystatusid=2,customerquery_lastupdateddate=getdate(),customerquery_iscustomervisited=0,customerquery_IsAdvisorReminderSent=1
                       where customerquery_customerqueryid=" + queryId;
            DbTable.ExecuteQuery(query);
            SendEmail();
            txtresponse.Text = "";
        }
        BindResponse();
    }
    private void SendEmail()
    {
        string query = @"select * from tbl_customerquery
                        join tbl_querytopic on querytopic_querytopicid=customerquery_querytopicid
                        join tbl_clientuser on clientuser_clientuserid=customerquery_clientuserid
                        join tbl_client on client_clientid=clientuser_clientid
                        where customerquery_customerqueryid=" + GetId();
        DataRow dr = DbTable.ExecuteSelectRow(query);
        //string type = "FEMA";
        string appurl = ConfigurationManager.AppSettings["AppUrl"];
        string toEmailId = GlobalUtilities.ConvertToString(dr["clientuser_username"]);//  GlobalUtilities.ConvertToString(dr["clientuser_username"]);
        //string bccEmailid = "";
        //if (GlobalUtilities.ConvertToInt(dr["querytopic_querytypeid"]) == 1)//exim
        //{
        //    type = "EXIM";
        //    toEmailId = Common.GetSetting("Advisor_EximToEmailId");
        //    bccEmailid = Common.GetSetting("Advisor_EximBccEmailId");
        //}
        //else
        //{
        //    toEmailId = Common.GetSetting("Advisor_FemaToEmailId");
        //    bccEmailid = Common.GetSetting("Advisor_FemaBccEmailId");
        //}
        string url = appurl + "/customerlogin.aspx?url=customerquerydetail.aspx?qid=" + GlobalUtilities.ConvertToString(dr["customerquery_uniqueid"]);
        string subject = "Query Resolved - " + GlobalUtilities.ConvertToString(dr["customerquery_subject"]);
        string body = Common.GetSetting("CustomerQuery_ReplyToCustomer");
        body = body.Replace("$url$", url);
        body = body.Replace("$clientuser_name$", GlobalUtilities.ConvertToString(dr["clientuser_name"]));
        body = body.Replace("$customerquery_subject$", GlobalUtilities.ConvertToString(dr["customerquery_subject"]));
        body = body.Replace("$customerquery_query$", GlobalUtilities.ConvertToString(dr["customerquery_query"]));

        body = Common.GetFormattedSettingForEmail(body, dr, false);

        BulkEmail.SendMail_Alert(toEmailId, "", "", subject, body, "");
    }
    private void BindResponse()
    {
        string query = @"select * from tbl_customerqueryreply
                        left join tbl_user on user_userid=customerqueryreply_userid
                        left join tbl_clientuser on clientuser_clientuserid=customerqueryreply_clientuserid
                        where customerqueryreply_customerqueryid=" + GetId() +
                        " order by customerqueryreply_customerqueryreplyid desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%' cellpadding='5'>");
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
                    string fileName = Common.GetFileName(fileFullName);
                    html.Append("<tr><td><a href='../upload/customerqueryreply/" + fileFullName + "' target='_blank'>" + fileName + "</a></td></tr>");
                }
                html.Append("</table></td></tr>");
            }

            html.Append("</table></div>");
            html.Append("</td></tr>");
        }
        html.Append("</table>");
        ltreplies.Text = html.ToString();
    }
    private int GetId()
    {
        return GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
        
    }
    
}
