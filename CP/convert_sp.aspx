<%@ Page Title="" Language="C#" MasterPageFile="~/CP/ConfigureMaster.master" AutoEventWireup="true" CodeFile="convert_sp.aspx.cs" Inherits="CP_convert_sp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
    <tr><td class="page-title"><asp:Label id="lblTitle" runat="server" Text="Convert Module"></asp:Label></td></tr>
    <tr><td><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td></tr>
    <tr>
    <td>
        <table width="100%">
            <tr>
                <td width="200">Convert From Main Table</td>
                <td width="200"><asp:TextBox ID="txtMainTableName" runat="server"></asp:TextBox></td>
                <td><asp:CheckBox ID="chkOverwriteStoredProcedure" runat="server" Text="Overwrite Stored Procedure?" /></td>
            </tr>
            <tr>
                <td>Sub Module Table</td>
                <td><asp:TextBox ID="txtSubTableName" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>To Main Table</td>
                <td><asp:TextBox ID="txtToMainTable" runat="server"></asp:TextBox></td>
                <td width="150">Code column name</td>
                <td width="100"><asp:TextBox ID="txtCodeColumn" runat="server"></asp:TextBox></td>
                <td width="100">Code Prefix</td>
                <td><asp:TextBox ID="txtCodePrefix" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>To Sub Table</td>
                <td><asp:TextBox ID="txtToSubTable" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Button Text</td>
                <td><asp:TextBox ID="txtButtonText" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td></td>
                <td><asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" /></td>
            </tr>
            <tr>
                <td>Stored Procedure</td>
                <td colspan="10">
                    <asp:TextBox TextMode="MultiLine" Rows="20" runat="server" ID="txtStoredProcedure" Width="800"></asp:TextBox>
                </td>
            </tr>
        </table>
    </td>
    </tr>
</table>    

</asp:Content>

