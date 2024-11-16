<%@ Page Title="" Language="C#" MasterPageFile="~/CP/ConfigureMaster.master" AutoEventWireup="true" CodeFile="report-settings.aspx.cs" Inherits="CP_report_settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
    <tr><td class="page-title">Report Settings</td></tr>
    <tr><td class="error"><asp:Label ID="lblMessage" runat="server"></asp:Label></td></tr>
    <tr><td>
        <asp:PlaceHolder ID="form" runat="server">
        <table width="100%">
            <tr><td width="200">Report Type</td>
            <td>
                <asp:DropDownList ID="ddlReportType" runat="server">
                    <asp:ListItem Text="Normal" Value="Normal"></asp:ListItem>
                    <asp:ListItem Text="Pie Chart" Value="Pie Chart"></asp:ListItem>
                    <asp:ListItem Text="Bar Chart" Value="Bar Chart"></asp:ListItem>
                    <asp:ListItem Text="Summary with Count" Value="Summary with Count"></asp:ListItem>
                </asp:DropDownList>    
            </td>
            </tr>
            <tr>
                <td>X Axis Column Name</td>
                <td>
                    <asp:DropDownList ID="Column1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlColumn1_Changed"></asp:DropDownList>
                    <asp:TextBox Width="500" ID="txtXAxisColumns" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Y Axis Column Name</td>
                <td>
                    <asp:DropDownList ID="Column2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlColumn2_Changed"></asp:DropDownList>
                    <asp:TextBox Width="500" ID="txtYAxisColumns" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td><asp:Button ID="btnSubmit" Text="Submit" CssClass="button" runat="server" OnClick="btnSubmit_Click"/></td>
            </tr>
        </table>
        </asp:PlaceHolder>
    </td></tr>
</table>
</asp:Content>

