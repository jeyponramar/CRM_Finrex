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

public partial class PaymentVoucher_Add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_ledgervoucher", "ledgervoucherid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            //FillDropDown_START
			
			gblData.FillDropdown(ddlaccountadjustmentmethodid, "tbl_accountadjustmentmethod", "accountadjustmentmethod_adjustmentmethod", "accountadjustmentmethod_accountadjustmentmethodid", "", "accountadjustmentmethod_adjustmentmethod");
			gblData.FillDropdown(ddlpaymentmodeid, "tbl_paymentmode", "paymentmode_paymentmode", "paymentmode_paymentmodeid", "", "paymentmode_paymentmode");
			//FillDropDown_END
            
            if (Request.QueryString["id"] == null)
            {
                ddlpaymentmodeid.SelectedIndex = 2;
       			//PopulateAcOnAdd_START
			
			    //PopulateAcOnAdd_END
				//PopulateOnAdd_START
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
                PopulateByReferenceNo();
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                
                ViewState["PrevRefNo"] = txtreferenceno.Text;
            }
            
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Payment Voucher";
        }
        else
        {
            lblPageTitle.Text = "Edit Payment Voucher";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    private void PopulateByReferenceNo()
    {
        if (Request.QueryString["mid"] == null) return;
        txtmodule.Text = Request.QueryString["m"];
        txtmoduleid.Text = Request.QueryString["mid"];
        string customer=Request.QueryString["c"];
        string m = txtmodule.Text;

        DataRow drLedger=Accounts.GetLedger(customer);
        if(drLedger==null)return;
        txtledgerid.Text = drLedger["ledger_ledgerid"].ToString();
        ledger.Text = drLedger["ledger_ledgername"].ToString();
        txtreferenceno.Text = Request.QueryString["rno"];
        ddlaccountadjustmentmethodid.SelectedValue = "2";
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData(true);
    }
    protected void btnSaveAndView_Click(object sender, EventArgs e)
    {
        SaveData(false);
        Response.Redirect("~/VoucherPayment/view.aspx");
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;

        if (Request.QueryString["id"] == null)
        {
            txtvoucherno.Text = UniqueCode.GetUniqueCode(gblData, "voucherno", "ID", "ledgervoucher_accountvouchertypeid=" + Convert.ToInt32(VoucherType.Payment), 1);
        }
        gblData.AddExtraValues("accountvouchertypeid", Convert.ToInt32(VoucherType.Payment));
        gblData.AddExtraValues("cramount", 0);

        //ExtraValues_START//ExtraValues_END
        int id = 0;
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
           
            //SaveFile_START
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END
            Ledger bankAccount = Ledger.BankAccount;
            if (Convert.ToInt32(ddlpaymentmodeid.SelectedValue) == 2)
            {
                bankAccount = Ledger.CashAccount;
            }
            Accounts.SavePaymentReceipt(id, GlobalUtilities.ConvertToInt(txtledgerid.Text), txtvoucherdate.Text, 
                                        GlobalUtilities.ConvertToDouble(txtdramount.Text),
                                        bankAccount, VoucherType.Payment, txtreferenceno.Text);

            Accounts.AdjustVoucherPayment(GlobalUtilities.ConvertToString(ViewState["PrevRefNo"]), txtreferenceno.Text, true);

            UpdateParentBalance();

            lblMessage.Text = "Data saved successfully!";
            lblMessage.Visible = true;
            string target = "";
            
            string script = "";
            string close = "";
            if (Request.QueryString["id"] == null)
            {
                gblData.ResetForm(form);
            }
            else
            {
                close = "parent.closeTab();";
            }
            script = target + close;
            if (script != "" && isclose)
            {
                script = "<script>" + script + "</script>";
                ClientScript.RegisterClientScriptBlock(typeof(Page), "closetab", script);
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
    private void UpdateParentBalance()
    {
        if (txtreferenceno.Text.Trim() == "") return;
        string crdrPaidAmount = "";
        string m = txtmodule.Text;
        
        if (m == "purchaseorder")
        {
            crdrPaidAmount = "dr";
        }
        else if (m == "expenses")
        {
            crdrPaidAmount = "cr";
        }
        else
        {
            return;
        }
        int mid = GlobalUtilities.ConvertToInt(txtmoduleid.Text);
        string query = "select * from tbl_" + m + " WHERE " + m + "_" + m + "id=" + mid;
        DataRow drParent = DbTable.ExecuteSelectRow(query);
        if (drParent == null) return;
        int ParentId = GlobalUtilities.ConvertToInt(drParent[m + "_" + m + "id"]);
        double dblTotalAmount = GlobalUtilities.ConvertToDouble(drParent[m + "_totalamount"]);
        query = "select sum(isnull(ledgervoucher_" + crdrPaidAmount + "amount,0)) as amountpaid from tbl_ledgervoucher " +
                "WHERE ledgervoucher_accountvouchertypeid = " + Convert.ToInt16(VoucherType.Payment) + " AND ledgervoucher_referenceno='" + txtreferenceno.Text + "'";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return;
        double dblAmountPaid = GlobalUtilities.ConvertToDouble(dr["amountpaid"]);
        //double dblBalance = dblTotalAmount - dblAmountPaid;
        query = "update tbl_" + m + " set " + m + "_amountpaid=" + dblAmountPaid + " WHERE " + m + "_" + m + "id=" + ParentId;
        bool issuccess = DbTable.ExecuteQuery(query);
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
    
}
