<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="delete.aspx.cs" Inherits="utilities_delete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        $(".canceldel").click(function() {
            window.history.back();
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server" Text="Delete Confirmation"/>
        </td>
     </tr>
     <tr><td>&nbsp;</td></tr>
     <tr>
        <td><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td>
     </tr>
     <tr><td><asp:Button ID="btnOK" runat="server" Visible="false" Text="OK" CssClass="button cancel" Width="50"/></td></tr>
     <tr><td>&nbsp;</td></tr>
     <tr><td><asp:Literal ID="ltRelatedModules" runat="server"></asp:Literal></td></tr>
     <tr><td>&nbsp;</td></tr>
     <tr><td><asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" CssClass="btnred button"/>&nbsp;&nbsp;
             <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="button cancel"/></td></tr>
    
</table>
</asp:Content>

