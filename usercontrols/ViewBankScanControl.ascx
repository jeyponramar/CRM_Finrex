<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ViewBankScanControl.ascx.cs" Inherits="usercontrols_ViewBankScanControl" %>
<table width='100%' cellspacing=0 cellpadding=0>
    <tr>
        <td class='page-inner2' cellpadding="0" cellspacing=0>
        <table width="100%">
            <tr>
                <td class="page-title2">BankScan</td>
            </tr>
           <%-- <tr>
                <td style="padding:10px;padding-right:100px;" align="right"><a href="addbankaudit.aspx" style="color: #fff;
    background-color: #ff8080;
    padding: 7px 15px;
    text-decoration: none;
    border-radius: 5px;" class="jq-lnkaddbankaudit">Add New Audit</a></td>
            </tr> --%>
            <tr>
                <td>
                    <asp:Literal ID="ltdata" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
        
       </td>
    </tr>
 </table>