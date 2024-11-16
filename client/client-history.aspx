<%@ Page Language="C#" AutoEventWireup="true" CodeFile="client-history.aspx.cs" Inherits="client_client_history" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Client History</title>
    <link href="../css/print.css" media="print" rel="Stylesheet" type="text/css" />
    <link href="../css/common.css" rel="Stylesheet" type="text/css" />
    <script src="../js/jquery.min.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            fixPrintHeight();
        });
    </script>

</head>
<body style="background-color:#fff;">
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="900px">
     <tr class="noprint">
      <td align="right">
        <img src="../images/printer.png" style="cursor:pointer" title="Print" class="print" onclick="window.print()" />
      </td>
      </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="5" width="100%">
                    <tr><td align="center"><asp:Label ID="lblClientName" runat="server" style="font-size:16px;font-weight:bold;text-decoration:underline;"></asp:Label></td></tr>
                    <tr><td align="center" style="font-size:14px;text-decoration:underline;">Job / History Card</td></tr>
                    <tr><td style="padding-top:10px">
                        <table cellpadding="0" cellspacing="5" width="100%">
                            <tr><td width="100">Date</td><td class="collon">:</td><td><asp:Label ID="lblAmcDate" runat="server"></asp:Label></td></tr>
                            <tr><td>Contact</td><td class="collon">:</td><td><asp:Label ID="lblContactPersons" runat="server"></asp:Label></td></tr>
                            <tr><td>Tel.</td><td class="collon">:</td><td><asp:Label ID="lblLandlineNos" runat="server"></asp:Label></td></tr>
                            <tr><td>Cell</td><td class="collon">:</td><td><asp:Label ID="lblModileNos" runat="server"></asp:Label></td></tr>
                            <tr><td>Email</td><td class="collon">:</td><td><asp:Label ID="lblEmails" runat="server"></asp:Label></td></tr>
                            <tr><td>&nbsp;</td></tr>
                            <tr><td>Site Location</td><td class="collon">:</td><td><asp:Label ID="lblSiteLocation" runat="server"></asp:Label></td></tr>
                        </table>        
                    </td></tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr><td class="print-subtitle">Invoices Raised</td></tr>
                    <tr><td><asp:Literal ID="ltInvoices" runat="server"></asp:Literal></td></tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr><td class="print-subtitle">Work Report Detail</td></tr>
                    <tr><td><asp:Literal ID="ltWorkReport" runat="server"></asp:Literal></td></tr>
                </table>
            </td>
        </tr>        
     </table>   
    </form>
</body>
</html>
