var _lastSpotLiverateId = 0;
var _isSpotRateLiveChart = true;
var _spotRateLiveChart = null;
var _currencyId = 2;
$(document).ready(function() {
    initSpotRateChart();
    $(".jq-faq").click(function() {
        $(this).closest(".faq-row").find(".faq-ans").toggle(200);
        $(this).closest(".faq-row").find(".faq-arr").toggleClass("faq-arr-down faq-arr-up");
    });
});
function initSpotRateChart() {
    $(".tblspotrate .repeater-header-left").attr("title", "Click here to view the history");
    $(".tblspotrate .repeater-header-left").click(function() {
        var c = $(this).text();
        _currencyId = 0;
        if (c == "USDINR") {
            _currencyId = 2;
        }
        else if (c == "EURINR") {
            _currencyId = 3;
        }
        else if (c == "GBPINR") {
            _currencyId = 4;
        }
        else if (c == "JPYINR") {
            _currencyId = 5;
        }
        if (_currencyId == 0) return;
        $("#tblDailyHistoricalChart").dialog({ width: 800, height: 600 });
        $("#ui-dialog-title-tblDailyHistoricalChart").text("Daily Historical Data - " + c);
        $(".jq-liveratedate-spotrate").hide();
        $(".jq-spotrate-live-chart-panel").show();
        $(".jq-spotrate-daily-chart-panel").hide();
        $(".jq-txtliveratestartdate-spotrate").val("");
        $(".jq-txtliverateenddate-spotrate").val("");
        $(".jq-ddlliveratedate-spotrate").val("0");
        $(".jq-spotrate-daily-chart-panel").html("");
        $(".jq-spotrate-live-chartpanel").html("");
        $(".jq-spotrate-live-chartpanel").html("<div class='jq-spotrate-live-chart' ct='3' gridcolor='#222' colors='yellow' xaxislabel='Close' data='' labels='' pointradius='0'></div>");
        _spotRateLiveChart = null;
        _lastSpotLiverateId = 0;
        bindSpotRateLiveChart();
    });
    $(".jq-ddlliveratedate-spotrate").change(function() {
        if ($(this).val() == 7) {
            $(".jq-liveratedate-spotrate").show();
        }
        else {
            $(".jq-liveratedate-spotrate").hide();
        }
    });
    $(".jq-btnliveratechart-spotrate").click(function() {
        $(".jq-spotrate-daily-chart-panel").html("");
        $(".jq-spotrate-live-chartpanel").html("");
        if ($(".jq-ddlliveratedate-spotrate").val() == 0) {
            $(".jq-spotrate-live-chart-panel").show();
            $(".jq-spotrate-daily-chart-panel").hide();
            _lastSpotLiverateId = 0;
            _spotRateLiveChart = null;
            $(".jq-spotrate-live-chartpanel").html("<div class='jq-spotrate-live-chart' ct='3' gridcolor='#222' colors='yellow' xaxislabel='Close' data='' labels='' pointradius='0'></div>");
            bindSpotRateLiveChart();
        }
        else {
            $(".jq-spotrate-live-chart-panel").hide();
            $(".jq-spotrate-daily-chart-panel").show();
            $(".jq-spotrate-daily-chart-panel").html("<div class='jq-spotrate-daily-chart db-chartjs-panel' gridcolor='#222' colors='yellow' ct='3' xaxislabel='Close' data='' labels='' pointradius='0'></div>");
            bindSpotRateDailyChart();
        }
    });
}
function bindSpotRateLiveChart() {
    if (!_isSpotRateLiveChart) {
        return;
    }
    if(!$("#tblDailyHistoricalChart").is(":visible")) return;
    var url = "finstationhandler.ashx?m=getspotratelivechart&cid=" + _currencyId + "&dateid=0";
    url += "&lastid=" + _lastSpotLiverateId;
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
            if (_spotRateLiveChart == null) {
                $(".jq-spotrate-live-chart").attr("data", data.vals);
                $(".jq-spotrate-live-chart").attr("labels", data.lbls);
                $(".jq-spotrate-live-chart").addClass("db-chartjs-panel");
                initChartJs();
                _spotRateLiveChart = _currentChart;
                _lastSpotLiverateId = data.lastid;
            }
            else {
                if (data.vals != "") {
                    var arrdata = data.vals.split(',');
                    var arrlbls = data.lbls.split(',');
                    for (var i = 0; i < arrdata.length; i++) {
                        _spotRateLiveChart.data.datasets[0].data.push(arrdata[i]);
                    }
                    _spotRateLiveChart.data.labels.push(arrlbls);
                    _spotRateLiveChart.update();
                    _lastSpotLiverateId = data.lastid;
                    _spotRateLiveChart.draw();
                }
            }
        },
        complete: function() {
            setTimeout("bindSpotRateLiveChart()", 5000);
        }
    });
}
function bindSpotRateDailyChart() {
    var dateRange = $(".jq-ddlliveratedate-spotrate").val();
    var url = "finstationhandler.ashx?m=getspotratelivechart&cid=" + _currencyId + "&dateid=" + dateRange;
    url += "&sd=" + $(".jq-txtliveratestartdate-spotrate").val();
    url += "&ed=" + $(".jq-txtliverateenddate-spotrate").val();
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
            $(".jq-spotrate-daily-chart").attr("data", data.vals);
            $(".jq-spotrate-daily-chart").attr("labels", data.lbls);
            initChartJs();
        }
    });
}