var _insid = 0;
var _DETAIL_DATA = null;
var _DETAIL_PRE_DATA = null;
var _MAINOBJ = null;
function populateData(tbl) {
    new Jdb
        (
            {
                table: tbl
            }
        ).populate();
}

function Jdb(options) {
    this.table = options.table;
    //this.m = options.m;
    //this.ev = options.ev;
    this.populate = populate;
    function populate() {
        try {
            if (QueryString("id") == "") return;
            var setting = this.table.find(".g_setting");
            var param = this.table.find(".g_param").val();
            var m = getParam(setting, "m"); 
            var mt = getParam(setting, "mt"); 
            var jt = getParam(setting, "jt"); 
            var ic = getParam(setting, "ic"); 
            var ashxurl = "getdetail.php?m=" + m + "&mt=" + mt + "&ic=" + ic + "&id=" + QueryString("id") + "&prm=" + param + "&jt=" + jt;
            //alert(ashxurl);
            //document.write(ashxurl);
            showLoader(this.table);
            _DETAIL_PRE_DATA = RequestData(ashxurl);
            var _DETAIL_DATA = jQuery.parseJSON(_DETAIL_PRE_DATA);
            //alert(_DETAIL_DATA);
            var arrCols = getFields(this.table).split(',');

            for (i = 0; i < _DETAIL_DATA.length; i++) {
                var d = _DETAIL_DATA[i];
                for (j = 0; j < arrCols.length; j++) {
                    var colName = arrCols[j];
                    var value = d[colName];
                    if (value == undefined || value == "null") value = "";
                    value = checkAndConvertToDate(value);
                    var objCtrl = this.table.find("#" + colName);
                    if (objCtrl.attr("dt") != undefined) {
                        value = format(value, objCtrl.attr("dt"));
                    }
                    var cls = objCtrl.attr("class");
                    if (cls != undefined) {
                        if (cls.indexOf("val-dbl") >= 0) {
                            if (value == "" || value == null) {
                                value = "0.00";
                            }
                            else {
                                value = parseFloat(value).toFixed(2);
                            }
                        }
                    }
                    var tagname = "";
                    try {
                        tagname = objCtrl[0].tagName;
                    } catch (e) { }
                    
                    if (objCtrl.attr("class").indexOf("datepicker") >= 0) {
                        value = convertToDate(value);
                    }
                    if (tagname == "INPUT" || tagname == "TEXTAREA" || tagname == "SELECT") {
                        objCtrl.val(value);

                        if (tagname == "SELECT") {
                            objCtrl.attr("v", value);
                            if (objCtrl.val() != "" && objCtrl.val() != "0") {
                                replaceDropdownWithText(objCtrl);
                            }
                        }
                        else {
                            replaceTextBoxWithText(objCtrl);
                        }
                    }
                    else {
                        objCtrl.text(value);
                    }
                }

            }
            if (QueryString("a") == "detail") {
                this.table.find("input").each(function() {
                    if ($(this).attr("type") == "button" || $(this).attr("type") == "submit") {
                        $(this).hide();
                    }
                });
            }
            hideLoader(this.table);
        } catch (e) {
            hideLoader(this.table);
            alert(e)
        }
    }
    function getFields(table) {
        var fields = "";
        table.find(".dfield").each(function() {
            if (fields == "") {
                fields = $(this).attr("id");
            }
            else {
                fields = fields + "," + $(this).attr("id");
            }
        });
        return fields;
    }
}
function bindDetailForm() {
    mainobj.find("input,textarea,select").each(function() {
        var isvalidctrl = false;
        var val = "";
        if ($(this)[0].tagName == "SELECT") {
            isvalidctrl = true;
            val = $(this).find("option:selected").text();
            if (val.indexOf("Select") == 0) {
                val = "&nbsp;";
            }
        }
        else if ($(this).attr("type") == "text" || $(this)[0].tagName == "TEXTAREA") {
            val = $(this).val();
            isvalidctrl = true;
        }
        else if ($(this).attr("type") == "button" || $(this).attr("type") == "submit") {
            $(this).hide();
        }
        if (isvalidctrl) {
            var span = $("<span>" + val + "</span>")
            $(this).replaceWith(span);
            span.closest("td").attr("class", "val");
        }

    });
}
function setDateFormat(frm) {
    $(".datepicker").each(function() {
        var dt = $(this).val();
        var arr = '';
        if (dt.indexOf('-') > 0) {
            arr = dt.split('-');
        }
        else if (dt.indexOf('/') > 0) {
            arr = dt.split('/');
        }
        else if (dt.indexOf('.') > 0) {
            arr = dt.split('.');
        }
        if (arr.length == 3) {
            var d = arr[0];
            var m = arr[1];
            var y = arr[2];
            if (d.length == 1) d = '0' + d;
            if (m.length == 1) m = '0' + m;
            if (y.length == 2) y = '20' + y;
            $(this).val(d + '-' + m + '-' + y);
        }
    });
}

