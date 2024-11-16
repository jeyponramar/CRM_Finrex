using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using WebComponent;
using System.Collections;

/// <summary>
/// Summary description for Commodity
/// </summary>
public class Commodity
{
    private static DateTime _lastCommodityLiveRateRequest = DateTime.Now;
    DataTable _dttblMetal = new DataTable();
	public void Process()
	{
        string action = Common.GetQueryString("a");
        if (action == "u")
        {
            UpdateRate();
            return;
        }
        if (CustomSession.Session("Login_IsLoggedIn") == null) return;
        string m = Common.GetQueryString("m");
        if (m == "tradingsummary")
        {
            BindTradingSummary(false);
        }
        else if (m == "currentyearsummary")
        {
            BindTradingSummary(true);
        }
        else if (m == "bindpricegraph")
        {
            string fromDate = Common.GetQueryString("from");
            string toDate = Common.GetQueryString("to");
            int type = Common.GetQueryStringValue("type");
            HttpContext.Current.Response.Write(GetPriceGraph(fromDate, toDate, type));
        }
        else if (m == "bindmonthsummary")
        {
            string month = Common.GetQueryString("month");
            Array arr = month.Split('-');
            int imonth = Convert.ToInt32(arr.GetValue(0));
            int year = Convert.ToInt32(arr.GetValue(1));
            BindMonthSummary(imonth, year);
        }
        else if (m == "dashboardticker")
        {
            GetDashboardTicker();
        }
        else if (m == "getcommodityliveratechart")
        {
            GetCommodityLiveRate();
        }
        else if (m == "getlmemetalsettlementrates")
        {
            string fromDate = Common.GetQueryString("ctdate");
            HttpContext.Current.Response.Write(GetLMESettlementRateHtml(fromDate));
        }
        else if (m == "getlmestockrates")
        {
            string fromDate = Common.GetQueryString("ctdate");
            HttpContext.Current.Response.Write(GetLMEWarehouseStockHtml(fromDate));
        }
	}
    public static bool IsCommodityEnabled()
    {
        return Finstation.IsMetalCommodityEnabled();
        //int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        //return IsCommodityEnabled(clientId);
    }
    public static bool IsCommodityEnabled(int clientId)
    {
        int subscriptionId = Finstation.GetSubscriptionId(clientId);
        string query = "";
        DataRow dr = null;
        if (subscriptionId > 0)
        {
            query = "select * from tbl_subscriptionprospects " +
                             "join tbl_subscription ON subscription_subscriptionid=subscriptionprospects_subscriptionid " +
                             "WHERE subscriptionprospects_prospectid =6 AND subscription_clientid=" + clientId + " and subscription_subscriptionid=" + subscriptionId;
            dr = DbTable.ExecuteSelectRow(query);
        }
        if (dr == null)
        {
            int trialId = Finstation.GetSubscriptionId(clientId);
            query = "select * from tbl_trialprospect " +
                        "join tbl_trial ON trial_trialid=trialprospect_trialid " +
                        "WHERE trialprospect_prospectid = 6 AND trial_clientid=" + clientId + " and trial_trialid=" + trialId;
            dr = DbTable.ExecuteSelectRow(query);
            if (dr == null) return false;
        }
        return true;
    }
    private string GetMetalName()
    {
        int metalId = Common.GetQueryStringValue("id");
        DataRow dr = DbTable.GetOneRow("tbl_metal", metalId);
        return GlobalUtilities.ConvertToString(dr["metal_metalname"]);
    }
    private int MetalId
    {
        get
        {
            return Common.GetQueryStringValue("id");
        }
    }
    private void BindTradingSummary(bool isCurrentYear)
    {
        StringBuilder html = new StringBuilder();
        string query = "";
        DataRow dr;
        if (isCurrentYear && Common.GetQueryString("dt")!="")
        {
            string dt = GlobalUtilities.ConvertToSqlDate(GlobalUtilities.ConvertToDateFromTextBox(Common.GetQueryString("dt")));
            query = @"select top 1 * from tbl_DailyLMEMetalRate where DailyLMEMetalRate_metalid=" + MetalId +
                    " and cast(DailyLMEMetalRate_date as date)=cast('" + dt + "' as date) order by 1 desc";
            dr = DbTable.ExecuteSelectRow(query);
        }
        else
        {
            query = @"select top 1 * from tbl_DailyLMEMetalRate where DailyLMEMetalRate_metalid=" + MetalId + " order by 1 desc";
            dr = DbTable.ExecuteSelectRow(query);
        }
        if (dr == null)
        {
            HttpContext.Current.Response.Write("Error:No data found.");
            return;
        }
        DateTime dtdate = Convert.ToDateTime(dr["DailyLMEMetalRate_date"]);
        string sqldate = GlobalUtilities.ConvertToSqlDate(dtdate);
        string date = GlobalUtilities.ConvertToDate(dtdate);

        string metalName = GetMetalName();
        double cash = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_cash"]);
        double threemonths = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_threemonths"]);
        double oneyear = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_oneyear"]);
        double cashoffer = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_cashoffer"]);
        double threemonthsoffer = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_threemonthsoffer"]);
        double oneyearoffer = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_oneyearoffer"]);
        double openingstock = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_openingstock"]);
        double livewarrants = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_livewarrants"]);
        double cancelledwarrants = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_cancelledwarrants"]);
        string asiancontract = GlobalUtilities.ConvertToString(dr["DailyLMEMetalRate_asiancontract"]);
        double asignrate = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_asianrate"]);
        string metalNameLower = metalName.ToLower();

        double cashAsk = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_cashask"]);
        double cashbid = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_cashbid"]);
        double threemonthsAsk = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_threemonthsask"]);
        double threemonthsBid = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_threemonthsbid"]);
        double oneyearask = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_oneyearask"]);
        double oneyearbid = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_oneyearbid"]);

        string nextYear = "Dec-" + Convert.ToString(DateTime.Now.Year + 1).Substring(2);
        double exchangeRate = 0;
        double sterlingCash = 0;
        double sterlingThreeMonths = 0;
        double totalopeningstock = livewarrants + cancelledwarrants;
        html.Append("<table width='100%'><tr><td width='40%'>");

        html.Append("<table width='100%' cellspacing=10>");
        html.Append("<tr><td>Data valid for " + dtdate.Day + " " + GlobalUtilities.GetMonthName(dtdate.Month) + " " + dtdate.Year);
        html.Append("<tr><td class='bold'>LME " + metalName + " OFFICIAL PRICES, US$ PER TONNE</td></tr>");
        html.Append("<tr><td>");
        html.Append("<table class='grid-ui' cellpadding='3' border='1'>");
        html.Append("<tr class='grid-ui-header'><td>CONTRACT</td><td>BID (US$ / TONNE)</td><td>OFFER (US$ / TONNE)*</td></tr>");
        html.Append("<tr><td>Cash</td><td>" + cashbid + "</td><td>" + cashAsk + "</td></tr>");
        html.Append("<tr class='grid-ui-row-alt'><td>3-months</td><td>" + threemonthsBid + "</td><td>" + threemonthsAsk + "</td></tr>");
        html.Append("<tr><td>" + nextYear + "</td><td>" + oneyearbid + "</td><td>" + oneyearask + "</td></tr>");
        html.Append("</table>");
        html.Append("</td></tr>");
        html.Append("<tr><td>* LME Official Settlement Price = cash offer</td></tr>");

        html.Append("<tr><td>&nbsp;</td></tr>");

        html.Append("<tr><td class='bold'>LME " + metalName + " OFFICIAL OPENING STOCKS IN TONNES</td></tr>");
        html.Append("<tr><td>");
        html.Append("<table class='grid-ui' cellpadding='3' border='1'>");
        html.Append("<tr class='grid-ui-header'><td>STOCKS</td><td>AMOUNT</td></tr>");
        html.Append("<tr><td>Opening Stock</td><td>" + totalopeningstock + "</td></tr>");
        html.Append("<tr class='grid-ui-row-alt'><td>Live Warrants</td><td>" + livewarrants + "</td></tr>");
        html.Append("<tr><td>Cancelled Warrants</td><td>" + cancelledwarrants + "</td></tr>");
        html.Append("</table>");
        html.Append("</td></tr>");

//        query = @"select * from tbl_MetalCurrencyRate
//                join tbl_MetalCurrency on MetalCurrency_MetalCurrencyid=MetalCurrencyRate_MetalCurrencyid 
//                where cast(MetalCurrencyRate_date as date)=cast('" + sqldate + @"' as date)
//                and metalcurrencyrate_metalid=" + MetalId + @"
//                order by MetalCurrency_MetalCurrencyid";
//        DataTable dttblcurrency = DbTable.ExecuteSelect(query);
//        html.Append("<tr><td class='bold'>LME " + metalName + " SETTLEMENT EXCHANGE RATES</td></tr>");
//        html.Append("<tr><td>");
//        html.Append("<table class='grid-ui' cellpadding='3' border='1'>");
//        html.Append("<tr class='grid-ui-header'><td>CURRENCY</td><td>EXCHANGE RATE</td></tr>");
//        for (int i = 0; i < dttblcurrency.Rows.Count; i++)
//        {
//            string currencyName = GlobalUtilities.ConvertToString(dttblcurrency.Rows[i]["MetalCurrency_currencyname"]);
//            double rate = GlobalUtilities.ConvertToDouble(ExportExposurePortal.DecimalPoint(dttblcurrency.Rows[i]["MetalCurrencyRate_rate"], 4));
//            if (i == 0) exchangeRate = rate;
//            string rowcss = "";
//            if (i % 2 == 0) rowcss = " class='grid-ui-row-alt'";
//            html.Append("<tr" + rowcss + "><td>" + currencyName + "</td><td>" + rate + "</td></tr>");
//        }
//        html.Append("</table>");
//        html.Append("</td></tr>");

//        if (exchangeRate > 0)
//        {
//            sterlingCash = cash / exchangeRate;
//            sterlingThreeMonths = threemonths / exchangeRate;
//        }
//        html.Append("<tr><td class='bold'>LME " + metalName + " STERLING EQUIVALENTS</td></tr>");
//        html.Append("<tr><td>");
//        html.Append("<table class='grid-ui' cellpadding='3' border='1'>");
//        html.Append("<tr class='grid-ui-header'><td>CONTRACT</td><td>AMOUNT</td></tr>");
//        html.Append("<tr><td><span class='capitalize'>" + metalNameLower + "</span> Cash Seller & Settlement</td><td>" + ExportExposurePortal.DecimalPoint(sterlingCash, 2) + "</td></tr>");
//        html.Append("<tr class='grid-ui-row-alt'><td><span class='capitalize'>" + metalNameLower + "</span> 3-months Seller</td><td>" + ExportExposurePortal.DecimalPoint(sterlingThreeMonths, 2) + "</td></tr>");
//        html.Append("</table>");
//        html.Append("</td></tr>");

//        html.Append("<tr><td class='bold'>LME " + metalName + " ASIAN REFERENCE PRICE, US$ PER TONNE</td></tr>");
//        html.Append("<tr><td>");
//        html.Append("<table class='grid-ui' cellpadding='3' border='1'>");
//        html.Append("<tr class='grid-ui-header'><td>METAL</td><td></td><td>PRICE</td></tr>");
//        html.Append("<tr><td class='capitalize'>" + metalNameLower + "</td><td>" + asiancontract + "</td><td>" + asignrate + "</td></tr>");
//        html.Append("</table>");
//        html.Append("</td></tr>");

        html.Append("</table>");

        html.Append("</td><td width='10'>&nbsp;</td><td valign='top'><table width='100%'>");

        if (isCurrentYear)
        {
            html.Append("<tr>");
            html.Append("<td class='box2'><table with='100%' cellpadding=5><tr><td colspan='2'>SELECT DATE FROM CURRENCT CALENDAR YEAR</td></tr>");
            html.Append("<tr><td><input type='text' class='datepicker jq-txtcommodity-ctyearsummary' value='" + date + "'/></td><td><input type='button' class='button jq-btncommodity-ctyearsummary' value='UPDATE'/></td></tr>");
            html.Append("</table></td>");
            html.Append("</tr><tr><td>&nbsp;</td></tr>");
        }

        html.Append("<tr><td class='box' style='height:400px;' valign='top'>");

        html.Append("<table width='100%'>");

        html.Append("<tr><td class='bold'>LME " + metalName + " OFFICIAL PRICES CURVE</td></tr>");
        string priceCurveData = cash + "," + threemonths + "," + oneyear;
        html.Append(@"<tr><td class='chart'><div class='db-chartjs-panel' ct='3' data='" + priceCurveData
                        + "' xaxislabel='Bid' labels='Cash,3-months," + nextYear + "'></div></td></tr>");
        html.Append("</table>");
        html.Append("</td></tr></table>");

        html.Append("</td></tr></table>");

        HttpContext.Current.Response.Write(html.ToString());
    }
    public string GetPriceGraph(string from, string to, int type)
    {
        string query = "";
        from = GlobalUtilities.ConvertToSqlDate(GlobalUtilities.ConvertToDateFromTextBox(from));
        to = GlobalUtilities.ConvertToSqlDate(GlobalUtilities.ConvertToDateFromTextBox(to));
        string column = "cash";
        string lbl = "Cash";
        if (type == 2)
        {
            column = "3months";
            lbl = "3 months";
        }
        query = @"select min(DailyLMEMetalRate_" + column + @") as val,DailyLMEMetalRate_date as lbl from tbl_DailyLMEMetalRate
                where DailyLMEMetalRate_metalid=1
                and cast(DailyLMEMetalRate_date as date)>=cast('" + from + @"' as date)
                and cast(DailyLMEMetalRate_date as date)<=cast('" + to + @"' as date)
                group by DailyLMEMetalRate_date
                order by DailyLMEMetalRate_date";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        string chart = GetChart(dttbl, lbl, 1, 5);
        return chart;
    }
    public string GetChart(DataTable dttbl, string xaxislabel, int labelType, int pointradius)
    {
        StringBuilder vals = new StringBuilder();
        StringBuilder lbls = new StringBuilder();

        Finstation.GetChartData(dttbl, labelType, ref lbls, ref vals);
        string html = "<div class='db-chartjs-panel' ct='3' xaxislabel='" + xaxislabel + "' data='" + vals.ToString() + "' labels='" + lbls.ToString() + "' pointradius='" + pointradius + "'></div>";
        return html;
    }
    
