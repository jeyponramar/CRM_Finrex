using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class advancedreportsubscription : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
		//ChartEvent_START
		
		//ChartEvent_END
        if (!IsPostBack)
        {
			//FillDropDown_START
			
			gblData.FillDropdown(subscription_subscriptionstatusid, "tbl_subscriptionstatus", "subscriptionstatus_status", "");
			//FillDropDown_END
            //BindDataOnLoad_START
			
			//BindDataOnLoad_END
            //BindSubReportControl_START
            grid.IsSearch = true;
            
			Common.BindSubReportControls(plSearch);
			{
				grid.InitGrid();
                grid.IsReport = false;
				grid.BindData();
			}
			//BindSubReportControl_END
            if (Common.RoleId == 1)
            {
                reportfield_employeeid.Visible = true;
            }
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        //grid.IsSearch = true;
        //grid.InitGrid();
        //grid.gridSettings.IsReport = false;
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
	
	//BindChart_END
    
}