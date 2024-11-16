$(document).ready(function() {
    $(".i-mainmenu").removeClass("i-mainmenu-active");
    $(".menu-home").addClass("i-mainmenu-active");
    $(".i-mainmenu").click(function() {
        $(this).find(".submenu").show();
    });
    $(".submenu").hover(function() { }, function() { $(this).hide(); });
    initCCInterestCalc();
    initCancellationForwardContractCalc();
    initEarlyUtilizationCalc();
    initFinstationMenu();
    initMainLeftMenu();
    initUserProfileMenu();
    initPushNotification();
    initEEFCCostCalc();
    $(".txtcal").keyup(function() {
        //getTableColVal($(this),1);
        //calculateTable();
        calculate($(this));
    });
    $(".txtbuyercredit").keyup(function(e1) {
        var total = 0;
        $(".txtbuyercredit").each(function(e2) {
            total += ConvertToDouble($(this).val());
        });
        $("#totalbuyer-credit").text(total.toFixed(2));
    });
    $(".txtsuppliercredit").keyup(function(e1) {
        var total = 0;
        $(".txtsuppliercredit").each(function(e2) {
            total += ConvertToDouble($(this).val());
        });
        $("#totalsupplier-credit").text(total.toFixed(2));
    });
    $(".txtcalbilldisc").change(function() {
        calcBillsDiscount($(this));
    });
    $(".txtcalbilldisc").blur(function() {
        calcBillsDiscount($(this));
    });
    $(".lnksubmenu").click(function() {
        if($(this).closest(".jq-fullfinstation").length > 0 && _isFinstationEnabled == false)
        {
            window.location.href = "noaccessfortrial.aspx";
            return false;
        }
        setTimeout(function() {
            $(".submenu").hide();
        }, 10);
        $(".submenu").hide();
        var w = 0;
        var h = 0;
        if ($(this).attr("w") != undefined) {
            w = $(this).attr("w");
        }
        if ($(this).attr("h") != undefined) {
            h = $(this).attr("h");
            if (h == "100%") h = $(window).height();
        }
        var targetm = $(this).attr("target");
        var target = $("#" + $(this).attr("target"));
        var title = $(this).attr("modaltitle");
        if(title==undefined)title=$(this).attr("title");
        if(title==undefined)title=$(this).text();
        if(title!=undefined){
            target.removeAttr("title");
            target.attr("title", title);
        }
        if($(this).attr("calctype")!=undefined)target.attr("calctype",$(this).attr("calctype"));
        if(w == undefined || w==0)
        {
            target.dialog({ width: "auto",title:title});
        }
        else
        {
            target.dialog({ width: w, height: h,title:title });
        }
        target.find(".jq-result").html("");
        $(".ddl-clear").val("0");
        $(".txtclear").val("");
        $(".lblclear").text("");
        if(targetm == "tblcashconversioncalc")
        {
            bindCurrencyMargin(target.find(".jq-ddlcurrency-margin"));
        }
       
        if(target.find(".jq-forwardannualmonthlypermium1").length > 0)
        {
            bindForwardAnnualMonthlyPremium(target);
        }
    });
    $("#lnkhistory-customrate").live("click",function() {
        window.open("custom-rate-history.aspx", "", "width=600,height=500");
        return false;
    });
    $(".jq-btncurrencyfuture").live("click",function(){
        var target = $(".jq-currencyfuture-placeholder");
        var url = "liverate.ashx?a=ratehtml&m=ratesection&sid="+$(this).attr("sid");
        $(".jq-btncurrencyfuture").removeClass("btncurrency-active");
        $(this).addClass("btncurrency-active");
        ajaxCall(url,function(response){
            target.html(response);
            var modal = target.closest(".ui-dialog");
            var l = $(window).width() / 2 - modal.width()/2;
            modal.css("left",l+"px");
        });
    });
    $(".jq-lnkliverate-modal").click(function(){
        var target = $("#divliverate-modal");
        var url = "liverate.ashx?a=ratehtml&m="+$(this).attr("m");
        if($(this).attr("sid")!=undefined)url+="&sid="+$(this).attr("sid");
        ajaxCall(url,function(response){
            target.html(response);
            var modal = target.closest(".ui-dialog");
            var l = $(window).width() / 2 - modal.width()/2;
            modal.css("left",l+"px");
        });
    });
    $("#jq-btncalc-cashconversioncalc").click(function(){
        var url = "finstationhandler.ashx?m=cashconversion&c="+$("#cashconversioncalc-ddlcurrency").val()+"&margin="+$(".jq-cashconversioncalc-margin").val();
        var isexport = false;
        if($(".jq-rbtncashconversion-covertype").find("input:first").is(":checked"))
        {
            isexport = true;
        }
        url+="&ie="+isexport;
        url+="&calctype="+$(this).closest(".modal").attr("calctype");
        ajaxCalc(url, $("#jq-cashconversioncalc-result"));
    });
    $(".jq-ddlcurrency-margin").change(function(){
        bindCurrencyMargin($(this));
    });
    setTime();
});
function ajaxCalc(URL, resultDiv)
{
    $.ajax({
        url:URL,
        isAsync:true,
        cache:false,
        success:function(response)
        {
            if(response.indexOf("message : ")==0)
            {
                alert(response.replace("message : ",""));
                return;
            }
            resultDiv.html(response);
        }
    });
}
function setTime() {
    var monthNames = [
                      "Jan", "Feb", "Mar",
                      "Apr", "May", "Jun", "Jul",
                      "Aug", "Sep", "Oct",
                      "Nov", "Dec"
                    ];

    var date = new Date();
    var day = date.getDate();
    var monthIndex = date.getMonth();
    var year = date.getFullYear();
    var hh = date.getHours();
    var mm = date.getMinutes();
    var ss = date.getSeconds();
    var ampm = "AM";
    if (hh >= 12) ampm = "PM";
    hh = hh % 12 || 12;
    if (("" + day).length == 1) day = "0" + day;
    if (("" + hh).length == 1) hh = "0" + hh;
    if (("" + mm).length == 1) mm = "0" + mm;
    if (("" + ss).length == 1) ss = "0" + ss;
    var date = day + "-" + monthNames[monthIndex] + "-" + year + " " + hh + ":" + mm + ":" + ss
    $(".jq-currentdate").text(date);
    setTimeout("setTime()", 1000);
}
function calculate(obj) {
    if(obj==undefined || obj==null)
    {
        calcForeignRupee();
        calcCCLoanFCNR();
        calcBuyerCredit();
    }
    else
    {
        var cls = obj.attr("class");
        if(obj.closest(".tblcalcforeignrupee-pcfc-rpc").length > 0)
        {
            calcForeignRupee_PCFC();
        }
        else if (cls.indexOf("txtspread") >= 0 || cls.indexOf("txtsubvention") >= 0) {
            calcForeignRupee();
        }
        else if (cls.indexOf("txtccinterestrate") >= 0 || cls.indexOf("txtfcnr_spread") >= 0) {
            calcCCLoanFCNR();
        }
        else if(cls.indexOf("txtbci_") >= 0)
        {
            calcBuyerCredit();
        }
        else if(cls.indexOf("txtbcic_") >= 0)
        {
            calcBuyerCreditInterest(obj);
        }
        else if (cls.indexOf("cfc_") >= 0) {
            calcCancellationForwardContract();
        }
        else if (cls.indexOf("ceu_") >= 0) {
            calcEarlyUtilization();
        }
        else if(obj.closest("#tblpcfcvsepcnetgainloss").length > 0)
        {
            calPcFcVsEPCNetGainLoss();
        }
    }
}
function calcForeignRupee() {
    $(".tblcalcforeignrupee").each(function() {
        for (i = 1; i <= 4; i++) {
            var txtspread = $(this).find(".txtspread" + i);
            var txtsubvention = $(this).find(".txtsubvention" + i);
            var lblcalcpremium = $(this).find(".lblcalcpremium" + i);
            var lblcal_foreignrupee_result = $(this).find(".lblcal_foreignrupee_result" + i);

            var libor = ConvertToDouble($(this).find(".lblcalclibor" + i).text());
            var spread = ConvertToDouble(txtspread.val());
            var premium = ConvertToDouble(lblcalcpremium.text());
            var subvention = ConvertToDouble($(this).find(".txtsubvention" + i).val());

            var lblcalc_pcfccosting = $(this).find(".lblcalc_pcfccosting" + i);
            var lblcal_rfccosting = $(this).find(".lblcal_rfccosting" + i);


            var pcfc = parseFloat(ConvertToDouble(libor) + ConvertToDouble(spread)).toFixed(4);
            var rpc = parseFloat(ConvertToDouble(subvention) - ConvertToDouble(premium)).toFixed(4);
            lblcalc_pcfccosting.text(pcfc);
            lblcal_rfccosting.text(rpc);

            if (txtspread.val() == "" || txtsubvention.val() == "") {
                lblcal_foreignrupee_result.text("");
            }
            else {
                if (rpc >= pcfc) {
                    lblcal_foreignrupee_result.text("GO with PCFC");
                }
                else {
                    lblcal_foreignrupee_result.text("Go with RPC");
                }
            }
        }
    });
}
function calcForeignRupee_PCFC() {
    $(".tblcalcforeignrupee-pcfc-rpc").each(function() {
        for (i = 1; i <= 4; i++) {
            var txtspread = $(this).find(".txtspread" + i);
            var txtrpcinterestrate = $(this).find(".txtrpcinterestrate" + i);
            var txtinterestsubvention = $(this).find(".txtinterestsubvention" + i);
            var lblcalcpremium = $(this).find(".lblcalcpremium" + i);
            var lblcal_foreignrupee_result = $(this).find(".lblcal_foreignrupee_result" + i);
            var lblcal_postsubvention = $(this).find(".lblcal_postsubvention" + i);
            var libor = ConvertToDouble($(this).find(".lblcalclibor" + i).text());
            var spread = ConvertToDouble(txtspread.val());
            var premium = ConvertToDouble(lblcalcpremium.text());
            var interestsubvention = ConvertToDouble(txtinterestsubvention.val());
            var rpcinterestrate = ConvertToDouble(txtrpcinterestrate.val());
            var lblcalc_pcfccosting = $(this).find(".lblcalc_pcfccosting" + i);
            var lblcal_rfccosting = $(this).find(".lblcal_rfccosting" + i);

            var pcfc = parseFloat(ConvertToDouble(libor) + ConvertToDouble(spread)).toFixed(4);
            var postsubvention = parseFloat(ConvertToDouble(rpcinterestrate) - ConvertToDouble(interestsubvention)).toFixed(4);
            var rpc = parseFloat(ConvertToDouble(postsubvention) - ConvertToDouble(premium)).toFixed(4);
            lblcalc_pcfccosting.text(pcfc);
            lblcal_rfccosting.text(rpc);
            lblcal_postsubvention.text(postsubvention);
            
            if (txtspread.val() == "" || txtinterestsubvention.val() == "" || txtrpcinterestrate.val() == "") {
                lblcal_foreignrupee_result.text("");
            }
            else {
                if (rpc >= pcfc) {
                    lblcal_foreignrupee_result.text("GO with PCFC");
                }
                else {
                    lblcal_foreignrupee_result.text("Go with RPC");
                }
            }
        }
    });
}
function calcCCLoanFCNR() {
    $(".jq-tblcalc_cc_fcnr").each(function() {
        for (i = 1; i <= 4; i++) {
            var txtspread = $(this).find(".txtfcnr_spread" + i);
            var txtccinterestrate = $(this).find(".txtccinterestrate" + i);
            var lblcal_result = $(this).find(".lblcal_result" + i);

            var libor = ConvertToDouble($(this).find(".lblcalclibor" + i).text());
            var ccinterest = ConvertToDouble(txtccinterestrate.val());
            var lblcalcpremium = ConvertToDouble($(this).find(".lblcalcpremium" + i).text());
            var hedgingcost = ConvertToDouble($(this).find(".lblhedgingcost" + i).text());
            var lblcalc_total = $(this).find(".lblcalc_total" + i);
            var lblcal_fcnrcost = $(this).find(".lblcal_fcnrcost" + i);
            var spread = ConvertToDouble(txtspread.val());

            var total = parseFloat(ConvertToDouble(libor) + ConvertToDouble(spread)).toFixed(4);
            var rpc = parseFloat(ConvertToDouble(total) + ConvertToDouble(hedgingcost)).toFixed(4);
            lblcalc_total.text(total);
            lblcal_fcnrcost.text(rpc);
            if (txtspread.val() == "" || txtccinterestrate.val() == "") {
                lblcal_result.text("");
            }
            else {
                if (ccinterest >= rpc) {
                    lblcal_result.text("GO with FCNR B");
                }
                else {
                    lblcal_result.text("Go with CC");
                }
            }
        }
    });
}
function calcBuyerCredit() {
    $(".tblcalcbuyercredit").each(function() {
        for (i = 1; i <= 4; i++) {
            var txtspread = $(this).find(".txtbci_spread" + i);
            var txtlou = $(this).find(".txtbci_lou" + i);
            var txtbci_other = $(this).find(".txtbci_other" + i);
            var txtbci_interestrate = $(this).find(".txtbci_interestrate" + i);
            var lblbci_libor = $(this).find(".lblcalc-bci-libor" + i);
            var lblbci_totalcost = $(this).find(".lblbci_totalcost" + i);
            var lblbci_hedging = $(this).find(".lblcalc-bci-hedging" + i);
            var lblbci_tothedging = $(this).find(".lblcal_bcitothedging" + i);
            var lblbci_result = $(this).find(".lblcal_bci_result" + i);

            var libor = ConvertToDouble(lblbci_libor.text());
            var spread = ConvertToDouble(txtspread.val());
            var lou = ConvertToDouble(txtlou.val());
            var othercost = ConvertToDouble(txtbci_other.val());
            var totalcost = ConvertToDouble(lblbci_totalcost.text());
            var hedging = ConvertToDouble(lblbci_hedging.text());
            
            var totalcost = parseFloat(ConvertToDouble(libor) + ConvertToDouble(spread) + ConvertToDouble(lou)
                                         + ConvertToDouble(othercost)).toFixed(2);
            lblbci_totalcost.text(totalcost);
            
            var totalhedging = parseFloat(ConvertToDouble(hedging) + ConvertToDouble(totalcost)).toFixed(2);
            lblbci_tothedging.text(totalhedging);
            
            if (txtbci_interestrate.val() == "") {
                lblbci_result.text("");
            }
            else {
                var ccinterest = ConvertToDouble(txtbci_interestrate.val());
                if (ccinterest >= totalhedging) {
                    lblbci_result.text("GO with BC");
                }
                else {
                    lblbci_result.text("Go with CC");
                }
            }
        }
    });
}
function calPcFcVsEPCNetGainLoss()
{
    var tbl = $("#tblpcfcvsepcnetgainloss");
    var netloanAmount = ConvertToDouble(tbl.find(".txtnotionalloanamount").val());
    var interestrate_foreign = ConvertToDouble(tbl.find(".txtinterestrate-foreign").val());
    var interestrate_rupee = ConvertToDouble(tbl.find(".txtinterestrate-rupee").val());
    var subvention_rate_foreign = ConvertToDouble(tbl.find(".txtsubvention-rate-foreign").val());
    var subvention_rate_rupee = ConvertToDouble(tbl.find(".txtsubvention-rate-rupee").val());
    var gain_forward_premium = ConvertToDouble(tbl.find(".txtgain-forward-premium").val());
    var credit_period_days = ConvertToDouble(tbl.find(".txtcredit-period-days").val());
    var current_spotrate = ConvertToDouble(tbl.find(".txtcurrent-spotrate").val());
    var future_spotrate = ConvertToDouble(tbl.find(".txtfuture-spotrate").val());
    //set the labels
    tbl.find(".jq-lbl-notional-amount-foreign").text("$ "+FormatAmountComma(netloanAmount));
    tbl.find(".jq-lbl-notional-current-spotrate").text(FormatAmountComma(current_spotrate));
    tbl.find(".jq-lbl-intesrestrate-foreign").text(interestrate_foreign+"%");
    tbl.find(".jq-lbl-intesrestrate-rupee").text(interestrate_rupee+"%");
    tbl.find(".jq-lbl-gain-forward-rupee").text(gain_forward_premium+"%");
    tbl.find(".jq-lbl-netcost-loan-terms-spotrate-foreign").text(FormatAmountComma(future_spotrate));
    tbl.find(".jq-lbl-benifit-subvention-amount-foreign").text(FormatAmountComma(subvention_rate_foreign));
    tbl.find(".jq-lbl-benifit-subvention-rate-rupee").text(subvention_rate_rupee+"%");
    //calculation
    var notionalloan_amount_rupee = ConvertToDouble(parseFloat(netloanAmount * current_spotrate).toFixed(2));
    tbl.find(".jq-lbl-notional-amount-rupee").text(FormatAmountComma(notionalloan_amount_rupee));
    var interestrate_amount_foreign = ConvertToDouble(parseFloat(netloanAmount * interestrate_foreign/100.0 * credit_period_days / 360.0).toFixed(2));
    tbl.find(".jq-lbl-intesrestrate-amount-foreign").text(FormatAmountComma(interestrate_amount_foreign));
    var interestrate_amount_rupee = ConvertToDouble(parseFloat(notionalloan_amount_rupee * interestrate_rupee/100.0 * credit_period_days / 365.0).toFixed(2));
    tbl.find(".jq-lbl-intesrestrate-amount-rupee").text(FormatAmountComma(interestrate_amount_rupee));
    //var gain_forward_amount_rupee = parseFloat(interestrate_amount_rupee * gain_forward_premium / 365.0).toFixed(2);
    var gain_forward_amount_rupee = ConvertToDouble(parseFloat(notionalloan_amount_rupee * gain_forward_premium / 100.0 * credit_period_days / 365.0).toFixed(2));
    tbl.find(".jq-lbl-gain-forward-amount-rupee").text(FormatAmountComma(gain_forward_amount_rupee));
    var hedgingcost_amount_rupee  = 0;
    tbl.find(".jq-lbl-hedgingcost-amount-rupee").text();
    var total_repayment_foreign  = ConvertToDouble(parseFloat(netloanAmount + interestrate_amount_foreign).toFixed(2));
    tbl.find(".jq-lbl-total-repayment-foreign").text("$ " + FormatAmountComma(total_repayment_foreign));
    var total_repayment_amount_rupee = ConvertToDouble(parseFloat(notionalloan_amount_rupee + interestrate_amount_rupee - gain_forward_amount_rupee + hedgingcost_amount_rupee).toFixed(2));
    tbl.find(".jq-lbl-total-repayment-amount-rupee").text(FormatAmountComma(total_repayment_amount_rupee));
    var netcost_interest_foreign = ConvertToDouble(parseFloat(total_repayment_foreign - netloanAmount).toFixed(2));
    tbl.find(".jq-lbl-netcost-interest-foreign").text("$ "+FormatAmountComma(netcost_interest_foreign));
    var netcost_interest_rupee = ConvertToDouble(parseFloat(total_repayment_amount_rupee - notionalloan_amount_rupee).toFixed(2));
    tbl.find(".jq-lbl-netcost-interest-rupee").text(FormatAmountComma(netcost_interest_rupee));
    var netcost_loan_terms_foreign = ConvertToDouble(parseFloat(netcost_interest_foreign * future_spotrate).toFixed(2));
    tbl.find(".jq-lbl-netcost-loan-terms-foreign").text(FormatAmountComma(netcost_loan_terms_foreign));
    var netcost_loan_terms_amount_rupee = netcost_interest_rupee;
    tbl.find(".jq-lbl-netcost-loan-terms-amount-rupee").text(FormatAmountComma(netcost_loan_terms_amount_rupee));
    var benifit_subvention_rate_foreign = ConvertToDouble(parseFloat(subvention_rate_foreign * notionalloan_amount_rupee * 365.0 / credit_period_days).toFixed(2));
    tbl.find(".jq-lbl-benifit-subvention-rate-foreign").text(benifit_subvention_rate_foreign+"%");
    var benifit_subvention_amount_rupee = ConvertToDouble(parseFloat(subvention_rate_rupee/100.0 * notionalloan_amount_rupee * credit_period_days / 365.0).toFixed(2));
    tbl.find(".jq-lbl-benifit-subvention-amount-rupee").text(FormatAmountComma(benifit_subvention_amount_rupee));
    var current_spot_amount_foreign = ConvertToDouble(parseFloat(netcost_loan_terms_foreign + subvention_rate_foreign).toFixed(2));
    tbl.find(".jq-lbl-current-spot-amount-foreign").text(FormatAmountComma(current_spot_amount_foreign));
    var current_spot_amount_rupee = ConvertToDouble(parseFloat(netcost_loan_terms_amount_rupee - benifit_subvention_amount_rupee).toFixed(2));//changed + to - on 7/12/22
    tbl.find(".jq-lbl-current-spot-amount-rupee").text(FormatAmountComma(current_spot_amount_rupee));
    var net_gain_amount_foreign = ConvertToDouble(parseFloat(current_spot_amount_rupee - current_spot_amount_foreign).toFixed(2));
    tbl.find(".jq-lbl-net-gain-amount-foreign").text(FormatAmountComma(net_gain_amount_foreign));
    var net_gain_amount_rupee = ConvertToDouble(parseFloat(current_spot_amount_foreign - current_spot_amount_rupee).toFixed(2));
    tbl.find(".jq-lbl-net-gain-amount-rupee").text(FormatAmountComma(net_gain_amount_rupee));
    var netgain_annual_amount_foreign = parseFloat(net_gain_amount_foreign / notionalloan_amount_rupee * 365.0 / credit_period_days * 100).toFixed(2);
    tbl.find(".jq-lbl-netgain-annual-amount-foreign").text(netgain_annual_amount_foreign+"%");
    var netgain_annual_amount_rupee = parseFloat(net_gain_amount_rupee / notionalloan_amount_rupee * 365.0 / credit_period_days * 100).toFixed(2);
    tbl.find(".jq-lbl-netgain-annual-amount-rupee").text(netgain_annual_amount_rupee+"%");
}
function getTableColVal(ctrl, row) {
    var tbl = ctrl.closest("table");
    var col = 0;
    var foundcol = false;
    ctrl.closest("tr").find("td").each(function() {

    });
}
function calculateTable() {
    $(".tablecalc").each(function() {
        try {
            var calc = $(this).attr("calc");
            var val = "";
            var isexists = true;
            while (isexists) {
                if (calc.indexOf("#") >= 0) {
                    var startIndex = calc.indexOf("#");
                    var endIndex = calc.indexOf("]");
                    var c = calc.substring(startIndex + 1, endIndex + 1);
                    var ctCalc = "#" + c;
                    var arr1 = c.split('[');
                    var scode = arr1[0];
                    var temp = arr1[1].replace("]", "");
                    var arr2 = temp.split(',');
                    var row = arr2[0];
                    var col = arr2[1];
                    if (scode == "tbl") {
                        alert(row);
                        alert(col);
                        var d = "data"; //$("." + scode + "_" + row + "_" + col).text();
                        var data = d;
                        calc = calc.replace(ctCalc, d);
                    }
                }
                else {
                    isexists = false;
                }
            }
            
        }
        catch (e1) { }
    });
}
function calcBillsDiscount(obj)
{
    var tbl = obj.closest(".tblcalbilldisc");
    var txtbdbillamount = ConvertToDouble(tbl.find(".txtbd-billamount").val());
    var txtbdmargin = ConvertToDouble(tbl.find(".txtbd-margin").val());
    var lblbddiscountamount = tbl.find(".lblbd-discountamount");
    var txtbdtenor = ConvertToDouble(tbl.find(".txtbd-tenor").val());
    var txtbdinvoicedate = tbl.find(".txtbd-invoicedate").val();
    var txtbdbldate = tbl.find(".txtbd-bldate").val();
    var txtbdduedate = tbl.find(".txtbd-duedate").val();
    var lblbddiscountamount2 = tbl.find(".lblbd-discountamount2");
    var txtbdconversionrate = ConvertToDouble(tbl.find(".txtbd-conversionrate").val());
    var lblbdinterestamount = tbl.find(".lblbd-interestamount");
    var txtbdinterestrate = ConvertToDouble(tbl.find(".txtbd-interestrate").val());
    var txtbdbilldate = tbl.find(".txtbd-billdate").val();
    var lblbdcreditperiod = tbl.find(".lblbd-creditperiod");
    var lblbdinterestamount2 = tbl.find(".lblbd-interestamount2");
    var txtbdremittance = tbl.find(".txtbd-remittance").val();
    var lblbdoverduedays = tbl.find(".lblbd-overduedays");
    var txtbdoverdueinterestrate = ConvertToDouble(tbl.find(".txtbd-overdueinterestrate").val());
    var lblbdoverdueinterestamount = tbl.find(".lblbd-overdueinterestamount");
    
    var bddiscountamount = parseFloat(txtbdbillamount - txtbdbillamount / 100.0 * txtbdmargin).toFixed(2);
    lblbddiscountamount.text(bddiscountamount);
    
    lblbddiscountamount2.text(bddiscountamount);
    
    var bdinterestamount = parseFloat(bddiscountamount * txtbdconversionrate).toFixed(2);
    lblbdinterestamount.text(bdinterestamount);
    
    var creditperiod = 0;
    if(txtbdduedate!="" && txtbdbilldate!="")
    {
        var duedate = parseDate(txtbdduedate);
        var billdate = parseDate(txtbdbilldate);
        creditperiod = daydiff(billdate,duedate);
    }
    lblbdcreditperiod.text(creditperiod);
    
    var interestamount = 0;
    interestamount = bdinterestamount
    lblbdinterestamount2.text(parseFloat(interestamount * txtbdinterestrate / 100.0 * creditperiod / 365.0).toFixed(2));
    
    var overdueDays = 0;
    if(txtbdduedate!="" && txtbdremittance!="")
    {
        overdueDays = daydiff(parseDate(txtbdduedate),parseDate(txtbdremittance));
    }
    lblbdoverduedays.text(overdueDays);
    
    var overdueInterest = 0;
    if(txtbdoverdueinterestrate > 0)
    {
        overdueInterest = parseFloat(bdinterestamount * overdueDays / 365.0 * (txtbdoverdueinterestrate / 100.0)).toFixed(2);
    }
    lblbdoverdueinterestamount.text(overdueInterest);
}
function parseDate(str) {
    var mdy = str.split('-')
    return new Date(mdy[2], mdy[1]-1, mdy[0]);
}

