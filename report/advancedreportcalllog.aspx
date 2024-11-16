<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="advancedreportcalllog.aspx.cs" Inherits="advancedreportcalllog" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>
<%@ Register TagName="BarChart" TagPrefix="uc" Src="~/usercontrols/BarChart.ascx" %>
<%@ Register TagName="PieChart" TagPrefix="uc" Src="~/usercontrols/PieChart.ascx" %>
<%@ Register TagName="LineChart" TagPrefix="uc" Src="~/usercontrols/LineChart.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server"  Text="Advanced Report Call Log"></asp:Label>
        </td>
        <td width="30">
        <asp:HyperLink ID="lnkPrintReport" runat="server" Visible="false" Target="_blank">
        <img src="../images/print-s.png" class="refresh" title="Print report"/>
        </asp:HyperLink>
        </td>
     </tr>
     <tr>
        <td class="title">
            <asp:Label ID="lblMessage" runat="server"  CssClass="error"></asp:Label>
        </td>
     </tr>        
     <tr>
        <td colspan="2">
			<table width='100%'>
			<!--CONTROLS_START-->
					
			<tr><td>
				<asp:PlaceHolder ID="plSearch" runat="server">
				<table width='75%'><tr><td>
					
					<div class='report-field' id='reportfield_clientid' runat='server'>
						<div class='report-label'>Client</div>
					<div class='report-control'><asp:TextBox ID="calllog_clientid" runat="server" m="client" cn="customercode" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtcalllog_clientid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></div>
					</div>
					
					<div class='report-field' id='reportfield_notificationtypeid' runat='server'>
						<div class='report-label'>Notification Type</div>
					<div class='report-control'><asp:DropDownList ID="calllog_notificationtypeid" runat="server"></asp:DropDownList></div>
					</div>
					
					<div class='report-field' id='reportfield_emailsmssentstatusid' runat='server'>
						<div class='report-label'>Status</div>
					<div class='report-control'><asp:DropDownList ID="calllog_emailsmssentstatusid" runat="server"></asp:DropDownList></div>
					</div>
					
					<div class='report-field' id='reportfield_sentdate' runat='server'>
						<div class='report-label'>Sent Date</div>
					<div class='report-label'>From : </div><div class='report-control'><asp:TextBox ID="calllog_sentdate_from" CssClass="datepicker from" runat="server" Format="Date"/></div>
						<div class='report-label'>To : </div><div class='report-control'><asp:TextBox ID="calllog_sentdate_to" CssClass="datepicker to" runat="server" Format="Date"/></div>
					</div>
					
					<div class='report-field' id='reportfield_employeeid' runat='server'>
						<div class='report-label'>Sent By</div>
					<div class='report-control'><asp:TextBox ID="calllog_employeeid" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtcalllog_employeeid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></div>
					</div>
						<div class='report-field'><asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /></div>
				</td></tr></table>
				</asp:PlaceHolder></td></tr>
				<tr><td><uc:Grid ID="grid" runat="server" Module="advancedreportcalllog" IsReport="true"/></td></tr>
					<!--CONTROLS_END-->
			</table>
        </td>
     </tr>
</table>
</asp:Content>