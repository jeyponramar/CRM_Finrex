<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="viewinvoice.aspx.cs" 
Inherits="viewinvoice" Title="View Invoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr>
        <td class='page-inner2' cellpadding="0" cellspacing=0>
        <table width="100%">
            <tr>
                <td class="page-title2">View Invoice</td>
            </tr>
            <tr>
                <td style="border-bottom:solid 1px #ddd;">
                    <asp:Literal ID="lttab" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td><asp:Literal ID="ltdata" runat="server"></asp:Literal></td>
            </tr>
        </table>
        
       </td>
    </tr>
 </table>
</asp:Content>

