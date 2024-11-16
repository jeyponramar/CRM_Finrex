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

public partial class mysubscription : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindPlanDetails();
            BindUsers();
        }
    }
    private void BindPlanDetails()
    {
        string query = "";
        int clientId = Common.ClientId;
        query = @"select * from tbl_client
                 left join tbl_serviceplan on serviceplan_serviceplanid=client_serviceplanid
                 where client_clientid=" + clientId;
        DataRow drc = DbTable.ExecuteSelectRow(query);
        if (drc != null)
        {
            lblserviceplan.Text = GlobalUtilities.ConvertToString(drc["serviceplan_planname"]);
        }
        query = @"select * from tbl_clientservices
                join tbl_service on service_serviceid=clientservices_serviceid
                where clientservices_clientid=" + clientId;
        DataTable dttbls = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        for (int i = 0; i < dttbls.Rows.Count; i++)
        {
            html.Append("<div>" + GlobalUtilities.ConvertToString(dttbls.Rows[i]["service_service"]) + "</div>");
        }
        ltservices.Text = html.ToString();

        query = @"select * from tbl_clientprospects
                join tbl_prospect on prospect_prospectid=clientprospects_prospectid
                where clientprospects_clientid=" + clientId;
        DataTable dttblsoft = DbTable.ExecuteSelect(query);
        html = new StringBuilder();
        for (int i = 0; i < dttblsoft.Rows.Count; i++)
        {
            html.Append("<div>" + GlobalUtilities.ConvertToString(dttblsoft.Rows[i]["prospect_prospect"]) + "</div>");
        }
        ltsoftwares.Text = html.ToString();

    }
    private void BindUsers()
    {
        StringBuilder html = new StringBuilder();
        string query = "";
        query = @"select * from tbl_contacts
                 left join tbl_designation on designation_designationid=contacts_designationid
                 where contacts_clientid=" + Common.ClientId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        html.Append("<table width='100%' cellpadding=7 class='grid-ui' border=1>");
        html.Append(@"<tr class='grid-ui-header'><td>Name</td><td>Email</td><td>Mobile</td><td>Designation</td><td>Softwares</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string css = "grid-ui-row-alt";
            if (i % 2 == 0)
            {
                css = "grid-ui-row";
            }

            html.Append("<tr class='" + css + "'>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["contacts_contactperson"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["contacts_emailid"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["contacts_mobileno"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["designation_designation"]) + "</td>");
            html.Append("<td class='list'>");
            if (GlobalUtilities.ConvertToBool(dttbl.Rows[i]["contacts_iswebuser"])) html.Append("<div>Finstation</div>");
            if (GlobalUtilities.ConvertToBool(dttbl.Rows[i]["contacts_isexeuser"])) html.Append("<div>Finwatch</div>");
            if (GlobalUtilities.ConvertToBool(dttbl.Rows[i]["contacts_ismobileuser"])) html.Append("<div>Fin Icon</div>");
            html.Append("</td>");
            html.Append("</tr>");
        }
        html.Append("</table>");

        ltusers.Text = html.ToString();
    }
}
