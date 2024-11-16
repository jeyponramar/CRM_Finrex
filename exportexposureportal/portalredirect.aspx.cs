using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;

public partial class exportexposureportal_portalredirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sessionId = Common.GetQueryString("sid");
        string query = "select * from tbl_tempsession where tempsession_sessionid='" + global.CheckInputData(sessionId) + "'";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            Response.Redirect("~/customerlogin.aspx");
            return;
        }
        string session = GlobalUtilities.ConvertToString(dr["tempsession_session"]);
        Array arr = session.Split('~');
        int clientUserId = 0;
        for (int i = 0; i < arr.Length; i += 2)
        {
            if(arr.GetValue(i).ToString()=="cuserid")
            {
                clientUserId = GlobalUtilities.ConvertToInt(arr.GetValue(i+1));
            }
        }
        query = "select * from tbl_clientuser where clientuser_clientuserid="+clientUserId;
        DataRow drc = DbTable.ExecuteSelectRow(query);
        int clientId = GlobalUtilities.ConvertToInt(drc["clientuser_clientid"]);
        CustomSession.Session("Login_IsLoggedIn", true);
        CustomSession.Session("Login_ClientUserId", clientUserId);
        CustomSession.Session("Login_ClientId", clientId);

        query = "delete from tbl_tempsession where tempsession_sessionid='" + global.CheckInputData(sessionId) + "'";
        DbTable.ExecuteQuery(query);

        query = "select * from tbl_clientuser where clientuser_clientuserid=" + clientUserId;
        DataRow druser = DbTable.ExecuteSelectRow(query);
        if (druser["clientuser_isportalfirstlogin"] == DBNull.Value)
        {
            Response.Redirect("~/exportexposureportal/terms.aspx?pt=" + Request.QueryString["pt"]);
        }
        else
        {
            Response.Redirect("~/exportexposureportal/portal.aspx?pt="+Request.QueryString["pt"]);
        }
    }
}
