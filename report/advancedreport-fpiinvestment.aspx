<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="advancedreport-fpiinvestment.aspx.cs" Inherits="advancedreport_fpiinvestment" %>
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
            <asp:Label ID="lblPageTitle" runat="server"  Text="View FPI Investment"></asp:Label>
        </td>
        <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
     </tr>
     <tr>
        <td colspan="2">
			<table width='100%'>
			<!--CONTROLS_START-->
					
			<tr><td>
				<asp:PlaceHolder ID="plSearch" runat="server">
				<table width='75%'><tr><td>
					
					<div class='report-field' id='reportfield_date' runat='server'>
						<div class='report-label'>Date</div>
					<div class='report-label'>From : </div><div class='report-control'><asp:TextBox ID="fpiinvestment_date_from" CssClass="datepicker from" runat="server" Format="Date"/></div>
						<div class='report-label'>To : </div><div class='report-control'><asp:TextBox ID="fpiinvestment_date_to" CssClass="datepicker to" runat="server" Format="Date"/></div>
					</div>
					<div class='report-field'><asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /></div>
				</td></tr></table>
				</asp:PlaceHolder></td></tr>
				<tr><td><uc:Grid ID="grid" runat="server" Module="advancedreportfpiinvestment" IsReport="true"/></td></tr>
					<!--CONTROLS_END-->
			</table>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>