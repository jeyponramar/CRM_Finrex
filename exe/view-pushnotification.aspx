<%@ Page Language="C#" MasterPageFile="~/exe/ExeMasterPage.master" AutoEventWireup="true" CodeFile="view-pushnotification.aspx.cs" 
Inherits="exe_view_pushnotification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr>
        <td class='page-inner2' cellpadding="0" cellspacing=0>
        <table width="100%">
            <tr>
                <td class="page-title2">
                    <table width="100%">
                        <tr>
                            <td width="30"><i class='icon ion-android-arrow-back jq-page-back'></i></td>
                            <td>View Push Notification</td>
                        </tr>
                    </table>
                
                </td>
            </tr>
            <tr><td><asp:Literal ID="ltpushnotifications" runat="server"></asp:Literal></td></tr>
        </table>
        </td>
     </tr>
</table>
</asp:Content>

