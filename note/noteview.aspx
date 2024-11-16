<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="noteview.aspx.cs" Inherits="note_noteview" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="../js/note.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="title">
            My Notes
        </td>
        <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
     </tr>
     <tr>
        <td colspan="2" class="spage bold" href="#/note/a/view" title="View Note" align="center">Grid View</td>
     </tr>
     <tr>
        <td>
            <table width="80%">
                <tr><td><asp:Literal ID="ltMyNotes" runat="server"></asp:Literal></td></tr>
            </table>
            
        </td>
     </tr>
</table>         
</asp:Content>

