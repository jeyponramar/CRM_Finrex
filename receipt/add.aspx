 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Receipt_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%><%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        SetDetailPage('<%=Request.QueryString["id"]%>');
        PopulateDetail();
        var getTotAmt = 0;
        $(".amountchange").blur(function() {
            CalculateTotalAmount();
            getTotAmt = $(".totalamount").val();
        });
        $(".invno").blur(function() {
            PopulateDetail();
        });
        $(".pop").blur(function() {
            $(".totalamount").val(getTotAmt);
        });
        function CalculateTotalAmount() {
            var Discount = ConvertToDouble($(".Discount").val());
            var amountpaid = ConvertToDouble($(".amountpaid").val());
            var totalamount = (Discount) + (amountpaid);
            $(".totalamount").val(totalamount); //=totalamount;
        }
        function PopulateDetail() {
            var totalamount = 0;
            var balanceamount = 0;

            var id = $(".invoiceno").val();
            var data = RequestData("../getdata.ashx?m=invoiceamount&invid=" + id);
            if (data == "" || data == null) return;
            for (i = 0; i < data.length; i++) {
                totalamount = data[i].TotalAmount;
                balanceamount = data[i].BalanceAmount;
                if (balanceamount == "") {
                    balanceamount = totalamount;
                }
            }
            $(".inv-amt").text(totalamount);
            $(".inv-bal").text(balanceamount);
            $(".inv-amt").closest("tr").show();
        }
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
                 <asp:TextBox ID="h_balanceamount" runat="server" CssClass="hidden" Text="0"></asp:TextBox>                
                <asp:TextBox ID="h_prevamoutpaid" runat="server" CssClass="hidden" Text="0"></asp:TextBox>
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
                <tr>
                    <td>
						    <table width="100%">
						        <tr style="display:none">
						            <td></td><asp:Literal ID="ltbindbalanceamount" runat="server" Visible="false" ></asp:Literal>
						            <td>Balance Amount: <b style="font-size:small"><asp:Label CssClass="error inv-bal" ID="lblbalanceamount" runat="server" ></asp:Label></b>&nbsp;&nbsp;
						            Invoice Amount: <b style="font-size:small"><asp:Label ID="lblProformaInvoiceAmount" CssClass="error inv-amt" runat="server" ></asp:Label></b></td>
						        </tr>
						    </table>
						</td>
                </tr>
				<!--CONTROLS_START-->
					<table width='100%' cellpadding='0' cellspacing='0'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5'>
										<tr>
											<td class="label">Invoice No</td>
											<td><asp:TextBox ID="invoice"  Enabled="false"  dcn="invoice_invoiceno" MaxLength="100" runat="server" m="invoice" cn="invoiceno" CssClass="textbox ac txtac invno "></asp:TextBox>
											<asp:TextBox id="txtinvoiceid"  dcn="receipt_invoiceid"  Text="0" runat="server" class=" hdnac invoiceno"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Payment Date <span class="error">*</span></td>
											<td><asp:TextBox ID="txtpaymentdate"  dcn="receipt_paymentdate" runat="server" MaxLength="10" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Payment Date" ValidationGroup="vg" ControlToValidate="txtpaymentdate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Payment Mode <span class="error">*</span></td>
											<td><asp:DropDownList ID="ddlpaymentmodeid"  dcn="receipt_paymentmodeid" runat="server" m="paymentmode" cn="paymentmode" CssClass="ddl"></asp:DropDownList>
						<img src="../images/down-arr1.jpg" class="quick-menu epage"/>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Payment Mode" ValidationGroup="vg" ControlToValidate="ddlpaymentmodeid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Discount</td>
											<td><asp:TextBox ID="txtdiscount"  dcn="receipt_discount" runat="server" MaxLength="15" CssClass="textbox val-dbl amountchange Discount " Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Amount Paid</td>
											<td><asp:TextBox ID="txtamountpaid"  dcn="receipt_amountpaid" runat="server" MaxLength="15" CssClass="textbox val-dbl amountchange amountpaid " Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Total Amount <span class="error">*</span></td>
											<td><asp:TextBox ID="txttotalamount"  dcn="receipt_totalamount"  Enabled="false" runat="server" MaxLength="15" CssClass="textbox val-dbl totalamount " Text="0"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv5" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Total Amount" ValidationGroup="vg" ControlToValidate="txttotalamount"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Cheque No</td>
											<td><asp:TextBox ID="txtchequeno" MaxLength="100" runat="server"  dcn="receipt_chequeno" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Bank Name</td>
											<td><asp:TextBox ID="txtbankname" MaxLength="100" runat="server"  dcn="receipt_bankname" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Bank Branch</td>
											<td><asp:TextBox ID="txtbankbranch" MaxLength="100" runat="server"  dcn="receipt_bankbranch" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Comment</td>
											<td><asp:TextBox TextMode="MultiLine" ID="txtcomment"  dcn="receipt_comment" MaxLength="500" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5'>
										<tr>
											<td class="label">Customer Name</td>
											<td><asp:TextBox ID="client"  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac pop "></asp:TextBox><asp:TextBox id="txtclientid"  dcn="receipt_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Contact Person</td>
											<td><asp:TextBox ID="txtcontactperson" MaxLength="100" runat="server"  dcn="receipt_contactperson" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Mobile No</td>
											<td><asp:TextBox ID="txtmobileno"  dcn="receipt_mobileno" runat="server" MaxLength="10" CssClass="textbox val-i" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Email Id</td>
											<td><asp:TextBox ID="txtemailid"  dcn="receipt_emailid" MaxLength="100" runat="server" CssClass="textbox val-email"></asp:TextBox></td>
										</tr>
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
                <!--SAVEBUTTON_END-->
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
