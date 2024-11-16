<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="correctdata.aspx.cs" Inherits="correctdata" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
    <tr><td><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td></tr>
    <tr>
        <td>
            <table>
                <tr>
                    <td><asp:Button ID="btncorrectsubscriptionprospects" runat="server" Text="Correct Subscription Prospects by Latest Invoice" CssClass="button btnaction" OnClick="btncorrectsubscriptionprospects_Click"/></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>

