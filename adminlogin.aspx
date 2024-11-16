<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="adminlogin.aspx.cs" Inherits="configure_login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Finrex Treasury Advisors</title>
    <link href="css/common.css" rel="stylesheet" type="text/css" />
    <link href="css/admin.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image:url(images/banner/login.jpg);background-repeat:repeat;background-position:center center;background-color:#ffffff;">
    <form id="form1" runat="server">
    
<table width="100%" cellpadding="0" cellspacing="0">
    <tr><td style="padding-top:200px;">
        <table width="450" align="center" cellpadding="0" cellspacing="0">
            <tr>
            <td align="center" class="login">
                <table cellpadding="0" cellspacing="0" width="100%" id="tblLogin" runat="server">
                    <tr><td colspan="2" class="login-title">LOGIN - <asp:Label ID="lblCompanyName" runat="server"></asp:Label></td></tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr><td align="center"><asp:Label runat="server" Visible="false" ID="lblwarningmsg" CssClass="error" Font-Size="19px" Text="" ></asp:Label> </td></tr>
                    <tr>
                        <td class="login-form">
                            <table align="center" cellspacing="5">
                                <tr><td colspan="2" class="error"><asp:Label ID="lblMessage" runat="server"></asp:Label></td></tr>
                                <tr><td style='color:#000000;width:100px;'>User Name</td><td><asp:TextBox style="height:25px;" Width="200" id="txtUserName" runat="server" CssClass="textbox"></asp:TextBox></td></tr>
                                <tr><td style='color:#000000'>Password</td><td><asp:TextBox style="height:25px;" width="200" id="txtPassword" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox></td></tr>
                                <tr><td>&nbsp;</td></tr>
                                <tr><td></td><td><asp:Button ID="btnLogin" Text="Login" runat="server" OnClick="btnLogin_Click" CssClass="button" Width="90" Height="40"/></td></tr>
                                       
                            </table>
                        </td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>     
                </table>
                <table cellpadding="0" cellspacing="0" width="100%" id="tblChangePassword" runat="server" visible="false">
                    <tr><td colspan="2" class="login-title">Change Password</td></tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr>
                        <td class="login-form">
                            <table align="center" cellspacing="5">
                                <tr><td colspan="2" class="error"><asp:Label ID="lblMessage2" runat="server"></asp:Label></td></tr>
                                <tr><td style='color:#000000;width:100px;'>New Password</td><td><asp:TextBox style="height:25px;" Width="200" id="txtNewPassword" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox></td></tr>
                                <tr><td style='color:#000000'>Confirm Password</td><td><asp:TextBox style="height:25px;" width="200" id="txtConfirmPassword" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox></td></tr>
                                <tr><td>&nbsp;</td></tr>
                                <tr><td></td><td><asp:Button ID="btlChangePassword" Text="Change Password" runat="server" OnClick="btnChangePassword_Click" CssClass="button" Height="40"/></td></tr>
                                       
                            </table>
                        </td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>     
                </table>
            </td></tr>
        </table>
        
    </td></tr>
</table>
    </form>
</body>
</html>

