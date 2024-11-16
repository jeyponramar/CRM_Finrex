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

public partial class Tax_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_tax", "taxid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
			CommonPage.DisableOnQuickAddEdit(btnSubmitAndView);
            //FillDropDown_START
			
			gblData.FillDropdown(ddltaxtypeid, "tbl_taxtype", "taxtype_taxtype", "taxtype_taxtypeid", "", "taxtype_taxtype");
			gblData.FillDropdown(ddlgsttypeid, "tbl_gsttype", "gsttype_gsttype", "gsttype_gsttypeid", "", "gsttype_gsttype");
			EnableControlsOnEdit();
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
       			//PopulateOnAdd_START
				DataRow drpop = CommonPage.PopulateOnAdd(form);//pop=true&popm=&popjoin=&popid=
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                ViewState["OldLedgerName"] = txttax.Text;
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Tax";
        }
        else
        {
            lblPageTitle.Text = "Edit Tax";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
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
        if (SaveData(false) > 0)
		{
        	Response.Redirect("~/Tax/view.aspx");
		}
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        //SetCode_START//SetCode_END
        //ExtraValues_START//ExtraValues_END
        int id = 0;
        int ledgerId = Accounts.SaveLedger(txttax.Text, LedgerGroup.Tax, 
                       GlobalUtilities.ConvertToString(ViewState["OldLedgerName"]), LedgerType.Tax);
        if (ledgerId == 0)
        {
            lblMessage.Text = "Error occurred while saving data in accounts!";
            lblMessage.Visible = true;
            return 0;
        }
        else if (ledgerId == -1)
        {
            lblMessage.Text = "Ledger already exists in Accounts!";
            lblMessage.Visible = true;
            return 0;
        }
        if (Request.QueryString["id"] == null)
        {
            ddltaxtypeid.SelectedValue = "6";
        }
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            //SaveSubTable_START
			
			//SaveSubTable_END
            //SaveFile_START
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END
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
    
}
