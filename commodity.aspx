<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="commodity.aspx.cs" Inherits="commodity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    _isLiverateActive = true;
    $(document).ready(function() {
        $(".i-mainmenu").removeClass("i-mainmenu-active");
        $(".menu-commodity").addClass("i-mainmenu-active");
    });
</script> 
<style>
.repeater-header-left
{
	width:200px;
}
</style> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td class="title" style="padding-left:5px;">
            Commodity 
        </td>
    </tr>
    <tr>
        <td style="vertical-align: top;">
            <asp:Literal ID="ltcommodity" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td class="title" style="padding-left:5px;">
            Indices 
        </td>
    </tr>
    <tr>
        <td style="vertical-align: top;">
            <asp:Literal ID="ltIndices" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td class="title" style="padding-left:5px;">
            Indice Futures 
        </td>
    </tr>
    <tr>
        <td style="vertical-align: top;">
            <asp:Literal ID="ltIndiceFutures" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td class="title" style="padding-left:5px;">
            Government Bond 10Y Yield
        </td>
    </tr>
    <tr>
        <td style="vertical-align: top;">
            <asp:Literal ID="ltGovernmenrBond" runat="server"></asp:Literal>
        </td>
    </tr>
</table>
</asp:Content>

