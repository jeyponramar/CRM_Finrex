 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="BulkSMS_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%>
<%@ Register Src="~/Usercontrols/MultiCheckbox.ascx" TagName="MultiCheckbox" TagPrefix="uc" %>
<%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        SetDetailPage('<%=Request.QueryString["id"]%>');
        $(".btnretry").click(function() {
            return confirm("Failed sms will be tried again, are you sure you want to retry?");
        });
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
				<asp:Button ID="btnstart" Text="Start" OnClick="btnstart_Click"  Visible="false"  runat="server" CssClass="button btnaction btngreen btnstart "></asp:Button>
				<asp:Button ID="btnstop" Text="Stop" OnClick="btnstop_Click"  Visible="false"  runat="server" CssClass="button btnaction btnred "></asp:Button>
				<asp:Button ID="btnretry" Text="Retry" OnClick="btnretry_Click"  Visible="false"  runat="server" CssClass="button btnaction btnorange btnretry "></asp:Button>
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
										<tr>
											<td class="label">Date <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtdate"  dcn="bulksms_date" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker cdate "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Date" ValidationGroup="vg" ControlToValidate="txtdate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Template Name</td>
											<td ti='1'><asp:DropDownList ID="ddlbulksmstemplateid"  dcn="bulksms_bulksmstemplateid"  Autopostback="true" OnSelectedIndexChanged="ddlbulksmstemplate_changed"  search='true' runat="server" m="bulksmstemplate" cn="templatename" CssClass="ddl"></asp:DropDownList></td>
										</tr>
										<tr>
											<td class="label">Client Groups <span class="error">*</span></td>
											<td ti='2'><uc:MultiCheckbox ID="mcclientgroups"  Module="clientgroup" Column="groupname"  TargetModule="clientgroups" runat="server" ></uc:MultiCheckbox></td>
										</tr>
										<tr>
											<td class="label">Message <span class="error">*</span></td>
											<td ti='3'><asp:TextBox TextMode="MultiLine" ID="txtmessage"  dcn="bulksms_message"  search='true' ml="300" runat="server" CssClass="textarea"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv3" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Message" ValidationGroup="vg" ControlToValidate="txtmessage"></asp:RequiredFieldValidator></td>
										</tr>
										<tr ID="trbulksmsid" runat="server" style="display:none">
											<td class="label">Bulk SMS Id</td>
											<td ti='4'><asp:TextBox ID="txtbulksmsid"  dcn="bulksms_bulksmsid"  IsSave="false" runat="server" MaxLength="10" CssClass="mbox val-i bulksmsid " Text="0"></asp:TextBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Total SMS</td>
											<td ti='0'><asp:TextBox ID="txttotalsms"  dcn="bulksms_totalsms"  Enabled="false" runat="server" MaxLength="10" CssClass="mbox val-i totalsms " Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Total Sent</td>
											<td ti='1'><asp:TextBox ID="txttotalsent"  dcn="bulksms_totalsent"  Enabled="false" runat="server" MaxLength="10" CssClass="mbox val-i totalsmssent " Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Total Failed</td>
											<td ti='2'><asp:TextBox ID="txttotalfailed"  dcn="bulksms_totalfailed"  Enabled="false" runat="server" MaxLength="10" CssClass="mbox val-i totalsmsfailed " Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Balance</td>
											<td ti='3'><asp:TextBox ID="txtbalance"  dcn="bulksms_balance"  Enabled="false" IsSave="false" runat="server" MaxLength="10" CssClass="mbox val-i totalsmsbalance " Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Status</td>
											<td ti='4'><asp:TextBox ID="emailsmsstatus"  Enabled="false"  search='true'  dcn="emailsmsstatus_status" MaxLength="100" runat="server" m="emailsmsstatus" cn="status" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemailsmsstatusid"  dcn="bulksms_emailsmsstatusid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
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
