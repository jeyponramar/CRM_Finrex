<%@ Page Language="C#" AutoEventWireup="true" CodeFile="advisorhome.aspx.cs" Inherits="advisorhome" %>
<%@ Register src="~/header.ascx" TagName="header" TagPrefix="uc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Finrex Treasury Advisors</title>
    <link href="css/common.css" rel="stylesheet" type="text/css" />
    <link href="css/admin.css" rel="stylesheet" type="text/css" />
    <link href="css/advisor.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />    
    <script src="js/constant.js" type="text/javascript"></script>
    <script src="js/jquery.min.js" type="text/javascript"></script>     
    <script src="js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="js/tab.js" type="text/javascript"></script>
    <script src="js/common.js" type="text/javascript"></script>
    <script src="js/global.js" type="text/javascript"></script>
    <script src="js/admin.js" type="text/javascript"></script>
   
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
          <td class="b-menu" id="header">
              <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td class="main-menu-bg">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td><img src="images/refux-logo.png" /></td>
                                <td style="padding-left:20px;">&nbsp;</td>
                                <td class="menu-sep"></td>
                                <td class="menu homemenu adminhome"><a href="advisorhome.aspx">Home</a></td>
                                <td class="menu-sep"></td>
                                <td class="menu">Cutomer Query</td>
                                <td class="menu-sep"></td>
                                <td class="menu"><a href="logout.aspx">Logout</a></td>
                                <td class="menu-sep"></td>
                                <td>&nbsp;</td>
                                <td><asp:Image ID="imgLoggedInUser" runat="server" Width="30" CssClass="imgborder"/></td>
                                <td style="color:#eee;font-family:Sans-Serif;font-weight:bold;font-size:15px;text-align:right;padding-left:10px" class="hand" align="right">
                                <asp:Label Text=""  runat="server" ID="lblLoginUserName"></asp:Label></td>            
                            </tr>
                         </table>
                         <div class='submenu hidden' style='position:absolute;left:100px;top:45px;' target='Cutomer Query'>
                            <table><tr><td style='padding:0px;width:250px;vertical-align:top;'>
                            <table cellspacing=0 cellpadding=0>
                            <tr><td class='lnksubmenu smenu-report' href='advisor/viewcustomerquery.aspx?t=1'>View Query</td></tr>
                            <tr><td class='lnksubmenu smenu-report' href='advisor/viewcustomerquery.aspx?t=2'>Open Queries</td></tr>
                            <tr><td class='lnksubmenu smenu-report' href='advisor/viewcustomerquery.aspx?t=3'>Waiting your response</td></tr>
                            <tr><td class='lnksubmenu smenu-report' href='advisor/viewcustomerquery.aspx?t=4'>Pending Queries</td></tr>
                            <tr><td class='lnksubmenu smenu-report' href='advisor/viewcustomerquery.aspx?t=5'>Closed Queries</td></tr>
                            </table>
                            </td>
                            </tr></table>
                         </div>
                    </td>
                </tr>
                </table>
          </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td class="tab-bar">
                            <div class="left tabpage" id="tab-1" title="Home">
                            <div class="tab-sep">&nbsp;</div><div class="tab-hover t" p="0">
                            <div class="tpage">Home</div></div></div>
                        </td>
                        <asp:Literal ID="ltMultiTabFunction" runat="server"></asp:Literal>
                    </tr>
                </table>
            </td>
            
        </tr>
        <tr><td id="inner" class="valign" style="background-color:#fff;">
            <div class="tab-content" id="page-1">
                <iframe id="frame-1" src="advisor/advisor-dashboard.aspx" width="100%" class="tab-if" scrolling="auto" style="border:none;margin:0px;padding:0px;"></iframe>
            </div>
        </td></tr>
        
        <tr>
            <td style="background-color:#d1cfcf;width:100%;vertical-align:top;">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="border-bottom:1px solid #f9f7f7;height:1px;" colspan="2"></td>
                    </tr>
                    
                    <tr><td class="b-menu" colspan="2">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <%--<tr>
                                <td style="padding-left:10px;"><a href="http://www.refuxsoftwareservices.com" target="_blank"><img height="25" src="images/refux-logo.png" /></a></td>
                                <td align="right">
                                    <table>
                                        <tr>
                                            <td class='pb'>Powered by : <a href="http://www.refuxsoftwareservices.com" target="_blank"  class='refux'>Refux Software Services</a></td>
                                            <td class='pb'>RPLUS CRM Verion 5.1</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                        </table>
                    </td></tr>
                </table>
            </td>
        </tr>
        
    </table> 
    </form>
</body>
</html>
