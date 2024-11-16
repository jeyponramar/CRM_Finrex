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

public partial class graph_leadstatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dttbl = new DataTable();
        InsertUpdate obj = new InsertUpdate();
        string query = "select * from tbl_enquirystatus";
        DataTable dttblStatus = obj.ExecuteSelect(query);
        dttblStatus.Columns.Add("count");
        StringBuilder html = new StringBuilder();

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
            InsertUpdate obj1 = new InsertUpdate();
            query = "select count(*) as totrecs from tbl_enquiry where enquiry_enquirystatusid=" + Convert.ToString(dttblStatus.Rows[i]["enquirystatus_enquirystatusid"]);
            dttbl = obj1.ExecuteSelect(query);
            string status = Convert.ToString(dttblStatus.Rows[i]["enquirystatus_status"]);
            int count = GlobalUtilities.ConvertToInt(dttbl.Rows[0][0]);
            script.Append("['" + status + "'," + count + "]");
            if (i != dttblStatus.Rows.Count - 1)
            {
                script.Append(",");
            }
            bool isOddTdExists = false;
            
            if (i % 2 == 0)
            {
                html.Append("<tr>");
                html.Append("<td width='100'>" + status + "</td><td class='bold'>" + count.ToString() + "</td>");
            }
            else
            {
                html.Append("<td width='100'>" + status + "</td><td class='bold'>" + count.ToString() + "</td>");
                isOddTdExists = true;
                html.Append("</tr>");
            }
            if (!isOddTdExists && i == dttblStatus.Rows.Count)
            {
                html.Append("<td>&nbsp;</td>");
                html.Append("</tr>");
            }
            
           
        }
        script.Append("]);" + Environment.NewLine);
        script.Append("var options = { 'title': 'Enquiry Status','is3D':true,'chartArea': {'width': '100%', 'height': '80%'}," + Environment.NewLine);
        script.Append("'width': 280," + Environment.NewLine);
        script.Append("'height': 300" + Environment.NewLine);
        script.Append("};" + Environment.NewLine);
        script.Append("var chart = new google.visualization.PieChart(document.getElementById('chart_div'));" + Environment.NewLine);
        script.Append("chart.draw(data, options);" + Environment.NewLine);
        script.Append("}");
        //ltbindstatus.Text = html.ToString();
        ClientScript.RegisterClientScriptBlock(typeof(Page), "Graph",
            "<script>" + script + "</script>");
    }
}
