var _lastliverateId = 0;
var _isCommodityLiveChart = true;
var _commodityLiveChart = null;
var _commodityLiverateDateRange = 0;
$(document).ready(function() {
    initTab();
    initCommodityTab();
    initLMESettlementMore();
});
function setMenu(menu) {
    var isactive = false;
    $(".i-mainmenu a").each(function() {
        if (menu == $(this).attr("m")) {
            $(this).parent().addClass("active-menu");
            isactive = true;
        }
    });
    if(!isactive) $(".menu-home").addClass("active-menu");
}
function initTab() {
    $(".tabs div").click(function() {
        var target = $(this).text().replace(/ /g, "").toLowerCase();
        $(".jq-tab").hide();
        $(".tabs div").removeClass("tab-active");
        $("#" + target).show();
        $(this).addClass("tab-active");
        _isCommodityLiveChart = false;
    });
}
function initCommodityTab() {
    $(".jq-tradesumma-tab").click(function() {
        var url = "commodity.ashx?m=tradingsummary&id=" + $(".jq-metalid").val();
        ajaxCall(url, function(response) {
            $(".jq-tradingsummary").html(response);
            initChartJs();
        });
    });
    $(".jq-ctyearsummary-tab").click(function() {
        bindCurrentYearSummary(true);
    });
    $(".jq-btncommodity-ctyearsummary").live("click", function() {
        bindCurrentYearSummary(true);
    });
    $(".jq-btnpricegraph").click(function() {
        var from = $(".jq-pricegraph-date-from").val();
        var to = $(".jq-pricegraph-date-to").val();
        var type = $(".jq-pricegraph-contracttype").val();
        var url = "commodity.ashx?m=bindpricegraph&id=" + $(".jq-metalid").val() + "&from=" + from + "&to=" + to + "&type=" + type;
        ajaxCall(url, function(response) {
            $(".jq-pricegraph").html(response);
            initChartJs();
        });
    });
    $(".jq-btnmonthlysummary").click(function() {
        var url = "commodity.ashx?m=bindmonthsummary&id=" + $(".jq-metalid").val() + "&month=" + $(".ddlmonth").val();
        ajaxCall(url, function(response) {
            $(".jq-monthlysummary").html(response);
            initChartJs();
        });
    });
    $(".jq-ddlliveratedate").change(function() {
        if ($(this).val() == 7) {
            $(".jq-liveratedate").show();
        }
        else {
            $(".jq-liveratedate").hide();
        }
    });
    $(".jq-btnliveratechart").click(function() {
        _commodityLiverateDateRange = $(".jq-ddlliveratedate").val();
        if ($(".jq-ddlliveratedate").val() == 0) {
            $(".jq-commodity-live-chart-panel").show();
            $(".jq-commodity-daily-chart-panel").hide();
            bindCommodityLiveChart();
        }
        else {
            $(".jq-commodity-live-chart-panel").hide();
            $(".jq-commodity-daily-chart-panel").show();
            $(".jq-commodity-daily-chart-panel").html("<div class='jq-commodity-daily-chart db-chartjs-panel' ct='3' xaxislabel='Bid' data='' labels='' pointradius='0'></div>");
            bindCommodityDailyChart();
        }
    });
}
function bindCurrentYearSummary(isCurrentYear) {
    var url = "commodity.ashx?m=currentyearsummary&id=" + $(".jq-metalid").val();
    if (isCurrentYear && $(".jq-txtcommodity-ctyearsummary").val()!=undefined) url += "&dt=" + $(".jq-txtcommodity-ctyearsummary").val();
    ajaxCall(url, function(response) {
        if (response.indexOf("Error:") >= 0) {
            alert(response.replace("Error:", ""));
            return;
        }
        if (isCurrentYear) {
            $(".jq-currentyearsummary").html(response);
            loadDatePicker();
        }
        else {
            $(".jq-tradingsummary").html(response);
        }
        initChartJs();
    });
}
function commodityLiverate(metalId) {
    if(_commodityLiverateDateRange != 0)
    {
        setTimeout("commodityLiverate(" + metalId + ")", 1000);
        return;
    }
    var URL = "commodity.ashx?m=dashboardticker&mid=" + metalId;
    $.ajax({
        url: URL,
        type: 'GET',
        async: true,
        cache: false,
        success: function(json) {
            try {
                if (json == "Session Expired") {
                    window.location = "customerlogin.aspx";
                }
                if (json != "") {
                    var data = jQuery.parseJSON(json);
                    $.each(data, function(key, value) {
                        var mid = value.mid;
                        $.each(value, function(key, value) {
                            if (key == "date") {
                                value = convertToDateTime(value);
                            }
                            var lbl = $(".comm-liverate[col=" + key + "][metalid=" + mid + "]");
                            if (lbl.length > 0) {
                                setUpDownStatus(lbl, value);
                            }
                        });
                    });
                }
            } catch (e) { }
        },
        complete: function() {
            setTimeout("commodityLiverate(" + metalId + ")", 1000);
        }
    });
}
function convertToDateTime(value) {
    var arr = value.split('/');
    var d = arr[1];
    var m = arr[0];
    if (d.length == 1) d = "0" + d;
    if (m.length == 1) m = "0" + m;
    value = d + '-' + m + '-' + arr[2];
    return value;
}
function setUpDownStatus(obj, rate) {
    if (!isNaN(rate)) {
        rate = parseFloat(rate);
        var prevrate = ConvertToDouble(obj.text());
        obj.attr("prate", prevrate);
        if (rate > prevrate) {
            obj.removeClass("rate-down").addClass("rate-mid");
            setTimeout(function() {
                obj.removeClass("rate-mid").addClass("rate-up");
            }, 100);
        }
        else if (rate < prevrate) {
            obj.removeClass("rate-up").addClass("rate-mid");
            setTimeout(function() {
                obj.removeClass("rate-mid").addClass("rate-down");
            }, 100);
        }
    }
    obj.text(rate);

}
function bindCommodityLiveChart() {
    if (!_isCommodityLiveChart) {
        return;
    }
    
    var url = "commodity.ashx?m=getcommodityliveratechart&id=" + $(".jq-metalid").val() + "&dateid=0";
    url += "&sd=" + $(".jq-txtliveratestartdate").val();
    url += "&ed=" + $(".jq-txtliverateenddate").val();
    url += "&lastid=" + _lastliverateId;
    $.ajax({
        url: url,
        async: true,
        cache: false,
        success: function(response) {
            if (response.indexOf("Error:") >= 0) {
                alert(response.replace("Error:", ""));
                return;
            }
            var data = jQuery.parseJSON(response);
            if (_commodityLiveChart == null) {
                $(".jq-commodity-live-chart").attr("data", data.vals);
                $(".jq-commodity-live-chart").attr("labels", data.lbls);
                $(".jq-commodity-live-chart").addClass("db-chartjs-panel");
                initChartJs();
                _commodityLiveChart = _currentChart;
                _lastliverateId = data.lastid;
            }
            else {
                if (data.vals != "") {
                    var arrdata = data.vals.split(',');
                    var arrlbls = data.lbls.split(',');
                    for (var i = 0; i < arrdata.length; i++) {
                        _commodityLiveChart.data.datasets[0].data.push(arrdata[i]);
                    }
                    _commodityLiveChart.data.labels.push(arrlbls);
                    _commodityLiveChart.update();
                    _lastliverateId = data.lastid;
                    _commodityLiveChart.draw();
                }
            }
        },
        complete: function() {
            setTimeout("bindCommodityLiveChart()", 30000);
        }
    });
}
function bindCommodityDailyChart() {
    var dateRange = $(".jq-ddlliveratedate").val();
    var url = "commodity.ashx?m=getcommodityliveratechart&id=" + $(".jq-metalid").val() + "&dateid=" + dateRange;
    url += "&sd=" + $(".jq-txtliveratestartdate").val();
    url += "&ed=" + $(".jq-txtliverateenddate").val();
    $.ajax({
        url: url,
        async: true,
        cache: false,
        success: function(response) {
            if (response.indexOf("Error:") >= 0) {
                alert(response.replace("Error:", ""));
                return;
            }
            var data = jQuery.parseJSON(response);
            $(".jq-commodity-daily-chart").attr("data", data.vals);
            $(".jq-commodity-daily-chart").attr("labels", data.lbls);
            initChartJs();
        }
    });
}
function initLMESettlementMore() {
    $(".jq-lme-settlement-expand").live("click",function() {
        if ($(this).attr("src") == "images/arrow_expand_right.png") {
            $(this).attr("src", "images/arrow_expand_left.png");
            $(this).closest(".repeater").find(".jq-lme-settlement-more").show();
        }
        else {
            $(this).attr("src", "images/arrow_expand_right.png");
            $(this).closest(".repeater").find(".jq-lme-settlement-more").hide();
        }
    });
}
function getCurrentHour(){
    var d = new Date();
    return d.getHours();
}
function initCommodityMetalHomePage(){
    commodityLiverate(0);
    bindLMEMetalSettlementRates();
    bindLMEMetalStockRates();
}
function bindLMEMetalSettlementRates(){
    var h = getCurrentHour();
    if(h < 16 || h > 19) 
    {
        setTimeout("bindLMEMetalSettlementRates()", 300000);//5mins
        return;
    }
    var url = "commodity.ashx?m=getlmemetalsettlementrates&ctdate="+$(".jq-lmesettlement-date").text();
    $.ajax({
        url:url,
        success:function(response){
            if(response!="")
            {
                $(".jq-lmesettlementrates").html(response);
            }
        },
        complete: function() {
            setTimeout("bindLMEMetalSettlementRates()", 300000);
        }
    });
}
function bindLMEMetalStockRates(){
    var h = getCurrentHour();
    if(h < 12 || h > 15) 
    {
        setTimeout("bindLMEMetalStockRates()", 300000);
        return;
    }
    var url = "commodity.ashx?m=getlmestockrates&ctdate="+$(".jq-lmestock-date").text();
    $.ajax({
        url:url,
        success:function(response){
            if(response!="")
            {
                $(".jq-lmestockrates").html(response);
            }
        },
        complete: function() {
            setTimeout("bindLMEMetalStockRates()", 300000);
        }
    });
}