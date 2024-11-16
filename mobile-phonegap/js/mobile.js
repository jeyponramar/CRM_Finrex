//var _RPLUS_API = "";
var _isLiveRateLoaded = false;
var _liveViewType = "";
var _page;
var _requestCounter = 0;
var _isFinstationView = true;
var _isMetalLiveRateLoaded = false;
var _sessionId = "";
$(document).ready(function(){

//    if (navigator.userAgent.match(/(iPhone|iPod|iPad|Android|BlackBerry|IEMobile)/)) {
//      document.addEventListener("deviceready", onDeviceReady, false);
//    } else {
      load_mobile(); //this is for the browser
    //}
}); 
function load_mobile(){
    //$("#username").val("cookie session: " + getCookie("sessionid"));
    initMobile();
    initPushNotification();
}
document.addEventListener('init', function(event) {
    var mobileVersion = getQueryString("mversion");
    var device = getQueryString("device");
    if(device == "" || device == "android")
    {
        if(mobileVersion == "")
        {
            Alert("Latest version is available in play store, please update the app!");
        }
    }
    var page = event.target;
    //_page = $(page);
    setCurrentPage();
    var pageId = page.id;
    _liveViewType = "";
    if (page.id == 'dashboard-page') {
        if(!_isLiveRateLoaded)
        {
            if($("#dashboard-page").length>0)
            {
                _liveViewType = "spotrate";
                bindLiveRate();
                setInterval(function(){bindLiveRate();},3000);
                _isLiveRateLoaded = true;
            }
        }
        bindSpotRate();
    }
    else if(pageId == "forward-page" || pageId == "indices-page" || pageId == "metal-indices-commodities-page") {
        bindLiveRate();
    }
    else if(page.id == "brokendatecalc-page"){
        loadBrokenDateCalc();
    }
    else if(page.id == "setalert-page"){
        loadAlertEmailMobileNos();
    }
    else if(page.id == "config-currency"){
        loadConfigCurrency();
    }
    else if(page.id == "logout-page"){
        logout();
    }
    else if (page.id == 'metal-dashboard-page') {
        _liveViewType = "lme3mforward";
        if(!_isLiveRateLoaded)
        {
            bindLiveRate();
            setInterval(function(){bindLiveRate();},3000);
            _isLiveRateLoaded = true;
        }
    }
    else if(pageId == "pushnotification-page")
    {
        bindPushNotificationList(false);
    }
    loadImage();
});
function logout(){
    deleteCookie("sessionid");
    //redirect("login.html","FinIcon");
    window.location.reload();
    //window.location = window.location.href;
}
//tab click
document.addEventListener('postchange', function(event) {
    setCurrentPage();
});
function initMobile()
{   
    loadStaticHtml();
    loadCSS();
    initBrokenDateCalc();
    loadDatePicker();
    initSetAlert();
    initMetal();
    initExpandCollapse();
    
    $(document).on("click",".btnsubmit",function(){
        postFormRequest($(this));
    });
    $(document).on("click",".lnkmenu",function(){
        openMenu($(this));
    });
    $(document).on("click",".lnkmenu-metstation",function(){
        _isFinstationView = false;
        setMainView();
        redirect("metal-dashboard.html", "MetStation");
    });
    $(document).on("click",".lnkmenu-finstation",function(){
        _isFinstationView = true;
        setMainView();
        redirect("dashboard.html", "FinIcon");
    });
    $(document).on("click","ons-tab",function(){
        //alert($(this).attr("page"));
    });
    $(document).on("click", "#btnalertmodalok", function() {
        $(".alert-modal").hide();
        $(".fade").hide();
    });

    $(document).on("click",".lnk",function(){
        var href = $(this).attr("href");
        var url = href;
        var arr = url.split("?_=");
        if(arr.length > 0)
        {
            url = arr[0];
            var index = arr[1].indexOf("/");
            url = arr[0] + arr[1].substring(index);
        }
        window.open(url, '_system');
        return false;
    });
}

