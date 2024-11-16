var _txtba = null;var _parentRow = null;
$(document).ready(function() {
    initDetailModule();
    adjustDatePickerAlways();
    initMoreSearch();
    populateFim();
    $("#gridpanel").css("width", $(window).width());
    $("#gridpanel").css("height", $(window).height());
    $("#dtcommon").datepicker({
        dateFormat: 'dd-mm-yy',
        onSelect: function(dateText, inst) {
            _txtba.val(dateText);
            _txtba.closest("tr").attr("changed", "true");
            _txtba.focus();
            $("#dtcommon").hide();
        }
    });
    $(".tdlnkadvsearch").click(function() {
        $(".tradvsearch").show();
    });
    $(".ba-btnaddnew").click(function() {
        var tr = $(".bagrid-template").clone();
        tr.removeClass("bagrid-template");
        tr.insertAfter($(".bagrid-header"));
        tr.show();
        tr.removeClass("bagrid-row").addClass("bagrid-row");
        tr.find("td:first").find(".txtba").focus();
        tr.find(".ac").each(function() {
            setAutoComplete($(this));
        });
    });
    $("#btnbagrid_save").click(function() {
        var ctrow = _txtba.closest("tr");
        var isvalid = isValidRow(ctrow);
        if (!isvalid) return false;
        saveba(ctrow, false);
    });
    $(".bagrid-footer").click(function() {
        var tr = $(".bagrid-template");
        tr.insertBefore($(this));
        tr.show();
        tr.removeClass("bagrid-row").addClass("bagrid-row");
        tr.find("td:first").find(".txtba").focus();
    });
    $(".bagrid-row").find("td").live("click", function() {
        $(this).find(".txtba").show();
        $(this).find(".hidden").hide();
        if ($(this).find(".lblba").length > 0) {
            $(this).closest("tr").find(".txtba:first").focus();
        }
    });
    function isValidRow(ctrow) {
        var isvalid = true;
        ctrow.find(".txtba").each(function() {
            if (isvalid) {
                if ($(this).attr("ir") != undefined) {
                    if ($(this).val().trim() == "") {
                        isvalid = false;
                        $(this).focus();
                        return false;
                    }
                }
            }
        });
        return isvalid;
    }
    $(".txtba").live("keypress", function(e) {
        if (e.keyCode != 13) return;
        var index = $(this).parent().index();
        var ctrow = $(this).closest("tr");
        var isvalid = isValidRow(ctrow);
        if (!isvalid) return false;

        if (ctrow.attr("changed") == "true") {
            saveba(ctrow, false);
        }
        else {
            if (ctrow.next().length > 0) {
                ctrow.next().find("td:eq(" + index + ")").find(".txtba").focus();
            }
        }
        ctrow.attr("changed", "false");
        return false;

    });
    $(".txtba").live("keydown", function(e) {
        var index = $(this).parent().index();
        var ctrow = $(this).closest("tr");
        if (e.keyCode == 36)//HOME
        {
            ctrow.find(".txtba:first").focus();
        }
        if (e.keyCode == 35)//END
        {
            ctrow.find(".txtba:last").focus();
        }
        if (e.keyCode == 38) {//UP
            if (ctrow.prev().attr("class").indexOf("bagrid-header") < 0) {//last row
                ctrow.prev().find("td:eq(" + index + ")").find(".txtba").focus();
            }
        }
        else if (e.keyCode == 40) {//UP
            if (ctrow.next().attr("class").indexOf("bagrid-footer") < 0) {//last row
                ctrow.next().find("td:eq(" + index + ")").find(".txtba").focus();
            }
        }
        else if (e.keyCode == 13) {//ENTER
        }
        else {
            ctrow.attr("changed", "true");
        }
    });

    $(".txtba").live("focus", function() {
        $(this).closest("td").css("border", "solid 2px #0867B1");
        $("#dtcommon").hide();
        _txtba = $(this);
        if ($(this).attr("class").indexOf("val-dt") >= 0) {
            $("#dtcommon").css("left", $(this).position().left - 10);
            $("#dtcommon").css("top", $(this).position().top + 30);
            var dt = $(this).val();
            if (dt != "") {
                var arr = dt.split('-');
                if (arr.length == 3) {
                    var m = ConvertToInt(arr[1]) - 1;
                    var cdt = new Date(arr[2], m, arr[0]);
                    $("#dtcommon").datepicker("setDate", cdt);
                }
            }
            $("#dtcommon").show();
        }
    });

    $(".txtba").live("blur", function() {
        $(this).closest("td").css("border", "solid 1px #aaa");
        $(this).closest("td").css("border-top", "0px");
        $(this).closest("td").css("border-left", "0px");
    });
    $(".ac").live("blur", function() {
        if ($(this).attr("ir") == undefined) return;
        if ($(this).parent().find(".hdnac").val() == "") {
            $(this).focus();
            return false;
        }
    });
    $(".ba-delete").live("click", function() {
        if (confirm("Are you sure you want to delete this data?")) {
            var m = $(this).closest(".bagrid").attr("m");
            var tr = $(this).closest("tr");
            var id = ConvertToInt(tr.find(".hdnbaid").val());
            if (id == 0) {
                tr.remove();
                alert("Data deleted successfully!");
            }
            else {
                var pid = $(".bagrid").attr("pid");
                var pm = $(".bagrid").attr("pm");
                var URL = "bagridexportexposure.ashx?m=" + m + "&id=" + id + "&a=d&pid=" + pid + "&pm=" + pm;
                $.ajax({
                    isAsync: false,
                    url: URL,
                    success: function(response) {
                        if (response == "Session Expired") {
                            alert("Your session has expired, please login now!");
                            window.location = "../customerlogin.aspx";
                        }
                        else if (response.indexOf("Error") == 0) {
                            alert("Error occurred!");
                        }
                        else {
                            tr.remove();
                            var pm = $(".bagrid").attr("pm");
                            if (response == "Ok") {
                            }
                            else {
                                var parentDetail = jQuery.parseJSON(response);
                                var trParent = parent._parentRow;

                                trParent.find("label").each(function() {
                                    var columnName = pm + $(this).attr("id").replace("lblba", "");
                                    setFieldData($(this), parentDetail[columnName]);
                                });
                            }
                            //summary detail
                            bindSummaryDetail();
                            alert("Data deleted successfully!");
                        }
                    }
                });
            }
        }
    });
});
function exportorder_calc() {
    $(".txtba").live("blur", function(e) {
        var tr = $(this).closest("tr");
        var exportorderamount = ConvertToDouble(tr.find("#txtba_exportorderamount-dbl").val());
        var amountreceived = ConvertToDouble(tr.find("#txtba_amountreceived-dbl").val());
        var costing = ConvertToDouble(tr.find("#txtba_costing-dbl").val());
        var forwardbookingamount = ConvertToDouble(tr.find("#txtba_forwardbookingamount-dbl").val());
        var forwardbookingrate = ConvertToDouble(tr.find("#txtba_forwardbookingrate-dbl").val());
        var pcfcamount = ConvertToDouble(tr.find("#txtba_pcfcamount-dbl").val());
        var pcfcrate = ConvertToDouble($("#txtba_pcfcrate-dbl").val());

        var netamount = 0;
        if (amountreceived <= exportorderamount) {
            netamount = exportorderamount - amountreceived;
        }
        tr.find("#txtba_netamount-dbl").val(netamount);
        var value = netamount * costing;
        value = value.toFixed(2);
        tr.find("#txtba_value-dbl").val(value);

        var unhedgedamount = netamount - forwardbookingamount - pcfcamount;
        unhedgedamount = unhedgedamount.toFixed(2);
        tr.find("#txtba_unhedgedamount-dbl").val(unhedgedamount);

        var effectiverate = (forwardbookingamount * forwardbookingrate + pcfcamount * pcfcrate) / (forwardbookingamount + pcfcamount);
        effectiverate = effectiverate.toFixed(2);
        tr.find("#txtba_effectiverate-dbl").val(effectiverate);

    });

}
function forwardcontract_calc() {
    $(".txtba").live("blur", function(e) {
        var tr = $(this).closest("tr");
        var sold = ConvertToDouble(tr.find("#txtba_sold-dbl").val());
        var utilized = ConvertToDouble(tr.find("#txtba_utilised-dbl").val());
        var balancesold = 0;
        if (utilized <= sold) {
            balancesold = sold - utilized;
        }
        balancesold = balancesold.toFixed(2);
        tr.find("#txtba_balancesold-dbl").val(balancesold);
        var rate = ConvertToDouble(tr.find("#txtba_rate-dbl").val());
        var soldamountinRs = balancesold * rate;
        soldamountinRs = soldamountinRs.toFixed(2);
        tr.find("#txtba_soldamountinrs-dbl").val(soldamountinRs);

        var mtmrate = 0;
        
    });

}
function pcfc_calc() {
    $(".txtba").live("blur", function(e) {
        var tr = $(this).closest("tr");
        var fcamount = ConvertToDouble(tr.find("#txtba_fcamount-dbl").val());
        var repayment = ConvertToDouble(tr.find("#txtba_repayment-dbl").val());
        var fcamountbalance = fcamount - repayment;
        fcamountbalance = fcamountbalance.toFixed(2);
        tr.find("#txtba_fcamountbalance-dbl").val(fcamountbalance);
        var repayment = ConvertToDouble(tr.find("#txtba_repayment-dbl").val());
        var spotrate = ConvertToDouble(tr.find("#txtba_sellingspotrate-dbl").val());
        var product = spotrate * fcamountbalance;
        product = product.toFixed(2);
        tr.find("#txtba_product-dbl").val(product);

    });
}
function saveba(tr, isLastRow) {
    $(".txtba").each(function() {
        $(this).removeAttr("name");
    });
    $(".hdnbaid").removeAttr("name");
    var txthdn = tr.find(".hdnbaid");
    txthdn.attr("name", txthdn.attr("id"));
    
    tr.find(".txtba").each(function() {
        $(this).attr("name", $(this).attr("id"));
    });
    var m = $(".bagrid").attr("m");
    var pm = $(".bagrid").attr("pm");
    var pid = $(".bagrid").attr("pid");
    var URL = "bagridexportexposure.ashx?m=" + m + "&id=" + tr.find(".hdnbaid").val()+"&pid="+pid+"&pm="+pm;
    $.ajax({
        type: "POST",
        url: URL,
        isAsync: false,
        data: $("form").serialize(),
        success: function(response) {
            $("#dtcommon").hide();
            if (response == "Session Expired") {
                alert("Your session has expired, please login now!");
                window.location = "../customerlogin.aspx";
            }
            var divmsg = null;
            if ($(".bagrid-msg").length == 0) {
                $("body").append("<div class='bagrid-msg'></div>");
            }
            divmsg = $(".bagrid-msg");
            if (response.indexOf("Error :") == 0) {
                divmsg.removeClass("bagrid-msg-success").removeClass("bagrid-msg-err").addClass("bagrid-msg-err");
                divmsg.text(response);
            }
            else if (response.indexOf("Error") == 0 || response == "-1" || response == "0") {
                //divmsg.attr("background-color", "#ff0000");
                if (response == "-1") {
                    divmsg.text("Duplicate entry not allowed!");
                    alert("Duplicate entry not allowed!");
                }
                else {
                    divmsg.text("Error occurred!");
                    alert("Error occurred!");
                }
                divmsg.removeClass("bagrid-msg-success").removeClass("bagrid-msg-err").addClass("bagrid-msg-err");
            }
            else {
                var data = jQuery.parseJSON(response);
                var newid = data["id"];
                tr.find(".hdnbaid").val(newid);
                tr.find("label").each(function() {
                    var columnName = m + $(this).attr("id").replace("lblba", "");
                    setFieldData($(this), data[columnName]);
                });
                //update parent order detail
                var parentDetail = data["ParentDetail"];
                if (parentDetail != undefined) {
                    parentDetail = parentDetail.replace(/&quot;/g, '"');
                    parentDetail = jQuery.parseJSON(parentDetail);
                    var trParent = parent._parentRow;
                    var pm = $(".bagrid").attr("pm");
                    trParent.find("label").each(function() {
                        var columnName = pm + $(this).attr("id").replace("lblba", "");
                        setFieldData($(this), parentDetail[columnName]);
                    });
                }
                //summary detail
                bindSummaryDetail();

                divmsg.removeClass("bagrid-msg-success").removeClass("bagrid-msg-err").addClass("bagrid-msg-success");
                divmsg.text("Data saved successfully");
                alert("Data saved successfully!");
                if (isLastRow) {
                    var trnew = $("<tr class='bagrid-row'>" + $(".bagrid-template").html() + "</tr>");
                    trnew.insertAfter(tr);
                    trnew.find(".txtba:first").focus();
                    trnew.find(".ac").each(function() {
                        setAutoComplete($(this));
                    });
                }
            }
            divmsg.css("left", $(window).width() / 2 - divmsg.width() / 2);
            divmsg.show();
            setTimeout(function() { $(".bagrid-msg").hide(); }, 4000);
        }
    });
}
function adjustDatePickerAlways() {
    setTimeout(function() {
        adjustDatePickerAlways();
    }, 500);
    if ($("#dtcommon").css("display") == "none") return;
    $("#dtcommon").css("left", _txtba.position().left - 10);
    $("#dtcommon").css("top", _txtba.position().top + 30);

}
function initDetailModule() {
    $(".btnfem-submodule").live("click", function(e) {
        if ($(this).closest(".bagrid-row").find(".hdnbaid").val() == 0) {
            alert("Please save the entry first!");
            return false;
        }
        showDetailModal($(this), $(this).attr("sm"), $(this).attr("lbl") + " Detail");
        return false;
    });
    $(".detailmodal-close").click(function() {
        $(".detailmodal").hide();
        $(".fade").hide();
        bindSummaryDetail();
        $("#btnbagrid_save").show();
    });
}
function showDetailModal(obj, m, title) {
    $("#btnbagrid_save").hide();
    _parentRow = obj.closest(".bagrid-row");
    var parentId = obj.closest(".bagrid-row").find(".hdnbaid").val();
    var url = "bagridview.aspx?issubmodule=true&m="+m+"&pid="+parentId+"&pm="+$(".bagrid").attr("m");
    var modal = $(".detailmodal");
    var ifr = $("#ifrdetail");
    $(".detailmodal-title").text(title);
    $(".fade").show();
    modal.show();
    modal.attr("width", $(window).width() * 0.9);
    modal.attr("height", 400);
    ifr.attr("src", url);
    ifr.attr("width", $(window).width() * 0.9);
    ifr.attr("height", 400);
}
function bindSummaryDetail() {
    var m = $(".bagrid").attr("m");
    var pid = $(".bagrid").attr("pid");
    var pm = $(".bagrid").attr("pm");
    var ew = $(".txtwhere").val();
    var URL = "bagridexportexposure.ashx?a=sum&m=" + m + "&pid=" + pid + "&pm=" + pm + "&ew=" + ew;
    $.ajax({
        url: URL,
        success: function(response) {
            bindSumHtml(jQuery.parseJSON(response));
        }
    });
}
function bindSumHtml(json) {
    $(".bagrid-footer").find("td:first").text("Total");
    $(".bagrid-footer").find("td").each(function() {
        var cn = $(this).attr("cn");
        var obj = $(this);
        $.each(json, function(key, val) {
            if (key == cn) {
                obj.text(val);
            }
        });
    });
}
function setFieldData(obj, data) {
    if (obj.hasClass("nonzero")) {
        if (ConvertToDouble(data) == 0) {
            obj.text("-");
        }
        else {
            obj.text(data);
        }
    }
    else {
        obj.text(data);
    }
}
function initMoreSearch() {
    $(".jq-lnkmore").click(function() {
        if ($(this).text() == "More") {
            $("#trmoresearch").show();
            $(this).text("Less");
        }
        else {
            $("#trmoresearch").hide();
            $(this).text("More");
        }
    });
    var ismoresearch = false;
    $("#trmoresearch").find("input").each(function() {
        if ($(this).val().trim() == "" || $(this).val().trim() == "0") {
        }
        else {
            ismoresearch = true;
        }
    });
    if (ismoresearch) {
        $(".jq-lnkmore").text("Less");
        $("#trmoresearch").show();
    }
    else {
        $(".jq-lnkmore").text("More");
    }
}
function populateFim() {
    $("#ba_fimimportorderid").live("blur", function() {
        var tr = $(this).closest("tr");
        var m = $(this).closest(".bagrid").attr("m");
        var hdnid = $(this).closest("td").find(".hdnac").val();
        if (hdnid == "0") return;
        var URL = "bagridexportexposure.ashx?a=pop&m=" + m + "&id=" + hdnid + "&popm=fimimportorder&";
        $.ajax({
            type: "POST",
            url: URL,
            isAsync: false,
            success: function(response) {
                var tradecredit = 0;
                if (response == "") {
                    return;
                }
                var json = jQuery.parseJSON(response);
                tradecredit = json.fimimportorder_tradecredit;
                tr.find("#lblba_tradecreditamount").text(tradecredit);
            }
        });
    });
}