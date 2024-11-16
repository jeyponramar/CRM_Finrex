<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="quotationbycategory.aspx.cs" Inherits="quotationbycategory" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--DESIGN_START-->
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server"  Text="Quotation By Category"></asp:Label>
        </td>
        <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
     </tr>
     <tr>
        <td colspan="2">

				<asp:PlaceHolder ID="plSearch" runat="server">
				<table>
					
					<tr>
					<td>Quotation For</td><td><asp:DropDownList ID="quotation_quotationforid" runat="server"></asp:DropDownList></td>
					
					<td>Quotation Date</td>
					<td>From : <asp:TextBox ID="quotation_quotationdate_from" CssClass="datepicker" runat="server" Format="Date"/>
						To : <asp:TextBox ID="quotation_quotationdate_to" CssClass="datepicker" runat="server" Format="Date"/>
						<asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClick="btnReport_Click" /></td>
				</tr>
				</table>
				</asp:PlaceHolder>
        </td>
     </tr>   
     <tr>
        <td colspan="2">
            <uc:Grid id="grid" runat="server" Module="quotationbycategory" IsReport="true"/>
        </td>
     </tr>
</table>
<!--DESIGN_END-->
</asp:Content>