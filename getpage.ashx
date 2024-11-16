<%@ WebHandler Language="C#" Class="getpage" %>

using System;
using System.Web;
using WebComponent;
using System.Data;
using System.Text;

public class getpage : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        Common.ValidateAjaxRequest();
        context.Response.ContentType = "text/plain";
        string m = context.Request.QueryString["m"];

        StringBuilder html = new StringBuilder();
        if (m == "getproductsizecolor")
        {
            int sscid = GlobalUtilities.ConvertToInt(context.Request.QueryString["sscid"]);
            string query = "select * from tbl_size WHERE size_subsubcategoryid=" + sscid;
            DataTable dttbl = DbTable.ExecuteSelect(query);
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                string size = GlobalUtilities.ConvertToString(dttbl.Rows[i]["size_size"]);
                if (i == 0)
                {
                    html.Append(size);
                }
                else
                {
                    html.Append("~" + size);
                }
            }
            html.Append("`");
            query = "select * from tbl_dbconstants WHERE dbconstants_name='CommaSepColor'";
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (dr != null)
            {
                string val = GlobalUtilities.ConvertToString(dr["dbconstants_value"]);
                html.Append(val);
            }
        }
        else if (m == "staticdialog")
        {
            int id = GlobalUtilities.ConvertToInt(context.Request.QueryString["id"]);
            string query = "select * from tbl_staticdialogpage WHERE staticdialogpage_staticdialogpageid=" + id;
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (dr != null)
            {
                html.Append(GlobalUtilities.ConvertToString(dr["staticdialogpage_html"]));
            }
        }
        context.Response.Write(html.ToString());
    }
    private DataTable GetNotifications()
    {
        int loggedinUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId"));
        string query = "select notification_notificationid,notification_fromuserid,notification_modulename,notification_moduleid,notification_message "+
                       "from tbl_notification where notification_touserid=" + loggedinUserId +
                       " order by notification_notificationid desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        return dttbl;
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}