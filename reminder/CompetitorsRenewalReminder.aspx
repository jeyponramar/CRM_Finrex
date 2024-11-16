<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/InnerMaster.master" CodeFile="CompetitorsRenewalReminder.aspx.cs" Inherits="CompetitorsRenewalReminder" %>

<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--DESIGN_START-->
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server"/>
        </td>
     </tr>
     <tr>
        <td colspan="2">
            <uc:Grid id="grid" runat="server" Module="competitorsrenewalreminder"/>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>