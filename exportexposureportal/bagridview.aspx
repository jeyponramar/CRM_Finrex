<%@ Page Title="" Language="C#" MasterPageFile="~/exportexposureportal/ExportPortalMasterPage.master" AutoEventWireup="true" CodeFile="bagridview.aspx.cs" Inherits="exportexposureportal_bagridview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="js/fem-bagrid.js?v=<%=VersionNo %>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="fade"></div>
    <div class="detailmodal">
        <table width="100%">
            <tr><td class="detailmodal-header">
                <table width="100%">
                    <tr><td class="detailmodal-title"></td><td align="right"><div class="detailmodal-close">x</div></td></tr>
                </table>
            </td></tr>
            <tr><td><iframe id="ifrdetail" style="border:0px;"></iframe></td></tr>
        </table>
    </div>
    <div id="dtcommon" style="display:none;position:absolute;"></div>
    <div style="width:500px;height:350px;overflow:scroll;" id="gridpanel">
    <table width="100%">
        <tr id="tradvsearch" class="tradvsearch" runat="server">
            <td>
                <table>
                    <tr>
                        <td>Outstanding : <asp:DropDownList ID="ddloutstanding" runat="server" Width="100">
                            <asp:ListItem Text="Outstanding" Value="0"></asp:ListItem>
                            <asp:ListItem Text="All" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="txtwhere" runat="server" CssClass="txtwhere hidden"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddldatewise" runat="server" Width="120">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtfromdatewise" runat="server" CssClass="datepicker"></asp:TextBox>
                            <asp:TextBox ID="txttodatewise" runat="server" CssClass="datepicker"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlamountwise" runat="server" Width="120">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtamountwise_from" runat="server" CssClass="val-dbl" Width="50"></asp:TextBox>
                            <asp:TextBox ID="txtamountwise_to" runat="server" CssClass="val-dbl" Width="50"></asp:TextBox>
                        </td>
                   
                        <td>
                            Keyword : <asp:TextBox ID="txtkeyword" runat="server" Width="100"></asp:TextBox>
                        </td>
                        <td id="tdMoreSearch" runat="server" visible="false"><a href="#" class="jq-lnkmore">More</a></td>
                        <td><asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnsearch_click" style="background-color: #eee;padding: 10px;height: 30px;line-height: 10px;"/></td>
                        <td><asp:Button ID="btnExport" processing="false" runat="server" Text="Export" OnClick="btnexport_click" style="background-color: #eee;padding: 10px;height: 30px;line-height: 10px;" CssClass="noprocessing"/></td>
                    </tr>
                    <tr id="trmoresearch" class="hidden">
                        <td colspan="20">
                            <table id="tblSearch" runat="server">
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td><table><tr>
            <%--<td style='color:#035081;height:20px;cursor:pointer;' class="ba-btnaddnew">Add New</td>--%>
            <td><input type="button" class="ba-btnaddnew" style="background-color: #eee;padding: 10px;height: 30px;line-height: 10px;" value="Add New"/></td>
            <td style="padding-left:20px;"><input type="button" id="btnbagrid_save" style="background-color: #eee;padding: 10px;height: 30px;line-height: 10px;" value="Save"/></td>
            <%--<td style="position:relative;">
                <div style="position: fixed;right: 20px;margin-top:-12px;">
                    <input type="button" id="btnbagrid_save" style="background-color: #eee;padding: 10px;height: 30px;line-height: 10px;" value="Save"/></div>
            </td>--%>
            <td id="tdPaging_top" runat="server"><table><tr>
            <td><asp:Label ID="lblTotalRecords_top" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
            <td>
                <ul class="paging">
                <li class="prev-page">
                    <asp:LinkButton ID="lnkPrevPage_top" Text="<" ToolTip="Previous Page" runat="server" OnClick="lnkPrevPage_Click"></asp:LinkButton>
                </li>
                <asp:Repeater ID="rptPaging_top" runat="server">
                    <ItemTemplate>
                        <li id="page_td" runat="server"><asp:LinkButton ID="lnkPage" runat="server" OnCommand="lnkPage_OnCommand"></asp:LinkButton></li>
                    </ItemTemplate>
                </asp:Repeater>
                <li class="next-page"><asp:LinkButton ID="lnkNextPage_top" Text=">" ToolTip="Next Page" runat="server" OnClick="lnkNextPage_Click"></asp:LinkButton></li>
                </ul>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPageSize_top" runat="server" Width="45" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_Changed">
                        <asp:ListItem Text="5" Value="5" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                        <asp:ListItem Text="40" Value="40"></asp:ListItem>
                        <asp:ListItem Text="80" Value="80"></asp:ListItem>
                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                    </asp:DropDownList>
                </td>
             </tr></table></td>
            </tr></table></td>
        </tr>
        <tr><td><table width="100%"><tr><td><asp:Literal id="ltbagrid" runat="server"></asp:Literal></td></tr></table></td></tr>
        <tr id="trpaging" runat="server"><td style="padding-left:450px;">
            <table>
                <tr>    
                    <td><asp:Label ID="lblTotalRecords" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td>
                    <ul class="paging">
                    <li class="prev-page">
                        <asp:LinkButton ID="lnkPrevPage" Text="<" ToolTip="Previous Page" runat="server" OnClick="lnkPrevPage_Click"></asp:LinkButton>
                    </li>
                    <asp:Repeater ID="rptPaging" runat="server">
                        <ItemTemplate>
                            <li id="page_td" runat="server"><asp:LinkButton ID="lnkPage" runat="server" OnCommand="lnkPage_OnCommand"></asp:LinkButton></li>
                        </ItemTemplate>
                    </asp:Repeater>
                    <li class="next-page"><asp:LinkButton ID="lnkNextPage" Text=">" ToolTip="Next Page" runat="server" OnClick="lnkNextPage_Click"></asp:LinkButton></li>
                    </ul>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPageSize" runat="server" Width="45" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_Changed">
                            <asp:ListItem Text="5" Value="5" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="40" Value="40"></asp:ListItem>
                            <asp:ListItem Text="80" Value="80"></asp:ListItem>
                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </td></tr>
    </table>
    
    </div>
</asp:Content>

