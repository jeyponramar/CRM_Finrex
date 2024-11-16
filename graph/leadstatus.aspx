<%@ Page Language="C#" AutoEventWireup="true" CodeFile="leadstatus.aspx.cs" Inherits="graph_leadstatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="../js/jsapi.js"></script>
</head>
<body style="margin:0px">
    <form id="form1" runat="server">
    <!--Div that will hold the pie chart-->
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td><div id="chart_div" style="margin:0px"></div></td>
        </tr>
        <%--<tr>
            <td style="background-color:#f5f5f5;padding-left:20px;font-size:small;">
                <table width="100%" cellpadding="5" cellspacing="0">
                    <asp:Literal ID="ltbindstatus" runat="server"></asp:Literal>
                </table>
            </td>
        </tr>--%>
    </table>
    
    </form>
</body>
</html>
