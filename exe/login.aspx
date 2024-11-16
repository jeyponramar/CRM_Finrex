<%@ Page Language="C#" MasterPageFile="~/exe/ExeMasterPage.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="exe_login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%" cellpadding="0">
    <tr><td style="height:12px;"><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td></tr>
    <tr>
        <td>
            <table width="100%" cellpadding="5" id="tblLogin" runat="server">
                <tr>
                    <td>User Name</td>
                    <td><asp:TextBox ID="txtusername" runat="server" CssClass="textbox" style="width:120px;"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Password</td>
                    <td><asp:TextBox ID="txtpassword" runat="server" CssClass="textbox" TextMode="Password" style="width:120px;"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Button ID="btnLogin" runat="server" CssClass="button" Text="Login" OnClick="btnLogin_Click"/></td>
                </tr>
            </table>            
        </td>
    </tr>
    
</table>
</asp:Content>

