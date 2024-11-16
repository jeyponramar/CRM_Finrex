$(document).ready(function() {
    $(".page,.spage,.btnspage").live("click", function() {
        var title = $(this).attr("title");
        if (title == undefined) title = $(this).text() + "";
        if ($(this).attr("class").indexOf("btnspage") >= 0) title = $(this).val();
        if (title == null || title.trim() == "" || title == undefined) {
            title = $(this).attr("pt");
        }
        if (title == undefined || title.trim() == "" || title == "&nbsp;") return;
        loadPage($(this).attr("href"), title); 
    });
    /*
    $(".menu").live("click", function() {
    var menu = $(this).text();
    var targetmenu;
    $(".submenu").removeClass("smenu-selected").addClass("smenu");
    $(".left-options").find(".submenus").each(function() {
    $(this).hide();
    if ($(this).attr("menu") == menu) {
    targetmenu = $(this);
    }
    });
    if (targetmenu == undefined) return;
    targetmenu.show("slow");
    targetmenu.find(".submenu").each(function() {
    if ($(this).attr("default") == "true") {
    $(this).removeClass("smenu").addClass("smenu-selected");
    loadPage($(this).attr("href"), $(this).text());
    }
    });
    });*/
    $(".adminhome").click(function() {
        $('#ifrmain').attr("src", "admin/dashboard.aspx");
    });
    $(".tab-multiscreen").click(function() {
        if ($(this).css("background-image").indexOf("minimize") > 0) {
            $(this).css("background-image", "url(images/maximize.png)");
            $(".tab-content").addClass("tab-content-multiscreen");
            $(".tab-content").show();
            var ch = $(".tab-content").height();
            var h = ($(window).height() - 100) / 2;
            $(".tab-content").css("height", h);
            $(".tab-content").attr("ph", ch);
        }
        else {
            $(this).css("background-image", "url(images/minimize.png)");
            $(".tab-content").removeClass("tab-content-multiscreen");
            $(".tab-content").css("height", $(".tab-content").attr("ph"));
            $(".tab-content").hide();
            var p = parent.curPage;
            highlightTab();
            showPage(p);
            $("#tab-" + p, window.top.document).find(".t").removeClass("tab").addClass("tab-hover");
        }
    });
    /*$(".submenu").live("click", function() {
    if ($(this).attr("in") == "1") {
    window.open($(this).attr("href"));
    return;
    }
    $(this).closest(".submenus").find(".submenu").removeClass("smenu-selected").addClass("smenu");
    $(this).removeClass("smenu").addClass("smenu-selected");
    loadPage($(this).attr("href"), $(this).text());

    });*/
    $(".lnksubmenu").live("click", function() {
        if ($(this).attr("in") == "1") {
            window.open($(this).attr("href"));
            return;
        }
        $(this).closest(".submenus").find(".submenu").removeClass("smenu-selected").addClass("smenu");
        $(this).removeClass("smenu").addClass("smenu-selected");
        loadPage($(this).attr("href"), $(this).text());
        _selectedMenu = $(this).closest(".submenu").attr("target");
    });
    $(".t").live("mouseenter", function() {
        $(this).find(".tclose").show();
        $(this).find(".trefresh").show();
    });
    $(".t").live("mouseleave", function() {
        $(this).find(".tclose").hide();
    });

    $(".tpage").live("click", function() {
        var p = parseInt($(this).closest(".tabpage").attr("id").replace("tab-", ""));
        if (p != parseInt(parent.prevPage)) {
            var pp = parent.curPage;
            hidePage(pp);
            showPage(p);
            $("#tab-" + pp, window.top.document).find(".t").removeClass("tab-hover").addClass("tab");
            $("#tab-" + p, window.top.document).find(".t").removeClass("tab").addClass("tab-hover");
        }
    });
    $(".tclose,.close-page").live("click", function() {
        var p = parseInt($(this).closest(".tabpage").attr("id").replace("tab-", ""));
        closeTab(p, $(this));
    });
    $(".cancel").click(function() {
        closeTab();
    });
    $(".grid-setting").click(function() {
        var url = "grid/grid-setting.aspx?m=" + $(this).attr("m");
        loadPage(url, "Grid Settings - " + $(this).attr("m"));
    });
    $(document).keydown(function(e) {
        if (e.keyCode == 27)//ESC
        {
            if (confirm("Are you sure you want to close the current TAB")) {
                closeTab();
            }
        }
    });
});
function checkAccess(URL) {
    var action = "";
    var f = QueryString("", URL);
    if (f == "home") return true;
    if (QueryString("id", URL) != "") {
        action = "Edit";
    }
    else if (QueryString("a", URL) == "add") {
        action = "Create";
    }
    else {
        action = QueryString("a", URL);
    }

    return true;
}
function getPageUrl(url) {
    if (url == undefined) return "";
    var pageurl = "";
    if (url.indexOf(".aspx") > 0) {
        pageurl = url.replace("#", "");
    }
    else {
        pageurl = QueryString("", url) + "/" + QueryString("a", url) + ".aspx";
        var arr = url.split('/');
        if (arr.length > 2) {
            for (i = 4; i < arr.length; i++) {
                if (i % 2 == 0) {
                    if (pageurl.indexOf("?") < 0) {
                        pageurl = pageurl + "?" + arr[i] + "=";
                    }
                    else {
                        pageurl = pageurl + "&" + arr[i] + "=";
                    }
                }
                else {
                    pageurl = pageurl + arr[i];
                }
            }
        }
        
    }
    return pageurl;
}
function isAddTabAlreadyOpened(title) {
    title = title.toLowerCase();
    if (title.indexOf("add ") < 0) return false;
    var isexists = false;
    $(".tpage").each(function() {
        if ($(this).text().toLowerCase() == title) {
            var pageIndex = $(this).closest(".tabpage").attr("id").split('-')[1];
            if (pageIndex != parseInt(parent.prevPage)) {
                var pp = parent.curPage;
                hidePage(pp);
                showPage(pageIndex);
                $("#tab-" + pp, window.top.document).find(".t").removeClass("tab-hover").addClass("tab");
                $("#tab-" + pageIndex, window.top.document).find(".t").removeClass("tab").addClass("tab-hover");
            }
            isexists = true;
        }
    });
    return isexists;
}
function removeViewTabAlreadyOpened(title) {
    title = title.toLowerCase();
    if (title.indexOf("view ") < 0) return;
    $(".tpage").each(function() {
        if ($(this).text().toLowerCase() == title) {
            var pageIndex = $(this).closest(".tabpage").attr("id").split('-')[1];
            //if (pageIndex != parseInt(parent.prevPage)) {
                var p = parseInt($(this).closest(".tabpage").attr("id").replace("tab-", ""));
                closeTab(p, $(this));
            //}
        }
    });
}
function loadPage(url, title, extras) {
    //if (!checkAccess(url)) return;
    if (isAddTabAlreadyOpened(title)) return;
    removeViewTabAlreadyOpened(title);
    var pageurl = getPageUrl(url);
    if (extras != undefined) {
        if (pageurl.indexOf("?") > 0) {
            pageurl = pageurl + "&" + extras;
        }
        else {
            pageurl = pageurl + "?" + extras;
        }
    }

    parent._currrentUrl = pageurl;
    //if (parseInt(parent.prevPage) > 0) 
    {
        hidePage(parent.prevPage);
    }

    var div = addTab(pageurl, title);
    if (div.length == 0) {
        window.open("../" + pageurl);
    }
    else {
        var h = parseInt(parent.__SCREEN_HEIGHT) - 130;
        if (pageurl.indexOf("?") > 0) {
            pageurl = pageurl + "&tab=" + parent.curPage;
        }
        else {
            pageurl = pageurl + "?tab=" + parent.curPage;
        }
        var html = "<iframe id='frame-" + parent.curPage + "' class='tab-if' src='" + pageurl + "' width='100%' scrolling='auto' height='" + h + "' " +
                   "style='border:none;margin:0px;padding:0px;'></iframe>";
        $("#page-" + parent.curPage, window.top.document).html(html);
        //remove tabs if more than 10
        if ($(".tab-if").length > 9) {
            var index = 0;
            var c = 0;
            $(".tab-if").each(function() {
                if (c == 1) {
                    index = $(this).attr("id").split('-')[1];
                }
                c++;
            });
            if (index > 0) {
                $("#page-" + index).remove();
                $("#tab-" + index).remove();
            }
        }
    }
}
function getCurrentUrl(p) {
    if (p == undefined) p = curPage;
    return $("#tab-" + p, window.top.document).find(".url").val();
}
function getCurrentTitle(p) {
    if (p == undefined) p = curPage;
    return $("#tab-" + p, window.top.document).attr("title");
}
//child function
function setTitle(title) {
    var p = QueryString("tab");
    if(title == undefined)
    {
        title = $("#tab-" + p, window.top.document).find(".tpage").text()
    }
    var alt = title;
    if (title.length > 13) {
        title = title.substring(0, 13) + "...";
    }
    $("#tab-" + p, window.top.document).find(".tpage").text(title);
}
function addTab(url, title) {
    parent.pageId++;
    //parent.prevPage = parent.curPage;
    parent.curPage = parent.pageId;
    //remove if already added
//    $(".tab-bar", window.top.document).find(".tabpage").each(function() {
//        var t = $(this).attr("title");
//        var page = parseInt($(this).attr("id").replace("tab-", ""));
//        if (title == t) {
//            $(this).closest(".tabpage", window.top.document).remove();
//            $("#page-" + page, window.top.document).remove();
//        }
//    });

    highlightTab();
    $("#inner", window.top.document).append("<div class='tab-content' id='page-" + parent.pageId + "'></div>");
    alt = title;
    if (title.length > 13) {
        title = title.substring(0, 13) + "...";
    }
    var html = "";
    var css = "";
    if (title.indexOf("Reminder - ") == 0) {
        css = " tab-reminder";
    }
    html = "<div class='left tabpage' id='tab-" + parent.pageId + "' title='" + alt + "'>" +
                   "<div class='tab-sep'>&nbsp;</div>" +
                   "<div class='tab-hover t' p='0'>" +
                        "<div class='tclose'><input type='hidden' value='" + parent._currrentUrl + "' class='url'/></div>" +
                   "<div class='tpage"+css+"' style='opacity'>" + title + "</div></div>" +
               "</div>";

    $(".tab-bar", window.top.document).append(html);
    
    parent.prevPage = parent.pageId;
    return $("#page-" + parent.pageId, window.top.document);
}
function highlightTab() {
    $("#tab-" + parent.prevPage, window.top.document).find(".t").removeClass("tab-hover").addClass("tab");
    $("#tab-" + parent.curPage, window.top.document).find(".t").removeClass("tab").addClass("tab-hover");
}
function showPage(p) {
    if (parseInt(p) < 1) return;
    $("#page-" + p, window.top.document).show();
    parent.prevPage = p;
    parent.curPage = p;
}
function hidePage(p) {
    //if (parseInt(p) < 1) return;
    $("#page-" + p, window.top.document).hide();
}

