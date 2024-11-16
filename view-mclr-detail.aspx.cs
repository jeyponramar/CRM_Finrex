using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class view_mclr_detail : System.Web.UI.Page
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
        StringBuilder html = new StringBuilder();
        string query = @"select * from tbl_mclrhistory
                        left join tbl_mclrbank on mclrbank_mclrbankid=mclrhistory_mclrbankid
                        where mclrhistory_mclrid=" + Common.GetQueryStringValue("id") +
                        "order by mclrhistory_mclrhistoryid desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        html.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        html.Append(@"<tr class='grid-ui-header' style='text-align:center;'><td>Bank</td><td>1 Month MCLR</td><td>3 Month MCLR</td><td>6 Month MCLR</td><td>1 Year MCLR</td>
                    <td>Latest Update With Effect from</td><td>Base Rate</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string css = "grid-ui-alt";
            if (i % 2 == 0) css = "grid-ui-row";
            html.Append("<tr class='" + css + "' style='text-align:center;'>");
            html.Append("<td style='text-align:left;'>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["mclrbank_bankname"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.FormatAmount(dttbl.Rows[i]["mclrhistory_1month"]) + "%</td>");
            html.Append("<td>" + GlobalUtilities.FormatAmount(dttbl.Rows[i]["mclrhistory_3months"]) + "%</td>");
            html.Append("<td>" + GlobalUtilities.FormatAmount(dttbl.Rows[i]["mclrhistory_6months"]) + "%</td>");
            html.Append("<td>" + GlobalUtilities.FormatAmount(dttbl.Rows[i]["mclrhistory_1year"]) + "%</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["mclrhistory_effectivedate"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.FormatAmount(dttbl.Rows[i]["mclrhistory_baserate"]) + "%</td>");
            html.Append("</tr>");
        }
        html.Append("</table>");

        lthistory.Text = html.ToString();
    }
}
