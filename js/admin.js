var _customreport_data = null;
var _ismouseOnSubMenu = false;
var _KEY_ALT = false;
$(document).ready(function() {
    initSubMenu();
    initShareData();
    initQuickEdit();
    initGridResize();
    initAutoPopSamedata();
    setAcDefaultVal();
    initTour();
    //initInfoWindow();
    checkMaxInputLength();
    showSubmoduleCountActionButton();
    initActionConfirm();
    initFEMPortal();
    initAdvancedReport();
    initMultiCheckbox();
    initCustomerQuery();
    initServicePlan();
    initMultiSelectAc();
    //initialize checkbox
    //initCheckbox();
    $(".edit").click(function() {
        showSubmoduleCountActionButton();
    });
    $(".repeater-row,.repeater-alt").each(function() {
        initEditGrid($(this));
    });
    $(".circle").each(function() {
        //call after img load
        //        $(this).css("border-top-left-radius", $(this).width() / 2);
        //        $(this).css("border-top-right-radius", $(this).width() / 2);
        //        $(this).css("border-bottom-left-radius", $(this).height() / 2);
        //        $(this).css("border-bottom-right-radius", $(this).height() / 2);
    });
    if ($(".repeater").length > 0) $(".repeater").colResizable();
    if ($(".corner").length > 0) $(".corner").corner("5px");
    $(".custimize-report-list").live("click", function() {
        var isselected = ConvertToBool($(this).attr("isselected"));
        isselected = !isselected;
        $(this).attr("isselected", isselected);
        if (isselected) {
            var reportName = $(this).find(".rep-title").text();
            var img = $(this).find("img").attr("src");
            $(this).css("border-color", "#048aa2");
            $(this).css("background-color", "#ebebeb");
        }
        else {
            $(this).css("background-color", "#ffffff");
            $(this).css("border-color", "#f1f1f1");
            $(".custom-report-preview").hide();
        }
    });
    $(".report-configuration-search").keyup(function(e) {
        bindCustomReport();
    });

});
function openReportConfiguration() {
    $("#report-configuration").dialog({
        autoOpen: true,
        modal: true,
        width: $(window).width() - 200,
        height: $(window).height() - 100,
        show: {
            effect: "blind",
            duration: 500
        },
        hide: {
            effect: "explode",
            duration: 500
        },
        open: function(event, ui) {
            setTimeout(function() {
                _customreport_data = RequestData("../getdata.ashx?m=rightpanel-report");
                $(".ui-dialog-titlebar").addClass("custom-dialog-report-config-header");
                bindCustomReport();
            }, 3000);
        }
    });

}
function bindCustomReport() {
    var w = $(window).width() - 250;
    var h = $(window).height() - 210;
    var html = "<div style='width:" + w + "px;height:" + h + "px;overflow:scroll;'><table width='100%' cellspacing='10'>";
    var count = 0;
    var keyword = $(".report-configuration-search").val().toLowerCase();
    for (i = 0; i < _customreport_data.length; i++) {
        var reportTitle = _customreport_data[i].report_reportname;
        if (reportTitle.toLowerCase().indexOf(keyword) >= 0) {
            var reportId = _customreport_data[i].report_reportid;
            if (count % 5 == 0) {
                html += "<tr>";
            }
            html += "<td class='custimize-report-list' isselected='false' rid='" + reportId + "' align='center'><table><tr><td><div><img src='../images/reports/1.jpg' width='200'></div></td></tr>" +
                                "<tr><td class='rep-title' align='center'>" + reportTitle + "</td></tr></table></td>";
            if ((count + 1) % 5 == 0) {
                html += "</tr>";
            }
            count++;
        }
    }
    if (count == 0) {
        html += "<tr><td class='error'>No report found</td></tr>";
        $(".report-configuration-action").hide();
    }
    else {
        $(".report-configuration-action").show();
    }
    html += "</table></div>";
    $(".report-configuration-reports").html(html);
}
function initSubMenu() {
    $(".menu").click(function() {
        var left = $(this).position().left;
        var top = $(this).position().left;
        var submenu = null;
        var menu = $(this).text();
        $(".submenu").hide();
        $(".submenu").each(function() {
            if ($(this).attr("target") == menu) {
                submenu = $(this);
            }
        });
        if (submenu == null) return;
        //left = left - submenu.width() / 2 + $(this).width() / 2 + 30;
        left = left - 100;
        submenu.css("left", left);
        submenu.show();
        $(".menu").removeClass("menu-hover");
        $(this).addClass("menu-hover");
    });
    $(".menu").hover(function() {
//        var left = $(this).position().left;
//        var top = $(this).position().left;
//        var submenu = null;
//        var menu = $(this).text();
//        $(".submenu").hide();
//        $(".submenu").each(function() {
//            if ($(this).attr("target") == menu) {
//                submenu = $(this);
//            }
//        });
//        if (submenu == null) return;
//        //left = left - submenu.width() / 2 + $(this).width() / 2 + 30;
//        left = left - 100;
//        submenu.css("left", left);
//        submenu.show();
//        $(".menu").removeClass("menu-hover");
//        $(this).addClass("menu-hover");
    }, function(e) {
        if (e.pageY < 10) {
            $(".submenu").hide();
            highlightmenu();
        }
    });
    $(".submenu").hover(function() {
        _ismouseOnSubMenu = true;
    }, function(e) {
        if (e.pageY > 50) {
            $(this).hide();
            highlightmenu();
        }
    });
    $(window).click(function(e) {
        if (e.pageY > 50) {
            $(".submenu").hide();
            highlightmenu();
        }
    });
}
function initCheckbox() {
    var chkhtml = '<div style="width:70px;"><div class="toggle-light"><div class="toggle"></div></div></div>';
    $("input:checkbox").each(function() {
        var newcheckbox = $(chkhtml);
        newcheckbox.find('.toggle').toggles({ drag: false, width: 60, on: $(this).prop("checked") });
        $(this).closest("td").append(newcheckbox);
        $(this).hide();

    });
}
function highlightmenu() {
    $(".menu").removeClass("menu-hover");
    $(".menu").each(function() {
        if ($(this).text() == _selectedMenu) {
            $(this).addClass("menu-hover");
        }
    });
}

