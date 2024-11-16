 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="ReceiptVoucher_add" EnableEventValidation="false" ValidateRequest="false"%>
<%--CONTROLREGISTER_START--%><%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        SetDetailPage('<%=Request.QueryString["id"]%>');
        $(".ledger").blur(function() {
            var lid = ConvertToInt($(".ledgerid").val());
            if (lid == 0) {
            }
            else {
                var url = "../utilities.ashx?m=outstanding-bills&lid=" + lid;
                var html = RequestData(url);
                $(".bills").html(html);
            }
        });
        $(".jq-calamountpaid").blur(function() {
            var amountReceived = ConvertToDouble($(".txtamountreceived").val());
            var tds = ConvertToDouble($(".txttdsamount").val());
            $(".txtamountpaid").val(amountReceived + tds);
        });
        $("form").submit(function() {
            var adjustmethod = ConvertToInt($(".adjustmentmethod").val());
            var refno = $(".refno").val();
            if (adjustmethod == 1)//advance
            {

            }
            else if (adjustmethod == 2)//Against Reference
            {

            }
            else if (adjustmethod == 3)//New Reference
            {

            }
            else if (adjustmethod == 4)//On Account
            {
                if (refno != "") {
                    alert("You can not enter Reference No for On Account Payment");
                    return false;
                }
            }
        });
    });
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--DESIGN_START-->
    <asp:PlaceHolder ID="form" runat="server">
    <table width="100%">
         <tr>
            <td class="title" colspan="2">
                <asp:Label ID="lblPageTitle" runat="server"/>
            </td>
         </tr>
         <tr>
            <td>
                <input type="button" value="Edit" class="edit button dpage"/>
                <input type="button" value="Copy" class="copy button dpage"/>
                <asp:TextBox ID="h_IsCopy" runat="server" CssClass="iscopy hidden" Text="0"></asp:TextBox>
                <!--ACTION_START--><!--ACTION_END-->
                <asp:Button ID="btnthanksmail" Text="Thanks Message" OnClick="btnthanksmail_Click"  Visible="false" runat="server" CssClass="button btnaction btngreen "></asp:Button>
            </td>
         </tr>
         <tr>
            <td class="form" colspan="2">
                <table width="90%" cellpadding="0">
                <tr>
                    <td align="center"><asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                    <asp:TextBox ID="txtmodule" runat="server" CssClass="hidden"></asp:TextBox>
                    <asp:TextBox ID="txtmoduleid" runat="server" CssClass="hidden"></asp:TextBox>
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
											<td class="label">Voucher No</td>
											<td ti='0'><asp:TextBox ID="txtvoucherno"  Enabled="false" MaxLength="100" runat="server"  dcn="ledgervoucher_voucherno" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Ledger <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="ledger"  dcn="ledger_ledgername" MaxLength="100" runat="server" m="ledger" cm="sundrycreditor" cn="ledgername" CssClass="textbox ac txtac ledger"></asp:TextBox>
											<asp:TextBox id="txtledgerid"  dcn="ledgervoucher_ledgerid"  Text="0" runat="server" class=" hdnac ledgerid"/><img src="../images/down-arrow.png" class="epage"/>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Ledger" ValidationGroup="vg" ControlToValidate="ledger"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Adjustment Method <span class="error">*</span></td>
											<td ti='2'><asp:DropDownList ID="ddlaccountadjustmentmethodid"  dcn="ledgervoucher_accountadjustmentmethodid" runat="server" m="accountadjustmentmethod" cn="adjustmentmethod" CssClass="ddl adjustmentmethod"></asp:DropDownList>
											<asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Adjustment Method" ValidationGroup="vg" ControlToValidate="ddlaccountadjustmentmethodid" InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Reference No</td>
											<td ti='3'><asp:TextBox ID="txtreferenceno" MaxLength="100" runat="server"  dcn="ledgervoucher_referenceno" CssClass="textbox refno"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Voucher Date</td>
											<td ti='4'><asp:TextBox ID="txtvoucherdate"  dcn="ledgervoucher_voucherdate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker cdate "></asp:TextBox></td>
										</tr>
										<tr>
										</tr>
										    <td class="label">Invoice Amount</td>
										    <td><asp:Label ID="lblInvoiceAmount" runat="server" CssClass="bold"></asp:Label>
										    <asp:TextBox ID="t_balanceamount" runat="server" Visible="false"></asp:TextBox></td>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Received Amount<span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtreceivedamount"  dcn="ledgervoucher_receivedamount" runat="server" MaxLength="15" CssClass="mbox val-dbl txtamountreceived jq-calamountpaid" Text="0"></asp:TextBox>
											<asp:RequiredFieldValidator ID="rfv5" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Amount Paid" ValidationGroup="vg" ControlToValidate="txtreceivedamount"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">TDS Amount</td>
											<td ti='1'><asp:TextBox ID="txttdsamount"  dcn="ledgervoucher_tdsamount" runat="server" MaxLength="15" CssClass="mbox val-dbl txttdsamount jq-calamountpaid" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Amount Paid </td>
											<td ti='0'><asp:TextBox ID="txtcramount"  dcn="ledgervoucher_cramount" Enabled="false" runat="server" MaxLength="15" CssClass="textbox val-dbl txtamountpaid" Text="0"></asp:TextBox>
						</td>
										</tr>
										<tr>
											<td class="label">Payment Mode</td>
											<td ti='1'><asp:DropDownList ID="ddlpaymentmodeid"  dcn="ledgervoucher_paymentmodeid" runat="server" m="paymentmode" cn="paymentmode" CssClass="ddl paymentmode"></asp:DropDownList></td>
										</tr>
										<tr>
											<td class="label">Cheque No</td>
											<td ti='2'><asp:TextBox ID="txtchequeno" MaxLength="100" runat="server"  dcn="ledgervoucher_chequeno" CssClass="textbox chequeno"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Narration</td>
											<td ti='3'><asp:TextBox TextMode="MultiLine" ID="txtnarration"  dcn="ledgervoucher_narration" ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
					    <td>
					        <table style='width:700px'>
					            <tr><td class="bills"></td></tr>
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
                <!--SAVEBUTTON_END-->
                <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" Visible="false" CssClass="delete button"/>
                <input type="button" class="close-page cancel" value="Cancel"/>
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->

</asp:Content>
