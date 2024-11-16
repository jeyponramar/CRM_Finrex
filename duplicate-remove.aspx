<%@ Page Language="C#" AutoEventWireup="true" CodeFile="duplicate-remove.aspx.cs" Inherits="fixduplicate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remove Duplicates</title>
    <script src="js/jquery.min.js" type="text/javascript"></script>    

    <link href="css/common.css" rel="stylesheet" type="text/css" />
    <link href="css/admin.css" rel="stylesheet" type="text/css" />
    <script>
        $(document).ready(function() {
            $(".btnremoveduplicate").click(function() {
                return confirm("Are you sure you want to remove duplicates from selected modules?\n\nOnce it is deleted you can not rollback");
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
            <td>Duplicate Module</td>
            <td><asp:TextBox ID="txtModule" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Duplicate Column Name</td>
            <td><asp:TextBox ID="txtColumnName" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="button"/></td>
        </tr>
        <tr>
            <td colspan="2">This column found in below module</td>
        </tr>
        <tr><td colspan="2">
            <asp:Literal ID="ltDuplicateTables" runat="server"></asp:Literal>
        </td></tr>
        
        <tr>
            <td colspan="2">Duplicate items</td>
        </tr>
        <tr><td colspan="2">
            <asp:Literal ID="ltDuplicateItems" runat="server"></asp:Literal>
        </td></tr>
        <tr>
            <td>Remove Duplicates from </td>
            <td><asp:TextBox ID="txtRemoveDuplicateModule" runat="server" Width="600"></asp:TextBox>
            <span class="error">Remove from here if you do not want to consider any module)</span></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:Button ID="btnRemoveDuplicates" runat="server" Text="Remove Duplicates" OnClick="btnRemoveDuplicates_Click" CssClass="button btnred btnremoveduplicate"/></td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
