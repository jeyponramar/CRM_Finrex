<%@ Control Language="C#" AutoEventWireup="true" CodeFile="header-direct.ascx.cs" Inherits="header_direct" %>

<table width="100%" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <table width="1024px" cellpadding="0" cellspacing="0" align="center">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" width="50%">
                            <tr>
                                <td class="company-name">
                                    <a href="../home/default.aspx"><img width="250" src="../images/logo.png"/></a>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="height:100%">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="right">
                                    <asp:Literal ID="ltLogin" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align:bottom;padding-left:10px;padding-top:10px;">
                                    <table cellpadding="0" cellspacing="5" width="100%">
                                        <tr><td width="350">
                                        <asp:TextBox ID="txtsearch" runat="server" CssClass='search-keyword watermark'
                                        wm="Search for product,brand or any category"></asp:TextBox></td>
                                        <td class="sbutton btnsearch">Search</td>
                                        <td align="right"><div class="btncart"><div class="cart-icon"></div><div class="cart-items cart-mstr-count"></div></div></td>
                                        </tr>
                                    </table>
                                </td>            
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tdcatmenu">
               <table width="1024" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="topmenu-allcategories">ALL CATEGORIES</td>
                        <td align="right" class="offer-zone-menu">
                             <asp:Literal ID="ltOfferCategory" runat="server"></asp:Literal>
                        </td>
                    </tr>
               </table> 
        </td>
    </tr>
</table>