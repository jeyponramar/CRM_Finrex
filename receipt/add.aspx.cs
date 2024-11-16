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

public partial class Receipt_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_receipt", "receiptid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
			CommonPage.DisableOnQuickAddEdit(btnSubmitAndView);
            //FillDropDown_START
			
			gblData.FillDropdown(ddlpaymentmodeid, "tbl_paymentmode", "paymentmode_paymentmode", "paymentmode_paymentmodeid", "", "paymentmode_paymentmode");
			//FillDropDown_END
            if (Request.QueryString["invid"] != null || Request.QueryString["invno"] != null)
            {
                txtinvoiceid.Text = Convert.ToString(Request.QueryString["invid"]);
                invoice.Text = Convert.ToString(Request.QueryString["invno"]);
            }
            if (Request.QueryString["id"] == null)
            {
                PopulateClientDetails(GlobalUtilities.ConvertToInt(Common.GetOneColumnData("tbl_invoice", GlobalUtilities.ConvertToInt(txtinvoiceid.Text), "clientid")));
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
            lblPageTitle.Text = "Add Receipt";
        }
        else
        {
            lblPageTitle.Text = "Edit Receipt";
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
    private void PopulateClientDetails(int clientid)
    {
        string query = "SELECT * FROM tbl_client WHERE client_clientid="+ clientid;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        gblData.PopulateForm(dr, form);

    }
    protected void btnSaveAndView_Click(object sender, EventArgs e)
    {
        if (SaveData(false) > 0)
		{
        	Response.Redirect("~/Receipt/view.aspx");
		}
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        //SetCode_START//SetCode_END
        //ExtraValues_START//ExtraValues_END
        int id = 0;
        CalculateTotalAmount();
       // id = gblData.SaveForm(form, GetId());
        if (CheckInvoicePaymentDetil())
        {
            if (h_IsCopy.Text == "1")
            {
                id = gblData.SaveForm(form, 0);
            }
            else
            {
                id = gblData.SaveForm(form);
            }
        }
        else
        {
            return 0;
        }

        if (id > 0)
        {
            //SaveSubTable_START
			
			//SaveSubTable_END
            //SaveFile_START
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END
            UpdateBalanceAmount();
            lblMessage.Text = "Data saved successfully!";
            lblMessage.Visible = true;
            string target = "";
            if (Request.QueryString["targettxt"] != null)
            {
                //pass data from this page to previous tab
                //target = "setPassData(" + Request.QueryString["tpage"] + ",'" + Request.QueryString["targettxt"] 
                //    + "','" + Request.QueryString["targethdn"] + "'," + id + ",'" + txtamccode.Text + "');";
            }
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
    private void UpdateBalanceAmount()
    {
        //update balance amount to invoice table
        string query = "";
        ReceiptDAO dao = new ReceiptDAO();
        DataRow dr = dao.Updateinvoicereceipt(Convert.ToDouble(h_balanceamount.Text), Convert.ToInt32(txtinvoiceid.Text));
    }
    private void PopulateBalanceamountAndInvoiceAmount(int paymentid)
    {
        StringBuilder Html = new StringBuilder();
        ReceiptDAO dao = new ReceiptDAO();
        DataRow dr = dao.Getinvoicedetail_by_receipt(paymentid);
        double TotalAmount = 0;
        double BalanceAmount = 0;
        if (dr != null)
        {
            TotalAmount = GlobalUtilities.ConvertToDouble(dr["TotalAmount"]);//totalamount
            double dblnewpaidamount = GlobalUtilities.ConvertToDouble(txttotalamount.Text);//new amount 
            if (dr["BalanceAmount"] != Convert.DBNull)
            {
                BalanceAmount = GlobalUtilities.ConvertToDouble(dr["BalanceAmount"]);//balance
            }
            else
            {
                BalanceAmount = TotalAmount;
            }
            if (Request.QueryString["id"] != null)//edit
            {
                BalanceAmount = (GlobalUtilities.ConvertToDouble(h_prevamoutpaid.Text)) + (BalanceAmount);
            }
        }
        Html.Append("<td>Balance Amount :<b style='font-size:medium' class='error'>" + BalanceAmount + "</b>&nbsp;&nbsp; Invoice Amount :<b style='font-size:medium' class='error'>" + TotalAmount + "</b>");
        lblbalanceamount.Text ="Balance Amount :"+Convert.ToString(BalanceAmount);
        lblProformaInvoiceAmount.Text = "Invoice Amount :" + Convert.ToString(TotalAmount);
        ltbindbalanceamount.Text = Html.ToString();
    }
    private bool CheckInvoicePaymentDetil()
    {
        ReceiptDAO dao = new ReceiptDAO();
        int invoiceid = Convert.ToInt32(txtinvoiceid.Text);
        DataRow dr = dao.GetInvoiceReceiptdetail(invoiceid);
        double BalanceAmount = 0;
        if (dr != null)
        {
            double TotalAmount = GlobalUtilities.ConvertToDouble(dr["TotalAmount"]);//totalamount
            double dblnewpaidamount = GlobalUtilities.ConvertToDouble(txttotalamount.Text);//new amount 
            if (dr["BalanceAmount"] != Convert.DBNull)
            {
                BalanceAmount = GlobalUtilities.ConvertToDouble(dr["BalanceAmount"]);//balance
            }
            else
            {
                BalanceAmount = TotalAmount;
            }
            if (Request.QueryString["id"] != null)//edit
            {
                BalanceAmount = (GlobalUtilities.ConvertToDouble(h_prevamoutpaid.Text)) + (BalanceAmount);
            }

            if (dblnewpaidamount > BalanceAmount)//new add
            {
                displayErrorMessage("Paid Amount Can not be greater than the Balance Amount, You Can Enter The Amount till:" + Convert.ToString(BalanceAmount));
                return false;
            }
            BalanceAmount = BalanceAmount - dblnewpaidamount;
            h_balanceamount.Text = Convert.ToString(BalanceAmount);
            return true;
        }
        else
        {
            return false;
        }
    }
    private void displayErrorMessage(string message)
    {
        lblMessage.Visible = true;
        lblMessage.Text = message;
    }
    protected void invoiceno_Changed(object sender, EventArgs e)
    {
        int invoiceid = GlobalUtilities.ConvertToInt(txtinvoiceid.Text);
        if (invoiceid > 0)
        {
            //PopulateBalanceamountAndInvoiceAmount(invoiceid);
        }
    }
    private void CalculateTotalAmount()
    {
        double discountAmt = GlobalUtilities.ConvertToDouble(txtdiscount.Text);
        double amountPaid = GlobalUtilities.ConvertToDouble(txtamountpaid.Text);
        double totalAmt = discountAmt + amountPaid;
        txttotalamount.Text = GlobalUtilities.ConvertToString(totalAmt);
    }
    
}
