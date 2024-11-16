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
        int mkpersonId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_MarketingPersonId"));
        string extraWhere = "";
        if (mkpersonId > 0)
        {
            extraWhere = "marketingperson_marketingpersonid=" + mkpersonId + @" AND";
        }
        //if (GlobalUtilities.ConvertToInt(CustomSession.Session("Login_MarketingPersonId")) > 0)
        {
            StringBuilder script = new StringBuilder();
            script.Append("google.load('visualization', '1', { packages: ['columnchart'] });" + Environment.NewLine);
            script.Append("google.setOnLoadCallback(drawChart);" + Environment.NewLine);
            script.Append("function drawChart() {" + Environment.NewLine);
            script.Append("var data = google.visualization.arrayToDataTable([" + Environment.NewLine);
            script.Append("['Enquiry', 'Total Enquiry ', 'Won']," + Environment.NewLine);

            string query = @"SELECT SUBSTRING(CAST(DATENAME(mm,MIN(enquiry_enquirydate)) AS VARCHAR),0,4)+' '+CAST(YEAR(MIN(enquiry_enquirydate))AS VARCHAR) AS EnquiryDate,
                    'Enquiry Status'AS EnquiryStatus,COUNT(*)AS TotalEnquiry,SUM(ISNULL(Won,0))AS TotalWon 
                    FROM
                    (
						SELECT enquiry_enquirydate,enquirystatus_status,CASE WHEN enquiry_enquirystatusid=3 THEN 1 ELSE 0 END AS Won
						FROM tbl_enquiry 
						JOIN tbl_enquirystatus ON enquirystatus_enquirystatusid = enquiry_enquirystatusid
						left JOIN tbl_employee ON employee_employeeid=enquiry_employeeid
						WHERE " + extraWhere + @" (MONTH(enquiry_enquirydate)> MONTH(DATEADD(mm,-6,GETDATE()))
                        AND YEAR(enquiry_enquirydate)=YEAR(GETDATE()))" +
                        @" )r1                
                    GROUP BY MONTH(r1.enquiry_enquirydate) , YEAR(r1.enquiry_enquirydate)";
            InsertUpdate objc = new InsertUpdate();
            DataTable dttblc = objc.ExecuteSelect(query);
            for (int i = 0; i < dttblc.Rows.Count; i++)
            {
                string Enqdate = Convert.ToString(dttblc.Rows[i]["EnquiryDate"]);
                string Total = Convert.ToString(dttblc.Rows[i]["TotalEnquiry"]);
                string Won = Convert.ToString(dttblc.Rows[i]["TotalWon"]);
                if (i == dttblc.Rows.Count - 1)
                {
                    script.Append("['" + Enqdate + "', " + Total + ", " + Won + "],");
                }
                else
                {
                    script.Append("['" + Enqdate + "', " + Total + ", " + Won + "],");
                }
            }

            script.Append("]);" + Environment.NewLine);
            script.Append("var chart = new google.visualization.ColumnChart(document.getElementById('chart_div'));" + Environment.NewLine);
            script.Append("chart.draw(data, { width: 300, height: 230, is3D: true, title: 'Total Enquiry VS Won' });" + Environment.NewLine);
            script.Append("}");

            ClientScript.RegisterClientScriptBlock(typeof(Page), "Graph",
                "<script>" + script + "</script>");
        }
    }
   
}
