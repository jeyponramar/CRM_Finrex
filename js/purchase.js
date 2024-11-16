function initPurchaseSales(mainobj, m, IsBreakupCr, VoucherType) {
    mainobj.find("#RefNo").focus();
    FillDropdown(mainobj);
    addTaxItems(mainobj.find("#ProductId"));

    if (QueryString("id") == "") {
        setDatePicker(mainobj.find("#VoucherDate"));
        setAutoNumber(VoucherType, mainobj.find(".AutoNumber"));
        if (QueryString("amcid") != "") {
            convertAmcToSale();
            new dgrid(
                {
                    gridtable: mainobj.find(".grid"),
                    remove: findtotal,
                    onedit: onedit
                }
             ).edit();
            populateClientDetail();
            findtotal();
        }
    }
    else {
        populateData(mainobj);
        var param = mainobj.find("#tblBreakupDetail").find(".g_param")
        setParam(param, "w", "t1.LedgerVoucherId=" + QueryString("id"));
        populate_dgrid(mainobj.find(".grid"), findtotal, onedit);
        moveParentdata();
        findtotal();
    }
    function moveParentdata() {
        var grid = mainobj.find(".grid");
        var first = true;
        grid.find("tr").each(function() {
            var obj = $(this).find(".IsParent");
            var isparent = false;
            var ledgerId = "0";
            if (obj != undefined) {
                if (obj.val() != undefined) {

                    if (obj.val() == "1") {
                        isparent = true;
                        
                        ledgerId = $(this).find(".LedgerId").val();
                        if (first) {
                            first = false;
                            var refNo = $(this).find(".RefNo").val();
                            mainobj.find("#RefNo").val(refNo);

                            mainobj.find(".PartyLedger").val(ledgerId);
                            mainobj.find(".PartyLedger").attr("v",ledgerId);
                        }
                        else {
                            mainobj.find(".SalesPurchaseLedger").val(ledgerId);
                            mainobj.find(".SalesPurchaseLedger").attr("v",ledgerId);
                        }
                        $(this).remove();
                    }

                }
            }
            if (!isparent) {
                ledgerId = $(this).find(".LedgerId").val();
                if (ledgerId != undefined) {
                    if (parseInt(ledgerId) > 0) {
                        var ledger = ledgerdetail(ledgerId);
                        var ledgerName = ledger[0].LedgerName;
                        var colindex = 0;
                        $(this).find("td").each(function() {
                            if (colindex == 1) {
                                $(this).text(ledgerName);
                            }
                            colindex++;
                        });
                    }
                }
            }
        });
    }
    mainobj.find('.save').click(function() {
        if (!isvalidPurchase()) return false;
        SetZeroForAmount(mainobj);
        adjustBreakup();
        Save(mainobj);

        return false;
    });
    function isvalidPurchase() {
        return validate(mainobj);
    }
    mainobj.find(".newrow").find("input").live("keypress", function(event) {
        if (event.keyCode == 13) {
            adddgrid($(this), event, mainobj.find("#tblBreakupDetail"), findtotal, calulateAmount, onedit);
            return false;
        }
    });
    mainobj.find("#ProductId").live("change", function() {
        var istax = false;
        var tax = 0;
        var p = $(this).parent().parent();
        if ($(this).find("option:selected").attr("per") != undefined) {
            istax = true;
            tax = parseFloat($(this).find("option:selected").attr("per"));
            p.find(".Price").val(tax);
        }

        if (istax) {
            p.find(".IsTax").val(1);
        }
        else {
            p.find(".IsTax").val(0);
        }
        showHidecontrols();
    });
    function showHidecontrols() {
        var obj = mainobj.find("#ProductId");
        var p = obj.parent().parent();
        if (p.find(".IsTax").val() == "1") {
            //p.find(".Price").val(tax);
            p.find(".Price").show();
            //p.find(".Price").focus();
            p.find(".Quantity").hide();
            p.find(".Unit").hide();
            p.find(".Amount").show();
            if (p.find(".LedgerId").val() != undefined) {
                if (parseInt(p.find(".LedgerId").val()) > 0) {
                    obj.find("option").each(function() {
                        if ($(this).attr("per") != undefined) {
                            //only for tax ledgers
                            if ($(this).val() == p.find(".LedgerId").val()) {
                                $(this).attr("selected", "selected");
                            }
                        }
                    });

                }
            }
            p.find(".Price").focus();
        }
        else {
            p.find(".Quantity").show();
            //p.find(".Quantity").focus();
            p.find(".Price").show();
            p.find(".Unit").show();
            p.find(".Amount").show();
        }

    }
    function onedit() {
        showHidecontrols();
    }
    mainobj.find(".newrow").find(".Amount").change(function() {
        var a = parseFloat(mainobj.find("#Amount").val());
        var q = parseFloat(mainobj.find("#Quantity").val());
        mainobj.find("#Price").val(parseFloat(a / q).toFixed(2));
        mainobj.find("#Amount").val(a.toFixed(2));
    });
    function calulateAmount() {
        if (mainobj.find("#IsTax").val() != "1") {
            if (mainobj.find("#Quantity").val() == "") {
                mainobj.find("#Quantity").focus();
                return false;
            }
        }
        if (mainobj.find("#Price").val() == "") {
            mainobj.find("#Price").focus();
            return false;
        }
        if (mainobj.find("#ProductId").val() == "0") {
            mainobj.find("#ProductId").focus();
            return false;
        }
        var r = parseFloat(mainobj.find("#Price").val());
        if (mainobj.find("#IsTax").val() == "1") {
            if (mainobj.find("#Amount").val() == "") {
                var subtotal = getSubTotal(mainobj.find("#tblBreakupDetail"), "Amount");
                mainobj.find("#Amount").val(parseFloat((r / 100) * subtotal).toFixed(2));
            }
            mainobj.find("#Unit").val("%");
        }
        else {
            var q = parseFloat(mainobj.find("#Quantity").val());
            mainobj.find("#Amount").val(parseFloat(r * q).toFixed(2));
        }
        if (mainobj.find("#Amount").val() == "NaN") mainobj.find("#Amount").val("");
        return true;
    }

    function findtotal() {
        var drtotal = parseFloat(getTotal(mainobj.find("#tblBreakupDetail"), "Amount"));
        mainobj.find(".lbltotal").text(drtotal.toFixed(2));
        mainobj.find("#TotalAmount").val(drtotal.toFixed(2));
    }
    function adjustBreakup() {
        if (mainobj.find("#RefNo") != undefined && mainobj.find("#ReferenceNo") != undefined) {
            mainobj.find("#ReferenceNo").val(mainobj.find("#RefNo").val());
        }
        mainobj.find("#tblBreakupDetail").find("tr").each(function() {
            var isTax = $(this).find(".IsTax").val();
            if (isTax == "1") {
                if (IsBreakupCr) {
                    $(this).find(".DrAmount").val($(this).find(".Amount").val());
                }
                else {
                    $(this).find(".CrAmount").val($(this).find(".Amount").val());
                }
                if ($(this).find(".ProductId").val() != "0") {
                    $(this).find(".LedgerId").val($(this).find(".ProductId").val());
                }
                $(this).find(".ProductId").val("0");
            }
            else {
                $(this).find(".LedgerId").val("0");
            }
            //                 if ($(this).find(".Quantity").val() == "") {
            //                     $(this).find(".Quantity").val("0");
            //                 }
        });
        mainobj.find(".SourceCrDrAmount").val(mainobj.find("#TotalAmount").val());
        var subtotal = getSubTotal(mainobj.find("#tblBreakupDetail"), "Amount");
        mainobj.find(".DestCrDrAmount").val(subtotal);
    }
    mainobj.find(".PartyLedger").change(function() {
        populateClientDetail();
    });
    function populateClientDetail() {
        var id = "";
        if(mainobj.find(".PartyLedger").attr("v")!=undefined)
        {
            id=mainobj.find(".PartyLedger").attr("v");
        }
        else
        {
            id=mainobj.find(".PartyLedger").val();
        }
        if (parseInt(id) > 0) {
            //mainobj.find(".ClientName").val($(this).find("option:selected").text());
            var data = RequestData("data.php?m=ledger-detail&id=" + id);
            data = jQuery.parseJSON(data);
            mainobj.find(".ledger-det").each(function() {
                var cn = $(this).attr("cn");
                if (cn == undefined) cn = $(this).attr("id");
                $(this).val(data[0][cn]);
            });
            mainobj.find("#ClientId").val(id);
        }
        else {
            mainobj.find(".ledger-det").val("");
            mainobj.find("#ClientId").val("");
        }
    }
    function bindEditableGrid(data,grid,gc) {
        if (data == "") return;
        for (i = 0; i < data.length; i++) {
            
            var c = "repeater-row";
            if (parseInt(i) % 2 == 0) {
                c = "repeater-alt";
            }
            var tr = "<tr id='grid_" + (parseInt(i) + 1) + "' class='" + c + "'>";
            tr += "<td class='hdn'>";
            grid.find(".newrow").find("input").each(function() {
                var id = $(this).attr("id");
                var css = $(this).attr("class");
                var value = "";
                if (data[i][id] != undefined && data[i][id] + '' != 'null') {
                    value = "value='" + data[i][id] + "'";
                }
                tr += "<input type='hidden' name='@sm_" + id + "-" + (parseInt(i) + 1) + "' class='" + css + "' " + value + "/>";
            });
            grid.find(".newrow").find("select").each(function() {
                var id = $(this).attr("id");
                var css = $(this).attr("class");
                var value = "";
                if (data[i][id] != undefined && data[i][id] + '' != 'null') {
                    value = "value='" + data[i][id] + "'";
                }
                tr += "<input type='hidden' name='@sm_" + id + "-" + (parseInt(i) + 1) + "' class='" + css + "' "+value+"/>";
            });
            var arrgc = gc.split(',');
            for (j = 0; j < arrgc.length; j++) {
                var value = "";
                id = arrgc[j];
                if (data[i][id] != undefined && data[i][id] + '' != 'null') {
                    value = data[i][id];
                }
                var css = grid.find(".newrow ." + id).attr("class");
                var align = "";
                if (css != undefined) {
                    if (css.indexOf("right") >= 0) {
                        align = " right";
                    }
                    if (css.indexOf("nozero") >= 0 && value=="0") {
                        value = "";
                    }
                    if (css.indexOf("val-dbl") >= 0) {
                        value = parseFloat(value).toFixed(2);
                    }
                }
                tr += "<td class='gridtr gridtd"+align+"'>" + value + "</td>";
            }
            tr = tr + '<td class="delete-row"><img src="images/close.png" width="16px" height="16px"></td>';
            tr = tr + "</tr>";
            var objTr = $(tr);
            objTr.insertBefore(grid.find(".newrow"));
        }
    }
    function convertAmcToSale() {
        var data = RequestJSONData("data.php?m=amc-product-detail&id=" + QueryString("amcid"));
        var grid = mainobj.find(".grid");
        if (data == "") return;
        mainobj.find(".PartyLedger").attr("v",data[0].ClientId);
        for (i = 0; i < data.length; i++) {
            if (parseInt(data[i].TaxId) > 0) {
                data[i]["ProductName"] = data[i].LedgerName;
            }
        }
        bindEditableGrid(data,grid,'ProductName,Quantity,Price,UnitName,Amount');
    }
}