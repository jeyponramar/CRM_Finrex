<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewFindoc.aspx.cs" Inherits="ViewFindoc" Title="ViewFindoc Document " %>
<%@ Register Src="~/usercontrols/FinDocMenu.ascx" TagName="findocmenu" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<style>
body{background-color:#fff !important;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <table width='100%' cellpadding=0 cellspacing=0>
        <tr>
            <td class="finmenubg">
               <uc:findocmenu id="findocmenu" runat="server" IsAdminPage="false"></uc:findocmenu>
            </td>
             <td class='page-inner2' cellpadding="0" cellspacing="0">
                 <table width='100%'>
                       <tr>
                            <td class="page-title2">View Findoc Document</td>
                       </tr>
                       <tr>
                            <td align="right" style="padding-right:20px;">
                                <a href="AddFindoc.aspx" class="add-link">Add Document</a>
                            </td>
                       </tr>
                       <tr>
                <td>
                    <asp:Literal ID="ltdata" runat="server"></asp:Literal>
                </td>
            </tr>
                 </table>
             </td>
        </tr>
 </table>
</asp:Content>

