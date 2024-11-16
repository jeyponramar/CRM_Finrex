<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Reminder.ascx.cs" Inherits="usercontrols_Dashboard_Reminder" %>
<table width="100%" cellpadding="0" cellspacing="0">
    <tr><td class="dashboard-title">Reminders</td></tr>
    <tr>
        <td align="center" style="padding:20px">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table cellspacing=10>
                            <tr>
                               <td href="#reminder/AMCReminders.aspx" class="spage" title="AMCReminders">
                                 <table>
                                    <tr><td style="padding-left:20px">
                                        <div>
                                            <div class="left"><img src="../images/reminder2.png" height="30"/></div>
                                            <div class="left notification-circle"><span id="amc-reminders-count" class="bold" ></span></div>
                                       </div>
                                    </td></tr>
                                    <tr><td align="center">AMC</td></tr>
                                 </table>   
                               </td>
                               <td href="#reminder/AMCServiceReminders.aspx" class="spage" title="AMC Service Reminders">
                                 <table>
                                    <tr><td style="padding-left:20px">
                                        <div>
                                            <div class="left"><img src="../images/reminder3.png" height='30'/></div>
                                            <div class="left notification-circle"><span id="amcservice-reminders-count" class="bold"></span></div>
                                       </div>
                                    </td></tr>
                                    <tr><td align="center">AMC Service</td></tr>
                                 </table>   
                               </td>
                               <td href="#reminder/SalesWarrantyReminder.aspx" class="spage" title="Sales Reminder">
                                 <table>
                                    <tr><td style="padding-left:20px">
                                        <div>
                                            <div class="left"><img src="../images/warrenty.png" height="30"/></div>
                                            <div class="left notification-circle"><span id="sales-remainders-count" class="bold"></span></div>
                                       </div>
                                    </td></tr>
                                    <tr><td align="center">Warrenty</td></tr>
                                 </table>   
                               </td>
                               <td href="#reminder/SalesServiceReminders.aspx" class="spage" title="Sales Service Reminders">
                                 <table>
                                    <tr><td style="padding-left:20px">
                                        <div>
                                            <div class="left"><img src="../images/reminder3.png" height='30'/></div>
                                            <div class="left notification-circle"><span id="salesservice-reminders-count" class="bold"></span></div>
                                       </div>
                                    </td></tr>
                                    <tr><td align="center">Sales Service</td></tr>
                                 </table>   
                               </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                
            </table>
        </td>
    </tr>
</table>