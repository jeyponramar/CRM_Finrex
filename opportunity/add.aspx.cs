using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class Opportunity_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_opportunity", "opportunityid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillDropDown_START
			
			gblData.FillDropdown(ddlpriorityid, "tbl_priority", "priority_priority", "priority_priorityid", "", "priority_priority");
			gblData.FillDropdown(ddlopportunitystatusid, "tbl_opportunitystatus", "opportunitystatus_status", "opportunitystatus_opportunitystatusid", "", "opportunitystatus_status");
			gblData.FillDropdown(ddlopportunitystageid, "tbl_opportunitystage", "opportunitystage_stage", "opportunitystage_opportunitystageid", "", "opportunitystage_stage");
			EnableControlsOnEdit();
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
                grid.AddUrl = "#quotation/add.aspx";
                grid.Visible = false;
       			//PopulateAcOnAdd_START
			
			//PopulateAcOnAdd_END
				//PopulateOnAdd_START
				//PopulateOnAdd_END
                //SetDefault_START
				ddlopportunitystatusid.SelectedValue = "1";//SetDefault_END
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                ViewState["AssignedToId"] = txtemployeeid.Text;
                ViewState["EnquiryId"] = Convert.ToString(gblData._CurrentRow["opportunity_enquiryid"]);

                grid.AddUrl = "#quotation/add.aspx?&enquiryid="+ GlobalUtilities.ConvertToInt(txtenquiryid.Text)+ "&opportunityid=" + GetId() ;
                if (gblData._CurrentRow != null)
                {
                    int statusid = GlobalUtilities.ConvertToInt(gblData._CurrentRow["opportunity_opportunitystatusid"]);
                    if (statusid == 2)//closed
                    {
                        lblMessage.Text = "You can not edit this opportunity as it is already converted to sale.";
                        grid.EnableAddLink = false;
                        Followups.EnableAddlink = false;
                        lblMessage.Visible = true;
                        btncancel.Visible = false;
                        btnreject.Visible = false;
                        btnSubmit.Visible = false;
                        btnDelete.Visible = false;
                    }
                }
                BindSubscriptionDetail();
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Opportunity";
        }
        else
        {
            lblPageTitle.Text = "Edit Opportunity";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    //PopulateSubGrid_START
			
			//PopulateSubGrid_END
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData(true);
    }
    protected void btnSaveAndView_Click(object sender, EventArgs e)
    {
        SaveData(false);
        Response.Redirect("~/Opportunity/view.aspx");
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        //SetCode_START//SetCode_END
        //ExtraValues_START//ExtraValues_END
        int id = 0;

        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            ViewState["OpportunityId"] = id;
            //SaveSubTable_START
			
			//SaveSubTable_END
            //SaveFile_START
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END
            Custom.UpdateOpportunityStatus(id);
            lblMessage.Text = "Data saved successfully!";
            lblMessage.Visible = true;
            CommonPage.CloseQuickAddEditWindow(Page, form, id);
        }
        else if (id == -1)
        {
            lblMessage.Text = "Data already exists, duplicate entry not allowed!";
            lblMessage.Visible = true;
        }
        else
        {
            lblMessage.Text = "Error occurred while saving data</br>Error : " + gblData._error;
            lblMessage.Visible = true;
        }
        return id;
    }
    //EnableControlsOnEdit_START
	private void EnableControlsOnEdit()
	{
		if (Request.QueryString["id"] == null) return;
		btncancel.Visible = true;
		btnreject.Visible = true;
		btnDelete.Visible = true;
	}//EnableControlsOnEdit_END
    private int GetId()
    {
        if (h_IsCopy.Text == "1")
        {
            return 0;
        }
        else
        {
            return GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Common.Delete();
    }
    private int GetquotationId(ref DataRow drQuotation)
    {
        string query = @"select top 1 * from tbl_quotation where quotation_opportunityid=" + GetId() + " order by quotation_quotationid DESC";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        drQuotation = dr;
        int quotationId = 0;
        if (dr != null)
        {
            quotationId = GlobalUtilities.ConvertToInt(dr["quotation_quotationid"]);
        }
        return quotationId;
    }
	protected void btnwon_Click(object sender, EventArgs e)
	{
        DataRow drQuotation = null;
        int quoteId = GetquotationId(ref drQuotation);
        if (quoteId == 0)
        {
            lblMessage.Text = "Please create quotation first for this Opportunity";
            lblMessage.Visible = true;
            return;
        }
        string query = "";
        int approvedOption = GlobalUtilities.ConvertToInt(drQuotation["quotation_quotationoptionid"]);
        if (approvedOption == 0)
        {
            //check whether multiple options
            query = "select count(*) c from tbl_quotationdetailoptionb WHERE quotationdetailoptionb_quotationid=" + quoteId;
            DataRow drOptionB = DbTable.ExecuteSelectRow(query);
            if (drOptionB != null)
            {
                lblMessage.Text = "Please select which option is approved by client. (Check in Quotation)";
                lblMessage.Visible = true;
                return;
            }
        }
        query = "update tbl_quotation set quotation_isapproved=1 WHERE quotation_quotationid=" + quoteId;
        DbTable.ExecuteQuery(query);

        //update status
        Common.UpdateStatus("opportunitystatusid", 2);
        
        query = "update tbl_enquiry set enquiry_enquirystatusid=5 where enquiry_enquiryid=" + GlobalUtilities.ConvertToInt(txtenquiryid.Text);
        DbTable.ExecuteQuery(query);

        if (approvedOption == 0) approvedOption = 1;
        string childTable = "";
        if (approvedOption == 2)
        {
            childTable = "tbl_quotationdetailoptionb";
        }

          Common.ConvertAndRedirect("ConvertQuotationToSales", quoteId, childTable, true);
        
	}
    
    private int GetquotationId()
    {
        string query = @"select top 1 * from tbl_quotation where quotation_opportunityid=" + GetId() + " order by quotation_quotationid DESC";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        int quotationId = 0;
        if (dr != null)
        {
            quotationId = GlobalUtilities.ConvertToInt(dr["quotation_quotationid"]);
        }
        return quotationId;
    }
    private void updateStatus()
    {
        string query = "";
        query = @"update tbl_quotation set quotation_quotationstatusid=2 where quotation_quotationid=" + GetquotationId();
        DbTable.ExecuteQuery(query);

        query = @"update tbl_enquiry set enquiry_enquirystatusid=5 where enquiry_enquiryid=" + GlobalUtilities.ConvertToInt(txtenquiryid.Text);
        DbTable.ExecuteQuery(query);
        ddlopportunitystatusid.SelectedValue = "2";
        Common.UpdateStatus("opportunitystatusid", 2);
        lblMessage.Text = "Status has been updated to Cancelled";
        lblMessage.Visible = true;
    }
	protected void btncancel_Click(object sender, EventArgs e)
	{
        if (txtremarks.Text == "")
        {
            lblMessage.Text = "Please enter remark for this cancellation.";
            lblMessage.Visible = true;
            return;
        }
        string query = "";
        query = @"update tbl_quotation set quotation_quotationstatusid=9 where quotation_quotationid=" + GetquotationId();
        DbTable.ExecuteQuery(query);

        query = @"update tbl_enquiry set enquiry_enquirystatusid=8 where enquiry_enquiryid=" + GlobalUtilities.ConvertToInt(txtenquiryid.Text);
        DbTable.ExecuteQuery(query);
        ddlopportunitystatusid.SelectedValue = "3";
        Common.UpdateStatus("opportunitystatusid", 3);
        lblMessage.Text = "Status has been updated to Cancelled";
        lblMessage.Visible = true;
	}
	protected void btnreject_Click(object sender, EventArgs e)
	{
        if (txtremarks.Text == "")
        {
            lblMessage.Text = "Please enter remark for this cancellation.";
            lblMessage.Visible = true;
            return;
        }
        string query = "";
        query = @"update tbl_quotation set quotation_quotationstatusid=10 where quotation_quotationid=" + GetquotationId();
        DbTable.ExecuteQuery(query);

        query = @"update tbl_enquiry set enquiry_enquirystatusid=9 where enquiry_enquiryid=" + GlobalUtilities.ConvertToInt(txtenquiryid.Text);
        DbTable.ExecuteQuery(query);
        ddlopportunitystatusid.SelectedValue = "4";
        Common.UpdateStatus("opportunitystatusid", 4);
        lblMessage.Text = "Status has been updated to Rejected";
        lblMessage.Visible = true;
	}
    private void UpdateEnquiryStatus(int eid, int status)
    {
        string query = "update tbl_enquiry set enquiry_enquirystatusid=" + status + " where enquiry_enquiryid=" + eid;
        InsertUpdate obj = new InsertUpdate();
        obj.ExecuteQuery(query);
    }
    private void UpdateQuotationsalesid(int sid, int quotationid)
    {
        string query = "update tbl_quotation set quotation_salesid=" + sid + "where quotation_quotationid= " + quotationid;
        InsertUpdate obj = new InsertUpdate();
        obj.ExecuteQuery(query);
    }
    protected void btnsendpassword_Click(object sender, EventArgs e)
    {
        int id = Common.GetQueryStringValue("id");
        if (id == 0)
        {
            id = GlobalUtilities.ConvertToInt(ViewState["OpportunityId"]);
        }
        if (id == 0)
        {
            return;
        }
        
    }
    
	protected void btnsubscription_Click(object sender, EventArgs e)
    {
        string query = "select * from tbl_subscription WHERE subscription_opportunityid=" + GetId();
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            Response.Redirect("~/subscription/add.aspx?opportunityid=" + GetId());
        }
        else
        {
            Response.Redirect("~/subscription/add.aspx?id=" + GlobalUtilities.ConvertToInt(dr["subscription_subscriptionid"]));
        }
    }
    private void BindSubscriptionDetail()
    {
        string query = "select * from tbl_subscription WHERE subscription_opportunityid=" + GetId();
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            btnsubscription.Text = "Trial";
        }
        else
        {
            int subscriptionstatusId = GlobalUtilities.ConvertToInt(dr["subscription_subscriptionstatusid"]);
            if (subscriptionstatusId == 2 || subscriptionstatusId == 4)
            {
                btnsubscription.Text = "Subscription";
            }
            else
            {
                btnsubscription.Text = "Trial";
            }
        }
    }
}
