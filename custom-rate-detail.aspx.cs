using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class custom_rate_detail : System.Web.UI.Page
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
        string dt = GlobalUtilities.ConvertMMDateToDD(Common.GetQueryString("dt"));
        string query = @"select * from tbl_customrate 
                        join tbl_othercurrency on othercurrency_othercurrencyid=customrate_othercurrencyid
                        where cast(customrate_date as date)=cast('" + dt + "' as date) order by customrate_date";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        string date = String.Format("{0:D}", dttbl.Rows[0]["customrate_date"]);
        html.Append("<tr><td colspan='3'>CUSTOM EXCHANGE RATES (All Rates Per Unit) <b>W.E.F. Date (" + date + ")</b></td></tr>");
        html.Append("<tr class='grid-ui-header'><td>CURRENCY</td><td>IMPORT</td><td>EXPORT</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string currency = GlobalUtilities.ConvertToString(dttbl.Rows[i]["othercurrency_currency"]);
            string import = GlobalUtilities.FormatAmount(dttbl.Rows[i]["customrate_import"]);
            string export = GlobalUtilities.FormatAmount(dttbl.Rows[i]["customrate_export"]);
            string css = "grid-ui-alt";
            if (i % 2 == 0) css = "grid-ui-row";
            html.Append("<tr class='" + css + "'>");
            html.Append("<td>" + currency + "</td>");
            html.Append("<td>" + import + "</td>");
            html.Append("<td>" + export + "</td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
}
