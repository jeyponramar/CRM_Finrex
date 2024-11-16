using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using WebComponent;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

/// <summary>
/// Summary description for StatusManager
/// </summary>
public static class StatusManager
{
    public static void BindStatusSummary(Literal ltSummary, string moduleName)
    {

        string query = "select count(*) as status_count,min(" + moduleName + "status_" + moduleName + "status) as module_status," +
                        "min("+moduleName + "status_backgroundcolor) as bgcolor,min(" + moduleName + "status_textcolor) as textcolor " +
                        "from tbl_" + moduleName +
                        " join tbl_" + moduleName + "status on " + moduleName + "_" + moduleName + "statusid=" + moduleName + "status_" + moduleName + "statusid " +
                        " group by " + moduleName + "_" + moduleName + "statusid";
        DataTable dttblStatus = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table cellpadding=5>");
        for (int i = 0; i < dttblStatus.Rows.Count; i++)
        {
            string status = GlobalUtilities.ConvertToString(dttblStatus.Rows[i]["module_status"]);
            int count = GlobalUtilities.ConvertToInt(dttblStatus.Rows[i]["status_count"]);
            string statusColor_bg = GlobalUtilities.ConvertToString(dttblStatus.Rows[i]["bgcolor"]);
            string statusColor_text = GlobalUtilities.ConvertToString(dttblStatus.Rows[i]["textcolor"]);

            html.Append("<tr><td><div class='grid-status' style='background-color:" + statusColor_bg + ";color:" + statusColor_text + "'>" + status
                            + "</div></td><td><div style='background-color:" + statusColor_bg + ";color:" + statusColor_text + "' class='grid-status-count'>" + count + "</div></td></tr>");
        }
        html.Append("</table>");
        ltSummary.Text = html.ToString();
    }
    public static void BindStatusPieChart(Page page, Literal ltSummary, string moduleName, string title)
    {
        StringBuilder html = new StringBuilder();
        string chartId = "chart_" + Guid.NewGuid().ToString();
        html.Append("<div id='" + chartId + "' style='margin:0px'></div>");


        string query = "select count(*) as status_count,min(" + moduleName + "status_" + moduleName + "status) as module_status," +
                        "min(" + moduleName + "status_backgroundcolor) as bgcolor,min(" + moduleName + "status_textcolor) as textcolor " +
                        "from tbl_" + moduleName +
                        " join tbl_" + moduleName + "status on " + moduleName + "_" + moduleName + "statusid=" + moduleName + "status_" + moduleName + "statusid " +
                        " group by " + moduleName + "_" + moduleName + "statusid";
        string colors = "";
        DataTable dttblStatus = DbTable.ExecuteSelect(query);

        StringBuilder script = new StringBuilder();
        script.Append("google.load('visualization', '1.0', { 'packages': ['corechart'] });" + Environment.NewLine);
        script.Append("google.setOnLoadCallback(drawChart);" + Environment.NewLine);
        script.Append("function drawChart() {" + Environment.NewLine);
        script.Append("var data = new google.visualization.DataTable();" + Environment.NewLine);
        script.Append("data.addColumn('string', 'Topping');" + Environment.NewLine);
        script.Append("data.addColumn('number', 'Slices');" + Environment.NewLine);
        script.Append("data.addRows([" + Environment.NewLine);

        for (int i = 0; i < dttblStatus.Rows.Count; i++)
        {
            string status = GlobalUtilities.ConvertToString(dttblStatus.Rows[i]["module_status"]);
            int count = GlobalUtilities.ConvertToInt(dttblStatus.Rows[i]["status_count"]);
            string statusColor_bg = GlobalUtilities.ConvertToString(dttblStatus.Rows[i]["bgcolor"]);
            string statusColor_text = GlobalUtilities.ConvertToString(dttblStatus.Rows[i]["textcolor"]);

            script.Append("['" + status + "'," + count + "]");
            if (i != dttblStatus.Rows.Count - 1)
            {
                script.Append(",");
            }
            if (i == 0)
            {
                colors = "'" + statusColor_bg + "'";
            }
            else
            {
                colors += ",'" + statusColor_bg + "'";
            }
        }
        script.Append("]);" + Environment.NewLine);
        script.Append("var options = { backgroundColor:'#f7f7f7','title': '" + title + "','is3D':true,'chartArea': {'width': '100%', 'height': '80%'}," +
                                       "colors: [" + colors + "]," + Environment.NewLine);
        script.Append("'width': 280," + Environment.NewLine);
        script.Append("'height': 300" + Environment.NewLine);
        script.Append("};" + Environment.NewLine);
        script.Append("var chart = new google.visualization.PieChart(document.getElementById('"+chartId+"'));" + Environment.NewLine);
        script.Append("chart.draw(data, options);" + Environment.NewLine);
        script.Append("}");

        page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Graph","<script>" + script + "</script>");

        ltSummary.Text = html.ToString();
    }
}
