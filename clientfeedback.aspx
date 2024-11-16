<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="clientfeedback.aspx.cs" 
Inherits="clientfeedback" Title="Feedback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr>
        <td class='page-inner2' cellpadding="0" cellspacing=0>
        <table width="100%">
            <tr>
                <td class="page-title2">Feedback</td>
            </tr>
            <tr><td style="padding-left:100px;"><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td></tr>
            <tr id="trfeedback" runat="server">
                <td style="padding:10px;padding-left:100px;">
                    <table width="100%">
                        <tr><td>Please enter your valuable feedback below to us to improve our service.</td></tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtfeedback" runat="server" TextMode="MultiLine" CssClass="textarea" Width="500" Height="200">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr><td><asp:Button ID="btnsubmit" runat="server" Text="Submit" OnClick="btnsubmit_Click" CssClass="button"/></td></tr>
                    </table>
                </td>
            </tr>
        </table>
        
       </td>
    </tr>
 </table>
</asp:Content>

