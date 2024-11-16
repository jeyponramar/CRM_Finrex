$(document).ready(function(e) {
    initContextMenu();
    //initFileUpload();
    function enableHtmlEditor(menu) {
        $(".heditor").attr("contenteditable", "false");
        _htmlEditor = menu.closest("td").find(".heditor");
        _htmlEditor.attr("contenteditable", "true");
    }
    $(".htmleditor").each(function(e) {
        var w = $(this).width();
        var h = $(this).height();
        var headerWidth = "100%";
        if (w < 480) {
            w = 480;
            headerWidth = "500px";
        }
        else {
            headerWidth = (w + 20) + "px";
        }
        var menuHtml = "<div class='heditor-header' style='width:" + headerWidth + ";'>" +
                            "<a href='#' class='heditor-menu-bold'><div class='heditor-menu'><b>B</b></div></a>" +
                            "<a href='#' class='heditor-menu-underline'><div class='heditor-menu'><u>U</u></div></a>" +
                            "<a href='#' class='heditor-menu-italic'><div class='heditor-menu'><i>I</i></div></a>" +
                            "<a href='#' class='heditor-menu-strike'><div class='heditor-menu'><strike>ABC</strike></div></a>" +
                            "<a href='#' class='heditor-menu-color'><div class='heditor-menu pickcolor' style='background-image:url(../images/htmleditor/forecolor.png);vertical-align:top;'><input type='text' class='hidden heditor-txtcolor'/></div></a>" +
                            "<a href='#' class='heditor-menu-bgcolor'><div class='heditor-menu pickcolor' style='background-image:url(../images/htmleditor/bgcolor.png);'><input type='text' class='hidden heditor-txtbgcolor'/></div></a>" +
                            "<a href='#' class='heditor-menu-left'><div class='heditor-menu' style='background-image:url(../images/htmleditor/align-left.png);'></div></a>" +
                            "<a href='#' class='heditor-menu-center'><div class='heditor-menu' style='background-image:url(../images/htmleditor/align-center.png);'></div></a>" +
                            "<a href='#' class='heditor-menu-right'><div class='heditor-menu' style='background-image:url(../images/htmleditor/align-right.png);'></div></a>" +
                            "<a href='#' class='heditor-menu-justify'><div class='heditor-menu' style='background-image:url(../images/htmleditor/align-justify.png);'></div></a>" +
                            "<a href='#' class='heditor-menu-list-no'><div class='heditor-menu' style='background-image:url(../images/htmleditor/list-number.png);'></div></a>" +
                            "<a href='#' class='heditor-menu-list-dot'><div class='heditor-menu' style='background-image:url(../images/htmleditor/list-dot.png);'></div></a>" +
                            "<a href='#' class='heditor-menu-fontsize'><select style='width:30px;height:20px;margin:5px;' id='heditor-font-size'><option value=''></option>" +
                                "<option value='h1'>Heading 1</option>" +
                                "<option value='h2'>Heading 2</option><option value='h3'>Heading 3</option><option value='h4'>Heading 4</option><option value='h5'>Heading 5</option>" +
                            "</select></a>" +
                            "<a href='#' class='heditor-menu-link'><div class='heditor-menu' style='background-image:url(../images/htmleditor/link.png);'></div></a>" +
                            "<a href='#' class='heditor-menu-hr'><div class='heditor-menu' style='background-image:url(../images/htmleditor/hr.png);'></div></a>" +
                            //"<a href='#' class='heditor-menu-table'><div class='heditor-menu' style='background-image:url(../images/htmleditor/table.png);'></div></a>" +
                            "<a href='#' class='heditor-menu-imgupload'>" +
                                    "<div class='imgfilediv heditor-menu' style='margin-left:5px;background-image:url(../images/htmleditor/img-upload.png);background-position:5px 5px;'heditor-imgupload='true' folder='upload/htmleditor' isguid='true' title='Click here to upload image'>" +
                                        "<input type='file' name='file'>" +
                                    "</div>" +
                            "</a>" +
                            "<a href='#' class='heditor-menu-html'><div class='heditor-menu' style='font-size:10px;padding-top:8px;height:15px;width:25px;'>HTML</div></a>" +
                        "</div>";
        var editorHtml = "<div class='heditor' id='heditor' contenteditable='true' unselectable='off' " +
                         "style='overflow:scroll;width:" + w + "px;height:" + h + "px;'>" + $(this).val() + "</div>";
        $(this).hide();
        if ($(this).width() < w) {
            $(this).css("width", headerWidth);
        }
        $(menuHtml).insertBefore($(this));
        var editor = $(editorHtml);
        editor.insertAfter($(this));


    });

    $("form").submit(function(e) {
        $(".htmleditor").each(function(e) {
            $(this).val($(this).parent().find(".heditor").html());
        });
    });
    $(".heditor").mousedown(function(e) {
    })
    $(".heditor").mouseout(function(e) {
        _heditorSelectedRange = saveSelection();
    });
    $(".heditor-menu-bold").click(function(e) {
        enableHtmlEditor($(this));
        document.execCommand('bold', false, null);
        $(this).closest("td").find(".heditor").focus();
    });
    $(".heditor-menu-underline").click(function(e) {
        enableHtmlEditor($(this));
        document.execCommand('underline', false, null);
        $(this).closest("td").find(".heditor").focus();
    });
    $(".heditor-menu-italic").click(function(e) {
        enableHtmlEditor($(this));
        document.execCommand('italic', false, null);
        $(this).closest("td").find(".heditor").focus();
    });
    //    $(".heditor-menu-strike").click(function(e) {
    //        enableHtmlEditor($(this));
    //        alert(document.getSelection());
    //        document.execCommand('insertHTML', false, "<span><strike>" + document.getSelection() + "</strike></span>");
    //        $(this).closest("td").find(".heditor").focus();
    //    });
    $(".heditor-menu-center").click(function(e) {
        enableHtmlEditor($(this));
        document.execCommand('justifyCenter', false, null);
        $(this).closest("td").find(".heditor").focus();
    });
    $(".heditor-menu-left").click(function(e) {
        enableHtmlEditor($(this));
        document.execCommand('justifyLeft', false, null);
        $(this).closest("td").find(".heditor").focus();
    });
    $(".heditor-menu-right").click(function(e) {
        enableHtmlEditor($(this));
        document.execCommand('justifyRight', false, null);
        $(this).closest("td").find(".heditor").focus();
    });
    $(".heditor-menu-justify").click(function(e) {
        enableHtmlEditor($(this));
        document.execCommand('justifyFull', false, null);
        $(this).closest("td").find(".heditor").focus();
    });
    $(".heditor-menu-list-no").click(function(e) {
        enableHtmlEditor($(this));
        document.execCommand('insertOrderedList', false, null);
        $(this).closest("td").find(".heditor").focus();
    });
    $(".heditor-menu-list-dot").click(function(e) {
        enableHtmlEditor($(this));
        document.execCommand('insertUnorderedList', false, null);
        $(this).closest("td").find(".heditor").focus();
    });
    $(".heditor-menu-color").click(function(e) {
        enableHtmlEditor($(this));
        _heditorEnabled = true;
        _heditorForeColor = true;
    });
    $(".heditor-menu-bgcolor").click(function(e) {
        $(".heditor").attr("contenteditable", "false");
        $(this).closest("td").find(".heditor").attr("contenteditable", "true");
        _heditorEnabled = true;
        _heditorBgColor = true;
    });
    $(".heditor-menu-table").click(function(e) {
        enableHtmlEditor($(this));
        _heditorSelectedRange = saveSelection();
        newtable();
        //        var rows = 3;
        //        cols = 5;
        //        var html = "<table width='95%' border=1 cellspacing=0>";
        //        for (i = 0; i < rows; i++) {
        //            html += "<tr>";
        //            for (j = 0; j < cols; j++) {
        //                html += "<td>&nbsp;</td>";
        //            }
        //            html += "</tr>";
        //        }
        //        $(this).closest("td").find(".heditor").focus();
        //        document.execCommand('insertHTML', true, html);
        //        $(".heditor").find("table").colResizable();
        //        $(this).closest("td").find(".heditor").focus();
    });
    $(".heditor-menu-link").click(function(e) {
        enableHtmlEditor($(this));
        var linkURL = prompt('Enter a URL:', 'http://');
        document.execCommand('insertHTML', false, '<a href="' + linkURL + '" target="_blank">' + document.getSelection() + '</a>');
        $(this).closest("td").find(".heditor").focus();
    });
    $(".heditor-menu-hr").click(function(e) {
        enableHtmlEditor($(this));
        document.execCommand('insertHTML', false, '<hr/>');
        $(this).closest("td").find(".heditor").focus();
    });
    $(".heditor-menu-html").click(function(e) {
        enableHtmlEditor($(this));
        var heditor = $(this).closest("td").find(".heditor");
        var txteditor = $(this).closest("td").find(".htmleditor");
        if (txteditor.css("display") == "inline-block") {
            heditor.html(txteditor.val());
            heditor.show();
            txteditor.hide(); //textbox
            heditor.css("width", txteditor.width());
            heditor.css("height", txteditor.height());
            //heditor.find("table").colResizable();
        }
        else {
            //heditor.find("table").colResizable("destroy");
            heditor.hide();
            txteditor.val(heditor.html());
            txteditor.show();
        }
    });
    $(".heditor-menu-imgupload").click(function(e) {
        enableHtmlEditor($(this));
    });
    $("#heditor-font-size").change(function(e) {
        document.execCommand('formatBlock', false, "<" + $(this).val() + ">");
    });
    $(".heditor-menu-imgupload").click(function(e) {
        initImgUpload();
    });
    $(".heditor").bind("paste", function(e) {
        var pastedData = e.originalEvent.clipboardData.getData('text');
        var arr = pastedData.split('\n');
        var html = "<div class='tbl-editor-div'><table style='border-collapse:collapse;' width='100%' cellpadding='5' cellspacing='0' border='1' bordercolor='#000'>";
        var rows = arr.length;
        var cols = 0;
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] != "") {
                var arr2 = arr[i].split('\t');
                cols = arr2.length;
                if (i == 0) {
                    html += "<tr class='tbl-edittor-header'>";
                }
                else {
                    html += "<tr>";
                }
                for (var j = 0; j < arr2.length; j++) {
                    html += "<td>" + arr2[j] + "</td>";
                }
                html += "</tr>";
            }
        }
        html += "</table></div>";
        if (rows > 2 && cols > 2) {
            document.execCommand('insertHTML', true, html);
            $(this).focus();
            e.preventDefault();
        }
    });
    function saveTableProperties() {
        var rows = ConvertToInt($("#heditor-tbl-rows").val());
        var cols = ConvertToInt($("#heditor-tbl-cols").val());
        var cellpad = ConvertToInt($("#heditor-tbl-cellpad").val());
        var cellspace = ConvertToInt($("#heditor-tbl-cellspace").val());
        var w = $("#heditor-tbl-width").val();
        var h = $("#heditor-tbl-height").val();
        if (rows == 0) rows = 5;
        if (cols == 0) cols = 3;
        var width = ""; var height = "";
        if (w == "") w = '100%';
        width = " width='" + w + "'";
        if (h != '') height = " height='" + h + "'";
        var html = "<table" + width + height + " cellspacing='" + cellspace + "' cellpadding='" + cellpad + "' border=1>";
        var islabelvalue = $("#heditor-tbl-islabelvalue").prop("checked");
        for (i = 0; i < rows; i++) {
            html += "<tr>";
            if (islabelvalue) {
                html += "<td class='heditor-label'>Label</td><td class='heditor-lv-sep'>:</td>";
                html += "<td class='heditor-val'>Value</td>";
            }
            else {
                for (j = 0; j < cols; j++) {
                    html += "<td>&nbsp;</td>";
                }
            }
            html += "</tr>";
        }
        html += "</table>";
        $("#heditor-tbl").dialog("close");
        _htmlEditor.focus();
        var newtbl = $(html);
        _htmlEditor.append(newtbl);
        if ($("#heditor-tbl-isborder").prop("checked")) {
            newtbl.find("td").css("border", "1");
        }
        else {
            newtbl.find("td").css("border", "dotted 1px #888888");
        }
        //newtbl.colResizable();
    }
    $("#heditor-tbl-btnsave").live("click", function(e) {
        saveTableProperties();
    });
    function initFileUpload() {
        $('#heditor-imgupload-file').fileupload({
            dataType: 'json',
            add: function(e, data) {
                data.context = $('<button/>').text('Upload')
                .appendTo(document.body)
                .click(function() {
                    data.context = $('<p/>').text('Uploading...').replaceAll($(this));
                    data.submit();
                });
            },
            done: function(e, data) {
                data.context.text('Upload finished.');
            }
        });
    }
    $("#heditor-imgupload-btnsave").live("click", function() {
        //        $('#heditor-imgupload-file').fileupload({
        //            url: "../bulkfileupload.ashx",
        //            done: function(e, data) {
        //                alert(1)
        //            }
        //        });
        //        $.ajax({
        //        url: "../bulkfileupload.ashx",
        //        type: "post",
        //        data: { file: $("#heditor-imgupload-file").val() },
        //        success: function(text) {
        //            if(text == "success") {
        //                alert("Your image was uploaded successfully");
        //            }
        //        },
        //        error: function() {
        //            alert("An error occured, please try again.");         
        //        }
        //    });
        alert($('#heditor-imgupload-file'));
        $('#heditor-imgupload-file').fileupload({
            url: '../bulkfileupload.ashx',
            autoUpload: true,
            success: function(msg) {
                alert(msg);

            }
        });
    });
    $(".heditor").mousedown(function(e) {
        //loadContextMenu($(this));
    });

    function newtable() {
        inittable();
    }
    function loadContextMenu(heditor) {
        heditor.find("table").find("td").contextMenu({
            menu: 'heditor-cmenu-tableprop'
        },
	    function(action, el, pos) {
	        if (action == "insertrowabove") {
	            addNewTableRow(el, true);
	        }
	        else if (action == "insertrowbelow") {
	            addNewTableRow(el, false);
	        }
	        else if (action == "delrow") {
	            el.closest("tr").remove();
	        }
	        else if (action == "insertcolumnbefore") {
	            addNewTableCol(el, true);
	        }
	        else if (action == "insertcolumnafter") {
	            addNewTableCol(el, false);
	        }
	        else if (action == "delcol") {
	            DeleteTableCol(el);
	        }
	        else if (action == "heditor-cell-border-all") {
	            el.css("border", "solid 1px #000");
	        }
	        else if (action == "heditor-cell-border-no") {
	            el.css("border", "dotted 1px #888888");
	        }
	        else if (action == "heditor-cell-border-left") {
	            el.css("border-left", "solid 1px #000");
	            el.css("border-top", "dotted 1px #888888");
	            el.css("border-right", "dotted 1px #888888");
	            el.css("border-bottom", "dotted 1px #888888");
	        }
	        else if (action == "heditor-cell-border-top") {
	            el.css("border-left", "dotted 1px #888888");
	            el.css("border-top", "solid 1px #000");
	            el.css("border-right", "dotted 1px #888888");
	            el.css("border-bottom", "dotted 1px #888888");
	        }
	        else if (action == "heditor-cell-border-right") {
	            el.css("border-left", "dotted 1px #888888");
	            el.css("border-top", "dotted 1px #888888");
	            el.css("border-right", "solid 1px #000");
	            el.css("border-bottom", "dotted 1px #888888");
	        }
	        else if (action == "heditor-cell-border-bottom") {
	            el.css("border-left", "dotted 1px #888888");
	            el.css("border-top", "dotted 1px #888888");
	            el.css("border-right", "dotted 1px #888888");
	            el.css("border-bottom", "solid 1px #000");
	        }
	        $("#heditor-tbl-btnsave").live("click", function(e) {
	            saveTableProperties();
	        });
	    });
    }
    function addNewTableCol(td, isbefore) {
        var index = 0;
        var tdindex = 0;
        var targettd;

        td.closest("tr").find("td").each(function(e) {
            if ($(this).is(td)) {
                tdindex = index;
            }
            index++;
        });

        td.closest("table").find("tr").each(function(e) {
            index = 0;
            $(this).find("td").each(function(e) {
                if (index == tdindex) {
                    targettd = $(this);
                }
                index++;
            });
            var newtd = "<td style='" + targettd.css("style") + "'>1</td>";
            if (isbefore) {
                targettd.before(newtd);
            }
            else {
                targettd.after(newtd);
            }
        });

    }
    function DeleteTableCol(td) {
        var index = 0;
        var tdindex = 0;
        var targettd;

        td.closest("tr").find("td").each(function(e) {
            if ($(this).is(td)) {
                tdindex = index;
            }
            index++;
        });

        td.closest("table").find("tr").each(function(e) {
            index = 0;
            $(this).find("td").each(function(e) {
                if (index == tdindex) {
                    targettd = $(this);
                }
                index++;
            });
            targettd.remove();
        });
    }

    function addNewTableRow(td, isabove) {
        var tr = td.closest("tr");
        //find max no of cells
        var maxcells = 0;
        tr.closest("table").find("tr").each(function(e) {
            if ($(this).find("td").length > maxcells) maxcells = $(this).find("td").length;
        });
        var html = "<tr>";
        for (i = 0; i < maxcells; i++) {
            html += "<td style='" + td.attr("style") + "'>&nbsp;</td>";
        }
        html += "</tr>";
        if (isabove) {
            td.closest("tr").before(html);
        }
        else {
            td.closest("tr").after(html);
        }
    }
    function initImgUpload() {
        //        if ($('#heditor-imgupload').length == 0) {
        //            var html = "<div id='heditor-imgupload' title='Image Upload'>" +
        //                "<table cellspacing='5'>" +
        //                    "<tr>" +
        //                        "<td width='100'>Select Image</td><td><input type='file' id='heditor-imgupload-file'/></td>" +
        //                    "</tr>" +
        //                    "<tr><td>&nbsp;</td></tr>" +
        //                    "<tr class='heditor-imgupload-progress hidden'><td colspan='4'><img src='../images/ajax-loader-fb.gif'/></td></tr>" +
        //                    "<tr><td colspan='4' align='center'><input type='button' id='heditor-imgupload-btnsave' value='Save' class='button'/></td></tr>" +
        //                "</table>" +
        //            "</div>";
        //            $(html).appendTo('body');
        //        }
        $("#heditor-imgupload").dialog({
            modal: true,
            width: 500,
            height: 200
        });

    }
    function initContextMenu() {
        //table properties
        var html = '<ul id="heditor-cmenu-tableprop" class="contextMenu">' +
			    '<li><a href="#edit">Edit</a></li>' +
			    '<li class="separator"><a href="#insertrowbelow">Insert Row Below</a></li>' +
			    '<li><a href="#insertrowabove">Insert Row Above</a></li>' +
			    '<li class="del"><a href="#delrow">Delete Row</a></li>' +
			    '<li class="separator"><a href="#insertcolumnbefore">Insert Column Before</a></li>' +
			    '<li><a href="#insertcolumnafter">Insert Column After</a></li>' +
			    '<li class="del"><a href="#delcol">Delete Column</a></li>' +
			    '<li class="seperaror"><a href="#">Cell Properties</a>' +
			        '<ul><li class="heditor-cell-border-all"><a href="#heditor-cell-border-all">All Borders</a></li>' +
			        '<li class="heditor-cell-border-left"><a href="#heditor-cell-border-left">Left Border</a></li>' +
			        '<li class="heditor-cell-border-right"><a href="#heditor-cell-border-right">Right Border</a></li>' +
			        '<li class="heditor-cell-border-top"><a href="#heditor-cell-border-top">Top Border</a></li>' +
			        '<li class="heditor-cell-border-bottom"><a href="#heditor-cell-border-bottom">Bottom Border</a></li>' +
			        '<li class="heditor-cell-border-no"><a href="#heditor-cell-border-no">No Border</a></li>' +
			        '</ul></li>' +
			    '</ul>';
        $(html).appendTo('body');
    }
    function inittable() {
        if ($('#heditor-tbl').length == 0) {
            var html = "<div id='heditor-tbl' title='Table Properties'>" +
                "<table cellspacing='5'>" +
                    "<tr>" +
                        "<td width='100'>Rows</td><td><input type='text' id='heditor-tbl-rows' class='val-i sbox'/></td>" +
                        "<td width='100'>Columns</td><td><input type='text' id='heditor-tbl-cols' class='val-i sbox'/></td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td>Width</td><td><input type='text' id='heditor-tbl-width' class='val-i sbox'/></td>" +
                        "<td>Height</td><td><input type='text' id='heditor-tbl-height' class='val-i sbox'/></td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td>Cell Padding</td><td><input type='text' id='heditor-tbl-cellpad' class='val-i sbox'/></td>" +
                        "<td>Cell Spacing</td><td><input type='text' id='heditor-tbl-cellspace' class='val-i sbox'/></td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td>Border</td><td><input type='checkbox' id='heditor-tbl-isborder'/></td>" +
                        "<td>Label/Value</td><td><input type='checkbox' id='heditor-tbl-islabelvalue'/></td>" +
                    "</tr>" +
                    "<tr><td colspan='4' align='center'><input type='button' id='heditor-tbl-btnsave' value='Save' class='button'/></td></tr>" +
                "</table>" +
            "</div>";
            $(html).appendTo('body');
        }
        $("#heditor-tbl").dialog({
            modal: true,
            width: 500
        });
    }

});
function saveSelection() {
    if (window.getSelection) {
        sel = window.getSelection();
        try {
            return sel.getRangeAt(0);
        } catch (e) { return null; }
    } else if (document.selection && document.selection.createRange) {
        return document.selection.createRange();
    }
}

function restoreSelection(range) {
    if (range) {
        if (window.getSelection) {
            sel = window.getSelection();
            sel.removeAllRanges();
            sel.addRange(range);
        } else if (document.selection && range.select) {
            range.select();
        }
    }
}