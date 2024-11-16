<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="myprofile.aspx.cs" 
Inherits="myprofile" Title="My Profile" %>
<%@ Register Src="~/Usercontrols/AddEditClientKYC.ascx" TagName="AddEditClientKYC" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr>
        <td class='page-inner2' cellpadding="0" cellspacing=0>
        <table width="100%">
            <tr>
                <td class="page-title2">My Profile</td>
            </tr>
            <tr>
                <td style="background-color:#fff;color:#000;"><uc:AddEditClientKYC id="AddEditClientKYC1" runat="server" /></td>
            </tr>
        </table>
        
       </td>
    </tr>
 </table>
</asp:Content>

