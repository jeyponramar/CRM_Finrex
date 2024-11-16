 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Trial_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>
<%@ Register Src="~/Usercontrols/MultiCheckbox.ascx" TagName="MultiCheckbox" TagPrefix="uc" %>
<%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        SetDetailPage('<%=Request.QueryString["id"]%>');
        $(".btnfemportal").click(function() {
            window.open('../exportportal.aspx?ia=true&tid=<%=Request.QueryString["id"]%>');
            return false;
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
				<asp:Button ID="btntrialfeedback" Text="Trial Feedback" OnClick="btntrialfeedback_Click"  Visible="false"  runat="server" CssClass="button btnaction "></asp:Button>
				<asp:Button ID="btntrialexpiredmail" Text="Trial Expired Mail" OnClick="btntrialexpiredmail_Click"  Visible="false"  runat="server" CssClass="button btnaction btnred "></asp:Button>
				<asp:Button ID="btnconverttosubscription" Text="Convert to Subscription" OnClick="btnconverttosubscription_Click"  Visible="false"  runat="server" CssClass="button btnaction btngreen "></asp:Button>
				<asp:Button ID="btnlogemail" Text="Log Email" OnClick="btnlogemail_Click"  Visible="false"  runat="server" CssClass="button btnaction "></asp:Button>
				<asp:Button ID="btnlogsms" Text="Log SMS" OnClick="btnlogsms_Click"  Visible="false"  runat="server" CssClass="button btnaction "></asp:Button>
				<asp:Button ID="btnlogwhatsapp" Text="Log WhatsApp" OnClick="btnlogwhatsapp_Click"  Visible="false"  runat="server" CssClass="button btnaction "></asp:Button>
				<asp:Button ID="btncallloghistory" Text="Call Log History" OnClick="btncallloghistory_Click"  Visible="false"  runat="server" CssClass="button btnaction "></asp:Button>
				<asp:Button ID="btnfemportal" Text="FEM Portal" OnClick="btnfemportal_Click"  Visible="false"  runat="server" CssClass="button btnaction btnfemportal "></asp:Button>
				<asp:Button ID="btncompanyprofile" Text="Company Profile" OnClick="btncompanyprofile_Click"  Visible="false"  runat="server" CssClass="button btnaction btnemail "></asp:Button>
				<asp:Button ID="btnaddcontact" Text="Add Contact" OnClick="btnaddcontact_Click"  Visible="false"  runat="server" CssClass="button btnaction "></asp:Button>
				<asp:Button ID="btnviewcontacts" Text="View Contacts" OnClick="btnviewcontacts_Click"  Visible="false"  runat="server" CssClass="button btnaction "></asp:Button>
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
											<td class="label">Trial Code</td>
											<td ti='0'><asp:TextBox ID="txttrialcode"  Enabled="false"  IsUnique="true"  MaxLength="100" runat="server"  dcn="trial_trialcode" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Customer Name <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="client"  search='true'  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtclientid"  dcn="trial_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Customer Name" ValidationGroup="vg" ControlToValidate="client"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Start Date <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txtstartdate"  dcn="trial_startdate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker cdate "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Start Date" ValidationGroup="vg" ControlToValidate="txtstartdate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Software End Date <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="txtenddate"  dcn="trial_enddate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv3" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Software End Date" ValidationGroup="vg" ControlToValidate="txtenddate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Whatsapp End Date <span class="error">*</span></td>
											<td ti='4'><asp:TextBox ID="txtwhatsappenddate"  dcn="trial_whatsappenddate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv4" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Whatsapp End Date" ValidationGroup="vg" ControlToValidate="txtwhatsappenddate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Attachment</td>
											<td ti='5'><uc:MultiFileUpload ID="mfuattachment"  IsMultiple="true" FileType="ANy" FolderPath="upload/trial" runat="server" CssClass="textbox "></uc:MultiFileUpload></td>
										</tr>
										<tr>
											<td class="label">Status</td>
											<td ti='6'><asp:TextBox ID="subscriptionstatus"  Enabled="false"  search='true'  dcn="subscriptionstatus_status" MaxLength="100" runat="server" m="subscriptionstatus" cn="status" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtsubscriptionstatusid"  dcn="trial_subscriptionstatusid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr ID="trcompanyprofilesentstatusid" runat="server" class="hdn">
											<td ti='7'><asp:TextBox ID="companyprofilesentstatus"  dcn="emailsmssentstatus_status" MaxLength="100" runat="server" m="emailsmssentstatus" cn="status" CssClass="textbox ac txtac hdn "></asp:TextBox><asp:TextBox id="txtcompanyprofilesentstatusid"  dcn="trial_companyprofilesentstatusid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Software</td>
											<td ti='0'><uc:MultiCheckbox ID="mctrialtrialprospect"  Module="prospect" Column="prospect_prospect" TargetModule="trialprospect"  runat="server" ></uc:MultiCheckbox></td>
										</tr>
										<tr ID="trusername" runat="server" Visible="false">
											<td class="label">User Name</td>
											<td ti='1'><asp:TextBox ID="txtusername"  MaxLength="100" runat="server"  dcn="trial_username" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Remarks</td>
											<td ti='2'><asp:TextBox TextMode="MultiLine" ID="txtremarks"  dcn="trial_remarks" ml="1000" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Enquiry No</td>
											<td ti='3'><asp:TextBox ID="enquiry"  Enabled="false"  dcn="enquiry_enquiryno" MaxLength="100" runat="server" m="enquiry" cn="enquiryno" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtenquiryid"  dcn="trial_enquiryid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">Assign To <span class="error">*</span></td>
											<td ti='4'><asp:TextBox ID="employee"  search='true'  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemployeeid"  dcn="trial_employeeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv11" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Assign To" ValidationGroup="vg" ControlToValidate="employee"></asp:RequiredFieldValidator></td>
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
