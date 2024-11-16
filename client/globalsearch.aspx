<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="globalsearch.aspx.cs" Inherits="client_globalsearch" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
             <tr>
                <td class="title">
                    <asp:Label ID="lblPageTitle" runat="server" Text="Global Search"/>
                </td>
                <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
             </tr>
             <tr>
                <td class="form" colspan="2">
                    <table width="90%" cellpadding="0">
                    <tr>
                        <td align="center" colspan="4"> <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                    </tr>
					<tr><td><table width='100%'><tr><td class='valign'><table width='100%'>
					<tr>
						<td class="label">Client</td>
						<td><asp:TextBox ID="txtclient"  runat="server" CssClass="textbox"></asp:TextBox></td>
						<td class="label">Contact Person</td>
						<td><asp:TextBox ID="txtcontactperson"  runat="server" CssClass="textbox"></asp:TextBox></td>
						<td class="label">Mobile No</td>
						<td><asp:TextBox ID="txtmobileno"  runat="server" CssClass="textbox"></asp:TextBox></td>
						<td class="label">Email Id</td>
						<td><asp:TextBox ID="txtemailid"  runat="server" CssClass="textbox"></asp:TextBox></td>
					</tr>
					<tr>
					    <td></td>
					    <td><asp:Button ID="btnTrack" runat="server" OnClick="btnTrack_Click" Text="Search" ValidationGroup="vg" CssClass="button"/></td>
					</tr>
				   </table>
				</td>
			    </tr>
			    </table>
		    </td>
	        </tr></table>
	    </td>
	</tr>
	<tr>
	    <td class="title"><asp:Literal runat="server" ID="ltcustomerdetail"></asp:Literal></td>
	</tr>
	</table>
</asp:Content>

