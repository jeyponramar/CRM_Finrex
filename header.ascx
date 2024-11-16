<%@ Control Language="C#" AutoEventWireup="true" CodeFile="header.ascx.cs" Inherits="header" %>
<script>
    $(document).ready(function(){
        $(".jq-imgchatagent").click(function(){
            window.open("chat/chat-agent.aspx");
        });
    });
</script>
<div>
<asp:Literal ID="ltSubMenu" runat="server"></asp:Literal>
</div>
<table cellpadding="0" cellspacing="0" width="100%">
<tr>
    <td class="main-menu-bg">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td><img src="images/refux-logo.png" /></td>
                <td style="padding-left:20px;">&nbsp;</td>
                <td class="menu-sep"></td>
                <td class="menu homemenu adminhome"><a href="admin.aspx">Home</a></td>
                <td class="menu-sep"></td>
                <asp:Literal ID="ltMenu" runat="server"></asp:Literal>
                <td class="menu"><a href="logout.aspx">Logout</a></td>
                <td class="menu-sep"></td>
                <td>&nbsp;</td>
                <td><asp:Image ID="imgLoggedInUser" runat="server" Width="30" CssClass="imgborder"/></td>
                <td style="color:Red;font-family:Sans-Serif;font-weight:bold;font-size:15px;text-align:right;padding-left:10px" class="hand" align="right"><asp:Label Text=""  runat="server" ID="lblLoginUserName"></asp:Label></td>            
                <td align="right" width="100%" style="padding-right:10px;" id="tdLiveChat" runat="server" visible="false">
                    <asp:Image ID="imgLiveChat" runat="server" CssClass="jq-imgchatagent hand" ImageUrl="~/chat/images/chat-agent_30.png" Width="25" ToolTip="Click to start Live Chat"/>
                </td>
                
            </tr>
         </table>
    </td>
</tr>
</table>
