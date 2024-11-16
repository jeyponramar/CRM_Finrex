<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Calllogreport.aspx.cs" Inherits="communicationsources_Calllogreport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../css/print.css" media="print" rel="Stylesheet" type="text/css" />
    <link href="../css/common.css" rel="Stylesheet" type="text/css" />
    <script src="../js/jquery.min.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            fixPrintHeight();
        });
    </script>
    <title>Call Log Report</title>
</head>
<body style="font-family:Arial;background-color:#fff;padding:10px;">
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="800px">
     <tr class="noprint">
      <td align="right">
      <img src="../images/printer.png" style="cursor:pointer" title="Print" class="print" onclick="window.print()" />
      </td>
      </tr>
        <tr>
                    <td style="border:solid 1px #000000;">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="padding-left:5px;"><img src="../images/finrex.png" width="120px" /></td>
                                <td style="text-align:center;width:65%;">
                                    <table width="100%">
                                        <tr>    
                                            <td style="font-size:25PX;"><b><asp:Label ID="lblCompanyName" runat="server"></asp:Label></b></td>
                                        </tr>
                                        <tr>
                                            <td style="font-size:13PX;"><asp:Label ID="lblCompanyAddress" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="90%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="font-size:13PX;">Emailid : <asp:Label ID="lblemailid" runat="server"></asp:Label></td>
                                                        <td style="font-size:13PX;">website : <asp:Label ID="lblwebsite" runat="server"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="font-size:14px;vertical-align:top;border-left:solid 1px #000000; width:30%;">
                                    <table width="100%">
                                        <tr>  
                                            <td style="width:80px;vertical-align:top;">Phone No. :</td> <td><asp:Label ID="lblcphoneno" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>  
                                            <td>Mobile No:</td> <td><asp:Label ID="lblFaxNo" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
               </tr>    
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" width="100%">
                            
                            <tr>
                                <td style="padding-top:7px;vertical-align:top;" id="td2" runat="server">
                                    <table width="100%" cellpadding="0" cellspacing="0" height="100%">
                                        <asp:Literal ID="ltcalllogdetail" runat="server"></asp:Literal>
                                    </table>
                               </td>
                            </tr>    
                            
                            <tr>
                                <td style="padding-top:30px;text-align:right;padding-right:10px;"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                
       </table>
       
       
    </form>
</body>
</html>
