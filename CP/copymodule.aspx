<%@ Page Title="" Language="C#" MasterPageFile="~/CP/ConfigureMaster.master" AutoEventWireup="true" CodeFile="copymodule.aspx.cs" Inherits="CP_copymodule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
    <tr><td class="page-title"><asp:Label id="lblTitle" runat="server" Text="Copy Module"></asp:Label></td></tr>
    <tr><td><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td></tr>
    <tr>
    <td>
        <table width="100%">
            <tr>
                <td>Copy Project Folder Path</td>
                <td><asp:TextBox ID="txtCopyProjectFolder" runat="server" Width="700" Text=""></asp:TextBox>
                   <span class="error">Make sure that you have the latest code in this folder, keep this blank if you want to copy from same project</span>
                </td>
            </tr>
            <tr>
                <td>Copy Project Connection String</td>
                <td><asp:TextBox ID="txtCopyProjectConnectionString" runat="server" Width="700"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Copy Module</td>
                <td><asp:TextBox ID="txtCopyModule" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCopyModule" 
                    ErrorMessage="Required Module Name" Display="Dynamic" ValidationGroup="vg"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>New Module Name</td>
                <td><asp:TextBox ID="txtModuleName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtModuleName" 
                    ErrorMessage="Required Module Name" Display="Dynamic" ValidationGroup="vg"></asp:RequiredFieldValidator>
                </td>
            </tr>
           
            <tr>
                <td></td>
                <td><asp:Button ID="btnCopy" runat="server" Text="Copy" CssClass="button" OnClick="btnCopy_Click"/></td>
            </tr>
            <tr>
                <td></td>
                <td style="font-size:15px;">
                    <table border=1 cellspacing="0" cellpadding="5">
                        <tr style="background-color:#aaaaaa">
                            <td>Object</td>
                            <td>Name</td>
                            <td>Action/Status</td>
                        </tr>
                        <asp:Literal ID="ltStatus" runat="server"></asp:Literal>
                    </table>
                </td>
            </tr>
            
        </table>
    </td>
    </tr>
</table>        
</asp:Content>

