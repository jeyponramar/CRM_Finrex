using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WebComponent;
using System.Collections;

/// <summary>
/// Summary description for FinstationExe
/// </summary>
public class FinstationExe
{
    public static int ClientUserId
    {
        get
        {
            string query = "";
            string sessionId = SessionId;
            query = "select * from tbl_clientuser where clientuser_exesessionid=@sessionid";
            Hashtable hstbl = new Hashtable();
            hstbl.Add("sessionid", sessionId);
            DataRow dr = DbTable.ExecuteSelectRow(query, hstbl);
            if (dr == null) return 0;
            return Convert.ToInt32(dr["clientuser_clientuserid"]);
        }
    }
    public static string SessionId
    {
        get
        {
            return Common.GetQueryString("sid");
        }
    }
}
