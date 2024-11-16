<%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" CodeFile="DbBackup.aspx.cs" Inherits="DbBackup" Title="Database Backup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script>
    $(document).ready(function() {
        $(".btnbackupclick").click(function() {
            return confirm("Are you sure you want to take BackUp? And Make sure that This is Server PC");
        });
    });
        
</script>
<table width="100%">
     <tr>
        <td class="title">
            Database Backup
        </td>
     </tr>
     <tr>
        <td align="center"> <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
     </tr>
     <tr>
        <td class="form">
            <table>
                <tr><td class="label">Last Database Backup</td><td class="val"><asp:Label ID="lblLastBackup" runat="server" ></asp:Label></td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr><td></td>
                    <td>
                        <asp:Button ID="btnBackup" runat="server" CssClass="button btnbackupclick" OnClick="btnBackup_Click" Text="Backup Database" style="background-color:#055b77;color:#ffffff;"/>
                        <asp:Button ID="btnCancel" runat="server" CssClass="button" OnClick="btnCancel_Click" Text="Cancel"/>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>    
</asp:Content>

