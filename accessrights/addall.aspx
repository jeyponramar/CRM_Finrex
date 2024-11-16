<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="addall.aspx.cs" Inherits="accessrights_addall" %>

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
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server" Text="Access Rights"/>
        </td>
        <td width="30"><img src="../images/refresh.png" class="refresh" title="Refresh this page"/></td>
     </tr>
     <tr>
        <td colspan="2" align="center"><asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td>
     </tr>
     <tr>
        <td colspan="2">
            <table width="100%">
                <tr>
                    <td>Role</td>
                    <td><asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_Changed"></asp:DropDownList>
                        <input type="button" id="selectall" class="selectall button" value="Select All" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Literal ID="ltRights" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                <td></td>
                <td align="center">
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