function Save(frm,isClose,issubmit,isclear) {
    var isvalid = validate(frm);
    if (!isvalid) return;

    if(issubmit == undefined)
    {
        var id = 0;
        var setting = frm.find(".g_setting");
        var param = frm.find(".g_param").val();
        var m = getParam(setting, "m");
        var sm = getParam(setting, "sm");
        var ic = getParam(setting, "ic");

        if (QueryString("id") != "") id = QueryString("id");
        var URL = "adddata.php?m=" + m + "&sm=" + sm + "&ic=" + ic + "&id=" + id;
    }
    else
    {
        URL = frm.attr("action");
    }
    setDateFormat();
    
//    frm.attr("action", URL);
//    frm.submit();
//    return true;
//    
    $.ajax({
        url: URL,
        type: 'post',
        dataType: 'json',
        async: false,
        data: frm.serialize(),
        success: function(data) {
            var newId = "" + data;
            _insid = newId;
            if (newId.indexOf("Error") >= 0) {
                alert("Error occurred while saving data");
            }
            else {
                alert("Data saved successfully");
                if(isclear!=false)
                {
                    frm[0].reset();
                    frm.find("textarea").val("");
                }
                if (QueryString("close") == "1") {
                    closeTab(curPage);
                }
                else {
                    if (isClose == undefined) {
                        if (QueryString("id") != "") {
                            closeTab(curPage);
                        }
                        else {
                            loadPage(getCurrentUrl(), getCurrentTitle());
                        }
                    }
                    else {
                        if (isClose) {
                            closeTab(curPage);
                        }
                    }
                }
            }
        },
        complete: function(data) {
            //alert(data);
        },
        error: function(xhr, status, error) {
            alert("Error occurred while saving data");
        }
    });
    
    return false;  
    //return _insid;            
}

function FillDropdown(table) {
    table.find(".mddl").each(function() {
        m = $(this).attr("m");
        idval = $(this).attr("id");
        dcn = $(this).attr("dcn");

        icn = $(this).attr("icn");
        if (icn != undefined) {
            $(this).append($("<option></option>").val("-1").html("Create New..."));
        }        
        $(this).append($("<option></option>").val("0").html("Select"));       
        dropdown($(this), m, dcn);

    });

    $(".mddl").live("change", function() {
        //findLegderBal($(this));
        mid = $(this).val();
        mcn = $(this).attr("Id");
        target = $(this).attr("target");
        if (target == undefined) return;

        dcn = $(this).attr("dcn");
        //        dcn = $("#" + target).attr("dcn"); 
        //        if (dcn == undefined) {
        //            dcn = "";
        //        }        
        m = $("#" + target).attr("m");
        $("#" + target).find('option').remove()
        $("#" + target).append($("<option></option>").val("0").html("Select"));
        dropdown($("#" + target), m, dcn, mid, mcn);
        
    });
}
function dropdown(ddl, m, dcn, mid, mcn) {
    if (mid == undefined) mid = "";
    if (mcn == undefined) mcn = "";
    if (dcn == undefined) dcn = ""; //display column name
    var ec = ddl.attr("ec");
    if (ec == undefined) ec = "";
    URL = "dropdown.php?m=" + m + "&mid=" + mid + "&mcn=" + mcn + "&dcn=" + dcn + "&ec=" + ec;
    //alert(URL);
    //document.write(URL);
    $.ajax({
        url: URL,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: true,
        success: function(data) {
            //var data = jQuery.parseJSON(data);
            var extracols = ec.split(',');
            for (i = 0; i < data.length; i++) {
                var id = data[i][0];
                var nm;
                if (dcn == "") {
                    nm = data[i][1];
                }
                else {
                    nm = data[i][dcn];
                }
                var extravals = "";
                if (extracols.length > 0) {
                    for (j = 0; j < extracols.length; j++) {
                        extravals += " " + extracols[j] + "='" + data[i][extracols[j]] + "'";
                    }
                }
                ddl.append($("<option value='" + id + "' " + extravals + ">" + nm + "</option>"));
            }
            
            _en = new Date();
            $("#en").text(_en);
            $("#diff").text(_en - _st);
            
            if (ddl.attr("v") != undefined) {
                ddl.val(ddl.attr("v"));
                replaceDropdownWithText(ddl, 1);
            }
        }
    });
    if (ddl.attr("istax") == "1") {
        addTaxItems(ddl);   
    }
}
function replaceDropdownWithText(ddl) {
    if (QueryString("a") == "detail") {
        var id = ddl.val();
        if (id == "" || id == "0") {
            id = ddl.attr("v");
            if (id == "" || id == "0" || id == undefined) return;
        }
        var val = ddl.find("option:selected").text();
        if (val == "") val = "&nbsp;";
        
        var span = $("<span>" + val + "</span>")
        ddl.replaceWith(span);
        span.closest("td").attr("class", "val");
        //ddl.closest("td").append(span);
    }
}
function replaceTextBoxWithText(txt) {
    if (QueryString("a") == "detail") {
        if (txt.attr("type") == "text" || txt[0].tagName == "TEXTAREA") {
            val = txt.val();
            var span = $("<span>" + val + "</span>")
            txt.replaceWith(span);
            span.closest("td").attr("class", "val");
        }
    }
}
function deletedetail(frm) {
    var setting = frm.find(".g_setting");    
    var m = getParam(setting, "m");    
    var sm = getParam(setting, "sm");
    var ic = getParam(setting, "ic");
    var ashxurl = "delete.php?m=" + m + "&sm=" + sm + "&ic=" + ic + "&id=" + QueryString("id"); // + "&param=" + param;
    var prejsndata = RequestData(ashxurl);
    alert("Data Deleted"); 
}
function master(id,setdatepicker,validate) {
    var mainobj = getobj(id);
    _MAINOBJ = mainobj;
    if (setdatepicker == true) {
        mainobj.find(".datepicker").each(function() {
            setDatePicker($(this));
        });
    }
    FillDropdown(mainobj);
    populateData(mainobj);
    mainobj.find('.save').click(function() {
        if (validate != undefined) {
            if(!validate())return false;
        }
        Save(mainobj);
        return false;
    });
}