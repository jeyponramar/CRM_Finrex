var _iscalcFocus = false;
var _freezeCount = 0;
var _isLiverateActive = false;
$(document).ready(function() {
    if(_isLiverateActive) liverate(true);
    calculateRate();
    initBrokenDateCalc();
    initLiveRateAlert();
    initMultiSelectDate();
    setTimeout("initBroadcastMessage()", 2000);
    setTimeout("setStaticTick()", 5000);
    initLiveRateMore();
    initForwardRateYear();
    initConfigLiverate();
    $(".liverate").click(function() {
        navigator.clipboard.writeText($(this).attr("rid"));
        if ($(".divcalculate").length == 0) return;
        if (_iscalcFocus) {
            var c = $(this).attr("c");
            var arr = c.split('_');
            var code = "#" + arr[0] + "[" + arr[1] + "," + arr[2] + "]";
            $(".txtcalc").val($(".txtcalc").val() + " " + code);
            $(".txtcalc").focus();
        }
        else {
            $(".txtcalctarget").val($(this).attr("rid"));
        }
    });
    $(".btncalc").click(function() {
        if ($(".txtcalctarget").val() == "") {
            $(".txtcalctarget").focus();
            return false;
        }
        if ($(".txtcalc").val() == "") {
            $(".txtcalc").focus();
            return false;
        }
    });
    $(".txtcalc").focus(function() {
        _iscalcFocus = true;
    });
    $(".txtcalctarget").focus(function() {
        _iscalcFocus = false;
    });
    $("#btnclearcalc").click(function() {
        $(".txtcalc").val("");
        $(".txtcalctarget").val("");
    });
    $("#btnclosecalc").click(function() {
        $(".divcalculate").hide();
    });
});
function getRateVal(obj)
{
    //if(obj.attr("rate") != undefined)return obj.attr("rate");
    var val = obj.text().trim().replace(/[^\x00-\x7F]/g, "");//remove NON-ASCII special chars
    return val;
}
function calculateRate() {
    var calccounter = 0;
    $(".calc").each(function() {
        try {
            calccounter++;
            if($(this).attr("istick")=="1" && $(this).attr("calculated")=="true")return;
            if($(this).attr("istick")=="0")
            {
                $(this).attr("calculated","true");
            }
            var calc = $(this).attr("calc");
            var sc = $(this).attr("sc");
            var rid = $(this).attr("rid");
            if (rid == "266") {
                rid = rid;
            }
            //console.log(rid);
            if (calc!=undefined && calc.indexOf("self") >= 0) return;
            var val = "";
            var isexists = true;
            var exitcounter = 0;
            while (isexists) {
                if(exitcounter > 10) 
                {
                    isexists = false;
                    break;
                }
                exitcounter++;
                
                if(calc.indexOf("&#") >= 0)
                {
                    //console.log("rid="+rid+";"+calc);
                    isexists = false;
                    break;
                }
                if (calc.indexOf("#") >= 0) {
                    var startIndex = calc.indexOf("#");
                    var endIndex = calc.indexOf("]");
                    var c = calc.substring(startIndex + 1, endIndex + 1);
                    var ctCalc = "#" + c;
                    var arr1 = c.split('[');
                    var scode = arr1[0];
                    var temp = arr1[1].replace("]", "");
                    var arr2 = temp.split(',');
                    var row = arr2[0];
                    var col = arr2[1];
                    var d = getRateVal($("." + scode + "_" + row + "_" + col+":first"));
//                    if(c=="EURINR[1,3]" || c=="GBPINR[1,3]" || c=="JPYINR[1,3]")
//                    {
//                        d=$("#jq-spotdate").text();
//                    }
                    var data = d;
                    calc = calc.replace(ctCalc, d);
                }
                else {
                    isexists = false;
                }
            }
            isexists = true;
            //for date diff
            exitcounter=0;
            while (isexists) {
                if(exitcounter > 10) break;
                exitcounter++;
                if (calc.indexOf("DATEDIFF") >= 0) {
                    var datestart = calc.indexOf("DATEDIFF");
                    var dateend = calc.indexOf(")");
                    var dates = calc.substring(datestart + 9, dateend);
                    var arrdates = dates.split('-');
                    var date1 = new Date(arrdates[1] + "-" + arrdates[0] + "-" + arrdates[2]);
                    var date2 = new Date(arrdates[4] + "-" + arrdates[3] + "-" + arrdates[5]);
//                    if($(this).attr("rid")=="370")
//                    {
//                        alert(date1);
//                        alert(date2);
//                    }
                    var timeDiff = Math.abs(date1.getTime() - date2.getTime());
                    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
                    var datesformula = calc.substring(datestart, dateend + 1);
                    calc = calc.replace(datesformula, diffDays);
                }
                else {
                    isexists = false;
                }
            }
            $(this).attr("calres", calc);
            var newdata = ConvertToDouble(eval(calc));
            if ((newdata + "").indexOf("Infinity") >= 0) {
                newdata = "0";
            }
            if ((newdata + "").indexOf(".") >= 0) {
                var arrd = (newdata + "").split('.');
                if (arrd[1].length > 4) {
                    newdata = parseFloat(newdata).toFixed(4);
                }
            }
            setUpDownStatus($(this), newdata);
        }
        catch (e1) {
            e1 = e1;
        }
    });
}
function liverate(isAll) {
    var URL = "liverate.ashx?a=s";
    if (isAll) URL += "&all=1";
    //$(".calc").attr("calculated","false");
    $.ajax({
        url: URL,
        type: 'GET',
        async: true,
        cache:false,
        success: function(json) {
            try {
                if (json == "Session Expired") {
                    window.location = "customerlogin.aspx";
                }
                if(json!="")
                {
                    var data = jQuery.parseJSON(json);
                    for (i = 0; i < data.length; i++) {
                        var rid = data[i].rid;
                        var rate = data[i].cr;
                        $(".rate").each(function() {
                            if ($(this).attr("rid") == rid) {
                                var calc = $(this).attr("calc");
                                if (calc!=undefined && calc.indexOf("self") >= 0) {
                                    var arr1=rate.toString().split('.');
                                    var digits = 0;
                                    if(arr1.length==2)digits=arr1[1].length;
                                    
                                    if(calc=="self/100")
                                    {
                                        rate = rate / 100;
                                    }
                                    else if(calc=="self/1000")
                                    {
                                        rate = rate / 1000;
                                    }
                                    if(digits>0)rate=parseFloat(rate).toFixed(digits);
                                    $(this).attr("calculated","true");
                                    calc = calc.replace("self", rate);
                                }
                                $(this).attr("rate",rate);
                                if($(this).hasClass("calc"))
                                {
                                    $(this).attr("calculated","false");
                                }
                                setUpDownStatus($(this), rate);
                            }
                        });
                    }
                    calculateRate();
                }
                _freezeCount++;
                if(_freezeCount==5)
                {
                    _freezeCount = 0;
                    setStaticTick();
                }
            } catch (e) { }
        },
        complete: function() {
            setTimeout("liverate(false)", 1000);
        }
    });
}
function setStaticTick()
{
    $(".rate").each(function(){
        if($(this).attr("istick")=="1")
        {
            var rate = ConvertToDouble($(this).text());
            var prate = ConvertToDouble($(this).attr("prate"));
            if($(this).closest(".repeater").closest("tr").prev().find(".title").text().indexOf("INDICES")>=0)
            {
                rate = parseFloat(rate).toFixed(2);
                prate = parseFloat(prate).toFixed(2);
            }
            if(prate==rate)
            {
                $(this).removeClass("rate-down").removeClass("rate-up");
            }
        }
    });
    //setTimeout(function(){setStaticTick()},5000);
}
function setUpDownStatus(obj, rate) {
    if(!isNaN(rate))
    {
        rate = parseFloat(rate);
        var prevrate = ConvertToDouble(obj.text());
        obj.attr("prate", prevrate);
        if(obj.attr("istick")=="1")
        {
            if (rate > prevrate) {
                obj.removeClass("rate-down").addClass("rate-mid");
                setTimeout(function(){
                    obj.removeClass("rate-mid").addClass("rate-up");
                },500);
            }
            else if (rate < prevrate) {
                obj.removeClass("rate-up").addClass("rate-mid");
                setTimeout(function(){
                    obj.removeClass("rate-mid").addClass("rate-down");
                },500);
            }
        }
    }
    obj.text(rate);
    
}
function bindSpotDate() {
    var date = "";
    var rate = "";
    if ($(".USDINR_1_3").length > 0) {
        date = $(".USDINR_1_3").text();
        rate = $(".USDINR_1_1").text();
    }
    if ($(".EURINR_1_3").length > 0) {
        date = $(".EURINR_1_3").text();
        rate = $(".EURINR_1_1").text();
    }
    if ($(".GBPINR_1_3").length > 0) {
        date = $(".GBPINR_1_3").text();
        rate = $(".GBPINR_1_1").text();
    }
    if ($(".JPYINR_1_3").length > 0) {
        date = $(".JPYINR_1_3").text();
        rate = $(".JPYINR_1_1").text();
    }
    if ($(".EURUSD_1_3").length > 0) {
        date = $(".EURUSD_1_3").text();
        rate = $(".EURUSD_1_1").text();
    }
    if ($(".GBPUSD_1_3").length > 0) {
        date = $(".GBPUSD_1_3").text();
        rate = $(".GBPUSD_1_1").text();
    }
    if ($(".USDJPY_1_3").length > 0) {
        date = $(".USDJPY_1_3").text();
        rate = $(".USDJPY_1_1").text();
    }
    $("#jq-spotdate").text(date);
    $("#jq-spotrate").text(rate);
}
function initBrokenDateCalc()
{
    $("#broken-sportdate").text($(".jq-USDINR-spotdate").text());
    $(".mnu-brokendatecalc").click(function() {
        $(".jq-multidate-selected-items").html("");
        $("#broken-result").html("");
        $("#broken-txtbdate").val("");
        var calctype = $(this).attr("calctype");
        $("#tblbrokendatecalc").attr("calctype", calctype);
        $("#tblbrokendatecalc").dialog({ width: 900, height: 600});
        bindCurrencyMargin($("#tblbrokendatecalc").find(".jq-ddlcurrency-margin"));
    });
//    $(".mnu-cashconversioncalc").click(function() {
//        $("#cashconversioncalc-result").html("");
//        $("#cashconversioncalc-txtdate").val("");
//        $("#tblcashconversioncalc").dialog({ width: 400, height: 500 });
//    });
    $("#broken-btncalc").click(function(){
        var currency = $("#broken-ddlcurrency").val();
        var bdate = $("#broken-txtbdate").val();
        if(bdate=="")
        {
            alert("Please select date");
            $("#broken-txtbdate").focus();
            return;
        }
        var spotDateCss = "jq-"+$("#broken-ddlcurrency").find("option:selected").text().toLowerCase()+"-spotdate";
        var spotdate = $(".jq-USDINR-spotdate").text();//$("."+spotDateCss).text();
        var isexport = false;
        if($(".broken-covertype").find("input:first").is(":checked"))
        {
            isexport = true;
        }
        $("#broken-sportdate").text(spotdate);
        var margin = $(".jq-brokendatecalc-margin").val();
        var URL = "brokendatecalc.ashx?sdate="+spotdate+"&bdate="+bdate+"&ie="+isexport+"&c="+currency+"&margin="+margin;
        $.ajax({
            url:URL,
            isAsync:true,
            cache:false,
            success:function(response)
            {
                if(response.indexOf("message : ")==0)
                {
                    alert(response.replace("message : ",""));
                    return;
                }
                $("#broken-result").html(response);
            }
        });
    });
}
function initBroadcastMessage()
{
    var URL = "broadcast.ashx?m=broadcast";
    $.ajax({
            url:URL,
            isAsync:true,
            cache:false,
            success:function(response)
            {
                setTimeout("initBroadcastMessage()", 240000);//4mins
                if(response=="")
                {
                    return;
                }
                $("#jq-task-notification-data").html(response);
                var panel = $(".jq-broadcast-panel");
                panel.css("right","-350px");
                panel.show();
                panel.animate(
                {
                    right:"+=350"
                },500,function(){});
            },
            error:function(e)
            {
                setTimeout("initBroadcastMessage()", 10000);
            }
        });
    $(".jq-close-broadcast").click(function(){
        var panel = $(".jq-broadcast-panel");
        panel.css("right","0px");
        panel.animate(
        {
            right:"-=350"
        },500,function(){panel.hide();});
    });
}
function initLiveRateAlert() {
    $(".set-alert").click(function() {
        var rid = $(this).attr("rid");
        var url = "setliveratealert.aspx";
        if (rid != undefined) {
            url += "?rid=" + rid;
        }
        else if ($(this).attr("aid") != undefined) {
            url += "?id=" + $(this).attr("aid");
        }
        var w = ConvertToInt($("#divConfigLiveRateAlert").attr("w"));
        var h = ConvertToInt($("#divConfigLiveRateAlert").attr("h"));

        $("#divConfigLiveRateAlert").dialog
            ({ width: w, height: h });
        $("#divConfigLiveRateAlert").find("iframe").attr("src", url);
        $("#divConfigLiveRateAlert").find("iframe").css("width", w - 50);
        $("#divConfigLiveRateAlert").find("iframe").css("height", h - 50);
    });
}
function hideLiveRateAlert() {
    $("#divConfigLiveRateAlert").dialog("close");
    if (window.location.toString().toLowerCase().indexOf("viewalerts.aspx") > 0) {
        window.location.reload();
    }
}
function initMultiSelectDate() {
    $(".jq-multidate").live("change", function() {
        var panel = $(this).closest(".jq-multidate-panel");
        var maxitems = ConvertToInt(panel.attr("maxitems"));
        var hdn = panel.find(".jq-hdn");
        if (maxitems > 0) {
            if (hdn.val().split(',').length >= maxitems) {
                $(this).val("");
                alert("You can not add more than " + maxitems);
                return;
            }
        }
        addMultiSelectDate(panel, $(this));
        setMultiSelectDateVal(panel);
        $(this).val("");
    });
    $(".jq-multidate-remove").live("click", function() {
        removeMultiSelectDate($(this));
    });
    $(".jq-multidate-selected-item").live("mouseover", function() {
        $(this).find(".jq-multidate-remove").show();
    });
    $(".jq-multidate-selected-item").live("mouseleave", function() {
        $(this).find(".jq-multidate-remove").hide();
    });
}
function addMultiSelectDate(panel, obj) {
    var hdn = panel.find(".jq-hdn");
    var div = panel.find(".jq-multidate-selected-items");
    if (div.length == 0) panel.append("<div class='jq-multidate-selected-items'></div>");
    div = panel.find(".jq-multidate-selected-items");
    div.find(".jq-multidate-selected-val").each(function() {
        if ($(this).text() == obj.val()) {
            $(this).closest(".jq-multidate-selected-item").remove();
        }
    });
    div.append("<div class='jq-multidate-selected-item'><span class='jq-multidate-selected-val'>" + obj.val() + "</span><div class='jq-multidate-remove'>x</div></div>");
}
function removeMultiSelectDate(obj) {
    var item = obj.closest(".jq-multidate-selected-item");
    var panel = item.closest(".jq-multidate-panel");
    item.remove();
    setMultiSelectDateVal(panel);
}
function setMultiSelectDateVal(panel) {
    var hdn = panel.find(".jq-hdn");
    var vals = "";
    panel.find(".jq-multidate-selected-val").each(function() {
        if (vals == "") {
            vals = $(this).text();
        }
        else {
            vals += ","+ $(this).text();
        }
    });
    hdn.val(vals);
}
function initLiveRateMore() {
    $(".jq-liverate-expand").unbind().click(function() {
        var target = $(this).attr("target");
        var morediv = $(this).closest(".repeater").find(".jq-liverate-more-rate");
        if(target!=undefined)morediv=$("."+target);
        if ($(this).attr("src") == "images/arrow_expand_right.png") {
            $(this).attr("src", "images/arrow_expand_left.png");
            morediv.show();
        }
        else {
            $(this).attr("src", "images/arrow_expand_right.png");
            morediv.hide();
        }
    });
}
function initForwardRateYear(){
    if($(".jq-forwardrate-prev").length==0)return;
    showForwardRateYear(0);
    $(".jq-forwardrate-prev").click(function(){
        var year = ConvertToInt($(this).closest("tr").attr("y"));
        year--;
        if(year == 0)
        {
            $(this).hide();
        }
        $(".jq-forwardrate-next").show();
        $(this).closest("tr").attr("y",year);
        showForwardRateYear(year);
        return false;
    });
    $(".jq-forwardrate-next").click(function(){
        var year = ConvertToInt($(this).closest("tr").attr("y"));
        year++;
        if(year>=4)
        {
            year=4;
            $(this).hide();
        }
        $(".jq-forwardrate-prev").show();
        $(this).closest("tr").attr("y",year);
        showForwardRateYear(year);
        return false;
    });
}
function showForwardRateYear(year){
    if(year == 0)
    {
        $(".jq-forwardrate-next-msg").hide();
    }
    else
    {
        $(".jq-forwardrate-next-msg").show();
    }
    $(".jq-tblforwardrate").find(".repeater").find("tr").each(function(){
        var startIndex = year*12+2;
        var endIndex = startIndex+11;
        var index=0;
        $(this).find("td").hide();
        $(this).find("td").each(function(){
            if((index==0 || index == 1) || (index >= startIndex && index <= endIndex)){
                $(this).show();
            }
            index++;
        });
    });
}
function bindCurrencyMargin(objcurreny)
{
    var currency = objcurreny.val();
    if(currency=="0")return;
    var url = "finstationhandler.ashx?m=getcurrencymargin&cid="+currency;
    $.ajax({
        url:url,
        success:function(response){
            var json = $.parseJSON(response);
            $(".jq-currency-margin").val(json.margin);
        }
    });
}
function initConfigLiverate()
{
    $(".jq-config-liverate").click(function(){
        $(".jq-liverate-configuser-modal").dialog({ width: "300",height:"400",title:$(this).attr("title")});
        var url = "finstationhandler.ashx?m=getliverateuserconfig&apptype=1&ctype="+$(this).attr("ctype");
        $(".jq-liverate-configuser-modal").attr("ctype",$(this).attr("ctype"));
        $.ajax({
            url:url,
            success:function(response){
                $(".jq-liverate-configuser").html(response);
            }
        });
    });
    $(".jq-liverate-config-save").click(function(){
        var cids = "";
        $(".jq-chk-userconfig-currency").each(function(){
            if($(this).is(":checked"))
            {
                if(cids == "")
                {
                    cids = $(this).attr("cid");
                }
                else
                {
                    cids += "," + $(this).attr("cid");
                }
            }
        });
        if(cids == "")
        {
            alert("Please choose any item.");
            return;
        }
        var arr = cids.split(',');
        var modal = $(this).closest(".jq-liverate-configuser-modal");
        var maxCurrencyCount = ConvertToInt(modal.find(".jq-expance-collapse").attr("maxcurrency"));
        if(arr.length > maxCurrencyCount)
        {
            alert("You can not choose more than "+maxCurrencyCount+" items.");
            return;
        }
        var url = "finstationhandler.ashx?m=saveliverateuserconfig&apptype=1&ctype="+$(this).closest(".jq-liverate-configuser-modal").attr("ctype")+"&cids="+cids;
        $.ajax({
            url:url,
            success:function(response){
                if(response=="ok")
                {
                    alert("Configuration saved successfully!");
                    window.location.href = window.location.href;
                }
            }
        });
    });
}
function initLiverateAdmin()
{
    $(".jq-currentdate").click(function(){
        if($(this).attr("highlighted") == "true")
        {
            $(".apirate").removeClass("apirate-active");
            $(".xlrate").removeClass("xlrate-active");
            $(".calc").removeClass("calc-active");
            $(this).attr("highlighted","false");
        }
        else
        {
            $(".apirate").addClass("apirate-active");
            $(".xlrate").addClass("xlrate-active");
            $(".calc").not(".apirate").addClass("calc-active");
            $(this).attr("highlighted","true");
        }
    });
}