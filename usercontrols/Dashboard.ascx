<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Dashboard.ascx.cs" Inherits="usercontrols_Dashboard" %>
<%@ Register Src="~/Grid.ascx" TagName="Grid" TagPrefix="uc" %>
<%@ Register Src="~/usercontrols/Dashboard/TrialReminder.ascx" TagName="TrailReminder" TagPrefix="uc" %>
<%@ Register Src="~/usercontrols/Dashboard/SubscriptionReminder.ascx" TagName="SubscriptionReminder" TagPrefix="uc" %>
<%@ Register Src="~/usercontrols/Dashboard/CompetitorsRenewalReminder.ascx" TagName="CompetitorsRenewalReminder" TagPrefix="uc" %>
<%@ Register Src="~/usercontrols/ReminderSummary.ascx" TagName="ReminderSummary" TagPrefix="uc" %>
<%@ Register Src="~/usercontrols/Dashboard/EnquiryStatus.ascx" TagName="EnquiryStatus" TagPrefix="uc" %>
<%@ Register Src="~/usercontrols/Dashboard/ExpectedLeadByCampaign.ascx" TagName="ExpectedLeadByCampaign" TagPrefix="uc" %>
<%@ Register Src="~/usercontrols/Dashboard/SmartEnquirySummary.ascx" TagName="SmartEnquirySummary" TagPrefix="uc" %>
<%@ Register Src="~/usercontrols/Dashboard/Notes.ascx" TagName="Notes" TagPrefix="uc" %>
<%@ Register Src="~/usercontrols/Dashboard/MyEnquiryFollowups.ascx" TagName="MyEnquiryFollowups" TagPrefix="uc" %>
<%@ Register Src="~/usercontrols/Dashboard/PostDatedChequePaid.ascx" TagName="PostDatedChequePaid" TagPrefix="uc" %>
<%@ Register Src="~/usercontrols/Dashboard/PostDatedChequeReceived.ascx" TagName="PostDatedChequeReceived" TagPrefix="uc" %>

<table cellpadding="0" cellspacing="0" width="100%">
<tr>
    <td> 
        <table width="100%">
            <tr><td class="title" style="font-size:18px;">Dashboard</td>
                <td class="title" style="font-size:18px;">Welcome to <asp:Label ID="lblcompanyname" runat="server"></asp:Label></td>
                <td align="right"><img src="../images/refresh.png" class="refresh"/></td>
            </tr>
        </table>
    </td>
</tr>
<tr>
    <td style="background-color:#f5f5f5">
        <table width="95%" align="center">
            <tr>
                <td align="center">
                    <table width="100%">
                        <tr>
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td class="home-title tcorner">My Task</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <uc:Grid ID="gridMyTask" runat="server" Module="MyOpenTask" />
                                                    </td>
                                                </tr>
                                            </table>            
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="dashboard-panel">
                                <div class="dashboard-box">
                                    <uc:EnquiryStatus ID="EnquiryStatus" runat="server" />
                                </div>    
                                <div class="dashboard-box">
                                    <uc:ExpectedLeadByCampaign ID="ExpectedLeadByCampaign" runat="server" />
                                </div>                            
                               
                                
                            </td>
                            <td align="center" class="dashboard-panel">
                                <div class="dashboard-box" id="divenquiryFollowup" runat="server">
                                    <uc:MyEnquiryFollowups ID="MyEnquiryFollowups" runat="server" />
                                </div>
                                <div class="dashboard-box" id="divTrialReminder" runat="server">
                                    <uc:TrailReminder ID="TrialReminder" runat="server" />
                                </div>
                                <div class="dashboard-box" id="divFeedbackReminder" runat="server">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr><td class="dashboard-title">Feedback Reminders</td></tr>
                                        <tr>
                                            <td align="center">
                                                <uc:Grid ID="gridFeedbackReminder" runat="server" Module="feedbackreminder" IsReport="true"/>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                               
                            </td>
                            <td align="center" class="dashboard-panel">
                                 <div class="dashboard-box">
                                    <uc:Notes ID="Notes" runat="server" />
                                </div>
                                <div class="dashboard-box" id="divSubscriptionReminder" runat="server">
                                    <uc:SubscriptionReminder ID="SubscriptionReminder" runat="server" />
                                </div>
                                <div class="dashboard-box" id="divCompetitorsRenewalReminder" runat="server">
                                    <uc:CompetitorsRenewalReminder ID="CompetitorsRenewalReminder" runat="server" />
                                </div>
                                <div class="dashboard-box" id="divsmartenquirysummary" runat="server">
                                    <uc:SmartEnquirySummary ID="SmartEnquirySummary" runat="server" />
                                </div> 
                               
                            </td>
                        </tr>
                    </table>
                   
                </td>
            </tr>
        </table>
    </td>
</tr>
</table>
<script language="javascript">
    $(document).ready(function() {
        $.ajax({
            type: "POST",
            url: "../GetAllCounts.ashx",
            contentType: "application/text; charset=utf-8",
            dataType: "text",
            success: function(jsonObj) {
                var arrCounts = jsonObj.split(',');
                $("#client-count").text(arrCounts[0]);
            },
            error: function(ex) {
                //alert("Unable to populate counts");
            }
        });
        $.ajax({
            type: "POST",
            url: "../getallcountbasedonlogin.ashx",
            contentType: "application/text; charset=utf-8",
            dataType: "text",
            success: function(jsonObj) {
                var arrCounts = jsonObj.split(',');
                $("#open-enquiry-count").text(arrCounts[0]);
                $("#assign-enquiry-count").text(arrCounts[1]);
                $("#opport-enquiry-count").text(arrCounts[2]);
                $("#hold-enquiry-count").text(arrCounts[3]);
                $("#quotation-created-enquiry-count").text(arrCounts[4]);
                $("#won-enquiry-count").text(arrCounts[5]);
            },
            error: function(ex) {
                //alert("Unable to populate counts");
            }
        });
    });
</script>