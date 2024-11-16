 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="DailyEmailSetting_add" EnableEventValidation="false" ValidateRequest="false"%>
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
											<td class="label">Name <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="txtname"  IsUnique="true" MaxLength="100" runat="server"  dcn="dailyemailsetting_name" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Name" ValidationGroup="vg" ControlToValidate="txtname"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Subject <span class="error">*</span></td>
											<td ti='1'><asp:TextBox ID="txtsubject" MaxLength="300" runat="server"  dcn="dailyemailsetting_subject" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Subject" ValidationGroup="vg" ControlToValidate="txtsubject"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Query <span class="error">*</span></td>
											<td ti='2'><asp:TextBox TextMode="MultiLine" ID="txtquery"  Width="400" Height="150"  dcn="dailyemailsetting_query" ml="0" runat="server" CssClass="textarea"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Query" ValidationGroup="vg" ControlToValidate="txtquery"></asp:RequiredFieldValidator></td>
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
					<tr><td class='label'>Email Body</td></tr>
					<tr>
						<td><asp:TextBox TextMode="MultiLine" ID="txtemailbody"  Width="800" Height="150"  dcn="dailyemailsetting_emailbody" runat="server" CssClass="htmleditor"></asp:TextBox></td>
					</tr>
					
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="2">
										<tr>
											<td class="label">To Email Id</td>
											<td ti='0'><asp:TextBox ID="txttoemailid" MaxLength="100" runat="server"  dcn="dailyemailsetting_toemailid" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Role</td>
											<td ti='1'><asp:TextBox ID="txtrole"  dcn="dailyemailsetting_role" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Time <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txttime" MaxLength="100" runat="server"  dcn="dailyemailsetting_time" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv6" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Time" ValidationGroup="vg" ControlToValidate="txttime"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Is Email</td>
											<td ti='3'><asp:CheckBox ID="chkisemail"  AutoPostBack="true" OnCheckedChanged="chkIsEmail_Click"  dcn="dailyemailsetting_isemail" runat="server"></asp:CheckBox></td>
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
					<!--SAVEBUTTON_END-->
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
