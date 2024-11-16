<%@ Page Title="View Module" Language="C#" MasterPageFile="~/CP/ConfigureMaster.master" AutoEventWireup="true" CodeFile="viewsubmenu.aspx.cs" Inherits="viewmodule" %>
<%@ Register Src="~/CP/CPGridData.ascx" TagName="GridData" TagPrefix="uc" %>
<%--<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        populateFromRow();
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
    <tr><td class="page-title"><asp:Label id="lblTitle" runat="server"></asp:Label></td></tr>
    <tr><td>
        <table width="100%"><tr><td>
        <asp:PlaceHolder ID="plmenu" runat="server">
        <table width="100%">
            <tr>
                <td width="100">Main Menu</td>
                <td><asp:DropDownList ID="ddlMenuId" runat="server" CssClass="menuid" AutoPostBack="true" OnSelectedIndexChanged="ddlMenuId_Changed"></asp:DropDownList></td>
                <td  width="100">Sub Menu</td>
                <td><asp:TextBox ID="txtSubMenuName" runat="server" CssClass="textbox submenuname"></asp:TextBox></td>
                <td  width="100">URL</td>
                <td><asp:TextBox ID="txtURL" runat="server" CssClass="textbox url"></asp:TextBox></td>
                <td  width="100">Sequence</td>
                <td><asp:TextBox ID="txtSequence" runat="server" CssClass="textbox sequence" Width="40"></asp:TextBox></td>
                <td width="120"><asp:CheckBox ID="chkIsNewWindow" runat="server" Text="Is New Window"/></td>
                <td width="400">
                    <asp:Button ID="btnSave" runat="server" CssClass="button" OnClick="btnSave_Click" Text="Save"/>&nbsp;<asp:Button ID="btnDelete" runat="server" CssClass="button delete" OnClick="btnDelete_Click" Text="Delete"/>
                    <asp:TextBox ID="h_SubMenuId" runat="server" CssClass="submenuid hidden" Text="0"></asp:TextBox>
                </td>
                <td  width="200">
                    <asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label>
                </td>
            </tr>
        </table>
        </asp:PlaceHolder>
        </td></tr></table>
    </td></tr>
    <tr><td>
        <uc:GridData ID="gridData" runat="server"  Module="submenu" EnablePaging="true" EnableSorting="true"/>
        <%--<uc:Grid ID="grid2" runat="server" Module="SubMenu"/> --%>
    </td></tr>
</table>
</asp:Content>

