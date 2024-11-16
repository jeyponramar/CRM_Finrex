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

public partial class ProformaInvoice_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_proformainvoice", "proformainvoiceid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillDropDown_START
			
			gblData.FillDropdown(ddlsetupfortermsandconditionid, "tbl_setupfortermsandcondition", "setupfortermsandcondition_name", "setupfortermsandcondition_setupfortermsandconditionid", "setupfortermsandcondition_termsandconditiontypeid=5", "setupfortermsandcondition_name");
			EnableControlsOnEdit();
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
       			//PopulateAcOnAdd_START
			
			//PopulateAcOnAdd_END
				//PopulateOnAdd_START
				DataRow drpop = CommonPage.PopulateOnAdd(form);//pop=true&popm=&popjoin=&popid=
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
                lnkPrint.Visible = false;
                txtsalesstatusid.Text = "1";
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END

                Common.PopulateAutoComplete(leadby, txtleadbyid, GlobalUtilities.ConvertToInt(gblData._CurrentRow["proformainvoice_leadbyid"]));
                Common.PopulateAutoComplete(advisoryby, txtadvisorybyid, GlobalUtilities.ConvertToInt(gblData._CurrentRow["proformainvoice_advisorybyid"]));
                Common.PopulateAutoComplete(meeting1, txtmeeting1id, GlobalUtilities.ConvertToInt(gblData._CurrentRow["proformainvoice_meeting1id"]));
                Common.PopulateAutoComplete(meeting2, txtmeeting2id, GlobalUtilities.ConvertToInt(gblData._CurrentRow["proformainvoice_meeting2id"]));
                Common.PopulateAutoComplete(closedby, txtclosedbyid, GlobalUtilities.ConvertToInt(gblData._CurrentRow["proformainvoice_closedbyid"]));



                lnkPrint.Visible = true;
                lnkPrint.NavigateUrl = "~/proformainvoice/proformainvoiceFormat.aspx?piid=" + GetId();
                int salesstatusId = GlobalUtilities.ConvertToInt(gblData._CurrentRow["proformainvoice_salesstatusid"]);
                visibleHideButtonAccordingToStatus(salesstatusId);
            }
            //CallPopulateSubGrid_START
			
			PopulateSubGrid();
			PopulateSubGrid2();
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Proforma Invoice";
        }
        else
        {
            lblPageTitle.Text = "Edit Proforma Invoice";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_END
    }
    //PopulateSubGrid_START
			
	private void PopulateSubGrid()
	{
		SubGrid sgrid = new SubGrid();
		sgrid.Populate(gblData, ltproformainvoiceproductdetail, proformainvoiceproductdetail_param, proformainvoiceproductdetail_setting);
	}
	private void PopulateSubGrid2()
	{
		SubGrid sgrid = new SubGrid();
		sgrid.Populate(gblData, ltprofitsharingdetail, profitsharingdetail_param, profitsharingdetail_setting);
	}
			//PopulateSubGrid_END
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData(true);
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        //SetCode_START
		if(Request.QueryString["id"] == null)
		{
			if(txtproformainvoiceno.Text == "") txtproformainvoiceno.Text = UniqueCode.GetUniqueCode(gblData, "proformainvoiceno", "PINV-", 1);
		}
		//SetCode_END
        //ExtraValues_START//ExtraValues_END
        int id = 0;

        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            //SaveSubTable_START
			
			gblData.SaveSubTable(id, gblData, proformainvoiceproductdetail_setting);
			gblData.SaveSubTable(id, gblData, profitsharingdetail_setting);
			//SaveSubTable_END
            //SaveFile_START
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END
            //PopulateSubGrid2();
            SaveServiceTypeDetail(id);

            PopulateSubGrid();
            gblData.PopulateForm(form, id);

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
    private void SaveServiceTypeDetail(int id)
    {
        string m = Common.GetModuleName();
        string query = @"select * from tbl_proformainvoiceproductdetail 
                       where proformainvoiceproductdetail_proformainvoiceid=" + id;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        query = "delete from tbl_proformainvoicedetail where proformainvoicedetail_proformainvoiceid=" + id;
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
            int serviceTypeId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["proformainvoiceproductdetail_servicetypeid"]);
            double rate = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["proformainvoiceproductdetail_amount"]);
            double quantity = 1;
            double taxableamount = rate;
            totaltaxableamount += taxableamount;
            double cgst = 0; double sgst = 0; double igst = 0; double gst = 0;
            cgst = GlobalUtilities.ConvertToDouble(GlobalUtilities.FormatAmount(taxableamount * cgstper / 100.0));
            sgst = GlobalUtilities.ConvertToDouble(GlobalUtilities.FormatAmount(taxableamount * sgstper / 100.0));
            igst = GlobalUtilities.ConvertToDouble(GlobalUtilities.FormatAmount(taxableamount * igstper / 100.0));
            gst = cgst + sgst + igst;
            double amount = taxableamount + gst;
            totalcgst += cgst; totalsgst += sgst; totaligst += igst; totalgst += gst;
            Hashtable hstbl = new Hashtable();
            hstbl.Add("proformainvoiceid", id);
            hstbl.Add("servicetypeid", serviceTypeId);
            hstbl.Add("cgstpercentage", cgstper);
            hstbl.Add("sgstpercentage", sgstper);
            hstbl.Add("igstpercentage", igstper);
            hstbl.Add("cgst", cgst);
            hstbl.Add("sgst", sgst);
            hstbl.Add("igst", igst);
            hstbl.Add("taxableamount", taxableamount);
            hstbl.Add("gst", gst);
            hstbl.Add("quantity", quantity);
            hstbl.Add("amount", amount);
            
            InsertUpdate obj = new InsertUpdate();
            obj.InsertData(hstbl, "tbl_proformainvoicedetail", false);
            totalAmount += amount;
        }
        query = "update tbl_proformainvoice set proformainvoice_totalamount=" + totalAmount + ",proformainvoice_cgst=" + totalcgst + "," +
                "proformainvoice_sgst=" + totalsgst + ",proformainvoice_igst=" + totaligst + ",proformainvoice_gst=" + totalgst + "," +
                "proformainvoice_taxableamount=" + totaltaxableamount +
                " where proformainvoice_proformainvoiceid=" + id;
        DbTable.ExecuteQuery(query);
        SaveProfitSharingAmount(id, totaltaxableamount);
    }
    private void SaveProfitSharingAmount(int id, double total)
    {
        string query = "select * from tbl_profitsharingdetail where profitsharingdetail_proformainvoiceid=" + id;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int detailId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["profitsharingdetail_profitsharingdetailid"]);
            double percentage = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["profitsharingdetail_profitpercentage"]);
            double profitAmount = GlobalUtilities.ConvertToDouble(GlobalUtilities.FormatAmount(total / 100.0 * percentage));
            query = "update tbl_profitsharingdetail set profitsharingdetail_profitamount=" + profitAmount + " where profitsharingdetail_profitsharingdetailid=" + detailId;
            DbTable.ExecuteQuery(query);
        }
    }
    //EnableControlsOnEdit_START
	private void EnableControlsOnEdit()
	{
		if (Request.QueryString["id"] == null) return;
		btnconverttoinvoice.Visible = true;
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
    
	protected void btnconverttoinvoice_Click(object sender, EventArgs e)
	{
        DateTime dtInvoiceDate = DateTime.Now;
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
        string query = @"select top 1 * from tbl_invoice 
                            where cast(invoice_invoicedate as date)>=cast('" + fromDate + "' as date) and cast(invoice_invoicedate as date)<=cast('" + toDate + @"' as date)
                            order by invoice_count desc";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            count = GlobalUtilities.ConvertToInt(dr["invoice_count"]) + 1;
        }

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
        string invoiceNo = strcount + "/" + strFromYear + "-" + strToYear;
        Hashtable hstbl = new Hashtable();
        hstbl.Add("invoicedate", "getdate()");
        hstbl.Add("invoiceno", invoiceNo);
        hstbl.Add("referenceno", invoiceNo);
        hstbl.Add("count", count);
        int invoiceId = Common.ConvertAndRedirect("ConvertProformaInvoiceToInvoice", true, hstbl, false);
        query = @"insert into tbl_invoiceprofitsharingdetail(invoiceprofitsharingdetail_sequence,invoiceprofitsharingdetail_invoiceid,
                invoiceprofitsharingdetail_employeeid,invoiceprofitsharingdetail_profitpercentage,invoiceprofitsharingdetail_profitamount)
                select profitsharingdetail_sequence," + invoiceId + @",
                profitsharingdetail_employeeid,profitsharingdetail_profitpercentage,profitsharingdetail_profitamount
                from tbl_profitsharingdetail where profitsharingdetail_proformainvoiceid=" + GetId();
        DbTable.ExecuteQuery(query);

        int voucherId = Accounts.SaveVoucherDetail("invoice", client.Text, invoiceNo, GlobalUtilities.ConvertToDate(DateTime.Now),
                     GlobalUtilities.ConvertToDouble(txttotalamount.Text), VoucherType.Sales, invoiceId, "invoicedetail",
                     invoiceNo, "");

        Invoice.DeleteInvicePending("invoice", invoiceId);

        Response.Redirect("~/invoice/add.aspx?id=" + invoiceId);
	}
    private void visibleHideButtonAccordingToStatus(int salesstatusId)
    {
        if (salesstatusId == 4)
        {
            btnconverttoinvoice.Visible = false;
            btnSubmit.Visible = false;
            btnSubmitAndView.Visible = false;
            btnDelete.Visible = false;
            lblMessage.Text = "You Can't Edit this Proforma Invoice because is converted into Invoice";
            lblMessage.Visible = true;
        }
    }
}
