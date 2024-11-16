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
using System.Text;

public partial class chat_chatfeedback : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CloseChat();
        }
    }
    private void CloseChat()
    {
        int chatId = GlobalUtilities.ConvertToInt(Session["S_ChatId"]);
        string query = "update tbl_chat set chat_chatstatusid=3 where chat_chatid=" + chatId;
        DbTable.ExecuteQuery(query);
        SendChatHistory();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int chatId = GlobalUtilities.ConvertToInt(Session["S_ChatId"]);
        Hashtable hstbl = new Hashtable();
        hstbl.Add("rating", hdnRating.Text);
        hstbl.Add("feedback", txtFeedback.Text);
        InsertUpdate obj = new InsertUpdate();
        obj.UpdateData(hstbl, "tbl_chat", chatId);
        trFeedback.Visible = false;
        trMessage.Visible = true;
    }
    private void SendChatHistory()
    {
        int chatId = GlobalUtilities.ConvertToInt(Session["S_ChatId"]);
        string query = "select * from tbl_chatdetail " +
                       "where chatdetail_chatid=" + chatId+" order by chatdetail_chatdetailid";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='800'>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string name = GlobalUtilities.ConvertToString(dttbl.Rows[i]["chatdetail_name"]);
            string message = GlobalUtilities.ConvertToString(dttbl.Rows[i]["chatdetail_message"]);
            int agentId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["chatdetail_agentid"]);
            string style = "border-top:solid 1px #f5f5f5;border-bottom:solid 1px #f5f5f5;";
            if (agentId == 0)
            {
                style = "background-color:#f5f5f5;";
            }
            html.Append("<tr><td style='" + style + "'><table width='100%'>");
            html.Append("<tr><td width='100' style='font-weight:bold;'>" + name + " : </td><td>" + message + "</td></tr>");
            html.Append("<tr><td colspan='2' align='right' style='font-size:10px;color:#888;'>" + GlobalUtilities.ConvertToDateTime(dttbl.Rows[i]["chatdetail_createddate"]) + "</td></tr>");
            html.Append("</table></td></tr>");
        }
        html.Append("</table>");
        string body = html.ToString();
        query = "select * from tbl_chat JOIN tbl_clientuser ON clientuser_clientuserid=chat_clientuserid where chat_chatid=" + chatId;
        DataRow drChat = DbTable.ExecuteSelectRow(query);
        string emailId = GlobalUtilities.ConvertToString(drChat["clientuser_username"]);
        BulkEmail.SendMail(emailId, "Chat History - Finstation Support", body, "");
    }
}
