using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class graph_expectedleadsbycampaign : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindGraph();
    }
    private void BindGraph()
    {
        StringBuilder script = new StringBuilder();
        script.Append("google.load('visualization', '1', { packages: ['columnchart'] });" + Environment.NewLine);
        script.Append("google.setOnLoadCallback(drawChart);" + Environment.NewLine);
        script.Append("function drawChart() {" + Environment.NewLine);
        script.Append("var data = google.visualization.arrayToDataTable([" + Environment.NewLine);
        script.Append("['Campaign', 'Leads Expected', 'Response']," + Environment.NewLine);

        string query = "select * from tbl_campaign where campaign_isactive='1'";
        InsertUpdate objc = new InsertUpdate();
        DataTable dttblc = objc.ExecuteSelect(query);
        for (int i = 0; i < dttblc.Rows.Count; i++)
        {
            string expected = Convert.ToString(dttblc.Rows[i]["campaign_expectedresponse"]);
            string campainName=Convert.ToString(dttblc.Rows[i]["campaign_campaignname"]);
            InsertUpdate obj = new InsertUpdate();
            query = "select count(*) c from tbl_enquiry where enquiry_campaignid=" + GlobalUtilities.ConvertToInt(dttblc.Rows[i]["campaign_campaignid"]);
            string response = "0";
            DataRow dr = obj.ExecuteSelectRow(query);
            if (dr != null)
            {
                response = Convert.ToString(dr["c"]);
            }
            if (i == dttblc.Rows.Count - 1)
            {
                script.Append("['" + campainName + "', " + expected + ", " + response + "],");
            }
            else
            {
                script.Append("['" + campainName + "', " + expected + ", " + response + "],");
            }
        }
       
        script.Append("]);" + Environment.NewLine);
        script.Append("var chart = new google.visualization.ColumnChart(document.getElementById('chart_div'));" + Environment.NewLine);
        script.Append("chart.draw(data, { width: 300, height: 230, is3D: true, title: 'Leads Expected VS Response' });" + Environment.NewLine);
        script.Append("}");

        ClientScript.RegisterClientScriptBlock(typeof(Page), "Graph",
            "<script>" + script + "</script>");
    }
   
}
