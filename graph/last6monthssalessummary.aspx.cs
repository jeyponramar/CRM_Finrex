using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WebComponent;
using System.Text;

public partial class graph_lastsixmonthssales : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        DataTable dttbl = new DataTable();
        InsertUpdate obj = new InsertUpdate();
         string query = @"SELECT SUBSTRING(CAST(DATENAME(mm,MIN(sales_date)) AS VARCHAR),0,4)+' '+CAST(YEAR(MIN(sales_date))AS VARCHAR) AS sales_date,SUM(sales_totalamount)AS sales_totalamount,SUM(sales_totalamount)AS sales_totalsales,COUNT(*)AS sales_salescount FROM
                            (
	                            SELECT *,MONTH(sales_date)AS SalesMonth,YEAR(sales_date)AS Salesyear
	                            FROM tbl_sales
	                            WHERE (MONTH(sales_date)> MONTH(DATEADD(mm,-6,GETDATE())) AND YEAR(sales_date)=YEAR(GETDATE()))
                            )r1
                                GROUP BY r1.SalesMonth,r1.Salesyear";
         DataTable dttblCampaign = DbTable.ExecuteSelect(query);
        DataTable dt = DbTable.ExecuteSelect(query);
        dttblCampaign.Columns.Add("count");
        StringBuilder script = new StringBuilder();
        script.Append("google.load('visualization', '1.0', { 'packages': ['corechart'] });" + Environment.NewLine);
        script.Append("google.setOnLoadCallback(drawChart);" + Environment.NewLine);
        script.Append("function drawChart() {" + Environment.NewLine);
        script.Append("var data = new google.visualization.DataTable();" + Environment.NewLine);
        script.Append("data.addColumn('string', 'Topping');" + Environment.NewLine);
        script.Append("data.addColumn('number', 'Slices');" + Environment.NewLine);
        script.Append("data.addRows([" + Environment.NewLine);

        for (int i = 0; i < dttblCampaign.Rows.Count; i++)
        {
            script.Append("['" + Convert.ToString(dttblCampaign.Rows[i]["sales_date"]) + "'," + GlobalUtilities.ConvertToString(dttblCampaign.Rows[i]["sales_salescount"]) + "]");
            if (i != dttblCampaign.Rows.Count - 1)
            {
                script.Append(",");
            }
        }
        script.Append("]);" + Environment.NewLine);
        script.Append("var options = { 'title': '','is3D':true,'chartArea': {'width': '100%', 'height': '80%'}," + 
                            Environment.NewLine);
        script.Append("'width': 280," + Environment.NewLine);
        script.Append("'height': 300" + Environment.NewLine);
        script.Append("};" + Environment.NewLine);
        script.Append("var chart = new google.visualization.LineChart(document.getElementById('chart_div'));" + Environment.NewLine);
        script.Append("chart.draw(data, options);" + Environment.NewLine);
        script.Append("}");

        ClientScript.RegisterClientScriptBlock(typeof(Page), "Graph",
            "<script>" + script + "</script>");
    }
}
