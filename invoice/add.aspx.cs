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

public partial class Invoice_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_invoice", "invoiceid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bool isEditAccess = false;
            int id = Common.GetQueryStringValue("id");
            if (Common.RoleId == 1 || Common.RoleId == 9)
            {
                isEditAccess = true;
            }
            else
            {
                if (id > 0)
                {
                    string query = @"select * from tbl_invoice
                                join tbl_client on client_clientid=invoice_clientid
                                where invoice_invoiceid=" + id +
                                " and  (client_employeeid=" + Common.EmployeeId + " or invoice_createdby=" + Common.UserId + ")";
                    DataRow dr = DbTable.ExecuteSelectRow(query);
                    if (dr == null)
                    {
                        Session["S_Error"] = "You do not have access rights to perform this operation!";
                        Response.Redirect("../error.aspx");
                        return;
                    }
                }
            }
            if (isEditAccess)
            {
            }
            else
            {
                btnDelete.Visible = false;
                btngotoreceipt.Visible = false;
                btnSubmit.Visible = false;
                btnSubmitAndView.Visible = false;
                lblMessage.Text = "You do not have access rights to edit any detail.";
                lblMessage.Visible = true;
            }
            //FillDropDown_START
			
			gblData.FillDropdown(ddlsetupfortermsandconditionid, "tbl_setupfortermsandcondition", "setupfortermsandcondition_name", "setupfortermsandcondition_setupfortermsandconditionid", "setupfortermsandcondition_termsandconditiontypeid=3", "setupfortermsandcondition_name");
			EnableControlsOnEdit();
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
       			//PopulateOnAdd_START
				DataRow drpop = CommonPage.PopulateOnAdd(form);//pop=true&popm=&popjoin=&popid=
				DataRow drproject = CommonPage.PopulateOnAdd(form, "projectid");
				DataRow drclient = CommonPage.PopulateOnAdd(form, "clientid");
				DataRow drsubscription = CommonPage.PopulateOnAdd(form, "subscriptionid");
				//PopulateOnAdd_END
                ///lnkPrint.Visible = false;
                //SetDefault_START//SetDefault_END
                ddlsetupfortermsandconditionid.SelectedIndex = 1;
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                if (GlobalUtilities.ConvertToInt(gblData._CurrentRow["invoice_invoiceformatversion"]) > 0)
                {
                    trinvoicedetail.Visible = false;
                }

                Common.PopulateAutoComplete(leadby, txtleadbyid, GlobalUtilities.ConvertToInt(gblData._CurrentRow["invoice_leadbyid"]));
                Common.PopulateAutoComplete(advisoryby, txtadvisorybyid, GlobalUtilities.ConvertToInt(gblData._CurrentRow["invoice_advisorybyid"]));
                Common.PopulateAutoComplete(meeting1, txtmeeting1id, GlobalUtilities.ConvertToInt(gblData._CurrentRow["invoice_meeting1id"]));
                Common.PopulateAutoComplete(meeting2, txtmeeting2id, GlobalUtilities.ConvertToInt(gblData._CurrentRow["invoice_meeting2id"]));
                Common.PopulateAutoComplete(closedby, txtclosedbyid, GlobalUtilities.ConvertToInt(gblData._CurrentRow["invoice_closedbyid"]));

                ViewState["PrevRefNo"] = txtreferenceno.Text;
                if (!GlobalUtilities.ConvertToBool(gblData._CurrentRow["invoice_isupdatestatusforothermodule"]))
                {
                    updateStatusOfOtherModule(gblData._CurrentRow);
                }
                if(GlobalUtilities.ConvertToBool(gblData._CurrentRow["invoice_ispaid"]))
                {
                    btnSubmit.Visible=false;
                    lblMessage.Text="You can't edit this invoice once payment done";
                    lblMessage.Visible=true;
                }
                lnkPrint.Visible = true;
                lnkPrint.NavigateUrl = "~/invoice/InvoiceFormat.aspx?id=" + GetId();
            }
            //CallPopulateSubGrid_START
			
			PopulateSubGrid();
			PopulateSubGrid2();
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Invoice";
        }
        else
        {
            lblPageTitle.Text = "Edit Invoice";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    //PopulateSubGrid_START
			
	private void PopulateSubGrid()
	{
		SubGrid sgrid = new SubGrid();
		sgrid.Populate(gblData, ltinvoicedetail, invoicedetail_param, invoicedetail_setting);
	}
	private void PopulateSubGrid2()
	{
		SubGrid sgrid = new SubGrid();
		sgrid.Populate(gblData, ltinvoiceprofitsharingdetail, invoiceprofitsharingdetail_param, invoiceprofitsharingdetail_setting);
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
        	Response.Redirect("~/Invoice/view.aspx");
		}
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
		
        /*
        if(Request.QueryString["id"] == null)
		{
			if(txtinvoiceno.Text == "") txtinvoiceno.Text = UniqueCode.GetUniqueCode(gblData, "invoiceno", "Inv-", 1);
		}*/
        int invoiceformatversionId = 1;
        if (Request.QueryString["id"] != null)
        {
            DataRow dr = DbTable.GetOneRow("tbl_invoice", Common.GetQueryStringValue("id"));
            invoiceformatversionId = GlobalUtilities.ConvertToInt(dr["invoice_invoiceformatversion"]);
        }
        if (invoiceformatversionId > 0)
        {
            if (GlobalUtilities.ConvertToInt(txtserviceplanid.Text) == 0)
            {
                lblMessage.Text = "Please choose service plan.";
                lblMessage.Visible = true;
                return 0;
            }
        }

        string query = "";
        query = "select top 1 * from tbl_invoice order by 1 desc";
        DataRow drinvoice = DbTable.ExecuteSelectRow(query);
        int lastInvoiceId = GlobalUtilities.ConvertToInt(drinvoice["invoice_invoiceid"]);
        bool isUpdateCode = false;
        if (lastInvoiceId == GetId()) isUpdateCode = true;
        if (txtinvoiceno.Text.Trim() == "" || isUpdateCode)
        {
            DateTime dtInvoiceDate = Convert.ToDateTime(GlobalUtilities.ConvertMMDateToDD(txtinvoicedate.Text));
            int year = dtInvoiceDate.Year;
            int month = dtInvoiceDate.Month;
            int fromYear = year;
            int toYear = year;
            if (month > 3)
            {
                toYear = year + 1;
            }
            else
            {
                fromYear = year - 1;
            }
            string strFromYear = fromYear.ToString();
            string strToYear = toYear.ToString().Substring(2);
            int count = 1;
            string fromDate = fromYear + "-04-01";
            string toDate = toYear + "-03-31";
            query = @"select top 1 * from tbl_invoice 
                            where cast(invoice_invoicedate as date)>=cast('" + fromDate + "' as date) and cast(invoice_invoicedate as date)<=cast('" + toDate + @"' as date)
                            ";
            if (isUpdateCode) query += " and invoice_invoiceid<>" + lastInvoiceId;
            query += " order by invoice_count desc";
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (dr != null)
            {
                count = GlobalUtilities.ConvertToInt(dr["invoice_count"]) + 1;
            }
            gblData.AddExtraValues("count", count);
            string strcount = count.ToString();
            if (strcount.Length == 1)
            {
                strcount = "000" + strcount;
            }
            else if (strcount.Length == 2)
            {
                strcount = "00" + strcount;
            }
            else if (strcount.Length == 3)
            {
                strcount = "0" + strcount;
            }
            txtinvoiceno.Text = strcount + "/" + strFromYear + "-" + strToYear;
        }
		if(Request.QueryString["id"] == null)
		{
			Common.AddExtraQueryStringVal(gblData, "projectid");
			//gblData.AddExtraValues("balanceamount", hdnTotalAmount.Text);
			Common.AddExtraQueryStringVal(gblData, "moduleid");
			Common.AddExtraQueryStringVal(gblData, "subscriptionid");
            gblData.AddExtraValues("invoiceformatversion", "1");
		}
        if (Request.QueryString["id"] == null)
        {
            if (Request.QueryString["module"] != null)
            {
                gblData.AddExtraValues("module", Request.QueryString["module"]);
            }
        }
        //if (txtreferenceno.Text == "") txtreferenceno.Text = txtinvoiceno.Text;
        txtreferenceno.Text = txtinvoiceno.Text;

        int id = 0;
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            SaveInvoiceProductDetails(id, invoiceformatversionId);
            //SaveSubTable_START
			
			gblData.SaveSubTable(id, gblData, invoicedetail_setting);
			gblData.SaveSubTable(id, gblData, invoiceprofitsharingdetail_setting);
			mcinvoiceservices.Save(id);
			mcinvoicesoftwares.Save(id);
			//SaveSubTable_END
            //SaveFile_START
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END
            int voucherId = Accounts.SaveVoucherDetail(client.Text,txtinvoiceno.Text, txtinvoicedate.Text,
                     GlobalUtilities.ConvertToDouble(txttotalamount.Text), VoucherType.Sales, id, "invoicedetail",
                     txtreferenceno.Text.Trim(), GlobalUtilities.ConvertToString(ViewState["PrevRefNo"]).Trim());
            if (voucherId == 0) return 0;

            //Accounts.AdjustVoucher(id, GlobalUtilities.ConvertToString(ViewState["PrevRefNo"]).Trim(), txtreferenceno.Text.Trim(), 
              //                      GlobalUtilities.ConvertToDouble(hdnTotalAmount.Text));

            Invoice.DeleteInvicePending(txtmodule.Text, GlobalUtilities.ConvertToInt(txtmoduleid.Text));
            SaveServiceTypeDetail(id);

            UpdateSubscription();

            Custom.UpdateClientProspects("invoice", id, GlobalUtilities.ConvertToInt(txtclientid.Text), GlobalUtilities.ConvertToInt(txtserviceplanid.Text));

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
    private void SaveInvoiceProductDetails(int id, int invoiceformatversionId)
    {
        string query = "";
        if (Request.QueryString["id"] != null && invoiceformatversionId == 0) return;
        int serviceTypeId = 0;
        query = "select * from tbl_servicetype where servicetype_serviceplanid=" + GlobalUtilities.ConvertToInt(txtserviceplanid.Text);
        DataRow drst = DbTable.ExecuteSelectRow(query);
        if (drst == null)
        {
            Response.Clear();
            Response.Write("Service type does not exists, please contact administrator.");
            Response.End();
            return;
        }
        serviceTypeId = GlobalUtilities.ConvertToInt(drst["servicetype_serviceplanid"]);
        Hashtable hstbl = new Hashtable();
        hstbl.Add("invoiceid", id);
        hstbl.Add("servicetypeid", serviceTypeId);
        hstbl.Add("amount", GlobalUtilities.ConvertToDouble(txttaxableamount.Text));
        InsertUpdate obj = new InsertUpdate();
        query = "select count(*) as c from tbl_invoicedetail where invoicedetail_invoiceid=" + id;
        DataRow drc = DbTable.ExecuteSelectRow(query);
        if (GlobalUtilities.ConvertToInt(drc["c"]) > 1)
        {
            query = "delete from tbl_invoicedetail where invoicedetail_invoiceid=" + id;
            DbTable.ExecuteQuery(query);
        }
        query = "select * from tbl_invoicedetail where invoicedetail_invoiceid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            obj.InsertData(hstbl, "tbl_invoicedetail", false);
        }
        else
        {
            obj.UpdateData(hstbl, "tbl_invoicedetail", GlobalUtilities.ConvertToInt(dr["invoicedetail_invoicedetailid"]), false);
        }
    }
    private void UpdateSubscription()
    {
        int clientId = GlobalUtilities.ConvertToInt(txtclientid.Text);
        string query = "select top 1 * from tbl_subscription where subscription_clientid=" + clientId + " order by 1 desc";
        DataRow drs = DbTable.ExecuteSelectRow(query);
        if (drs == null) return;
        int subscriptionId = GlobalUtilities.ConvertToInt(drs["subscription_subscriptionid"]);
        Hashtable hstbl = new Hashtable();
        hstbl.Add("invoiceperiodfrom", txtperiodfrom.Text);
        hstbl.Add("invoiceperiodto", txtperiodto.Text);
        hstbl.Add("latestinvoicedate", txtinvoicedate.Text);
        InsertUpdate obj = new InsertUpdate();
        obj.UpdateData(hstbl, "tbl_subscription", subscriptionId);
    }
    private void SaveServiceTypeDetail(int id)
    {
        string m = Common.GetModuleName();
        string query = @"select * from tbl_invoicedetail
                       where invoicedetail_invoiceid=" + id;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        query = "delete from tbl_invoicedetail where invoicedetail_invoiceid=" + id;
        DbTable.ExecuteQuery(query);
        int stateId = GlobalUtilities.ConvertToInt(txtstateid.Text);
        if (stateId == 0)
        {
            query = "select * from tbl_client where client_clientid=" + txtclientid.Text;
            DataRow drclient = DbTable.ExecuteSelectRow(query);
            stateId = GlobalUtilities.ConvertToInt(drclient["client_stateid"]);
        }
        if (stateId == 0) stateId = 1;
        double cgstper = 0; double sgstper = 0; double igstper = 0;
        if (stateId == 1)
        {
            cgstper = 9;
            sgstper = 9;
        }
        else
        {
            igstper = 18;
        }
        double totalAmount = 0; double totaltaxableamount = 0;
        double totalcgst = 0; double totalsgst = 0; double totaligst = 0; double totalgst = 0;
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int serviceTypeId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["invoicedetail_servicetypeid"]);
            double taxableamount = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["invoicedetail_amount"]);
            double quantity = 1;
            totaltaxableamount += taxableamount;
            double cgst = 0; double sgst = 0; double igst = 0; double gst = 0;
            cgst = Math.Round(GlobalUtilities.ConvertToDouble(GlobalUtilities.FormatAmount(taxableamount * cgstper / 100.0)), 0);
            sgst = Math.Round(GlobalUtilities.ConvertToDouble(GlobalUtilities.FormatAmount(taxableamount * sgstper / 100.0)), 0);
            igst = Math.Round(GlobalUtilities.ConvertToDouble(GlobalUtilities.FormatAmount(taxableamount * igstper / 100.0)), 0);
            gst = cgst + sgst + igst;
            double subtotal = taxableamount + gst;
            totalcgst += cgst; totalsgst += sgst; totaligst += igst; totalgst += gst;
            Hashtable hstbl = new Hashtable();
            hstbl.Add("invoiceid", id);
            hstbl.Add("servicetypeid", serviceTypeId);
            hstbl.Add("cgstpercentage", cgstper);
            hstbl.Add("sgstpercentage", sgstper);
            hstbl.Add("igstpercentage", igstper);
            hstbl.Add("cgst", cgst);
            hstbl.Add("sgst", sgst);
            hstbl.Add("igst", igst);
            hstbl.Add("taxableamount", taxableamount);
            hstbl.Add("amount", taxableamount);
            hstbl.Add("gst", gst);
            hstbl.Add("quantity", quantity);
            hstbl.Add("subtotal", subtotal);

            InsertUpdate obj = new InsertUpdate();
            obj.InsertData(hstbl, "tbl_invoicedetail", false);
            totalAmount += subtotal;
        }
        query = "update tbl_invoice set invoice_totalamount=" + totalAmount + ",invoice_cgst=" + totalcgst + "," +
                "invoice_sgst=" + totalsgst + ",invoice_igst=" + totaligst + ",invoice_gst=" + totalgst + "," +
                "invoice_taxableamount=" + totaltaxableamount +
                " where invoice_invoiceid=" + id;
        DbTable.ExecuteQuery(query);
        query = "update tbl_invoice set invoice_balanceamount=invoice_totalamount-invoice_amountpaid where invoice_invoiceid=" + id;
        DbTable.ExecuteQuery(query);

        SaveProfitSharingAmount(id, totaltaxableamount);
    }
    private void SaveProfitSharingAmount(int id, double total)
    {
        string query = "select * from tbl_invoiceprofitsharingdetail where invoiceprofitsharingdetail_invoiceid=" + id;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int detailId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["invoiceprofitsharingdetail_invoiceprofitsharingdetailid"]);
            double percentage = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["invoiceprofitsharingdetail_profitpercentage"]);
            double profitAmount = GlobalUtilities.ConvertToDouble(GlobalUtilities.FormatAmount(total / 100.0 * percentage));
            query = "update tbl_invoiceprofitsharingdetail set invoiceprofitsharingdetail_profitamount=" + profitAmount + " where invoiceprofitsharingdetail_invoiceprofitsharingdetailid=" + detailId;
            DbTable.ExecuteQuery(query);
        }
    }
    //EnableControlsOnEdit_START
	private void EnableControlsOnEdit()
	{
		if (Request.QueryString["id"] == null) return;
		btngotoreceipt.Visible = true;
		btnsendinvoice.Visible = true;
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
    
	protected void btngotoreceipt_Click(object sender, EventArgs e)
	{
        //Response.Redirect("../receipt/add.aspx?invid=" + GlobalUtilities.ConvertToInt(Request.QueryString["id"]) + "&invno=" + txtinvoiceno.Text);
        Response.Redirect("../receiptvoucher/add.aspx?m=invoice&mid=" + Request.QueryString["id"]);
	}
	protected void btnaddexpenses_Click(object sender, EventArgs e)
	{
        Response.Redirect("../expenses/add.aspx?invoiceid=" + GlobalUtilities.ConvertToInt(Request.QueryString["id"]) + "&referenceno=" + txtinvoiceno.Text + "&expensesfor=Invoice");
	}
    private void updateStatusOfOtherModule(DataRow dr)
    {
        int complaintId = GlobalUtilities.ConvertToInt(dr["invoice_complaintid"]);
        int amcserviceId = GlobalUtilities.ConvertToInt(dr["invoice_amcserviceid"]);
        int amcId = GlobalUtilities.ConvertToInt(dr["invoice_amcid"]);
        int serviceId = GlobalUtilities.ConvertToInt(dr["invoice_serviceid"]);
        int saleId = GlobalUtilities.ConvertToInt(dr["invoice_salesid"]);
        string query = "";
        if (saleId > 0)
        {
            updateQuotationStatus(saleId);
        }
        else if (complaintId > 0)
        {
            query = @"update tbl_complaint set complaint_statusid=7 where complaint_complaintid=" + complaintId;
        }
        else if (amcserviceId > 0)
        {
            query = @"update tbl_amcservice set amcservice_statusid=7 where amcservice_amcserviceid=" + amcserviceId;
        }
        else if (amcId > 0)
        {
            query = @"update tbl_amc set amc_amcstatusid=3 where amc_amcid=" + amcId;
        }
        else if (serviceId > 0)
        {
            query = @"update tbl_service set service_statusid=7 where service_serviceid=" + serviceId;
        }
        if(query!="")
        DbTable.ExecuteQuery(query);
        query = @"update tbl_invoice set invoice_isupdatestatusforothermodule=1 where  invoice_invoiceid=" + GetId();
        DbTable.ExecuteQuery(query);
    }
    private void updateQuotationStatus(int salesId)
    {
        string query = @"select * from tbl_sales where sales_salesid=" + salesId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            int quoteId = GlobalUtilities.ConvertToInt(dr["sales_quotationid"]);
            query = @"update tbl_quotation set quotation_quotationstatusid=4 where quotation_quotationid=" + quoteId;
            DbTable.ExecuteQuery(query);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Common.Delete();
    } 
	protected void btnsendinvoice_Click(object sender, EventArgs e)
	{
        int id = GetId();
        FinrexInvoicePdf obj = new FinrexInvoicePdf();
        string fileName = obj.GenerateInvoice(id);
        if (fileName == "")
        {
            lblMessage.Text = "Unable to generate pdf.";
            lblMessage.Visible = true;
            return;
        }
        string mobileNos = "";
        string emailIds = "";
        if (txtdirectoremailid.Text != "")
        {
            emailIds = GlobalUtilities.CommaSeperator(emailIds, txtdirectoremailid.Text);
        }
        if (txtfinanceemailid.Text != "")
        {
            emailIds = GlobalUtilities.CommaSeperator(emailIds, txtfinanceemailid.Text);
        }
        if (emailIds == "")
        {
            lblMessage.Text = "Please enter director/finance person email id.";
            lblMessage.Visible = true;
            return;
        }
        if (txtdirectormobile.Text != "")
        {
            mobileNos = GlobalUtilities.CommaSeperator(mobileNos, txtdirectormobile.Text);
        }
        if (txtfinancemobile.Text != "")
        {
            mobileNos = GlobalUtilities.CommaSeperator(mobileNos, txtfinancemobile.Text);
        }
        if (mobileNos == "")
        {
            lblMessage.Text = "Please enter director/finance person mobile no.";
            lblMessage.Visible = true;
            return;
        }
        DataRow dr = Custom.GetInvoiceData(id);
        //RPlusWhatsAppAPI.SendMessage("", false, "invoice", id, dr, mobileNos, EnumWhatsAppMessageType.Text,
        //        "Invoice To Customer", "", "", "", "https://finstation.in/upload/temp/" + fileName, "Invoice.pdf");
        //lblMessage.Text = "Invoice has been sent to WhatsApp.";
        //lblMessage.Visible = true;

        string attachment = fileName;
        string subject = Common.GetSetting("Invoice Email Subject");
        string body = Common.GetFormattedSettingForEmail("Invoice Email", dr, true);
        Email.SaveEmailAndRedirect(emailIds, subject, body, "invoice", id, attachment);
	}
}
