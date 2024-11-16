<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="advancedsearchinvoice.aspx.cs" Inherits="advancedsearchinvoice" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--DESIGN_START-->
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server"  Text="Advanced Search Invoice"></asp:Label>
        </td>
        <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
     </tr>
     <tr>
        <td colspan="2">

				<asp:PlaceHolder ID="plSearch" runat="server">
				<table>
					
					<tr>
					<td>Customer Name</td>
					<td><asp:TextBox ID="invoice_clientid" runat="server" m="client" cn="customername" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox id="txtinvoice_clientid" Text="0" runat="server" class=" hdnac" Format="Int" /></td>
					
					<td>Invoice Date</td>
					<td>From : <asp:TextBox ID="invoice_invoicedate_from" CssClass="datepicker" runat="server" Format="Date"/>
						To : <asp:TextBox ID="invoice_invoicedate_to" CssClass="datepicker" runat="server" Format="Date"/></td>
					
					</tr>
					<tr>
					<td>Invoice for</td><td><asp:DropDownList ID="invoice_invoiceforid" runat="server"></asp:DropDownList>
						<asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /></td>
				</tr>
				</table>
				</asp:PlaceHolder>
        </td>
     </tr>   
     <tr>
        <td colspan="2">
            <uc:Grid id="grid" runat="server" Module="advancedsearchinvoice" IsReport="true"/>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>