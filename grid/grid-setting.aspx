<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="grid-setting.aspx.cs" Inherits="grid_setting" EnableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    var leftList;
    var rightList;
    $(document).ready(function() {
        leftList = $(".leftlist");
        rightList = $(".rightlist");

        $(".right-arr").click(function() {
            leftList.find("option").each(function() {
                if ($(this).attr("selected") == "selected") {
                    rightList.append("<option value='" + $(this).attr("value") + "'>" + $(this).text() + "</option>");
                    $(this).remove();
                }
            });
            bindGridColumns();
        });
        $(".left-arr").click(function() {
            rightList.find("option").each(function() {
                if ($(this).attr("selected") == "selected") {
                    leftList.append("<option value='" + $(this).attr("value") + "'>" + $(this).text() + "</option>");
                    $(this).remove();
                }
            });
            bindGridColumns();
        });
        $(".up-arr").click(function() {
            var selectedRow;
            var prevRow;
            rightList.find("option").each(function() {
                if ($(this).attr("selected") == "selected") {
                    selectedRow = $(this);
                }
                else {
                    if (selectedRow == undefined) prevRow = $(this);
                }
            });
            if (selectedRow.attr("value") != prevRow.attr("value")) {
                selectedRow.insertBefore(prevRow);
            }
            bindGridColumns();
        });
        $(".down-arr").click(function() {
            var selectedRow;
            var prevRow;
            rightList.find("option").each(function() {
                if ($(this).attr("selected") == "selected") {
                    selectedRow = $(this);
                }
                else {
                    if (selectedRow != undefined && prevRow == undefined) prevRow = $(this);
                }
            });
            if (selectedRow.attr("value") != prevRow.attr("value")) {
                selectedRow.insertAfter(prevRow);
            }
            bindGridColumns();
        });
        $(".save").click(function() {
            var gc = "";
            var gclabels = "";
            if (parseInt(rightList.find("option").length) < 1) {
                alert("Grid should have atleast one column");
                return false;
            }
            rightList.find("option").each(function() {
                if (gc == "") {
                    gc = $(this).attr("value");
                    gclabels = $(this).text();
                }
                else {
                    gc = gc + "," + $(this).attr("value");
                    gclabels += "," + $(this).text();
                }
            });
            $(".gridcolumns").val(gc);
            $(".gridcolumnlabels").val(gclabels);
        });
    });
    function bindGridColumns() {
        var html = "<table class='repeater' cellpadding='2' cellspacing='0'><tr class='repeater-header'>";
        var row = "<tr class='grid-set-row'>";
        rightList.find("option").each(function() {
            html += "<td>" + $(this).text() + "</td>";
            row += "<td style='border-left:1px solid #eeeeee;'>Data</td>";
        });
        html += "</tr>";
        row += "</tr>";
        html += row + "</table>";
        $(".grid").html(html);
        $(".grid").find("table").colResizable();
    }
    function PreSave() {
        var gridColWidths = "";
        var totalWidth = 0;
        var currentWidth = 0;
        $(".grid-set-row td").each(function() {
            var w = $(this).css("width");
            w = w.replace("px", "");
            totalWidth += parseFloat(w);
        });
        $(".grid-set-row td").each(function() {
            var w = $(this).css("width");
            w = w.replace("px", "");
            w = parseInt(parseFloat(w) / parseFloat(totalWidth) * 100);
            currentWidth += w;
            if (gridColWidths == "") {
                gridColWidths = w;
            }
            else {
                gridColWidths += "," + w;
            }
        });
        if (currentWidth < 100) {
            var arr = gridColWidths.split(',');
            arr[0] = parseInt(arr[0]) + (100 - parseInt(currentWidth));
            gridColWidths = "";
            for (i = 0; i < arr.length; i++) {
                if (gridColWidths == "") {
                    gridColWidths = arr[i];
                }
                else {
                    gridColWidths += "," + arr[i];
                }
            }
        }
        $(".grid-column-width").val(gridColWidths);
    }
</script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="title">
            Grid Setting
        </td>
     </tr>
     <tr>
         <td>
            <table width="100%">
                <tr>
                    <td><table><tr>
                    <td>
                        <asp:ListBox SelectionMode="Multiple" ID="lstLeft" CssClass="leftlist" runat="server" Height="250" Width="300"></asp:ListBox>
                        <asp:TextBox ID="hdnGridColumns" runat="server" CssClass="gridcolumns hidden"/>
                        <asp:TextBox ID="hdnGridColumnLabels" runat="server" CssClass="gridcolumnlabels hidden"/>
                    </td>
                    <td style="vertical-align:top;padding:10px;">
                        <table cellspacing="10">
                            <tr><td><img src="../images/arrow-right.png" class="right-arr hand" title="Move Right"/></td></tr>
                            <tr><td><img src="../images/arrow-left.png" class="left-arr hand" title="Move Left"/></td></tr>
                        </table>
                    </td>
                    <td>
                        <asp:ListBox SelectionMode="Multiple" ID="lstRight" CssClass="rightlist" runat="server" Height="250" Width="300"></asp:ListBox>
                    </td>
                    <td style="vertical-align:top;padding:10px;">
                    <table cellpadding="10">
                        <tr><td><img src="../images/up-arrow-gr.png" class="up-arr hand" title="Move Up"/></td></tr>
                        <tr><td><img src="../images/down-arrow-gr.png" class="down-arr hand" title="Move Down"/></td></tr>
                    </table>
                </td>
                </tr></table></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table width="100%">
                            <tr><td style="font-weight:bold;">Set Default Grid Column Width
                            <asp:TextBox ID="h_GridColumnWidth" runat="server" CssClass="grid-column-width hidden"></asp:TextBox>
                            </td></tr>
                            <tr>
                                <td class="grid">
                                    <asp:Literal ID="ltGrid" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr><td class="title">Advanced Settings</td></tr>
                            <tr><td>
                                <table>
                                    <tr>
                                        <td class="label">Enable Paging?</td>
                                        <td><asp:CheckBox ID="chkEnablePaging" runat="server" Checked="true"/></td>
                                        <td class="label">Enable Sorting?</td>
                                        <td><asp:CheckBox ID="chkEnableSorting" runat="server" Checked="true"/>
                                        &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkEnableSearch" runat="server" Checked="true" Text="Enable Search"/></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Default Page Size</td>
                                        <td><asp:TextBox ID="txtPageSize" runat="server" CssClass="sbox" Text="20"></asp:TextBox></td>
                                        <td class="label">Top Records</td>
                                        <td><asp:TextBox ID="txtTopRecords" runat="server" CssClass="sbox val-i" Text="0"/></td>
                                    </tr>
                                    <tr>
                                        <td class="label">Default Sort</td>
                                        <td><asp:DropDownList ID="ddlSort" runat="server" CssClass="ddl"></asp:DropDownList> </td>
                                        <td class="label">Sort Direction</td>
                                        <td><asp:DropDownList ID="ddlSortDirection" runat="server" CssClass="ddl">
                                            <asp:ListItem Text="Ascending" Value="ASC"></asp:ListItem>
                                            <asp:ListItem Text="Descending" Value="DESC"></asp:ListItem>
                                        </asp:DropDownList> </td>
                                    </tr>
                                </table>
                            </td></tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btnSubmit" CssClass="button save" runat="server" OnClick="btnSubmit_Click" Text="Submit" OnClientClick="PreSave()"/>
                    </td>
                </tr>
            </table>
         </td>
     </tr>
     
</table>     

</asp:Content>

