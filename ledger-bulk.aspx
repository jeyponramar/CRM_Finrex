<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ledger-bulk.aspx.cs" Inherits="ledger_bulk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bulk Ledger Creation</title>
    <script src="js/jquery.min.js" type="text/javascript"></script>    

    <link href="css/common.css" rel="stylesheet" type="text/css" />
    <link href="css/admin.css" rel="stylesheet" type="text/css" />
    <script>
        $(document).ready(function() {
            $(".module").blur(function() {
                $(".ledgername").val($(this).val() + "_");
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
        <tr><td colspan="2"><asp:Label ID="lblMessage" CssClass="error" runat="server"></asp:Label></td></tr>
        <tr>
            <td>Module</td>
            <td><asp:TextBox ID="txtModule" runat="server" CssClass="textbox module"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Ledger Group</td>
            <td><asp:DropDownList ID="ddlGroup" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Ledger Type</td>
            <td><asp:DropDownList ID="ddlLedgerType" runat="server">
                <asp:ListItem Text="Ledger" Value="0"></asp:ListItem>
                <asp:ListItem Text="Customer" Value="1"></asp:ListItem>
                <asp:ListItem Text="Vendor" Value="2"></asp:ListItem>
                <asp:ListItem Text="Employee" Value="3"></asp:ListItem>
                <asp:ListItem Text="Labour" Value="4"></asp:ListItem>
                <asp:ListItem Text="Expense" Value="5"></asp:ListItem>
                <asp:ListItem Text="Others" Value="6"></asp:ListItem>
                <asp:ListItem Text="Tax" Value="7"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Ledger Name Column</td>
            <td><asp:TextBox ID="txtLedgerName" CssClass="textbox ledgername" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="button"/></td>
        </tr>
        
    </table>
    </div>
    </form>
</body>
</html>
