<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="Uploaddailydata.aspx.cs" Inherits="dailydata_Uploaddailydata" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>_enabledFileUpload = false;</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <table width='50%' cellpadding='0' cellspacing='5'>
        <tr>
            <td>File Name:</td>
            <td><asp:FileUpload ID="fleupload" runat="server" /></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" /></td>
        </tr>
        <tr>
            <td colspan="2"><asp:Label ID="lblmessage" runat="server" class="error"></asp:Label></td>
        </tr>
 </table>
</asp:Content>

