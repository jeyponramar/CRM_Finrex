using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using WebComponent;
using System.Collections;

/// <summary>
/// Summary description for ExportExposurePortal
/// </summary>
public class ExportExposurePortal
{
    int clientId;
    int _year = 0;
    public ExportExposurePortal(int cid)
    {
        clientId = cid;
    }
    public string GetDashboardData(int portalType, int currency, bool isEmail, out DataTable dttbl)
    {
        return GetDashboardData(portalType, currency, isEmail, 0, out dttbl);
    }
    public string GetDashboardData(int portalType, int currency, bool isEmail, int year, out DataTable dttbl)
    {
        dttbl = new DataTable();
        if (portalType == 1)
        {
            return GetDashboardData_Export(currency, isEmail, year, out dttbl);
        }
        else if (portalType == 2)
        {
            return GetDashboardData_Import(currency, isEmail, out dttbl);
        }
        return "";
    }
    public string GetDashboardData_Export(int currency, bool isEmail, out DataTable dttbl)
    {
        return GetDashboardData_Export(currency, isEmail, 0, out dttbl);
    }
    public string GetDashboardData_Export(int currency, bool isEmail, int year, out DataTable dttbl)
    {
        _year = year;
        dttbl = new DataTable();
        StringBuilder html = new StringBuilder();
        dttbl.Columns.Add("Label");

        for (int i = 1; i <= 12; i++)
        {
            dttbl.Columns.Add("Month" + i);
        }
        dttbl.Columns.Add("Total");
        dttbl.Columns.Add("IsVisible");
        dttbl.Columns.Add("DecimalPoint");

        DataRow dr = dttbl.NewRow();
        dr["IsVisible"] = true;
        dr["Label"] = "Export";
        DateTime dtcurrent = DateTime.Now;
        dtcurrent = dtcurrent.AddYears(year);
        int m = dtcurrent.Month;
        int y = dtcurrent.Year;
        DateTime date = new DateTime(y, m, GlobalUtilities.GetMonthEndDay(m, y));
        for (int i = 0; i < 12; i++)
        {
            DateTime dt = date.AddMonths(i);
            string strdate = string.Format("{0:MMM-yy}", dt);
            dr["Month" + (i + 1)] = strdate;
        }
        dr["Total"] = "Total";
        dttbl.Rows.Add(dr);

        AddExportOrderReceivedRow(dttbl, currency, true, 2);
        AddPCFCOSRow(dttbl, currency, true, 2);
        AddForwardsAlreadyBookedRow(dttbl, currency, true, 2);
        AddUnhedgedPositionRow(dttbl, true, 2);
        AddHedgedRatioRow(dttbl, true, 2);
        AddAverageCostingRateRow(dttbl, currency, true, 4);
        AddAveragePCFCRateRow(dttbl, currency, true, 4);
        AddAverageForwardBookedRateRow(dttbl, currency, true, 4);
        AddCurrentForwardRateRow(dttbl, currency, true, 4, year);
        AddEffectiveRateExportPositionRow(dttbl, true, 2);
        AddProfitLossperFCunitRow(dttbl, false, 2);
        AddPLonExportPortfolioRow(dttbl, false, 2);
        AddMtmWithAverageCostingRow(dttbl, false, 2);
        AddMtmOfForwardRow(dttbl, false, 2);
        AddPLonUnhedgedExposureRow(dttbl, false, 2);
        AddPLonHedgedexposureRow(dttbl, false, 2);
        AddPLFromCosting(dttbl, true, 2);
        AddPLonHedgedExposure(dttbl, true, 2);
        AddPLonUnHedgedExposure(dttbl, true, 2);
        CalculateTotal(dttbl, true);

        if (isEmail)
        {
            html.Append("<table width='100%' cellspacing=0 cellpadding=5  style='border-collapse: collapse;border:1px solid #000;' border='1'>");
        }
        else
        {
            html.Append("<table width='100%' cellspacing=0 cellpadding=5 class='da-grid'>");
        }
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (GlobalUtilities.ConvertToBool(dttbl.Rows[i]["IsVisible"]) == false) continue;
            if (i == 0)
            {
                if (isEmail)
                {
                    html.Append("<tr style='font-weight:bold;'>");
                }
                else
                {
                    html.Append("<tr class='da-grid-header'>");
                }
            }
            else
            {
                html.Append("<tr class='da-grid-row'>");
            }
            for (int j = 0; j < dttbl.Columns.Count - 2; j++)
            {
                string data = GlobalUtilities.ConvertToString(dttbl.Rows[i][j]);
                if (j == 0)
                {
                    html.Append("<td row='" + i + "'>" + data + "</td>");
                }
                else
                {
                    html.Append("<td style='text-align:center;'>" + data + "</td>");
                }
            }
            html.Append("</tr>");
        }
        html.Append("</table>");

