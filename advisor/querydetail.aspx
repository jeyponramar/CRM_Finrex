 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="querydetail.aspx.cs" Inherits="querydetail" EnableEventValidation="false" ValidateRequest="false"%>
 
<%--CONTROLREGISTER_START--%>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>
<%--CONTROLREGISTER_END--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--DESIGN_START-->
    <asp:PlaceHolder ID="form" runat="server">
    <table width="100%">
         <tr>
            <td class="title">
                <asp:Label ID="lblPageTitle" runat="server" Text="Query Details"/>
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
											<td class="label">Query Topic</td>
											<td><asp:Label ID="lbltopicname" runat="server" CssClass="val"></asp:Label></td>
										</tr>
										<tr>
											<td class="label">Subject</td>
											<td><asp:Label ID="lblsubject" runat="server" CssClass="val"></asp:Label></td>
										</tr>
										<tr>
											<td class="label">Attachments</td>
											<td ti='4'><asp:Literal ID="ltattachment" runat="server"></asp:Literal></td>
										</tr>
										<tr>
											<td class="label">Query</td>
											<td><asp:Label ID="lblquery" runat="server" CssClass="val"></asp:Label></td>
										</tr>
										</table>
									</td>
									<td width='50%' class='valign'>
										<table width='100%' cellpadding='3' cellspacing='5' class='tblform' tblindex="1">
										<tr>
											<td class="label">Status</td>
											<td><asp:Label ID="lblstatus" runat="server" CssClass="val"></asp:Label></td>
										</tr>
										<tr>
											<td class="label">User Name</td>
											<td><asp:Label ID="lblname" runat="server" CssClass="val"></asp:Label></td>
										</tr>
										<tr>
											<td class="label">User Email Id</td>
											<td><asp:Label ID="lblusername" runat="server" CssClass="val"></asp:Label></td>
										</tr>
										<tr>
											<td class="label">User Mobile No</td>
											<td><asp:Label ID="lblusermobileno" runat="server" CssClass="val"></asp:Label></td>
										</tr>
										<tr>
											<td class="label">Client Name</td>
											<td><asp:Label ID="lblcustomername" runat="server" CssClass="val"></asp:Label></td>
										</tr>
										<tr>
											<td class="label">Client Email Id</td>
											<td><asp:Label ID="lblemailid" runat="server" CssClass="val"></asp:Label></td>
										</tr>
										<tr>
											<td class="label">Client Mobile No</td>
											<td><asp:Label ID="lblmobileno" runat="server" CssClass="val"></asp:Label></td>
										</tr>
										<tr>
											<td class="label">Created Date</td>
											<td><asp:Label ID="lbldate" Format="DateTime" runat="server" CssClass="val"></asp:Label></td>
										</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					</table>
				</td></tr>
                <tr>
                    <td colspan="2">
                        <table width="50%">
                            <tr><td class="title">Responses</td></tr>
                            <tr><td>&nbsp;</td></tr>
                            
                            <tr id="trresponse" runat="server">
                                <td>
                                    <table width="100%">
                                        <tr><td class="jq-query-reply">Click here to Reply <span>+</span></td></tr>
                                        <tr>
                                             <td class="jq-query-reply-panel">
                                                 <table width="100%" cellpadding="10">
                                                    <tr>
                                                        <td class="label">Your Reply Message</td>
                                                        <td>
                                                            <asp:TextBox ID="txtresponse" runat="server" CssClass="textarea" TextMode="MultiLine" Width="400" Height="100"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label">Attachments</td>
                                                        <td>
                                                            <uc:MultiFileUpload ID="mfureplyattachment" IsMultiple="true" FileType="Any" FolderPath="upload/customerqueryreply" runat="server"></uc:MultiFileUpload>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td><asp:Button ID="btnreply" runat="server" OnClick="btnreply_click" Text="Reply" CssClass="button" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr><td>&nbsp;</td></tr>
                            <tr><td><asp:Literal ID="ltreplies" runat="server"></asp:Literal></td></tr>
                            <tr><td>&nbsp;</td></tr>
                            <tr><td>
                               
                            </td></tr>
                            <tr><td>&nbsp;</td></tr>
                        </table>
                    </td>
                 </tr>
    </table>
    </td>
     </tr>
     <tr>
        <td align="center">
			<input type="button" class="close-page cancel" value="Cancel"/>
        </td>
    </tr>
    </table>
</asp:PlaceHolder>
<!--DESIGN_END-->
<!--JSCODE_START-->
					
					<!--JSCODE_END-->

</asp:Content>
