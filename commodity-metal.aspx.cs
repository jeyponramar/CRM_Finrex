using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;
using System.Collections;

public partial class commodity_metal : System.Web.UI.Page
{
    DataTable _dttblMetal = new DataTable();
    DataTable _dttblSettlementBidRate = new DataTable();
    string date = "";
    string sqldate = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Finstation.CheckMetalCommodityAccess();
            BindMetals();
            BindMetalLiverate();
            BindSettlementBid();
            BindStock();
            //BindAsiaRefPrice();
            //double exchangeRate = BindExchangeRates();
            //BindSterlingEquivalants(exchangeRate);
            BindSettlementDailyBid();
            BindSettlementMonthlyBid();
            //BindBombayExchangeRate();
        }
    }
    
    private void BindMetals()
    {
        string query = "select * from tbl_metal";
        _dttblMetal = DbTable.ExecuteSelect(query);
    }
    private void BindMetalLiverate()
    {
        string query = "";
        query = @"select *
                 from tbl_MetalLiveRate 
                 order by MetalLiveRate_metalid";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        StringBuilder lme3mSpreadHtml = new StringBuilder();
        html.Append("<table width='100%' cellspacing=0 cellpadding=0 class='repeater' border=1>");
        html.Append(@"<tr class='repeater-header'><td>METAL</td><td width='60px' class='hidden'>TIME</td><td>BID</td><td>ASK</td><td>OPEN</td>
                    <td>HIGH</td><td>LOW</td><td>P.CLOSE</td><td>CHANGE</td><td>CHANGE%</td>
                    <td>Trading Volumes</td><td>Trading Value</td>
                    </tr>");
        lme3mSpreadHtml.Append("<table width='100%' cellspacing=0 cellpadding=0 class='repeater' border=1>");
        lme3mSpreadHtml.Append(@"<tr class='repeater-header'><td>METAL</td><td>LTP</td><td>BID</td><td>ASK</td><td>CLOSE</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            DataRow dr = dttbl.Rows[i];
            int metalId = GlobalUtilities.ConvertToInt(dr["MetalLiveRate_metalid"]);
            if (metalId == 7) continue;//ALUMINALLOY
            string metalName = GetMetalName(metalId);
            string time = GlobalUtilities.ConvertToDateTime(dr["metalliverate_date"]);
            double bid = GlobalUtilities.ConvertToDouble(dr["metalliverate_bid"]);
            double ask = GlobalUtilities.ConvertToDouble(dr["metalliverate_ask"]);
            double open = GlobalUtilities.ConvertToDouble(dr["metalliverate_open"]);
            double high = GlobalUtilities.ConvertToDouble(dr["metalliverate_high"]);
            double low = GlobalUtilities.ConvertToDouble(dr["metalliverate_low"]);
            double prevclose = GlobalUtilities.ConvertToDouble(dr["metalliverate_prevclose"]);
            double change = GlobalUtilities.ConvertToDouble(dr["metalliverate_change"]);
            double changeper = GlobalUtilities.ConvertToDouble(dr["metalliverate_changeper"]);
            //string expirydate = GlobalUtilities.ConvertToDate(dr["metalliverate_expirydate"]);
            double tradingvolume = GlobalUtilities.ConvertToDouble(dr["metalliverate_tradingvolume"]);
            double tradingvalue = GlobalUtilities.ConvertToDouble(dr["metalliverate_tradingvalue"]);

            double threemcashspead_bid = GlobalUtilities.ConvertToDouble(dr["metalliverate_3mcashspreadbid"]);
            double threemcashspead_ask = GlobalUtilities.ConvertToDouble(dr["metalliverate_3mcashspreadask"]);
            double threemcashspead_close = GlobalUtilities.ConvertToDouble(dr["metalliverate_3mcashspreadclose"]);
            double threemcashspead_ltp = GlobalUtilities.ConvertToDouble(dr["metalliverate_3mcashspreadltp"]);

            html.Append("<tr>");
            html.Append("<td class='repeater-header-left'>" + metalName + "</td>");
            html.Append("<td class='rate-padding comm-liverate nowrap hidden' metalid='" + metalId + "' col='date'>" + time + "</td>");
            html.Append("<td><div class='rate comm-liverate' col='bid' metalid='" + metalId + "'>" + bid + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='ask' metalid='" + metalId + "'>" + ask + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='open' metalid='" + metalId + "'>" + open + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='high' metalid='" + metalId + "'>" + high + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='low' metalid='" + metalId + "'>" + low + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='prevclose' metalid='" + metalId + "'>" + prevclose + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='change' metalid='" + metalId + "'>" + change + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='changeper' metalid='" + metalId + "'>" + changeper + "</div></td>");

            //html.Append("<td><div class='rate'>" + expirydate + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='tradingvolume' metalid='" + metalId + "'>" + tradingvolume + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='tradingvalue' metalid='" + metalId + "'>" + tradingvalue + "</div></td>");
            html.Append("</tr>");

            lme3mSpreadHtml.Append("<tr>");
            lme3mSpreadHtml.Append("<td class='repeater-header-left'>" + metalName + "</td>");
            lme3mSpreadHtml.Append("<td><div class='rate comm-liverate' col='3mcashspreadltp' metalid='" + metalId + "'>" + threemcashspead_ltp + "</div></td>");
            lme3mSpreadHtml.Append("<td><div class='rate comm-liverate' col='3mcashspreadbid' metalid='" + metalId + "'>" + threemcashspead_bid + "</div></td>");
            lme3mSpreadHtml.Append("<td><div class='rate comm-liverate' col='3mcashspreadask' metalid='" + metalId + "'>" + threemcashspead_ask + "</div></td>");
            lme3mSpreadHtml.Append("<td><div class='rate comm-liverate' col='3mcashspreadclose' metalid='" + metalId + "'>" + threemcashspead_close + "</div></td>");
            lme3mSpreadHtml.Append("</tr>");

        }
        html.Append("</table>");
        lme3mSpreadHtml.Append("</table>");
        ltliverate.Text = html.ToString();
        ltlme3mcashspread.Text = lme3mSpreadHtml.ToString();
    }
    private void BindMetalLiverate1()
    {
        string query = "";
        query = @"select *
                 from tbl_MetalLiveRate 
                 order by MetalLiveRate_metalid";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%' cellspacing=0 cellpadding=0 class='repeater' border=1>");
        html.Append(@"<tr class='repeater-header'><td>METAL</td><td width='60px'>TIME</td><td>BID</td><td>ASK</td><td>OPEN</td>
                    <td>HIGH</td><td>LOW</td><td>P.CLOSE</td><td>CHANGE</td><td>CHANGE%</td>
                    <td>1W % Change</td><td>1M % Change</td><td>3M % Change</td><td>1YR % Change</td><td>Ct Week High</td>
                    <td>Ct Week Low</td><td>Ct Month High</td><td>Ct Month Low</td><td>52W High</td><td>52W Low</td>
                    </tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            DataRow dr = dttbl.Rows[i];
            int metalId = GlobalUtilities.ConvertToInt(dr["MetalLiveRate_metalid"]);
            string metalName = GetMetalName(metalId);
            string time = GlobalUtilities.ConvertToDateTime(dr["metalliverate_date"]);
            double bid = GlobalUtilities.ConvertToDouble(dr["metalliverate_bid"]);
            double ask = GlobalUtilities.ConvertToDouble(dr["metalliverate_ask"]);
            double open = GlobalUtilities.ConvertToDouble(dr["metalliverate_open"]);
            double high = GlobalUtilities.ConvertToDouble(dr["metalliverate_high"]);
            double low = GlobalUtilities.ConvertToDouble(dr["metalliverate_low"]);
            double prevclose = GlobalUtilities.ConvertToDouble(dr["metalliverate_prevclose"]);
            double change = GlobalUtilities.ConvertToDouble(dr["metalliverate_change"]);
            double changeper = GlobalUtilities.ConvertToDouble(dr["metalliverate_changeper"]);
            double oneweekchangeper = GlobalUtilities.ConvertToDouble(dr["metalliverate_oneweekchangeper"]);
            double onemonthchangeper = GlobalUtilities.ConvertToDouble(dr["metalliverate_onemonthchangeper"]);
            double threemonthchangeper = GlobalUtilities.ConvertToDouble(dr["metalliverate_threemonthchangeper"]);
            double oneyearchangeper = GlobalUtilities.ConvertToDouble(dr["metalliverate_oneyearchangeper"]);
            double ctweekhigh = GlobalUtilities.ConvertToDouble(dr["metalliverate_ctweekhigh"]);
            double ctweeklow = GlobalUtilities.ConvertToDouble(dr["metalliverate_ctweeklow"]);
            double ctmonthhigh = GlobalUtilities.ConvertToDouble(dr["metalliverate_ctmonthhigh"]);
            double ctmonthlow = GlobalUtilities.ConvertToDouble(dr["metalliverate_ctmonthlow"]);
            double fiftyweekhigh = GlobalUtilities.ConvertToDouble(dr["metalliverate_fiftytwoweekhigh"]);
            double fiftyweeklow = GlobalUtilities.ConvertToDouble(dr["metalliverate_fiftytwoweeklow"]);
            html.Append("<tr>");
            html.Append("<td class='repeater-header-left'>" + metalName + "</td>");
            html.Append("<td class='rate-padding comm-liverate nowrap' metalid='" + metalId + "' col='date'>" + time + "</td>");
            html.Append("<td><div class='rate comm-liverate' col='bid' metalid='" + metalId + "'>" + bid + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='ask' metalid='" + metalId + "'>" + ask + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='open' metalid='" + metalId + "'>" + open + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='high' metalid='" + metalId + "'>" + high + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='low' metalid='" + metalId + "'>" + low + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='prevclose' metalid='" + metalId + "'>" + prevclose + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='change' metalid='" + metalId + "'>" + change + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='changeper' metalid='" + metalId + "'>" + changeper + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='oneweekchangeper' metalid='" + metalId + "'>" + oneweekchangeper + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='onemonthchangeper' metalid='" + metalId + "'>" + onemonthchangeper + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='threemonthchangeper' metalid='" + metalId + "'>" + threemonthchangeper + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='oneyearchangeper' metalid='" + metalId + "'>" + oneyearchangeper + "</div></td>");
            html.Append("<td><div class='rate'>" + ctweekhigh + "</div></td>");
            html.Append("<td><div class='rate'>" + ctweeklow + "</div></td>");
            html.Append("<td><div class='rate'>" + ctmonthhigh + "</div></td>");
            html.Append("<td><div class='rate'>" + ctmonthlow + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='fiftytwoweekhigh' metalid='" + metalId + "'>" + fiftyweekhigh + "</div></td>");
            html.Append("<td><div class='rate comm-liverate' col='fiftytwoweeklow' metalid='" + metalId + "'>" + fiftyweeklow + "</div></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        ltliverate.Text = html.ToString();
    }
    private void BindBombayExchangeRate()
    {
        string query = "";
        StringBuilder html = new StringBuilder();
        query = @"select top 1 * from tbl_bombaymetalexchangerate order by bombaymetalexchangerate_date desc";
        string sqldate1 = "";
        string date1 = "";
        DataRow drLast = DbTable.ExecuteSelectRow(query);
        if (drLast == null)
        {
            sqldate1 = GlobalUtilities.ConvertToSqlDate(DateTime.Now);
            date1 = GlobalUtilities.ConvertToDateMMM(DateTime.Now);
        }
        else
        {
            sqldate1 = GlobalUtilities.ConvertToSqlDate(Convert.ToDateTime(drLast["bombaymetalexchangerate_date"]));
            date1 = GlobalUtilities.ConvertToDateMMM(Convert.ToDateTime(drLast["bombaymetalexchangerate_date"]));
        }
        query = @"select * from tbl_bombaymetalexchangerate where cast(bombaymetalexchangerate_date as date)=cast('" + sqldate1 + "' as date)";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        html.Append("<table width='100%' cellspacing=0 cellpadding=0>");
        html.Append("<tr><td>" + date1 + "</td><td align='right'>Rs. Per KG</td></tr>");
        html.Append("<tr><td colspan='2'>");
        html.Append("<table width='100%' cellspacing=0 cellpadding=0 class='repeater' border=1>");
        html.Append(@"<tr class='repeater-header'><td>Metal</td><td>Description</td><td>Today</td><td>Prev. Day</td><td>Year Ago</td><td>Month Ago</td>
                            <td>Change%</td><td>Month Change%</td><td>Year Change%</td>
                    </tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            html.Append("<tr>");
            html.Append("<td class='repeater-header-left nowrap'>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["bombaymetalexchangerate_metalname"]) + "</td>");
            html.Append("<td class='repeater-header-left nowrap'>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["bombaymetalexchangerate_metaldescription"]) + "</td>");

            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["bombaymetalexchangerate_todayrate"]) / 100000, 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["bombaymetalexchangerate_prevdayrate"]) / 100000, 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["bombaymetalexchangerate_yearagorate"]) / 100000, 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["bombaymetalexchangerate_monthagorate"]) / 100000, 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["bombaymetalexchangerate_changepercentage"]), 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["bombaymetalexchangerate_monthchangepercentage"]), 2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["bombaymetalexchangerate_yearchangepercentage"]), 2) + "</div></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        html.Append("</td></tr></table>");
        //ltbombayexchangerate.Text = html.ToString();
    }
    private void BindSettlementBid()
    {
        Commodity obj = new Commodity();
        ltlmesettlement.Text = obj.GetLMESettlementRateHtml("");
    }
    private void BindSettlementBid1()
    {
        string query = "";
        query = @"select top 1 * from tbl_DailyLMEMetalRate where DailyLMEMetalRate_isactive=1 order by DailyLMEMetalRate_date desc";
        DataRow drLast = DbTable.ExecuteSelectRow(query);
        if (drLast == null)
        {
            sqldate = GlobalUtilities.ConvertToSqlDate(DateTime.Now);
            date = GlobalUtilities.ConvertToDateMMM(DateTime.Now);
        }
        else
        {
            sqldate = GlobalUtilities.ConvertToSqlDate(Convert.ToDateTime(drLast["DailyLMEMetalRate_date"]));
            date = GlobalUtilities.ConvertToDateMMM(Convert.ToDateTime(drLast["DailyLMEMetalRate_date"]));
        }
        query = @"select * from tbl_DailyLMEMetalRate where DailyLMEMetalRate_isactive=1 and cast(DailyLMEMetalRate_date as date)=cast('" + sqldate + "' as date)";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        _dttblSettlementBidRate = dttbl;
        StringBuilder html = new StringBuilder();
        string nextYear = "Dec-"+Convert.ToString(DateTime.Now.Year + 1).Substring(2);
        html.Append("<table width='100%' cellspacing=0 cellpadding=0>");
        html.Append("<tr><td>" + date + "</td></tr>");
        html.Append("<tr><td>");
        html.Append("<table width='100%' cellspacing=0 cellpadding=0 class='repeater' border=1>");
        html.Append(@"<tr class='repeater-header'><td>METAL</td><td>Cash</td><td class='nowrap'>3-months</td><td class='nowrap'>" + nextYear + @"</td>
                    </tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            DataRow dr = dttbl.Rows[i];
            int metalId = GlobalUtilities.ConvertToInt(dr["DailyLMEMetalRate_metalid"]);
            string metalName = GetMetalName(metalId);
            double cash = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_cash"]);
            double threemonths = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_threemonths"]);
            double oneyear = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_oneyear"]);
            
            html.Append("<tr>");
            html.Append("<td class='repeater-header-left'>" + metalName + "</td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(cash,2) + "</div></td>");
            html.Append("<td><div class='rate'>" +  ExportExposurePortal.DecimalPoint(threemonths,2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(oneyear, 2) + "</div></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        html.Append("</td></tr></table>");
        ltlmesettlement.Text = html.ToString();
    }
    private void BindStock()
    {
        Commodity obj = new Commodity();
        ltstock.Text = obj.GetLMEWarehouseStockHtml("");
    }
    private void BindAsiaRefPrice()
    {
        StringBuilder html = new StringBuilder();
        string nextYear = "Dec-" + Convert.ToString(DateTime.Now.Year + 1).Substring(2);
        html.Append("<table width='100%' cellspacing=0 cellpadding=0>");
        html.Append("<tr><td>" + date + " <div style='float:right;'>3-months ABR</div></td></tr>");
        html.Append("<tr><td>");
        html.Append("<table width='100%' cellspacing=0 cellpadding=0 class='repeater' border=1>");
        html.Append(@"<tr class='repeater-header'><td>METAL</td><td>PRICE</td>
                      </tr>");
        for (int i = 0; i < _dttblSettlementBidRate.Rows.Count; i++)
        {
            DataRow dr = _dttblSettlementBidRate.Rows[i];
            int metalId = GlobalUtilities.ConvertToInt(dr["DailyLMEMetalRate_metalid"]);
            string metalName = GetMetalName(metalId);
            string contract = GlobalUtilities.ConvertToString(dr["DailyLMEMetalRate_asiancontract"]);
            double rate = GlobalUtilities.ConvertToDouble(dr["DailyLMEMetalRate_asianrate"]);
            
            html.Append("<tr>");
            html.Append("<td class='repeater-header-left'>" + metalName + "</td>");
            //html.Append("<td>" + contract + "</td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(rate ,2)+ "</div></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        html.Append("</td></tr></table>");
        //ltasianrefprice.Text = html.ToString();
        
    }
    private double BindExchangeRates()
    {
        double exchangeRate = 0;
        string query = "";
        query = @"select top 1 * from tbl_MetalCurrencyRate order by MetalCurrencyRate_date desc";
        DataRow drLast = DbTable.ExecuteSelectRow(query);
        string sqldate = "";
        string date = "";
        if (drLast == null)
        {
            sqldate = GlobalUtilities.ConvertToSqlDate(DateTime.Now);
            date = GlobalUtilities.ConvertToDateMMM(DateTime.Now);
        }
        else
        {
            sqldate = GlobalUtilities.ConvertToSqlDate(Convert.ToDateTime(drLast["MetalCurrencyRate_date"]));
            date = GlobalUtilities.ConvertToDateMMM(Convert.ToDateTime(drLast["MetalCurrencyRate_date"]));
        }
        
        query = @"select * from tbl_MetalCurrencyRate
                join tbl_MetalCurrency on MetalCurrency_MetalCurrencyid=MetalCurrencyRate_MetalCurrencyid 
                where cast(MetalCurrencyRate_date as date)=cast('" + sqldate + @"' as date)
                and metalcurrencyrate_metalid=1
                order by MetalCurrency_MetalCurrencyid";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        string nextYear = "Dec-" + Convert.ToString(DateTime.Now.Year + 1).Substring(2);
        html.Append("<table width='100%' cellspacing=0 cellpadding=0>");
        html.Append("<tr><td>" + date + "</td></tr>");
        html.Append("<tr><td>");
        html.Append("<table width='100%' cellspacing=0 cellpadding=0 class='repeater' border=1>");
        html.Append(@"<tr class='repeater-header'><td>CURRENCY</td><td>EXCHANGE RATE</td>
                      </tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            DataRow dr = dttbl.Rows[i];
            string currencyName = GlobalUtilities.ConvertToString(dr["MetalCurrency_currencyname"]);
            double rate = GlobalUtilities.ConvertToDouble(ExportExposurePortal.DecimalPoint(dr["MetalCurrencyRate_rate"],4));
            if (i == 0) exchangeRate = rate;
            html.Append("<tr>");
            html.Append("<td class='repeater-header-left'>" + currencyName + "</td>");
            html.Append("<td><div class='rate'>" + rate + "</div></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        html.Append("</td></tr></table>");
        //ltlmeexchangerate.Text = html.ToString();
        return exchangeRate;
    }
    private void BindSterlingEquivalants(double exchangeRate)
    {
        StringBuilder html = new StringBuilder();
        string nextYear = "Dec-" + Convert.ToString(DateTime.Now.Year + 1).Substring(2);
        html.Append("<table width='100%' cellspacing=0 cellpadding=0>");
        html.Append("<tr><td>" + date + "</td></tr>");
        html.Append("<tr><td>");
        html.Append("<table width='100%' cellspacing=0 cellpadding=0 class='repeater' border=1>");
        html.Append(@"<tr class='repeater-header'><td>METAL</td><td>Cash</td><td class='nowrap'>3-months</td>
                      </tr>");
        for (int i = 0; i < _dttblMetal.Rows.Count; i++)
        {
            DataRow dr = _dttblMetal.Rows[i];
            int metalId = GlobalUtilities.ConvertToInt(dr["metal_metalid"]);
            string metalName = GlobalUtilities.ConvertToString(dr["metal_metalname"]);
            double cash = 0;
            double threemonths = 0;
            for (int j = 0; j < _dttblSettlementBidRate.Rows.Count; j++)
            {
                if (GlobalUtilities.ConvertToInt(_dttblSettlementBidRate.Rows[j]["DailyLMEMetalRate_metalid"]) == metalId)
                {
                    cash = GlobalUtilities.ConvertToDouble(_dttblSettlementBidRate.Rows[j]["DailyLMEMetalRate_cash"]);
                    threemonths = GlobalUtilities.ConvertToDouble(_dttblSettlementBidRate.Rows[j]["DailyLMEMetalRate_threemonths"]);
                    if (exchangeRate == 0)
                    {
                        cash = 0;
                        threemonths = 0;
                    }
                    else
                    {
                        cash = cash / exchangeRate;
                        threemonths = threemonths / exchangeRate;
                    }
                    break;
                }
            }
            html.Append("<tr>");
            html.Append("<td class='repeater-header-left'>" + metalName + "</td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(cash,2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(threemonths,2) + "</div></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        html.Append("</td></tr></table>");
        //ltsterling.Text = html.ToString();
    }
    private void BindSettlementDailyBid()
    {
        string query = "";
        StringBuilder html = new StringBuilder();
        int metalCount = _dttblMetal.Rows.Count;
        int top = metalCount * 10;//10days
        query = @"select top 10 * from tbl_DailyLMEMetalRate
                 where DailyLMEMetalRate_metalid=1 and DailyLMEMetalRate_isactive=1
                 order by DailyLMEMetalRate_date desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        string date = "";
        html.Append("<table width='100%' cellspacing=0 cellpadding=0 class='repeater' border=1>");
        html.Append("<tr class='repeater-header'><td></td>");
        for (int i = 0; i < _dttblMetal.Rows.Count; i++)
        {
            html.Append("<td colspan=2>" + GlobalUtilities.ConvertToString(_dttblMetal.Rows[i]["metal_metalname"]) + "</td>");
        }
        html.Append("<td colspan=2>USDINR</td></tr>");
        html.Append(@"<tr class='comm-sheader'><td>DATE</td>");
        for (int i = 0; i < _dttblMetal.Rows.Count; i++)
        {
            html.Append("<td>CASH</td><td>3-MONTH</td>");
        }
        html.Append("<td>CLOSING RATE</td><td>RBI REF RATE</td>");
        html.Append("</tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            date = GlobalUtilities.ConvertToDate(dttbl.Rows[i]["DailyLMEMetalRate_date"]);
            string sqldate = GlobalUtilities.ConvertToSqlDate(Convert.ToDateTime(dttbl.Rows[i]["DailyLMEMetalRate_date"]));
            query = @"select * from tbl_DailyLMEMetalRate
                       where DailyLMEMetalRate_isactive=1 and cast(DailyLMEMetalRate_date as date)=cast('" + sqldate + "' as date)";
            DataTable dttblrate = DbTable.ExecuteSelect(query);
//            query = @"select * from tbl_historicaldata
//                       where cast(historicaldata_date as date)=cast('" + sqldate + "' as date)";
//            DataRow drhistory = DbTable.ExecuteSelectRow(query);
            html.Append("<tr>");
            html.Append("<td class='repeater-header-left'>" + date + "</td>");
            for (int j = 0; j < _dttblMetal.Rows.Count; j++)
            {
                int metalid = GlobalUtilities.ConvertToInt(_dttblMetal.Rows[j]["metal_metalid"]);
                double cash = 0;
                double threemonths = 0;
                for (int k = 0; k < dttblrate.Rows.Count; k++)
                {
                    if (GlobalUtilities.ConvertToInt(dttblrate.Rows[k]["DailyLMEMetalRate_metalid"]) == metalid &&
                        GlobalUtilities.ConvertToDate(dttblrate.Rows[k]["DailyLMEMetalRate_date"]) == date)
                    {
                        cash = GlobalUtilities.ConvertToDouble(dttblrate.Rows[k]["DailyLMEMetalRate_cashask"]);
                        threemonths = GlobalUtilities.ConvertToDouble(dttblrate.Rows[k]["DailyLMEMetalRate_threemonthsask"]);
                    }
                }
                html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(cash,2) + "</div></td>");
                html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(threemonths,2) + "</div></td>");
            }
            double usdclose = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["DailyLMEMetalRate_usdinrclose"]);
            double usdrbiref = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["DailyLMEMetalRate_usdinrrbirefrate"]);
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(usdclose,2) + "</div></td>");
            html.Append("<td><div class='rate'>" + ExportExposurePortal.DecimalPoint(usdrbiref, 2) + "</div></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");

        ltsettlementratedaily.Text = html.ToString();
    }
    private void BindSettlementMonthlyBid()
    {
        string query = "";
        StringBuilder html = new StringBuilder();
        int metalCount = _dttblMetal.Rows.Count;
        int top = metalCount * 10;//10days
        query = @"select top 10 * from tbl_MonthlyLMEMetalRate
                 where MonthlyLMEMetalRate_metalid=1
                 order by MonthlyLMEMetalRate_date desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        string date = "";
        html.Append("<table width='100%' cellspacing=0 cellpadding=0 class='repeater' border=1>");
        html.Append("<tr class='repeater-header'><td></td>");
        for (int i = 0; i < _dttblMetal.Rows.Count; i++)
        {
            html.Append("<td colspan=2>" + GlobalUtilities.ConvertToString(_dttblMetal.Rows[i]["metal_metalname"]) + "</td>");
        }
        html.Append("<td colspan=2>USDINR</td></tr>");
        html.Append(@"<tr class='comm-sheader'><td>DATE</td>");
        for (int i = 0; i < _dttblMetal.Rows.Count; i++)
        {
            html.Append("<td>CASH</td><td>3-MONTH</td>");
        }
        html.Append("<td>CLOSING RATE</td><td>RBI REF RATE</td>");
        html.Append("</tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            date = GlobalUtilities.ConvertToDate(dttbl.Rows[i]["MonthlyLMEMetalRate_date"]);
            DateTime dt = Convert.ToDateTime(dttbl.Rows[i]["MonthlyLMEMetalRate_date"]);
            string year = dt.Year.ToString().Substring(2);
            string m = GlobalUtilities.GetMonthShortName(dt.Month);
            string month = m + "-" + year;
            string sqldate = GlobalUtilities.ConvertToSqlDate(Convert.ToDateTime(dttbl.Rows[i]["MonthlyLMEMetalRate_date"]));
            query = @"select * from tbl_MonthlyLMEMetalRate
                       where cast(MonthlyLMEMetalRate_date as date)=cast('" + sqldate + "' as date)";
            DataTable dttblrate = DbTable.ExecuteSelect(query);
//            query = @"select * from tbl_historicaldata
//                       where cast(historicaldata_date as date)=cast('" + sqldate + "' as date)";
//            DataRow drhistory = DbTable.ExecuteSelectRow(query);
            html.Append("<tr>");
            html.Append("<td class='repeater-header-left'>" + month + "</td>");
            for (int j = 0; j < _dttblMetal.Rows.Count; j++)
            {
                int metalid = GlobalUtilities.ConvertToInt(_dttblMetal.Rows[j]["metal_metalid"]);
                double cash = 0;
                double threemonths = 0;
                for (int k = 0; k < dttblrate.Rows.Count; k++)
                {
                    if (GlobalUtilities.ConvertToInt(dttblrate.Rows[k]["MonthlyLMEMetalRate_metalid"]) == metalid &&
                        GlobalUtilities.ConvertToDate(dttblrate.Rows[k]["MonthlyLMEMetalRate_date"]) == date)
                    {
                        cash = GlobalUtilities.ConvertToDouble(dttblrate.Rows[k]["MonthlyLMEMetalRate_cash"]);
                        threemonths = GlobalUtilities.ConvertToDouble(dttblrate.Rows[k]["MonthlyLMEMetalRate_threemonths"]);
                    }
                }
                html.Append("<td><div class='rate'>" + cash + "</div></td>");
                html.Append("<td><div class='rate'>" + threemonths + "</div></td>");
            }
            double usdclose = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["MonthlyLMEMetalRate_usdinrclose"]);
            double usdrbiref = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["MonthlyLMEMetalRate_usdinrrbirefrate"]);
            html.Append("<td><div class='rate'>" + usdclose + "</div></td>");
            html.Append("<td><div class='rate'>" + usdrbiref + "</div></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");

        ltsettlementratemonthly.Text = html.ToString();
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
}
