<%@ Page Title="" Language="C#" MasterPageFile="~/CP/ConfigureMaster.master" AutoEventWireup="true" CodeFile="addons.aspx.cs" Inherits="CP_addons" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td style="background-color:#3d3c3c; vertical-align:top;width:200px;">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="background-color:#262729;border:1px solid #3f3e3e;vertical-align:top;">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </td>
        <td style="vertical-align:top;height:100%;" class="content-panel">
            <table width="100%" cellspacing="5">
                <tr><td style="vertical-align:top;height:500px;border:solid 1px #3d3c3c;">
                    <table width="100%" cellspacing="5">
                        <tr><td class="page-title"><asp:Label ID="lblTitle" runat="server" Text="Manage Add-ons"></asp:Label></td></tr>
                        <tr><td class="error"><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td></tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td class="label">Select Module</td>
                                        <td><asp:DropDownList ID="ddlModule" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlModule_Changed"></asp:DropDownList></td>
                                    </tr>
                                    <tr><td>&nbsp;</td></tr>
                                    <tr>
                                        <td></td>
                                        <td><asp:CheckBox ID="chkFollowup" Text="Activities / Followup?" runat="server"/></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td><asp:CheckBox ID="chkComments" Text="Post / Comments?" runat="server"/></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnSubmit" CssClass="button" runat="server" OnClick="btnSubmit_Click" Text="Submit" />
                            </td>
                        </tr>
                    </table>
                </td></tr>
            </table>
        </td>
    </tr>
    
</table>
</asp:Content>

