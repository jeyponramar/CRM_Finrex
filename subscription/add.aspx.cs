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

public partial class Subscription_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_subscription", "subscriptionid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            //FillDropDown_START
			
			EnableControlsOnEdit();
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
       			//PopulateAcOnAdd_START
			
			//PopulateAcOnAdd_END
				//PopulateOnAdd_START
				DataRow drpop = CommonPage.PopulateOnAdd(form);//pop=true&popm=&popjoin=&popid=
				DataRow dropportunity = CommonPage.PopulateOnAdd(form, "opportunityid");
				DataRow drtrial = CommonPage.PopulateOnAdd(form, "trialid");
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
                BindTrialDetail();
                if (GlobalUtilities.ConvertToInt(Request.QueryString["subscriptionparentid"]) > 0)
                {
                    SetSubscriptionCode();
                    txtsubscriptioncode.Text = "";
                }
                
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END

                CheckAllControlsVisible(GlobalUtilities.ConvertToBool(gblData._CurrentRow["subscription_isrenew"]));
                BindCallLogButtons();
                int status = GlobalUtilities.ConvertToInt(txtsubscriptionstatusid.Text);
                if (status == 6)//subscribed
                {
                    btnthanksmail.Visible = true;
                }
                else if (status == 4)//subscription expired
                {
                    btnsubscriptionexpiredmail.Visible = true;
                    //btncreateuser.Visible = false;
                    //btndecline.Visible = false;
                    btnlogemail.Visible = true;
                    btnlogsms.Visible = true;
                    btnlogwhatsapp.Visible = true;
                }
                Common.SetViewButtonLabelWithWhere(btnviewcontacts, "contacts", "contacts_clientid=" + txtclientid.Text);
                Common.SetViewButtonLabel(btncallloghistory, "calllog", "calllog_clientid=" + txtclientid.Text);
                InsertUpdate obj = new InsertUpdate();
                string query = @"select * from tbl_subscriptionprospects where subscriptionprospects_prospectid=4 and subscriptionprospects_subscriptionid=" + GetId();
                DataRow dr = obj.ExecuteSelectRow(query);
                if (dr != null)
                {
                    btnfemportal.Visible = true;
                }
                else
                {
                    btnfemportal.Visible = false;
                }
                int clientId = GlobalUtilities.ConvertToInt(txtclientid.Text);
                query = @"select * from tbl_client where client_clientid="+clientId;
                DataRow drclient = DbTable.ExecuteSelectRow(query);
                if (Common.RoleId != 1)//admin
                {
                    btnapiconfig.Visible = false;
                }
                if (!GlobalUtilities.ConvertToBool(drclient["client_isapienabled"]))
                {
                    btnapiconfig.Visible = false;
                }
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        if (Common.RoleId > 1)
        {
            //btnrenewsubscription.Visible = false;
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Subscription";
        }
        else
        {
            lblPageTitle.Text = "Edit Subscription";
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
    
    private int SaveData(bool isclose)
    {
        if (Request.QueryString["id"] == null)
        {
            //multiple subscription restriction
            int clientId = GlobalUtilities.ConvertToInt(txtclientid.Text);
            string query = "select * from tbl_subscription where subscription_clientid=" + clientId;
            DataRow drs = DbTable.ExecuteSelectRow(query);
            if (drs != null)
            {
                lblMessage.Text = "Subscription has been already created for this client!";
                lblMessage.Visible = true;
                return 0;
            }
        }
        lblMessage.Visible = false;
        //if (!IsValid()) return 0;
        //SetCode_START
		if(Request.QueryString["id"] == null)
		{
			if(txtsubscriptioncode.Text == "") txtsubscriptioncode.Text = UniqueCode.GetUniqueCode(gblData, "subscriptioncode", "SUBC-", 1);
		}
		//SetCode_END
        //ExtraValues_START
		if(Request.QueryString["id"] == null)
		{
			Common.AddExtraQueryStringVal(gblData, "opportunityid");
			gblData.AddExtraValues("isfirstlogin", "1");
			Common.AddExtraQueryStringVal(gblData, "trialid");
			Common.AddExtraQueryStringVal(gblData, "enquiryid");
			gblData.AddExtraValues("subscriptionstatusid", "2");
		}
		//ExtraValues_END
        
        SetaddExtraValueForParentSubscriptionCode();
        int id = 0;
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            //SaveSubTable_START
			
			mcsubscriptionsubscriptionservices.Save(id);
			mcsubscriptionsubscriptionprospects.Save(id);
			//SaveSubTable_END
            //SaveFile_START
			mfuattachment.Save(id);
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END
            UpdateSubscription();
            UpdateOpportunityStatus();
            UpdateClientFeatureFlag(id);
            Custom.UpdateSubscriptionStatus(id);
            Custom.UpdateSubscriptionOnClient(id, false);
            Custom.UpdateClientProspects("subscription", id, GlobalUtilities.ConvertToInt(txtclientid.Text), 0);
            SendAuditWelcomeEmail(id);
            //if (GlobalUtilities.ConvertToInt(txtsubscriptionstatusid.Text) == 1)
            //{
            //    SendTrialEmail(id);
            //}
            Custom.SaveFutureFeedback(GlobalUtilities.ConvertToInt(txtclientid.Text));
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
    private void SendAuditWelcomeEmail(int id)
    {
        //As discussed with Himesh Sir, Audit welcome email has been stopped
        return;
        string query = "";
        DataRow dr = gblData._CurrentRow;
        query = "select * from tbl_subscriptionprospects where subscriptionprospects_subscriptionid=" + id + 
                    " and subscriptionprospects_prospectid=8";
        DataRow drprospect = DbTable.ExecuteSelectRow(query);
        if (drprospect == null) return;
        query = "select * from tbl_client where client_clientid=" + GlobalUtilities.ConvertToInt(txtclientid.Text);
        DataRow drclient = DbTable.ExecuteSelectRow(query);
        string toEmailId = GlobalUtilities.ConvertToString(drclient["client_emailid"]);
        string subject = "BankScan: Audit Questionnaire by Finrex Treasury Advisors LLP";
        string body = Common.GetSetting("BankAudit_Email_Welcome");
        BulkEmail.SendMail_Alert(toEmailId, subject, body, "");
    }
	private void EnableControlsOnEdit()
	{
		if (Request.QueryString["id"] == null) return;
		btncallloghistory.Visible = true;
		btnfemportal.Visible = true;
		btnaddcontact.Visible = true;
		btnviewcontacts.Visible = true;
		btnDelete.Visible = true;
        btnupdatekycdetails.Visible = true;
        btnlogwhatsapp.Visible = true;
        btnapiconfig.Visible = true;
	}
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
    private void UpdateClientDetail()
    {
        int clientId = GlobalUtilities.ConvertToInt(txtclientid.Text);
        Hashtable hstbl = new Hashtable();
        hstbl.Add("startdate", txtstartdate.Text);
        hstbl.Add("enddate", txtenddate.Text);
        hstbl.Add("subscriptionstatusid", txtsubscriptionstatusid.Text);
        InsertUpdate obj = new InsertUpdate();
        obj.UpdateData(hstbl, "tbl_client", clientId);
    }
    private void BindTrialDetail()
    {
        if (Request.QueryString["trialid"] != null)
        {
            int trialid = GlobalUtilities.ConvertToInt(Request.QueryString["trialid"]);
            //mcsubscriptionsubscriptionnotificationtype.Bind(trialid, "notificationtype", "notificationtype", "opportunitynotificationtype", "opportunity");
            //mcsubscriptionsubscriptioncurrency.Bind(trialid, "currencyexposure", "currencyexposure", "opportunitycurrency", "opportunity");
            mcsubscriptionsubscriptionservices.Bind(trialid, "service", "service", "opportunityservices", "opportunity");
        }

    }
    //protected void btnconverttoinvoice_Click(object sender, EventArgs e)
    //{
    //    Common.RedirectToSubModuleAddEdit("invoice");
    //}
    //protected void btnrenewsubscription_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("add.aspx?isrenew=true&subscriptionparentid=" + GlobalUtilities.ConvertToInt(Request.QueryString["id"]));
    //}
    private void SetSubscriptionCode()
    {
        if (Request.QueryString["subscriptionparentid"] == null)
        {
            txtsubscriptionparentid.Text = "0";
            return;
        }
        int parentId = GlobalUtilities.ConvertToInt(Request.QueryString["subscriptionparentid"]);
        InsertUpdate obj = new InsertUpdate();
        string query = @"select top 1 * from tbl_subscription 
                        JOIN tbl_client ON client_clientid=subscription_clientid
                        LEFT JOIN tbl_subscriptionstatus ON subscriptionstatus_subscriptionstatusid = subscription_subscriptionstatusid
                        where subscription_subscriptionid=" + parentId;
        DataRow dr = obj.ExecuteSelectRow(query);
        if (dr != null)
        {
            //populate main page
            gblData.PopulateForm(dr, form);
            //populate all multi checkbox
            
            //mcsubscriptionsubscriptionnotificationtype.Bind(parentId);
            //mcsubscriptionsubscriptioncurrency.Bind(parentId);
            mcsubscriptionsubscriptionservices.Bind(parentId);

            string AmcEndDate = Convert.ToString(dr["subscription_enddate"]);
            DateTime dtAmcEndDate = Convert.ToDateTime((dr["subscription_enddate"]));
            dtAmcEndDate = dtAmcEndDate.AddDays(1);

            txtstartdate.Text = GlobalUtilities.ConvertToDate(dtAmcEndDate);
            string strNewAmcStartDate = GlobalUtilities.ConvertToDate(dtAmcEndDate);
            //txtstartdate.Text = GlobalUtilities.ConvertToDate(GlobalUtilities.ConvertMMDateToDD(dtAmcEndDate));
            //string strNewAmcStartDate = GlobalUtilities.ConvertToDate(GlobalUtilities.ConvertMMDateToDD(dtAmcEndDate));
            if (strNewAmcStartDate.Contains("/"))
            {
                txtstartdate.Text = strNewAmcStartDate.Replace("/", "-");
            }

            if (GlobalUtilities.ConvertToInt(dr["subscription_subscriptionparentid"]) > 0)
            {
                txtsubscriptionparentid.Text = Convert.ToString(dr["subscription_subscriptionparentid"]);
            }
            else
            {
                txtsubscriptionparentid.Text = Convert.ToString(Request.QueryString["subscriptionparentid"]);

            }
        }
        txtenddate.Text = "";
    }
    private void SetaddExtraValueForParentSubscriptionCode()
    {
        int subscriptionId = GlobalUtilities.ConvertToInt(Request.QueryString["subscriptionparentid"]);
        if (subscriptionId == 0) return;
        DataRow dr = Common.GetOneRowData("tbl_subscription", subscriptionId);

        if (dr != null)
        {
            string parentamccode = GlobalUtilities.ConvertToString(dr["subscription_parentcode"]);
            if (parentamccode == "")
            {
                parentamccode = GlobalUtilities.ConvertToString(dr["subscription_subscriptioncode"]);
            }
            gblData.AddExtraValues("parentcode", parentamccode);
        }
    }
    private void UpdateSubscription()
    {
        if (GlobalUtilities.ConvertToInt(Request.QueryString["subscriptionparentid"]) > 0)
        {
            string query = "";
            InsertUpdate obj = new InsertUpdate();
            query = "UPDATE tbl_subscription SET subscription_isrenew=1 WHERE subscription_subscriptionid=" + GlobalUtilities.ConvertToInt(Request.QueryString["subscriptionparentid"]);
            obj.ExecuteQuery(query);
        }
    }
    private void UpdateOpportunityStatus()
    {
        int subscriptionstatus = GlobalUtilities.ConvertToInt(txtsubscriptionstatusid.Text);
        int subscriptionStatusId = 0;
        int enquiryStatusId = 0;
        if (subscriptionstatus == 6)//subscribed
        {
            subscriptionStatusId = 2;
            enquiryStatusId = 5;
        }
        string query = "";
        int trialId = GlobalUtilities.ConvertToInt(Request.QueryString["trialId"]);
        if (trialId == 0)
        {
            trialId = GlobalUtilities.ConvertToInt(txttrialid.Text);
        }
        if (subscriptionStatusId > 0)
        {
            query = "update tbl_trial set trial_subscriptionstatusid=" + subscriptionStatusId + " where trial_trialid=" + trialId;
            DbTable.ExecuteQuery(query);
        }
        if (enquiryStatusId > 0)
        {
            query = "update tbl_enquiry set enquiry_enquirystatusid=" + enquiryStatusId +
                    " where enquiry_enquiryid=(select top 1 trial_enquiryid from tbl_trial where trial_trialid=" + trialId + ")";
            DbTable.ExecuteQuery(query);
        }

    }

    private void CheckAllControlsVisible(bool isrenewed)
    {
        int status = GlobalUtilities.ConvertToInt(txtsubscriptionstatusid.Text);
        if (isrenewed)
        {
            //btnrenewsubscription.Visible = false;
            btnSubmit.Visible = false;
            //btnconverttoinvoice.Visible = false;
            lblMessage.Visible = true;
            lblMessage.Text = "You can't edit this Subscription";
        }
        else
        {
            if (status == 1)//trial
            {
                //btnconverttoinvoice.Visible = false;
                //btndecline.Visible = false;
                //btnrenewsubscription.Visible = false;
                btnlogemail.Visible = true;
                btnlogsms.Visible = true;
                btnlogwhatsapp.Visible = true;
            }
            else if (status == 2)
            {
                btnlogemail.Visible = true;
                btnlogsms.Visible = true;
                btnlogwhatsapp.Visible = true;
                string strEndDate = txtenddate.Text;

                int intTotalDays = 0;// GlobalUtilities.ConvertToDate(strEndDate);
                DateTime dtEndDate = GlobalUtilities.ConvertToDateTimeFromDDMM(strEndDate);

                double dbintTotalDays = dtEndDate.Subtract(DateTime.Now).TotalDays;
                int days = GlobalUtilities.ConvertToInt(Common.GetSetting("SubscriptionReminder"));
                if (days > 0)
                {

                }
                else
                {
                    days = 8;
                }
                if (dbintTotalDays < days)
                {
                    //btnrenewsubscription.Visible = true;
                    //btnconverttoinvoice.Visible = false;
                    //btndecline.Visible = true;
                }
                else
                {
                    //btnrenewsubscription.Visible = false;
                    //btndecline.Visible = false;
                }
            }
            
        }
    }
    //protected void btndecline_Click(object sender, EventArgs e)
    //{
    //    txtsubscriptionstatusid.Text = "5";
    //    subscriptionstatus.Text = "Declined";
    //    SaveData(false);
    //    lblMessage.Text = "Subscribtion status has been changed to Declined!";
    //    lblMessage.Visible = true;
    //}
   
	protected void btnlogemail_Click(object sender, EventArgs e)
	{
	}
	protected void btnlogsms_Click(object sender, EventArgs e)
	{
	}
    private void BindCallLogButtons()
    {
        btnlogemail.CssClass = "button btnaction btnspage";
        btnlogemail.Attributes.Add("href", "calllog/add.aspx?subscriptionid=" + Request.QueryString["id"]+"&isemail=1&ntype=1");
        btnlogsms.CssClass = "button btnaction btnspage";
        btnlogsms.Attributes.Add("href", "calllog/add.aspx?subscriptionid=" + Request.QueryString["id"] + "&isemail=0&ntype=2");

        btnlogwhatsapp.CssClass = "button btnaction btnspage";
        btnlogwhatsapp.Attributes.Add("href", "calllog/add.aspx?subscriptionid=" + Request.QueryString["id"] + "&isemail=0&ntype=3");
    }
	protected void btncallloghistory_Click(object sender, EventArgs e)
	{
        Response.Redirect("~/utilities/view.aspx?m=calllog&ew=calllog_subscriptionid~" + GetId());
	}
	protected void btnthanksmail_Click(object sender, EventArgs e)
	{
        SendThanksEmail(GetId());
	}
    private void SendThanksEmail(int id)
    {
        DataRow dr = GetSubscriptionData(id);
        string welcomemailSubject = Common.GetSetting("Subscription Thanks Email Subject");
        string WelcomEmail = Common.GetFormattedSettingForEmail("Subscription Thanks Email Body", dr, true);
        string toEmailId = GlobalUtilities.ConvertToString(dr["client_emailid"]);
        Email.SaveEmailAndRedirect(toEmailId, welcomemailSubject, WelcomEmail, "Subscription Thanks", id);
    }
    private DataRow GetSubscriptionData(int id)
    {
        string query = "select * from tbl_subscription " +
                       "JOIN tbl_client ON client_clientid=subscription_clientid " +
                       "WHERE subscription_subscriptionid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        return dr;
    }
	protected void btnsubscriptionexpiredmail_Click(object sender, EventArgs e)
	{
        SendSubscriptionExpiredEmail(GetId());
	}
    private void SendSubscriptionExpiredEmail(int id)
    {
        string query = "select * from tbl_subscription " +
                       "JOIN tbl_client ON client_clientid=subscription_clientid " +
                       "WHERE subscription_subscriptionid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);

        string subject = Common.GetSetting("Subscription Expired Mail Subject");
        string body = Common.GetFormattedSettingForEmail("Subscription Expired Mail Body", dr, true);
        string clientEmailId = GlobalUtilities.ConvertToString(dr["client_emailid"]);

        string emailType = "Subscription Expired Email";
        Email.SaveEmailAndRedirect(clientEmailId, subject, body, emailType);
    }
	protected void btncreateuser_Click(object sender, EventArgs e)
	{
        Response.Redirect("~/clientuser/add.aspx?subscriptionid=" + GetId());
	}
	protected void btnviewuser_Click(object sender, EventArgs e)
	{
        Common.RedirectToSubModuleView("clientuser", "clientuser_clientid=" + txtclientid.Text);
	}
	protected void btnfemportal_Click(object sender, EventArgs e)
	{
	}
    protected void btnaddcontact_Click(object sender, EventArgs e)
    {
        Common.Redirect("~/contacts/add.aspx?clientid=" + txtclientid.Text);
    }
    protected void btnviewcontacts_Click(object sender, EventArgs e)
    {
        Common.RedirectToSubModuleView("contacts", "contacts_clientid~" + txtclientid.Text);
    }
	protected void btnupdatekycdetails_Click(object sender, EventArgs e)
	{
        Common.Redirect("~/client/updateclientkycdetail.aspx?id=" + GlobalUtilities.ConvertToInt(txtclientid.Text));
	}
	protected void btnlogwhatsapp_Click(object sender, EventArgs e)
	{
	}
    private void UpdateClientFeatureFlag(int id)
    {
        string query = "";
        int isApiEnabled = 0;
        int clientId = GlobalUtilities.ConvertToInt(txtclientid.Text);
        query = @"select * from tbl_subscriptionservices where subscriptionservices_subscriptionid=" + id + 
                " and subscriptionservices_serviceid=20";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            isApiEnabled = 1;
        }
        query = "update tbl_client set client_isapienabled=" + isApiEnabled + " where client_clientid=" + clientId;
        DbTable.ExecuteQuery(query);
    }
    protected void btnapiconfig_Click(object sender, EventArgs e)
    {
        int clientId = GlobalUtilities.ConvertToInt(txtclientid.Text);
        Common.Redirect("~/client/apiconfig.aspx?id=" + clientId);
    }
}
