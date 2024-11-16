function graph(options) {
    //this.xInterval = options.xInterval;
    //this.yInterval = options.yInterval;
    this.xValues = options.xValues;
    this.yValues = options.yValues;
    this.values = options.values;
    this.width = options.width;
    this.height = options.height;
    this.targetDiv = options.targetDiv;
    this.draw = draw;
    
    function draw() {
        var arrx = this.xValues.split(',');
        var arry = this.yValues.split(',');
        var arrvalues = this.values.split(',');

        var maxX = arrx[arrx.length - 1];
        var maxY = arry[arry.length - 1];

        var html = "";
        html = "<div style='position:relative;width:" + this.width + "px;height:" + this.height + "px;border-left:solid 1px;border-bottom:solid 1px;'>";
        var left = 0;
        var top = 0;
        for (i = 0; i < arrx.length; i++) {
            var val = parseInt(arrvalues[i]);
            top = this.height - val;
            html += "<div style='width:30px;top:"+top+"px;left:"+left+"px;height:"+val+"px;position:absolute;background-color:red;float:left;'>";
            html += "&nbsp;";
            html += "</div>";
            left += 50;
        }
        html += "</div>";
        this.targetDiv.append(html);
    }
}