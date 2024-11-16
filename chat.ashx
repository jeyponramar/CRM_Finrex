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
        Common.ValidateAjaxRequest();    
        context.Response.ContentType = "application/json";
        
        if (context.Request.QueryString["a"] == "n")//new chat
        {
            int toUserId = GlobalUtilities.ConvertToInt(context.Request.QueryString["uid"]);
            int fromUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId"));
            if (fromUserId == 0)
            {
                context.Response.Write("session expired");
            }
            else
            {
                Hashtable hstbl = new Hashtable();
                hstbl.Add("fromuserid", fromUserId);
                hstbl.Add("touserid", toUserId);
                InsertUpdate obj = new InsertUpdate();
                int id = obj.InsertData(hstbl, "tbl_chat",false);
                context.Response.Write(id);
            }
        }
        else if (context.Request.QueryString["a"] == "s")//send message
        {
            int toUserId = GlobalUtilities.ConvertToInt(context.Request.QueryString["uid"]);
            int cid = GlobalUtilities.ConvertToInt(context.Request.QueryString["cid"]);
            string msg = Convert.ToString(context.Request.QueryString["msg"]);
            int fromUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId"));
            
            Hashtable hstbl = new Hashtable();
            hstbl.Add("chatid", cid);
            hstbl.Add("fromuserid", fromUserId);
            hstbl.Add("touserid", toUserId);
            hstbl.Add("message", msg);
            InsertUpdate obj = new InsertUpdate();
            int id = obj.InsertData(hstbl, "tbl_chatdetail", false);
            context.Response.Write("1");
        }
        else if (context.Request.QueryString["a"] == "r")//receive message
        {
            int userid = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId"));
            string query = "select chatdetail_chatdetailid,chatdetail_chatid,user_fullname,chatdetail_message,chatdetail_fromuserid from tbl_chatdetail " +
                            "join tbl_user on user_userid=chatdetail_fromuserid "+
                            "where chatdetail_isread=0 and chatdetail_touserid=" + userid;
            InsertUpdate obj = new InsertUpdate();
            DataTable dttbl = new DataTable();
            dttbl = obj.ExecuteSelect(query);
            bool isgetstatus = false;
            if (context.Request.QueryString["s"] == "1")//get status
            {
                isgetstatus = true;
                //query="select user_userid,user_fullname from 
            }
            if (dttbl.Rows.Count > 0)
            {
                StringBuilder chatids = new StringBuilder();
                for (int i = 0; i < dttbl.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        chatids.Append(Convert.ToString(dttbl.Rows[i]["chatdetail_chatdetailid"]));
                    }
                    else
                    {
                        chatids.Append("," + Convert.ToString(dttbl.Rows[i]["chatdetail_chatdetailid"]));
                    }
                }
                query = "update tbl_chatdetail set chatdetail_isread = 1 where chatdetail_chatdetailid in(" + chatids.ToString() + ")";
                InsertUpdate objupdate = new InsertUpdate();
                objupdate.ExecuteQuery(query);
            }
            
            StringBuilder json = new StringBuilder();
            json = SetJson(dttbl);
            context.Response.Write(json);
        }
    }
    private StringBuilder SetJson(DataTable dttbl)
    {
        StringBuilder json = new StringBuilder();
        if (dttbl.Rows.Count > 0)
        {
            if (json.ToString() == "")
            {
                json.Append("[");
            }
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                json.Append("{");
                for (int j = 0; j < dttbl.Columns.Count; j++)
                {
                    string val = Convert.ToString(dttbl.Rows[i][j]).Replace("\r\n", "<br/>");
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
            if (json.ToString() != "")
            {
                json.Append("]");
            }
        }
        return json;
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}