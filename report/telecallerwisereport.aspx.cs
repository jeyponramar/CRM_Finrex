using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class telecallerwisereport : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        grid.bindChartUsingGrid += new BindChartUsingGrid(bindChart);
		//ChartEvent_START
		
		//ChartEvent_END
        if (!IsPostBack)
        {
			//FillDropDown_START
			
			//FillDropDown_END
            //BindDataOnLoad_START
			//BindDataOnLoad();
			//BindDataOnLoad_END
            BindTelecallingChartReport(" telecalling_assigneddate BETWEEN '" + GlobalUtilities.ConvertToSqlMinDateFormat("01-" + DateTime.Now.AddMonths(-3).Month + "-" + DateTime.Now.Year) + "' AND '" + GlobalUtilities.ConvertToSqlMaxDateFormat(GlobalUtilities.ConvertToDate(DateTime.Now)) + "'");
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        //BindData();
        BindTelecallingChartReport("");
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
	
	//BindChart_END

    private void BindTelecallingChartReport(string WhereDefaultLast3Month)
    {
        string strExtraWhere = "";// "  (telecalling_employeeid = " + GlobalUtilities.ConvertToInt(txttelecalling_employeeid.Text) + " OR 0=0)       ";//AND telecalling_assigneddate BETWEEN AND
        if (WhereDefaultLast3Month == "")
        {
            strExtraWhere = (telecalling_assigneddate_from.Text != "" && telecalling_assigneddate_to.Text != "") ? "    telecalling_assigneddate BETWEEN '" + GlobalUtilities.ConvertToSqlMinDateFormat(telecalling_assigneddate_from.Text) + "' AND '" + GlobalUtilities.ConvertToSqlMaxDateFormat(telecalling_assigneddate_to.Text) + "'" : strExtraWhere;
        }
        else
        {
            strExtraWhere += WhereDefaultLast3Month;
        }
        strExtraWhere = (strExtraWhere != "") ? " WHERE " + strExtraWhere : "";
        string query = @"SELECT employee_employeeid,MIN(telecalling_employeeid)AS telecalling_employeeid,MIN(employee_employeename)AS employee_employeename,MIN(employee_employeename)AS TelecallerName,SUM(NotInterested) as NotInterested,SUM(InProgress) as InProgress,COUNT(*) as Assigned,COUNT(*) AS TotalCall,
                    SUM(ConvertedToEnquiry)AS ConvertedToEnquiry,SUM(NotInterested)AS NotInterested,SUM(ConvertedToSale)AS ConvertedToSale,
                    SUBSTRING(CAST(DATENAME(mm,MIN(telecalling_assigneddate)) AS VARCHAR),0,4)+' '+CAST(YEAR(MIN(telecalling_assigneddate))AS VARCHAR) AS CallingDate,MIN(telecalling_telecallingid)AS telecalling_telecallingid
                    FROM(
                    SELECT 
	                    case when telecalling_telecallerstatusid = 3 then 1 else 0 end as InProgress,	
	                    case when telecalling_telecallerstatusid = 4 then 1 else 0 end as ConvertedToEnquiry,
	                    case when telecalling_telecallerstatusid = 5 then 1 else 0 end as NotInterested,
	                    case when telecalling_telecallerstatusid = 6 then 1 else 0 end as ConvertedToSale,*
                    FROM tbl_telecalling 
                    JOIN tbl_employee ON employee_employeeid=telecalling_employeeid
                     " +strExtraWhere+
                    @" )r
                    GROUP BY employee_employeeid";
        DataTable dt = DbTable.ExecuteSelect(query);
        //chart.data = dt;
        //chart.BindChart();
        //return;
        grid.GridQuery = query;
        grid.Data = dt;
        grid.InitGrid();
        grid.BindGridData();
    }
    //BindChart_START
    private void bindChart(DataTable dttbl)
    {
        chart.data = dttbl;
        chart.BindChart();
    }
    //BindChart_END
}