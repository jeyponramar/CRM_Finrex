 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="CustomerQuery_add" EnableEventValidation="false" ValidateRequest="false"%>
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
											<td class="label">Query Topic <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="querytopic"  search='true'  dcn="querytopic_topicname" MaxLength="100" runat="server" m="querytopic" cn="topicname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtquerytopicid"  dcn="customerquery_querytopicid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Query Topic" ValidationGroup="vg" ControlToValidate="querytopic"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Subject <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtsubject"  search='true'  MaxLength="0" runat="server"  dcn="customerquery_subject" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Subject" ValidationGroup="vg" ControlToValidate="txtsubject"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Date</td>
											<td ti='2'><asp:TextBox ID="txtdate"  dcn="customerquery_date" runat="server" autocomplete="off" MaxLength="20" Format="DateTime" CssClass="textbox datetimepicker"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Query</td>
											<td ti='3'><asp:TextBox TextMode="MultiLine" ID="txtquery"  dcn="customerquery_query"  runat="server" CssClass="textarea"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Attachments</td>
											<td ti='4'><uc:MultiFileUpload ID="mfuattachments"  IsMultiple="true" FileType="Any" FolderPath="upload/customerquery/attachment" runat="server" CssClass="textbox "></uc:MultiFileUpload></td>
										</tr>
										<tr>
											<td class="label">isadvisorvisited</td>
											<td ti='5'><asp:CheckBox ID="chkisadvisorvisited"  dcn="customerquery_isadvisorvisited" runat="server"></asp:CheckBox></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Status</td>
											<td ti='0'><asp:TextBox ID="querystatus"  Enabled="false"  search='true'  dcn="querystatus_status" MaxLength="100" runat="server" m="querystatus" cn="status" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtquerystatusid"  dcn="customerquery_querystatusid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
										</tr>
										<tr>
											<td class="label">User</td>
											<td ti='1'><asp:TextBox ID="user"  Enabled="false"  dcn="user_name" MaxLength="100" runat="server" m="user" cn="name" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtuserid"  dcn="customerquery_userid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="quick-new epage"/></td>
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
