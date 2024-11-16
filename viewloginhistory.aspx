<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="viewloginhistory.aspx.cs"
 Inherits="viewloginhistory" Title="View Login History" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr>
        <td class='page-inner2' cellpadding="0" cellspacing=0>
        <table width="100%">
            <tr>
                <td class="page-title2">View Login History</td>
            </tr>
            <tr>
                <td style="padding:10px;"><asp:Literal ID="ltdata" runat="server"></asp:Literal></td>
            </tr>
        </table>
        
       </td>
    </tr>
 </table>

</asp:Content>

