<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="add.aspx.cs" Inherits="trackclient_add" Title="Untitled Page" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
             <tr>
                <td class="title">
                    <asp:Label ID="lblPageTitle" runat="server" Text="Track Client"/>
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
						<td class="label">Client <span class="error">*</span></td>
						<td><asp:TextBox ID="client"  dcn="client_customername" runat="server" m="client" cn="customername" CssClass="textbox txtqa ac txtac"></asp:TextBox>
						<asp:TextBox id="txtclientid"  dcn="amc_clientid"  Text="0" runat="server" class=" hdnac hdnqa"/>
						<img src="../images/down-arrow.png" class="epage"/>
						<asp:RequiredFieldValidator ID="rfv5" Display="Dynamic" runat="server" ErrorMessage="Required Client" ValidationGroup="vg" ControlToValidate="client"></asp:RequiredFieldValidator></td>
					</tr>
					<tr>
					    <td></td>
					    <td><asp:Button ID="btnTrack" runat="server" OnClick="btnTrack_Click" Text="Track" ValidationGroup="vg" CssClass="button"/></td>
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
	    <td class="title">Open Enquiry</td>
	</tr>
	<tr><td><uc:Grid id="gridenquiry" runat="server" Module="trackenquiry"/></td></tr>
	<tr>
	    <td class="title">Open Opportunity</td>
	</tr>
	<tr><td><uc:Grid id="gridopportunity" runat="server" Module="trackopportunity"/></td></tr>
	<tr>
	    <td class="title">Open Quotation</td>
	</tr>
	<tr><td><uc:Grid id="gridQuotation" runat="server" Module="trackquotation"/></td></tr>
	<tr>
	    <td class="title">Open Sales</td>
	</tr>
	<tr><td><uc:Grid id="gridSales" runat="server" Module="tracksales"/></td></tr>
	<tr>
	    <td class="title">Payment Pending Invoice</td>
	</tr>
	<tr><td><uc:Grid id="gridinvoice" runat="server" Module="trackinvoice"/></td></tr>
	<tr>
	    <td class="title">Followup Against Enquiry</td>
	</tr>
	<tr><td><uc:Grid id="gridenquiryfollowups" runat="server" Module="trackenquiryfollowups"/></td></tr>
	<tr>
	    <td class="title">Followup Against Opportunity</td>
	</tr>
	<tr><td><uc:Grid id="gridopportunityfollowups" runat="server" Module="trackopportunityfollowups"/></td></tr>
	<tr>
	    <td class="title">Followup Against client</td>
	</tr>
	<tr><td><uc:Grid id="gridfollowup" runat="server" Module="trackfollowupsclient"/></td></tr>
	</table>
				   	
</asp:Content>

