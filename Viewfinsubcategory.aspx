<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Viewfinsubcategory.aspx.cs" Inherits="Viewfinsubcategory" Title="Viewfinsubcategory" %>
<%@ Register Src="~/usercontrols/FinDocMenu.ascx" TagName="findocmenu" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style>
body{background-color:#fff !important;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellpadding=0 cellspacing=0>
        <tr>
            <%--<td class="finmenubg">
                <uc:findocmenu ID="findocmenu" runat="server" IsAdminPage="false"></uc:findocmenu>
            </td>--%>  
             <td class='page-inner2' cellpadding="0" cellspacing="0">
                 <table width='100%'>
                       <tr>
                            <td class="page-title2">View Subcategory</td>
                       </tr>
                       <tr>
                            <td>
                                <table width='100%' cellpadding=0 cellspacing=0>
                                    <tr>
                                        <td style="padding-left:10px;">
                                            <a href="Addfinsubcategory.aspx" class="add-link">Add Subcategory</a>
                                        </td>
                                        <td align="right" style="padding-right:10px;">
                                            <uc:findocmenu id="findocmenu" runat="server" IsAdminPage="false"></uc:findocmenu>
                                            
                                        </td>
                                    </tr>
                                </table>
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

