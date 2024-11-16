<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="campaignvssales.aspx.cs" Inherits="campaignvssales" %>
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
            <asp:Label ID="lblPageTitle" runat="server"  Text="Campaign Vs Sales"></asp:Label>
        </td>
        <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
     </tr>
     <tr>
        <td colspan="2">
			<table width='100%'>
			<!--CONTROLS_START-->
					
			<tr><td>
				<asp:PlaceHolder ID="plSearch" runat="server">
				<table>
					
					<tr>
					<td>Campaign</td>
					<td><asp:TextBox ID="enquiry_campaignid" runat="server" m="campaign" cn="campaignname" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtenquiry_campaignid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					<td>Enquiry Date</td>
					<td>From : <asp:TextBox ID="enquiry_enquirydate_from" CssClass="datepicker from" runat="server" Format="Date"/>
						To : <asp:TextBox ID="enquiry_enquirydate_to" CssClass="datepicker to" runat="server" Format="Date"/></td>
					
					</tr>
					<tr>
					<td>Existing Customer</td>
					<td><asp:TextBox ID="enquiry_clientid" runat="server" m="client" cn="customername" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtenquiry_clientid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					<td>Area</td>
					<td><asp:TextBox ID="enquiry_areaid" runat="server" m="area" cn="areaname" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtenquiry_areaid" Text="0" runat="server" class=" hdnac" Format="Int" />
						<asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /><img src='../images/down-arr1.jpg'/></td>
					</tr>
				</table>
				</asp:PlaceHolder></td></tr>
				<tr><td><uc:Grid ID="grid" runat="server" Module="campaignvssales" IsReport="true"/></td></tr>
					<!--CONTROLS_END-->
			</table>
        </td>
     </tr>
     <tr id="trSummary" runat="server" visible=false>
        <td colspan="2">
            <table>
                <tr><td class="label">Actual Expense (Rs.)</td><td align="right"><asp:Label ID="lblActualExpense" runat="server" CssClass="val"></asp:Label></td></tr>
                <tr><td class="label">Expected Sale (Rs.)</td><td align="right"><asp:Label ID="lblExpectedSale" runat="server" CssClass="val"></asp:Label></td></tr>
                <tr><td class="label">Total Sale (Rs.)</td><td align="right"><asp:Label ID="lblTotalSale" runat="server" CssClass="val"></asp:Label></td></tr>
                <tr><td class="label">Difference (Rs.)</td><td align="right"><asp:Label ID="lblDifference" runat="server" CssClass="val"></asp:Label></td></tr>
            </table>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>