function shareViewData(data) {
    parent._shareViewData = data;
}
function initShareData() {
    var data = parent._shareViewData;
    if (data != "") {
        var m = getSetting(data, "m");
        var grid = $(".repeater:first");
        var targetm = grid.closest(".gridtd").attr("m");
        if (m == targetm) {
            updateViewPageGrid(grid, data);
            parent._shareViewData = "";
        }
    }
    setTimeout("initShareData()", 1000);
}
function updateViewPageGrid(grid,data) {
    var id = getSetting(data, "id");
    var targetCols = "";
    var isadd = true;
    var tdHtml = "";
    var isfirstCol = false;
    var updateRow = null;
    grid.find(".repeater-header").find("td").each(function() {
        var htext = $(this).text();
        if (htext == "Edit") {
            tdHtml += '<td class="gedit">&nbsp;</td>';
        }
        else {
            var dcolName = $(this).find("a").attr("col");
            var dval = getSetting(data, dcolName);

            if ($(this).find("a").attr("img") != undefined) {
                if (dval == "") dval = "jpg";
                dval = "<img height='30' src='../img.ashx?url=" + $(this).find("a").attr("img") + "/" + id + "." + dval + "'/>";
            }
            if (dval == "") {
                dval = "&nbsp;";
            }
            var attr = "";
            if (!isfirstCol) {
                attr = " class='idval' idval='" + id + "'";
            }

            tdHtml += "<td" + attr + ">" + dval + "</td>";
            isfirstCol = true;
        }
    });

    var found = false;
    grid.find("tr").each(function() {
        if (!found) {
            var tdval = $(this).find(".idval");
            if (tdval.length > 0) {
                var idval = tdval.attr("idval");
                if (idval == id) {
                    isadd = false;
                    updateRow = $(this);
                    found = true;
                }
            }
        }
    });
    if (isadd) {
        var rowHtml = "<tr class='repeater-row' style='background-color:#8efaa8;'>" + tdHtml + "</tr>";
        var newrow = $(rowHtml);
        newrow.insertBefore(grid.find(".repeater-header").next());
        initEditGrid(newrow);
    }
    else {
        updateRow.html(tdHtml);
        updateRow.css("background-color", "#8efaa8");
        initEditGrid(updateRow);
    }
}
function getSetting(setting, name) {
    var arr = setting.split('~');
    var i = 0;
    while (i < arr.length - 1) {
        if (name == arr[i]) {
            return arr[i + 1];
        }
        i += 2;
    }
    return "";
}
function closeQuickAdd(targettxt, targethdn, txtval, hdnval) {
    if (targethdn == "undefined") {
        //for dropdown
        var idexists = false;
        var selectedOption = null;
        $("#" + targettxt, window.opener.document).find("option").each(function() {
            if ($(this).attr("value") == hdnval) {
                idexists = true;
                selectedOption = $(this);
            }
        });
        if (idexists) {
            selectedOption.attr("value", hdnval);
            selectedOption.text(txtval);
        }
        else
        {
            $("#" + targettxt, window.opener.document).append("<option selected value='" + hdnval + "'>" + txtval + "</option>");
        }
        var nm = $("#" + targettxt, window.opener.document).attr("name");
        var clientid = $("#" + targettxt, window.opener.document).attr("id");
        
        if ($("#" + targettxt, window.opener.document).parent().find(".hdnddlqa").length == 0)
        {
            var hdnddl = $("<input type='hidden' value='" + hdnval + "' name='" + nm + "_qa' class='hdnddlqa'/>" +
                           "<input type='hidden' value='" + txtval + "' name='" + clientid + "_txtddlqa' class='txtddlqa'/>");
            $("#" + targettxt, window.opener.document).parent().append(hdnddl);
        }
        else {
            $("#" + targettxt, window.opener.document).parent().find(".hdnddlqa").val(hdnval);
            $("#" + targettxt, window.opener.document).parent().find(".txtddlqa").val(txtval);
        }
    }
    else {
        $("#" + targettxt, window.opener.document).val(txtval);
        $("#" + targethdn, window.opener.document).val(hdnval);
    }
    window.close();
}
function initQuickEdit() {
    $(".quickedit-close").click(function() {
        $(".tdrightpanel").hide();
        $(".tddatapanel").fadeTo("slow", 1);
    });
}
function loadQuickEdit(m, id, cols) {
    $(".tdrightpanel").addClass("rightpanel-quickedit");
    $(".tdrightpanel").css("width", $(window).width() / 2);
    $(".tdrightpanel-inner").css("height", $(window).height() - 100);
    $(".ifrRightPanel").css("height", $(window).height() - 120);
    $(".tdrightpanel").show();
    $(".tddatapanel").fadeTo("slow", 0.8);
    $(".rightpanel-quickedit").css("top", $(document).scrollTop() + 20);
    $("#ifrRightPanel").attr("src", "../" + m + "/add.aspx?qe=1&id=" + id + "&vc=" + cols);
}
function closeQuickEdit(viewdata) {
    var arrdata = viewdata.split('~');
    var index = 0;
    try {
        _selectedRow_QE.find("td").each(function() {
            var coldata = arrdata[index].split('`');
            var colname = coldata[0];
            if (colname != "") {
                var val = coldata[1];
                val = val.replace(/_NEWLINE_/g, "\n");
                $(this).text(val);
            }
            index++;
        });
        _selectedRow_QE.css("background-color", "#caffdd");
    } catch (e) { }
    $(".tdrightpanel").hide();
    $(".tddatapanel").fadeTo("slow", 1);
}
function initGridResize() {
    if ($(".repeater") != undefined && $(".repeater").length > 0) {
        $(".repeater").colResizable();
    }
}
function initAutoPopSamedata() {
    $(".popsamedata").each(function() {
        $(this).blur(function() {
            if ($(this).val().trim() != "") {
                $("." + $(this).attr("popsamedata_target")).val($(this).val());
            }
        });
    });
}
function setAcDefaultVal() {
    $(".ac").each(function() {
        $(this).attr("acval", $(this).val());
    });
}
function initEditGrid(row) {
    //remove existing event if any
    row.find("td").unbind("click");
    row.find("td").click(function() {
        if($(this).find("a").length>0)return;
        var tr = $(this).closest("tr");
        //        if ($(this).hasClass("noedit") || $(this).find(".noedit").length > 0) {
        //            return false;
        //        }
        if ($(this).find(".jq-viewfemportal").length > 0) {
            window.open('../exportportal.aspx?ia=true&sid=' + $(this).closest("tr").find(".idval").attr("idval"));
            return false;
        }
        if (tr.attr("class").indexOf("noedit") >= 0 || !_iseditlink) return;
        var grid = tr.closest(".gridtd");
        var m = grid.attr("m");
        if (grid.attr("enableedit") == "false") {
            return false;
        }
        grid = tr.closest(".repeater");
        if (grid.attr("enableedit") == "false") {
            return false;
        }
        var id = tr.find(".idval").attr("idval");
        if (id == undefined) return;

        if ($(this).attr("class") != undefined) {
            if ($(this).attr("class").indexOf("gedit") >= 0) {
                var cols = "";
                $(this).closest(".repeater").find(".repeater-header").find("td").each(function() {
                    var a = $(this).find("a").attr("col");
                    if (cols == "") {
                        cols = a;
                    }
                    else {
                        cols += "," + a;
                    }
                });
                _selectedRow_QE = $(this).closest("tr");

                loadQuickEdit(m, id, cols);
                return;
            }
        }
        if ($(this).find(".gedit") != null) {
            if ($(this).find(".gedit").attr(".gedit") == "true") {
                return;
            }
        }
        var cls = $(this).attr("class") + "";
        if (cls != "") {
            if (cls.indexOf(" spage") >= 0) return;
        }

        var url = "#/" + m + "/a/add/id/" + id;
        if(m == "client" && window.location.href.indexOf("advancedreportkyc.aspx")>0)
        {
            url = "client/updateclientkycdetail.aspx?id="+id;
        }
        if (QueryString("notab") == "true") {
            window.location = "../" + m + "/add.aspx?id=" + id;
            return;
        }
        loadPage(url, "Edit " + m);
    });
}
function shareQeData(data) {
    var grid = $(".repeater:first");
    updateViewPageGrid(grid, data);
    $(".tdrightpanel").hide();
    $(".tddatapanel").fadeTo("slow", 1);
}
function initTour() {
    $(".btntour").click(function() {
        startTour();
    });
    if (QueryString("tour") == "true") {
        startTour();
    }
}
function startTour() {
    if ($('#tour').length == 0) return;
    $('#tour').joyride({
        autoStart: true,
        postStepCallback: function(index, tip) {
            var h4 = tip.find("h4");
            var url = h4.attr("url");
            if (url != undefined) {
                loadPage(url, h4.text());
            }
            if (h4.attr("last") == "true") {
                $(this).joyride('destroy');
            }
        },
        modal: true,
        expose: true
    });
}
function initInfoWindow() {
    var html = '<div class="info-window"><table width="100%" cellpadding="0" cellspacing="0"><tr><td></td><td align="right" class="info-window-close">X</td></tr><tr><td class="info-window-msg"></td></tr></table></div>';
    if ($(".info-window").length == 0) {
        $("body").append($(html));
    }
    $(".ac").focus(function() {
        showInfoWindow('ac');
    });
    $(".qa,.qa-e,.qa-new").mouseenter(function() {
        showInfoWindow($(this).attr("class"));
    });
    $(".addmenudiv").mouseleave(function() {
        hideInfoWindow();
    });
    $(".grid").find("input").focus(function() {
        showInfoWindow('grid');
    });
    $(".grid").find("input").blur(function() {
        hideInfoWindow();
    });
    $(".info-window-close").click(function() {
        $(".info-window").hide();
    });
    $(".heditor").focus(function() {
        showInfoWindow("html");
    });
    $(".ac,.heditor").blur(function() {
        hideInfoWindow();
    });
//    $(".label").each(function() {
//        var err = $(this).find(".error");
//        if (err.length > 0 && err.text() == "*") {
//            var txt = $(this).closest("tr").find("input");
//            if (txt.length > 0) {
//                txt.focus(function() {
//                    showInfoWindow("required");
//                });
//            }
//        }
//    });
}
function showInfoWindow(type) {
    var html = "";
    if (type == "ac") {
        html = "<b>Auto Complete / Quick Add</b><br/><br/>Just click and type starting 2-3characters, data will get automatically listed. You can select any data listed in the suggestion list.<br/>" +
               "If the data is not available you can add the master data using Quick Add option(Mouseover on nearest down arrow)";
    }
    else if (type == "qa") {
        html = "<b>Quick Add</b><br/><br/>Type the data in the text box and click on Quick Add to add the master data directly without going to master!";
    }
    else if (type == "qa-e") {
        html = "<b>Quick Edit</b><br/><br/>You can modify the master data if anything entered wrong or if you want to add more detail in the !";
    }
    else if (type == "qa-new") {
        html = "<b>Quick Add</b><br/><br/>Create master data without closing the current page. This will open a popup window where you can add the master!";
    }
    else if (type == "grid") {
        html = '<b>How to add multiple entries in Sub Grid</b>' +
               '<p>Enter required detail in the grid row. Then Press "ENTER" to add multiple entries.! If you want to add TAX, type TAX in product and Amount will be automatically calculated.</p>' +
               '<p><div class="enterkey" /></p>';
    }
    else if (type == "html") {
        html = "<b>HTML Editor</b><br/><br/>You can type the text in html format. You can set text color, background color, images, table etc!";
    }
    else if (type == "required") {
        html = "<b>Required Field</b><br/><br/>This is a mandatory field, you can not leave it blank!";
    }
    if (html != "") $(".info-window-msg").html(html);
    $(".info-window").show();
}
function hideInfoWindow() {
    $(".info-window").hide(200);
}
function checkBrowser() {
    if ($.browser.msie) {
        if ($("#browser-support").length == 0) {
            var html = '<div id="browser-support" title="Browser Compatibility"><table><tr><td class="error bold">You are using unsupported browser!</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td>Advanced features may not be supported by this browser, please install chrome for better user experience and performance.</td></tr><tr><td>&nbsp;</td></tr><tr><td><table><tr><td><a href="https://www.google.com/intl/en/chrome/browser/promo/gmail/?brand=CHHM"><img src="images/chrome.png" /></a></td><td style="padding-left:20px;">Install Google Chrome for free</td><td><a href="https://www.google.com/intl/en/chrome/browser/promo/gmail/?brand=CHHM" style="font-size:16px;color:#1868c2">Install Now</a></td></tr></table></td></tr></table></div>';
            $('body').append($(html));
        }
        $("#browser-support").dialog({
            autoOpen: true,
            modal: true,
            width: 500,
            height: 250,
            open: function(event, ui) {
                $(".ui-dialog-titlebar-close").hide();
            },
            show: {
                effect: "blind",
                duration: 500
            },
            hide: {
                effect: "blind",
                duration: 500
            }
        });
    }
}
function checkMaxInputLength() {
    $("input,textarea").keypress(function(e) {
        try {
            var maxLength = ConvertToInt($(this).attr("maxlength"));
            if (maxLength > 0 && $(this).val().length == maxLength) {
                alert("Max number of characters exeeded!\n\nNo more text allowed for this field!");
            }
        } catch (ex) { }
    });
    $("textarea").each(function(e) {
        var ml = ConvertToInt($(this).attr("ml"));
        if (ml > 0) $(this).attr("maxlength", ml);
    });
}
function enableDisableTab() {
    $(".tab-multi").click(function() {
        var istabEnabled = IsTabEnabled();
        if (istabEnabled) {
            $(this).removeClass("enable-tab");
            $(this).addClass("disable-tab");
            if (RequestData("utilities.ashx?m=enable-tab&a=d") == "1") {
                alert("Tab functionality has been disabled!");
                window.location.reload();
            }
            else {
                alert("Error occurred!");
            }
        }
        else {
            $(this).removeClass("disable-tab");
            $(this).addClass("enable-tab");
            if (RequestData("utilities.ashx?m=enable-tab&a=e") == "1") {
                alert("Tab functionality has been enabled for your login\n\nWhenever you open a page it will open in a built in Tab.");
            }
            else {
                alert("Error occurred!");
            }
        }
    });
}
function IsTabEnabled() {
    if ($(".tab-multi", window.top.document).attr("class").indexOf("enable-tab") > 0) {
        return true;
    }
    else {
        return false;
    }
}
function showSubmoduleCountActionButton() {
    $(".btnaction").each(function() {
        if ($(this).attr("submodcount") != undefined) {
            var count = ConvertToInt($(this).attr("submodcount"));
            if (count > 0) {
                var div = $("<div style='position:absolute;' class='notification-circle'>" + $(this).attr("submodcount") + "</div>");
                $("body").append(div);
                div.css("left", $(this).position().left + $(this).width() + 10);
                div.css("top", $(this).position().top - 8);
            }
        }
        else if ($(this).attr("sentstatusid") != undefined) {
            var sentstatus = ConvertToInt($(this).attr("sentstatusid"));
            var emailsentstatus = "";
            var statuscss = "";
            if (sentstatus <= 1) {
                emailsentstatus = "Not Sent"; statuscss = "open";
            }
            else if (sentstatus == 3) {
                emailsentstatus = "Failed"; statuscss = "failed";
            }
            else if (sentstatus == 2) {
                emailsentstatus = "Sent"; statuscss = "sent";
            }
            var id = "jq-btnemailsmssentstatus-" + $(this).attr("id");
            var div = null;
            if ($("#" + id).length == 0) {
                div = $("<div style='position:absolute;' class='sentstatus-" + statuscss + "' id='" + id + "'>" + emailsentstatus + "</div>");
                $("body").append(div);
            }
            else {
                div = $("#" + id);
            }
            div.css("left", $(this).position().left + $(this).width());
            div.css("top", $(this).position().top - 12);
        }
    });
}
function initActionConfirm() {
    $(".btnaction").click(function() {
        if ($(this).val().toLowerCase().indexOf("view ") >= 0) return true;
        return confirm("Are you sure you want to proceed?");
    });
}
function initFEMPortal() {
    $(".jq-viewfemportal").live("click", function() {
        window.open('../exportportal.aspx?ia=true&sid=' + $(this).closest("tr").find(".idval").attr("idval"));
        return false;
    });
}
function initAdvancedReport() {
    if ($(".report-control").length == 0) {
        $(".report-field:last").find(":submit").hide();
    }
}
function initMultiCheckbox() {
    $(".jq-mchkselectall").click(function() {
        var checked = $(this).is(":checked");
        $(this).closest(".mchk").find("input").each(function() {
            if ($(this).hasClass("jq-mchkselectall")) return;
            $(this).prop("checked", checked);
        });
    });
}
function initCustomerQuery() {
    $(".jq-query-reply").click(function() {
        $(".jq-query-reply-panel").toggle();
        if ($(".jq-query-reply-panel").is(":visible")) {
            $(".jq-query-reply").find("span").text("-");
        }
        else {
            $(".jq-query-reply").find("span").text("+");
        }
    });
}
function initServicePlan()
{
    $(".populateclientservices,.populateserviceplandetail").blur(function(){
        if ($(this).css("display") == "none") return false;
        var obj = $(this);
        var id = $(this).parent().find(".hdnac").val();
        if(id=="0" || id=="")return;
        var url = "../detail.ashx?id="+id+"&m=";
        if($(this).hasClass("populateserviceplandetail"))
        {
            url+="get-serviceplan-details";
        }
        else
        {
            url+="get-client-serviceplan-details";
        }
        ajaxCall(url,function(json){
            if(!obj.hasClass("populateserviceplandetail"))
            {
                var txtserviceplan = $(".txtserviceplan");
                if(txtserviceplan.length > 0)
                {
                    txtserviceplan.val(json.serviceplan);
                    txtserviceplan.parent().find(".hdnac").val(json.serviceplanid);
                }
            }
            var serviceids = json.serviceids;
            var prospectids = json.prospectids;
            setMultiCheckVals($(".mchk-service_service"),serviceids);
            setMultiCheckVals($(".mchk-prospect_prospect"),prospectids);
        });
    });
}
function setMultiCheckVals(div, ids)
{
    div.find(".chk").prop("checked",false);
    if(ids == "")return;
    var arr=ids.split(',');
    div.find(".chk").each(function(){
        var id = $(this).attr("dval");
        for(var i=0;i<arr.length;i++)
        {
            if(id == arr[i])
            {
                $(this).prop("checked",true);
                break;
            }
        }
    });
}
function initMultiSelectAc(){
    $(".multiselectac").each(function(){
        if($(this).val()!="")
        {
            var selectedText = $(this).val();
            $(this).val("");
            var arrText = selectedText.split('|');
            var arrVal = $(this).closest("td").find(".hdnac").val().split(',');
            var td = $(this).closest("td");
            td.append('<div class="multicheckbox-selected-text"></div>');
            var multiCheckDiv = td.find(".multicheckbox-selected-text");
            for(var i=0;i<arrText.length;i++)
            {
                var val = arrVal[i];
                var multiSelectHtml = '<div class="mchkjs-selected-val mchkjs-selected-val-'+val+'" value="'+val+'"><div class="mchkjs-text">'+arrText[i]
                                                +'</div></div>';
                multiCheckDiv.append(multiSelectHtml);
            }
        }
    });
    $("form").submit(function() {
        $(".multiselectac").each(function(){
            var selectedText = "";
            $(this).closest("td").find(".mchkjs-text").each(function(){
                if(selectedText=="")
                {
                    selectedText=$(this).text();
                }
                else
                {
                    selectedText+="|"+$(this).text();
                }
            });
            $(this).val(selectedText);
        });
    });
    $(".jq-mchkall").live("click",function(){
       var div = $(this).closest(".mchk");
       if($(this).is(":checked"))
       {    
            div.find(".chk").prop("checked",true);
       }
       else
       {
            div.find(".chk").prop("checked",false);
       } 
    });
    $(".mchk").each(function(){
        var totalcount = $(this).find(".chk:checked").length;
        var checkedcount = $(this).find(".chk").length;
        if(totalcount == checkedcount)
        {
            $(this).find(".jq-mchkall").prop("checked", true);
        }
    });
}