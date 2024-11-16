<%@ Page Title="Create Report" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="create-report.aspx.cs" Inherits="create_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
    $(document).ready(function() {
        $(".bulkreportgenerator").click(function() {
            var ModuleId = ConvertToInt($('#<%=ddlModule.ClientID %> option:selected').val());
            var menuId = ConvertToInt($('#<%=ddlMenu.ClientID %> option:selected').val());
            if (ModuleId > 0 && menuId > 0) {
            }
            else {
                alert("Please select module and Menu");
                return false;
            }
        });
        //Bulk Report
        $(".Bulkreport_Generation").click(function() {
            if (confirm("Are you sure you want to generate Bulk Report")) {
                var _Report = "";
                $(".chk_ReportName").each(function() {
                    if ($(this).is(':checked')) {
                        var ReportId = $(this).attr("ReportId");
                        _Report += (_Report == "") ? ReportId : "," + ReportId;
                    }
                });
                $(".BulkreportId").val(_Report);
                if (_Report == "") {
                    alert("Please select any one report");
                    return false;
                }
            }
            else {
                return false;
            }
        });
        //
    });
</script>
    <table width="100%">
    <tr><td><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td>
        <td align="right"><asp:Button CssClass="button redbutton delete" runat="server" ID="btnDelete" OnClick="btnDelete_Click" Text="Delete" Visible="false"/></td>
    </tr>
    <tr><td align="center" valign="top" colspan="2">
        <table width="100%">
            <tr><td align="center" valign="top"><asp:TextBox ID="h_BulkreportId" runat="server" CssClass="hidden BulkreportId" Text=""></asp:TextBox>
                <table align="center" cellspacing="10">
                    <tr>
                        <td>Select Report</td>
                        <td>
                            <asp:DropDownList id="ddlReport" runat="server" CssClass="ddl" 
                            OnSelectedIndexChanged="ddlReport_Changed" AutoPostBack="true"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="hidden"><td style="font-size:20px;text-align:center" colspan="2">OR</td></tr>
                    <tr>
                        <td>Report Name</td>
                        <td><asp:TextBox id="txtReportName" runat="server" CssClass="textbox"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Module</td>
                        <td><asp:DropDownList ID="ddlModule" runat="server"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>Under</td>
                        <td><asp:DropDownList ID="ddlMenu" runat="server"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>Apply View Rights</td>
                        <td>
                            <asp:CheckBox ID="chkapplyviewrights" runat="server" />
                        </td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>
                    
                    <tr><td>&nbsp;</td><td><asp:Button CssClass="button" runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text="Save"/></td></tr>
                    <tr><td></td><td><asp:HyperLink ID="lnkConfigReport" runat="server" Text="Config Report" Visible="false" Target="_blank"></asp:HyperLink></td></tr>
                </table>
            </td>
            <td valign="top">
                <table>
                    <tr>
                        <td></td>
                        <td><asp:CheckBox ID="chkIsBindOnLoad" runat="server" Text="Is Bind On LOAD?" /></td>
                    </tr>
                    <tr>
                        <td>Grid Title</td>
                        <td><asp:TextBox ID="txtGridTitle" runat="server" Width="400"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Chart Type</td>
                        <td><asp:DropDownList ID="ddlChartType" runat="server">
                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Bar Chart" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Pie Chart" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Line" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Area" Value="4"></asp:ListItem>
                        </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:CheckBox ID="chkIsDisplayChartBelowGrid" runat="server" Text="Is Display Chart Below Grid?" /></td>
                    </tr>
                    <tr>
                        <td>Chart Header Text</td>
                        <td><asp:TextBox ID="txtChartHeaderColumns" runat="server" Width="400"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Chart Columns</td>
                        <td><asp:TextBox ID="txtChartColumns" runat="server" Width="400"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Chart Colors</td>
                        <td><asp:TextBox ID="txtChartColors" runat="server" Width="400"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Chart Extra Attributes</td>
                        <td><asp:TextBox ID="txtChartAttributes" runat="server" Width="400"></asp:TextBox></td>
                    </tr>
                    <%--<tr><td>&nbsp;</td><td><asp:Button CssClass="button" runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Save"/></td></tr>--%>
                </table>
            </td>
            </tr>
        </table>
        
    </td></tr>
    <tr>
     <td align="center">
            <table width="900">
                <tr>
                    <td>
                        <table class="repeater" border="1" cellspacing="0">
                            <tr class="repeater-header">
                                <td><b>Select Report</b></td>
                                <td>Report Name</td>
                                <td>Custom Report Name</td>
                                <td>Edit</td>
                            </tr>
                            <asp:Literal runat="server" Visible="false" ID="ltSuggestedReport"></asp:Literal>
                        </table>
                    </td>
                </tr>               
            </table>                
        </td>
    </tr>
    <tr>
        <td>
            <asp:Literal runat="server" Visible="false" ID="ltAlreadyExistsReportNames"></asp:Literal>
        </td>
    </tr>
     <tr>
         <td><asp:Button CssClass="button bulkreportgenerator" Width="150" runat="server" ID="btnBulkReportGenerater" OnClick="btnBulkReportGenerater_Click" Text="Get Bulk Report's"/></td>
        <td>
            <asp:Button runat="server" Visible="false" ID="btngenerateBulkReport" OnClick="btngenerateBulkReport_Click" Text="Generate Bulk Report" CssClass="button Bulkreport_Generation" />
        </td>
        
    </tr>
</table>
</asp:Content>

