 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="BulkEmail_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%>
<%@ Register Src="~/Usercontrols/MultiCheckbox.ascx" TagName="MultiCheckbox" TagPrefix="uc" %>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>
<%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        SetDetailPage('<%=Request.QueryString["id"]%>');
        $(".save").click(function(){
            return confirm("Are you sure you want to send bulk email?\nYou can not rollback your request so please check the message and confirm.");
        });
        $(".btnretry").click(function(){
            return confirm("Are you sure you want to send bulk email again?\nYou can not rollback your request so please check the message and confirm.");
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
											<td class="label">Date  <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtdate"  dcn="bulkemail_date" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker cdate "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Date " ValidationGroup="vg" ControlToValidate="txtdate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Template Name</td>
											<td ti='1'><asp:DropDownList ID="ddlbulkemailtemplateid"  dcn="bulkemail_bulkemailtemplateid"  Autopostback="true" OnSelectedIndexChanged="btnTemplate_Changed" runat="server" m="bulkemailtemplate" cn="templatename" CssClass="ddl"></asp:DropDownList></td>
										</tr>
										<tr>
											<td class="label">Client Groups <span class="error">*</span></td>
											<td ti='2'><uc:MultiCheckbox ID="mcclientgroups"  Module="clientgroup" Column="groupname"  TargetModule="clientgroups" runat="server" ></uc:MultiCheckbox></td>
										</tr>
										<tr>
											<td class="label">Subject <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="txtsubject"  MaxLength="100" runat="server"  dcn="bulkemail_subject" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv5" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Subject" ValidationGroup="vg" ControlToValidate="txtsubject"></asp:RequiredFieldValidator></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Attachment</td>
											<td ti='0'><uc:MultiFileUpload ID="mfuattachment"  IsMultiple="true" FileType="Any" FolderPath="upload/bulkemail" runat="server" CssClass="textbox "></uc:MultiFileUpload></td>
										</tr>
										<tr>
											<td class="label">Status</td>
											<td ti='1'><asp:TextBox ID="emailsmssentstatus"  Enabled="false"  search='true'  dcn="emailsmssentstatus_status" MaxLength="100" runat="server" m="emailsmssentstatus" cn="status" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemailsmssentstatusid"  dcn="bulkemail_emailsmssentstatusid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr><td class='label'>Message</td></tr>
					<tr>
						<td><asp:TextBox TextMode="MultiLine" ID="txtmessage"  width="800px" height="250px"  dcn="bulkemail_message" runat="server" CssClass="htmleditor"></asp:TextBox></td>
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
					<asp:Button ID="btnSubmit1" runat="server" OnClick="btnSubmit1_Click" Text="Send via new email" CssClass="save button" ValidationGroup="vg"/>
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
