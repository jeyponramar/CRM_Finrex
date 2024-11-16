 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="LedgerVoucher_add" EnableEventValidation="false" ValidateRequest="false"%>
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
					<table width='100%' cellpadding='0' cellspacing='0'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
										<tr>
											<td class="label">Voucher No <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtvoucherno" MaxLength="50" runat="server"  dcn="ledgervoucher_voucherno" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Voucher No" ValidationGroup="vg" ControlToValidate="txtvoucherno"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Reference No</td>
											<td ti='1'><asp:TextBox ID="txtreferenceno" MaxLength="50" runat="server"  dcn="ledgervoucher_referenceno" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Voucher Date</td>
											<td ti='2'><asp:TextBox ID="txtvoucherdate"  dcn="ledgervoucher_voucherdate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Total Amount</td>
											<td ti='3'><asp:TextBox ID="txttotalamount"  dcn="ledgervoucher_totalamount" runat="server" MaxLength="15" CssClass="textbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Narration</td>
											<td ti='4'><asp:TextBox TextMode="MultiLine" ID="txtnarration"  dcn="ledgervoucher_narration" ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Ledger</td>
											<td ti='5'><asp:TextBox ID="ledger"  Enabled="false"  dcn="ledger_ledgername" MaxLength="100" runat="server" m="ledger" cn="ledgername" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtledgerid"  dcn="ledgervoucher_ledgerid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Voucher Type</td>
											<td ti='6'><asp:TextBox ID="accountvouchertype"  dcn="accountvouchertype_vouchertype" MaxLength="100" runat="server" m="accountvouchertype" cn="vouchertype" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtaccountvouchertypeid"  dcn="ledgervoucher_accountvouchertypeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Adjustment Method</td>
											<td ti='7'><asp:DropDownList ID="ddlaccountadjustmentmethodid"  dcn="ledgervoucher_accountadjustmentmethodid" runat="server" m="accountadjustmentmethod" cn="adjustmentmethod" CssClass="ddl"></asp:DropDownList></td>
										</tr>
										<tr>
											<td class="label">Payment Mode</td>
											<td ti='8'><asp:DropDownList ID="ddlpaymentmodeid"  dcn="ledgervoucher_paymentmodeid" runat="server" m="paymentmode" cn="paymentmode" CssClass="ddl"></asp:DropDownList></td>
										</tr>
										<tr>
											<td class="label">Cheque No</td>
											<td ti='9'><asp:TextBox ID="txtchequeno" MaxLength="20" runat="server"  dcn="ledgervoucher_chequeno" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Amount Paid</td>
											<td ti='10'><asp:TextBox ID="txtamountpaid"  dcn="ledgervoucher_amountpaid" runat="server" MaxLength="15" CssClass="textbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Balance Amount</td>
											<td ti='11'><asp:TextBox ID="txtbalanceamount"  dcn="ledgervoucher_balanceamount" runat="server" MaxLength="15" CssClass="textbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Dr Amount</td>
											<td ti='12'><asp:TextBox ID="txtdramount"  dcn="ledgervoucher_dramount" runat="server" MaxLength="15" CssClass="textbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Cr Amount</td>
											<td ti='13'><asp:TextBox ID="txtcramount"  dcn="ledgervoucher_cramount" runat="server" MaxLength="15" CssClass="textbox val-dbl" Text="0"></asp:TextBox></td>
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
                <asp:Button ID="btnSubmitAndView" runat="server" OnClick="btnSaveAndView_Click" Text="Save & View" CssClass="save button" ValidationGroup="vg"/>
		  <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" CssClass="delete button" ValidationGroup="vg" Visible="false"/>
                <!--SAVEBUTTON_END-->
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
