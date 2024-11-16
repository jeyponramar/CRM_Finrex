<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="config-mobile.aspx.cs" Inherits="CP_config_mobile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        $(".chkcol").click(function() {
            if ($(this).prop("checked")) {
                $(this).closest("tr").find(".txtcol").val('ismobile="true"');
            }
            else {
                $(this).closest("tr").find(".txtcol").val("");
            }
        });
        $(".chkissingle").click(function() {
            if ($(this).prop("checked")) {
                $(this).closest("tr").find(".txtcol").val('ismobile="true" colspan="2" class="bold"');
            }
        });
        $(".ddlmodule").change(function() {
            $(".ddlreport").val("0");
        });
        $(".ddlreport").change(function() {
            $(".ddlmodule").val("0");
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server" Text="Config Mobile"/>
        </td>
     </tr>
      <tr>
        <td class="error">
            <asp:Label ID="lblMessage" runat="server"/>
        </td>
     </tr>
     <tr>
        <td>
            <table>
                <tr>
                    <td>Module</td>
                    <td><asp:DropDownList ID="ddlModule" runat="server" CssClass="ddlmodule"></asp:DropDownList></td>
                    <td><asp:DropDownList ID="ddlReport" runat="server" CssClass="ddlreport"></asp:DropDownList></td>
                    <td><asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="button" /></td>
                </tr>
                
            </table>
        </td>
     </tr>
     <tr>
        <td><asp:Literal ID="ltColumns" runat="server"></asp:Literal></td>
     </tr>
     <tr><td><asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="button" /></td></tr>
</table>
</asp:Content>

