 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Followups_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%><%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script>
    $(document).ready(function() {
        $(".followupaction").change(function() {
            if ($(this).val() == "2") {
                $(".remindme").find("input").attr("checked", true);
            }
        });
        $(".save").click(function() {
            if ($(".remindme").find("input").is(":checked")) {
                if ($(".reminderdate").val() == "") {
                    alert("Please enter reminder date!");
                    $(".reminderdate").focus();
                    return false;
                }
            }
//            if ($(".followupaction").val() == "6") {//meeting
//                if ($(".message").val() == "") {
//                    alert("Please enter your meeting agenda in message box");
//                    $(".message").focus();
//                    return false;
//                }
//            }
        });
        $(".reminderdate").blur(function() {
            if ($(this).val() != "") {
                $(".remindme").find("input").attr("checked", "checked");
            }
        });
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
				<asp:Button ID="btnsendmeetingrequest" Text="Send Meeting Request" OnClick="btnsendmeetingrequest_Click"  Visible="false" runat="server" CssClass="button btnaction "></asp:Button>
				<asp:Button ID="btnsendmom" Text="Send MOM" OnClick="btnsendmom_Click"  Visible="false" runat="server" CssClass="button btnaction btnorange "></asp:Button>
				<asp:Button ID="btnthanksmail" Text="Thanks Mail" OnClick="btnthanksmail_Click"  Visible="false" runat="server" CssClass="button btnaction "></asp:Button>
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
					<table width='100%' cellpadding='0' cellspacing='0'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
										<tr>
											<td class="label">Client</td>
											<td ti='0'><asp:TextBox ID="client"  search='true'  dcn="client_customername" MaxLength="100" runat="server" m="client" cn="customername" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtclientid"  dcn="followups_clientid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr ID="trsubject" runat="server" Visible="false">
											<td class="label">Subject</td>
											<td ti='1'><asp:TextBox ID="txtsubject"  MaxLength="100" runat="server"  dcn="followups_subject" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Date <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txtdate"  dcn="followups_date" runat="server" autocomplete="off" MaxLength="20" Format="DateTime" CssClass="textbox datetimepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Date" ValidationGroup="vg" ControlToValidate="txtdate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Assigned To</td>
											<td ti='3'><asp:TextBox ID="employee"  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemployeeid"  dcn="followups_employeeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Followup Action <span class="error">*</span></td>
											<td ti='4'><asp:DropDownList ID="ddlfollowupactionid"  dcn="followups_followupactionid"  OnSelectedIndexChanged="ddlfollowupactionid_Changed" AutoPostback="true" runat="server" m="followupaction" cn="action" CssClass="ddl followupaction "></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv4" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Followup Action" ValidationGroup="vg" ControlToValidate="ddlfollowupactionid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Reminder Date</td>
											<td ti='5'><asp:TextBox ID="txtreminderdate"  dcn="followups_reminderdate" runat="server" autocomplete="off" MaxLength="20" Format="DateTime" CssClass="textbox datetimepicker reminderdate "></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Meeting Done Date</td>
											<td ti='6'><asp:TextBox ID="txtmeetingdonedate"  dcn="followups_meetingdonedate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox></td>
										</tr>
										<tr ID="trmodule" runat="server" Visible="false">
											<td class="label">Module</td>
											<td ti='7'><asp:TextBox ID="txtmodule"  MaxLength="30" runat="server"  dcn="followups_module" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr ID="trmid" runat="server" Visible="false">
											<td class="label">mid</td>
											<td ti='8'><asp:TextBox ID="txtmid"  dcn="followups_mid" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Followup Status <span class="error">*</span></td>
											<td ti='0'><asp:DropDownList ID="ddlfollowupstatusid"  dcn="followups_followupstatusid" runat="server" m="followupstatus" cn="status" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv7" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Followup Status" ValidationGroup="vg" ControlToValidate="ddlfollowupstatusid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Remarks</td>
											<td ti='1'><asp:TextBox TextMode="MultiLine" ID="txtremarks"  dcn="followups_remarks" ml="300" runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Close All Previous Tasks</td>
											<td ti='2'><asp:CheckBox ID="chkcloseallprevioustasks"  Checked="true"  dcn="followups_closeallprevioustasks" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">MOM Sent</td>
											<td ti='3'><asp:CheckBox ID="chkmomsent"  Enabled="false"  dcn="followups_momsent" runat="server"></asp:CheckBox></td>
										</tr>
										<tr>
											<td class="label">Meeting Request Sent</td>
											<td ti='4'><asp:CheckBox ID="chkmeetingrequestsent"  Enabled="false"  dcn="followups_meetingrequestsent" runat="server"></asp:CheckBox></td>
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
