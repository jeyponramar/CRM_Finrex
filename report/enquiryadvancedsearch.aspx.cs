using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class enquiryadvancedsearch : System.Web.UI.Page
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
			
			gblData.FillDropdown(enquiry_enquirystatusid, "tbl_enquirystatus", "enquirystatus_status", "");
			//FillDropDown_END
            //BindDataOnLoad_START
			
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
    
}