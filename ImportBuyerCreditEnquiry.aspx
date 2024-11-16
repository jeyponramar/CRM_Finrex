<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportBuyerCreditEnquiry.aspx.cs" Inherits="ImportBuyerCreditEnquiry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Import Finance</title>
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="900">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="5" width="100%">
                    <tr>
                        <td colspan="2" style="padding-top:5px;padding-bottom:5px;font-weight:bold;">Importer Applicant Details</td>
                    </tr>
                    <tr>
                        <td style="width:150px;">Importer Firm Name <span style="color:#ff0000;">*</span></td>
                        <td>
                            <asp:TextBox ID="txtimporterfirmname" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="txtimporterfirmname" Text="Enter Importer firm Name"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:150px;">Contact Person <span style="color:#ff0000;">*</span></td>
                        <td><asp:TextBox ID="txtbuyercontactperson" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width:150px;">Email<span style="color:#ff0000;">*</span></td>
                        <td><asp:TextBox ID="txtbuyereemail" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width:150px;">Mobile Number <span style="color:#ff0000;">*</span></td>
                        <td><asp:TextBox ID="txtbuyermobilenumber" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-top:5px;padding-bottom:5px;font-weight:bold;">Exporter  Details</td>
                    </tr>
                    <tr>
                        <td style="width:150px;">Beneficiary Name (Supplier) <span style="color:#ff0000;">*</span></td>
                        <td><asp:TextBox ID="txtbuyerbeneficiarynamesupplier" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width:150px;">Exporter Country</td>
                        <td><asp:TextBox ID="txtbuyerexportercountry" runat="server"></asp:TextBox></td>
                    </tr>
                    
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
