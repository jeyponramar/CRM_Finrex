var _JG_DATA = null;
var _JG_PRE_DATA = null;
function JGrid(options) {
    this.grid = options.grid;
    this.detailtable = options.detailtable;
    this.action = options.action;
    this.onBind = options.onBind;
    this.bind = BindJGData;
    this.success = options.success;
    this.error = options.error;
    this.paging = GetPaging;
    this.enablePaging = true;
    this.gridType = 1;
    this.pageSize = 10;
    this.recCount = 0;
    this.maxPages = 4;
    this.pageNo = 1;
    this.currentIndex = options.currentIndex;
    this.totPages = 0;
    this.editUrl;
    this.addUrl = options.addUrl;
    this.viewUrl;

    function BindJGData() {
        _JG_DATA = null;
        _JG_PRE_DATA = null;
        if (options.onBind != undefined) this.onBind();
        if (options.success != undefined) this.success();
        try {
            showLoader(this.grid);
            //ashxurl = "getdata.php?hc=CustomerId&gc=CustomerName,EmailId,PhoneNo&m=GetCustomer&param=p~1~ps~20";
            //read settings and parameters from grid
            var setting = this.grid.find(".g_setting");
            var param = this.grid.find(".g_param");
            
            var hc = getParam(setting, "hc");
            var gt = getParam(setting, "gt");
            var ep = getParam(setting, "ep");
            var viewtarget = getParam(setting, "viewtarget");
            this.editUrl = getParam(setting, "editUrl");
            this.viewUrl = getParam(setting, "viewUrl");
            var editTitle = getParam(setting, "edit-title");
            var viewTitle = getParam(setting, "view-title");

            var qp = getQparam(setting, param);
            //alert(qp);
            var ashxurl = "getdata.php?" + qp;
            //var ashxurl = "getdata.php?m=" + m + "&mt="+mt+"&hc=" + hc + "&gc=" + gc + "&pm=" + param + "&jt="+jt;
            this.currentIndex = getParam(this.grid.find(".g_param"), "p");
            this.pageSize = getParam(this.grid.find(".g_param"), "ps");
            
            if (gt != "") {
                this.gridType = parseInt(gt);
            }
            if (ep === "false") {
                this.enablePaging = false;
            }
            //document.write(ashxurl);
            _JG_PRE_DATA = RequestData(ashxurl);
            //alert(_JG_PRE_DATA)
            _en = new Date();
            $("#en").text(_en);
            $("#diff").text(_en - _st);
            
            //alert(ashxurl);
            
            var ishead = true;
            this.grid.find("tr").each(function() {
                if (!ishead) {
                    $(this).remove();
                }
                if ($(this).attr("class") == "repeater-header" || $(this).attr("class") == "srepeater-header") {
                    ishead = false;
                }
            });
            var refresh = "<td class='refresh'><img src='images/refresh.png'/></td>";
            var gridsetting = "<td><img class='gridsetting hand' title='Configure Grid Columns' src='images/grid-setting.png'/></td>";

            if (_JG_PRE_DATA == "Error occurred!" || _JG_PRE_DATA.trim() + '' == "")
            {
                this.grid.append("<tr><td class='error'>"+_JG_PRE_DATA+"</td></tr>");
                
                this.grid.find(".trpaging").remove();
                
                this.grid.find(".repeater-header").before("<tr class='trpaging'><td colspan='20'>"+
                                "<table width='100%'><tr><td width='90%'>&nbsp;</td>"
                                + refresh + gridsetting + "</tr></table></td></tr>");
                if (_JG_PRE_DATA.trim() + '' == "") {
                    this.grid.append("<tr><td class='error'>No data found.</td></tr>");
                }                                
                hideLoader(this.grid);
                return;
            }
            
            var _JG_DATA = jQuery.parseJSON(_JG_PRE_DATA);
            //alert(_JG_DATA);
            var arrSetting = _JG_DATA[0].replace('[', '').replace(']', '').split('|');
            var err = arrSetting[0];
            
            if (err != "") {
                this.grid.html(err);
                hideLoader(this.grid);
                return;
            }
            var arrhc = hc.split(',');


            this.recCount = arrSetting[1];

            this.totPages = parseInt(this.recCount / this.pageSize);
            if (this.recCount % this.pageSize > 0) this.totPages = this.totPages + 1;
            setParam(this.grid.find(".g_param"), "tp", this.totPages);
            
            var gcolumns = arrSetting[1].replace(/["']/g, "");
            var html = ""; //"<table class='repeater' cellpadding=3 cellspacing=0 width='100%'>";
            //if (QueryString("p") != "") this.enablePaging = true;
            var objFtr = this.grid.find(".repeater-header");
            var firsttr = objFtr.html();
            var colCount = objFtr.find("td").length;
//            if (firsttr.indexOf('>Edit<') < 0) {
//                html += "<tr class='repeater-header'><td width='30'>Edit</td>" + firsttr + "</tr>";
//            }
//            else {
//                html += "<tr class='repeater-header'>" + firsttr + "</tr>";
//            }
            
            if (this.enablePaging) {

                //html += "<tr><td colspan='20'><table width='100%'><tr><td width='40%'>Total Records : " + this.recCount + "</td><td align='right'>" + this.paging() + "</td></table></td></tr>";
                this.grid.find(".trpaging").remove();
                var print = ""; //"<td><img src='images/print.png'/></td>";
                var excel = ""; //"<td><img src='images/excel.png'/></td>";
                var pdf = ""; //"<td><img src='images/pdf.png'/></td>";
                
                this.grid.find(".repeater-header").before("<tr class='trpaging'><td colspan='20'>"+
                                "<table width='100%'><tr><td width='40%'>Total Records : "
                                + this.recCount + "</td><td align='right'>" + this.paging() + "</td>"
                                + refresh + gridsetting + print + excel + pdf + "</tr></table></td></tr>");
            }
            var hiddenRow = 1;
            var rowId = 1;
            var grid = this.grid;
            for (i = 1; i < _JG_DATA.length; i++) {
                if (this.gridType == 2) {
                    if (i % 2 == 0)
                        html += "<tr class='repeater-row' id='grid_" + rowId + "'>";
                    else
                        html += "<tr class='repeater-alt' id='grid_" + rowId + "'>";
                }
                else {
                    if (i % 2 == 0)
                        html += "<tr class='repeater-row'>";
                    else
                        html += "<tr class='repeater-alt'>";
                }
                rowId++;
                var d = _JG_DATA[i];
                var id = "";
                var viewUrl = this.viewUrl;
                var editUrl = this.editUrl;
                var colIndex = 0;
                var isfirst = true;

                if (this.gridType == 2) {
                    $.each(d, function(key, value) {
                    
                        value = checkAndConvertToDate(value);
                        if (arrhc.length > colIndex) {
                            html += "<input type='hidden' name='@sm_" + arrhc[colIndex] + "-" + hiddenRow + "' value='" + value + "'/>";
                        }
                        else {
                            if (value + '' == 'null' || value + '' == '') {
                                value = '&nbsp;';
                            }
                            html += "<td class='gridtr'>" + value + "</td>";
                            isfirst = false;
                        }
                        colIndex++;
                    });
                    html += "<td class='delete-row'><img src='images/close.png' width='16px' height='16px' /></td>";
                    hiddenRow++;
                }
                else {
                    var dataindex = 0;

                    $.each(d, function(key, value) {
                        value = checkAndConvertToDate(value);
                        if (arrhc.length > colIndex) {
                            editUrl = editUrl.replace("$" + arrhc[colIndex], value);
                            viewUrl = viewUrl.replace("$" + arrhc[colIndex], value);
                        }
                        else {
                            if (isfirst) {
                                id = value;
                                viewedit = "";
                                var target = "";
                                if (editUrl != "") {
                                    viewedit = "<a class='menu' href='" + editUrl + "' pt='" + editTitle + "'><img src='images/edit.png' width=20 title='Edit'/></a>&nbsp;";
                                }
                                if (viewUrl != "") {
                                    if (viewtarget == "1") {
                                        target = "target='_blank'";
                                    }
                                    else {
                                        target = "class='menu' ";
                                    }
                                    viewedit += "<a " + target + " href='" + viewUrl + "' pt='" + viewTitle + "'><img src='images/view.png' width=20 title='View Detail'/></a>";
                                }
                                html += "<td>" + viewedit + "</td>";
                            }
                            if (value + '' == 'null' || value + '' == '') {
                                value = '&nbsp;';
                            }
                            var gcolindex = 0;
                            var css = "";
                            var dt = "";
                            grid.find(".repeater-header").find("td").each(function() {
                                if (parseInt(gcolindex) == parseInt(dataindex) + 1) {
                                    dt = $(this).attr("dt");
                                }
                                gcolindex++;
                            });
                            if (dt == "amt") {
                                value = parseFloat(value).toFixed(2);
                                css = "right";
                            }
                            else if (dt == "int") {
                                css = "right";
                            }

                            html += "<td class='" + css + "'>" + value + "</td>";
                            isfirst = false;
                            dataindex++;
                        }
                        colIndex++;
                    });
                }
//                if (this.editUrl != "") {
//                    html += "<td><a href='"+this.editUrl+id+"'>Edit</a></td>";
//                    id = "";
//                }
                html += "</tr>";
            }
            if (this.gridType == 2) {
                prm = this.grid.find(".g_param");
                setParam(prm, "cr", rowId);
            }
            /*for(i=data.length-1;i<this.pageSize;i++) {
                if (i % 2 == 1)
                    html += "<tr class='repeater-row'><td colspan='"+colCount+"'>&nbsp;</td></tr>";
                else
                    html += "<tr class='repeater-alt'><td colspan='" + colCount + "'>&nbsp;</td></tr>";
            }*/
            if (this.enablePaging) {

                html += "<tr><td colspan='20'><table width='100%'><tr><td width='40%'>Total Records : " + this.recCount + "</td><td align='right'>" + this.paging() + "</td></table></td></tr>";
            }
            //html += "</table>";
            this.grid.append(html);
            hideLoader(this.grid);

            _en = new Date();
            $("#en").text(_en);
            $("#diff").text(_en-_st);
            
        }
        catch (e) {
            if (options.error != undefined) this.error(e.message); 
            hideLoader(this.grid);
            return;
        }
        if (options.success != undefined) this.success();
    }
    var grid = this.grid;
    this.grid.find(".delete-row").live("click", function() {
        $(this).parent().remove();
        var btnAdd = $("#btnAdd" + QueryString("") + "Detail");
        btnadd.val("Add");
    });

    this.grid.find(".gridtr").live("click", function() {
        var prm = grid.find(".g_param");
        var trid = $(this).parent().attr("id");
        sid = $(this).parent().attr("id").split('_')[1];
        setParam(prm, "sid", sid);
        
        var btnAdd = $("#btnAdd" + QueryString("") + "Detail");
        btnAdd.val("Update");

        $("#" + trid + " :input").each(function() {
            var nm = $(this).attr("name").split('-')[0].split('_')[1];
            $("#" + nm).val($(this).val());
        });
    });
    this.grid.find(".gridsetting").live("click",function(){
        var setting = grid.find(".g_setting");    
        var f = getParam(setting,"m");
        var jt = getParam(setting,"jt");
        jt = jt.replace(/l_/g,"");
        loadPage("#/grid/a/view/f/"+f+"/jt/"+jt, "Configure Grid");
    });
        
    function GetPaging() {
        var html = "<ul class='paging'>";
        this.totPages = parseInt(this.recCount / this.pageSize);
        if (this.recCount % this.pageSize > 0) this.totPages = this.totPages + 1;
        var st = 1;
        var en = this.totPages;
        for (i = 0; i < this.totPages; i++)
        {
            var PageBoxStart = this.maxPages * parseInt(i) + 1;
            var PageBoxEnd = this.maxPages * (parseInt(i) + 1);
            if (this.currentIndex >= PageBoxStart && this.currentIndex <= PageBoxEnd) {

                st = PageBoxStart;
                en = st + this.maxPages - 1;
                if (en >= this.totPages)
                {
                    en = this.totPages;
                    break;
                }
            }
        }
        //find the paging url
        var url = window.location.href;
        var inx = url.indexOf("#");
        var newurl = "";
        if (inx > 0) {
            var arr = url.substring(inx + 1).split('/');
            for (i = 1; i < arr.length; i++) {
                if (arr[i-1].toLowerCase() == "p") {
                    newurl = newurl + "/PAGE_NO";
                }
                else {
                    if (newurl == "") {
                        newurl += arr[i];
                    }
                    else {
                        newurl = newurl + "/" + arr[i];
                    }
                }
            }
        }
        newurl = "#/" + newurl;
        var prevPage = 1;
        var nextPage = this.totPages;
        if (parseInt(this.currentIndex) > 1) {
            prevPage = parseInt(this.currentIndex) - 1;
        }
        if (parseInt(this.currentIndex) < parseInt(this.totPages)) {
            nextPage = parseInt(this.currentIndex) + 1;
        }
        html += "<li><a href='" + newurl.replace("PAGE_NO", prevPage) + "'>Prev</a></li>";
        
        for (i = st; i <= en; i++) {
            url = newurl.replace("PAGE_NO", i);
            if (parseInt(i) == this.currentIndex) {
                html += "<li style='background-color:#0f65f1;'><a href='" + url + "' style='color:#ffffff'>" + i + "</a></li>";
            }
            else {
                html += "<li><a href='" + url + "'>" + i + "</a></li>";
            }
        }
        html += "<li><a href='" + newurl.replace("PAGE_NO", nextPage) + "'>Next</a></li>";
        html += "</ul>";
        return html;
    }
}
function refreshGrid(ctrl) {
    new JGrid
    (
        {
            grid: ctrl.closest(".grid")
        }
    ).bind();
}
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
        setParam(objParam, "p", pno)
        refreshGrid($(this));
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
        refreshGrid($(this));
    });
    //Search in grid
    $(".search").live("click", function() {
        var objgrid = $(this).closest(".grid");
        search(objgrid)
    });
    $(".clear-search").live("click", function() {
        var objgrid = $(this).closest(".grid");
        var param = objgrid.find(".g_param");
        setParam(param, "a", "");
        setParam(param, "w", "");
        objgrid.find(".keyword").val("");
        refreshGrid($(this));
    });

    $(".keyword").live("keypress", function(event) {
        if (event.which == 13) {
            var objgrid = $(this).closest(".grid");
            search(objgrid);
            return false;
        }
    });
    $(".refresh").live("click", function() {
        var objgrid = $(this).closest(".grid");
        refreshGrid($(this));
    });
    $(".viewdetail").live("click", function() {
        var objgrid = $(this).closest(".grid");
        var setting = objgrid.find(".g_setting");
        var viewUrl = getParam(setting, "viewUrl");
        //var url = $(this).closest("tr").find(".viewlnk").attr("href");
        //var pt = $(this).closest("tr").find(".viewlnk").attr("href");
        $(this).closest("tr").find(".viewlnk").trigger("click");
    });
});
function search_omkar(objgrid) {
    var param = objgrid.find(".g_param");
    var setting = objgrid.find(".g_setting");
    setParam(param, "a", "s");
    setParam(param, "p", "1");

    var w = "";
    var cw = getParam(param, "cw");//customized where condition
    var ew = getParam(param, "ew");//extra where condition
    var sb = objgrid.find(".searchby").val();
    var kw = objgrid.find(".keyword").val();
    var mt = getParam(setting, "mt");
    if (cw != "") {
        setParam(param,"w", cw);
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
            
            if (ew != "") {
                setParam(param, "w", "(" + w + ") " + ew );
            }
            else {
                setParam(param, "w", w);
            }
        }
    }
    refreshGrid(param);

}
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

    refreshGrid(param);

}
function LoadGrid(page) {
    page.find(".grid").each(function() {
        var targetGrid = $(this);
        new JGrid
        (
            {
                grid: targetGrid,
                currentIndex: 1,
                success: function() { },
                next: function() { },
                error: function(e) {

                    targetGrid.append("<tr><td colspan='20' class='error center'>Error : " + e + "</td></tr>");
                    hideLoader(targetGrid);
                }
            }
        ).bind();
    });
    
}
