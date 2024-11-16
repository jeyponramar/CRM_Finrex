<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="bulksms.aspx.cs" Inherits="utilities_bulksms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script>
    $(document).ready(function() {
        senBulkSMS();
    });
    function isAllSent() {
        var totalsms = ConvertToInt($(".totalsms"));
        var totalsmssent = ConvertToInt($(".totalsmssent"));
        var totalfailed = ConvertToInt($(".totalfailed"));
        if (totalsms == (totalfailed + totalsmssent)) {
            return true;
        }
        else {
            return false;
        }
    }
    function senBulkSMS() {
        if (!isAllSent()) return;
        var id = $(".bulksmsid").val();
        var URL = "utilities/bulksms.ashx?id=" + id;
        $.ajax({
            url: URL,
            type: 'GET',
            async: true,
            success: function(jsonObj) {
                var data = jQuery.parseJSON(jsonObj);
                $(".totalsmssent").val(data.totalsent);
                $(".totalfailed").val(data.totalfailed);
                if (isAllSent()) {
                    alert("All sms have been sent successfully!");
                }
                else {
                    setTimeout("senBulkSMS()", 3000);
                }
            },
            error: function(err) {
                if (isAllSent()) {
                    alert("All sms have been sent successfully!");
                }
                else {
                    setTimeout("senBulkSMS()", 3000);
                }
            }
        });
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server" Text="Enquiry Data Import"/>
        </td>
     </tr>
     <tr>
        <td align="center">
            <asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label>
        </td>
     </tr>
     <tr>
        <td>
            <table width="100%">
                <tr>
                    <td>Total SMS</td>
                    <td><asp:Label ID="lblTotalSMS" runat="server" CssClass="totalsms"></asp:Label></td>
                </tr>
                <tr>
                    <td>SMS Sent</td>
                    <td><asp:Label ID="lblTotalSMSSent" runat="server" Text="0" CssClass="totalsmssent"></asp:Label></td>
                </tr>
                <tr>
                    <td>Failed</td>
                    <td><asp:Label ID="lblTotalFailed" runat="server" Text="0" CssClass="totalfailed"></asp:Label></td>
                </tr>
                <tr>
                    <td>    
                        <asp:TextBox ID="txtbulksmsid" runat="server" CssClass="hidden bulksmsid" Text="0"></asp:TextBox>
                        <asp:Button ID="btnSend" runat="server" Text="Send" OnClick="btnSend_Click" CssClass="button" />&nbsp;&nbsp;
                        <asp:Button ID="btnRetry" runat="server" Text="Retry" OnClick="btnReSend_Click" CssClass="button" />
                    </td>
                </tr>
            </table>
        </td>
     </tr>
</table>     
</asp:Content>

