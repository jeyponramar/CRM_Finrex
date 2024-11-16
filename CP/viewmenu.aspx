<%@ Page Title="View Module" Language="C#" MasterPageFile="~/CP/ConfigureMaster.master" AutoEventWireup="true" CodeFile="viewmenu.aspx.cs" Inherits="viewmodule" %>
<%@ Register Src="~/CP/CPGridData.ascx" TagName="GridData" TagPrefix="uc" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>
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
        <table><tr><td>
        <asp:PlaceHolder ID="plmenu" runat="server">
        <table width="100">
            <tr>
                <td class="label">Menu</td>
                <td><asp:TextBox ID="txtMenuName" runat="server" CssClass="textbox menuname"></asp:TextBox></td>
                <td class="label">URL</td>
                <td><asp:TextBox ID="txtURL" runat="server" CssClass="textbox url"></asp:TextBox></td>
                <td class="label">Sequence</td>
                <td><asp:TextBox ID="txtSequence" runat="server" CssClass="textbox sequence"></asp:TextBox></td>
                <td><asp:Button ID="btnSave" runat="server" CssClass="button" OnClick="btnSave_Click" Text="Save"/>&nbsp;<asp:Button ID="btnDelete" runat="server" CssClass="button delete" OnClick="btnDelete_Click" Text="Delete"/>
                    <asp:TextBox ID="h_MenuId" runat="server" CssClass="menuid hidden" Text="0"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label>
                </td>
            </tr>
        </table>
        </asp:PlaceHolder>
        </td></tr></table>
    </td></tr>
    <tr><td>
        <uc:GridData ID="gridData" runat="server"  Module="menu" EnablePaging="true" EnableSorting="true"/>
        <%--<uc:Grid ID="grid2" runat="server" Module="Menu"/> --%>
    </td></tr>
</table>
</asp:Content>

