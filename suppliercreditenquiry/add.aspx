 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="SupplierCreditEnquiry_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>
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
											<td class="label">Importer Firm Name <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtimporterfirmname"  search='true'  MaxLength="100" runat="server"  dcn="suppliercreditenquiry_importerfirmname" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Importer Firm Name" ValidationGroup="vg" ControlToValidate="txtimporterfirmname"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Contact Person <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtcontactperson"  search='true'  MaxLength="50" runat="server"  dcn="suppliercreditenquiry_contactperson" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Contact Person" ValidationGroup="vg" ControlToValidate="txtcontactperson"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Email <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txtemail"  dcn="suppliercreditenquiry_email" MaxLength="100" runat="server" CssClass="textbox val-email val-email "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Email" ValidationGroup="vg" ControlToValidate="txtemail"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Mobile Number <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="txtmobilenumber"  search='true'  MaxLength="100" runat="server"  dcn="suppliercreditenquiry_mobilenumber" CssClass="textbox val-mobile "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv3" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Mobile Number" ValidationGroup="vg" ControlToValidate="txtmobilenumber"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Beneficiary Name (Supplier) <span class="error">*</span></td>
											<td ti='4'><asp:TextBox ID="txtbeneficiarynamesupplier"  MaxLength="100" runat="server"  dcn="suppliercreditenquiry_beneficiarynamesupplier" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv4" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Beneficiary Name (Supplier)" ValidationGroup="vg" ControlToValidate="txtbeneficiarynamesupplier"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Importer Country</td>
											<td ti='5'><asp:TextBox ID="txtimportercountry"  MaxLength="100" runat="server"  dcn="suppliercreditenquiry_importercountry" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">LC Issuing Bank</td>
											<td ti='6'><asp:TextBox ID="txtlcissuingbank"  MaxLength="100" runat="server"  dcn="suppliercreditenquiry_lcissuingbank" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Goods Type <span class="error">*</span></td>
											<td ti='7'><asp:DropDownList ID="ddlgoodstypeid"  dcn="suppliercreditenquiry_goodstypeid"  search='true' runat="server" m="goodstype" cn="goodstype" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv7" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Goods Type" ValidationGroup="vg" ControlToValidate="ddlgoodstypeid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Description of Underlying Goods <span class="error">*</span></td>
											<td ti='8'><asp:TextBox TextMode="MultiLine" ID="txtdescriptionofunderlyinggoods"  dcn="suppliercreditenquiry_descriptionofunderlyinggoods" ml="800" runat="server" CssClass="textarea"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv8" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Description of Underlying Goods" ValidationGroup="vg" ControlToValidate="txtdescriptionofunderlyinggoods"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Transaction Amount with Currency <span class="error">*</span></td>
											<td ti='9'><asp:TextBox ID="txttransactionamountwithcurrency"  dcn="suppliercreditenquiry_transactionamountwithcurrency" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv12" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Transaction Amount with Currency" ValidationGroup="vg" ControlToValidate="txttransactionamountwithcurrency"></asp:RequiredFieldValidator></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">LC Status <span class="error">*</span></td>
											<td ti='0'><asp:DropDownList ID="ddllcstatusid"  dcn="suppliercreditenquiry_lcstatusid"  search='true' runat="server" m="lcstatus" cn="lcstatus" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv9" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required LC Status" ValidationGroup="vg" ControlToValidate="ddllcstatusid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Mode of Payment <span class="error">*</span></td>
											<td ti='1'><asp:DropDownList ID="ddlenquirymodeofpaymentid"  dcn="suppliercreditenquiry_enquirymodeofpaymentid"  search='true' runat="server" m="enquirymodeofpayment" cn="modeofpayment" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv10" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Mode of Payment" ValidationGroup="vg" ControlToValidate="ddlenquirymodeofpaymentid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Expected LC Opening Date <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txtexpectedlcopeningdate"  dcn="suppliercreditenquiry_expectedlcopeningdate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv11" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Expected LC Opening Date" ValidationGroup="vg" ControlToValidate="txtexpectedlcopeningdate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Tenure of Finance/ Credit in Days <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="txttenureoffinancecreditindays"  dcn="suppliercreditenquiry_tenureoffinancecreditindays" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv13" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Tenure of Finance/ Credit in Days" ValidationGroup="vg" ControlToValidate="txttenureoffinancecreditindays"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Country of Origin <span class="error">*</span></td>
											<td ti='4'><asp:TextBox ID="txtcountryoforigin"  MaxLength="100" runat="server"  dcn="suppliercreditenquiry_countryoforigin" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv14" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Country of Origin" ValidationGroup="vg" ControlToValidate="txtcountryoforigin"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Expected Shipping Date <span class="error">*</span></td>
											<td ti='5'><asp:TextBox ID="txtexpectedshippingdate"  dcn="suppliercreditenquiry_expectedshippingdate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv15" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Expected Shipping Date" ValidationGroup="vg" ControlToValidate="txtexpectedshippingdate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Port of Loading <span class="error">*</span></td>
											<td ti='6'><asp:TextBox ID="txtportofloading"  MaxLength="100" runat="server"  dcn="suppliercreditenquiry_portofloading" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv16" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Port of Loading" ValidationGroup="vg" ControlToValidate="txtportofloading"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">LC No (If applicable)</td>
											<td ti='7'><asp:TextBox ID="txtlcnoifapplicable"  MaxLength="100" runat="server"  dcn="suppliercreditenquiry_lcnoifapplicable" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Remarks</td>
											<td ti='8'><asp:TextBox TextMode="MultiLine" ID="txtremarks"  dcn="suppliercreditenquiry_remarks" ml="800" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Upload Documents</td>
											<td ti='9'><uc:MultiFileUpload ID="mfuuploaddocuments"  IsMultiple="true" FileType="ANy" FolderPath="upload/suppliercreditenquiry" runat="server" CssClass="textbox "></uc:MultiFileUpload></td>
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
