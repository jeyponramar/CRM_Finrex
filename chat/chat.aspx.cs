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
using System.Net;
using System.Xml;

public partial class chat_chat : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if chat already started then go to chat page
            if (Session["S_ChatId"] != null)
            {
                Response.Redirect("~/chat/chat-client.aspx");
            }
            if (AgentAvailable())
            {
                CreateNewChat();
            }
        }
    }
    private void CreateNewChat()
    {
        string name = "";// global.CheckInputData(Request.QueryString["name"]);
        string emailId = "";// global.CheckInputData(Request.QueryString["emailid"]);
        int clientUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
        if (clientUserId == 0)
        {
            Session["S_Error"] = "Session expired, please login and try again!";
            Response.Redirect("error.aspx");
            return;
        }
        string query = "select * from tbl_chatclient " +
                       "where chatclient_clientuserid='" + clientUserId + "'";
        DataRow dr = DbTable.ExecuteSelectRow(query);

        int chatClientId = 0;
        query = "select * from tbl_clientuser join tbl_client on client_clientid=clientuser_clientid where clientuser_clientuserid=" + clientUserId;
        DataRow drClient = DbTable.ExecuteSelectRow(query);
        string cname = GlobalUtilities.ConvertToString(drClient["clientuser_name"]);
        string cemailid = GlobalUtilities.ConvertToString(drClient["clientuser_username"]);
        int clientId = GlobalUtilities.ConvertToInt(drClient["clientuser_clientid"]);
        string customerName = GlobalUtilities.ConvertToString(drClient["client_customername"]);
        if (dr == null)
        {
            Hashtable hstblClient = new Hashtable();
            hstblClient.Add("name", cname);
            hstblClient.Add("emailid", cemailid);
            hstblClient.Add("clientid", clientId);
            hstblClient.Add("clientuserid", clientUserId);
            InsertUpdate obj1 = new InsertUpdate();
            chatClientId = obj1.InsertData(hstblClient, "tbl_chatclient");
        }
        else
        {
            chatClientId = GlobalUtilities.ConvertToInt(dr["chatclient_chatclientid"]);
            if (GlobalUtilities.ConvertToBool(dr["chatclient_isblocked"]))
            {
                return;
            }
        }
        //update client chat status
        query = "update tbl_chat set chat_chatstatusid=3 where chat_chatclientid=" + chatClientId;
        DbTable.ExecuteQuery(query);

        string IP = "";
        string city ="";

        try
        {
            //IP = "1.39.11.164";
            IP = Server.HtmlEncode(Request.UserHostAddress);
            XmlDocument doc = new XmlDocument();
            string getdetails = "http://www.freegeoip.net/xml/" + IP;
            doc.Load(getdetails);
            XmlNodeList nodeLstCity = doc.GetElementsByTagName("City");
            city = nodeLstCity[0].InnerText;
        }
        catch { }

        Hashtable hstbl = new Hashtable();
        hstbl.Add("chatclientid", chatClientId);
        hstbl.Add("clientid", clientId);
        hstbl.Add("clientuserid", clientUserId);
        hstbl.Add("date", "getdate()");
        hstbl.Add("isactive", "1");
        hstbl.Add("ip", IP);
        hstbl.Add("clientname", cname);
        hstbl.Add("chatstatusid", 1);
        hstbl.Add("lastclientupdate", "getdate()");
        hstbl.Add("city", city);
        hstbl.Add("agentid", "0");

        InsertUpdate obj = new InsertUpdate();
        int chatId = obj.InsertData(hstbl, "tbl_chat");
        Session["S_ChatId"] = chatId;
        Session["S_ChatClientId"] = chatClientId;
        Session["S_ClientName"] = cname;
        Response.Redirect("~/chat/chat-client.aspx");
    }
    private bool AgentAvailable()
    {
        //logout agents who are not online
        //string query = "update tbl_agent set agent_onlinestatusid=2 where datediff(s,agent_lastupdate,getdate()) > 60";
        //DbTable.ExecuteQuery(query);

        string query = "select count(*) as c from tbl_agent where agent_onlinestatusid=1";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        bool isavailable = false;
        if (dr != null)
        {
            if (GlobalUtilities.ConvertToInt(dr["c"]) > 0)
            {
                isavailable = true;
            }
        }
        if (isavailable == false)
        {
            string noAgentMessage = Common.GetSetting("No Agent Available Message");
            lblMessage.Text = noAgentMessage;
        }
        return isavailable;
    }
}
