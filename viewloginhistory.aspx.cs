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

public partial class viewloginhistory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }
    private void BindData()
    {
        string query = @"select top 1000 * from tbl_loginhistory
                        where loginhistory_applicationtypeid=1
                        and loginhistory_clientid=" + Common.ClientId + " and loginhistory_clientuserid=" + Common.ClientUserId +
                        " order by loginhistory_loginhistoryid desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table cellpadding=7 class='grid-ui' border=1>");
        html.Append(@"<tr class='grid-ui-header'><td>Login Time</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string css = "grid-ui-row-alt";
            if (i % 2 == 0)
            {
                css = "grid-ui-row";
            }
            html.Append("<tr class='" + css + "'>");
            html.Append("<td>" + GlobalUtilities.ConvertToDateTime(dttbl.Rows[i]["loginhistory_logintime"]) + "</td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
}
