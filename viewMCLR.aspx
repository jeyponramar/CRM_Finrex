<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="viewMCLR.aspx.cs" Inherits="viewmclr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        $(".lnkcustomrate").click(function() {
            window.open("view-mclr-detail.aspx?id=" + $(this).attr("mclrid"), "", "width=1000,height=550");
        });
       
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr><td class='page-inner2'>
        <table width='100%'><tr><td class='page-title2'>MCLR</td></tr>
        <tr><td style="padding:10px;">Updated on <asp:Label ID="lblupdatedon" runat="server"></asp:Label></td></tr>
        <tr><td style="padding:10px;"><asp:Literal ID="lthistory" runat="server"></asp:Literal></td></tr>
        <tr><td>&nbsp;</td></tr>
        </table>
       </td>
    </tr>
    
 </table>
</asp:Content>

