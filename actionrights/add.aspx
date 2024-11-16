 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="ActionRights_add" EnableEventValidation="false" ValidateRequest="false"%>
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
											<td class="label">Role <span class="error">*</span></td>
											<td ti='0'><asp:TextBox ID="role"  search='true'  dcn="role_rolename" MaxLength="100" runat="server" m="role" cn="rolename" CssClass="textbox ac txtac search='true' "></asp:TextBox><asp:TextBox id="txtroleid"  dcn="actionrights_roleid"  Text="0" runat="server" class=" hdnac"/><img src="../images/down-arrow.png" class="epage"/>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Role" ValidationGroup="vg" ControlToValidate="role"></asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td class="label">Module Id</td>
											<td ti='1'><asp:TextBox ID="txtmoduleid"  dcn="actionrights_moduleid" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox></td>
										</tr>
										<tr>
											<td class="label">Action</td>
											<td ti='2'><asp:TextBox ID="txtaction"  dcn="actionrights_action" runat="server" MaxLength="10" CssClass="mbox val-i" Text="0"></asp:TextBox></td>
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
					<!--SAVEBUTTON_END-->
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
