using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using WebComponent;
using System.Text;
using System.Configuration;

public partial class index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Finstation obj = new Finstation();
        //double d = obj.GetLiveRateCalc(1015);
        if (!IsPostBack)
        {
            if (GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId")) > 0 && (Finstation.IsFinstationEnabled() 
                || Finstation.IsMiniFinstationEnabled()))
            {
            }
            else
            {
                Response.Redirect("~/customerlogin.aspx");
            }
            ViewState["Currency"] = Currency.USDINR;
            Finstation objfinstation = new Finstation();
            int clientUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
            string spotRateIncludeCurrencies = objfinstation.GetUserConfigLiverateCurrencies(Enum_AppType.Finstation, 1, clientUserId);
            string crossCurrencies = objfinstation.GetUserConfigLiverateCurrencies(Enum_AppType.Finstation, 2, clientUserId);
            string offshoreCurrencies = objfinstation.GetUserConfigLiverateCurrencies(Enum_AppType.Finstation, 3, clientUserId);
            BindRate(1, ltSpotRate, "", "", "", 6, "", spotRateIncludeCurrencies);
            BindRate(50, ltOffshore, "Bid,High,Low", "", "3,4,5", 6, "", offshoreCurrencies);//1,5,6
            //BindRate(4, ltIndices);
            //BindRate(58, ltcommodityindices, "");
            BindIndicesAndCommodities();

            //BindRate(2, ltCrossCurrencies);
            //BindRate(26, ltLiborRates);

            BindRate(2, ltCrossCurrencies, "", "6", "", 3, "", crossCurrencies);//"83,84,85,86,87,90,91,92");//exclude USDCHF

            BindRate(5, ltUSDINRPreminumMonthEnd, false, true);
            BindRate(6, ltUSDINROutrightRate, true, true);
            BindRate(7, ltUSDINRAnnualisedPremium,true,false);
            //ltUSDINRAnnualisedPremium.Text = ltUSDINRAnnualisedPremium.Text.Replace("<table class='repeater' border=1 style='width:100%;'>", "");
            //BindRate(27, ltUSDINRPremium_Monthwise);


            BindRate(28, ltUSDINROutrightRate_Monthwise);
            BindRate(29, ltUSDINRAnnualised_Monthwise);

            //Finstation.BindCurrency(ltUSDINR, 30);
            //BindCurrencies();
            //bind commodity related
            //BindRate(57, ltglobalindices, "LTP,Change,%Change");
            BindRate(61, ltglobalindices, "LTP,Change,% Chg,Open,High,Low","7","1",3);

            //BindRate(58, ltcommodityindices, "LTP,Change,%Change");
            
            BindFPIInvestment();
            BindSpotDate();
        }
        if (!AppConstants.IsLive)
        {
            //divCalculate.Visible = true;
        }
        //FinstationPortal obj = new FinstationPortal();
        //DataTable dttbl = obj.GetRateDataTable(61);
    }
    //private void BindCurrencies()
    //{
    //    Finstation.BindCurrency(ltUSDINR, 30);
    //    Finstation.BindCurrency(ltEURINR, 32);
    //    Finstation.BindCurrency(ltGBPINR, 34);
    //    Finstation.BindCurrency(ltJPYINR, 36);
    //    Finstation.BindCurrency(ltEURUSD, 38);
    //    Finstation.BindCurrency(ltGBPUSD, 40);
    //    Finstation.BindCurrency(ltUSDJPY, 42);
    //}
    private void BindSpotDate()
    {
        string query = "";
        query = "select * from tbl_liverate where liverate_liverateid=1029";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        ltspotdate.Text = "<span id='jq-spotdate' rid='"+GlobalUtilities.ConvertToString(dr["liverate_liverateid"])+"'>" + GlobalUtilities.ConvertToString(dr["liverate_currentrate"]) + "</span>";
    }
    private void BindRate(int sectionId, Literal lt)
    {
        BindRate(sectionId, lt, false, false);
    }
    private void BindRate(int sectionId, Literal lt, bool isExcludeTableStart, bool isExcludeTableEnd)
    {
        BindRate(sectionId, lt, "");
        if (isExcludeTableStart) lt.Text = lt.Text.Replace("<table class='repeater' border=1 style='width:100%;'>", "");
        if (isExcludeTableEnd) lt.Text = lt.Text.Replace("</table>", "");
    }
    private void BindRate(int sectionId, Literal lt, string columns)
    {
        BindRate(sectionId, lt, columns, "");
    }
    private void BindRate(int sectionId, Literal lt, string columns, string extrawhere)
    {
        BindRate(sectionId, lt, columns, extrawhere, "");
    }
    private void BindRate(int sectionId, Literal lt, string columns, string extrawhere, string excludecols)
    {
        BindRate(sectionId, lt, columns, extrawhere, excludecols, 0);
    }
    private void BindRate(int sectionId, Literal lt, string columns, string extrawhere, string excludecols, int showMoreIndex)
    {
        BindRate(sectionId, lt, columns, extrawhere, excludecols, showMoreIndex, "", "", false);
    }
    private void BindRate(int sectionId, Literal lt, string columns, string extrawhere, string excludecols, int showMoreIndex, string includeRows,
        string includeCurrencies)
    {
        BindRate(sectionId, lt, columns, extrawhere, excludecols, showMoreIndex, includeRows, includeCurrencies, false);
    }
    private void BindRate(int sectionId, Literal lt, string columns, string excludeRows, string excludecols, int showMoreIndex, string includeRows,
        string includeCurrencies, bool isShowCountry)
    {
        FinstationPortal obj = new FinstationPortal();
        obj.BindRate(sectionId, lt, columns, excludeRows, excludecols, showMoreIndex, includeRows, includeCurrencies, isShowCountry);
    }
    protected void btnMajor_Click(object sender, EventArgs e)
    {
        btnAsia.CssClass = "btncurrency";
        btnMajor.CssClass = "btncurrency-active";
        //BindRate(2, ltCrossCurrencies, "Bid,Ask,% Chg");
        BindRate(2, ltCrossCurrencies, "", "6", "", 3, "", "83,84,85,86,87,90,91,92");//exclude USDCHF
    }
    protected void btnAsia_Click(object sender, EventArgs e)
    {
        btnAsia.CssClass = "btncurrency-active";
        btnMajor.CssClass = "btncurrency";
        //BindRate(3, ltCrossCurrencies, "Bid,Ask,% Chg");
        BindRate(3, ltCrossCurrencies, "", "6", "", 3);//exclude USDCHF
    }
    
    protected void btnUSDINRMonthEnd_Click(object sender, EventArgs e)
    {
        btnUSDINRMonthwise.CssClass = "btncurrency";
        btnUSDINRMonthEnd.CssClass = "btncurrency-active";
        BindRate(5, ltUSDINRPreminumMonthEnd);
        trUSDINRMonthwise.Visible = false;
        trUSDINTMonthEnd.Visible = true;
    }
    
    protected void btnEURINR_Click(object sender, EventArgs e)
    {
        ViewState["Currency"] = Currency.EURINR;
        ResetButton();
        trforwardrateTab.Visible = true;
        btnEURINR.CssClass = "btncurrency-active";

        btnForwardRateMonthEnd_Click(sender, e);
    }
    protected void btnGBPINR_Click(object sender, EventArgs e)
    {
        ViewState["Currency"] = Currency.GBPINR;
        ResetButton();
        btnGBPINR.CssClass = "btncurrency-active";
        trforwardrateTab.Visible = true;
        btnForwardRateMonthEnd_Click(sender, e);
    }
    protected void btnJPYINR_Click(object sender, EventArgs e)
    {
        ResetButton();
        btnJPYINR.CssClass = "btncurrency-active";
        BindRate(14, ltJPYINRPremium);
        BindRate(15, ltJPYINROutrightRate);
        BindRate(16, ltJPYINRAnnualisedPremium);
        trJPYINR.Visible = true;

        //Finstation.BindCurrency(ltJPYINR, 36);
    }
    protected void btnEURUSD_Click(object sender, EventArgs e)
    {
        ResetButton();
        btnEURUSD.CssClass = "btncurrency-active";
        BindRate(17, ltEURUSDPremium);
        BindRate(18, ltEURUSDOutrightRate);
        BindRate(19, ltEURUSDAnnualisedPremium);
        trEURUSD.Visible = true;
        //Finstation.BindCurrency(ltEURUSD, 38);
    }
    protected void btnGBPUSD_Click(object sender, EventArgs e)
    {
        ResetButton();
        btnGBPUSD.CssClass = "btncurrency-active";
        BindRate(20, ltGBPUSDPremium);
        BindRate(21, ltGBPUSDOutrightRate);
        BindRate(22, ltGBPUSDAnnualisedPremium);
        trGBPUSD.Visible = true;
        //Finstation.BindCurrency(ltGBPUSD, 40);
    }
    protected void btnUSDJPY_Click(object sender, EventArgs e)
    {
        ResetButton();
        btnUSDJPY.CssClass = "btncurrency-active";
        BindRate(23, ltUSDJPYPremium);
        BindRate(24, ltUSDJPYOutrightRate);
        BindRate(25, ltUSDJPYAnnualisedPremium);
        trUSDJPY.Visible = true;
        //Finstation.BindCurrency(ltUSDJPY, 42);
    }
    
    private void ResetButton()
    {
        btnUSDINR.CssClass = "btncurrency";
        //btnUSDINR_Monthwise.CssClass = "btncurrency";
        btnForwardRateMonthEnd.CssClass = "btncurrency";
        btnForwardRateMonthwise.CssClass = "btncurrency";
        btnEURINR.CssClass = "btncurrency";
        btnGBPINR.CssClass = "btncurrency";
        btnJPYINR.CssClass = "btncurrency";
        btnEURUSD.CssClass = "btncurrency";
        btnGBPUSD.CssClass = "btncurrency";
        btnUSDJPY.CssClass = "btncurrency";

        trUSDINRMonthwise.Visible = false;
        trUSDINTMonthEnd.Visible = false;
        trUSDINRSpotRate.Visible = false;
        trEURINR.Visible = false;
        trGBPINR.Visible = false;
        trJPYINR.Visible = false;
        trEURUSD.Visible = false;
        trGBPUSD.Visible = false;
        trUSDJPY.Visible = false;
        trforwardrateTab.Visible = false;
        
    }
    private void BindCurrency1(Literal ltCurrency, int sectionId)
    {
        string query = "select * from tbl_liverate " +
                       "JOIN tbl_liveratesection ON liveratesection_liveratesectionid=liverate_liveratesectionid " +
                       "where liverate_liveratesectionid=" + sectionId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        //bid rate
        StringBuilder html=new StringBuilder();
        double dblPrevRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[0]["liverate_prevrate"]);
        double dblCurrentRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[0]["liverate_currentrate"]);
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
        dblPrevRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[1]["liverate_prevrate"]);
        dblCurrentRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[1]["liverate_currentrate"]);
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

        html.Append("<td class='" + cls + " rate liverate' rid='" + liverateId + "' rc='" + rtdCode + "' c='" + code + "'>" + dblCurrentRate + "</td>");

        //Spot Date
        string date = GlobalUtilities.ConvertToString(dttbl.Rows[2]["liverate_currentrate"]);
        sectionCode = GlobalUtilities.ConvertToString(dttbl.Rows[2]["liveratesection_code"]);
        liverateId = GlobalUtilities.ConvertToInt(dttbl.Rows[2]["liverate_liverateid"]);
        rtdCode = GlobalUtilities.ConvertToString(dttbl.Rows[2]["liverate_rtdcode"]);
        row = GlobalUtilities.ConvertToInt(dttbl.Rows[2]["liverate_row"]);
        col = GlobalUtilities.ConvertToInt(dttbl.Rows[2]["liverate_column"]);

        code = sectionCode + "_" + row + "_" + col;
        html.Append("<td style='padding-left:10px'>Spot Date</td><td class='" + code + " liverate' rid='" + liverateId + "' rc='" + rtdCode + "' c='" + code + "'>" + date + "</td>");

        ltCurrency.Text = html.ToString();
    }
    protected void btnUSDINRMonthwise_Click(object sender, EventArgs e)
    {
        ResetButton();
        //btnUSDINR_Monthwise.CssClass = "btncurrency-active";
        //BindRate(27, ltUSDINRPremium_Monthwise);
        //BindRate(28, ltUSDINROutrightRate_Monthwise);
        //BindRate(29, ltUSDINRAnnualised_Monthwise);
        //trUSDINRMonthwise.Visible = true;
        //trUSDINTMonthEnd.Visible = false;
        //trUSDINRSpotRate.Visible = true;
        //BindCurrency(ltUSDINR, 30);
    }
    //USD INR Month End
    protected void btnUSDINR_Click(object sender, EventArgs e)
    {
        ViewState["Currency"] = Currency.USDINR;
        ResetButton();
        btnUSDINR.CssClass = "btncurrency-active";
        btnForwardRateMonthEnd_Click(sender, e);
        trforwardrateTab.Visible = true;
        //BindRate(5, ltUSDINRPreminumMonthEnd);
        //BindRate(6, ltUSDINROutrightRate);
        //BindRate(7, ltUSDINRAnnualisedPremium);
        
        //trUSDINTMonthEnd.Visible = true;
        //trUSDINRMonthwise.Visible = false;
        //trUSDINRSpotRate.Visible = true;
        //BindCurrency(ltUSDINR, 30);
    }
    protected void btnApplyCalculation_Click(object sender, EventArgs e)
    {
        string query = "update tbl_liverate set liverate_calculation='" + txtCalculation.Text + "' where liverate_liverateid=" + txtTargetCalculate.Text;
        DbTable.ExecuteQuery(query);
        Response.Redirect("~/index.aspx");

    }
    protected void btnForwardRateMonthEnd_Click(object sender, EventArgs e)
    {
        btnForwardRateMonthwise.CssClass = "btncurrency";
        btnForwardRateMonthEnd.CssClass = "btncurrency-active";
        Currency currency = (Currency)ViewState["Currency"];
        if (currency == Currency.USDINR)
        {
            BindRate(5, ltUSDINRPreminumMonthEnd, false, true);
            BindRate(6, ltUSDINROutrightRate, true, true);
            BindRate(7, ltUSDINRAnnualisedPremium, true, false);

            trUSDINTMonthEnd.Visible = true;
            trUSDINRMonthwise.Visible = false;
            trUSDINRSpotRate.Visible = true;
            //Finstation.BindCurrency(ltUSDINR, 30);
        }
        else if (currency == Currency.EURINR)
        {
            BindRate(51, ltEURINRPremium, false, true);
            BindRate(52, ltEURINROutrightRate, true, true);
            BindRate(53, ltEURINRAnnualisedPremium, true, false);
            trEURINR.Visible = true;
            //Finstation.BindCurrency(ltEURINR, 32);
        }
        else if (currency == Currency.GBPINR)
        {
            BindRate(54, ltGBPINRPremium, false, true);
            BindRate(55, ltGBPINROutrightRate, true, true);
            BindRate(56, ltGBPINRAnnualisedPremium, true, false);
            trGBPINR.Visible = true;
            //Finstation.BindCurrency(ltGBPINR, 34);
        }
    }
    protected void btnForwardRateMonthwise_Click(object sender, EventArgs e)
    {
        btnForwardRateMonthwise.CssClass = "btncurrency-active";
        btnForwardRateMonthEnd.CssClass = "btncurrency";
        Currency currency = (Currency)ViewState["Currency"];
        if (currency == Currency.USDINR)
        {
            BindRate(27, ltUSDINRPremium_Monthwise, false, true);
            BindRate(28, ltUSDINROutrightRate_Monthwise, true, true);
            BindRate(29, ltUSDINRAnnualised_Monthwise, true, false);
            trUSDINRMonthwise.Visible = true;
            trUSDINTMonthEnd.Visible = false;
            trUSDINRSpotRate.Visible = true;
            //Finstation.BindCurrency(ltUSDINR, 30);
        }
        else if (currency == Currency.EURINR)
        {
            BindRate(8, ltEURINRPremium, false, true);
            BindRate(9, ltEURINROutrightRate, true, true);
            BindRate(10, ltEURINRAnnualisedPremium, true, false);
            trEURINR.Visible = true;
            //Finstation.BindCurrency(ltEURINR, 32);
        }
        else if (currency == Currency.GBPINR)
        {
            BindRate(11, ltGBPINRPremium, false, true);
            BindRate(12, ltGBPINROutrightRate, true, true);
            BindRate(13, ltGBPINRAnnualisedPremium, true, false);
            trGBPINR.Visible = true;

            //Finstation.BindCurrency(ltGBPINR, 34);
        }
    }
    private void BindFPIInvestment()
    {
        string query = "";
        query = "select top 1 * from tbl_fpiinvestment order by fpiinvestment_date desc";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        StringBuilder html = new StringBuilder();
        string equity = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_equity"], 2);
        string debt = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_debt"], 2);
        string debtvrr = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_debtvrr"], 2);
        string hybrid = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_hybrid"], 2);
        string debtfar = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_debtfar"], 2);
        string mutualfund = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_mutualfund"], 2);
        string aifs = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_aifs"], 2);

        string totaldebt = ExportExposurePortal.DecimalPoint(Convert.ToDouble(debt) + Convert.ToDouble(debtvrr) + Convert.ToDouble(debtfar)
                                + Convert.ToDouble(hybrid), 2);

        html.Append("<table class='repeater' width='100%' border='1' cellspacing=0 cellpadding=3><tr class='title'><td style='white-space:nowrap;'>FPI</td><td>Equity</td><td>Debt</td></tr>");
        html.Append("<tr><td class='repeater-header-left'>" + GlobalUtilities.ConvertToDate(dr["fpiinvestment_date"]) + "</td>");
        html.Append("<td>$ " + equity + "</td>");
        html.Append("<td>$ " + totaldebt + "</td>");
        html.Append("</tr>");
        query = @"select sum(fpiinvestment_equity) as fpiinvestment_equity,sum(fpiinvestment_debt) as fpiinvestment_debt,sum(fpiinvestment_debtvrr) as fpiinvestment_debtvrr,
                    sum(fpiinvestment_hybrid) as fpiinvestment_hybrid,sum(fpiinvestment_debtfar) as fpiinvestment_debtfar,
                    sum(fpiinvestment_mutualfund) as fpiinvestment_mutualfund,sum(fpiinvestment_aifs) as fpiinvestment_aifs
                    from (select top 7 * from tbl_fpiinvestment order by fpiinvestment_date desc)r";
        dr = DbTable.ExecuteSelectRow(query);
        equity = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_equity"], 2);
        debt = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_debt"], 2);
        debtvrr = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_debtvrr"], 2);
        hybrid = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_hybrid"], 2);
        debtfar = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_debtfar"], 2);
        mutualfund = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_mutualfund"], 2);
        aifs = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_aifs"], 2);

        totaldebt = ExportExposurePortal.DecimalPoint(Convert.ToDouble(debt) + Convert.ToDouble(debtvrr) + Convert.ToDouble(debtfar)
                                + Convert.ToDouble(hybrid), 2);

        html.Append("<tr class='repeater-row'><td class='repeater-header-left'>Last 7 days</td>");
        html.Append("<td>$ " + equity + "</td>");
        html.Append("<td>$ " + totaldebt + "</td>");
        html.Append("</tr>");
        query = @"select sum(fpiinvestment_equity) as fpiinvestment_equity,sum(fpiinvestment_debt) as fpiinvestment_debt,sum(fpiinvestment_debtvrr) as fpiinvestment_debtvrr,
                    sum(fpiinvestment_hybrid) as fpiinvestment_hybrid,sum(fpiinvestment_debtfar) as fpiinvestment_debtfar,
                    sum(fpiinvestment_mutualfund) as fpiinvestment_mutualfund,sum(fpiinvestment_aifs) as fpiinvestment_aifs
                    from (select top 15 * from tbl_fpiinvestment order by fpiinvestment_date desc)r";
        dr = DbTable.ExecuteSelectRow(query);
        equity = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_equity"], 2);
        debt = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_debt"], 2);
        debtvrr = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_debtvrr"], 2);
        hybrid = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_hybrid"], 2);
        debtfar = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_debtfar"], 2);
        mutualfund = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_mutualfund"], 2);
        aifs = ExportExposurePortal.DecimalPoint(dr["fpiinvestment_aifs"], 2);

        totaldebt = ExportExposurePortal.DecimalPoint(Convert.ToDouble(debt) + Convert.ToDouble(debtvrr) + Convert.ToDouble(debtfar)
                                + Convert.ToDouble(hybrid), 2);

        html.Append("<tr class='repeater-row-alt'><td class='repeater-header-left'>Last 15 days</td>");
        html.Append("<td>$ " + equity + "</td>");
        html.Append("<td>$ " + totaldebt + "</td>");
        html.Append("</tr>");
        html.Append("</table>");
        ltfpiinvestment.Text = html.ToString();
    }
    private void BindIndicesAndCommodities()
    {
        string query = "select * from tbl_liveratesection where liveratesection_liveratesectionid in(4,59,60)";
        DataTable dttblSection = DbTable.ExecuteSelect(query);
        string rows = "";
        for (int i = 0; i < dttblSection.Rows.Count; i++)
        {
            if (i == 0)
            {
                rows = GlobalUtilities.ConvertToString(dttblSection.Rows[i]["liveratesection_rows"]);
            }
            else
            {
                rows += "," + GlobalUtilities.ConvertToString(dttblSection.Rows[i]["liveratesection_rows"]);
            }
        }
        Array arrRows = rows.Split(',');
        StringBuilder html = new StringBuilder();
        int sectionId = 4;
        html.Append("<table class='repeater' border=1 style='width:100%;'>");
        html.Append("<tr class='repeater-header'><td align='center'><td align='center'>LTP<td align='center'>Change</td>");
        html.Append("<td align='center' style='white-space:nowrap;'>% Chg <img src='images/arrow_expand_right.png' target='jq-liverate-more-data-"+sectionId+"' class='jq-liverate-expand hand' title='Show more'/></td>");
        html.Append("<td align='center' class='hidden jq-liverate-more-data-" + sectionId + "'>Open</td>");
        html.Append("<td align='center' class='hidden jq-liverate-more-data-" + sectionId + "'>High</td>");
        html.Append("<td align='center' class='hidden jq-liverate-more-data-" + sectionId + "'>Low</td>");
        html.Append("</tr>");

        query = @"select liverate_row,liverate_column,liverate_currentrate,liverate_prevrate,liverate_liverateid,liverate_rtdcode,
                liverate_calculation,liverate_istick,liverate_liveratesectionid,liverate_decimalplaces,liverate_isapirate,liverate_isexcelupdate 
                from tbl_liverate
                WHERE liverate_liveratesectionid in(4,59,60)";
        query += " order by liverate_liveratesectionid,liverate_row,liverate_column";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        dttbl = FinstationPortal.CorrectRateData(dttbl, sectionId);
        dttbl = Finstation.CorrectLiveRateValues(dttbl);
        int rowCounter = 0;
        int prevRow = 0;
        int startIndex = 0;
        int colCount = 6;
        string sectionCode = "indiced_comm";
        int showMoreIndex = 3;
        int prevRowindex = -1;
        int colIndex = 0;
        int rowIndex = 0;
        int currencyId = 0;
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string currentRate = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_currentrate"]);
            string prevRate = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_prevrate"]);
            int liverateId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liverate_liverateid"]);
            int sid = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liverate_liveratesectionid"]);
            string rtdCode = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_rtdcode"]);
            int rowNo = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liverate_row"]);
            int colNo = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liverate_column"]);
            int istick = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liverate_istick"]);
            bool isapirate = GlobalUtilities.ConvertToBool(dttbl.Rows[i]["liverate_isapirate"]);
            bool isexcelrate = GlobalUtilities.ConvertToBool(dttbl.Rows[i]["liverate_isexcelupdate"]);
            string calculation = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_calculation"]);
            if (sid > 4 && colNo == 1) continue;
            if(sid > 4) colNo = colNo - 1;
            
            if (prevRowindex != rowNo)
            {
                if (prevRow == -1)
                {
                    html.Append("</tr>");
                }
                string currency = arrRows.GetValue(rowIndex).ToString();
                currencyId = Finstation.SaveCurrency(currency, Enum_CurrencyType.IndicesAndCommodities);
                html.Append("<tr><td class='repeater-header-left' style='white-space:nowrap;'>" + currency + "</td>");
                rowIndex++;
            }
            Finstation.UpdateLiverateCurrency(liverateId, currencyId);
            string code = sectionCode + "_" + rowNo + "_" + colNo;
            string tdcss = "";
            if (showMoreIndex > 0 && colNo > showMoreIndex)
            {
                tdcss = "hidden jq-liverate-more-data-" + sectionId;
            }
            if (calculation == "")
            {
                if (isapirate)
                {
                    tdcss += " apirate";
                }
                else if (isexcelrate)
                {
                    tdcss += " xlrate";
                }
            }

            html.Append("<td sc='" + sectionCode + "' sid='" + sid + "' row='" + rowNo + "' col='" + colNo + "' rid='" + liverateId + "' rc='" + rtdCode + "'" + " c='" + code + "' class='liverate rate " + tdcss + "' istick='" + istick + "' rate='" + currentRate + "'>" + currentRate + "</td>");
            prevRowindex = rowNo;
            
        }
        html.Append("</tr>");
        html.Append("</table>");
        ltIndicesAndCommodities.Text = html.ToString();
        
    }
    public string VersionNo
    {
        get
        {
            return ConfigurationManager.AppSettings["VersionNo"].ToString();
        }
    }
}