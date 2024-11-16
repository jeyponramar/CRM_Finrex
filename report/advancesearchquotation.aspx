<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="advancesearchquotation.aspx.cs" Inherits="advancesearchquotation" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--DESIGN_START-->
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server"  Text="Advance Search Quotation"></asp:Label>
        </td>
        <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
     </tr>
     <tr>
        <td colspan="2">

				<asp:PlaceHolder ID="plSearch" runat="server">
				<table>
					
					<tr>
					<td>Customer Name</td>
					<td><asp:TextBox ID="quotation_clientid" runat="server" m="client" cn="customername" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox id="txtquotation_clientid" Text="0" runat="server" class=" hdnac" Format="Int" /></td>
					
					<td>Quotation For</td><td><asp:DropDownList ID="quotation_quotationforid" runat="server"></asp:DropDownList></td>
					
					</tr>
					<tr>
					<td>Status</td><td><asp:DropDownList ID="quotation_quotationstatusid" runat="server"></asp:DropDownList></td>
					
					<td>Quotation Date</td>
					<td>From : <asp:TextBox ID="quotation_quotationdate_from" CssClass="datepicker" runat="server" Format="Date"/>
						To : <asp:TextBox ID="quotation_quotationdate_to" CssClass="datepicker" runat="server" Format="Date"/>
						<asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /></td>
				</tr>
				</table>
				</asp:PlaceHolder>
        </td>
     </tr>   
     <tr>
        <td colspan="2">
            <uc:Grid id="grid" runat="server" Module="advancesearchquotation" IsReport="true"/>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>