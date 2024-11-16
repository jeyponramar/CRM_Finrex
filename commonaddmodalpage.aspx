<%@ Page Language="C#" AutoEventWireup="true" CodeFile="commonaddmodalpage.aspx.cs" Inherits="commonaddmodalpage" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<body>
<form id="f" runat="server">
    <div id="jq-page-content">
        <asp:PlaceHolder id="plkycclientownercontact" runat="server" visible="false">
            <table width="100%" cellpadding="5">
                <tr>
                    <td class="label">Name <span class="error">*</span></td>
                    <td><asp:TextBox ID="txtkycclientownercontact_contactperson" runat="server" CssClass="textbox required"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="label">Contact Number <span class="error">*</span></td>
                    <td><asp:TextBox ID="txtkycclientownercontact_mobileno" runat="server" CssClass="textbox val-ph val-mobile required"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="label">Email Id</td>
                    <td><asp:TextBox ID="txtkycclientownercontact_emailid" runat="server" CssClass="textbox val-email"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td><input type="submit" class="button jq-common-btnaddmodal-save" value="Save" targetgrid="jq-kycownercontactgrid"/></td>
                </tr>
            </table>
        </asp:PlaceHolder>
        <asp:PlaceHolder id="plkycclientfinancecontact" runat="server" visible="false">
            <table width="100%" cellpadding="5">
                <tr>
                    <td class="label">Name <span class="error">*</span></td>
                    <td><asp:TextBox ID="txtkycclientfinancecontact_contactperson" runat="server" CssClass="textbox required"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="label">Contact Number <span class="error">*</span></td>
                    <td><asp:TextBox ID="txtkycclientfinancecontact_mobileno" runat="server" CssClass="textbox val-ph val-mobile required"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="label">Email Id</td>
                    <td><asp:TextBox ID="txtkycclientfinancecontact_emailid" runat="server" CssClass="textbox val-email"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td><input type="submit" class="button jq-common-btnaddmodal-save" value="Save" targetgrid="jq-kycfinancecontactgrid"/></td>
                </tr>
            </table>
        </asp:PlaceHolder>
        <asp:PlaceHolder id="plkycbankdetail" runat="server" visible="false">
            <table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
				<tr>
					<td class="label">Name of the Bank <span class="error">*</span></td>
					<td ti='1'><asp:TextBox ID="bankauditbank" MaxLength="100" runat="server" m="bankauditbank" cn="bankname" CssClass="textbox ac txtac required"></asp:TextBox>
					<asp:TextBox id="txtkycbankdetail_bankauditbankid"  dcn="clientkycbankdetail_bankauditbankid"  Text="0" runat="server" class=" hdnac"/><span class="ac-arrow"></span></td>
				
					<td class="label">Account Number</td>
					<td ti='2'><asp:TextBox ID="txtkycbankdetail_accountnumber" MaxLength="100" runat="server" CssClass="textbox"></asp:TextBox></td>
				</tr>
				<tr>
					<td class="label">Branch Name</td>
					<td ti='3'><asp:TextBox ID="txtkycbankdetail_branchname" MaxLength="100" runat="server" CssClass="textbox"></asp:TextBox></td>
				
					<td class="label">Bank Margin in Paisa</td>
					<td ti='4'><asp:TextBox ID="txtkycbankdetail_bankmargininpaisa" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
				</tr>
				<tr>
					<td class="label">Treasury Contact Number</td>
					<td ti='5'><asp:TextBox ID="txtkycbankdetail_treasurycontactnumber" MaxLength="100" runat="server" CssClass="textbox val-ph"></asp:TextBox></td>
				
					<td class="label">Branch RM Name</td>
					<td ti='6'><asp:TextBox ID="txtkycbankdetail_branchrmcontactperson" MaxLength="100" runat="server" CssClass="textbox"></asp:TextBox></td>
				</tr>
				<tr>
					<td class="label">Branch RM Contact Number</td>
					<td ti='7'><asp:TextBox ID="txtkycbankdetail_branchrmcontactnumber" MaxLength="100" runat="server" CssClass="textbox val-ph"></asp:TextBox></td>
				
					<td class="label">Branch Head Name</td>
					<td ti='8'><asp:TextBox ID="txtkycbankdetail_branchheadcontactperson" MaxLength="100" runat="server" CssClass="textbox"></asp:TextBox></td>
				</tr>
				<tr>
					<td class="label">Branch Head Contact Number</td>
					<td ti='9'><asp:TextBox ID="txtkycbankdetail_branchheadcontactnumber" MaxLength="100" runat="server" CssClass="textbox val-ph"></asp:TextBox></td>
				
					<td class="label">Sanctioned Letter Renewal date</td>
					<td ti='10'><asp:TextBox ID="txtkycbankdetail_sanctionedletterrenewaldate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
				</tr>
				<tr>
					<td class="label">Is AUDIT Done</td>
					<td ti='11'><asp:CheckBox ID="chkkycbankdetail_isauditdone" runat="server"></asp:CheckBox></td>
				</tr>
				<tr>
                    <td colspan="4" align="center"><input type="submit" class="button jq-common-btnaddmodal-save" value="Save" targetgrid="jq-kycbankdetailgrid"/></td>
                </tr>
				</table>
        </asp:PlaceHolder>
    </div>
    </form>
</body>
</html>
