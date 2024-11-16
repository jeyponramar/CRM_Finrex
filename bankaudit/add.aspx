 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="BankAudit_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%>
<%@ Register Src="~/Usercontrols/MultiCheckbox.ascx" TagName="MultiCheckbox" TagPrefix="uc" %>
<%--CONTROLREGISTER_END--%>
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
											<td class="label">Client <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="client"  search='true'  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtclientid"  dcn="bankaudit_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Client" ValidationGroup="vg" ControlToValidate="client"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Date</td>
											<td ti='1'><asp:TextBox ID="txtdate"  dcn="bankaudit_date" runat="server" autocomplete="off" MaxLength="20" Format="DateTime" CssClass="textbox datetimepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Business Into</td>
											<td ti='2'><asp:TextBox ID="txtbusinessinto"  MaxLength="100" runat="server"  dcn="bankaudit_businessinto" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Bank</td>
											<td ti='3'><asp:TextBox ID="bank"  search='true'  dcn="bank_bankname" MaxLength="100" runat="server" m="bank" cn="bankname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtbankid"  dcn="bankaudit_bankid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Bank Branch</td>
											<td ti='4'><asp:TextBox ID="txtbankbranch"  search='true'  MaxLength="100" runat="server"  dcn="bankaudit_bankbranch" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Status <span class="error">*</span></td>
											<td ti='5'><asp:TextBox ID="bankauditstatus"  search='true'  dcn="bankauditstatus_status" MaxLength="100" runat="server" m="bankauditstatus" cn="status" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtbankauditstatusid"  dcn="bankaudit_bankauditstatusid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv5" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Status" ValidationGroup="vg" ControlToValidate="bankauditstatus"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Currency</td>
											<td ti='6'><uc:MultiCheckbox ID="mccurrency"  Module="bankauditcurrency" Column="bankauditcurrency_currency"  TargetModule="currency" runat="server" ></uc:MultiCheckbox></td>
										</tr>
										<tr>
											<td class="label">Invoice Count Per Month</td>
											<td ti='7'><asp:TextBox ID="txtinvoicecountpermonth"  dcn="bankaudit_invoicecountpermonth" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Avg PCFC Amount</td>
											<td ti='8'><asp:TextBox ID="txtavgpcfcamount"  dcn="bankaudit_avgpcfcamount" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Forex Expected Saving</td>
											<td ti='9'><asp:TextBox ID="txtforexexpectedsaving"  dcn="bankaudit_forexexpectedsaving" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Forex Remarks</td>
											<td ti='10'><asp:TextBox TextMode="MultiLine" ID="txtforexremarks"  dcn="bankaudit_forexremarks"  runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Forward Contract Expected Saving</td>
											<td ti='11'><asp:TextBox ID="txtforwardcontractexpectedsaving"  dcn="bankaudit_forwardcontractexpectedsaving" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Forward Contract Remarks</td>
											<td ti='12'><asp:TextBox TextMode="MultiLine" ID="txtforwardcontractremarks"  dcn="bankaudit_forwardcontractremarks"  runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Last Updated Date</td>
											<td ti='13'><asp:TextBox ID="txtlastupdateddate"  dcn="bankaudit_lastupdateddate" runat="server" autocomplete="off" MaxLength="20" Format="DateTime" CssClass="textbox datetimepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Client Remarks</td>
											<td ti='14'><asp:TextBox TextMode="MultiLine" ID="txtclientremarks"  dcn="bankaudit_clientremarks"  runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Finrex Remarks</td>
											<td ti='15'><asp:TextBox TextMode="MultiLine" ID="txtfinrexremarks"  dcn="bankaudit_finrexremarks"  runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Code <span class="error">*</span></td>
											<td ti='16'><asp:TextBox ID="txtcode"  IsUnique="true"  search='true'  MaxLength="100" runat="server"  dcn="bankaudit_code" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv16" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Code" ValidationGroup="vg" ControlToValidate="txtcode"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Last Remarks</td>
											<td ti='17'><asp:TextBox ID="txtlastremarks"  MaxLength="100" runat="server"  dcn="bankaudit_lastremarks" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Bank Letter Sent Date</td>
											<td ti='18'><asp:TextBox ID="txtbanklettersentdate"  dcn="bankaudit_banklettersentdate" runat="server" autocomplete="off" MaxLength="20" Format="DateTime" CssClass="textbox datetimepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Value Of Shipment</td>
											<td ti='19'><asp:TextBox ID="txtvalueofshipment"  dcn="bankaudit_valueofshipment" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
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
