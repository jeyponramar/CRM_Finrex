using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.Drawing;


public partial class campaignvssales : System.Web.UI.Page
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
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        BindData();
        string query = "select sum(sales_totalamount) as totalamount from tbl_enquiry " +
                      "join tbl_sales on sales_enquiryid=enquiry_enquiryid where " + grid.ReportWhere;
        InsertUpdate obj = new InsertUpdate();
        DataTable dttbl = obj.ExecuteSelect(query);
        if (GlobalUtilities.IsValidaTable(dttbl))
        {
            trSummary.Visible = true;
            double dblTotalSale = GlobalUtilities.ConvertToDouble(dttbl.Rows[0][0]);
            lblTotalSale.Text = GlobalUtilities.FormatAmount(dblTotalSale);
            DataRow drCampaign = Common.GetOneRowData("tbl_campaign", GlobalUtilities.ConvertToInt(txtenquiry_campaignid.Text));
            if (drCampaign == null) return;
            double dblExpectedRevenue = GlobalUtilities.ConvertToDouble(drCampaign["campaign_expectedrevenue"]);
            lblExpectedSale.Text = GlobalUtilities.FormatAmount(dblExpectedRevenue);
            double dblActualExpense = GlobalUtilities.ConvertToDouble(drCampaign["campaign_actualexpense"]);
            lblActualExpense.Text = GlobalUtilities.FormatAmount(dblActualExpense);
            if (dblTotalSale - dblExpectedRevenue > 0)
            {
                lblDifference.Text = GlobalUtilities.FormatAmount(Math.Abs(dblTotalSale - dblExpectedRevenue)) + " (PROFIT)";
                lblDifference.ForeColor = Color.Green;
            }
            else
            {
                lblDifference.Text = GlobalUtilities.FormatAmount(Math.Abs(dblTotalSale - dblExpectedRevenue)) + " (LOSS)";
                lblDifference.ForeColor = Color.Red;
            }
        }
        else
        {
            trSummary.Visible = false;
        }
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