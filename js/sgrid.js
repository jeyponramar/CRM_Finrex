$(document).ready(function() {
    //restrict adding product duplicate 
    //restrictDuplicateProduct();
});
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
    if (!isDuplicateProduct(ctrl, isadd)) return false;
    var c = "repeater-row gr";
    if (currentRow % 2 == 0) {
        c = "repeater-alt gr";
    }
    var tr = $("<tr id='grid_" + currentRow + "' class='" + c + "'></tr>");
    var imgDel = "../images/close.png";
    if (_isMobile) imgDel = "../../images/close.png";
    var delbtn = $("<td class='delete-row'><img src='"+imgDel+"'/></td>");
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
            var m = $(this).attr("m");
            if (m != undefined) {
                //fix: for dropdown
                cls = cls.replace(m, "");
            }
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
            if (ctrl.length == 0) {
                //fix: for dropdown server control
                grid.find("input,select,textarea").each(function() {
                    var id = $(this).attr("id");
                    if (id != undefined) {
                        if (id.indexOf("ctl00_") == 0) {
                            if (id.indexOf("_" + colname) > 0) {
                                ctrl = $(this);
                            }
                        }
                    }
                });
                if (ctrl.length == 0) ctrl = grid.find("." + colname);
            }
            try
            {
                if (ctrl != undefined && ctrl[0].tagName == "SELECT") {
                    val = ctrl.find("option:selected").text();
                }
                else {
                    val = ctrl.val();
                }
            }
            catch(e)
            {
                alert("Error");
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
            if (val == "Select" || val == "0") val = "";

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
    editRow.find("select").val(0);
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
    return true;
}
function IsValidRow(ctrl) {
    var isvalid = true;
    ctrl.closest("tr").find("input").each(function() {
        if ($(this).val().trim() == "" && isvalid && $(this).attr("type") == "text" && $(this).css("display") != "none" && $(this).attr("ir") == "1") {
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
    /*
    if (ctrl.closest("tr").find("#productid").length > 0 && ctrl.closest("tr").find("#makeid").length > 0 && ctrl.closest("tr").find("#modelnoid").length > 0) {
        //if grid have product, make and model no
        var pid = ConvertToInt(ctrl.closest("tr").find("#productid").val());
        //
        var taxid = ctrl.closest("tr").find("#productname").attr("v");
        var arrTaxid = taxid.split('-');
        if (arrTaxid.length >= 2) {
            taxid = ConvertToInt(arrTaxid[2]);
            if (taxid > 0) {
                isvalid = true;
            }
        }
        //   
        var mid = ConvertToInt(ctrl.closest("tr").find("#makeid").val());
        var mnid = ConvertToInt(ctrl.closest("tr").find("#modelnoid").val());
        if (pid > 0 || mid > 0 || mnid > 0) {
            var url = "../utilities.ashx?m=validate-modelno&pid=" + pid + "&mid=" + mid + "&mnid=" + mnid;
            var isvalid = RequestData(url);
            if (isvalid == "0") {
                alert("Invalid combination of Product, Make and Model No");
                isvalid = false;
            }
        }
    }*/

    
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
    //var hdnid = obj.parent().find("input:first");
    var hdnid = obj.parent().find(".id");
    if (hdnid.length == 0) {
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
            if (ctrl.length == 0) {
                grid.find("." + nm).val($(this).val());
            }
            else {
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
function IsValidGrid(grid, totalCtrl) {
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
function IsProductExistsInGrid(gridCss) {
    var productid = ConvertToInt($("#productid").val());
    var makeid = ConvertToInt($("#makeid").val());
    var modelnoid = ConvertToInt($("#modelnoid").val());

    if ($("." + gridCss).length == 0) return false;
    var exists = false;
    $("." + gridCss).find(".repeater-row,.repeater-alt").each(function() {
        var pid = ConvertToInt($(this).attr("productid"));
        var mid = ConvertToInt($(this).attr("makeid"));
        var mnid = ConvertToInt($(this).attr("modelnoid"));
        if (pid == productid && mid == makeid && mnid == modelnoid) {
            exists = true;
        }
    });
    if (exists) {
        return true;
    }
    else {
        alert("Product does not exists, please check!");
        return false;
    }
}
function GetQuantityOfProductInGrid(gridCss) {
    var productid = ConvertToInt($("#productid").val());
    var makeid = ConvertToInt($("#makeid").val());
    var modelnoid = ConvertToInt($("#modelnoid").val());

    if ($("." + gridCss).length == 0) return 0;
    var quantity = 0;
    $("." + gridCss).find(".repeater-row,.repeater-alt").each(function() {
        var pid = ConvertToInt($(this).attr("productid"));
        var mid = ConvertToInt($(this).attr("makeid"));
        var mnid = ConvertToInt($(this).attr("modelnoid"));
        if (pid == productid && mid == makeid && mnid == modelnoid) {
            quantity = ConvertToInt($(this).attr("quantity"));
        }
    });
    return quantity;
}
function restrictDuplicateProduct1() {
    $("#productname").live("blur", function(e) {
        if ($(this).closest(".grid").length > 0)//check whether product in subgrid
        {
            var $this = $(this);
            var ctProductId = $("#productid").val();
            $(this).closest(".grid").find("input").each(function() {
                var nm = $(this).attr("name");
                if (nm != undefined && nm.indexOf("_productid-") > 0) {
                    var pid = $(this).val();
                    if (pid == ctProductId) {
                        alert("Product already added!");
                        $this.focus();
                        e.preventDefault();
                    }
                }
            });
        }
    });
}
function isDuplicateProduct(ctrl, isadd) {
    return true;//NO NEED TO CHECK DUPLICATE
    if ($("#productid").length == 0) return true;
    var ctProductId = $("#productid").val();
    if (ctProductId.indexOf("-") > 0) {
        var arr = ctProductId.split('-');
        if (arr.length > 2) {
            if (arr[2] == "1") {//check for tax/charges
                return true;
            }
        }
    }
    var isvalid = true;
    var occurance = 0;
    ctrl.closest(".grid").find(".hdn").each(function() {//loop thru rows
        if (ctrl.css("display") == "none") return;
        $(this).find("input").each(function() {
            var nm = $(this).attr("name");
            if (nm != undefined && nm.indexOf("_productid-") > 0) {
                var pid = $(this).val().replace("-0-0", "");
                if (pid == ctProductId) {
                    occurance++;
                }
            }
        });
    });
    if ((isadd == "true" && occurance > 0) || (isadd != "true" && occurance > 1)) {
        alert("Product already added!");
        ctrl.focus();
        isvalid = false;
    }
    return isvalid;
}