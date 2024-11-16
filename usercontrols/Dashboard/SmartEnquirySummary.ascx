<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SmartEnquirySummary.ascx.cs" Inherits="usercontrols_Dashboard_SmartEnquirySummary" %>
<table id="tblsmartsummaryforenquiry" width="100%" cellpadding="0" cellspacing="0">
    <tr><td class="dashboard-title">Smart Enquiry Summary </td></tr>
    <tr>
        <td align="center" style="padding:20px">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr id="trformarketing" runat="server">
                    <td>
                        <table>
                            <tr>
                               <td><a href="utilities/view.aspx?m=enquiry&ew=enquiry_enquirystatusid=8" class="spage"  title="View Assigned Enquiry" ><img id="imgAssigned" runat="server" /></a></td>
                                <td class="sub-home1"><a href="utilities/view.aspx?m=enquiry&ew=enquiry_enquirystatusid=8" class="spage"  title="View Assigned Enquiry" >Assigned&nbsp;Enquiry&nbsp;(<span id="assign-enquiry-count" class="bold error"></span>)</a></td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table>
                            <tr>
                               <td><a href="utilities/view.aspx?m=enquiry&ew=enquiry_enquirystatusid=4" class="spage" title="View Opportunity Enquiry" ><img id="imgOpportunity" runat="server"/></a></td>
                               
                               <td class="sub-home1"><a href="utilities/view.aspx?m=enquiry&ew=enquiry_enquirystatusid=4" title="View Opportunity Enquiry" class="spage" >Opportunity&nbsp;Enquiry&nbsp;(<span id="opport-enquiry-count" class="bold error"></span>)</a></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="trformarketing1" runat="server">
                    <td>
                        <table>
                            <tr>
                               <td><a href="utilities/view.aspx?m=enquiry&ew=enquiry_enquirystatusid=9" title="View Quotation Created Enquiry" class="spage" ><img id="imgQuoteCreated" runat="server"/></a></td>
                                <td class="sub-home1"><a href="utilities/view.aspx?m=enquiry&ew=enquiry_enquirystatusid=9" title="View Quotation Created Enquiry" class="spage" >Quotation&nbsp;Sent&nbsp;(<span id="quotation-created-enquiry-count" class="bold error"></span>)</a></td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table>
                            <tr>
                               <td><a href="utilities/view.aspx?m=enquiry&ew=enquiry_enquirystatusid=5" title="View Enquiry Are Won" class="spage" ><img id="imgWon" runat="server"/></a></td>
                               
                               <td class="sub-home1"><a href="utilities/view.aspx?m=enquiry&ew=enquiry_enquirystatusid=5" title="View Enquiry Are Won" class="spage" >Won&nbsp;Enquiry&nbsp;(<span id="won-enquiry-count" class="bold error"></span>)</a></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                 <tr id="trformarketing2" runat="server">
                    <td>
                        <table>
                            <tr>
                               <td><a href="utilities/view.aspx?m=enquiry&ew=enquiry_enquirystatusid=10" title="View Hold Enquiry" class="spage" ><img id="imgHold" runat="server" /></a></td>
                               
                               <td class="sub-home1"><a href="utilities/view.aspx?m=enquiry&ew=enquiry_enquirystatusid=10" title="View Hold Enquiry" class="spage" >Hold&nbsp;Enquiry&nbsp;(<span id="hold-enquiry-count" class="bold error"></span>)</a></td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table>
                            <tr>
                               <td><a href="utilities/view.aspx?m=enquiry&ew=enquiry_enquirystatusid=1" class="spage" title="View Open Enquiry" ><img id="imgOpen" runat="server" /></a></td>
                                <td class="sub-home1"><a href="utilities/view.aspx?m=enquiry&ew=enquiry_enquirystatusid=1" class="spage" title="View Open Enquiry" >Open&nbsp;Enquiry&nbsp;(<span id="open-enquiry-count" class="bold error"></span>)</a></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
