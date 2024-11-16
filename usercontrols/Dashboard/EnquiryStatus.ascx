<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EnquiryStatus.ascx.cs" Inherits="usercontrols_Dashboard_EnquiryStatus" %>
<table id="tblenquirybystatus" width="100%" cellpadding="0" cellspacing="0">
     <tr><td class="dashboard-title">Enquiry Status</td></tr>
   <tr>
        <td align="center" style="padding:20px">
            <iframe id="iFrame" runat="server" width="300" height="235" style="border:0px" scrolling="no"></iframe>
        </td>
    </tr> 
    <tr>
        <td style="background-color:#f5f5f5;padding-left:20px;font-size:10.5px;">
        <table width="100%" cellpadding="5" cellspacing="0">
            <asp:Literal ID="ltEnquiryStatus" runat="server"></asp:Literal>
            </table>
        </td>
    </tr>                                      
</table>