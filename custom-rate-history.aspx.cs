using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class custom_rate_history : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] == "true")
        {
            this.Page.MasterPageFile = "~/MasterPage.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Finstation.CheckFullFinstationAccess();
        if (!IsPostBack)
        {
            BindData();
        }
    }
    private void BindData()
    {
        StringBuilder html = new StringBuilder();
        string query = @"select * from tbl_customrate 
                        join tbl_othercurrency on othercurrency_othercurrencyid=customrate_othercurrencyid
                        where 
                        customrate_date=(select MAX(customrate_date) from tbl_customrate)";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        html.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        string date = String.Format("{0:D}", dttbl.Rows[0]["customrate_date"]);
        string excludedate = GlobalUtilities.ConvertToDate(dttbl.Rows[0]["customrate_date"]);
        excludedate = GlobalUtilities.ConvertMMDateToDD(excludedate);
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
        //ltdata.Text = html.ToString();

        query = "select distinct year(customrate_date) as y from tbl_customrate order by year(customrate_date) desc";
        DataTable dttbly = DbTable.ExecuteSelect(query);
        StringBuilder htmlhistory = new StringBuilder();
        htmlhistory.Append("<table>");
        for (int i = 0; i < dttbly.Rows.Count; i++)
        {
            int year = GlobalUtilities.ConvertToInt(dttbly.Rows[i]["y"]);
            query = "select distinct(cast(customrate_date as date)) as dt from tbl_customrate where year(customrate_date)=" + year +
                    " and cast(customrate_date as date)<>cast('" + excludedate + "' as date)" +
                    " order by cast(customrate_date as date) desc";
            DataTable dttblhistory = DbTable.ExecuteSelect(query);
            htmlhistory.Append("<tr><td class='folder-icon'>Year " + year + "</td></tr>");
            htmlhistory.Append("<tr class='hidden'><td style='padding:10px;'><table>");
            for (int j = 0; j < dttblhistory.Rows.Count; j++)
            {
                string date3 = GlobalUtilities.ConvertToDate(dttblhistory.Rows[j]["dt"]);
                string date2 = String.Format("{0:D}", dttblhistory.Rows[j]["dt"]);
                htmlhistory.Append("<tr><td class='lnkarrow'><a href='#' class='lnkcustomrate' dt='" + date3 + "'>" + date2 + "</a></td></tr>");
            }
            htmlhistory.Append("</table>");
        }
        htmlhistory.Append("</table>");
        lthistory.Text = htmlhistory.ToString();
    }
}
