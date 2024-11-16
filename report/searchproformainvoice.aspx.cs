using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class searchproformainvoice : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
		//ChartEvent_START
		
		//ChartEvent_END
        if (!IsPostBack)
        {
			//FillDropDown_START
			
			gblData.FillDropdown(proformainvoice_salesstatusid, "tbl_salesstatus", "salesstatus_status", "");
			//FillDropDown_END

            proformainvoice_salesstatusid.SelectedValue = "1";
            //BindDataOnLoad_START
			BindDataOnLoad();
			//BindDataOnLoad_END
            //BindSubReportControl_START
			Common.BindSubReportControls(plSearch);
			//BindSubReportControl_END
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
	
	//BindChart_END
    
}