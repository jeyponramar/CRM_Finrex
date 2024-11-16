<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="message.aspx.cs" Inherits="message_message" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
    <tr><td><asp:Label ID="lblTitle" runat="server" CssClass="title"></asp:Label></td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td></tr>
</table>
</asp:Content>

