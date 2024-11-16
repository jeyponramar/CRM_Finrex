<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="assignedservicebyengineer.aspx.cs" Inherits="assignedservicebyengineer" %>
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
            <asp:Label ID="lblPageTitle" runat="server"  Text="Assigned Service by Engineer"></asp:Label>
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
					<td>Assigned To</td>
					<td><asp:TextBox ID="amcservice_employeeid" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtamcservice_employeeid" Text="0" runat="server" class=" hdnac" Format="Int" />
						<asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /><img src='../images/down-arr1.jpg'/></td>
					</tr>
				</table>
				</asp:PlaceHolder></td></tr>
				<tr><td><uc:Grid ID="grid" runat="server" Module="assignedservicebyengineer" IsReport="true"/></td></tr>
					<!--CONTROLS_END-->
			</table>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>