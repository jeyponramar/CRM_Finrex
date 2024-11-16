<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="viewbankaudit.aspx.cs" Inherits="viewbankaudit" %>
<%@ Register Src="~/usercontrols/ViewBankScanControl.ascx" TagName="bankscan" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    
    $(document).ready(function(){
        $(".jq-lnkaddbankaudit").click(function(){
            showDialog($("#divguidelinedialog"), "BankScan Guidelines");
            return false;
        });       
    });
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<uc:bankscan runat="server" />
<div id="divguidelinedialog" class="dialog" style="width:600px;height:400px;color:#000;">
    <table width="100%">
        <tr>
            <td style="padding:10px;">
                <div>
                Please note that to fill the Audit Questionnaire you require following details. Request you to collect the data before you start the process.
                </div>
                <ul style="font-size:14px;">
                    <li>Last 3years Export Import turnover and banking cost.</li>
                    <li>Schedule of Bank Charges. </li>
                    <li>Sanctioned letters and bank debit advises of Export and import Transactions.</li>
                </ul>
                <div><b>Other Instructions</b></div>
                <ul style="font-size:14px;">
                    <li>Instructions are not provided for the fields which are self explanatory. If you have any query is mark mail to <a href="mailto:audit@finrex.in">audit@finrex.in</a></li>
                    <li>If the space within any of the fields is not sufficient to provide all the information, then additional details can be provided in Remark column</li>
                    <li>Save the page in regular interval.</li>
                    <li>Click <span style="color:#ff0000;font-weight:bold;">SEND FOR REVIEW</span> button available in Summary to submit for review.</li>
                </ul>
            </td>
        </tr>
        <tr>
            <td align="right" style="padding:20px;"><asp:Button ID="btnaddbankaudit" Text="Proceed" runat="server" CssClass="button" OnClick="btnaddbankaudit_Click"/></td>
        </tr>
    </table>
</div>
</asp:Content>

