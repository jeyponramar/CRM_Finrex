using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class graph_lastthreemonthsenqanalysis : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindGraph();
    }
    private void BindGraph()
    {
        DataTable dttbl = new DataTable();
        InsertUpdate obj = new InsertUpdate();
        DataTable dttblStatus = GetEnqStatus();
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
            script.Append("['" + Convert.ToString(dttblStatus.Rows[i]["enquirystatus_status"]) + "'," + GlobalUtilities.ConvertToInt(dttblStatus.Rows[i]["StatusCount"]) + "]");
            if (i != dttblStatus.Rows.Count - 1)
            {
                script.Append(",");
            }
        }
        script.Append("]);" + Environment.NewLine);
        script.Append("var options = { 'title': '','is3D':true,'chartArea': {'width': '100%', 'height': '80%'}," + Environment.NewLine);
        script.Append("'width': 280," + Environment.NewLine);
        script.Append("'height': 300" + Environment.NewLine);
        script.Append("};" + Environment.NewLine);
        script.Append("var chart = new google.visualization.PieChart(document.getElementById('chart_div'));" + Environment.NewLine);
        script.Append("chart.draw(data, options);" + Environment.NewLine);
        script.Append("}");

        ClientScript.RegisterClientScriptBlock(typeof(Page), "Graph",
            "<script>" + script + "</script>");
    }
    private DataTable GetEnqStatus()
    {
         string strQuery = @"                            
                        SELECT MIN(enquirystatus_status)AS enquirystatus_status,dbo.uf_GetEnqStatus_month(enquiry_enquirystatusid,-3)AS StatusCount
                        FROM tbl_enquiry
                        JOIN tbl_enquirystatus ON  enquirystatus_enquirystatusid=enquiry_enquirystatusid
                        WHERE (MONTH(enquiry_enquirydate)> MONTH(DATEADD(mm,-3,GETDATE())) AND YEAR(enquiry_enquirydate)=YEAR(GETDATE()))
                        GROUP BY enquiry_enquirystatusid
                        UNION
                        SELECT 'TotalEnquiry'AS enquirystatus_status, COUNT(*)AS StatusCount  FROM tbl_enquiry WHERE (MONTH(enquiry_enquirydate)> MONTH(DATEADD(mm,-3,GETDATE())) AND YEAR(enquiry_enquirydate)=YEAR(GETDATE()))";
        DataTable dt = DbTable.ExecuteSelect(strQuery);
        return dt;
    }
   
}
