<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="customerlogin.aspx.cs" Inherits="customerlogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Finstation</title>
    <%--<link href="css/common.css" rel="stylesheet" type="text/css" />
    <link href="css/admin.css" rel="stylesheet" type="text/css" />--%>
    <script src="js/jquery.min.js" type="text/javascript"></script>    
    <style>
        body 
        {
	        font:normal 12px Arial,Helvetica,sans-serif !important;
	        margin:0px;
	        background-color:#f5f5f5;
        }

        .textbox
        {
            height:30px;
            background-color:#fff;
            color:#000;
            width:100%;
            outline: none;
            border-radius:5px;
            border:0px;
        }
        a
        {
        	color:#37C9F0;
        }
        .button
        {
        	background-color:#37C9F0;
        	border:solid 1px #18b6dd;
        	border-radius:3px;
        	color:#fff;
        	min-width: 125px;
            font-weight: bold;
            font-size: 18px;
            height:40px;
            cursor:pointer;
        }
        .label
        {
        	color:#4C6E8A;font-size:16px;
        }
        .login-title
        {
        	color:#fff;font-size:40px;padding-left:30px;padding-top:50px;
        }
        .error{color:#ff0000;font-size:14px;}
        .hidden{display:none;}
    </style>
    <script>
        $(document).ready(function(){
            //$(".jq-loginpanel").css("height",$(window).height());
            $(".jq-info2").fadeIn(2000);
            $(".jq-cname").animate({fontSize:50});
            //$(".jq-logo-line").animate({width:"100%"},500);
            $(".jq-login").keypress(function(e){
                //alert(e.keyCode);
            });
        });
    </script>
</head>
<body style="background-color:#fff;margin:80px 200px;margin-bottom:20px;">
    <form id="form1" runat="server">
<table width="100%">
    <tr>
        <td>
            <table width="900" cellpadding="0" cellspacing="0" border="0" align="center">
                <tr><td colspan="2" align="right" style="padding-bottom:10px;"><a href="https://finrex-in.bookmyforex.com/" target="_blank"><img src="images/fx-traveler.png" /></a></td></tr>
                <tr>
                    <td width="60%" style="vertical-align:top;background-color:#eee">
                        <table width="100%">
                            <tr>
                                <td style="padding-left: 20px;padding-top: 20px;background-color: #eee;padding-bottom: 20px;height:50px;">
                                    <img src="images/finrex_logo.png" style="vertical-align:top;" />
                                </td>
                            </tr>
                            <%--<tr><td><div class="jq-logo-line" style="background-color:#F0803D;height:2px;width:1px;"></div></td></tr>--%>
                            <tr>
                                <td style="text-align:center;font-size:20px;padding-top:50px;color:#32347e;font-weight:bold;height:70px;">
                                    <div class="jq-cname">FinStation</div>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:center;color:#32347e;padding-top:10px;font-size:14px;">
                                    Web Portal for Forex, Banking, EXIM, Treasury & Finance functions
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:center;font-size:20px;padding-top:50px;color:#f0803D;font-weight:bold;height:100px;">
                                    <div class="jq-info2 hidden">
                                        <span style="font-size:35px;">B</span>etter <span style="font-size:35px;">I</span>nformation, 
                                        <span style="font-size:35px;">S</span>marter <span style="font-size:35px;">N</span>egotiation
                                        <br />
                                        <span style="font-size:35px;">B</span>est <span style="font-size:35px;">S</span>aving
                                    </div>
                                </td>
                            </tr>
                           
                            <tr>
                                <td style="padding-top:50px;text-align:center;">
                                     <table width="100%">
                                            <tr>
                                                <td align="right" style="padding-right:13px;" colspan="2">
                                                    <table cellpadding=5>
                                                        <tr>
                                                            <td align="right" style="height:30px;">
                                                                <table>
                                                                    <tr>
                                                                        <td><img src="images/whatsapp.png" style="width:20px;" /></td>
                                                                        <td><a href="https://www.linkedin.com/in/finrex-treasury-research-6087ab137" target="_blank"><img src="images/linked.png" style="width:20px;" /></a></td>
                                                                        <td><a href="https://twitter.com/finrextreasury" target="_blank"><img src="images/twitter.png" style="width:20px;" /></a></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="vertical-align:bottom;">© 2014 Finrex All rights reserved.</td>
                                                <td style="color:#444444;text-align:right;padding-right:20px;">Email : <a href="mailto:info@finrex.in" style="color:rgb(5, 21, 64);text-decoration:none;">info@finrex.in</a></td>
                                            </tr>
                                     </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="40%" style="vertical-align:top;background-color:#051540;" class="jq-loginpanel" id="tdLogin" runat="server">
                         <table align="center" width="100%">
                            <tr><td class="login-title">Sign In</td></tr>
                            <tr>    
                                <td style="padding-left:30px;">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr><td>&nbsp;</td></tr>
                                        <tr><td align="center"><asp:Label runat="server" Visible="false" ID="lblwarningmsg" CssClass="error" ></asp:Label> </td></tr>
                                        <tr>
                                            <td class="login-form" style="padding-top:50px;">
                                                <table cellspacing="5" width="300" border=0>
                                                    <tr><td colspan="2" class="error"><asp:Label ID="lblMessage" runat="server"></asp:Label></td></tr>
                                                    <tr><td class="label" colspan="2">Email address </td></tr>
                                                    <tr><td colspan="2"><asp:TextBox id="txtUserName" runat="server" CssClass="textbox jq-login"></asp:TextBox></td></tr>
                                                    <tr><td class="label" colspan="2">Password</td></tr>
                                                    <tr><td colspan="2"><asp:TextBox id="txtPassword" runat="server" CssClass="textbox jq-login" TextMode="Password"></asp:TextBox></td></tr>
                                                    <tr><td>&nbsp;</td></tr>
                                                    <tr>
                                                        <td><asp:CheckBox ID="chkRememberMe" runat="server" Text="Remember me" style="color:#fff;font-size:14px;"/></td>
                                                        <td><asp:LinkButton ID="lnkforgotpassword" runat="server" OnClick="lnkforgotpassword_Click" style="font-size:14px;">Forgot Password</asp:LinkButton></td>
                                                    </tr>
                                                    <tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr>
                                                    <tr><td align="center" colspan="2"><asp:Button ID="btnLogin" Text="Login" runat="server" OnClick="btnLogin_Click" CssClass="button"/></td></tr>
                                                    <tr><td></td></tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr><td>&nbsp;</td></tr>     
                                        <tr><td>&nbsp;</td></tr>     
                                    </table>
                                </td>
                            </tr>
                           
                        </table>        
                    </td>
                    <td width="40%" style="vertical-align:top;background-color:#051540;" class="jq-loginpanel" id="tdchangepassword" runat="server" visible="false">
                         <table align="center" width="100%">
                            <tr><td class="login-title">Change Password</td></tr>
                            <tr>    
                                <td>
                                    <table align="center" cellspacing="5" width="100%" border=0>
                                        <tr><td colspan="2"><asp:Label ID="lblMessage2" runat="server" CssClass="error"></asp:Label></td></tr>
                                        <tr><td class="label" colspan="2">New Password</td></tr>
                                        <tr><td colspan="2"><asp:TextBox style="height:25px;" id="txtNewPassword" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox></td></tr>
                                        <tr><td class="label" colspan="2">Confirm Password</td></tr>
                                        <tr><td colspan="2"><asp:TextBox style="height:25px;" id="txtConfirmPassword" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox></td></tr>
                                        <tr><td>&nbsp;</td></tr>
                                        <tr><td colspan="2" align="center"><asp:Button ID="btnChangePassword" Text="Change Password" runat="server" OnClick="btnChangePassword_Click" CssClass="button" Width="200"/></td></tr>
                                    </table>
                                </td>
                            </tr>
                        </table>        
                    </td>
                    <td width="40%" style="vertical-align:top;background-color:#051540;" class="jq-loginpanel" id="tdforgotpassword" runat="server" visible="false">
                         <table align="center" width="100%">
                            <tr><td class="login-title">Forgot Password</td></tr>
                            <tr>    
                                <td>
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr><td>&nbsp;</td></tr>
                                        <tr><td align="center"><asp:Label runat="server" Visible="false" ID="lblMessage_fp" CssClass="error" ></asp:Label> </td></tr>
                                        <tr>
                                            <td class="login-form" style="padding-top:50px;padding-left:30px;">
                                                <table cellspacing="5" width="300" border=0>
                                                    <tr><td class="label" colspan="2">Email address </td></tr>
                                                    <tr><td colspan="2"><asp:TextBox id="txtemailid_fp" runat="server" CssClass="textbox"></asp:TextBox></td></tr>
                                                    <tr><td>&nbsp;</td></tr>
                                                    <tr>
                                                        <td><asp:LinkButton ID="lnkbacktologin" OnClick="lnkbacktologin_Click" style="font-size:14px;" runat="server">Back to Login</asp:LinkButton></td>
                                                    </tr>
                                                    <tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr>
                                                    <tr><td align="center" colspan="2"><asp:Button ID="btnForgotPassword" Text="Forgot Password" runat="server" OnClick="btnForgotPassword_Click" CssClass="button" Width="200"/></td></tr>
                                                    <tr><td></td></tr>
                                                           
                                                </table>
                                            </td>
                                        </tr>
                                        <tr><td>&nbsp;</td></tr>     
                                        <tr><td>&nbsp;</td></tr>     
                                    </table>
                                </td>
                            </tr>
                        </table>        
                    </td>
                    
                </tr>
            </table>
        </td>
    </tr>
</table>

    </form>
</body>
</html>

