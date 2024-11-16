$(document).ready(function(){
    initPushNotification();
    $(".jq-page-back").click(function(){
        history.back();
    });
    loadDatePicker();
});
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
    $(".jq-header-bell").click(function(){
        //bindPushNotification();
        window.location.href = "view-pushnotification.aspx?istop=true&sid="+getSessionId();
    });
    $(".jq-notification-row").live("click",function(){
        var nid = $(this).attr("nid");
        window.location.href = "view-pushnotification.aspx?id="+nid+"&sid="+getSessionId();
    });
    $(".notification-row-viewall").live("click",function(){
        window.location.href = "view-pushnotification.aspx?viewall=true&sid="+getSessionId();
    });
    bindPushNotificationCount();
    setInterval(function(){
        bindPushNotificationCount();
    }, 20000);
}
function getSessionId()
{
    return $(".jq-sessionid").val();
}
function bindPushNotificationCount()
{
    var url = "../utilities.ashx?m=pushnotification&a=finwatch-homemsgcount&app=finwatch&sid="+getSessionId();
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
function bindPushNotification()
{
    var url = "../utilities.ashx?m=pushnotification&a=topmessages&app=finwatch&sid="+getSessionId();
    ajaxCall(url, function(response){
        var panel = $(".jq-push-notify-panel");
        var div = $(".push-notify-msg-list");
        panel.find(".push-notify-msg-count").hide();
        div.html(response);
        div.show();
        setWindowHeight();
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
function setWindowHeight()
{
    try
    {
        var h = $("body").height();
        if(h > 500) h = 500;
        external.setHeight(h);
    }catch(e){}
}