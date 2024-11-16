using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class quotationbystatus : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            //gblData.FillDropdown(quotation_quotationstatusid, "tbl_quotationstatus", "quotationstatus_status", "");
            gblData.FillDropdown(quotation_quotationstatusid, "tbl_quotationstatus", "quotationstatus_status", "quotationstatus_quotationstatusid not in (3,5) "); 
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        grid.SearchHolderId = plSearch.ID;
        grid.Report();
    }
}