 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Email_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>
<%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        //SetDetailPage('<%=Request.QueryString["id"]%>');
        $(".edit").hide();
        $(".save").click(function() {
            if ($(".heditor").text() == "") {
                alert("Please enter message!");
                return false;
            }
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
										<tr ID="tremailtype" runat="server" Visible="false">
											<td class="label">Email Type</td>
											<td ti='0'><asp:TextBox ID="txtemailtype"  search='true'  MaxLength="100" runat="server"  dcn="email_emailtype" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">From Email Id</td>
											<td ti='1'><asp:TextBox ID="txtfromemailid"  Width="500"  dcn="email_fromemailid" MaxLength="100" runat="server" CssClass="textbox val-email val-email "></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">To Email Id</td>
											<td ti='2'><asp:TextBox ID="txttoemailid"  Width="500"  dcn="email_toemailid"  runat="server" CssClass="textbox val-email val-email "></asp:TextBox></td>
										</tr>
										<tr ID="trcc" runat="server" Visible="false">
											<td class="label">CC</td>
											<td ti='3'><asp:TextBox ID="txtcc"  Width="500"  dcn="email_cc" MaxLength="100" runat="server" CssClass="textbox val-email val-email "></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">BCC</td>
											<td ti='4'><asp:TextBox ID="txtbcc"  Width="500"  dcn="email_bcc" MaxLength="100" runat="server" CssClass="textbox val-email val-email "></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Subject <span class="error">*</span></td>
											<td ti='5'><asp:TextBox ID="txtsubject"  Width="500"  search='true'  MaxLength="1000" runat="server"  dcn="email_subject" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv7" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Subject" ValidationGroup="vg" ControlToValidate="txtsubject"></asp:RequiredFieldValidator></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Status</td>
											<td ti='0'><asp:TextBox ID="emailsmssentstatus"  Enabled="false"  search='true'  dcn="emailsmssentstatus_status" MaxLength="100" runat="server" m="emailsmssentstatus" cn="status" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemailsmssentstatusid"  dcn="email_emailsmssentstatusid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Attachment</td>
											<td ti='1'><uc:MultiFileUpload ID="mfuattachment"  IsMultiple="true" FileType="Any" FolderPath="upload/email" runat="server" CssClass="textbox "></uc:MultiFileUpload></td>
										</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr><td class='label'>Message</td></tr>
					<tr>
						<td><asp:TextBox TextMode="MultiLine" ID="txtmessage"  Width="700" Height="300"  dcn="email_message" runat="server" CssClass="htmleditor"></asp:TextBox></td>
					</tr>
					
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="2">
										<tr ID="trmodule" runat="server" Visible="false">
											<td class="label">Module</td>
											<td ti='0'><asp:TextBox ID="txtmodule"  MaxLength="100" runat="server"  dcn="email_module" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr ID="trmoduleid" runat="server" Visible="false">
											<td class="label">Module Id</td>
											<td ti='1'><asp:TextBox ID="txtmoduleid"  dcn="email_moduleid" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox></td>
										</tr>
										<tr ID="trisdraft" runat="server" Visible="false">
											<td class="label">Is Draft</td>
											<td ti='2'><asp:CheckBox ID="chkisdraft"  dcn="email_isdraft" runat="server"></asp:CheckBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="3">
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
