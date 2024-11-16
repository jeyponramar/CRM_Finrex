<%@ Page Language="C#" AutoEventWireup="true" CodeFile="error.aspx.cs" Inherits="error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Error</title>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%">
     <tr>
        <td class="title">
            <asp:Label ID="lblPageTitle" runat="server" Text="Error"/>
        </td>
     </tr>
     <tr>
        <td class="error">
            <asp:Label ID="lblErrorMessage" runat="server"/>
        </td>
     </tr>   
    </table>
</form>
</body>
</html>


