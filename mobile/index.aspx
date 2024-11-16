<%@ Page Language="C#" MasterPageFile="~/mobile/MasterPage.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="mobile_index" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page" page="login">
        <div class="login-header"><div style="padding:10px;">FinMessenger</div></div>
        <div class="login-form form">
            <table width="80%" align="center" cellpadding="10">
                <tr><td><input type="text" class="input" name="username" placeholder="User Name"/></td></tr>
                <tr><td><input type="password" class="input" name="password" placeholder="Password"/></td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr><td align="center"><input type="button" class="button btnform" value="Login"/></td></tr>
            </table>
        </div>
    </div>
    <div class="page hidden" page="notification">
        <div class="notification-modal">
            Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.
        </div>
    </div>
</asp:Content>

