var _page;
$(document).ready(function() {
    load_mobile();
});
function load_mobile() {
    initMobile();
}
document.addEventListener('init', function(event) {
    var page = event.target;
    setCurrentPage();
    $("#menu").show();
    if (page.id == "logout-page") {
        logout();
    }
    else if (page.id == "homepage") {
    }
    else if (page.id == "loginpage") {
        $("#menu").hide();
    }
    else {
        if (page.id != "") {
            page_load(page.id);
        }
    }
});
function page_load(pageId) {
    pageId = pageId.replace("-page", "");
    var url = "mobile_handler.ashx?m=pageload&page=" + pageId + "&sessionid=" + getSessionId();
    if (pageId == "latestnotification") {
        var device = QueryString("device");
        var onesignalId = QueryString("onesignalid");
        if (device != "" && onesignalId != "") {
            url += "&device=" + device + "&onesignalid=" + onesignalId;
        }
    }
    $.ajax({
        url: url,
        cache: false,
        type: "POST",
        success: function(response) {
            if (response == "session expired") {
                redirect("login.html", "Login");
                return;
            }
            setTimeout(function() {
                _page.find(".addpage-content").html(response);
            }, 100);
        }
    });
}
function logout() {
    deleteCookie("sessionid");
    window.location.reload();
}
document.addEventListener('postchange', function(event) {
    setCurrentPage();
});
function initMobile() {
    $(document).on("click", ".btnsubmit", function() {
        postFormRequest($(this));
    });
    $(document).on("click", ".lnkmenu", function() {
        openMenu($(this));
    });
    $(document).on("click", "#btnalertmodalok", function() {
        $(".alert-modal").hide();
        $(".fade").hide();
    });
}
function getCurrentPage() {
    return document.querySelector('#myNavigator').getCurrentPage().page;
}
function openMenu(obj) {
    var url = obj.attr("href");
    if (url == undefined) url = obj.attr("url");
    var title = obj.text();
    var m = obj.attr("m");
    var mid = obj.attr("mid");
    document.querySelector('#myNavigator').pushPage(url, { data: { title: title, m: m, mid: mid} });
    document.querySelector('ons-splitter-side').close();

    setTimeout(function() {
        var pageid = url.replace(".html", "") + "-page";
        $("#" + pageid).find("ons-tab").each(function() {
            if ($(this).attr("class").indexOf("active") >= 0) {
                //_page = $("#"+$(this).attr("page").replace(".html","")+"-page");
            }
        });
    }, 500);
    setTimeout(function() {
        for (var i = 0; i < document.querySelector('#myNavigator').pages.length; i++) {
            var p = $(document.querySelector('#myNavigator').pages[i]);
            if (p.css("display") == "block") {
                //_page = p;
            }
        }
    }, 100);
}
function setCurrentPage() {
    if (document.querySelector('#myNavigator') == null) return;
    for (var i = 0; i < document.querySelector('#myNavigator').pages.length; i++) {
        var p = $(document.querySelector('#myNavigator').pages[i]);
        if (p.css("display") == "block") {
            _page = p;
        }
    }
}

function getSessionId() {
    return getCookie("sessionid");
}
function postFormRequest(obj) {
    var m = obj.attr("m"); var action = obj.attr("action");
    //if (!isValidForm(obj)) return;
    var Data = obj.closest("ons-page").find(":input").serialize();
    var url = "mobile_handler.ashx?action=" + action + "&sessionid=" + getSessionId();
    if (m != undefined) url += "&m=" + m;
    var device = QueryString("device");
    var onesignalId = QueryString("onesignalid");
    if (device != "" && onesignalId != "") {
        url += "&device=" + device + "&onesignalid=" + onesignalId;
    }
    var URL = url;
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
                var arr = json.redirect.split(',');
                redirect(arr[0], arr[1]);
                if (m == "login") {
                    setCookie("sessionid", json.data.sessionid, 3650); //10years
                    //                    setTimeout(function() {
                    //                        _page.find(".jq-latestnotification").html(json.data.notification);
                    //                    }, 100);
                }

            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            Alert("Error occurred!");
        }
    });
}
function redirect(url, title) {
    document.querySelector('#myNavigator').pushPage(url, { data: { title: title} });
    document.querySelector('ons-splitter-side').close();
}
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}
function deleteCookie(cname) {
    var d = new Date();
    d.setTime(d.getTime() - (1 * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=;" + expires + ";path=/";
}
function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
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