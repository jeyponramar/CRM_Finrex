<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="change_password.aspx.cs" Inherits="change_password" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%">
         <tr>
            <td class="page-title">
                CHANGE PASSWORD
            </td>
         </tr>
         <tr>
            <td class="form" colspan="2">
                <table width="90%" cellpadding="5">
                <tr>
                    <td align="center" colspan="4"> <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                </tr>
                
					<tr>
					    <td class="label" style="width:150px;">
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
					<tr>
					    <td></td>
					    <td><asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="button" ValidationGroup="vg"/></td>
					</tr>
					</table>
            </td>
         </tr>
		
    </table>
</asp:Content>

