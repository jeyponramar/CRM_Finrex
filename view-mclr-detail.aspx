<%@ Page Title="" Language="C#" MasterPageFile="~/Blank.master" AutoEventWireup="true" CodeFile="view-mclr-detail.aspx.cs" Inherits="view_mclr_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr><td class='page-inner2'>
        <table width='100%'><tr><td class='page-title2'>MCLR Rate Archive</td></tr>
        <tr><td>&nbsp;</td></tr><tr><td style="padding:10px;"><asp:Literal ID="lthistory" runat="server"></asp:Literal></td></tr>
        <tr><td>&nbsp;</td></tr>
        </table>
       </td>
    </tr>
    
 </table>
</asp:Content>