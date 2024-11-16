 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="advisor-settings.aspx.cs" Inherits="advisor_settings" EnableEventValidation="false" ValidateRequest="false"%>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:PlaceHolder ID="form" runat="server">
    <table width="100%">
         <tr>
            <td class="title">
                <asp:Label ID="lblPageTitle" runat="server" Text="Advisor Settings"/>
            </td>
         </tr>
         <tr>
            <td class="form">
                <table width="90%" cellpadding="0">
                <tr>
                    <td align="center"><asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                </tr>
                <tr>
                    <td>
					<table width='100%' cellpadding='0' cellspacing='0' id='tblcontrols1' runat='server'>
					<tr>
						<td>
							<table width='100%' cellpadding='3' cellspacing='5'>
								<tr>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="0">
										<tr>
											<td class="label">EXIM To Email Id <span class="error">*</span></td>
											<td><asp:TextBox ID="txteximemailid" runat="server" CssClass="textbox"></asp:TextBox>
											<asp:RequiredFieldValidator id="rfv1" runat="server" ControlToValidate="txteximemailid" ErrorMessage="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
											</td>
										</tr>
										<tr>
											<td class="label">EXIM Bcc Email Id</td>
											<td><asp:TextBox ID="txteximbccemailid" runat="server" CssClass="textbox"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td class="label">FEMA To Email Id</td>
											<td><asp:TextBox ID="txtfemaemailid" runat="server" CssClass="textbox"></asp:TextBox>
											<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ControlToValidate="txtfemaemailid" ErrorMessage="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
											</td>
										</tr>
										<tr>
											<td class="label">FEMA Bcc Email Id <span class="error">*</span></td>
											<td><asp:TextBox ID="txtfemabccemailid" runat="server" CssClass="textbox"></asp:TextBox>
											</td>
										</tr>
										<tr><td></td>
										<td><asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button" OnClick="btnSubmit_Click" ValidationGroup="vg"/></td>
										</tr>
										</table>
									</td>
									
								</tr>
							</table>
						</td>
					</tr>
					</table>
				</td></tr>
                
    </table>
    </td>
     </tr>
     
    </table>
</asp:PlaceHolder>

</asp:Content>
