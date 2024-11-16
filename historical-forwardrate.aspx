<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="historical-forwardrate.aspx.cs" 
Inherits="historical_forwardrate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style>
.page-inner2
{
	background-color:#000;
	color:#fff;
}
.grid-ui-header
{
	background-color:#111;
}
.grid-ui
{
	border-color:#222;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr><td class='page-inner2' style="padding-bottom:50px;">
        <table width='100%'><tr><td class='page-title2'>Historical Forward Rate</td></tr>
        <tr>
            <td>
                <table cellspacing=10>
                    <tr>
                        <td width="100" class="bold">Currency <span class="error">*</span> : </td>
                        <td><asp:DropDownList ID="ddlcurrency" runat="server" style="width:120px;height:25px;">
                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                            <asp:ListItem Text="USDINR" Value="2"></asp:ListItem>
                            <asp:ListItem Text="EURINR" Value="3"></asp:ListItem>
                            <asp:ListItem Text="GBPINR" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="ddlcurrency" InitialValue="0" ValidationGroup="vg" ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="bold">Date <span class="error">*</span> : </td>
                        <td>
                            <asp:TextBox ID="txtdate" runat="server" CssClass="datepicker"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv2" runat="server" ControlToValidate="txtdate" ValidationGroup="vg" ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr><td></td><td><asp:Button ID="btnSubmit" CssClass="button-ui" runat="server" Text="Search" OnClick="btnSubmit_Click" ValidationGroup="vg"/></td></tr>
                </table>
            </td>
        </tr>
        
        <tr><td>&nbsp;</td></tr>
        <tr style="display:none;"><td>
        <asp:Literal ID="ltspotrate" runat="server"></asp:Literal>
        </td></tr>
        <tr><td><asp:Literal ID="ltdata" runat="server"></asp:Literal></td></tr>
        </table>
       </td>
    </tr>
    
 </table>
</asp:Content>

