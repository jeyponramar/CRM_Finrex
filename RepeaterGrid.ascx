<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RepeaterGrid.ascx.cs" Inherits="RepeaterGrid" %>
<table cellpadding="0" cellspacing="0" width="100%">
<tr><td  class="view-detail"><table width="100%">
    <tr><td><asp:Label ID="lblError" runat="server" CssClass="error" Text="No data found" Visible="false"></asp:Label></td></tr>
    
    <tr><td><table width="100%" cellpadding="3" cellspacing="0">
    <tr>
        <td id="trPaging" runat="server"><table width="100%"><tr>
        <td><asp:Label ID="lblTotalRecords" runat="server"></asp:Label></td>
        <td>
            <table width="100%" id="tblPaging" runat="server" cellpadding="0" cellspacing="0">
                <tr>
                <td><table cellpadding="2" cellspacing="0"><tr>
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
                    <asp:DropDownList ID="ddlPageSize" runat="server" Width="52" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_Changed">
                        <asp:ListItem Text="20" Value="20" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="40" Value="40"></asp:ListItem>
                        <asp:ListItem Text="80" Value="80"></asp:ListItem>
                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSearchBy" runat="server" Width="100">
                    </asp:DropDownList>
                </td>
                <td><asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox></td>
                <td><asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" CssClass="button"/></td>
                <%--<td><input type="button" id="advsearch" value="Advanced Search" /></td>--%>
                <td><asp:Image ImageUrl="~/images/grid-setting.png" class='grid-setting hand' runat="server" id="imgGridSetting"/></td>
                </tr></table></td>
                
            </tr>
           
            </table>
        </td>
        
        </tr></table></td>
        <td class="add-lnk"><asp:HyperLink ID="lnkAddPage" runat="server" CssClass="create-link"></asp:HyperLink></td>
    </tr>
    <tr><td colspan="2" id="tblData" runat="server" class="gridtd">
        <table class="repeater" cellpadding="2" cellspacing="0">
        <tr class="repeater-header">
        <asp:Repeater ID="rptHead" runat="server" OnItemDataBound="rptHead_OnItemDataBound">
            <ItemTemplate>
            <td id="tdHeader" runat="server"><asp:LinkButton ID="lnkHead" runat="server" OnCommand="lnkHead_Command" CommandName="ColumnName"></asp:LinkButton><asp:Label ID="lblHead" runat="server"></asp:Label></td>
            </ItemTemplate>
        </asp:Repeater>
        </tr>
        <tr id="trNodata" runat="server" visible="false"><td class="error">No data found</td></tr>
        <tr>
            <td>
                <table width="100%">
                    <asp:Literal ID="ltGrid" runat="server"></asp:Literal>
                </table>
            </td>
        </tr>
        
        <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_OnItemDataBound">
            <ItemTemplate>
                <asp:Literal ID="ltItem" runat="server"></asp:Literal>
            </ItemTemplate>
        </asp:Repeater>
        </table>
    </td></tr>
    </table></td></tr>
    
</table>
</td></tr></table>

