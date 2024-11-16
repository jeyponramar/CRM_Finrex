<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="customerquery.aspx.cs" Inherits="customerquery" %>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link href="js/upload/jquery.fileupload-ui.css" rel="stylesheet" type="text/css" />    
<script src="js/upload/jquery.fileupload.js?v=<%=VersionNo %>"></script>
<script src="js/upload/jquery.fileupload-ui.js?v=<%=VersionNo %>"></script>
<script src="js/upload/multifileupload.js?v=<%=VersionNo %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr><td class='page-inner2'>
        <table width='100%'><tr><td class='page-title2'>Customer Query</td></tr>
        <tr><td><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td></tr>
        <tr>
            <td>
                <table cellspacing=10>
                    <tr>
                        <td>Topic</td><td><asp:Label ID="lbltopic" runat="server" CssClass="bold"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Subject <span class="error">*</span></td>
                        <td><asp:TextBox ID="txtsubject" runat="server" CssClass="textbox" Width="400"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtsubject" ErrorMessage="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Your Query <span class="error">*</span></td>
                        <td><asp:TextBox ID="txtquery" runat="server" CssClass="textarea" Width="400" Height="100" TextMode="MultiLine"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv2" runat="server" ControlToValidate="txtquery" ErrorMessage="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
					    <td class="label">Attachments</td>
					    <td><uc:MultiFileUpload ID="mfuattachments" IsPopulateFiles="false" runat="server" IsMultiple="true" FileType="Any" FolderPath="upload/customerquery/attachment" runat="server"></uc:MultiFileUpload></td>
				    </tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr><td></td><td><asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click" CssClass="button" ValidationGroup="vg"/></td></tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr><td></td><td><a href="viewcustomerquery.aspx">View Queries</a></td></tr>
                </table>
            </td>
        </tr>
        </table>
       </td>
    </tr>
    
 </table>
</asp:Content>

