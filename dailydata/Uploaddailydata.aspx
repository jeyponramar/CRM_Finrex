<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="Uploaddailydata.aspx.cs" Inherits="dailydata_Uploaddailydata" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>_enabledFileUpload = false;</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="title">
            Daily File Upload
        </td>
     </tr>
     <tr>
        <td>
             <table cellpadding='0' cellspacing='10'>
                <tr>
                    <td>InoFinExportData.csv File: </td>
                    <td><asp:FileUpload ID="fleinofinupload" runat="server" /></td>
                </tr>
                <tr>
                    <td>Url</td>
                    <td><asp:Literal ID="ltdownloadurl" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="button" OnClick="btnUpload_Click" /></td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Label ID="lblmessage" runat="server" class="error"></asp:Label></td>
                </tr>
         </table>
        </td>
     </tr>
</table>
</asp:Content>

