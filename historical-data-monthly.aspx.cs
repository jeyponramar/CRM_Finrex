using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class historical_data_monthly : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Custom.CheckSubscriptionAccess();
        Finstation.CheckFullFinstationAccess();
        if (!IsPostBack)
        {
            GlobalData gbldata = new GlobalData();
            gbldata.FillDropdown(ddlmonth, "tbl_month", "month_month", "month_monthid", "", "month_monthid");
            BindYear();
            ddlcurrency.SelectedValue = Common.GetQueryStringValue("currency").ToString();
            SetMonth();
            BindData();
        }
    }
    private void SetMonth()
    {
        string query = @"select top 1 month(historicaldata_date) as m from tbl_historicaldata 
                        order by year(historicaldata_date) desc,month(historicaldata_date) desc";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return;
        ddlmonth.SelectedValue = GlobalUtilities.ConvertToString(dr["m"]);
    }
    private void BindYear()
    {
        string query = @"select distinct year(historicaldata_date) as id, year(historicaldata_date) as val from tbl_historicaldata 
                        order by year(historicaldata_date) desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        ddlyear.DataSource = dttbl;
        ddlyear.DataTextField = "val";
        ddlyear.DataValueField = "id";
        ddlyear.DataBind();
    }
    private void BindData()
    {
        string query = "";
        StringBuilder html = new StringBuilder();
        StringBuilder htmlsummary = new StringBuilder();
        int year = GlobalUtilities.ConvertToInt(ddlyear.SelectedValue);
        int month = GlobalUtilities.ConvertToInt(ddlmonth.SelectedValue);
        int currencyId = GlobalUtilities.ConvertToInt(ddlcurrency.SelectedValue);
        tdchart.Visible = false;
        lblMessage.Visible = false;
        lbltitle.Text = "MONTHLY PERFORMANCE";
        hdndates.Text = "";
        hdnrates.Text = "";
        if (month == 0)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Please choose month!";
            return;
        }
        if (month == 0)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Please choose currency!";
            return;
        }
        lbltitle.Text = "MONTHLY " + ddlcurrency.SelectedItem.Text + " CHANGES - " + GlobalUtilities.GetMonthShortName(month) + " " + year;

        query = "select * from tbl_historicaldata";
        query += " where historicaldata_currencyid=" + currencyId;
        query += " AND month(historicaldata_date)=" + month + " and year(historicaldata_date)=" + year;
        query += @"order by historicaldata_date";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        html.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        html.Append("<tr class='grid-ui-header'><td colspan='10' style='text-align:center'>Daily Price</td></tr>");
        html.Append("<tr class='grid-ui-header'><td>Date</td><td>Day</td><td>Open</td><td>High</td><td>Low</td><td>Close</td></tr>");
        if (!GlobalUtilities.IsValidaTable(dttbl))
        {
            html.Append("</table>");
            ltdata.Text = html.ToString();
            ltsummary.Text = "";
            return;
        }
        tdchart.Visible = true;
        double avgClose = 0;
        double totalClose = 0;
        double totalFirstHalfClose = 0;
        double totalSecondHalfClose = 0;
        int firstHalfDays = 0;
        int secondHalfDays = 0;
        double avgFirstHalfClose = 0;
        double avgSecondHalfClose = 0;
        double maxRate = 0;
        double minRate = int.MaxValue;
        int monthEnd = GlobalUtilities.GetMonthEndDay(month, year);
        StringBuilder dates = new StringBuilder();
        StringBuilder opens = new StringBuilder();
        StringBuilder highs = new StringBuilder();
        StringBuilder lows = new StringBuilder();
        StringBuilder closes = new StringBuilder();
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string css = "grid-ui-alt";
            if (i % 2 == 0) css = "grid-ui-row";
            DateTime dt = Convert.ToDateTime(dttbl.Rows[i]["historicaldata_date"]);
            string day = dt.ToString("dddd");
            double close = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["historicaldata_close"]);
            double open = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["historicaldata_open"]);
            double high = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["historicaldata_high"]);
            double low = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["historicaldata_low"]);
            string date = GlobalUtilities.ConvertToDate(dttbl.Rows[i]["historicaldata_date"]);
            if (i == 0)
            {
                dates.Append(date);
                opens.Append(open.ToString());
                highs.Append(high.ToString());
                lows.Append(low.ToString());
                closes.Append(close.ToString());
            }
            else
            {
                dates.Append("," + date);
                opens.Append(","+open.ToString());
                highs.Append("," + high.ToString());
                lows.Append("," + low.ToString());
                closes.Append("," + close.ToString());
            }
            html.Append("<tr class='" + css + "'>");
            html.Append("<td>" + date + "</td>");
            html.Append("<td>" + day + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(open) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(high) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(low) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(close) + "</td>");
            html.Append("</tr>");
            totalClose += close;
            if (dt.Day <= 15)
            {
                totalFirstHalfClose += close;
                firstHalfDays++;
            }
            if (open > maxRate) maxRate = open;
            if (high > maxRate) maxRate = high;
            if (low > maxRate) maxRate = low;
            if (close > maxRate) maxRate = close;

            if (open < minRate) minRate = open;
            if (high < minRate) minRate = high;
            if (low < minRate) minRate = low;
            if (close < minRate) minRate = close;

        }
        hdndates.Text = dates.ToString();
        hdnrates.Text = opens.ToString() + "|" + highs.ToString() + "|" + lows.ToString() + "|" + closes.ToString();
        html.Append("</table>");
        secondHalfDays = dttbl.Rows.Count - firstHalfDays;
        totalSecondHalfClose = totalClose - totalFirstHalfClose;
        avgClose = totalClose / dttbl.Rows.Count;
        avgFirstHalfClose = totalFirstHalfClose / firstHalfDays;
        avgSecondHalfClose = totalSecondHalfClose / secondHalfDays;
        
        string monthEndString = "";
        if (monthEnd == 31)
        {
            monthEndString = monthEnd.ToString() + "st";
        }
        else
        {
            monthEndString = monthEnd.ToString() + "th";
        }
        htmlsummary.Append("<table width='100%'><tr><td>");
        htmlsummary.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        htmlsummary.Append("<tr class='grid-ui-header'><td colspan='10' style='text-align:center'>Summary</td></tr>");
        htmlsummary.Append("<tr><td width='300'>Daily Average rate</td><td>" + GlobalUtilities.FormatAmount(avgClose) + "</td></tr>");
        htmlsummary.Append("<tr><td>1st Fortnightly (1-15th) Average rate</td><td>" + GlobalUtilities.FormatAmount(avgFirstHalfClose) + "</td></tr>");
        htmlsummary.Append("<tr><td>2nd Fortnightly (16-" + monthEndString + ") Average Rate</td><td>" + GlobalUtilities.FormatAmount(avgSecondHalfClose) + "</td></tr>");
        htmlsummary.Append("<tr><td>Maximum Value</td><td>" + GlobalUtilities.FormatAmount(maxRate) + "</td></tr>");
        htmlsummary.Append("<tr><td>Minimum Value</td><td>" + GlobalUtilities.FormatAmount(minRate) + "</td></tr>");
        htmlsummary.Append("</table>");
        htmlsummary.Append("</td></tr>");

        DateTime dtqtrend = new DateTime(year, month, GlobalUtilities.GetMonthEndDay(month, year));
        DateTime dtqtrstart = dtqtrend.AddMonths(-13);
        dtqtrstart = new DateTime(dtqtrstart.Year, dtqtrstart.Month, 1);
        query = "select * from tbl_historicaldata";
        query += " where historicaldata_currencyid=" + currencyId;
        query += " AND cast(historicaldata_date as date)>=cast('" + GlobalUtilities.ConvertToSqlDate(dtqtrstart) + "' as date)";
        query += " AND cast(historicaldata_date as date)<=cast('" + GlobalUtilities.ConvertToSqlDate(dtqtrend) + "' as date)";
        query += " order by historicaldata_date";
        DataTable dttblhistory = DbTable.ExecuteSelect(query);

        //weekly calculation starts here
        int weekCount = 0;
        DataTable dttblweekly = Finstation.GetAvgRateTemplate();

        DateTime dtstartweek = new DateTime();
        DateTime dtendweek = new DateTime();
        bool isfirstweek = true;
        for (int d = 1; d <= monthEnd; d++)
        {
            DateTime dt = new DateTime(year, month, d);
            if (isfirstweek)
            {
                if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }
            }
            if (dt.DayOfWeek == DayOfWeek.Monday)
            {
                isfirstweek = false;
                dtstartweek = dt;
                weekCount++;
            }
            if (dt.DayOfWeek == DayOfWeek.Sunday || d == monthEnd)
            {
                dtendweek = dt;
                DataRow dr = dttblweekly.NewRow();
                //dr["Week"] = weekCount;
                dr["FromDate"] = dtstartweek;
                dr["ToDate"] = dtendweek;
                dttblweekly.Rows.Add(dr);
                isfirstweek = false;
            }
        }
        dttblweekly = Finstation.CalculateAvgRatesByDateRange(dttblhistory, dttblweekly);
        htmlsummary.Append("<tr><td>&nbsp;</td></tr>");
        htmlsummary.Append("<tr><td>");
        htmlsummary.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        htmlsummary.Append("<tr class='grid-ui-header'><td colspan='10' style='text-align:center'>Weekly Price</td></tr>");
        htmlsummary.Append(@"<tr class='grid-ui-header'><td style='min-width:30px;'>Week</td><td style='min-width:70px;'>Date from</td>
                            <td style='min-width:70px;'>Date To</td><td style='min-width:80px;'>Open</td><td style='min-width:80px;'>High</td>
                            <td style='min-width:80px;'>Low</td><td style='min-width:80px;'>Close</td><td style='min-width:80px;'>Avg</td></tr>");
        for (int i = 0; i < dttblweekly.Rows.Count; i++)
        {
            string css = "grid-ui-alt";
            if (i % 2 == 0) css = "grid-ui-row";
            htmlsummary.Append("<tr class='" + css + "'>");
            htmlsummary.Append("<td>" + (i + 1) + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.ConvertToDate(dttblweekly.Rows[i]["FromDate"]) + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.ConvertToDate(dttblweekly.Rows[i]["ToDate"]) + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.FormatAmount(dttblweekly.Rows[i]["open"]) + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.FormatAmount(dttblweekly.Rows[i]["high"]) + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.FormatAmount(dttblweekly.Rows[i]["low"]) + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.FormatAmount(dttblweekly.Rows[i]["close"]) + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.FormatAmount(dttblweekly.Rows[i]["avgclose"]) + "</td>");
            htmlsummary.Append("</tr>");
        }
        htmlsummary.Append("</table>");
        htmlsummary.Append("</td></tr>");
        //weekly calculation ends here

        //monthly calculation starts here
        DataTable dttblmonthly = Finstation.GetAvgRateTemplate();
        DateTime dtendmonth = new DateTime(year, month, 1);
        for (int i = 1; i <= 6; i++)
        {
            DataRow dr = dttblmonthly.NewRow();
            DateTime dtfrom = dtendmonth.AddMonths(-1 * i);
            DateTime dtto = new DateTime(dtfrom.Year, dtfrom.Month, GlobalUtilities.GetMonthEndDay(dtfrom.Month, dtfrom.Year));
            dr["FromDate"] = dtfrom;
            dr["ToDate"] = dtto;
            dttblmonthly.Rows.Add(dr);
        }
        dttblmonthly = Finstation.CalculateAvgRatesByDateRange(dttblhistory, dttblmonthly);
        htmlsummary.Append("<tr><td>&nbsp;</td></tr>");
        htmlsummary.Append("<tr><td>");
        htmlsummary.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        htmlsummary.Append("<tr class='grid-ui-header'><td colspan='10' style='text-align:center'>Monthly Price</td></tr>");
        htmlsummary.Append(@"<tr class='grid-ui-header'><td style='min-width:70px;'>Month</td><td style='min-width:80px;'>Open</td><td style='min-width:80px;'>High</td>
                            <td style='min-width:80px;'>Low</td><td style='min-width:80px;'>Close</td><td style='min-width:80px;'>Avg</td></tr>");
        for (int i = 0; i < dttblmonthly.Rows.Count; i++)
        {
            string css = "grid-ui-alt";
            if (i % 2 == 0) css = "grid-ui-row";
            DateTime dtfrom = Convert.ToDateTime(dttblmonthly.Rows[i]["FromDate"]);
            htmlsummary.Append("<tr class='" + css + "'>");
            htmlsummary.Append("<td>" + GlobalUtilities.GetMonthShortName(dtfrom.Month) + " - " + dtfrom.Year + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.FormatAmount(dttblmonthly.Rows[i]["open"]) + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.FormatAmount(dttblmonthly.Rows[i]["high"]) + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.FormatAmount(dttblmonthly.Rows[i]["low"]) + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.FormatAmount(dttblmonthly.Rows[i]["close"]) + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.FormatAmount(dttblmonthly.Rows[i]["avgclose"]) + "</td>");
            htmlsummary.Append("</tr>");
        }
        htmlsummary.Append("</table>");
        htmlsummary.Append("</td></tr>");
        //monthly calculation ends here

        //quarterly calculation starts here
        DataTable dttblquarterly = Finstation.GetAvgRateTemplate();
        DateTime dtendqtr = new DateTime(year, month, 1).AddMonths(-1);
        for (int i = 1; i <= 4; i++)
        {
            DataRow dr = dttblquarterly.NewRow();
            DateTime dtto = new DateTime(dtendqtr.Year, dtendqtr.Month, GlobalUtilities.GetMonthEndDay(dtendqtr.Month, dtendqtr.Year));
            DateTime dtfrom = dtto.AddMonths(-2);
            dtfrom = new DateTime(dtfrom.Year, dtfrom.Month, 1);
            dr["FromDate"] = dtfrom;
            dr["ToDate"] = dtto;
            dttblquarterly.Rows.Add(dr);
            dtendqtr = dtendqtr.AddMonths(-3);
        }
        dttblquarterly = Finstation.CalculateAvgRatesByDateRange(dttblhistory, dttblquarterly);
        htmlsummary.Append("<tr><td>&nbsp;</td></tr>");
        htmlsummary.Append("<tr><td>");
        htmlsummary.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        htmlsummary.Append("<tr class='grid-ui-header'><td colspan='10' style='text-align:center'>Quarterly Price</td></tr>");
        htmlsummary.Append(@"<tr class='grid-ui-header'><td style='min-width:70px;'>Quarter</td><td style='min-width:80px;'>Open</td><td style='min-width:80px;'>High</td>
                            <td style='min-width:80px;'>Low</td><td style='min-width:80px;'>Close</td><td style='min-width:80px;'>Avg</td></tr>");
        for (int i = 0; i < dttblquarterly.Rows.Count; i++)
        {
            string css = "grid-ui-alt";
            if (i % 2 == 0) css = "grid-ui-row";
            DateTime dtfrom = Convert.ToDateTime(dttblquarterly.Rows[i]["FromDate"]);
            DateTime dtto = Convert.ToDateTime(dttblquarterly.Rows[i]["ToDate"]);
            htmlsummary.Append("<tr class='" + css + "'>");
            htmlsummary.Append("<td>" + GlobalUtilities.GetMonthShortName(dtfrom.Month) + " " + dtfrom.Year + " - " + GlobalUtilities.GetMonthShortName(dtto.Month) + " " + dtto.Year + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.FormatAmount(dttblquarterly.Rows[i]["open"]) + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.FormatAmount(dttblquarterly.Rows[i]["high"]) + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.FormatAmount(dttblquarterly.Rows[i]["low"]) + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.FormatAmount(dttblquarterly.Rows[i]["close"]) + "</td>");
            htmlsummary.Append("<td>" + GlobalUtilities.FormatAmount(dttblquarterly.Rows[i]["avgclose"]) + "</td>");
            htmlsummary.Append("</tr>");
        }
        htmlsummary.Append("</table>");
        htmlsummary.Append("</td></tr>");
        //quarterly calculation ends here

        ltsummary.Text = htmlsummary.ToString();
        ltdata.Text = html.ToString();
    }
    
    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
    
}
