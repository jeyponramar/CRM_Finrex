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

public partial class ReceiptVoucher_add : System.Web.UI.Page
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
                ddlpaymentmodeid.SelectedValue = "2";
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
                PopulateInvoiceAmount();
                ViewState["PrevRefNo"] = txtreferenceno.Text;
                btnthanksmail.Visible = true;
                if (Common.RoleId == 1)
                {
                    btnDelete.Visible = true;
                }
            }
            
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Receipt Voucher";
        }
        else
        {
            lblPageTitle.Text = "Edit Receipt Voucher";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    private void PopulateByReferenceNo()
    {
        if (Request.QueryString["mid"] == null) return;
        txtmodule.Text = Request.QueryString["m"];
        txtmoduleid.Text = Request.QueryString["mid"];
        string m = txtmodule.Text;

        string query = "select * from tbl_" + m +
                " JOIN tbl_client ON client_clientid=invoice_clientid " +
                "JOIN tbl_ledger ON ledger_ledgername=client_customername " +
                "WHERE " + m + "_" + m + "id=" + global.CheckInputData(txtmoduleid.Text);
        DataRow drInvoice = DbTable.ExecuteSelectRow(query);
        if (drInvoice != null)
        {
            txtledgerid.Text = GlobalUtilities.ConvertToString(drInvoice["ledger_ledgerid"]);
            ledger.Text = GlobalUtilities.ConvertToString(drInvoice["ledger_ledgername"]);
            txtreferenceno.Text = GlobalUtilities.ConvertToString(drInvoice[m + "_referenceno"]);
            ddlaccountadjustmentmethodid.SelectedValue = "2";
            lblInvoiceAmount.Text = GlobalUtilities.FormatAmount(drInvoice["invoice_totalamount"]);
            t_balanceamount.Text = GlobalUtilities.FormatAmount(drInvoice["invoice_balanceamount"]);
        }
    }
    private void PopulateInvoiceAmount()
    {
        string m = txtmodule.Text;
        string query = "select * from tbl_" + m +
                " JOIN tbl_client ON client_clientid=invoice_clientid " +
                "JOIN tbl_ledger ON ledger_ledgername=client_customername " +
                "WHERE " + m + "_" + m + "id=" + global.CheckInputData(txtmoduleid.Text);
        DataRow drInvoice = DbTable.ExecuteSelectRow(query);
        if (drInvoice != null)
        {
            lblInvoiceAmount.Text = GlobalUtilities.FormatAmount(drInvoice["invoice_totalamount"]);
            t_balanceamount.Text = GlobalUtilities.FormatAmount(drInvoice["invoice_balanceamount"]);
        }
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
        txtcramount.Text = Convert.ToString(GlobalUtilities.ConvertToDouble(txtreceivedamount.Text) + GlobalUtilities.ConvertToDouble(txttdsamount.Text));
        if (Request.QueryString["id"] == null)
        {
            txtvoucherno.Text = UniqueCode.GetUniqueCode(gblData, "voucherno", "ID", "ledgervoucher_accountvouchertypeid=" + Convert.ToInt32(VoucherType.Receipt), 1);
        }
        gblData.AddExtraValues("accountvouchertypeid", Convert.ToInt32(VoucherType.Receipt));
        gblData.AddExtraValues("dramount", 0);

        double amountPaid = GlobalUtilities.ConvertToDouble(txtcramount.Text);
        double balanceAmount = GlobalUtilities.ConvertToDouble(t_balanceamount.Text);
        double totalAmount = GlobalUtilities.ConvertToDouble(lblInvoiceAmount.Text);
        string query = "";
        if (Request.QueryString["id"] != null)
        {
            query = "select * from tbl_ledgervoucher where ledgervoucher_ledgervoucherid=" + GetId();
            DataRow drLedgerVoucher = DbTable.ExecuteSelectRow(query);
            balanceAmount += GlobalUtilities.ConvertToDouble(drLedgerVoucher["ledgervoucher_cramount"]);
        }
        if (amountPaid > balanceAmount)
        {
            lblMessage.Text = "Total amount paid can not be greater than total invoice amount!" + balanceAmount.ToString()+"-"+amountPaid.ToString();
            lblMessage.Visible = true;
            return 0;
        }
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
            Accounts.SavePaymentReceipt(id, GlobalUtilities.ConvertToInt(txtledgerid.Text), txtvoucherdate.Text, GlobalUtilities.ConvertToDouble(txtcramount.Text),
                                        bankAccount, VoucherType.Receipt, txtreferenceno.Text);

            Accounts.AdjustVoucherPayment(GlobalUtilities.ConvertToString(ViewState["PrevRefNo"]), txtreferenceno.Text, false);

            UpdateInvoiceBalance();

            lblMessage.Text = "Data saved successfully!";
            SendThanksMail(id);

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

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Accounts.DeletePaymentReceipt(GetId(), txtreferenceno.Text, false);
        lblMessage.Text = "Data saved successfully!";
        lblMessage.Visible = true;
        btnDelete.Visible = false;
        btnSubmit.Visible = false;
        
    }
    private void UpdateInvoiceBalance()
    {
        if (txtreferenceno.Text.Trim() == "") return;
        string crdrPaidAmount = "";
        string m = txtmodule.Text;
        if (m == "invoice")
        {
            crdrPaidAmount = "cr";
        }
        else if (m == "purchase")
        {
            crdrPaidAmount = "dr";
        }
        
        int mid = GlobalUtilities.ConvertToInt(txtmoduleid.Text);
        string query = "select * from tbl_" + m + " WHERE " + m + "_referenceno='" + txtreferenceno.Text + "'";
        DataRow drInvoice = DbTable.ExecuteSelectRow(query);
        if (drInvoice == null) return;
        int invoiceId = GlobalUtilities.ConvertToInt(drInvoice[m + "_" + m + "id"]);
        double dblTotalAmount = GlobalUtilities.ConvertToDouble(drInvoice[m + "_totalamount"]);
        query = "select sum(isnull(ledgervoucher_" + crdrPaidAmount + "amount,0)) as amountpaid from tbl_ledgervoucher " +
                "WHERE ledgervoucher_accountvouchertypeid = " + Convert.ToInt16(VoucherType.Receipt) + " AND ledgervoucher_referenceno='" + txtreferenceno.Text + "'";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return;
        double dblAmountPaid = GlobalUtilities.ConvertToDouble(dr["amountpaid"]);
        double dblBalance = dblTotalAmount - dblAmountPaid;
        query = "update tbl_" + m + " set " + m + "_amountpaid=" + dblAmountPaid + "," + m + "_balanceamount=" + dblBalance + " WHERE " + m + "_" + m + "id=" + invoiceId;
        DbTable.ExecuteQuery(query);
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
    protected void btnthanksmail_Click(object sender, EventArgs e)
    {
        SendThanksMail(GetId());
    }
    private void SendThanksMail(int id)
    {
        string m = txtmodule.Text;
        string query = "select * from tbl_" + m + " WHERE " + m + "_referenceno='" + txtreferenceno.Text + "'";
        DataRow drInvoice = DbTable.ExecuteSelectRow(query);
        int invoiceId = GlobalUtilities.ConvertToInt(drInvoice[m + "_" + m + "id"]);

        string mobileNos = "";
        string emailIds = "";
        string directorEmailId = GlobalUtilities.ConvertToString(drInvoice[m + "_directoremailid"]);
        string finnanceEmailId = GlobalUtilities.ConvertToString(drInvoice[m + "_financeemailid"]);
        string directorMobileNo = GlobalUtilities.ConvertToString(drInvoice[m + "_directormobile"]);
        string finnanceMobileNo = GlobalUtilities.ConvertToString(drInvoice[m + "_financemobile"]);

        mobileNos = GlobalUtilities.CommaSeperator(mobileNos, directorMobileNo);
        mobileNos = GlobalUtilities.CommaSeperator(mobileNos, finnanceMobileNo);

        emailIds = GlobalUtilities.CommaSeperator(emailIds, directorEmailId);
        emailIds = GlobalUtilities.CommaSeperator(emailIds, finnanceEmailId);

        if (emailIds == "")
        {
            lblMessage.Text = "Please enter director/finance person email id in Invoice to send email.";
            lblMessage.Visible = true;
            return;
        }
        if (mobileNos == "")
        {
            lblMessage.Text = "Please enter director/finance person mobile no. in Invoice to send WhatsApp message";
            lblMessage.Visible = true;
            return;
        }

        DataRow dr = GetReceiptData(id);

        string mobileNo = GlobalUtilities.ConvertToString(dr["client_mobileno"]);
        RPlusWhatsAppAPI.SendMessage("", false, "receiptvoicer", id, dr, mobileNos, EnumWhatsAppMessageType.Text,
                "Payment Receipt To Customer", "", "", "", "", "");

        string welcomemailSubject = Common.GetSetting("Thanks Mail Receipt Subject");
        string WelcomEmail = Common.GetFormattedSettingForEmail("Thanks Mail Receipt Body", dr, true);
        //string toEmailId = GlobalUtilities.ConvertToString(dr["client_emailid"]);
        Email.SaveEmailAndRedirect(emailIds, welcomemailSubject, WelcomEmail, "Payment Received", id);
    }
    private DataRow GetReceiptData(int id)
    {
        string query = "select * from tbl_ledgervoucher " +
                        "join tbl_invoice on ledgervoucher_moduleid=invoice_invoiceid AND ledgervoucher_module='Invoice' " +
                        "join tbl_client on client_clientid=invoice_clientid " +
                        "where ledgervoucher_ledgervoucherid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        return dr;
    }
}
