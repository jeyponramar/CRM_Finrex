//init
$(document).ready(function() {
    setTimeout(passData, 1000);
    displayProgressbar();
    showHideGridActions();
    initPage();
    focusOnLoad();
    setTabHeight();
    loadSelectDeselect();
    initStatusButton();
    initGoBack();
    loadDelete();
    initPopWindowButton();
    initSelectDeselect();
    initDelete();
    initAcSearch();
    loadDatePicker();
    loadDayTimePicker();
    loadMonthYearPicker();
    printReport();
    printPage();
    initMultiSelect();
    initConfirmButton();
    intiCloseDialog();
    initOutsideDivClick();
    intiCurveTab();
    initClientAddPageModal();
    initCommonAjaxContent();
    /*$(".textbox,.sbox,.mbox").live("keypress", function(event) {
    if (event.which == 13) {
    if (!$(this).attr("enableenter") == "true") {
    return false;
    }
    }
    });*/
    $("form").submit(function() {
        //find all auto complete and check whether data selected or not
        var isvalid = true;
        $(".ac").each(function() {
            if ($(this).val().trim() != "") {
                var hdn = $(this).parent().find(".hdnac");
                if (hdn.length == 0) {
                }
                else {
                    var id = hdn.val();
                    if (id == "" || id == "0") {
                        var m = $(this).attr("m");
                        if (m == undefined) m = "Data";
                        alert("Invalid " + m);
                        $(this).focus();
                        isvalid = false;
                    }
                }
            }
        });
        if (isvalid) {
            isvalid = ValidateGridData();
        }
        if (!isvalid) Page_IsValid = false;
        return isvalid;
        return isvalid;
    });
    $(".save").click(function() {
        return ValidateGridData();
    });
    function ValidateGridData() {
        var isvalid = true;
        $(".grid").find(".newrow").find("input").each(function() {
            if ($(this).val().trim() != "" && $(this).val().trim() != "0.00") {
                //isvalid = false;
                CalculateTaxAmount($(this));
                if (!addGridRow($(this))) {
                    $(this).focus();
                    isvalid = false;
                }
                setTotal($(this).closest(".grid"));
            }
        });
        if (!isvalid) {
            alert("Please press ENTER to add data in the grid");
            return false;
        }
        var invalidProducts = "";
        $(".grid").find(".hdn").find("input").each(function() {
            if ($(this).val() == "INVALID PRODUCT") {
                var pname = $(this).closest("tr").find(".productname").text();
                if (invalidProducts == "") {
                    invalidProducts = pname;
                }
                else {
                    invalidProducts += "," + pname;
                }
            }
        });
        if (invalidProducts != "") {
            alert("Invalid products\n" + invalidProducts);
            return false;
        }
        return isvalid;
    }
    /*
    $(".ac").live("blur", function() {
    if ($(this).val().trim() != "") {
    var id = $(this).parent().find(".hdnac").val();
    if (id == "" || id == "0") {
    $(this).focus();
    }
    }
    });*/
    $(".datetimepicker").each(function() {
        if ($(this).attr("disabled") != "disabled") {
            $(this).datetimepicker({
                dayOfWeekStart: 1,
                showOn: 'both',
                format: 'd-m-Y g:i A',
                formatTime: 'g:i A',
                allowTimes: true,
                'setDate': new Date(),
                lang: 'en'
            });
            if (($(this).attr("class").indexOf("cdate") >= 0 || $(this).attr("class").indexOf("currentdate") >= 0)
            && $(this).val() == "") {
                var currentdate = new Date();
                var hours = currentdate.getHours();
                var ampm = hours >= 12 ? 'PM' : 'AM';
                hours = hours % 12;
                hours = hours ? hours : 12;
                var d = currentdate.getDate();
                var m = currentdate.getMonth() + 1;
                if ((d + "").length == 1) d = "0" + d;
                if ((m + "").length == 1) m = "0" + m;
                var min = currentdate.getMinutes();
                if ((min + "").length == 1) min = "0" + min;
                var datetime = d + "-" + m + "-" + currentdate.getFullYear() + " "
                               + hours + ":" + min + " " + ampm;
                $(this).val(datetime);
            }
        }
    });

    $(".quick-menu").live("mouseover", function() {
        _addmenu = $(this);
        var offset = $(this).offset();
        _menuleft = offset.left;
        _menutop = offset.top;
        var menu = $(".addmenudiv");
        menu.css("top", _menutop + "px");
        menu.css("left", _menuleft + "px");
        $(".add-menu").show();
    });
    $(".add-menu").live("mouseover", function() {
        var menu = $(".addmenudiv");
        menu.css("top", _menutop + "px");
        menu.css("left", _menuleft + "px");
        $(this).show();
    }).mouseleave(function() {
        $(this).hide();
    });
    $(".qa").live("click", function() {
        var txt = _addmenu.parent().find(".txtqa");
        var qaparent = txt.attr("qaparent");
        var qaparents = "";
        if (qaparent != undefined) {
            qaparents = qaparent.split(',');
            for (i = 0; i < qaparents.length; i++) {
                var ctrl = GetDataControl(qaparents[i]);
                if (ctrl.val() == "" || ctrl.val() == "0") {
                    if (ctrl.attr("class").indexOf("hdnac") >= 0) {
                        ctrl = ctrl.parent().find(".ac");
                    }
                    //ctrl.css("border","solid 1px #ff0000");
                    txt.val("");
                    alert("Please select the parent level first");
                    ctrl.focus();
                    return;
                }
            }
        }

        if (txt.attr("id") == undefined) {
            var ddl = _addmenu.parent().find(".ddl");
            showQAPop(ddl);
            return;
        }
        if (txt.val().trim() == "") {
            txt.focus();
            return;
        }
        var hdn = _addmenu.parent().find(".hdnqa");

        var id = saveQa(txt);

        if (parseInt(id) > 0) hdn.val(id);

    });
    $(".qa-new").live("click", function() {
        var txt = _addmenu.parent().find(".txtqa");
        var hdn = _addmenu.parent().find(".hdnqa");

        if (txt.length == 0) {
            txt = _addmenu.parent().find("select");
        }
        var w = $(window).width() * 0.7;
        var h = $(window).height() * 0.9;
        var m = txt.attr("m");
        var dcn = txt.attr("dcn");
        window.open("../" + m + "/add.aspx?qa=1&dcn=" + dcn + "&targettxt=" + txt.attr("id") + "&targethdn=" + hdn.attr("id") + "&cn=" + txt.attr("cn"), "qa", "width=" + w + ",height=" + h + "");
    });
    $(".qa-s").live("click", function() {
        var txt = _addmenu.parent().find(".txtqa");
        var hdn = _addmenu.parent().find(".hdnqa");
        var m = txt.attr("m");
        var extras = "tpage=" + parent.curPage + "&targettxt=" + txt.attr("id") + "&targethdn=" + hdn.attr("id") + "&issearch=1&search=" + txt.val();
        parent.loadPage("#/" + m + "/a/view", "View " + m, extras);
    });
    $(".qa-e").live("click", function() {
        var txt = _addmenu.parent().find(".txtqa");
        var hdn = _addmenu.parent().find(".hdnqa");
        var id = hdn.val();
        if (hdn.val() == "" || hdn.val() == "0") {
            txt.focus();
            return;
        }
        if (txt.length == 0) {
            txt = _addmenu.parent().find("select");
            id = txt.val();
        }
        //var extras = "tpage=" + parent.curPage + "&targettxt=" + txt.attr("id") + "&targethdn=" + hdn.attr("id");
        //parent.loadPage("#/" + m + "/a/add/id/" + hdn.val(), "Edit " + m, extras);

        var m = txt.attr("m");
        var w = $(window).width() * 0.7;
        var h = $(window).height() * 0.9;
        var dcn = txt.attr("dcn");
        window.open("../" + m + "/add.aspx?id=" + id + "&qa=1&dcn=" + dcn + "&targettxt=" + txt.attr("id") + "&targethdn=" + hdn.attr("id") + "&cn=" + txt.attr("cn"), "qae", "width=" + w + ",height=" + h + "");

    });
    $(".ac").each(function() {
        setAutoComplete($(this));
    });
    $(".qa-refresh").click(function() {
        var ddl = _addmenu.parent().find(".ddl");
        fillDropDown(ddl);
    });
    $(".close-pop").click(function() {
        closeQA();
    });
    $(".ddlqa-save").click(function() {
        saveQAddl();
    });
    $(".txtqa-for-ddl").keypress(function(event) {
        if (event.which == 13) {
            saveQAddl();
            return false;
        }
    });
    $(".content-panel").css("height", $(window).height() - 102);
    $(".edit").click(function() {
        _viewmode = "edit";
        setViewMode();
    });
    $(".copy").click(function() {
        copyPage();

    });
    $(".save").click(function() {
        var txt;
        var isvalidEntry = true;
        $(".newrow").find("input").each(function() {
            if ($(this).val() != "") {
                isvalidEntry = false;
                txt = $(this);
            }
        });
        if (!isvalidEntry) {
            alert("Invalid data entry in the grid, please press ENTER to add row in grid");
            txt.focus();
        }
    });

    $(".refresh").click(function() {
        location.reload();
    });
    $(".saveenter").keypress(function(event) {
        if (event.which == 13) {
            $("." + $(this).attr("savetarget")).trigger("click");
            return false;
        }
    });

    $(".watermark").focus(function() {
        if ($(this).val() == $(this).attr("wm")) {
            $(this).val("");
        }
    });
    $(".watermark").blur(function() {
        if ($(this).val().trim() == "") {
            $(this).val($(this).attr("wm"));
        }
    });
    $(".delete").click(function() {
        var r = confirm("Are you sure you want to delete this record?\n\nYou can not rollback once data is deleted!")
        return r;
    });

    $(".openclose").click(function() {
        var tr = $(this).parent().next();
        if (tr.css("display") == "none") {
            tr.show("slow");
        }
        else {
            tr.hide("slow");
        }

    });
    $(".pop").live("blur", function() {
        if ($(this).css("display") == "none") return false;
        var jt = $(this).attr("jt");
        var w = $(this).attr("w");
        var m = $(this).attr("m");
        var id = $(this).parent().find(".hdnac").val();
        if (ConvertToInt(id) > 0) {
            if (!confirm("Are you sure you want to populate the detail automatically?")) return false;
            PopulateDetail(m, id, "", jt, w);
        }
    });
    $(".chat-user").click(function() {
        var statusclass = $(this).find(".chat-status").attr("class");
        var status = statusclass.substring(statusclass.indexOf(' ') + 1);
        if (status == "offline") {
            alert("You can not chat with offline user");
            return;
        }
        openChatWindow($(this).attr("uid"), $(this).text());
    });
    $(".ac").focus(function() {
        $(this).attr("acval", $(this).val());
    });
    $(".ac").blur(function() {
        var dcn = $(this).parent().find(".hdnac").attr("dcn");
        var parentAc = $(this);
        var acval = $(this).attr("acval");
        if (acval == $(this).val() || acval == undefined) {
            return; //no action if user didn't change anything in parent ac
        }
        if ($(this).parent().find(".hdnac").val() == "" || $(this).parent().find(".hdnac").val() == "0") return;

        if (dcn != undefined) {

            var arr = dcn.split('_');
            if (arr && arr.length) {

                var cn = arr[1];

                $(".ac").each(function() {
                    var acparents = $(this).attr("acparent");

                    if (acparents != undefined) {
                        var arracparents = acparents.split(',');
                        for (i = 0; i < arracparents.length; i++) {
                            if (arracparents[i] == cn) {
                                $(this).val("");
                                $(this).parent().find(".hdnac").val("");
                            }
                        }
                    }
                });
            }
        }

    });
    //    $(".btnstatus").click(function() {
    //        var rem = $(this).attr("rem");
    //        if (rem != undefined) {
    //            if ($("." + rem).val().trim() == "") {
    //                alert("Please enter remarks");
    //                $("." + rem).focus();
    //                return false;
    //            }
    //        }
    //        var sc = $(this).attr("sc");
    //        var sid = $(this).attr("sid");
    //        var msg = $(this).attr("msg");
    //        var m = $(this).attr("m");
    //        var ab = $(this).attr("ab");
    //        var ad = $(this).attr("ad");
    //        var id = getUrlVars()["id"];
    //        if (sc == undefined) sc = "";
    //        if (ab == undefined) ab = "";
    //        if (ad == undefined) ad = "";
    //        if (msg == undefined) msg = "Data saved successfully";
    //        var url = "../status.ashx?m=" + m + "&sid=" + sid + "&sc=" + sc + "&id=" + id + "&ab=" + ab + "&ad=" + ad;
    //        var data = RequestData(url);
    //        alert(msg);
    //        closeTab();
    //        return false;
    //    });
    $(".btnvalidate").click(function() {
        var validate = $(this).attr("validate");
        var arr = validate.split(",");
        for (i = 0; i < arr.length; i++) {
            var obj = $("." + arr[i]);
            if (obj.val().trim() == "") {
                alert("Please enter " + obj.closest("tr").find(".label").text());
                obj.focus();
                return false;
            }
        }
        return true;
    });
    $(".confirmstatus").click(function() {
        var ConfirmMessage = "";
        ConfirmMessage = $(this).attr("msg");
        if (ConfirmMessage != null && ConfirmMessage != undefined) {
            return confirm(ConfirmMessage);
        }
        return confirm(ConfirmMessage);
    });
    //Populate NonEditableGrid
    $(".popnoneditgrid").blur(function() {
        try {
            var ModuleName = '<%=Common.GetModuleName()%>';
            var URl = '<%=Request.Url.ToString()%>';
            var GridName = $(this).attr("noneditgridname");
            var GridModuleName = $(this).attr("modulename");
            var targetPopClass = $(this).attr("targetpopgrid");
            if (GridName == "" || GridName == undefined) {
                alert("Please give the noneditable gird name");
                return;
            }
            if (targetPopClass == undefined || targetPopClass == "") {
                alert("Please Give the Target POP place");
                return;
            }
            GridModuleName = (GridModuleName == "" || GridModuleName == undefined) ? ModuleName : GridModuleName;
            URl = URl.substring(URl.indexOf(ModuleName));
            var html_ = RequestData("../utilities.ashx?m=PopulateNonEditableGrid&nongridname=" + GridName + "&mn=" + GridModuleName + "&url=" + URl);
            $("." + targetPopClass).html(html_);
        } catch (e) {
            alert(e)
        }
    });
    //Populate End
});
function displayProgressbar() {
    var maxcount = 0;
    var maxwidth = 0;
    $(".progressbar").each(function() {
        var c = ConvertToDouble($(this).attr("count"));
        if (c > maxcount) maxcount = c;
        maxwidth = $(this).attr("maxwidth");
    });
    if (maxwidth == undefined) maxwidth = 300;

    $(".progressbar").each(function() {
        var c = ConvertToDouble($(this).attr("count"));
        var style = "";
        var color = $(this).attr("color");
        if (color != undefined) {
            style = "background-color:" + color;
        }
        if (c > 0) {
            var w = maxwidth / maxcount * c;
            /*if (style == "") {
                style = "width:" + w + "px";
            }
            else {
                style = style + ";width:" + w + "px";
            }*/
            var html = "<div class='progressbar-inner' style='" + style + "' w='"+w+"'></div>";
            $(this).append(html);
        }
    });
    animateProgressbar(maxwidth/100,maxwidth/100, maxwidth);
}
function animateProgressbar(currentWidth,interval,maxwidth) {
    var barExists = false;
    if (interval == 0) interval = 1;
    if (currentWidth == 0) currentWidth = 1;
    $(".progressbar-inner").each(function() {
        var w = ConvertToDouble($(this).attr("w"));
        if (currentWidth > w) {
            $(this).css("width", w);
        }
        else {
            $(this).css("width", currentWidth);
            barExists = true;
        }
    });
    if (barExists) {
        currentWidth = currentWidth + interval;
        setTimeout("animateProgressbar(" + currentWidth + "," + interval + ", " + maxwidth + ")", 30);
    }
    else {
        $(".progressbar-inner").each(function() {
            $(this).css("width", $(this).attr("w") + "px");
        });
    }
}
function getUrlVars()
{
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for(var i = 0; i < hashes.length; i++)
    {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}
function PopulateDetail(m, id, cls, jt,w) {

    if (jt == undefined) jt = "";
    if (w == undefined) w = "";
    var url = "../detail.ashx?m=" + m + "&id=" + id + "&jt=" + jt + "&w=" + w+"&module="+_module+"&moduleid="+_moduleId;

    var data = RequestData(url);
    PopulateData(data);
    
}
function PopulateData(data) {
    if (data == "" || data == null) return;
    var ctrls = "";
    for (i = 0; i < data.length; i++) {
        var col = data[i].column;
        var val = data[i].value;
        var isbinded = true;
        if(col == "serviceids")
        {
            setMultiCheckVals($(".mchk-service_service"),val);
        }   
        else if(col == "prospectids")
        {
            setMultiCheckVals($(".mchk-prospect_prospect"),val);
        }   
        else if(col == "serviceplanid")
        {
            $(".txtserviceplan").parent().find(".hdnac").val(val);
        }
        else if(col == "serviceplan")
        {
            $(".txtserviceplan").val(val);
        }
        else
        {
            isbinded = false;
        }
        if(isbinded)continue;
        var ctrl = GetDataControl(col);
        if (ctrl != undefined) {
            var nm = ctrl.attr("name");
            var ispopulated = false;
            var arr = ctrls.split(',');
            for (j = 0; j < arr.length; j++) {
                if (nm == arr[j]) {
                    ispopulated = true;
                    break;
                }
            }
            if (ispopulated == false) {
                ctrl.val(ConvertToString(val));
                if (ctrls == "") {
                    ctrls = nm;
                }
                else {
                    ctrls += "," + nm;
                }
            }
        }
    }
}
function GetDataControl(cn)
{
    var ctrl;
    if(cn.indexOf('_')>0)
    {
        var arr = cn.split('_');
        cn = arr[1];
    }
    $("input,select,textarea").each(function(){
        var dcn = ConvertToString($(this).attr("dcn"));
        var pcn = ConvertToString($(this).attr("pcn"));
        if(dcn != "")
        {
            arr = dcn.split('_');
            dcn = arr[1];
            if(dcn == cn)
            {
                ctrl = $(this);
            }
        }
       
        if(pcn != "")
        {
            arr = pcn.split('_');
            pcn = arr[1];
            if(pcn == cn)
            {
                ctrl = $(this);
            }
        }
    });
    if (ctrl == undefined) {
        ctrl = $("#" + cn);
    }
    return ctrl;
}
function passData()
{
    var data = $("#_datapasser").val();
    if(data!="" && data!=undefined)
    {
        var arr = data.split('~');
        for(i=0;i<arr.length-1;i++)
        {
            if(i%2==0)
            {
                var arr1 = arr[i].split('@');
                var arr2 = arr[i+1].split('^');
                $("#frame-"+arr1[0]).contents().find("#"+arr1[1]).val(arr2[0]);
                $("#frame-"+arr1[0]).contents().find("#"+arr1[2]).val(arr2[1]);
            }
        }
        $("#_datapasser").val("");
        //showPage(arr1[0]);
    }
    
    setTimeout(passData,1000);
}

function RequestData(URL) {
    var p;
    var isAsc = false;
    $.ajax({
        url: URL,
        type: 'GET',
        async: isAsc,
        success: function(jsonObj) {
            if ((jsonObj+"").indexOf("session expired") > 0) {
                window.location = "../login.aspx";
                return;
            }
            else if ((jsonObj+"").indexOf("Error") >= 0) {
                alert("Error : \n\n" + jsonObj);
                jsonObj = "Error occurred!";
            }
            p = jsonObj;
        },
        complete: function() {
        },
        error: function(err, status, jqXHR) {
        }
    });
    return p;
}
function QueryString(name, URL) {
    var url = "";
    if (URL == undefined) {
        url = window.location.href;
        var index = url.indexOf("?");
        if (index > 0) {
            url = url.substring(index + 1);
            var arr1 = url.split('&');
            for (var i = 0; i < arr1.length; i++) {
                var arr2 = arr1[i].split('=');
                if (arr2[0] == name) {
                    return arr2[1];
                }
            }
            return "";
        }
    }
    else {
        url = URL;
    }
    var inx = url.indexOf("#");
    if (inx >= 0) {
        var arr = url.substring(inx + 1).split('/');
        for (i = 0; i < arr.length; i++) {
            if (arr[i].toLowerCase() == name.toLowerCase() && i % 2 == 0) {
                return arr[i + 1];
            }
        }
    }
    return "";
}
function getParam(id, name) {
    var arr = $(id).val().split('~');
    for (i = 0; i < arr.length; i++) {
        if (arr[i].toLowerCase() == name.toLowerCase() && i % 2 == 0) {
            return arr[i + 1];
        }
    }
    return "";
}
function setParam(ctrl, name, val) {
    var arr;
    var exists = false;
    var param = '';
    if ($(ctrl).val() != "") {
        arr = $(ctrl).val().split('~');
        for (i = 0; i < arr.length; i++) {
            if (arr[i] == name && i % 2 == 0) {
                arr[i + 1] = val;
                exists = true;
            }
            if (param == '') {
                param = arr[i];
            }
            else {
                param = param + '~' + arr[i];
            }
        }
    }
    if (!exists) {
        if (param == '') {
            param = name + '~' + val;
        }
        else {
            param = param + '~' + name + '~' + val; 
        }
    }
    $(ctrl).val(param);
}
function setPassData(tpage, txtid, hdnid, newid, val) {
    var arr;
    var exists = false;
    var param = '';
    //get parent control
    var txt = $("#_datapasser", top.document);
    var name = tpage+'@'+hdnid+'@'+txtid;
    val = newid + '^' + val;
    if (txt.val() != "") {
        arr = txt.val().split('~');
        for (i = 0; i < arr.length; i++) {
            if (arr[i] == name && i % 2 == 0) {
                arr[i + 1] = val;
                exists = true;
            }
            if (param == '') {
                param = arr[i];
            }
            else {
                param = param + '~' + arr[i];
            }
        }
    }
    if (!exists) {
        if (param == '') {
            param = name + '~' + val;
        }
        else {
            param = param + '~' + name + '~' + val; 
        }
    }
    txt.val(param);
}
/*
$(document).ready(function() {
    $(".print").live("click", function() {
        window.print();
    });
});
*/
function getTotal(table, nm) {
    var colIndex = -1;
    var i = 0;
    var total = 0;
    table.find("input").each(function() {
        if ($(this).attr("name") != undefined) {
            if ($(this).attr("name").indexOf("_" + nm + "-") > 0
            || $(this).attr("name").indexOf(nm + "-") == 0) {
                try {
                    if ($(this).closest("tr").css("display") != "none") {
                        if (isNaN($(this).val()) == false && $(this).val() != "") total += parseFloat($(this).val());
                    }
                } catch (e) { }
            }
        }
    });
    return parseFloat(parseFloat(total).toFixed(0));

}
function getSubTotal(table, nm) {
    var colIndex = -1;
    var i = 0;
    var total = 0;
    var istax = 0;
    table.find(".repeater-row,.repeater-alt").each(function(e) {
        if ($(this).css("display") != "none") {
            $(this).find("input").each(function() {
                if ($(this).attr("name") != undefined) {
                    if ($(this).attr("name").indexOf("IsTax-") > 0) {
                        istax = $(this).val();
                    }
                    else if ($(this).attr("name").indexOf(nm + "-") > 0 && istax != 1) {
                        try {
                            if (isNaN($(this).val()) == false && $(this).val() != "") total += parseFloat($(this).val());
                        } catch (e) { }
                    }
                }
            });
        }
    });
    return total;

}
function GetSubTotalForTax(table, ctrlProduct) {
    var total = 0;
    var istaxInPrevRow = false;
    var row = ctrlProduct.closest("tr");
    var taxType = 0;
    var reachedCurrentRow = false;
    var pid;

    if (row.attr("class").indexOf("newrow") >= 0) {
        pid = $("#productid").val();
    }
    else {
        row.find("input").each(function() {
            var nm = $(this).attr("name") + "";
            if (nm.indexOf("_productid-") > 0) {
                pid = $(this).val();
            }
        });
    }
    if (pid.indexOf('-') > 0) {
        var arr = pid.split('-');
        if (parseInt(arr[2]) == 1) {
            if (arr.length == 4) {
                taxType = ConvertToInt(arr[3]);
            }
        }
        else {
            return 0;
        }
    }

    if (taxType == 0) taxType = 1;

    if (taxType == 1) {//On Sub Total
        reachedCurrentRow = false;
        table.find(".repeater-row,.repeater-alt").each(function() {
            if ($(this).is(row)) {
                reachedCurrentRow = true;
            }
            if (reachedCurrentRow == false) {
                var istax = false;
                $(this).find("input").each(function() {
                    var nm = $(this).attr("name") + "";
                    if (nm.indexOf("_productid-") > 0) {
                        var pid = $(this).val();
                        if (pid.indexOf('-') > 0) {
                            var arr = pid.split('-');
                            if (parseInt(arr[2]) == 1) {
                                istax = true;
                            }
                        }
                    }
                });
                $(this).find("input").each(function() {
                    var nm = $(this).attr("name") + "";
                    if (nm.indexOf("_amount-") > 0) {
                        total += ConvertToDouble($(this).val());
                    }
                });
                if (istax) total = 0;
            }
        });
    }
    else if (taxType == 2)//On Previous Tax
    {
        row.prev().find("input").each(function() {
            var nm = $(this).attr("name") + "";
            if (nm.indexOf("_productid-") > 0) {
                var pid = $(this).val();
                if (pid.indexOf('-') > 0) {
                    var arr = pid.split('-');
                    if (parseInt(arr[2]) == 1) {
                        istaxInPrevRow = true;
                    }
                }
            }
            if (nm.indexOf("_amount-") > 0 && istaxInPrevRow) {
                total = ConvertToDouble($(this).val());
            }
        });
    }
    else if (taxType == 3)//on previous product
    {
        row.prev().find("input").each(function() {
            var nm = $(this).attr("name") + "";
            if (nm.indexOf("_productid-") > 0) {
                var pid = $(this).val();
                if (pid.indexOf('-') > 0) {
                    var arr = pid.split('-');
                    if (parseInt(arr[2]) == 1) {
                        istaxInPrevRow = true;
                    }
                }
            }
            if (nm.indexOf("_amount-") > 0 && istaxInPrevRow == false) {//only if product
                total = ConvertToDouble($(this).val());
            }
        });
    }
    else if (taxType == 4)//Manual
    {
    }
    else if (taxType == 5)//Only On First Product Sub Total
    {
        var isTaxFound = false;
        reachedCurrentRow = false;
        table.find(".repeater-row,.repeater-alt").each(function() {
            if ($(this).is(row)) {
                reachedCurrentRow = true;
            }
            if (reachedCurrentRow == false) {
                if (isTaxFound == false) {
                    $(this).find("input").each(function() {
                        var nm = $(this).attr("name") + "";
                        if (nm.indexOf("_productid-") > 0) {
                            var pid = $(this).val();
                            if (pid.indexOf('-') > 0) {
                                var arr = pid.split('-');
                                if (parseInt(arr[2]) == 1) {
                                    isTaxFound = true;
                                }
                            }
                        }
                    });
                    if (isTaxFound == false) {
                        $(this).find("input").each(function() {
                            var nm = $(this).attr("name") + "";
                            if (nm.indexOf("_amount-") > 0) {
                                total += ConvertToDouble($(this).val());
                            }
                        });
                    }
                }
            }
        });
    }
    else if (taxType == 6)//Only On Previous Product Sub Total
    {
        var isrowExists = true;
        var ctRow = row;
        var productRow = null;
        var isproductRow = false;
        var productRow = null;
        var isTax = false;
        while (isrowExists) {
            var r = getPrevRow(ctRow);
            if (r == undefined || r == null || r.length == 0) {
                isrowExists = false;
            }
            else {
                if (isproductRow == false) {
                    isTax = isTaxRow(r);
                    if (isTax == false) {
                        productRow = r;
                        isproductRow = true;
                        isrowExists = false;
                    }
                }
            }
            ctRow = r;
        }
        if (isproductRow) {
            ctRow = productRow;
            isrowExists = true;
            while (isrowExists) {
                isTax = isTaxRow(ctRow);
                if (isTax) {
                    isrowExists = false;
                }
                else {
                    var amount = getAmount(ctRow);
                    total += amount;
                    ctRow = getPrevRow(ctRow);
                    if (ctRow == null || ctRow == undefined || ctRow.length == 0) {
                        isrowExists = false;
                    }
                }
            }
        }
    }
    return total;

}
function getPrevRow(row) {
    var isexists = true;
    var ctRow = row.prev();
    if (ctRow == undefined || ctRow == null || ctRow.length == 0) {
        isexists = false;
        return null;
    }
    while (isexists) {
        if (ctRow == undefined || ctRow == null || ctRow.length == 0) {
            isexists = false;
            return null;
        }
        else {
            if (ctRow.css("display") == "none") {
                isexists = true;
            }
            else {
                isexists = false;
                return ctRow;
            }
        }
        ctRow = ctRow.prev();
    }
}
function getNextRow(row) {
    var isexists = true;
    var ctRow = row.next();
    if (ctRow == undefined || ctRow == null || ctRow.length == 0) {
        isexists = false;
        return null;
    }
    while (isexists) {
        if (ctRow == undefined || ctRow == null || ctRow.length == 0) {
            isexists = false;
            return null;
        }
        else {
            if (ctRow.css("display") == "none") {
                isexists = true;
            }
            else {
                isexists = false;
                return ctRow;
            }
        }
        ctRow = ctRow.prev();
    }
}
function isTaxRow(row) {
    var isTaxFound = false;
    if (row == null || row == undefined || row.length == 0) return false;
    row.find("input").each(function() {
        var nm = $(this).attr("name") + "";
        if (nm.indexOf("_productid-") > 0) {
            var pid = $(this).val();
            if (pid.indexOf('-') > 0) {
                var arr = pid.split('-');
                if (parseInt(arr[2]) == 1) {
                    isTaxFound = true;
                }
            }
        }
    });
    return isTaxFound;
}
function getAmount(row) {
    var amount = 0;
    row.find("input").each(function() {
        var nm = $(this).attr("name") + "";
        if (nm.indexOf("_amount-") > 0) {
            amount = ConvertToDouble($(this).val());
        }
    });
    return amount;
}   
function setDatePicker(dt) {
    var d = new Date();
    var dat = getDay(d.getDate());
    var mon = getDay(d.getMonth() + 1);
    var year = d.getFullYear();
    var todayDate = dat + "-" + mon + "-" + year;
    dt.val(todayDate)
}
//function setDatePicker(dt)
//{
//    dt.datepicker({ dateFormat: 'dd-mm-yy' });
//    if (dt.val() == "") dt.datepicker().datepicker('setDate', new Date());
//}
function getDay(d) {
    d = d + '';
    if (d.length == 1) {
        return "0" + d;
    }
    return d;
}
function ConvertToDate(dt) {
    if (dt == "" || dt==null || dt.indexOf("1900")>0) {
        return "";
    }
    var sp = dt.indexOf(' ');
    if (sp > 0) {
        dt = dt.substring(0, sp);
    }
    return dt;
}
function ConvertToDateTime(dt) {
    if (dt == "" || dt == null || dt.indexOf("1900") > 0) {
        return "";
    }
    dt = dt.replace('/', '-').replace('/', '-');
    return dt;
}
function checkAndConvertToDate(dt) {
    if (dt != null) {
        if (dt.length == 19) {
            if (dt.indexOf(" 00:00:00") > 0) {
                return convertToDate(dt);
            }
        }
    }
    return dt;
}
function convertTOSQLDate(dt) {
   
    arr = dt.split('-');
    dt = arr[2] + '-' + arr[1] + '-' + arr[0];
    return dt;
}
function SetZeroForAmount(obj) {
    obj.find(".val-dbl").each(function() {
        if ($.trim($(this).val()) == "") {
            $(this).val("0.00");
        }
    });
    obj.find(".val-i").each(function() {
        if ($.trim($(this).val()) == "") {
            $(this).val("0");
        }
    });
}
function formatAmnt(ctrl) {
    try {

        $(ctrl).val(parseFloat($(ctrl).val()).toFixed(2));
    }
    catch (e) { }
}
$(".link").live("click", function() {
    window.location = $(this).find("a").attr("href");

});
function setCurrentPeriod() {
    $(".currentperiod").text($.cookie("PeriodFrom") + " to " + $.cookie("PeriodTo"));
}
function ConvertToString(data) {
    if (data == undefined || data == null || data == "") {
        return "";
    }
    else {
        data = data.toString().replace(/&dquot;/g, "\"");
        data = data.toString().replace(/&dquot;/gi, "\"");
        data = data.replace(" 12:00:00 AM", "");
    }
    return data;
}
function getacurl(txt)
{
    var cn = txt.attr("cn");
    var m = txt.attr("m");
    var cm = txt.attr("cm");
    var w = ConvertToString(txt.attr("w"));
    var val = txt.val();

    var url = getUrlPrefix() + "ac.ashx?m=" + m + "&cn=" + cn + "&q=" + val + "&cm=" + cm + "&w=" + w;
    if (IsMobile()) {
        url = "../" + url;
    }
    return url;    
}
function setAutoComplete(txt) {
    var txtInitVal = txt.val();
    txt.val("");
    txt.autocomplete(getacurl(txt), {
       //width: 250,
       matchContains: true, 
       hdnval: txt.parent().find(".hdnac"),
       selectFirst: false
   });
   txt.val(txtInitVal);
}
function SetDetailPage(id) {
    if (id == "") {
        _viewmode = "edit";
    }
    else {
        _viewmode = "detail";
    }
    setViewMode();
    freezepage();
    $(".copy").hide();
}
function setViewMode() {
    
    if (_viewmode == null || _viewmode == "detail") {
        $(".dpage").show();
        $(".epage").hide();
        $(".quick-menu").hide();
        var urlPrefix = "";
        if (IsMobile()) urlPrefix = "../";
        $(".form").find("input[type=text],input[type=password],textarea,select").each(function() {
            var val = "";
            if ($(this)[0].tagName == "SELECT") {
                val = $(this).find("option:selected").text();
                if (val == "Select") val = "";
            }
            else {
                val = $(this).val();
            }
            var cls = ConvertToString($(this).attr("class"));
            var dcn = ConvertToString($(this).attr("dcn")); 
            if (cls.indexOf("edpage") >= 0) return;
            if (dcn == "followup_module") {
                $(this).hide();
                $(this).parent().append($("<span class='val dval spage' href='#" + val + "/add.aspx?id=" + $(this).attr("mid") + "'>" + val + "</span>"));
                return;
            }
            if ($(this).css("display") != "none") {
                var islink = false;
//                if (cls.indexOf(" txtac") >= 0) {
//                    var id = $(this).parent().find(".hdnac").val();
//                    var m = $(this).attr("m");
//                    if (id != "") {
//                        $(this).parent().append($("<span class='val dval spage' href='#" + m + "/add.aspx?id=" + id + "'>" + val + "</span>"));
//                        islink = true;
//                    }
//                }
                if (islink == false) {
                    if (cls.indexOf("val-ph") >= 0) {
                        $(this).parent().append($("<span class='val dval'>" + val + "<img src='" + urlPrefix + "../images/phone.png'/></span>"));
                    }
                    else if (cls.indexOf("val-email") >= 0) {
                        if (val.trim() != "") {
                            $(this).parent().append($("<span class='val dval'>" + val + "<img src='" + urlPrefix + "../images/email.png' class='hand sendemail' email='" + val + "'/></span>"));
                        }
                        else {
                            $(this).parent().append($("<span class='val dval'></span>"));
                        }
                    }
                    else if (dcn.indexOf("address") >= 0) {
                        if (val.trim() != "") {
                            $(this).parent().append($("<span class='val dval'>" + val + "<img src='" + urlPrefix + "../images/icon/google-map.jpg' class='gmap hand' addr='" + val + "'/></span>"));
                        }
                        else {
                            $(this).parent().append($("<span class='val dval'></span>"));
                        }
                    }
                    else if (dcn.indexOf("skype") >= 0) {
                        if (val != "") {
                            $(this).parent().append($("<span class='val dval'>" + val +
                                  "<a href='skype:" + val + "?call'><img src='" + urlPrefix + "../images/skype.jpg'/></a></span>"));
                        }
                        else {
                            $(this).parent().append($("<span class='val dval'></span>"));
                        }
                    }
                    else {
                        $(this).parent().append($("<span class='val dval'>" + val + "</span>"));
                    }
                }
                $(this).hide();
            }
        });
    }
    else {
        $(".dpage").hide();
        $(".epage").show();
        $(".quick-menu").show();
        var isfirst = true;
        $(".form").find("input[type=text],textarea,select").each(function() {
            var cls = $(this).attr("class");
            if (cls != undefined && (cls.indexOf("hdn") >= 0 || cls.indexOf("hidden") >= 0)) {
            }
            else {
                $(this).show();
            }
            if (isfirst) {
                //$(this).focus();
                isfirst = false;
            }
        });
        $(".dval").remove();
    }
    $(".htmleditor").hide();
    $(".datepicker").each(function() {
        if ($(this).attr("format") == "Date") {
            $(this).val($(this).val().replace(" 12:00:00 AM", ""));
        }
    });
}
function focux(editRow) {
    if (editRow.find(".first") != undefined) {
        editRow.find(".first").focus();
        return;
    }
    var first = true;
    editRow.find("td").each(function() {
        $(this).find("select").each(function() {
            if ($(this).css("display") != "none") {
                if (first) $(this).focus();
                first = false;
            }
        });
    });
    editRow.find("td").each(function() {
        $(this).find("input").each(function() {
            if ($(this).css("display") != "none") {
                if (first) $(this).focus();
                first = false;
            }
        });
    });
}
function fillDropDown(ddl) {
    var url = "../ddl.ashx?m=" + ddl.attr("m") + "&cn=" + ddl.attr("cn") + "&id=" + ddl.attr("id");
    var data = RequestData(url);
    if (isValidData(data)) {
        ddl.find("option").remove();
        ddl.append("<option value='0'>Select</option>");
        for (i = 0; i < data.length; i++) {
            ddl.append("<option value='" + data[i].id + "'>" + data[i].Name + "</option>");
        }
    }
}
function isValidData(data)
{
    if (data == null || data == "") {
        return false;
    }
    if (data.indexOf("Error")>=0) {
        return false;
    }
    return true;
}
function showQAPop(obj) {
    _targetQAddl = obj;
    var popbox = $(".popbox");
    popbox.css({ 'left': obj.position().left - popbox.width() / 2 + obj.width() / 2, 'top': obj.position().top + 20 });
    popbox.find(".arrow").css({ 'left': popbox.width() / 2 - 10 });
    popbox.find(".arrow-border").css({ 'left': popbox.width() / 2 - 10 });
    popbox.show();
    var txt = $(".txtqa-for-ddl");
    txt.val("");
    txt.show();
    txt.focus();
}
function saveQAddl() {
    var txt = $(".txtqa-for-ddl");
    if (txt.val().trim() == "") {
        txt.focus();
    }
    txt.attr("m", _targetQAddl.attr("m"));
    txt.attr("cn", _targetQAddl.attr("cn"));
    var id = saveQa(txt);
    if (id > 0) {
        _targetQAddl.append("<option selected value='" + id + "'>" + txt.val() + "</option>");
        var nm = _targetQAddl.attr("name");
        var hdntxt = null;
        if (_targetQAddl.parent().find(".hdnddlqa").length == 0) {
            hdntxt = $("<input type='hidden' value='" + id + "' name='" + nm + "_qa' class='hdnddlqa'/><input type='hidden' value='" + txt.val() + "' name='" + _targetQAddl.attr("id") + "_txtddlqa' class='txtddlqa'/>");
            _targetQAddl.parent().append(hdntxt);
        }
        else {
            hdntxt = _targetQAddl.parent().find(".hdnddlqa");
            hdntxt.val(id);
            _targetQAddl.parent().find(".txtddlqa").val(txt.val());
            
        }
        closeQA();
    }
}
function saveQa(obj) {
    var v = obj.val();
    var cn = obj.attr("cn");
    var m = obj.attr("m");
    var cm = obj.attr("cm");
    var qaparent = obj.attr("qaparent");
    var qaparents = "";
    var ev = obj.attr("ev");
    var ec = obj.attr("ec");
    if(ev==undefined)ev="";
    if(ec==undefined)ec="";
    if(qaparent!=undefined)
    {
        qaparents = qaparent.split(',');
        for(i=0;i<qaparents.length;i++)
        {
            var ctrl = GetDataControl(qaparents[i]);
            var cn1 = "";
            if (ctrl.attr("dcn") == undefined) {
                cn1 = qaparents[i];
            }
            else
            {
                if (ctrl.attr("dcn").indexOf('_') > 0) {
                    var arr = ctrl.attr("dcn").split('_');
                    cn1 = arr[1];
                }
            }
            var val = ctrl.val();
            if (val.indexOf('-') > 0) {
                var arrval = val.split('-');
                val = arrval[0];
            }
            if(ec == "")
            {
                ec = cn1;
                ev = val;
            }
            else
            {
                ec = ec + "," + cn1;
                ev = ev + "," + val;
            }
        }
    }
    v = RemoveSpecialChars(v);
    var url = "../qa.ashx?m=" + m + "&cn=" + cn + "&v=" + v + "&ev=" + ev + "&ec=" + ec + "&cm=" + cm;
    var id = RequestData(url);
    if (id == -1) {
        alert("Data already exists, duplicate entry not allowed!");
        obj.focus();
    }
    else if (id > 0) {
        alert("Data saved!");
        obj.focus();
    }
    else {
        alert("Error occurred while saving data!");
    }
    return id;
} 
function RemoveSpecialChars(data)
{
    data = data.replace(/&/g,"SC__AND");
    return data;
}
function closeQA() {
    $(".popbox").fadeOut("fast");
    _targetQAddl = null;
}
function copyPage() {
    $(".iscopy").val("1");
    _viewmode = "edit";
    setViewMode();
    $(".copy").hide();
    $(".title").find("span").text($(".title").find("span").text().replace("Edit","Copy and Create New"));
    
}
function ConvertToDouble(val) {
    if (val == "" || val == null || val == undefined) {
        return 0;
    }
    return parseFloat(val);
}
function ConvertToInt(val) {
    if (val == "" || val == null || val == undefined) {
        return 0;
    }
    return parseInt(val);
}
function ConvertToBool(val) {
    if (val == undefined || val == "" || val == null) {
        return false;
    }
    if ((val + "").toLowerCase() == "true" || val + "" == "1") {
        return true;
    }
    else {
        return false;
    }
}
function findAmount(ctrl) {
    var lid = ctrl.parent().parent().find("#ledgerid").val();
    if (lid != undefined) {
        if (ConvertToInt(lid) > 0) {
            return;
        }
    }
    var amount = ctrl.parent().parent().find("input[id=amount]");
    if (amount.length == 0) return;
    var quantity = ctrl.parent().parent().find("input[id=quantity]");
    if (quantity.length == 0) return;
    var rate = ctrl.parent().parent().find("input[id=price]");
    if (ctrl.closest("tr").find("#productname").val().toLowerCase().indexOf("discount") >= 0) {
        var subtotal = getSubTotal(ctrl.closest(".grid"), "amount");
        var dblAmount = 0;
        if (ConvertToInt(quantity.val()) > 0) {
            dblAmount = (-1) * ConvertToDouble(subtotal * ConvertToInt(quantity.val()) / 100).toFixed(2);
        }
        else {
            dblAmount = (-1) * ConvertToDouble(amount.val()).toFixed(2);
        }
        amount.val(dblAmount);
        return;
    }
    if (rate.length == 0) {
        rate = ctrl.parent().parent().find("input[id=rate]");
        if (rate.length == 0) {
            return;
        }
    }
    
    amount.val(ConvertToDouble(ConvertToDouble(rate.val()) * ConvertToInt(quantity.val())).toFixed(2));
}
function setTotal(grid, column, totalcss) {
    if (_isAdjustTax) {
        AdjustTaxAmounts(grid);
    }
    if (column == null) column = "amount";
    if (totalcss == null) totalcss = "gridtotal";
    var totctrl = grid.find("." + totalcss);
    if (totctrl.length == 0) return;
    total = getTotal(grid, column);
    total = parseFloat(total).toFixed(0);
    totctrl.text(total);
    totctrl.val(total);
}
function AdjustTaxAmounts(grid) {
    grid.find(".gr").each(function() {
        var qty = 0;
        var rate = 0;
        var istax = false;
        var taxAmount = 0;
        if ($(this).css("display") != "none") {
            $(this).find("input").each(function() {
                var nm = $(this).attr("name") + "";
                if (nm.indexOf("_productid-") > 0) {
                    var pid = $(this).val();
                    if (pid.indexOf('-') > 0) {
                        var arr = pid.split('-');
                        if (parseInt(arr[2]) == 1) {
                            istax = true;
                            if (arr.length > 3) {
                                if (parseInt(arr[3]) == 4)//manual tax
                                {
                                    istax = false;
                                }
                            }
                            if (istax) {
                                var taxPer = ConvertToDouble(arr[1]);
                                taxAmount = GetSubTotalForTax(grid, $(this));
                                taxAmount = ConvertToDouble(taxAmount * taxPer / 100).toFixed(2);
                            }
                        }
                    }
                }
                else if (nm.indexOf("_quantity-") > 0 || nm.indexOf("_totalinquantity-") > 0) {
                    qty = ConvertToDouble($(this).val());
                }
                else if (nm.indexOf("_rate-") > 0 || nm.indexOf("_price-") > 0) {
                    rate = ConvertToDouble($(this).val());
                }
                else if (nm.indexOf("_amount-") > 0) {
                    var amount = 0;
                    var iscalculated = false;
                    if (istax) {
                        amount = parseFloat(taxAmount).toFixed(2);
                        iscalculated = true;
                    }
                    else {
                        if (qty > 0 && rate > 0) {
                            amount = parseFloat(qty * rate).toFixed(2);
                            iscalculated = true;
                        }
                        else if (rate > 0) {
                            amount = rate;
                            iscalculated = true;
                        }
                        else {
                            amount = $(this).val()
                        }
                    }
                    if (iscalculated) {
                        $(this).val(amount);
                        $(this).closest("tr").find(".amount").text(amount);
                    }
                }
            });
        }
    });
}
function AdjustTaxAmounts1(grid) {
    var subTotal = 0;
    var istax = false;
    var istax_prev = false;
    var taxAmount_prev = 0;

    grid.find(".gr").each(function() {
        var qty = 0;
        var rate = 0;
        if ($(this).css("display") != "none") {
            $(this).find("input").each(function() {
                var nm = $(this).attr("name") + "";
                if (nm.indexOf("_quantity-") > 0 || nm.indexOf("_totalinquantity-") > 0) {
                    qty = ConvertToDouble($(this).val());
                }
                else if (nm.indexOf("_rate-") > 0 || nm.indexOf("_price-") > 0) {
                    rate = ConvertToDouble($(this).val());
                }
                else if (nm.indexOf("_amount-") > 0) {
                    var amount = 0;
                    var iscalculated = false;
                    if (qty > 0 && rate > 0) {
                        amount = parseFloat(qty * rate).toFixed(2);
                        iscalculated = true;
                    }
                    else if (rate > 0) {
                        amount = rate;
                        iscalculated = true;
                    }

                    if (iscalculated) {
                        $(this).val(amount);
                        $(this).closest("tr").find(".amount").text(amount);
                    }
                }
            });
        }
    });
    grid.find(".gr").each(function() {
        if ($(this).css("display") != "none") {
            istax = false;
            var amount = 0;
            var taxPer = 0;
            $(this).find("input").each(function() {
                var nm = $(this).attr("name") + "";
                if (nm.indexOf("_amount-") > 0) {
                    amount = ConvertToDouble($(this).val());
                    if (istax) amount = 0;
                }
                if (nm.indexOf("_productid-") > 0) {
                    var pid = $(this).val();
                    if (pid.indexOf('-') > 0) {
                        var arr = pid.split('-');
                        if (parseInt(arr[2]) == 1) {
                            istax = true;
                            taxPer = ConvertToDouble(arr[1]);
                        }
                    }
                }
                if (nm.indexOf("_amount-") > 0) {
                    if (istax) {
                        if (istax_prev) {
                            subTotal = taxAmount_prev;
                        }
                        var taxAmount = parseFloat(subTotal * taxPer / 100).toFixed(2);
                        $(this).val(taxAmount);
                        $(this).closest("tr").find(".amount").text(taxAmount);
                        taxAmount_prev = taxAmount;
                        subTotal = 0;
                    }
                    subTotal += ConvertToDouble(amount);
                }

            });
            istax_prev = istax;
        }
    });
}
function getReminders() {
    var timeInterval = 300000;
    var url = "data.ashx?m=reminder";
    if (_isMobile) {
        url = "../../data.ashx?m=reminder-count";
    }
    var data = RequestData(url);
    if (_isMobile) {
        var reminderCount = ConvertToInt(data);
        if (reminderCount > 0) {
            $(".remindercount").text(reminderCount);
            $(".trReminder").show("slow");
            $(".header-bg").css("top", "40px");
        }
        
        setTimeout("getReminders()", timeInterval);
        return;
    }
    if (data == "-1") {
        window.location = "adminlogin.aspx";
    }
    for (i = 0; i < data.length; i++) {
        var title = "Reminder - " + data[i].subject + i.toString();
        var id = data[i].id;
        var moduless = data[i].module;
        var mid = data[i].mid;
        var qsv = "";
        if (moduless != null && moduless != undefined) {
            var moduleId = "&mid=" + mid;
            qsv = "&qsv=" + ConvertToString(moduless) + ConvertToString(moduleId);
        }
        var tabExists = false;
        $(".tab-bar", window.top.document).find(".tabpage").each(function() {
            var t = $(this).attr("title");
            var page = parseInt($(this).attr("id").replace("tab-", ""));
            if (t == title) {
                tabExists = true;
            }
        });
        if (!tabExists) {
            loadPage("#reminder/reminder.aspx?id=" + id + qsv, title);
        }
    }
    setTimeout("getReminders()", timeInterval);
}

/*
$(".gmap").live("click", function() {
    var addr = $(this).attr("addr");
    window.open("http://maps.google.com/maps?q=" + addr);
});
$(".sendemail").live("click", function() {
    var email = $(this).attr("email");
    loadPage("#email/add.aspx?e_id="+email,"Send Mail");
});
*/
function getFileExt(fileName)
{
    var arr = fileName.split('.');
    return arr[arr.length-1];
}
function bindChatList()
{
    var html = "";
    //html = "<div>sfdsfds</div>";
    //$(".chat-list").html(html);
}
function showChatList()
{
}
function openChatWindow(uid,name,chatid)
{
    var left = $(window).width() - 220;
    var top = $(window).height() - 255;
    var shortname = name;
    if(shortname.length > 17)
    {
        shortname  = shortname.substring(0,17) + '...';
    }    
    var isexists = false;
    $(".chat-window").each(function(){
        var id = $(this).attr("uid");
        if(id == uid)
        {
            $(this).show();
            isexists = true;
            $("#chat-"+uid).find(".chat-msg").focus();
            arrangeChatWindows(false);
        }
    });
    if(isexists) return;
    var cid = chatid;
    if(chatid == undefined)
    {
        var url = "chat.ashx?a=n&uid="+uid;
        cid = RequestData(url);
    }
    var html = '<div class="chat-window" uid="'+uid+'" id="chat-'+uid+'" cid="'+cid+'">'+
                '<table width="100%" cellpadding="0" cellspacing="0">'+
                    '<tr>'+
                        '<td class="chat-window-header">'+
                            '<table cellpadding="0" cellspacing="0" width="100%">'+
                                '<tr>'+
                                    '<td width="50">'+
                                        '<div class="left">'+
                                            '<img src="images/user/thumb/'+uid+'.jpg" style="border-radius:5px;width:30px;"/>'+
                                        '</div>'+
                                        '<div class="chat-window-status-online"></div>'+
                                    '</td>'+
                                    '<td class="chat-window-nm">'+shortname+'</td>'+
                                    '<td align="right"><div class="close-chat"></div></td>'+
                                '</tr>'+
                            '</table>'+
                        '</td>'+
                    '</tr>'+
                    '<tr>'+
                        '<td class="chat-msg-history">'+
                            '<div class="chat-msg-panel"></div>'+
                        '</td>'+
                    '</tr>'+
                    '<tr>'+ 
                        '<td>'+
                            '<textarea id="chat-message" class="chat-msg"></textarea>'+
                        '</td>'+
                    '</tr>'+
                '</table>'+
            '</div>';
    $("body").append(html);            
//    $("#chat-"+uid).css("left",left+"px");
//    $("#chat-"+uid).css("top",top+"px");
    //$("#chat-"+uid).find(".chat-msg").focus();
    arrangeChatWindows(true);
}
$(document).ready(function(){
    //setTimeout("receiveMessage()",1000);
});
function receiveMessage()
{
    parent._CHATTER_BLUR_COUNT++;
    if(parent._CHATTER_FOCUS == false)
    {
        if(parent._CHATTER_BLUR_COUNT <= 3)
        {
            setTimeout("receiveMessage()", 1000);
            return;
        }
    }
    
    if(parent._CHATTER_BLUR_COUNT >= 3)
    {
        parent._CHATTER_BLUR_COUNT = 0;
    }
    var url = "chat.ashx?a=r";
    var messages = RequestData(url);
    if(messages != "" && messages != undefined)
    {
        for (i = 0; i < messages.length; i++)
        {
            try
            {
                var cid = messages[i].chatdetail_chatid;
                var uid = messages[i].chatdetail_fromuserid;
                var userName = messages[i].user_fullname;
                var msg = messages[i].chatdetail_message;
                openChatWindow(uid,userName,cid);
                var shortname = userName;
                if(shortname.length > 17)
                {
                    shortname  = shortname.substring(0,17) + '...';
                }
                var chatwindow = $("#chat-"+uid);
                var html = "<div><span class='bold'>"+userName+" : </span><span>"+msg+"</span></div>";
                chatwindow.find(".chat-msg-panel").append(html);
            }
            catch(e){}
        }
    }
    arrangeChatWindows(false);
    setTimeout("receiveMessage()", 1000);
}
function arrangeChatWindows(isfocus)
{
    var arrChatIds = new Array();
    var index = 0;
    $(".chat-window").each(function(){
        arrChatIds[index] = $(this).attr("cid");
        index++;
    });
    arrChatIds.sort();
    
    var maxleft = $(window).width() - 220;
    var top = $(window).height() - 255;
    var count = 0;
    for(i=0;i<index;i++)
    {
        var uid = 0;
        $(".chat-window").each(function(){
            if($(this).attr("cid") == arrChatIds[i])
            {
                uid = $(this).attr("uid");
            }
        });
        if($("#chat-"+uid).css("display")!="none")
        {
            var space = 0;
            if(parseInt(count)>0)
            {
                space = count * 10;
            }
            if(parseInt(count) < 4)
            {
                left = parseInt(maxleft) - parseInt(count) * 200 - space;
            }
            if(parseInt(count) >= 3)
            {
                $("#chat-"+uid).css("z-index",arrChatIds[i]);    
            }
            $("#chat-"+uid).css("left",left+"px");
            $("#chat-"+uid).css("top",top+"px");
            if(isfocus)
            {
                $("#chat-"+uid).find(".chat-msg").focus();
            }
            count++;
        }
    }
}
function getMonthId(mn)
{
    mn = mn.toUpperCase();
    if(mn=='JAN')return 0;
    if(mn=='FEB')return 1;
    if(mn=='MAR')return 2;
    if(mn=='APR')return 3;
    if(mn=='MAY')return 4;
    if(mn=='JUN')return 5;
    if(mn=='JUL')return 6;
    if(mn=='AUG')return 7;
    if(mn=='SEP')return 8;
    if(mn=='OCT')return 9;
    if(mn=='NOV')return 10;
    if(mn=='DEC')return 11;
}
function getMonthName(id)
{
    if(id==0)return "Jan";
    if(id==1)return "Feb";
    if(id==2)return "Mar";
    if(id==3)return "Apr";
    if(id==4)return "May";
    if(id==5)return "Jun";
    if(id==6)return "Jul";
    if(id==7)return "Aug";
    if(id==8)return "Sep";
    if(id==9)return "Oct";
    if(id==10)return "Nov";
    if(id==11)return "Dec";
}
//Thiru Start
function HideShowMyButton(id,btn)
{
    if(id=="")
    {
        $(btn).hide();
    }
    else
    {
         $(btn).show();
    }
}
//Thiru End
function SaveFormData(obj) {
    var id = 0;
    var frm = obj.closest("form");
    var URL = "../addupdate.ashx?m=" + obj.attr("m");
    var data = frm.serialize();
    var nonchecked = "";
    var table = $("." + obj.attr("target"));
    table.find('input[type=checkbox]').each(function() {
        if (!this.checked) {
            if (nonchecked == "") {
                nonchecked += this.name;
            }
            else {
                nonchecked += "," + this.name;
            }
        }
    });
    URL = URL + "&nc=" + nonchecked;
    $.ajax({
        url: URL,
        type: 'post',
        dataType: 'json',
        async: false,
        data: frm.serialize(),
        success: function(data) {
            id = data;
        }
    });
    return id;
}
function CalculateTaxAmount(obj) {
    try
    {
        var pid = obj.closest("tr").find("#productid").val();
        if (pid.indexOf("-") > 0) {
            var arr = pid.split('-');
            var amount = parseFloat(arr[1]);
            var taxCalculated = false;
            if (parseInt(arr[2]) == 1) {//is tax
                if (arr.length > 3) {
                    if (parseInt(arr[3]) == 4)//manual tax
                    {
                        taxCalculated = true;
                    }
                }
                if (taxCalculated == false) {
                    var subtotal = GetSubTotalForTax(obj.closest(".grid"), obj);
                    obj.closest("tr").find("#amount").val(parseFloat(subtotal * amount / 100).toFixed(2));
                }
            }
        }
    }catch(e){}
}
function PopulateProductPrice(obj) {
    try {
        var pid = obj.closest("tr").find("#productid").val();
        if (pid.indexOf("-") > 0) {
            var arr = pid.split('-');
            var amount = parseFloat(arr[1]);
            if (parseInt(arr[2]) > 0) {
                var subtotal = GetSubTotalForTax(obj.closest(".grid"), obj);
                obj.closest("tr").find("#amount").val(parseFloat(subtotal * amount / 100).toFixed(2));
            }
            else if (amount > 0) {
                var ctrlPrice = null;
                if (obj.closest("tr").find("#rate").length > 0) {
                    ctrlPrice = obj.closest("tr").find("#rate");
                }
                else {
                    ctrlPrice = obj.closest("tr").find("#price");
                }
                ctrlPrice.val(amount);
            }
        }
    } catch (e) { }
}
function GetJsonInputData(dtarget) {
    var data = "";
    var arr = [];
    $('.' + dtarget).each(function() {
        var $this = $(this);
        arr.push({ name: $this.attr('name'), value: $this.val() });
    });
    data = $.param(arr);
    return data;
}
function GetColumnDataFromRow(datarow, column) {
    for (i = 0; i < datarow.length; i++) {
        if (datarow[i]["column"] == column) {
            return datarow[i]["value"];
        }
    }
    return "";
}
function showHideGridActions() {
    $(".pagingbg").hover(function() {
        $(this).find(".grid-action").show();
    }, function() {
        $(this).find(".grid-action").hide();
    });
}
function initPage() {
    $(document).ready(function() {
        $(".homemenu").click(function() {
            $.cookie("pageurl", "home/dashboard.aspx");
        });
        try {
            var pageurl = $.cookie("pageurl");
            if ($.cookie("pageurl") != null && $.cookie("pageurl") != "") {
                //open opened url in tab    
                //$("#ifrmain").attr("src", $.cookie("pageurl"));
            }
        } catch (e) {
        }
    });
}
function setCurrentPage(pageurl) {
    $.cookie("pageurl", pageurl);
}
function freezepage() {
    //var val = $(".freezesettings").attr("FreezeSettings")
    var val = $(".freezesettings").val();    
    try {
        if (val.length > 0) {
            var arr = val.split(',');
            for (i = 0; i < arr.length; i++) {
                Setfreeze(arr[i]);
            }
        }

    } catch (e) {

    }
}
function Setfreeze(arr) {
    var arrInnnerval = arr.split('-');
    var msg = "";
    try {
        if (arrInnnerval.length > 0) {
            //TargetVal
            var Tarval = arrInnnerval[0];
            //CompareVal
            var CmpTarval = $("." + arrInnnerval[1]).val();
            if (Tarval == CmpTarval) {
                $("." + arrInnnerval[2]).remove();              
                msg = arrInnnerval[3];
                if (msg != "") {
                    alert(msg);
                }
            }
        }
    }
    catch (e) {}
}
function focusOnLoad() {

    var isfirst = false;
    $(".form").find("input").each(function() {
        if (!isfirst) {
            var css = ConvertToString($(this).attr("class"));
            if ($(this).attr("disabled") != "disabled" && $(this).css("display") != "none") {
                if (css.indexOf("datepicker") >= 0 || css.indexOf("datetimepicker") >= 0) {
                }
                else {
                    $(this).focus();
                    isfirst = true;
                }
            }
        }
    });
}
$(document).ready(function() {
    //grid view
    $(".advanced-search-filter").click(function() {
        var tblmain = $(this).closest(".tblgridmain");
        tblmain.find(".tradvancedsearch").show();
        tblmain.find(".btnadvanced-search").hide();
        tblmain.find(".advsearch-controls").html("<img src='../images/ajax-loader-fb.gif'/>");
        var m = tblmain.find(".gridtd").attr("m");
        var URL = "../advanced-search.ashx?m=" + m;
        $.ajax({
            url: URL,
            type: 'GET',
            async: true,
            success: function(html) {
                tblmain.find(".advsearch-controls").html(html);
                tblmain.find(".btnadvanced-search").show();
                tblmain.find(".ac").each(function() {
                    setAutoComplete($(this));
                });
                tblmain.find(".datepicker").each(function() {
                    $(this).datepicker({ dateFormat: 'dd-mm-yy' });
                });
                var keyword = $(".h_as_keyword").val();
                if (keyword != "") {
                    var arrkeyword = keyword.split('`');
                    for (i = 0; i < arrkeyword.length; i++) {
                        var arrkeywordctrl = arrkeyword[i].split(':');
                        $("#" + arrkeywordctrl[0]).val(arrkeywordctrl[1]);
                    }
                }
            }
        });

    });
    /*
    $(".as_more").live("click", function() {
        var bgimg = $(this).css("background-image");
        if (bgimg.indexOf("arrow_expand_down") > 0) {
            $(".as_tdadvmore").show();
            $(this).css("background-image", "url(../images/arrow_expand_up.png)");
        }
        else {
            $(".as_tdadvmore").hide();
            $(this).css("background-image", "url(../images/arrow_expand_down.png)");
        }
    });
    */
    $(".btnadvanced-search").click(function(e) {
        var keyword = "";
        $(this).closest(".tblgridmain").find(".as_keyword").each(function() {
            if ($(this).val() != "") {
                if (keyword == "") {
                    keyword = $(this).attr("id") + ":" + $(this).val();
                }
                else {
                    keyword += "`" + $(this).attr("id") + ":" + $(this).val();
                }
            }
        });
        $(".h_as_keyword").val(keyword);
    });
});
function setTabHeight() {
    __SCREEN_HEIGHT = $(window).height();
    var h=__SCREEN_HEIGHT - 110;
    $("#inner").css("height", h);
    $(".tab-if").css("height", h);
}
function fixPrintHeight() {
    $(".blankheight").each(function() {
        fixPrintHeightOfTable($(this));
    });
}
function fixPrintHeightOfTable(obj) {
    var h = ConvertToInt(obj.attr("h"));
    if (h > 0) {
        var tbl = obj.closest("table");
        var c = tbl.find("tr:first").find("td").length;
        for (i = 0; i < c; i++) {
            if (i == c - 1) {
                obj.append("<td style='border-left:solid 1px #000;border-right:solid 1px #000'>&nbsp;</td>");
            }
            else {
                obj.append("<td style='border-left:solid 1px #000'>&nbsp;</td>");
            }
        }
        for (i = 1; i < 1000; i++) {
            if (tbl.height() + i > h) {
                break;
            }
            obj.find("td").css("height", i);
        }
    }
}
function IsMobile() {
    if (window.location.href.indexOf("/mobile/") > 0) {
        return true;
    }
    return false;
}
function loadSelectDeselect() {
    $(".selectdeselect").click(function() {
        var target = $(this).attr("target");
        if ($(this).val() == "Select All") {
            $(this).val("Deselect All");
            $("." + target).prop("checked", true);
        }
        else {
            $("." + target).prop("checked", false);
            $(this).val("Select All");
        }
    });
}
function initStatusButton() {
    $(".btnstatus").click(function() {
        return confirm("Are you sure you want to update the status to " + $(this).val());
    });
}
function initGoBack() {
    $(".back").click(function() {
        window.history.back();
    });
}
function loadDelete() {
//    try {
//        if (__Login_RoleId != "1") {
//            $(".delete").remove();
//        }
//    } catch (e) { }
}
function initPopWindowButton() {
    $(".popwindow").click(function() {
        window.open($(this).attr("url"));
        return false;
    });
}
function initSelectDeselect() {
    $(".selectdeselect").live("click", function() {
        $(".selectdeselect").closest("table").find("input").prop("checked", $(this).prop("checked"));
    });
}
function initDelete() {
    $(".del").live("click", function() {
        if (!confirm("Are you sure you want to delete this data?")) {
            return false;
        }
        var obj = $(this);
        var url = "../delete.ashx?m=" + $(this).attr("m") + "&id=" + $(this).attr("did");
        $.ajax({
            url: url,
            type: 'GET',
            async: true,
            success: function(response) {
                if (response == "1") {
                    alert("Data deleted successfully");
                    obj.closest("tr").remove();
                }
                else {
                    alert("Error occured while processing your request!");
                }
            },
            complete: function() {
            },
            error: function(err, status, jqXHR) {
            }
        });
    });
}
function initAcSearch() {
    $(".acsearch").each(function() {
        $(this).closest("td").append("<img src='../images/search.png' class='imgacsearch hand' title='Advanced Search'/>");
    });
    $(".imgacsearch").live("click", function() {
        var txt = $(this).closest("td").find(".ac");
        var cn = txt.attr("cn");
        var m = txt.attr("m");
        var acsearchcols = txt.attr("acsearchcols");
        var acsearchresultcols = txt.attr("acsearchresultcols");
        var txtid = txt.attr("id");
        var hdnid = $(this).closest("td").find(".hdnac").attr("id");
        var url = "../acsearch/acsearch.aspx?m=" + m + "&cn=" + cn + "&acsearchcols=" + acsearchcols + "&acsearchresultcols=" + acsearchresultcols + "&txtid=" + txtid + "&hdnid=" + hdnid;
        window.open(url, "", "width=800,height=600");
    });
}
function loadDatePicker(obj) {
    $(".datepicker").live("click", function() {
        if ($(this).attr("disabled") != "disabled") {
            $(this).datepicker({ dateFormat: 'dd-mm-yy' });
            if (($(this).attr("class").indexOf("cdate") >= 0 || $(this).attr("class").indexOf("currentdate") >= 0)
                && $(this).val() == "") {
                $(this).datepicker().datepicker('setDate', new Date());
            }
        }
    });
    if (obj == undefined) {
        $(".datepicker").each(function() {
            if ($(this).attr("disabled") != "disabled") {
                $(this).datepicker({ dateFormat: 'dd-mm-yy' });
                if (($(this).attr("class").indexOf("cdate") >= 0 || $(this).attr("class").indexOf("currentdate") >= 0)
            && $(this).val() == "") {
                    $(this).datepicker().datepicker('setDate', new Date());
                }
            }
        });
    }
    else {
        obj.find(".datepicker").each(function() {
            if ($(this).attr("disabled") != "disabled") {
                $(this).datepicker({ dateFormat: 'dd-mm-yy' });
                if (($(this).attr("class").indexOf("cdate") >= 0 || $(this).attr("class").indexOf("currentdate") >= 0)
            && $(this).val() == "") {
                    $(this).datepicker().datepicker('setDate', new Date());
                }
            }
        });
    }
}
function loadDayTimePicker() {
    $(".daytimepicker").each(function() {
        loadDaytimePicker($(this));
    });
    $("form").submit(function() {
        $(".daytimepicker").each(function() {
            var values = "";
            for (i = 1; i <= 7; i++) {
                var val = $(this).parent().find("#day" + i).val();
                if (i == 1) {
                    values = val;
                }
                else {
                    values += "," + val;
                }
            }
            $(this).val(values);
        });
    });
}
function loadMonthYearPicker() {
    $(".monthyearpicker").each(function() {
        loadMonthyearPicker($(this));
    });
    $(".monthyearpicker").focus(function () {
        $(".ui-datepicker-calendar").hide();
        $("#ui-datepicker-div").position({
            my: "center top",
            at: "center bottom",
            of: $(this)
        });
    });
}
function loadMonthyearPicker(obj){
    obj.datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        dateFormat: 'MM yy',
        onClose: function(dateText, inst) { 
            $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
        }
    });
   
}
function loadDaytimePicker(obj) {
    obj.addClass("hidden");
    var values = obj.val();
    var arrVals = values.split(',');
    var html = "<table><tr><td>Mon</td><td>Tue</td><td>Wed</td><td>Thu</td><td>Fri</td><td>Sat</td><td>Sun</td></tr><tr>";
    for (i = 1; i <= 7; i++) {
        var val = "";
        if (values != "") val = arrVals[i - 1];
        html += "<td><input type='text' id='day" + i + "' class='sbox' value='" + val + "'/></td>";
    }
    html += "</tr></table>";
    obj.parent().append(html);
}
function daysInMonth(month, year) {
    return new Date(year, month, 0).getDate();
}
function printReport() {
    $(".print-report").click(function() {
        var rm = $(".gridtd").attr("rm");
        window.open("../utilities/print-report.aspx?rm=" + rm + "&t=" + $(".title").text());
    });
}
function printPage() {
//    $(".print-page").click(function() {
//        window.open("../utilities/print-page.aspx");
    //    });
    //if (parent.__ROLEID != 1) return;
    if ($(".print").length > 0) return;
    var imgPrint = $('<td><img src="../images/print-s.png" class="print-page hand" title="Print"/></td>');
    $(".lightbutton").closest("tr").append(imgPrint);
    imgPrint.click(function() {
    window.open("../utilities/print-page.aspx");
    });
}
function sendEmailReminders() {
    var URL = "triggeremail.aspx";
    $.ajax({
        url: URL,
        type: 'GET',
        async: true,
        success: function(jsonObj) {
        },
        complete: function() {
        },
        error: function(err, status, jqXHR) {
        }
    });
}
function sendWishes() {
    var URL = "reminder.ashx?m=wishes";
    $.ajax({
        url: URL,
        type: 'GET',
        async: true,
        success: function(jsonObj) {
        },
        complete: function() {
        },
        error: function(err, status, jqXHR) {
        }
    });
}
function sendBulkSMS() {
    if (isAllSMSSent()) return;
    var id = $(".bulksmsid").val();
    var URL = "../utilities/bulkemailsms.ashx?id=" + id;
    $.ajax({
        url: URL,
        type: 'GET',
        async: true,
        success: function(jsonObj) {
            try {
                var index = jsonObj.indexOf("{");
                if (index > 0) {
                    jsonObj = jsonObj.substring(index);
                }
                var data = jQuery.parseJSON(jsonObj);
                var totalsent = ConvertToInt(data.totalsent);
                var totalfailed = ConvertToInt(data.totalfailed);
                var totalsms = ConvertToInt($(".totalsms").val());
                var balance = totalsms - totalsent - totalfailed;
                $(".totalsmssent").val(totalsent);
                $(".totalsmssent").closest("td").find(".dval").text(totalsent);
                $(".totalsmsfailed").val(totalfailed);
                $(".totalsmsfailed").closest("td").find(".dval").text(totalfailed);
                $(".totalsmsbalance").val(balance);
                $(".totalsmsbalance").closest("td").find(".dval").text(balance);
            } catch (ex) { }
            if (isAllSMSSent()) {
                alert("All sms have been sent successfully!");
                window.location = "../report/bulksmssentdetail.aspx?bulksmsid=" + id;
            }
            else {
                setTimeout("sendBulkSMS()", 3000);
            }
        },
        error: function(err) {
            if (isAllSMSSent()) {
                alert("All sms have been sent successfully!");
                window.location = "../report/bulksmssentdetail.aspx?bulksmsid=" + id;
            }
            else {
                setTimeout("sendBulkSMS()", 3000);
            }
        }
    });
}
function isAllSMSSent() {
    var totalsms = ConvertToInt($(".totalsms").val());
    var totalsmssent = ConvertToInt($(".totalsmssent").val());
    var totalfailed = ConvertToInt($(".totalsmsfailed").val());
    if (totalsms == (totalfailed + totalsmssent)) {
        return true;
    }
    else {
        return false;
    }
}
function startSendBulkSMS() {
    if (ConvertToInt($(".totalsmsbalance").val()) > 0) {
        $(".edit").hide();
        $(".save").hide();
        $(".delete").hide();
        $(".btnstart").hide();
        $(".totalsmssent").closest("td").find(".dval").addClass("success-text");
        $(".totalsmsfailed").closest("td").find(".dval").addClass("failed-text");
        sendBulkSMS();
    }
    else {
        alert("No more sms to send!");
    }
}
function ajaxCall(url, callback) {
    $.ajax({
        url: url,
        async: true,
        cache: false,
        success: function(response) {
            if (callback != undefined) callback(response);
        }
    });
}
function ajaxCallWithData(url,data, succeeCallback, errorCallback) {
    $.ajax({
        url: url,
        cache: false,
        data:data,
        type:"POST",
        success: function(response) {
            if (succeeCallback != undefined) succeeCallback(response);
        },
        error: function(err) {
            if (errorCallback != undefined) errorCallback(err);
        }
    });
}
function insertString(str, index, substr) {
  if (index > 0) {
    return str.substring(0, index) + substr + str.substr(index);
  }
  return string + this;
}
function FormatAmountComma(amount)
{
    var amount = amount.toString();
    var arr = amount.split('.');
    var amount1 = arr[0];
    var amount2 = "";
    if (arr.length == 2) amount2 = arr[1];
    if (amount1.length <= 3) return amount;
    var isNegative = false;
    if (amount1.indexOf("-")>=0)
    {
        isNegative = true;
        amount1 = amount1.replace("-", "");
    }
    if (amount1.length == 4)
    {
        amount1 = insertString(amount1, 1, ",");
    }
    else if (amount1.length == 5)
    {
        amount1 = insertString(amount1, 2, ",");
    }
    else if (amount1.length == 6)
    {
        amount1 = insertString(insertString(amount1, 1, ","), 4,",");
    }
    else if (amount1.length == 7)
    {
        amount1 = insertString(insertString(amount1, 2, ","), 5,",");
    }
    else if (amount1.length == 8)
    {
        amount1 = insertString(insertString(insertString(amount1, 1, ","), 4,","),7,",");
    }
    else if (amount1.length == 9)
    {
        amount1 = insertString(insertString(insertString(amount1, 2, ","), 5,","),8,",");
    }
    else if (amount1.length == 10)
    {
        amount1 = insertString(insertString(insertString(insertString(amount1, 1, ","), 4,","),7,","),10,",");
    }
    else
    {
        return amount;
    }
    if (amount2 != "")
    {
        amount = amount1 + "." + amount2;
    }
    else
    {
        amount = amount1;
    }
    if (isNegative)
    {
        amount = "-" + amount;
    }
    return amount;
}
function initMultiSelect(){
    $(".ddlmultiselect").live("change", function(){
        if($(this).hasClass("disabled")) return;
        if($(this).val()!="0")
        {
            var td = $(this).closest("td");
            var val = $(this).val();
            var vals = td.find(".hdn").val();
            var arrval = vals.split(',');
            for(var i=0;i<arrval.length;i++)
            {
                if(val==arrval[i])
                {
                    alert("Already selected");
                    $(this).val("0");
                    return;
                }
            }
            var selectedText = $(this).find("option:selected").text();
            if(vals == "")
            {
                vals=val;
            }
            else
            {
                vals+=","+val;
            }
            if(td.find(".multicheckbox-selected-text").length == 0) 
            {
                td.append('<div class="multicheckbox-selected-text" style="max-width:300px;padding-top:15px;"></div>');
            }
            var multiCheckDiv = td.find(".multicheckbox-selected-text");
            var multiSelectHtml = '<div class="mchkjs-selected-val mchkjs-selected-val-'+val+'" value="'+val+'"><div class="mchkjs-text">'+selectedText
                                    +'</div><div class="mchkjs-close" style="display: block;">x</div></div>';
            multiCheckDiv.append(multiSelectHtml);
            $(this).val("0");
            td.find(".hdn").val(vals);
        }
    });
    $(".mchkjs-close").live("click", function() {   
        var val=$(this).closest(".mchkjs-selected-val").attr("value");
        var td=$(this).closest("td");
        var hdn = td.find(".hdn");
        if(hdn.length == 0) hdn = td.find(".hdnac");
        var values = hdn.val();
        var arr=values.split(',');
        var vals="";
        for(var i=0;i<arr.length;i++)
        {
            if(arr[i]!=val)
            {   
                if(vals=="")
                {
                    vals=arr[i];
                }
                else{
                    vals+=","+arr[i];
                }
            }
        }
        td.find(".hdn").val(vals); 
        $(this).closest(".mchkjs-selected-val").remove();
    });
    $(".ddlmultiselect").each(function(){
        var isenabled = true;
        var ddl = $(this);
        var td = $(this).closest("td");
        var val = $(this).val();
        var vals = td.find(".hdn").val();
        if(vals=="")return;
        var arrval = vals.split(',');
        if($(this).hasClass("disabled"))isenabled=false;
        if(td.find(".multicheckbox-selected-text").length == 0) 
        {
            td.append('<div class="multicheckbox-selected-text" style="max-width:300px;padding-top:15px;"></div>');
        }
        var multiCheckDiv = td.find(".multicheckbox-selected-text");
        for(var i=0;i<arrval.length;i++)
        {
            var text = ddl.find("option[value='"+arrval[i]+"']").text();
            var closehtml = '<div class="mchkjs-close" style="display: block;">x</div>';
            if(!isenabled) closehtml="";
            var multiSelectHtml = '<div class="mchkjs-selected-val mchkjs-selected-val-'+val+'" value="'+val+'"><div class="mchkjs-text">'+text
                                    +'</div>'+closehtml+'</div>';
            multiCheckDiv.append(multiSelectHtml);
        }
    });
}
function initConfirmButton(){
    $(".confirm").click(function(){
        var msg = "Are you sure you want to proceed this action?";
        if($(this).attr("confirmmsg")!=undefined)msg=$(this).attr("confirmmsg");
        return confirm(msg);
    });
}
function intiCloseDialog() {
    $(".closedialog").live("click", function() {
        if ($(this).closest(".dialog").length > 0) 
        {
            $(this).closest(".dialog").hide();
            if($(this).closest(".dialog").attr("dynamic")=="true")
            {
                $(this).closest(".dialog").remove();
            }
        }
        $(".fade-page").hide();
        $(".fade-page", window.top.document).hide();
    });
}
function showDialog(dialog, title) {
    var urlprefix = "../";
    if(__LOGIN_USERID == "0") urlprefix = "";
    var tr = $("<tr><td class='dialog-title' colspan='5'>" + title + " <span style='float:right'><img src='"+urlprefix+"images/close.png' class='closedialog hand'/></span></td></tr><tr><td>&nbsp;</td></tr>");
    if (dialog.find(".dialog-title").length == 0) {
        tr.prependTo(dialog.find("tbody:first"));
    }
    var left = $(window).width() / 2 - dialog.width() / 2;
    dialog.css("left", left);
    dialog.show();
}
function initOutsideDivClick() {
    $(document).mouseup(function(e) {
        if (_outsideClickDiv != null && _outsideClickDiv.length > 0) {
            if (!_outsideClickDiv.is(e.target) && _outsideClickDiv.has(e.target).length === 0) {
                _outsideClickDiv.hide();
                _outsideClickDiv = null;
                return false;
            }
        }
    });
}
function intiCurveTab()
{
    $(".curve-tab li").live("click",function(){
        $(this).parent().find("li").each(function(){
            $("."+$(this).attr("target")).hide();
        });
        $("."+$(this).attr("target")).show();
        $(this).parent().find("li").removeClass("curve-tab-active");
        $(this).addClass("curve-tab-active");
        var ul = $(this).parent();
        if(ul.hasClass("jq-maintab"))
        {
            $(".jq-hdnmaintabid").val($(this).index());
        }
    });
    if($(".jq-maintab").length > 0)
    {
        var maintabid = $(".jq-hdnmaintabid").val();
        if(maintabid == "") maintabid = "0";
        var selectedli = $(".jq-maintab").find(".curve-tab-active");
        $("."+selectedli.attr("target")).show();
    }
}
function initClientAddPageModal()
{
    $(".jq-common-btnaddmodal").live("click",function(){
        var mn = $(this).attr("mn");
        var url = getUrlPrefix() + "commonaddmodalpage.aspx?modal="+mn;
        var modal = loadAjaxModal($(this), url);
        modal.attr("did","0");
        return false;
    });
    $(".jq-common-btneditmodal").live("click",function(){
        var mn = $(this).attr("mn");
        var id = $(this).attr("did");
        var url = getUrlPrefix() + "commonaddmodalpage.aspx?modal="+mn+"&id="+id+"&clientid="+_clientId;
        var modal = loadAjaxModal($(this), url);
         modal.attr("did", id);
        return false;
    });
    $(".jq-common-btnaddmodal-save").live("click",function(){
        var obj = $(this);
        if(obj.prop("saving") == "true") return false;
        var modal = obj.closest(".jq-modal");
        if(!isValidModalForm(modal))return false;
        var modalname = modal.attr("mn");
        var url = getUrlPrefix() + "commonhandler.ashx?m=save-"+modalname+"&clientid="+_clientId+"&id="+modal.attr("did");
        var data = modal.find("select, textarea, input").serialize();
        showLoading();
        ajaxCallWithData(url, data, function(response){
            hideLoading();
            obj.prop("saving", "false");
            var json = $.parseJSON(response);
            if(json.status == "ok")
            {   
                if(json.msg != undefined)
                {
                    Alert(json.msg);
                }
                //window.location.href = window.location.href;
                if(obj.attr("targetgrid")!=undefined)
                {
                    var grid = $("."+obj.attr("targetgrid"));
                    bindCommonAjaxContent(grid, grid.attr("m"));
                }
                hideDialog(modal);
            }
            else if(json.status == "validation")
            {   
                Alert(json.msg);
            }
            else
            {
                Alert("Error occurred!");
            }
        });
        return false;
    });
}
function loadAjaxModal(obj, url, callback)
{
    var title = obj.attr("modaltitle");
    if(title == undefined) title = obj.attr("title");
    if(title == undefined) title = obj.attr("value");
    var width = obj.attr("modalwidth");
    var height = obj.attr("modalheight");
    var modaldiv = getEmptyModal(width, height);
    var mn = obj.attr("mn");
    modaldiv.attr("mn", mn);
    showDialog(modaldiv, title);
    showLoading();
    loadModalPageContent(modaldiv.find(".jq-modal-content"),url,function(){
        hideLoading();
        if(callback!=null && callback!=undefined) callback();
        loadDatePicker(modaldiv);
        loadAutoComplete(modaldiv);
    });
    return modaldiv;
}
function getEmptyModal(width, height)
{
    var style = "";
    if(width != undefined)
    {
        style = " style='width:"+width+";height:"+height+";'";
    }
    var html = "<div class='dialog jq-modal'"+style+" dynamic='true'>"+
                    "<table width='100%'>"+
                        "<tr>"+
                            "<td class='jq-modal-content'><div class='loading'></div></td>"+
                        "</tr>"+
                    "</table>"+
                "</div>";
    var modal = $(html);
    $("body").append(modal);
    return modal;
}
function isValidModalForm(modal)
{
    var isvalid = true;
    modal.find(".required").each(function(){
        if(isvalid && $(this).val().trim()=="")
        {
            $(this).focus();
            isvalid = false;
        }
    });
    if(isvalid)
    {
        if(!validatePage())return false;
    }
    return isvalid;
}
function loadModalPageContent(div, url, callback)
{
    ajaxCall(url,function(response){
        var html = $(response).find("#jq-page-content").html();
        div.html(html);
        if(callback!=undefined)callback();
    });
}
function loadAutoComplete(div)
{
    div.find(".ac").each(function(){
        setAutoComplete($(this));
    });
}
function showLoading() {
    if($(".loader", window.top.document).length > 0)
    {
        $(".loader", window.top.document).show();
    }
    else
    {
        $(".loader").show();
    }
}
function hideLoading() {
    if($(".loader", window.top.document).length > 0)
    {
        $(".loader", window.top.document).hide();
    }
    $(".loader").hide();
}
function initCommonAjaxContent()
{
    $(".jq-common-ajax-content").live("click", function(){
        loadCommonAjaxContent($(this));
    });
    $(".jq-common-ajax-content-onload").each(function(){
        loadCommonAjaxContent($(this));
    });
}
function loadCommonAjaxContent(obj)
{
    var contentdiv = $("."+obj.attr("contentdiv"));
    if(contentdiv.length == 0) contentdiv = obj;
    var m = obj.attr("m");
    bindCommonAjaxContent(contentdiv,m);     
}
function bindCommonAjaxContent(contentdiv, m)
{
    var url = getUrlPrefix() + "commonhandler.ashx?m="+m+"&clientid="+_clientId;
    ajaxCall(url,function(response){
        contentdiv.html(response);
    });       
}
function hideDialog(modal) {
    modal.hide(); 
    if(modal.attr("dynamic")=="true")
    {
        modal.remove();
    }
}
function getUrlPrefix()
{
    var urlprefix = "../";
    var url = window.location.href;
    if(url.indexOf("exportexposureportal")>0)return urlprefix;
    if(__LOGIN_USERID == "0") urlprefix = "";
    return urlprefix;
}