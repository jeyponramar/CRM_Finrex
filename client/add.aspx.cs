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

public partial class Client_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_client", "clientid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Params["pageurl"].ToString().Contains("advancedreportkyc.aspx"))
            {
                Response.Redirect("~/client/updateclientkycdetail.aspx?id=" + Request.QueryString["id"]);
            }
            //FillDropDown_START
			
			gblData.FillDropdown(ddlindustrytypesid, "tbl_industrytypes", "industrytypes_industrytypes", "industrytypes_industrytypesid", "", "industrytypes_industrytypes");
			gblData.FillDropdown(ddlcontacttypeid, "tbl_contacttype", "contacttype_contacttype", "contacttype_contacttypeid", "", "contacttype_contacttype");
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
                Common.SetViewButtonLabelWithWhere(btnviewcontacts, "contacts", "contacts_clientid=" + GetId());

                ViewState["OldLedgerName"] = txtcustomername.Text;
                Common.SetViewButtonLabel(btnviewcompetitor, "clientcompetitor");
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        CommonPage.SetPageTitle(Page, lblPageTitle, "Client");
        //PageTitle_END
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
        	Response.Redirect("~/Client/view.aspx");
		}
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        //Himesh requested for this on 11-6-2020 in whatsapp
        string query = "select count(*) c from tbl_client where (client_mobileno='" + txtmobileno.Text + "' OR client_emailid='" + txtemailid.Text + "')";
        if (Request.QueryString["id"] != null)
        {
            query += " AND client_clientid<>" + GetId();
        }
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (GlobalUtilities.ConvertToInt(dr["c"]) > 0)
        {
            lblMessage.Text = "Client already exists with the given mobileno or email id!";
            lblMessage.Visible = true;
            return 0;
        }

        //SetCode_START
		if(Request.QueryString["id"] == null)
		{
			if(txtcustomercode.Text == "") txtcustomercode.Text = UniqueCode.GetUniqueCode(gblData, "customercode", "C-", 1);
		}
		//SetCode_END
        //ExtraValues_START//ExtraValues_END
        int id = 0;
        int ledgerId = Accounts.SaveLedger(txtcustomername.Text, LedgerGroup.SundryDebtor, GlobalUtilities.ConvertToString(ViewState["OldLedgerName"]), "", 0, 
                       " " , LedgerType.Customer);
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
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            //SaveSubTable_START
			
			//SaveSubTable_END
            //SaveFile_START
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END

            //Common.SaveContact(id,txtcustomername.Text, txtcontactperson.Text, txtemailid.Text, txtmobileno.Text, txtlandlineno.Text, "", txtwebsite.Text, " ", 0);
            Contact.SaveMainContact(id, txtcontactperson.Text, GlobalUtilities.ConvertToInt(txtdesignationid.Text),
                                    txtemailid.Text, txtmobileno.Text, txtlandlineno.Text);
            //SaveAdditionalContacts(id, txtcustomername.Text);

            Custom.SaveFutureFeedback(id);

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
		btnaddcontact.Visible = true;
		btnviewcontacts.Visible = true;
		btntrial.Visible = true;
		btnsubscription.Visible = true;
		btnaddcompetitor.Visible = true;
		btnviewcompetitor.Visible = true;
		btnupdatekycdetails.Visible = true;
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
	protected void btnclienthistory_Click(object sender, EventArgs e)
	{
	}
    private void SaveAdditionalContacts(int clientid,string companyname)
    {
        string query = "select * from tbl_additionalcontactdetails WHERE additionalcontactdetails_clientid=" + clientid;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            DataRow dr=dttbl.Rows[i];
            string name = GlobalUtilities.ConvertToString(dr["additionalcontactdetails_name"]);
            int designationid = GlobalUtilities.ConvertToInt(dr["additionalcontactdetails_designationid"]);
            string telephoneno = GlobalUtilities.ConvertToString(dr["additionalcontactdetails_telephoneno"]);
            string mobile = GlobalUtilities.ConvertToString(dr["additionalcontactdetails_mobile"]);
            string email = GlobalUtilities.ConvertToString(dr["additionalcontactdetails_email"]);

            Common.SaveContact(clientid,companyname, name, email, mobile, telephoneno, "", "", "", designationid);
        }
    }
    
	protected void btnmoredetail_Click(object sender, EventArgs e)
	{
        string query = "select * from tbl_client where client_clientid=" + GetId();
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return;
        query = "select top 1 * from tbl_subscription where subscription_clientid=" + GetId() + " order by subscription_subscriptionid desc";
        DataRow drSubscription = DbTable.ExecuteSelectRow(query);
        if (drSubscription == null) return;
        int subscriptionId = GlobalUtilities.ConvertToInt(drSubscription["subscription_subscriptionid"]);
        if (subscriptionId == 0) return;
        Common.Redirect("~/subscription/add.aspx?id=" + subscriptionId);
	}
	protected void btnaddcontact_Click(object sender, EventArgs e)
	{
        Common.Redirect("~/contacts/add.aspx?clientid=" + GetId());
	}
	protected void btnviewcontacts_Click(object sender, EventArgs e)
	{
        Common.RedirectToSubModuleView("contacts");
	}
	protected void btntrial_Click(object sender, EventArgs e)
	{
        Common.RedirectToSubModuleView("trial");
	}
	protected void btnsubscription_Click(object sender, EventArgs e)
	{
        Common.RedirectToSubModuleView("subscription");
	}
	protected void btnaddcompetitor_Click(object sender, EventArgs e)
	{
        Common.Redirect("~/clientcompetitor/add.aspx?clientid=" + GetId());
	}
	protected void btnviewcompetitor_Click(object sender, EventArgs e)
	{
        Common.RedirectToSubModuleView("clientcompetitor");
	}
	protected void btnupdatekycdetails_Click(object sender, EventArgs e)
	{
        Common.Redirect("~/client/updateclientkycdetail.aspx?id=" + GetId());
	}
}
