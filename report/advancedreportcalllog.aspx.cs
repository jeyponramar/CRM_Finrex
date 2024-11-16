using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class advancedreportcalllog : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
		//ChartEvent_START
		
		//ChartEvent_END
        if (!IsPostBack)
        {
			//FillDropDown_START
			
			gblData.FillDropdown(calllog_notificationtypeid, "tbl_notificationtype", "notificationtype_notificationtype", "");
			gblData.FillDropdown(calllog_emailsmssentstatusid, "tbl_emailsmssentstatus", "emailsmssentstatus_status", "");
			//FillDropDown_END
            //BindDataOnLoad_START
			
			//BindDataOnLoad_END
            //BindSubReportControl_START
			Common.BindSubReportControls(plSearch);
			//BindSubReportControl_END
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
        
        if (Request.QueryString["sr"] == "2")
        {
            if (calllog_clientid.Text == "")
            {
                lblMessage.Text = "Please enter client name!";
                lblMessage.Visible = true;
                return;
            }
            if (calllog_sentdate_from.Text == "")
            {
                lblMessage.Text = "Please enter start date!";
                lblMessage.Visible = true;
                return;
            }
            if (calllog_sentdate_from.Text == "")
            {
                lblMessage.Text = "Please enter end date!";
                lblMessage.Visible = true;
                return;
            }
            lnkPrintReport.NavigateUrl = "~/calllog/Calllogreport.aspx?cid=" + txtcalllog_clientid.Text + "&sd=" + calllog_sentdate_from.Text + "&ed=" + calllog_sentdate_to.Text;
            lnkPrintReport.Visible = true;
        }
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