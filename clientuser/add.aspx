 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="ClientUser_add" EnableEventValidation="false" ValidateRequest="false"%>
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
                <!--ACTION_START-->
				<asp:Button ID="btnsendpassword" Text="Send Password" OnClick="btnsendpassword_Click"  Visible="false"  runat="server" CssClass="button btnaction "></asp:Button>
				<!--ACTION_END-->
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
										<tr ID="trsubscriptionid" runat="server" Visible="false">
											<td class="label">Subscription Code</td>
											<td ti='0'><asp:TextBox ID="subscription"  search='true'  dcn="subscription_subscriptioncode" MaxLength="100" runat="server" m="subscription" cn="subscriptioncode" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtsubscriptionid"  dcn="clientuser_subscriptionid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr ID="trtrialid" runat="server" Visible="false">
											<td class="label">Trial Code</td>
											<td ti='1'><asp:TextBox ID="trial"  search='true'  dcn="trial_trialcode" MaxLength="100" runat="server" m="trial" cn="trialcode" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txttrialid"  dcn="clientuser_trialid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Customer Name <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="client"  search='true'  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtclientid"  dcn="clientuser_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Customer Name" ValidationGroup="vg" ControlToValidate="client"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Name <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="txtname"  search='true'  MaxLength="100" runat="server"  dcn="clientuser_name" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv3" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Name" ValidationGroup="vg" ControlToValidate="txtname"></asp:RequiredFieldValidator></td>
										</tr>
										<tr ID="trcontactsid" runat="server" Visible="false">
											<td class="label">Contact Person</td>
											<td ti='4'><asp:TextBox ID="contacts"  acparent="clientid"  search='true'  dcn="contacts_contactperson" MaxLength="100" runat="server" m="contacts" cn="contactperson" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtcontactsid"  dcn="clientuser_contactsid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">User Name <span class="error">*</span></td>
											<td ti='5'><asp:TextBox ID="txtusername"  dcn="clientuser_username"  search='true' MaxLength="100" runat="server" CssClass="textbox val-email val-email "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv5" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required User Name" ValidationGroup="vg" ControlToValidate="txtusername"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Finstation</td>
											<td ti='6'><asp:CheckBox ID="chkiswebuser"  dcn="clientuser_iswebuser" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Finwatch</td>
											<td ti='7'><asp:CheckBox ID="chkisexeuser"  dcn="clientuser_isexeuser" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">FinIcon</td>
											<td ti='8'><asp:CheckBox ID="chkismobileuser"  dcn="clientuser_ismobileuser" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">FinPulse</td>
											<td ti='9'><asp:CheckBox ID="chkisfinmessenger"  dcn="clientuser_isfinmessenger" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Is Admin</td>
											<td ti='10'><asp:CheckBox ID="chkisadmin"  dcn="clientuser_isadmin" runat="server"></asp:CheckBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Is Active</td>
											<td ti='0'><asp:CheckBox ID="chkisactive"  dcn="clientuser_isactive" runat="server"></asp:CheckBox></td>
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
				
				<asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" CssClass="delete button" Visible="false"/>
				<input type="button" class="close-page cancel" value="Cancel"/>
				
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
