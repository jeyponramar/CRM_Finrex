<%@ Page Title="" Language="C#" MasterPageFile="~/CommodityMasterPage.master" AutoEventWireup="true" CodeFile="commodity-metaldetails.aspx.cs" Inherits="commodity_metaldetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        bindCommodityLiveChart();
        commodityLiverate($(".jq-metalid").val());
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr><td class='page-inner2'>
        <table width='100%'><tr><td class='pagetitle'><asp:Label ID="lblpagetitle" runat="server"></asp:Label>
        <asp:TextBox ID="hdnmetalid" runat="server" CssClass="hidden jq-metalid"></asp:TextBox>
        </td></tr>
        <tr><td align="right">
            <table>
                <tr>
                    <td>
                        <div class="tabs">
                            <div class="jq-tradesumma-tab">Trading Summary</div>
                            <div class="jq-ctyearsummary-tab">Current year summary</div>
                            <div class="jq-btnpricegraph">Price Graph</div>
                            <div class="jq-btnmonthlysummary">Average Monhtly Price</div>
                        </div>
                    </td>
                </tr>
            </table>
        </td></tr>
         <tr class="jq-tab">
            <td>
                <table width="100%">
                    <tr><td class='stitle'><asp:Label ID="lblmetalname" runat="server"></asp:Label></td><td></td><td class='stitle'>CHART</td></tr>
                    <tr>
                        <td valign="top" width="30%"><asp:Literal ID="ltliverate" runat="server"></asp:Literal></td>
                        <td width="20">&nbsp;</td>
                        <td valign="top">
                            <table width="100%">
                                <tr>
                                    <td class="box" valign="top" style="padding:5px;">
                                        <table cellspacing="10">
                                            <tr>
                                                <td width="150">Select Date Range</td>
                                                <td>
                                                    <select class="ddl jq-ddlliveratedate" width="150">
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
                                                <td class="hidden jq-liveratedate">
                                                    <input type="text" class="datepicker jq-txtliveratestartdate"/>&nbsp;
                                                    <input type="text" class="datepicker jq-txtliverateenddate"/>&nbsp;
                                                </td>
                                                <td><input type="button" class="button jq-btnliveratechart" value="UPDATE" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr><td>&nbsp;</td></tr>
                                <tr class="jq-commodity-live-chart-panel">
                                    <td class='box' valign='top'>
                                        <div class='jq-commodity-live-chart' ct='3' xaxislabel='Bid' data='' labels='' pointradius='0'></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign='top' class="box jq-commodity-daily-chart-panel hidden">
                                        
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                
            </td>
        </tr>
        <tr class="hidden jq-tab" id="pricegraph">
            <td>
                <table width="100%">
                    <tr>
                        <td width="30%" class="box" valign="top">
                            <table width="100%" cellspacing="10">
                                <tr><td colspan="2">SHOW HISTORICAL DATA FOR</td></tr>
                                <tr>
                                    <td>Date From</td>
                                    <td><asp:TextBox ID="txtpricegraphdatefrom" runat="server" class="datepicker jq-pricegraph-date-from" /></td>
                                </tr>
                                <tr>
                                    <td>Date To</td>
                                    <td><asp:TextBox ID="txtpricegraphdateto" runat="server" class="datepicker jq-pricegraph-date-to" /></td>
                                </tr>
                                <tr>
                                    <td>Contract Type</td>
                                    <td>
                                        <select class="ddl jq-pricegraph-contracttype">
                                            <option value="1" selected>Cash</option>
                                            <option value="2">3 months</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr><td>&nbsp;</td></tr>
                                <tr>
                                    <td></td>
                                    <td><input type="button" class="button jq-btnpricegraph" value="UPDATE" /></td>
                                </tr>
                            </table>
                        </td>
                        <td width="20">&nbsp;</td>
                        <td class="box jq-pricegraph"></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="hidden jq-tab" id="currentyearsummary">
            <td class="jq-currentyearsummary">
            </td>
        </tr>
       
        <tr class="hidden jq-tab" id="tradingsummary">
            <td class="jq-tradingsummary">
                
            </td>
        </tr>
        
        <tr class="hidden jq-tab" id="averagemonhtlyprice">
            <td>
                <table width="100%">
                    <tr>
                        <td width="30%" class="box" valign="top">
                            <table width="100%" cellspacing="10">
                                <tr><td colspan="2">SELECT DATE FROM CURRENT CALENDAR YEAR</td></tr>
                                <tr>
                                    <td>Select Month</td>
                                    <td><asp:Literal ID="ltmonth" runat="server"></asp:Literal></td>
                                </tr>
                                <tr><td>&nbsp;</td></tr>
                                <tr>
                                    <td></td>
                                    <td><input type="button" class="button jq-btnmonthlysummary" value="UPDATE" /></td>
                                </tr>
                                <tr><td>&nbsp;</td></tr>
                                <tr><td></td></tr>
                            </table>
                        </td>
                        <td width="20">&nbsp;</td>
                        <td class="box jq-monthlysummary" valign="top"></td>
                    </tr>
                </table>
            </td>
        </tr>
        
        </table>
       </td>
    </tr>
    
 </table>
</asp:Content>

