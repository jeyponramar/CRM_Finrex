<%@ Page Language="C#" AutoEventWireup="true" CodeFile="generatequote.aspx.cs" Inherits="generatequote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:FileUpload ID="flLayout" runat="server" />
    </div>
    <br />
    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Submit"/>
    <br />
    <asp:Literal ID="ltData" runat="server"></asp:Literal>
    </form>
</body>
</html>
