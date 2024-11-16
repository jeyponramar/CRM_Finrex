<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="index.aspx.cs" Inherits="index" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script src="js/chartjs/utils.js?v=<%=VersionNo %>"></script><script src="js/chartjs/Chart.bundle.js?v=<%=VersionNo %>"></script>
<script src="js/chartjs/common-chart.js?v=<%=VersionNo %>"></script>
<script src="js/home.js?v=<%=VersionNo %>"></script>

<script>
    _isLiverateActive = true;
</script>
<style>
    .tblspotrate .liverate
    {
        font-size:13px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="tblDailyHistoricalChart" title="Daily Historical Chart" class="hdn">
        <table cellspacing="5" width="100%">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td class="box" valign="top" style="padding:5px;">
                                <table cellspacing="10">
                                    <tr>
                                        <td width="150">Select Date Range</td>
                                        <td>
                                            <select class="ddl jq-ddlliveratedate-spotrate" width="150">
                                                <option value="0">Live Rate</option>
                                                <option value="1">5D</option>
                                                <option value="2">1M</option>
                                                <option value="3">3M</option>
                                                <option value="4">6M</option>
                                                <option value="5">1Y</option>
                                                <option value="6">5Y</option>
                                                <option value="7">Custom Date</option>
                                            </select>
                                        </td>
                                        <td class="hidden jq-liveratedate-spotrate">
                                            <input type="text" class="datepicker jq-txtliveratestartdate-spotrate"/>&nbsp;
                                            <input type="text" class="datepicker jq-txtliverateenddate-spotrate"/>&nbsp;
                                        </td>
                                        <td><input type="button" class="button jq-btnliveratechart-spotrate" value="UPDATE" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr><td>&nbsp;</td></tr>
                        <tr class="jq-spotrate-live-chart-panel">
                            <td class='box jq-spotrate-live-chartpanel' valign='top'>
                                <%--<div class='jq-spotrate-live-chart' ct='3' gridcolor='#222' colors='yellow' xaxislabel='Close' data='' labels='' pointradius='0'></div>--%>
                            </td>
                        </tr>
                        <tr>
                            <td valign='top' class="box jq-spotrate-daily-chart-panel hidden"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="divCalculate" runat="server" visible="false" class="divcalculate" style="position: fixed;
        width: 1000px; height: 80px; top: 0px; background-color: #000; opacity: 0.95;
        padding: 20px; border: solid 1px #fff;">
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtCalculation" runat="server" Width="1000" CssClass="txtcalc"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtTargetCalculate" runat="server" Width="500" CssClass="txtcalctarget"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnApplyCalculation" runat="server" Text="Apply" OnClick="btnApplyCalculation_Click"
                        CssClass="btncalc" />
                    <input type="button" id="btnclearcalc" value="clear" />
                    <input type="button" id="btnclosecalc" value="Close" />
                </td>
            </tr>
        </table>
    </div>
    <table width="100%">
        <tr>
            <td class="valign">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="title">
                            <table width="100%">
                                <tr>
                                    <td>
                                        SPOT RATE <span id="jq-spotrate" style="display:none;"></span>
                                        
                                    </td>
                                    <td align="right">
                                        Spot Date : <asp:Literal ID="ltspotdate" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 185px; vertical-align: top;" class="tblspotrate">
                            <asp:Literal ID="ltSpotRate" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    
                </table>
            </td>
            
            <td class="valign">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td class="title">
                            <table cellpadding=0>
                                <tr>
                                    <td style="padding-left:5px;"></td>
                                    <td><asp:Button ID="btnMajor" runat="server" OnClick="btnMajor_Click" Text="Major" CssClass="btncurrency-active"/></td>
                                    <td><asp:Button ID="btnAsia" runat="server" OnClick="btnAsia_Click" Text="Asia" CssClass="btncurrency"/></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <asp:Literal ID="ltCrossCurrencies" runat="server"></asp:Literal>
                        </td>
                    </tr>
                   
                </table>
            </td>
            <td class="valign">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="title" style="padding-left:5px;">
                            INDICES & COMMODITIES
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;" class="indicesandcommodities">
                            <asp:Literal ID="ltIndicesAndCommodities" runat="server"></asp:Literal>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <table width="100%">
                                <%--<tr><td class="title" style="padding-left:5px;">OFFSHORE - Bid / High / Low</td></tr>--%>
                                <tr>
                                    <td class="offshore"><asp:Literal ID="ltOffshore" runat="server"></asp:Literal></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="valign">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td class="title">GLOBAL INDICES FUTURES</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <asp:Literal ID="ltglobalindices" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <asp:Literal ID="ltfpiinvestment" runat="server"></asp:Literal>
                        </td>
                    </tr>
                   
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4" valign="top">
                <table cellpadding="0" cellspacing="0" width="100%" class="jq-tblforwardrate">
                    <tr>
                        <td class="title">
                            <table cellpadding=0>
                                <tr>
                                    <td>
                                        FORWARD RATE
                                    </td>
                                    <td style="padding-left:10px;">
                                        <asp:Button ID="btnUSDINR" runat="server" OnClick="btnUSDINR_Click" Text="USDINR"
                                            CssClass="btncurrency-active" />
                                    </td>
                                   <%-- <td>
                                        <asp:Button ID="btnUSDINR_Monthwise" runat="server" OnClick="btnUSDINRMonthwise_Click"
                                            Text="USDINR (Monthwise)" CssClass="btncurrency" />
                                    </td>--%>
                                    <td>
                                        <asp:Button ID="btnEURINR" runat="server" OnClick="btnEURINR_Click" Text="EURINR"
                                            CssClass="btncurrency" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGBPINR" runat="server" OnClick="btnGBPINR_Click" Text="GBPINR"
                                            CssClass="btncurrency" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnJPYINR" runat="server" OnClick="btnJPYINR_Click" Text="JPYINR"
                                            CssClass="btncurrency" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnEURUSD" runat="server" OnClick="btnEURUSD_Click" Text="EURUSD"
                                            CssClass="btncurrency" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGBPUSD" runat="server" OnClick="btnGBPUSD_Click" Text="GBPUSD"
                                            CssClass="btncurrency" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnUSDJPY" runat="server" OnClick="btnUSDJPY_Click" Text="USDJPY"
                                            CssClass="btncurrency" />
                                    </td>
                                </tr>
                                
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr id="trforwardrateTab" runat="server">
                                    <td>
                                        <asp:Button ID="btnForwardRateMonthEnd" runat="server" OnClick="btnForwardRateMonthEnd_Click" Text="Month End"
                                            CssClass="btncurrency-active" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnForwardRateMonthwise" runat="server" OnClick="btnForwardRateMonthwise_Click" Text="Monthwise"
                                            CssClass="btncurrency" />
                                    </td>
                                    <td width="60">
                                        <a href="?#" class="jq-forwardrate-prev hidden">Prev Year</a>
                                    </td>
                                    <td>
                                        <a href="?#" class="jq-forwardrate-next">Next Year</a>
                                    </td>
                                    <td style="padding-left:10px;" class="hidden jq-forwardrate-next-msg">2-5 years Premium are Indicative Rates due to less Liquidity. Kindly call your advisors for current rates.</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%">
                                <tr id="trUSDINRSpotRate" runat="server">
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <%--<td style="display:none;">
                                                    <table width="500" border="1" cellpadding="3" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                USDINR
                                                            </td>
                                                            <asp:Literal ID="ltUSDINR" runat="server"></asp:Literal>
                                                        </tr>
                                                    </table>
                                                </td>--%>
                                                <td class="title" style="padding-top: 0px; padding-bottom: 0px; display: none;">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnUSDINRMonthEnd" runat="server" OnClick="btnUSDINRMonthEnd_Click"
                                                                    Text="USDINR Premium in Paisa (Month End)" CssClass="btncurrency-active" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnUSDINRMonthwise" runat="server" OnClick="btnUSDINRMonthwise_Click"
                                                                    Text="USDINR Premium in Paisa (Monthwise)" CssClass="btncurrency" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trUSDINTMonthEnd" runat="server">
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="ltUSDINRPreminumMonthEnd" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="stitle" colspan="20">
                                                    USDINR Outright Rate
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="ltUSDINROutrightRate" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="stitle" colspan="20">
                                                    USDINR Annualised Premium %
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="ltUSDINRAnnualisedPremium" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trUSDINRMonthwise" visible="false" runat="server">
                                    <td>
                                        <table width="100%">
                                            <tr r="1">
                                                <td>
                                                    <asp:Literal ID="ltUSDINRPremium_Monthwise" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr r="2">
                                                <td class="stitle" colspan="20">
                                                    USDINR Outright Rate
                                                </td>
                                            </tr>
                                            <tr r="3">
                                                <td>
                                                    <asp:Literal ID="ltUSDINROutrightRate_Monthwise" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr r="4">
                                                <td class="stitle" colspan="20">
                                                    USDINR Annualised Premium %
                                                </td>
                                            </tr>
                                            <tr r="5">
                                                <td>
                                                    <asp:Literal ID="ltUSDINRAnnualised_Monthwise" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trEURINR" runat="server" visible="false">
                        <td>
                            <table width="100%">
                                <tr>
                                    <td class="stitle" colspan="20">
                                        EURINR Premium in Paisa
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltEURINRPremium" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="stitle" colspan="20">
                                        EURINR Outright Rate
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltEURINROutrightRate" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="stitle" colspan="20">
                                        EURINR Annualised Premium %
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltEURINRAnnualisedPremium" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trGBPINR" runat="server" visible="false">
                        <td>
                            <table width="100%">
                                <tr>
                                    <td class="stitle" colspan="20">
                                        GBPINR Premium in Paisa
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltGBPINRPremium" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="stitle" colspan="20">
                                        GBPINR Outright Rate
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltGBPINROutrightRate" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="stitle" colspan="20">
                                        GBPINR Annualised Premium %
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltGBPINRAnnualisedPremium" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trJPYINR" runat="server" visible="false">
                        <td>
                            <table width="100%">
                                <%--<tr style="display:none;">
                                    <td>
                                        <table width="500" border="1" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    JPYINR
                                                </td>
                                                <asp:Literal ID="ltJPYINR" runat="server"></asp:Literal>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="stitle" colspan="20">
                                        JPYINR Premium in Paisa (Monthwise)
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltJPYINRPremium" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="stitle" colspan="20">
                                        JPYINR Outright Rate
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltJPYINROutrightRate" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="stitle" colspan="20">
                                        JPYINR Annualised Premium %
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltJPYINRAnnualisedPremium" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trEURUSD" runat="server" visible="false">
                        <td>
                            <table width="100%">
                                <%--<tr style="display:none;">
                                    <td>
                                        <table width="500" border="1" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    EURUSD
                                                </td>
                                                <asp:Literal ID="ltEURUSD" runat="server"></asp:Literal>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="stitle" colspan="20">
                                        EURUSD Premium in Pips (Monthwise)
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltEURUSDPremium" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="stitle" colspan="20">
                                        EURUSD Outright Rate
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltEURUSDOutrightRate" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="stitle" colspan="20">
                                        EURUSD Annualised Premium %
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltEURUSDAnnualisedPremium" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trGBPUSD" runat="server" visible="false">
                        <td>
                            <table width="100%">
                                <%--<tr style="display:none;">
                                    <td>
                                        <table width="500" border="1" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    GBPUSD
                                                </td>
                                                <asp:Literal ID="ltGBPUSD" runat="server"></asp:Literal>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="stitle" colspan="20">
                                        GBPUSD Premium in Pips(Monthwise)
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltGBPUSDPremium" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="stitle" colspan="20">
                                        GBPUSD Outright Rate
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltGBPUSDOutrightRate" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="stitle" colspan="20">
                                        GBPUSD Annualised Premium %
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltGBPUSDAnnualisedPremium" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trUSDJPY" runat="server" visible="false">
                        <td>
                            <table width="100%">
                                <%--<tr style="display:none;">
                                    <td>
                                        <table width="500" border="1" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    USDJPY
                                                </td>
                                                <asp:Literal ID="ltUSDJPY" runat="server"></asp:Literal>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="stitle" colspan="20">
                                        USDJPY Premium in Pips(Monthwise)
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltUSDJPYPremium" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="stitle" colspan="20">
                                        USDJPY Outright Rate
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltUSDJPYOutrightRate" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="stitle" colspan="20">
                                        USDJPY Annualised Premium %
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltUSDJPYAnnualisedPremium" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            
        </tr>
    </table>
    <div id="divliverate-configuser-modal" title="" class="hdn jq-liverate-configuser-modal">
        <table width="100%">
            <tr>
                <td><div class="jq-liverate-configuser"></div></td>
            </tr>
            <tr>
                <td align="center"><input type="button" class="button jq-liverate-config-save" value="Save"/></td>
            </tr>
        </table>
    </div>       
</asp:Content>
