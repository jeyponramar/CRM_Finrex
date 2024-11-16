var _DG_DATA = null;
var _DG_PRE_DATA = null;

function dgrid(options) {
    this.gridtable = options.gridtable;
    this.add = add;
    this.remove = options.remove;
    this.btnadd = options.btnadd;
    this.onedit = options.onedit;
    this.populate = populate;
    this.edit = edit;
    this.currentRow;
    
    function addhidden(gridtable, tr, currentRow) {
        var p = gridtable.find(".g_setting");
        var ssm = getParam(p, "ssm");
        if (ssm == "1") {
            ssm = getParam(p, "m") + "_";
        }
        gridtable.find(".newrow").find("input").each(function() {
            var type = $(this).attr("type");
            if (type != "button") {
                var id = $(this).attr("id");
                var val = $(this).val();
                var css = $(this).attr("class");
                var cls = id;
                var prefix = getPrefix($(this));
                cls = css;
                if ($(this).attr("include")=="0") {
                    prefix = '_';
                }
                var ctrl = $('<input/>').attr({ type: 'hidden', name: prefix + ssm + id + "-" + currentRow });
                if (cls != "") ctrl.attr("class", cls);

                ctrl.val(val);
                tr.find(".hdn").append(ctrl);
            }
        });
        gridtable.find(".newrow").find("select").each(function() {
            var type = $(this).attr("type");
            var id = $(this).attr("id");
            var val = $(this).val();
            var css = $(this).attr("class");
            var cls = id;
            cls = css;
            var ctrl = $('<input/>').attr({ type: 'hidden', name: getPrefix($(this)) + ssm + id + "-" + currentRow });
            if (cls != "") ctrl.attr("class", cls);

            ctrl.val(val);
            tr.find(".hdn").append(ctrl);
        });

    }
    function getPrefix(ctrl)
    {
        var param = ctrl.closest(".grid").find(".g_param");
        return getParam(param,"smprefix");
    }
    function add() {
        //try {
            var cr = 1;
            var sid = 0;
            var isadd = "true";
            gridtable = this.gridtable;
            
            var param = this.gridtable.find(".g_param");
            var selectedId = 0;
            if (getParam(param, "isadd") != "") {
                isadd = getParam(param, "isadd");
            }
            if (getParam(param, "cr") != "") {
                cr = parseInt(getParam(param, "cr"));
            }
            if (getParam(param, "sid") != "") {
                selectedId = parseInt(getParam(param, "sid"));
            }
            var p = gridtable.find(".g_setting");
            var ssm = getParam(p, "ssm");
            if (ssm == "1") {
                ssm = getParam(p, "m") + "_";
            }
            this.currentRow = cr;
            
            var c = "repeater-row";
            if (this.currentRow % 2 == 0) {
                c = "repeater-alt";
            }
            var tr = $("<tr id='grid_" + this.currentRow + "' class='" + c + "'></tr>");
            var delbtn = $("<td class='delete-row'><img src='../images/close.png' width='16px' height='16px' /></td>");
            var arrcols = new Array();
            var colname = "";
            tr.append("<td class='hdn'></td>");
            
            this.gridtable.find(".srepeater-header td").each(function() {
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

                    if (colname.indexOf("~") > 0) {
                        val = gridtable.find("." + colname.replace("~", "")).val();
                    }
                    else {
                        if (gridtable.find("#" + colname)[0].tagName == "SELECT") {
                            val = gridtable.find("#" + colname).find("option:selected").text();
                        }
                        else {
                            val = gridtable.find("#" + colname).val();
                        }
                    }

                    //} catch (e) { alert(e.message) }

                    var dt = $(this).attr("dt");
                    colname = colname.replace("~", "");
                    var colclass = colname;

                    var colcls = gridtable.find("#" + colname).attr("class");
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
                        /*
                        if (dt == "amt") {
                        if (val != "") {
                        val = parseFloat(val).toFixed(2);
                        align = " style='text-align:right'";
                        }
                        }
                        else if (dt == "i") {
                        align = " style='text-align:right'";
                        }*/
                    }
                    td = "<td" + align + " class='" + colclass + " gridtr'>" + val + "</td>";
                    tr.append(td);
                }
            });
            var colIndex = 0;
            var currentRow = this.currentRow;
            gridtable = this.gridtable;
            var editRow;

            if (isadd == "true") {
                addhidden(this.gridtable, tr, currentRow);

                editRow = this.gridtable.find(".newrow");
                this.gridtable.find(".newrow").remove();
                //alert(this.gridtable.find(".end"))
                tr.insertBefore(this.gridtable.find(".end"));
                editRow.insertBefore(this.gridtable.find(".end"));
                this.currentRow = this.currentRow + 1;
                setParam(param, "cr", this.currentRow);
                //placeAdd(this.detailtable, this.gridtable);
                editRow.find("input").val("");
                var first = true;

            }
            else {
                
                //placeAdd(this.detailtable, this.gridtable);
                this.gridtable.find("#grid_" + selectedId).replaceWith(tr);
                tr.attr("id", "grid_" + selectedId);

                addhidden(this.gridtable, tr, currentRow);


                editRow = this.gridtable.find(".newrow");
                this.gridtable.find(".newrow").remove();

                editRow.insertBefore(this.gridtable.find(".end"));
                editRow.find("input").val("");

                //placeAdd(this.detailtable, this.gridtable)
                setParam(param, "isadd", "true");

            }
            focux(editRow);
            this.gridtable.find(".newrow").find("input").each(function() {
                if ($(this)[0].tagName == "INPUT" || $(this)[0].tagName == "SELECT") {
                    if ($(this).attr("type") != "button") {
                        $(this).val("");
                    }
                    if($(this).attr("class")!=undefined)
                    {
                        if($(this).attr("class").indexOf("ac")>=0)
                        {
                            setAutoComplete($(this));
                        }    
                    }
                }
            });
            tr.append(delbtn);
            edit();
        //}
        //catch (e) {alert(e);
        //    return false;
        //}
        return false;
    }
    
    function edit() {
        gridtable = this.gridtable;
        this.gridtable.find(".delete-row").live("click", function() {
            $(this).parent().find("input").removeAttr("name");
            $(this).parent().remove();
            if (options.remove != undefined) options.remove();
        });
        this.gridtable.find(".gridtr").live("click", function() {
            var trid = $(this).parent().attr("id");
            if (trid == undefined) return;
            sid = $(this).parent().attr("id").split('_')[1];
            var prm = gridtable.find(".g_param");
            if (getParam(prm, "isadd") == "false") return;
            setParam(prm, "sid", sid);
            setParam(prm, "isadd", "false");
            var idctrl = $(this).parent().find(".id");
            
            var editRow = gridtable.find(".newrow");
            editRow.find("input").val("");
            $(this).parent().find("input").each(function() {
                var nm = $(this).attr("id").split('-')[0].split('_')[1];
                if (nm.indexOf("~") > 0) {
                    gridtable.find("." + nm.replace("~", "")).val($(this).val());
                }
                else {
                    gridtable.find("#" + nm).val($(this).val());
                }
            });

            editRow.insertBefore($(this).parent());
            $(this).parent().hide();
            if (options.onedit != undefined && options.onedit != null) options.onedit();
            if (editRow.find(".first") != undefined) {
                editRow.find(".first").focus();
            }
        });
    }

    function populate() {
        
        try{
        
        var setting = this.gridtable.find(".g_setting");
        var hc = getParam(setting, "hc");
        var gc = getParam(setting, "gc");
        var del = getParam(setting, "del");
        var view = getParam(setting, "view");
        var srno = getParam(setting, "srno");
        var param = this.gridtable.find(".g_param");
        if (getParam(param, "sb") == "") {
            sb = getParam(setting, "m") + "Id";
            setParam(param, "sb", sb);
        }
        var qp = getQparam(setting, param);
        var arrhc = hc.split(',');
        var arrgc = gc.split(',');
        var ssm = getParam(setting, "ssm");
        if (ssm == "1") {
            ssm = getParam(setting, "m") + "_";
        }

        var ashxurl = "getdata.php?" + qp;
        //alert(ashxurl);
        //var ashxurl = "getdata.php?m=" + m + "&mt="+mt+"&hc=" + hc + "&gc=" + gc + "&pm=" + param + "&jt="+jt;
        //document.write(ashxurl);
        var _DG_PRE_DATA = RequestData(ashxurl);
        var _DG_DATA = jQuery.parseJSON(_DG_PRE_DATA);
        //alert(_DG_DATA);
        var prevtr = this.gridtable.find(".srepeater-header");
        gridtable = this.gridtable;
        var isdel = "";
        var delbtn = "";
        if (del == "0" || view=="1") {
        }
        else {
            delbtn = "<td class='delete-row'><img src='../images/close.png' width='16px' height='16px' /></td>";
        }
        if (_DG_DATA.length > 1) {
            setParam(param, "cr", _DG_DATA.length);
        }
        for (i = 1; i < _DG_DATA.length; i++) {
            var d = _DG_DATA[i];
            var html = "";
            if (view == "1") {
                html += "<tr id='grid_" + i + "'>";
            }
            else {
                if (i % 2 == 0) {
                    html += "<tr class='repeater-row' id='grid_" + i + "'>";
                }
                else {
                    html += "<tr class='repeater-alt' id='grid_" + i + "'>";
                }
            }
            
            var colIndex = 0;
            var gccolIndex = 0;
            var hdnctrls = "";
            var first = true;
            $.each(d, function(key, value) {
                if (value + '' == 'null' || value + '' == '') {
                    value = '';
                }
                else {
                    value = checkAndConvertToDate(value);
                }
                if (arrhc.length > colIndex) {
                    var cls = "";
                    var ctrl = gridtable.find(".newrow").find("." + arrhc[colIndex]);
                    if (ctrl.attr("class") != undefined) {
                        cls = ctrl.attr("class");
                    }
                    hdnctrls += "<input type='hidden' name='@sm_" + ssm + arrhc[colIndex] + "-" + i + "' value='" + value + "' class='" + cls + "'/>";
                    colIndex++;
                }
                else {
                    var cls = "";
                    var ctrl = gridtable.find(".newrow").find("." + arrgc[gccolIndex]);
                    if (ctrl != undefined) {
                        if (ctrl.attr("class") != undefined) {
                            cls = ctrl.attr("class");
                        }
                    }
                    var lblcss = "";
                    if (cls.indexOf("right") >= 0) {
                        lblcss = "right";
                    }
                    if (cls.indexOf("val-dbl") >= 0) {
                        value = parseFloat(value).toFixed(2);
                    }
                    if (cls.indexOf("nozero") >= 0) {
                        if (value == 0) value = "";
                    }
                    if (first) {
                        html += "<td class='hdn'>" + hdnctrls + "</td>";
                        if (srno == "1") {
                            html += "<td>" + i + "</td>";
                        }
                        first = false;
                    }
                    if (value == '') {
                        value = '&nbsp;';
                    }
                    if (view == "1") {
                        html += "<td class='gridtd " + lblcss + "'>" + value + "</td>";
                    }
                    else {
                        html += "<td class='gridtr gridtd " + lblcss + "'>" + value + "</td>";
                    }
                    gccolIndex++;
                }

            });
            html += delbtn;
            html += "</tr>";
            var curtr = $(html);
            curtr.insertAfter(prevtr);
            prevtr = curtr;
        }
        if (del != "0") edit();
        gridtable.find(".newrow input").val(""); 
    }catch(edit){alert(edit);}
  } 
}
function populate_dgrid(grid, findtotal, onedit) {
    new dgrid(
        {
            gridtable: grid,
            remove: findtotal,
            onedit: onedit
        }
     ).populate();
}
function adddgrid(obj, event, grid, findtotal, preCalculate, onedit) {

    if (event.which == 13) {
        if (preCalculate != undefined) {
            if (!preCalculate(event)) {//validate or calculate before add
                return false;
            }
        }
        if (obj.attr("class") != undefined) {
            if (obj.attr("class").indexOf("val-dbl") >= 0) {
                formatAmnt(obj);
            }
        }
        new dgrid(
                {
                    gridtable: grid,
                    remove: findtotal,
                    onedit : onedit
                }
            ).add();
             
        if (findtotal != undefined) findtotal();
        
        return false;
    }
    return true;
}
$(document).ready(function() {
    $(".ddlcrbr").change(function() {
        if ($(this).val() == "Dr") {
            $(this).parent().find(".DrAmount").attr("readnly", "readonly");
        }
    });
});

