using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WebComponent;
using System.IO;
using System.Data.OleDb;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

/// <summary>
/// Summary description for Common
/// </summary>
public static class ChartScripter
{

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
