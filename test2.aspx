<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test2.aspx.cs" Inherits="test2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <table cellpadding='10' cellspacing='5'>
                <tr>
                     <td class='label'>File Name:</td>
                     <td><asp:FileUpload ID="FileUpload1" runat="server" /></td>
                </tr>
                <tr>
                     <td></td>
                     <td><asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" /></td>
                </tr>
                <tr>
                <td colspan='2'><asp:Label ID="lblmessage" runat="server"></asp:Label></td>
                </tr>
         </table>
         
    </div>
    </form>
</body>
</html>
