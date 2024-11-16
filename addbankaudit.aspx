<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="addbankaudit.aspx.cs" 
Inherits="addbankaudit" Title="BankScan" %>
<%@ Register Src="~/usercontrols/AddEditAuditControl.ascx" TagName="addeditaudit" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link href="js/upload/jquery.fileupload-ui.css" rel="stylesheet" type="text/css" />    
<script src="js/upload/jquery.fileupload.js?v=<%=VersionNo %>"></script>
<script src="js/upload/jquery.fileupload-ui.js?v=<%=VersionNo %>"></script>
<script src="js/upload/multifileupload.js?v=<%=VersionNo %>"></script>

<script src="js/bankaudit.js?v=<%=VersionNo %>"></script>
<style>
body{background-color:#fff !important;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<uc:addeditaudit id="addeditaudit" runat="server" IsAdminPage="false"></uc:addeditaudit>
</asp:Content>