function daydiff(first, second) {
    return Math.round((second-first)/(1000*60*60*24));
}
function initCCInterestCalc()
{
    var html = "";
    if($(".lblccbalance").length==0)
    {
        for(i=0;i<31;i++)
        {
            html+="<tr id='trcc_"+(i+1)+"'><td width='20%'>"+(i+1)+"</td><td width='20%'><input type='text' class='txtcalc val-dbl txtccwithdrawal txtccinterest'/></td width='20%'>"+
                    "<td><input type='text' class='txtcalc val-dbl txtccdeposit txtccinterest'/></td>"+
                    "<td class='lblccbalance' align='right' width='20%'></td><td width='20%' class='lblccinterest' align='right'></td></tr>";
        }
        $("#tblccinterest").append(html);
    }
    $(".txtccinterest").live("blur",function(e1){
        ccInterestCalc();
    });
    $(".txtccinterest").live("change",function(e1){
        ccInterestCalc();
    });
    showHideDaysInMonth();
    $(".ddldaysinmonth").change(function(e1){
        showHideDaysInMonth();
        ccInterestCalc();
    });
}
function showHideDaysInMonth()
{
    var daysinm = parseInt($(".ddldaysinmonth").val());
    for(i=daysinm;i<=31;i++)
    {
        $("#trcc_"+i).hide();
    }
    for(i=28;i<=daysinm;i++)
    {
        $("#trcc_"+i).show();
    }
}
function ccInterestCalc()
{
    var lblccinterestamount = $("#lblccinterestamount");
    var rateofinterest = ConvertToDouble($("#txtrateofinterest").val());
    var tccopeningbal = ConvertToDouble($("#txtccopeningbal").val());
    var ccbal = tccopeningbal;
    var totalccinterest = 0;
    lblccinterestamount.text(tccopeningbal);
    $("#tblccinterest").find("tr").each(function(e1){
        if($(this).css("display")!="none")
        {
            var withdrawal = ConvertToDouble($(this).find(".txtccwithdrawal").val());
            var deposit = ConvertToDouble($(this).find(".txtccdeposit").val());
            var bal  = parseFloat(ccbal - withdrawal + deposit).toFixed(2);
            $(this).find(".lblccbalance").text(bal);
            var ccinterest = 0;
            if(bal<0)
            {
                ccinterest = bal * rateofinterest / 100.0 * 1/365.0;
            }
            $(this).find(".lblccinterest").text(parseFloat(ccinterest).toFixed(2));
            totalccinterest += ccinterest;    
            ccbal = bal;
        }
    });
    $("#lblcctotalinterest").text(parseFloat(totalccinterest).toFixed(2));
}
function calcBuyerCreditInterest(obj)
{
    var tbl = obj.closest(".tblcalcbuyercreditinterest");
    var transamount = ConvertToDouble(tbl.find(".txtbcic_transamount").val());
    var convrate = ConvertToDouble(tbl.find(".txtbcic_convrate").val());
    var libor = ConvertToDouble(tbl.find(".txtbcic_libor").val());
    var spread = ConvertToDouble(tbl.find(".txtbcic_spread").val());
    var intper = ConvertToDouble(tbl.find(".txtbcic_intper").val());
    var tenor = ConvertToDouble(tbl.find(".txtbcic_tenor").val());
    var int_fc = ConvertToDouble(tbl.find(".lblbcic_int_fc").val());
    var swift_fc = ConvertToDouble(tbl.find(".txtbcic_swift_fc").val());
    var louper = ConvertToDouble(tbl.find(".txtbcic_louper").val());
    var arrfees = ConvertToDouble(tbl.find(".txtbcic_arrfees").val());
    var othercost = ConvertToDouble(tbl.find(".txtbcic_othercost").val());
    var hedgingper = ConvertToDouble(tbl.find(".txtbcic_hedgingper").val());
    var ccintper = ConvertToDouble(tbl.find(".txtbcic_ccintper").val());
    
    var lblbcic_transamount_inr=tbl.find(".lblbcic_transamount_inr");
    var lbl_bcic_totintcost = tbl.find(".lbl_bcic_totintcost");
    var lblbcic_int_fc = tbl.find(".lblbcic_int_fc");
    var lblbcic_int_inr = tbl.find(".lblbcic_int_inr");
    var lblbcic_swift_inr = tbl.find(".lblbcic_swift_inr");
    var lblbcic_lou_fc = tbl.find(".lblbcic_lou_fc");
    var lblbcic_lou_inr = tbl.find(".lblbcic_lou_inr");
    var lblbcic_tc_fc = tbl.find(".lblbcic_tc_fc");
    var lblbcic_tc_inr = tbl.find(".lblbcic_tc_inr");
    var lblbcic_thc_fc = tbl.find(".lblbcic_thc_fc");
    var lblbcic_thc_inr = tbl.find(".lblbcic_thc_inr");
    var lblbcic_ccint_inr = tbl.find(".lblbcic_ccint_inr");
    var lblbcic_benefit_fc = tbl.find(".lblbcic_benefit_fc");
    var lblbcic_benefit_inr = tbl.find(".lblbcic_benefit_inr");
    var lblbcic_hedging_inr=tbl.find(".lblbcic_hedging_inr");
    
    var c_transamount_inr = transamount * convrate;
    var c_totalint_cost = libor + spread;
    var c_intamount_fc = transamount * (c_totalint_cost / 100) * tenor / 360.0;
    var c_intamount_inr = c_intamount_fc * convrate;
    var c_swift_inr = swift_fc * convrate;
    var c_louamount_fc = transamount * louper/100.0 * tenor / 360.0;
    var c_louamount_inr = c_louamount_fc * convrate;
    var c_totalcost_inr = c_intamount_inr + c_swift_inr + c_louamount_inr + arrfees + othercost;
    var c_totalcost_fc = 0;
    if(c_transamount_inr!=0 && tenor != 0)
    {
        c_totalcost_fc = c_totalcost_inr / c_transamount_inr * 360.0 / tenor;
        c_totalcost_fc = c_totalcost_fc * 100.0;//convert to per.
    }
    var c_hedging_inr = c_transamount_inr * hedgingper * tenor / 365.0 / 100.0;
    var c_tothedging_fc = c_totalcost_fc + hedgingper;
    var c_tothedging_inr = c_totalcost_inr + c_hedging_inr;
    var c_ccint_inr = c_transamount_inr * tenor / 365.0 * ccintper / 100.0;
    var c_benifit_fc = ccintper  - c_tothedging_fc;
    var c_benifit_inr = c_ccint_inr - c_tothedging_inr;
    
    lblbcic_transamount_inr.text(parseFloat(c_transamount_inr).toFixed(2));
    lbl_bcic_totintcost.text(c_totalint_cost);
    lblbcic_int_fc.text(parseFloat(c_intamount_fc).toFixed(2));
    lblbcic_int_inr.text(parseFloat(c_intamount_inr).toFixed(2));
    lblbcic_swift_inr.text(parseFloat(c_swift_inr).toFixed(2));
    lblbcic_lou_fc.text(parseFloat(c_louamount_fc).toFixed(2));
    lblbcic_lou_inr.text(parseFloat(c_louamount_inr).toFixed(2));
    lblbcic_tc_fc.text(parseFloat(c_totalcost_fc).toFixed(2));
    lblbcic_tc_inr.text(parseFloat(c_totalcost_inr).toFixed(2));
    lblbcic_hedging_inr.text(parseFloat(c_hedging_inr).toFixed(2));
    lblbcic_thc_fc.text(parseFloat(c_tothedging_fc).toFixed(2));
    lblbcic_thc_inr.text(parseFloat(c_tothedging_inr).toFixed(2));
    lblbcic_ccint_inr.text(parseFloat(c_ccint_inr).toFixed(2));
    lblbcic_benefit_fc.text(parseFloat(c_benifit_fc).toFixed(2));
    lblbcic_benefit_inr.text(parseFloat(c_benifit_inr).toFixed(2));
}
function initCancellationForwardContractCalc() {
    var cashspot = ConvertToDouble($(".cfc_cashspotrate_export3").text()) / 100.0;
    $(".cfc_cashspotrate_export3").text(cashspot.toFixed(4));
    cashspot = ConvertToDouble($(".cfc_cashspotrate_export4").text()) / 100.0;
    $(".cfc_cashspotrate_export4").text(cashspot.toFixed(4));
    cashspot = ConvertToDouble($(".cfc_cashspotrate_import3").text()) / 100.0;
    $(".cfc_cashspotrate_import3").text(cashspot.toFixed(4));
    cashspot = ConvertToDouble($(".cfc_cashspotrate_import4").text()) / 100.0;
    $(".cfc_cashspotrate_import4").text(cashspot.toFixed(4));
    
    $("#cfc_ddlcurrency_exp").change(function() {
        calcPremium();
        calcCancellationForwardContract();
    });
    $("#ddlcfc_cashoutlay_co_exp").change(function() {
        if ($(this).val() == "1") {
            $(".trcashoutlay").show();
        }
        else {
            $(".trcashoutlay").hide();
        }
    });
    $("#lnkcancellationcontract_exp").click(function() {
        $(".calculator_cancellation_export").attr("title", "Cancellation of forward Contract (Export)");
        $("#ui-dialog-title-calculator_cancellation_export").text("Cancellation of forward Contract (Export)");
        $(".calculator_cancellation_export").attr("isexport", "true");
        $("#calculator_cancellation_export").find(".lblcal").text("");
        $("#calculator_cancellation_export").find(".txtcal").val("");
        $("#calculator_cancellation_export").find(".cfc_cashspotrate_import").hide();
        $("#calculator_cancellation_export").find(".cfc_cashspotrate_export").hide();
        //$("#calculator_cancellation_export").find(".cfc_spotrate_exp").hide();
        //$("#calculator_cancellation_export").find(".cfc_spotrate_imp").hide();
        $("#calculator_cancellation_export").find(".trcashoutlay").hide();
        $("#cfc_ddlcurrency_exp").val("0");
        $("#ddlcfc_cashoutlay_co_exp").val("0");
        $("#tdcfc_spotrate").text("");
    });
    $("#lnkcancellationcontract_imp").click(function() {
        $(".calculator_cancellation_export").attr("title", "Cancellation of forward Contract (Import)");
        $("#ui-dialog-title-calculator_cancellation_export").text("Cancellation of forward Contract (Import)");
        $(".calculator_cancellation_export").attr("isexport", "false");
        $("#calculator_cancellation_export").find(".lblcal").text("");
        $("#calculator_cancellation_export").find(".txtcal").val("");
        $("#calculator_cancellation_export").find(".cfc_cashspotrate_import").hide();
        $("#calculator_cancellation_export").find(".cfc_cashspotrate_export").hide();
        //$("#calculator_cancellation_export").find(".cfc_spotrate_exp").hide();
        //$("#calculator_cancellation_export").find(".cfc_spotrate_imp").hide();
        $("#calculator_cancellation_export").find(".trcashoutlay").hide();
        $("#cfc_ddlcurrency_exp").val("0");
        $("#ddlcfc_cashoutlay_co_exp").val("0");
        $("#tdcfc_spotrate").text("");
    });
    $("#lnkearlyutilization_imp").click(function() {
        var div = $("#" + $(this).attr("target"));
        div.attr("isexport", "false");
        clearData(div);
        var title = "Early Utilisation of Forward Contract (Import)";
        $("#ui-dialog-title-calculator_earlyutilization_export").text(title);
        $(".calculator_earlyutilization_export").attr("title", title);
        return false;
    });
    $("#lnkearlyutilization_exp").click(function() {
        var div = $("#" + $(this).attr("target"));
        div.attr("isexport", "true");
        clearData(div);
        var title = "Early Utilisation of Forward Contract (Export)";
        $("#ui-dialog-title-calculator_earlyutilization_export").text(title);
        $(".calculator_earlyutilization_export").attr("title", title);
        return false;
    });
    $("#cfc_ddlcurrency_exp").change(function() {
        var isexport = false;
        if ($("#calculator_cancellation_export").attr("isexport") == "true") {
            isexport = true;
        }
        /*$(".cfc_spotrate_exp").hide();
        $(".cfc_spotrate_imp").hide();
        if (isexport) {
        $(".cfc_spotrateimp" + $(this).val()).show();
        }
        else {
        $(".cfc_spotrateexp" + $(this).val()).show();
        }*/
        var currencyId = ConvertToInt($(this).val());
        var spotrate = "";
        if (currencyId > 0) {
            if (isexport) {
                spotrate = $(".SpotRate_" + currencyId + "_2:first").text();
            }
            else {
                spotrate = $(".SpotRate_" + currencyId + "_1:first").text();
            }
        }
        $("#tdcfc_spotrate").text(spotrate);
        calcPremium();
    });
}
function initEarlyUtilizationCalc() {
    $("#ceu_ddlcurrency_exp").change(function() {
        var isexport = false;

        if ($("#calculator_earlyutilization_export").attr("isexport") == "true") {
            isexport = true;
        }
        //set cash/spot
        var cashSpot = 0;
        var currencyId = ConvertToInt($(this).val());
        if (currencyId > 0) {
            if (isexport) {
                cashSpot = $(".cfc_cashspotrate_export" + currencyId).text();
            }
            else {
                cashSpot = $(".cfc_cashspotrate_import" + currencyId).text();
            }
            if (isexport) {
                spotrate = $(".SpotRate_" + currencyId + "_1:first").text();
            }
            else {
                spotrate = $(".SpotRate_" + currencyId + "_2:first").text();
            }
            $(".lblceu_spotrate_exp").text(spotrate);
            if (isexport) {
                spotrate = $(".SpotRate_" + currencyId + "_2:first").text();
            }
            else {
                spotrate = $(".SpotRate_" + currencyId + "_1:first").text();
            }
            $(".ceu_spotrate").text(spotrate);
        }
        cashSpot = parseFloat(cashSpot / 100.0).toFixed(4);
        $(".lblceu_cashspot_exp").text(cashSpot);
        //calculate premium
        calcPremium_earlyutil();
    });
    $(".ceu_forwardrate_exp").change(function() {
        $(".lblceu_contractedrate_exp").text($(this).val());
    });
    $(".ceu_delperiod1_exp").change(function() {
        $(".lblceu_forwardrateon_exp").text($(this).val());
    });
    $("#ddlceu_cashoutlay_co_exp").change(function() {
        if ($(this).val() == "0") {
            $(".trcashoutlay_earlyutil").hide();
        }
        else {
            $(".trcashoutlay_earlyutil").show();
        }
    });
}
function calcEarlyUtilization() {
    var earlyUtilRate = 0;
    var forwardRate = ConvertToDouble($(".ceu_forwardrate_exp").val());
    $(".lblceu_earlyutilizationrate_exp").text(earlyUtilRate);
    var forwardPremium = ConvertToDouble($(".lblceu_forwardpremium_exp").text());
    var bankMargin = ConvertToDouble($(".ceu_bankmargininpaisa_exp").val());
    var cashSpot = ConvertToDouble($(".lblceu_cashspot_exp").text());
    earlyUtilRate = forwardRate - forwardPremium - (bankMargin / 100.0) - (cashSpot);
    $(".lblceu_earlyutilizationrate_exp").text(earlyUtilRate.toFixed(4));
    var spotRate = ConvertToDouble($(".lblceu_spotrate_exp").text());
    var netgainloss = earlyUtilRate - spotRate;
    var isImport = false;

    if ($("#ui-dialog-title-calculator_earlyutilization_export").text().indexOf("Import") > 0) 
    {
        isImport = true;
    }
    if (isImport) {
        netgainloss = spotRate - earlyUtilRate;
    }
    $(".lblceu_netswapgainandloss_exp").text(netgainloss.toFixed(4));
    var totProfitLoss = 0;
    var amount = ConvertToDouble($(".ceu_amount_exp").val());
    totProfitLoss = (amount * netgainloss).toFixed(4);
    $(".lblceu_totalprofitandloss_exp").text(totProfitLoss);
    if (totProfitLoss < 0) {
        $(".lblceu_totalprofitandloss_exp").css("background-color", "#ff0000");
    }
    else {
        $(".lblceu_totalprofitandloss_exp").css("background-color", "#138407");
    }
    var interestRate = ConvertToDouble($(".ceu_interestrate_exp").val()) / 100.0;
    var spotDate = $(".ceu_spotdate").text();
    var fromDate = $(".ceu_delperiod1_exp").val();
    var spotRate2 = ConvertToDouble($(".ceu_spotrate").text());
    var noOfDays = ConvertToInt($(".lblceu_noofdays_exp").text());
    var interest = amount *(forwardRate - spotRate2) * interestRate * noOfDays / 365.0;
    interest = interest.toFixed(4);
    $(".lblceu_interest_exp").text(interest);
}
function calcCancellationForwardContract() {
    var forwardRate = 0;
    var spotrate = 0;
    var swapgain = 0;
    var cashspot = 0;
    var netswapgain = 0;
    var totalProfitLoss = 0;
    var interest = 0;

    var premium = ConvertToDouble($(".lblcfc_forwardpremium_exp").text());
    var contractRate = ConvertToDouble($(".cfc_forwardrate_exp").val());
    var currency = $("#cfc_ddlcurrency_exp").val();
    var bankmargin = ConvertToDouble($(".cfc_bankmargininpaisa_exp").val());
    var cfcAmount = ConvertToDouble($(".cfc_amount_exp").val());
    var interestRate = ConvertToDouble($(".cfc_interestrate_exp").val());
    var deliveryPeriod2 = $(".cfc_delperiod2_exp").val();
    var noofdays = ConvertToInt($(".lblcfc_noofdays_exp").text());
    var isexport = false;

    if ($("#calculator_cancellation_export").attr("isexport") == "true") {
        isexport = true;
    }
    $(".cfc_cashspotrate_export").hide();
    $(".cfc_cashspotrate_import").hide();
    if (isexport == false) {
        $(".cfc_cashspotrate_export" + currency).show();
        cashspot = ConvertToDouble($(".cfc_cashspotrate_export" + currency).text());
    }
    else {
        $(".cfc_cashspotrate_import" + currency).show();
        cashspot = ConvertToDouble($(".cfc_cashspotrate_import" + currency).text());
    }
    $(".lblcfc_forwardrate_exp").text(forwardRate);
    //swap the import and export values
//    if (isexport) {
//        spotrate = ConvertToDouble($(".cfc_spotrateimp" + currency).text());
//    }
//    else {
//        spotrate = ConvertToDouble($(".cfc_spotrateexp" + currency).text());
//    }
    spotrate = ConvertToDouble($("#tdcfc_spotrate").text());
    
    forwardRate = spotrate + premium;
    forwardRate = ConvertToDouble(forwardRate.toFixed(4));
    if (isexport) {
        swapgain = contractRate - forwardRate;
    }
    else {
        swapgain = forwardRate - contractRate;
    }
    
    swapgain = ConvertToDouble(swapgain.toFixed(4));
    $(".lblcfc_forwardrate_exp").text(forwardRate);
    $(".lblcfc_swapgainandloss_exp").text(swapgain);
    //REMOVED AS PER small changes in Finstation 10-12-2016
//    if (isexport) {
//        netswapgain = swapgain + bankmargin / 100.0 - cashspot / 100.0;
//    }
//    else {
//        swapgain = netswapgain = swapgain - bankmargin / 100.0 - cashspot / 100.0;
//    }
    if (isexport) {
        netswapgain = swapgain + bankmargin / 100.0;
    }
    else {
        swapgain = netswapgain = swapgain - bankmargin / 100.0;
    }
    
    netswapgain = ConvertToDouble(netswapgain.toFixed(4));
    $(".lblcfc_netswapgainandloss_exp").text(netswapgain);
    totalProfitLoss = cfcAmount * netswapgain;
    totalProfitLoss = ConvertToDouble(totalProfitLoss.toFixed(4));
    $(".lblcfc_totalprofitandloss_exp").text(totalProfitLoss);

    if (totalProfitLoss < 0) {
        $(".lblcfc_totalprofitandloss_exp").css("background-color", "#ff0000");
    }
    else {
        $(".lblcfc_totalprofitandloss_exp").css("background-color", "#138407");
    }
    interest = cfcAmount * (contractRate - spotrate) * interestRate * noofdays / 365.0;
    interest = interest / 100;
    interest = ConvertToDouble(interest.toFixed(4));
    $(".lblcfc_interest_exp").text(interest);
}
function calcPremium() {
    var isexport = false;

    if ($("#calculator_cancellation_export").attr("isexport") == "true") {
        isexport = true;
    }
    var currency = $("#cfc_ddlcurrency_exp").val();
    var bdate = 0;
    var bdate2 = "";
    if (isexport) {
        bdate = $(".cfc_delperiod2_exp").val();
    }
    else {
        bdate = $(".cfc_delperiod1_exp").val();
        bdate2 = $(".cfc_delperiod2_exp").val()
    }
    if (bdate == "") {
        return;
    }
    var spotDateCss = "jq-" + $("#cfc_ddlcurrency_exp").find("option:selected").text().toLowerCase() + "-spotdate";
    var spotdate = $("." + spotDateCss).text();
    $(".cfc_spotdate").text(spotdate);
    var isexport = $("#calculator_cancellation_export").attr("isexport");
    if (isexport == "true") {
        isexport = false;
    }
    else {
        isexport = true;
    }
    var URL = "brokendatecalc.ashx?action=premium&sdate=" + spotdate + "&bdate=" + bdate + "&ie=" + isexport + "&c=" + currency + "&bdate2=" + bdate2;
    $.ajax({
        url: URL,
        isAsync: true,
        success: function(response) {
            try {
                var arr = response.split(',');
                var premium = ConvertToDouble(arr[0]);
                var noofdays = ConvertToInt(arr[1]);
                $(".lblcfc_noofdays_exp").text(noofdays);
                $(".lblcfc_forwardpremium_exp").text(premium);
            }
            catch (ex) {
                $(".lblcfc_forwardpremium_exp").text("0");
            }
            calcCancellationForwardContract();
        }
    });
}
function calcPremium_earlyutil() {
    var isexport = false;

    if ($("#calculator_earlyutilization_export").attr("isexport") == "true") {
        isexport = true;
    }
    var currency = $("#ceu_ddlcurrency_exp").val();
    var bdate = 0;
    var bdate2 = "";
    if (isexport) {
        bdate = $(".ceu_delperiod2_exp").val();
    }
    else {
        bdate = $(".ceu_delperiod1_exp").val();
    }
    bdate2 = $(".ceu_delperiod1_exp").val()
    if (bdate == "") {
        return;
    }
    var spotDateCss = "jq-" + $("#ceu_ddlcurrency_exp").find("option:selected").text().toLowerCase() + "-spotdate";
    var spotdate = $("." + spotDateCss).text();
    $(".ceu_spotdate").text(spotdate);
    var isexport = $("#calculator_earlyutilization_export").attr("isexport");
    if (isexport == "true") {
        isexport = false;
    }
    else {
        isexport = true;
    }
    var URL = "brokendatecalc.ashx?action=premium&sdate=" + spotdate + "&bdate=" + bdate + "&ie=" + isexport + "&c=" + currency + "&bdate2=" + bdate2;
    $.ajax({
        url: URL,
        isAsync: true,
        success: function(response) {
            try {
                if(!checkApiResponse(response)) return;
                var arr = response.split(',');
                var premium = ConvertToDouble(arr[0]).toFixed(4);
                var noofdays = ConvertToInt(arr[1]);
                $(".lblceu_noofdays_exp").text(noofdays);
                $(".lblceu_forwardpremium_exp").text(premium);
            }
            catch (ex) {
                $(".lblceu_forwardpremium_exp").text("0");
            }
            calcEarlyUtilization();
        }
    });
}
function checkApiResponse(response)
{
    if(response.indexOf("message : ") == 0)
    {
        var msg = response.replace("message : ","");
        alert(msg);
        return false;
    }
    return true;
}
function clearData(div) {
    div.find(".txtcal").val("");
    div.find(".lblcal").text("");
    div.find("select").val("0");
    $(".trcashoutlay_earlyutil").hide();
}
function initFinstationMenu(){
    $(".jq-thirdparty-widget").find("a").remove();
    $("a").click(function(){
        if($(this).closest(".jq-fullfinstation").length > 0 && _isFinstationEnabled == false)
        {
            if($(this).attr("href")=="#" && $(this).attr("class")==undefined)
            {
            }
            else
            {
                window.location.href = "noaccessfortrial.aspx";
            }
        }
    });
}
function initMainLeftMenu()
{
    $(".main-left-menu-panel").css("height",$(document).height());
    $(".main-left-menu-arrow-left").show();
    $(".main-left-menu-panel").mouseover(function(){
        $(".main-left-menu-panel").addClass("main-left-menu-panel-active");
        if($(".main-left-menu-panel").prop("expanded")=="true")
        {
        }
        else
        {
            $(".main-left-menu-arrow-left").show();
        }
    });
    $(".main-left-menu-panel").mouseout(function(e){
        $(this).removeClass("main-left-menu-panel-active");
        //$(".main-left-menu-arrow-left").hide();
    });
    $(".main-left-menu-arrow-left").click(function(){
        $(".main-left-menu-panel").prop("expanded","true");
        $(this).hide();
        $(".main-left-menu-panel").animate({width:"250"},300,function(){
            $(".main-left-text").show();
            $(".main-left-menu-arrow-right").show();
            $(".main-left-smenu").css("left","250px");
        });
        
    });
    $(".main-left-menu-arrow-right").click(function(){
        $(this).hide();
        $(".main-left-text").hide();
        $(".main-left-menu-panel").animate({width:"50"},300,function(){
            $(".main-left-menu-arrow-left").show();
            $(".main-left-menu-panel").prop("expanded","false");
            $(".main-left-smenu").css("left","50px");
        });
    });
    $(".main-left-menu").click(function(){
        var url = $(this).attr("href");
        if(url==undefined)return;
        if($(this).attr("targetblank")=="true")
        {
            window.open(url);
        }
        else
        {
            window.location.href = url;
        }
    });
}
function initUserProfileMenu()
{
    $(".jq-img-userprofile").click(function(){
       var menu = $(".jq-user-profile-menu");
       menu.show(); 
       _outsideClickDiv = menu;
    });
}
function initPushNotification()
{
    $(".jq-header-bell").click(function(){
        bindPushNotification();
        _outsideClickDiv = $(".push-notify-msg-list");
    });
    $(".jq-notification-row").live("click",function(){
        var nid = $(this).attr("nid");
        window.location.href = "view-pushnotification.aspx?id="+nid;
    });
    $(".notification-row-viewall").live("click",function(){
        window.location.href = "view-pushnotification.aspx";
    });
    bindPushNotificationCount();
    setInterval(function(){
        bindPushNotificationCount();
    }, 20000);
}
function bindPushNotificationCount()
{
    var url = "utilities.ashx?m=pushnotification&a=finstation-homemsgcount";
    ajaxCall(url, function(response){
        var count = ConvertToInt(response);
        var panel = $(".jq-push-notify-panel");
        if(count > 0)
        {
            panel.find(".push-notify-msg-count").text(count);
            panel.find(".push-notify-msg-count").show();
            $(".jq-header-bell").addClass("header-bell-active");
        }
        else
        {
            panel.find(".push-notify-msg-count").hide();
            $(".jq-header-bell").removeClass("header-bell-active");
        }
    });
}
function bindPushNotification()
{
    var url = "utilities.ashx?m=pushnotification&a=finstation-topmessages";
    ajaxCall(url, function(response){
        var panel = $(".jq-push-notify-panel");
        var div = $(".push-notify-msg-list");
        panel.find(".push-notify-msg-count").hide();
        div.html(response);
        div.show();
    });
}
function bindForwardAnnualMonthlyPremium(modal)
{
    var currencyId = modal.attr("currencyid");
    var url = "finstationhandler.ashx?m=getannualisedpremium-monthlyvalues&isexport=true&currencyid="+currencyId;
    ajaxCall(url, function(response){
        var arr = response.split('~');
        modal.find(".jq-forwardannualmonthlypermium1").text(arr[0]);
        modal.find(".jq-forwardannualmonthlypermium2").text(arr[1]);
        modal.find(".jq-forwardannualmonthlypermium3").text(arr[2]);
        modal.find(".jq-forwardannualmonthlypermium4").text(arr[3]);
    });
}
function initEEFCCostCalc()
{
    $("#tbleefc-costcalculation").find("input").keyup(function(){
        calculateEEFCCost();
    });   
}
function calculateEEFCCost()
{
    var eefcamount = ConvertToDouble($("#jq-txteefc-eefcamount").val());
    var eefcdays = ConvertToDouble($("#jq-txteefc-days").val());
    var interestratepa = ConvertToDouble($("#jq-txteefc-interestratepa").val());
    var conversionRate = ConvertToDouble($("#jq-txteefc-conversionrate").val());
    var bankcommissionChargesFlat = ConvertToDouble($("#jq-txteefc-bankcommission-charges-flat").val());
    var commissionExchangePer = ConvertToDouble($("#jq-txteefc-commission-exchange-per").val());
    var lbleefcConversionAmount = $("#jq-lbleefc-conversionamount");
    var lbleefcInterestCost = $("#jq-lbleefc-interestcost");
    var lbleefcbankcommissioncharges = $("#jq-lbleefc-bankcommissioncharges");
    var lbleefcConversionAmount = $("#jq-lbleefc-conversionamount");
    var lbleefctotalcostineefc = $("#jq-lbleefc-totalcost-in-eefc");
    var lbleefccostpaisa = $("#jq-lbleefc-costpaisa");
    var lbleefcnetratetocovercost = $("#jq-lbleefc-netratetocovercost");
    
    var conversionAmount = ConvertToDouble(parseFloat(eefcamount * conversionRate).toFixed(2));
    var interestCost = ConvertToDouble(parseFloat(conversionAmount * interestratepa / 100.0 * eefcdays / 365.0).toFixed(2));
    lbleefcConversionAmount.text(conversionAmount);
    lbleefcInterestCost.text(interestCost);
    var bankCommissionChargesLieuOfExchange = conversionAmount * commissionExchangePer / 100.0
    lbleefcbankcommissioncharges.text(bankCommissionChargesLieuOfExchange); 
    var totalcosttokeepineefc = interestCost + bankcommissionChargesFlat + bankCommissionChargesLieuOfExchange;
    lbleefctotalcostineefc.text(totalcosttokeepineefc);
    var costintermsofpaisa = ConvertToDouble(parseFloat(totalcosttokeepineefc / eefcamount).toFixed(4));
    lbleefccostpaisa.text(costintermsofpaisa);
    var eefcnetratetocovercost = ConvertToDouble(conversionRate + costintermsofpaisa).toFixed(2);
    lbleefcnetratetocovercost.text(eefcnetratetocovercost);
    
    var conversionMarginExportPaisa = ConvertToDouble($("#jq-txteefc-conversion-margin-export-paisa").val());
    var cashspotExportPaisa = ConvertToDouble($("#jq-txteefc-cashspot-export-paisa").val());
    var cashconversionImportPaisa = ConvertToDouble($("#jq-txteefc-cashconversion-import-paisa").val());
    var bidaskDiffImportPaisa = ConvertToDouble($("#jq-txteefc-bidask-diff-import-paisa").val());
    var importPremiumPaisa = ConvertToDouble($("#jq-txteefc-import-premium-paisa").val());
    
    var lbleefcconversionmarginexportpaisa = $("#jq-lbleefc-conversion-margin-export-paisa");
    eefcconversionmarginexportpaisa = ConvertToDouble(parseFloat(eefcamount * conversionMarginExportPaisa / 100.0).toFixed(2));
    lbleefcconversionmarginexportpaisa.text(eefcconversionmarginexportpaisa);
    
    var lbleefccashspotexportpaisa = $("#jq-lbleefc-cashspot-export-paisa");
    eefccashspotexportpaisa = ConvertToDouble(parseFloat(eefcamount * cashspotExportPaisa / 100.0).toFixed(2));
    lbleefccashspotexportpaisa.text(eefccashspotexportpaisa);
    
    var lbleefccashconversionimportpaisa = $("#jq-lbleefc-cashconversion-import-paisa");
    eefccashconversionimportpaisa = ConvertToDouble(parseFloat(eefcamount * cashconversionImportPaisa / 100.0).toFixed(2));
    lbleefccashconversionimportpaisa.text(eefccashconversionimportpaisa);
    
    var lbleefcbidaskdiffimportpaisa = $("#jq-lbleefc-bidask-diff-import-paisa");
    eefcbidaskdiffimportpaisa = ConvertToDouble(parseFloat(eefcamount * bidaskDiffImportPaisa / 100.0).toFixed(2));
    lbleefcbidaskdiffimportpaisa.text(eefcbidaskdiffimportpaisa);
    
    var lbleefcimportpremiumpaisa = $("#jq-lbleefc-import-premium-paisa");
    eefcimportpremiumpaisa = ConvertToDouble(parseFloat(eefcamount * importPremiumPaisa / 100.0).toFixed(2));
    lbleefcimportpremiumpaisa.text(eefcimportpremiumpaisa);
    
    var lbleefctotalcost = $("#jq-lbleefc-totalcost");
    var totalcost = ConvertToDouble(parseFloat(eefcconversionmarginexportpaisa + eefccashspotexportpaisa + eefccashconversionimportpaisa + eefcbidaskdiffimportpaisa + eefcimportpremiumpaisa).toFixed(2));
    lbleefctotalcost.text(totalcost);
    
    var lbleefcsavingcost = $("#jq-lbleefc-savingcost");
    eefcsavingcost = totalcost - totalcosttokeepineefc;
    lbleefcsavingcost.text(eefcsavingcost);
}