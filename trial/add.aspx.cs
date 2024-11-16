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

public partial class Trial_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_trial", "trialid");

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
				DataRow drenquiry = CommonPage.PopulateOnAdd(form, "enquiryid");
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
                BindEnquiryDetail();
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                BindCallLogButtons();
                Common.SetViewButtonLabelWithWhere(btnviewcontacts, "contacts", "contacts_clientid=" + txtclientid.Text);
                Common.SetViewButtonLabel(btncallloghistory, "calllog", "calllog_clientid=" + txtclientid.Text);
                DisableSaveBtn();
                EnableDisableByStatus();
                string query = @"select * from tbl_trialprospect where trialprospect_prospectid=4 and trialprospect_trialid=" + GetId();
                DataRow dr = DbTable.ExecuteSelectRow(query);
                if (dr != null)
                {
                    btnfemportal.Visible = true;
                }
                else
                {
                    btnfemportal.Visible = false;
                }
                if (Common.RoleId == 1 || Common.RoleId == 9)//Admin or accounts
                {
                }
                else
                {
                    btnconverttosubscription.Visible = false;
                }
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Trial";
        }
        else
        {
            lblPageTitle.Text = "Edit Trial";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_END
    }
    //PopulateSubGrid_START
			
			//PopulateSubGrid_END
    private void DisableSaveBtn()
    {
        int status = GlobalUtilities.ConvertToInt(txtsubscriptionstatusid.Text);
        if (status == 2 || status == 4)
        {
            lblMessage.Text = "You can not modify as it is in subscription!";
            lblMessage.Visible = true;
            btnSubmit.Visible = false;
            btnDelete.Visible = false;
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData(true);
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        //Himesh requested for this on 11-6-2020 in whatsapp
        string query = "select count(*) c from tbl_trial where trial_clientid=" + txtclientid.Text;
        if (Request.QueryString["id"] != null)
        {
            query += " AND trial_trialid<>" + GetId();
        }
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (GlobalUtilities.ConvertToInt(dr["c"]) > 0)
        {
            lblMessage.Text = "Trial already exists for this client!";
            lblMessage.Visible = true;
            return 0;
        }

        //SetCode_START
		if(Request.QueryString["id"] == null)
		{
			if(txttrialcode.Text == "") txttrialcode.Text = UniqueCode.GetUniqueCode(gblData, "trialcode", "TR-", 1);
		}
		//SetCode_END
        //ExtraValues_START
		if(Request.QueryString["id"] == null)
		{
			Common.AddExtraQueryStringVal(gblData, "enquiryid");
			gblData.AddExtraValues("subscriptionstatusid", "1");
			gblData.AddExtraValues("isfirstlogin", "1");
		}
		//ExtraValues_END
        int id = 0;
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            //SaveSubTable_START
			
			mctrialtrialprospect.Save(id);
			//SaveSubTable_END
            //SaveFile_START
			mfuattachment.Save(id);
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END

            //password will be sent to user when we add client user
            //if (GlobalUtilities.ConvertToInt(txtsubscriptionstatusid.Text) == 6)
            //{
            //    gblData.AddExtraValues("password", Common.GeneratePassword());
            //    SendTrialEmail(id);
            //}
            if (GlobalUtilities.ConvertToInt(Request.QueryString["enquiryid"]) > 0)
            {
                updateEnquiryStatus();
            }
            Custom.UpdateSubscriptionStatus(id, true);
            Custom.UpdateSubscriptionOnClient(id, true);
            Custom.UpdateClientProspects("trial", id, GlobalUtilities.ConvertToInt(txtclientid.Text), 0);
            //if (Request.QueryString["id"] == null)
            //{
            //    UpdateContactPreferences(id);
            //    Response.Redirect("~/clientuser/add.aspx?trialid=" + id);
            //}
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
		btntrialfeedback.Visible = true;
		btntrialexpiredmail.Visible = true;
		btnconverttosubscription.Visible = true;
		btnlogemail.Visible = true;
		btnlogsms.Visible = true;
		btnlogwhatsapp.Visible = true;
		btncallloghistory.Visible = true;
		btncompanyprofile.Visible = true;
		btnaddcontact.Visible = true;
		btnviewcontacts.Visible = true;
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

    protected void btntrialfeedback_Click(object sender, EventArgs e)
    {
        SendTrialFeedback(GetId());
    }
    protected void btntrialexpiredmail_Click(object sender, EventArgs e)
    {
        SendTrialExpiredEmail(GetId());
    }
    private void SendTrialFeedback(int id)
    {
        string query = "select * from tbl_trial " +
                       "JOIN tbl_client ON client_clientid=trial_clientid " +
                       "WHERE trial_trialid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);

        string subject = Common.GetSetting("Trial Feedback Subject");
        string body = Common.GetFormattedSettingForEmail("Trial Feedback Body", dr, true);
        string clientEmailId = GlobalUtilities.ConvertToString(dr["client_emailid"]);

        string emailType = "Feedback After Trial";
        Email.SaveEmailAndRedirect(clientEmailId, subject, body, emailType);
    }
    private void SendTrialEmail(int id)
    {
        string query = "select * from tbl_trial " +
                       "JOIN tbl_client ON client_clientid=trial_clientid " +
                       "WHERE trial_trialid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);

        string subject = Common.GetSetting("Trial Email Subject");
        string body = Common.GetFormattedSettingForEmail("Trial Email Body", dr, true);
        string clientEmailId = GlobalUtilities.ConvertToString(dr["client_emailid"]);

        string emailType = "Trial Email";
        Email.SaveEmailAndRedirect(clientEmailId, subject, body, emailType);
    }
    private void SendTrialExpiredEmail(int id)
    {
        string query = "select * from tbl_trial " +
                       "JOIN tbl_client ON client_clientid=trial_clientid " +
                       "WHERE trial_trialid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);

        string subject = Common.GetSetting("Trial Expired Mail Subject");
        string body = Common.GetFormattedSettingForEmail("Trial Expired Mail Body", dr, true);
        string clientEmailId = GlobalUtilities.ConvertToString(dr["client_emailid"]);

        string emailType = "Trial Expired Email";
        Email.SaveEmailAndRedirect(clientEmailId, subject, body, emailType);
    }
    protected void btnconverttosubscription_Click(object sender, EventArgs e)
    {
        if (GlobalUtilities.ConvertToInt(txtemployeeid.Text) == 0)
        {
            lblMessage.Text = "Please select assigned to!";
            lblMessage.Visible = true;
            txtemployeeid.Focus();
        }
        else
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
                    return;
                }
            }

            //Common.CallBack callback = new Common.CallBack(convertToSubscription_callback);
            Common.ConvertAndRedirect("ConvertTrialToSubscription", true);
            //string query = "select * from tbl_subscription WHERE subscription_trialid=" + GetId();
            //DataRow dr = DbTable.ExecuteSelectRow(query);
            //if (dr == null)
            //{
            //    Response.Redirect("~/subscription/add.aspx?trialid=" + GetId());
            //}
            //lblMessage.Text = "Status has been updated to Subscription";
            //lblMessage.Visible = true;
        }
    }
    private void convertToSubscription_callback()
    {

    }
    private void BindEnquiryDetail()
    {
        if (Request.QueryString["enquiryid"] != null)
        {
            DateTime dtEnd = DateTime.Now.AddDays(15);
            string day = dtEnd.Day.ToString();
            string m = dtEnd.Month.ToString();
            if (day.Length == 1) day = "0" + day;
            if (m.Length == 0) m = "0" + m;
            txtenddate.Text = day + "-" + m + "-" + dtEnd.Year;

            dtEnd = DateTime.Now.AddDays(30);
            day = dtEnd.Day.ToString();
            m = dtEnd.Month.ToString();
            if (day.Length == 1) day = "0" + day;
            if (m.Length == 0) m = "0" + m;
            txtwhatsappenddate.Text = day + "-" + m + "-" + dtEnd.Year;
        }
    }
    private void updateEnquiryStatus()
    {
        string query = @"update tbl_enquiry set enquiry_enquirystatusid=12 
                where enquiry_enquiryid=" + GlobalUtilities.ConvertToInt(Request.QueryString["enquiryid"]);
        DbTable.ExecuteQuery(query);
    }
    protected void btnlogemail_Click(object sender, EventArgs e)
    {
    }
    protected void btnlogsms_Click(object sender, EventArgs e)
    {
    }
    protected void btncallloghistory_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/utilities/view.aspx?m=calllog&ew=calllog_clientid~" + txtclientid.Text);
    }
    private void BindCallLogButtons()
    {
        btnlogemail.CssClass = "button btnaction btnspage";
        btnlogemail.Attributes.Add("href", "calllog/add.aspx?trialid=" + Request.QueryString["id"] + "&isemail=1&ntype=1");
        btnlogsms.CssClass = "button btnaction btnspage";
        btnlogsms.Attributes.Add("href", "calllog/add.aspx?trialid=" + Request.QueryString["id"] + "&isemail=0&ntype=2");
        btnlogwhatsapp.CssClass = "button btnaction btnspage";
        btnlogwhatsapp.Attributes.Add("href", "calllog/add.aspx?trialid=" + Request.QueryString["id"] + "&isemail=0&ntype=3");
    }
    protected void btnnewuser_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/clientuser/add.aspx?trialid=" + GetId());
    }
    protected void btnviewuser_Click(object sender, EventArgs e)
    {
        Common.RedirectToSubModuleView("clientuser", "clientuser_clientid=" + txtclientid.Text);
    }
    private void EnableDisableByStatus()
    {
        int status = GlobalUtilities.ConvertToInt(txtsubscriptionstatusid.Text);
        if (status == 1)
        {
            btntrialexpiredmail.Visible = false;
            btntrialfeedback.Visible = false;
            string strEndDate = txtenddate.Text;

            int intTotalDays = 0;// GlobalUtilities.ConvertToDate(strEndDate);
            DateTime dtEndDate = GlobalUtilities.ConvertToDateTimeFromDDMM(strEndDate);

            double dbintTotalDays = dtEndDate.Subtract(DateTime.Now).TotalDays;
            int days = GlobalUtilities.ConvertToInt(Common.GetSetting("TrailReminder"));
            if (days > 0)
            {

            }
            else
            {
                days = 8;
            }
            if (dbintTotalDays < days)
            {
                btntrialexpiredmail.Visible = true;
                btnconverttosubscription.Visible = true;
            }
            else
            {
                btntrialexpiredmail.Visible = false;
                btnconverttosubscription.Visible = false;
            }
        }
        else if (status == 2 || status==4)//subscription
        {
            //btnnewuser.Visible = false;
            btnlogemail.Visible = false;
            btnlogsms.Visible = false;
            btnconverttosubscription.Visible = false;
            btntrialexpiredmail.Visible = false;
            btntrialfeedback.Visible = false;
            btnlogwhatsapp.Visible = false;
        }
        else if (status == 3)//trial expired
        {
            //btnnewuser.Visible = false;
            btnlogemail.Visible = true;
            btnlogsms.Visible = true;
            btnconverttosubscription.Visible = true;
            btntrialexpiredmail.Visible = true;
            btntrialfeedback.Visible = true;
            btnlogwhatsapp.Visible = true;
        }
    }
    private void UpdateContactPreferences(int trialId)
    {
        string query = "select * from tbl_trialnotificationtype where trialnotificationtype_trialid=" + trialId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        if (!GlobalUtilities.IsValidaTable(dttbl)) return;
        bool issmsEnable = false;
        bool isEmailEnable = false;
        bool isWhatsAppEnable = false;
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int notificationType = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["trialnotificationtype_notificationtypeid"]);
            if (notificationType == 1)
            {
                isEmailEnable = true;
            }
            else if (notificationType == 1)
            {
                issmsEnable = true;
            }
            else if (notificationType == 1)
            {
                isWhatsAppEnable = true;
            }
        }
        query = "update tbl_contacts set contacts_isemailcommunication=" + Convert.ToInt16(isEmailEnable) + "," +
                "contacts_issmscommunication=" + Convert.ToInt16(issmsEnable) + ",contacts_iswhatsappcommunication=" + Convert.ToInt16(isWhatsAppEnable) +
                "where contacts_clientid=" + txtclientid.Text;
        DbTable.ExecuteQuery(query);
    }
	protected void btnfem_Click(object sender, EventArgs e)
	{
	}
	protected void btnfemportal_Click(object sender, EventArgs e)
	{
	}
	protected void btncompanyprofile_Click(object sender, EventArgs e)
	{
        SendCorporateProfile(GetId());
	}
    private DataRow GetTrailData(int id)
    {
        string query = "select * from tbl_trial " +
                       "JOIN tbl_client ON client_clientid=trial_clientid " +
                       "WHERE trial_trialid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        return dr;
    }
    private void SendCorporateProfile(int id)
    {
        DataRow dr = GetTrailData(id);

        string attachment = Guid.NewGuid().ToString() + "_CompanyProfile-Finrex.pdf";
        System.IO.File.Copy(Server.MapPath("~/upload/companyprofile/companyprofile.pdf"), Server.MapPath("~/upload/temp/" + attachment));

        string welcomemailSubject = Common.GetSetting("Company Profile Subject");
        string WelcomEmail = Common.GetFormattedSettingForEmail("Company Profile", dr, true);
        string emailIds = Common.GetAllContactEmailIds(GlobalUtilities.ConvertToInt(txtclientid.Text));
        Email.SaveEmailAndRedirect(emailIds, welcomemailSubject, WelcomEmail, "Trial - Company Profile", id, attachment);

        //RPlusWhatsAppAPI.SendMessage("", false, "enquiry", GetId(), dr, GlobalUtilities.ConvertToString(dr["client_mobileno"]), EnumWhatsAppMessageType.Text,
        //        "Company Profile", "", "", "", "https://finstation.in/upload/companyprofile/companyprofile.pdf", "CompanyProfile-Finrex.pdf");

        //string attachment = Guid.NewGuid().ToString() + "_CompanyProfile-Finrex.pdf";
        //System.IO.File.Copy(Server.MapPath("~/upload/companyprofile/companyprofile.pdf"), Server.MapPath("~/upload/temp/" + attachment));

        //string EmailId = GlobalUtilities.ConvertToString(dr["client_emailid"]);
        //string welcomemailSubject = Common.GetSetting("Company Profile Subject");
        //string WelcomEmail = Common.GetFormattedSettingForEmail("Company Profile", dr, true);
        //Email.SaveEmailAndRedirect(EmailId, welcomemailSubject, WelcomEmail, "Company Profile", id);
    }
	protected void btnaddcontact_Click(object sender, EventArgs e)
	{
        Common.Redirect("~/contacts/add.aspx?clientid=" + txtclientid.Text);
	}
	protected void btnviewcontacts_Click(object sender, EventArgs e)
	{
        Common.RedirectToSubModuleView("contacts", "contacts_clientid~" + txtclientid.Text);
	}
	protected void btnlogwhatsapp_Click(object sender, EventArgs e)
	{
	}
}