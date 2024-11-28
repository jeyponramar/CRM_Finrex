 <%@ Page Language="C#" MasterPageFile="~/InnerMaster.master" AutoEventWireup="true" 
 CodeFile="auditadvisor-dashboard.aspx.cs" Inherits="auditadvisor_dashboard" EnableEventValidation="false" ValidateRequest="false"%>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:PlaceHolder ID="form" runat="server">
    <table width="100%">
      <tr>
        <td>
            <table width="100%" cellpadding="0">
                <tr>
                    <td>
                        <div style="width:20%;float:left;">
                            <table class="dashboard-widget-count-panel spage" href="auditadvisor/viewbankaudit.aspx?t=1" cellspacing="0" title="Total Audits">
                                <tr><td class="dashboard-widget-count" style="color:#29b604;"><asp:Label ID="lbltotalcount" runat="server"></asp:Label></td></tr>
                                <tr><td class="dashboard-widget-desc">Total Audits</td></tr>
                                <tr><td class="dashboard-widget-title" style="background-color: #29b604;">
                                        Total Audits<span style="float:right;"><i class="fa fa-bar-chart" aria-hidden="true"></i></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <%--<div style="width:20%;float:left;">
                            <table class="dashboard-widget-count-panel spage" href="auditadvisor/viewbankaudit.aspx?t=3" cellspacing="0" title="Waiting for your response">
                                <tr><td class="dashboard-widget-count" style="color:#f4292e;"><asp:Label ID="lblpendingresponsecount" runat="server"></asp:Label></td></tr>
                                <tr><td class="dashboard-widget-desc">Waiting for your response</td></tr>
                                <tr><td class="dashboard-widget-title" style="background-color: #f4292e;">
                                        Waiting for your response<span style="float:right;"><i class="fa fa-bar-chart" aria-hidden="true"></i></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="width:20%;float:left;">
                            <table class="dashboard-widget-count-panel spage" href="auditadvisor/viewbankaudit.aspx?t=4" cellspacing="0" title="Pending Audits">
                                <tr><td class="dashboard-widget-count" style="color:#297ff4;"><asp:Label ID="lblpendingcount" runat="server"></asp:Label></td></tr>
                                <tr><td class="dashboard-widget-desc">Pending Audits</td></tr>
                                <tr><td class="dashboard-widget-title" style="background-color: #297ff4;">
                                        Pending Audits<span style="float:right;"><i class="fa fa-bar-chart" aria-hidden="true"></i></span>
                                    </td>
                                </tr>
                            </table>
                        </div>--%>
                        <div style="width:20%;float:left;">
                            <table class="dashboard-widget-count-panel spage" href="auditadvisor/viewbankaudit.aspx?t=2" cellspacing="0" title="Open Audits">
                                <tr><td class="dashboard-widget-count" style="color:#e7540c;"><asp:Label ID="lblopencount" runat="server"></asp:Label></td></tr>
                                <tr><td class="dashboard-widget-desc">Open Audits</td></tr>
                                <tr><td class="dashboard-widget-title" style="background-color:#e7540c;">
                                        Open Audits<span style="float:right;"><i class="fa fa-bar-chart" aria-hidden="true"></i></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        
                        <div style="width:20%;float:left;">
                            <table class="dashboard-widget-count-panel spage" href="auditadvisor/viewbankaudit.aspx?t=5" cellspacing="0" title="Closed Audits">
                                <tr><td class="dashboard-widget-count" style="color:#f4a529;"><asp:Label ID="lblclosedcount" runat="server"></asp:Label></td></tr>
                                <tr><td class="dashboard-widget-desc">Closed Audits</td></tr>
                                <tr><td class="dashboard-widget-title" style="background-color: #f4a529;">
                                        Closed Audits<span style="float:right;"><i class="fa fa-bar-chart" aria-hidden="true"></i></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="45%" valign="top">
                                    <div class="chart-box">
                                        <div class="chart-title">Query by Status</div>
                                        <asp:Literal ID="ltquerystatus" runat="server"></asp:Literal>
                                    </div>
                                </td>
                                <td width="45%" valign="top">
                                    <div class="chart-box">
                                        <div class="chart-title">Monthly Audits</div>
                                        <asp:Literal ID="ltmonthlyquery" runat="server"></asp:Literal>
                                    </div>
                                </td>
                                <%--<td width="40%" valign="top">
                                    <div class="chart-box">
                                        <div class="chart-title">Query by Topic</div>
                                        <asp:Literal ID="ltquerybytopic" runat="server"></asp:Literal>
                                    </div>
                                </td>--%>
                            </tr>
                        </table>
                    </td>
                    
			    </tr>
            </table>
        </td>
     </tr>
    </table>
</asp:PlaceHolder>

</asp:Content>
