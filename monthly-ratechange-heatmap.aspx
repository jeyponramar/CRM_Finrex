<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="monthly-ratechange-heatmap.aspx.cs" Inherits="monthly_ratechange_heatmap" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr><td class='page-inner2'>
        <table width='100%'><tr><td class='page-title2' colspan="2"><asp:Label ID="lbltitle" runat="server"></asp:Label></td></tr>
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td class="bold" width="100">Year : </td>
            <td>
                From <asp:TextBox ID="txtfromyear" runat="server" CssClass="val-i mbox"></asp:TextBox>
                &nbsp;To <asp:TextBox ID="txttoyear" runat="server" CssClass="val-i mbox"></asp:TextBox>
            </td>
        </tr>
        <tr><td>&nbsp;</td></tr>
        <tr><td></td><td><asp:Button ID="btnSubmit" CssClass="button-ui" runat="server" Text="Search" OnClick="btnSubmit_Click" /></td></tr>
        <tr><td>&nbsp;</td></tr>
        <tr><td colspan="2" style="padding-bottom:100px;">
            <table width="100%">
                <tr>
                    <td><asp:Literal ID="ltdata" runat="server"></asp:Literal></td>
                </tr>
                <tr><td><asp:Literal ID="ltnote" runat="server"></asp:Literal></td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td>
                        <table width="100%" cellspacing="10">
                            <tr>
                                <td class="heatmap-green1 gray-border" width="50">&nbsp;</td><td>Gains over 2%</td>
                                <td width="500"></td>
                                <td class="heatmap-red1 gray-border" width="50">&nbsp;</td><td>Losses over -2%</td>
                            </tr>
                            <tr>
                                <td class="heatmap-green2 gray-border">&nbsp;</td><td>Gains between 1% -2%</td>
                                <td width="500"></td>
                                <td class="heatmap-red2 gray-border">&nbsp;</td><td>Loses between -1% -2%</td>
                            </tr>
                            <tr>
                                <td class="heatmap-green3 gray-border">&nbsp;</td><td>Gains between 0% -1%</td>
                                <td width="500"></td>
                                <td class="heatmap-red3 gray-border">&nbsp;</td><td>Losses between 0% -1%</td>
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
    
 </table>
</asp:Content>

