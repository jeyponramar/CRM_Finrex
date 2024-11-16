<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/InnerMaster.master" CodeFile="SubscriptionReminder.aspx.cs" Inherits="subscriptionreminder" %>

<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--DESIGN_START-->
<table width="100%">
<asp:Label ID="lbltest" runat="server" style="display:none"></asp:Label>
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server"/>
        </td>
     </tr>
     <tr>
        <td colspan="2">
            <uc:Grid id="grid" runat="server" Module="subscriptionreminder"/>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>