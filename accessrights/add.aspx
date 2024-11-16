<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="add.aspx.cs" Inherits="accessrights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function(){
        $(".selectall").click(function(){
            if($(this).val()=="Select All")
            {
                $(this).val("Deselect All");
                $(".chk").prop("checked", true);
            }
            else
            {
                $(".chk").prop("checked", false);
                $(this).val("Select All");
            }
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:PlaceHolder ID="form" runat="server">
<table width="100%" cellpadding="0" cellspacing="0">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server" Text="Access Rights"/>
        </td>
        
        <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
     </tr>
     <tr>
        <td><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td>
     </tr>
     <tr>
        <td>
                <table>
                    <tr>
                        <td>Role</td>
                        <td><asp:DropDownList ID="ddlRole" runat="server"></asp:DropDownList></td>
                    </tr>
                    
                    <tr>
                        <td>Module</td>
                        <td><asp:DropDownList ID="ddlModule" runat="server" OnSelectedIndexChanged="ddlModule_Change" AutoPostBack="false"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:Button ID="GetDetail" OnClick="btn_GetdetailClick" runat="server" Text="Get Detail" CssClass="button" /></td>
                    </tr>
                    <tr>
                    
                    </tr>
                    <tr>
                        <td>Menu :</td>
                        <td><asp:DropDownList ID="ddlMenu" runat="server" AutoPostBack="false"></asp:DropDownList>
                        <asp:Button class="access button" id="fullaccess" onclick="Btn_fullaccess_click" runat="server" Text="Full Access" /></td>
                    </tr>
                    <tr>
                        <td>Copy Access Rights From Role:</td>
                        <td><asp:DropDownList ID="ddlRole_From" runat="server" AutoPostBack="false"></asp:DropDownList>
                        <asp:Button class="access button" id="btn_copyaccess" onclick="Btn_copyaccess_click" runat="server" Text="Copy Access" /></td>
                    </tr>
                    <tr>
                        <td><input type="button" id="selectall" class="selectall button" value="Select All" /></td>
                        <td></td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:Literal ID="ltRights" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="button"/>
                        <input type="button" class="close-page cancel" value="Cancel"/>
                    </td>
                 </tr>
            </table>
        </td>
     </tr>
     
     
</table>
</asp:PlaceHolder>
</asp:Content>

