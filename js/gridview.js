$(document).ready(function() {
    $(".paging li").live("click", function() {
        var pno = $(this).text();
        var objParam = $(this).closest(".grid").find(".g_param");
        var totPages = parseInt(getParam(objParam, "tp"));
        var curPage = parseInt(getParam(objParam, "p"));
        if (pno == "Next") {
            pno = totPages;
            if (curPage < totPages) {
                pno = parseInt(curPage) + 1;
            }
        }
        else if (pno == "Prev") {
            pno = 1;
            if (curPage > 1) {
                pno = curPage - 1;
            }
        }
        setParam(objParam, "p", pno);
        bindGrid($(this));
    });
    function bindGrid(ctrl) {

        var objParam = ctrl.closest(".grid").find(".g_param");
        var setting = ctrl.closest(".grid").find(".g_setting").val();
        var param = objParam.val();
        var url = "gridview.php?set=" + setting + "&prm=" + param;
        var gv = ctrl.closest(".grid").find(".gv");
        showLoader(gv);
        var html = RequestData(url);
        gv.replaceWith($(html));
        hideLoader(gv);
    }
    $(".refresh").live("click", function() {
        bindGrid($(this));
    });
    $(".repeater-header td").live("click", function() {
        var currSort = $(this).text();
        if (currSort == "Edit") return;
        var i = 0;
        var currIndex = 0;
        $(this).parent().find("td").each(function() {
            if ($(this).text() == currSort) {
                currIndex = i;
            }
            if ($(this).text() != "Edit") i++;
        });
        //var objgrid = $(this).parent().parent();
        var objgrid = $(this).closest(".grid");
        var param = objgrid.find(".g_param");
        var gc = getParam(objgrid.find(".g_setting"), "gc");
        var arrcols = gc.split(',');
        var sb = arrcols[currIndex];

        //get previous sort and
        var isAsc = true;
        var psb = getParam(param, "sb");
        if (psb.replace(' ASC', '').replace(' DESC', '') == sb) {
            if (psb.indexOf(' DESC') > 0) {
                sb = sb + ' ASC';
            }
            else {
                sb = sb + ' DESC';
                isAsc = false;
            }
        }
        $(this).parent().find("img").remove();
        if (isAsc) {
            $(this).append("<img src='images/down-arr1.png'/>");
        }
        else {
            $(this).append("<img src='images/up-arr1.png'/>");
        }
        //set sort by
        setParam(param, "sb", sb);
        bindGrid($(this));
    });
    $(".search").live("click", function() {
        var objgrid = $(this).closest(".grid");
        search(objgrid);
    });
    $(".clear-search").live("click", function() {
        var objgrid = $(this).closest(".grid");
        var param = objgrid.find(".g_param");
        setParam(param, "a", "");
        setParam(param, "w", "");
        setParam(param, "w", getParam(param, "ew"));
        objgrid.find(".keyword").val("");
        bindGrid($(this));
    });

    $(".keyword").live("keypress", function(event) {
        if (event.which == 13) {
            var objgrid = $(this).closest(".grid");
            search(objgrid);
            return false;
        }
    });
    function search(objgrid) {
        var param = objgrid.find(".g_param");
        var setting = objgrid.find(".g_setting");
        setParam(param, "a", "s");
        setParam(param, "p", "1");

        var w = "";
        var wc = getParam(param, "wc");
        var sb = objgrid.find(".searchby").val();
        var kw = objgrid.find(".keyword").val();
        var mt = getParam(setting, "mt");

        if (getParam(param, "mt") == "o") {
            setParam(param, "sb", sb);
            setParam(param, "kw", kw);
            setParam(param, "sd", $(".sd").val());
            setParam(param, "ed", $(".ed").val());
        }
        else {
            if (sb != undefined) {
                if (sb != "") {
                    w = sb + " LKE '@PER" + kw + "@PER'";
                }
                else {
                    objgrid.find(".searchby option").each(function() {
                        if ($(this).val() != "") {
                            if (w == "") {
                                w = $(this).val() + " LKE '@PER" + kw + "@PER'";
                            }
                            else {
                                w += " OR " + $(this).val() + " LKE '@PER" + kw + "@PER'";
                            }
                        }
                    });
                }
                if (wc != "") {
                    setParam(param, "w", "(" + w + ") AND (" + wc + ")");
                }
                else {
                    setParam(param, "w", w);
                }
            }

            if (getParam(param, "ew") != "") {
                setParam(param, "w", "(" + w + ") AND (" + getParam(param, "ew") + ")");
            }
        }

        bindGrid(param);

    }


});