        return html.ToString();
    }
    private void GetStartEndDate(out string startDate, out string endDate)
    {
        DateTime dtcurrent = DateTime.Now.AddYears(_year);
        DateTime dtStart = new DateTime(dtcurrent.Year, dtcurrent.Month, 1);
        DateTime dtEnd = dtStart.AddMonths(12);
        string m = dtStart.Month.ToString();
        if (m.Length == 1) m = "0" + m;
        startDate = dtStart.Year + "-" + m + "-01";
        m = dtEnd.Month.ToString();
        if (m.Length == 1) m = "0" + m;
        string d = GlobalUtilities.GetMonthEndDay(dtEnd.Year, dtEnd.Month).ToString();
        if (d.Length == 1) d = "0" + d;
        endDate = dtEnd.Year + "-" + m + "-" + d;
    }
    private void AddExportOrderReceivedRow(DataTable dttbl, int currency, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Export Order Received";
        string startDate = "";
        string endDate = "";
        GetStartEndDate(out startDate, out endDate);
        string query = "select SUM(ISNULL(exportorder_netamount,0)) as netamount, MONTH(exportorder_expectedduedate) as month," +
                        "YEAR(exportorder_expectedduedate) as year from tbl_exportorder " +
                        "WHERE exportorder_expectedduedate BETWEEN '" + startDate + "' AND '" + endDate + "' " +
                        "AND exportorder_exposurecurrencymasterid=" + currency + " AND exportorder_clientid=" + clientId +
                        " GROUP BY MONTH(exportorder_expectedduedate),YEAR(exportorder_expectedduedate)";
        DataTable dttblData = DbTable.ExecuteSelect(query);
        SetMonthColumns(dr, dttblData);
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void SetMonthColumns(DataRow dr, DataTable dttblData)
    {
        SetMonthColumns(dr, dttblData, -1);
    }
    private void SetMonthColumns(DataRow dr, DataTable dttblData, int decimalplaces)
    {
        DateTime dtcurrent = DateTime.Now.AddYears(_year);
        DateTime startdate = new DateTime(dtcurrent.Year, dtcurrent.Month, 1);
        DateTime date = startdate;
        for (int i = 0; i < 12; i++)
        {
            date = startdate.AddMonths(i);
            int month = date.Month;
            int year = date.Year;
            string netamount = "0";
            for (int j = 0; j < dttblData.Rows.Count; j++)
            {
                int month_order = GlobalUtilities.ConvertToInt(dttblData.Rows[j]["month"]);
                int year_order = GlobalUtilities.ConvertToInt(dttblData.Rows[j]["year"]);
                if (month == month_order && year == year_order)
                {
                    if (decimalplaces > -1)
                    {
                        if (decimalplaces == 2)
                        {
                            netamount = String.Format("{0:0.00}", dttblData.Rows[j][0]);
                        }
                        else if (decimalplaces == 4)
                        {
                            netamount = String.Format("{0:0.0000}", dttblData.Rows[j][0]);
                        }
                        else
                        {
                            netamount = String.Format("{0:0.00}", dttblData.Rows[j][0]);
                        }
                    }
                    else
                    {
                        netamount = String.Format("{0:0.00}", dttblData.Rows[j][0]);
                    }
                    break;
                }
            }
            dr["Month" + (i + 1)] = netamount;
        }
    }
    private void AddPCFCOSRow(DataTable dttbl, int currency, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "PCFC Outstanding";

        string startDate = "";
        string endDate = "";
        GetStartEndDate(out startDate, out endDate);
        string query = "select SUM(ISNULL(pcfc_fcamountbalance,0)) as netamount, MONTH(pcfc_pcduedate) as month," +
                        "YEAR(pcfc_pcduedate) as year from tbl_pcfc " +
                        "WHERE pcfc_pcduedate BETWEEN '" + startDate + "' AND '" + endDate + "' " +
                        "AND pcfc_exposurecurrencymasterid=" + currency + " AND pcfc_clientid=" + clientId +
                        " GROUP BY MONTH(pcfc_pcduedate),YEAR(pcfc_pcduedate)";
        DataTable dttblData = DbTable.ExecuteSelect(query);
        SetMonthColumns(dr, dttblData);
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddForwardsAlreadyBookedRow(DataTable dttbl, int currency, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Forward Contract Outstanding";
        string startDate = "";
        string endDate = "";
        GetStartEndDate(out startDate, out endDate);
        string query = "select SUM(ISNULL(forwardcontract_balancesold,0)) as netamount, MONTH(forwardcontract_to) as month," +
                        "YEAR(forwardcontract_to) as year from tbl_forwardcontract " +
                        "WHERE forwardcontract_to BETWEEN '" + startDate + "' AND '" + endDate + "' " +
                        "AND forwardcontract_exposurecurrencymasterid=" + currency + " AND forwardcontract_clientid=" + clientId +
                        " GROUP BY MONTH(forwardcontract_to),YEAR(forwardcontract_to)";
        DataTable dttblData = DbTable.ExecuteSelect(query);
        SetMonthColumns(dr, dttblData);
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddUnhedgedPositionRow(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Unhedged Position";
        for (int i = 1; i < dttbl.Columns.Count - 3; i++)
        {
            double expOrder = GlobalUtilities.ConvertToDouble(dttbl.Rows[1][i]);
            double pcfcos = GlobalUtilities.ConvertToDouble(dttbl.Rows[2][i]);
            double forwardAlreadyBooked = GlobalUtilities.ConvertToDouble(dttbl.Rows[3][i]);
            double unhedgedPos = expOrder - pcfcos - forwardAlreadyBooked;
            dr["Month" + i] = unhedgedPos;
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddHedgedRatioRow(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Hedged Ratio";
        for (int i = 1; i < dttbl.Columns.Count - 3; i++)
        {
            double expOrder = GlobalUtilities.ConvertToDouble(dttbl.Rows[1][i]);
            double pcfcos = GlobalUtilities.ConvertToDouble(dttbl.Rows[2][i]);
            double forwardAlreadyBooked = GlobalUtilities.ConvertToDouble(dttbl.Rows[3][i]);
            string unhedgedPos = "0";
            //if (expOrder != 0) unhedgedPos = Convert.ToInt32((pcfcos + forwardAlreadyBooked) / expOrder);
            if (expOrder != 0) unhedgedPos = DecimalPoint((((pcfcos + forwardAlreadyBooked) / expOrder) * 100), decimalPoint);
            dr["Month" + i] = unhedgedPos + "%";
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddAverageCostingRateRow(DataTable dttbl, int currency, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Average Costing Rate";
        string startDate = "";
        string endDate = "";
        GetStartEndDate(out startDate, out endDate);
        string query = "select case when netamountsum=0 then 0 else sumcost/netamountsum end as weightnetamount,month,year " +
                        "from (select SUM(ISNULL(exportorder_netamount,0) * ISNULL(exportorder_costing,0)) as sumcost, " +
                        "SUM(ISNULL(exportorder_netamount,0)) as netamountsum," +
                        "MONTH(exportorder_expectedduedate) as month," +
                        "YEAR(exportorder_expectedduedate) as year from tbl_exportorder " +
                        "WHERE exportorder_expectedduedate BETWEEN '" + startDate + "' AND '" + endDate + "' " +
                        "AND exportorder_exposurecurrencymasterid=" + currency + " AND exportorder_clientid=" + clientId +
                        " GROUP BY MONTH(exportorder_expectedduedate),YEAR(exportorder_expectedduedate))r";
        DataTable dttblData = DbTable.ExecuteSelect(query);
        SetMonthColumns(dr, dttblData, 4);
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddAveragePCFCRateRow(DataTable dttbl, int currency, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Average PCFC  Rate";
        string startDate = "";
        string endDate = "";
        GetStartEndDate(out startDate, out endDate);
        string query = "select case when netamountsum=0 then 0 else sumcost/netamountsum end as weightnetamount,month,year " +
                        "from (select SUM(ISNULL(pcfc_fcamount,0) * ISNULL(pcfc_spotrate,0)) as sumcost, " +
                        "SUM(ISNULL(pcfc_fcamount,0)) as netamountsum," +
                        "MONTH(pcfc_pcduedate) as month," +
                        "YEAR(pcfc_pcduedate) as year from tbl_pcfc " +
                        "WHERE pcfc_pcduedate BETWEEN '" + startDate + "' AND '" + endDate + "' " +
                        "AND pcfc_exposurecurrencymasterid=" + currency + " AND pcfc_clientid=" + clientId +
                        " GROUP BY MONTH(pcfc_pcduedate),YEAR(pcfc_pcduedate))r";
        DataTable dttblData = DbTable.ExecuteSelect(query);
        SetMonthColumns(dr, dttblData, decimalPoint);
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddAverageForwardBookedRateRow(DataTable dttbl, int currency, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Average Forward Booked Rate";
        string startDate = "";
        string endDate = "";
        GetStartEndDate(out startDate, out endDate);
        string query = "select case when netamountsum=0 then 0 else sumcost/netamountsum end as weightnetamount,month,year " +
                        "from (select SUM(ISNULL(forwardcontract_balancesold,0) * ISNULL(forwardcontract_rate,0)) as sumcost, " +
                        "SUM(ISNULL(forwardcontract_balancesold,0)) as netamountsum," +
                        "MONTH(forwardcontract_to) as month," +
                        "YEAR(forwardcontract_to) as year from tbl_forwardcontract " +
                        "WHERE forwardcontract_to BETWEEN '" + startDate + "' AND '" + endDate + "' " +
                        "AND forwardcontract_exposurecurrencymasterid=" + currency + " AND forwardcontract_clientid=" + clientId +
                        " GROUP BY MONTH(forwardcontract_to),YEAR(forwardcontract_to))r";
        DataTable dttblData = DbTable.ExecuteSelect(query);
        SetMonthColumns(dr, dttblData, decimalPoint);
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddCurrentForwardRateRow(DataTable dttbl, int currency, bool isVisible, int decimalPoint, int year)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Current Forward Rate";
        //int minliverateId = 0;

        //if (currency == 1)
        //{
        //    minliverateId = 234;//USD
        //}
        //else if (currency == 2)//EUR
        //{
        //    minliverateId = 344;
        //}
        //else if (currency == 3)//GBP
        //{
        //    minliverateId = 449;
        //}
        //else if (currency == 4)//JPY
        //{
        //    minliverateId = 553;
        //}
        //if (year > 0)
        //{
        //    if (currency == 1)//USD
        //    {
        //        minliverateId = 2847 + year * 12;
        //    }
        //    else if (currency == 2)//EUR
        //    {
        //        minliverateId = 1292 + year * 12;
        //    }
        //    else if (currency == 3)//GBP
        //    {
        //        minliverateId = 3615 + year * 12;
        //    }
        //}
        //int maxliverateId = minliverateId + 11;

        //string query = "select liverate_currentrate from tbl_liverate where " +
        //                "liverate_liverateid between " + minliverateId + " AND " + maxliverateId + " order by liverate_liverateid";
        //DataTable dttblLiverate = DbTable.ExecuteSelect(query);


        DataTable dttblLiverate = GetOutrightRates(true, currency, year);

        for (int i = 0; i < dttblLiverate.Rows.Count; i++)
        {
            double rate = GlobalUtilities.ConvertToDouble(dttblLiverate.Rows[i]["liverate_currentrate"]);
            dr["Month" + (i + 1)] = DecimalPoint(rate, decimalPoint);
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    public DataTable GetOutrightRates(bool isExport, int currency, int year)
    {
        double spotrate = Finstation.GetSpotRate(isExport, currency);
        int minliverateId = 0;
        if (isExport)
        {
            if (currency == 1)
            {
                minliverateId = 208;//USD
            }
            else if (currency == 2)//EUR
            {
                minliverateId = 1266;
            }
            else if (currency == 3)//GBP
            {
                minliverateId = 1370;
            }
            else if (currency == 4)//JPY
            {
                minliverateId = 527;
            }
            if (year > 0)
            {
                if (currency == 1)//USD
                {
                    minliverateId = 2751 + (year - 1) * 12;
                }
                else if (currency == 2)//EUR
                {
                    minliverateId = 3135 + (year - 1) * 12;
                }
                else if (currency == 3)//GBP
                {
                    minliverateId = 3519 + (year - 1) * 12;
                }
            }
        }
        else
        {
            if (currency == 1)
            {
                minliverateId = 221;//USD
            }
            else if (currency == 2)//EUR
            {
                minliverateId = 1279;
            }
            else if (currency == 3)//GBP
            {
                minliverateId = 1383;
            }
            else if (currency == 4)//JPY
            {
                minliverateId = 540;
            }
            if (year > 0)
            {
                if (currency == 1)//USD
                {
                    minliverateId = 2799 + (year - 1) * 12;
                }
                else if (currency == 2)//EUR
                {
                    minliverateId = 3183 + (year - 1) * 12;
                }
                else if (currency == 3)//GBP
                {
                    minliverateId = 3567 + (year - 1) * 12;
                }
            }
        }
        int maxliverateId = minliverateId + 11;
        string query = "select liverate_currentrate from tbl_liverate where " +
                        "liverate_liverateid between " + minliverateId + " AND " + maxliverateId + " order by liverate_liverateid";
        DataTable dttblLiverate = DbTable.ExecuteSelect(query);
        //ErrorLog.WriteLog(Environment.NewLine+"spotrate:" + spotrate);
        for (int i = 0; i < dttblLiverate.Rows.Count; i++)
        {
            double rate = GlobalUtilities.ConvertToDouble(dttblLiverate.Rows[i]["liverate_currentrate"]);
            //ErrorLog.WriteLog(Environment.NewLine + "rate:" + rate);
            if (currency == 3 || currency == 4)//GBP/JPY
            {
                rate = rate / 100.0;
            }
            rate = spotrate + rate / 100.0;
            dttblLiverate.Rows[i]["liverate_currentrate"] = rate;
        }
        return dttblLiverate;
    }
    private void AddEffectiveRateExportPositionRow(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Effective Rate of Export Position";
        for (int i = 1; i <= 12; i++)
        {
            double exportOrderReceived = GlobalUtilities.ConvertToDouble(dttbl.Rows[1][i]);//CH6
            double pcfcOS = GlobalUtilities.ConvertToDouble(dttbl.Rows[2][i]);//CH7
            double forwardAlreadyBooked = GlobalUtilities.ConvertToDouble(dttbl.Rows[3][i]);//CH8
            double unhedgedPosition = GlobalUtilities.ConvertToDouble(dttbl.Rows[4][i]);//CH10
            double averagePcfcRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[7][i]);//CH15
            double averageforwardBookedRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[8][i]);//CH16
            double currentForwardRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[9][i]);//CH18

            double effectiveRateOfExpPosition = 0;
            if (unhedgedPosition >= 0)
            {
                if (exportOrderReceived > 0)
                {
                    effectiveRateOfExpPosition = ((unhedgedPosition * currentForwardRate) + (forwardAlreadyBooked * averageforwardBookedRate)
                                                    + (averagePcfcRate * pcfcOS)) / exportOrderReceived;
                }
            }
            else
            {
                double divfact = pcfcOS * forwardAlreadyBooked;
                if (divfact > 0)
                {
                    effectiveRateOfExpPosition = ((pcfcOS * averagePcfcRate) + (forwardAlreadyBooked * averageforwardBookedRate)) / divfact;
                }
            }
            dr["Month" + i] = DecimalPoint(effectiveRateOfExpPosition, decimalPoint);
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddProfitLossperFCunitRow(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Profit/Loss per FC unit";
        for (int i = 1; i < dttbl.Columns.Count - 3; i++)
        {
            double avgCostRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[6][i]);
            double effectiveRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[10][i]);
            double profitLostUnit = effectiveRate - avgCostRate;
            string strprofitLostUnit = DecimalPoint(profitLostUnit, decimalPoint);
            dr["Month" + i] = strprofitLostUnit;
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    public static string DecimalPoint(object value, int decimalPoint)
    {
        string strVal = "";
        if (value == DBNull.Value) return "0";
        value = GlobalUtilities.ConvertToDouble(value);
        if (decimalPoint == 2)
        {
            strVal = String.Format("{0:0.00}", value);
        }
        else if (decimalPoint == 4)
        {
            strVal = String.Format("{0:0.0000}", value);
        }
        else if (decimalPoint == 6)
        {
            strVal = String.Format("{0:0.000000}", value);
        }
        else
        {
            strVal = value.ToString();
        }
        return strVal;
    }
    private void AddPLonExportPortfolioRow(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "P&L on Export Portfolio (Rs.)";
        for (int i = 1; i < dttbl.Columns.Count - 3; i++)
        {
            double expOrder = GlobalUtilities.ConvertToDouble(dttbl.Rows[1][i]);
            double profitLostUnit = GlobalUtilities.ConvertToDouble(dttbl.Rows[11][i]);
            double plExportPortfolio = expOrder * profitLostUnit;
            string strplExportPortfolio = DecimalPoint(plExportPortfolio, decimalPoint);
            dr["Month" + i] = strplExportPortfolio;
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddMtmWithAverageCostingRow(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Mtm With Average Costing";
        for (int i = 1; i <= 12; i++)
        {
            double averagecostingRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[8][i]);//CH14
            double currentForwardRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[9][i]);//CH18

            double mtmWithAvgCostingRate = 0;
            if (currentForwardRate != 0)
            {
                if (averagecostingRate != 0)
                {
                    mtmWithAvgCostingRate = currentForwardRate - averagecostingRate;
                }
            }
            dr["Month" + i] = DecimalPoint(mtmWithAvgCostingRate, decimalPoint);
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddMtmOfForwardRow(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Mtm  Of Forward";
        for (int i = 1; i <= 12; i++)
        {
            double averageforwardBookedRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[8][i]);//CH16
            double currentForwardRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[9][i]);//CH18

            double mtmOfForward = 0;
            if (averageforwardBookedRate != 0)
            {
                mtmOfForward = averageforwardBookedRate - currentForwardRate;
            }
            dr["Month" + i] = DecimalPoint(mtmOfForward, decimalPoint);
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddPLonUnhedgedExposureRow(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "P&L on Unhedged Exposure (Rs.)";
        for (int i = 1; i <= 12; i++)
        {
            double unhedgedPosition = GlobalUtilities.ConvertToDouble(dttbl.Rows[13][i]);
            double currentForwardRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[9][i]);
            double avgCosting = GlobalUtilities.ConvertToDouble(dttbl.Rows[6][i]);

            double plOnUnhedged = unhedgedPosition * currentForwardRate - unhedgedPosition * avgCosting;
            dr["Month" + i] = DecimalPoint(plOnUnhedged, decimalPoint);
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddPLonHedgedexposureRow(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "P&L on Hedged exposure";
        for (int i = 1; i <= 12; i++)
        {
            double forwardAlreadyBooked = GlobalUtilities.ConvertToDouble(dttbl.Rows[3][i]);//CH8
            double mtmOfForward = GlobalUtilities.ConvertToDouble(dttbl.Rows[14][i]);//CH23

            double plHedgedExposure = forwardAlreadyBooked * mtmOfForward;
            dr["Month" + i] = DecimalPoint(plHedgedExposure, decimalPoint);
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddPLFromCosting(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "P&L from Costing";
        for (int i = 1; i <= 12; i++)
        {
            double exportOrderReceived = GlobalUtilities.ConvertToDouble(dttbl.Rows[1][i]);
            double avgCostingRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[6][i]);
            double pcfcOS = GlobalUtilities.ConvertToDouble(dttbl.Rows[2][i]);
            double avgPcfcRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[7][i]);
            double forwardAlreadyBooked = GlobalUtilities.ConvertToDouble(dttbl.Rows[3][i]);
            double avgForwardBookedRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[8][i]);
            double unhedgedPosition = GlobalUtilities.ConvertToDouble(dttbl.Rows[4][i]);
            double currentForwardRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[9][i]);
            double pcfcFromCosting = 0;
            if (exportOrderReceived != 0 && avgCostingRate != 0)
            {
                pcfcFromCosting = pcfcOS * avgPcfcRate + forwardAlreadyBooked * avgForwardBookedRate
                                   + unhedgedPosition * currentForwardRate - exportOrderReceived * avgCostingRate;

                //HttpContext.Current.Response.Write(Convert.ToString(avgForwardBookedRate));
                //HttpContext.Current.Response.End();
            }
            dr["Month" + i] = DecimalPoint(pcfcFromCosting, decimalPoint);
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddPLonHedgedExposure(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "P&L on Hedged Exposure";
        for (int i = 1; i <= 12; i++)
        {
            double exportOrderReceived = GlobalUtilities.ConvertToDouble(dttbl.Rows[1][i]);
            double avgCostingRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[6][i]);
            double pcfcOS = GlobalUtilities.ConvertToDouble(dttbl.Rows[2][i]);
            double avgPcfcRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[7][i]);
            double forwardAlreadyBooked = GlobalUtilities.ConvertToDouble(dttbl.Rows[3][i]);
            double avgForwardBookedRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[8][i]);
            double currentForwardRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[9][i]);
            double hedgedPosition = pcfcOS + forwardAlreadyBooked;
            double plOnHedgedExposure = 0;
            plOnHedgedExposure = (pcfcOS * avgPcfcRate) + (forwardAlreadyBooked * avgForwardBookedRate) - (hedgedPosition * currentForwardRate);
            dr["Month" + i] = DecimalPoint(plOnHedgedExposure, decimalPoint);
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddPLonUnHedgedExposure(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "P&L on UnHedged Exposure";
        for (int i = 1; i <= 12; i++)
        {
            double exportOrderReceived = GlobalUtilities.ConvertToDouble(dttbl.Rows[1][i]);
            double avgCostingRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[6][i]);
            double pcfcOS = GlobalUtilities.ConvertToDouble(dttbl.Rows[2][i]);
            double avgPcfcRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[7][i]);
            double forwardAlreadyBooked = GlobalUtilities.ConvertToDouble(dttbl.Rows[3][i]);
            double avgForwardBookedRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[8][i]);
            double currentForwardRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[9][i]);
            double unhedgedPosition = GlobalUtilities.ConvertToDouble(dttbl.Rows[4][i]);
            double plOnUnHedgedExposure = 0;
            if (avgCostingRate != 0 && unhedgedPosition != 0)
            {
                plOnUnHedgedExposure = (unhedgedPosition * currentForwardRate) - (unhedgedPosition * avgCostingRate);
            }
            dr["Month" + i] = DecimalPoint(plOnUnHedgedExposure, decimalPoint);
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void CalculateTotal(DataTable dttbl, bool isVisible)
    {
        for (int i = 1; i < dttbl.Rows.Count; i++)
        {
            double total = 0;
            for (int j = 1; j <= 12; j++)
            {
                string data = Convert.ToString(dttbl.Rows[i][j]);
                if (i == 5)//%
                {
                    data = data.Replace("%", "");
                }
                total += GlobalUtilities.ConvertToDouble(data);
            }
            if (i == 9 || i == 10 || i == 11 || i == 13 || i == 14)
            {
                dttbl.Rows[i]["Total"] = "-";
            }
            else
            {
                if (i == 5)//%
                {
                    double dblTotalForwardBooked = GlobalUtilities.ConvertToDouble(dttbl.Rows[3]["Total"]);
                    double dblTotalExportOrderReceived = GlobalUtilities.ConvertToDouble(dttbl.Rows[1]["Total"]);
                    double dblTotalPcfc = GlobalUtilities.ConvertToDouble(dttbl.Rows[2]["Total"]);
                    if (dblTotalExportOrderReceived > 0) total = ((dblTotalForwardBooked + dblTotalPcfc) / dblTotalExportOrderReceived) * 100;
                    if (total == 0)
                    {
                        dttbl.Rows[i]["Total"] = "0%";
                    }
                    else
                    {
                        dttbl.Rows[i]["Total"] = String.Format("{0:0}", total) + "%";
                    }
                }
                else
                {
                    int decimalPoint = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["DecimalPoint"]);
                    dttbl.Rows[i]["Total"] = DecimalPoint(total, decimalPoint);
                }
            }
        }
        //calculate total average costing rate 
        double sumOrderReceived = 0;
        double sumAvgCostRate = 0;
        for (int i = 1; i < dttbl.Columns.Count - 3; i++)
        {
            double orderReceived = GlobalUtilities.ConvertToDouble(dttbl.Rows[1][i]);
            double avgCostRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[6][i]);
            sumOrderReceived += orderReceived;
            sumAvgCostRate += orderReceived * avgCostRate;
        }
        double grossAvgCostRate = 0;
        if (sumOrderReceived > 0)
        {
            grossAvgCostRate = sumAvgCostRate / sumOrderReceived;
        }
        dttbl.Rows[6]["Total"] = String.Format("{0:0.0000}", grossAvgCostRate);
        //calculate total average pcfc rate 
        double sumPcfcOs = 0;
        double sumAvgPcfcRate = 0;
        for (int i = 1; i < dttbl.Columns.Count - 3; i++)
        {
            double pcfcos = GlobalUtilities.ConvertToDouble(dttbl.Rows[2][i]);
            double avgpcfcRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[7][i]);
            sumPcfcOs += pcfcos;
            sumAvgPcfcRate += avgpcfcRate * pcfcos;
        }
        double grossAvgPcfcRate = 0;
        if (sumPcfcOs > 0)
        {
            grossAvgPcfcRate = sumAvgPcfcRate / sumPcfcOs;
        }
        dttbl.Rows[7]["Total"] = String.Format("{0:0.0000}", grossAvgPcfcRate);

        //calculate total average pcfc rate 
        double sumAlreadyBooked = 0;
        double sumForwardBookedRate = 0;
        for (int i = 1; i < dttbl.Columns.Count - 3; i++)
        {
            double alreadyBooked = GlobalUtilities.ConvertToDouble(dttbl.Rows[3][i]);
            double forwardBooked = GlobalUtilities.ConvertToDouble(dttbl.Rows[8][i]);
            sumAlreadyBooked += alreadyBooked;
            sumForwardBookedRate += alreadyBooked * forwardBooked;
        }
        double grossForwardBookedRate = 0;
        if (sumAlreadyBooked > 0)
        {
            grossForwardBookedRate = sumForwardBookedRate / sumAlreadyBooked;
        }
        dttbl.Rows[8]["Total"] = String.Format("{0:0.0000}", grossForwardBookedRate);
        dttbl.Rows[8]["IsVisible"] = isVisible;
    }
    public DataTable GetColumns(string module)
    {
        string query = @"select * from tbl_columns 
                         join tbl_module on module_moduleid=columns_moduleid
                         where columns_isgenerate=1 AND REPLACE(module_modulename,' ','')='" + module + "' " +
                         "AND (ISNULL(columns_isenableinedit,0) = 0 OR (ISNULL(columns_isenableinedit,0) = 1 " +
                                  "AND columns_columnsid IN(select customercustomfields_columnsid from tbl_customercustomfields WHERE customercustomfields_clientid=" + clientId + "))) " +
                         @" order by columns_sequence";
        DataTable dttblCol = DbTable.ExecuteSelect(query);
        return dttblCol;
    }
    public string GetSummaryData(string module, string where)
    {
        DataRow dr = GetSummaryDataRow(module, where);
        string json = JSON.ConvertAmountComma(dr, "", true);
        json = json.Replace(".000000", ".00").Replace(".0000", ".00");
        return json;
    }
    public DataRow GetSummaryDataRow(string module, string where)
    {
        DataTable dttblCol = GetColumns(module);
        StringBuilder sumQuery = new StringBuilder();
        sumQuery.Append("select ");
        for (int i = 0; i < dttblCol.Rows.Count; i++)
        {
            string columnname = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_columnname"]);
            string attributes = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_attributes"]);
            string shortColum = columnname.Split('_').GetValue(1).ToString();
            if (attributes.Contains("IsTotal"))
            {
                sumQuery.Append(",SUM(" + columnname + ") as " + shortColum);
            }
            else if (attributes.Contains("WeightedAvg"))
            {
                string weightColumns = Common.GetAttributes(attributes, "WeightedAvg");
                Array arrwa = weightColumns.Split(',');
                string weightCol1 = module + "_" + arrwa.GetValue(0).ToString();
                string weightCol2 = module + "_" + arrwa.GetValue(1).ToString();
                sumQuery.Append(@",cast(CASE WHEN ISNULL(SUM(" + weightCol1 + @"),0) = 0 THEN 0 
                                            ELSE SUM(" + weightCol1 + " * " + weightCol2 + @")/SUM(" + weightCol1 + @") END as numeric(19,2)) AS " + shortColum);
            }
            else if (attributes.Contains("IsAvg"))
            {
                if (columnname == "exportorder_costing")
                {
                    sumQuery.Append(@",cast(CASE WHEN ISNULL(SUM(exportorder_netamount),0) = 0 THEN 0 
                                            ELSE SUM(exportorder_value)/SUM(exportorder_netamount) END as numeric(19,2)) AS " + shortColum);
                }
                else if (columnname == "forwardcontract_rate")
                {
                    sumQuery.Append(@",cast(CASE WHEN ISNULL(SUM(forwardcontract_balancesold),0) = 0 THEN 0 
                                            ELSE SUM(forwardcontract_soldamountinrs)/SUM(forwardcontract_balancesold) END as numeric(19,2)) AS " + shortColum);
                }
                else if (columnname == "pcfc_spotrate")
                {
                    sumQuery.Append(@",cast(CASE WHEN ISNULL(SUM(pcfc_fcamountbalance),0) = 0 THEN 0 
                                            ELSE SUM(pcfc_product)/SUM(pcfc_fcamountbalance) END as numeric(19,2)) AS " + shortColum);
                }
                else
                {
                    sumQuery.Append(",cast(AVG(" + columnname + ") as numeric(19,2)) as " + shortColum);
                }
            }
        }
        sumQuery.Append(" from tbl_" + module + " where " + module + "_clientid=" + clientId);
        if (Common.GetQueryString("pm") != "")
        {
            sumQuery.Append(" AND " + module + "_" + Common.GetQueryString("pm") + "id=" + Common.GetQueryStringValue("pid"));
        }
        //if (Common.GetQueryString("ew") != "")
        //{
        //    sumQuery.Append(" AND " + Common.GetQueryString("ew").Replace("~", "="));
        //}
        if (where != "")
        {
            where = where.Replace("~", "=").Replace("--", "");
            sumQuery.Append(" AND " + where);
        }
        string qry = sumQuery.ToString().Replace("select ,", "select ");
        //HttpContext.Current.Response.Write(qry);
        //HttpContext.Current.Response.End();
        DataRow dr = DbTable.ExecuteSelectRow(qry);

        return dr;
    }
    public static bool IsFEMPortalEnabled(int PortalType)
    {
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        int prospectType = 0;
        if (PortalType == 1)
        {
            prospectType = 4;
        }
        else
        {
            prospectType = 5;
        }
        int subscriptionId = Finstation.GetSubscriptionId(clientId);
        string query = "select * from tbl_subscriptionprospects " +
                        "join tbl_subscription ON subscription_subscriptionid=subscriptionprospects_subscriptionid " +
                        "WHERE subscriptionprospects_prospectid = " + prospectType + " AND subscription_clientid=" + clientId + " and subscription_subscriptionid=" + subscriptionId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            int trialId = Finstation.GetTrialId(clientId);
            query = "select * from tbl_trialprospect " +
                        "join tbl_trial ON trial_trialid=trialprospect_trialid " +
                        "WHERE trialprospect_prospectid = " + prospectType + " AND trial_clientid=" + clientId + " and trial_trialid=" + trialId;
            dr = DbTable.ExecuteSelectRow(query);
            if (dr == null) return false;
        }
        return true;
    }
    public string GetDashboardData_Import(int currency, bool isEmail, out DataTable dttbl)
    {
        return GetDashboardData_Import(currency, isEmail, 0, out dttbl);
    }
    public string GetDashboardData_Import(int currency, bool isEmail, int year, out DataTable dttbl)
    {
        _year = year;
        dttbl = new DataTable();
        StringBuilder html = new StringBuilder();
        dttbl.Columns.Add("Label");

        for (int i = 1; i <= 12; i++)
        {
            dttbl.Columns.Add("Month" + i);
        }
        dttbl.Columns.Add("Total");
        dttbl.Columns.Add("IsVisible");
        dttbl.Columns.Add("DecimalPoint");

        DataRow dr = dttbl.NewRow();
        dr["IsVisible"] = true;
        dr["Label"] = "Import";
        DateTime dtcurrent = DateTime.Now;
        dtcurrent = dtcurrent.AddYears(year);
        int m = dtcurrent.Month;
        int y = dtcurrent.Year;
        DateTime date = new DateTime(y, m, GlobalUtilities.GetMonthEndDay(m, y));
        for (int i = 0; i < 12; i++)
        {
            DateTime dt = date.AddMonths(i);
            string strdate = string.Format("{0:MMM-yy}", dt);
            dr["Month" + (i + 1)] = strdate;
        }
        dr["Total"] = "Total";
        dttbl.Rows.Add(dr);

        AddImportOrderRow_Import(dttbl, currency, true, 2);//D6
        AddTradeCreditRow_Import(dttbl, currency, true, 2);//D7
        AddForwardContractOutstandingRow_Import(dttbl, currency, true, 2);//D8
        AddUnhedgedPositionRow_Import(dttbl, true, 2);//D10
        AddHedgedRatioRow_Import(dttbl, true, 2);//D12
        AddAverageCostingRateRow_Import(dttbl, currency, true, 4);//D14
        AddAverageTradeCreditRow_Import(dttbl, currency, true, 4);//D15
        AddAverageForwardContractBookedRateRow_Import(dttbl, currency, true, 4);//D16
        AddCurrentForwardRateRow_Import(dttbl, currency, true, 4, year);//D19
        AddEffectiveRateExportPositionRow_Import(dttbl, true, 2);//D26
        AddProfitLossFromCostingRow_Import(dttbl, true, 2);//D28
        AddProfitLossHedgedExposureRow_Import(dttbl, true, 2);//D31
        AddProfitLossUnHedgedExposureRow_Import(dttbl, true, 2);//D34

        CalculateTotal_Import(dttbl, true);

        if (isEmail)
        {
            html.Append("<table width='100%' cellspacing=0 cellpadding=5  style='border-collapse: collapse;border:1px solid #000;' border='1'>");
        }
        else
        {
            html.Append("<table width='100%' cellspacing=0 cellpadding=5 class='da-grid'>");
        }
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (GlobalUtilities.ConvertToBool(dttbl.Rows[i]["IsVisible"]) == false) continue;
            if (i == 0)
            {
                if (isEmail)
                {
                    html.Append("<tr style='font-weight:bold;'>");
                }
                else
                {
                    html.Append("<tr class='da-grid-header'>");
                }
            }
            else
            {
                html.Append("<tr class='da-grid-row'>");
            }
            for (int j = 0; j < dttbl.Columns.Count - 2; j++)
            {
                string data = GlobalUtilities.ConvertToString(dttbl.Rows[i][j]);
                if (j == 0)
                {
                    html.Append("<td row='" + i + "'>" + data + "</td>");
                }
                else
                {
                    html.Append("<td style='text-align:center;'>" + data + "</td>");
                }
            }
            html.Append("</tr>");
        }
        html.Append("</table>");

        return html.ToString();
    }
    private void AddImportOrderRow_Import(DataTable dttbl, int currency, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Import Order";
        string startDate = "";
        string endDate = "";
        GetStartEndDate(out startDate, out endDate);
        string query = "select SUM(ISNULL(fimimportorder_netimportorderamountpayable,0)) as netamount, MONTH(fimimportorder_expectedduedate) as month," +
                        "YEAR(fimimportorder_expectedduedate) as year from tbl_fimimportorder " +
                        "WHERE fimimportorder_expectedduedate BETWEEN '" + startDate + "' AND '" + endDate + "' " +
                        "AND fimimportorder_exposurecurrencymasterid=" + currency + " AND fimimportorder_clientid=" + clientId +
                        " GROUP BY MONTH(fimimportorder_expectedduedate),YEAR(fimimportorder_expectedduedate)";
        DataTable dttblData = DbTable.ExecuteSelect(query);
        SetMonthColumns(dr, dttblData);
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddTradeCreditRow_Import(DataTable dttbl, int currency, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Trade Credit";
        string startDate = "";
        string endDate = "";
        GetStartEndDate(out startDate, out endDate);
        string query = "select SUM(ISNULL(fimtradecredit_outstandingtradecreditamount,0)) as netamount, MONTH(fimtradecredit_tradecreditduedate) as month," +
                        "YEAR(fimtradecredit_tradecreditduedate) as year from tbl_fimtradecredit " +
                        "WHERE fimtradecredit_tradecreditduedate BETWEEN '" + startDate + "' AND '" + endDate + "' " +
                        "AND fimtradecredit_exposurecurrencymasterid=" + currency + " AND fimtradecredit_clientid=" + clientId +
                        " GROUP BY MONTH(fimtradecredit_tradecreditduedate),YEAR(fimtradecredit_tradecreditduedate)";
        DataTable dttblData = DbTable.ExecuteSelect(query);
        SetMonthColumns(dr, dttblData);
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddForwardContractOutstandingRow_Import(DataTable dttbl, int currency, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Forward Contract Outstandings";
        string startDate = "";
        string endDate = "";
        GetStartEndDate(out startDate, out endDate);
        string query = "select SUM(ISNULL(fimforwardcontract_forwardbalanceamount,0)) as netamount, MONTH(fimforwardcontract_todate) as month," +
                        "YEAR(fimforwardcontract_todate) as year from tbl_fimforwardcontract " +
                        "WHERE fimforwardcontract_todate BETWEEN '" + startDate + "' AND '" + endDate + "' " +
                        "AND fimforwardcontract_exposurecurrencymasterid=" + currency + " AND fimforwardcontract_clientid=" + clientId +
                        " GROUP BY MONTH(fimforwardcontract_todate),YEAR(fimforwardcontract_todate)";
        DataTable dttblData = DbTable.ExecuteSelect(query);
        SetMonthColumns(dr, dttblData);
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddUnhedgedPositionRow_Import(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Unhedged Position";
        for (int i = 1; i < dttbl.Columns.Count - 3; i++)
        {
            double importOrder = GlobalUtilities.ConvertToDouble(dttbl.Rows[1][i]);
            double tradeCredit = GlobalUtilities.ConvertToDouble(dttbl.Rows[2][i]);
            double forwardContractOutstanding = GlobalUtilities.ConvertToDouble(dttbl.Rows[3][i]);
            double unhedgedPos = importOrder + tradeCredit - forwardContractOutstanding;
            dr["Month" + i] = unhedgedPos;
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddHedgedRatioRow_Import(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Hedged Ratio";
        for (int i = 1; i < dttbl.Columns.Count - 3; i++)
        {
            double importOrder = GlobalUtilities.ConvertToDouble(dttbl.Rows[1][i]);
            double tradeCredit = GlobalUtilities.ConvertToDouble(dttbl.Rows[2][i]);
            double forwardContractOutstanding = GlobalUtilities.ConvertToDouble(dttbl.Rows[3][i]);
            string hedgedRatio = "0";
            if (importOrder + tradeCredit != 0) hedgedRatio = DecimalPoint(((forwardContractOutstanding / (importOrder + tradeCredit)) * 100), decimalPoint);
            dr["Month" + i] = hedgedRatio + "%";
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddAverageCostingRateRow_Import(DataTable dttbl, int currency, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Average Costing Rate";
        string startDate = "";
        string endDate = "";
        GetStartEndDate(out startDate, out endDate);
        string query = "select case when netamountsum=0 then 0 else sumcost/netamountsum end as weightnetamount,month,year " +
                        "from (select SUM(ISNULL(fimimportorder_netimportorderamountpayable,0) * ISNULL(fimimportorder_costing,0)) as sumcost, " +
                        "SUM(ISNULL(fimimportorder_netimportorderamountpayable,0)) as netamountsum," +
                        "MONTH(fimimportorder_expectedduedate) as month," +
                        "YEAR(fimimportorder_expectedduedate) as year from tbl_fimimportorder " +
                        "WHERE fimimportorder_expectedduedate BETWEEN '" + startDate + "' AND '" + endDate + "' " +
                        "AND fimimportorder_exposurecurrencymasterid=" + currency + " AND fimimportorder_clientid=" + clientId +
                        " GROUP BY MONTH(fimimportorder_expectedduedate),YEAR(fimimportorder_expectedduedate))r";
        DataTable dttblData = DbTable.ExecuteSelect(query);
        SetMonthColumns(dr, dttblData, decimalPoint);
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddAverageTradeCreditRow_Import(DataTable dttbl, int currency, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Average Trade Credit Rate";
        string startDate = "";
        string endDate = "";
        GetStartEndDate(out startDate, out endDate);
        string query = "select case when netamountsum=0 then 0 else sumcost/netamountsum end as weightnetamount,month,year " +
                        "from (select SUM(ISNULL(fimtradecredit_outstandingtradecreditamount,0) * ISNULL(fimtradecredit_spotontradecreditavailed,0)) as sumcost, " +
                        "SUM(ISNULL(fimtradecredit_outstandingtradecreditamount,0)) as netamountsum," +
                        "MONTH(fimtradecredit_tradecreditduedate) as month," +
                        "YEAR(fimtradecredit_tradecreditduedate) as year from tbl_fimtradecredit " +
                        "WHERE fimtradecredit_tradecreditduedate BETWEEN '" + startDate + "' AND '" + endDate + "' " +
                        "AND fimtradecredit_exposurecurrencymasterid=" + currency + " AND fimtradecredit_clientid=" + clientId +
                        " GROUP BY MONTH(fimtradecredit_tradecreditduedate),YEAR(fimtradecredit_tradecreditduedate))r";
        DataTable dttblData = DbTable.ExecuteSelect(query);
        SetMonthColumns(dr, dttblData, decimalPoint);
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddAverageForwardContractBookedRateRow_Import(DataTable dttbl, int currency, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Average Forward Booked Rate";
        string startDate = "";
        string endDate = "";
        GetStartEndDate(out startDate, out endDate);
        string query = "select case when netamountsum=0 then 0 else sumcost/netamountsum end as weightnetamount,month,year " +
                        "from (select SUM(ISNULL(fimforwardcontract_forwardbalanceamount,0) * ISNULL(fimforwardcontract_forwardbookingrate,0)) as sumcost, " +
                        "SUM(ISNULL(fimforwardcontract_forwardbalanceamount,0)) as netamountsum," +
                        "MONTH(fimforwardcontract_todate) as month," +
                        "YEAR(fimforwardcontract_todate) as year from tbl_fimforwardcontract " +
                        "WHERE fimforwardcontract_todate BETWEEN '" + startDate + "' AND '" + endDate + "' " +
                        "AND fimforwardcontract_exposurecurrencymasterid=" + currency + " AND fimforwardcontract_clientid=" + clientId +
                        " GROUP BY MONTH(fimforwardcontract_todate),YEAR(fimforwardcontract_todate))r";
        DataTable dttblData = DbTable.ExecuteSelect(query);
        SetMonthColumns(dr, dttblData, decimalPoint);
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddCurrentForwardRateRow_Import(DataTable dttbl, int currency, bool isVisible, int decimalPoint, int year)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Current  Forward Rate";
        //int minliverateId = 248;//USD
        //int minPremiumMonthEnd = 0;
        //int spotrateId = 0;
        //if (currency == 2)//EUR
        //{
        //    minliverateId = 1305;// 357;taken month end
        //    minPremiumMonthEnd = 1279;
        //    spotrateId = 11;
        //}
        //else if (currency == 3)//GBP
        //{
        //    minliverateId = 1409;
        //    minPremiumMonthEnd = 1383;// 1370;
        //    spotrateId = 20;
        //}
        //else if (currency == 4)//JPY
        //{
        //    minliverateId = 566;
        //}
        //if (year > 0)
        //{
        //    if (currency == 1)//USD
        //    {
        //        minliverateId = 2895 + year * 12;
        //    }
        //    else if (currency == 2)//EUR
        //    {
        //        minliverateId = 1305 + year * 12;
        //    }
        //    else if (currency == 3)//GBP
        //    {
        //        minliverateId = 1409 + year * 12;
        //    }
        //}
        //int maxliverateId = minliverateId + 11;

        //string query = "";
        //if (currency == 2 || currency == 3)//EUR/GBP
        //{
        //    double sportrate = GetLiveRate(spotrateId);
        //    for (int i = 0; i < 12; i++)
        //    {
        //        double premium = GetLiveRate(minPremiumMonthEnd);
        //        if(currency ==3) premium = premium / 100.0;
        //        double outrightRate = sportrate + premium / 100.0;
        //        dr["Month" + (i + 1)] = DecimalPoint(outrightRate, decimalPoint);
        //        minPremiumMonthEnd++;
        //    }
        //}
        //else
        //{
        //    query = "select liverate_currentrate from tbl_liverate where " +
        //                     "liverate_liverateid between " + minliverateId + " AND " + maxliverateId + " order by liverate_liverateid";
        //    DataTable dttblLiverate = DbTable.ExecuteSelect(query);
        //    for (int i = 0; i < dttblLiverate.Rows.Count; i++)
        //    {
        //        double rate = GlobalUtilities.ConvertToDouble(dttblLiverate.Rows[i]["liverate_currentrate"]);
        //        dr["Month" + (i + 1)] = DecimalPoint(rate, decimalPoint);
        //    }
        //}

        DataTable dttblLiverate = GetOutrightRates(false, currency, year);
        for (int i = 0; i < dttblLiverate.Rows.Count; i++)
        {
            double rate = GlobalUtilities.ConvertToDouble(dttblLiverate.Rows[i]["liverate_currentrate"]);
            dr["Month" + (i + 1)] = DecimalPoint(rate, decimalPoint);
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private double GetLiveRate(int liveRateId)
    {
        string query = "select liverate_currentrate from tbl_liverate where liverate_liverateid=" + liveRateId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        return GlobalUtilities.ConvertToDouble(dr["liverate_currentrate"]);
    }
    private void AddEffectiveRateExportPositionRow_Import(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "Effective Rate of Import Position";
        for (int i = 1; i <= 12; i++)
        {
            double importOrder = GlobalUtilities.ConvertToDouble(dttbl.Rows[1][i]);
            double tradeCredit = GlobalUtilities.ConvertToDouble(dttbl.Rows[2][i]);
            double forwardContractOutstanding = GlobalUtilities.ConvertToDouble(dttbl.Rows[3][i]);
            double unhedgedPosition = GlobalUtilities.ConvertToDouble(dttbl.Rows[4][i]);
            double avgCostingRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[6][i]);
            double avgTradeCreditRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[7][i]);
            double avgForwardBookedRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[8][i]);
            double currentForwardRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[9][i]);

            //double avgForwardBookedRate = importOrder * avgCostingRate;
            double forwardsAlreadyBooked = tradeCredit * avgTradeCreditRate;
            //double d23 = forwardsAlreadyBooked * avgForwardBookedRate;
            double d23 = forwardContractOutstanding * avgForwardBookedRate;
            double d24 = unhedgedPosition * currentForwardRate;
            double effectiveRateOfImpPosition = 0;

            //if ((avgCostingRate + avgTradeCreditRate) != 0) effectiveRateOfImpPosition = (d23 + d24) / (avgCostingRate + avgTradeCreditRate);
            if (unhedgedPosition >= 0)
            {
                if ((importOrder + tradeCredit) != 0)
                {
                    effectiveRateOfImpPosition = (d23 + d24) / (importOrder + tradeCredit);
                }
            }
            else
            {
                effectiveRateOfImpPosition = d23 / forwardContractOutstanding;
            }
            dr["Month" + i] = DecimalPoint(effectiveRateOfImpPosition, decimalPoint);
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddProfitLossFromCostingRow_Import(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "P&L from Costing";
        for (int i = 1; i < dttbl.Columns.Count - 3; i++)
        {
            double importOrder = GlobalUtilities.ConvertToDouble(dttbl.Rows[1][i]);
            double tradeCredit = GlobalUtilities.ConvertToDouble(dttbl.Rows[2][i]);
            double forwardContractOutstanding = GlobalUtilities.ConvertToDouble(dttbl.Rows[3][i]);
            double unhedgedPosition = GlobalUtilities.ConvertToDouble(dttbl.Rows[4][i]);
            double avgCostingRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[6][i]);
            double avgTradeCreditRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[7][i]);
            double avgForwardBookedRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[8][i]);
            double currentForwardRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[9][i]);

            double d21 = importOrder * avgCostingRate;
            double d22 = tradeCredit * avgTradeCreditRate;
            //double d23 = d21 * d22;
            double d23 = forwardContractOutstanding * avgForwardBookedRate;
            double d24 = unhedgedPosition * currentForwardRate;

            double profitAndLostFromCosting = 0;
            if (d21 != 0)
            {
                profitAndLostFromCosting = (d21 + d22) - (d23 + d24);
            }
            string strprofitLostUnit = DecimalPoint(profitAndLostFromCosting, decimalPoint);
            dr["Month" + i] = strprofitLostUnit;
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddProfitLossHedgedExposureRow_Import(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "P&L on Hedged exposure";
        for (int i = 1; i < dttbl.Columns.Count - 3; i++)
        {
            double importOrder = GlobalUtilities.ConvertToDouble(dttbl.Rows[1][i]);
            double tradeCredit = GlobalUtilities.ConvertToDouble(dttbl.Rows[2][i]);
            double forwardContractOutstanding = GlobalUtilities.ConvertToDouble(dttbl.Rows[3][i]);//d8
            double unhedgedPosition = GlobalUtilities.ConvertToDouble(dttbl.Rows[4][i]);
            double avgCostingRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[6][i]);
            double avgTradeCreditRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[7][i]);
            double currentForwardRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[9][i]);//d19
            double avgForwardBookedRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[8][i]);//d16

            double d21 = importOrder * avgCostingRate;
            double d22 = tradeCredit * avgTradeCreditRate;
            //double d23 = d21 * d22;Himesh changed this calc as below
            double d23 = forwardContractOutstanding * avgForwardBookedRate;

            double d30 = forwardContractOutstanding * currentForwardRate;
            double profitLostHedgedExposure = d30 - d23;//d22 + d23 - d30;

            string strprofitLostUnit = DecimalPoint(profitLostHedgedExposure, decimalPoint);
            dr["Month" + i] = strprofitLostUnit;
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void AddProfitLossUnHedgedExposureRow_Import(DataTable dttbl, bool isVisible, int decimalPoint)
    {
        DataRow dr = dttbl.NewRow();
        dr["Label"] = "P&L on Unhedged Exposure ";
        for (int i = 1; i < dttbl.Columns.Count - 3; i++)
        {
            double importOrder = GlobalUtilities.ConvertToDouble(dttbl.Rows[1][i]);
            double tradeCredit = GlobalUtilities.ConvertToDouble(dttbl.Rows[2][i]);
            double forwardContractOutstanding = GlobalUtilities.ConvertToDouble(dttbl.Rows[3][i]);
            double unhedgedPosition = GlobalUtilities.ConvertToDouble(dttbl.Rows[4][i]);
            double avgCostingRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[6][i]);
            double avgTradeCreditRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[7][i]);
            double avgForwardBookedRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[8][i]);
            double currentForwardRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[9][i]);

            //double d21 = importOrder * avgCostingRate;
            //double d22 = tradeCredit * avgTradeCreditRate;
            //double d23 = d21 * d22;
            //double d23 = forwardContractOutstanding * avgForwardBookedRate;

            double d24 = unhedgedPosition * currentForwardRate;
            double d33 = unhedgedPosition * avgCostingRate;
            double profitLostUnHedgedExposure = 0;

            if (d33 != 0)
            {
                profitLostUnHedgedExposure = d33 - d24;//d24 - d33; changed this calc on Sep 6, 2021, himesh emailed
            }

            string strprofitLostUnit = DecimalPoint(profitLostUnHedgedExposure, decimalPoint);
            dr["Month" + i] = strprofitLostUnit;
        }
        dr["IsVisible"] = isVisible;
        dr["DecimalPoint"] = decimalPoint;
        dttbl.Rows.Add(dr);
    }
    private void CalculateTotal_Import(DataTable dttbl, bool isVisible)
    {
        for (int i = 1; i < dttbl.Rows.Count; i++)
        {
            double total = 0;
            if (i == 9 || i == 10)
            {
                dttbl.Rows[i]["Total"] = "-";
                continue;
            }
            for (int j = 1; j <= 12; j++)
            {
                string data = Convert.ToString(dttbl.Rows[i][j]);
                if (i == 5)//%
                {
                    data = data.Replace("%", "");
                }
                total += GlobalUtilities.ConvertToDouble(data);
            }
            int decimalPoint = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["DecimalPoint"]);
            dttbl.Rows[i]["Total"] = DecimalPoint(total, decimalPoint);
        }
    }
    
}