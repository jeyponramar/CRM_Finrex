<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="advancedwhatsappreport.aspx.cs" Inherits="advancedwhatsappreport" %>
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
            <asp:Label ID="lblPageTitle" runat="server"  Text="Advanced Whatsapp Report"></asp:Label>
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
					
					<div class='report-field' id='reportfield_clientid' runat='server'>
						<div class='report-label'>Customer Name</div>
					<div class='report-control'><asp:TextBox ID="client_clientid" runat="server" m="client" cn="customername" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtclient_clientid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></div>
					</div>
					<div class='report-field' id='reportfield_date' runat='server'>
						<div class='report-label'>Start Date</div>
					<div class='report-label'>From : </div><div class='report-control'><asp:TextBox ID="client_startdate_from" CssClass="datepicker from" runat="server" Format="Date"/></div>
						<div class='report-label'>To : </div><div class='report-control'><asp:TextBox ID="client_startdate_to" CssClass="datepicker to" runat="server" Format="Date"/></div>
					</div>
					<div class='report-field' id='Div1' runat='server'>
						<div class='report-label'>Whatsapp End Date</div>
					<div class='report-label'>From : </div><div class='report-control'><asp:TextBox ID="client_whatsappenddate_from" CssClass="datepicker from" runat="server" Format="Date"/></div>
						<div class='report-label'>To : </div><div class='report-control'><asp:TextBox ID="client_whatsappenddate_to" CssClass="datepicker to" runat="server" Format="Date"/></div>
					</div>
					<div class='report-field' id='Div2' runat='server'>
						<div class='report-label'>Assigned To</div>
					    <div class='report-control'><asp:TextBox ID="TextBox1" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtclient_employeeid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></div>
					</div>
					<div class='report-field' id='Div3' runat='server'>
						<div class='report-label'>Status</div>
					    <div class='report-control'><asp:TextBox ID="TextBox2" runat="server" m="subscriptionstatus" cn="status" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtclient_subscriptionstatusid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></div>
						<div class='report-field'><asp:Button ID="Button1" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /></div>
					</div>
				</td></tr></table>
				</asp:PlaceHolder></td></tr>
				<tr><td><uc:Grid ID="grid" runat="server" Module="advancedwhatsappreport" IsReport="true"/></td></tr>
					<!--CONTROLS_END-->
			</table>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>