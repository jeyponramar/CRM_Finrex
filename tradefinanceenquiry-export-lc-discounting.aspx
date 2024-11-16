<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="tradefinanceenquiry-export-lc-discounting.aspx.cs" Inherits="tradefinanceenquiry_export_lc_discounting" Title="Export Finance - Export LC Discounting" %>
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
        <table width='100%'><tr><td class='page-title2'>Export Finance - Export LC Discounting</td></tr>
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
											<td class="label">Exporter  Name</td>
											<td ti='0'><asp:TextBox ID="txtexportername"  MaxLength="100" runat="server"  dcn="exportlcdiscountingenquiry_exportername" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Contact Person <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtcontactperson"  search='true'  MaxLength="50" runat="server"  dcn="exportlcdiscountingenquiry_contactperson" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Contact Person" ValidationGroup="vg" ControlToValidate="txtcontactperson"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Email <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txtemail"  dcn="exportlcdiscountingenquiry_email" MaxLength="100" runat="server" CssClass="textbox val-email val-email "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Email" ValidationGroup="vg" ControlToValidate="txtemail"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Mobile Number <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="txtmobilenumber"  search='true'  MaxLength="100" runat="server"  dcn="exportlcdiscountingenquiry_mobilenumber" CssClass="textbox val-mobile "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv3" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Mobile Number" ValidationGroup="vg" ControlToValidate="txtmobilenumber"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">IEC Code <span class="error">*</span></td>
											<td ti='4'><asp:TextBox ID="txtieccode"  MaxLength="50" runat="server"  dcn="exportlcdiscountingenquiry_ieccode" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv4" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required IEC Code" ValidationGroup="vg" ControlToValidate="txtieccode"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Nature of Business</td>
											<td ti='5'><asp:TextBox ID="txtnatureofbusiness"  MaxLength="100" runat="server"  dcn="exportlcdiscountingenquiry_natureofbusiness" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Number of Years in Business</td>
											<td ti='6'><asp:TextBox ID="txtnumberofyearsinbusiness"  dcn="exportlcdiscountingenquiry_numberofyearsinbusiness" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Annual Turnover in Rs.</td>
											<td ti='7'><asp:TextBox ID="txtannualturnoverinrs"  dcn="exportlcdiscountingenquiry_annualturnoverinrs" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Importer Name (Supplier) <span class="error">*</span></td>
											<td ti='8'><asp:TextBox ID="txtimporternamesupplier"  MaxLength="100" runat="server"  dcn="exportlcdiscountingenquiry_importernamesupplier" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv8" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Importer Name (Supplier)" ValidationGroup="vg" ControlToValidate="txtimporternamesupplier"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Importer Country</td>
											<td ti='9'><asp:TextBox ID="txtimportercountry"  MaxLength="100" runat="server"  dcn="exportlcdiscountingenquiry_importercountry" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Turnover with Importer in USD</td>
											<td ti='10'><asp:TextBox ID="txtturnoverwithimporterinusd"  dcn="exportlcdiscountingenquiry_turnoverwithimporterinusd" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Avg Invoice Value in USD</td>
											<td ti='11'><asp:TextBox ID="txtavginvoicevalueinusd"  dcn="exportlcdiscountingenquiry_avginvoicevalueinusd" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Volume Per Month in USD</td>
											<td ti='12'><asp:TextBox ID="txtvolumepermonthinusd"  dcn="exportlcdiscountingenquiry_volumepermonthinusd" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">LC Issuing Bank</td>
											<td ti='0'><asp:TextBox ID="txtlcissuingbank"  MaxLength="100" runat="server"  dcn="exportlcdiscountingenquiry_lcissuingbank" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Port of Loading</td>
											<td ti='1'><asp:TextBox ID="txtportofloading"  MaxLength="100" runat="server"  dcn="exportlcdiscountingenquiry_portofloading" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Port of discharge</td>
											<td ti='2'><asp:TextBox ID="txtportofdischarge"  dcn="exportlcdiscountingenquiry_portofdischarge" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Transaction Amount with Currency <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="txttransactionamountwithcurrency"  dcn="exportlcdiscountingenquiry_transactionamountwithcurrency" runat="server" MaxLength="100" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv16" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Transaction Amount with Currency" ValidationGroup="vg" ControlToValidate="txttransactionamountwithcurrency"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Tenure of Finance/ Credit in days <span class="error">*</span></td>
											<td ti='4'><asp:TextBox ID="txttenureoffinancecreditindays"  dcn="exportlcdiscountingenquiry_tenureoffinancecreditindays" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv17" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Tenure of Finance/ Credit in days" ValidationGroup="vg" ControlToValidate="txttenureoffinancecreditindays"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Description of Underlying Goods <span class="error">*</span></td>
											<td ti='5'><asp:TextBox TextMode="MultiLine" ID="txtdescriptionofunderlyinggoods"  dcn="exportlcdiscountingenquiry_descriptionofunderlyinggoods" ml="800" runat="server" CssClass="textarea"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv18" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Description of Underlying Goods" ValidationGroup="vg" ControlToValidate="txtdescriptionofunderlyinggoods"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Mode of Payment <span class="error">*</span></td>
											<td ti='6'><asp:DropDownList ID="ddlenquirymodeofpaymentid"  dcn="exportlcdiscountingenquiry_enquirymodeofpaymentid" runat="server" m="enquirymodeofpayment" cn="modeofpayment" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv19" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Mode of Payment" ValidationGroup="vg" ControlToValidate="ddlenquirymodeofpaymentid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Remarks</td>
											<td ti='7'><asp:TextBox TextMode="MultiLine" ID="txtremarks"  dcn="exportlcdiscountingenquiry_remarks" ml="800" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<%--<tr>
											<td class="label">Upload Documents</td>
											<td ti='8'><uc:MultiFileUpload ID="mfuuploaddocuments"  IsMultiple="true" FileType="ANy" FolderPath="upload/exportlcdiscountingenquiry" runat="server" CssClass="textbox "></uc:MultiFileUpload></td>
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