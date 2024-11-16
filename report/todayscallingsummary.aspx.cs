using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class todayscallingsummary : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        dtGetColumnsAdDtaRow();
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
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        BindData();
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

    private void dtGetColumnsAdDtaRow()
    {
        string strQuery = @"SELECT ISNULL(SUM(NotInterested),0) as telecalling_notinterested,ISNULL(SUM(InProgress),0) as telecalling_inprogress,COUNT(*) as telecalling_assigned,COUNT(*) AS telecalling_totalcall,
                ISNULL(SUM(ConvertedToEnquiry),0)AS telecalling_convertedtoenquiry,ISNULL(SUM(NotInterested),0)AS telecalling_notinterested,ISNULL(SUM(ConvertedToSale),0)AS telecalling_convertedtosale,
                ISNULL(MIN(telecalling_telecallingid),0)AS telecalling_telecallingid,ISNULL(SUM(_Open),0)AS telecalling_open,
                SUBSTRING(CAST(DATENAME(mm,MIN(telecalling_assigneddate)) AS VARCHAR),0,4)+' '+CAST(YEAR(MIN(telecalling_assigneddate))AS VARCHAR) AS CallingDate
                FROM(
                SELECT 
                    case when telecalling_telecallerstatusid =2 then 1 else 0 end as _Open,	
                    case when telecalling_telecallerstatusid = 3 then 1 else 0 end as InProgress,	
                    case when telecalling_telecallerstatusid = 4 then 1 else 0 end as ConvertedToEnquiry,
                    case when telecalling_telecallerstatusid = 5 then 1 else 0 end as NotInterested,
                    case when telecalling_telecallerstatusid = 6 then 1 else 0 end as ConvertedToSale,*
                FROM tbl_telecalling 
                WHERE CAST(telecalling_assigneddate AS DATE)=CAST(GETDATE() AS DATE)
                )r1";


        DataTable dt = DbTable.ExecuteSelect(strQuery);
        DataTable dtTodaysCalling = new DataTable();
        dtTodaysCalling.Columns.Add("CallingStatus");
        dtTodaysCalling.Columns.Add("StatusCount");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string strColumnName = GlobalUtilities.ConvertToString(dt.Columns[i]);
                if (strColumnName == "telecalling_assigned") continue;
                string Data = GlobalUtilities.ConvertToString(dt.Rows[0][strColumnName]);
                DataRow dr = dtTodaysCalling.NewRow();
                dr["CallingStatus"] = strColumnName.Substring(strColumnName.IndexOf('_') + 1, strColumnName.Length - strColumnName.IndexOf('_') - 1);
                dr["StatusCount"] = Data;
                dtTodaysCalling.Rows.Add(dr);
            }
            chart.data = dtTodaysCalling;
            chart.BindChart();
            grid.GridQuery = strQuery;
            grid.Data = dt;
            grid.InitGrid();
            grid.BindGridData();
        }
    }

}