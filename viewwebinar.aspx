<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="viewwebinar.aspx.cs" 
Inherits="viewwebinar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr><td class='page-inner2'>
        <table width='100%'><tr><td class='page-title2'><asp:Label ID="lbltitle" runat="server" Text="Webinar"></asp:Label></td></tr>
        <tr><td>&nbsp;</td></tr>
        <tr>
        <td style="padding:10px;">
        <asp:Literal ID="ltdata" runat="server"></asp:Literal></td>
        </tr>
        </table>
       </td>
    </tr>
    
 </table>
</asp:Content>

