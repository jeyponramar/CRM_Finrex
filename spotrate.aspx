<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="spotrate.aspx.cs" Inherits="spotrate" Title="Spot Rate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    _isLiverateActive = true;
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
    <tr><td class="title">INR RATE</td></tr>
    <tr><td><asp:Literal ID="ltSpotRate" runat="server"></asp:Literal></td></tr>
</table>
</asp:Content>

