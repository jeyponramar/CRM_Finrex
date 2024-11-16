<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
CodeFile="updateclientkycdetail.aspx.cs" Inherits="client_updateclientkycdetail" Title="Update KYC Details" %>
<%@ Register Src="~/Usercontrols/AddEditClientKYC.ascx" TagName="AddEditClientKYC" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server" Text="Update KYC Details"/>
        </td>
     </tr>
     <tr>
        <td><uc:AddEditClientKYC id="AddEditClientKYC" runat="server" /></td>
     </tr>
</table>
</asp:Content>

