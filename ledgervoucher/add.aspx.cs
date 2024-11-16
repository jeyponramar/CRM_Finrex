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

public partial class LedgerVoucher_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_ledgervoucher", "ledgervoucherid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            RedirectToVoucherEntry();
			CommonPage.DisableOnQuickAddEdit(btnSubmitAndView);
            //FillDropDown_START
			
			gblData.FillDropdown(ddlaccountadjustmentmethodid, "tbl_accountadjustmentmethod", "accountadjustmentmethod_adjustmentmethod", "accountadjustmentmethod_accountadjustmentmethodid", "", "accountadjustmentmethod_adjustmentmethod");
			gblData.FillDropdown(ddlpaymentmodeid, "tbl_paymentmode", "paymentmode_paymentmode", "paymentmode_paymentmodeid", "", "paymentmode_paymentmode");
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
       			//PopulateAcOnAdd_START
			
			//PopulateAcOnAdd_END
				//PopulateOnAdd_START
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Ledger Voucher";
        }
        else
        {
            lblPageTitle.Text = "Edit Ledger Voucher";
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
        Response.Redirect("~/LedgerVoucher/view.aspx");
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
    //EnableControlsOnEdit_END
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
    private void RedirectToVoucherEntry()
    {
        DataRow dr = DbTable.GetOneRow("tbl_ledgervoucher", GetId());
        VoucherType voucherType = (VoucherType)dr["ledgervoucher_accountvouchertypeid"];
        if (voucherType == VoucherType.Payment)
        {
            Response.Redirect("~/paymentvoucher/add.aspx?id=" + GetId());
        }
        else if (voucherType == VoucherType.Receipt)
        {
            Response.Redirect("~/receiptvoucher/add.aspx?id=" + GetId());
        }
        else 
        {
            string m = GlobalUtilities.ConvertToString(dr["ledgervoucher_module"]);
            int mid = GlobalUtilities.ConvertToInt(dr["ledgervoucher_moduleid"]);
            Response.Redirect("~/" + m + "/add.aspx?id=" + mid);
        }
    }
}
