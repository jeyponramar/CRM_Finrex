<%@ Page Language="C#" AutoEventWireup="true" CodeFile="portal.aspx.cs" Inherits="exportexposureportal_portal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../js/constant.js" type="text/javascript"></script>
    <link href="../css/common.css" rel="stylesheet" type="text/css" />
    <link href="../css/admin.css" rel="stylesheet" type="text/css" />
    <link href="../css/popbox.css" rel="stylesheet" type="text/css" /> 
    <link href="../css/print.css" media="print" rel="stylesheet" type="text/css" />
    <link href="../css/jquery-ui.css" rel="stylesheet" type="text/css" />    
    <link href="../css/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />    
    <link href="../js/upload/jquery.fileupload-ui.css" rel="stylesheet" type="text/css" />    
    <link href="../js/htmleditor/htmleditor.css" rel="stylesheet" type="text/css" />
    <link href="../js/htmleditor/jquery.contextMenu.css" rel="stylesheet" type="text/css" />
    <link href="css/common.css" rel="stylesheet" type="text/css" />
    
    <script src="../js/jquery.min.js" type="text/javascript"></script>    
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/global.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/custom.js" type="text/javascript"></script>
    <script src="../js/tab.js" type="text/javascript"></script>
    <script src="../js/validate.js" type="text/javascript"></script>
    <script src="../js/sgrid.js" type="text/javascript"></script>
    
    <script src="../js/jquery.corner.js" type="text/javascript"></script>    
	<link rel="stylesheet" type="text/css" href="../css/jquery.autocomplete.css" />    
	<script type="text/javascript" src="../js/jquery.autocomplete.js"></script>
	<script type="text/javascript" src="../js/jquery.datetimepicker.js"></script>
	
	<script src="../js/htmleditor/htmleditor.js"></script>
	
    <script src="../js/upload/jquery.fileupload.js"></script>
    <script src="../js/upload/jquery.fileupload-ui.js"></script>
    <script src="../js/upload/multifileupload.js"></script>
    
    <script type="text/javascript" src="../js/jsapi.js"></script>
    <script src="../js/colResizable-1.3.min.js"></script>

    <link href="../css/colorpicker/colorpicker.css" rel="stylesheet" type="text/css"/>
    <script type="text/javascript" src="../js/colorpicker/colorpicker.js"></script>
    <script type="text/javascript" src="../js/colorpicker/eye.js"></script>
    <script type="text/javascript" src="../js/colorpicker/utils.js"></script>
    <script type="text/javascript" src="../js/colorpicker/layout.js?ver=1.0.2"></script>        <script type="text/javascript" src="../js/admin.js"></script>    <script type="text/javascript"  src="../js/note.js"></script>
    <link href="../js/tour/joyride-2.1.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/tour/jquery.joyride-2.1.js"></script>
    <script language="javascript">
        var _menuleft = 0;
        var _menutop = 0;
        var _addmenu;
        var _targetQAddl = null;
        var _viewmode;
        $(document).ready(function() {
            $(".g-close").click(function() {
                window.close();
            });
            $.ajax({
                url: "exportexposureschedular.ashx",
                isAsync: true,
                success: function() {
                }
            });
        });
    </script>
    
<script>
    $(document).ready(function() {
        $(".g-menu-border").css("width", $(".g-menu:first").width() + 60);
        var wh = $(window).height() - $(".g-header").height() - 25;
        $("#ifr-dashboard").css("height", wh);
        $(".g-menu").click(function() {
            $(".g-menu").removeClass("g-menu-hover");
            $(this).addClass("g-menu-hover");
            $(".g-menu-border").css("width", $(this).width());
            $(".g-menu-border").css("width", $(this).width() + 60);
            var l = $(this).position().left;
            var cl = $(".g-menu-border").css("left").replace("px", "");
            if (l > cl) {
                l = l - cl;
                $(".g-menu-border").animate({ left: "+=" + l }, 300);
            }
            else {
                l = cl - l;
                $(".g-menu-border").animate({ left: "-=" + l }, 300);
            }
            openTab($(this));
        });
        $(".g-moreoptions").click(function() {
            $(".g-moremenu").css("top", $(this).position().top + 40);
            $(".g-moremenu").css("left", $(this).position().left - 160);
            var h = $(".g-moremenu").height() + 40;
            $(".g-moremenu").show();
            $(".g-moremenu").animate({ top: "-=" + h }, 500);
            $(".fade-div").show();
        });
        $(".fade-div").click(function() {
            var h = $(".g-moremenu").height() + 40;
            $(".g-moremenu").animate({ top: "+=" + h }, 300, function() { $(".fade-div").hide(); $(".g-moremenu").hide(); });

        });
        function openTab(obj) {
            var ifrid = obj.text().toLowerCase().replace(/ /g, "");
            $("#ifrpanel").find("iframe").hide();
            if ($("#ifrpanel").find("#ifr-" + ifrid).length == 0) {
                $(".loader").show();
                var h = $(window).height() - $(".g-header").height() - 25;
                var newpage = "<iframe src='" + obj.attr("href") + "' scrolling='no' style='border:solid 0px;width:100%;height:" + h + "px;' id='ifr-" + ifrid + "'></iframe>";
                $("#ifrpanel").append(newpage);
            }
            else {
                $(".loader").show();
                $("#ifrpanel").find("#ifr-" + ifrid).show();
                $("#ifrpanel").find("#ifr-" + ifrid).attr("src", $("#ifr-" + ifrid).attr("src"));
                hideLoader();
            }

        }
    });
    function hideLoader() {
        $(".loader").hide();
        //$("#ifrpanel").animate({"padding-top":"-=50"}, 500);
        $("#ifrpanel").hide();
        $("#ifrpanel").fadeIn(1000);
    }
