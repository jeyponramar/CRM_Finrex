<%@ Page Title="" Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="config-bulkedit.aspx.cs" Inherits="CP_config_bulkedit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {

        $(".collabel").change(function() {
            var label = $(this).val().trim().toLowerCase().replace(/\W/g, "");
            var colname = label;
            if (colname.indexOf("_") < 0) {
                colname = $(".prefix").val() + colname;
            }
            $(this).closest('tr').find('.colname').val(colname);
        });
        $(".ddlcontrol,.ddlselectedval").each(function() {
            var selectedVal = $(this).attr("selectedval");
            if (selectedVal != "") {
                $(this).val(selectedVal);
            }
        });
        $(".section").each(function() {
            var selectedVal = ConvertToInt($(this).attr("selectedval"));
            if (selectedVal > 0) {
                $(this).val(selectedVal);
            }
        });
        $(".dropdownmoduleid").blur(function() {
            var id = $(this).closest('tr').find('.hdnac').val();
            if (ConvertToInt(id) == 0) return;
            var url = "getddlcol.ashx?id=" + id;
            var columnName = RequestData(url);
            $(this).closest('tr').find('.dropdowncolumn').val(columnName);
            var idcol = "";
            if (columnName != "") {
                var arr = columnName.split('_');
                idcol = $(".prefix").val() + arr[0] + "id";
            }
            $(this).closest('tr').find('.colname').val(idcol);
        });

    });
    
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
    <tr><td class="title"><asp:Label id="lblTitle" runat="server" Text="Config Module - Bulk Update"></asp:Label></td></tr>
    <tr><td><asp:TextBox CssClass="prefix hidden" ID="txtPrefix" runat="server"></asp:TextBox></td></tr>
    <tr>
        <td>
            <asp:Label runat="server" ID="lblMessage" CssClass="error" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Button runat="server" ID="btnResetSequence" OnClick="btnResetSequence_Click" CssClass="button" Text="Reset Sequence" />
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%">
                <tr><td>
                    <asp:Literal ID="ltColumnDetail" runat="server"></asp:Literal>
                </td></tr>
            </table>
        </td>
    </tr>
   
    <tr>
        <td align="center">
            <asp:Button runat="server" ID="btnCreateColumn" CssClass="button" Text="Submit" OnClick="btn_CreateColumnClick" />
        </td>
    </tr>
</table>    
</asp:Content>

