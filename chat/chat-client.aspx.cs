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

public partial class chat_client1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            if (Session["S_ChatId"] == null)
            {
                ClientScript.RegisterClientScriptBlock(typeof(Page), "sessionexpired", "<script>sessionExpired();</script>");
            }
            else
            {
                BindChat();
            }
        }
        ShowHideLogin();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //if (Session["S_ChatId"] != null)
        //{
        //    BindChat();
        //    return;
        //}

        //string query = "select * from tbl_chatclient "+
        //               "where chatclient_emailid='" + global.CheckInputData(txtEmailId.Text) + "'";
        //DataRow dr = DbTable.ExecuteSelectRow(query);
        //int chatClientId = 0;
        //if (dr == null)
        //{
        //    Hashtable hstblClient = new Hashtable();
        //    hstblClient.Add("name", txtName.Text);
        //    hstblClient.Add("companyname", txtCompanyName.Text);
        //    hstblClient.Add("mobileno", txtMobileNo.Text);
        //    hstblClient.Add("emailid", txtEmailId.Text);
        //    InsertUpdate obj1 = new InsertUpdate();
        //    chatClientId = obj1.InsertData(hstblClient, "tbl_chatclient");
        //}
        //else
        //{
        //    chatClientId = GlobalUtilities.ConvertToInt(dr["chatclient_chatclientid"]);
        //}
        //string ip = "";
        //Hashtable hstbl = new Hashtable();
        //hstbl.Add("chatclientid", chatClientId);
        //hstbl.Add("date", "getdate()");
        //hstbl.Add("isactive", "1");
        //hstbl.Add("ip", ip);
        //hstbl.Add("clientname", txtName.Text);
        //hstbl.Add("chatstatusid", 1);
        //InsertUpdate obj = new InsertUpdate();
        //int chatId = obj.InsertData(hstbl, "tbl_chat");
        //hdnchatid.Text = chatId.ToString();
        //hdnchatclientid.Text = chatClientId.ToString();
        //hdnname.Text = txtName.Text;
        //tblLogin.Visible = false;
        //tblChat.Visible = true;
        //Session["S_ChatId"] = chatId;
        //Session["S_ChatClientId"] = chatClientId;
        //Session["S_ClientName"] = txtName.Text;
    }
    private void ShowHideLogin()
    {
        //if (Session["S_ChatId"] == null)
        //{
        //    tblLogin.Visible = true;
        //    tblChat.Visible = false;
        //}
        //else
        //{
        //    tblLogin.Visible = false;
        //    tblChat.Visible = true;
        //    hdnchatclientid.Text = GlobalUtilities.ConvertToString(Session["S_ClientChatId"]);
        //}
    }
    private void BindChat()
    {
        int chatId = GlobalUtilities.ConvertToInt(Session["S_ChatId"]);
        hdnchatid.Text = chatId.ToString();
        hdnname.Text = Convert.ToString(Session["S_ClientName"]);
        int clientId = GlobalUtilities.ConvertToInt(Session["S_ChatClientId"]);
        hdnchatclientid.Text = clientId.ToString();

        string query = "select * from tbl_chat where chat_chatid=" + chatId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        hdnagentid.Text = GlobalUtilities.ConvertToInt(dr["chat_agentid"]).ToString();
        StringBuilder html = new StringBuilder();
        DataTable dttbl = GetFullChatDetail(chatId);
        if (!GlobalUtilities.IsValidaTable(dttbl))
        {
            string welcomeMessage = Common.GetSetting("Chat Welcome Message");
            html.Append("<tr><td class='chat-welcome-msg'>" + welcomeMessage + "</td></tr>");
        }
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            html.Append("<tr><td><b>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["name"]) + "</b> : " +
                        GlobalUtilities.ConvertToString(dttbl.Rows[i]["message"]) + "</td></tr>");
        }
        ltChatHistory.Text = html.ToString();
    }
    private DataTable GetFullChatDetail(int chatId)
    {
        if (chatId == 0) return new DataTable();
        string query = "select chatdetail_name as name,chatdetail_message as message from tbl_chatdetail " +
                       "where chatdetail_chatid=" + chatId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        return dttbl;
    }
}
