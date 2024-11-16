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

public partial class Sales_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_sales", "salesid");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CommonPage.DisableOnQuickAddEdit(btnSubmitAndView);
            //FillDropDown_START
			
			EnableControlsOnEdit();
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
                //PopulateOnAdd_START
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
                txtsalesstatusid.Text = "1";
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                btnaddexpenses.Visible = true;
                int salesstatusId = GlobalUtilities.ConvertToInt(gblData._CurrentRow["sales_salesstatusid"]);
                visibleHideButtonAccordingToStatus(salesstatusId);
               
                visibleHideButtonAccordingToStatus(salesstatusId);
                bool isconvertintoinvoice = GlobalUtilities.ConvertToBool(gblData._CurrentRow["sales_isconvertedtoinvoice"]);
                if (isconvertintoinvoice)
                {
                    btnconverttoinvoice.Visible = false;
                    btnSubmit.Visible = false;
                    btnSubmitAndView.Visible = false;
                    lblMessage.Text = "You Can't Edit this Sales because sales is converted into Invoice";
                    lblMessage.Visible = true;

                }
                
            }
            //CallPopulateSubGrid_START
			
			PopulateSubGrid();
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Sales";
        }
        else
        {
            lblPageTitle.Text = "Edit Sales";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    //PopulateSubGrid_START
			
	private void PopulateSubGrid()
	{
		SubGrid sgrid = new SubGrid();
		sgrid.Populate(gblData, ltsalesdetail, salesdetail_param, salesdetail_setting);
	}
			//PopulateSubGrid_END
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData(true);
    }
    protected void btnSaveAndView_Click(object sender, EventArgs e)
    {
        if (SaveData(false) > 0)
		{
        	Response.Redirect("~/Sales/view.aspx");
		}
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        //SetCode_START
		if(Request.QueryString["id"] == null)
		{
			txtbillno.Text = UniqueCode.GetUniqueCode(gblData, "billno", "SO-", 1);
		}
		//SetCode_END
        //ExtraValues_START//ExtraValues_END
        int id = 0;
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            //SaveSubTable_START
			
			gblData.SaveSubTable(id, gblData, salesdetail_setting);
			//SaveSubTable_END
            //SaveFile_START
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END
            DbTable.ExecuteQuery("UPDATE tbl_sales SET sales_totalsalesquantity=((SELECT SUM(ISNULL(salesdetail_quantity,0)) FROM tbl_salesdetail WHERE salesdetail_salesid=" + id + ")) WHERE sales_salesid=" + id);
            lblMessage.Text = "Data saved successfully!";
            lblMessage.Visible = true;
            CommonPage.CloseQuickAddEditWindow(Page, form, id);
            txtsalesstatusid.Text = "1";
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
		btnconverttoinvoice.Visible = true;
		btnaddexpenses.Visible = true;
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

    protected void btnconverttoinvoice_Click(object sender, EventArgs e)
    {
        Common.ConvertAndRedirect("ConvertSalesToInvoice");
    }

    private void visibleHideButtonAccordingToStatus(int salesstatusId)
    {
        if (salesstatusId == 4)
        {
            btnconverttoinvoice.Visible = false;
            btnSubmit.Visible = false;
            btnSubmitAndView.Visible = false;
            lblMessage.Text = "You Can't Edit this Sales because sales is converted into Invoice";
            lblMessage.Visible = true;
            btnDelete.Visible = false;
        }
        
    }
    	
	protected void btnaddexpenses_Click(object sender, EventArgs e)
	{
        Response.Redirect("../expenses/add.aspx?salesid=" + GlobalUtilities.ConvertToInt(Request.QueryString["id"]) + "&referenceno=" + txtbillno.Text + "&expensesfor=Sales");
	}
	
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Common.Delete();
    } 
	
}
    
