<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="Bulk_Menu_SubMenu_Delete.aspx.cs" Inherits="CP_Bulk_Menu_SubMenu_Delete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script>
    $(document).ready(function() {
        $(".deleteallmenusubmneu").click(function() {
            var Meeage = "Menu / Sub Menu";
            if (confirm("Are you sure you wnat to delete " + Meeage)) {
                var _MenuSubMenuId = "";
                $(".chk_menusubmenu").each(function() {
                    if ($(this).is(':checked')) {
                        var menu_submenuId = $(this).attr("menu_subMenuId"); alert(menu_submenuId);
                        _MenuSubMenuId += (_MenuSubMenuId == "") ? menu_submenuId : "," + menu_submenuId;
                    }
                });
                if (_MenuSubMenuId != "") {
                    $(".MenuSubMenuIdDelete").val(_MenuSubMenuId); alert($(".MenuSubMenuIdDelete").val());
                    return true;
                }
                else {
                    alert("Please select any one " + Meeage);
                    $(".MenuSubMenuIdDelete").val("");
                    return false;
                }

            }
            else {
                return false;
            }
        });
        $(".selectall").click(function() {
            if ($(this).val() == "Select All") {
                $(this).val("Deselect All");
                $(".chk_menusubmenu").prop("checked", true);
            }
            else {
                $(".chk_menusubmenu").prop("checked", false);
                $(this).val("Select All");
            }
        });
    });
</script>
<asp:PlaceHolder ID="form" runat="server">
    <table width="100%">
         <tr>
            <td class="title">
                <asp:Label ID="lblPageTitle" runat="server"/>
            </td>
         </tr>        
         <tr>
            <td class="form" colspan="2">
                <table width="90%" cellpadding="0">
                <tr>
                    <td align="center" colspan="2" ><asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td><td><asp:TextBox runat="server" ID="hdnMenuSubMenuId" CssClass="hdn MenuSubMenuIdDelete"></asp:TextBox> </td>
                </tr>
				<!--CONTROLS_START-->
				    
					<tr>                    
                    <td width="100">Main Menu</td>
                    <td><asp:DropDownList ID="ddlMenuId" runat="server" CssClass="menuid" AutoPostBack="true" OnSelectedIndexChanged="ddlMenuId_Changed"></asp:DropDownList>
                    <asp:RequiredFieldValidator ValidationGroup="vg" ControlToValidate="ddlMenuId" runat="server" ErrorMessage="Select Menu" ></asp:RequiredFieldValidator>
                    </td>                   
                </tr>
					<!--CONTROLS_END-->
                </table>
            </td>
         </tr>	
         <tr>
            <td align="center" colspan="2">
				<!--SAVEBUTTON_START-->
                <asp:Button ID="btnAllMenu" runat="server" OnClick="btnBind_Click" Text="Bind All Menu" CssClass="btngreen" /><asp:Button ID="Button1" Visible="false" runat="server" OnClick="btnBind_Click" Text="Bind All Sub Menu" CssClass="btngreen" ValidationGroup="vg"/>
                <!--SAVEBUTTON_END-->
            </td>
        </tr>
           <tr>
                <td><input runat="server" visible="false" type="button" id="selectall" class="selectall btngreen" value="Select All" /></td>
                <td></td>
            </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Literal ID="ltMenu_SubMenu" runat="server" ></asp:Literal>
            </td>
        </tr>        
         <tr>
            <td align="center" colspan="2">
				<!--SAVEBUTTON_START-->
                <asp:Button ID="btnSubmit" runat="server" Visible="false" OnClick="btnSubmit_Click" Text="Delete" CssClass="save btnred deleteallmenusubmneu"/>
                <!--SAVEBUTTON_END-->
            </td>
        </tr>
    </table>
</asp:PlaceHolder>
</asp:Content>

