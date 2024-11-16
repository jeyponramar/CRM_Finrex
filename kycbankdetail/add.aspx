 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="KYCBankDetail_add" EnableEventValidation="false" ValidateRequest="false"%>
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
											<td class="label">Customer <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="client"  search='true'  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtclientid"  dcn="kycbankdetail_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Customer" ValidationGroup="vg" ControlToValidate="client"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Name of the Bank <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="bankauditbank"  search='true'  dcn="bankauditbank_bankname" MaxLength="100" runat="server" m="bankauditbank" cn="bankname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtbankauditbankid"  dcn="kycbankdetail_bankauditbankid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Name of the Bank" ValidationGroup="vg" ControlToValidate="bankauditbank"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Account Number</td>
											<td ti='2'><asp:TextBox ID="txtaccountnumber"  search='true'  MaxLength="100" runat="server"  dcn="kycbankdetail_accountnumber" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Branch Name</td>
											<td ti='3'><asp:TextBox ID="txtbranchname"  search='true'  MaxLength="100" runat="server"  dcn="kycbankdetail_branchname" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Bank Margin in Paisa</td>
											<td ti='4'><asp:TextBox ID="txtbankmargininpaisa"  dcn="kycbankdetail_bankmargininpaisa" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Treasury Contact Number</td>
											<td ti='5'><asp:TextBox ID="txttreasurycontactnumber"  dcn="kycbankdetail_treasurycontactnumber"  search='true' MaxLength="100" runat="server" CssClass="textbox val-ph"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Branch RM Name</td>
											<td ti='6'><asp:TextBox ID="txtbranchrmcontactperson"  search='true'  MaxLength="100" runat="server"  dcn="kycbankdetail_branchrmcontactperson" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Branch RM Contact Number</td>
											<td ti='7'><asp:TextBox ID="txtbranchrmcontactnumber"  dcn="kycbankdetail_branchrmcontactnumber"  search='true' MaxLength="100" runat="server" CssClass="textbox val-ph"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Branch Head Name</td>
											<td ti='8'><asp:TextBox ID="txtbranchheadcontactperson"  search='true'  MaxLength="100" runat="server"  dcn="kycbankdetail_branchheadcontactperson" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Branch Head Contact Number</td>
											<td ti='9'><asp:TextBox ID="txtbranchheadcontactnumber"  dcn="kycbankdetail_branchheadcontactnumber"  search='true' MaxLength="100" runat="server" CssClass="textbox val-ph"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Sanctioned Letter Renewal date</td>
											<td ti='10'><asp:TextBox ID="txtsanctionedletterrenewaldate"  dcn="kycbankdetail_sanctionedletterrenewaldate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Is AUDIT Done</td>
											<td ti='11'><asp:CheckBox ID="chkisauditdone"  dcn="kycbankdetail_isauditdone" runat="server"></asp:CheckBox></td>
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