    private void BindMonthSummary(int month, int year)
    {
        string query = "";
        string metalName = GetMetalName();
        string nextYear = "Dec-" + Convert.ToString(DateTime.Now.Year + 1).Substring(2);
        query = @"select top 1 * from tbl_MonthlyLMEMetalRate
                  where year(MonthlyLMEMetalRate_date)=" + year + " and month(MonthlyLMEMetalRate_date)=" + month +
                  " and MonthlyLMEMetalRate_metalid=" + MetalId;
        StringBuilder html = new StringBuilder();
        DataRow dr = DbTable.ExecuteSelectRow(query);
        html.Append("<table width='100%'>");
        html.Append("<tr><td class='bold'>LME " + metalName + " AVERAGES OFFICIAL PRICES US$ PER TONNE</td></tr>");
        html.Append("<tr><td>");
        html.Append("<table class='grid-ui' cellpadding='3' border='1'>");
        html.Append("<tr class='grid-ui-header'><td>CONTRACT</td><td>BID (US$ / TONNE)</td></tr>");
        html.Append("<tr><td>Cash</td><td>" + GlobalUtilities.ConvertToString(dr["MonthlyLMEMetalRate_cash"]) + "</td></tr>");
        html.Append("<tr><td>3 months</td><td>" + GlobalUtilities.ConvertToString(dr["MonthlyLMEMetalRate_threemonths"]) + "</td></tr>");
        html.Append("</table>");
        html.Append("</td></tr>");
        html.Append("</table>");
        HttpContext.Current.Response.Write(html.ToString());
    }
    private void GetCommodityLiveRate()
    {
        string query = "";
        int dateid = Common.GetQueryStringValue("dateid");
        int lastId = Common.GetQueryStringValue("lastid");
        int labelType = 0;
        if (dateid == 0)
        {
            query = @"select MetalLiveRateHistory_MetalLiveRateHistoryid as id, MetalLiveRateHistory_date as lbl,
                    MetalLiveRateHistory_bid as val from tbl_MetalLiveRateHistory 
                    where MetalLiveRateHistory_metalid=" + MetalId + " and MetalLiveRateHistory_bid>0";
            if (lastId > 0)
            {
                query += " and MetalLiveRateHistory_MetalLiveRateHistoryId>" + lastId;
            }
            query += " order by MetalLiveRateHistory_MetalLiveRateHistoryid";
            labelType = 2;
        }
        else
        {
            labelType = 1;
            string startdate = "";
            string enddate = "";
            
           
            if (dateid == 7)
            {
                query = @"select DailyLMEMetalRate_DailyLMEMetalRateid as id, cast(DailyLMEMetalRate_date as date) as lbl,
                    DailyLMEMetalRate_bid as val from tbl_DailyLMEMetalRate 
                    where DailyLMEMetalRate_metalid=" + MetalId;

                if (Common.GetQueryString("sd") != "")
                {
                    startdate = GlobalUtilities.ConvertToSqlDate(GlobalUtilities.ConvertToDateFromTextBox(Common.GetQueryString("sd")));
                    query += " and cast(DailyLMEMetalRate_date as date)>=cast('" + startdate + "' as date)";
                }
                if (Common.GetQueryString("ed") != "")
                {
                    enddate = GlobalUtilities.ConvertToSqlDate(GlobalUtilities.ConvertToDateFromTextBox(Common.GetQueryString("ed")));
                    query += " and cast(DailyLMEMetalRate_date as date)<=cast('" + enddate + "' as date)";
                }
            }
            else
            {
                int top = 0;
                if (dateid == 1) //5D
                {
                    top = 5;
                }
                else if (dateid == 2) //1M
                {
                    top = 30;
                }
                else if (dateid == 3) //3M
                {
                    top = 90;
                }
                else if (dateid == 4) //6M
                {
                    top = 180;
                }
                else if (dateid == 5) //1Y
                {
                    top = 365;
                }
                else if (dateid == 6) //5Y
                {
                    top = 1825;
                }
                query = @"select DailyLMEMetalRate_DailyLMEMetalRateid as id, cast(DailyLMEMetalRate_date as date) as lbl,
                        DailyLMEMetalRate_close as val from(
                        select top "+top+" * from tbl_DailyLMEMetalRate where DailyLMEMetalRate_metalid=" + MetalId + @" order by DailyLMEMetalRate_date desc
                        )r order by DailyLMEMetalRate_date";
            }
        }

        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder lbls = new StringBuilder();
        StringBuilder vals = new StringBuilder();
        Finstation.GetChartData(dttbl, labelType, ref lbls, ref vals);
        if(dttbl.Rows.Count > 0) lastId = GlobalUtilities.ConvertToInt(dttbl.Rows[dttbl.Rows.Count - 1]["id"]);
        string json = "{\"lbls\":\"" + lbls.ToString() + "\",\"vals\":\"" + vals.ToString() + "\",\"lastid\":\"" + lastId + "\"}";
        HttpContext.Current.Response.Write(json);
    }
    public void UpdateRate()
    {
        //ErrorLog.WriteLog(Environment.NewLine + "Update Start At:" + DateTime.Now);
        //ErrorLog.WriteLog("rtd=" + HttpContext.Current.Request["rtd"] + "&v=" + HttpContext.Current.Request["v"]);
        //string rtds = global.CheckInputData(HttpContext.Current.Request["rtd"]).Replace("___", "#");
        //string rtds = "17#3#1#5949#EURINRCOMP#Open,17#1#1#6069#USDINRCOMP#Bid,17#3#1#5949#EURINRCOMP#Bid,17#3#1#6031#GBPINRCOMP#Bid,17#3#1#5978#JPYINRCOMP#Bid,17#1#1#6069#USDINRCOMP#Ask,17#3#1#5949#EURINRCOMP#Ask,17#3#1#6031#GBPINRCOMP#Ask,17#3#1#5978#JPYINRCOMP#Ask,17#1#1#6069#USDINRCOMP#%Chg,17#3#1#5949#EURINRCOMP#%Chg,17#3#1#6031#GBPINRCOMP#%Chg,17#3#1#5978#JPYINRCOMP#%Chg,17#1#1#6069#USDINRCOMP#High,17#3#1#5949#EURINRCOMP#High,17#3#1#6031#GBPINRCOMP#High,17#3#1#5978#JPYINRCOMP#High,17#1#1#6069#USDINRCOMP#Low,17#3#1#5949#EURINRCOMP#Low,17#3#1#6031#GBPINRCOMP#Low,17#3#1#5978#JPYINRCOMP#Low,17#1#1#6069#USDINRCOMP#Open,17#3#1#5949#EURINRCOMP#Open,17#3#1#6031#GBPINRCOMP#Open,17#3#1#5978#JPYINRCOMP#Open,17#1#1#6069#USDINRCOMP#Close,17#3#1#5949#EURINRCOMP#Close,17#3#1#6031#GBPINRCOMP#Close,17#3#1#5978#JPYINRCOMP#Close,17#1#1#6069#USDINRCOMP#52WkHigh,12#2#1#5949#SEURINR#52WkHigh,12#2#1#6031#SGBPINR#52WkHigh,12#2#1#5978#SJPYINR#52WkHigh,17#1#1#6069#USDINRCOMP#52WkLow,12#2#1#5949#SEURINR#52WkLow,12#2#1#6031#SGBPINR#52WkLow,12#2#1#5978#SJPYINR#52WkLow,3#1#3#658#LTP,4#1#2#3556#LTP,42#2#1#6219#2#LTP,42#4#1#6217#4#LTP,42#36#1#6222#36#LTP,42#1#1#6253#1#LTP,3#1#3#658#NetChg,4#1#2#3556#NetChg,42#2#1#6219#2#NetChg,42#4#1#6217#4#NetChg,42#36#1#6222#36#NetChg,42#1#1#6253#1#NetChg,3#1#3#658#%Chg,4#1#2#3556#%Chg,42#2#1#6219#2#%Chg,42#4#1#6217#4#%Chg,42#36#1#6222#36#%Chg,42#1#1#6253#1#%Chg,47#5#1#5629#USD1M.LIBOR#LTP,47#5#1#5629#EUR1M.LIBOR#LTP,47#5#1#5629#GBP1M.LIBOR#LTP,47#5#1#5629#JPY1M.LIBOR#LTP,47#5#1#5629#USD3M.LIBOR#LTP,47#5#1#5629#EUR3M.LIBOR#LTP,47#5#1#5629#GBP3M.LIBOR#LTP,47#5#1#5629#JPY3M.LIBOR#LTP,47#5#1#5629#USD6M.LIBOR#LTP,47#5#1#5629#EUR6M.LIBOR#LTP,47#5#1#5629#GBP6M.LIBOR#LTP,47#5#1#5629#JPY6M.LIBOR#LTP,47#5#1#5629#USD1Y.LIBOR#LTP,47#5#1#5629#EUR1Y.LIBOR#LTP,47#5#1#5629#GBP1Y.LIBOR#LTP,47#5#1#5629#JPY1Y.LIBOR#LTP,12#2#1#5949#SEURUSD#Bid,12#2#1#6031#SGBPUSD#Bid,12#2#1#6069#SUSDJPY#Bid,12#2#1#5903#SAUDUSD#Bid,12#2#1#6069#SUSDCAD#Bid,12#2#1#5949#SEURUSD#Ask,12#2#1#6031#SGBPUSD#Ask,12#2#1#6069#SUSDJPY#Ask,12#2#1#5903#SAUDUSD#Ask,12#2#1#6069#SUSDCAD#Ask,12#2#1#5949#SEURUSD#%Chg,12#2#1#6031#SGBPUSD#%Chg,12#2#1#6069#SUSDJPY#%Chg,12#2#1#5903#SAUDUSD#%Chg,12#2#1#6069#SUSDCAD#%Chg,12#2#1#5949#SEURUSD#High,12#2#1#6031#SGBPUSD#High,12#2#1#6069#SUSDJPY#High,12#2#1#5903#SAUDUSD#High,12#2#1#6069#SUSDCAD#High,12#2#1#5949#SEURUSD#Low,12#2#1#6031#SGBPUSD#Low,12#2#1#6069#SUSDJPY#Low,12#2#1#5903#SAUDUSD#Low,12#2#1#6069#SUSDCAD#Low,12#2#1#5949#SEURUSD#Open,12#2#1#6031#SGBPUSD#Open,12#2#1#6069#SUSDJPY#Open,12#2#1#5903#SAUDUSD#Open,12#2#1#6069#SUSDCAD#Open,12#2#1#5949#SEURUSD#Close,12#2#1#6031#SGBPUSD#Close,12#2#1#6069#SUSDJPY#Close,12#2#1#5903#SAUDUSD#Close,12#2#1#6069#SUSDCAD#Close,12#2#1#5949#SEURUSD#52WkHigh,12#2#1#6031#SGBPUSD#52WkHigh,12#2#1#6069#SUSDJPY#52WkHigh,12#2#1#5903#SAUDUSD#52WkHigh,12#2#1#6069#SUSDCAD#52WkHigh,12#2#1#5949#SEURUSD#52WkLow,12#2#1#6031#SGBPUSD#52WkLow,12#2#1#6069#SUSDJPY#52WkLow,12#2#1#5903#SAUDUSD#52WkLow,12#2#1#6069#SUSDCAD#52WkLow,12#2#1#6069#SUSDSGD#Bid,12#2#1#6069#SUSDHKD#Bid,12#2#1#6069#SUSDTWD#Bid,12#2#1#6069#SUSDKRW#Bid,12#2#1#6069#SUSDPHP#Bid,12#2#1#6069#SUSDIDR#Bid,12#2#1#6069#SUSDCNY#Bid,12#2#1#6069#SUSDMYR#Bid,12#2#1#6069#SUSDTHB#Bid,12#2#1#6069#SUSDSGD#Ask,12#2#1#6069#SUSDHKD#Ask,12#2#1#6069#SUSDTWD#Ask,12#2#1#6069#SUSDKRW#Ask,12#2#1#6069#SUSDPHP#Ask,12#2#1#6069#SUSDIDR#Ask,12#2#1#6069#SUSDCNY#Ask,12#2#1#6069#SUSDMYR#Ask,12#2#1#6069#SUSDTHB#Ask,12#2#1#6069#SUSDSGD#%Chg,12#2#1#6069#SUSDHKD#%Chg,12#2#1#6069#SUSDTWD#%Chg,12#2#1#6069#SUSDKRW#%Chg,12#2#1#6069#SUSDPHP#%Chg,12#2#1#6069#SUSDIDR#%Chg,12#2#1#6069#SUSDCNY#%Chg,12#2#1#6069#SUSDMYR#%Chg,12#2#1#6069#SUSDTHB#%Chg,12#2#1#6069#SUSDSGD#High,12#2#1#6069#SUSDHKD#High,12#2#1#6069#SUSDTWD#High,12#2#1#6069#SUSDKRW#High,12#2#1#6069#SUSDPHP#High,12#2#1#6069#SUSDIDR#High,12#2#1#6069#SUSDCNY#High,12#2#1#6069#SUSDMYR#High,12#2#1#6069#SUSDTHB#High,12#2#1#6069#SUSDSGD#Low,12#2#1#6069#SUSDHKD#Low,12#2#1#6069#SUSDTWD#Low,12#2#1#6069#SUSDKRW#Low,12#2#1#6069#SUSDPHP#Low,12#2#1#6069#SUSDIDR#Low,12#2#1#6069#SUSDCNY#Low,12#2#1#6069#SUSDMYR#Low,12#2#1#6069#SUSDTHB#Low,12#2#1#6069#SUSDSGD#Open,12#2#1#6069#SUSDHKD#Open,12#2#1#6069#SUSDTWD#Open,12#2#1#6069#SUSDKRW#Open,12#2#1#6069#SUSDPHP#Open,12#2#1#6069#SUSDIDR#Open,12#2#1#6069#SUSDCNY#Open,12#2#1#6069#SUSDMYR#Open,12#2#1#6069#SUSDTHB#Open,12#2#1#6069#SUSDSGD#Close,12#2#1#6069#SUSDHKD#Close,12#2#1#6069#SUSDTWD#Close,12#2#1#6069#SUSDKRW#Close,12#2#1#6069#SUSDPHP#Close,12#2#1#6069#SUSDIDR#Close,12#2#1#6069#SUSDCNY#Close,12#2#1#6069#SUSDMYR#Close,12#2#1#6069#SUSDTHB#Close,12#2#1#6069#SUSDSGD#52WkHigh,12#2#1#6069#SUSDHKD#52WkHigh,12#2#1#6069#SUSDTWD#52WkHigh,12#2#1#6069#SUSDKRW#52WkHigh,12#2#1#6069#SUSDPHP#52WkHigh,12#2#1#6069#SUSDIDR#52WkHigh,12#2#1#6069#SUSDCNY#52WkHigh,12#2#1#6069#SUSDMYR#52WkHigh,12#2#1#6069#SUSDTHB#52WkHigh,12#2#1#6069#SUSDSGD#52WkLow,12#2#1#6069#SUSDHKD#52WkLow,12#2#1#6069#SUSDTWD#52WkLow,12#2#1#6069#SUSDKRW#52WkLow,12#2#1#6069#SUSDPHP#52WkLow,12#2#1#6069#SUSDIDR#52WkLow,12#2#1#6069#SUSDCNY#52WkLow,12#2#1#6069#SUSDMYR#52WkLow,12#2#1#6069#SUSDTHB#52WkLow,35#1#1#6069#FUTCUR#N1#0#XX#Bid,35#1#1#6069#FUTCUR#1127865600#0#XX#Bid,35#1#1#6069#FUTCUR#1130457600#0#XX#Bid,35#1#1#6069#FUTCUR#N1#0#XX#Ask,35#1#1#6069#FUTCUR#1127865600#0#XX#Ask,35#1#1#6069#FUTCUR#1130457600#0#XX#Ask,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate2,17#2#1#6069#USDINRCOMP CASH/SPOT#Ask,17#2#1#6069#USDINRCOMP CASH/SPOT#Bid,FWRC1#USDINR#Month End Rates#0#S#Desc.4,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate4,FWRC1#USDINR#Month End Rates#0#BidPrice4,FWRC1#USDINR#Month End Rates#0#AskPrice4,FWRC1#USDINR#Month End Rates#0#S#Desc.5,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate5,FWRC1#USDINR#Month End Rates#0#BidPrice5,FWRC1#USDINR#Month End Rates#0#AskPrice5,FWRC1#USDINR#Month End Rates#0#S#Desc.6,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate6,FWRC1#USDINR#Month End Rates#0#BidPrice6,FWRC1#USDINR#Month End Rates#0#AskPrice6,FWRC1#USDINR#Month End Rates#0#S#Desc.7,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate7,FWRC1#USDINR#Month End Rates#0#BidPrice7,FWRC1#USDINR#Month End Rates#0#AskPrice7,FWRC1#USDINR#Month End Rates#0#S#Desc.8,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate8,FWRC1#USDINR#Month End Rates#0#BidPrice8,FWRC1#USDINR#Month End Rates#0#AskPrice8,FWRC1#USDINR#Month End Rates#0#S#Desc.9,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate9,FWRC1#USDINR#Month End Rates#0#BidPrice9,FWRC1#USDINR#Month End Rates#0#AskPrice9,FWRC1#USDINR#Month End Rates#0#S#Desc.10,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate10,FWRC1#USDINR#Month End Rates#0#BidPrice10,FWRC1#USDINR#Month End Rates#0#AskPrice10,FWRC1#USDINR#Month End Rates#0#S#Desc.11,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate11,FWRC1#USDINR#Month End Rates#0#BidPrice11,FWRC1#USDINR#Month End Rates#0#AskPrice11,FWRC1#USDINR#Month End Rates#0#S#Desc.12,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate12,FWRC1#USDINR#Month End Rates#0#BidPrice12,FWRC1#USDINR#Month End Rates#0#AskPrice12,FWRC1#USDINR#Month End Rates#0#S#Desc.13,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate13,FWRC1#USDINR#Month End Rates#0#BidPrice13,FWRC1#USDINR#Month End Rates#0#AskPrice13,FWRC1#USDINR#Month End Rates#0#S#Desc.14,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate14,FWRC1#USDINR#Month End Rates#0#BidPrice14,FWRC1#USDINR#Month End Rates#0#AskPrice14,FWRC1#USDINR#Month End Rates#0#S#Desc.15,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate15,FWRC1#USDINR#Month End Rates#0#BidPrice15,FWRC1#USDINR#Month End Rates#0#AskPrice15,17#1#1#6069#USDINRCOMP#Bid,17#1#1#6069#USDINRCOMP#Ask,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate0,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid4,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk4,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid5,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk5,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid6,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk6,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid7,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk7,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid8,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk8,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid9,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk9,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid10,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk10,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid11,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk11,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid12,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk12,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid13,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk13,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid14,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk14,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid15,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk15,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate2,17#2#1#6069#USDINRCOMP CASH/SPOT#Ask,17#2#1#6069#USDINRCOMP CASH/SPOT#Bid,FWRC1#EURINR#Month Wise Rates#0#S#Desc.7,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate7,FWRC1#USDINR#Month Wise Rates#0#BidPrice7,FWRC1#USDINR#Month Wise Rates#0#AskPrice7,FWRC1#EURINR#Month Wise Rates#0#S#Desc.8,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate8,FWRC1#USDINR#Month Wise Rates#0#BidPrice8,FWRC1#USDINR#Month Wise Rates#0#AskPrice8,FWRC1#EURINR#Month Wise Rates#0#S#Desc.9,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate9,FWRC1#USDINR#Month Wise Rates#0#BidPrice9,FWRC1#USDINR#Month Wise Rates#0#AskPrice9,FWRC1#EURINR#Month Wise Rates#0#S#Desc.10,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate10,FWRC1#USDINR#Month Wise Rates#0#BidPrice10,FWRC1#USDINR#Month Wise Rates#0#AskPrice10,FWRC1#EURINR#Month Wise Rates#0#S#Desc.11,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate11,FWRC1#USDINR#Month Wise Rates#0#BidPrice11,FWRC1#USDINR#Month Wise Rates#0#AskPrice11,FWRC1#EURINR#Month Wise Rates#0#S#Desc.12,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate12,FWRC1#USDINR#Month Wise Rates#0#BidPrice12,FWRC1#USDINR#Month Wise Rates#0#AskPrice12,FWRC1#EURINR#Month Wise Rates#0#S#Desc.13,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate13,FWRC1#USDINR#Month Wise Rates#0#BidPrice13,FWRC1#USDINR#Month Wise Rates#0#AskPrice13,FWRC1#EURINR#Month Wise Rates#0#S#Desc.14,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate14,FWRC1#USDINR#Month Wise Rates#0#BidPrice14,FWRC1#USDINR#Month Wise Rates#0#AskPrice14,FWRC1#EURINR#Month Wise Rates#0#S#Desc.15,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate15,FWRC1#USDINR#Month Wise Rates#0#BidPrice15,FWRC1#USDINR#Month Wise Rates#0#AskPrice15,FWRC1#EURINR#Month Wise Rates#0#S#Desc.16,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate16,FWRC1#USDINR#Month Wise Rates#0#BidPrice16,FWRC1#USDINR#Month Wise Rates#0#AskPrice16,FWRC1#EURINR#Month Wise Rates#0#S#Desc.17,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate17,FWRC1#USDINR#Month Wise Rates#0#BidPrice17,FWRC1#USDINR#Month Wise Rates#0#AskPrice17,FWRC1#EURINR#Month Wise Rates#0#S#Desc.18,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate18,FWRC1#USDINR#Month Wise Rates#0#BidPrice18,FWRC1#USDINR#Month Wise Rates#0#AskPrice18,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid7,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk7,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid8,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk8,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid9,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk9,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid10,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk10,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid11,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk11,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid12,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk12,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid13,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk13,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid14,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk14,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid15,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk15,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid16,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk17,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid17,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk17,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid18,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk18,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate2,FWRC1#EURINR#Month Wise Rates#0#BidPrice2,FWRC1#EURINR#Month Wise Rates#0#AskPrice2,FWRC1#EURINR#Month Wise Rates#0#S#Desc.7,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate7,FWRC1#EURINR#Month Wise Rates#0#BidPrice7,FWRC1#EURINR#Month Wise Rates#0#AskPrice7,FWRC1#EURINR#Month Wise Rates#0#S#Desc.8,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate8,FWRC1#EURINR#Month Wise Rates#0#BidPrice8,FWRC1#EURINR#Month Wise Rates#0#AskPrice8,FWRC1#EURINR#Month Wise Rates#0#S#Desc.9,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate9,FWRC1#EURINR#Month Wise Rates#0#BidPrice9,FWRC1#EURINR#Month Wise Rates#0#AskPrice9,FWRC1#EURINR#Month Wise Rates#0#S#Desc.10,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate10,FWRC1#EURINR#Month Wise Rates#0#BidPrice10,FWRC1#EURINR#Month Wise Rates#0#AskPrice10,FWRC1#EURINR#Month Wise Rates#0#S#Desc.11,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate11,FWRC1#EURINR#Month Wise Rates#0#BidPrice11,FWRC1#EURINR#Month Wise Rates#0#AskPrice11,FWRC1#EURINR#Month Wise Rates#0#S#Desc.12,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate12,FWRC1#EURINR#Month Wise Rates#0#BidPrice12,FWRC1#EURINR#Month Wise Rates#0#AskPrice12,FWRC1#EURINR#Month Wise Rates#0#S#Desc.13,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate13,FWRC1#EURINR#Month Wise Rates#0#BidPrice13,FWRC1#EURINR#Month Wise Rates#0#AskPrice13,FWRC1#EURINR#Month Wise Rates#0#S#Desc.14,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate14,FWRC1#EURINR#Month Wise Rates#0#BidPrice14,FWRC1#EURINR#Month Wise Rates#0#AskPrice14,FWRC1#EURINR#Month Wise Rates#0#S#Desc.15,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate15,FWRC1#EURINR#Month Wise Rates#0#BidPrice15,FWRC1#EURINR#Month Wise Rates#0#AskPrice15,FWRC1#EURINR#Month Wise Rates#0#S#Desc.16,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate16,FWRC1#EURINR#Month Wise Rates#0#BidPrice16,FWRC1#EURINR#Month Wise Rates#0#AskPrice16,FWRC1#EURINR#Month Wise Rates#0#S#Desc.17,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate17,FWRC1#EURINR#Month Wise Rates#0#BidPrice17,FWRC1#EURINR#Month Wise Rates#0#AskPrice17,FWRC1#EURINR#Month Wise Rates#0#S#Desc.18,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate18,FWRC1#EURINR#Month Wise Rates#0#BidPrice18,FWRC1#EURINR#Month Wise Rates#0#AskPrice18,17#3#1#5949#EURINRCOMP#Bid,17#3#1#5949#EURINRCOMP#Ask,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate0,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid7,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk7,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid8,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk8,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid9,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk9,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid10,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk10,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid11,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk11,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid12,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk12,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid13,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk13,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid14,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk14,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid15,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk15,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid16,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk16,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid17,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk17,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid18,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk18,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate2,FWRC1#GBPINR#Month Wise Rates#0#BidPrice2,FWRC1#GBPINR#Month Wise Rates#0#AskPrice2,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.7,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate7,FWRC1#GBPINR#Month Wise Rates#0#BidPrice7,FWRC1#GBPINR#Month Wise Rates#0#AskPrice7,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.8,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate8,FWRC1#GBPINR#Month Wise Rates#0#BidPrice8,FWRC1#GBPINR#Month Wise Rates#0#AskPrice8,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.9,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate9,FWRC1#GBPINR#Month Wise Rates#0#BidPrice9,FWRC1#GBPINR#Month Wise Rates#0#AskPrice9,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.10,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate10,FWRC1#GBPINR#Month Wise Rates#0#BidPrice10,FWRC1#GBPINR#Month Wise Rates#0#AskPrice10,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.11,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate11,FWRC1#GBPINR#Month Wise Rates#0#BidPrice11,FWRC1#GBPINR#Month Wise Rates#0#AskPrice11,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.12,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate12,FWRC1#GBPINR#Month Wise Rates#0#BidPrice12,FWRC1#GBPINR#Month Wise Rates#0#AskPrice12,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.13,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate13,FWRC1#GBPINR#Month Wise Rates#0#BidPrice13,FWRC1#GBPINR#Month Wise Rates#0#AskPrice13,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.14,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate14,FWRC1#GBPINR#Month Wise Rates#0#BidPrice14,FWRC1#GBPINR#Month Wise Rates#0#AskPrice14,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.15,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate15,FWRC1#GBPINR#Month Wise Rates#0#BidPrice15,FWRC1#GBPINR#Month Wise Rates#0#AskPrice15,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.16,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate16,FWRC1#GBPINR#Month Wise Rates#0#BidPrice16,FWRC1#GBPINR#Month Wise Rates#0#AskPrice16,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.17,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate17,FWRC1#GBPINR#Month Wise Rates#0#BidPrice17,FWRC1#GBPINR#Month Wise Rates#0#AskPrice17,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.18,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate18,FWRC1#GBPINR#Month Wise Rates#0#BidPrice18,FWRC1#GBPINR#Month Wise Rates#0#AskPrice18,17#3#1#6031#GBPINRCOMP#Bid,17#3#1#6031#GBPINRCOMP#Ask,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate0,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid7,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk7,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid8,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk8,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid9,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk9,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid10,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk10,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid11,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk11,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid12,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk12,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid13,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk13,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid14,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk14,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid15,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk15,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid16,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk16,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid17,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk17,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid18,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk18,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.2,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate2,FWRC1#JPYINR#Month Wise Rates#0#BidPrice2,FWRC1#JPYINR#Month Wise Rates#0#AskPrice2,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.7,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate7,FWRC1#JPYINR#Month Wise Rates#0#BidPrice7,FWRC1#JPYINR#Month Wise Rates#0#AskPrice7,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.8,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate8,FWRC1#JPYINR#Month Wise Rates#0#BidPrice8,FWRC1#JPYINR#Month Wise Rates#0#AskPrice8,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.9,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate9,FWRC1#JPYINR#Month Wise Rates#0#BidPrice9,FWRC1#JPYINR#Month Wise Rates#0#AskPrice9,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.10,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate10,FWRC1#JPYINR#Month Wise Rates#0#BidPrice10,FWRC1#JPYINR#Month Wise Rates#0#AskPrice10,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.11,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate11,FWRC1#JPYINR#Month Wise Rates#0#BidPrice11,FWRC1#JPYINR#Month Wise Rates#0#AskPrice11,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.12,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate12,FWRC1#JPYINR#Month Wise Rates#0#BidPrice12,FWRC1#JPYINR#Month Wise Rates#0#AskPrice12,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.13,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate13,FWRC1#JPYINR#Month Wise Rates#0#BidPrice13,FWRC1#JPYINR#Month Wise Rates#0#AskPrice13,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.14,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate14,FWRC1#JPYINR#Month Wise Rates#0#BidPrice14,FWRC1#JPYINR#Month Wise Rates#0#AskPrice14,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.15,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate15,FWRC1#JPYINR#Month Wise Rates#0#BidPrice15,FWRC1#JPYINR#Month Wise Rates#0#AskPrice15,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.16,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate16,FWRC1#JPYINR#Month Wise Rates#0#BidPrice16,FWRC1#JPYINR#Month Wise Rates#0#AskPrice16,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.17,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate17,FWRC1#JPYINR#Month Wise Rates#0#BidPrice17,FWRC1#JPYINR#Month Wise Rates#0#AskPrice17,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.18,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate18,FWRC1#JPYINR#Month Wise Rates#0#BidPrice18,FWRC1#JPYINR#Month Wise Rates#0#AskPrice18,17#3#1#5978#JPYINRCOMP#Bid,17#3#1#5978#JPYINRCOMP#Ask,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate0,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid7,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk7,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid8,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk8,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid9,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk9,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid10,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk10,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid11,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk11,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid12,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk12,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid13,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk13,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid14,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk14,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid15,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk15,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid16,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk16,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid17,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk17,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid18,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk18,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.2,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate2,FWRC1#EURUSD#Month Wise Rates#0#BidPrice2,FWRC1#EURUSD#Month Wise Rates#0#AskPrice2,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.7,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate7,FWRC1#EURUSD#Month Wise Rates#0#BidPrice7,FWRC1#EURUSD#Month Wise Rates#0#AskPrice7,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.8,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate8,FWRC1#EURUSD#Month Wise Rates#0#BidPrice8,FWRC1#EURUSD#Month Wise Rates#0#AskPrice8,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.9,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate9,FWRC1#EURUSD#Month Wise Rates#0#BidPrice9,FWRC1#EURUSD#Month Wise Rates#0#AskPrice9,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.10,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate10,FWRC1#EURUSD#Month Wise Rates#0#BidPrice10,FWRC1#EURUSD#Month Wise Rates#0#AskPrice10,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.11,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate11,FWRC1#EURUSD#Month Wise Rates#0#BidPrice11,FWRC1#EURUSD#Month Wise Rates#0#AskPrice11,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.12,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate12,FWRC1#EURUSD#Month Wise Rates#0#BidPrice12,FWRC1#EURUSD#Month Wise Rates#0#AskPrice12,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.13,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate13,FWRC1#EURUSD#Month Wise Rates#0#BidPrice13,FWRC1#EURUSD#Month Wise Rates#0#AskPrice13,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.14,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate14,FWRC1#EURUSD#Month Wise Rates#0#BidPrice14,FWRC1#EURUSD#Month Wise Rates#0#AskPrice14,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.15,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate15,FWRC1#EURUSD#Month Wise Rates#0#BidPrice15,FWRC1#EURUSD#Month Wise Rates#0#AskPrice15,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.16,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate16,FWRC1#EURUSD#Month Wise Rates#0#BidPrice16,FWRC1#EURUSD#Month Wise Rates#0#AskPrice16,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.17,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate17,FWRC1#EURUSD#Month Wise Rates#0#BidPrice17,FWRC1#EURUSD#Month Wise Rates#0#AskPrice17,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.18,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate18,FWRC1#EURUSD#Month Wise Rates#0#BidPrice18,FWRC1#EURUSD#Month Wise Rates#0#AskPrice18,FWRC1#EURUSD#Month Wise Rates#0#BidPrice0,FWRC1#EURUSD#Month Wise Rates#0#AskPrice0,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate0,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid2,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk2,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid7,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk7,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid8,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk8,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid9,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk9,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid10,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk10,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid11,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk11,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid12,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk12,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid13,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk13,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid14,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk14,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid15,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk15,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid16,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk16,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid17,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk17,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid18,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk18,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.2,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate2,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice2,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice2,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.7,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate7,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice7,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice7,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.8,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate8,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice8,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice8,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.9,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate9,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice9,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice9,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.10,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate10,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice10,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice10,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.11,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate11,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice11,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice11,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.12,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate12,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice12,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice12,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.13,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate13,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice13,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice13,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.14,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate14,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice14,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice14,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.15,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate15,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice15,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice15,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.16,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate16,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice16,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice16,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.17,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate17,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice17,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice17,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.18,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate18,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice18,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice18,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice0,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice0,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate0,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid2,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk2,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid7,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk7,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid8,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk8,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid9,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk9,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid10,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk10,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid11,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk11,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid12,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk12,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid13,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk13,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid14,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk14,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid15,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk15,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid16,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk16,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid17,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk17,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid18,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk18,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.2,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate2,FWRC1#USDJPY#Month Wise Rates#0#BidPrice2,FWRC1#USDJPY#Month Wise Rates#0#AskPrice2,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.7,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate7,FWRC1#USDJPY#Month Wise Rates#0#BidPrice7,FWRC1#USDJPY#Month Wise Rates#0#AskPrice7,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.8,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate8,FWRC1#USDJPY#Month Wise Rates#0#BidPrice8,FWRC1#USDJPY#Month Wise Rates#0#AskPrice8,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.9,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate9,FWRC1#USDJPY#Month Wise Rates#0#BidPrice9,FWRC1#USDJPY#Month Wise Rates#0#AskPrice9,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.10,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate10,FWRC1#USDJPY#Month Wise Rates#0#BidPrice10,FWRC1#USDJPY#Month Wise Rates#0#AskPrice10,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.11,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate11,FWRC1#USDJPY#Month Wise Rates#0#BidPrice11,FWRC1#USDJPY#Month Wise Rates#0#AskPrice11,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.12,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate12,FWRC1#USDJPY#Month Wise Rates#0#BidPrice12,FWRC1#USDJPY#Month Wise Rates#0#AskPrice12,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.13,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate13,FWRC1#USDJPY#Month Wise Rates#0#BidPrice13,FWRC1#USDJPY#Month Wise Rates#0#AskPrice13,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.14,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate14,FWRC1#USDJPY#Month Wise Rates#0#BidPrice14,FWRC1#USDJPY#Month Wise Rates#0#AskPrice14,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.15,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate15,FWRC1#USDJPY#Month Wise Rates#0#BidPrice15,FWRC1#USDJPY#Month Wise Rates#0#AskPrice15,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.16,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate16,FWRC1#USDJPY#Month Wise Rates#0#BidPrice16,FWRC1#USDJPY#Month Wise Rates#0#AskPrice16,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.17,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate17,FWRC1#USDJPY#Month Wise Rates#0#BidPrice17,FWRC1#USDJPY#Month Wise Rates#0#AskPrice17,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.18,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate18,FWRC1#USDJPY#Month Wise Rates#0#BidPrice18,FWRC1#USDJPY#Month Wise Rates#0#AskPrice18,FWRC1#USDJPY#Month Wise Rates#0#BidPrice0,FWRC1#USDJPY#Month Wise Rates#0#AskPrice0,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate0,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid2,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk2,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid7,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk7,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid8,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk8,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid9,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk9,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid10,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk10,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid11,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk11,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid12,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk12,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid13,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk13,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid14,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk14,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid15,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk15,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid16,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk16,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid17,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk17,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid18,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk18,FWRC1#USDINR#Month Wise Rates#0#FwdOutrightAsk16,35#1#1#6069#FUTCUR#N1#0#XX#Bid,35#1#1#6069#FUTCUR#N2#0#XX#Bid,35#1#1#6069#FUTCUR#N3#0#XX#Bid,35#1#1#6069#FUTCUR#N1#0#XX#Ask,35#1#1#6069#FUTCUR#N2#0#XX#Ask,35#1#1#6069#FUTCUR#N3#0#XX#Ask,35#1#1#5949#FUTCUR#N1#0#XX#Bid,35#1#1#5949#FUTCUR#N2#0#XX#Bid,35#1#1#5949#FUTCUR#N3#0#XX#Bid,35#1#1#5949#FUTCUR#N1#0#XX#Ask,35#1#1#5949#FUTCUR#N2#0#XX#Ask,35#1#1#5949#FUTCUR#N3#0#XX#Ask,35#1#1#6031#FUTCUR#N1#0#XX#Bid,35#1#1#6031#FUTCUR#N2#0#XX#Bid,35#1#1#6031#FUTCUR#N3#0#XX#Bid,35#1#1#6031#FUTCUR#N1#0#XX#Ask,35#1#1#6031#FUTCUR#N2#0#XX#Ask,35#1#1#6031#FUTCUR#N3#0#XX#Ask,35#1#1#5978#FUTCUR#N1#0#XX#Bid,35#1#1#5978#FUTCUR#N2#0#XX#Bid,35#1#1#5978#FUTCUR#N3#0#XX#Bid,35#1#1#5978#FUTCUR#N1#0#XX#Ask,35#1#1#5978#FUTCUR#N2#0#XX#Ask,35#1#1#5978#FUTCUR#N3#0#XX#Ask,FWRC1#USDINR#Month Wise Rates#1#Bid%7,FWRC1#USDINR#Month Wise Rates#1#Bid%9,FWRC1#USDINR#Month Wise Rates#1#Bid%12,FWRC1#USDINR#Month Wise Rates#1#Bid%18,FWRC1#EURINR#Month Wise Rates#1#Bid%7,FWRC1#EURINR#Month Wise Rates#1#Bid%9,FWRC1#EURINR#Month Wise Rates#1#Bid%12,FWRC1#EURINR#Month Wise Rates#1#Bid%18,FWRC1#GBPINR#Month Wise Rates#1#Bid%7,FWRC1#GBPINR#Month Wise Rates#1#Bid%10,FWRC1#GBPINR#Month Wise Rates#1#Bid%12,FWRC1#GBPINR#Month Wise Rates#1#Bid%18,FWRC1#JPYINR#Month Wise Rates#1#Bid%8,FWRC1#JPYINR#Month Wise Rates#1#Bid%9,FWRC1#JPYINR#Month Wise Rates#1#Bid%12,FWRC1#JPYINR#Month Wise Rates#1#Bid%18,FWRC1#USDINR#Month Wise Rates#1#Bid%7,FWRC1#USDINR#Month Wise Rates#1#Bid%9,FWRC1#USDINR#Month Wise Rates#1#Bid%12,FWRC1#USDINR#Month Wise Rates#1#Bid%12,FWRC1#USDINR#Month Wise Rates#1#Bid%18,FWRC1#EURINR#Month Wise Rates#1#Bid%7,FWRC1#EURINR#Month Wise Rates#1#Bid%9,FWRC1#EURINR#Month Wise Rates#1#Bid%12,FWRC1#EURINR#Month Wise Rates#1#Bid%18,FWRC1#GBPINR#Month Wise Rates#1#Bid%7,FWRC1#GBPINR#Month Wise Rates#1#Bid%10,FWRC1#GBPINR#Month Wise Rates#1#Bid%12,FWRC1#GBPINR#Month Wise Rates#1#Bid%18,FWRC1#JPYINR#Month Wise Rates#1#Bid%8,FWRC1#JPYINR#Month Wise Rates#1#Bid%9,FWRC1#JPYINR#Month Wise Rates#1#Bid%12,FWRC1#JPYINR#Month Wise Rates#1#Bid%18,FWRC1#USDINR#Month Wise Rates#1#Bid%7,FWRC1#USDINR#Month Wise Rates#1#Bid%9,FWRC1#USDINR#Month Wise Rates#1#Bid%12,FWRC1#USDINR#Month Wise Rates#1#Bid%18,FWRC1#EURINR#Month Wise Rates#1#Bid%7,FWRC1#EURINR#Month Wise Rates#1#Bid%9,FWRC1#EURINR#Month Wise Rates#1#Bid%12,FWRC1#EURINR#Month Wise Rates#1#Bid%18,FWRC1#GBPINR#Month Wise Rates#1#Bid%7,FWRC1#GBPINR#Month Wise Rates#1#Bid%10,FWRC1#GBPINR#Month Wise Rates#1#Bid%12,FWRC1#GBPINR#Month Wise Rates#1#Bid%18,FWRC1#JPYINR#Month Wise Rates#1#Bid%8,FWRC1#JPYINR#Month Wise Rates#1#Bid%9,FWRC1#JPYINR#Month Wise Rates#1#Bid%12,FWRC1#JPYINR#Month Wise Rates#1#Bid%18,FWRC1#JPYINR#Month Wise Rates#1#Bid%7,42#66#1#6252#100#LTP,42#66#1#6252#100#NetChg,42#66#1#6252#100#%Chg,17#3#1#5903#AUDINRCOMP#Open,17#3#1#5903#AUDINRCOMP#Bid,17#3#1#5903#AUDINRCOMP#Ask,17#3#1#5903#AUDINRCOMP#NetChg,17#3#1#5903#AUDINRCOMP#High,17#3#1#5903#AUDINRCOMP#Low,17#3#1#5903#AUDINRCOMP#Close,12#2#1#5903#SAUDINR#52WkLow,12#2#1#5903#SAUDINR#52WkHigh,12#2#1#6069#SUSDINR#Bid,12#2#1#6069#SUSDINR#Ask,12#2#1#6069#SUSDINR#%Chg,12#2#1#6069#SUSDINR#Open,12#2#1#6069#SUSDINR#High,12#2#1#6069#SUSDINR#Low,12#2#1#6069#SUSDINR#Close,17#3#1#5921#CADINRCOMP#Bid,17#3#1#5921#CADINRCOMP#Ask,17#3#1#5921#CADINRCOMP#%Chg,17#3#1#5921#CADINRCOMP#Open,17#3#1#5921#CADINRCOMP#High,17#3#1#5921#CADINRCOMP#Low,17#3#1#5921#CADINRCOMP#Close,12#2#1#5921#SCADINR#52WkHigh,12#2#1#5921#SCADINR#52WkLow,17#3#1#5929#CNYINRCOMP#Bid,17#3#1#5929#CNYINRCOMP#Ask,17#3#1#5929#CNYINRCOMP#%Chg,17#3#1#5929#CNYINRCOMP#High,17#3#1#5929#CNYINRCOMP#Low,17#3#1#5929#CNYINRCOMP#Open,17#3#1#5929#CNYINRCOMP#Close,12#2#1#5929#SCNYINR#52WkHigh,12#2#1#5929#SCNYINR#52WkLow,17#3#1#6066#AEDINRCOMP#Bid,17#3#1#6066#AEDINRCOMP#Ask,17#3#1#6066#AEDINRCOMP#%Chg,17#3#1#6066#AEDINRCOMP#High,17#3#1#6066#AEDINRCOMP#Low,17#3#1#6066#AEDINRCOMP#Open,17#3#1#6066#AEDINRCOMP#Close,12#2#1#6066#SAEDINR#52WkHigh,12#2#1#6066#SAEDINR#52WkLow,FWRC1#EURINR#Month End Rates#0#BidPrice2,FWRC1#EURINR#Month End Rates#0#BidPrice4,FWRC1#EURINR#Month End Rates#0#BidPrice5,FWRC1#EURINR#Month End Rates#0#BidPrice6,FWRC1#EURINR#Month End Rates#0#BidPrice7,FWRC1#EURINR#Month End Rates#0#BidPrice8,FWRC1#EURINR#Month End Rates#0#BidPrice9,FWRC1#EURINR#Month End Rates#0#BidPrice10,FWRC1#EURINR#Month End Rates#0#BidPrice11,FWRC1#EURINR#Month End Rates#0#BidPrice12,FWRC1#EURINR#Month End Rates#0#BidPrice13,FWRC1#EURINR#Month End Rates#0#BidPrice14,FWRC1#EURINR#Month End Rates#0#BidPrice15,FWRC1#EURINR#Month End Rates#0#AskPrice2,FWRC1#EURINR#Month End Rates#0#AskPrice4,FWRC1#EURINR#Month End Rates#0#AskPrice5,FWRC1#EURINR#Month End Rates#0#AskPrice6,FWRC1#EURINR#Month End Rates#0#AskPrice7,FWRC1#EURINR#Month End Rates#0#AskPrice8,FWRC1#EURINR#Month End Rates#0#AskPrice9,FWRC1#EURINR#Month End Rates#0#AskPrice10,FWRC1#EURINR#Month End Rates#0#AskPrice11,FWRC1#EURINR#Month End Rates#0#AskPrice12,FWRC1#EURINR#Month End Rates#0#AskPrice13,FWRC1#EURINR#Month End Rates#0#AskPrice14,FWRC1#EURINR#Month End Rates#0#AskPrice15,FWRC1#GBPINR#Month End Rates#0#BidPrice2,FWRC1#GBPINR#Month End Rates#0#BidPrice4,FWRC1#GBPINR#Month End Rates#0#BidPrice5,FWRC1#GBPINR#Month End Rates#0#BidPrice6,FWRC1#GBPINR#Month End Rates#0#BidPrice7,FWRC1#GBPINR#Month End Rates#0#BidPrice8,FWRC1#GBPINR#Month End Rates#0#BidPrice9,FWRC1#GBPINR#Month End Rates#0#BidPrice10,FWRC1#GBPINR#Month End Rates#0#BidPrice11,FWRC1#GBPINR#Month End Rates#0#BidPrice12,FWRC1#GBPINR#Month End Rates#0#BidPrice13,FWRC1#GBPINR#Month End Rates#0#BidPrice14,FWRC1#GBPINR#Month End Rates#0#BidPrice15,FWRC1#GBPINR#Month End Rates#0#AskPrice2,FWRC1#GBPINR#Month End Rates#0#AskPrice4,FWRC1#GBPINR#Month End Rates#0#AskPrice5,FWRC1#GBPINR#Month End Rates#0#AskPrice6,FWRC1#GBPINR#Month End Rates#0#AskPrice7,FWRC1#GBPINR#Month End Rates#0#AskPrice8,FWRC1#GBPINR#Month End Rates#0#AskPrice9,FWRC1#GBPINR#Month End Rates#0#AskPrice10,FWRC1#GBPINR#Month End Rates#0#AskPrice11,FWRC1#GBPINR#Month End Rates#0#AskPrice12,FWRC1#GBPINR#Month End Rates#0#AskPrice13,FWRC1#GBPINR#Month End Rates#0#AskPrice14,FWRC1#GBPINR#Month End Rates#0#AskPrice15,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate0,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate2,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate4,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate5,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate6,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate7,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate8,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate9,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate10,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate11,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate12,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate13,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate14,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate15,FWRC1#GBPINR#Month End Rates#0#S#PillarDate0,FWRC1#GBPINR#Month End Rates#0#S#PillarDate1,FWRC1#GBPINR#Month End Rates#0#S#PillarDate2,FWRC1#GBPINR#Month End Rates#0#S#PillarDate3,FWRC1#GBPINR#Month End Rates#0#S#PillarDate4,FWRC1#GBPINR#Month End Rates#0#S#PillarDate5,FWRC1#GBPINR#Month End Rates#0#S#PillarDate6,FWRC1#GBPINR#Month End Rates#0#S#PillarDate7,FWRC1#GBPINR#Month End Rates#0#S#PillarDate8,FWRC1#GBPINR#Month End Rates#0#S#PillarDate9,FWRC1#GBPINR#Month End Rates#0#S#PillarDate10,FWRC1#GBPINR#Month End Rates#0#S#PillarDate11,FWRC1#GBPINR#Month End Rates#0#S#PillarDate12,FWRC1#GBPINR#Month End Rates#0#S#PillarDate13,FWRC1#GBPINR#Month End Rates#0#S#PillarDate14,FWRC1#GBPINR#Month End Rates#0#S#PillarDate15,FWRC1#EURINR#Month End Rates#0#S#Desc.0,FWRC1#EURINR#Month End Rates#0#S#Desc.2,FWRC1#EURINR#Month End Rates#0#S#Desc.4,FWRC1#EURINR#Month End Rates#0#S#Desc.5,FWRC1#EURINR#Month End Rates#0#S#Desc.6,FWRC1#EURINR#Month End Rates#0#S#Desc.7,FWRC1#EURINR#Month End Rates#0#S#Desc.8,FWRC1#EURINR#Month End Rates#0#S#Desc.9,FWRC1#EURINR#Month End Rates#0#S#Desc.10,FWRC1#EURINR#Month End Rates#0#S#Desc.11,FWRC1#EURINR#Month End Rates#0#S#Desc.12,FWRC1#EURINR#Month End Rates#0#S#Desc.13,FWRC1#EURINR#Month End Rates#0#S#Desc.14,FWRC1#EURINR#Month End Rates#0#S#Desc.15,FWRC1#GBPINR#Month End Rates#0#S#Desc.0,FWRC1#GBPINR#Month End Rates#0#S#Desc.1,FWRC1#GBPINR#Month End Rates#0#S#Desc.2,FWRC1#GBPINR#Month End Rates#0#S#Desc.3,FWRC1#GBPINR#Month End Rates#0#S#Desc.4,FWRC1#GBPINR#Month End Rates#0#S#Desc.5,FWRC1#GBPINR#Month End Rates#0#S#Desc.6,FWRC1#GBPINR#Month End Rates#0#S#Desc.7,FWRC1#GBPINR#Month End Rates#0#S#Desc.8,FWRC1#GBPINR#Month End Rates#0#S#Desc.9,FWRC1#GBPINR#Month End Rates#0#S#Desc.10,FWRC1#GBPINR#Month End Rates#0#S#Desc.11,FWRC1#GBPINR#Month End Rates#0#S#Desc.12,FWRC1#GBPINR#Month End Rates#0#S#Desc.13,FWRC1#GBPINR#Month End Rates#0#S#Desc.14,FWRC1#GBPINR#Month End Rates#0#S#Desc.15,XCUUSD_DATE_TIME,XCUUSD_BID,XCUUSD_ASK,XCUUSD_OPEN,XCUUSD_HIGH,XCUUSD_LOW,XCUUSD_CHANGE,XCUUSD_PERCENT_CHANGE,XCUUSD_1WEEK_PERCENT_CHANGE,XCUUSD_1MONTH_PERCENT_CHANGE ,XCUUSD_3MONTH_PERCENT_CHANGE,XCUUSD_1YEAR_PERCENT_CHANGE,XCUUSD_52 W HIGH,XCUUSD_52 W LOW,XALUSD_DATE_TIME,XALUSD_BID,XALUSD_ASK,XALUSD_OPEN,XALUSD_HIGH,XALUSD_LOW,XALUSD_CHANGE,XALUSD_PERCENT_CHANGE,XALUSD_1WEEK_PERCENT_CHANGE,XALUSD_1MONTH_PERCENT_CHANGE ,XALUSD_3MONTH_PERCENT_CHANGE,XALUSD_1YEAR_PERCENT_CHANGE,XALUSD_52 W HIGH,XALUSD_52 W LOW,XNIUSD_DATE_TIME,XNIUSD_BID,XNIUSD_ASK,XNIUSD_OPEN,XNIUSD_HIGH,XNIUSD_LOW,XNIUSD_CHANGE,XNIUSD_PERCENT_CHANGE,XNIUSD_1WEEK_PERCENT_CHANGE,XNIUSD_1MONTH_PERCENT_CHANGE ,XNIUSD_3MONTH_PERCENT_CHANGE,XNIUSD_1YEAR_PERCENT_CHANGE,XNIUSD_52 W HIGH,XNIUSD_52 W LOW,XZNUSD_DATE_TIME,XZNUSD_BID,XZNUSD_ASK,XZNUSD_OPEN,XZNUSD_HIGH,XZNUSD_LOW,XZNUSD_CHANGE,XZNUSD_PERCENT_CHANGE,XZNUSD_1WEEK_PERCENT_CHANGE,XZNUSD_1MONTH_PERCENT_CHANGE ,XZNUSD_3MONTH_PERCENT_CHANGE,XZNUSD_1YEAR_PERCENT_CHANGE,XZNUSD_52 W HIGH,XZNUSD_52 W LOW,XPBUSD_DATE_TIME,XPBUSD_BID,XPBUSD_ASK,XPBUSD_OPEN,XPBUSD_HIGH,XPBUSD_LOW,XPBUSD_CHANGE,XPBUSD_PERCENT_CHANGE,XPBUSD_1WEEK_PERCENT_CHANGE,XPBUSD_1MONTH_PERCENT_CHANGE ,XPBUSD_3MONTH_PERCENT_CHANGE,XPBUSD_1YEAR_PERCENT_CHANGE,XPBUSD_52 W HIGH,XPBUSD_52 W LOW,XAUUSDOZ_DATE_TIME,XAUUSDOZ_BID,XAUUSDOZ_CHANGE,XAUUSDOZ_PERCENT_CHANGE,XAUUSDOZ_OPEN,XAUUSDOZ_HIGH,XAUUSDOZ_LOW,XAGUSDOZ_DATE_TIME,XAGUSDOZ_BID,XAGUSDOZ_CHANGE,XAGUSDOZ_PERCENT_CHANGE,XAGUSDOZ_OPEN,XAGUSDOZ_HIGH,XAGUSDOZ_LOW,UKOil_DATE_TIME,UKOil_BID,UKOil_CHANGE,UKOil_PERCENT_CHANGE,UKOil_OPEN,UKOil_HIGH,UKOil_LOW,ET0Y.ip_DATE_TIME,ET0Y.ip_BID,ET0Y.ip_CHANGE,ET0Y.ip_PERCENT_CHANGE,ET0Y.ip_OPEN,ET0Y.ip_HIGH,ET0Y.ip_LOW,I*INDIAVIX.in_DATE_TIME,I*INDIAVIX.in_LAST/BID/ASK,I*INDIAVIX.in_CHANGE,I*INDIAVIX.in_PERCENT_CHANGE,I*INDIAVIX.in_OPEN,I*INDIAVIX.in_HIGH,I*INDIAVIX.in_LOW,NIFTYBANK.in_DATE_TIME,NIFTYBANK.in_LAST/BID/ASK,NIFTYBANK.in_CHANGE,NIFTYBANK.in_PERCENT_CHANGE,NIFTYBANK.in_OPEN,NIFTYBANK.in_HIGH,NIFTYBANK.in_LOW,HKG33_DATE_TIME,HKG33_LAST/BID/ASK,HKG33_CHANGE,HKG33_PERCENT_CHANGE,HKG33_OPEN,HKG33_HIGH,HKG33_LOW,CHN50_DATE_TIME,CHN50_LAST/BID/ASK,CHN50_CHANGE,CHN50_PERCENT_CHANGE,CHN50_OPEN,CHN50_HIGH,CHN50_LOW,JPN225_DATE_TIME,JPN225_LAST/BID/ASK,JPN225_CHANGE,JPN225_PERCENT_CHANGE,JPN225_OPEN,JPN225_HIGH,JPN225_LOW,UK100_DATE_TIME,UK100_LAST/BID/ASK,UK100_CHANGE,UK100_PERCENT_CHANGE,UK100_OPEN,UK100_HIGH,UK100_LOW,GER30_DATE_TIME,GER30_LAST/BID/ASK,GER30_CHANGE,GER30_PERCENT_CHANGE,GER30_OPEN,GER30_HIGH,GER30_LOW,FRA40_DATE_TIME,FRA40_LAST/BID/ASK,FRA40_CHANGE,FRA40_PERCENT_CHANGE,FRA40_OPEN,FRA40_HIGH,FRA40_LOW,ESP35_DATE_TIME,ESP35_LAST/BID/ASK,ESP35_CHANGE,ESP35_PERCENT_CHANGE,ESP35_OPEN,ESP35_HIGH,ESP35_LOW,US30_DATE_TIME,US30_LAST/BID/ASK,US30_CHANGE,US30_PERCENT_CHANGE,US30_OPEN,US30_HIGH,US30_LOW,IN10Y.GBOND_DATE_TIME,IN10Y.GBOND_LAST/BID/ASK,IN10Y.GBOND_CHANGE,IN10Y.GBOND_PERCENT_CHANGE,IN10Y.GBOND_OPEN,IN10Y.GBOND_HIGH,IN10Y.GBOND_LOW,US10Y.GBOND_DATE_TIME,US10Y.GBOND_LAST/BID/ASK,US10Y.GBOND_CHANGE,US10Y.GBOND_PERCENT_CHANGE,US10Y.GBOND_OPEN,US10Y.GBOND_HIGH,US10Y.GBOND_LOW,UK10Y.GBOND_DATE_TIME,UK10Y.GBOND_LAST/BID/ASK,UK10Y.GBOND_CHANGE,UK10Y.GBOND_PERCENT_CHANGE,UK10Y.GBOND_OPEN,UK10Y.GBOND_HIGH,UK10Y.GBOND_LOW,JP10Y.GBOND_DATE_TIME,JP10Y.GBOND_LAST/BID/ASK,JP10Y.GBOND_CHANGE,JP10Y.GBOND_PERCENT_CHANGE,JP10Y.GBOND_OPEN,JP10Y.GBOND_HIGH,JP10Y.GBOND_LOW,AU10Y.GBOND_DATE_TIME,AU10Y.GBOND_LAST/BID/ASK,AU10Y.GBOND_CHANGE,AU10Y.GBOND_PERCENT_CHANGE,AU10Y.GBOND_OPEN,AU10Y.GBOND_HIGH,AU10Y.GBOND_LOW,CN10Y.GBOND_DATE_TIME,CN10Y.GBOND_LAST/BID/ASK,CN10Y.GBOND_CHANGE,CN10Y.GBOND_PERCENT_CHANGE,CN10Y.GBOND_OPEN,CN10Y.GBOND_HIGH,CN10Y.GBOND_LOW,FR10Y.GBOND_DATE_TIME,FR10Y.GBOND_LAST/BID/ASK,FR10Y.GBOND_CHANGE,FR10Y.GBOND_PERCENT_CHANGE,FR10Y.GBOND_OPEN,FR10Y.GBOND_HIGH,FR10Y.GBOND_LOW,DE10Y.GBOND_DATE_TIME,DE10Y.GBOND_LAST/BID/ASK,DE10Y.GBOND_CHANGE,DE10Y.GBOND_PERCENT_CHANGE,DE10Y.GBOND_OPEN,DE10Y.GBOND_HIGH,DE10Y.GBOND_LOW,XCUUSD_CLOSE,XALUSD_CLOSE,XNIUSD_CLOSE,XZNUSD_CLOSE,XPBUSD_CLOSE,12#2#1#6069#SUSDCHF#Bid,12#2#1#6069#SUSDCHF#Ask,12#2#1#6069#SUSDCHF#%Chg,12#2#1#6069#SUSDCHF#High,12#2#1#6069#SUSDCHF#Low,12#2#1#6069#SUSDCHF#Open,12#2#1#6069#SUSDCHF#Close,12#2#1#6069#SUSDCHF#52WkHigh,12#2#1#6069#SUSDCHF#52WkLow,12#2#1#6069#SUSDCNH#Bid,12#2#1#6069#SUSDCNH#Ask,12#2#1#6069#SUSDCNH#%Chg,17#3#1#6037#SARINRCOMP#Bid,17#3#1#6037#SARINRCOMP#Ask,17#3#1#6037#SARINRCOMP#%Chg,17#3#1#6037#SARINRCOMP#High,17#3#1#6037#SARINRCOMP#Low,17#3#1#6037#SARINRCOMP#Open,17#3#1#6037#SARINRCOMP#Close,17#3#1#6037#SARINRCOMP#52WkHigh,17#3#1#6037#SARINRCOMP#52WkLow,42#66#1#6252#100#Open,42#66#1#6252#100#High,42#66#1#6252#100#Low,4#1#2#3556#Open,4#1#2#3556#High4#1#2#3556#Low,3#1#3#658#Open,3#1#3#658#High,3#1#3#658#Low";
        //string rtds = "17#3#1#5949#EURINRCOMP#Open,17#1#1#6069#USDINRCOMP#Bid,17#3#1#5949#EURINRCOMP#Bid,17#3#1#6031#GBPINRCOMP#Bid,17#3#1#5978#JPYINRCOMP#Bid,17#1#1#6069#USDINRCOMP#Ask,17#3#1#5949#EURINRCOMP#Ask,17#3#1#6031#GBPINRCOMP#Ask,17#3#1#5978#JPYINRCOMP#Ask,17#1#1#6069#USDINRCOMP#%Chg,17#3#1#5949#EURINRCOMP#%Chg,17#3#1#6031#GBPINRCOMP#%Chg,17#3#1#5978#JPYINRCOMP#%Chg,17#1#1#6069#USDINRCOMP#High,17#3#1#5949#EURINRCOMP#High,17#3#1#6031#GBPINRCOMP#High,17#3#1#5978#JPYINRCOMP#High,17#1#1#6069#USDINRCOMP#Low,17#3#1#5949#EURINRCOMP#Low,17#3#1#6031#GBPINRCOMP#Low,17#3#1#5978#JPYINRCOMP#Low,17#1#1#6069#USDINRCOMP#Open,17#3#1#5949#EURINRCOMP#Open,17#3#1#6031#GBPINRCOMP#Open,17#3#1#5978#JPYINRCOMP#Open,17#1#1#6069#USDINRCOMP#Close,17#3#1#5949#EURINRCOMP#Close,17#3#1#6031#GBPINRCOMP#Close,17#3#1#5978#JPYINRCOMP#Close,17#1#1#6069#USDINRCOMP#52WkHigh,12#2#1#5949#SEURINR#52WkHigh,12#2#1#6031#SGBPINR#52WkHigh,12#2#1#5978#SJPYINR#52WkHigh,17#1#1#6069#USDINRCOMP#52WkLow,12#2#1#5949#SEURINR#52WkLow,12#2#1#6031#SGBPINR#52WkLow,12#2#1#5978#SJPYINR#52WkLow,3#1#3#658#LTP,4#1#2#3556#LTP,42#2#1#6219#2#LTP,42#4#1#6217#4#LTP,42#36#1#6222#36#LTP,42#1#1#6253#1#LTP,3#1#3#658#NetChg,4#1#2#3556#NetChg,42#2#1#6219#2#NetChg,42#4#1#6217#4#NetChg,42#36#1#6222#36#NetChg,42#1#1#6253#1#NetChg,3#1#3#658#%Chg,4#1#2#3556#%Chg,42#2#1#6219#2#%Chg,42#4#1#6217#4#%Chg,42#36#1#6222#36#%Chg,42#1#1#6253#1#%Chg,47#5#1#5629#USD1M.LIBOR#LTP,47#5#1#5629#EUR1M.LIBOR#LTP,47#5#1#5629#GBP1M.LIBOR#LTP,47#5#1#5629#JPY1M.LIBOR#LTP,47#5#1#5629#USD3M.LIBOR#LTP,47#5#1#5629#EUR3M.LIBOR#LTP,47#5#1#5629#GBP3M.LIBOR#LTP,47#5#1#5629#JPY3M.LIBOR#LTP,47#5#1#5629#USD6M.LIBOR#LTP,47#5#1#5629#EUR6M.LIBOR#LTP,47#5#1#5629#GBP6M.LIBOR#LTP,47#5#1#5629#JPY6M.LIBOR#LTP,47#5#1#5629#USD1Y.LIBOR#LTP,47#5#1#5629#EUR1Y.LIBOR#LTP,47#5#1#5629#GBP1Y.LIBOR#LTP,47#5#1#5629#JPY1Y.LIBOR#LTP,12#2#1#5949#SEURUSD#Bid,12#2#1#6031#SGBPUSD#Bid,12#2#1#6069#SUSDJPY#Bid,12#2#1#5903#SAUDUSD#Bid,12#2#1#6069#SUSDCAD#Bid,12#2#1#5949#SEURUSD#Ask,12#2#1#6031#SGBPUSD#Ask,12#2#1#6069#SUSDJPY#Ask,12#2#1#5903#SAUDUSD#Ask,12#2#1#6069#SUSDCAD#Ask,12#2#1#5949#SEURUSD#%Chg,12#2#1#6031#SGBPUSD#%Chg,12#2#1#6069#SUSDJPY#%Chg,12#2#1#5903#SAUDUSD#%Chg,12#2#1#6069#SUSDCAD#%Chg,12#2#1#5949#SEURUSD#High,12#2#1#6031#SGBPUSD#High,12#2#1#6069#SUSDJPY#High,12#2#1#5903#SAUDUSD#High,12#2#1#6069#SUSDCAD#High,12#2#1#5949#SEURUSD#Low,12#2#1#6031#SGBPUSD#Low,12#2#1#6069#SUSDJPY#Low,12#2#1#5903#SAUDUSD#Low,12#2#1#6069#SUSDCAD#Low,12#2#1#5949#SEURUSD#Open,12#2#1#6031#SGBPUSD#Open,12#2#1#6069#SUSDJPY#Open,12#2#1#5903#SAUDUSD#Open,12#2#1#6069#SUSDCAD#Open,12#2#1#5949#SEURUSD#Close,12#2#1#6031#SGBPUSD#Close,12#2#1#6069#SUSDJPY#Close,12#2#1#5903#SAUDUSD#Close,12#2#1#6069#SUSDCAD#Close,12#2#1#5949#SEURUSD#52WkHigh,12#2#1#6031#SGBPUSD#52WkHigh,12#2#1#6069#SUSDJPY#52WkHigh,12#2#1#5903#SAUDUSD#52WkHigh,12#2#1#6069#SUSDCAD#52WkHigh,12#2#1#5949#SEURUSD#52WkLow,12#2#1#6031#SGBPUSD#52WkLow,12#2#1#6069#SUSDJPY#52WkLow,12#2#1#5903#SAUDUSD#52WkLow,12#2#1#6069#SUSDCAD#52WkLow,12#2#1#6069#SUSDSGD#Bid,12#2#1#6069#SUSDHKD#Bid,12#2#1#6069#SUSDTWD#Bid,12#2#1#6069#SUSDKRW#Bid,12#2#1#6069#SUSDPHP#Bid,12#2#1#6069#SUSDIDR#Bid,12#2#1#6069#SUSDCNY#Bid,12#2#1#6069#SUSDMYR#Bid,12#2#1#6069#SUSDTHB#Bid,12#2#1#6069#SUSDSGD#Ask,12#2#1#6069#SUSDHKD#Ask,12#2#1#6069#SUSDTWD#Ask,12#2#1#6069#SUSDKRW#Ask,12#2#1#6069#SUSDPHP#Ask,12#2#1#6069#SUSDIDR#Ask,12#2#1#6069#SUSDCNY#Ask,12#2#1#6069#SUSDMYR#Ask,12#2#1#6069#SUSDTHB#Ask,12#2#1#6069#SUSDSGD#%Chg,12#2#1#6069#SUSDHKD#%Chg,12#2#1#6069#SUSDTWD#%Chg,12#2#1#6069#SUSDKRW#%Chg,12#2#1#6069#SUSDPHP#%Chg,12#2#1#6069#SUSDIDR#%Chg,12#2#1#6069#SUSDCNY#%Chg,12#2#1#6069#SUSDMYR#%Chg,12#2#1#6069#SUSDTHB#%Chg,12#2#1#6069#SUSDSGD#High,12#2#1#6069#SUSDHKD#High,12#2#1#6069#SUSDTWD#High,12#2#1#6069#SUSDKRW#High,12#2#1#6069#SUSDPHP#High,12#2#1#6069#SUSDIDR#High,12#2#1#6069#SUSDCNY#High,12#2#1#6069#SUSDMYR#High,12#2#1#6069#SUSDTHB#High,12#2#1#6069#SUSDSGD#Low,12#2#1#6069#SUSDHKD#Low,12#2#1#6069#SUSDTWD#Low,12#2#1#6069#SUSDKRW#Low,12#2#1#6069#SUSDPHP#Low,12#2#1#6069#SUSDIDR#Low,12#2#1#6069#SUSDCNY#Low,12#2#1#6069#SUSDMYR#Low,12#2#1#6069#SUSDTHB#Low,12#2#1#6069#SUSDSGD#Open,12#2#1#6069#SUSDHKD#Open,12#2#1#6069#SUSDTWD#Open,12#2#1#6069#SUSDKRW#Open,12#2#1#6069#SUSDPHP#Open,12#2#1#6069#SUSDIDR#Open,12#2#1#6069#SUSDCNY#Open,12#2#1#6069#SUSDMYR#Open,12#2#1#6069#SUSDTHB#Open,12#2#1#6069#SUSDSGD#Close,12#2#1#6069#SUSDHKD#Close,12#2#1#6069#SUSDTWD#Close,12#2#1#6069#SUSDKRW#Close,12#2#1#6069#SUSDPHP#Close,12#2#1#6069#SUSDIDR#Close,12#2#1#6069#SUSDCNY#Close,12#2#1#6069#SUSDMYR#Close,12#2#1#6069#SUSDTHB#Close,12#2#1#6069#SUSDSGD#52WkHigh,12#2#1#6069#SUSDHKD#52WkHigh,12#2#1#6069#SUSDTWD#52WkHigh,12#2#1#6069#SUSDKRW#52WkHigh,12#2#1#6069#SUSDPHP#52WkHigh,12#2#1#6069#SUSDIDR#52WkHigh,12#2#1#6069#SUSDCNY#52WkHigh,12#2#1#6069#SUSDMYR#52WkHigh,12#2#1#6069#SUSDTHB#52WkHigh,12#2#1#6069#SUSDSGD#52WkLow,12#2#1#6069#SUSDHKD#52WkLow,12#2#1#6069#SUSDTWD#52WkLow,12#2#1#6069#SUSDKRW#52WkLow,12#2#1#6069#SUSDPHP#52WkLow,12#2#1#6069#SUSDIDR#52WkLow,12#2#1#6069#SUSDCNY#52WkLow,12#2#1#6069#SUSDMYR#52WkLow,12#2#1#6069#SUSDTHB#52WkLow,35#1#1#6069#FUTCUR#N1#0#XX#Bid,35#1#1#6069#FUTCUR#1127865600#0#XX#Bid,35#1#1#6069#FUTCUR#1130457600#0#XX#Bid,35#1#1#6069#FUTCUR#N1#0#XX#Ask,35#1#1#6069#FUTCUR#1127865600#0#XX#Ask,35#1#1#6069#FUTCUR#1130457600#0#XX#Ask,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate2,17#2#1#6069#USDINRCOMP CASH/SPOT#Ask,17#2#1#6069#USDINRCOMP CASH/SPOT#Bid,FWRC1#USDINR#Month End Rates#0#S#Desc.4,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate4,FWRC1#USDINR#Month End Rates#0#BidPrice4,FWRC1#USDINR#Month End Rates#0#AskPrice4,FWRC1#USDINR#Month End Rates#0#S#Desc.5,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate5,FWRC1#USDINR#Month End Rates#0#BidPrice5,FWRC1#USDINR#Month End Rates#0#AskPrice5,FWRC1#USDINR#Month End Rates#0#S#Desc.6,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate6,FWRC1#USDINR#Month End Rates#0#BidPrice6,FWRC1#USDINR#Month End Rates#0#AskPrice6,FWRC1#USDINR#Month End Rates#0#S#Desc.7,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate7,FWRC1#USDINR#Month End Rates#0#BidPrice7,FWRC1#USDINR#Month End Rates#0#AskPrice7,FWRC1#USDINR#Month End Rates#0#S#Desc.8,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate8,FWRC1#USDINR#Month End Rates#0#BidPrice8,FWRC1#USDINR#Month End Rates#0#AskPrice8,FWRC1#USDINR#Month End Rates#0#S#Desc.9,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate9,FWRC1#USDINR#Month End Rates#0#BidPrice9,FWRC1#USDINR#Month End Rates#0#AskPrice9,FWRC1#USDINR#Month End Rates#0#S#Desc.10,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate10,FWRC1#USDINR#Month End Rates#0#BidPrice10,FWRC1#USDINR#Month End Rates#0#AskPrice10,FWRC1#USDINR#Month End Rates#0#S#Desc.11,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate11,FWRC1#USDINR#Month End Rates#0#BidPrice11,FWRC1#USDINR#Month End Rates#0#AskPrice11,FWRC1#USDINR#Month End Rates#0#S#Desc.12,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate12,FWRC1#USDINR#Month End Rates#0#BidPrice12,FWRC1#USDINR#Month End Rates#0#AskPrice12,FWRC1#USDINR#Month End Rates#0#S#Desc.13,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate13,FWRC1#USDINR#Month End Rates#0#BidPrice13,FWRC1#USDINR#Month End Rates#0#AskPrice13,FWRC1#USDINR#Month End Rates#0#S#Desc.14,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate14,FWRC1#USDINR#Month End Rates#0#BidPrice14,FWRC1#USDINR#Month End Rates#0#AskPrice14,FWRC1#USDINR#Month End Rates#0#S#Desc.15,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate15,FWRC1#USDINR#Month End Rates#0#BidPrice15,FWRC1#USDINR#Month End Rates#0#AskPrice15,17#1#1#6069#USDINRCOMP#Bid,17#1#1#6069#USDINRCOMP#Ask,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate0,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid4,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk4,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid5,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk5,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid6,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk6,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid7,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk7,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid8,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk8,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid9,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk9,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid10,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk10,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid11,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk11,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid12,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk12,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid13,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk13,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid14,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk14,FWRC1#USDINR#Month End Rates#0#FwdOutrightBid15,FWRC1#USDINR#Month End Rates#0#FwdOutrightAsk15,FWRC1#USDINR#Month End Rates#0#S#MonthEndDate2,17#2#1#6069#USDINRCOMP CASH/SPOT#Ask,17#2#1#6069#USDINRCOMP CASH/SPOT#Bid,FWRC1#EURINR#Month Wise Rates#0#S#Desc.7,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate7,FWRC1#USDINR#Month Wise Rates#0#BidPrice7,FWRC1#USDINR#Month Wise Rates#0#AskPrice7,FWRC1#EURINR#Month Wise Rates#0#S#Desc.8,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate8,FWRC1#USDINR#Month Wise Rates#0#BidPrice8,FWRC1#USDINR#Month Wise Rates#0#AskPrice8,FWRC1#EURINR#Month Wise Rates#0#S#Desc.9,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate9,FWRC1#USDINR#Month Wise Rates#0#BidPrice9,FWRC1#USDINR#Month Wise Rates#0#AskPrice9,FWRC1#EURINR#Month Wise Rates#0#S#Desc.10,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate10,FWRC1#USDINR#Month Wise Rates#0#BidPrice10,FWRC1#USDINR#Month Wise Rates#0#AskPrice10,FWRC1#EURINR#Month Wise Rates#0#S#Desc.11,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate11,FWRC1#USDINR#Month Wise Rates#0#BidPrice11,FWRC1#USDINR#Month Wise Rates#0#AskPrice11,FWRC1#EURINR#Month Wise Rates#0#S#Desc.12,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate12,FWRC1#USDINR#Month Wise Rates#0#BidPrice12,FWRC1#USDINR#Month Wise Rates#0#AskPrice12,FWRC1#EURINR#Month Wise Rates#0#S#Desc.13,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate13,FWRC1#USDINR#Month Wise Rates#0#BidPrice13,FWRC1#USDINR#Month Wise Rates#0#AskPrice13,FWRC1#EURINR#Month Wise Rates#0#S#Desc.14,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate14,FWRC1#USDINR#Month Wise Rates#0#BidPrice14,FWRC1#USDINR#Month Wise Rates#0#AskPrice14,FWRC1#EURINR#Month Wise Rates#0#S#Desc.15,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate15,FWRC1#USDINR#Month Wise Rates#0#BidPrice15,FWRC1#USDINR#Month Wise Rates#0#AskPrice15,FWRC1#EURINR#Month Wise Rates#0#S#Desc.16,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate16,FWRC1#USDINR#Month Wise Rates#0#BidPrice16,FWRC1#USDINR#Month Wise Rates#0#AskPrice16,FWRC1#EURINR#Month Wise Rates#0#S#Desc.17,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate17,FWRC1#USDINR#Month Wise Rates#0#BidPrice17,FWRC1#USDINR#Month Wise Rates#0#AskPrice17,FWRC1#EURINR#Month Wise Rates#0#S#Desc.18,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate18,FWRC1#USDINR#Month Wise Rates#0#BidPrice18,FWRC1#USDINR#Month Wise Rates#0#AskPrice18,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid7,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk7,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid8,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk8,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid9,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk9,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid10,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk10,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid11,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk11,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid12,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk12,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid13,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk13,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid14,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk14,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid15,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk15,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid16,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk17,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid17,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk17,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightBid18,FWRC1#USDINR#Month Wise Rates#1#FwdOutrightAsk18,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate2,FWRC1#EURINR#Month Wise Rates#0#BidPrice2,FWRC1#EURINR#Month Wise Rates#0#AskPrice2,FWRC1#EURINR#Month Wise Rates#0#S#Desc.7,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate7,FWRC1#EURINR#Month Wise Rates#0#BidPrice7,FWRC1#EURINR#Month Wise Rates#0#AskPrice7,FWRC1#EURINR#Month Wise Rates#0#S#Desc.8,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate8,FWRC1#EURINR#Month Wise Rates#0#BidPrice8,FWRC1#EURINR#Month Wise Rates#0#AskPrice8,FWRC1#EURINR#Month Wise Rates#0#S#Desc.9,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate9,FWRC1#EURINR#Month Wise Rates#0#BidPrice9,FWRC1#EURINR#Month Wise Rates#0#AskPrice9,FWRC1#EURINR#Month Wise Rates#0#S#Desc.10,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate10,FWRC1#EURINR#Month Wise Rates#0#BidPrice10,FWRC1#EURINR#Month Wise Rates#0#AskPrice10,FWRC1#EURINR#Month Wise Rates#0#S#Desc.11,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate11,FWRC1#EURINR#Month Wise Rates#0#BidPrice11,FWRC1#EURINR#Month Wise Rates#0#AskPrice11,FWRC1#EURINR#Month Wise Rates#0#S#Desc.12,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate12,FWRC1#EURINR#Month Wise Rates#0#BidPrice12,FWRC1#EURINR#Month Wise Rates#0#AskPrice12,FWRC1#EURINR#Month Wise Rates#0#S#Desc.13,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate13,FWRC1#EURINR#Month Wise Rates#0#BidPrice13,FWRC1#EURINR#Month Wise Rates#0#AskPrice13,FWRC1#EURINR#Month Wise Rates#0#S#Desc.14,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate14,FWRC1#EURINR#Month Wise Rates#0#BidPrice14,FWRC1#EURINR#Month Wise Rates#0#AskPrice14,FWRC1#EURINR#Month Wise Rates#0#S#Desc.15,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate15,FWRC1#EURINR#Month Wise Rates#0#BidPrice15,FWRC1#EURINR#Month Wise Rates#0#AskPrice15,FWRC1#EURINR#Month Wise Rates#0#S#Desc.16,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate16,FWRC1#EURINR#Month Wise Rates#0#BidPrice16,FWRC1#EURINR#Month Wise Rates#0#AskPrice16,FWRC1#EURINR#Month Wise Rates#0#S#Desc.17,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate17,FWRC1#EURINR#Month Wise Rates#0#BidPrice17,FWRC1#EURINR#Month Wise Rates#0#AskPrice17,FWRC1#EURINR#Month Wise Rates#0#S#Desc.18,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate18,FWRC1#EURINR#Month Wise Rates#0#BidPrice18,FWRC1#EURINR#Month Wise Rates#0#AskPrice18,17#3#1#5949#EURINRCOMP#Bid,17#3#1#5949#EURINRCOMP#Ask,FWRC1#EURINR#Month Wise Rates#0#S#PillarDate0,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid7,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk7,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid8,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk8,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid9,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk9,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid10,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk10,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid11,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk11,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid12,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk12,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid13,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk13,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid14,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk14,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid15,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk15,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid16,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk16,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid17,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk17,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightBid18,FWRC1#EURINR#Month Wise Rates#0#FwdOutrightAsk18,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate2,FWRC1#GBPINR#Month Wise Rates#0#BidPrice2,FWRC1#GBPINR#Month Wise Rates#0#AskPrice2,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.7,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate7,FWRC1#GBPINR#Month Wise Rates#0#BidPrice7,FWRC1#GBPINR#Month Wise Rates#0#AskPrice7,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.8,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate8,FWRC1#GBPINR#Month Wise Rates#0#BidPrice8,FWRC1#GBPINR#Month Wise Rates#0#AskPrice8,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.9,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate9,FWRC1#GBPINR#Month Wise Rates#0#BidPrice9,FWRC1#GBPINR#Month Wise Rates#0#AskPrice9,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.10,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate10,FWRC1#GBPINR#Month Wise Rates#0#BidPrice10,FWRC1#GBPINR#Month Wise Rates#0#AskPrice10,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.11,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate11,FWRC1#GBPINR#Month Wise Rates#0#BidPrice11,FWRC1#GBPINR#Month Wise Rates#0#AskPrice11,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.12,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate12,FWRC1#GBPINR#Month Wise Rates#0#BidPrice12,FWRC1#GBPINR#Month Wise Rates#0#AskPrice12,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.13,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate13,FWRC1#GBPINR#Month Wise Rates#0#BidPrice13,FWRC1#GBPINR#Month Wise Rates#0#AskPrice13,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.14,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate14,FWRC1#GBPINR#Month Wise Rates#0#BidPrice14,FWRC1#GBPINR#Month Wise Rates#0#AskPrice14,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.15,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate15,FWRC1#GBPINR#Month Wise Rates#0#BidPrice15,FWRC1#GBPINR#Month Wise Rates#0#AskPrice15,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.16,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate16,FWRC1#GBPINR#Month Wise Rates#0#BidPrice16,FWRC1#GBPINR#Month Wise Rates#0#AskPrice16,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.17,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate17,FWRC1#GBPINR#Month Wise Rates#0#BidPrice17,FWRC1#GBPINR#Month Wise Rates#0#AskPrice17,FWRC1#GBPINR#Month Wise Rates#0#S#Desc.18,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate18,FWRC1#GBPINR#Month Wise Rates#0#BidPrice18,FWRC1#GBPINR#Month Wise Rates#0#AskPrice18,17#3#1#6031#GBPINRCOMP#Bid,17#3#1#6031#GBPINRCOMP#Ask,FWRC1#GBPINR#Month Wise Rates#0#S#PillarDate0,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid7,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk7,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid8,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk8,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid9,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk9,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid10,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk10,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid11,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk11,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid12,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk12,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid13,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk13,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid14,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk14,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid15,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk15,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid16,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk16,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid17,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk17,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightBid18,FWRC1#GBPINR#Month Wise Rates#0#FwdOutrightAsk18,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.2,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate2,FWRC1#JPYINR#Month Wise Rates#0#BidPrice2,FWRC1#JPYINR#Month Wise Rates#0#AskPrice2,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.7,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate7,FWRC1#JPYINR#Month Wise Rates#0#BidPrice7,FWRC1#JPYINR#Month Wise Rates#0#AskPrice7,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.8,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate8,FWRC1#JPYINR#Month Wise Rates#0#BidPrice8,FWRC1#JPYINR#Month Wise Rates#0#AskPrice8,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.9,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate9,FWRC1#JPYINR#Month Wise Rates#0#BidPrice9,FWRC1#JPYINR#Month Wise Rates#0#AskPrice9,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.10,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate10,FWRC1#JPYINR#Month Wise Rates#0#BidPrice10,FWRC1#JPYINR#Month Wise Rates#0#AskPrice10,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.11,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate11,FWRC1#JPYINR#Month Wise Rates#0#BidPrice11,FWRC1#JPYINR#Month Wise Rates#0#AskPrice11,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.12,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate12,FWRC1#JPYINR#Month Wise Rates#0#BidPrice12,FWRC1#JPYINR#Month Wise Rates#0#AskPrice12,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.13,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate13,FWRC1#JPYINR#Month Wise Rates#0#BidPrice13,FWRC1#JPYINR#Month Wise Rates#0#AskPrice13,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.14,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate14,FWRC1#JPYINR#Month Wise Rates#0#BidPrice14,FWRC1#JPYINR#Month Wise Rates#0#AskPrice14,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.15,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate15,FWRC1#JPYINR#Month Wise Rates#0#BidPrice15,FWRC1#JPYINR#Month Wise Rates#0#AskPrice15,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.16,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate16,FWRC1#JPYINR#Month Wise Rates#0#BidPrice16,FWRC1#JPYINR#Month Wise Rates#0#AskPrice16,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.17,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate17,FWRC1#JPYINR#Month Wise Rates#0#BidPrice17,FWRC1#JPYINR#Month Wise Rates#0#AskPrice17,FWRC1#JPYINR#Month Wise Rates#0#S#Desc.18,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate18,FWRC1#JPYINR#Month Wise Rates#0#BidPrice18,FWRC1#JPYINR#Month Wise Rates#0#AskPrice18,17#3#1#5978#JPYINRCOMP#Bid,17#3#1#5978#JPYINRCOMP#Ask,FWRC1#JPYINR#Month Wise Rates#0#S#PillarDate0,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid7,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk7,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid8,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk8,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid9,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk9,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid10,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk10,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid11,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk11,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid12,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk12,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid13,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk13,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid14,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk14,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid15,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk15,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid16,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk16,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid17,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk17,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightBid18,FWRC1#JPYINR#Month Wise Rates#0#FwdOutrightAsk18,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.2,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate2,FWRC1#EURUSD#Month Wise Rates#0#BidPrice2,FWRC1#EURUSD#Month Wise Rates#0#AskPrice2,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.7,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate7,FWRC1#EURUSD#Month Wise Rates#0#BidPrice7,FWRC1#EURUSD#Month Wise Rates#0#AskPrice7,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.8,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate8,FWRC1#EURUSD#Month Wise Rates#0#BidPrice8,FWRC1#EURUSD#Month Wise Rates#0#AskPrice8,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.9,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate9,FWRC1#EURUSD#Month Wise Rates#0#BidPrice9,FWRC1#EURUSD#Month Wise Rates#0#AskPrice9,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.10,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate10,FWRC1#EURUSD#Month Wise Rates#0#BidPrice10,FWRC1#EURUSD#Month Wise Rates#0#AskPrice10,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.11,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate11,FWRC1#EURUSD#Month Wise Rates#0#BidPrice11,FWRC1#EURUSD#Month Wise Rates#0#AskPrice11,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.12,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate12,FWRC1#EURUSD#Month Wise Rates#0#BidPrice12,FWRC1#EURUSD#Month Wise Rates#0#AskPrice12,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.13,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate13,FWRC1#EURUSD#Month Wise Rates#0#BidPrice13,FWRC1#EURUSD#Month Wise Rates#0#AskPrice13,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.14,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate14,FWRC1#EURUSD#Month Wise Rates#0#BidPrice14,FWRC1#EURUSD#Month Wise Rates#0#AskPrice14,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.15,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate15,FWRC1#EURUSD#Month Wise Rates#0#BidPrice15,FWRC1#EURUSD#Month Wise Rates#0#AskPrice15,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.16,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate16,FWRC1#EURUSD#Month Wise Rates#0#BidPrice16,FWRC1#EURUSD#Month Wise Rates#0#AskPrice16,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.17,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate17,FWRC1#EURUSD#Month Wise Rates#0#BidPrice17,FWRC1#EURUSD#Month Wise Rates#0#AskPrice17,FWRC1#EURUSD#Month Wise Rates#0#S#Desc.18,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate18,FWRC1#EURUSD#Month Wise Rates#0#BidPrice18,FWRC1#EURUSD#Month Wise Rates#0#AskPrice18,FWRC1#EURUSD#Month Wise Rates#0#BidPrice0,FWRC1#EURUSD#Month Wise Rates#0#AskPrice0,FWRC1#EURUSD#Month Wise Rates#0#S#PillarDate0,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid2,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk2,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid7,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk7,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid8,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk8,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid9,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk9,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid10,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk10,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid11,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk11,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid12,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk12,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid13,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk13,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid14,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk14,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid15,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk15,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid16,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk16,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid17,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk17,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightBid18,FWRC1#EURUSD#Month Wise Rates#0#FwdOutrightAsk18,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.2,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate2,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice2,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice2,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.7,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate7,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice7,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice7,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.8,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate8,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice8,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice8,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.9,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate9,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice9,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice9,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.10,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate10,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice10,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice10,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.11,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate11,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice11,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice11,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.12,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate12,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice12,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice12,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.13,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate13,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice13,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice13,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.14,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate14,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice14,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice14,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.15,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate15,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice15,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice15,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.16,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate16,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice16,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice16,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.17,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate17,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice17,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice17,FWRC1#GBPUSD#Month Wise Rates#0#S#Desc.18,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate18,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice18,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice18,FWRC1#GBPUSD#Month Wise Rates#0#BidPrice0,FWRC1#GBPUSD#Month Wise Rates#0#AskPrice0,FWRC1#GBPUSD#Month Wise Rates#0#S#PillarDate0,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid2,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk2,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid7,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk7,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid8,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk8,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid9,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk9,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid10,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk10,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid11,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk11,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid12,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk12,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid13,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk13,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid14,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk14,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid15,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk15,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid16,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk16,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid17,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk17,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightBid18,FWRC1#GBPUSD#Month Wise Rates#0#FwdOutrightAsk18,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.2,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate2,FWRC1#USDJPY#Month Wise Rates#0#BidPrice2,FWRC1#USDJPY#Month Wise Rates#0#AskPrice2,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.7,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate7,FWRC1#USDJPY#Month Wise Rates#0#BidPrice7,FWRC1#USDJPY#Month Wise Rates#0#AskPrice7,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.8,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate8,FWRC1#USDJPY#Month Wise Rates#0#BidPrice8,FWRC1#USDJPY#Month Wise Rates#0#AskPrice8,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.9,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate9,FWRC1#USDJPY#Month Wise Rates#0#BidPrice9,FWRC1#USDJPY#Month Wise Rates#0#AskPrice9,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.10,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate10,FWRC1#USDJPY#Month Wise Rates#0#BidPrice10,FWRC1#USDJPY#Month Wise Rates#0#AskPrice10,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.11,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate11,FWRC1#USDJPY#Month Wise Rates#0#BidPrice11,FWRC1#USDJPY#Month Wise Rates#0#AskPrice11,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.12,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate12,FWRC1#USDJPY#Month Wise Rates#0#BidPrice12,FWRC1#USDJPY#Month Wise Rates#0#AskPrice12,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.13,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate13,FWRC1#USDJPY#Month Wise Rates#0#BidPrice13,FWRC1#USDJPY#Month Wise Rates#0#AskPrice13,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.14,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate14,FWRC1#USDJPY#Month Wise Rates#0#BidPrice14,FWRC1#USDJPY#Month Wise Rates#0#AskPrice14,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.15,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate15,FWRC1#USDJPY#Month Wise Rates#0#BidPrice15,FWRC1#USDJPY#Month Wise Rates#0#AskPrice15,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.16,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate16,FWRC1#USDJPY#Month Wise Rates#0#BidPrice16,FWRC1#USDJPY#Month Wise Rates#0#AskPrice16,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.17,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate17,FWRC1#USDJPY#Month Wise Rates#0#BidPrice17,FWRC1#USDJPY#Month Wise Rates#0#AskPrice17,FWRC1#USDJPY#Month Wise Rates#0#S#Desc.18,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate18,FWRC1#USDJPY#Month Wise Rates#0#BidPrice18,FWRC1#USDJPY#Month Wise Rates#0#AskPrice18,FWRC1#USDJPY#Month Wise Rates#0#BidPrice0,FWRC1#USDJPY#Month Wise Rates#0#AskPrice0,FWRC1#USDJPY#Month Wise Rates#0#S#PillarDate0,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid2,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk2,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid7,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk7,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid8,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk8,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid9,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk9,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid10,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk10,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid11,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk11,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid12,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk12,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid13,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk13,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid14,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk14,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid15,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk15,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid16,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk16,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid17,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk17,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightBid18,FWRC1#USDJPY#Month Wise Rates#0#FwdOutrightAsk18,FWRC1#USDINR#Month Wise Rates#0#FwdOutrightAsk16,35#1#1#6069#FUTCUR#N1#0#XX#Bid,35#1#1#6069#FUTCUR#N2#0#XX#Bid,35#1#1#6069#FUTCUR#N3#0#XX#Bid,35#1#1#6069#FUTCUR#N1#0#XX#Ask,35#1#1#6069#FUTCUR#N2#0#XX#Ask,35#1#1#6069#FUTCUR#N3#0#XX#Ask,35#1#1#5949#FUTCUR#N1#0#XX#Bid,35#1#1#5949#FUTCUR#N2#0#XX#Bid,35#1#1#5949#FUTCUR#N3#0#XX#Bid,35#1#1#5949#FUTCUR#N1#0#XX#Ask,35#1#1#5949#FUTCUR#N2#0#XX#Ask,35#1#1#5949#FUTCUR#N3#0#XX#Ask,35#1#1#6031#FUTCUR#N1#0#XX#Bid,35#1#1#6031#FUTCUR#N2#0#XX#Bid,35#1#1#6031#FUTCUR#N3#0#XX#Bid,35#1#1#6031#FUTCUR#N1#0#XX#Ask,35#1#1#6031#FUTCUR#N2#0#XX#Ask,35#1#1#6031#FUTCUR#N3#0#XX#Ask,35#1#1#5978#FUTCUR#N1#0#XX#Bid,35#1#1#5978#FUTCUR#N2#0#XX#Bid,35#1#1#5978#FUTCUR#N3#0#XX#Bid,35#1#1#5978#FUTCUR#N1#0#XX#Ask,35#1#1#5978#FUTCUR#N2#0#XX#Ask,35#1#1#5978#FUTCUR#N3#0#XX#Ask,FWRC1#USDINR#Month Wise Rates#1#Bid%7,FWRC1#USDINR#Month Wise Rates#1#Bid%9,FWRC1#USDINR#Month Wise Rates#1#Bid%12,FWRC1#USDINR#Month Wise Rates#1#Bid%18,FWRC1#EURINR#Month Wise Rates#1#Bid%7,FWRC1#EURINR#Month Wise Rates#1#Bid%9,FWRC1#EURINR#Month Wise Rates#1#Bid%12,FWRC1#EURINR#Month Wise Rates#1#Bid%18,FWRC1#GBPINR#Month Wise Rates#1#Bid%7,FWRC1#GBPINR#Month Wise Rates#1#Bid%10,FWRC1#GBPINR#Month Wise Rates#1#Bid%12,FWRC1#GBPINR#Month Wise Rates#1#Bid%18,FWRC1#JPYINR#Month Wise Rates#1#Bid%8,FWRC1#JPYINR#Month Wise Rates#1#Bid%9,FWRC1#JPYINR#Month Wise Rates#1#Bid%12,FWRC1#JPYINR#Month Wise Rates#1#Bid%18,FWRC1#USDINR#Month Wise Rates#1#Bid%7,FWRC1#USDINR#Month Wise Rates#1#Bid%9,FWRC1#USDINR#Month Wise Rates#1#Bid%12,FWRC1#USDINR#Month Wise Rates#1#Bid%12,FWRC1#USDINR#Month Wise Rates#1#Bid%18,FWRC1#EURINR#Month Wise Rates#1#Bid%7,FWRC1#EURINR#Month Wise Rates#1#Bid%9,FWRC1#EURINR#Month Wise Rates#1#Bid%12,FWRC1#EURINR#Month Wise Rates#1#Bid%18,FWRC1#GBPINR#Month Wise Rates#1#Bid%7,FWRC1#GBPINR#Month Wise Rates#1#Bid%10,FWRC1#GBPINR#Month Wise Rates#1#Bid%12,FWRC1#GBPINR#Month Wise Rates#1#Bid%18,FWRC1#JPYINR#Month Wise Rates#1#Bid%8,FWRC1#JPYINR#Month Wise Rates#1#Bid%9,FWRC1#JPYINR#Month Wise Rates#1#Bid%12,FWRC1#JPYINR#Month Wise Rates#1#Bid%18,FWRC1#USDINR#Month Wise Rates#1#Bid%7,FWRC1#USDINR#Month Wise Rates#1#Bid%9,FWRC1#USDINR#Month Wise Rates#1#Bid%12,FWRC1#USDINR#Month Wise Rates#1#Bid%18,FWRC1#EURINR#Month Wise Rates#1#Bid%7,FWRC1#EURINR#Month Wise Rates#1#Bid%9,FWRC1#EURINR#Month Wise Rates#1#Bid%12,FWRC1#EURINR#Month Wise Rates#1#Bid%18,FWRC1#GBPINR#Month Wise Rates#1#Bid%7,FWRC1#GBPINR#Month Wise Rates#1#Bid%10,FWRC1#GBPINR#Month Wise Rates#1#Bid%12,FWRC1#GBPINR#Month Wise Rates#1#Bid%18,FWRC1#JPYINR#Month Wise Rates#1#Bid%8,FWRC1#JPYINR#Month Wise Rates#1#Bid%9,FWRC1#JPYINR#Month Wise Rates#1#Bid%12,FWRC1#JPYINR#Month Wise Rates#1#Bid%18,FWRC1#JPYINR#Month Wise Rates#1#Bid%7,42#66#1#6252#100#LTP,42#66#1#6252#100#NetChg,42#66#1#6252#100#%Chg,17#3#1#5903#AUDINRCOMP#Open,17#3#1#5903#AUDINRCOMP#Bid,17#3#1#5903#AUDINRCOMP#Ask,17#3#1#5903#AUDINRCOMP#NetChg,17#3#1#5903#AUDINRCOMP#High,17#3#1#5903#AUDINRCOMP#Low,17#3#1#5903#AUDINRCOMP#Close,12#2#1#5903#SAUDINR#52WkLow,12#2#1#5903#SAUDINR#52WkHigh,12#2#1#6069#SUSDINR#Bid,12#2#1#6069#SUSDINR#Ask,12#2#1#6069#SUSDINR#%Chg,12#2#1#6069#SUSDINR#Open,12#2#1#6069#SUSDINR#High,12#2#1#6069#SUSDINR#Low,12#2#1#6069#SUSDINR#Close,17#3#1#5921#CADINRCOMP#Bid,17#3#1#5921#CADINRCOMP#Ask,17#3#1#5921#CADINRCOMP#%Chg,17#3#1#5921#CADINRCOMP#Open,17#3#1#5921#CADINRCOMP#High,17#3#1#5921#CADINRCOMP#Low,17#3#1#5921#CADINRCOMP#Close,12#2#1#5921#SCADINR#52WkHigh,12#2#1#5921#SCADINR#52WkLow,17#3#1#5929#CNYINRCOMP#Bid,17#3#1#5929#CNYINRCOMP#Ask,17#3#1#5929#CNYINRCOMP#%Chg,17#3#1#5929#CNYINRCOMP#High,17#3#1#5929#CNYINRCOMP#Low,17#3#1#5929#CNYINRCOMP#Open,17#3#1#5929#CNYINRCOMP#Close,12#2#1#5929#SCNYINR#52WkHigh,12#2#1#5929#SCNYINR#52WkLow,17#3#1#6066#AEDINRCOMP#Bid,17#3#1#6066#AEDINRCOMP#Ask,17#3#1#6066#AEDINRCOMP#%Chg,17#3#1#6066#AEDINRCOMP#High,17#3#1#6066#AEDINRCOMP#Low,17#3#1#6066#AEDINRCOMP#Open,17#3#1#6066#AEDINRCOMP#Close,12#2#1#6066#SAEDINR#52WkHigh,12#2#1#6066#SAEDINR#52WkLow,FWRC1#EURINR#Month End Rates#0#BidPrice2,FWRC1#EURINR#Month End Rates#0#BidPrice4,FWRC1#EURINR#Month End Rates#0#BidPrice5,FWRC1#EURINR#Month End Rates#0#BidPrice6,FWRC1#EURINR#Month End Rates#0#BidPrice7,FWRC1#EURINR#Month End Rates#0#BidPrice8,FWRC1#EURINR#Month End Rates#0#BidPrice9,FWRC1#EURINR#Month End Rates#0#BidPrice10,FWRC1#EURINR#Month End Rates#0#BidPrice11,FWRC1#EURINR#Month End Rates#0#BidPrice12,FWRC1#EURINR#Month End Rates#0#BidPrice13,FWRC1#EURINR#Month End Rates#0#BidPrice14,FWRC1#EURINR#Month End Rates#0#BidPrice15,FWRC1#EURINR#Month End Rates#0#AskPrice2,FWRC1#EURINR#Month End Rates#0#AskPrice4,FWRC1#EURINR#Month End Rates#0#AskPrice5,FWRC1#EURINR#Month End Rates#0#AskPrice6,FWRC1#EURINR#Month End Rates#0#AskPrice7,FWRC1#EURINR#Month End Rates#0#AskPrice8,FWRC1#EURINR#Month End Rates#0#AskPrice9,FWRC1#EURINR#Month End Rates#0#AskPrice10,FWRC1#EURINR#Month End Rates#0#AskPrice11,FWRC1#EURINR#Month End Rates#0#AskPrice12,FWRC1#EURINR#Month End Rates#0#AskPrice13,FWRC1#EURINR#Month End Rates#0#AskPrice14,FWRC1#EURINR#Month End Rates#0#AskPrice15,FWRC1#GBPINR#Month End Rates#0#BidPrice2,FWRC1#GBPINR#Month End Rates#0#BidPrice4,FWRC1#GBPINR#Month End Rates#0#BidPrice5,FWRC1#GBPINR#Month End Rates#0#BidPrice6,FWRC1#GBPINR#Month End Rates#0#BidPrice7,FWRC1#GBPINR#Month End Rates#0#BidPrice8,FWRC1#GBPINR#Month End Rates#0#BidPrice9,FWRC1#GBPINR#Month End Rates#0#BidPrice10,FWRC1#GBPINR#Month End Rates#0#BidPrice11,FWRC1#GBPINR#Month End Rates#0#BidPrice12,FWRC1#GBPINR#Month End Rates#0#BidPrice13,FWRC1#GBPINR#Month End Rates#0#BidPrice14,FWRC1#GBPINR#Month End Rates#0#BidPrice15,FWRC1#GBPINR#Month End Rates#0#AskPrice2,FWRC1#GBPINR#Month End Rates#0#AskPrice4,FWRC1#GBPINR#Month End Rates#0#AskPrice5,FWRC1#GBPINR#Month End Rates#0#AskPrice6,FWRC1#GBPINR#Month End Rates#0#AskPrice7,FWRC1#GBPINR#Month End Rates#0#AskPrice8,FWRC1#GBPINR#Month End Rates#0#AskPrice9,FWRC1#GBPINR#Month End Rates#0#AskPrice10,FWRC1#GBPINR#Month End Rates#0#AskPrice11,FWRC1#GBPINR#Month End Rates#0#AskPrice12,FWRC1#GBPINR#Month End Rates#0#AskPrice13,FWRC1#GBPINR#Month End Rates#0#AskPrice14,FWRC1#GBPINR#Month End Rates#0#AskPrice15,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate0,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate2,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate4,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate5,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate6,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate7,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate8,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate9,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate10,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate11,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate12,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate13,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate14,FWRC1#EURINR#Month End Rates#0#S#MonthEndDate15,FWRC1#GBPINR#Month End Rates#0#S#PillarDate0,FWRC1#GBPINR#Month End Rates#0#S#PillarDate1,FWRC1#GBPINR#Month End Rates#0#S#PillarDate2,FWRC1#GBPINR#Month End Rates#0#S#PillarDate3,FWRC1#GBPINR#Month End Rates#0#S#PillarDate4,FWRC1#GBPINR#Month End Rates#0#S#PillarDate5,FWRC1#GBPINR#Month End Rates#0#S#PillarDate6,FWRC1#GBPINR#Month End Rates#0#S#PillarDate7,FWRC1#GBPINR#Month End Rates#0#S#PillarDate8,FWRC1#GBPINR#Month End Rates#0#S#PillarDate9,FWRC1#GBPINR#Month End Rates#0#S#PillarDate10,FWRC1#GBPINR#Month End Rates#0#S#PillarDate11,FWRC1#GBPINR#Month End Rates#0#S#PillarDate12,FWRC1#GBPINR#Month End Rates#0#S#PillarDate13,FWRC1#GBPINR#Month End Rates#0#S#PillarDate14,FWRC1#GBPINR#Month End Rates#0#S#PillarDate15,FWRC1#EURINR#Month End Rates#0#S#Desc.0,FWRC1#EURINR#Month End Rates#0#S#Desc.2,FWRC1#EURINR#Month End Rates#0#S#Desc.4,FWRC1#EURINR#Month End Rates#0#S#Desc.5,FWRC1#EURINR#Month End Rates#0#S#Desc.6,FWRC1#EURINR#Month End Rates#0#S#Desc.7,FWRC1#EURINR#Month End Rates#0#S#Desc.8,FWRC1#EURINR#Month End Rates#0#S#Desc.9,FWRC1#EURINR#Month End Rates#0#S#Desc.10,FWRC1#EURINR#Month End Rates#0#S#Desc.11,FWRC1#EURINR#Month End Rates#0#S#Desc.12,FWRC1#EURINR#Month End Rates#0#S#Desc.13,FWRC1#EURINR#Month End Rates#0#S#Desc.14,FWRC1#EURINR#Month End Rates#0#S#Desc.15,FWRC1#GBPINR#Month End Rates#0#S#Desc.0,FWRC1#GBPINR#Month End Rates#0#S#Desc.1,FWRC1#GBPINR#Month End Rates#0#S#Desc.2,FWRC1#GBPINR#Month End Rates#0#S#Desc.3,FWRC1#GBPINR#Month End Rates#0#S#Desc.4,FWRC1#GBPINR#Month End Rates#0#S#Desc.5,FWRC1#GBPINR#Month End Rates#0#S#Desc.6,FWRC1#GBPINR#Month End Rates#0#S#Desc.7,FWRC1#GBPINR#Month End Rates#0#S#Desc.8,FWRC1#GBPINR#Month End Rates#0#S#Desc.9,FWRC1#GBPINR#Month End Rates#0#S#Desc.10,FWRC1#GBPINR#Month End Rates#0#S#Desc.11,FWRC1#GBPINR#Month End Rates#0#S#Desc.12,FWRC1#GBPINR#Month End Rates#0#S#Desc.13,FWRC1#GBPINR#Month End Rates#0#S#Desc.14,FWRC1#GBPINR#Month End Rates#0#S#Desc.15,XCUUSD_DATE_TIME,XCUUSD_BID,XCUUSD_ASK,XCUUSD_OPEN,XCUUSD_HIGH,XCUUSD_LOW,XCUUSD_CHANGE,XCUUSD_PERCENT_CHANGE,XCUUSD_1WEEK_PERCENT_CHANGE,XCUUSD_1MONTH_PERCENT_CHANGE ,XCUUSD_3MONTH_PERCENT_CHANGE,XCUUSD_1YEAR_PERCENT_CHANGE,XCUUSD_52 W HIGH,XCUUSD_52 W LOW,XALUSD_DATE_TIME,XALUSD_BID,XALUSD_ASK,XALUSD_OPEN,XALUSD_HIGH,XALUSD_LOW,XALUSD_CHANGE,XALUSD_PERCENT_CHANGE,XALUSD_1WEEK_PERCENT_CHANGE,XALUSD_1MONTH_PERCENT_CHANGE ,XALUSD_3MONTH_PERCENT_CHANGE,XALUSD_1YEAR_PERCENT_CHANGE,XALUSD_52 W HIGH,XALUSD_52 W LOW,XNIUSD_DATE_TIME,XNIUSD_BID,XNIUSD_ASK,XNIUSD_OPEN,XNIUSD_HIGH,XNIUSD_LOW,XNIUSD_CHANGE,XNIUSD_PERCENT_CHANGE,XNIUSD_1WEEK_PERCENT_CHANGE,XNIUSD_1MONTH_PERCENT_CHANGE ,XNIUSD_3MONTH_PERCENT_CHANGE,XNIUSD_1YEAR_PERCENT_CHANGE,XNIUSD_52 W HIGH,XNIUSD_52 W LOW,XZNUSD_DATE_TIME,XZNUSD_BID,XZNUSD_ASK,XZNUSD_OPEN,XZNUSD_HIGH,XZNUSD_LOW,XZNUSD_CHANGE,XZNUSD_PERCENT_CHANGE,XZNUSD_1WEEK_PERCENT_CHANGE,XZNUSD_1MONTH_PERCENT_CHANGE ,XZNUSD_3MONTH_PERCENT_CHANGE,XZNUSD_1YEAR_PERCENT_CHANGE,XZNUSD_52 W HIGH,XZNUSD_52 W LOW,XPBUSD_DATE_TIME,XPBUSD_BID,XPBUSD_ASK,XPBUSD_OPEN,XPBUSD_HIGH,XPBUSD_LOW,XPBUSD_CHANGE,XPBUSD_PERCENT_CHANGE,XPBUSD_1WEEK_PERCENT_CHANGE,XPBUSD_1MONTH_PERCENT_CHANGE ,XPBUSD_3MONTH_PERCENT_CHANGE,XPBUSD_1YEAR_PERCENT_CHANGE,XPBUSD_52 W HIGH,XPBUSD_52 W LOW,XAUUSDOZ_DATE_TIME,XAUUSDOZ_BID,XAUUSDOZ_CHANGE,XAUUSDOZ_PERCENT_CHANGE,XAUUSDOZ_OPEN,XAUUSDOZ_HIGH,XAUUSDOZ_LOW,XAGUSDOZ_DATE_TIME,XAGUSDOZ_BID,XAGUSDOZ_CHANGE,XAGUSDOZ_PERCENT_CHANGE,XAGUSDOZ_OPEN,XAGUSDOZ_HIGH,XAGUSDOZ_LOW,UKOil_DATE_TIME,UKOil_BID,UKOil_CHANGE,UKOil_PERCENT_CHANGE,UKOil_OPEN,UKOil_HIGH,UKOil_LOW,ET0Y.ip_DATE_TIME,ET0Y.ip_BID,ET0Y.ip_CHANGE,ET0Y.ip_PERCENT_CHANGE,ET0Y.ip_OPEN,ET0Y.ip_HIGH,ET0Y.ip_LOW,I*INDIAVIX.in_DATE_TIME,I*INDIAVIX.in_LAST/BID/ASK,I*INDIAVIX.in_CHANGE,I*INDIAVIX.in_PERCENT_CHANGE,I*INDIAVIX.in_OPEN,I*INDIAVIX.in_HIGH,I*INDIAVIX.in_LOW,NIFTYBANK.in_DATE_TIME,NIFTYBANK.in_LAST/BID/ASK,NIFTYBANK.in_CHANGE,NIFTYBANK.in_PERCENT_CHANGE,NIFTYBANK.in_OPEN,NIFTYBANK.in_HIGH,NIFTYBANK.in_LOW,HKG33_DATE_TIME,HKG33_LAST/BID/ASK,HKG33_CHANGE,HKG33_PERCENT_CHANGE,HKG33_OPEN,HKG33_HIGH,HKG33_LOW,CHN50_DATE_TIME,CHN50_LAST/BID/ASK,CHN50_CHANGE,CHN50_PERCENT_CHANGE,CHN50_OPEN,CHN50_HIGH,CHN50_LOW,JPN225_DATE_TIME,JPN225_LAST/BID/ASK,JPN225_CHANGE,JPN225_PERCENT_CHANGE,JPN225_OPEN,JPN225_HIGH,JPN225_LOW,UK100_DATE_TIME,UK100_LAST/BID/ASK,UK100_CHANGE,UK100_PERCENT_CHANGE,UK100_OPEN,UK100_HIGH,UK100_LOW,GER30_DATE_TIME,GER30_LAST/BID/ASK,GER30_CHANGE,GER30_PERCENT_CHANGE,GER30_OPEN,GER30_HIGH,GER30_LOW,FRA40_DATE_TIME,FRA40_LAST/BID/ASK,FRA40_CHANGE,FRA40_PERCENT_CHANGE,FRA40_OPEN,FRA40_HIGH,FRA40_LOW,ESP35_DATE_TIME,ESP35_LAST/BID/ASK,ESP35_CHANGE,ESP35_PERCENT_CHANGE,ESP35_OPEN,ESP35_HIGH,ESP35_LOW,US30_DATE_TIME,US30_LAST/BID/ASK,US30_CHANGE,US30_PERCENT_CHANGE,US30_OPEN,US30_HIGH,US30_LOW,IN10Y.GBOND_DATE_TIME,IN10Y.GBOND_LAST/BID/ASK,IN10Y.GBOND_CHANGE,IN10Y.GBOND_PERCENT_CHANGE,IN10Y.GBOND_OPEN,IN10Y.GBOND_HIGH,IN10Y.GBOND_LOW,US10Y.GBOND_DATE_TIME,US10Y.GBOND_LAST/BID/ASK,US10Y.GBOND_CHANGE,US10Y.GBOND_PERCENT_CHANGE,US10Y.GBOND_OPEN,US10Y.GBOND_HIGH,US10Y.GBOND_LOW,UK10Y.GBOND_DATE_TIME,UK10Y.GBOND_LAST/BID/ASK,UK10Y.GBOND_CHANGE,UK10Y.GBOND_PERCENT_CHANGE,UK10Y.GBOND_OPEN,UK10Y.GBOND_HIGH,UK10Y.GBOND_LOW,JP10Y.GBOND_DATE_TIME,JP10Y.GBOND_LAST/BID/ASK,JP10Y.GBOND_CHANGE,JP10Y.GBOND_PERCENT_CHANGE,JP10Y.GBOND_OPEN,JP10Y.GBOND_HIGH,JP10Y.GBOND_LOW,AU10Y.GBOND_DATE_TIME,AU10Y.GBOND_LAST/BID/ASK,AU10Y.GBOND_CHANGE,AU10Y.GBOND_PERCENT_CHANGE,AU10Y.GBOND_OPEN,AU10Y.GBOND_HIGH,AU10Y.GBOND_LOW,CN10Y.GBOND_DATE_TIME,CN10Y.GBOND_LAST/BID/ASK,CN10Y.GBOND_CHANGE,CN10Y.GBOND_PERCENT_CHANGE,CN10Y.GBOND_OPEN,CN10Y.GBOND_HIGH,CN10Y.GBOND_LOW,FR10Y.GBOND_DATE_TIME,FR10Y.GBOND_LAST/BID/ASK,FR10Y.GBOND_CHANGE,FR10Y.GBOND_PERCENT_CHANGE,FR10Y.GBOND_OPEN,FR10Y.GBOND_HIGH,FR10Y.GBOND_LOW,DE10Y.GBOND_DATE_TIME,DE10Y.GBOND_LAST/BID/ASK,DE10Y.GBOND_CHANGE,DE10Y.GBOND_PERCENT_CHANGE,DE10Y.GBOND_OPEN,DE10Y.GBOND_HIGH,DE10Y.GBOND_LOW,XCUUSD_CLOSE,XALUSD_CLOSE,XNIUSD_CLOSE,XZNUSD_CLOSE,XPBUSD_CLOSE,12#2#1#6069#SUSDCHF#Bid,12#2#1#6069#SUSDCHF#Ask,12#2#1#6069#SUSDCHF#%Chg,12#2#1#6069#SUSDCHF#High,12#2#1#6069#SUSDCHF#Low,12#2#1#6069#SUSDCHF#Open,12#2#1#6069#SUSDCHF#Close,12#2#1#6069#SUSDCHF#52WkHigh,12#2#1#6069#SUSDCHF#52WkLow,12#2#1#6069#SUSDCNH#Bid,12#2#1#6069#SUSDCNH#Ask,12#2#1#6069#SUSDCNH#%Chg,17#3#1#6037#SARINRCOMP#Bid,17#3#1#6037#SARINRCOMP#Ask,17#3#1#6037#SARINRCOMP#%Chg,17#3#1#6037#SARINRCOMP#High,17#3#1#6037#SARINRCOMP#Low,17#3#1#6037#SARINRCOMP#Open,17#3#1#6037#SARINRCOMP#Close,17#3#1#6037#SARINRCOMP#52WkHigh,17#3#1#6037#SARINRCOMP#52WkLow,42#66#1#6252#100#High,42#66#1#6252#100#Low,42#66#1#6252#100#Open,42#66#1#6252#100#Close,4#1#2#3556#High,4#1#2#3556#Low,4#1#2#3556#Open,4#1#2#3556#Close,3#1#3#658#High,3#1#3#658#Low,3#1#3#658#Open,3#1#3#658#Close";
        string rtds = "17#1#1#6069#USDINRCOMP#52WkHigh,12#2#1#5949#SEURINR#52WkHigh,12#2#1#6031#SGBPINR#52WkHigh,12#2#1#5978#SJPYINR#52WkHigh,17#1#1#6069#USDINRCOMP#52WkLow,12#2#1#5949#SEURINR#52WkLow,12#2#1#6031#SGBPINR#52WkLow,12#2#1#5978#SJPYINR#52WkLow,3#1#3#658#LTP,4#1#2#3556#LTP,3#1#3#658#NetChg,4#1#2#3556#NetChg,3#1#3#658#%Chg,4#1#2#3556#%Chg,12#2#1#6069#SUSDSGD#52WkHigh,12#2#1#6069#SUSDHKD#52WkHigh,12#2#1#6069#SUSDTWD#52WkHigh,12#2#1#6069#SUSDKRW#52WkHigh,12#2#1#6069#SUSDPHP#52WkHigh,12#2#1#6069#SUSDIDR#52WkHigh,12#2#1#6069#SUSDCNY#52WkHigh,12#2#1#6069#SUSDMYR#52WkHigh,12#2#1#6069#SUSDTHB#52WkHigh,12#2#1#6069#SUSDSGD#52WkLow,12#2#1#6069#SUSDHKD#52WkLow,12#2#1#6069#SUSDTWD#52WkLow,12#2#1#6069#SUSDKRW#52WkLow,12#2#1#6069#SUSDPHP#52WkLow,12#2#1#6069#SUSDIDR#52WkLow,12#2#1#6069#SUSDCNY#52WkLow,12#2#1#6069#SUSDMYR#52WkLow,12#2#1#6069#SUSDTHB#52WkLow,35#1#1#6069#FUTCUR#N1#0#XX#Bid,35#1#1#6069#FUTCUR#1127865600#0#XX#Bid,35#1#1#6069#FUTCUR#1130457600#0#XX#Bid,35#1#1#6069#FUTCUR#N1#0#XX#Ask,35#1#1#6069#FUTCUR#1127865600#0#XX#Ask,35#1#1#6069#FUTCUR#1130457600#0#XX#Ask,35#1#1#6069#FUTCUR#N1#0#XX#Bid,35#1#1#6069#FUTCUR#N2#0#XX#Bid,35#1#1#6069#FUTCUR#N3#0#XX#Bid,35#1#1#6069#FUTCUR#N1#0#XX#Ask,35#1#1#6069#FUTCUR#N2#0#XX#Ask,35#1#1#6069#FUTCUR#N3#0#XX#Ask,35#1#1#5949#FUTCUR#N1#0#XX#Bid,35#1#1#5949#FUTCUR#N2#0#XX#Bid,35#1#1#5949#FUTCUR#N3#0#XX#Bid,35#1#1#5949#FUTCUR#N1#0#XX#Ask,35#1#1#5949#FUTCUR#N2#0#XX#Ask,35#1#1#5949#FUTCUR#N3#0#XX#Ask,35#1#1#6031#FUTCUR#N1#0#XX#Bid,35#1#1#6031#FUTCUR#N2#0#XX#Bid,35#1#1#6031#FUTCUR#N3#0#XX#Bid,35#1#1#6031#FUTCUR#N1#0#XX#Ask,35#1#1#6031#FUTCUR#N2#0#XX#Ask,35#1#1#6031#FUTCUR#N3#0#XX#Ask,35#1#1#5978#FUTCUR#N1#0#XX#Bid,35#1#1#5978#FUTCUR#N2#0#XX#Bid,35#1#1#5978#FUTCUR#N3#0#XX#Bid,35#1#1#5978#FUTCUR#N1#0#XX#Ask,35#1#1#5978#FUTCUR#N2#0#XX#Ask,35#1#1#5978#FUTCUR#N3#0#XX#Ask,42#66#1#6252#100#LTP,42#66#1#6252#100#NetChg,42#66#1#6252#100#%Chg,XAUUSDOZ_DATE_TIME,XAUUSDOZ_BID,XAUUSDOZ_CHANGE,XAUUSDOZ_PERCENT_CHANGE,XAUUSDOZ_OPEN,XAUUSDOZ_HIGH,XAUUSDOZ_LOW,XAGUSDOZ_DATE_TIME,XAGUSDOZ_BID,XAGUSDOZ_CHANGE,XAGUSDOZ_PERCENT_CHANGE,XAGUSDOZ_OPEN,XAGUSDOZ_HIGH,XAGUSDOZ_LOW,UKOil_DATE_TIME,UKOil_BID,UKOil_CHANGE,UKOil_PERCENT_CHANGE,UKOil_OPEN,UKOil_HIGH,UKOil_LOW,ET0Y.ip_DATE_TIME,ET0Y.ip_BID,ET0Y.ip_CHANGE,ET0Y.ip_PERCENT_CHANGE,ET0Y.ip_OPEN,ET0Y.ip_HIGH,ET0Y.ip_LOW,I*INDIAVIX.in_DATE_TIME,I*INDIAVIX.in_LAST/BID/ASK,I*INDIAVIX.in_CHANGE,I*INDIAVIX.in_PERCENT_CHANGE,I*INDIAVIX.in_OPEN,I*INDIAVIX.in_HIGH,I*INDIAVIX.in_LOW,NIFTYBANK.in_DATE_TIME,NIFTYBANK.in_LAST/BID/ASK,NIFTYBANK.in_CHANGE,NIFTYBANK.in_PERCENT_CHANGE,NIFTYBANK.in_OPEN,NIFTYBANK.in_HIGH,NIFTYBANK.in_LOW,HKG33_DATE_TIME,HKG33_LAST/BID/ASK,HKG33_CHANGE,HKG33_PERCENT_CHANGE,HKG33_OPEN,HKG33_HIGH,HKG33_LOW,CHN50_DATE_TIME,CHN50_LAST/BID/ASK,CHN50_CHANGE,CHN50_PERCENT_CHANGE,CHN50_OPEN,CHN50_HIGH,CHN50_LOW,JPN225_DATE_TIME,JPN225_LAST/BID/ASK,JPN225_CHANGE,JPN225_PERCENT_CHANGE,JPN225_OPEN,JPN225_HIGH,JPN225_LOW,UK100_DATE_TIME,UK100_LAST/BID/ASK,UK100_CHANGE,UK100_PERCENT_CHANGE,UK100_OPEN,UK100_HIGH,UK100_LOW,GER30_DATE_TIME,GER30_LAST/BID/ASK,GER30_CHANGE,GER30_PERCENT_CHANGE,GER30_OPEN,GER30_HIGH,GER30_LOW,FRA40_DATE_TIME,FRA40_LAST/BID/ASK,FRA40_CHANGE,FRA40_PERCENT_CHANGE,FRA40_OPEN,FRA40_HIGH,FRA40_LOW,ESP35_DATE_TIME,ESP35_LAST/BID/ASK,ESP35_CHANGE,ESP35_PERCENT_CHANGE,ESP35_OPEN,ESP35_HIGH,ESP35_LOW,US30_DATE_TIME,US30_LAST/BID/ASK,US30_CHANGE,US30_PERCENT_CHANGE,US30_OPEN,US30_HIGH,US30_LOW,IN10Y.GBOND_DATE_TIME,IN10Y.GBOND_LAST/BID/ASK,IN10Y.GBOND_CHANGE,IN10Y.GBOND_PERCENT_CHANGE,IN10Y.GBOND_OPEN,IN10Y.GBOND_HIGH,IN10Y.GBOND_LOW,US10Y.GBOND_DATE_TIME,US10Y.GBOND_LAST/BID/ASK,US10Y.GBOND_CHANGE,US10Y.GBOND_PERCENT_CHANGE,US10Y.GBOND_OPEN,US10Y.GBOND_HIGH,US10Y.GBOND_LOW,UK10Y.GBOND_DATE_TIME,UK10Y.GBOND_LAST/BID/ASK,UK10Y.GBOND_CHANGE,UK10Y.GBOND_PERCENT_CHANGE,UK10Y.GBOND_OPEN,UK10Y.GBOND_HIGH,UK10Y.GBOND_LOW,JP10Y.GBOND_DATE_TIME,JP10Y.GBOND_LAST/BID/ASK,JP10Y.GBOND_CHANGE,JP10Y.GBOND_PERCENT_CHANGE,JP10Y.GBOND_OPEN,JP10Y.GBOND_HIGH,JP10Y.GBOND_LOW,AU10Y.GBOND_DATE_TIME,AU10Y.GBOND_LAST/BID/ASK,AU10Y.GBOND_CHANGE,AU10Y.GBOND_PERCENT_CHANGE,AU10Y.GBOND_OPEN,AU10Y.GBOND_HIGH,AU10Y.GBOND_LOW,CN10Y.GBOND_DATE_TIME,CN10Y.GBOND_LAST/BID/ASK,CN10Y.GBOND_CHANGE,CN10Y.GBOND_PERCENT_CHANGE,CN10Y.GBOND_OPEN,CN10Y.GBOND_HIGH,CN10Y.GBOND_LOW,FR10Y.GBOND_DATE_TIME,FR10Y.GBOND_LAST/BID/ASK,FR10Y.GBOND_CHANGE,FR10Y.GBOND_PERCENT_CHANGE,FR10Y.GBOND_OPEN,FR10Y.GBOND_HIGH,FR10Y.GBOND_LOW,DE10Y.GBOND_DATE_TIME,DE10Y.GBOND_LAST/BID/ASK,DE10Y.GBOND_CHANGE,DE10Y.GBOND_PERCENT_CHANGE,DE10Y.GBOND_OPEN,DE10Y.GBOND_HIGH,DE10Y.GBOND_LOW,XCUUSD_CLOSE,XALUSD_CLOSE,XNIUSD_CLOSE,XZNUSD_CLOSE,XPBUSD_CLOSE,42#66#1#6252#100#High,42#66#1#6252#100#Low,42#66#1#6252#100#Open,42#66#1#6252#100#Close,4#1#2#3556#High,4#1#2#3556#Low,4#1#2#3556#Open,4#1#2#3556#Close,3#1#3#658#High,3#1#3#658#Low,3#1#3#658#Open,3#1#3#658#Close,!DXX_LAST,!DXX_CHANGE,!DXX_PERCENT_CHANGE,!DXX_OPEN,!DXX_HIGH,!DXX_LOW,NIFTY50.in_LAST,NIFTY50.in_CHANGE,NIFTY50.in_PERCENT_CHANGE,NIFTY50.in_OPEN,NIFTY50.in_HIGH,NIFTY50.in_LOW";
        //string historyRtds = "17#1#1#6069#USDINRCOMP#Bid,17#3#1#5949#EURINRCOMP#Bid,17#3#1#6031#GBPINRCOMP#Bid,17#3#1#5978#JPYINRCOMP#Bid";
        string values = global.CheckInputData(HttpContext.Current.Request["v"]);
        if (values.Contains(",,"))
        {
            ErrorLog.WriteLog("rtd=" + HttpContext.Current.Request["rtd"] + "&v=" + HttpContext.Current.Request["v"]);
        }
        Array arrRtdIndex = HttpContext.Current.Request["rtd"].Split(',');
        Array arrRtd = rtds.Split(',');
        Array arrValues = values.Split(',');
        //Array arrHistoryRtd = historyRtds.Split(',');
        ArrayList arrFinalRtdCodes = new ArrayList();
        for (int i = 0; i < arrRtdIndex.Length; i++)
        {
            int rtdIndex = Convert.ToInt32(arrRtdIndex.GetValue(i));
            arrFinalRtdCodes.Add(arrRtd.GetValue(rtdIndex));
        }
        string query = "";
        bool isHistoryUpdate = false;
        TimeSpan ts = DateTime.Now - _lastCommodityLiveRateRequest;
        if (ts.Seconds >= 20)
        {
            isHistoryUpdate = true;
            _lastCommodityLiveRateRequest = DateTime.Now;
        }
        
        //ErrorLog.WriteLog(rtds);
        //ErrorLog.WriteLog(values);
        /*
        UpdateCommodityRate(arrFinalRtdCodes, arrValues, "XCUUSD", 1, 1, isHistoryUpdate);//copper
        UpdateCommodityRate(arrFinalRtdCodes, arrValues, "XALUSD", 2, 2, isHistoryUpdate);//ALUMINIUM
        UpdateCommodityRate(arrFinalRtdCodes, arrValues, "XNIUSD", 3, 3, isHistoryUpdate);//NICKEL
        UpdateCommodityRate(arrFinalRtdCodes, arrValues, "XZNUSD", 4, 4, isHistoryUpdate);//ZINC
        UpdateCommodityRate(arrFinalRtdCodes, arrValues, "XPBUSD", 5, 5, isHistoryUpdate);//LEAD
        
        //UpdateCommodityRate(arrRtd, arrValues, "", 8, 8);//TIN

        query = @"update tbl_metalliverate set 
                    metalliverate_ctweekhigh=case when metalliverate_high>metalliverate_ctweekhigh then metalliverate_high else metalliverate_ctweekhigh end,
                    metalliverate_ctweeklow=case when metalliverate_low<metalliverate_ctweeklow or metalliverate_ctweeklow=0 then metalliverate_low else metalliverate_ctweeklow end,
                    metalliverate_ctmonthhigh=case when metalliverate_high>metalliverate_ctmonthhigh then metalliverate_high else metalliverate_ctmonthhigh end,
                    metalliverate_ctmonthlow=case when metalliverate_low<metalliverate_ctmonthlow or metalliverate_ctmonthlow=0 then metalliverate_low else metalliverate_ctmonthlow end
                        ";
        DbTable.ExecuteQuery(query);
        */
        for (int i = 0; i < arrFinalRtdCodes.Count; i++)
        {
            string rtd = arrFinalRtdCodes[i].ToString();
            string value = arrValues.GetValue(i).ToString().Trim();
            if (rtd.StartsWith("XCUUSD") || rtd.StartsWith("XALUSD") || rtd.StartsWith("XNIUSD") || rtd.StartsWith("XZNUSD") || rtd.StartsWith("XPBUSD"))
            {
            }
            else
            {
                if (IsValidRate(value))
                {
                    query = "UPDATE tbl_liverate SET liverate_prevrate=liverate_currentrate,liverate_currentrate='" +
                        value + "',liverate_modifieddate=GETDATE(),liverate_isexcelupdate=1 WHERE liverate_rtdcode='" + rtd + "' and isnull(liverate_isapirate,0)=0";
                    DbTable.ExecuteQuery(query);
                    if (isHistoryUpdate)
                    {
                        query = @"insert into tbl_liveratehistory(liveratehistory_liverateid,liveratehistory_currentrate,liveratehistory_date,liveratehistory_modifieddate)
                                select liverate_liverateid,liverate_currentrate,getdate(),getdate() from tbl_liverate 
                                where liverate_rtdcode='" + rtd + "' and liverate_issavehistory=1 and isnull(liverate_isapirate,0)=0";
                        DbTable.ExecuteQuery(query);
                    }
                }
            }
        }
        //ErrorLog.WriteLog(Environment.NewLine+"Update At:" + DateTime.Now);
        HttpContext.Current.Response.Write("Success");
    }
    private bool IsValidRate(string value)
    {
        if (value == "" || value == "N/A" || value == "NaN" || value == "") return false;
        double dbl = 0;
        if (double.TryParse(value, out dbl))
        {
            if (Convert.ToDouble(value) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return true;
    }
    private void UpdateCommodityRate(ArrayList arrRtd, Array arrValues, string startwith, int metalId, int liveRateId, bool isHistoryUpdate)
    {
        Hashtable hstbl = new Hashtable();
        int i = 0;
        try
        {
            
            string cols = "bid,ask,open,high,low,change,ctweekhigh,ctmonthhigh,ctmonthlow,cash,ctweeklow,close,threemonths,oneyear";
            string[] arrcols = cols.Split(',');
            for (i = 0; i < arrRtd.Count; i++)
            {
                string rtd = arrRtd[i].ToString().Trim();
                string value = arrValues.GetValue(i).ToString().Trim();
                if (rtd.StartsWith(startwith + "_") && value != "")
                {
                    string rtdcode = rtd.Replace(startwith + "_", "");
                    string colname = "";
                    if (rtdcode == "DATE_TIME")
                    {
                        colname = "date";
                        if (value == "0" || value == "" || value == "N/A")
                        {
                            return;
                        }
                        Array arr = value.Split('/');
                        value = arr.GetValue(1).ToString() + "/" + arr.GetValue(0).ToString() + "/" + arr.GetValue(2).ToString();
                    }
                    else if (rtdcode == "PERCENT_CHANGE")
                    {
                        colname = "changeper";
                    }
                    else if (rtdcode == "1WEEK_PERCENT_CHANGE")
                    {
                        colname = "oneweekchangeper";
                    }
                    else if (rtdcode == "1MONTH_PERCENT_CHANGE")
                    {
                        colname = "onemonthchangeper";
                    }
                    else if (rtdcode == "3MONTH_PERCENT_CHANGE")
                    {
                        colname = "threemonthchangeper";
                    }
                    else if (rtdcode == "1YEAR_PERCENT_CHANGE")
                    {
                        colname = "oneyearchangeper";
                    }
                    else if (rtdcode == "52 W HIGH")
                    {
                        colname = "fiftytwoweekhigh";
                    }
                    else if (rtdcode == "52 W LOW")
                    {
                        colname = "fiftytwoweeklow";
                    }
                    else if (rtdcode == "CLOSE")
                    {
                        colname = "prevclose";
                    }
                    //else if (rtdcode == "ctweekhigh" || rtdcode == "ctweeklow" || rtdcode == "ctmonthhigh" || rtdcode == "ctmonthlow")
                    //{
                    //    colname = rtdcode;
                    //}
                    else
                    {
                        if (arrcols.Contains(rtdcode.ToLower()))
                        {
                            colname = rtdcode.ToLower();
                        }
                    }
                    if (colname != "")
                    {
                        hstbl.Add(colname, value);
                    }
                }
            }
            if (hstbl.Keys.Count == 0) return;
            InsertUpdate obj = new InsertUpdate();
            hstbl.Add("metalid", metalId);
            obj.UpdateData(hstbl, "tbl_MetalLiveRate", liveRateId);

            //ErrorLog.WriteLog(Environment.NewLine + "Update metalid :" + metalId);
            //ErrorLog.WriteLog(Environment.NewLine + "qry :" + obj._query);

            if (isHistoryUpdate)
            {
                string query = @"insert into tbl_MetalLiveRateHistory
(metalliveratehistory_metalid,metalliveratehistory_bid,metalliveratehistory_ask,metalliveratehistory_open,metalliveratehistory_high,
metalliveratehistory_low,metalliveratehistory_prevclose,metalliveratehistory_change,metalliveratehistory_changeper,metalliveratehistory_ctweekhigh,
metalliveratehistory_ctmonthhigh,metalliveratehistory_ctmonthlow,metalliveratehistory_cash,metalliveratehistory_createddate,metalliveratehistory_createdby,
metalliveratehistory_date,metalliveratehistory_close,metalliveratehistory_oneweekchangeper,
metalliveratehistory_onemonthchangeper,metalliveratehistory_threemonthchangeper,metalliveratehistory_oneyearchangeper,metalliveratehistory_fiftytwoweekhigh,
metalliveratehistory_threemonths,metalliveratehistory_oneyear,metalliveratehistory_fiftytwoweeklow)
select metalliverate_metalid,metalliverate_bid,metalliverate_ask,metalliverate_open,metalliverate_high,
metalliverate_low,metalliverate_prevclose,metalliverate_change,metalliverate_changeper,metalliverate_ctweekhigh,
metalliverate_ctmonthhigh,metalliverate_ctmonthlow,metalliverate_cash,getdate(),1,
metalliverate_date,metalliverate_close,metalliverate_oneweekchangeper,
metalliverate_onemonthchangeper,metalliverate_threemonthchangeper,metalliverate_oneyearchangeper,metalliverate_fiftytwoweekhigh,
metalliverate_threemonths,metalliverate_oneyear,metalliverate_fiftytwoweeklow
from tbl_MetalLiveRate where MetalLiveRate_MetalLiveRateId=" + liveRateId;
                DbTable.ExecuteQuery(query);
                //obj.InsertData(hstbl, "tbl_MetalLiveRateHistory");
            }
        }
        catch (Exception ex)
        {
            ErrorLog.WriteLog("Error in UpdateCommodityRate: "+ex.Message);
            throw ex;
        }
    }
    private void GetDashboardTicker()
    {
        string query = "";
        int metalId = Common.GetQueryStringValue("mid");
        query = @"select MetalLiveRate_date as date, MetalLiveRate_metalid as mid, MetalLiveRate_bid as bid,MetalLiveRate_ask as ask,MetalLiveRate_open as [open],
                    MetalLiveRate_high as high,MetalLiveRate_low as low,MetalLiveRate_change as change,metalliverate_changeper as changeper,
                    metalliverate_oneweekchangeper as oneweekchangeper,metalliverate_onemonthchangeper as onemonthchangeper,
                    metalliverate_threemonthchangeper as threemonthchangeper,metalliverate_oneyearchangeper as oneyearchangeper,
                    metalliverate_fiftytwoweekhigh as fiftytwoweekhigh, metalliverate_fiftytwoweeklow as fiftytwoweeklow
                    from tbl_MetalLiveRate where DATEDIFF(ss, MetalLiveRate_modifieddate,GETDATE())<=5";//5seconds
        if (metalId > 0) query += " and MetalLiveRate_MetalLiveRateid="+metalId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        string json = JSON.Convert(dttbl);
        HttpContext.Current.Response.Write(json);
    }
    public void UpdateCommodityRate(string sqldate)
    {
        string query = "";
        query = @"update tbl_DailyLMEMetalRate 
                    set dailylmemetalrate_usdinrclose=(select top 1 historicaldata_close from tbl_historicaldata where cast(historicaldata_date as date)=cast('" + sqldate + @"' as date) and historicaldata_currencyid=2),
                    dailylmemetalrate_usdinrrbirefrate=(select top 1 historicaldata_rbirefrate from tbl_historicaldata where cast(historicaldata_date as date)=cast('" + sqldate + @"' as date) and historicaldata_currencyid=2)
                    where cast(DailyLMEMetalRate_date as date)=cast('" + sqldate + @"' as date)
                    ";
        DbTable.ExecuteQuery(query);
    }
    public string GetLMEWarehouseStockHtml(string currentDate)
    {
        string query = "";
        if (currentDate != "")
        {
            query = "select top 1 * from tbl_lmestockmetalrate where cast(lmestockmetalrate_date as date)<>cast('" + currentDate + "' as date)";
            DataRow dr1 = DbTable.ExecuteSelectRow(query);
            if (dr1 == null) return "";
        }
        query = @"select * from tbl_lmestockmetalrate";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        string date = GlobalUtilities.ConvertToDateMMM(Convert.ToDateTime(dttbl.Rows[0]["lmestockmetalrate_date"] == DBNull.Value ? DateTime.Now : dttbl.Rows[0]["lmestockmetalrate_date"]));
        StringBuilder html = new StringBuilder();
        string nextYear = "Dec-" + Convert.ToString(DateTime.Now.Year + 1).Substring(2);
        html.Append("<table width='100%' cellspacing=0 cellpadding=0>");
        html.Append("<tr><td class='jq-lmestock-date'>" + date + "</td></tr>");
        html.Append("<tr><td>");
        html.Append("<table width='100%' cellspacing=0 cellpadding=0 class='repeater' border=1>");
        html.Append(@"<tr class='repeater-header'><td>METAL</td><td>Live Warrants</td><td>Cancelled Warrants</td><td>Opening Stock(Total)</td>
                        <td>CHANGE (+ /-)</td>
                      </tr>");
        BindMetals();
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            DataRow dr = dttbl.Rows[i];
            int metalId = GlobalUtilities.ConvertToInt(dr["lmestockmetalrate_metalid"]);
            string metalName = GetMetalName(metalId);
            double livewarrants = GlobalUtilities.ConvertToDouble(dr["lmestockmetalrate_livewarrants"]);
            double cancelledwarrants = GlobalUtilities.ConvertToDouble(dr["lmestockmetalrate_cancelledwarrants"]);
            double openingstock = GlobalUtilities.ConvertToDouble(dr["lmestockmetalrate_openingstock"]);
            double prevdayopeningstock = GlobalUtilities.ConvertToDouble(dr["lmestockmetalrate_prevdayopeningstock"]);
            double totalopeningstock = livewarrants + cancelledwarrants;
            double change = GlobalUtilities.ConvertToDouble(dr["lmestockmetalrate_stocknetchange"]);//totalopeningstock - prevdayopeningstock;
            html.Append("<tr>");
            html.Append("<td class='repeater-header-left'>" + metalName + "</td>");
            html.Append("<td><div class='rate'>" + livewarrants + "</div></td>");
            html.Append("<td><div class='rate'>" + cancelledwarrants + "</div></td>");
            html.Append("<td><div class='rate'>" + totalopeningstock + "</div></td>");
            //html.Append("<td><div class='rate'>" + prevdayopeningstock + "</div></td>");
            html.Append("<td><div class='rate'>" + change + "</div></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        html.Append("</td></tr></table>");
        return html.ToString();
        //ltstock.Text = html.ToString();
    }
    public string GetLMESettlementRateHtml(string currentDate)
    {
        string query = "";
        if (currentDate != "")
        {
            query = "select top 1 * from tbl_lmesettlementmetalrate where cast(lmesettlementmetalrate_date as date)<>cast('" + currentDate + "' as date)";
            DataRow dr1 = DbTable.ExecuteSelectRow(query);
            if (dr1 == null) return "";
        }

        query = @"select * from tbl_lmesettlementmetalrate";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        string date = GlobalUtilities.ConvertToDateMMM(Convert.ToDateTime(dttbl.Rows[0]["lmesettlementmetalrate_date"] == DBNull.Value ? DateTime.Now : dttbl.Rows[0]["lmesettlementmetalrate_date"]));
        StringBuilder html = new StringBuilder();
        string nextYear = "Dec-" + Convert.ToString(DateTime.Now.Year + 1).Substring(2);
        string nextYear2 = "Dec-" + Convert.ToString(DateTime.Now.Year + 2).Substring(2);
        string nextYear3 = "Dec-" + Convert.ToString(DateTime.Now.Year + 3).Substring(2);
        html.Append("<table width='100%' cellspacing=0 cellpadding=0>");
        html.Append("<tr><td class='jq-lmesettlement-date'>" + date + "</td></tr>");
        html.Append("<tr><td>");
        html.Append("<table width='100%' cellspacing=0 cellpadding=0 class='repeater' border=1>");
        html.Append(@"<tr class='repeater-header'><td>METAL</td><td colspan=2>Cash Settlement</td><td class='nowrap' colspan=2>3M-Settlement</td>
                            <td class='nowrap' colspan=2>" + nextYear + @" <img src='images/arrow_expand_right.png' class='jq-lme-settlement-expand hand' title='Show more'></td>" +
                            @"<td class='nowrap jq-lme-settlement-more' colspan=2>" + nextYear2 +
                            @"</td><td class='nowrap jq-lme-settlement-more' colspan=2>" + nextYear3 + @"</td>
                    </tr>");
        html.Append(@"<tr class='repeater-header'><td></td><td>Bid</td><td>Ask</td><td>Bid</td><td>Ask</td><td>Bid</td><td>Ask</td>
                    <td class='jq-lme-settlement-more'>Bid</td><td class='jq-lme-settlement-more'>Ask</td><td class='jq-lme-settlement-more'>Bid</td><td class='jq-lme-settlement-more'>Ask</td></tr>");
        BindMetals();
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            DataRow dr = dttbl.Rows[i];
            int id = GlobalUtilities.ConvertToInt(dr["lmesettlementmetalrate_lmesettlementmetalrateid"]);
            int metalId = GlobalUtilities.ConvertToInt(dr["lmesettlementmetalrate_metalid"]);
            string metalName = GetMetalName(metalId);
            double cashAsk = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_cashask"]);
            double cashbid = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_cashbid"]);
            double threemonthsAsk = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_threemonthsask"]);
            double threemonthsBid = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_threemonthsbid"]);
            double oneyearask = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_oneyearask"]);
            double oneyearbid = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_oneyearbid"]);
            double twoyearask = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_twoyearsask"]);
            double twoyearbid = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_twoyearsbid"]);
            double threeyearask = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_threeyearsask"]);
            double threeyearbid = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_threeyearsbid"]);

            html.Append("<tr>");
            html.Append("<td class='repeater-header-left'>" + metalName + "</td>");

            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(cashbid, 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(cashAsk, 2) + "</div></td>");

            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(threemonthsBid, 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(threemonthsAsk, 2) + "</div></td>");

            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(oneyearbid, 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(oneyearask, 2) + "</div></td>");

            html.Append("<td><div class='rate jq-lme-settlement-more'>" + ExportExposurePortal.DecimalPoint(twoyearbid, 2) + "</div></td>");
            html.Append("<td><div class='rate jq-lme-settlement-more'>" + ExportExposurePortal.DecimalPoint(twoyearask, 2) + "</div></td>");

            html.Append("<td><div class='rate jq-lme-settlement-more'>" + ExportExposurePortal.DecimalPoint(threeyearbid, 2) + "</div></td>");
            html.Append("<td><div class='rate jq-lme-settlement-more'>" + ExportExposurePortal.DecimalPoint(threeyearask, 2) + "</div></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        html.Append("</td></tr></table>");
        return html.ToString();
    }
    
    private string GetMetalName(int metalId)
    {
        for (int i = 0; i < _dttblMetal.Rows.Count; i++)
        {
            if (Convert.ToInt32(_dttblMetal.Rows[i]["metal_metalid"]) == metalId)
            {
                return Convert.ToString(_dttblMetal.Rows[i]["metal_metalname"]);
            }
        }
        return "";
    }
    private void BindMetals()
    {
        string query = "select * from tbl_metal";
        _dttblMetal = DbTable.ExecuteSelect(query);
    }
}
