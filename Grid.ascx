<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Grid.ascx.cs" Inherits="Grid" %>
<table cellpadding="0" cellspacing="0" width="100%" id="tblMobile" runat="server" visible="false" class="tblgrid-m">
    <tr><td style="border-bottom:solid 1px #cfcfcf;background-color:#f5f5f5;">
        <table width="100%">
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td width="150"><asp:DropDownList ID="ddlSearchBy_m" runat="server" Width="150" CssClass="searchby ddl hidden"></asp:DropDownList></td>
                            <td style="padding-left:5px;"><asp:TextBox CssClass="hidden txtkeyword_m textbox" ID="txtKeyword_m" runat="server" Width="100%"></asp:TextBox></td>            
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
                <td align="right">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>&nbsp;</td>
                            <td class="tdbtnsearch"><asp:ImageButton ID="btnSearch_m" runat="server" OnClick="btnSearch_Click" Text="Search" ImageUrl="~/images/action-buttons/search.png" Width="25" Height="25"/></td>
                            <td class="add-lnk" align="right">&nbsp;</td>
                        </tr>
                    </table>
                </td>
                
            </tr>    
        </table>
    </td></tr>
    <tr><td><asp:Literal ID="ltMobile" runat="server"></asp:Literal></td></tr>
</table>

<table cellpadding="0" cellspacing="0" width="100%" id="tblNonMobile" runat="server" visible="false">
<tr><td class="view-detail boxborder" style="padding:10px"><table width="100%" cellpadding="0" cellspacing="0">
    <tr><td><asp:Label ID="lblError" runat="server" CssClass="error" Text="No data found" Visible="false"></asp:Label></td></tr>
    <tr><td valign="top">
    <table width="100%" cellpadding="2" cellspacing="0" class="tblgridmain">
    <tr>
        <td id="trPaging" runat="server" class="pagingbg">
        <asp:TextBox ID="h_as_keyword" runat="server" CssClass="h_as_keyword hidden"></asp:TextBox>
        <table width="100%" cellpadding="0" cellspacing="0"><tr>
        <td><asp:Label ID="lblTotalRecords" runat="server"></asp:Label></td>
        <td>
            <table width="100%" cellpadding="0" cellspacing="0" class="tblPaging">
                <tr>
                <td>
                    <table cellpadding="0" cellspacing="3" id="tblPaging" runat="server">
                    <tr>
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
                                <asp:ListItem Text="20" Value="20" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                <asp:ListItem Text="80" Value="80"></asp:ListItem>
                                <asp:ListItem Text="All" Value="All"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSearchBy" runat="server" Width="150" CssClass="searchby"></asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="txtKeyword" runat="server" CssClass="mbox" enableenter="true"></asp:TextBox></td>
                        <td><asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" ImageUrl="~/images/action-buttons/search.png" ToolTip="" Width="25" Height="25"/></td>
                        <%--<td><img width="20" src="../images/action-buttons/advanced-search-filter.png" class="advanced-search-filter hand" title="Advanced Search"/></td>--%>
                        <td><asp:Image  ImageUrl="~/images/action-buttons/grid-setting.png" class='grid-action grid-setting hand' runat="server" id="imgGridSetting"/></td>
                        </tr>
                        
                    </table>
                </td>
                <td>&nbsp;</td>
                <%--<td><asp:Image ImageUrl="~/images/search.png" CssClass="imgadvsearch" runat="server" ID="imgAdvSearch"/></td>--%>
                <td width="30" align="right"><asp:ImageButton CssClass="grid-action1 btnexport" ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Export" ImageUrl="~/images/excel.png" Width="25" Height="25" Visible="false"/></td>
                <td width="30" align="right"><asp:Image ID="imgCreateNew" ImageUrl="~/images/createnew.png" runat="server" CssClass="grid-action1 hand page" ToolTip="Create New"/></td>
                <td width="30" align="right"><asp:ImageButton CssClass="grid-action1" ID="imgRefresh" runat="server" OnClick="imgRefresh_Click" ImageUrl="~/images/refresh.png" ToolTip="Refresh this grid"/></td>
                
            </tr>
            
            </table>
        </td>
        
        </tr></table></td>
        <%--<td class="add-lnk hidden"><asp:HyperLink ID="lnkAddPage" runat="server" CssClass="create-link"></asp:HyperLink></td>--%>
    </tr>
   <%-- <tr class="tradvancedsearch hidden">
        <td>
            <table width="100%">
                <tr><td class="advsearch-controls"></td></tr>
                <tr><td align="center"><asp:Button ID="btnAdvancedSearch" runat="server" Text="Advanced Search" CssClass="button btnadvanced-search" OnClick="btnAdvancedSearch_Click" /></td></tr>
            </table>
        </td>
    </tr>--%>
    <tr><td id="tblData" runat="server" class="gridtd">
        <table class="repeater" cellpadding="2" cellspacing="0">
        <tr class="repeater-header">
        <td width="20" id="tdEdit" runat="server" style="width:20px !important">Edit</td>
        <asp:Repeater ID="rptHead" runat="server" OnItemDataBound="rptHead_OnItemDataBound">
            <ItemTemplate>
            <td id="tdHeader" runat="server"><asp:LinkButton ID="lnkHead" runat="server" OnCommand="lnkHead_Command" CommandName="ColumnName"></asp:LinkButton><asp:Label ID="lblHead" runat="server"></asp:Label></td>
            </ItemTemplate>
        </asp:Repeater>
        <td id="tdFEMheader" runat="server" visible="false" width="40">FEM</td>
        </tr>
        <tr id="trNodata" runat="server" visible="false"><td class="error" colspan="20">No data found</td></tr>
        <asp:Literal ID="ltGrid" runat="server"></asp:Literal>
      
        </table>
    </td></tr>
    </table></td></tr>
    
</table>
</td></tr></table>
