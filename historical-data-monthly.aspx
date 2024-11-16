<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="historical-data-monthly.aspx.cs" 
Inherits="historical_data_monthly" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function(){
        $(".jq-daily-history-chart").attr("labels",$(".jq-hdndates").val());
        $(".jq-daily-history-chart").attr("data",$(".jq-hdnrates").val());
        $(".jq-daily-history-chart").addClass("db-chartjs-panel");
        initChartJs();
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr><td class='page-inner2'>
        <table width='100%'>
        <tr><td class='page-title2'><asp:Label ID="lbltitle" runat="server" Text="MONTHLY PERFORMANCE"></asp:Label></td></tr>
        <tr><td><asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label>
        <asp:TextBox ID="hdndates" runat="server" CssClass="jq-hdndates hidden"></asp:TextBox>
        <asp:TextBox ID="hdnrates" runat="server" CssClass="jq-hdnrates hidden"></asp:TextBox>
        </td></tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td valign="top" width="40%">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <table cellspacing=10>
                                            <tr>
                                                <td width="100" class="bold">Currency <span class="error">*</span> : </td>
                                                <td colspan="2"><asp:DropDownList ID="ddlcurrency" runat="server" style="width:120px;height:25px;">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="USDINR" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="EURINR" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="GBPINR" Value="4"></asp:ListItem>
                                                    <%--<asp:ListItem Text="JPYINR" Value="5"></asp:ListItem>--%>
                                                </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td class="bold">Month <span class="error">*</span> : </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlmonth" runat="server" CssClass="ddl" Width="120"></asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlyear" runat="server" CssClass="ddl" Width="120"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr><td></td><td><asp:Button ID="btnSubmit" CssClass="button-ui" runat="server" Text="Search" OnClick="btnSubmit_Click" /></td></tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Literal ID="ltdata" runat="server"></asp:Literal></td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td valign="top" id="tdchart" runat="server">
                                        <div class='jq-daily-history-chart' ct='3' xaxislabel='Open,High,Low,Close' data='' labels='' pointradius='3' width='300' height="100"></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Literal ID="ltsummary" runat="server"></asp:Literal></td>
                                </tr>
                            </table>
                        </td>
                        
                    </tr>
                </table>
            </td>
        </tr>
        
        <tr><td>&nbsp;</td></tr>
        
        </table>
       </td>
    </tr>
    
 </table>
</asp:Content>

