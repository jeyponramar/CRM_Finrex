<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="forgot-password.aspx.cs" Inherits="forgot_password" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Finstation</title>
    <link href="css/common.css" rel="stylesheet" type="text/css" />
    <link href="css/admin.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image:url(images/ticker.jpg);background-position:center center;background-color:#ffffff;">
    <form id="form1" runat="server">
<table width="100%" cellspacing="0">
        <tr>
            <td style="padding-top:200px;">
                <table width="450" align="center" cellpadding="0" cellspacing="0" style="background-color:#fff;">
                    <tr><td class="login-title">FinStation -Forgot Password</td></tr>
                     <%--<tr><td align="center"><img src="images/finrex.png"/></td></tr>--%>
                     <tr>
                        <td class="form" colspan="2" style="padding:20px">
                            <table width="90%" cellpadding="5">
                            <tr>
                                <td align="center" colspan="4"> <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="error"></asp:Label></td>
                            </tr>
            					<tr><td colspan="2">Please mention your UserName to recover your password.  </td></tr>
					            <tr>
						            <td align="right" class="label">Enter Your UserName: &nbsp;&nbsp;</td>
                                    <td><asp:TextBox ID="txtUserName" runat="server" CssClass="textbox" />
                                        
                                    </td>
					            </tr>
		                        <tr><td></td>    
                                    <td>
                                        <asp:Button ID="Button1" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="button" ValidationGroup="vg"/>
                                        
                                    </td>
                                </tr>
                                <tr><td></td>    
                                    <td>
                                        <a href="customerlogin.aspx">Back to Login</a>
                                        
                                    </td>
                                </tr>
                            </table>
                        </td>
                     </tr>
                </table>
            </td>
        </tr>
		 
        <tr><td style="height:100px">&nbsp;</td></tr>
    </table>
   </form>
</body>
</html>
