<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoiceFormat.aspx.cs" Inherits="invoice_InvoiceFormat" %>
<%@ Register Src="~/usercontrols/PrintHeader.ascx" TagPrefix="uc" TagName="PrintHeader" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<link href="../css/print.css" media="print" rel="Stylesheet" type="text/css" />
<script src="../js/jquery.min.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            fixPrintHeight();
        });
    </script>
    <title>Invoice</title>
   
</head>
<body style="font-family:Arial;font-size:14px;margin:0px;">
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="900px">
     <tr class="noprint">
      <td align="right">
      <img src="../images/printer.png" style="cursor:pointer" title="Print" class="print" onclick="window.print()" />
      </td>
      </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                               <%-- <td>
                                    <uc:PrintHeader ID="PrintHeader" runat="server" />
                                </td>--%>
                                <td><img src="../images/finrex.png" /></td>
                                <td style="width:100px;">&nbsp;</td>
                                <td>
                                     <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="font-weight:bold;"><asp:Label ID="lblCompanyName" runat="server"></asp:Label></td></tr>
                                        <tr>
                                            <td style="padding-top:5px; font-weight:bold;"><asp:Label ID="lblAddress" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top:5px;"><b>Tel No. : </b><asp:Label ID="lblphone" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;<b>Email Id : </b><asp:Label ID="lblemail" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td  style="padding-top:5px;padding-bottom:5px;"><b>Web Site : </b><span style="color:#ED8237;font-weight:bold;"><asp:Label ID="lblwebsite" runat="server"></asp:Label></span></td>
                                        </tr>
                                        <tr>
                                            <td  style="padding-top:5px;padding-bottom:5px;"><b>State : </b><asp:Label ID="lblstate" runat="server"></asp:Label> &nbsp;&nbsp;&nbsp;<b>State Code : </b><asp:Label ID="lblstatecode" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                    
                </table>
            </td>
            
        </tr>
        <tr><td style="border-top:solid 1px black;border-bottom:solid 1px black;">&nbsp;</td></tr>
        <tr>
            <td style="border-bottom:solid 1px black; width:100%; padding:5px;">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="width:35%;">&nbsp;</td>
                        <td align="center" style="width:35%;font-weight:bold;">TAX INVOICE</td>
                        <td align="right" style="width:35%;font-size:11px;">Original For Recipient</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding-left:25px;padding-top:10px;">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="border-top:solid 1px black;border-bottom:solid 1px black;">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="width:50%; vertical-align:top;">
                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td style="padding-bottom:10px;font-weight:bold;padding-top:2px;">Bill To : </td>
                                                        </tr>
                                                        <tr>
                                                            <td ><b>M/s. <asp:Label ID="lblClientName" runat="server"></asp:Label>,</b></td>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Label ID="lblClientAddress" runat="server"></asp:Label></td>
                                                        </tr>
                                                        <%--<tr>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td style="padding-bottom:7px;width:55px;"><b>Tel. </b></td>
                                                                        <td style="padding-bottom:7px;width:5px;">:</td>
                                                                        <td style="padding-bottom:7px;"><asp:Label ID="lblTelephone" runat="server"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding-bottom:7px;"><b>Cell </b></td>
                                                                        <td style="padding-bottom:7px;width:5px;">:</td>
                                                                        <td style="padding-bottom:7px;"><asp:Label ID="lblMobileNo" runat="server"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding-bottom:7px;"><b>E-mail </b></td>
                                                                        <td style="padding-bottom:7px;width:5px;">:</td>
                                                                        <td style="padding-bottom:7px;"><asp:Label ID="lblclientemailid" runat="server"></asp:Label></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>--%>
                                                    </table>
                                                </td>
                                                <td  style="width:50%;padding:0 5px 0 5px;vertical-align:top;">
                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td style="padding-bottom:7px;width:100px;"><b>Invoice No. </b></td>
                                                            <td style="padding-bottom:7px;width:5px;padding-right:5px;">:</td>
                                                            <td style="padding-bottom:7px;"><asp:Label ID="lblquotationno" runat="server"></asp:Label></td>
                                                            
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-bottom:7px;"><b>Invoice Date. </b></td>
                                                            <td style="padding-bottom:7px;width:5px;">:</td>
                                                            <td style="padding-bottom:7px;"><asp:Label ID="lblDate" runat="server"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-bottom:7px;"><b>PAN NO. </b></td>
                                                            <td style="padding-bottom:7px;width:5px;">:</td>
                                                            <td style="padding-bottom:7px;"><asp:Label ID="lblpanno" runat="server"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-bottom:7px;"><b>GST No.</b></td>
                                                            <td style="padding-bottom:7px;width:5px;">:</td>
                                                            <td style="padding-bottom:7px;"><asp:Label ID="lblgstno" runat="server"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-bottom:7px;"><b>SAC No</b></td>
                                                            <td style="padding-bottom:7px;width:5px;">:</td>
                                                            <td style="padding-bottom:7px;">998311</td>
                                                        </tr>
                                                        
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-top:solid 1px black;">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="border-right:solid 1px black;border-left:solid 1px black;">
                                                    <table>
                                                        <tr>
                                                            <td style="font-weight:bold;vertical-align:top;">Director Name</td>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Label ID="lbldirectorname" runat="server"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="">
                                                    <table>
                                                        <tr>
                                                            <td style="font-weight:bold;vertical-align:top;">Director Mobile No</td>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Label ID="lbldirectormobileno" runat="server"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="border-left:solid 1px black;">
                                                    <table>
                                                        <tr>
                                                            <td style="font-weight:bold;vertical-align:top;">Director Email Id</td>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Label ID="lbldirectoremailid" runat="server"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="border-right:solid 1px black;border-left:solid 1px black;">
                                                    <table>
                                                        <tr>
                                                            <td style="vertical-align:top; font-weight:bold;">GSTIN No</td>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Label ID="lblgstin" runat="server"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-left:solid 1px black;border-top:solid 1px black;">
                                                    <table>
                                                        <tr>
                                                            <td style="font-weight:bold;vertical-align:top;">Finance Name</td>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Label ID="lblfinancename" runat="server"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="border-top:solid 1px black;border-left:solid 1px black;">
                                                    <table>
                                                        <tr>
                                                            <td style="font-weight:bold;vertical-align:top;">Finance Mobile No</td>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Label ID="lblfinancemobileno" runat="server"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="border-top:solid 1px black;border-left:solid 1px black;">
                                                    <table>
                                                        <tr>
                                                            <td style="font-weight:bold;vertical-align:top;">Finance Email Id</td>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Label ID="lblfinanceemailid" runat="server"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="border-right:solid 1px black;border-left:solid 1px black;border-top:solid 1px black;vertical-align:top;">
                                                    <table>
                                                        <tr>
                                                            <td style="font-weight:bold;vertical-align:top;">Office Tel No.</td>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Label ID="lblofficetelno" runat="server"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr><td><asp:Literal ID="ltgstTable" runat="server"></asp:Literal></td></tr>
                                <tr id="trOldProforInvoiceFormat" runat="server">
                                    <td>
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="vertical-align:top;" id="tdTableHeight" runat="server">
                                                    <table width="100%" cellpadding="0" cellspacing="0" height="100%">
                                                        <tr> 
                                                              <td style="width:2%;padding:5px;border-top:1px solid black;border-left:solid 1px black;border-bottom:1px solid black;text-align:center;"><b>Sr.No</b></td>  
                                                              <td style="width:40%;padding:5px;border-left:1px solid black;border-top:1px solid black;border-bottom:1px solid black;text-align:center;"><b>Description</b></td>
                                                              <td style="width:5%;padding:5px;border-left:1px solid black;border-top:1px solid black;border-bottom:1px solid black;text-align:center;"><b>QTY</b></td>
                                                              <td style="width:7%;padding:5px;border-left:1px solid black;border-top:1px solid black;border-bottom:1px solid black;text-align:center;"><b>Rate </b></td>
                                                              <td style="width:7%;padding:5px;border-left:1px solid black;border-right:solid 1px black;border-top:1px solid black;border-bottom:1px solid black;text-align:center;"><b>Amount </b></td>
                                                        </tr>
                                                        <asp:Repeater ID="rptinvoice" runat="server">
                                                            <ItemTemplate>
                                                                <tr style="font-size:13px;">
                                                                <td style="width:2%;padding:5px;text-align:center;vertical-align:top;border-left:solid 1px black;"><asp:Label ID="lblSerialno" runat="server"></asp:Label></td>
                                                                    <td style="width:40%;padding:5px;border-left:1px solid black;text-align:left;padding-left:10px;"><b><asp:Label ID="lblDescription" runat="server" Text=""></asp:Label></b></td>
                                                                    <td style="text-align:center;border-left:1px solid black;text-align:center;"><b><asp:Label ID="lblQuantity" runat="server" Text="" /></b></td>
                                                                    <td style="text-align:center;border-left:1px solid black;text-align:center;"><b><asp:Label ID="lblrate" runat="server" Text="" /></b></td>
                                                                    <td style="text-align:center;border-left:1px solid black;text-align:center;border-right:solid 1px black;"><b><asp:Label ID="lblAmount" runat="server" Text="" /></b></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <tr>
                                                            <asp:Literal ID="ltsetCustomHeight" runat="server"></asp:Literal>
                                                        </tr>
                                                        <asp:Literal ID="ltsubtotal" Visible="false" runat="server" ></asp:Literal> 
                                                        <asp:Literal ID="ltTax" Visible="false" runat="server" ></asp:Literal>  
                                                        <tr>
                                                            <td colspan="4" style="border-bottom:1px solid black;border-left:1px solid black;text-align:right; padding-right:20px;padding-top:10px; padding-bottom:10px;"><b>Total</b></td>
                                                            <td align="right" style="border-bottom:1px solid black;border-left:1px solid black;border-right:1px solid black;padding-right:10px;"><b><asp:Label ID="lblTotalAmount" runat="server"></asp:Label></b></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-top:5px;padding-bottom:5px;border-bottom:solid 1px black;font-size:14px;border-right:solid 1px black;border-left:solid 1px black;padding-left:5px;">Rupees : <asp:Label ID="lblAmountinWord" runat="server"></asp:Label></b>                
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr><td style="border-right:solid 1px black;border-left:solid 1px black;padding:5px;"> TDS on Professional Fees u/s 194J @ 10% </td></tr>
                                <tr>
                                    <td style="border-right:solid 1px black;border-left:solid 1px black;">
                                        <table cellpadding="0" cellspacing="5" width="100%">
                                            <tr>
                                                <td style="font-size:14px;"><asp:Label class="a-pb" ID="lblBankdetail" runat="server"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr> 
                                <tr>
                                    <td style="border-top:1px solid black;border-right:1px solid black; font-size:13px;">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                             <td style="padding-top:10px;border-right:solid 1px black;border-left:solid 1px black;">
                                                                <table cellpadding="0" cellspacing="5" width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <table>
                                                                            <tbody><tr>
                                                                                <td><b><u>Bank Detail NEFT/RTGS : </u></b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table>
                                                                                        <tbody>
                                                                                        <tr>
                                                                                            <td>Beneficiary Name</td>
                                                                                            <td>:</td>
                                                                                            <td>FINREX TREASURY ADVISORS LLP</td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>Bank Name</td>
                                                                                            <td>:</td>
                                                                                            <td>ICICI BANK LTD.</td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>A/c Number</td>
                                                                                            <td>:</td>
                                                                                            <td>697705600488</td>
                                                                                       </tr>
                                                                                      <tr>
                                                                                          <td>Branch</td>
                                                                                          <td>:</td>
                                                                                          <td>Andheri Kurla Branch, Visal Apartments, Andheri Kurla Road, Mumbai 400069.</td>
                                                                                      </tr>
                                                                                      <tr>
                                                                                          <td>IFSC Code</td>
                                                                                          <td>:</td>
                                                                                          <td>ICIC0006977</td>
                                                                                     </tr>
                                                                                     <tr>
                                                                                          <td><b>MICR Code</b></td>
                                                                                          <td>:</td>
                                                                                          <td>400229170</td>
                                                                                     </tr>
                                                                                     </tbody>
                                                                                 </table>
                                                                              </td>
                                                                          </tr>
                                                                          </tbody>
                                                                       </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="2" width="100%">
                                                                    <%--<tr>
                                                                        <td>&nbsp;</td>
                                                                        <td align="right" style="padding-right:35px;">Thanking You & Best Regards,</td>
                                                                    </tr>--%>
                                                                    <tr>
                                                                        <td><span style="font-weight:bold;">For M/s. <asp:Label ID="lblCompanyName1" runat="server"></asp:Label>,</span></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="height:45px;" align="center"><img src='../images/signature.jpg' height='100'/>'</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center"><b>Authorised Signatory</b></td>
                                                                    </tr>
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
                        </td>
                    </tr>
                    <tr><td style="font-size:12px;padding-top:10px;">This is Computer Generated Invoice</td></tr>
                </table>
            </td>
        </tr>
       </table>
    </form>
</body>
</html>
