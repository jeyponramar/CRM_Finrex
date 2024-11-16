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

public partial class Expenses_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_expenses", "expensesid");
    
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
       			//PopulateAcOnAdd_START
			
			//PopulateAcOnAdd_END
				//PopulateOnAdd_START
				CommonPage.PopulateOnAdd(form, "complaintid");
				CommonPage.PopulateOnAdd(form, "amcserviceid");
				CommonPage.PopulateOnAdd(form, "serviceid");
				CommonPage.PopulateOnAdd(form, "amcid");
				CommonPage.PopulateOnAdd(form, "invoiceid");
				CommonPage.PopulateOnAdd(form, "salesid");
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
                populateDataBasedONRequestQueryString();
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                if (GlobalUtilities.ConvertToBool(gblData._CurrentRow["expenses_ispaid"]))
                {
                    btnSubmit.Visible = false;
                    btnSubmitAndView.Visible = false;
                    lblMessage.Text = "You can't edit once payment done.";
                    lblMessage.Visible = true;
                }
                lnkPrint.Visible = true;
                lnkPrint.NavigateUrl = "voucherprint.aspx?expensesid=" + GetId();
                btnaddpayment.Visible = true;
                ViewState["PrevRefNo"] = txtreferanceno.Text;
            }
            //CallPopulateSubGrid_START
			
			PopulateSubGrid();
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Expenses";
        }
        else
        {
            lblPageTitle.Text = "Edit Expenses";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    //PopulateSubGrid_START
			
	private void PopulateSubGrid()
	{
		SubGrid sgrid = new SubGrid();
		sgrid.Populate(gblData, ltexpensesdetail, expensesdetail_param, expensesdetail_setting);
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
        	Response.Redirect("~/Expenses/view.aspx");
		}
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        //SetCode_START
		if(Request.QueryString["id"] == null)
		{
			if(txtvoucherno.Text == "") txtvoucherno.Text = UniqueCode.GetUniqueCode(gblData, "voucherno", "VNo-", 1);
		}
		//SetCode_END
        //ExtraValues_START
		if(Request.QueryString["id"] == null)
		{
			Common.AddExtraQueryStringVal(gblData, "complaintid");
			Common.AddExtraQueryStringVal(gblData, "amcserviceid");
			Common.AddExtraQueryStringVal(gblData, "serviceid");
			Common.AddExtraQueryStringVal(gblData, "amcid");
			Common.AddExtraQueryStringVal(gblData, "invoiceid");
			Common.AddExtraQueryStringVal(gblData, "salesid");
		}
		//ExtraValues_END
        if (Request.QueryString["id"] == null)
        {
            gblData.AddExtraValues("balanceamount", hdnTotalAmount.Text);
            gblData.AddExtraValues("amountpaid", 0);
        }
        int id = 0;
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            //SaveSubTable_START
			
			gblData.SaveSubTable(id, gblData, expensesdetail_setting);
			//SaveSubTable_END
            //SaveFile_START
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END

            //save in account
            Accounts.SaveVoucherDetail(employee.Text, txtvoucherno.Text, txtexpensedate.Text,
                     GlobalUtilities.ConvertToDouble(hdnTotalAmount.Text), VoucherType.IndirectExpense, id, "expensesdetail",
                     txtreferanceno.Text.Trim(), GlobalUtilities.ConvertToString(ViewState["PrevRefNo"]).Trim());

            lblMessage.Text = "Data saved successfully!";
            lblMessage.Visible = true;
            CommonPage.CloseQuickAddEditWindow(Page, form, id);
            if (Request.QueryString["complaintid"] != null || Request.QueryString["amcserviceid"] != null || Request.QueryString["serviceid"] != null || Request.QueryString["amcid"] != null || Request.QueryString["invoiceid"] != null || Request.QueryString["salesid"] != null)
            {
                Response.Redirect("~/Expenses/view.aspx");
            }
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
		btnaddpayment.Visible = true;
		//btnDelete.Visible = true;
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
    
	protected void btnaddpayment_Click(object sender, EventArgs e)
	{
        Response.Redirect("../paymentvoucher/add.aspx?m=expenses&mid=" + Request.QueryString["id"] + "&c=" + employee.Text + "&rno=" + txtreferanceno.Text);
	}
    private void populateDataBasedONRequestQueryString()
    {
        if (Request.QueryString["salesid"] != null && Request.QueryString["referenceno"] != null && Request.QueryString["expensesfor"] != null)
        {
            txtreferanceno.Text = Convert.ToString(Request.QueryString["referenceno"]);
            txtexpensesfor.Text = Convert.ToString(Request.QueryString["expensesfor"]);
        }
        else if (Request.QueryString["amcid"] != null && Request.QueryString["referenceno"] != null && Request.QueryString["expensesfor"] != null)
        {
            txtreferanceno.Text = Convert.ToString(Request.QueryString["referenceno"]);
            txtexpensesfor.Text = Convert.ToString(Request.QueryString["expensesfor"]);
        }
        else if (Request.QueryString["amcserviceid"] != null && Request.QueryString["expensesfor"] != null && Request.QueryString["referenceno"] != null)
        {
            txtexpensesfor.Text = Convert.ToString(Request.QueryString["expensesfor"]);
            txtreferanceno.Text = Convert.ToString(Request.QueryString["referenceno"]);
        }
        else if (Request.QueryString["complaintid"] != null && Request.QueryString["referenceno"] != null && Request.QueryString["expensesfor"] != null)
        {
            txtreferanceno.Text = Convert.ToString(Request.QueryString["referenceno"]);
            txtexpensesfor.Text = Convert.ToString(Request.QueryString["expensesfor"]);
        }
        else if (Request.QueryString["serviceid"] != null && Request.QueryString["referenceno"] != null && Request.QueryString["expensesfor"] != null)
        {
            txtreferanceno.Text = Convert.ToString(Request.QueryString["referenceno"]);
            txtexpensesfor.Text = Convert.ToString(Request.QueryString["expensesfor"]);
        }
        else if (Request.QueryString["invoiceid"] != null && Request.QueryString["referenceno"] != null && Request.QueryString["expensesfor"] != null)
        {
            txtreferanceno.Text = Convert.ToString(Request.QueryString["referenceno"]);
            txtexpensesfor.Text = Convert.ToString(Request.QueryString["expensesfor"]);
        }
    }
}
