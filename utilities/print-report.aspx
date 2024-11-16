<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="print-report.aspx.cs" Inherits="utilities_print_report" Title="Print" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    //window.print();
    $(document).ready(function() {
        bindColumns();
        bindSearchPanel();
        $(".print").click(function() {
            window.print();
        });
        $(".btnsaveprint").click(function() {
            $("#printcolumns").dialog("close");
        });
        $(".chkprint").click(function() {
            showHideColumns();
        });
        function showHideColumns() {
            $(".chkprint").each(function() {
                var colindex = ConvertToInt($(this).attr("id").replace("chk", "")) + 1;
                var isshow = $(this).is(":checked");
                var col = 0;
                $(".grid-report").find("tr").each(function() {
                    if (isshow) {
                        $(this).find("td:nth-child(" + colindex + ")").show();
                    }
                    else {
                        $(this).find("td:nth-child(" + colindex + ")").hide();
                    }
                });
            });
        }
        function bindSearchPanel() {
            var tbl = $(".tblplsearch", window.opener.document);
            var html = tbl.html();
            $(".tblsearchpanel").html(html);
            $(".tblsearchpanel").find("img").remove();
            $(".tblsearchpanel").find(".hdnac").remove();
            $(".tblsearchpanel").find(".button").remove();
            $(".tblsearchpanel").find("input[type=text],input[type=password],textarea,select").each(function() {
                var val = "";
                if ($(this)[0].tagName == "SELECT") {
                    val = $(this).find("option:selected").text();
                    if (val == "Select") val = "";
                }
                else {
                    val = $(this).val();
                }
                val = ": " + val;
                $(this).parent().append($("<span class='val dval'>" + val + "</span>"));
                $(this).hide();
            });
        }
        $(".setprintcolumns").click(function() {
            //$(".tblcolumns").show();
            $("#printcolumns").dialog({
                autoOpen: true,
                width: 500,
                height: 500,
                modal: true,
                show: {
                    effect: "blind",
                    duration: 500
                },
                hide: {
                    effect: "explode",
                    duration: 500
                }
            });
        });
        function bindColumns() {
            var html = "<table>";
            var i = 0;
            $(".rowheader").find("td").each(function() {
                html += "<tr><td><input checked='checked' class='chkprint' type='checkbox' id='chk" + i + "'/></td><td>" + $(this).text() + "</td></tr>";
                i++;
            });
            html += "</table>";
            $(".tblcolumns").html(html);
        }
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="printcolumns" title="Print Columns" class="hidden">
    <table width="100%">
        <tr>
            <td>
            <div style="height:400px;overflow:scroll;">
                <table class="tblcolumns"></table>
            </div>    
            </td>
        </tr>
        <tr>
            <td align="center"><input type="button" id="btnsaveprint" class="button" value="OK" /></td>
        </tr>
    </table>
    
</div>
<table width="100%">
    <tr><td colspan="2" align="right" class="noprint">
        <table>
            <tr>
                <td><img src="../images/action-buttons/grid-setting.png" title="Set Grid Columns"class="hand setprintcolumns"/></td>
                <td><img src="../images/print-s.png" title="Print" class="print hand"/></td>
            </tr>
        </table>
    </td></tr>
    <tr><td colspan="2" align="center"><h2><asp:Label ID="lblCompanyName" runat="server"></asp:Label></h2></td></tr>
    <tr>    
        <td><asp:Label ID="lblTitle" runat="server" CssClass="title"></asp:Label></td>
        <td align="right"><asp:Label ID="lblDate" CssClass="bold" runat="server"></asp:Label></td>
    </tr>
    <tr><td colspan="2"><table class="tblsearchpanel"></table></td></tr>
    <tr>
        <td colspan="2">
            <asp:Literal ID="ltData" runat="server"></asp:Literal>
        </td>
    </tr>
</table>
</asp:Content>

