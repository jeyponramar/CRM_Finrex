using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using WebComponent;
using System.Text;

public partial class cross_currency : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindRate(2, ltCrossCurrencies);
            BindRate(26, ltliberrates);
            BindRate(48, ltRBI);
            BindRate(44, ltFutureCurrency);
            BindCustomRate();
            BindAlternateRefRate();
        }
    }
    private void BindCustomRate()
    {
        StringBuilder html = new StringBuilder();
        string query = @"select * from tbl_customrate 
                        join tbl_othercurrency on othercurrency_othercurrencyid=customrate_othercurrencyid
                        where 
                        customrate_date=(select MAX(customrate_date) from tbl_customrate)";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        html.Append("<table class='repeater' border=1 style='width:100%'>");
        string date = String.Format("{0:D}", dttbl.Rows[0]["customrate_date"]);
        string excludedate = GlobalUtilities.ConvertToDate(dttbl.Rows[0]["customrate_date"]);
        excludedate = GlobalUtilities.ConvertMMDateToDD(excludedate);
        lblcustomratedate.Text = "CUSTOM EXCHANGE RATES (All Rates Per Unit) <br/><b>W.E.F. Date (" + date + ")</b>";
        html.Append("<tr class='repeater-header'><td>CURRENCY</td><td>IMPORT</td><td>EXPORT</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string currency = GlobalUtilities.ConvertToString(dttbl.Rows[i]["othercurrency_currency"]);
            string import = GlobalUtilities.FormatAmount(dttbl.Rows[i]["customrate_import"]);
            string export = GlobalUtilities.FormatAmount(dttbl.Rows[i]["customrate_export"]);
            
            html.Append("<tr>");
            html.Append("<td class='repeater-header-left'>" + currency + "</td>");
            html.Append("<td style='text-align:center;'>" + import + "</td>");
            html.Append("<td style='text-align:center;'>" + export + "</td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        ltcustomrate.Text = html.ToString();
    }
    protected void btnMajor_Click(object sender, EventArgs e)
    {
        btnAsia.CssClass = "btncurrency";
        btnMajor.CssClass = "btncurrency-active";
        BindRate(2, ltCrossCurrencies);
    }
    protected void btnAsia_Click(object sender, EventArgs e)
    {
        btnAsia.CssClass = "btncurrency-active";
        btnMajor.CssClass = "btncurrency";
        BindRate(3, ltCrossCurrencies);
    }
    
    protected void btnUSDINRFuture_Click(object sender, EventArgs e)
    {
        ResetButton();
        btnUSDINRFuture.CssClass = "btncurrency-active";
        BindRate(44, ltFutureCurrency);
    }
    protected void btnEURINRFuture_Click(object sender, EventArgs e)
    {
        ResetButton();
        btnEURINRFuture.CssClass = "btncurrency-active";
        BindRate(45, ltFutureCurrency);
    }
    protected void btnGBPINRFuture_Click(object sender, EventArgs e)
    {
        ResetButton();
        btnGBPINRFuture.CssClass = "btncurrency-active";
        BindRate(46, ltFutureCurrency);
    }
    protected void btnJPYINRFuture_Click(object sender, EventArgs e)
    {
        ResetButton();
        btnJPYINRFuture.CssClass = "btncurrency-active";
        BindRate(47, ltFutureCurrency);
    }
    private void ResetButton()
    {
        btnUSDINRFuture.CssClass = "btncurrency";
        btnEURINRFuture.CssClass = "btncurrency";
        btnGBPINRFuture.CssClass = "btncurrency";
        btnJPYINRFuture.CssClass = "btncurrency";
    }
    private void BindRate(int sectionId, Literal lt)
    {
        string query = "select * from tbl_liveratesection where liveratesection_liveratesectionid=" + sectionId;
        DataRow drSection = DbTable.ExecuteSelectRow(query);
        string rows = GlobalUtilities.ConvertToString(drSection["liveratesection_rows"]);
        string cols = GlobalUtilities.ConvertToString(drSection["liveratesection_columns"]);
        string sectionCode = GlobalUtilities.ConvertToString(drSection["liveratesection_code"]);
        //rows = rows.Replace(",USDCNH", "");//excluded USDCNH

        Array arrRows = rows.Split(',');
        Array arrCols = cols.Split(',');
        int colCount = arrCols.Length;
        int tempCount = 0;
        bool isColFound = true;
        if (Int32.TryParse(cols, out tempCount))
        {
            colCount = tempCount;
            isColFound = false;
        }
        StringBuilder html = new StringBuilder();
        html.Append("<table class='repeater' border=1 style='width:100%;height:100%;'>");
        if (isColFound)
        {
            html.Append("<tr class='repeater-header'><td>&nbsp;</td>");

            for (int i = 0; i < arrCols.Length; i++)
            {
                if (i == 5)
                {
                    html.Append("<td>" + arrCols.GetValue(i).ToString() + "&nbsp;<img src='images/arrow_expand_right.png' class='jq-liverate-expand hand' target='jq-liverate-cross-currency-more' title='Show more'/></td>");
                }
                else if (i > 5)
                {
                    html.Append("<td class='jq-liverate-cross-currency-more hidden'>" + arrCols.GetValue(i).ToString() + "</td>");
                }
                else
                {
                    html.Append("<td>" + arrCols.GetValue(i).ToString() + "</td>");
                }
            }
            html.Append("</tr>");
        }
        query = "select * from tbl_liverate WHERE liverate_liveratesectionid=" + sectionId;// +" and liverate_row<>7";//excluded USDCNH
        DataTable dttbl = DbTable.ExecuteSelect(query);
        dttbl = Finstation.CorrectLiveRateValues(dttbl);

        for (int i = 0; i < arrRows.Length; i++)
        {
            html.Append("<tr><td class='repeater-header-left'>" + arrRows.GetValue(i).ToString() + "</td>");
            for (int j = 0; j < colCount; j++)
            {
                string currentRate = "";
                string prevRate = "";
                int liverateId = 0;
                string rtdCode = "";
                string calculation = "";
                int rowNo = 0;
                int colNo = 0;
                double dblCurrentRate = 0;
                double dblPrevRate = 0;
                bool isnumber = false;
                int istick = 0;
                for (int k = 0; k < dttbl.Rows.Count; k++)
                {
                    if (GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_row"]) == (i + 1) &&
                        GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_column"]) == (j + 1))
                    {
                        currentRate = GlobalUtilities.ConvertToString(dttbl.Rows[k]["liverate_currentrate"]);
                        if (currentRate.Contains("/"))
                        {
                            Array arr = currentRate.Split('/');
                            currentRate = arr.GetValue(1).ToString() + "-" + arr.GetValue(0).ToString() + "-" + arr.GetValue(2).ToString();
                        }
                        prevRate = GlobalUtilities.ConvertToString(dttbl.Rows[k]["liverate_prevrate"]);
                        liverateId = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_liverateid"]);
                        rtdCode = GlobalUtilities.ConvertToString(dttbl.Rows[k]["liverate_rtdcode"]);
                        calculation = GlobalUtilities.ConvertToString(dttbl.Rows[k]["liverate_calculation"]);
                        rowNo = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_row"]);
                        colNo = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_column"]);
                        istick = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_istick"]);
                        isnumber = Double.TryParse(currentRate, out dblCurrentRate);
                        Double.TryParse(prevRate, out dblPrevRate);
                        break;
                    }
                }
                string cls = "";
                string calc = "";
                string code = sectionCode + "_" + rowNo + "_" + colNo;
                if (istick == 1)
                {
                    if (isnumber)
                    {
                        if (dblCurrentRate > dblPrevRate)
                        {
                            cls = "rate-up";
                        }
                        else
                        {
                            cls = "rate-down";
                        }
                    }
                }
                if (calculation != "")
                {
                    cls += " calc";
                    calc = " calc='" + calculation + "'";
                }
                cls += " " + code;
                string tdcls = "";
                if (j > 5)
                {
                    tdcls = " jq-liverate-cross-currency-more hidden";
                }
                if (rtdCode.Contains("#"))
                {
                    html.Append("<td class='" + tdcls + "'><div class='" + cls + " rate liverate' rid='" + liverateId + "' rc='" + rtdCode + "'" + calc + " c='" + code + "' istick='" + istick + "' prate='" + dblPrevRate + "'>" + currentRate + "</div></td>");
                }
                else
                {
                    if (calculation == "")
                    {
                        html.Append("<td rid='" + liverateId + "' rc='" + rtdCode + "'" + calc + " c='" + code + "' class='liverate" + tdcls + "'>" + rtdCode + "</td>");
                    }
                    else
                    {
                        html.Append("<td class='" + tdcls + "'><div class='calc liverate' rid='" + liverateId + "' rc='" + rtdCode + "' calc='" + calculation + "' c='" + code + "'></div></td>");
                    }
                }
            }
            html.Append("</tr>");
        }

        html.Append("</table>");

        lt.Text = html.ToString();
    }
    private void BindAlternateRefRate()
    {
        StringBuilder html = new StringBuilder();
        string query = "";
        query = "select * from tbl_currency";
        DataTable dttblcurrency = DbTable.ExecuteSelect(query);
        query = "select * from tbl_arrmaster";
        DataTable dttblarr = DbTable.ExecuteSelect(query);
        html.Append("<table class='repeater' border=1 style='width:100%;height:100%;'>");
        html.Append(@"<tr class='repeater-header'><td>Currency</td><td>ARR</td><td>Date</td><td>O/n</td><td>1Week</td>
                <td width='100'>1-month TERM / Average</td><td width='100'>3-month TERM / Average</td><td width='100'>6-month TERM / Average</td>
                <td width='100'>12-month TERM / Average</td></tr>");
        for (int i = 0; i < dttblcurrency.Rows.Count; i++)
        {
            int currencyId = GlobalUtilities.ConvertToInt(dttblcurrency.Rows[i]["currency_currencyid"]);
            string currency = GlobalUtilities.ConvertToString(dttblcurrency.Rows[i]["currency_currency"]);
            for (int j = 0; j < dttblarr.Rows.Count; j++)
            {
                int arrmasterId = GlobalUtilities.ConvertToInt(dttblarr.Rows[j]["arrmaster_arrmasterid"]);
                query = @"select top 1 * from tbl_alternativereferencerate
                        join tbl_arrmaster on arrmaster_arrmasterid=alternativereferencerate_arrmasterid
                        where alternativereferencerate_currencyid=" + currencyId + " and alternativereferencerate_arrmasterid=" + arrmasterId +
                          @" order by alternativereferencerate_date desc";
                DataRow dr = DbTable.ExecuteSelectRow(query);
                if (dr != null)
                {
                    html.Append("<tr class='repeater-row-black'>");
                    html.Append("<td>" + currency + "</td>");
                    html.Append("<td>" + GlobalUtilities.ConvertToString(dr["arrmaster_name"]) + "</td>");
                    html.Append("<td style='white-space:nowrap;'>" + GlobalUtilities.ConvertToDate(dr["alternativereferencerate_date"]) + "</td>");
                    html.Append("<td>" + FormatData(dr["alternativereferencerate_on"]) + "</td>");
                    html.Append("<td>" + FormatData(dr["alternativereferencerate_1week"]) + "</td>");
                    html.Append("<td>" + FormatData(dr["alternativereferencerate_1monthtermaverage"]) + "</td>");
                    html.Append("<td>" + FormatData(dr["alternativereferencerate_3monthtermaverage"]) + "</td>");
                    html.Append("<td>" + FormatData(dr["alternativereferencerate_6monthtermaverage"]) + "</td>");
                    html.Append("<td>" + FormatData(dr["alternativereferencerate_12monthtermaverage"]) + "</td>");
                }
            }
        }
        html.Append("</table>");
        ltalternaterefrates.Text = html.ToString();
    }
    private string FormatData(object val)
    {
        if (GlobalUtilities.ConvertToDouble(val) == 0) return "-";
        string data = ExportExposurePortal.DecimalPoint(val, 4);
        return data;
    }
}
