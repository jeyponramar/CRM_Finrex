//var _RPLUS_API = "";
var _isLiveRateLoaded = false;
var _liveViewType = "";
var _page;
$(document).ready(function(){
//    if (navigator.userAgent.match(/(iPhone|iPod|iPad|Android|BlackBerry|IEMobile)/)) {
//      document.addEventListener("deviceready", onDeviceReady, false);
//    } else {
      load_mobile(); //this is for the browser
    //}
}); 
function load_mobile(){
    initMobile();
    Alert("This is a test message for alert!");
}
document.addEventListener('init', function(event) {
    var page = event.target;
    //_page = $(page);
    setCurrentPage();
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
    }
    else if(page.id == "forward-page"){
        bindLiveRate();
    }
    else if(page.id == "indices-page"){
        bindLiveRate();
    }
    else if(page.id == "brokendatecalc-page"){
        loadBrokenDateCalc();
    }
    else if(page.id == "setalert-page"){
        loadAlertEmailMobileNos();
    }
    else if(page.id == "logout-page"){
        logout();
    }
});
function logout(){
    deleteCookie("sessionid");
    //redirect("login.html","FinIcon");
    window.location.reload();
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
    $(document).on("click",".btnsubmit",function(){
        postFormRequest($(this));
    });
    $(document).on("click",".lnkmenu",function(){
        openMenu($(this));
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
    $.ajax({
        url: url,
        cache: false,
        success: function(response) {
            var index = response.indexOf("<template");
            response = response.substring(index);
            response = response.replace("</form></body></html>", "");
            $("body").append(response);
            setDecimalPlaces();
            var serverSessionId = $(".txtserversessionid").val();
            if (localSessionId != "" && serverSessionId == localSessionId) {
                $("#menu").show();
                redirect("dashboard.html", "FinIcon");
            }
            else {
                $("#loginpage").show();
            }
        },
        error: function(err) {
            alert("Error occurred!");
        }
    });
}
function getSessionId()
{
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
	    url:URL,    
	    cache:false,
	    data:Data,
	    type:"POST",
	    success:function(json){
	        json = $.parseJSON(json);
	        if(json.msg!=undefined && json.msg!="")
	        {
	            Alert(json.msg);
	        }
		    if(json.error!=undefined && json.error!="")
		    {
		        Alert(json.error.replace("Error:", ""));
		        return;
		    }
		    if(json.redirect!=undefined && json.redirect!="")
		    {
		        if(m=="login")
	            {
	                $("#menu").show();
	                setCookie("sessionid",json.data.sessionid,3650);//10years
	            }
		        var arr=json.redirect.split(',');
		        redirect(arr[0],arr[1]);
		    }
	    },
	    error: function (xhr, ajaxOptions, thrownError) {
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
	var URL = _RPLUS_API + "/liverate.ashx?a=mobile-liverate&source=phonegap&type="+_liveViewType+"&lrids="+liverateIds; 
	$.ajax({
		url:URL,    
		type:"GET",
		cache:false,
		success:function(json){
			json = json.replace(/__NEWLINE__/g,"\n").replace(/__NEWLINER__/g,"\n");
			if (json != "") {
                var data = jQuery.parseJSON(json);
                for (i = 0; i < data.length; i++) {
                    var rid = data[i].rid;
                    var rate = data[i].cr;
                    setUpDownStatus(rid, rate);
                }
            }
		},
		error: function (xhr, ajaxOptions, thrownError) {
            //alert("Error occurred!");
          }
	});  
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
        $("#broken-sportdate").text($(".liverate_"+liveRateId).text());
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
function setCookie(cname, cvalue, exdays) {
  var d = new Date();
  d.setTime(d.getTime() + (exdays*24*60*60*1000));
  var expires = "expires="+ d.toUTCString();
  document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}
function deleteCookie(cname) {
  var d = new Date();
  d.setTime(d.getTime() - (1*24*60*60*1000));
  var expires = "expires="+ d.toUTCString();
  document.cookie = cname + "=;" + expires + ";path=/";
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