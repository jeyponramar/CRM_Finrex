 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="add.aspx.cs" Inherits="ChangePassword_add" EnableEventValidation="false" %>
<%--CONTROLREGISTER_START--%>
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
                
					<tr>
					    <td class="label">
					        User Name
					    </td>
					    <td>
					    <asp:TextBox ID="username" runat="server"  Enabled="false" CssClass="textbox"></asp:TextBox>
					    </td>
					</tr>
					<tr>
						<td class="label">Current Password <span class="error">*</span></td>
						<td><asp:TextBox TextMode="Password" ID="txtcurrentpassword" runat="server"  dcn="changepassword_currentpassword" CssClass="textbox"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv1" Display="Dynamic" runat="server" ErrorMessage="Required Current Password" ValidationGroup="vg" ControlToValidate="txtcurrentpassword"></asp:RequiredFieldValidator></td>
					</tr>
					<tr>
						<td class="label">New Password <span class="error">*</span></td>
						<td><asp:TextBox TextMode="Password" ID="txtnewpassword" runat="server"  dcn="changepassword_newpassword" CssClass="textbox"></asp:TextBox>
						
						<asp:RequiredFieldValidator ID="rfv2" Display="Dynamic" runat="server" ErrorMessage="Required New Password" ValidationGroup="vg" ControlToValidate="txtnewpassword"></asp:RequiredFieldValidator></td>
					</tr>
					<tr>
						<td class="label">Confirm Password <span class="error">*</span></td>
						<td><asp:TextBox TextMode="Password" ID="txtconfirmpassword" runat="server"  dcn="changepassword_confirmpassword" CssClass="textbox"></asp:TextBox>
						<asp:CompareValidator ID="CompareValidator1" ValidationGroup="vg" ControlToValidate="txtconfirmpassword" Display="Dynamic" ControlToCompare="txtnewpassword" Operator="Equal" runat="server" ErrorMessage="Your passwords do not match"></asp:CompareValidator>
						</td>
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
        <!--COMMENTS_START-->
        <!--COMMENTS_END-->
    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
