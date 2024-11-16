 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="EmailTemplate_add" EnableEventValidation="false" ValidateRequest="false" %>
<%--CONTROLREGISTER_START--%>
<%--CONTROLREGISTER_END--%>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<%-- <!-- Load TinyMCE -->
<script type="text/javascript" src="../js/editor/jscripts/tiny_mce/jquery.tinymce.js"></script>
<script src="../js/editor/tinymce.js" type="text/javascript"></script>
<!-- /TinyMCE -->--%>

<script>
    $(document).ready(function() {
        //SetDetailPage('<%=Request.QueryString["id"]%>');
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
            <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
         </tr>
         <tr>
            <td>
               <%-- <input type="button" value="Edit" class="edit button dpage"/>--%>
                <input type="button" value="Copy" class="copy button dpage"/>
                <asp:TextBox ID="h_IsCopy" runat="server" CssClass="iscopy hidden" Text="0"></asp:TextBox>
            </td>
         </tr>
         <tr>
            <td class="form" colspan="2">
                <table width="90%" cellpadding="0">
                <tr>
                    <td align="center" colspan="4"> <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                </tr>
                <!--CONTROLS_START-->
					
					<tr>
						<td class="label">Template Name <span class="error">*</span></td>
						<td><asp:TextBox ID="txttemplatename" runat="server"  dcn="emailtemplate_templatename" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" Display="Dynamic" runat="server" ErrorMessage="Required Template Name" ValidationGroup="vg" ControlToValidate="txttemplatename"></asp:RequiredFieldValidator></td>
					</tr>
					<tr>
						<td class="label">Subject</td>
						<td><asp:TextBox ID="txtsubject" runat="server"  dcn="emailtemplate_subject" CssClass="textbox"></asp:TextBox></td>
					</tr>
					<tr>
						<td class="label">Mail Format</td>
						<td><asp:DropDownList ID="ddlmailformat" AutoPostBack="true"  dcn="emailtemplate_mailformat" OnSelectedIndexChanged="ddlMailType_Changed"  runat="server" m="mailformat" cn="" CssClass="ddl">
						    <asp:ListItem Value="HTML" Text="HTML" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Plain Text" Text="Plain Text"></asp:ListItem>
						</asp:DropDownList></td>
						
					</tr>
					<tr>
						<td class="label">Upload File</td>
						<td><uc:MultiFileUpload ID="mfuuploadfile"  IsMutiple="true"   FileType="Any" FolderPath="upload/emailtemplate" runat="server" CssClass="textbox "></uc:MultiFileUpload></td>
					</tr>
				    <%--<tr>
                        <td class="label">
                            Attach File
                        </td>
                        <td>
                            <uc:Multifileupload ID="multifileUploader" runat="server" />
                        </td>
                    </tr> --%>
					<tr>
                        <td style="vertical-align:top;" class="label">Message</td><td>
                        <asp:TextBox CssClass="textarea tinymce desc" ID="txtbody" runat="server" TextMode="MultiLine" Height="300" Width="750"/></td>
                        <td></td>
                    </tr>
				
                <!--SUBGRID_START-->
					
					<!--SUBGRID_END-->
                </table>
            </td>
         </tr>
		 <tr>
            <td align="center">
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="button" ValidationGroup="vg"/>
                <input type="button" class="close-page cancel" value="Cancel"/>
            </td>
        </tr>
        <!--FOLLOWUP_START-->
        <!--FOLLOWUP_END-->
    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
