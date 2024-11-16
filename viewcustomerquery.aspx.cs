using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;
using System.Collections;

public partial class viewcustomerquery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Custom.CheckSubscriptionAccess();
        Finstation.CheckFullFinstationAccess();
        if (!IsPostBack)
        {
            BindQuery();
        }
    }
    private void BindQuery()
    {
        string query = "";
        int clientUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
        query = @"select * from tbl_customerquery
                  join tbl_querytopic on querytopic_querytopicid=customerquery_querytopicid
                  join tbl_querytype on querytype_querytypeid=querytopic_querytypeid
                  join tbl_querystatus on querystatus_querystatusid=customerquery_querystatusid
                  where customerquery_clientuserid=" + clientUserId;
        int type = Common.GetQueryStringValue("t");
        if (type == 0) type = 1;
        if (type == 1)//all
        {
        }
        else if (type == 2)
        {
            query += " and customerquery_querystatusid = 2";
        }
        else if (type == 3)
        {
            query += " and customerquery_querystatusid = 3";
        }
        else if (type == 4)
        {
            query += " and customerquery_querystatusid in(1)";
        }
        else if (type == 5)
        {
            query += " and customerquery_querystatusid = 5";
        }

        query += " order by customerquery_lastupdateddate desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        if (!GlobalUtilities.IsValidaTable(dttbl))
        {
            ltquery.Text = "<div class='error'>No data found.</div>";
            return;
        }
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%' cellpadding=7 class='grid-ui' border=1>");
        html.Append("<tr class='grid-ui-header'><td>Topic</td><td>Type</td><td>Subject</td><td>Submitted Date</td><td>Last Updated Date</td><td>Status</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int queryId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["customerquery_customerqueryid"]);
            string status = GlobalUtilities.ConvertToString(dttbl.Rows[i]["querystatus_status"]);
            string css = "grid-ui-row-alt";
            if (i % 2 == 0)
            {
                css = "grid-ui-row";
            }
            if (!GlobalUtilities.ConvertToBool(dttbl.Rows[i]["customerquery_iscustomervisited"]))
            {
                css += " grid-ui-active";
            }
            html.Append("<tr class='"+css+"'>");
            html.Append("<td><a href='customerquerydetail.aspx?id="+queryId+"'>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["querytopic_topicname"]) + "</a></td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["querytype_querytype"]) + "</td>");
            html.Append("<td><a href='customerquerydetail.aspx?id=" + queryId + "'>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["customerquery_subject"]) + "</a></td>");
            html.Append("<td>" + GlobalUtilities.ConvertToDateTime(dttbl.Rows[i]["customerquery_date"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToDateTime(dttbl.Rows[i]["customerquery_lastupdateddate"]) + "</td>");
            html.Append("<td><div class='qstatus qstatus-" + status.Replace(" ", "").ToLower() + "'>" + status + "</div></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        ltquery.Text = html.ToString();
    }

}