function intitgrid(options) {
    var mainobj = options.table;
    var isform = options.isform;
    this.start = start;
    this.grid = options.grid;
    this.findtotal = options.findtotal;
    this.aftertotal = options.aftertotal;
    this.preCalculate = options.preCalculate;
    this.onedit = options.onedit;
    this.onsave = options.onsave;
    this.onsaved = options.onsaved;
    this.m = options.m;
    this.ic = options.ic;
    this.bind = bind;
       
    var grid;
    var newrow = mainobj.find(".newrow");
    var findtotal;
    var m = this.m;

    if (options.grid == undefined) {
        grid = mainobj.find(".grid");
    }
    else {
        grid = options.grid;
    }
    if (options.findtotal == undefined) {
        findtotal = findTotal;
    }
    else {
        findtotal = options.findtotal;
    }
    function start() {
        if (grid.html() != null) {
            FillDropdown(mainobj);
            newrow.find("input").live("keypress", function(event) {
                if (event.keyCode == 13) {
                    return adddgrid($(this), event, grid, findtotal, validategrid, options.onedit);
                }
            });
            frm = mainobj.closest("form");
            if (isform != false) {
                frm.find('.save').click(function() {

                    if (options.onsave != undefined) {
                        if (!options.onsave()) {
                            return false;
                        }
                    }
                    SetZeroForAmount(frm);
                    Save(frm);
                    if (options.onsaved != undefined) {
                        options.onsaved();
                    }                   
                    return false;

                });
            }
            mainobj.find("#ProductId").live("change", function() {
                if ($(this).find("option:selected").attr("per") == undefined) {
                    mainobj.find("#Quantity").show();
                    mainobj.find("#Price").show();
                    mainobj.find("#Amount").show();
                    mainobj.find("#IsTax").val("0");
                    mainobj.find("#TaxId").val("0");
                }
                else {
                    mainobj.find("#Quantity").hide();
                    mainobj.find("#Price").show();
                    mainobj.find("#Amount").show();
                    mainobj.find("#Quantity").val("");
                    mainobj.find("#Price").val($(this).find("option:selected").attr("per"));
                    mainobj.find("#IsTax").val("1");
                    mainobj.find("#TaxId").val($(this).val());
                }
            });
            bind();
            findTotal();
        }
    }
    function findTotal() {
        mainobj.find(".total").each(function() {
            var cn = $(this).attr("cn");
            var total = 0;            
            if ($(this).attr("dt") == "i") {
                total = parseInt(getTotal(grid, cn));             
            }
            else {
                total = parseFloat(getTotal(grid, cn)).toFixed(2);                 
            }
            if ($(this)[0].tagName == "TD") {
                $(this).text(total);
            }
            else {
            
                $(this).val(total);
            }

        });
        setTaxId();
        if(options.aftertotal!=undefined)options.aftertotal();
    }    
    function validategrid() {
        var isvalid = true;
        newrow.find("input").each(function() {
            var ir = $(this).attr("ir");
            if (ir == "1") {
                if ($.trim($(this).val()) == "") {
                    $(this).focus();
                    isvalid = false;
                    return false;
                }
            }
        });
        newrow.find("select").each(function() {
            var ir = $(this).attr("ir");
            if (ir == "1") {
                if ($(this).val() == "0") {
                    $(this).focus();
                    isvalid = false;
                    return false;
                }
            }
        });
        calculateAmount();
        
        if (options.preCalculate != undefined) {
            if (!options.preCalculate()) {
                return false;
            }
        }
        return isvalid;
    }
    function setTaxId() {
        mainobj.find(".grid").find(".repeater-row,.repeater-alt").each(function() {
            if ($(this).find(".TaxId").val() != "" && $(this).find(".TaxId").val() != "0") {
                $(this).find(".ProductId").val("0");
            }
        });
    }
    function calculateAmount() {
        if (mainobj.find("#Price") == undefined) return;
        if (mainobj.find("#Amount") == undefined) return;
        var istax = false;
        if (mainobj.find("#IsTax") != undefined) {
            if (mainobj.find("#IsTax").val() == "1") {
                istax = true;
            }
        }
        var r = parseFloat(mainobj.find("#Price").val());
        if (istax) {
            if (mainobj.find("#Amount").val() == "") {
                var subtotal = getSubTotal(mainobj.find(".grid"), "Amount");
                mainobj.find("#Amount").val(parseFloat((r / 100) * subtotal).toFixed(2));
            }
            mainobj.find("#Unit").val("%");
        }
        else {
            var q = parseFloat(mainobj.find("#Quantity").val());
            mainobj.find("#Amount").val(parseFloat(r * q).toFixed(2));
        }
        if (mainobj.find("#Amount").val() == "NaN") mainobj.find("#Amount").val("");
    }
    
    function bind() {
        if (QueryString("id") != "") {
            populateData(mainobj);
            var param = mainobj.find(".grid").find(".g_param");
            var setting = mainobj.find(".grid").find(".g_setting");
            setParam(param, "w", getParam(setting, "ic") + "=" + QueryString("id"));
            populate_dgrid(mainobj.find(".grid"));            
            populateTax();
        }
    }
    function populateTax()
    {
        mainobj.find(".grid").find(".TaxId").each(function() {
            if ($(this).val() != "" && $(this).val() != "0") {
                var lnm = getLedgerName($(this).val());
                $(this).parent().parent().find(".gridtd:first").text(lnm);
            }
        });   
    }
}
