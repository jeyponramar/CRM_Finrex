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

public partial class graph_leadbycampaign : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        DataTable dttbl = new DataTable();
        InsertUpdate obj = new InsertUpdate();
        string query = "select * from tbl_campaign where campaign_isactive='1'";
        DataTable dttblCampaign = obj.ExecuteSelect(query);
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
            InsertUpdate obj1 = new InsertUpdate();
            query = "select count(*) as totrecs from tbl_enquiry where enquiry_campaignid=" + 
                        Convert.ToString(dttblCampaign.Rows[i]["campaign_campaignid"]);
            dttbl = obj1.ExecuteSelect(query);
            int count = GlobalUtilities.ConvertToInt(dttbl.Rows[0][0]);
            script.Append("['" + Convert.ToString(dttblCampaign.Rows[i]["campaign_campaignname"]) + "'," + count + "]");
            if (i != dttblCampaign.Rows.Count - 1)
            {
                script.Append(",");
            }
        }
        script.Append("]);" + Environment.NewLine);
        script.Append("var options = { 'title': 'Enquiry by Campaign (This Month)','is3D':true,'chartArea': {'width': '100%', 'height': '80%'}," + 
                            Environment.NewLine);
        script.Append("'width': 280," + Environment.NewLine);
        script.Append("'height': 300" + Environment.NewLine);
        script.Append("};" + Environment.NewLine);
        script.Append("var chart = new google.visualization.PieChart(document.getElementById('chart_div'));" + Environment.NewLine);
        script.Append("chart.draw(data, options);" + Environment.NewLine);
        script.Append("}");

        ClientScript.RegisterClientScriptBlock(typeof(Page), "Graph",
            "<script>" + script + "</script>");
    }
}
