function addGridRow(ctrl) {
    if (!IsValidRow(ctrl)) return false;
    var grid = getGrid(ctrl);
    var param = GetParam(ctrl);
    var smprefix = getParam(param, "smprefix");
    var currentRow = getCurrentRow(ctrl);
    var isadd = getParam(param, "isadd");
    var selectedId = getParam(param, "sid");
    if (isadd == "") isadd = "true";
    if (isadd != "true") {
        currentRow = selectedId;
    }
    var c = "repeater-row gr";
    if (currentRow % 2 == 0) {
        c = "repeater-alt gr";
    }
    var tr = $("<tr id='grid_" + currentRow + "' class='" + c + "'></tr>");
    var delbtn = $("<td class='delete-row'><img src='../images/close.png'/></td>");
    tr.append("<td class='hdn'></td>");
    $(".ac_results").hide();
    $(".add-menu").hide();
    grid.find(".newrow").find("input,select,textarea").each(function() {
        var type = $(this).attr("type");
        if (type != "button") {
            var id = $(this).attr("id");
            var val = $(this).val();
            var css = $(this).attr("class");
            if (css != undefined && css != null) {
                css = css.replace("ac ", "");
            }
            var cls = id;
            cls = css;
            var sprefix = smprefix
            if ($(this).attr("include") == "0") {
                sprefix = '_';
            }
            //for sales/purchase TAX

            if (_TAX_CRDR != "") {
                if (ConvertToInt($("#ledgerid").val()) > 0) {
                    if (id == "amount") {
                        id = _TAX_CRDR + "amount";
                    }
                }
            }
            var ctrlname = sprefix + id + "-" + currentRow;

            var ctrl = $('<input/>').attr({ type: 'hidden', name: ctrlname });
            if (cls != "") {
                ctrl.attr("class", cls);

                if (val == "") {
                    if (cls != undefined && (cls.indexOf("val-dbl") >= 0 || cls.indexOf("val-i") >= 0)) {
                        val = "0";
                    }
                }
            }
            ctrl.val(val);

            tr.find(".hdn").append(ctrl);

        }
    });
    grid.find(".srepeater-header td").each(function() {
        if ($(this).attr("cn") == undefined) {
            colname = $(this).text();
        }
        else {
            colname = $(this).attr("cn");
        }
        colname = colname.toLowerCase();
        if (colname != "") {
            var val = "";
            var align = "";
            //try {
            var ctrl = "";
            ctrl = grid.find("#" + colname);
            if(ctrl.length == 0)ctrl = grid.find("." + colname);
            
            if (ctrl[0].tagName == "SELECT") {
                val = ctrl.find("option:selected").text();
            }
            else {
                val = ctrl.val();
            }

            //} catch (e) { alert(e.message) }

            var colclass = colname;

            var colcls = ctrl.attr("class");
            if (colcls != undefined) {
                if (colcls.indexOf("val-dbl") >= 0) {
                    val = parseFloat(val).toFixed(2);
                    if (val == "NaN") {
                        val = "";
                    }
                }
                if (colcls.indexOf("right") >= 0) {
                    colclass += " right";
                }
            }
            if(val=="Select" || val=="0")val="";
            
            td = "<td" + align + " class='" + colclass + " gridtr'>" + val + "</td>";
            tr.append(td);
        }
    });
    tr.append(delbtn);
    
    var editRow = grid.find(".newrow");
    grid.find(".newrow").remove();

    if (isadd == "true") {//add
        tr.insertBefore(grid.find(".end"));
    }
    else {//edit
        grid.find("#grid_" + selectedId).replaceWith(tr);
        tr.attr("id", "grid_" + selectedId);
    }
    editRow.insertBefore(grid.find(".end"));
    editRow.find("input").val("");
    editRow.find("textarea").val("");
    focux(editRow);
    grid.find(".newrow").find("input").each(function() {
        if ($(this)[0].tagName == "INPUT" || $(this)[0].tagName == "SELECT") {
            if ($(this).attr("type") != "button") {
                $(this).val("");
            }
            if ($(this).attr("class") != undefined) {
                if ($(this).attr("class").indexOf("ac") >= 0) {
                    setAutoComplete($(this));
                }
            }
        }
    });
    setParam(param, "isadd", "true");
}
function IsValidRow(ctrl) {
    var isvalid = true;
    ctrl.closest("tr").find("input").each(function() {
    if ($(this).val().trim() == "" && isvalid && $(this).attr("type") == "text" && $(this).css("display")!="none" && $(this).attr("ir")=="1") {
            $(this).focus();
            isvalid = false;
        }
    });
    ctrl.closest("tr").find("select").each(function() {
    if ($(this).val() == "0" && isvalid && $(this).css("display") != "none" && $(this).attr("ir") == "1") {
            $(this).focus();
            isvalid = false;
        }
    });
    return isvalid;
}
function getGrid(obj) {
    return obj.closest(".grid");
}
function GetParam(obj) {
    return getGrid(obj).find(".g_param");
}
function getCurrentRow(obj) {
    var grid = getGrid(obj);
    var row = 0;
    grid.find(".gr").each(function() {
        row = $(this).attr("id").split('_')[1];
    });
    row = parseInt(row) + 1;
    return row;
}
function removeGridRow(obj) {
    var hdnid = obj.parent().find("input:first");
    if (hdnid.val() == "") {
        obj.parent().find("input").removeAttr("name");
        obj.parent().remove();
    }
    else {
        hdnid.val("-del" + hdnid.val());
        obj.parent().find(".input").val("");
        obj.parent().hide();
    }
}
function editGridRow(obj) {
    var grid = getGrid(obj);
    var trid = obj.parent().attr("id");
    if (trid == undefined) return;
    sid = obj.parent().attr("id").split('_')[1];
    var prm = GetParam(obj);
    if (getParam(prm, "isadd") == "false") return;
    setParam(prm, "sid", sid);
    setParam(prm, "isadd", "false");
    var idctrl = obj.parent().find(".id");
    var editRow = grid.find(".newrow");
    editRow.find("input").val("");
    obj.parent().find("input").each(function() {
        var cls = $(this).attr("class");
        //if (cls != "id") 
        {
            var nm = $(this).attr("name").split('-')[0].split('_')[1];
            if (nm == "cramount" || nm == "dramount") {
                nm = "amount";
            }
            var ctrl = grid.find("#" + nm);
            if(ctrl.length == 0)
            {
                grid.find("." + nm).val($(this).val());
            }
            else
            {
                grid.find("#" + nm).val($(this).val());
            }
        }
    });

    editRow.insertBefore(obj.parent());
    obj.parent().hide();
    if (editRow.find(".first") != undefined) {
        editRow.find(".first").focus();
    }
}
function IsValidGrid(grid,totalCtrl) {
    var isvalid = false;
    var isblank = true;
    grid.find(".newrow").find("input").each(function() {
        var type = $(this).attr("type");
        if (type != "button") {
            if ($(this).val() != "") {
                isblank = false;
            }
        }
    });
    if (!isblank) {
        alert("Please press enter sub grid row to add the data");
        grid.find(".newrow").find(".first").focus();
        return false;
    }
    if (totalCtrl != undefined) {
        var total = totalCtrl.text();
        if (ConvertToDouble(total) == 0) {
            alert("Total amount can not be zero");
        }
    }
    return true;
}