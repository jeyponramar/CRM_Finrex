<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="print-page.aspx.cs" Inherits="utilities_print_page" Title="Print" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        bindPage();
        function bindPage() {
            var tbl = $(".title", window.opener.document).closest("table");
            var tdpage = $(".tdpage");
            $(".tdpage").html(tbl.html());
            tdpage.find("img").hide();
            tdpage.find(":button").hide();
            tdpage.find(":submit").hide();
            tdpage.find(".title").text(tdpage.find(".title").text().replace("Edit ", ""));
            var frm = tdpage.find(".form");
            frm.removeClass("form");
            frm.css("border", "none");
            frm.addClass("circle");
            frm.find(".error").each(function() {
                if ($(this).text() == "*") $(this).remove();
            });
            tdpage.find("input[type=text],textarea,select").each(function() {
                if ($(this).css("display") != "none") {
                    if ($(this).parent().find(".dval").length == 0) {
                        var val = "";
                        if ($(this)[0].tagName == "SELECT") {
                            val = $(this).find("option:selected").text();
                            if (val == "Select") val = "";
                        }
                        else {
                            val = $(this).val();
                        }
                        $(this).parent().append($("<span class='val dval'>" + val + "</span>"));
                        $(this).hide();
                    }
                }
            });
        }
        $(".print").click(function() {
            window.print();
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="800">
    <tr class="noprint">
        <td><img src="../images/print-s.png" title="Print" class="print hand"/></td>
    </tr>
    <tr><td style="border-bottom:solid 1px #000;">
        <table width="100%">
            <tr>
                <td width="90%" align="center"><h2><asp:Label ID="lblCompanyName" runat="server"></asp:Label></h2></td>
                <td align="right"><asp:Label ID="lblDate" CssClass="bold" runat="server"></asp:Label></td>
            </tr>
        </table>
        
    </td></tr>
    <tr>
        <td>
            <table class="tdpage" width="100%"></table>
        </td>
    </tr>
</table>
</asp:Content>

