<%@ WebHandler Language="C#" Class="broadcast" %>

using System;
using System.Web;
using WebComponent;
using System.Data;
using System.Text;

public class broadcast : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        int userId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
        string m = Common.GetQueryString("m");
        if (m == "broadcast")
        {
            string query = "select clientuser_lastbroadcastmessageid from tbl_clientuser where clientuser_clientuserid=" + userId;
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (dr == null) return;
            int lastBroadcastMsgId = GlobalUtilities.ConvertToInt(dr["clientuser_lastbroadcastmessageid"]);
            query = "select top 10 * from tbl_broadcastmessage where broadcastmessage_broadcastmessageid>" + lastBroadcastMsgId+
                    " order by broadcastmessage_broadcastmessageid desc";
            DataTable dttbl = DbTable.ExecuteSelect(query);
            if (!GlobalUtilities.IsValidaTable(dttbl)) return;
            StringBuilder html = new StringBuilder();
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                if (i == 0)
                {
                    query = "update tbl_clientuser set clientuser_lastbroadcastmessageid=" + GlobalUtilities.ConvertToInt(dttbl.Rows[i]["broadcastmessage_broadcastmessageid"]) +
                            " where clientuser_clientuserid=" + userId;
                    DbTable.ExecuteQuery(query);
                }
                html.Append("<div class='broadcast-detail'><div>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["broadcastmessage_message"]) + "</div>"+
                                "<div style='color:#aaa;font-size:12px;float:right;'>" + GlobalUtilities.ConvertToDateTime(dttbl.Rows[i]["broadcastmessage_createddate"]) + "</div>" +
                            "</div>");
            }
            context.Response.Write(html.ToString());
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}