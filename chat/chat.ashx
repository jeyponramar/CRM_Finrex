<%@ WebHandler Language="C#" Class="chat" %>

using System;
using System.Web;
using WebComponent;
using System.Data;
using System.Collections;
using System.Text;

public class chat : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        //Common.ValidateAjaxRequest();    
        context.Response.ContentType = "application/text";
        try
        {
            string action = context.Request.QueryString["a"];

            if (action == "n")//new chat
            {
                SaveNewChat();
            }
            else if (action == "s")//send message from client
            {
                int id = SendMessage();
                if (id > 0)
                {
                    context.Response.Write(id.ToString());
                }
            }
            else if (action == "ra")//receive message for agent
            {
                ReceiveMessage_Agent();
            }
            else if (action == "rc")//receive message for client
            {
                ReceiveMessage_Client();
                if (GlobalUtilities.ConvertToBool(context.Request.QueryString["hb"]))
                {
                    UpdateHeartBeat();
                }
            }
            else if (action == "addchat")
            {
                //calls when agent add the client to chat list
                AddChat();
            }
            else if (action == "getchat")
            {
                //get full history
                GetChatdetailForAgentByChatId();
            }
            else if (action == "closechat")
            {
                CloseChat();
            }
            else if (action == "getproduct")
            {
                GetProductDetail();
            }
            else if (action == "heartbeat")
            {
                UpdateHeartBeat();
            }
            else if (action == "heartbeat-agent")
            {
                UpdateHeartBeatOfAgent();
            }
            else if (action == "online-agent")
            {
                GetOnlineAgents();
            }
            else if (action == "forward")
            {
                ForwardChat();
            }
        }
        catch(Exception ex)
        {
            context.Response.Clear();
            context.Response.Write("");
            context.Response.End();
        }
    }
    private void UpdateHeartBeat()
    {
        int chatId = Common.GetQueryStringValue("id");
        string query = "update tbl_chat set chat_lastclientupdate=getdate() where chat_chatid="+chatId;
        DbTable.ExecuteQuery(query);
    }
    private void UpdateHeartBeatOfAgent()
    {
        int agentId = Common.GetQueryStringValue("aid");
        string query = "update tbl_agent set agent_lastupdate=getdate() where agent_agentid=" + agentId;
        DbTable.ExecuteQuery(query);

        query = "update tbl_agent set agent_onlinestatusid=2 where datediff(d,agent_lastupdate,getdate()) > 0 or datediff(s,agent_lastupdate,getdate()) > 60";
        DbTable.ExecuteQuery(query);
        
    }
    private void GetChatdetailForAgentByChatId()
    {
        int chatId = Common.GetQueryStringValue("id");
        DataTable dttbl = GetFullChatDetail(chatId);
        string existingChat = "{\"chat\":" + SetJson(dttbl) + "}";
        HttpContext.Current.Response.Write(existingChat);

        string query = "update tbl_chatdetail set chatdetail_isagentread=1 where chatdetail_chatid=" + chatId;
        DbTable.ExecuteQuery(query);
    }
    private DataTable GetChatDetailForAgent(int chatId)
    {
        if (chatId == 0) return new DataTable();
        string query = "select chatdetail_name as name,chatdetail_message as message from tbl_chatdetail " +
                       "where chatdetail_chatid=" + chatId +
                       " AND chatdetail_isagentread = 0";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        return dttbl;
    }
    private DataTable GetChatDetailForClient(int chatId)
    {
        if (chatId == 0) return new DataTable();
        string query = "select chatdetail_name as name,chatdetail_message as message from tbl_chatdetail " +
                       "where chatdetail_chatid=" + chatId +
                       " AND chatdetail_isclientread = 0";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        return dttbl;
    }
    private DataTable GetFullChatDetail(int chatId)
    {
        if (chatId == 0) return new DataTable();
        string query = "select chatdetail_name as name,chatdetail_message as message from tbl_chatdetail " +
                       "where chatdetail_chatid=" + chatId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        return dttbl;
    }
    private void SaveNewChat()
    {
        int toUserId = GlobalUtilities.ConvertToInt(Common.GetQueryStringValue("uid"));
        int fromUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId"));
        if (fromUserId == 0)
        {
            HttpContext.Current.Response.Write("Session Expired");
        }
        else
        {
            Hashtable hstbl = new Hashtable();
            hstbl.Add("fromuserid", fromUserId);
            hstbl.Add("touserid", toUserId);
            InsertUpdate obj = new InsertUpdate();
            int id = obj.InsertData(hstbl, "tbl_chat", false);
            HttpContext.Current.Response.Write(id);
        }
    }
    private int SendMessage()
    {
        int chatid = GlobalUtilities.ConvertToInt(HttpContext.Current.Request.Form["hdnchatid"]);
        int agentid = GlobalUtilities.ConvertToInt(HttpContext.Current.Request.Form["hdnagentid"]);
        int chatclientid = GlobalUtilities.ConvertToInt(HttpContext.Current.Request.Form["hdnchatclientid"]);
        string message = HttpContext.Current.Request.Form["txtmessage"];
        string ut = HttpContext.Current.Request.QueryString["ut"];
        string name = GlobalUtilities.ConvertToString(HttpContext.Current.Request.Form["hdnname"]);
        message = message.Replace("../upload/chat/temp", Common.GetMainURL() + "/" + "upload/chat/temp"); 
        //if (ut == "c")
        //{
        //    name = GlobalUtilities.ConvertToString(DbTable.GetOneColumnData("tbl_chatclient", "chatclient_name", chatclientid));
        //}
        //else
        //{
        //    name = GlobalUtilities.ConvertToString(DbTable.GetOneColumnData("tbl_agent", "agent_agentname", agentid));
        //}
        if (ut == "c")
        {
        }
        else
        {
            //agent
            string query = "select datediff(s,chat_lastclientupdate,getdate()) as diff from tbl_chat where chat_chatid=" + chatid;
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (dr != null)
            {
                int diff = GlobalUtilities.ConvertToInt(dr["diff"]);
                if (diff > 20)
                {
                    //update the chat status to closed
                    query = "update tbl_chat set chat_chatstatusid=3 WHERE chat_chatid=" + chatid;
                    DbTable.ExecuteQuery(query);
                    
                    HttpContext.Current.Response.Write("ClientClosed");
                    return 0;
                }
            }
        }
        Hashtable hstbl = new Hashtable();
        hstbl.Add("chatid", chatid);
        hstbl.Add("agentid", agentid);
        hstbl.Add("chatclientid", chatclientid);
        if (ut == "c")
        {
            hstbl.Add("isclientread", 1);
            hstbl.Add("isagentread", 0);
        }
        else
        {
            hstbl.Add("isclientread", 0);
            hstbl.Add("isagentread", 1);
        }
        hstbl.Add("name", name);
        hstbl.Add("message", message);
        InsertUpdate obj = new InsertUpdate();
        int id = obj.InsertData(hstbl, "tbl_chatdetail");
        return id;
    }
    private void ReceiveMessage_Agent()
    {
        int chatId = GlobalUtilities.ConvertToInt(HttpContext.Current.Request.QueryString["id"]);
        string query = "";

        DataTable dttbl = GetChatDetailForAgent(chatId);
        StringBuilder data = new StringBuilder();
        data.Append("{\"chat\":");
        if (GlobalUtilities.IsValidaTable(dttbl))
        {
            query = "update tbl_chatdetail set chatdetail_isagentread = 1 where chatdetail_chatid=" + chatId;
            DbTable.ExecuteQuery(query);

        }
        data.Append(SetJson(dttbl));

        //get new clients
        query = "select chat_chatid as chatid,chat_city as city,chat_country as country,chat_longitude as longitude,chat_latitude as latitude," +
                "chat_ip as ip,client_customername as clientname,chat_chatclientid as chatclientid,clientuser_name as contactperson, " +
                "client_subscriptionstatusid AS subscriptionstatusid " +
                "from tbl_chat "+
                "JOIN tbl_clientuser ON clientuser_clientuserid=chat_clientuserid "+
                "JOIN tbl_client ON client_clientid=chat_clientid "+
                "WHERE chat_chatstatusid=1 AND chat_agentid=0";
        DataTable dttblNewChat = DbTable.ExecuteSelect(query);
        data.Append(",\"newchat\":" + SetJson(dttblNewChat));
        
        //get chating client message count
        int agentId = Common.GetQueryStringValue("aid");
        DataTable dttblChatCount = GetChatClientWithCount(agentId);
        data.Append(",\"chatcount\":" + SetJson(dttblChatCount));
        
        data.Append("}");
        HttpContext.Current.Response.Write(data.ToString());
    }
    private DataTable GetChatClientWithCount(int agentId)
    {
        StringBuilder data = new StringBuilder();
        string query = "select (select count(*) from tbl_chatdetail WHERE chatdetail_chatid=chat_chatid AND chatdetail_isagentread=0) as msgcount," +
                        "chat_chatid as chatid,DATEDIFF(s,chat_lastclientupdate,getdate()) as diff,chat_clientname as clientname," +
                        "chat_chatclientid as chatclientid " +
                        "FROM tbl_chat " +
                        "WHERE chat_chatstatusid=2 AND chat_agentid=" + agentId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        //for (int i = 0; i < dttbl.Rows.Count; i++)
        //{
        //    int diff = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["diff"]);
        //    if (diff >= 20)
        //    {
        //        int chatId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["chatid"]);
        //        query = "update tbl_chat set chat_chatstatusid=3 WHERE chat_chatid=" + chatId;
        //        DbTable.ExecuteQuery(query);
        //    }
        //}
        return dttbl;
    }
    private void ReceiveMessage_Client()
    {
        int clientId = GlobalUtilities.ConvertToInt(HttpContext.Current.Request.QueryString["cid"]);
        int chatId = GlobalUtilities.ConvertToInt(HttpContext.Current.Request.QueryString["id"]);
        string query = "";
        
        DataTable dttbl = GetChatDetailForClient(chatId);
        StringBuilder data = new StringBuilder();
        data.Append("{\"chat\":");
        if (GlobalUtilities.IsValidaTable(dttbl))
        {
            query = "update tbl_chatdetail set chatdetail_isclientread = 1 where chatdetail_chatid=" + chatId;
            DbTable.ExecuteQuery(query);

        }
        data.Append(SetJson(dttbl));
        data.Append("}");
        HttpContext.Current.Response.Write(data.ToString());
    }
    private void AddChat()
    {
        int chatId = Common.GetQueryStringValue("id");
        int agentId = Common.GetQueryStringValue("aid");

        string query = "select datediff(s,chat_lastclientupdate,getdate()) as diff from tbl_chat where chat_chatid=" + chatId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            int diff = GlobalUtilities.ConvertToInt(dr["diff"]);
            if (diff > 20)
            {
                //update the chat status to closed
                query = "update tbl_chat set chat_chatstatusid=3 WHERE chat_chatid=" + chatId;
                DbTable.ExecuteQuery(query);

                HttpContext.Current.Response.Write("ClientClosed");
                return;
            }
        }
        
        //assign agent & change status to chatting
        query = "update tbl_chat set chat_agentid=" + agentId + ",chat_chatstatusid=2 WHERE chat_chatid=" + chatId;
        bool idupdated = DbTable.ExecuteQuery(query);

        //get old chat history
        StringBuilder data = new StringBuilder();
        DataTable dttbl = GetFullChatDetail(chatId);
        string existingChat = "{\"chat\":" + SetJson(dttbl) + "}";

        //update as read
        query = "update tbl_chatdetail set chatdetail_isagentread = 1 where chatdetail_chatid=" + chatId;
        DbTable.ExecuteQuery(query);
        
        //set chat id to agent to know current chat
        query = "update tbl_agent set agent_chatid=" + chatId + " where agent_agentid=" + agentId;
        DbTable.ExecuteQuery(query);

        //add welcome message
        string welcomeMessage = Common.GetSetting("Start Chat Message");
        if (welcomeMessage != "")
        {
            query = "select * from tbl_chat where chat_chatid=" + chatId;
            DataRow drChat = DbTable.ExecuteSelectRow(query);
            if (drChat != null)
            {
                int chatclientid = GlobalUtilities.ConvertToInt(drChat["chat_chatclientid"]);
                string clientName = GlobalUtilities.ConvertToString(drChat["chat_clientname"]);
                string agentName = "";
                query = "select * from tbl_agent where agent_agentid=" + agentId;
                DataRow drAgent = DbTable.ExecuteSelectRow(query);
                if (drAgent != null)
                {
                    agentName = GlobalUtilities.ConvertToString(drAgent["agent_agentname"]);
                }
                welcomeMessage = welcomeMessage.Replace("$chat_clientname$", clientName);
                welcomeMessage = welcomeMessage.Replace("$agent_agentname$", agentName);
                
                Hashtable hstbl = new Hashtable();
                hstbl.Add("chatid", chatId);
                hstbl.Add("agentid", agentId);
                hstbl.Add("chatclientid", chatclientid);
                hstbl.Add("isclientread", 0);
                hstbl.Add("isagentread", 0);
                hstbl.Add("name", agentName);
                hstbl.Add("message", welcomeMessage);
                InsertUpdate obj = new InsertUpdate();
                int id = obj.InsertData(hstbl, "tbl_chatdetail");
            }
        }
        
        HttpContext.Current.Response.Write(existingChat);
    }
    private void GetOnlineAgents()
    {
        int agentId = Common.GetQueryStringValue("aid");
        if (agentId == 0) return;
        string query = "select agent_agentid as agentid,agent_agentname as agentname from tbl_agent " +
                     "where agent_onlinestatusid=1 AND agent_agentid<>" + agentId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        HttpContext.Current.Response.Write(SetJson(dttbl));
    }
    private void CloseChat()
    {
        int id = Common.GetQueryStringValue("id");
        string query = "update tbl_chat set chat_chatstatusid=3 where chat_chatid=" + id;
        bool result = DbTable.ExecuteQuery(query);
        if (result)
        {
            SendChatHistory(id);
            HttpContext.Current.Response.Write("1");    
        }
    }
    private void GetProductDetail()
    {
        int productId = Common.GetQueryStringValue("id");
        string query = "select * from tbl_product where product_productid=" + productId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            string productName = GlobalUtilities.ConvertToString(dr["product_productname"]);
            string description = GlobalUtilities.ConvertToString(dr["product_description"]);
            string attachment = GlobalUtilities.ConvertToString(dr["product_attachment"]);
            string data = "<b>" + productName + " : </b><br/>" + description + "<br/>";
            string url = Common.GetMainURL() + "/upload/product/" + productId + ".jpg";
            data += "<img src='" + url + "' height='200'/>";
            HttpContext.Current.Response.Write(data);
        }
    }
    private void ForwardChat()
    {
        int chatId = Common.GetQueryStringValue("id");
        int agentId = Common.GetQueryStringValue("aid");
        int toAgentId = Common.GetQueryStringValue("taid");
        string query = "update tbl_chat set chat_agentid=" + toAgentId + " where chat_chatid=" + chatId;
        bool issuccess = DbTable.ExecuteQuery(query);
        if (issuccess)
        {
            HttpContext.Current.Response.Write("1");
        }
    }   
    private StringBuilder SetJson(DataTable dttbl)
    {
        StringBuilder json = new StringBuilder();
        json.Append("[");
        if (dttbl.Rows.Count > 0)
        {
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                json.Append("{");
                for (int j = 0; j < dttbl.Columns.Count; j++)
                {
                    string val = Convert.ToString(dttbl.Rows[i][j]).Replace("\r\n", "<br/>").Replace("\"","");
                    json.Append("\"" + dttbl.Columns[j].ColumnName + "\": \"" + val + "\"");
                    if (j != dttbl.Columns.Count - 1)
                    {
                        json.Append(",");
                    }
                }
                json.Append("}");
                if (i != dttbl.Rows.Count - 1)
                {
                    json.Append(",");
                }
            }
            
        }
        json.Append("]");
        return json;
    }
    private void SendChatHistory(int chatId)
    {
        string query = "select * from tbl_chatdetail " +
                       "where chatdetail_chatid=" + chatId + " order by chatdetail_chatdetailid";
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

    public bool IsReusable {
        get {
            return false;
        }
    }

}