using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class historical_data : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Custom.CheckSubscriptionAccess();
        Finstation.CheckFullFinstationAccess();

        if (!IsPostBack)
        {
            int currencyMasterId = Common.GetQueryStringValue("cmid");
            if (currencyMasterId == 0)
            {
                tdcurrency.Visible = true;
            }
            
            if (HistoryType == EnumFinstationHistoryType.RBIRefRate)
            {
                lbltitle.Text = "Historical Data - RBI Reference Rate";
            }
            else if (HistoryType == EnumFinstationHistoryType.MonthlyAvg)
            {
                lbltitle.Text = "Monthly Avg Rate";
                trdate.Visible = false;
                trmonth.Visible = true;
            }
            else if (HistoryType == EnumFinstationHistoryType.CashSpot)
            {
                lbltitle.Text = "Historical Data - Cash Spot Rate";
            }
            if (currencyMasterId > 0)
            {
                //ddlcurrency.SelectedValue = currency.ToString();
                string curreny = DbTable.GetOneColumnData("tbl_currencymaster", "currencymaster_currency", currencyMasterId - 1);
                lbltitle.Text += " - " + curreny;
            }
            
            string m = DateTime.Now.Month.ToString();
            if (m.Length == 1) m = "0" + m;
            string d = DateTime.Now.Day.ToString();
            if (d.Length == 1) d = "0" + d;
            txtfromdate.Text = "01-" + m + "-" + DateTime.Now.Year.ToString();
            txttodate.Text = d + "-" + m + "-" + DateTime.Now.Year.ToString();
        }
    }
    private EnumFinstationHistoryType HistoryType
    {
        get
        {
            int type =  Common.GetQueryStringValue("type");
            if (type == 0) type = 1;
            return (EnumFinstationHistoryType)type;
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
    private void BindData()
    {
        lblspotrate.Text = "";
        int currencyMasterId = Common.GetQueryStringValue("cmid");
        if (currencyMasterId == 0)
        {
            currencyMasterId = Convert.ToInt32(ddlcurrency.SelectedValue);
        }
        string currencyName = "";
        if (currencyMasterId == 0)
        {
            ltdata.Text = "<span class='error'>Please select currency!</span>";
            return;
        }
        if (currencyMasterId > 0)
        {
            currencyName = DbTable.GetOneColumnData("tbl_currencymaster", "currencymaster_currency", currencyMasterId - 1);
        }
        if (HistoryType == EnumFinstationHistoryType.MonthlyAvg)
        {
            txtfromdate.Text = "";
            txttodate.Text = "";
            if (txtfrommonth.Text != "") txtfromdate.Text = GlobalUtilities.ConvertToDate(Convert.ToDateTime(txtfrommonth.Text));
            if (txttomonth.Text != "")
            {
                DateTime dtTo = Convert.ToDateTime(txttomonth.Text);
                dtTo = new DateTime(dtTo.Year, dtTo.Month, GlobalUtilities.GetMonthEndDay(dtTo.Month, dtTo.Year));
                txttodate.Text = GlobalUtilities.ConvertToDate(dtTo);
            }
        }
        if (HistoryType == EnumFinstationHistoryType.SpotRate)
        {
            lblspotrate.Text = Custom.GetSportRate(currencyMasterId, true).ToString();
        }

        else if (HistoryType == EnumFinstationHistoryType.MonthlyAvg)
        {
            lbltitle.Text = "Monthly Avg Rate - " + currencyName;
        }
        else if (HistoryType == EnumFinstationHistoryType.CashSpot)
        {
            lbltitle.Text = "Cash Spot Rate - " + currencyName;
        }
        if (HistoryType == EnumFinstationHistoryType.CashSpot)
        {
            BindCashSpotHistory();
        }
        else
        {
            //currencyId = currencyId + 1;//tbl_historicaldata considers currency like INR,USD
            ltdata.Text = Finstation.GetHistoricalData(currencyMasterId, txtfromdate.Text, txttodate.Text, HistoryType);
        }
    }
    private void BindCashSpotHistory()
    {
        StringBuilder html = new StringBuilder();
        string query = "";
        string fromDate = txtfromdate.Text;
        string toDate = txttodate.Text;
        int currencyId = Convert.ToInt32(ddlcurrency.SelectedValue) - 1;
        query = "select * from tbl_dailyhistoricalliverate where 1=1";
        //query += " where dailyhistoricalliverate_currencyid=" + currencyId;
        if (fromDate != "") query += " AND cast(dailyhistoricalliverate_date as date)>=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(fromDate)) + "' as date)";
        if (toDate != "") query += " AND cast(dailyhistoricalliverate_date as date)<=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(toDate)) + "' as date)";
        query += @" order by dailyhistoricalliverate_date desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        html.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        html.Append(@"<tr class='grid-ui-header'><td>Date</td><td>Spot Rate BID(Export)</td><td>Spot Rate ASK(Import)</td>
                        <td>Cash Spot Rate BID(Export)</td><td>Cash Spot Rate ASK(Import)</td>
                        <td>Cash Rate BID(Export)</td><td>Cash Rate ASK(Import)</td></tr>");
        string prevDate = "";
        int counter = 0;
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (counter > 1000) break;
            string date = GlobalUtilities.ConvertToDate(dttbl.Rows[i]["dailyhistoricalliverate_date"]);
            if (date == prevDate) continue;
            string css = "grid-ui-alt";
            if (counter % 2 == 0) css = "grid-ui-row";
            int spotrateLiveRateId_export = Finstation.GetSpotRateLiveRateId(true, currencyId);
            int spotrateLiveRateId_import = Finstation.GetSpotRateLiveRateId(false, currencyId);
            int cashSpotrateLiveRateId_export = Finstation.GetCashSpotLiveRateId(true, currencyId);
            int cashSpotrateLiveRateId_import = Finstation.GetCashSpotLiveRateId(false, currencyId);
            double spotrate_export = GetCashSpotLiverateHistory(dttbl, spotrateLiveRateId_export, date);
            double spotrate_import = GetCashSpotLiverateHistory(dttbl, spotrateLiveRateId_import, date);
            double cashspotrate_export = GetCashSpotLiverateHistory(dttbl, cashSpotrateLiveRateId_export, date);
            double cashspotrate_import = GetCashSpotLiverateHistory(dttbl, cashSpotrateLiveRateId_import, date);
            if (currencyId == 3)//GBPINR
            {
                cashspotrate_export = cashspotrate_export / 100.0;
                cashspotrate_import = cashspotrate_import / 100.0;
            }
            double cashrate_export = spotrate_export - cashspotrate_export / 100.0;
            double cashrate_import = spotrate_import - cashspotrate_import / 100.0;
            html.Append("<tr class='" + css + "'>");
            html.Append("<td>" + date + "</td>");
            html.Append("<td>" + spotrate_export + "</td>");
            html.Append("<td>" + spotrate_import + "</td>");
            html.Append("<td>" + cashspotrate_export + "</td>");
            html.Append("<td>" + cashspotrate_import + "</td>");
            html.Append("<td>" + cashrate_export + "</td>");
            html.Append("<td>" + cashrate_import + "</td>");
            html.Append("</tr>");
            prevDate = date;
            counter++;
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
    private double GetCashSpotLiverateHistory(DataTable dttbl, int id, string date)
    {
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (GlobalUtilities.ConvertToDate(dttbl.Rows[i]["dailyhistoricalliverate_date"]) == date
                && id == GlobalUtilities.ConvertToInt(dttbl.Rows[i]["dailyhistoricalliverate_liverateid"]))
            {
                return GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["dailyhistoricalliverate_currentrate"]);
            }
        }
        return 0;
    }
}
