 <%@ Page Language="C#" MasterPageFile="~/Mobile/MasterPage.master" AutoEventWireup="true" 
 CodeFile="~/FPIInvestment/add.aspx.cs" Inherits="FPIInvestment_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%><%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        SetDetailPage('<%=Request.QueryString["id"]%>');
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--DESIGN_START-->
    <asp:PlaceHolder ID="form" runat="server">
    <table width="100%">
         <tr>
            <td class="title">
                <asp:Label ID="lblPageTitle" runat="server"/>
            </td>
         </tr>
         <tr>
            <td>
                <input type="button" value="Edit" class="edit button dpage"/>
                <input type="button" value="Copy" class="copy button dpage"/>
                <asp:TextBox ID="h_IsCopy" runat="server" CssClass="iscopy hidden" Text="0"></asp:TextBox>
                <!--ACTION_START--><!--ACTION_END-->
            </td>
            <td align="right"> 
                <uc:NextPrevDetail ID="NextPrevDetail" runat="server" />
            </td>
         </tr>
         <tr>
            <td class="form" colspan="2">
                <table width="100%" cellpadding="0">
                <tr>
                    <td align="center"><asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                </tr>
                <tr>
                <td>
				<!--CONTROLS_START-->
					<table width='100%' cellpadding='5' cellspacing='0' id='tblcontrols2' runat='server'>
					<tr>
					<td><div class='label'>Date <span class="error">*</span></div>
						<div><asp:TextBox ID="txtdate"  dcn="fpiinvestment_date" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv8" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Date" ValidationGroup="vg" ControlToValidate="txtdate"></asp:RequiredFieldValidator></div></td>
				</tr>
					<tr>
					<td><div class='label'>Equity <span class="error">*</span></div>
						<div><asp:TextBox ID="txtequity"  dcn="fpiinvestment_equity" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv9" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Equity" ValidationGroup="vg" ControlToValidate="txtequity"></asp:RequiredFieldValidator></div></td>
				</tr>
					<tr>
					<td><div class='label'>Debt <span class="error">*</span></div>
						<div><asp:TextBox ID="txtdebt"  dcn="fpiinvestment_debt" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv10" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Debt" ValidationGroup="vg" ControlToValidate="txtdebt"></asp:RequiredFieldValidator></div></td>
				</tr>
					<tr>
					<td><div class='label'>Debt VRR <span class="error">*</span></div>
						<div><asp:TextBox ID="txtdebtvrr"  dcn="fpiinvestment_debtvrr" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv11" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Debt VRR" ValidationGroup="vg" ControlToValidate="txtdebtvrr"></asp:RequiredFieldValidator></div></td>
				</tr>
					<tr>
					<td><div class='label'>Hybrid <span class="error">*</span></div>
						<div><asp:TextBox ID="txthybrid"  dcn="fpiinvestment_hybrid" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv12" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Hybrid" ValidationGroup="vg" ControlToValidate="txthybrid"></asp:RequiredFieldValidator></div></td>
				</tr>
					<tr>
					<td><div class='label'>Debt – FAR</div>
						<div><asp:TextBox ID="txtdebtfar"  dcn="fpiinvestment_debtfar" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></div></td>
				</tr>
					<tr>
					<td><div class='label'>Mutual Fund</div>
						<div><asp:TextBox ID="txtmutualfund"  dcn="fpiinvestment_mutualfund" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></div></td>
				</tr>
					<tr>
					<td><div class='label'>AIFS</div>
						<div><asp:TextBox ID="txtaifs"  dcn="fpiinvestment_aifs" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></div></td>
				</tr>
					</table>
					<!--CONTROLS_END-->
				</td>
				</tr>
                </table>
            </td>
         </tr>
		 <tr>
            <td align="center" colspan="2">
				<!--SAVEBUTTON_START-->
					<asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Save" CssClass="save button" ValidationGroup="vg"/>
				<asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" CssClass="delete button" Visible="false"/>
				<input type="button" class="close-page cancel" value="Cancel"/>
				<asp:Button ID="btnSubmitAndView" runat="server" Visible="false"/>
					<!--SAVEBUTTON_END-->
            </td>
        </tr>
    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
