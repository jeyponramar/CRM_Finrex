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

public partial class commodity_metal_bombay_exchange : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindBombayExchangeRate();
        }
    }

    private void BindBombayExchangeRate()
    {
        string query = "";
        StringBuilder html = new StringBuilder();
        query = @"select top 1 * from tbl_bombaymetalexchangerate order by bombaymetalexchangerate_date desc";
        string sqldate1 = "";
        string date1 = "";
        DataRow drLast = DbTable.ExecuteSelectRow(query);
        if (drLast == null)
        {
            sqldate1 = GlobalUtilities.ConvertToSqlDate(DateTime.Now);
            date1 = GlobalUtilities.ConvertToDateMMM(DateTime.Now);
        }
        else
        {
            sqldate1 = GlobalUtilities.ConvertToSqlDate(Convert.ToDateTime(drLast["bombaymetalexchangerate_date"]));
            date1 = GlobalUtilities.ConvertToDateMMM(Convert.ToDateTime(drLast["bombaymetalexchangerate_date"]));
        }
        query = @"select * from tbl_bombaymetalexchangerate where cast(bombaymetalexchangerate_date as date)=cast('" + sqldate1 + "' as date)";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        html.Append("<table width='100%' cellspacing=0 cellpadding=0>");
        html.Append("<tr><td>" + date1 + "</td><td align='right'>Rs. Per KG</td></tr>");
        html.Append("<tr><td colspan='2'>");
        html.Append("<table width='100%' cellspacing=0 cellpadding=0 class='repeater' border=1>");
        html.Append(@"<tr class='repeater-header'><td>Metal</td><td>Description</td><td>Today</td><td>Prev. Day</td><td>Year Ago</td><td>Month Ago</td>
                            <td>Change%</td><td>Month Change%</td><td>Year Change%</td>
                    </tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            html.Append("<tr>");
            html.Append("<td class='repeater-header-left nowrap'>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["bombaymetalexchangerate_metalname"]) + "</td>");
            html.Append("<td class='repeater-header-left nowrap'>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["bombaymetalexchangerate_metaldescription"]) + "</td>");

            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["bombaymetalexchangerate_todayrate"]) / 100000, 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["bombaymetalexchangerate_prevdayrate"]) / 100000, 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["bombaymetalexchangerate_yearagorate"]) / 100000, 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["bombaymetalexchangerate_monthagorate"]) / 100000, 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["bombaymetalexchangerate_changepercentage"]), 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["bombaymetalexchangerate_monthchangepercentage"]), 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["bombaymetalexchangerate_yearchangepercentage"]), 2) + "</div></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        html.Append("</td></tr></table>");
        ltbombayexchangerate.Text = html.ToString();
    }
    
}
