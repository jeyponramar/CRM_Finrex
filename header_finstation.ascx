<%@ Control Language="C#" AutoEventWireup="true" CodeFile="header_finstation.ascx.cs" Inherits="header_finstation" %>
<table width="90%">
    <tr>
        <td style="color:#32347e;font-size:25px;font-weight:bold;width:150px;">
            <asp:Literal ID="ltappname" runat="server"></asp:Literal>
        </td>
        <td style="color:#000;">
            <table width="100%" id="tblLoginSection" runat="server" visible="false">
                <tr>
                    <td align="right">
                        <table >
                            <tr>
                                <td style="font-size:12px;padding-right:10px;width:150px;color:#000;">
                                    <asp:Literal ID="ltctdate" runat="server"></asp:Literal>
                                </td>
                                <%--<td id="div_setalert" runat="server"><div class="btnsetalert set-alert">Set Alert</div></td>--%>
                                <td>
                                    <table>
                                        <tr>
                                            <td style="vertical-align:top;padding-right:15px;">
                                                <div style="position:relative;" class="jq-push-notify-panel">
                                                    <div><i class='icon ion-ios-bell header-bell jq-header-bell'></i></div>
                                                    <div class="push-notify-msg-count"></div>
                                                    <div class="push-notify-msg-list"></div>
                                                </div>
                                            </td>
                                            <td id="div_finwatch" runat="server" style="vertical-align:top;">
                                                <div style="background-image: url(images/download-icon.png);background-color: #F83E15;width: 122px;
                                                    background-size: 14px;background-repeat: no-repeat;padding-left: 22px;padding-left: 35px;background-position: 10px;
                                                    font-weight: bold;padding-top: 5px;padding-bottom: 5px;">
                                                   <a href="https://finstation.in/softwares/FinWatchSetup.msi?v=2.6" target="_blank" style="color:#fff;text-decoration:none;">Download FinWatch</a> 
                                                </div>
                                            </td>
                                            <td style="vertical-align:top;">
                                                 <div style="background-color: #0000ff;width: 125px;margin-left: 10px;padding: 3px;border-radius: 5px;text-align:center;">
                                                    <a href="buy-sell-scrips-enquiry.aspx" style="color:#fff;text-decoration:none;">Buy - Sell Scrips RODTEP/ROSCTL</a>
                                                </div>
                                            </td>
                                            <td style="vertical-align:top;">
                                                <div style="margin-left: 10px;margin-top: -4px;">
                                                    <a href="https://finrex-in.bookmyforex.com/" target="_blank"><img src="images/fx-traveler.png" height="42"/></a>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <%--<td align="right">   
                                   <a href="change_password.aspx" class="lnkblue">Change Password</a>
                                </td>--%>
                                <td style="color:#222;font-size:13px;width:200px;display:none;" align="right">Welcome &nbsp;<asp:Label ID="lblUserName" runat="server" CssClass="loginuser" style="color:#000;font-weight:bold;"></asp:Label></td>
                                <td align="right" style="padding-left:5px;display:none;"><asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="lnkLogout_Click" CssClass="btnlogout"/></td>
                                <td>
                                    <div style="position:relative;display:none;">
                                        <div class="user-profile-initial jq-img-userprofile"><asp:Label ID="lbluserinitial2" runat="server"></asp:Label></div>
                                        <ul class="user-profile-menu jq-user-profile-menu">
                                            <li><a href="myprofile.aspx">My Profile</a></li>
                                            <li><a href="viewinvoice.aspx">My Invoices</a></li>
                                            <li><a href="mysubscription.aspx">My Subscription</a></li>
                                            <li><a href="viewadvisorcontact.aspx">Advisor Contact</a></li>
                                            <li><a href="">Account Security</a>
                                                <ul>
                                                    <li><a href="change_password.aspx">Change Password</a></li>
                                                     <li><a href="viewloginhistory.aspx">Login Activity</a></li>
                                                </ul>
                                            </li>
                                            <li><a href="clientfeedback.aspx">Feedback</a></li>
                                        </ul>
                                    </div>
                                </td>
                            </tr>
                            <tr><asp:Literal ID="ltMarque" runat="server"></asp:Literal></tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
  
</table>

