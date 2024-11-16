var _chart_data = null;
var _chart_lbls = null;
var _chart_index = 0;
var _chart_type = "pie";
var _chart_accs = "";
var _chart_ct_data = null;
var _jsChartColorNames = Object.keys(window.chartColors);
var _jsChartColor = Chart.helpers.color;
var jschartCount = 0;
var _isMobile = false;
var _currentChart = null;
$(document).ready(function(){
    initChartJs();
});
function initChartJs() {
    $(".db-chartjs-panel").each(function () {
        var cid = "chartjs_" + jschartCount;
        var ctype = $(this).attr("ct");
        var canvas = null;
        if(ctype=="2" || ctype=="5" || ctype=="9")//pie
        {
            var width = "60%";
            var w = "100%";
            if(_isMobile)
            {
                width = "50%";
                w = ConvertToInt($(window).width()*0.5) + "px";
            }
            canvas = $("<table width='100%'><tr><td width='"+width+"'><canvas id='" + cid + "' style='width:"+w+";'/></td><td class='jq-chartjs-labels'></td></tr></table>");    
        }
        else
        {
            var w = "100%";
            if(_isMobile)
            {
                w = $(window).width() + "px;height:300px;";
            }
            canvas = $("<canvas id='" + cid + "' style='width:"+w+";'/>");    
        }
        $(this).append(canvas);
        loadJsChart($(this));
        jschartCount++;
    });
    function loadJsChart(obj, labelDiv) {
        //var arrdata = obj.attr("data").split(",");
        var arrlbl = obj.attr("labels").split(",");
        var canvas = obj.find("canvas");
        var ctype = obj.attr("ct");
        if(ctype=="0")ctype="1";
        var xaxislabel = obj.attr("xaxislabel");
        var arrxaxis = xaxislabel.split(',');
        var ccolors = [];
        var chartType = ctype;
        var gridcolor = "";
        if(chartType=="1" || chartType=="6" || chartType=="8")
        {
            chartType="bar";
        }
        else if(chartType=="2")
        {
            chartType="pie";
        }
        else if(chartType=="3" || chartType=="7")
        {
            chartType="line";
        }
        else if(chartType=="9" || chartType=="5")
        {
            chartType="doughnut";
        }
        if(chartType=="bar-stack")chartType="bar";
        if (obj.attr("colors") != undefined) ccolors = obj.attr("colors").split(',');
        if (obj.attr("gridcolor") != undefined) gridcolor = obj.attr("gridcolor");
        var isShowLabel = true;
        if (chartType == "pie" || chartType == "doughnut") {
            isShowLabel = false;
        }
        var config = {
            type: chartType,
           
            data: {
                datasets: [],
                labels: arrlbl
            },
            backgroundColor: [],
            options: {
                legend: { display: isShowLabel },
                scales : (ctype == "bar-stack" || ctype == "5")? {
                            xAxes: [{stacked: true,display:false}],
                            yAxes: [{stacked: true,display:false}]
                          } : 
                    {
                    xAxes:[
                        {
                            display: true,
                            gridLines: {
                              color: gridcolor == "" ? "#eee" : gridcolor,
                            },
                        }
                    ],
                   yAxes:[
                        {
                            display: true,
                            gridLines: {
                              color: gridcolor == "" ? "#eee" : gridcolor,
                            },
                        }
                    ]
                }
            }
        };
        if (chartType == "pie" || chartType == "doughnut") {
            var arrdata = obj.attr("data").split(",");
            var data = {
                data: arrdata,
                label: arrxaxis[0],
                backgroundColor: []
            };
            config.data.datasets.push(data);
            var divLabels = obj.find(".jq-chartjs-labels");
            var labelHtml = "<table cellspacing='10'>";
            for (var index = 0; index < config.data.labels.length; ++index) {
                var colorName = _jsChartColorNames[index % _jsChartColorNames.length];
                var newColor = window.chartColors[colorName];
                config.data.datasets[0].backgroundColor.push(newColor);
                labelHtml+="<tr><td><div class='chart-lbl-color' style='background-color:"+newColor+";'></div></td><td>"+config.data.labels[index]+" ("+arrdata[index]+")</td></tr>";
            }
            labelHtml+="</table>";
            divLabels.html(labelHtml);
        }
        else if (chartType == "line") {
            var arrdatas = obj.attr("data").split("|");
            var pointRadius = 5;
            if(obj.attr("pointradius")!=undefined)
            {
                pointRadius = obj.attr("pointradius");
            }
            var lineradius = 2;
            if(_isMobile) 
            {
                pointRadius = 5;
            }
            for (var i = 0; i < arrdatas.length; i++) {
                var arrdata = arrdatas[i].split(',');
                var colorName = _jsChartColorNames[i % _jsChartColorNames.length];
                if (ccolors.length > 0) colorName = ccolors[i];
                var newColor = window.chartColors[colorName];
                var isfilled = false;
                //if(i==arrdatas.length-1)isfilled = true;
                var bgColor = _jsChartColor(newColor).alpha(0.5).rgbString();
                var data = {
                    data: arrdata,
                    label: arrxaxis[i],
                    borderColor: newColor,
                    backgroundColor : bgColor,
                    fill: isfilled,
                    //borderDash: [lineradius, lineradius],
                    pointRadius: pointRadius,
                    pointHoverRadius: 10,
                    borderWidth:1,
                    tension:0,
                    //showLine: false
                };
                config.data.datasets.push(data);
            }
        }
        else if (chartType == "bar") {
            var arrdatas = obj.attr("data").split("|");
            //var arrlabels = obj.attr("labels").split("|");
            for (var i = 0; i < arrdatas.length; i++) {
                var arrdata = arrdatas[i].split(',');
                //var arrlabel = arrlabels[i].split(',');
                var colorName = _jsChartColorNames[i % _jsChartColorNames.length];
                if (ccolors.length > 0) colorName = ccolors[i];
                var newColor = window.chartColors[colorName];
                var bgColor = _jsChartColor(newColor).alpha(0.5).rgbString();
                var data = {
                    data: arrdata,
                    label: arrxaxis[i],
                    borderColor: newColor,
                    backgroundColor: bgColor,
                    borderWidth: 1
                };
                config.data.datasets.push(data);
            }
        }
        else if (ctype == "bar-stack") {
            var arrdatas = obj.attr("data").split("|");
            for (var i = 0; i < arrdatas.length; i++) {
                var arrdata = arrdatas[i].split(',');
                var colorName = _jsChartColorNames[i % _jsChartColorNames.length];
                if (ccolors.length > 0) colorName = ccolors[i];
                var newColor = window.chartColors[colorName];
                var bgColor = _jsChartColor(newColor).alpha(0.5).rgbString();
                var data = {
                    data: arrdata,
                    label: arrxaxis[i],
                    borderColor: newColor,
                    backgroundColor: bgColor,
                    borderWidth: 1
                };
                config.data.datasets.push(data);
                
            }
        }
        var ctx = document.getElementById(canvas.attr("id")).getContext("2d");
        _currentChart = new Chart(ctx, config);
    }
}