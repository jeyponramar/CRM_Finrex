<%@ Page Language="C#" AutoEventWireup="true" CodeFile="query.aspx.cs" Inherits="query" EnableEventValidation="false" ValidateRequest="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblError" runat="server" ForeColor='Red'></asp:Label><br />
        <asp:TextBox ID="txtPassword" runat="server" Width="800" TextMode="Password"></asp:TextBox>
        <br />
        <asp:TextBox ID="txtQuery" runat="server" TextMode="MultiLine" Width="800" Height="550"></asp:TextBox>
        <br />
        <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Execute"/>
        &nbsp;&nbsp;<asp:Button ID="btnSelect" runat="server" OnClick="btnSelect_Click" Text="Select"/>
        <br />
        <asp:GridView ID="gv" runat="server"></asp:GridView>
    </div>
    </form>
</body>
</html>
