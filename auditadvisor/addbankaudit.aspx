<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="addbankaudit.aspx.cs" Inherits="auditadvisor_addbankaudit" Title="Bank Audit" %>
<%@ Register Src="~/usercontrols/AddEditAuditControl.ascx" TagName="addeditaudit" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="../js/bankaudit.js?v=<%=Common.VersionNo %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<uc:addeditaudit id="addeditaudit" runat="server" IsAdminPage="true"></uc:addeditaudit>
</asp:Content>

