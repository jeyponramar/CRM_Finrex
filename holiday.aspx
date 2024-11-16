<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="holiday.aspx.cs" Inherits="holiday" Title="Holiday" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="page-title">
            Holiday
        </td>
     </tr>
     <tr>
        <td>
            <table>
                <tr>
                    <td>Currency</td>
                    <td>
                    <asp:DropDownList ID="ddlcurrency" runat="server" OnSelectedIndexChanged="ddlcurrency_Changed" AutoPostBack="true" Width="120" Height="30">
                        <asp:ListItem Text="Indian Holiday" Value="1"></asp:ListItem>
                        <asp:ListItem Text="US Holiday" Value="2"></asp:ListItem>
                        <asp:ListItem Text="European Holiday" Value="3"></asp:ListItem>
                        <asp:ListItem Text="UK Holiday" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Japan Holiday" Value="5"></asp:ListItem>
                    </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </td>
     </tr>
     <tr>
        <td>
            <asp:Literal ID="ltHoliday" runat="server"></asp:Literal>
        </td>
     </tr>
</table>
</asp:Content>

