<%@ Page Language="C#" MasterPageFile="~/mobile/MasterPage.master" AutoEventWireup="true" CodeFile="Copy of index.aspx.cs" Inherits="mobile_index" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page" page="login">
        <div class="login-header"><div style="padding:10px;">FinMessenger</div></div>
        <div class="login-form form">
            <table width="80%" align="center" cellpadding="10">
                <tr><td><input type="text" class="input" id="txtusername" placeholder="User Name"/></td></tr>
                <tr><td><input type="password" class="input" id="txtpassword" placeholder="Password"/></td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr><td align="center"><input type="button" class="button btnlogin" value="Login"/></td></tr>
            </table>
        </div>
    </div>
    <div class="page hidden" page="notification">
        <div class="notification-modal">
            <div class="jq-notification-latest">
            </div>
        </div>
        <div class="btndismiss">Dismiss</div>
    </div>
</asp:Content>

