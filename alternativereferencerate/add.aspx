 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="AlternativeReferenceRate_add" EnableEventValidation="false" ValidateRequest="false"%>
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
                <table width="90%" cellpadding="0">
                <tr>
                    <td align="center"><asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                </tr>
				<!--CONTROLS_START-->
					<table width='100%' cellpadding='0' cellspacing='0' id='tblcontrols1' runat='server'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
										<tr>
											<td class="label">Currency <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="currency"  search='true'  dcn="currency_currency" MaxLength="100" runat="server" m="currency" cn="currency" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtcurrencyid"  dcn="alternativereferencerate_currencyid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Currency" ValidationGroup="vg" ControlToValidate="currency"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">ARR <span class="error">*</span></td>
											<td ti='1'><asp:DropDownList ID="ddlarrmasterid"  dcn="alternativereferencerate_arrmasterid"  search='true' runat="server" m="arrmaster" cn="name" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required ARR" ValidationGroup="vg" ControlToValidate="ddlarrmasterid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Date</td>
											<td ti='2'><asp:TextBox ID="txtdate"  dcn="alternativereferencerate_date" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">O/n</td>
											<td ti='3'><asp:TextBox ID="txton"  MaxLength="100" runat="server"  dcn="alternativereferencerate_on" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">1 Week</td>
											<td ti='4'><asp:TextBox ID="txt1week"  MaxLength="100" runat="server"  dcn="alternativereferencerate_1week" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">1-month TERM  / Average</td>
											<td ti='5'><asp:TextBox ID="txt1monthtermaverage"  MaxLength="100" runat="server"  dcn="alternativereferencerate_1monthtermaverage" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">3-month TERM  / Average</td>
											<td ti='6'><asp:TextBox ID="txt3monthtermaverage"  MaxLength="100" runat="server"  dcn="alternativereferencerate_3monthtermaverage" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">6-month TERM  / Average</td>
											<td ti='7'><asp:TextBox ID="txt6monthtermaverage"  MaxLength="100" runat="server"  dcn="alternativereferencerate_6monthtermaverage" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">12-month TERM  / Average</td>
											<td ti='8'><asp:TextBox ID="txt12monthtermaverage"  MaxLength="100" runat="server"  dcn="alternativereferencerate_12monthtermaverage" CssClass="textbox"></asp:TextBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					</table>
					<!--CONTROLS_END-->
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
