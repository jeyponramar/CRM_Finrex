<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CPGridData.ascx.cs" Inherits="CPGridData" %>
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
                    <asp:LinkButton ID="lnkPrevPage" Text="Prev" ToolTip="Previous Page" runat="server" OnClick="lnkPrevPage_Click"></asp:LinkButton>
                </li>
                <asp:Repeater ID="rptPaging" runat="server">
                    <ItemTemplate>
                        <li id="page_td" runat="server"><asp:LinkButton ID="lnkPage" runat="server" OnCommand="lnkPage_OnCommand"></asp:LinkButton></li>
                    </ItemTemplate>
                </asp:Repeater>
                <li class="next-page"><asp:LinkButton ID="lnkNextPage" Text="Next" ToolTip="Next Page" runat="server" OnClick="lnkNextPage_Click"></asp:LinkButton></li>
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
                </tr></table></td>
                
            </tr>
           
            </table>
        </td>
        
        </tr></table></td>
        <td class="add-lnk"><asp:HyperLink ID="lnkAddPage" runat="server" CssClass="create-link"></asp:HyperLink></td>
    </tr>
    <tr><td colspan="2" id="tblData" runat="server">
        <table class="repeater" cellpadding="2" cellspacing="0">
        <tr class="repeater-header">
        <asp:Repeater ID="rptHead" runat="server" OnItemDataBound="rptHead_OnItemDataBound">
            <ItemTemplate>
            <td id="tdHeader" runat="server"><asp:LinkButton ID="lnkHead" runat="server" OnCommand="lnkHead_Command" CommandName="ColumnName"></asp:LinkButton><asp:Label ID="lblHead" runat="server"></asp:Label></td>
            </ItemTemplate>
        </asp:Repeater>
        </tr>
        <tr id="trNodata" runat="server" visible="false"><td class="error">No data found</td></tr>
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

 <script>
     $(document).ready(function() {
         var prevRow;
         var IsMultiSelect = '<%=IsMultiSelect %>';
         var reached = false;
         $(".repeater-row,.repeater-alt").click(function() {
             HiddenIds = "";
             if (IsMultiSelect == "True") {
                 $this = $(this);

                 reached = false;
                 $this.find("td").each(function() {
                     if ($(this).attr("class") == "hidden") {
                         if (HiddenIds == "") {
                             HiddenIds = $(this).text();
                         }
                         else {
                             HiddenIds = "," + $(this).text();
                         }
                     }
                     else {
                         if (!reached) {
                             reached = true;
                             SelectedId = $(this).text();
                         }
                     }
                 });
                 if ($this.attr("class") == "repeater-row") {
                     $this.removeClass("repeater-row").addClass("repeater-row-hover");
                 }
                 else if ($this.attr("class") == "repeater-alt") {
                     $this.removeClass("repeater-alt").addClass("repeater-alt-hover");
                 }
                 else if ($this.attr("class") == "repeater-alt-hover") {
                     $this.removeClass("repeater-alt-hover").addClass("repeater-alt");
                 }
                 else if ($this.attr("class") == "repeater-row-hover") {
                     $this.removeClass("repeater-row-hover").addClass("repeater-row");
                 }
             }
             else {
                 if (prevRow != null) {
                     if (prevRow.attr("class") == "repeater-row-hover") {
                         prevRow.removeClass("repeater-row-hover").addClass("repeater-row");
                     }
                     else {
                         prevRow.removeClass("repeater-alt-hover").addClass("repeater-alt");
                     }
                 }
                 $this = $(this); prevRow = $this;

                 //SelectedId = $this.find("td:first").text();
                 reached = false;
                 $this.find("td").each(function() {
                     if ($(this).attr("class") == "hidden") {
                         if (HiddenIds == "") {
                             HiddenIds = $(this).text();
                         }
                         else {
                             HiddenIds = "," + $(this).text();
                         }
                     }
                     else {
                         if (!reached) {
                             reached = true;
                             SelectedId = $(this).text();
                         }
                     }
                 });
                 
                 if ($this.attr("class") == "repeater-row") {
                     $this.removeClass("repeater-row").addClass("repeater-row-hover");
                 }
                 else {
                     $this.removeClass("repeater-alt").addClass("repeater-alt-hover");
                 }
             }
         });
         var currentKey = 0;
         var IsShiftKey = false;
         $(document).keydown(function(e) {
             if (e.keyCode == 16) IsShiftKey = true;
         });
         $(document).keyup(function(e) {
             IsShiftKey = false;
         });
     });
  </script>