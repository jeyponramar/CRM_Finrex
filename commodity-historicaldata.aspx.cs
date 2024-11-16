using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using WebComponent;

public partial class commodity_historicaldata : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GlobalData gbldata = new GlobalData();
            gbldata.FillDropdown(ddlmetal, "tbl_metal", "metal_metalname", "");
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ddlmetal.SelectedIndex == 0)
        {
            ltdata.Text = "<span class='error'>Please select metal!</span>";
            return;
        }
        string query = @"select top 1000 *,
                        (select top 1 MetalCurrencyRate_rate from tbl_MetalCurrencyRate 
                            where MetalCurrencyRate_metalid=DailyLMEMetalRate_metalid 
                                      and MetalCurrencyRate_metalcurrencyid=1 and MetalCurrencyRate_date=DailyLMEMetalRate_date
                            ) as MetalCurrencyRate_rate,
                        (select top 1 historicaldata_rbirefrate from tbl_historicaldata where cast(historicaldata_date as date)=cast(DailyLMEMetalRate_date as date) and historicaldata_currencyid=2) as usdinrrbirefrate
                        from tbl_DailyLMEMetalRate
                        join tbl_metal on metal_metalid=DailyLMEMetalRate_metalid";
        query += " where DailyLMEMetalRate_metalid=" + ddlmetal.SelectedValue;
        //query += " and DailyLMEMetalRate_isactive=1 ";
        if (txtfromdate.Text != "") query += " AND cast(DailyLMEMetalRate_date as date)>=cast('" + GlobalUtilities.ConvertMMDateToDD(txtfromdate.Text) + "' as date)";
        if (txttodate.Text != "") query += " AND cast(DailyLMEMetalRate_date as date)<=cast('" + GlobalUtilities.ConvertMMDateToDD(txttodate.Text) + "' as date)";
        query += " order by DailyLMEMetalRate_date desc,DailyLMEMetalRate_metalid";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table class='grid-ui' cellpadding='7' border='1'>");
//        html.Append(@"<tr class='grid-ui-header'><td>Date</td><td>Metal</td><td>Open</td><td>High</td><td>Low</td><td>Close</td>
//                    <td>SETTLEMENT BID RATE Cash</td><td>SETTLEMENT BID RATE 3-months</td><td>Live Warrants</td><td>Cancelled Warrants</td>
//                    <td>Opening Stock</td><td>ASIAN REFERENCE PRICE</td><td>STERLING EQUIVALENTS Cash</td>
//                    <td>STERLING EQUIVALENTS 3-months</td><td>USDINR Close</td><td>USDINR RBI REF</td></tr>
//                    ");
        html.Append(@"<tr class='grid-ui-header'><td>Date</td><td>Metal</td><td>Open</td><td>High</td><td>Low</td><td>Close</td>
                    <td>SETTLEMENT ASK RATE Cash</td><td>SETTLEMENT ASK RATE 3-months</td><td>Live Warrants</td><td>Cancelled Warrants</td>
                    <td>Opening Stock</td><td>USDINR Close</td><td>USDINR RBI REF</td></tr>
                    ");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string css = "grid-ui-alt";
            if (i % 2 == 0) css = "grid-ui-row";
            double currencyExchangeRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["MetalCurrencyRate_rate"]);
            double cash = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["DailyLMEMetalRate_cash"]);
            double threemonths = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["DailyLMEMetalRate_threemonths"]);
            double sterlingCash = 0;
            double sterlingThreemonth = 0;
            if (currencyExchangeRate > 0)
            {
                sterlingCash = cash / currencyExchangeRate;
                sterlingThreemonth = threemonths / currencyExchangeRate;
            }
            double livewarrants = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["DailyLMEMetalRate_livewarrants"]);
            double cancelledwarrants = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["DailyLMEMetalRate_cancelledwarrants"]);
            double openingstock = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["DailyLMEMetalRate_openingstock"]);
            double totalopeningstock = livewarrants + cancelledwarrants;
            html.Append("<tr class='" + css + "'>");
            html.Append("<td style='white-space:nowrap;'>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["DailyLMEMetalRate_date"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["metal_metalname"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["DailyLMEMetalRate_open"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["DailyLMEMetalRate_high"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["DailyLMEMetalRate_low"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["DailyLMEMetalRate_close"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["dailylmemetalrate_cashask"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["dailylmemetalrate_threemonthsask"]) + "</td>");
            html.Append("<td>" + livewarrants + "</td>");
            html.Append("<td>" + cancelledwarrants + "</td>");
            html.Append("<td>" + totalopeningstock + "</td>");
            //html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["DailyLMEMetalRate_asianrate"]) + "</td>");
            //html.Append("<td>" + ExportExposurePortal.DecimalPoint(dttbl.Rows[i]["DailyLMEMetalRate_sterlingequivalentcash"], 2) + "</td>");
            //html.Append("<td>" + ExportExposurePortal.DecimalPoint(dttbl.Rows[i]["DailyLMEMetalRate_sterlingequivalentthreemonths"], 2) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["dailylmemetalrate_usdinrclose"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["usdinrrbirefrate"]) + "</td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
}
