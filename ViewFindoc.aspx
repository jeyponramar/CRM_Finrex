﻿<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewFindoc.aspx.cs" Inherits="ViewFindoc" Title="ViewFindoc Document " %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <table width='100%' cellpadding=0 cellspacing=0>
        <tr>
             <td class='page-inner2' cellpadding="0" cellspacing="0">
                 <table width='100%'>
                       <tr>
                            <td class="page-title2">View Findoc Document</td>
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
