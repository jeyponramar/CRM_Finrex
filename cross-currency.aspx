<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="cross-currency.aspx.cs" Inherits="cross_currency" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="js/liverate.js" type="text/javascript"></script> 
<script>
    _isLiverateActive = true;
    $(document).ready(function() {
        $(".i-mainmenu").removeClass("i-mainmenu-active");
        $(".menu-crosscurrency").addClass("i-mainmenu-active");
        
    });
</script>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table cellpadding="0" cellspacing="0">
    <tr>
        <td class="title">
            <table>
                <tr>
                    <td>CROSS CURRENCIES</td>
                    <td><asp:Button ID="btnMajor" runat="server" OnClick="btnMajor_Click" Text="Major" CssClass="btncurrency-active"/></td>
                    <td><asp:Button ID="btnAsia" runat="server" OnClick="btnAsia_Click" Text="Asia" CssClass="btncurrency"/></td>
                </tr>
            </table>
        </td>
        <td class="title">LIBOR RATES</td>
    </tr>
    <tr>
        <td><asp:Literal ID="ltCrossCurrencies" runat="server"></asp:Literal></td>
        <td style="vertical-align:top;height:212px;"><asp:Literal ID="ltliberrates" runat="server"></asp:Literal></td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%">
                <tr>
                    <td style="height:180px;vertical-align:top;width:250px;">
                        <table width="100%" height="100%">
                            <tr><td class="title" style="padding-left:5px;">RBI Reference Rate</td></tr>
                            <tr><td style="vertical-align:top;"><asp:Literal ID="ltRBI" runat="server"></asp:Literal></td></tr>
                        </table>
                    </td>
                    <td style="height:180px;vertical-align:top;width:350px;">
                        <table width="100%" height="100%">
                            <tr><td class="title"><asp:Label ID="lblcustomratedate" runat="server"></asp:Label></td></tr>
                            <tr><td style="vertical-align:top;"><asp:Literal ID="ltcustomrate" runat="server"></asp:Literal></td></tr>
                            <tr><td><a href="#" id="lnkhistory-customrate">Custom Rate Archive</a></td></tr>
                        </table>
                    </td>
                    <td style="vertical-align:top;">
                        <table width="100%" height="100%">
                            <tr><td class="title" style="padding-left:5px;">Alternative Reference Rates</td></tr>
                            <tr><td style="vertical-align:top;"><asp:Literal ID="ltalternaterefrates" runat="server"></asp:Literal></td></tr>
                            <tr><td><a href="viewalternatereferencerates.aspx">Alternative Reference Rates Archive</a></td></tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table width="500" height="100%">
                 <tr><td class="title" style="padding-left:5px;">Currency Future</td></tr>
                <tr><td>
                    <asp:Button ID="btnUSDINRFuture" runat="server" OnClick="btnUSDINRFuture_Click" Text="USDINR" CssClass="btncurrency-active"/>
                    <asp:Button ID="btnEURINRFuture" runat="server" OnClick="btnEURINRFuture_Click" Text="EURINR" CssClass="btncurrency"/>
                    <asp:Button ID="btnGBPINRFuture" runat="server" OnClick="btnGBPINRFuture_Click" Text="GBPINR" CssClass="btncurrency"/>
                    <asp:Button ID="btnJPYINRFuture" runat="server" OnClick="btnJPYINRFuture_Click" Text="JPYINR" CssClass="btncurrency"/>
                </td></tr>
                <tr><td style="vertical-align:top;"><asp:Literal ID="ltFutureCurrency" runat="server"></asp:Literal></td></tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>