function setMainView()
{
    if(_isFinstationView)
    {
        $("#jq-metstation-menu").hide();
        $("#jq-finicon-menu").show();
    }
    else
    {
        $("#jq-metstation-menu").show();
        $("#jq-finicon-menu").hide();
    }
}
function getCurrentPage(){
    return document.querySelector('#myNavigator').getCurrentPage().page;
}
function openMenu(obj){
    var url = obj.attr("href");
    if(url == undefined)url = obj.attr("url");
    var title = obj.text();
    var m = obj.attr("m");
    var mid = obj.attr("mid");
    document.querySelector('#myNavigator').pushPage(url, { data: { title: title, m: m, mid: mid} });
    document.querySelector('ons-splitter-side').close();
    
    setTimeout(function(){
    var pageid = url.replace(".html","")+"-page";
    $("#"+pageid).find("ons-tab").each(function(){
        if($(this).attr("class").indexOf("active") >= 0){
            //_page = $("#"+$(this).attr("page").replace(".html","")+"-page");
        }
    });
    },500);
    setTimeout(function(){
    for(var i=0;i<document.querySelector('#myNavigator').pages.length;i++){
        var p = $(document.querySelector('#myNavigator').pages[i]);
        if(p.css("display")=="block"){
            //_page = p;
        }
    }
    },100);
}
function setCurrentPage(){
    for(var i=0;i<document.querySelector('#myNavigator').pages.length;i++){
        var p = $(document.querySelector('#myNavigator').pages[i]);
        if(p.css("display")=="block"){
            _page = p;
        }
    }
}
function loadStaticHtml()
{
    var localSessionId = getSessionId();
    var url = _RPLUS_API + "/mobile-phonegap/static-html.aspx?sessionid="+localSessionId;
    var device = getQueryString("device");
    var onesignalid = getQueryString("onesignalid");
    var isnotification = getQueryString("isnotification");
    url = url + "&device=" + device+"&onesignalid="+onesignalid+"&isnotification="+isnotification;
    //console.log(url);
    $.ajax({
        url: url,
        cache: false,
        success: function(response) {
            var index = response.indexOf("<page-start");
            response = response.substring(index);
            response = response.replace("</form></body></html>", "");
            $("body").append(response);
            $("#menu").html($("#jq-menu_html").html());
            setDecimalPlaces();
            var serverSessionId = $(".txtserversessionid").val();
            //Alert("localSessionId session:"+localSessionId);
            //Alert("serverSessionId session:"+serverSessionId);
            if (localSessionId != "" && serverSessionId == localSessionId) {
                $("#menu").show();
                if($(".txtisfinstationenabled").val()=="true")
                {
                    _isFinstationView = true;
                    setMainView();
                    if($(".txtismetalcommodityenabled").val()!="true")
                    {
                        $(".lnkmenu-metstation").hide();
                    }
                    redirect("dashboard.html", "FinIcon");
                }
                else if($(".txtismetalcommodityenabled").val()=="true")
                {
                    _isFinstationView = false;
                    setMainView();
                    if($(".txtisfinstationenabled").val()!="true")
                    {
                        $(".lnkmenu-finstation").hide();
                    }
                    redirect("metal-dashboard.html", "MetStation");
                }
                setInterval(function() { checkLoginStatus(); }, 10000);
            }
            else {
                $("#loginpage").show();
            }
        },
        error: function(err) {
            Alert("Error occurred!");
        }
    });
}
function checkLoginStatus() {
    return;
    var localSessionId = getSessionId();
    var url = _RPLUS_API + "/mobile-phonegap/mobile_handler.ashx?m=checkloginstatus&sessionid=" + localSessionId;
    $.ajax({
        url: url,
        cache: false,
        success: function(response) {
            if (response != "ok") {
                logout();
            }
        }
    });
}
function getSessionId()
{
    if(_sessionId != "") return _sessionId;
    return getCookie("sessionid");
}
function isValidForm(obj){
    var m = obj.attr("m");var action = obj.attr("action");
    var isvalid = true;
    obj.closest("ons-page").find(".required").each(function(){
        if(!isvalid)return;
        if($(this)[0].tagName == "SELECT"){
            if($(this).val()=="0")
            {
                isvalid=false;
            }
        }
        else{
            if($(this).val().trim()=="")
            {
                isvalid=false;
            }
        }
        if(!isvalid)$(this).focus();
    });
    if(isvalid)
    {
        if(m == "setalert")
        {
            return validateAlertForm();
        }
    }
    return isvalid;
}
function postFormRequest(obj){
    var m = obj.attr("m");var action = obj.attr("action");
    if(!isValidForm(obj))return;
//    if(m=="login")
//    {
//        redirect("dashboard.html", "FinIcon");
//        return;
//    }
    var Data = obj.closest("ons-page").find(":input").serialize();
    var url = "/mobile-phonegap/mobile_handler.ashx?action="+action+"&sessionid="+getSessionId();
    if(m!=undefined)url+="&m="+m;
    
    var URL = _RPLUS_API + url;
    $.ajax({
        url: URL,
        cache: false,
        data: Data,
        type: "POST",
        success: function(json) {
            json = $.parseJSON(json);
            if (json.msg != undefined && json.msg != "") {
                Alert(json.msg);
            }
            if (json.error != undefined && json.error != "") {
                Alert(json.error.replace("Error:", ""));
                return;
            }
            if (json.redirect != undefined && json.redirect != "") {
                if (m == "login") {
                    $("#menu").show();
                    //Alert(json.data.sessionid);
                    setCookie("sessionid", json.data.sessionid); //10years
                    _sessionId = json.data.sessionid;
                    //alert(getSessionId());
                }
                var arr = json.redirect.split(',');
                redirect(arr[0], arr[1]);
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            Alert("Error occurred!");
        }
    });  
}
function redirect(url,title){
    document.querySelector('#myNavigator').pushPage(url, { data: {title:title} });
    document.querySelector('ons-splitter-side').close();
}
function bindLiveRate(){          
    var liverateIds = "";
    if(_page==undefined)return;
    _page.find(".lr-rate").each(function(){
        if(liverateIds==""){
            liverateIds = $(this).attr("lrid");
        }
        else{
            liverateIds += "," + $(this).attr("lrid");
        }
    });
    if (_requestCounter > 10) {
        _requestCounter = 0;
    }
    var URL = _RPLUS_API + "/liverate.ashx?a=mobile-liverate&source=phonegap&type=" + _liveViewType + "&lrids=" + liverateIds + "&counter=" + _requestCounter + "&sessionid=" + getSessionId();
    $.ajax({
        url: URL,
        type: "GET",
        cache: false,
        success: function(json) {
            _requestCounter++;
            json = json.replace(/__NEWLINE__/g, "\n").replace(/__NEWLINER__/g, "\n");
            if (json != "") {
                if (json == "session expired") {
                    logout();
                    return;
                }
                var data = jQuery.parseJSON(json);
                if(_liveViewType == "lme3mforward")
                {
                    bindCommodityLiverate(data);
                }
                else
                {
                    for (i = 0; i < data.length; i++) {
                        var rid = data[i].rid;
                        var rate = data[i].cr;
                        setUpDownStatus(rid, rate);
                    }
                    calculateRate();
                }
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            //alert("Error occurred!");
        }
    });  
} 
function bindCommodityLiverate(data)
{
    $.each(data, function(key, value) {
        var mid = value.mid;
        $.each(value, function(key, value) {
            var lbl = $(".comm-liverate[col=" + key + "][metalid=" + mid + "]");
            if (lbl.length > 0) {
                setUpDownStatus_Commodity(lbl, value);
            }
        });
    });
}
function setUpDownStatus_Commodity(obj, rate) {
    if (!isNaN(rate)) {
        rate = parseFloat(rate);
        var prevrate = ConvertToDouble(obj.text());
        obj.attr("prate", prevrate);
        if (rate > prevrate) {
            obj.removeClass("rate-down").addClass("rate-mid");
            setTimeout(function() {
                obj.removeClass("rate-mid").addClass("rate-up");
            }, 100);
        }
        else if (rate < prevrate) {
            obj.removeClass("rate-up").addClass("rate-mid");
            setTimeout(function() {
                obj.removeClass("rate-mid").addClass("rate-down");
            }, 100);
        }
    }
    obj.text(rate);

}
function calculateRate() {
//    $(".liverate[sc=GBPINRPremiumPaisaMonthEnd][row=3],.liverate[sc=GBPINRPremiumPaisaMonthEnd][row=4]").each(function(){
//        var val = ConvertToDouble($(this).text());
//        val = parseFloat(val/100.0).toFixed(2);
//        $(this).text(val);
//    });
    $(".calc").each(function() {
        try {
            var calc = $(this).attr("calc");
            var val = "";
            var isexists = true;
            while (isexists) {
                if(calc.indexOf("self")>=0)
                {
                    calc = calc.replace("self",$(this).text());
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
                    var d = $("." + scode + "_" + row + "_" + col).text();
                    var data = d;
                    //data = ConvertToDouble(d);
                    calc = calc.replace(ctCalc, d);
                }
                else {
                    isexists = false;
                }
            }
            isexists = true;
            //for date diff
            while (isexists) {
                if (calc.indexOf("DATEDIFF") >= 0) {
                    var datestart = calc.indexOf("DATEDIFF");
                    var dateend = calc.indexOf(")");
                    var dates = calc.substring(datestart + 9, dateend);
                    var arrdates = dates.split('-');
                    var date1 = new Date(arrdates[1] + "-" + arrdates[0] + "-" + arrdates[2]);
                    var date2 = new Date(arrdates[4] + "-" + arrdates[3] + "-" + arrdates[5]);
                    var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
                    var datesformula = calc.substring(datestart, dateend + 1);
                    calc = calc.replace(datesformula, diffDays);
                }
                else {
                    isexists = false;
                }
            }
            $(this).attr("calres", calc);
            var newdata = ConvertToDouble(executeCalc(calc));
            if ((newdata + "").indexOf("Infinity") >= 0) {
                newdata = "0";
            }
            if ((newdata + "").indexOf(".") >= 0) {
                var arrd = (newdata + "").split('.');
                if (arrd[1].length > 4) {
                    newdata = parseFloat(newdata).toFixed(4);
                }
            }
            setUpDownStatus($(this).attr("lrid"), newdata);
        }
        catch (e1) { }
    });
}
function executeCalc(calc)
{
    //calc = "88.01+ 57.89/100";
    var arr1 = calc.split('+');
    var sum=0;
    for(var i=0;i<arr1.length;i++)
    {
        var val = arr1[i];
        if(val.indexOf("/")>0)
        {
            var arr2=val.split('/');
            var val1=ConvertToDouble(arr2[0]);
            var val2=ConvertToDouble(arr2[1]);
            val=val1/val2;
        }
        else
        {
            val = ConvertToDouble(val);
        }
        sum+=val;
    }
    return sum;
}
function setDecimalPlaces()
{
    $(".lr-rate").each(function(){
        var rate = $(this).text();
        if (!isNaN(rate)) {
            if($(this).closest(".divideby100").length > 0)
            {
                var rate = parseFloat(rate) / 100.0;
                $(this).text(rate);
            }
            setDecimalPlace($(this));
        }
    });
}
function setDecimalPlace(obj){
    var rate = obj.text();
    if (!isNaN(rate)) {
        rate = parseFloat(rate);
        if(obj.closest(".decimalplaces").length > 0)
        {
           rate = rate.toFixed(obj.closest(".decimalplaces").attr("decimalplaces"));
        }  
        else{
            rate = rate.toFixed(2);
        } 
        obj.text(rate);
    }
}
function setUpDownStatus(rid, rate) {
    var obj;
    _page.find(".lr-rate").each(function(){
        if($(this).attr("lrid")==rid){
            obj = $(this);
        }
    });
    if(obj==undefined)return;
    var prevrate = parseFloat(obj.text());
    
    if (isNaN(rate)) {
        obj.text(rate);
        return;
    }
    else{
        if(obj.closest(".divideby100").length > 0)
        {
            rate = parseFloat(rate) / 100.0;
        }
        obj.text(rate);
        setDecimalPlace(obj);
//        rate = parseFloat(rate);
//        if(obj.closest(".decimalplaces").length > 0)
//        {
//            rate = rate.toFixed(obj.closest(".decimalplaces").attr("decimalplaces"));
//        }
        if(obj.closest(".nocolorchange").length > 0)return;
        if (rate < prevrate) {
            obj.removeClass("rate-down").addClass("rate-mid");
            setTimeout(function() {
                obj.removeClass("rate-mid").addClass("rate-up");
            }, 500);
        }
        else if (rate > prevrate) {
            obj.removeClass("rate-up").addClass("rate-mid");
            setTimeout(function() {
            obj.removeClass("rate-mid").addClass("rate-down");
            }, 500);
        }
    }
    //obj.text(rate);
}
function loadCSS(){
     $('head').append("<link rel='stylesheet' href='"+_RPLUS_API+"/mobile-phonegap/css/custom.css?ts="+new Date().getTime()+"' type='text/css' />");
}
function initBrokenDateCalc(){
     $(document).on("change","#broken-ddlcurrency",function(){
        var liveRateId=$(this).find("option:selected").attr("targetrateid");
        $("#broken-sportdate").text($(".liverate_"+liveRateId).first().text());
     });
    $(document).on("click","#broken-btncalc",function(){
        var currency = $("#broken-ddlcurrency").val();
        var bdate = $("#broken-txtbdate").val();
        if(bdate=="")
        {
            Alert("Please select date");
            $("#broken-txtbdate").focus();
            return;
        }
        var spotDateCss = "jq-"+$("#broken-ddlcurrency").find("option:selected").text().toLowerCase()+"-spotdate";
        var spotdate = $("#broken-sportdate").text();
        var isexport = false;
        if($(".broken-covertype").find("input:first").is(":checked"))
        {
            isexport = true;
        }
        $("#broken-sportdate").text(spotdate);
        var URL = _RPLUS_API + "/brokendatecalc.ashx?source=phonegap&sdate="+spotdate+"&bdate="+bdate+"&ie="+isexport+"&c="+currency;
        $.ajax({
            url:URL,
            cache:false,
            success:function(response)
            {
                if(response.indexOf("message : ")==0)
                {
                    Alert(response.replace("message : ", ""));
                    return;
                }
                $("#broken-result").html(response);
            }
        });
    });
}
function loadBrokenDateCalc(){
    if($("#broken-sportdate").text()=="")
    {
        $("#broken-sportdate").text($(".liverate_1029").text());//USR INR Broken Date
    }
}
function loadDatePicker(){
    $(document).on("change",".ons-datepicker",function(){
        var date = $(this).val();
        var arr = date.split('-');
        date = arr[2]+'-'+arr[1]+'-'+arr[0];
        $(this).closest("td").find("input:first").val(date);
    });
    $(document).on("click",".datepicker",function(){
        $(this).closest("td").find("input:last").focus();
        $(this).closest("td").find("input:last").click();
        
    });
}
function validateAlertForm(){
    var target = ConvertToDouble($(".txttarget").val());
    var stoploss = ConvertToDouble($(".txtstoploss").val());
    if (target == 0 && stoploss == 0) {
        Alert("Please enter target or stop loss!");
        return false;
    }
    var emailids = ""; var mobilenos = "";
    $(".tblemailids").find(".chktwoselect").each(function() {
        if ($(this).is(":checked")) {
            if (emailids == "") {
                emailids = $(this).val();
            }
            else {
                emailids += "," + $(this).val();
            }
        }
    });
    $(".tblmobilenos").find(".chktwoselect").each(function() {
        if ($(this).is(":checked")) {
            if (mobilenos == "") {
                mobilenos = $(this).val();
            }
            else {
                mobilenos += "," + $(this).val();
            }
        }
    });
    $(".txtemailid").val(emailids);
    $(".txtmobileno").val(mobilenos);
    return true;
}
function initSetAlert(){
    $(document).on("click",".chktwoselect",function(){
        var count = 0;
        var emailids = "";
        $(this).closest("table").find(".chktwoselect").each(function() {
            if ($(this).is(":checked")) {
                count++;
            }
        });
        if (count > 2) {
            Alert("You can not select more than one contact!");
            $(this).removeAttr("checked");
            return false;
        }
    });
}
function getCookie(cname) {
  var name = cname + "=";
  var decodedCookie = decodeURIComponent(document.cookie);
  var ca = decodedCookie.split(';');
  for(var i = 0; i <ca.length; i++) {
    var c = ca[i];
    while (c.charAt(0) == ' ') {
      c = c.substring(1);
    }
    if (c.indexOf(name) == 0) {
      return c.substring(name.length, c.length);
    }
  }
  return "";
}
function setCookie(cname, cvalue) {
  var c = null;
  try
  {
      var d = new Date();
      d.setFullYear(d.getFullYear() + 10);
      //var expires = "expires='Thu, 17 Oct 2054 00:25:56 GMT'";//+ d.toUTCString();
      var expires = "expires=" + d.toUTCString();
      
      c = cname + "=" + cvalue + "; " + expires + "; path=/;";
      
      //Alert("C:"+c);
  }catch(e){Alert("error c1.");}
  document.cookie = c;
  try
  {
      var c1 = getCookie("sessionid");
      //Alert("c1:"+c1);
  }
  catch(e){alert("error");}
}
function setCookie1(cname, cvalue, exdays) {
  var c = null;
  try
  {
      var d = new Date();
      d.setTime(d.getTime() + (exdays*24*60*60*1000));
      var expires = "expires="+ d.toUTCString();
      c = cname + "=" + cvalue + ";" + expires + ";path=/";
  }catch(e){Alert("error")}
  document.cookie = c;
}
function deleteCookie(cname) {
  var d = new Date();
  d.setTime(d.getTime() - (1*24*60*60*1000));
  var expires = "expires="+ d.toUTCString();
  document.cookie = cname + "=;" + expires + ";path=/";
}

function loadAlertEmailMobileNos(){
    if($("#ltemailids").html() != "") return;
    var URL = _RPLUS_API + "/mobile-phonegap/mobile_handler.ashx?m=bindalertemailmobile&sessionid="+getSessionId();
    $.ajax({
        url:URL,
        cache:false,
        success:function(response)
        {   
            var json = $.parseJSON(response);
            $("#ltemailids").html(json.data.emailhtml);
            $("#ltmobilenos").html(json.data.mobilehtml);
        }
    });
}
function loadConfigCurrency(){
    var URL = _RPLUS_API + "/mobile-phonegap/mobile_handler.ashx?m=getliverateuserconfig&sessionid="+getSessionId();
    $.ajax({
        url:URL,
        cache:false,
        success:function(response)
        {   
            $(".jq-config-currencies").html(response);
        }
    });
}
function ConvertToDouble(val) {
    if (val == "" || val == null || val == undefined) {
        return 0;
    }
    return parseFloat(val);
}
function Alert(msg) {
    $(".fade").show();
    $(".alert-modal").show();
    $(".alert-modal-msg").text(msg);
}
function initMetal(){
    $(document).on("click",".jq-btn-more-data",function(){
        if ($(this).attr("src") == "https://finstation.in/images/arrow_expand_right.png") {
            $(this).attr("src", "https://finstation.in/images/arrow_expand_left.png");
            $(this).closest(".jq-tbl").find(".jq-more-data").show();
        }
        else {
            $(this).attr("src", "https://finstation.in/images/arrow_expand_right.png");
            $(this).closest(".jq-tbl").find(".jq-more-data").hide();
        }
    });
}
function loadImage()
{
    $(".jq-img").each(function(){
        if($(this).attr("src").indexOf("http")==-1)
        {
            var imgurl = _RPLUS_API + "/mobile-phonegap/"+$(this).attr("src");
            $(this).attr("src", imgurl);
        }
    });
}
function initExpandCollapse(){
    $(document).on("click",".jq-expance-collapse",function(){
        var div = $(this).closest(".jq-expand-collapse-panel");
        div.find(".jq-expance-collapse").removeClass("expance-collapse-exp").addClass("expance-collapse-col");
        if($(this).next().is(":visible"))
        {
            $(this).next().hide();
            $(this).removeClass("expance-collapse-exp").addClass("expance-collapse-col");
        }
        else
        {
            div.find(".expance-collapse-data").hide();
            $(this).next().show();
            $(this).removeClass("expance-collapse-col").addClass("expance-collapse-exp");
        }
        
    });
    $(document).on("click",".jq-saveconfig-currency",function(){
        var currencyTypes = "";
        var currencies = "NA";
        var onlycurrencies = "";
        _page.find(".jq-expance-collapse").each(function(){
            var ctypeId = $(this).attr("ctid");
            if(currencyTypes == "")
            {
                currencyTypes = ctypeId;
            }
            else
            {
                currencyTypes += "," + ctypeId;
            }
            var currency = "";
            $(this).next().find(".jq-chk-userconfig-currency").each(function(){
                if($(this).is(":checked"))
                {
                    if(currency == "")
                    {
                        currency = $(this).attr("cid");
                    }
                    else
                    {
                        currency += "," + $(this).attr("cid");
                    }
                    if(onlycurrencies == "")
                    {
                        onlycurrencies = $(this).attr("cid");
                    }
                    else
                    {
                        onlycurrencies += "," + $(this).attr("cid");
                    }
                }
            });
            if(currencies == "NA")
            {
                currencies = currency;
            }
            else
            {
                currencies += "|" + currency;
            }
        });
        var arr = onlycurrencies.split(',');
        if(onlycurrencies == "")
        {
            Alert("Please choose any item.");
            return false;
        }
        var maxCurrencyCount = parseInt($(".jq-expance-collapse:first").attr("maxcurrency"));
        if(arr.length > maxCurrencyCount)
        {
            Alert("You can not choose more than "+maxCurrencyCount+" items.");
            return false;
        }

        currencies = currencyTypes + "~" + currencies;
        
        var URL = _RPLUS_API + "/mobile-phonegap/mobile_handler.ashx?m=saveliverateuserconfig&sessionid="+getSessionId()+"&c="+currencies;
        $.ajax({
            url:URL,
            cache:false,
            success:function(response)
            {   
                if(response == "ok")
                {
                    //window.location.href = window.location.href;   
                    bindSpotRate();                
                    redirect("dashboard.html", "FinIcon");
                    //initMobile();
                }
            }
        });
    });
}
function bindSpotRate()
{
    var URL = _RPLUS_API + "/mobile-phonegap/mobile_handler.ashx?m=getrate&ratetype=spot&sessionid="+getSessionId();
    $.ajax({
        url:URL,
        cache:false,
        success:function(response)
        {   
            if(response.indexOf("Error:")>=0)return;
            $(".jq-spotrate").html(response);
        }
    });
}
function showPrevPage_mob()
{
    alert("Back button pressed");
}
function getQueryString(name)
{
    var url = window.location.href;
    if(url.indexOf("?")<0)return "";
    url = url.substring(url.indexOf("?")+1);
    var arr = url.split('&');
    for(var i=0;i<arr.length;i++)
    {
        var qs = arr[i];
        var arr1 = qs.split('=');
        if(arr1.length == 2)
        {
            if(arr1[0].toString().toLowerCase() == name.toLowerCase())
            {
                return arr1[1];
            }
        }
    }
    return "";
}
function ConvertToInt(val) {
    if (val == "" || val == null || val == undefined) {
        return 0;
    }
    return parseInt(val);
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
function initPushNotification()
{
    var localSessionId = getSessionId();
    $(document).on("click",".jq-header-bell",function(){
        redirect("pushnotification.html", "Push Notifications");
    });
    $(document).on("click",".notification-row-viewall",function(){
        bindPushNotificationList(true);
    });
    setTimeout(function(){
        bindPushNotificationCount();
    }, 5000);
    setInterval(function(){
        bindPushNotificationCount();
    }, 20000);
}
function bindPushNotificationCount()
{
    var localSessionId = getSessionId();
    var url = _RPLUS_API + "/mobile-phonegap/mobile_handler.ashx?m=pushnotification&a=finicon-homemsgcount&app=finicon&sessionid="+localSessionId;
    ajaxCall(url, function(response){
        var count = ConvertToInt(response);
        var panel = $(".jq-push-notify-panel");
        if(count > 0)
        {
            panel.find(".push-notify-msg-count").text(count);
            panel.find(".push-notify-msg-count").show();
            $(".jq-header-bell").addClass("header-bell-active");
        }
        else
        {
            panel.find(".push-notify-msg-count").hide();
            $(".jq-header-bell").removeClass("header-bell-active");
        }
    });
}
function bindPushNotificationList(isall)
{
    var localSessionId = getSessionId();
    var url = _RPLUS_API + "/mobile-phonegap/mobile_handler.ashx?m=pushnotification&a=finicon-notificationlist&app=finicon&sessionid="+localSessionId+"&isall="+isall;
    ajaxCall(url, function(response){
        var panel = $(".jq-push-notify-panel");
        var div = $(".jq-pushnotification-list");
        panel.find(".push-notify-msg-count").hide();
        div.html(response);
        div.show();
    });
}