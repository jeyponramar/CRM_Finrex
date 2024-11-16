<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="advancedreportsubscription.aspx.cs" Inherits="advancedreportsubscription" %>
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
            <asp:Label ID="lblPageTitle" runat="server"  Text="Advanced Report Subscription"></asp:Label>
        </td>
        <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
     </tr>
     <tr>
        <td colspan="2">
			<table width='100%'>
			<!--CONTROLS_START-->
					
			<tr><td>
				<asp:PlaceHolder ID="plSearch" runat="server">
				<table width='95%'><tr><td>
					
					<div class='report-field' id='reportfield_clientid' runat='server'>
						<div class='report-label'>Customer Name</div>
					<div class='report-control'><asp:TextBox ID="subscription_clientid" runat="server" m="client" cn="customercode" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtsubscription_clientid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></div>
					</div>
					
					<div class='report-field' id='reportfield_subscriptionstatusid' runat='server'>
						<div class='report-label'>Subscription Status</div>
					<div class='report-control'><asp:DropDownList ID="subscription_subscriptionstatusid" runat="server" Width="120"></asp:DropDownList></div>
					</div>
					
					<div class='report-field' id='reportfield_startdate' runat='server'>
						<div class='report-label'>Software Start Date</div>
					<div class='report-label'>From : </div><div class='report-control'><asp:TextBox ID="subscription_startdate_from" CssClass="datepicker from" runat="server" Format="Date"/></div>
						<div class='report-label'>To : </div><div class='report-control'><asp:TextBox ID="subscription_startdate_to" CssClass="datepicker to" runat="server" Format="Date"/></div>
					</div>
					
					
				</td></tr>
				<tr>
				    <td>
				        <div class='report-field' id='reportfield_enddate' runat='server'>
						<div class='report-label'>Software End Date</div>
					<div class='report-label'>From : </div><div class='report-control'><asp:TextBox ID="subscription_enddate_from" CssClass="datepicker from" runat="server" Format="Date"/></div>
						<div class='report-label'>To : </div><div class='report-control'><asp:TextBox ID="subscription_enddate_to" CssClass="datepicker to" runat="server" Format="Date"/></div>
					</div>
					<div class='report-field' id='reportfield_employeeid' runat='server' visible="false">
						<div class='report-label'>Assigned To</div>
					    <div class='report-control'>
					        <asp:TextBox ID="subscription_employeeid" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						    <asp:TextBox ID="txtsubscription_employeeid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/>
					    </div>
					</div>
					
				    </td>
				</tr>
				<tr>
				    <td>
				        <div class='report-field' id='reportfield_periodfrom' runat='server'>
						    <div class='report-label'>Invoice Period From</div>
					        <div class='report-label'>From : </div><div class='report-control'><asp:TextBox ID="subscription_invoiceperiodfrom_from" CssClass="datepicker from" runat="server" Format="Date"/></div>
						    <div class='report-label'>To : </div><div class='report-control'><asp:TextBox ID="subscription_invoiceperiodfrom_to" CssClass="datepicker to" runat="server" Format="Date"/></div>
					    </div>
					    <div class='report-field' id='reportfield_periodto' runat='server'>
						    <div class='report-label'>Invoice Period To</div>
					        <div class='report-label'>From : </div><div class='report-control'><asp:TextBox ID="subscription_invoiceperiodto_from" CssClass="datepicker from" runat="server" Format="Date"/></div>
						    <div class='report-label'>To : </div><div class='report-control'><asp:TextBox ID="subscription_invoiceperiodto_to" CssClass="datepicker to" runat="server" Format="Date"/></div>
					    </div>
					    <div class='report-field' id='reportfield_latestinvoicedate' runat='server'>
						    <div class='report-label'>Invoice Date</div>
					        <div class='report-label'>From : </div><div class='report-control'><asp:TextBox ID="subscription_latestinvoicedate_from" CssClass="datepicker from" runat="server" Format="Date"/></div>
						    <div class='report-label'>To : </div><div class='report-control'><asp:TextBox ID="subscription_latestinvoicedate_to" CssClass="datepicker to" runat="server" Format="Date"/></div>
					    </div>
				       
				        <div class='report-field'><asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /></div>
				    </td>
				</tr>
				</table>
				</asp:PlaceHolder></td></tr>
				<tr><td><uc:Grid ID="grid" runat="server" Module="advancedreportsubscription" IsReport="true"/></td></tr>
					<!--CONTROLS_END-->
			</table>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>