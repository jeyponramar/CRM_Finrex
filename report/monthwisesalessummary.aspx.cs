using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class monthwisesalessummary : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
		//ChartEvent_START
		grid.bindChartUsingGrid += new BindChartUsingGrid(bindChart);
		//ChartEvent_END
        if (!IsPostBack)
        {
			//FillDropDown_START
			
			//FillDropDown_END
            //BindDataOnLoad_START
			//BindDataOnLoad();
			//BindDataOnLoad_END
            if (Common.GetQueryStringValue("dashboard") > 0)
            {
                tdsearchsales.Visible = false;
                plSearch.Visible = false;
                gridmonthwisesales.Visible = false;
                chart.Width = "300";
                chart.Height = "500";
                
            }
            bind_data();
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        //BindData();
        bind_data();
    }
    private void BindData()
    {
        grid.SearchHolderId = plSearch.ID;
        grid.Report();
    }
    private void BindDataOnLoad()
    {
        BindData();
    }
    //BindChart_START
	private void bindChart(DataTable dttbl)
	{
		chart.data = dttbl;
		chart.BindChart();
	}
	//BindChart_END

    private void bind_data()
    {
        string strExtraWhere = (sales_date_from.Text != "" && sales_date_to.Text != "") ? "    sales_date BETWEEN '" + GlobalUtilities.ConvertToSqlMinDateFormat(sales_date_from.Text) + "' AND '" + GlobalUtilities.ConvertToSqlMaxDateFormat(sales_date_to.Text) + "' " : "";
        strExtraWhere = (strExtraWhere == "") ? "   (MONTH(sales_date)> MONTH(DATEADD(mm,-6,GETDATE())) AND YEAR(sales_date)=YEAR(GETDATE()))   " : strExtraWhere;

        string query = @"SELECT SUBSTRING(CAST(DATENAME(mm,MIN(sales_date)) AS VARCHAR),0,4)+' '+CAST(YEAR(MIN(sales_date))AS VARCHAR) AS sales_date,SUM(sales_totalamount)AS sales_totalamount,SUM(sales_totalamount)AS sales_totalsales,COUNT(*)AS sales_salescount FROM
                            (
	                            SELECT *,MONTH(sales_date)AS SalesMonth,YEAR(sales_date)AS Salesyear
	                            FROM tbl_sales
	                            WHERE " + strExtraWhere +
                            @" )r1
                                GROUP BY r1.SalesMonth,r1.Salesyear";
        DataTable dt = DbTable.ExecuteSelect(query);
        grid.GridQuery = query;
        grid.Data = dt;
        grid.InitGrid();
        grid.BindGridData();
    }
}