 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Ledger_add" EnableEventValidation="false" ValidateRequest="false"%>
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
											<td class="label">Ledger Name <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtledgername"  IsUnique="true" MaxLength="100" runat="server"  dcn="ledger_ledgername" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Ledger Name" ValidationGroup="vg" ControlToValidate="txtledgername"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Billing Name <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtbillingname" MaxLength="100" runat="server"  dcn="ledger_billingname" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Billing Name" ValidationGroup="vg" ControlToValidate="txtbillingname"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Ledger Group <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="ledgergroup"  dcn="ledgergroup_ledgergroupname" MaxLength="100" runat="server" m="ledgergroup" cn="ledgergroupname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtledgergroupid"  dcn="ledger_ledgergroupid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Ledger Group" ValidationGroup="vg" ControlToValidate="ledgergroup"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Opening Balance Cr</td>
											<td ti='3'><asp:TextBox ID="txtopeningbalancecr"  dcn="ledger_openingbalancecr" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Opening Balance Dr</td>
											<td ti='4'><asp:TextBox ID="txtopeningbalancedr"  dcn="ledger_openingbalancedr" runat="server" MaxLength="15" CssClass="mbox val-dbl" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Email Id</td>
											<td ti='5'><asp:TextBox ID="txtemailid" MaxLength="100" runat="server"  dcn="ledger_emailid" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Mobile No</td>
											<td ti='6'><asp:TextBox ID="txtmobileno" MaxLength="100" runat="server"  dcn="ledger_mobileno" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Landline No</td>
											<td ti='7'><asp:TextBox ID="txtlandlineno" MaxLength="100" runat="server"  dcn="ledger_landlineno" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Address</td>
											<td ti='8'><asp:TextBox TextMode="MultiLine" ID="txtaddress"  dcn="ledger_address" ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
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
