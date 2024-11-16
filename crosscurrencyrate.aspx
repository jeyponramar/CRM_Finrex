<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="crosscurrencyrate.aspx.cs" Inherits="crosscurrencyrate" Title="Spot Rate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    _isLiverateActive = true;
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
    <tr><td class="title">CROSS CURRENCY RATE</td></tr>
    <tr><td><asp:Literal ID="ltRate" runat="server"></asp:Literal></td></tr>
</table>
</asp:Content>

