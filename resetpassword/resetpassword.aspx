<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="resetpassword.aspx.cs" Inherits="resetpassword_resetpassword"  EnableEventValidation="false" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:PlaceHolder ID="form" runat="server">
    <table width="100%">
         <tr>
            <td class="title">
                <asp:Label ID="lblPageTitle" runat="server"/>
            </td>
         </tr>
         <tr>
            <td>
               
                <input type="button" value="Copy" class="copy button dpage"/>
                <asp:TextBox ID="h_IsCopy" runat="server" CssClass="iscopy hidden" Text="0"></asp:TextBox>
            </td>
            <td align="right"> 
                &nbsp;
            </td>
         </tr>
         <tr>
            <td class="form" colspan="2">
                <table width="90%" cellpadding="0">
                <tr>
                    <td align="center"><asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                </tr>
					<table width='100%' cellpadding='0' cellspacing='0'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5'>
										<tr>
					                        <td class="label">Select User <span class="error">*</span></td>
					                        <td><asp:DropDownList ID="ddluserid"  dcn="user_username" runat="server" m="user" cn="username" CssClass="ddl"></asp:DropDownList>
					                        <asp:RequiredFieldValidator ID="rfv2" Display="Dynamic" runat="server" ErrorMessage="Required Select User" ValidationGroup="vg" ControlToValidate="ddluserid"  InitialValue="0"></asp:RequiredFieldValidator></td>
				                        </tr>
										<tr>
											<td class="label">Reset Password <span class="error">*</span></td>
											<td><asp:TextBox ID="txtresetpassword"  MaxLength="100" runat="server"  CssClass="textbox" TextMode="Password"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfv0" SetFocusOnError="true" Display="Dynamic" runat="server" ErrorMessage="Required Reset Password " ValidationGroup="vg" ControlToValidate="txtresetpassword"></asp:RequiredFieldValidator></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5'>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					</table>
                </table>
            </td>
         </tr>
		 <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Reset" CssClass="save button" ValidationGroup="vg"/>
            </td>
        </tr>

    </table>
</asp:PlaceHolder>
</asp:Content>

