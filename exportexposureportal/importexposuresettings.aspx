<%@ Page Title="" Language="C#" MasterPageFile="~/exportexposureportal/ExportPortalMasterPage.master" 
AutoEventWireup="true" CodeFile="importexposuresettings.aspx.cs" Inherits="exportexposureportal_importexposuresettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
    <tr>
        <td colspan="2" align="center">
            <asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%">
                <tr><td class="bold">Configure Alerts</td></tr>
                <tr>
                    <td align="right">
                        <a href="bagridview.aspx?m=bank">Configure Bank</a>
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr><td>
                    <asp:PlaceHolder ID="form" runat="server">
                    <table>
                        <tr>
                            <td>Email Id</td>
                            <td><asp:TextBox ID="txtemailid" runat="server" CssClass="textbox" Width="400"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Send Daily Dashboard</td>
                            <td><asp:CheckBox ID="chksenddailydashboard" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Send Forward Contract due date mail</td>
                            <td><asp:CheckBox ID="chksendforwardcontractduedatemail" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Send Trade Credit due date mail</td>
                            <td><asp:CheckBox ID="chksendtradecreditduedatemail" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Send Import Order due date mail</td>
                            <td><asp:CheckBox ID="chksendimportorderduedatemail" runat="server" /></td>
                        </tr>
                    </table>
                    </asp:PlaceHolder>
                </td></tr>
            </table>
        </td>
        <td>
            <table>
                <tr>
                    <td class="bold">Currency</td>
                </tr>
                <tr><td><asp:Literal ID="ltCurrency" runat="server"></asp:Literal></td></tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table>
                <tr><td class="bold">Configure Optional Fields</td></tr>
                <tr><td><asp:Literal ID="ltOptionalFields" runat="server"></asp:Literal></td></tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="center">
            <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="button" />
        </td>
    </tr>
</table>
</asp:Content>

