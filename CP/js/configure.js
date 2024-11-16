var _config_lbl;
var _config_txt;
var _config_type;
var _prev_uid = 0;
$(document).ready(function() {
    $(".delete").click(function() {
        var r = confirm("Are you sure you want to delete this record?\n\nYou can not rollback once data is deleted!")
        return r;
    });
    droptool();
    $(".content-panel").css("height", $(window).height() - 80);
    $(".issingle input").click(function() {
        var html = "";
        if ($(this).is(":checked")) {
            html = '<tr>' +
                        '<td width="100%" class="left-column">' +
                            '<table width="100%" class="container" cellpadding="0" cellspacing="0">' +
                                '<tr><td class="drop last" style="height:100px">&nbsp;</td><td class="drop last">&nbsp;</td></tr>' +
                            '</table>' +
                        '</td>' +
                    '</tr>';
        }
        else {
            html = '<tr>' +
                        '<td width="50%" class="left-column">' +
                            '<table width="100%" class="container" cellpadding="0" cellspacing="0">' +
                                '<tr><td class="drop last" style="height:100px">&nbsp;</td><td class="drop last">&nbsp;</td></tr>' +
                            '</table>' +
                        '</td>' +
                        '<td width="50%" class="right-column">' +
                            '<table width="100%" class="container" cellpadding="0" cellspacing="0">' +
                                '<tr><td class="drop last" style="height:100px">&nbsp;</td><td class="drop last">&nbsp;</td></tr>' +
                            '</table>' +
                        '</td>' +
                    '</tr>';
        }
        $(".maincontainer").html(html);
        droptool();
    });
    $(".tool").draggable({
        appendTo: "body",
        helper: "clone"
    });
    function droptool() {
        $(".drop").droppable({
            activeClass: "ui-state-hover",
            hoverClass: "drop-hover",
            drop: function(event, ui) {
                var focusctrl = $(this);
                _config_type = ui.draggable.text();
                var control = "";
                var tr;
                if (_config_type == "Sub Grid" || _config_type == "Section") {
                }
                else
                {
                    _prev_uid = focusctrl.closest("tr").prev().find("input,textarea,select").attr("uid");
                    if(_prev_uid == undefined) _prev_uid = 0;
                }
                $(".frmcolumn").find("select").val("0");
                $(".frmcolumn").find("input").each(function() {
                    if ($(this).attr("type") == "checkbox") {
                        $(this).removeAttr('checked');
                    }
                    else if ($(this).attr("type") == "text" && $(this).attr("class").indexOf("nodisplay") < 0) {
                        $(this).val("");
                    }
                });
                $(".size").val("50");
                
                if (_config_type == "Sub Grid") {
                    tr = $("<tr><td style='background-color:#222222' colspan=2 class='sgrid'>" +
                            "<table class='subgrid' width='100%'><tr><td class='stitle' colspan=2>Sub Grid</td></tr>" +
                            "<tr><td class='drop' colspan=10 style='background-color:#444444;width:100%;'>&nbsp;</td></tr></table>" +
                            "</td></tr>");
                    $(".maincontainer").append(tr);
                    $(".controltype").text(_config_type);
                    $(".control").val(_config_type);
                    openConfigure(tr.find(".stitle"), tr.find(".sgrid"), true,false);
                    droptool();
                    return;
                }
                else if (_config_type == "Section") {
                    tr = $("<tr><td style='background-color:#222222' colspan=2 class='section'>" +
                            "<table class='subsection' width='100%'><tr><td class='sectiontitle' colspan=2>Section</td></tr>" +
                                    "<tr>" +
                                    "<td width='50%' class='left-column'>" +
                                        "<table width='100%' class='container' cellpadding='0' cellspacing='0'>" +
                                            "<tr><td class='drop last' style='height:100px'>&nbsp;</td><td class='drop last'>&nbsp;</td></tr>" +
                                        "</table>" +
                                    "</td>" +
                                    "<td width='50%' class='right-column'>" +
                                        "<table width='100%' class='container' cellpadding='0' cellspacing='0'>" +
                                            "<tr><td class='drop last' style='height:100px'>&nbsp;</td><td class='drop last'>&nbsp;</td></tr>" +
                                        "</table>" +
                                    "</td>" +
                                "</tr>" +
                            "</table>" +
                            "</td></tr>");
                    $(".maincontainer").append(tr);
                    $(".controltype").text(_config_type);
                    $(".control").val(_config_type);
                    openConfigure(tr.find(".sectiontitle"), tr.find(".section"), false,true);
                    droptool();
                    return;
                }
                else if (_config_type == "Title") {
                    tr = "<tr><td style='background-color:#222222' colspan=2>Title</td></tr>";
                    $(".maincontainer").append(tr);
                    return;
                }
                else if (_config_type == "Text Box" || _config_type == "Phone No" || _config_type == "Email Id" ||
                    _config_type == "Amount" || _config_type == "Date" || _config_type == "Number") {
                    control = '<input type="text" value="' + _config_type + '">';
                }
                else if (_config_type == "Dropdown") {
                    control = "<select><option>Dropdown</option></select> ";
                }
                else if (_config_type == "Multi Line") {
                    control = "<textarea>" + _config_type + "</textarea>";
                    $(".size").val("300");
                }
                else if (_config_type == "Checkbox") {
                    control = '<input type="checkbox">';
                }
                else if (_config_type == "File Upload" || _config_type == "Image Upload") {
                    control = '<input type="file">';
                }
                $(".controltype").text(_config_type);
                $(".control").val(_config_type);
                tr = $("<tr></tr>");
                var label = $("<td class='label drop'>Label</td>");
                var ctrl = $('<td class="ctrl drop">' + control + '</td>');
                
                tr.append(label);
                tr.append(ctrl);
                tr.insertBefore(focusctrl.parent());
                openConfigure(label, ctrl,false,false);
                droptool();
            }
        });
    }
    $(".lbl").change(function() {
        var label = $(this).val().trim().toLowerCase().replace(/\W/g, "");
        var colname = label;
        if (colname.indexOf("_") < 0) {
            colname = $(".prefix").val() + colname;
        }
        $(".columnname").val(colname);
        $(".gridcolumnname").val($(this).val().trim());
        $(".isgenerate").attr("checked", true);
    });
    $(".modulename").change(function() {
        $(".tablename").val("tbl_" + $(this).val().trim().toLowerCase().replace(/\W/g, ""));
        $(".addtitle").val("Add " + $(this).val());
        $(".edittitle").val("Edit " + $(this).val());
        $(".viewtitle").val("View " + $(this).val());
    });
    $(".saveconfig").click(function() {
        if ($(".lbl").val().trim() == "") {
            alert("Please enter label");
            $(".lbl").focus();
            return;
        }
        if ($(".columnname").val().trim() == "") {
            alert("Please enter column name");
            $(".columnname").focus();
            return;
        }
        if ($(".gridcolumnname").val().trim() == "") {
            alert("Please enter grid column name");
            $(".gridcolumnname").focus();
            return;
        }
        //var sequence = parseInt(_config_txt.closest(".container").find("tr").index(_config_txt.closest("tr"))) + 1;
        //var totrows = _config_txt.closest(".container").find("tr").length;
//        if (sequence < totrows - 1) {
//            var url = "utilities.ashx?m=column_sequence&s=" + sequence + "&mid=" + $(".moduleid").val();
//            RequestData(url);
//        }
//        $(".sequence").val(sequence);
        var isleftcolumn = 1;
        if (_config_txt.closest(".right-column").length == 1) {
            isleftcolumn = 0;
        }
        $(".isleftcolumn").val(isleftcolumn);

        var id = saveData($(this));
        if (id == "-1") {
            alert("Column name already exists");
            return;
        }
        _config_txt.attr("uid", id);
        var label = $(".lbl").val();
        _config_lbl.text(label);
        if ($(".isrequired").is(":checked")) {
            _config_lbl.append("<span class='error'>*</span>");
        }
        hideConfig();
    });
    $(".deleteconfig").click(function() {
        deleteControl($(this));
        _config_txt.closest("tr").remove();
        hideConfig();
    });
    function deleteControl(obj) {
        var id = $(".columnsid").val();
        var cn = $(".columnname").val();
        var url = "del.ashx?id=" + id + "&m=" + obj.attr("m") + "&t=column&cn=" + cn;
        RequestData(url);
    }
    function hideConfig() {
        $(".popbox").hide("fast");
        $(".popbox").find(".textbox").val("");
        $(".size").val("50");
    }
    function openConfigure(labeltd, ctrltd, issubgrid,issection) {
        var txt;
        if ((issubgrid == undefined || issubgrid == false) && (issection==false)) {
            txt = findControl(ctrltd);
        }
        else {
            txt = ctrltd;
        }
        _config_txt = txt;
        _config_lbl = labeltd;
        var popbox = $(".popbox");
        popbox.css({ 'left': txt.position().left - popbox.width() / 2 + txt.width() / 2, 'top': txt.position().top + 30 });
        popbox.find(".arrow").css({ 'left': popbox.width() / 2 - 10 });
        popbox.find(".arrow-border").css({ 'left': popbox.width() / 2 - 10 });
        popbox.show("fast");
        $(".lbl").focus();
        $(".columnsid").val("0");
        //if (txt.closest(".sgrid").length > 0) {
        //sub grid columns
        //$(".submoduleid").val(txt.closest(".sgrid").attr("uid"));
        //}
        if (issubgrid) {
            $(".submoduleid").val("0");
        }
        else {
            if (txt.closest(".sgrid").length > 0) {
                //sub grid columns
                $(".submoduleid").val(txt.closest(".sgrid").attr("uid"));
            }
        }
        if(issection)
        {
            $(".subsectionid").val("0");
        }
        else
        {
            if (txt.closest(".section").length > 0) {
                //sub section columns
                $(".subsectionid").val(txt.closest(".section").attr("uid"));
            }
        }
    }

    $(".drop,.stitle").live("mouseenter", function() {
        if ($(this).attr("class").indexOf("last") == -1) {
            $(this).append("<img src='img/setting.png' class='setting hand' height=18/>");
        }
    });
    $(".drop,.stitle").live("mouseleave", function() {
        $(this).find(".setting").remove();
    });
    function findControl(ctrltd) {
        var txt = ctrltd.find("input");
        if (txt.length == 0) {
            txt = ctrltd.find("textarea");
        }
        if (txt.length == 0) {
            txt = ctrltd.find("select");
        }
        return txt;
    }
    $(".setting").live("click", function() {
        var labeltd;
        var ctrltd;
        labeltd = $(this).parent().parent().find(".label");
        ctrltd = $(this).parent().parent().find(".ctrl");
        var txt = findControl(ctrltd);
        var id = txt.attr("uid");
        var issubgrid = false;
        var issubsection = false;
        if (id == undefined) {
            if($(this).closest(".sgrid").length==0)
            {
                id = $(this).closest(".section").attr("uid");
                labeltd = $(this).closest(".section").find(".sectiontitle");
                ctrltd = labeltd;
                issubgrid = true;
            }
            else
            {
                id = $(this).closest(".sgrid").attr("uid");
                labeltd = $(this).closest(".stitle");
                ctrltd = labeltd;
                issubgrid = true;
            }
        }
        populate("columns", id);
        openConfigure(labeltd, ctrltd, issubgrid,issubsection);
        $(".columnsid").val(id);

    });
    function populate(m, id) {
        var url = "../detail.ashx?m=" + m + "&id=" + id;
        var data = (RequestData(url));
         if (data == null) return;
        for (i = 0; i < data.length; i++) {
            var col = data[i].column;
            var value = data[i].value;
            var arr = col.split("_");
            var key = arr[1];
            value = value.replace(/&quot;/g,"'");
            value = value.replace(/&dquot;/g,"\"");
            if (key.indexOf("is") == 0) {
                //for bool values
                if (value == "True" || value == "true" || value == "1") {
                    $("." + key).attr("checked", true);
                }
                else {
                    $("." + key).attr("checked", false);
                }
            }
            else {
                $("." + key).each(function() {
                    if ($(this)[0].tagName == "TD") {
                        $(this).text(value);
                    }
                    else {
                        $(this).val(value);
                    }
                });
            }
        }
       
//        $.each(data[0], function(key, value) {
//            if (key.indexOf("is") == 0) {
//                //for bool values
//                if (value == "True") {
//                    $("." + key).attr("checked", true);
//                }
//                else {
//                    $("." + key).attr("checked", false);
//                }
//            }
//            else {
//                $("." + key).each(function() {
//                    if ($(this)[0].tagName == "TD") {
//                        $(this).text(value);
//                    }
//                    else {
//                        $(this).val(value);
//                    }
//                });
//            }
//        });
    }
    $(".close-config").click(function() {
        hideConfig();
    });
    $(".more").click(function() {
        if ($(".more-options").css("display") == "none") {
            $(".more-options").show("slow");
        }
        else {
            $(".more-options").hide("fast");
        }
    });
    $(".dropdownmoduleid").change(function() {
        var url = "getddlcol.ashx?id=" + $(this).val();
        var columnName = RequestData(url);
        $(".dropdowncolumn").val(columnName);
        var idcol = "";
        if (columnName != "") {
            arr = columnName.split('_');
            idcol = $(".prefix").val() + arr[0] + "id";
        }
        $(".columnname").val(idcol);
    });
    function saveData(obj) {
        var id = 0;
        var frm = obj.closest("form");
        var URL = "addupdate.ashx?m=" + obj.attr("m")+"&puid=" + _prev_uid;
        var data = frm.serialize();
        var nonchecked = "";
        var table = $("." + obj.attr("target"));
        table.find('input[type=checkbox]').each(function() {
            if (!this.checked) {
                if (nonchecked == "") {
                    nonchecked += this.name;
                }
                else {
                    nonchecked += "," + this.name;
                }
            }
        });
        URL = URL + "&nc=" + nonchecked;
        $.ajax({
            url: URL,
            type: 'post',
            dataType: 'json',
            async: false,
            data: frm.serialize(),
            success: function(data) {
                id = data;
            }
        });
        return id;
    }
    function RequestData(URL) {
        var p;
        $.ajax({
            url: URL,
            type: 'GET',
            async: false,
            success: function(data) {
                p = data;
            },
            error: function(e) {
                alert("Error occurred while processing your request.");
            }
        });
        return p;
    }
});
function populateFromRow() {
    $(".repeater-row,.repeater-alt").click(function() {
        $(this).find("td").each(function() {
            var cn = $(this).attr("cn");
            if (cn != undefined) {
                $("." + cn).val($(this).text());
            }
        });
    });
}
function saveMultiData(obj) {
        var id = 0;
        var frm = obj.closest("form");
        var URL = "bulkadd.ashx?m=" + obj.attr("m") + "&prefix=" + obj.attr("p") + "&cn=" + obj.attr("cn");
        var data = frm.serialize();
        var nonchecked = "";
        $.ajax({
            url: URL,
            type: 'post',
            dataType: 'json',
            async: false,
            data: frm.serialize(),
            success: function(data) {
                id = data;
            }
        });
        return id;
}
