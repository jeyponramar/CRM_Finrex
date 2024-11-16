<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="UpdateModuleActions.aspx.cs" Inherits="CP_UpdateModuleActions" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
    <tr>
        <td>
            <asp:Button ID="btnUpdateModuleAction" Text="UpdateModuleAction" OnClick="btn_moduleUpdateClick" CssClass="button" runat="server" />
        </td>
    </tr>
</table>
</asp:Content>

