<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="bulk-module-delete.aspx.cs" Inherits="CP_bulk_module_delete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
    <tr><td class="title"><asp:Label id="lblPageTitle" Text="Bulk Module Delete" runat="server"></asp:Label></td></tr>
    <tr><td><asp:Label id="lblMessage" CssClass="error" runat="server"></asp:Label></td></tr>
    <tr>
        <td>
            <asp:Literal ID="ltModules" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td align="center"><asp:Button ID="btnDelete" CssClass="delete redbutton" Text="Delete Modules" runat="server" OnClick="btnDelete_Click"/></td>
    </tr>
</table>    
</asp:Content>

