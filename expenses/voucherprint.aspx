<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="voucherprint.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/print.css" rel="stylesheet" media="print" type="text/css"></link>
    <style>
    .compamyname{font-size:35px; font-family:Arial;text-align:left;font-weight:bold;}
    .border-tlr{border-left:1px solid black;border-top:1px solid black;border-right:1px solid black;padding:3px;}
    .border-tl{border-left:1px solid black;border-top:1px solid black;}
    .border-trl{border-top:1px solid black;border-right:1px solid black;padding:3px;border-left:1px solid black;}
    .border-blr{border-left:1px solid black;border-bottom:1px solid black;border-right:1px solid black;padding:3px;}
    </style>
</head>
<body style="font-family:Arial;font-size:15px;">
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="5" width="800px">
            <tr>
                <tr>
                <td style="padding-left:20px;"><img style="cursor:pointer" src="../images/printer.png" title="Print" class="noprint" onclick="window.print()"/>
                </tr>
                <td style="border:1px solid black;padding:5px;">
                    <table  cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width:75%">
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr><td class="compamyname"><asp:Label ID="lblCompanyName" runat="server"></asp:Label></td></tr>
                                                <tr><td><asp:Label ID="lblCompamyAddress" runat="server"></asp:Label></td></tr>
                                                <tr><td>Tel : <asp:Label ID="lblTelephoneno" runat="server"></asp:Label></td></tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <table>
                                                            <tr>
                                                                <td>E-mail : <asp:Label ID="lblEmailId" runat="server"></asp:Label></td>
                                                                <td>Website : <asp:Label ID="lblWebsite" runat="server"></asp:Label></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>                                        
                                        <td style="width:25%">
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr><td>No</td><td style="width:5px;">:</td> <td style="text-align:left;"><asp:Label ID="lblvoucherno" runat="server"></asp:Label></td></tr>
                                                <tr><td>Date </td><td>:</td><td><asp:Label ID="lblvocherdate" runat="server"></asp:Label></td></tr>
                                                <tr>
                                                    <td style="width:35px;">Rs. </td><td>:</td>
                                                    <td style="border:1px solid black; padding:3px;width:80px;font-weight:bold;"><asp:Label ID="totalamount" runat="server"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </td>

                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr><td>&nbsp;</td></tr>
                        <tr>
                            <td>
                                <center>
                                <table>
                                    <tr><td style="background-color:Black; color:White;text-align:center;width:180px;padding:3px;font-weight:bold;">CASH VOUCHER</td></tr>
                                </table>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td class="border-tlr" style="padding-left:5px;">Paid to : <b><asp:Label ID="lblemployeename" runat="server"></asp:Label></b></td>
                        </tr>
                        <%--<tr>
                            <td class="border-tlr" style="padding-left:5px;">Debit : <b><asp:Label ID="lblexpense" runat="server"></asp:Label></b></td>
                        </tr>--%>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <%--<tr>
                                                    <td style="text-align:center;width:70%;" class="border-tl">Particulars</td>
                                                    <td style="text-align:center;width:15%;" class="border-tl">Rs.</td>
                                                    <td style="text-align:center;width:15%;" class="border-trl">Ps.</td>
                                                </tr>--%>
                                               <asp:Literal ID="ltvoucher" runat="server"></asp:Literal>
                                                <tr>
                                                    <td class="border-tl" style="text-align:right;padding-right:5px;"><b>Total</b></td>
                                                    <td class="border-trl" align="right" style="padding-right:10px;"><b><asp:Label ID="lbltotalamount" runat="server"></asp:Label></b></td>
                                                    <%--<td class="border-trl"></td>--%>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr><td class="border-trl" style="padding-left:5px;font-style:italic;"><b>Rupees&nbsp;&nbsp;&nbsp;: </b><asp:Label ID="lblamtinwords" runat="server"></asp:Label></td></tr>
                                    <tr><td style="border-bottom:1px solid black;"></td></tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width:79%;" class="border-blr">
                                            <table cellpadding="0" cellspacing="10" width="100%">
                                                <tr>
                                                    <td>
                                                        Authorised by : ________________
                                                    </td>
                                                    <td>Passed by : __________________</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        Paid Cash / Cheque drawn on : _________________________
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Cheque No. : ______________________
                                                    </td>
                                                    <td>Date : ___________________________</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width:20%;">
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td style="padding-left:20px;">
                                                        <center>
                                                        <table cellpadding="0" cellspacing="0" width="50%">
                                                            <tr><td style="border:1px solid black;text-align:center;width:0px; height:50px;">&nbsp;</td></tr>
                                                        </table>
                                                        </center>
                                                    </td>
                                                </tr>
                                                <tr><td style="text-align:center;">Receiver's Signature</td></tr>
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
    </form>
</body>
</html>
