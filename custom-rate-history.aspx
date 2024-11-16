<%@ Page Title="" Language="C#" MasterPageFile="~/Blank.master" AutoEventWireup="true" CodeFile="custom-rate-history.aspx.cs" Inherits="custom_rate_history" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        $(".lnkcustomrate").click(function() {
            window.open("custom-rate-detail.aspx?dt=" + $(this).attr("dt"), "", "width=550,height=450");
        });
        $(".folder-icon").click(function() {
            var tr = $(this).closest("tr").next();
            if (tr.css("display") == "none") {
                tr.show();
            }
            else {
                tr.hide();
            }
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr><td class='page-inner2'>
        <table width='100%'><tr><td class='page-title2'>Custom Rate Archive</td></tr>
        <%--<tr><td>&nbsp;</td></tr><tr><td><asp:Literal ID="ltdata" runat="server"></asp:Literal></td></tr>--%>
        <tr><td>&nbsp;</td></tr><tr><td style="padding:10px;"><asp:Literal ID="lthistory" runat="server"></asp:Literal></td></tr>
        <tr><td>&nbsp;</td></tr>
        <tr><td><a href="https://www.cbic.gov.in/entities/cbic-content-mst/MTcwMDU%3D" target="_blank">https://www.cbic.gov.in/entities/cbic-content-mst/MTcwMDU=</a></td></tr>
        </table>
       </td>
    </tr>
    
 </table>
</asp:Content>

