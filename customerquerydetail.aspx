<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="customerquerydetail.aspx.cs" Inherits="customerquerydetail" %>
<%@ Register Src="~/QueryMenu.ascx" TagName="QueryMenu" TagPrefix="uc" %>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link href="js/upload/jquery.fileupload-ui.css" rel="stylesheet" type="text/css" />    
<script src="js/upload/jquery.fileupload.js?v=<%=VersionNo %>"></script>
<script src="js/upload/jquery.fileupload-ui.js?v=<%=VersionNo %>"></script>
<script src="js/upload/multifileupload.js?v=<%=VersionNo %>"></script>
<script>
    $(document).ready(function() {
        $(".btnresolvequery").click(function() {
            if (!confirm("Are you sure you want to close your query?")) return false;
            return true;
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr>
        <td class='page-inner2' cellpadding="0" cellspacing=0>
        <table width="100%">
            <tr>
                 <td width="250" valign="top" class="left-menu-box">
                    <uc:QueryMenu ID="queryMenu" runat="server" />
                </td>
                <td valign="top">
                    <table width='100%'><tr><td class='page-title2'>Query Details</td></tr>
                        <tr><td><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td></tr>
                        <tr>
                            <td>
                                <table cellspacing=10>
                                    <tr>
                                        <td class="label">Query Type</td><td style="min-width:200px;"><asp:Label ID="lbltype" runat="server" CssClass="val"></asp:Label></td>
                                        <td class="label">Topic</td><td><asp:Label ID="lbltopic" runat="server" CssClass="val"></asp:Label></td>
                                    </tr>
                                     <tr>
                                        <td class="label">Status</td><td><asp:Label ID="lblstatus" runat="server" CssClass="val"></asp:Label></td>
                                        <td class="label">Submitted Date</td><td><asp:Label ID="lbldate" runat="server" CssClass="val"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Subject</td>
                                        <td colspan="3"><asp:Label ID="lblsubject" runat="server" CssClass="val"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Your Query</td>
                                        <td colspan="3"><asp:Label ID="lblquery" runat="server" CssClass="val"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Attachments</td>
                                        <td colspan="3">
                                            <asp:Literal ID="ltattachment" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr><td>&nbsp;</td></tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr><td class="bold">Responses :</td></tr>
                                    <tr><td>&nbsp;</td></tr>
                                    <tr><td><asp:Literal ID="ltreplies" runat="server"></asp:Literal></td></tr>
                                    <tr><td>&nbsp;</td></tr>
                                    <tr><td><asp:Button ID="btnResolve" runat="server" OnClick="btnresolved_click" Text="Resolved" CssClass="button btnresolvequery"/></td></tr>
                                    <tr><td>
                                        <table width="100%" cellpadding="10">
                                            <tr>
                                                <td class="label">Your Reply Message <span class="error">*</span></td>
                                                <td>
                                                    <asp:TextBox ID="txtresponse" runat="server" CssClass="textarea" TextMode="MultiLine" Width="400" Height="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtresponse" ErrorMessage="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
					                            <td class="label">Attachments</td>
					                            <td><uc:MultiFileUpload ID="mfureplyattachment" runat="server" IsPopulateFiles="false" IsMultiple="true" FileType="Any" FolderPath="upload/customerquery/attachment"></uc:MultiFileUpload></td>
				                            </tr>
                                            <tr>
                                                <td></td>
                                                <td><asp:Button ID="btnreply" runat="server" OnClick="btnreply_click" Text="Reply" CssClass="button" ValidationGroup="vg"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </td></tr>
                                    <tr><td>&nbsp;</td></tr>
                                </table>
                            </td>
                         </tr>
                        </table>
                </td>
            </tr>
        </table>
        
       </td>
    </tr>
 </table>
 
</asp:Content>

