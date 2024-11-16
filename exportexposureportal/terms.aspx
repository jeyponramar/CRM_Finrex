<%@ Page Title="" Language="C#" MasterPageFile="~/exportexposureportal/ExportPortalMasterPage.master" AutoEventWireup="true" CodeFile="terms.aspx.cs" Inherits="exportexposureportal_terms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        $(".btndecline").click(function() {
            window.close();
            return false;
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="600" align="center">
        <tr>
            <td style="color:#32347e;font-size:25px;font-weight:bold;">Finstation</td>
        </tr>
        <tr>
            <td class="title">Terms and Conditions</td>
        </tr>
        <tr>
            <td>
                <div style="width:100%;height:450px;overflow-y:auto;background-color:#fff;border:solid 1px #aaa;padding:10px;">
                    <asp:Label ID="lblterms" runat="server"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td><asp:Button ID="btnAgree" runat="server" Text="I Agree" CssClass="button" OnClick="btnAgree_Click" /></td>
                        <td><asp:Button ID="btnDeline" runat="server" Text="I Decline" CssClass="cancel btndecline"/></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

