<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="view.aspx.cs" Inherits="Note_view" %>
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
        <%--<td width="50"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>--%>
     </tr>
     <tr>
        <td colspan="2">
            <uc:Grid id="grid" runat="server" Module="Note"/>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>