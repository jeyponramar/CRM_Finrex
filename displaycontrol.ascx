<%@ Control Language="C#" AutoEventWireup="true" CodeFile="displaycontrol.ascx.cs" Inherits="displaycontrol" %>

<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
	    <td><asp:Literal ID="ltdisplaycontrol" runat="server"></asp:Literal></td>
	</tr>
	<tr>
        <td align="center">
            <asp:Button ID="btn_AddContent" runat="server" OnClick="btn_AddContent_Click" Text="Add Content" CssClass="button" ValidationGroup="vg"/>
        </td>            
    </tr>
</table>