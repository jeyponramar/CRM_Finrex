<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReminderSummary.ascx.cs" Inherits="usercontrols_ReminderSummary" %>
<table width="100%">
    <tr>
        <td>
            <table align="center" cellspacing=10>
                <tr>
                   <td href="#reminder/AMCReminders.aspx" class="spage">
                     <table>
                        <tr><td style="padding-left:30px">
                            <div>
                                <div class="left"><img src="../images/amc-reminders.png" /></div>
                                <div class="left notification-circle">12</div>
                           </div>
                        </td></tr>
                        <tr><td align="center">AMC Reminders</td></tr>
                     </table>   
                   </td>
                   <td href="#reminder/AMCReminders.aspx" class="spage">
                     <table>
                        <tr><td style="padding-left:30px">
                            <div>
                                <div class="left"><img src="../images/amc-reminders.png" /></div>
                                <div class="left notification-circle">12</div>
                           </div>
                        </td></tr>
                        <tr><td align="center">AMC Reminders</td></tr>
                     </table>   
                   </td>
                   <td href="#reminder/AMCReminders.aspx" class="spage">
                     <table>
                        <tr><td style="padding-left:30px">
                            <div>
                                <div class="left"><img src="../images/amc-reminders.png" /></div>
                                <div class="left notification-circle">12</div>
                           </div>
                        </td></tr>
                        <tr><td align="center">AMC Reminders</td></tr>
                     </table>   
                   </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        
        <td>
            <table>
                <tr>
                   <td><a href="#reminder/AMCReminders.aspx" class="spage" ><div><div class="left" style="padding-top:10px"><img src="../images/amc-reminders.png" /></div>
                   <asp:Label CssClass="left notification-circle" ID="lblAmcReminderCount" runat="server"></asp:Label></div></a>AMC Reminders</td>
                   <td class="sub-home1"><a href="#reminder/AMCReminders.aspx" class="spage"></a></td>
                </tr>
            </table>
        </td>
        <%--<td>
            <table>
                <tr>
                   <td style=" width:30px;"><a href="#reminder/AMCServiceReminders.aspx" class="spage" ><img src="../images/amc-reminders.png" /></a></td>
                   <td class="sub-home1"><a href="#reminder/AMCServiceReminders.aspx" class="spage"><div><div class="left" style="padding-top:10px">AMC Service Reminders</div>
                   <div class="left notification-circle" id="amcservice-reminders-count"></div></div></a></td>                                                
                </tr>
            </table>
        </td>
        <td>
            <table>
            <tr>
               <td><a href="#reminder/SalesReminder.aspx" id="SalesWarrantyid" class="spage" ><img src="../images/amc-reminders.png" /></a></td>
               <td class="sub-home1"><a href="#reminder/SalesReminder.aspx" class="spage" >Sales&nbsp;Warranty&nbsp;Reminder&nbsp;(<span id="sales-remainders-count" class="bold"></span>)</a></td>
            </tr>
        </table>
        </td>
        <td>
            <table>
                <tr>
                   <td style=" width:30px;"><a href="#reminder/SalesServiceReminders.aspx" class="spage" ><img src="../images/amc-reminders.png" /></a></td>
                   <td class="sub-home1"><a href="#reminder/SalesServiceReminders.aspx" class="spage">Sales Service Reminders (<span id="salesservice-reminders-count" class="bold error"></span>)</a></td>                                                
                </tr>
            </table>
        </td>--%>
    </tr>
</table>