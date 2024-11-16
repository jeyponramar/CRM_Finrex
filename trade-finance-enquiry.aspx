<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="trade-finance-enquiry.aspx.cs" Inherits="trade_finance_enquiry" Title="Trade Finance Enquiry" %>
<%@ Register Src="~/Usercontrols/MultiFileUpload.ascx" TagName="MultiFileUpload" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width='100%' cellspacing=0 cellpadding=0>
    <tr><td class='page-inner2'>
        <table width='100%'>
            <tr><td class='page-title2'>Trade Finance</td></tr>
             <tr><td>&nbsp;</td></tr>
             <tr>
                <td style="padding-left:50px;">
                    <table width="100%">
                        <tr><td class="bold">Import Finance</td></tr>
                         <tr><td>
                            <ul>
                                <li><a href="tradefinanceenquiry-buyer-credit.aspx">Buyer Credit</a></li>
                                <li style="padding-top:10px;"><a href="tradefinanceenquiry-supplier-credit.aspx">Supplier Credit</a></li>
                            </ul>
                        </td></tr>
                        <tr><td class="bold">Export Finance</td></tr>
                         <tr><td>
                            <ul>
                                <li><a href="tradefinanceenquiry-export-factor.aspx">Export Factoring</a></li>
                                <li style="padding-top:10px;padding-bottom:10px;"><a href="tradefinanceenquiry-export-lc-discounting.aspx">Export LC Discounting</a></li>
                                <li><a href="tradefinanceenquiry-domestic-lc-discounting.aspx">Domestic LC Discounting</a></li>
                            </ul>
                        </td></tr>
                    </table>
                </td>
             </tr>
             
        </table>
    </td></tr>
</table>
</asp:Content>

