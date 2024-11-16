<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="opportunitybyengineer.aspx.cs" Inherits="opportunitybyengineer" %>
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
            <asp:Label ID="lblPageTitle" runat="server"  Text="Opportunity By Engineer"></asp:Label>
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
					<td>Assign To</td>
					<td><asp:TextBox ID="opportunity_employeeid" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtopportunity_employeeid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					<td>Status</td><td><asp:DropDownList ID="opportunity_opportunitystatusid" runat="server"></asp:DropDownList></td>
					
					</tr>
					<tr>
					<td>Opportunity Date</td>
					<td>From : <asp:TextBox ID="opportunity_opportunitydate_from" CssClass="datepicker from" runat="server" Format="Date"/>
						To : <asp:TextBox ID="opportunity_opportunitydate_to" CssClass="datepicker to" runat="server" Format="Date"/>
						<asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /></td>
					</tr>
				</table>
				</asp:PlaceHolder></td></tr>
				<tr><td><uc:Grid ID="grid" runat="server" Module="opportunitybyengineer" IsReport="true" GridSumColumns="quotation_totalamount"/></td></tr>
					<!--CONTROLS_END-->
			</table>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>