<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="tradefinanceenquiry-buyer-credit.aspx.cs" Inherits="tradefinanceenquiry_buyer_credit" Title="Import Finance - Buyer Credit" %>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link href="js/upload/jquery.fileupload-ui.css" rel="stylesheet" type="text/css" />    
<script src="js/upload/jquery.fileupload.js?v=<%=VersionNo %>"></script>
<script src="js/upload/jquery.fileupload-ui.js?v=<%=VersionNo %>"></script>
<script src="js/upload/multifileupload.js?v=<%=VersionNo %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr><td class='page-inner2'>
        <table width='100%'><tr><td class='page-title2'>Import Finance - Buyer Credit</td></tr>
           <tr>
            <td class="form" colspan="2">
                <table width="90%" cellpadding="0">
                <tr>
                    <td align="center"><asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                </tr>
                <tr>
                <td>
                <asp:PlaceHolder ID="form" runat="server">
					<table width='100%' cellpadding='0' cellspacing='0' id='tblcontrols1' runat='server'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
										<tr>
											<td class="label">Importer Firm Name <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtimporterfirmname"  search='true'  MaxLength="50" runat="server"  dcn="buyercreditenquiry_importerfirmname" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Importer Firm Name" ValidationGroup="vg" ControlToValidate="txtimporterfirmname"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Contact Person <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtcontactperson"  search='true'  MaxLength="50" runat="server"  dcn="buyercreditenquiry_contactperson" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Contact Person" ValidationGroup="vg" ControlToValidate="txtcontactperson"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Email <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txtemail"  dcn="buyercreditenquiry_email" MaxLength="100" runat="server" CssClass="textbox val-email val-email "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Email" ValidationGroup="vg" ControlToValidate="txtemail"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Mobile Number <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="txtmobilenumber"  search='true'  MaxLength="100" runat="server"  dcn="buyercreditenquiry_mobilenumber" CssClass="textbox val-mobile "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv3" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Mobile Number" ValidationGroup="vg" ControlToValidate="txtmobilenumber"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Beneficiary Name (Supplier) <span class="error">*</span></td>
											<td ti='4'><asp:TextBox ID="txtbeneficiarynamesupplier"  MaxLength="100" runat="server"  dcn="buyercreditenquiry_beneficiarynamesupplier" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv4" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Beneficiary Name (Supplier)" ValidationGroup="vg" ControlToValidate="txtbeneficiarynamesupplier"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Importer Country</td>
											<td ti='5'><asp:TextBox ID="txtimportercountry"  MaxLength="100" runat="server"  dcn="buyercreditenquiry_importercountry" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">SBLC Bank <span class="error">*</span></td>
											<td ti='6'><asp:TextBox ID="txtsblcbank"  MaxLength="100" runat="server"  dcn="buyercreditenquiry_sblcbank" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv6" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required SBLC Bank" ValidationGroup="vg" ControlToValidate="txtsblcbank"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Goods Type <span class="error">*</span></td>
											<td ti='7'><asp:DropDownList ID="ddlgoodstypeid"  dcn="buyercreditenquiry_goodstypeid" runat="server" m="goodstype" cn="goodstype" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv7" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Goods Type" ValidationGroup="vg" ControlToValidate="ddlgoodstypeid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Description of Underlying Goods <span class="error">*</span></td>
											<td ti='8'><asp:TextBox TextMode="MultiLine" ID="txtdescriptionofunderlyinggoods"  dcn="buyercreditenquiry_descriptionofunderlyinggoods" ml="800" runat="server" CssClass="textarea"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv8" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Description of Underlying Goods" ValidationGroup="vg" ControlToValidate="txtdescriptionofunderlyinggoods"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Transaction Amount with Currency <span class="error">*</span></td>
											<td ti='9'><asp:TextBox ID="txttransactionamountwithcurrency"  dcn="buyercreditenquiry_transactionamountwithcurrency" runat="server" MaxLength="100" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv12" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Transaction Amount with Currency" ValidationGroup="vg" ControlToValidate="txttransactionamountwithcurrency"></asp:RequiredFieldValidator></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Type of Buyers Credit <span class="error">*</span></td>
											<td ti='0'><asp:DropDownList ID="ddltypeofbuyerscreditid"  dcn="buyercreditenquiry_typeofbuyerscreditid" runat="server" m="typeofbuyerscredit" cn="typeofbuyerscredit" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv9" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Type of Buyers Credit" ValidationGroup="vg" ControlToValidate="ddltypeofbuyerscreditid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Mode of Payment <span class="error">*</span></td>
											<td ti='1'><asp:DropDownList ID="ddlenquirymodeofpaymentid"  dcn="buyercreditenquiry_enquirymodeofpaymentid" runat="server" m="enquirymodeofpayment" cn="modeofpayment" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv10" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Mode of Payment" ValidationGroup="vg" ControlToValidate="ddlenquirymodeofpaymentid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Due date of Transaction <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txtduedateoftransaction"  dcn="buyercreditenquiry_duedateoftransaction" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv11" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Due date of Transaction" ValidationGroup="vg" ControlToValidate="txtduedateoftransaction"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Tenure of Finance/ Credit in Days <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="txttenureoffinancecreditindays"  dcn="buyercreditenquiry_tenureoffinancecreditindays" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv13" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Tenure of Finance/ Credit in Days" ValidationGroup="vg" ControlToValidate="txttenureoffinancecreditindays"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Country of Origin <span class="error">*</span></td>
											<td ti='4'><asp:TextBox ID="txtcountryoforigin"  MaxLength="50" runat="server"  dcn="buyercreditenquiry_countryoforigin" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv14" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Country of Origin" ValidationGroup="vg" ControlToValidate="txtcountryoforigin"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Bill of Lading Date <span class="error">*</span></td>
											<td ti='5'><asp:TextBox ID="txtbillofladingdate"  dcn="buyercreditenquiry_billofladingdate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv15" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Bill of Lading Date" ValidationGroup="vg" ControlToValidate="txtbillofladingdate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Port of Loading <span class="error">*</span></td>
											<td ti='6'><asp:TextBox ID="txtportofloading"  MaxLength="100" runat="server"  dcn="buyercreditenquiry_portofloading" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv16" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Port of Loading" ValidationGroup="vg" ControlToValidate="txtportofloading"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">LC No (If applicable)</td>
											<td ti='7'><asp:TextBox ID="txtlcnoifapplicable"  MaxLength="100" runat="server"  dcn="buyercreditenquiry_lcnoifapplicable" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Remarks</td>
											<td ti='8'><asp:TextBox TextMode="MultiLine" ID="txtremarks"  dcn="buyercreditenquiry_remarks" ml="800" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<%--<tr>
											<td class="label">Upload Documents</td>
											<td ti='9'><uc:MultiFileUpload ID="mfuuploaddocuments"  IsMultiple="true" FileType="ANy" FolderPath="upload/buyercreditenquiry" runat="server" CssClass="textbox "></uc:MultiFileUpload></td>
										</tr>--%>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
                        <td align="center" style="padding-bottom:50px;">
			                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="save button" ValidationGroup="vg"/>
                        </td>
                    </tr>
					</table>
				</asp:PlaceHolder>
				</td></tr>
                </table>
            </td>
         </tr>
		 
     </table>
    </td></tr>
</table>
</asp:Content>

