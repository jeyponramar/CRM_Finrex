 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="User_add" EnableEventValidation="false" ValidateRequest="false"%>
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
            <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
         </tr>
         <tr>
            <td>
                <input type="button" value="Edit" class="edit button dpage"/>
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
                
					<table width='100%' cellpadding='0' cellspacing='0'>
					<!--CONTROLS_START-->
					<table width='100%' cellpadding='0' cellspacing='0'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
										<tr>
											<td class="label">User Name</td>
											<td ti='0'><asp:TextBox ID="txtusername"  MaxLength="100" runat="server"  dcn="user_username" CssClass="textbox"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Email Id</td>
											<td ti='1'><asp:TextBox ID="txtemailid"  dcn="user_emailid"  search='true' MaxLength="100" runat="server" CssClass="textbox val-email val-email "></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Mobile No <span class="error">*</span></td>
											<td ti='2'><asp:TextBox ID="txtmobileno"  MaxLength="100" runat="server"  dcn="user_mobileno" CssClass="textbox val-mobile "></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv2" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Mobile No" ValidationGroup="vg" ControlToValidate="txtmobileno"></asp:RequiredFieldValidator></td>
										</tr>
										<tr ID="trpassword" runat="server" trPassword>
											<td class="label">Password <span class="error">*</span></td>
											<td ti='3'><asp:TextBox ID="txtpassword"  TextMode="Password"  MaxLength="100" runat="server"  dcn="user_password" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv3" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Password" ValidationGroup="vg" ControlToValidate="txtpassword"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Full Name <span class="error">*</span></td>
											<td ti='4'><asp:TextBox ID="txtfullname"  search='true'  MaxLength="100" runat="server"  dcn="user_fullname" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv4" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Full Name" ValidationGroup="vg" ControlToValidate="txtfullname"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Role <span class="error">*</span></td>
											<td ti='5'><asp:DropDownList ID="ddlroleid"  dcn="user_roleid"  search='true' runat="server" m="role" cn="rolename" CssClass="ddl"></asp:DropDownList>
						<asp:RequiredFieldValidator ID="rfv5" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Role" ValidationGroup="vg" ControlToValidate="ddlroleid"  InitialValue="0"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Employee Name</td>
											<td ti='6'><asp:TextBox ID="employee"  search='true'  dcn="employee_employeename" MaxLength="100" runat="server" m="employee" cn="employeename" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtemployeeid"  dcn="user_employeeid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Agent</td>
											<td ti='7'><asp:TextBox ID="agent"  dcn="agent_agentname" MaxLength="100" runat="server" m="agent" cn="agentname" CssClass="textbox ac txtac"></asp:TextBox><asp:TextBox id="txtagentid"  dcn="user_agentid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/></td>
										</tr>
										<tr>
											<td class="label">Photo</td>
											<td ti='8'><uc:MultiFileUpload ID="mfuphoto"  IsMutiple="false" FileType="Image" ReSize="200x200" SaveExt="true" FolderPath="upload/user" SaveAs="jpg" runat="server" CssClass="textbox "></uc:MultiFileUpload>
							<asp:TextBox ID="txtphoto" runat="server" CssClass="hidden"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Email Signature</td>
											<td ti='9'><asp:TextBox TextMode="MultiLine" ID="txtemailsignature"  Height="80"  dcn="user_emailsignature" runat="server" CssClass="htmleditor"></asp:TextBox></td>
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
        <!--COMMENTS_START-->
        <!--COMMENTS_END-->
    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
