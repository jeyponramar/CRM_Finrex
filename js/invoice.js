_tax = "";

function initTax() {
    var objjson = RequestData("getdata.php?m=Tax&mt=t");
    _tax = jQuery.parseJSON(objjson);
}
function getTax(id, nm) {
    var val;
    for (i = 1; i < _tax.length; i++) {
        if (id == i) {
            $.each(_tax[i], function(key, value) {
                if (key == nm) {
                    val = value;
                    return;
                }
            });
        }
    }
    return val;
}
function getTotal(table, nm) {
    var colIndex = -1;
    var i = 0;
    var total = 0;
    table.find("input").each(function() {
        if ($(this).attr("name").indexOf(nm + "-") > 0) {
            try {
                total += parseFloat($(this).val());
            } catch (e) { }
        }
    });
    return total;

}
function calculateTax(grid) {
    //Tax Type 1 - Apply on all above amount
    //Tax Type 2 - Apply on above product amount
    //Tax Type 3 - Apply on above tax amount
    var nm = "Amount";
    var rowIndex = 0;
    var prevAmount = 0;
    var allPrevAmountTotal = -1;
    var prevTaxAmount = -1;

    grid.find("tr").each(function() {
        if (rowIndex > 0) {
            var taxId = 0;
            var amount = 0;
            var calAmount = 0;
            var objhdnAmount = 0;

            $(this).find("input").each(function() {
                if ($(this).attr("name").indexOf("TaxId-") > 0) {
                    taxId = parseInt($(this).val());
                }
                else if ($(this).attr("name").indexOf("Amount-") > 0) {
                    amount = parseFloat($(this).val());
                    objhdnAmount = $(this);
                }
            });
            if (taxId == 1) {//apply tax for all above products
                var rowIndextmp = 0;
                allPrevAmountTotal = 0;

                grid.find("tr").each(function() {
                    if (rowIndextmp < rowIndex) {
                        $(this).find("input").each(function() {
                            if ($(this).attr("name").indexOf("Amount-") > 0) {
                                allPrevAmountTotal += parseFloat($(this).val());
                            }
                        });
                    }
                    rowIndextmp++;
                });
                calAmount = parseFloat(allPrevAmountTotal * parseFloat(getTax(taxId, "TaxPercentage")) / 100);
                $(this).find(".Amount").text(calAmount.toFixed(2));
                objhdnAmount.val(calAmount);
            }
            else if (taxId == 2) {//Apply tax on above product amount
                calAmount = parseFloat(prevAmount * parseFloat(getTax(taxId, "TaxPercentage")) / 100);
                $(this).find(".Amount").text(calAmount.toFixed(2));
                objhdnAmount.val(calAmount);
            }
            else if (taxId == 3) {//Apply tax on above tax amount
                var rowIndextmp = 0;
                var prevTaxId = 0;
                var prevTaxAmount = 0;

                grid.find("tr").each(function() {
                    if (rowIndextmp > 0) {
                        if (rowIndextmp < rowIndex) {
                            $(this).find("input").each(function() {
                                if ($(this).attr("name").indexOf("TaxId-") > 0) {
                                    if (parseInt($(this).val()) > 0) {
                                        prevTaxId += parseInt($(this).val());
                                    }
                                }
                            });
                            if (parseInt(prevTaxId) > 0) {
                                prevTaxAmount = parseFloat($(this).find(".Amount").text());
                            }
                        }
                    }
                    rowIndextmp++;
                });

                if (prevTaxId > 0) {
                    calAmount = parseFloat(prevTaxAmount * parseFloat(getTax(taxId, "TaxPercentage")) / 100);
                    $(this).find(".Amount").text(calAmount.toFixed(2));
                    objhdnAmount.val(calAmount);
                }
            }

            if (taxId == 0) prevAmount = amount;
        }
        rowIndex++;
    });
}