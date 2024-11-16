<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="enquiryadvancedsearch.aspx.cs" Inherits="enquiryadvancedsearch" %>
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
            <asp:Label ID="lblPageTitle" runat="server"  Text="Enquiry Advanced Search"></asp:Label>
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
					<td>Enquiry Date</td>
					<td>From : <asp:TextBox ID="enquiry_enquirydate_from" CssClass="datepicker from" runat="server" Format="Date"/>
						To : <asp:TextBox ID="enquiry_enquirydate_to" CssClass="datepicker to" runat="server" Format="Date"/></td>
					
					<td>Existing Customer</td>
					<td><asp:TextBox ID="enquiry_clientid" runat="server" m="client" cn="customername" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtenquiry_clientid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					</tr>
					<tr>
					<td>Campaign</td>
					<td><asp:TextBox ID="enquiry_campaignid" runat="server" m="campaign" cn="campaignname" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtenquiry_campaignid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					<td>Designation</td>
					<td><asp:TextBox ID="enquiry_designationid" runat="server" m="designation" cn="designation" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtenquiry_designationid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					</tr>
					<tr>
					<td>Product</td>
					<td><asp:TextBox ID="enquiry_productid" runat="server" m="product" cn="productname" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtenquiry_productid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					<td>City</td>
					<td><asp:TextBox ID="enquiry_cityid" runat="server" m="city" cn="cityname" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtenquiry_cityid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					</tr>
					<tr>
					<td>Area</td>
					<td><asp:TextBox ID="enquiry_areaid" runat="server" m="area" cn="areaname" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtenquiry_areaid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					<td>Communication Source</td>
					<td><asp:TextBox ID="enquiry_communicationsourceid" runat="server" m="communicationsource" cn="communicationsource" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtenquiry_communicationsourceid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					</tr>
					<tr>
					<td>Enquiry Status</td><td><asp:DropDownList ID="enquiry_enquirystatusid" runat="server"></asp:DropDownList></td>
					
					<td>Enquiry For</td>
					<td><asp:TextBox ID="enquiry_enquiryforid" runat="server" m="enquiryfor" cn="enquiryfor" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtenquiry_enquiryforid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></td>
					
					</tr>
					<tr>
					<td>Followups Date</td>
					<td>From : <asp:TextBox ID="enquiry_followupsdate_from" CssClass="datepicker from" runat="server" Format="Date"/>
						To : <asp:TextBox ID="enquiry_followupsdate_to" CssClass="datepicker to" runat="server" Format="Date"/></td>
					
					<td>Visits Date</td>
					<td>From : <asp:TextBox ID="enquiry_visitsdate_from" CssClass="datepicker from" runat="server" Format="Date"/>
						To : <asp:TextBox ID="enquiry_visitsdate_to" CssClass="datepicker to" runat="server" Format="Date"/></td>
					
					</tr>
					<tr>
					<td>Assigned To</td>
					<td><asp:TextBox ID="enquiry_employeeid" runat="server" m="employee" cn="employeename" cm="marketingexecutive" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtenquiry_employeeid" Text="0" runat="server" class=" hdnac" Format="Int" />
						<asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /><img src='../images/down-arr1.jpg'/></td>
					</tr>
				</table>
				</asp:PlaceHolder></td></tr>
				<tr><td><uc:PieChart ID="chart" runat="server" ColumnName="" Module="enquiryadvancedsearch" ColumnHeader="" BarColor="" /></td></tr>
				<tr><td><uc:Grid ID="grid" runat="server" Module="enquiryadvancedsearch" IsReport="true"/></td></tr>
					<!--CONTROLS_END-->
			</table>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>