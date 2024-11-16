<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="advancedreportkyc.aspx.cs" Inherits="advancedreportkyc" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>
<%@ Register TagName="BarChart" TagPrefix="uc" Src="~/usercontrols/BarChart.ascx" %>
<%@ Register TagName="PieChart" TagPrefix="uc" Src="~/usercontrols/PieChart.ascx" %>
<%@ Register TagName="LineChart" TagPrefix="uc" Src="~/usercontrols/LineChart.ascx" %>
<%@ Register Src="~/Usercontrols/MultiCheckbox.ascx" TagName="MultiCheckbox" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style>
.ac{min-width:170px;width:170px;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--DESIGN_START-->
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server"  Text="Advanced Report - KYC"></asp:Label>
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
					
					<div class='report-field' id='reportfield_cityid' runat='server'>
						<div class='report-label'>State</div>
					<div class='report-control'><asp:TextBox ID="client_stateid" runat="server" m="state" cn="state" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtclient_stateid" Text="0" dcn="client_stateid" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></div>
					</div>
					
					<div class='report-field' id='Div1' runat='server'>
						<div class='report-label'>City</div>
					<div class='report-control'><asp:TextBox ID="TextBox1" runat="server" acparent="stateid" m="city" cn="cityname" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtclient_cityid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></div>
					</div>
					
					<div class='report-field' id='Div2' runat='server'>
						<div class='report-label'>Industry</div>
					<div class='report-control'><asp:TextBox ID="TextBox2" runat="server" m="industrytypes" cn="industrytypes" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtclient_industrytypesid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></div>
					</div>
					
					<div class='report-field' id='Div3' runat='server'>
						<div class='report-label'>Business Into</div>
					    <div class='report-control'><asp:TextBox ID="TextBox7" runat="server" m="business" cn="business" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						    <asp:TextBox ID="txtclient_businessid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/>
						</div>
					</div>
					
					<div class='report-field' id='Div4' runat='server'>
						<div class='report-label'>Business Type</div>
					    <div class='report-control'><asp:TextBox ID="TextBox9" runat="server" m="businesstype" cn="businesstype" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						    <asp:TextBox ID="txtclient_businesstypeid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/>
						</div>
					</div>
					
					<div class='report-field' id='Div5' runat='server'>
						<div class='report-label'>Export Annual Turnover</div>
						<div class='report-control'><asp:TextBox ID="TextBox11" runat="server" m="annualturnover" cn="turnover" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						    <asp:TextBox ID="txtclient_exportannualturnoverid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/>
						</div>
					</div>
					
					<div class='report-field' id='Div6' runat='server'>
						<div class='report-label'>Import Annual Turnover</div>
					    <div class='report-control'><asp:TextBox ID="TextBox13" runat="server" m="annualturnover" cn="turnover" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						    <asp:TextBox ID="txtclient_importannualturnoverid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/>
						</div>
					</div>
					
					<div class='report-field' id='Div8' runat='server'>
						<div class='report-label'>Enterprise Type</div>
					    <div class='report-control'><asp:TextBox ID="TextBox3" runat="server" m="enterprisetype" cn="enterprisetype" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						    <asp:TextBox ID="txtclient_enterprisetypeid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/>
						</div>
					</div>
					
					<div class='report-field' id='Div9' runat='server'>
						<div class='report-label'>Importing/Exporting Country</div>
					    <div class='report-control'><asp:TextBox ID="TextBox15" runat="server" m="country" cn="country" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						    <asp:TextBox ID="txtclient_importexportcountryid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/>
						</div>
					</div>
					
					<div class='report-field' id='Div10' runat='server'>
						<div class='report-label'>Currency Dealing in</div>
					    <div class='report-control'><asp:TextBox ID="TextBox17" runat="server" m="bankauditcurrency" cn="currency" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						    <asp:TextBox ID="txtclient_currencydealinginid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/>
						</div>
					</div>
					
					<div class='report-field' id='Div11' runat='server'>
						<div class='report-label'>Hedging Policy</div>
					<div class='report-control'><asp:TextBox ID="TextBox4" runat="server" m="hedgingpolicymaster" cn="hedgingpolicy" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtclient_hedgingpolicyid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></div>
					</div>
					
					<div class='report-field' id='Div12' runat='server'>
						<div class='report-label'>Exposure Sheet maintained</div>
					<div class='report-control'><asp:TextBox ID="TextBox6" runat="server" m="exposuresoftwaremaster" cn="exposuresoftware" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtclient_exposuresoftwareid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></div>
					</div>
					
					<div class='report-field' id='Div14' runat='server'>
						<div class='report-label'>Forward Limits</div>
					<div class='report-control'><asp:TextBox ID="TextBox10" runat="server" m="forwardlimitmaster" cn="forwardlimit" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtclient_forwardlimitid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></div>
					</div>
					
					<div class='report-field' id='Div15' runat='server'>
						<div class='report-label'>Forward Contract Booking</div>
					<div class='report-control'><asp:TextBox ID="TextBox12" runat="server" m="forwardcontractbookingmaster" cn="forwardcontractbooking" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtclient_forwardcontractbookingid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></div>
					</div>
					
					<div class='report-field' id='Div16' runat='server'>
						<div class='report-label'>Type of Booking</div>
					<div class='report-control'><asp:TextBox ID="TextBox14" runat="server" m="bookingtypemaster" cn="bookingtype" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtclient_bookingtypeid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></div>
					</div>
					
					<div class='report-field' id='Div13' runat='server'>
						<div class='report-label'>Type of Funding</div>
					<div class='report-control'><asp:TextBox ID="TextBox5" runat="server" m="fundingtypemaster" cn="fundingtype" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtclient_fundingtypeid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></div>
					</div>
					
					<div class='report-field' id='Div17' runat='server'>
						<div class='report-label'>Bank</div>
					<div class='report-control'><asp:TextBox ID="TextBox8" runat="server" m="bankauditbank" cn="bankname" CssClass="textbox ac txtac" Include="0"></asp:TextBox>
						<asp:TextBox ID="txtclient_bankauditbankid" Text="0" runat="server" class=" hdnac" Format="Int" /><img src='../images/down-arr1.jpg'/></div>
					</div>
					
					<div class='report-field' id='Div18' runat='server'>
						<div class='report-label'>Sanctioned Letter Renewal date</div>
					    <div class='report-label'>From : </div><div class='report-control'><asp:TextBox ID="client_sanctionedletterrenewaldate_from" CssClass="datepicker from" runat="server" Format="Date"/></div>
						<div class='report-label'>To : </div><div class='report-control'><asp:TextBox ID="client_sanctionedletterrenewaldate_to" CssClass="datepicker to" runat="server" Format="Date"/></div>
					</div>
					
					<div class='report-field' id='Div7' runat='server'>
						<div class='report-label'>KYC Updated?</div>
					    <div class='report-control'>
					        <asp:DropDownList ID="ddlclient_iskycupdated" runat="server" CssClass="ddl" Width="80">
					            <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
					            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
					            <asp:ListItem Text="No" Value="0"></asp:ListItem>
					        </asp:DropDownList>
					    </div>
					</div>
					
					<div class='report-field'><asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /></div>
					
				</td></tr>
				
				</table>
				</asp:PlaceHolder></td></tr>
				<tr><td><uc:Grid ID="grid" runat="server" Module="advancedreportkyc" IsReport="true"/></td></tr>
					<!--CONTROLS_END-->
			</table>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>