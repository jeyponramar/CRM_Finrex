using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using WebComponent;
using System.Data;
using System.Collections;

public partial class BarChart : System.Web.UI.UserControl
{
    private bool _IsColumnChart = true;
    private string _XAxisColumns = "";
    private string _YAxisColumns = "";
    private string _barColor = "";
    private string _Width = "600";
    private string _height = "480";
    private string _ColumnName = "";
    private string _ColumnHeader = "";
    private DataTable _Data = null;
    private Array arrColumns;
    private Array arrColumnsHeader;
    private string _Title = "";
    private bool _isAnimatedChart = false;
    private bool _isFirstColAuto = false;
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    public string Width
    {
        get
        {
            return _Width;
        }
        set
        {
            _Width = value;
        }

    }
     public string Height
    {
        get
        {
            return _height;
        }
        set
        {
            _height = value;
        }

    }
    public string BarColor
    {
        set
        {
            _barColor = value;
        }
        get
        {
            return _barColor;
        }
    }
    public string Title
    {
        set
        {
            _Title = value;
        }
        get
        {
            return _Title;
        }
    }
    public DataTable data
    {
        set
        {
            _Data = value;
        }
        get
        {
            return _Data;
        }
    }
    public string ColumnName
    {
        set
        {
            _ColumnName = value;
        }
        get
        {
            return _ColumnName;
        }
    }
    public string ColumnHeader
    {
        set
        {
            _ColumnHeader = value;
        }
        get
        {
            return _ColumnHeader;
        }
    }
    public bool IsColumnChart
    {
        set
        {
            _IsColumnChart = value;
        }
        get
        {
            return _IsColumnChart;
        }
    }
    public string XAxisColumns
    {
        set
        {
            _XAxisColumns = value;
        }
        get
        {
            return _XAxisColumns;
        }
    }
    public string YAxisColumns
    {
        set
        {
            _YAxisColumns = value;
        }
        get
        {
            return _YAxisColumns;
        }
    }
    public string Module
    {
        get
        {
            return Convert.ToString(ViewState["PC_Module"]);
        }
        set
        {
            ViewState["PC_Module"] = value;
        }
    }    
    private string getArrayValueScript(Array arr)
    {
        string val = "";
        val="[";
        for (int i = 0; i < arr.Length; i++)
        {
            string colval = "'" + arr.GetValue(i).ToString() + "'" + Convert.ToString((i == arr.Length - 1) ? "" : ",");
            val += colval;
        }
        val += "]";
        return val;
    }
    private string getColumnDataScript(DataRow dr, Array arr)
    {
        string _val = "";
        _val = "[";        
        if (dr != null)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                string columnname = arr.GetValue(i).ToString();
                string colval = GlobalUtilities.ConvertToString(dr[columnname]);
                try
                {
                    colval =GlobalUtilities.FormatAmount(GlobalUtilities.ConvertToDouble(colval));
                    colval = "" + colval + "" + Convert.ToString((i == arr.Length - 1) ? "" : ",");
                }
                catch
                {
                    colval = "'" + colval + "'" + Convert.ToString((i == arr.Length - 1) ? "" : ",");
                }
                
                _val += colval;
            }            
        }
        _val += "]";
        return _val;
    }
    private string getColumnDataScript(DataRow dr)
    {
        return getColumnDataScript(dr, arrColumns);
    }
    private string getoptionsScript()
    {
        string AnimatedScript = "";
        string script = "";
        if (_isAnimatedChart)
        {
            AnimatedScript = @" ,legend: 'bottom',
                                hAxis: {
                                title:''
                            },
                                animation: {
                                    duration: 1000
                                },
                                vAxis: {
                                // set these values to make the initial animation smoother
                                minValue: 0,
                                maxValue: 10
                            }  ";
        }

        script = @"var options = {
                                width: " + _Width + ", height: " + _height + ", is3D: true," +
                            "   title: '" + Title + "'$animatedScript$};" + Environment.NewLine;
        return script.Replace("$animatedScript$", AnimatedScript);

    }
    private string getViewScriptForInitialDraw(Array arr)
    {
        string view = "view.setColumns([0, ";
        for (int i = 1; i < arr.Length; i++)
        {
            string scriptView = @"
            {
                type: 'number',
                label: data.getColumnLabel(" + (i) + ")," +
                "calc: function() { return 0; }" +
            "}" + Convert.ToString((i == arr.Length - 1) ? "" : ",");

            view += scriptView;
        }
        view += "]);"+Environment.NewLine;
        return view;
    }
    private void setAllInput()
    {
        try
        {
            string query = "select * from tbl_report where report_reportname='" + Module + "'";
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (_Data == null)
            {
                query = GlobalUtilities.ConvertToString(dr["report_jointables"]);
                data = DbTable.ExecuteSelect(query);
            }
            if (_ColumnName == "")
            {
                ColumnName = GlobalUtilities.ConvertToString(dr["report_columnname"]);
            }
            if (_ColumnHeader == "")
            {
                ColumnHeader = GlobalUtilities.ConvertToString(dr["report_columnheader"]);
            }
            if (_Title == "")
            {
                Title = GlobalUtilities.ConvertToString(dr["report_reportname"]);
            }
        }
        catch (Exception ex)
        {
        }

    }
    public void BindChart()
    {
        try
        {
            setAllInput();
            Array arrBarColor =_barColor.Split(',');
            arrColumns = _ColumnName.Split(',');
            arrColumnsHeader = _ColumnHeader.Split(',');
            int ColumnCount = arrColumns.Length;
            StringBuilder html = new StringBuilder();
            StringBuilder Script = new StringBuilder();
            string chartId = "chart_" + Guid.NewGuid().ToString();
            html.Append("<div id='" + chartId + "' style='margin:0px'></div>");
            Script.Append("google.load('visualization', '1', { packages: ['corechart'], callback: drawChart });" + Environment.NewLine);
            //Script.Append(" google.setOnLoadCallback(drawChart);" + Environment.NewLine);

            Script.Append(@"function drawChart() {" + Environment.NewLine +
            @" var data = google.visualization.arrayToDataTable([" +
                        getArrayValueScript(arrColumnsHeader).Replace("]", (_barColor != "") ? ",{ role: 'style' }]" : "]"));
            Script.Append(",");
            string Columndata = "";
            for (int i = 0; i < _Data.Rows.Count; i++)
            {
                string barcolor ="";
                try
                {
                barcolor= GlobalUtilities.ConvertToString(arrBarColor.GetValue(i));
                }catch{}
                string scriptdata = getColumnDataScript((DataRow)_Data.Rows[i]).Replace("]", (barcolor != "") ? ",'"+barcolor+"']" : "]");
                Columndata += (Columndata == "") ? scriptdata : "," + scriptdata;
            }
            Columndata += "]);" + Environment.NewLine;
            Script.Append(Columndata);

            Script.Append(@"var chart = new google.visualization." + ((_IsColumnChart) ? "ColumnChart" : "BarChart") + "(document.getElementById('" + chartId + "'));" + Environment.NewLine);
            Script.Append(getoptionsScript() + Environment.NewLine);
            if (_isAnimatedChart)
            {
                Script.Append(@"var view = new google.visualization.DataView(data);" + Environment.NewLine);
                Script.Append(getViewScriptForInitialDraw(arrColumns) + Environment.NewLine);

                Script.Append(@" var runOnce = google.visualization.events.addListener(chart, 'ready', function() {
                            google.visualization.events.removeListener(runOnce);
                            chart.draw(data, options);
                        });" + Environment.NewLine +
                            " chart.draw(view, options);" + Environment.NewLine);
            }
            else
            {
                Script.Append("chart.draw(data, options);" + Environment.NewLine);
                // Script.Append("chart.draw(view, options);"+Environment.NewLine);
            }
            Script.Append("}");

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Graph", "<script>" + Script + "</script>");
            ltBarChart.Text = html.ToString();
        }
        catch
        {
        }
    }
    public static string GetBarChartScript(ArrayList arrData, int NoofColumns, int index, int height, int width, string title, string DivId)
    {
        StringBuilder script = new StringBuilder();
        script.Append("google.setOnLoadCallback(drawChart" + index + ");" + Environment.NewLine);
        script.Append("function drawChart" + index + "() {" + Environment.NewLine);
        script.Append("var data = google.visualization.arrayToDataTable([" + Environment.NewLine);
        int colCount = 0;

        for (int j = 0; j < arrData.Count; j++)
        {

            if (j % NoofColumns == 0)
            {
                script.Append("[");
            }
            if (colCount == 0)
            {
                script.Append("'" + arrData[j] + "'");
            }
            else
            {
                if (j <= 2)
                {
                    script.Append(",'" + arrData[j] + "'");
                }
                else
                {
                    script.Append("," + arrData[j]);
                }
            }
            if (j % NoofColumns == NoofColumns - 1)
            {
                script.Append("]");
            }
            colCount++;
            if (colCount >= NoofColumns)
            {
                script.Append(",");
                colCount = 0;
            }
        }
        if (!script.ToString().EndsWith("]"))
        {
            script.Append("]");
        }
        script.Append(")" + Environment.NewLine);
        if (DivId == "")
            DivId = "chart_div" + index;
        script.Append(" var chart = new google.visualization.ColumnChart(document.getElementById('" + DivId + "'));" + Environment.NewLine);
        script.Append("chart.draw(data, { width: " + width + ", height: " + height + ", is3D: true, title: '" + title + " ' });" + Environment.NewLine);
        script.Append("}" + Environment.NewLine);
        return script.ToString();
    }

    public static string GetBarChartScript(ArrayList arrData, int NoofColumns, int index, int height, int width, string title)
    {
        return GetBarChartScript(arrData, NoofColumns, index, height, width, title, "");
    }
}
