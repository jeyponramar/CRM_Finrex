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

public partial class commodity_metal_mcx_futurerates : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindMcxFutureRates();
        }
    }

    private void BindMcxFutureRates()
    {
        string query = "";
        query = @"select * from tbl_mcxfuturerate";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%' cellspacing=0 cellpadding=0>");
        html.Append(@"<tr class='repeater-header'><td>Commodity Name</td><td>Date Time</td><td>Price Quote Unit</td><td>Symbol</td><td>Expiry date</td>
            <td>Bid Price</td><td>Ask Price</td><td>LTP</td><td>Net Change</td><td>% Change</td><td>Open</td><td>High</td><td>Low</td><td>Close</td>
            <td>Volume</td><td>OI</td><td>Value</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            DataRow dr = dttbl.Rows[i];
            int id = GlobalUtilities.ConvertToInt(dr["mcxfuturerate_mcxfuturerateid"]);

            html.Append("<tr>");
            html.Append("<td class='repeater-header-left'>" + GlobalUtilities.ConvertToString(dr["mcxfuturerate_commodityname"]) + "</td>");
            
            html.Append("<td><div class='rate' style='width:140px'>" + GlobalUtilities.ConvertToDateTime(dr["mcxfuturerate_date"]) + "</div></td>");
            html.Append("<td><div class='rate' style='width:105px'>" + GlobalUtilities.ConvertToString(dr["mcxfuturerate_pricequoteunit"]) + "</div></td>");
            
            html.Append("<td><div class='rate'>" + GlobalUtilities.ConvertToString(dr["mcxfuturerate_symbol"]) + "</div></td>");
            html.Append("<td><div class='rate' style='width:85px'>" + GlobalUtilities.ConvertToDateMMM(dr["mcxfuturerate_expirydate"]) + "</div></td>");
            
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(dr["mcxfuturerate_bidprice"], 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(dr["mcxfuturerate_askprice"], 2) + "</div></td>");
            
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(dr["mcxfuturerate_ltp"], 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(dr["mcxfuturerate_netchange"], 2) + "</div></td>");
            
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(dr["mcxfuturerate_changepercentage"], 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(dr["mcxfuturerate_open"], 2) + "</div></td>");

            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(dr["mcxfuturerate_high"], 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(dr["mcxfuturerate_low"], 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(dr["mcxfuturerate_close"], 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(dr["mcxfuturerate_volume"], 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(dr["mcxfuturerate_openinterest"], 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(dr["mcxfuturerate_value"], 2) + "</div></td>");

            html.Append("</tr>");
        }
        html.Append("</table>");
        html.Append("</td></tr></table>");
        ltmcxfuturerates.Text = html.ToString();
    }
    
}
