<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="buildreport.aspx.cs" Inherits="addreport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        $(".column").change(function() {
            if ($(this).val() == "0") return;
            var tr;
            var column = $(this).find("option:selected").text();
            var rowIndex = parseInt($(".rowindex").val());
            $(".rowindex").val(rowIndex + 1);
            var prefix = "@sm_";
            tr = "<tr class='where-row'><td class='label rowcol'>" +
                    "<input type='text' value='" + column + "' name='" + prefix + "columnname-" + rowIndex +
                    "' class='hidden colname'/>" + column + "</td>";
            tr += "<td><select name='" + prefix + "control-" + rowIndex + "' style='width:100px' class='control'>" +
                   "<option value='Default'>Default</option>" +
                   "<option value='Text Box'>Text Box</option>" +
                   "<option value='Date Range'>Date Range</option>" +
                   "<option value='Dropdown'>Dropdown</option>" +
                   "<option value='Autocomplete'>Autocomplete</option></select></td>";
            tr += "<td><select name='" + prefix + "operator-" + rowIndex + "' style='width:100px' class='operator'>" +
                      "<option value='Equal to'>Equal to</option>" +
                      "<option value='NOT Equal to'>NOT Equal to</option>" +
                      "<option value='Greater than'>Greater than</option>" +
                      "<option value='Less than'>Less than</option>" +
                      "<option value='LIKE'>LIKE</option>" +
                      "<option value='Between'>Between</option>" +
                  "</select></td>";
            tr += "<td><input type='text' value='' name='" + prefix + "columnvalue-" + rowIndex + "' style='width:100px' class='value'/></td>";
            tr += "<td><select name='" + prefix + "constant-" + rowIndex + "' style='width:120px' class='constant'>" +
                        "<option value=''>Select</option>" +
                        "<option value='LoggedInUser'>Logged In User</option>" +
                        "<option value='LoggedInRole'>Logged In Role</option>" +
                        "<option value='Today'>Today</option>" +
                        "<option value='ThisWeek'>This Week</option>" +
                        "<option value='ThisMonth'>This Month</option>" +
                        "<option value='ThisYear'>This Year</option>" +
                    "</select></td>";
            tr += "<td><input type='text' name='" + prefix + "BetweenFrom-" + rowIndex + "' style='width:100px' class='from'></td>";
            tr += "<td><input type='text' name='" + prefix + "BetweenTo-" + rowIndex + "' style='width:100px' class='to'></td>";
            tr += "<td><select name='" + prefix + "AndOr-" + rowIndex + "' class='andor'><option value='OR'>OR</option><option value='AND'>AND</option></select>";
            tr += "<td><img src='img/close-red.png' class='delete hand'/></td>";
            tr += "</tr>";
            $(".submodule").append($(tr));
            buildQuery();
        });
        $(".delete").live("click", function() {
            var tr = $(this).parent().parent();
            var hdnid = tr.find("input:first");
            if (hdnid.val() == "") {
                tr.find("input").removeAttr("name");
                tr.remove();
            }
            else {
                hdnid.val("-del" + hdnid.val());
                tr.find(".input").val("");
                tr.hide();
            }
            buildQuery();
        });
        $(".where-row select,.where-row input").live("change", function() {
            buildQuery();
        });
        $('.constant').live("change", function() {
            var tr = $(this).parent().parent();
            if ($(this).val() == '') {
                tr.find(".value").val("");
            }
            if ($(this).val() == 'Today' || $(this).val() == 'ThisWeek' || $(this).val() == 'ThisMonth' || $(this).val() == 'ThisYear') {
                tr.find(".val").val("");
                tr.find(".from").val("$" + $(this).val().replace(/ /, "") + "From$");
                tr.find(".to").val("$" + $(this).val().replace(/ /, "") + "To$");
            }
            else {
                tr.find(".value").val("$" + $(this).val() + "$");
            }
            buildQuery();
        });
        $(".control").live("change", function() {
            if ($(this).val() == "Date Range") {
                var tr = $(this).parent().parent();
                var column = tr.find(".rowcol").text();
                tr.find(".from").val("$" + column + "_from$");
                tr.find(".to").val("$" + column + "_to$");
                $(this).find(".operator").val("Between");
            }
            else if ($(this).val() != "Default") {
                //for user input
                var tr = $(this).parent().parent();
                var column = tr.find(".rowcol").text();
                tr.find(".value").val("$" + column + "$");
            }
            buildQuery();
        });
        function buildQuery() {
            var where = "";
            $(".where-row").each(function() {
                if ($(this).css("display") != "none") {
                    var ddlcontrol = $(this).find(".control").val();
                    var ddloperator = $(this).find(".operator").val();
                    var txtcolumnvalue = $(this).find(".value").val();
                    var txtBetweenFrom = $(this).find(".from").val();
                    var txtBetweenTo = $(this).find(".to").val();
                    var AndOr = $(this).find(".andor").val() + " ";
                    var colName = "";
                    var operator = "";
                    var value = "";
                    var concatenated = false;

                    colName = $(this).find(".rowcol").text();
                    if (ddloperator == "Between") {
                        //default
                        value = txtBetweenFrom + "' AND '" + txtBetweenTo;
                    }
                    else {
                        //where condition by user input
                        value = txtcolumnvalue;
                    }
                    if (ddloperator == "Equal to") {
                        operator = " = ";
                    }
                    else if (ddloperator == "NOT Equal to") {
                        operator = " <> ";
                    }
                    else if (ddloperator == "Greater than") {
                        operator = " > ";
                    }
                    else if (ddloperator == "Less than") {
                        operator = " < ";
                    }
                    else if (ddloperator == "LIKE") {
                        operator = " LIKE '%" + value + "%'";
                        value = "";
                    }
                    else if (ddloperator == "Between") {
                        operator = " BETWEEN ";
                    }

                    if (ddlcontrol == "Date Range") {
                        operator = "";
                        value = " BETWEEN '" + txtBetweenFrom + "' AND '" + txtBetweenTo + "' ";
                    }
                    else if (ddlcontrol == "Dropdown") {
                        concatenated = true;
                        where = where + "\n ('$" + colName + "$' = '0' OR " + colName + " = '$" + colName + "$') ";
                    }
                    else if (ddlcontrol == "Autocomplete") {
                        concatenated = true;
                        where = where + "\n ('$" + colName + "$' = '0' OR " + colName + " = '$" + colName + "$') ";
                    }
                    else {
                        value = " '" + value + "' ";
                    }

                    var br = "";
                    if (concatenated) {
                        where += AndOr;
                    }
                    else {
                        if (where != "") br = "\n";
                        where = where + br + colName + operator + value + AndOr;
                    }
                }
            });
            $(".where").val(where);
        }
        $(".savequery").click(function() {
            saveMultiData($(this));
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
    <tr><td class="page-title"><asp:Label id="lblTitle" runat="server"></asp:Label>
    <asp:TextBox ID="h_rowIndex" runat="server" CssClass="hidden rowindex" Text="1"></asp:TextBox>
    </td></tr>
    <tr><td>
        <table>
        <tr>
            <td><table><tr>
            <td>Add Column Filter</td>
            <td><asp:DropDownList ID="ddlColumns" Font-Size="14" runat="server" CssClass="column"></asp:DropDownList></td>
            <td id="tdResetGridSetting" runat="server" visible="false">
                <table>
                    <tr>
                        <td><asp:CheckBox ID="chkResetGridSetting" runat="server" Text="Reset All Grid Settings"/></td>
                        <td><asp:CheckBox ID="chkResetColumns" Text="Reset Columns" runat="server" /></td>
                    </tr>
                </table>
            </td>
            </tr></table></td>
        </tr>
        <tr><td>
            <table class="submodule" border="1" cellspacing="0">
                <tr>
                    <td>Column</td>
                    <td>Type</td>
                    <td>Operator</td>
                    <td>Value</td>
                    <td>Variable</td>
                    <td>From</td>
                    <td>To</td>
                    <td>Join</td>
                    <td>Remove</td>
                </tr>
                <asp:Literal ID="ltQueryBuilder" runat="server"></asp:Literal>
            </table>
        </td></tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td id="tdJoinTables" runat="server">
                            <table>
                                <tr>
                                    <td>Join Tables</td>
                                    <td><asp:TextBox TextMode="MultiLine" Width="350" Height="300" runat="server" ID="txtJoinTables" CssClass="jointables"></asp:TextBox></td>
                                </tr>
                            </table>
                        </td>
                        
                        <td>Filter Criteria</td><td><asp:TextBox TextMode="MultiLine" Width="800" Height="120" runat="server" ID="txtWhere" CssClass="where"></asp:TextBox></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr><td align="center"><asp:Button ID="btnSave" runat="server" CssClass="button" OnClick="btnSave_Click" Text="Save"/><asp:Button Width="250" ID="btnGotoViewPageSetting" runat="server" CssClass="button" OnClick="btnGotoViewPageSetting_Click" Text="Go to Add Remove View Page Columns Settings"/></td></tr>
        </table>
    </td></tr>
</table>
</asp:Content>

