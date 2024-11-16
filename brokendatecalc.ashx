<%@ WebHandler Language="C#" Class="brokendatecalc" %>

using System;
using System.Web;
using WebComponent;
using System.Text;
using System.Data;
using System.Collections;

public class brokendatecalc : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string source = context.Request.QueryString["source"];
        if (source == "phonegap")
        {
            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
        }
        if (context.Request.QueryString["sdate"] == "") return;
        if (context.Request.QueryString["bdate"] == "") return;
        DateTime spotDate = Convert.ToDateTime(context.Request.QueryString["sdate"]);
        string brokenDate = context.Request.QueryString["bdate"];
        bool isExport = GlobalUtilities.ConvertToBool(context.Request.QueryString["ie"]);
        int currency = GlobalUtilities.ConvertToInt(context.Request.QueryString["c"]);
        double margin = GlobalUtilities.ConvertToDouble(context.Request.QueryString["margin"]);
        int calctype = GlobalUtilities.ConvertToInt(context.Request.QueryString["calctype"]);
        int endDateRateId_Start = 0;
        int endDateRateId_End = 0;
        int rateId_Start = 0;
        int rateId_End = 0;
        double dblDivisionFactor = 0;
        double dblForwardRateDivFactor = 1;
        bool isMonthEndBrokenDateCalc = false;
        Array arrbrokenDates = brokenDate.Split(',');
        ArrayList arrbrokenDate = new ArrayList();
        ArrayList arrdays = new ArrayList();
        ArrayList arrspotRate = new ArrayList();
        ArrayList arrmargin = new ArrayList();
        ArrayList arrdblPremium_PrevMonth = new ArrayList();
        ArrayList arroutRate = new ArrayList();
        int future_endDateRateId_Start = 0;
        int future_rateId_Start = 0;
        for (int d = 0; d < arrbrokenDates.Length; d++)
        {
            brokenDate = arrbrokenDates.GetValue(d).ToString();
            DateTime dtBrokenDate = GlobalUtilities.ConvertToDateFromTextBox(brokenDate);

            //validate the date first
            if (!IsValidDate(brokenDate, dtBrokenDate, currency))
            {
                return;
            }

            if (currency == 1)//USDINR
            {
                endDateRateId_Start = 194;
                future_endDateRateId_Start = 2703;
                if (isExport)
                {
                    rateId_Start = 207;
                    future_rateId_Start = 2751;
                }
                else
                {
                    rateId_Start = 220;
                    future_rateId_Start = 2799;
                }
                dblDivisionFactor = 100;
                isMonthEndBrokenDateCalc = true;
            }
            else if (currency == 2)//EURINR
            {
                endDateRateId_Start = 1252;
                future_endDateRateId_Start = 3087;
                if (isExport)
                {
                    rateId_Start = 1265;
                    future_rateId_Start = 3135;
                }
                else
                {
                    rateId_Start = 1278;
                    future_rateId_Start = 3135;
                }
                dblDivisionFactor = 100;
                isMonthEndBrokenDateCalc = true;
            }
            else if (currency == 3)//GBPINR
            {
                endDateRateId_Start = 1356;
                future_endDateRateId_Start = 3471;
                if (isExport)
                {
                    rateId_Start = 1369;
                    future_rateId_Start = 3519;
                }
                else
                {
                    rateId_Start = 1382;
                    future_rateId_Start = 3567;
                }
                //dblDivisionFactor = 100;
                dblDivisionFactor = 10000;//changed from 100 to 10,000 as requested by Himesh on 2nd mar 21
                dblForwardRateDivFactor = 100;
                isMonthEndBrokenDateCalc = true;
            }
            else if (currency == 4)//JPYINR
            {
                endDateRateId_Start = 513;
                if (isExport)
                {
                    rateId_Start = 526;
                }
                else
                {
                    rateId_Start = 539;
                }
                dblDivisionFactor = 100;
                dblForwardRateDivFactor = 100;
            }
            else if (currency == 5)//EURUSD
            {
                endDateRateId_Start = 617;
                if (isExport)
                {
                    rateId_Start = 630;
                }
                else
                {
                    rateId_Start = 643;
                }
                dblDivisionFactor = 10000;
            }
            else if (currency == 6)//GBPUSD
            {
                endDateRateId_Start = 721;
                if (isExport)
                {
                    rateId_Start = 734;
                }
                else
                {
                    rateId_Start = 747;
                }
                dblDivisionFactor = 1000;
                dblForwardRateDivFactor = 10;
            }
            else if (currency == 7)//USDJPY
            {
                endDateRateId_Start = 825;
                if (isExport)
                {
                    rateId_Start = 838;
                }
                else
                {
                    rateId_Start = 851;
                }
                dblDivisionFactor = 0;
                dblForwardRateDivFactor = 100;
            }
            rateId_End = rateId_Start + 12;

            DataTable dttblMonthEndDate = GetLiveRatesBetween(endDateRateId_Start, endDateRateId_Start + 12, future_endDateRateId_Start,
                                            future_endDateRateId_Start + 47);// 12 * 4years = 48 rates

            //find forward date
            int brokenDateMonth = dtBrokenDate.Month;
            int brokenDateYear = dtBrokenDate.Year;
            int forwardDateIndex = -1;
            DateTime dtForwardDate_Current = DateTime.MinValue;
            DateTime dtForwardDate_PrevMonth = DateTime.MinValue;
            double dblPremium_PrevMonth = 0;
            bool isPremiumExists = false;
            if (isMonthEndBrokenDateCalc)
            {
                for (int i = 1; i < dttblMonthEndDate.Rows.Count; i++)
                {
                    try
                    {
                        DateTime dt = Convert.ToDateTime(dttblMonthEndDate.Rows[i]["liverate_currentrate"]);
                        TimeSpan sp1 = dt - dtBrokenDate;
                        if (sp1.Days == 0)
                        {
                            isPremiumExists = true;
                            forwardDateIndex = i;
                            dtForwardDate_Current = dt;
                            break;
                        }
                        else
                        {
                            if (dt.Year == brokenDateYear && dt.Month == brokenDateMonth)
                            {
                                forwardDateIndex = i;
                                dtForwardDate_PrevMonth = Convert.ToDateTime(dttblMonthEndDate.Rows[i - 1]["liverate_currentrate"]);
                                dtForwardDate_Current = Convert.ToDateTime(dttblMonthEndDate.Rows[i]["liverate_currentrate"]);
                                break;
                            }
                        }

                    }
                    catch
                    {
                    }
                }
                if (dtBrokenDate <= Convert.ToDateTime(dttblMonthEndDate.Rows[0]["liverate_currentrate"]) || forwardDateIndex == -1)
                {
                    ReturenError("Invalid Broken Date(" + brokenDate + ")!");
                    return;
                }
            }
            else
            {
                for (int i = 0; i < dttblMonthEndDate.Rows.Count; i++)
                {
                    try
                    {
                        DateTime dt = Convert.ToDateTime(dttblMonthEndDate.Rows[i]["liverate_currentrate"]);
                        TimeSpan sp1 = dt - dtBrokenDate;
                        if (sp1.Days >= 0)
                        {
                            forwardDateIndex = i;
                            dtForwardDate_Current = dt;
                            break;
                        }
                    }
                    catch
                    {
                    }
                }
                if (forwardDateIndex < 0)
                {
                    ReturenError("Invalid Broken Date(" + brokenDate + ")!");
                    return;
                }
            }
            //find liverate import/export value which is near to broken date
            DataTable dttblRates = GetLiveRatesBetween(rateId_Start, rateId_End, future_rateId_Start, future_rateId_Start + 47);
            double forwardRate = GlobalUtilities.ConvertToDouble(dttblRates.Rows[forwardDateIndex]["liverate_currentrate"]);
            double spotRate = Custom.GetSportRate(currency, isExport);

            forwardRate = forwardRate / dblForwardRateDivFactor;

            //find days = Forward Date - Spot Date
            TimeSpan sp = dtBrokenDate - spotDate;
            int days = sp.Days;

            //find premium = SpotRate * (SpotRate / (Broken Date - Spot Date))
            //first find the Broken Date - Spot Date
            sp = dtForwardDate_Current - spotDate;
            int brokenDateDiff = sp.Days;

            //per day
            double dblPerday = forwardRate / brokenDateDiff;

            dblPremium_PrevMonth = dblPerday * days;

            //new Premium calculation
            double dblPremium = dblPremium_PrevMonth;
            if (isMonthEndBrokenDateCalc)
            {
                TimeSpan tsMonthDiff = dtForwardDate_Current - dtForwardDate_PrevMonth;
                int daysBetweenMonths = tsMonthDiff.Days;
                double forwardRate_Current = GlobalUtilities.ConvertToDouble(dttblRates.Rows[forwardDateIndex]["liverate_currentrate"]);
                int forwardDateIndex_prev = 0;
                double forwardRate_Prev = 0;
                if (forwardDateIndex == 1)
                {
                    //consider spot rate
                    forwardRate_Prev = 0;// spotRate;
                }
                else
                {
                    forwardDateIndex_prev = forwardDateIndex - 1;
                    forwardRate_Prev = GlobalUtilities.ConvertToDouble(dttblRates.Rows[forwardDateIndex_prev]["liverate_currentrate"]);
                }
                double premiumMonths = forwardRate_Current - forwardRate_Prev;
                double premiumPerDay = premiumMonths / daysBetweenMonths;
                TimeSpan tsDayDiffSelectDate = dtBrokenDate - dtForwardDate_PrevMonth;
                int dayDiffSelectedDate = tsDayDiffSelectDate.Days;
                double premiumUptoSelectDate = premiumPerDay * dayDiffSelectedDate;
                double totalPremium = forwardRate_Prev + premiumUptoSelectDate;
                dblPremium = totalPremium;
            }
            //convert to paisa

            if (dblDivisionFactor > 0) dblPremium = dblPremium / dblDivisionFactor;

            //if selected date is equal to month end date then there is no calculation needed
            if (isPremiumExists)
            {
                dblPremium_PrevMonth = forwardRate;
            }
            dblPremium_PrevMonth = Convert.ToDouble(String.Format("{0:#.0000}", dblPremium));

            if (context.Request.QueryString["action"] == "premium")
            {
                if (context.Request.QueryString["bdate2"] != "")
                {
                    DateTime dtBrokenDate2 = GlobalUtilities.ConvertToDateFromTextBox(context.Request.QueryString["bdate2"]);
                    TimeSpan sp2 = dtBrokenDate2 - spotDate;
                    brokenDateDiff = sp2.Days;
                }
                context.Response.Write(dblPremium_PrevMonth.ToString() + "," + brokenDateDiff);
                context.Response.End();
                return;
            }
            //find outright rate
            double outRate = spotRate + dblPremium_PrevMonth;
            double marginDivided = 0;
            if (currency > 4)
            {
                marginDivided = margin / 10000.0;
            }
            else
            {
                marginDivided = margin / 100.0;
            }
            if (isExport)
            {
                outRate = outRate - marginDivided;
            }
            else
            {
                outRate = outRate + marginDivided;
            }
            outRate = Convert.ToDouble(String.Format("{0:#.0000}", outRate));
            
            arrbrokenDate.Add(brokenDate);
            arrdays.Add(days);
            arrspotRate.Add(spotRate);
            arrdblPremium_PrevMonth.Add(dblPremium_PrevMonth);
            arroutRate.Add(outRate);
            arrmargin.Add(margin);
        }
        StringBuilder html = new StringBuilder();
        string width = "";// "width='300px'";
        if (source == "phonegap")
        {
            width = "width='100%'";
        }
        string marginLabel = "Margin in Paisa";
        if (currency > 4) marginLabel = "Margin in pips";
        html.Append("<table " + width + " cellspacing=0 border='1' cellpadding=4>" +
                        "<tr><td>Broken Date</td>" + GetHorizontalBrokenVal(arrbrokenDate) + "</tr>" +
                        "<tr><td>Days</td>" + GetHorizontalBrokenVal(arrdays) + "</tr>" +
                        "<tr><td>Spot Rate</td>" + GetHorizontalBrokenVal(arrspotRate) + "</tr>" +
                        "<tr><td>Premium</td>" + GetHorizontalBrokenVal(arrdblPremium_PrevMonth) + "</tr>" +
                        "<tr><td>" + marginLabel + "</td>" + GetHorizontalBrokenVal(arrmargin) + "</tr>" +
                        "<tr><td>Outright Rate</td>" + GetHorizontalBrokenVal(arroutRate) + "</tr>" +
                        "</table>");
        context.Response.Write(html.ToString());
        Finstation.UpdateCurrencyMargin(currency, margin);
    }
    
    private string GetHorizontalBrokenVal(ArrayList arr)
    {
        StringBuilder vals = new StringBuilder();
        for (int i = 0; i < arr.Count; i++)
        {
            vals.Append("<td class='broken-val'>" + arr[i].ToString() + "</td>");
        }
        return vals.ToString();
    }
    private void ReturenError(string message)
    {
        HttpContext.Current.Response.Write("message : " + message);
    }
    private DataTable GetLiveRatesBetween(int start,int end, int start2, int end2)
    {
        string query = "select * from tbl_liverate where liverate_liverateid between " + start + " and " + end + " order by liverate_liverateid";
        if (start2 > 0)
        {
            query = "select * from tbl_liverate where liverate_liverateid between " + start + " and " + end + 
                " OR liverate_liverateid between " + start2 + " and " + end2 + 
                " order by liverate_liverateid";
        }
        DataTable dttbl = DbTable.ExecuteSelect(query);
        return dttbl;
    }
    private bool IsValidDate(string brokenDate,DateTime date, int currency)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        {
            ReturenError("Date(" + brokenDate + ") can not be Saturday/Sunday!");
            return false;
        }
        string strDate = date.Year + "-" + date.Month + "-" + date.Day;
        string currencyIds = "";
        if (currency == 1)//USDINR
        {
            currencyIds = "2,1";
        }
        else if (currency == 2)//EURINR
        {
            currencyIds = "3,1";
        }
        else if (currency == 3)//GBPINR
        {
            currencyIds = "4,1";
        }
        else if (currency == 4)//JPYINR
        {
            currencyIds = "5,1";
        }
        else if (currency == 5)//EURUSD
        {
            currencyIds = "3,2";
        }
        else if (currency == 6)//GBPUSD
        {
            currencyIds = "4,2";
        }
        else if (currency == 7)//USDJPY
        {
            currencyIds = "2,5";
        }
        string query = "select * from tbl_holiday where cast(holiday_date as date)=cast('" + strDate + "' as date) AND holiday_currencyid IN("+currencyIds+")";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if(dr!=null)
        {
            ReturenError("Given date(" + brokenDate + ") is a Holiday!");
            return false;
        }
        return true;
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}