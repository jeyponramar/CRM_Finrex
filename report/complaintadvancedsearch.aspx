<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="complaintadvancedsearch.aspx.cs" Inherits="complaintadvancedsearch" %>
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
            <asp:Label ID="lblPageTitle" runat="server"  Text="Complaint Advanced Search"></asp:Label>
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
					<td><asp:TextBox ID="complaint_clientid" runat="server" m="client" cn="customername" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtcomplaint_clientid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					<td>Complaint Type</td>
					<td><asp:TextBox ID="complaint_complainttypeid" runat="server" m="complainttype" cn="complainttype" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtcomplaint_complainttypeid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					</tr>
					<tr>
					<td>Complaint Chargeable</td>
					<td><asp:TextBox ID="complaint_chargeableid" runat="server" m="chargeable" cn="chargeable" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtcomplaint_chargeableid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					<td>Complaint Date</td>
					<td>From : <asp:TextBox ID="complaint_complaintdate_from" CssClass="datepicker from" runat="server" Format="Date"/>
						To : <asp:TextBox ID="complaint_complaintdate_to" CssClass="datepicker to" runat="server" Format="Date"/></td>
					
					</tr>
					<tr>
					<td>Complaint Mode</td>
					<td><asp:TextBox ID="complaint_complaintmodeid" runat="server" m="complaintmode" cn="complaintmode" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtcomplaint_complaintmodeid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					<td>Priority</td>
					<td><asp:TextBox ID="complaint_priorityid" runat="server" m="priority" cn="priority" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtcomplaint_priorityid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					</tr>
					<tr>
					<td>Status</td><td><asp:DropDownList ID="complaint_statusid" runat="server"></asp:DropDownList></td>
					
					<td>Assigned To</td>
					<td><asp:TextBox ID="complaint_employeeid" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtcomplaint_employeeid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					</tr>
					<tr>
					<td>Assigned Date</td>
					<td>From : <asp:TextBox ID="complaint_assigneddate_from" CssClass="datepicker from" runat="server" Format="Date"/>
						To : <asp:TextBox ID="complaint_assigneddate_to" CssClass="datepicker to" runat="server" Format="Date"/>
						<asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /></td>
					</tr>
				</table>
				</asp:PlaceHolder></td></tr>
				<tr><td><uc:Grid ID="grid" runat="server" Module="complaintadvancedsearch" IsReport="true"/></td></tr>
					<!--CONTROLS_END-->
			</table>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>