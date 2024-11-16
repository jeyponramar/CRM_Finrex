<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="scheduledsalesservices.aspx.cs" Inherits="scheduledsalesservices" %>
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
            <asp:Label ID="lblPageTitle" runat="server"  Text="Scheduled Sales Services"></asp:Label>
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
					<td>Customer Name</td>
					<td><asp:TextBox ID="amcservice_clientid" runat="server" m="client" cn="customername" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtamcservice_clientid" Text="0" runat="server" class=" hdnac" Format="Int" />
						<asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /><img src='../images/down-arr1.jpg'/></td>
					</tr>
				</table>
				</asp:PlaceHolder></td></tr>
				<tr><td><uc:Grid ID="grid" runat="server" Module="scheduledsalesservices" IsReport="true"/></td></tr>
					<!--CONTROLS_END-->
			</table>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>