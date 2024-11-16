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

public partial class chat_agent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CustomSession.Session("AgentId") == "")
        {
            StartAgent();
        }
        BindAgentDetail();
        if (!IsPostBack)
        {
            BindChat();
        }
    }
    private void StartAgent()
    {
        int userId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId"));
        if (userId == 0)
        {
            Response.Redirect("~/adminlogin.aspx");
        }
        string query = "select * from tbl_user " +
                       "JOIN tbl_agent ON agent_agentid=user_agentid " +
                       "where user_userid=" + userId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            Session["S_Error"] = "You are not an agent to start the chat!";
            Response.Redirect("~/error.aspx");
        }
        int agentId = GlobalUtilities.ConvertToInt(dr["agent_agentid"]);
        string agentName = GlobalUtilities.ConvertToString(dr["agent_agentname"]);
        //update agent status
        query = "update tbl_agent set agent_onlinestatusid=1,agent_lastupdate=getdate() where agent_agentid=" + agentId;
        DbTable.ExecuteQuery(query);

        CustomSession.Session("AgentId", agentId);
        CustomSession.Session("AgentName", agentName);
        Response.Redirect("~/chat/chat-agent.aspx");
    }
    private void BindAgentDetail()
    {
        int userId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId"));
        if (userId == 0)
        {
            Response.Redirect("~/adminlogin.aspx");
        }
        string agentName = CustomSession.Session("AgentName");
        int agentId = GlobalUtilities.ConvertToInt(CustomSession.Session("AgentId"));
        lblAgentName.Text = "Welcome " + agentName;
        hdnagentid.Text = agentId.ToString();
        hdnname.Text = agentName;
    }

    private void BindChat()
    {
        int agentId = GlobalUtilities.ConvertToInt(CustomSession.Session("AgentId"));
        string query = "select * from tbl_agent WHERE agent_agentid=" + agentId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return;

        query = "select chat_chatid as chatid,chat_city as city,chat_country as country,chat_longitude as longitude,chat_latitude as latitude," +
                        "chat_ip as ip,chat_clientname as clientname,chat_chatclientid as chatclientid,chat_companyname as companyname " +
                        "from tbl_chat WHERE chat_chatstatusid=2 AND chat_agentid=" + agentId;

        DataTable dttblOnline = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        int chatId = 0;
        int agentChatId = GlobalUtilities.ConvertToInt(dr["agent_chatid"]);
        if (!GlobalUtilities.IsValidaTable(dttblOnline))
        {
            agentChatId = 0;
        }
        for (int i = 0; i < dttblOnline.Rows.Count; i++)
        {
            string clientName = GlobalUtilities.ConvertToString(dttblOnline.Rows[i]["clientname"]);
            string city = GlobalUtilities.ConvertToString(dttblOnline.Rows[i]["city"]);
            string ip = GlobalUtilities.ConvertToString(dttblOnline.Rows[i]["ip"]);
            chatId = GlobalUtilities.ConvertToInt(dttblOnline.Rows[i]["chatid"]);
            int chatclientid = GlobalUtilities.ConvertToInt(dttblOnline.Rows[i]["chatclientid"]);
            string activeCss = "";
            if (chatId == agentChatId)
            {
                activeCss = " chatlist-row-hover";
            }
            html.Append("<tr cid='" + chatId + "' ccid='" + chatclientid + "' class='row chatingclient'><td class='chatlist-row" + activeCss + "'>" +
                         "<table><tr><td class='bold'>" + clientName + "</td><td class='tdcount'></td></tr>" +
                                 "<tr><td class='city'>" + city + "</td></tr>" +
                                 "<tr><td class='ip'>" + ip + "</td></tr>" +
                         "</table>" +
                         "</td></tr>");

        }
        ltChatList.Text = html.ToString();
     
        //bind 
        hdnchatid.Text = agentChatId.ToString();
        html = new StringBuilder();
        if (agentChatId > 0)
        {
            DataTable dttblChatHistory = GetFullChatDetail(agentChatId);
            
            for (int i = 0; i < dttblChatHistory.Rows.Count; i++)
            {
                html.Append("<tr><td><b>" + GlobalUtilities.ConvertToString(dttblChatHistory.Rows[i]["name"]) + "</b> : " +
                            GlobalUtilities.ConvertToString(dttblChatHistory.Rows[i]["message"]) + "</td></tr>");
            }
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
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/logout.aspx");
    }
}