function closeTab(cpage, ctrl) {
    
    if (cpage == 0 || cpage == null) cpage = parent.curPage;
    var newPage = 0;
    var tcount = 0;
    var nPage = 0;
    var pPage = 0;
    var p = 0;
    cpage = parseInt(cpage);
    if (cpage == 1) return;
    $(".tab-bar", window.top.document).find(".tabpage").each(function() {
        p = parseInt($(this).attr("id").replace("tab-", ""));
        tcount++;
        //if (p > cpage && p!=cpage) newPage = p;
        if (p > cpage) nPage = p;
        if (p < cpage) pPage = p;
    });
    if (nPage > 0) {
        newPage = nPage;
    }
    else if(pPage>0) {
        newPage = pPage;
    }
    //if (newPage == 0 && tcount > 0) newPage = p;
    if (newPage > 0) {
        showPage(newPage);
        highlightTab();
    }
    $("#tab-" + cpage, window.top.document).remove();
    $("#page-" + cpage, window.top.document).remove();
    
}
// slight update to account for browsers not supporting e.which
function disableF5(e) {
    if ((e.which || e.keyCode) == 116) {
        if (confirm("You will lose this page, are you sure you want to go to home page?\n\nIf you want to refresh the current page please click on refresh icon.")) {
            location.reload();
        }
        else {
            e.preventDefault();
        }
    }
};
$(document).bind("keydown", disableF5);