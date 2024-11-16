function setPayment(mainobj,isPayment) {
    mainobj.find("#AccountLedgerId").focus();
    setDatePicker(mainobj.find("#VoucherDate"));
    FillDropdown(mainobj);

    if (QueryString("id") != "") {
        populateData(mainobj);
        var param = mainobj.find(".grid").find(".g_param");
        setParam(param, "w", "t1.LedgerVoucherId=" + QueryString("id"));
        populate_dgrid(mainobj.find(".grid"));
        moveParentdata();
        findtotal();
        showPendingBills();
    }
    else {
        if (isPayment) {
            setAutoNumber("2", mainobj.find(".PaymentAuto"));
        }
        else {
            setAutoNumber("3", mainobj.find(".ReceiptAuto"));
        }
    }
    function moveParentdata() {
        var grid = mainobj.find(".grid");
        var first = true;
        var second = true;
        grid.find(".repeater-row,.repeater-alt").each(function() {
            var isparent = false;
            var ledgerId = "0";
            ledgerId = $(this).find(".LedgerId").val();
            if (first) {
                mainobj.find("#AccountLedgerId").val(ledgerId);
                var crAmount = $(this).find(".CrAmount").val();
                mainobj.find("#TotalCrAmount").val(crAmount);
                $(this).remove();
                first = false;
            }
            else {
                if (second) {
                    mainobj.find("#CustomerLedgerId").val(ledgerId);
                    second = false
                }
            }
        });
    }
    mainobj.find('.save').click(function() {
        setLedgerId();
        SetZeroForAmount(mainobj);
        Save(mainobj);
        return false;
    });
    function setLedgerId() {
        var ledgerid = mainobj.find("#CustomerLedgerId").val();
        mainobj.find(".grid").find(".LedgerId").val(ledgerid);
    }
    function IsBillbyBill() {
        var isbillwise = 0;
        mainobj.find("#CustomerLedgerId").find("option").each(function() {
            if ($(this).attr("selected") == "selected") {
                isbillwise = $(this).attr("isbillbybill");
            }
        });
        return isbillwise;
    }
    mainobj.find(".SplitAmount").live("keypress", function(event) {
        if (event.keyCode == 13) {
            if ($(this).val() != "") {
                var isvalid = validatePaymentDetail($(this));
                if (!isvalid) return false;

                ret = adddgrid($(this), event, mainobj.find(".grid"), findtotal);
                findtotal();
                if (IsBillbyBill() == 1) {
                }
                else {
                    mainobj.find("#Narration").focus();
                }
                return ret;
            }
            return false;
        }
    });
    function findtotal() {
        var colName = "";
        if (isPayment) {
            colName = "DrAmount";
        }
        else {
            colName = "CrAmount";
        }
        var total = parseFloat(getTotal(mainobj.find(".grid"), colName)).toFixed(2);
        mainobj.find(".lbltotal").text(total);
        mainobj.find(".total").val(total);
    }
    mainobj.find("#CustomerLedgerId").live("change", function() {
        CustmerChange();
    });
    function CustmerChange() {
        if (IsBillbyBill() == 1) {
            showPendingBills();
        }
        else {
            mainobj.find(".pending-bills").hide();
        }
        showhidecontrols();
    }
    function showhidecontrols() {
        if (IsBillbyBill() == 1) {
            mainobj.find("#RefMethod").show();
            mainobj.find("#RefNo").show();
            mainobj.find("#DueDate").show();
        }
        else {
            mainobj.find("#RefMethod").hide();
            mainobj.find("#RefNo").hide();
            mainobj.find("#DueDate").hide();
            mainobj.find(".grid").find(".repeater-row").remove();
            mainobj.find(".grid").find(".repeater-alt").remove();
            findtotal();
        }
    }

    function showPendingBills() {
        var grd = mainobj.find(".pending-bills");
        grd.show();
        var id = parseInt(mainobj.find("#CustomerLedgerId").val());
        if (id == 0) return;
        var url = "";
        if (isPayment) {
            url = "data.php?m=pending-bills&id=" + id;
        }
        else {
            url = "data.php?m=pending-receipt&id=" + id
        }
        var data = RequestData(url);
        var html = "";
        html = "<table class='pending-bill' cellspacing=0 cellpadding=3><tr><td class='pending-title' colspan=3>Pending Bills</td></tr>";
        if (data == "") {
            html += "<tr><td class='error'>No pending bills</td></tr>";
        }
        else {
            html += "<tr class='bold'><td>RefNo</td><td>Date</td><td>Amount</td></tr>";
            data = jQuery.parseJSON(data);
            for (i = 0; i < data.length; i++) {
                var cls = "";
                if (parseInt(i) % 2 == 0) {
                    cls = "class='pending-bill-alt'";
                }
                html += "<tr " + cls + "><td class='ref'>" + data[i].RefNo + "</td><td>" + convertToDate(data[i].VoucherDate) + "</td><td class='right balance'>" + parseFloat(data[i].Balance).toFixed(2) + "</td></tr>";
            }
        }
        html += "</table>";
        grd.html(html);
    }
    function validatePaymentDetail(obj) {
        var p = obj.parent().parent();
        var matching = false;
        if (p.find("#RefMethod").val() == "Against Ref") {
            var refNo = p.find("#RefNo").val();
            if (refNo != "") {
                var grd = mainobj.find(".pending-bill");

                grd.find(".ref").each(function() {
                    if (refNo == $(this).text()) {
                        matching = true;
                    }
                });
            }
            if (!matching) {
                alert("Invalid reference number");
                p.find("#RefNo").focus();
                return false;
            }
        }
        return true;
    }
    function setBillAmount() {
        if (mainobj.find("#RefMethod").val() == "Against Ref") {
            var refNo = mainobj.find("#RefNo").val();
            var grd = mainobj.find(".pending-bill");
            var matching = false;
            var balance = 0;
            grd.find(".ref").each(function() {
                if (refNo == $(this).text()) {
                    matching = true;
                    balance = $(this).parent().find(".balance").text();
                }
            });
            if (matching) {
                if (isPayment) {
                    mainobj.find("#DrAmount").val(balance);
                }
                else {
                    mainobj.find("#CrAmount").val(balance);
                }
            }
            else {
                alert("Invalid reference number");
                return false;
            }

        }
        return true;
    }
}