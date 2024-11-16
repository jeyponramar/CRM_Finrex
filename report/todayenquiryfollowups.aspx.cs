using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class todayenquiryfollowups : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
		//ChartEvent_START
		
		//ChartEvent_END
        if (!IsPostBack)
        {
			//FillDropDown_START
			
			//FillDropDown_END
            //BindDataOnLoad_START
			
			//BindDataOnLoad_END
            grid.GridQuery = @"SELECT * FROM tbl_enquiry JOIN tbl_client ON enquiry_clientid=client_clientid
                              left JOIN tbl_campaign ON enquiry_campaignid=campaign_campaignid
                              left JOIN tbl_designation ON enquiry_designationid=designation_designationid
                              left JOIN tbl_product ON enquiry_productid=product_productid
                              left JOIN tbl_city ON enquiry_cityid=city_cityid
                            left JOIN tbl_area ON enquiry_areaid=area_areaid
                            left JOIN tbl_employee ON enquiry_employeeid=employee_employeeid
                            left JOIN tbl_communicationsource ON enquiry_communicationsourceid=communicationsource_communicationsourceid
                            left JOIN tbl_enquirystatus ON enquiry_enquirystatusid=enquirystatus_enquirystatusid
                            left JOIN tbl_enquiryfor ON enquiry_enquiryforid=enquiryfor_enquiryforid where 
                            enquiry_enquiryid in (select followups_mid from tbl_followups where followups_module='enquiry' and
	                        cast(followups_date as date) =cast(getdate() as date)) AND enquiry_enquirystatusid NOT IN(3)";
            grid.BindReport();
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