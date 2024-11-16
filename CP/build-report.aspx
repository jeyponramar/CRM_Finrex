<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="build-report.aspx.cs" Inherits="build_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
       
        $('.constant').live("change", function() {
            var tr = $(this).parent().parent();
            if ($(this).val() == '') {
                $(".value").val("");
            }
            if ($(this).val() == 'Today' || $(this).val() == 'ThisWeek' || $(this).val() == 'ThisMonth' || $(this).val() == 'ThisYear') {
                $(".val").val("");
                $(".from").val("$" + $(this).val().replace(/ /, "") + "From$");
                $(".to").val("$" + $(this).val().replace(/ /, "") + "To$");
            }
            else {
                $(".value").val("$" + $(this).val() + "$");
            }
            buildQuery();
        });
        $(".control").live("change", function() {
            var column = $(".column").find("option:selected").text();
            if ($(this).val() == "Date Range") {
                $(".from").val("$" + column + "_from$");
                $(".to").val("$" + column + "_to$");
                $(".operator").val("Between");
            }
            else if ($(this).val() != "Default") {
                //for user input
                $(".value").val("$" + column + "$");
            }
            buildQuery();
        });
        $(".operator").live("change", function() {
            buildQuery();
        });
        function buildQuery() {
            var ddlcontrol = $(".control").val();
            var ddloperator = $(".operator").val();
            var txtcolumnvalue = $(".value").val();
            var txtBetweenFrom = $(".from").val();
            var txtBetweenTo = $(".to").val();
            var AndOr = $(".andor").val() + " ";
            var colName = "";
            var operator = "";
            var value = "";
            var concatenated = false;
            var where = "";

            colName = $(".column").find("option:selected").text();
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
                where = "('$" + colName + "$' = '0' OR " + colName + " = '$" + colName + "$') ";
            }
            else if (ddlcontrol == "Autocomplete") {
                concatenated = true;
                where = "('$" + colName + "$' = '0' OR " + colName + " = '$" + colName + "$') ";
            }
            else {
                value = " '" + value + "' ";
            }

            if (!concatenated) where = colName + operator + value + AndOr;

            $(".where").val(where);
        }


    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
    <tr><td class="page-title"><asp:Label id="lblTitle" runat="server"></asp:Label>
    <asp:TextBox ID="h_rowIndex" runat="server" CssClass="hidden rowindex" Text="1"></asp:TextBox>
    
    </td></tr>
    <tr><td><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td></tr>
    <tr><td>
        <table>
        <tr>
            <td><table><tr>
            <td></td>
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
        <tr>
            <td>
                <table>
                    <tr>
                        <td>Column</td>
                        <td><asp:DropDownList ID="ddlColumns" Font-Size="14" runat="server" CssClass="column"></asp:DropDownList></td>
                        <td>Type</td>
                        <td>
                            <asp:DropDownList ID="ddlControlType" runat="server" Width="100" CssClass="control">
                                 <asp:ListItem Value="" Selected="True">Select</asp:ListItem>
                                 <asp:ListItem Value="Text Box">Text Box</asp:ListItem>
                                 <asp:ListItem Value="Date Range">Date Range</asp:ListItem>
                                 <asp:ListItem Value="Dropdown">Dropdown</asp:ListItem>
                                 <asp:ListItem Value="Autocomplete">Autocomplete</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>Operator</td>
                        <td>
                            <asp:DropDownList ID="ddlOperator" runat="server" Width="100" CssClass="operator">
                                 <asp:ListItem Value="Equal to">Equal to</asp:ListItem>
                                 <asp:ListItem Value="NOT Equal to">NOT Equal to</asp:ListItem>
                                 <asp:ListItem Value="Greater than">Greater than</asp:ListItem>
                                 <asp:ListItem Value="Less than">Less than</asp:ListItem>
                                 <asp:ListItem Value="LIKE">LIKE</asp:ListItem>
                                 <asp:ListItem Value="Between">Between</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>Value</td>
                        <td><asp:TextBox ID="txtValue" runat="server" CssClass="value"></asp:TextBox></td>
                        <td>Variable</td>
                        <td>
                            <asp:DropDownList ID="ddlVariable" runat="server" Width="100" CssClass="constant">
                                 <asp:ListItem Value="LoggedInUser">Logged In User</asp:ListItem>
                                 <asp:ListItem Value="LoggedInRole">Logged In Role</asp:ListItem>
                                 <asp:ListItem Value="Today">Today</asp:ListItem>
                                 <asp:ListItem Value="ThisWeek">This Week</asp:ListItem>
                                 <asp:ListItem Value="ThisMonth">This Month</asp:ListItem>
                                 <asp:ListItem Value="This Year">This Year</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>FROM</td>
                        <td><asp:TextBox ID="txtFrom" runat="server" Width="100" CssClass="from"></asp:TextBox></td>
                        <td>TO</td>
                        <td><asp:TextBox ID="txtTo" runat="server" Width="100" CssClass="to"></asp:TextBox></td>
                        <td>JOIN</td>
                        <td>
                            <asp:DropDownList ID="ddlJOIN" runat="server" Width="50" CssClass="andor">
                                 <asp:ListItem Value="OR" Selected="True">OR</asp:ListItem>
                                 <asp:ListItem Value="AND">AND</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>CSS</td>
                        <td><asp:TextBox ID="txtCss" runat="server"></asp:TextBox></td>
                   </tr>
                   <tr>
                        <td colspan="10">
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtWhereCondition" runat="server" Width="400" CssClass="where"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAddWhere" runat="server" Text="Save WHERE" OnClick="btnAddWhere_Click" CssClass="button"/>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnRemoveWhere" Visible="false" runat="server" Text="Remove WHERE" OnClick="btnRemoveWhere_Click" CssClass="button"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="ltControls" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            
        </tr>
        <tr><td>
            <table width="400">
                <tr>
                    <td>
                        <table class="repeater" border="1" cellspacing="0">
                            <tr class="repeater-header">
                                <td>Column</td>
                                <td>Control</td>
                                <td>Action</td>
                            </tr>
                            <asp:Literal ID="ltQueryBuilder" runat="server"></asp:Literal>
                        </table>
                    </td>
                </tr>
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
                                    <td><asp:TextBox TextMode="MultiLine" Width="600" Height="250" runat="server" ID="txtJoinTables" CssClass="jointables"></asp:TextBox></td>
                                </tr>
                            </table>
                        </td>
                        
                        <td>Filter Criteria</td><td><asp:TextBox TextMode="MultiLine" Width="600" Height="250" runat="server" ID="txtWhere" CssClass="wherefinal"></asp:TextBox></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr><td align="center"><asp:Button ID="btnSave" runat="server" CssClass="button" OnClick="btnSave_Click" Text="Save"/><asp:Button Width="250" ID="btnGotoViewPageSetting" runat="server" CssClass="button" OnClick="btnGotoViewPageSetting_Click" Text="Go to Add Remove View Page Columns Settings"/></td></tr>
        </table>
    </td></tr>
    
</table>
</asp:Content>

