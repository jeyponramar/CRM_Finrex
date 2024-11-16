<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="managesubmenusequence.aspx.cs" Inherits="CP_managesubmenu" Title="Manage Sub Menu Sequence" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    var leftList;
    var customsubmenus;
    $(document).ready(function() {
        customsubmenus = $(".customsubmenus");
       
        $(".up-arr").click(function() {
            var selectedRow;
            var prevRow;
            customsubmenus.find("option").each(function() {
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
            //bindGridColumns();
        });
         $(".save").click(function() {
            var gc = "";
            var gclabels = "";
            if (parseInt(customsubmenus.find("option").length) < 1) {
                alert("Grid should have atleast one column");
                return false;
            }
            customsubmenus.find("option").each(function() {
                if (gc == "") {
                    gc = $(this).attr("value");
                }
                else {
                    gc = gc + "," + $(this).attr("value");
                }
            });
             $(".gridcolumns").val(gc);
          });
        $(".down-arr").click(function() {
            var selectedRow;
            var prevRow;
            customsubmenus.find("option").each(function() {
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
           // bindGridColumns();
        });
        
    });
    
</script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="title">
           Sub Menu Setting
        </td>
     </tr>
     <tr>
         <td>
            <table width="100%">
                <tr>
                <td colspan="2" align="center">
                <asp:Label ID="lblmessage" runat="server" Visible="false" CssClass="error"></asp:Label>
                </td>
                </tr>
                <tr>                    
                    <td width="100">Main Menu</td>
                    <td><asp:DropDownList ID="ddlMenuId" runat="server" CssClass="menuid" AutoPostBack="true" OnSelectedIndexChanged="ddlMenuId_Changed"></asp:DropDownList></td>                   
                </tr>
                <tr>
                    <td><table><tr>
                    <td>
                        <asp:ListBox SelectionMode="Multiple" ID="lstSubmenus" CssClass="customsubmenus" runat="server" Height="250" Width="300"></asp:ListBox>                       
                        <asp:TextBox ID="hdnGridColumns" runat="server" CssClass="gridcolumns hidden"/>
                        <asp:TextBox ID="hdnMenuId" runat="server" CssClass="hidden"/>
                    </td>                    
                    <td>
                        
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
                    <td colspan="4" align="center">
                        <asp:Button ID="btnSubmit" CssClass="button save" runat="server" OnClick="btnSubmit_Click" Text="Submit" OnClientClick="PreSave()"/>
                    </td>
                </tr>
            </table>
         </td>
     </tr>
     
</table>   
</asp:Content>

