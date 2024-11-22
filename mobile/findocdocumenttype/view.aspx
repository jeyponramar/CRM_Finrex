<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/MasterPage.master" AutoEventWireup="true" CodeFile="~/FinDocDocumentType/view.aspx.cs" Inherits="FinDocDocumentType_view" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--DESIGN_START-->
<!--DESIGN_START-->
 <table width="100%" cellpadding="0" cellspacing="0">
    <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server"/>
        </td>
     </tr>
     <tr>
        <td>
            <uc:Grid id="grid" runat="server" Module="FinDocDocumentType"/>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>