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
using WebComponent;
using System.Data;
using System.Text;
public partial class usercontrols_ViewBankScanControl : System.Web.UI.UserControl
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
        string query = "";
        int clientId = Common.ClientId;
        query = @"select * from tbl_bankaudit
                join tbl_bankauditbank on bankauditbank_bankauditbankid=bankaudit_bankauditbankid
                join tbl_bankauditstatus on bankauditstatus_bankauditstatusid=bankaudit_bankauditstatusid
                where bankaudit_clientid=" + clientId;
        query += " order by bankaudit_bankauditid desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%' cellpadding=7 class='grid-ui' border=1>");
        html.Append(@"<tr class='grid-ui-header'><td>Code</td><td>Bank</td><td>Branch</td><td>Currencies</td>
                    <td>Submitted Date</td><td>Last Updated Date</td><td>Status</td><td>Download Report</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["bankaudit_bankauditid"]);
            int statusId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["bankaudit_bankauditstatusid"]);
            string status = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditstatus_status"]);
            
            string css = "grid-ui-row-alt";
            if (i % 2 == 0)
            {
                css = "grid-ui-row";
            }
            if (!GlobalUtilities.ConvertToBool(dttbl.Rows[i]["bankaudit_iscustomervisited"]))
            {
                css += " grid-ui-active";
            }
            html.Append("<tr class='" + css + "'>");
            html.Append("<td><a href='addbankaudit.aspx?id=" + id + "'>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankaudit_code"]) + "</a></td>");
            html.Append("<td><a href='addbankaudit.aspx?id=" + id + "'>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditbank_bankname"]) + "</a></td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankaudit_bankbranch"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankaudit_currencies"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToDateTime(dttbl.Rows[i]["bankaudit_date"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToDateTime(dttbl.Rows[i]["bankaudit_lastupdateddate"]) + "</td>");
            //html.Append("<td><div class='qstatus qstatus-" + status.Replace(" ", "").ToLower() + "'>" + status + "</div></td>");
            html.Append("<td><div class='grid-status' style='background-color:#" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditstatus_backgroundcolor"]) +
                            ";color:#" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditstatus_textcolor"]) + ";'>" + status + "</div></td>");
            if (statusId == 4)
            {
                string guid = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankaudit_guid"]);
                html.Append(@"<td><a href='download-file.aspx?f=bankaudit/doc/" + id + "/AuditReport-" + guid + ".pdf'" + ">Download</a></td>");
            }
            else
            {
                html.Append(@"<td></td>");
            }
            html.Append("</tr>");
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
}
