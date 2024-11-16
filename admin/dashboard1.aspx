<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="dashboard1.aspx.cs" Inherits="admin_dashboard" %>
<%@ Register Src="~/usercontrols/Dashboard.ascx" TagName="Dashboard" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<ol id="tour">
  <li data-id="tblenquirybystatus" data-button="Next" class="custom" data-options="tipLocation:right">
    <h4>Dashboard - Enquiry by Status</h4>
    <p>Graphical summary of enquries by status!</p>
  </li>
  <li data-id="tblleadsbycampaign" data-button="Next" data-options="tipLocation:right">
    <h4>Dashboard - Expected Leads By Campaign</h4>
    <p>Analyze which campaign is generating more leads!</p>
  </li>
  <li data-id="tblnotes" data-button="Next" data-options="tipLocation:right">
    <h4>Dashboard - Sticky Notes</h4>
    <p>Note down important things in sticky notes to remember easily!</p>
  </li>
  <li data-id="tblsmartsummary" data-button="Next" data-options="tipLocation:right">
    <h4 last="true">Dashboard - Smart Summary</h4>
    <p>Know the number of enquiries againt each enquiry status!</p>
  </li>
  
</ol>
<uc:Dashboard ID="Dashboard" runat="server" />
</asp:Content>

