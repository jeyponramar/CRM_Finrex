<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NextPrevDetail.ascx.cs" Inherits="NextPrevDetail" %>
<table>
    <tr>
        <td><asp:Button ID="btnPrev" runat="server" Text="Prev" OnClick="btnPrev_Click" CssClass="lightbutton"/></td>
        <td><asp:Label ID="lblCurrentPage" runat="server"></asp:Label></td>
        <td><asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" CssClass="lightbutton"/></td>
    </tr>
</table>
