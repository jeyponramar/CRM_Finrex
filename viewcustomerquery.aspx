<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="viewcustomerquery.aspx.cs" Inherits="viewcustomerquery" %>
<%@ Register Src="~/QueryMenu.ascx" TagName="QueryMenu" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr>
        <td class='page-inner2' cellpadding="0" cellspacing=0>
        <table width="100%">
            <tr>
                 <td width="250" valign="top" class="left-menu-box">
                    <uc:QueryMenu ID="queryMenu" runat="server" />
                </td>
                <td valign="top">
                    <table width='100%'><tr><td class='page-title2'>Your Queries</td></tr>
                    <tr>
                        <td>
                            <asp:Literal ID="ltquery" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    </table>
                </td>
            </tr>
        </table>
        
       </td>
    </tr>
 </table>
</asp:Content>

