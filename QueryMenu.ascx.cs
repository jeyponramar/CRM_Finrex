using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using WebComponent;

public partial class QueryMenu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindMenu();
        }
    }
    private void BindMenu()
    {
        StringBuilder html = new StringBuilder();
        int type = Common.GetQueryStringValue("t");
        if (type == 0) type = 1;
        int clientUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
        string query = @"select count(*) as total, sum(case when customerquery_querystatusid = 1 then 1 else 0 end) as total_open,
                            sum(case when customerquery_querystatusid = 2 then 1 else 0 end) as total_advreplied,
                            sum(case when customerquery_querystatusid = 3 then 1 else 0 end) as total_custreplied,
                            sum(case when customerquery_querystatusid = 5 then 1 else 0 end) as total_closed
                            from tbl_customerquery where customerquery_clientuserid=" + clientUserId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        int total = GlobalUtilities.ConvertToInt(dr["total"]);
        int total_open = GlobalUtilities.ConvertToInt(dr["total_open"]);
        int total_advreplied = GlobalUtilities.ConvertToInt(dr["total_advreplied"]);
        int total_custreplied = GlobalUtilities.ConvertToInt(dr["total_custreplied"]);
        int total_closed = GlobalUtilities.ConvertToInt(dr["total_closed"]);
        html.Append("<table width='100%' cellpadding='10' cellspacing=0>");
        html.Append("<tr><td class='left-menu-title' colspan='3'>View</td></tr>");
        html.Append(@"<tr class='left-menu" + (type == 1 ? " left-menu-active" : "") + @"'>
                        <td><a href='viewcustomerquery.aspx?t=1'>View all Queries</a><span>" +total+@"</span></td>
                    </tr>");
        html.Append(@"<tr class='left-menu" + (type == 2 ? " left-menu-active" : "") + @"'>
                        <td><a href='viewcustomerquery.aspx?t=2'>Advisor Replied</a><span>" + total_advreplied + @"</span></td>
                    </tr>");
        html.Append(@"<tr class='left-menu" + (type == 3 ? " left-menu-active" : "") + @"'>
                        <td><a href='viewcustomerquery.aspx?t=3'>You Replied</a><span>" + total_custreplied + @"</span></td>
                    </tr>");
        html.Append(@"<tr class='left-menu" + (type == 4 ? " left-menu-active" : "") + @"'>
                        <td><a href='viewcustomerquery.aspx?t=4'>Open Queries</a><span>" + total_open + @"</span></td>
                    </tr>");
        html.Append(@"<tr class='left-menu" + (type == 5 ? " left-menu-active" : "") + @"'>
                        <td><a href='viewcustomerquery.aspx?t=5'>Closed Queries</a><span>" + total_closed + @"</span></td>
                    </tr>");
        html.Append("</table>");
        ltmenu.Text = html.ToString();
    }
}
