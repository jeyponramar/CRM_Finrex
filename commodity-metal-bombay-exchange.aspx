<%@ Page Title="" Language="C#" MasterPageFile="~/CommodityMasterPage.master" AutoEventWireup="true" 
CodeFile="commodity-metal-bombay-exchange.aspx.cs" Inherits="commodity_metal_bombay_exchange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        //commodityLiverate(0);
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <table width="100%">
                <tr>
                    <td class="valign">
                        <table width='100%' cellspacing=0 cellpadding=0>
                            <tr><td class="title" style="white-space:nowrap;">BOMBAY METAL EXCHANGE RATES</td></tr>
                            <tr><td><asp:Literal ID="ltbombayexchangerate" runat="server"></asp:Literal></td></tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>

