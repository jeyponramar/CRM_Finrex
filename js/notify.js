function checkNotificationCount() {
    if ($(".notify-pop-tr").length == 0) {
        $(".tbl-notify-msg-pop").append("<tr class='no-rec'><td align='center'>No data found!</td></tr>");
        $(".btn-notify-remove-all").hide();
    }
    else {
        $(".btn-notify-remove-all").show();
    }
}
function loadAllNotificationCount() {
    var url = "getnotificationcount.ashx?lnid=" + _lastNotificationId;
    var data = RequestData(url);
    if (data != "") {
        var amcreminders = ConvertToInt(data[0].AmcRemindersCount);
        var opencomplaints = ConvertToInt(data[0].OpenComplaintsCount);
        var notifications = ConvertToInt(data[0].NotificationCount);
        var notificationid = ConvertToInt(data[0].NotificationId);
        var moduleid = ConvertToInt(data[0].ModuleId);
        var module = ConvertToString(data[0].Module);
        var newnotification = ConvertToString(data[0].NewNotification);
        if (amcreminders == 0) {
            $(".amcreminder-count").hide(); 
        }
        else {
            $(".amcreminder-count").find("div").text(amcreminders);
            $(".amcreminder-count").show();
        }
        if (opencomplaints == 0) {
            $(".opencomplaints-count").hide();
        }
        else {
            $(".opencomplaints-count").find("div").text(opencomplaints);
            $(".opencomplaints-count").show();
        }
        if (notifications == 0) {
            $(".notification-count").hide();
            $(".tdnotification-new").hide();
        }
        else {
            $(".notification-count").find("div").text(notifications);
            $(".notification-count").show();
            $(".tdnotification-new").attr("nid", notificationid);
            if (notificationid > 0) {
                $(".notification-new").text(newnotification);
                if ($(".tdnotification-new").css("display") == "none") {
                    $(".tdnotification-new").fadeIn(2000);
                }
                else {
                    $(".tdnotification-new").fadeOut(1000, function() {
                        $(".tdnotification-new").fadeIn(1000);
                    });
                }
                
                $(".tdnotification-new").attr("m", module);
                $(".tdnotification-new").attr("mid", moduleid);
            }
        }

    }
    setTimeout("loadAllNotificationCount()", 10000);//refresh every 10secs
}
function loadNotification() {
    var url = "getpage.ashx?m=notify-pop";
    $.ajax({
        url: url,
        type: 'GET',
        async: true,
        success: function(html) {
            if ((html + "").indexOf("session expired") > 0) {
                window.location = "../login.aspx";
                return;
            }
            else if ((html + "").indexOf("Error") >= 0) {
                alert("Error Occurred!");
                return;
            }
            $(".notify-msg-pop").find("table").html(html);
            checkNotificationCount();
        },
        complete: function() {
        },
        error: function(err, status, jqXHR) {
            alert("Error Occurred!");
        }
    });
}
$(document).ready(function() {
    loadAllNotificationCount();
    $(".notify-del").live("click", function() {
        var nid = $(this).closest("tr").attr("nid");
        var url = "utilities.ashx?m=notification_d&nid=" + nid;
        var isdel = RequestData(url);
        if (isdel == "1") {
            $(this).closest("tr").fadeOut(500);
        }
        else {
            alert("Error Occurred!");
        }
        var newnid = $(this).closest("tr").attr("nid");
        if (nid == newnid) {
            _lastNotificationId = $(this).closest("tr").attr("nid");
            $(".tdnotification-new").hide("slow");
        }
    });
    $(".notify-detail").live("click", function() {
        var id = $(this).closest("tr").attr("mid");
        var mn = $(this).closest("tr").attr("mn");
        $(".notify-msg-pop").hide();
        $(".notify-arrow").hide();
        loadPage(mn + "/add.aspx?id=" + id, mn);
    });
    $(".notify-pop-close").live("click", function() {
        $(this).closest(".notify-msg-pop").fadeOut(1000);
        $(".notify-arrow").hide();
    });
    $(".btn-notify-remove-all").live("click", function() {
        var url = "utilities.ashx?dall=1&m=notification_d&nid=" + $(this).closest("tr").attr("nid");
        var isdel = RequestData(url);
        if (isdel == "1") {
            $(".notify-pop-tr").fadeOut(500,
                    function() {
                        $(this).remove();
                        checkNotificationCount();
                    }
                    );
        }
        else {
            alert("Error Occurred!");
        }
        $(".tdnotification-new").hide("slow");
    });

    $(".tdnotify-msg").click(function() {
        var p = $(".tdnotify-msg").position();
        $(".notify-msg-pop").css("left", p.left - 170);
        $(".notify-arrow").css("left", p.left);
        $(".notify-msg-pop").show();
        $(".notify-arrow").show();
        var table = "";
        var htmlpreload = "<tr><td class='notify-pop-title'>Notifications</td></tr><tr><td></td></tr><tr><td class='notify-pop-loader' align='center'><img src='images/google-loader.gif'/></td></tr>";
        $(".notify-msg-pop").find("table").html(htmlpreload);
        setTimeout("loadNotification()", 1000);
    });
    $(".notification-new-close").click(function() {
        $(".tdnotification-new").hide();
        _lastNotificationId = $(".tdnotification-new").attr("nid");
    });
    $(".notification-new").click(function() {
        var id = $(this).closest(".tdnotification-new").attr("mid");
        var mn = $(this).closest(".tdnotification-new").attr("m");
        $(".tdnotification-new").hide();
        _lastNotificationId = $(".tdnotification-new").attr("nid");
        loadPage(mn + "/add.aspx?id=" + id, mn);
    });
});
