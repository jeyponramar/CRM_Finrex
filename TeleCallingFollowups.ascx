<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TeleCallingFollowups.ascx.cs" Inherits="Followups" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>
<style>
    .repeater
    {
        border:0px;
	    white-space:normal;
    }
    
</style>
<script type="text/javascript">
    $(document).ready(function() {
        $(".fol-page").click(function() {

            var ahref = ConvertToString($(this).attr("href"));
            var newHref = ahref.replace("#", "");

            loadPage(newHref, "Edit Followups");

        });
    });
</script>
<table width="100%">
    <tr><td class="title">Activities</td></tr>
    <tr>
        <td align="center">
            <uc:Grid ID="grid" runat="server" Module="followups"/>
        </td>
    </tr>    
</table>