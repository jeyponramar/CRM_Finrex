<%@ Page Title="" Language="C#" MasterPageFile="~/exportexposureportal/ExportPortalMasterPage.master" AutoEventWireup="true" CodeFile="portal-dashboard.aspx.cs" Inherits="exportexposureportal_portal_dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        window.parent.hideLoader();
        $("#btntestmail").click(function() {
            $.ajax({
                url: "exportexposureschedular.ashx?istest=true",
                isAsync: true,
                success: function() {
                    alert("Reminders mails sent successfully!");
                }
            });
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
    <tr>
        <td>
            <table>
                <tr><td>Currency : <asp:DropDownList ID="ddlcurrency" runat="server" OnSelectedIndexChanged="ddlcurrency_changed" style="background-color:#eee;" AutoPostBack="true"></asp:DropDownList></td>
                <td>Spot Rate : </td>
                <td><asp:Label ID="lblspotrate" runat="server" CssClass="bold"></asp:Label></td>
                <td style="padding-left:30px;width:100px;"><asp:LinkButton ID="lnkprevyear" Visible="false" runat="server" Text="Prev Year" OnClick="lnkprevyear_Click"></asp:LinkButton></td>
                <td><asp:LinkButton ID="lnknextyear" runat="server" Text="Next Year" OnClick="lnknextyear_Click"></asp:LinkButton></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Literal ID="ltDashboard" runat="server"></asp:Literal>
        </td>
    </tr>
</table>

</asp:Content>