</script>

</head>
<body>
    <form id="form1" runat="server">
<div class="loader"></div>
<div class="fade-div"></div>
<%--<div class="g-moremenu">
    <ul>
        <li>Dashboard</li>
        <li>Export Order</li>
        <li>Forward Contract</li>
        <li>PCFC</li>
        <li>Bank</li>
        <li>Settings</li>
    </ul>
</div>--%>
<table width="100%" cellspacing="0">
    <tr>
        <td class="g-header">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td><asp:Label ID="lblCompanyName" runat="server" CssClass="g-title"></asp:Label></td>
                    <%--<td><asp:LinkButton ID="lnkswitchportal" runat="server" OnClick="lnkswitchportal_Click" style="color:#fff;"></asp:LinkButton></td>--%>
                    <td style="color:#32347e;font-size:18px;width:210px;"><asp:Label ID="lbltitle" runat="server" Text="Finrex Exposure Manager"></asp:Label></td>
                    <td><asp:DropDownList ID="ddlportaltype" runat="server" CssClass="ddl" style="background: rgb(2, 136, 229);color: #fff;" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlportaltype_Changed">
                    <asp:ListItem Value="1" Text="Export"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Import"></asp:ListItem>
                    </asp:DropDownList>
                    <span style="color:#32347e;font-size:18px;"><asp:Label ID="lblportaltypetitle" runat="server" Visible="false"></asp:Label></span>
                    </td>
                    <td align="right" style="color:#f5f5f5;">Welcome <asp:Label ID="lblusername" runat="server" style="color:#fff;"></asp:Label> !</td>
                    <td class="g-close" align="right">
                        <div><img src="../images/g-close.png" width="25"/></div>
                    </td>
                </tr>
                <tr>
                    <td colspan="10" style="padding-top:0px;vertical-align:bottom;">
                        <table align="center" width="900" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <div style="width:100%;position:relative;" id="divexportmenu" runat="server" visible="false">
                                        <div class="g-menu g-menu-hover" href="portal-dashboard.aspx?pt=1">Dashboard</div>
                                        <div class="g-menu lnkcases" href="bagridview.aspx?m=exportorder">Export Order</div>
                                        <div class="g-menu lnknewcases" href="bagridview.aspx?m=forwardcontract">Forward Contract</div>
                                        <div class="g-menu" href="bagridview.aspx?m=pcfc">PCFC</div>
                                        <div class="g-menu" href="bagridview.aspx?m=femepc">EPC</div>
                                        <div class="g-menu" href="exportexposuresettings.aspx">Settings</div>
                                        <div class="g-menu-border"></div>
                                    </div>
                                    <div style="width:100%;position:relative;" id="divimportmenu" runat="server" visible="false">
                                        <div class="g-menu g-menu-hover" href="portal-dashboard.aspx?pt=2">Dashboard</div>
                                        <div class="g-menu lnkcases" href="bagridview.aspx?m=fimimportorder">Import Order</div>
                                        <div class="g-menu lnknewcases" href="bagridview.aspx?m=fimtradecredit">Trade Credit</div>
                                        <div class="g-menu" href="bagridview.aspx?m=fimforwardcontract">Forward Contract</div>
                                        <div class="g-menu" href="importexposuresettings.aspx">Settings</div>
                                        <div class="g-menu-border"></div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td style="padding-top:10px;">
            <table align="center" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="vertical-align:text-bottom;padding-top:0px;" align="center" id="ifrpanel">
                        <asp:Literal ID="ltdashboardlink" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
    </form>
</body>
</html>
