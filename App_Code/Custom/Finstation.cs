using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using WebComponent;
using System.Collections;
using System.Web.UI.WebControls;

public enum Enum_Prospect
{
    Mobile_Ticker = 1,
    Full_Finstation = 2,
    Website = 3,
    Export_Portal = 4,
    Import_Portal = 5,
    Metal_Commodity = 6,
    Mini_Finstation = 7,
    BankScan = 8
}
public enum Enum_AppType
{
    Finstation = 1,
    Finwatch = 2,
    FinIcon = 3,
    FinPulse = 4
}
public enum Enum_CurrencyType
{
    INRCurrencies = 1,
    CrossCurrencies = 2,
    OffshoreCurrencies = 3,
    IndicesAndCommodities = 4,
    GlobalIndicesFutures = 5,
    GovernmentBondYield = 6
}
public enum Enum_Currency
{
    INR = 1,
    USD = 2,
    EUR = 3,
    GBP = 4,
    JPY = 5,
}
public enum Enum_CurrencyLiveRateColumn
{
    None = 0,
    Bid = 1,
    Ask = 2,
    PercentageChg = 3,
    Open = 4,
    High = 5,
    Low = 6
}
public enum Enum_Role
{
    Administrator = 1,
    Advisor = 7,
    Research = 8,
    Accounts = 9,
    EXIMAdvisor = 10,
    FEMAAdvisor = 11,
    AuditAdvisor = 12
}
public enum Enum_PushnotificationApplicationType
{
    Finstation = 1,
    Finwatch = 2,
    FinIcon = 3
}
public class Finstation
{
    public void Process()
    {
        string m = Common.GetQueryString("m");
        if (m == "getspotratelivechart")
        {
            GetSportLiveRateChart();
        }
        else if (m == "cashconversion")
        {
            GetCashConversion();
        }
        else if (m == "getcurrencymargin")
        {
            GetCurrencyMargin();
        }
        else if (m == "getliverateuserconfig")
        {
            int clientUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
            int currencyTypeId = Common.GetQueryStringValue("ctype");
            Enum_AppType appType = (Enum_AppType)Common.GetQueryStringValue("apptype");
            string html = GetLiverateUserConfig(clientUserId, appType, currencyTypeId);
            HttpContext.Current.Response.Write(html);
        }
        else if (m == "saveliverateuserconfig")
        {
            string currencies = Common.GetQueryString("cids");
            int appTypeId = Common.GetQueryStringValue("apptype");
            int clientUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
            int currencyTypeId = Common.GetQueryStringValue("ctype");
            SaveLiverateUserConfig(clientUserId, appTypeId, currencyTypeId, currencies);
            HttpContext.Current.Response.Write("ok");
        }
        else if (m == "getscrollnews")
        {
            GetScrollNews();
        }
        else if (m == "getannualisedpremium-monthlyvalues")
        {
            bool isExport = Common.GetQueryStringBool("isexport");
            Enum_Currency curreny = (Enum_Currency)Common.GetQueryStringValue("currencyid");
            string values = GetAnnualisedPremiumMonthlyValues(curreny, isExport);
            HttpContext.Current.Response.Write(values);
        }
    }
    public static int QueryTypeId
    {
        get
        {
            if (Common.RoleId == 10) return 1;
            if (Common.RoleId == 11) return 2;
            return 0;
        }
    }
    public static bool IsAdvisor_Exim
    {
        get
        {
            if (Common.RoleId == 10) return true;
            return false;
        }
    }
    public static bool IsAdvisor_Fema
    {
        get
        {
            if (Common.RoleId == 11) return true;
            return false;
        }
    }
    public static bool IsAdvisor
    {
        get
        {
            return IsAdvisor_Exim || IsAdvisor_Fema;
        }
    }
    private void GetSportLiveRateChart()
    {
        string query = "";
        int dateid = Common.GetQueryStringValue("dateid");
        int lastId = Common.GetQueryStringValue("lastid");
        int currencyId = Common.GetQueryStringValue("cid");
        int labelType = 0;
        if (dateid == 0)
        {
            int liverateId = 0;
            if (currencyId == 2)//USDINR
            {
                liverateId = 1;
            }
            else if (currencyId == 3)//EURINR
            {
                liverateId = 10;
            }
            else if (currencyId == 4)//GBPINR
            {
                liverateId = 19;
            }
            else if (currencyId == 5)//JPYINR
            {
                liverateId = 28;
            }
            query = @"select liveratehistory_liveratehistoryid as id, liveratehistory_date as lbl,
                    liveratehistory_currentrate as val from tbl_liveratehistory 
                    where liveratehistory_liverateid=" + liverateId;
            if (lastId > 0)
            {
                query += " and liveratehistory_liveratehistoryid>" + lastId;
            }
            query += " order by liveratehistory_liveratehistoryid";
            labelType = 2;
        }
        else
        {
            labelType = 1;
            string startdate = "";
            string enddate = "";

            if (dateid == 7)
            {
                query = @"select historicaldata_historicaldataid as id, cast(historicaldata_date as date) as lbl,
                    historicaldata_close as val from tbl_historicaldata 
                    where historicaldata_currencyid=" + currencyId;

                if (Common.GetQueryString("sd") != "")
                {
                    startdate = GlobalUtilities.ConvertToSqlDate(GlobalUtilities.ConvertToDateFromTextBox(Common.GetQueryString("sd")));
                    query += " and cast(historicaldata_date as date)>=cast('" + startdate + "' as date)";
                }
                if (Common.GetQueryString("ed") != "")
                {
                    enddate = GlobalUtilities.ConvertToSqlDate(GlobalUtilities.ConvertToDateFromTextBox(Common.GetQueryString("ed")));
                    query += " and cast(historicaldata_date as date)<=cast('" + enddate + "' as date)";
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
                query = @"select historicaldata_historicaldataid as id, cast(historicaldata_date as date) as lbl,
                        historicaldata_close as val from(
                        select top " + top + " * from tbl_historicaldata where historicaldata_currencyid=" + currencyId + @" order by historicaldata_date desc
                        )r order by historicaldata_date";
            }
        }

        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder lbls = new StringBuilder();
        StringBuilder vals = new StringBuilder();
        GetChartData(dttbl, labelType, ref lbls, ref vals);
        if (dttbl.Rows.Count > 0) lastId = GlobalUtilities.ConvertToInt(dttbl.Rows[dttbl.Rows.Count - 1]["id"]);
        string json = "{\"lbls\":\"" + lbls.ToString() + "\",\"vals\":\"" + vals.ToString() + "\",\"lastid\":\"" + lastId + "\"}";
        HttpContext.Current.Response.Write(json);
    }
    public static void GetChartData(DataTable dttbl, int labelType, ref StringBuilder lbls, ref StringBuilder vals)
    {
        vals = new StringBuilder();
        lbls = new StringBuilder();

        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string val = GlobalUtilities.ConvertToString(dttbl.Rows[i]["val"]);
            string lbl = "";
            if (labelType == 1)
            {
                lbl = GlobalUtilities.ConvertToDate(dttbl.Rows[i]["lbl"]);
            }
            else if (labelType == 2)
            {
                if (dttbl.Rows[i]["lbl"] != DBNull.Value) lbl = GlobalUtilities.ConvertToTime(dttbl.Rows[i]["lbl"]);
            }
            else
            {
                lbl = GlobalUtilities.ConvertToString(dttbl.Rows[i]["lbl"]);
            }
            if (i == 0)
            {
                vals.Append(val);
                lbls.Append(lbl);
            }
            else
            {
                vals.Append("," + val);
                lbls.Append("," + lbl);
            }
        }
    }
    public static string GetHistoricalData(int currencyId, string fromDate, string toDate, EnumFinstationHistoryType historyType)
    {
        string query = "";
        if (historyType == EnumFinstationHistoryType.MonthlyAvg)
        {
            query = @"select * from tbl_historicaldata";
            query += " where historicaldata_currencyid=" + currencyId;
            if (fromDate != "") query += " AND cast(historicaldata_date as date)>=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(fromDate)) + "' as date)";
            if (toDate != "") query += " AND cast(historicaldata_date as date)<=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(toDate)) + "' as date)";
            query += "order by historicaldata_date desc";
        }
        else
        {
            query = "select * from tbl_historicaldata";
            query += " where historicaldata_currencyid=" + currencyId;
            if (fromDate != "") query += " AND cast(historicaldata_date as date)>=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(fromDate)) + "' as date)";
            if (toDate != "") query += " AND cast(historicaldata_date as date)<=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(toDate)) + "' as date)";
            query += @"order by historicaldata_date desc";
        }
        //ErrorLog.WriteLog(query);
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        if (historyType == EnumFinstationHistoryType.RBIRefRate)
        {
            html.Append("<tr class='grid-ui-header'><td>Date</td><td>RBI Ref Rate</td></tr>");
        }
        else if (historyType == EnumFinstationHistoryType.MonthlyAvg)
        {
            DataTable dttblmonthly = Finstation.GetAvgRateTemplate();
            int prevmonth = 0;
            int prevyear = 0;
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                DateTime dt = Convert.ToDateTime(dttbl.Rows[i]["historicaldata_date"]);
                if (!(dt.Month == prevmonth && dt.Year == prevyear))
                {
                    DataRow dr = dttblmonthly.NewRow();
                    dr["FromDate"] = new DateTime(dt.Year, dt.Month, 1);
                    dr["ToDate"] = new DateTime(dt.Year, dt.Month, GlobalUtilities.GetMonthEndDay(dt.Month, dt.Year), 23, 59, 59);
                    dttblmonthly.Rows.Add(dr);
                }
                prevmonth = dt.Month;
                prevyear = dt.Year;
            }
            dttbl = Finstation.CalculateAvgRatesByDateRange(dttbl, dttblmonthly, true);

            html.Append("<tr class='grid-ui-header'><td>Month</td><td>Open</td><td>High</td><td>Low</td><td>Close</td><td>Daily Closing price Avg</td></tr>");
        }
        else
        {
            html.Append("<tr class='grid-ui-header'><td>Date</td><td>Open</td><td>High</td><td>Low</td><td>Close</td><td>Average</td><td>% Change</td><td>RBI Ref Rate</td></tr>");
        }
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string css = "grid-ui-alt";
            if (i % 2 == 0) css = "grid-ui-row";
            html.Append("<tr class='" + css + "'>");
            if (historyType == EnumFinstationHistoryType.MonthlyAvg)
            {
                DateTime dtfrom = Convert.ToDateTime(dttbl.Rows[i]["FromDate"]);
                html.Append("<td>" + GlobalUtilities.GetMonthShortName(dtfrom.Month) + " - " + dtfrom.Year + "</td>");
                html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["open"]) + "</td>");
                html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["high"]) + "</td>");
                html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["low"]) + "</td>");
                html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["close"]) + "</td>");
                html.Append("<td>" + String.Format("{0:0.0000}", Convert.ToDouble(dttbl.Rows[i]["avgclose"])) + "</td>");
            }
            else
            {
                html.Append("<td>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["historicaldata_date"]) + "</td>");
                if (historyType != EnumFinstationHistoryType.RBIRefRate)
                {
                    html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["historicaldata_open"]) + "</td>");
                    html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["historicaldata_high"]) + "</td>");
                    html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["historicaldata_low"]) + "</td>");
                    html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["historicaldata_close"]) + "</td>");
                    html.Append("<td>" + String.Format("{0:0.00}", dttbl.Rows[i]["historicaldata_average"]) + "</td>");
                    html.Append("<td>" + String.Format("{0:0.00}", dttbl.Rows[i]["historicaldata_change"]) + "</td>");
                }
                html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["historicaldata_rbirefrate"]) + "</td>");
            }
            html.Append("</tr>");
        }
        html.Append("</table>");
        return html.ToString();
    }
    public static void BindCurrency(Literal ltCurrency, int sectionId)
    {
        string query = "select * from tbl_liverate " +
                       "JOIN tbl_liveratesection ON liveratesection_liveratesectionid=liverate_liveratesectionid " +
                       "where liverate_liveratesectionid=" + sectionId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        int bidIndex = 0;
        int askIndex = 1;
        int dateIndex = 2;
        if (sectionId == 32 || sectionId == 34 || sectionId == 36)
        {
            dateIndex = 0;
            bidIndex = 1;
            askIndex = 2;
        }
        //bid rate
        StringBuilder html = new StringBuilder();
        double dblPrevRate = 0;// GlobalUtilities.ConvertToDouble(dttbl.Rows[0]["liverate_prevrate"]);
        double dblCurrentRate = 0;// GlobalUtilities.ConvertToDouble(dttbl.Rows[0]["liverate_currentrate"]);
        double.TryParse(GlobalUtilities.ConvertToString(dttbl.Rows[bidIndex]["liverate_prevrate"]), out dblPrevRate);
        double.TryParse(GlobalUtilities.ConvertToString(dttbl.Rows[bidIndex]["liverate_currentrate"]), out dblCurrentRate);
        string sectionCode = GlobalUtilities.ConvertToString(dttbl.Rows[bidIndex]["liveratesection_code"]);
        int liverateId = GlobalUtilities.ConvertToInt(dttbl.Rows[bidIndex]["liverate_liverateid"]);
        string rtdCode = GlobalUtilities.ConvertToString(dttbl.Rows[bidIndex]["liverate_rtdcode"]);
        string section = GlobalUtilities.ConvertToString(dttbl.Rows[0]["liveratesection_name"]);
        int row = GlobalUtilities.ConvertToInt(dttbl.Rows[bidIndex]["liverate_row"]);
        int col = GlobalUtilities.ConvertToInt(dttbl.Rows[bidIndex]["liverate_column"]);

        string cls = "";
        string code = sectionCode + "_" + row + "_" + col;

        if (dblCurrentRate > dblPrevRate)
        {
            cls = "rate-up";
        }
        else
        {
            cls = "rate-down";
        }
        cls += " " + code;

        html.Append("<td class='" + cls + " rate jq-" + section + "-spotrate-bid liverate' " +
                    "rid='" + liverateId + "' rc='" + rtdCode + "' c='" + code + "' istick=1>" + dblCurrentRate + "</td>");

        //ask rate
        dblPrevRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[askIndex]["liverate_prevrate"]);
        dblCurrentRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[askIndex]["liverate_currentrate"]);
        sectionCode = GlobalUtilities.ConvertToString(dttbl.Rows[askIndex]["liveratesection_code"]);
        liverateId = GlobalUtilities.ConvertToInt(dttbl.Rows[askIndex]["liverate_liverateid"]);
        rtdCode = GlobalUtilities.ConvertToString(dttbl.Rows[askIndex]["liverate_rtdcode"]);
        row = GlobalUtilities.ConvertToInt(dttbl.Rows[askIndex]["liverate_row"]);
        col = GlobalUtilities.ConvertToInt(dttbl.Rows[askIndex]["liverate_column"]);

        cls = "";
        code = sectionCode + "_" + row + "_" + col;

        if (dblCurrentRate > dblPrevRate)
        {
            cls = "rate-up";
        }
        else
        {
            cls = "rate-down";
        }
        cls += " " + code;

        html.Append("<td class='" + cls + " rate jq-" + section + "-spotrate-ask liverate' rid='" + 
            liverateId + "' rc='" + rtdCode + "' c='" + code + "' istick=1>" + dblCurrentRate + "</td>");

        //Spot Date
        string date = GlobalUtilities.ConvertToString(dttbl.Rows[dateIndex]["liverate_currentrate"]);
        sectionCode = GlobalUtilities.ConvertToString(dttbl.Rows[dateIndex]["liveratesection_code"]);
        liverateId = GlobalUtilities.ConvertToInt(dttbl.Rows[dateIndex]["liverate_liverateid"]);
        rtdCode = GlobalUtilities.ConvertToString(dttbl.Rows[dateIndex]["liverate_rtdcode"]);
        row = GlobalUtilities.ConvertToInt(dttbl.Rows[dateIndex]["liverate_row"]);
        col = GlobalUtilities.ConvertToInt(dttbl.Rows[dateIndex]["liverate_column"]);

        code = sectionCode + "_" + row + "_" + col;
        html.Append("<td style='padding-left:10px' rid='" + liverateId + "'>Spot Date</td><td class='" + code +
                    " jq-" + section + "-spotdate jq-" + section.ToLower() + "-spotdate liverate' rid='" + liverateId + "' rc='" + rtdCode + "' c='" + code + "' >" + date + "</td>");

        ltCurrency.Text = "<table><tr>" + html.ToString() + "</tr></table>";
    }
    public static string GetCurrency_History(int sectionId, string liverateDate)
    {
        string query = @"select * from tbl_liverate 
                       join tbl_dailyhistoricalliverate on dailyhistoricalliverate_liverateid=liverate_liverateid
                       JOIN tbl_liveratesection ON liveratesection_liveratesectionid=liverate_liveratesectionid 
                       where liverate_liveratesectionid=" + sectionId + " and cast(dailyhistoricalliverate_date as date)=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(liverateDate)) + "' as date)";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        if (!GlobalUtilities.IsValidaTable(dttbl)) return "";
        
        //bid rate
        StringBuilder html = new StringBuilder();

        double dblPrevRate = GlobalUtilities.ConvertToDoubleZeroIfError(dttbl.Rows[0]["dailyhistoricalliverate_currentrate"]);
        double dblCurrentRate = GlobalUtilities.ConvertToDoubleZeroIfError(dttbl.Rows[0]["dailyhistoricalliverate_currentrate"]);
        string sectionCode = GlobalUtilities.ConvertToString(dttbl.Rows[0]["liveratesection_code"]);
        int liverateId = GlobalUtilities.ConvertToInt(dttbl.Rows[0]["liverate_liverateid"]);
        string rtdCode = GlobalUtilities.ConvertToString(dttbl.Rows[0]["liverate_rtdcode"]);
        int row = GlobalUtilities.ConvertToInt(dttbl.Rows[0]["liverate_row"]);
        int col = GlobalUtilities.ConvertToInt(dttbl.Rows[0]["liverate_column"]);

        string cls = "";
        string code = sectionCode + "_" + row + "_" + col;

        if (dblCurrentRate > dblPrevRate)
        {
            cls = "rate-up";
        }
        else
        {
            cls = "rate-down";
        }
        cls += " " + code;

        html.Append("<td class='" + cls + " rate liverate' rid='" + liverateId + "' rc='" + rtdCode + "' c='" + code + "'>" + dblCurrentRate + "</td>");

        //ask rate
        //string strPrevRate = GlobalUtilities.ConvertToString(dttbl.Rows[1]["dailyhistoricalliverate_currentrate"]);
        string strCurrentRate = GlobalUtilities.ConvertToString(dttbl.Rows[1]["dailyhistoricalliverate_currentrate"]);
        sectionCode = GlobalUtilities.ConvertToString(dttbl.Rows[1]["liveratesection_code"]);
        liverateId = GlobalUtilities.ConvertToInt(dttbl.Rows[1]["liverate_liverateid"]);
        rtdCode = GlobalUtilities.ConvertToString(dttbl.Rows[1]["liverate_rtdcode"]);
        row = GlobalUtilities.ConvertToInt(dttbl.Rows[1]["liverate_row"]);
        col = GlobalUtilities.ConvertToInt(dttbl.Rows[1]["liverate_column"]);

        cls = "";
        code = sectionCode + "_" + row + "_" + col;

        if (dblCurrentRate > dblPrevRate)
        {
            cls = "rate-up";
        }
        else
        {
            cls = "rate-down";
        }
        cls += " " + code;

        html.Append("<td class='" + cls + " rate liverate' rid='" + liverateId + "' rc='" + rtdCode + "' c='" + code + "'>" + strCurrentRate + "</td>");

        //Spot Date
        string date = GlobalUtilities.ConvertToString(dttbl.Rows[2]["dailyhistoricalliverate_currentrate"]);
        sectionCode = GlobalUtilities.ConvertToString(dttbl.Rows[2]["liveratesection_code"]);
        liverateId = GlobalUtilities.ConvertToInt(dttbl.Rows[2]["liverate_liverateid"]);
        rtdCode = GlobalUtilities.ConvertToString(dttbl.Rows[2]["liverate_rtdcode"]);
        row = GlobalUtilities.ConvertToInt(dttbl.Rows[2]["liverate_row"]);
        col = GlobalUtilities.ConvertToInt(dttbl.Rows[2]["liverate_column"]);

        code = sectionCode + "_" + row + "_" + col;
        html.Append("<td style='padding-left:10px'>Spot Date</td><td class='" + code + " liverate' rid='" + liverateId + "' rc='" + 
                    rtdCode + "' c='" + code + "'>" + date + "</td>");

        return html.ToString();
    }
    public static double GetLiveRate(int liverateid)
    {
        string query = "select * from tbl_liverate where liverate_liverateid=" + liverateid;
        DataRow dr1 = DbTable.ExecuteSelectRow(query);
        double rate = GlobalUtilities.ConvertToDouble(dr1["liverate_currentrate"]);
        string calc = GlobalUtilities.ConvertToString(dr1["liverate_calculation"]);
        if (calc != "")
        {
            rate = GetLiveRateCalc(rate, calc);
        }
        ////GBP, JPY
        //if (liverateid == 1369 || liverateid == 526)
        //{
        //    rate = rate / 100.0;
        //}
        return rate;
    }
    public static string GetLiveRateText(int liverateid)
    {
        string query = "select * from tbl_liverate where liverate_liverateid=" + liverateid;
        DataRow dr1 = DbTable.ExecuteSelectRow(query);
        string rate = GlobalUtilities.ConvertToString(dr1["liverate_currentrate"]);
        return rate;
    }
    public static int GetSpotRateLiveRateId(bool isExport, int currency)
    {
        int liverateid = 0;
        if (isExport)//export
        {
            if (currency == 1)
            {
                liverateid = 1;
            }
            else if (currency == 2)
            {
                liverateid = 10;
            }
            else if (currency == 3)
            {
                liverateid = 19;
            }
            else if (currency == 4)
            {
                liverateid = 28;
            }
        }
        else//import
        {
            if (currency == 1)
            {
                liverateid = 2;
            }
            else if (currency == 2)
            {
                liverateid = 11;
            }
            else if (currency == 3)
            {
                liverateid = 20;
            }
            else if (currency == 4)
            {
                liverateid = 29;
            }
        }
        return liverateid;
    }
    public static double GetSpotRate(bool isExport, int currency)
    {
        //bind spot rate
        int liverateid = GetSpotRateLiveRateId(isExport, currency);
        double rate = GetLiveRate(liverateid);
        return rate;
    }
    public static int GetCashSpotLiveRateId(bool isExport, int currency)
    {
        int liverateid = 0;
        if (isExport)//export
        {
            if (currency == 1)
            {
                liverateid = 207;
            }
            else if (currency == 2)
            {
                liverateid = 1265;
            }
            else if (currency == 3)
            {
                liverateid = 1369;
            }
            else if (currency == 4)
            {
                liverateid = 526;
            }
            else if (currency == 5)
            {
                liverateid = 630;
            }
            else if (currency == 6)
            {
                liverateid = 734;
            }
            else if (currency == 7)
            {
                liverateid = 838;
            }
        }
        else//import
        {
            if (currency == 1)
            {
                liverateid = 220;
            }
            else if (currency == 2)
            {
                liverateid = 1278;
            }
            else if (currency == 3)
            {
                liverateid = 1382;
            }
            else if (currency == 4)
            {
                liverateid = 539;
            }
            else if (currency == 5)
            {
                liverateid = 643;
            }
            else if (currency == 6)
            {
                liverateid = 747;
            }
            else if (currency == 7)
            {
                liverateid = 851;
            }
        }
        return liverateid;
    }
    public static double GetCashSpotRate(bool isExport, int currency)
    {
        //bind spot rate
        int liverateid = GetCashSpotLiveRateId(isExport, currency);
        double rate = GetLiveRate(liverateid);
        return rate;
    }
    public static DataTable CorrectLiveRateValues(DataTable dttbl)
    {
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int liverateId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liverate_liverateid"]);
            int decimalPlaces = 0;
            if (dttbl.Columns.Contains("liverate_decimalplaces"))
            {
                decimalPlaces = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liverate_decimalplaces"]);
            }
            if (decimalPlaces > 0)
            {
                if (dttbl.Columns.Contains("liverate_currentrate"))
                {
                    double currentRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["liverate_currentrate"]);
                    dttbl.Rows[i]["liverate_currentrate"] = ExportExposurePortal.DecimalPoint(currentRate, decimalPlaces);
                }
                if (dttbl.Columns.Contains("liverate_prevrate"))
                {
                    double prevRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["liverate_prevrate"]);
                    dttbl.Rows[i]["liverate_prevrate"] = ExportExposurePortal.DecimalPoint(prevRate, decimalPlaces);
                }
                if (dttbl.Columns.Contains("cr"))
                {
                    double currentRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["cr"]);
                    dttbl.Rows[i]["cr"] = ExportExposurePortal.DecimalPoint(currentRate, decimalPlaces);
                }
            }
            else if (decimalPlaces < 0)
            {
                if (dttbl.Columns.Contains("liverate_currentrate"))
                {
                    int noofDigits = decimalPlaces * -1 + 1;
                    string strrate = Convert.ToString(dttbl.Rows[i]["liverate_currentrate"]);
                    if (strrate.Length > noofDigits)
                    {
                        strrate = strrate.Substring(0, noofDigits);
                    }
                    dttbl.Rows[i]["liverate_currentrate"] = strrate;
                }
            }
        }
        return dttbl;
    }
    public static bool IsFinstationEnabled()
    {
        return IsProspectEnabled(Enum_Prospect.Full_Finstation);
        //int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        //return IsFinstationEnabled(clientId);
    }
    
    public static bool IsMiniFinstationEnabled()
    {
        return IsProspectEnabled(Enum_Prospect.Mini_Finstation);
    }
    public static bool IsBankScanEnabled()
    {
        return IsProspectEnabled(Enum_Prospect.BankScan);
    }
    public static bool IsMetalCommodityEnabled()
    {
        return IsProspectEnabled(Enum_Prospect.Metal_Commodity);
    }
    public static bool IsExportPortalEnabled()
    {
        return IsProspectEnabled(Enum_Prospect.Export_Portal);
    }
    public static bool IsImportPortalEnabled()
    {
        return IsProspectEnabled(Enum_Prospect.Import_Portal);
    }
    
    public static bool IsProspectEnabled(Enum_Prospect prospect)
    {
        string prospects = GlobalUtilities.ConvertToString(CustomSession.Session("Login_ProspectIds"));
        if (prospects == "") return false;
        Array arr = prospects.Split(',');
        for (int i = 0; i < arr.Length; i++)
        {
            if (Convert.ToInt16(arr.GetValue(i)) == (int)prospect)
            {
                return true;
            }
        }
        return false;
    }
    public static void CheckFullFinstationAccess()
    {
        if (!IsFinstationEnabled())
        {
            HttpContext.Current.Response.Redirect("~/noaccessfortrial.aspx");
        }
    }
    public static void CheckBankScanFinstationAccess()
    {
        if (!IsBankScanEnabled())
        {
            HttpContext.Current.Response.Redirect("~/noaccessfortrial.aspx");
        }
    }
    public static void CheckMetalCommodityAccess()
    {
        if (!IsMetalCommodityEnabled())
        {
            HttpContext.Current.Response.Redirect("~/noaccessfortrial.aspx");
        }
    }
    public static void CheckImportExportPortalAccess()
    {
        if (IsImportPortalEnabled() || IsExportPortalEnabled())
        {
        }
        else
        {
            HttpContext.Current.Response.Redirect("~/noaccessfortrial.aspx");
        }
    }
    public static int GetSubscriptionId(int clientId)
    {
        string query = "";
        query = "select top 1 * from tbl_subscription where subscription_clientid=" + clientId +
                " and subscription_subscriptionstatusid=2 order by subscription_subscriptionid desc";
        DataRow drs = DbTable.ExecuteSelectRow(query);
        if (drs == null) return 0;
        return GlobalUtilities.ConvertToInt(drs["subscription_subscriptionid"]);
    }
    public static int GetTrialId(int clientId)
    {
        string query = "";
        query = "select top 1 * from tbl_trial where trial_clientid=" + clientId +
                " and trial_subscriptionstatusid=1 order by trial_trialid desc";
        DataRow drt = DbTable.ExecuteSelectRow(query);
        if (drt == null) return 0;
        return GlobalUtilities.ConvertToInt(drt["trial_trialid"]);
    }
    public static void SetFinstationProspects(int clientId)
    {
        string query = "";
        int subscriptionId = GetSubscriptionId(clientId);
        DataTable dttbl = new DataTable();
        if (subscriptionId > 0)
        {
            query = "select subscriptionprospects_prospectid as prospectid from tbl_subscriptionprospects " +
                            "join tbl_subscription ON subscription_subscriptionid=subscriptionprospects_subscriptionid " +
                            "WHERE subscription_clientid=" + clientId + " and subscription_subscriptionid=" + subscriptionId;
            dttbl = DbTable.ExecuteSelect(query);
        }
        if (dttbl.Rows.Count == 0)
        {
            int trialId = GetTrialId(clientId);
            query = "select trialprospect_prospectid as prospectid from tbl_trialprospect " +
                        "join tbl_trial ON trial_trialid=trialprospect_trialid " +
                        "WHERE trial_clientid=" + clientId;
            dttbl = DbTable.ExecuteSelect(query);
            if (dttbl.Rows.Count == 0) return;
        }
        StringBuilder prospects = new StringBuilder();
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (i == 0)
            {
                prospects.Append(dttbl.Rows[i]["prospectid"]);
            }
            else
            {
                prospects.Append("," + dttbl.Rows[i]["prospectid"]);
            }
        }
        CustomSession.Session("Login_ProspectIds", prospects);
    }
    public static bool IsFinstationEnabled(int clientId)
    {
        string query = "select * from tbl_subscriptionprospects " +
                        "join tbl_subscription ON subscription_subscriptionid=subscriptionprospects_subscriptionid " +
                        "WHERE subscriptionprospects_prospectid in (2,8) AND subscription_clientid=" + clientId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            query = "select * from tbl_trialprospect " +
                        "join tbl_trial ON trial_trialid=trialprospect_trialid " +
                        "WHERE trialprospect_prospectid in (2,8) AND trial_clientid=" + clientId;
            dr = DbTable.ExecuteSelectRow(query);
            if (dr == null) return false;
        }
        return true;
    }
    public static string GetLiverateHtml()
    {
        string m = Common.GetQueryString("m");
        string html = "";
        FinstationPortal portal = new FinstationPortal();
        if(m == "customrate")
        {
            html = GetCustomRateHtml();
            html += "<div style='padding-top:20px;'><a href='#' id='lnkhistory-customrate'>Custom Rate Archive</a></div>";
        }
        else if (m == "rbirefraterate")
        {
            html = portal.GetRateHtml(48, "", "");
            html += "<div style='padding-top:20px;'><a href='historical-data.aspx?type=2'>FBIL/RBI Ref Rates Archive</a></div>";
        }
        else if (m == "currencyfuture")
        {
            html = @"<div>
                         <input type='button' class='btncurrency btncurrency-active jq-btncurrencyfuture' value='USDINR' sid='44'/>
                         <input type='button' class='btncurrency jq-btncurrencyfuture' value='EURINR' sid='45'/>
                         <input type='button' class='btncurrency jq-btncurrencyfuture' value='GBPINR' sid='46'/>
                         <input type='button' class='btncurrency jq-btncurrencyfuture' value='JPYINR' sid='47'/>
                     </div>";
            html += "<div class='jq-currencyfuture-placeholder'>" + portal.GetRateHtml(44, "", "") + "</div>";
        }
        else if (m == "ratesection")
        {
            int sectionId = Common.GetQueryStringValue("sid");
            string columns = "";
            if (sectionId == 62)
            {
                columns = "Time,LTP,Change,%Change,Open,High,Low";
            }
            html = portal.GetRateHtml(sectionId, columns, "");
        }
        else if (m == "arr")
        {
            html = GetAlternateRefRate();
            html += "<div><a href='viewalternatereferencerates.aspx'>Alternative Reference Rates Archive</a></div>";
        }
        return html;
    }
    private static string GetCustomRateHtml()
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
        html.Append("<tr><td colspan='3'>CUSTOM EXCHANGE RATES (All Rates Per Unit) <br/><b>W.E.F. Date (" + date + ")</b></td></tr>");
        html.Append("<tr class='repeater-header'><td>CURRENCY</td><td>IMPORT</td><td>EXPORT</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string currency = GlobalUtilities.ConvertToString(dttbl.Rows[i]["othercurrency_currency"]);
            string import = GlobalUtilities.FormatAmount(dttbl.Rows[i]["customrate_import"]);
            string export = GlobalUtilities.FormatAmount(dttbl.Rows[i]["customrate_export"]);

            html.Append("<tr>");
            html.Append("<td class='repeater-header-left' style='width:200px'>" + currency + "</td>");
            html.Append("<td style='text-align:center;'>" + import + "</td>");
            html.Append("<td style='text-align:center;'>" + export + "</td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        return html.ToString();
    }
    private static string GetAlternateRefRate()
    {
        StringBuilder html = new StringBuilder();
        string query = "";
        query = "select * from tbl_currency";
        DataTable dttblcurrency = DbTable.ExecuteSelect(query);
        query = "select * from tbl_arrmaster";
        DataTable dttblarr = DbTable.ExecuteSelect(query);
        html.Append("<table class='repeater' border=1 style='width:100%;'>");
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
        return html.ToString();
    }
    private static string FormatData(object val)
    {
        if (GlobalUtilities.ConvertToDouble(val) == 0) return "-";
        string data = ExportExposurePortal.DecimalPoint(val, 4);
        return data;
    }
    private void GetCashConversion()
    {
        int currency = Common.GetQueryStringValue("c");
        bool isExport = Common.GetQueryStringBool("ie");
        double margin = GlobalUtilities.ConvertToDouble(Common.GetQueryString("margin"));
        string calctype = Common.GetQueryString("calctype");
        double spotrate = GetSpotRate(isExport, currency);
        double cashspot = GetCashSpotRate(isExport, currency);
        cashspot = GlobalUtilities.ConvertToDouble(ExportExposurePortal.DecimalPoint(cashspot, 4));
        //if (currency == 3 || currency == 4)//GBP/JPY
        //{
        //    cashspot = GlobalUtilities.ConvertToDouble(ExportExposurePortal.DecimalPoint(cashspot / 100.0, 4));
        //}
        if (calctype == "tomconversion")
        {
            cashspot = cashspot / 2;//tomspot
        }
        double cashrate = 0;
        if (isExport)
        {
            //cashrate = spotrate - cashspot / 100.0 - margin / 100.0;
            //cashrate = spotrate - cashspot - margin / 100.0;
            //As per conversation with Himesh Sir on 20-09-2024 
            cashrate = spotrate - cashspot / 100.0 - margin / 100.0;
        }
        else
        {
            //cashrate = spotrate - cashspot / 100.0 + margin / 100.0;
            //cashrate = spotrate - cashspot + margin / 100.0;
            cashrate = spotrate - cashspot / 100.0 + margin / 100.0;
        }
        StringBuilder html = new StringBuilder();
        if (calctype == "tomconversion")
        {
            html.Append("<table cellspacing=0 border='1' cellpadding=4 width='300'>" +
                            "<tr><td>Spot Rate</td><td class='broken-val'>" + spotrate + "</td></tr>" +
                            "<tr><td>Tom Spot</td><td class='broken-val'>" + cashspot + "</td></tr>" +
                            "<tr><td>Margin in Paisa</td><td class='broken-val'>" + margin + "</td></tr>" +
                            "<tr><td>Tom Rate</td><td class='broken-val'>" + cashrate + "</td></tr>" +
                        "</table>");
        }
        else
        {
            html.Append("<table cellspacing=0 border='1' cellpadding=4 width='300'>" +
                            "<tr><td>Spot Rate</td><td class='broken-val'>" + spotrate + "</td></tr>" +
                            "<tr><td>Cash Spot</td><td class='broken-val'>" + cashspot + "</td></tr>" +
                            "<tr><td>Margin in Paisa</td><td class='broken-val'>" + margin + "</td></tr>" +
                            "<tr><td>Cash Rate</td><td class='broken-val'>" + cashrate + "</td></tr>" +
                        "</table>");
        }
        HttpContext.Current.Response.Write(html.ToString());

        UpdateCurrencyMargin(currency, margin);
    }
    public static void UpdateCurrencyMargin(int currenyId, double margin)
    {
        string query = "";
        int clientUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
        query = "select * from tbl_usercurrencymargin where usercurrencymargin_currencymasterid=" + currenyId + " and usercurrencymargin_clientuserid=" + clientUserId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        Hashtable hstbl = new Hashtable();
        hstbl.Add("clientuserid", clientUserId);
        hstbl.Add("currencymasterid", currenyId);
        hstbl.Add("margin", margin);
        InsertUpdate obj = new InsertUpdate();
        if (dr == null)
        {
            obj.InsertData(hstbl, "tbl_usercurrencymargin");
        }
        else
        {
            obj.UpdateData(hstbl, "tbl_usercurrencymargin", GlobalUtilities.ConvertToInt(dr["usercurrencymargin_usercurrencymarginid"]));
        }
    }
    private void GetCurrencyMargin()
    {
        int currenyId = Common.GetQueryStringValue("cid");
        int clientUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
        string query = "select * from tbl_usercurrencymargin where usercurrencymargin_currencymasterid=" + currenyId + " and usercurrencymargin_clientuserid=" + clientUserId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        double margin = 0;
        string json = "";
        if (dr != null)
        {
            margin = GlobalUtilities.ConvertToDouble(dr["usercurrencymargin_margin"]);
        }
        HttpContext.Current.Response.Write("{\"margin\":\"" + margin + "\"}");
    }
    public static DataTable GetAvgRateTemplate()
    {
        DataTable dttbl = new DataTable();
        dttbl.Columns.Add("FromDate", typeof(DateTime)); dttbl.Columns.Add("ToDate", typeof(DateTime));
        dttbl.Columns.Add("Open"); dttbl.Columns.Add("High"); dttbl.Columns.Add("Low"); dttbl.Columns.Add("Close"); dttbl.Columns.Add("AvgClose");
        return dttbl;
    }
    public static DataTable CalculateAvgRatesByDateRange(DataTable dttbl, DataTable dtdate)
    {
        return CalculateAvgRatesByDateRange(dttbl, dtdate, false);
    }
    public static DataTable CalculateAvgRatesByDateRange(DataTable dttbl, DataTable dtdate, bool isDescOrderData)
    {
        if (!GlobalUtilities.IsValidaTable(dttbl)) return dtdate;
        for (int i = 0; i < dtdate.Rows.Count; i++)
        {
            DateTime dtstart = Convert.ToDateTime(dtdate.Rows[i]["FromDate"]);
            DateTime dtend = Convert.ToDateTime(dtdate.Rows[i]["ToDate"]);
            double totalclose = 0;
            bool isfirst = true;
            double openRate = 0;
            double closeRate = 0;
            int daysCount = 0;
            double highRate = 0;
            double lowRate = int.MaxValue;
            double avgClose = 0;
            for (int j = 0; j < dttbl.Rows.Count; j++)
            {
                DateTime dt = Convert.ToDateTime(dttbl.Rows[j]["historicaldata_date"]);
                TimeSpan ts1 = dtstart - dt;
                TimeSpan ts2 = dtend - dt;
                if(dt > dtstart && dt < dtend)
                {
                    double close = GlobalUtilities.ConvertToDouble(dttbl.Rows[j]["historicaldata_close"]);
                    double high = GlobalUtilities.ConvertToDouble(dttbl.Rows[j]["historicaldata_high"]);
                    double low = GlobalUtilities.ConvertToDouble(dttbl.Rows[j]["historicaldata_low"]);
                    double open = GlobalUtilities.ConvertToDouble(dttbl.Rows[j]["historicaldata_open"]);
                    if (isfirst)
                    {
                        if (isDescOrderData)
                        {
                            closeRate = close;
                        }
                        else
                        {
                            openRate = open;
                        }
                    }
                    if (high > highRate) highRate = high;
                    if (low < lowRate) lowRate = low;
                    if (isDescOrderData)
                    {
                        openRate = open;
                    }
                    else
                    {
                        closeRate = close;
                    }
                    totalclose += close;
                    isfirst = false;
                    daysCount++;
                }
            }
            if (daysCount > 0)
            {
                avgClose = totalclose / daysCount;
                dtdate.Rows[i]["open"] = openRate;
                dtdate.Rows[i]["close"] = closeRate;
                dtdate.Rows[i]["high"] = highRate;
                dtdate.Rows[i]["low"] = lowRate;
                dtdate.Rows[i]["avgclose"] = avgClose;
            }
        }
        return dtdate;
    }
    public string GetLiverateUserConfig(int clientUserId, Enum_AppType appType, int currencyTypeId)
    {
        StringBuilder html = new StringBuilder();
        string query = "";
        int maxSelecttion = 0;
        DataTable dttbluserconfig = GetUserConfigLiverate(appType, currencyTypeId, clientUserId);
        if (appType == Enum_AppType.Finstation)
        {
            if (currencyTypeId == 1)
            {
                maxSelecttion = 11;
            }
            else if (currencyTypeId == 2)
            {
                maxSelecttion = 11;
            }
            else if (currencyTypeId == 3)
            {
                maxSelecttion = 3;
            }
        }
        else if (appType == Enum_AppType.Finwatch)
        {
            maxSelecttion = 20;
        }
        else if (appType == Enum_AppType.FinIcon)
        {
            maxSelecttion = 20;
        }
        query = "select * from tbl_currencytype";
        if (currencyTypeId > 0)
        {
            query += " where currencytype_currencytypeid=" + currencyTypeId;
        }
        else
        {
            if (appType == Enum_AppType.FinIcon)
            {
                query += " where currencytype_currencytypeid<>3";
            }
        }
        DataTable dttblcurrencyType = DbTable.ExecuteSelect(query);
        query = "select * from tbl_currencymaster";
        if (currencyTypeId > 0)
        {
            if (currencyTypeId == 3)
            {
                query += " where currencymaster_currencymasterid in(1,2,3,5,6)";
            }
            else
            {
                query += " where currencymaster_currencytypeid=" + currencyTypeId;
            }
        }
        else
        {
            if (appType == Enum_AppType.FinIcon)
            {
                query += " where currencymaster_currencytypeid<>3";
            }
        }
        query += " order by currencymaster_currency";
        DataTable dttblcurrency = DbTable.ExecuteSelect(query);
        
        html.Append("<table width='100%'>");
        html.Append("<tr><td class='jq-expand-collapse-panel'>");
        for (int i = 0; i < dttblcurrencyType.Rows.Count; i++)
        {
            string currencyType = GlobalUtilities.ConvertToString(dttblcurrencyType.Rows[i]["currencytype_type"]);
            int ctypeId = GlobalUtilities.ConvertToInt(dttblcurrencyType.Rows[i]["currencytype_currencytypeid"]);
            string selectedCurrencies = "";
            for (int j = 0; j < dttbluserconfig.Rows.Count; j++)
            {
                if (GlobalUtilities.ConvertToInt(dttbluserconfig.Rows[j]["liverateuserconfig_currencytypeid"]) == ctypeId)
                {
                    selectedCurrencies = GlobalUtilities.ConvertToString(dttbluserconfig.Rows[j]["liverateuserconfig_currencies"]);
                    break;
                }
            }
            if (selectedCurrencies == "")
            {
                if (appType == Enum_AppType.Finstation)
                {
                    selectedCurrencies = GetDefaultCurrencies(ctypeId, appType);
                }
            }
            Array arrselectedCurrency = null;
            if(selectedCurrencies!="") arrselectedCurrency = selectedCurrencies.Split(',');
            string arrowcss = "expance-collapse-col";
            string datacss = "";
            if (currencyTypeId == 0 && i == 0)
            {
                if(appType == Enum_AppType.FinIcon) arrowcss = "expance-collapse-exp";
            }
            if (i > 0)
            {
                if (appType == Enum_AppType.FinIcon)
                {
                    datacss = "hidden";
                }
            }
            html.Append("<div class='jq-expance-collapse " + arrowcss + "' ctid='" + ctypeId + "' maxcurrency='" + maxSelecttion + "'>" + currencyType + "</div>");
            html.Append("<div class='expance-collapse-data " + datacss + "'>");
            html.Append("<table width='100%'>");
            for (int j = 0; j < dttblcurrency.Rows.Count; j++)
            {
                int ctypeId2 = GlobalUtilities.ConvertToInt(dttblcurrency.Rows[j]["currencymaster_currencytypeid"]);
                int currencyId = GlobalUtilities.ConvertToInt(dttblcurrency.Rows[j]["currencymaster_currencymasterid"]);
                if (ctypeId == 3)
                {
                    if (currencyId == 1 || currencyId == 2 || currencyId == 3 || currencyId == 5 || currencyId == 6)
                    {
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (ctypeId != ctypeId2) continue;
                }
                //if (ctypeId2 != ctypeId) continue;
                string currency = GlobalUtilities.ConvertToString(dttblcurrency.Rows[j]["currencymaster_currency"]);
                
                string strchecked = "";
                if (arrselectedCurrency != null)
                {
                    for (int k = 0; k < arrselectedCurrency.Length; k++)
                    {
                        if (currencyId == Convert.ToInt32(arrselectedCurrency.GetValue(k)))
                        {
                            strchecked = " checked='checked'";
                            break;
                        }
                    }
                }
                html.Append("<tr><td><input type='checkbox' class='jq-chk-userconfig-currency' id='currencyconfig-" + currencyId + "'" + strchecked + " cid='" + currencyId + "'/><label for='currencyconfig-" + currencyId + "'>" + currency + "</span></td></tr>");
            }
            html.Append("</table>");
            html.Append("</div>");
        }
        html.Append("</td></tr>");
        html.Append("</table>");
        return html.ToString();
    }
    public string GetLiverateUserConfig_Columns(int clientUserId, Enum_AppType appType)
    {
        StringBuilder html = new StringBuilder();
        string query = "";
        int maxSelecttion = 0;
        DataTable dttbluserconfig = GetUserConfigLiverate(appType, 0, clientUserId);
        int mincolumns = 3;
        bool isColumnConfig = false;
        string selectedColumns = "";
        if (dttbluserconfig.Rows.Count > 0) selectedColumns = GlobalUtilities.ConvertToString(dttbluserconfig.Rows[0]["liverateuserconfig_columns"]);
        Array arrselectedCurrency = null;
        if (selectedColumns == "")
        {
            if (appType == Enum_AppType.Finwatch)
            {
                selectedColumns = GetDefaultCurrencyColumns(appType);
            }
        }
        Array arrselectedCols = null;
        ArrayList arrselectedColumns = new ArrayList();
        if (selectedColumns != "")
        {
            arrselectedCols = selectedColumns.Split(',');
            for (int k = 0; k < arrselectedCols.Length; k++)
            {
                arrselectedColumns.Add(arrselectedCols.GetValue(k));
            }
        }
        html.Append("<div class='jq-currency-cols' totcolumns='" + mincolumns + "'>");
        html.Append("<table>");
        html.Append("<tr><td colspan='10'>Columns : </td></tr>");
        html.Append("<tr>");
        html.Append("<td><input type='checkbox' id='currencyconfig-col-1' col='1' class='jq-chk-userconfig-currency-cols'" + (arrselectedColumns.Contains("1") ? " checked" : "") + "/><label for='currencyconfig-col-1'>Bid</label></td>");
        html.Append("<td><input type='checkbox' id='currencyconfig-col-2' col='2' class='jq-chk-userconfig-currency-cols'" + (arrselectedColumns.Contains("2") ? " checked" : "") + "/><label for='currencyconfig-col-2'>Ask</label></td>");
        html.Append("<td><input type='checkbox' id='currencyconfig-col-3' col='3' class='jq-chk-userconfig-currency-cols'" + (arrselectedColumns.Contains("3") ? " checked" : "") + "/><label for='currencyconfig-col-3'>% Chg</label></td>");
        html.Append("<td><input type='checkbox' id='currencyconfig-col-4' col='4' class='jq-chk-userconfig-currency-cols'" + (arrselectedColumns.Contains("4") ? " checked" : "") + "/><label for='currencyconfig-col-4'>Open</label></td>");
        html.Append("<td><input type='checkbox' id='currencyconfig-col-5' col='5' class='jq-chk-userconfig-currency-cols'" + (arrselectedColumns.Contains("5") ? " checked" : "") + "/><label for='currencyconfig-col-5'>High</label></td>");
        html.Append("<td><input type='checkbox' id='currencyconfig-col-6' col='6' class='jq-chk-userconfig-currency-cols'" + (arrselectedColumns.Contains("6") ? " checked" : "") + "/><label for='currencyconfig-col-6'>Low</label></td>");
        html.Append("</tr>");
        html.Append("</table>");
        html.Append("</div>");

        return html.ToString();
    }
    public string GetDefaultCurrencyColumns(Enum_AppType appType)
    {
        string columns = "1,5,6";//Bid,Ask,%Chg,Open,High,Low
        if (appType == Enum_AppType.Finwatch)
        {
        }
        return columns;
    }
    public string GetDefaultCurrencies(int currencyTypeId, Enum_AppType appType)
    {
        string currencies = "";
        if (appType == Enum_AppType.Finwatch)
        {
            if (currencyTypeId == 1)
            {
                currencies = "1,2,3";
            }
            else if (currencyTypeId == 2)
            {
                currencies = "83,84";
            }
        }
        else
        {
            if (currencyTypeId == 1)
            {
                currencies = "1,2,3,4,5,6,7,8,9";
            }
            else if (currencyTypeId == 2)
            {
                currencies = "83,84,85,86,87,90,91,92";
            }
            else if (currencyTypeId == 3)
            {
                currencies = "1,5,6";
            }
        }
        return currencies;
    }
    public void SaveLiverateUserConfig(int clientUserId, int appTypeId, string currenciesWithType)
    {
        SaveLiverateUserConfig(clientUserId, appTypeId, currenciesWithType, "");
    }
    public void SaveLiverateUserConfig(int clientUserId, int appTypeId, string currenciesWithType, string columns)
    {
        Array arr = currenciesWithType.Split('~');
        Array arrcurrencyType = arr.GetValue(0).ToString().Split(',');
        Array arrcurrency = arr.GetValue(1).ToString().Split('|');
        for (int i = 0; i < arrcurrencyType.Length; i++)
        {
            int currencyTypeId = Convert.ToInt32(arrcurrencyType.GetValue(i));
            string currencies = arrcurrency.GetValue(i).ToString();
            SaveLiverateUserConfig(clientUserId, appTypeId, currencyTypeId, currencies, columns);
        }
    }
    public static int SaveCurrency(string currency, Enum_CurrencyType currencytype)
    {
        string query = "";
        int currencytypeId = (int)currencytype;
        query = "select * from tbl_currencymaster where currencymaster_currency='" + currency + "'";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null) return GlobalUtilities.ConvertToInt(dr["currencymaster_currencymasterid"]);
        Hashtable hstbl = new Hashtable();
        hstbl.Add("currency", currency);
        hstbl.Add("currencytypeid", currencytypeId);
        InsertUpdate obj = new InsertUpdate();
        int id = obj.InsertData(hstbl, "tbl_currencymaster");
        return id;
    }
    public static void UpdateLiverateCurrency(int liverateId, int currencyId)
    {
        string query = "";
        query = "update tbl_liverate set liverate_currencymasterid=" + currencyId + " where liverate_liverateid=" + liverateId;
        DbTable.ExecuteQuery(query);
    }
    public void SaveLiverateUserConfig(int clientUserId, int appTypeId, int currencyTypeId, string currencies)
    {
        SaveLiverateUserConfig(clientUserId, appTypeId, currencyTypeId, currencies, "");
    }
    public void SaveLiverateUserConfig(int clientUserId, int appTypeId, int currencyTypeId, string currencies, string columns)
    {
        string query = "";
        if (currencies == "")
        {
            query = @"delete from tbl_liverateuserconfig where liverateuserconfig_clientuserid=" + clientUserId +
                        "and liverateuserconfig_applicationtypeid=" + appTypeId + " and liverateuserconfig_currencytypeid=" + currencyTypeId;
            DbTable.ExecuteQuery(query);
            return;
        }
        query = "select * from tbl_currencymaster where currencymaster_currencymasterid in(" + currencies + ")";
        DataTable dttblcurrency = DbTable.ExecuteSelect(query);
        //StringBuilder rows = new StringBuilder();
        //for (int i = 0; i < dttblcurrency.Rows.Count; i++)
        //{
        //    int liverateRowId = GlobalUtilities.ConvertToInt(dttblcurrency.Rows[i]["currencymaster_liveraterow"]);
        //    if (i == 0)
        //    {
        //        rows.Append(liverateRowId.ToString());
        //    }
        //    else
        //    {
        //        rows.Append("," + liverateRowId.ToString());
        //    }
        //}

        Hashtable hstbl = new Hashtable();
        hstbl.Add("currencies", currencies);
        hstbl.Add("applicationtypeid", appTypeId);
        hstbl.Add("currencytypeid", currencyTypeId);
        hstbl.Add("clientuserid", clientUserId);
        hstbl.Add("columns", columns);
        //hstbl.Add("rows", rows.ToString());
        InsertUpdate obj = new InsertUpdate();
        
        query = @"select * from tbl_liverateuserconfig where liverateuserconfig_clientuserid=" + clientUserId +
                        "and liverateuserconfig_applicationtypeid=" + appTypeId + " and liverateuserconfig_currencytypeid=" + currencyTypeId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            obj.InsertData(hstbl, "tbl_liverateuserconfig");
        }
        else
        {
            obj.UpdateData(hstbl, "tbl_liverateuserconfig", GlobalUtilities.ConvertToInt(dr["liverateuserconfig_liverateuserconfigid"]));
        }
    }
    public static int GetCurrencyTypeBySectionId(int sectionId)
    {
        int currencyTypeId = 0;
        if (sectionId == 1)
        {
            currencyTypeId = (int)Enum_CurrencyType.INRCurrencies;
        }
        else if (sectionId == 2 || sectionId == 3)
        {
            currencyTypeId = (int)Enum_CurrencyType.CrossCurrencies;
        }
        else if (sectionId == 4 || sectionId == 59 || sectionId == 60)
        {
            currencyTypeId = (int)Enum_CurrencyType.IndicesAndCommodities;
        }
        else if (sectionId == 61)
        {
            currencyTypeId = (int)Enum_CurrencyType.GlobalIndicesFutures;
        }
        else if (sectionId == 50)
        {
            currencyTypeId = (int)Enum_CurrencyType.OffshoreCurrencies;
        }
        else if (sectionId == 62)
        {
            currencyTypeId = (int)Enum_CurrencyType.GovernmentBondYield;
        }
        return currencyTypeId;
    }
    public string GetUserConfigLiverateCurrencies(Enum_AppType appType, int currencyTypeId, int clientUserId)
    {
        DataTable dttbluserconfig = GetUserConfigLiverate(appType, currencyTypeId, clientUserId);
        if (dttbluserconfig.Rows.Count == 0) return GetDefaultCurrencies(currencyTypeId, appType);
        string currencies = GlobalUtilities.ConvertToString(dttbluserconfig.Rows[0]["liverateuserconfig_currencies"]);
        return currencies;
    }
    public string GetUserConfigLiverateCurrenciesOnly(Enum_AppType appType, int currencyTypeId, int clientUserId)
    {
        DataTable dttbluserconfig = GetUserConfigLiverate(appType, currencyTypeId, clientUserId);
        if (dttbluserconfig.Rows.Count == 0) return "";
        string currencies = GlobalUtilities.ConvertToString(dttbluserconfig.Rows[0]["liverateuserconfig_currencies"]);
        return currencies;
    }
    public string GetUserConfigLiverateCurrencyColumns(Enum_AppType appType, int clientUserId)
    {
        DataTable dttbluserconfig = GetUserConfigLiverate(appType, 0, clientUserId);
        if (dttbluserconfig.Rows.Count == 0) return GetDefaultCurrencyColumns(appType);
        string columns = GlobalUtilities.ConvertToString(dttbluserconfig.Rows[0]["liverateuserconfig_columns"]);
        if (columns == "") return GetDefaultCurrencyColumns(appType);
        return columns;
    }
    public DataTable GetUserConfigLiverate(Enum_AppType appType, int currencyTypeId, int clientUserId)
    {
        string query = "";
        query = "select * from tbl_liverateuserconfig where liverateuserconfig_applicationtypeid=" + (int)appType + " and liverateuserconfig_clientuserid=" + clientUserId;
        if (currencyTypeId > 0)
        {
            query += " and liverateuserconfig_currencytypeid=" + currencyTypeId;
        }
        //ErrorLog.WriteLog("GetUserConfigLiverate:" + query);
        DataTable dttbluserconfig = DbTable.ExecuteSelect(query);
        return dttbluserconfig;
    }
    private void GetScrollNews()
    {
        string query = @"select * from tbl_setting where setting_settingname='Scrolling News'
                         and DATEDIFF(minute,setting_modifieddate,getdate()) <= 3";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            HttpContext.Current.Response.Write(dr["setting_settingvalue"]);
        }
    }
    public static int ClientUserIdBySessionId
    {
        get
        {
            DataRow drclientuser = ClientUserBySessionId();
            if (drclientuser == null) return 0;
            return GlobalUtilities.ConvertToInt(drclientuser["clientuser_clientuserid"]);
        }
    }
    public static int ClientIdBySessionId
    {
        get
        {
            DataRow drclientuser = ClientUserBySessionId();
            if (drclientuser == null) return 0;
            return GlobalUtilities.ConvertToInt(drclientuser["clientuser_clientid"]);
        }
    }
    public static DataRow ClientUserBySessionId()
    {
        string sessionId = Common.GetQueryString("sid");
        string query = "select * from tbl_clientuser WHERE clientuser_isactive=1 and clientuser_exesessionid=@sessionId";
        Hashtable hstblp = new Hashtable();
        hstblp.Add("sessionid", sessionId);
        DataRow drclientuser = DbTable.ExecuteSelectRow(query, hstblp);
        return drclientuser;
    }
    private static string GetLiveRateByRowColText(string section, int row, int col)
    {
        string query = @"select liverate_currentrate,liverate_calculation from tbl_liverate 
                         join tbl_liveratesection on liveratesection_liveratesectionid=liverate_liveratesectionid
                         where 
                         replace(liveratesection_name,' ','')='" + section + @"'
                         and liverate_row=" + row + " and liverate_column=" + col;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return "";
        string rate = GlobalUtilities.ConvertToString(dr["liverate_currentrate"]);
        string calc = GlobalUtilities.ConvertToString(dr["liverate_calculation"]);
        if (calc == "") return rate;
        return GetLiveRateCalc(GlobalUtilities.ConvertToDouble(rate), calc).ToString();
    }
    public static double GetLiveRateCalc(int liveRateId)
    {
        string query = "";
        query = "select liverate_currentrate,liverate_calculation from tbl_liverate where liverate_liverateid=" + liveRateId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        string calc = GlobalUtilities.ConvertToString(dr["liverate_calculation"]);
        double currentRate = GlobalUtilities.ConvertToDouble(dr["liverate_currentrate"]);
        return GetLiveRateCalc(currentRate, calc);
    }
    public static double GetLiveRateCalc(double rate, string calc)
    {
        if (calc == "self/100")
        {
            return rate / 100.0;
        }
        else if (calc == "self/1000")
        {
            return rate / 1000.0;
        }
        if (!calc.Contains("#")) return rate;
        bool isexists = true;
        int exitcounter = 0;
        while (isexists)
        {
            if (exitcounter > 10)
            {
                isexists = false;
                break;
            }
            exitcounter++;
            if (calc.IndexOf("#") >= 0)
            {
                int startIndex = calc.IndexOf("#");
                int endIndex = calc.IndexOf("]");
                string c = calc.Substring(startIndex + 1, endIndex - startIndex);
                Array arr1 = c.Split('[');
                string sectionCode = arr1.GetValue(0).ToString();
                string temp = arr1.GetValue(1).ToString().Replace("]", "");
                Array arr2 = temp.Split(',');
                int row = Convert.ToInt32(arr2.GetValue(0));
                int col = Convert.ToInt32(arr2.GetValue(1));
                string d = GetLiveRateByRowColText(sectionCode, row, col);
                calc = calc.Replace("#" + c, d);
            }
            else
            {
                isexists = false;
            }
        }
        isexists = true;
        exitcounter = 0;
        while (isexists)
        {
            if (exitcounter > 10) break;
            exitcounter++;
            if (calc.IndexOf("DATEDIFF") >= 0)
            {
                int datestart = calc.IndexOf("DATEDIFF");
                int dateend = calc.IndexOf(")");
                string dates = calc.Substring(datestart + 9, dateend - datestart - 9).Replace("(", "").Trim();
                Array arrdates = dates.Split('-');
                DateTime date1 = Convert.ToDateTime(arrdates.GetValue(1).ToString() + "-" + arrdates.GetValue(0).ToString() + "-" + arrdates.GetValue(2).ToString());
                DateTime date2 = Convert.ToDateTime(arrdates.GetValue(4).ToString() + "-" + arrdates.GetValue(3).ToString() + "-" + arrdates.GetValue(5).ToString());
                TimeSpan ts = date1 - date2;
                int diffDays = ts.Days;
                string datesformula = calc.Substring(datestart, dateend - datestart + 1);
                calc = calc.Replace(datesformula, diffDays.ToString());
            }
            else
            {
                isexists = false;
            }
        }
        double finalRate = GlobalUtilities.Evaluate(calc);
        return finalRate;
    }

    private string GetAnnualisedPremiumMonthlyValues(Enum_Currency curreny, bool isExport)
    {
        double premiumRateMonth1 = 0;
        double premiumRateMonth3 = 0;
        double premiumRateMonth6 = 0;
        double premiumRateMonth12 = 0;
        int annualisedPremiumMonthwiseLiveRateId = 0;
        
        if (curreny == Enum_Currency.USD)
        {
            //1M
            if (isExport)
            {
                annualisedPremiumMonthwiseLiveRateId = 1015;
            }
            else
            {
                annualisedPremiumMonthwiseLiveRateId = 1056;
            }
        }
        else if (curreny == Enum_Currency.EUR)
        {
            //1M
            if (isExport)
            {
                annualisedPremiumMonthwiseLiveRateId = 370;
            }
            else
            {
                annualisedPremiumMonthwiseLiveRateId = 384;
            }
            
        }
        else if (curreny == Enum_Currency.GBP)
        {
            //1M
            if (isExport)
            {
                annualisedPremiumMonthwiseLiveRateId = 475;
            }
            else
            {
                annualisedPremiumMonthwiseLiveRateId = 488;
            }

        }
        else if (curreny == Enum_Currency.JPY)
        {
            //1M
            if (isExport)
            {
                annualisedPremiumMonthwiseLiveRateId = 579;
            }
            else
            {
                annualisedPremiumMonthwiseLiveRateId = 592;
            }

        }
        premiumRateMonth1 = GetLiveRateCalc(annualisedPremiumMonthwiseLiveRateId);
        premiumRateMonth1 = GlobalUtilities.ConvertToDouble(ExportExposurePortal.DecimalPoint(premiumRateMonth1, 4));
        //3M
        annualisedPremiumMonthwiseLiveRateId += 2;
        premiumRateMonth3 = GetLiveRateCalc(annualisedPremiumMonthwiseLiveRateId);
        premiumRateMonth3 = GlobalUtilities.ConvertToDouble(ExportExposurePortal.DecimalPoint(premiumRateMonth3, 4));
        //6M
        annualisedPremiumMonthwiseLiveRateId += 3;
        premiumRateMonth6 = GetLiveRateCalc(annualisedPremiumMonthwiseLiveRateId);
        premiumRateMonth6 = GlobalUtilities.ConvertToDouble(ExportExposurePortal.DecimalPoint(premiumRateMonth6, 4));
        //12M
        annualisedPremiumMonthwiseLiveRateId += 6;
        if (annualisedPremiumMonthwiseLiveRateId == 381) annualisedPremiumMonthwiseLiveRateId = 382;
        premiumRateMonth12 = GetLiveRateCalc(annualisedPremiumMonthwiseLiveRateId);
        premiumRateMonth12 = GlobalUtilities.ConvertToDouble(ExportExposurePortal.DecimalPoint(premiumRateMonth12, 4));

        return premiumRateMonth1 + "~" + premiumRateMonth3 + "~" + premiumRateMonth6 + "~" + premiumRateMonth12;
        
    }
    private double GetAnnualisedPremiumValue(int spotDateLiveRateId, int month1DateLiveRateId, int premiumPaisaLiveRateId,
        int spotRateLiveRateId)
    {
        double spotRate = GetLiveRate(spotRateLiveRateId);
        double premiumPaisa = GetLiveRate(premiumPaisaLiveRateId);
        double annualisedPermium = 0;
        string spotDate = GetLiveRateText(spotDateLiveRateId);
        string monthDate = GetLiveRateText(month1DateLiveRateId);
        TimeSpan ts = Convert.ToDateTime(monthDate) - Convert.ToDateTime(spotRate);
        int daysDiff = ts.Days;
        if (daysDiff > 0)
        {
            annualisedPermium = premiumPaisa * (365 / daysDiff) / spotRate;
        }
        return annualisedPermium;
    }
}
