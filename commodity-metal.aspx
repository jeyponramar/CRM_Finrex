<%@ Page Title="" Language="C#" MasterPageFile="~/CommodityMasterPage.master" AutoEventWireup="true" 
CodeFile="commodity-metal.aspx.cs" Inherits="commodity_metal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        initCommodityMetalHomePage();
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%" cellpadding="0" cellspacing="0">
    <tr>
        <td style="padding-bottom:10px;">
            <table>
                <tr>
                    <td>
                        <table>
                            <tr><td class="title" style="white-space:nowrap;">LME - 3M Forward</td></tr>
                            <tr>
                                <td><asp:Literal ID="ltliverate" runat="server"></asp:Literal></td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table>
                            <tr><td class="title" style="white-space:nowrap;">LME - 3M Cash Spread</td></tr>
                            <tr>
                                <td><asp:Literal ID="ltlme3mcashspread" runat="server"></asp:Literal></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%">
                <tr>
                    <td class="valign">
                        <table width='100%' cellspacing=0 cellpadding=0>
                            <tr><td class="title" style="white-space:nowrap;">LME - SETTLEMENT RATE US$/TONNE</td></tr>
                            <tr><td class="jq-lmesettlementrates"><asp:Literal ID="ltlmesettlement" runat="server"></asp:Literal></td></tr>
                        </table>
                    </td>
                    <td class="valign">
                        <table width='100%' cellspacing=0 cellpadding=0>
                            <tr><td class="title">LME Warehouse Stock</td></tr>
                            <tr><td class="jq-lmestockrates"><asp:Literal ID="ltstock" runat="server"></asp:Literal></td></tr>
                        </table>
                    </td>
                    <%--<td class="valign">
                        <table width='100%' cellspacing=0 cellpadding=0>
                            <tr><td class="title">BOMBAY&nbsp;METAL&nbsp;EXCAHNGE&nbsp;RATES</td></tr>
                            <tr><td><asp:Literal ID="ltbombayexchangerate" runat="server"></asp:Literal></td></tr>
                        </table>
                    </td>
                    <td class="valign">
                        <table width='100%' cellspacing=0 cellpadding=0>
                            <tr><td class="title">LME&nbsp;ASIAN&nbsp;REFERENCE&nbsp;PRICE</td></tr>
                            <tr><td><asp:Literal ID="ltasianrefprice" runat="server"></asp:Literal></td></tr>
                        </table>
                    </td>
                    <td class="valign">
                        <table width='100%' cellspacing=0 cellpadding=0>
                            <tr><td class="title">LME&nbsp;STERLING&nbsp;EQUIVALENTS</td></tr>
                            <tr><td><asp:Literal ID="ltsterling" runat="server"></asp:Literal></td></tr>
                        </table>
                    </td>
                    
                    <td class="valign">
                        <table width='100%' cellspacing=0 cellpadding=0>
                            <tr><td class="title">LME&nbsp;SETTLEMENT&nbsp;EXCHANGE&nbsp;RATES</td></tr>
                            <tr><td><asp:Literal ID="ltlmeexchangerate" runat="server"></asp:Literal></td></tr>
                        </table>
                    </td>--%>
                </tr>
                <tr>
                    <td class="valign" colspan="10">
                        <table width='100%' cellspacing=0 cellpadding=0>
                            <tr><td class="title">LME - SETTLEMENT ASK RATE $  US$ / TONNE  - DAILY DATA</td></tr>
                            <tr><td><asp:Literal ID="ltsettlementratedaily" runat="server"></asp:Literal></td></tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="valign" colspan="10">
                        <table width='100%' cellspacing=0 cellpadding=0>
                            <tr><td class="title">LME  AVERAGES OFFICIAL PRICES US$ PER TONNE</td></tr>
                            <tr><td><asp:Literal ID="ltsettlementratemonthly" runat="server"></asp:Literal></td></tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>

