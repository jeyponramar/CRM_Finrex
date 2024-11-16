 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="Webinar_add" EnableEventValidation="false" ValidateRequest="false"%>
 <%@ Register Src="~/usercontrols/NextPrevDetail.ascx" TagName="NextPrevDetail" TagPrefix="uc" %>
<%--CONTROLREGISTER_START--%>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>
<%--CONTROLREGISTER_END--%>
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
					<table width='100%' cellpadding='0' cellspacing='0' id='tblcontrols1' runat='server'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
										<tr>
											<td class="label">Title <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txttitle"  IsUnique="true"  search='true'  MaxLength="0" runat="server"  dcn="webinar_title" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Title" ValidationGroup="vg" ControlToValidate="txttitle"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Presenters <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtpresenters"  search='true'  MaxLength="0" runat="server"  dcn="webinar_presenters" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Presenters" ValidationGroup="vg" ControlToValidate="txtpresenters"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Recorded Date <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txtrecordeddate"  dcn="webinar_recordeddate" runat="server" autocomplete="off" MaxLength="11" Format="Date" CssClass="textbox datepicker"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Recorded Date" ValidationGroup="vg" ControlToValidate="txtrecordeddate"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Description <span class="error">*</span></td>
											<td ti='3'><asp:TextBox TextMode="MultiLine" ID="txtdescription"  Width="500" Height="200"  dcn="webinar_description" runat="server" CssClass="htmleditor"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv3" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Description" ValidationGroup="vg" ControlToValidate="txtdescription"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Recording Url</td>
											<td ti='4'><asp:TextBox ID="txtrecordingurl"  search='true'  MaxLength="0" runat="server"  dcn="webinar_recordingurl" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Attachment</td>
											<td ti='5'><uc:MultiFileUpload ID="mfuattachment"  IsMultiple="true" FileType="Any" FolderPath="upload/webinar" runat="server" CssClass="textbox "></uc:MultiFileUpload></td>
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
