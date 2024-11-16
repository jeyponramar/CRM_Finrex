<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="company-config.aspx.cs" 
Inherits="utilities_company_config" Title="Company Configuration" %>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server" Text="Company Configuration"/>
        </td>
     </tr>
     <tr>
        <td align="center">
            <asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label>
        </td>
     </tr>
     <tr>
        <td>
            <table width="100%">
                <tr>
                    <td class="label">Company Profile</td>
                    <td>
                        <uc:MultiFileUpload ID="mfucompanyprofile" IsMultiple="false" FileType="Any" FolderPath="upload/companyprofile" runat="server"></uc:MultiFileUpload>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Button ID="btnsubmit" runat="server" Text="Submit" CssClass="button" OnClick="btnsubmit_Click" /></td>
                </tr>
            </table>
        </td>
     </tr>
</table>     
</asp:Content>

