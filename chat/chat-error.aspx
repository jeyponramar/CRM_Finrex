<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="chat-error.aspx.cs" 
Inherits="chat_error" Title="Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server" Text="Error"/>
        </td>
     </tr>
     <tr>
        <td class="error">
            <asp:Label ID="lblErrorMessage" runat="server"/>
        </td>
     </tr>   
     
     
</table>
</asp:Content>

