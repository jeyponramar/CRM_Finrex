<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="searchinvoice.aspx.cs" Inherits="searchinvoice" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>
<%@ Register TagName="BarChart" TagPrefix="uc" Src="~/usercontrols/BarChart.ascx" %>
<%@ Register TagName="PieChart" TagPrefix="uc" Src="~/usercontrols/PieChart.ascx" %>
<%@ Register TagName="LineChart" TagPrefix="uc" Src="~/usercontrols/LineChart.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--DESIGN_START-->
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server"  Text="Search Invoice"></asp:Label>
        </td>
        <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
     </tr>
     <tr>
        <td colspan="2">
			<table width='100%'>
			<!--CONTROLS_START-->
					
			<tr class='trSearchPanel_mob'><td>
				<asp:PlaceHolder ID="plSearch" runat="server">
				<table width='100%'><tr><td>
					
					<div class='report-field' id='reportfield_clientid' runat='server'>
						<div class='report-label'>Customer Name</div>
					<div class='report-control'><asp:TextBox ID="invoice_clientid" runat="server" m="client" cn="customername" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtinvoice_clientid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr.jpg'/></div>
					</div>
					
					<div class='report-field' id='reportfield_invoicestatusid' runat='server'>
						<div class='report-label'>Status</div>
					<div class='report-control'><asp:DropDownList ID="invoice_invoicestatusid" runat="server"></asp:DropDownList></div>
					</div>
					
					<div class='report-field' id='reportfield_invoicedate' runat='server'>
						<div class='report-label'>Invoice Date</div>
					    <div class='report-label'>From : </div><div class='report-control'><asp:TextBox ID="invoice_invoicedate_from" CssClass="datepicker from" runat="server" Format="Date"/></div>
						<div class='report-label'>To : </div><div class='report-control'><asp:TextBox ID="invoice_invoicedate_to" CssClass="datepicker to" runat="server" Format="Date"/></div>
					</div>
					
				</td></tr>
				<tr>
				    <td>
				        <div class='report-field' id='reportfield_periodfrom' runat='server'>
						    <div class='report-label'>Period From</div>
					        <div class='report-label'>From : </div><div class='report-control'><asp:TextBox ID="invoice_periodfrom_from" CssClass="datepicker from" runat="server" Format="Date"/></div>
						    <div class='report-label'>To : </div><div class='report-control'><asp:TextBox ID="invoice_periodfrom_to" CssClass="datepicker to" runat="server" Format="Date"/></div>
					    </div>
					    <div class='report-field' id='reportfield_periodto' runat='server'>
						    <div class='report-label'>Period To</div>
					        <div class='report-label'>From : </div><div class='report-control'><asp:TextBox ID="invoice_periodto_from" CssClass="datepicker from" runat="server" Format="Date"/></div>
						    <div class='report-label'>To : </div><div class='report-control'><asp:TextBox ID="invoice_periodto_to" CssClass="datepicker to" runat="server" Format="Date"/></div>
					    </div>
					    <div class='btnreport-div'><asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /></div>
				    </td>
				</tr>
				
				</table>
				</asp:PlaceHolder></td></tr>
				<tr><td><uc:Grid ID="grid" runat="server" Module="searchinvoice" IsReport="true"/></td></tr>
					<!--CONTROLS_END-->
			</table>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>