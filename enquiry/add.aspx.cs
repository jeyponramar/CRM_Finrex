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

public partial class Enquiry_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_enquiry", "enquiryid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillDropDown_START
			
			gblData.FillDropdown(ddlindustrytypesid, "tbl_industrytypes", "industrytypes_industrytypes", "industrytypes_industrytypesid", "", "industrytypes_industrytypes");
			EnableControlsOnEdit();
			//FillDropDown_END
            
            //gblData.FillDropdown(ddlpriorityid, "tbl_priority", "priority_priority", "priority_priorityid", "", "priority_priority");

            if (Request.QueryString["id"] == null)
            {
       			//PopulateAcOnAdd_START
			
			//PopulateAcOnAdd_END
				//PopulateOnAdd_START
				DataRow drpop = CommonPage.PopulateOnAdd(form);//pop=true&popm=&popjoin=&popid=
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
                DataRow drEmployee = DbTable.GetOneRow("tbl_employee", Common.EmployeeId);
                if (drEmployee != null)
                {
                    employee.Text = GlobalUtilities.ConvertToString(drEmployee["employee_employeename"]);
                    txtemployeeid.Text = GlobalUtilities.ConvertToString(drEmployee["employee_employeeid"]);
                }
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                //grid.Visible = true;
                h_telecallingid.Text = GlobalUtilities.ConvertToString(gblData._CurrentRow["enquiry_telecallingid"]);
                
                int enquiryStatusId = GlobalUtilities.ConvertToInt(gblData._CurrentRow["enquiry_enquirystatusid"]);
                hideButton(enquiryStatusId);
               
                ViewState["FollowupDate"] = txtfollowupsdate.Text;
                ViewState["EmployeeId"] = txtemployeeid.Text;

                //if (enquiryStatusId == 1 || enquiryStatusId == 8)
                //{
                //    btncompanyprofile.Visible = true;
                //    btnwelcomeemail.Visible = true;
                //}
                //else
                //{
                //    btncompanyprofile.Visible = false;
                //    btnwelcomeemail.Visible = false;
                //}
                if (Common.RoleId == 1)
                {
                    //btnUpdateMobile.Visible = true;
                }
            }
            //trfollowupsdate.Visible = false;
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        CommonPage.SetPageTitle(Page, lblPageTitle, "Enquiry");
        //PageTitle_START
        btnwelcomeemail.Visible = false;
    }
    //PopulateSubGrid_START
			
			//PopulateSubGrid_END
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsValid())
        {
            SaveData(true);
        }
    }
    protected void btnSaveAndView_Click(object sender, EventArgs e)
    {
        if (SaveData(false) > 0)
        {
            Response.Redirect("~/Enquiry/view.aspx");
        }
    }
    private bool IsValid()
    {
        if(GlobalUtilities.ConvertToInt(txtclientid.Text)>0)return true;

        //if (txtcompanyname.Text == "")
        //{
        //    lblMessage.Text = "Please fill the customer detail ";
        //    lblMessage.Visible = true;
        //    return false;
        //}
        //check customer name
        string query = "select * from tbl_client where client_customername='" + txtcompanyname.Text + "'";
        int oldClientId = GlobalUtilities.ConvertToInt(txtclientid.Text);
        if (Request.QueryString["id"] != null)
        {
            query += " AND client_clientid<>" + oldClientId;
        }
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            lblMessage.Text = "Customer already exists!";
            lblMessage.Visible = true;
            return false;
        }
        //check mobile no
        query = "select * from tbl_client where client_mobileno='" + txtmobileno.Text + "'";
        if (Request.QueryString["id"] != null)
        {
            query += " AND client_clientid<>" + oldClientId;
        }
        dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            lblMessage.Text = "Mobile number already mapped with another customer, please verify!";
            lblMessage.Visible = true;
            return false;
        }
        //check landline no
        //if (txtlandlineno.Text.Trim() != "")
        //{
        //    query = "select * from tbl_client where client_landlineno='" + txtlandlineno.Text + "'";
        //    if (Request.QueryString["id"] != null)
        //    {
        //        query += " AND client_clientid<>" + oldClientId;
        //    }
        //    dr = DbTable.ExecuteSelectRow(query);
        //    if (dr != null)
        //    {
        //        lblMessage.Text = "Landline number already mapped with another customer, please verify!";
        //        lblMessage.Visible = true;
        //        return false;
        //    }
        //}
        //check email id
        query = "select * from tbl_client where client_emailid='" + txtemailid.Text + "'";
        if (Request.QueryString["id"] != null)
        {
            query += " AND client_clientid<>" + oldClientId;
        }
        dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            lblMessage.Text = "Email Id already mapped with another customer, please verify!";
            lblMessage.Visible = true;
            return false;
        }
        return true;
    }
    private int SaveData(bool isclose)
    {
        if (txtcompanyname.Text == "")
        {
            lblMessage.Text = "Please enter customer name.";
            txtcompanyname.Focus();
            lblMessage.Visible = true;
            return 0;
        }
        lblMessage.Visible = false;
        if (employee.Text != "" && txtassigneddate.Text == "")
        {
            lblMessage.Text = "Please enter assigned date!";
            lblMessage.Visible = true;
            return 0;
        }
        if (employee.Text == "" && txtassigneddate.Text != "")
        {
            lblMessage.Text = "Please select assigned to!";
            lblMessage.Visible = true;
            return 0;
        }
        string query = "select * from tbl_enquiry where enquiry_emailid='" + global.CheckInputData(txtemailid.Text) + "'";
        if (Request.QueryString["id"] != null)
        {
            query += " and enquiry_enquiryid<>" + GetId();
        }
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            lblMessage.Text = "Enquiry already created against this email id!";
            lblMessage.Visible = true;
            return 0;
        }
        query = "select * from tbl_enquiry where enquiry_mobileno='" + global.CheckInputData(txtmobileno.Text) + "'";
        if (Request.QueryString["id"] != null)
        {
            query += " and enquiry_enquiryid<>" + GetId();
        }
        dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            lblMessage.Text = "Enquiry already created against this mobile number!";
            lblMessage.Visible = true;
            return 0;
        }
        //SetCode_START
		if(Request.QueryString["id"] == null)
		{
			if(txtenquiryno.Text == "") txtenquiryno.Text = UniqueCode.GetUniqueCode(gblData, "enquiryno", "E-", 1);
		}
		//SetCode_END
        //ExtraValues_START
		if(Request.QueryString["id"] == null)
		{
			Common.AddExtraQueryStringVal(gblData, "telecallingId");
		}
		//ExtraValues_END
        int id = 0;
        int mkpersonid=GlobalUtilities.ConvertToInt(txtemployeeid.Text);
        if (txtenquirystatusid.Text == "1" && mkpersonid > 0 || txtenquirystatusid.Text == "0" && mkpersonid>0)
        {
            
            gblData.AddExtraValues("enquirystatusid", "8");
            
        }
        else if (Request.QueryString["id"] == null)
        {
            gblData.AddExtraValues("enquirystatusid", "1");
        }
        //add client
        //if (Request.QueryString["id"] == null)
        //{
        //    int clientId = SaveClient();
        //    gblData.AddExtraValues("clientid", clientId);
        //}
        if (GlobalUtilities.ConvertToInt(txtclientid.Text) == 0)
        {
            int campaignId = 0;
            //int clientId = Custom.SaveClient(txtcompanyname.Text,txtemailid.Text,txtcontactperson.Text,txtlandlineno.Text,
            //              txtmobileno.Text,txtwebsite.Text,GlobalUtilities.ConvertToInt(txtdesignationid.Text),campaignId,
            //              GlobalUtilities.ConvertToInt(txtstateid.Text), GlobalUtilities.ConvertToInt(txtareaid.Text), GlobalUtilities.ConvertToInt(txtexposureid.Text),
            //              GlobalUtilities.ConvertToInt(txtbusinessid.Text), GlobalUtilities.ConvertToInt(ddlindustrytypesid.SelectedValue));

            //stateid set to 0 because of KYC requirement

            int stateId = 0;// GlobalUtilities.ConvertToInt(txtstateid.Text);
            int areaId = 0;// GlobalUtilities.ConvertToInt(txtareaid.Text);

            int clientId = Custom.SaveClient(txtcompanyname.Text, txtemailid.Text, txtcontactperson.Text, "",
                          txtmobileno.Text, "", GlobalUtilities.ConvertToInt(txtdesignationid.Text), campaignId,
                          stateId, areaId, 
                          GlobalUtilities.ConvertToInt(txtexposureid.Text),
                          GlobalUtilities.ConvertToInt(txtbusinessid.Text), GlobalUtilities.ConvertToInt(ddlindustrytypesid.SelectedValue));
            if (clientId == 0)
            {
                lblMessage.Text = "Unable to save client detail!";
                lblMessage.Visible = true;
                return 0;
            }
            else if (clientId < 0)
            {
                lblMessage.Text = "Client Name Already Exists, Please select the client.";
                lblMessage.Visible = true;
                return 0;
            }
            else
            {
                gblData.AddExtraValues("clientid", clientId);
            }
        }
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            TaskDAO task = new TaskDAO();
            task.CreateTask("Enquiry", id, TaskType.Enquiry);

            //SaveSubTable_START
			
			//SaveSubTable_END
            //SaveFile_START
			mfuattachment.Save(id);
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END
            //addFollowup(id);
            //save email reminders

            EmailReminder.SaveReminder("Enquiry Escalation Reminder",id, Employee.GetEmailId(), txtenquirydate.Text,
                                "enquirystatusid", GlobalUtilities.ConvertToInt(txtenquirystatusid.Text));
            //if (enquirystatus.Text == "Open" || enquirystatus.Text == "")//open
            //{
            //    SendWelcomeEmail(id);
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
		btntrial.Visible = true;
		btncanceled.Visible = true;
		btnreject.Visible = true;
		btncompanyprofile.Visible = true;
		btnwelcomeemail.Visible = true;
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
    private int GetquotationId()
    {
        string query = @"select top 1 * from tbl_quotation where quotation_enquiryid=" + GetId() + " order by quotation_quotationid DESC";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        int quotationId = 0;
        if (dr != null)
        {
            quotationId = GlobalUtilities.ConvertToInt(dr["quotation_quotationid"]);
        }
        return quotationId;
    }
    
	protected void btnwon_Click(object sender, EventArgs e)
	{
        int quoteId = GetquotationId();
        if (quoteId == 0)
        {
            lblMessage.Text = "Please create quotation first for this enquiry";
            lblMessage.Visible = true;
            return;
        }
        string query = "update tbl_quotation set quotation_isapproved=1 WHERE quotation_quotationid=" + quoteId;
        DbTable.ExecuteQuery(query);

        //update status
        Common.UpdateStatus("enquirystatusid", 5);

	}
    protected void btncancel_Click(object sender, EventArgs e)
	{
        enquirystatus.Text = "Cancelled";
        txtenquirystatusid.Text = "Cancelled";
        lblMessage.Visible = true;
        lblMessage.Text = "Enquiry will be Cancelled.";
	}
    private void hideButton(int statusid)
    {
        if (statusid == 4 || statusid == 5 || statusid == 6 || statusid == 7 || statusid == 2 || statusid == 3 || statusid == 12)
        {
            lblMessage.Visible = true;
            //btnSubmit.Visible = false;
            btnDelete.Visible = false;
            btncanceled.Visible = false;
            btnreject.Visible = false;
            //btncompanyprofile.Visible = false;

            //Followups.EnableAddlink = false;
            if (statusid == 6)
            {
                btncanceled.Visible = false;
                //lblMessage.Text = "You can't edit this enquiry once its Cancelled at Opportunity";
            }
            //else if (statusid == 4)
            //    lblMessage.Text = "You can't edit this enquiry once its Converted into Opportunity.";
            //else if (statusid == 5)
            //    lblMessage.Text = "You can't edit this enquiry once its Converted Into Subscription";
            //else if (statusid == 7)
            //    lblMessage.Text = "You can't edit this enquiry once its Rejected at Opportunity";
            //else if (statusid == 2)
            //    lblMessage.Text = "You can't edit this enquiry once its Rejected";
            //else if (statusid == 3)
            //    lblMessage.Text = "You can't edit this enquiry once its Cancelled";
            //else if (statusid == 12)
            //    lblMessage.Text = "You can't edit this enquiry once its Trial";
            if (Common.RoleId != 1)
            {
                client.Enabled = false;
                if (txtcompanyname.Text.Trim() != "") txtcompanyname.Enabled = false;
                if (txtcontactperson.Text.Trim() != "") txtcontactperson.Enabled = false;
                if (designation.Text.Trim() != "") designation.Enabled = false;
                if (txtmobileno.Text.Trim() != "") txtmobileno.Enabled = false;
                if (txtemailid.Text.Trim() != "") txtemailid.Enabled = false;
            }
        }
        if (Common.RoleId == 1 && Request.QueryString["id"] != null)
        {
            btnDelete.Visible = true;
        }
    }
	
    private void removeFollowupsAddActivityLink()
    {
        string query = @"select top 1 * from tbl_followups where followups_followupstatusid=1 and 
                        followups_module='enquiry' and followups_mid=" + GetId() + " order by followups_followupsid desc";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            Followups.EnableAddlink = false;
        }
    }
    private void addFollowup(int EnquiryId)
    {
        if (GlobalUtilities.ConvertToInt(txtemployeeid.Text) > 0)
        {

            if (ViewState["FollowupDate"].ToString() == txtfollowupsdate.Text)
            {
                return;
            }
            
            //New Followup when date modified and first set the follpwupdate
            Hashtable hstbl = new Hashtable();
            hstbl.Add("subject", "Followup");
            hstbl.Add("followupactionid", 8);
            hstbl.Add("followupstatusid", 1);
            hstbl.Add("date", txtfollowupsdate.Text);
            hstbl.Add("module", "enquiry");
            hstbl.Add("mid", EnquiryId);
            //hstbl.Add("clientid", txtclientid.Text);
            hstbl.Add("employeeid", txtemployeeid.Text);
            hstbl.Add("userid", GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId")));
            InsertUpdate obj = new InsertUpdate();
            int folId = obj.InsertData(hstbl, "tbl_followups", true);
        }
    }
	protected void btncanceled_Click(object sender, EventArgs e)
	{
        if (txtremarks.Text == "")
        {
            lblMessage.Text = "Please enter remark for this cancellation.";
            lblMessage.Visible = true;
            return;
        }
        enquirystatus.Text = "Cancelled";
        txtenquirystatusid.Text = "3";
        Common.UpdateStatus("enquirystatusid", 3);
        lblMessage.Text = "Status has been updated to Cancelled";
        lblMessage.Visible = true;
	}
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Common.Delete();
    }
	protected void btnreject_Click(object sender, EventArgs e)
	{
        if (txtremarks.Text.Trim() == "")
        {
            lblMessage.Text = "Please enter remark for this Rejected.";
            lblMessage.Visible = true;
            return;
        }
        enquirystatus.Text = "Rejected";
        txtenquirystatusid.Text = "2";
        Common.UpdateStatus("enquirystatusid", 2);
        lblMessage.Text = "Status has been updated to Rejected";
        lblMessage.Visible = true;
	}
	protected void btncompanyprofile_Click(object sender, EventArgs e)
	{
        SendCorporateProfile(GetId());
	}
	protected void btnwelcomeemail_Click(object sender, EventArgs e)
	{
        SendWelcomeEmail(GetId());
	}
    private void SendCorporateProfile(int id)
    {
        DataRow dr = Custom.GetEnquiryData(id);
        //RPlusWhatsAppAPI.SendMessage("", false, "enquiry", GetId(), dr, txtmobileno.Text, EnumWhatsAppMessageType.Text,
        //        "Company Profile", "", "", "", "https://finstation.in/upload/companyprofile/companyprofile.pdf", "CompanyProfile-Finrex.pdf");

        string attachment = Guid.NewGuid().ToString() + "_CompanyProfile-Finrex.pdf";
        System.IO.File.Copy(Server.MapPath("~/upload/companyprofile/companyprofile.pdf"), Server.MapPath("~/upload/temp/" + attachment));

        string welcomemailSubject = Common.GetSetting("Company Profile Subject");
        string WelcomEmail = Common.GetFormattedSettingForEmail("Company Profile", dr, true);
        string emailIds = Common.GetAllContactEmailIds(GlobalUtilities.ConvertToInt(txtclientid.Text));
        Email.SaveEmailAndRedirect(emailIds, welcomemailSubject, WelcomEmail, "Company Profile", id, attachment);
    }
    private void SendWelcomeEmail(int id)
    {
        DataRow dr = Custom.GetEnquiryData(id);
        string welcomemailSubject = Common.GetSetting("Welcome Email Subject");
        string WelcomEmail = Common.GetFormattedSettingForEmail("Welcome Email Body", dr, true);
        Email.SaveEmailAndRedirect(txtemailid.Text, welcomemailSubject, WelcomEmail, "New Enquiry - Welcome", id);
    }
    
	protected void btntrial_Click(object sender, EventArgs e)
	{
        if (GlobalUtilities.ConvertToInt(txtemployeeid.Text) == 0)
        {
            lblMessage.Text = "Please assign a Sales Person!";
            lblMessage.Visible = true;
            txtemployeeid.Focus();
        }
        else
        {
            string query = "select * from tbl_trial WHERE trial_enquiryid=" + GetId();
            DataRow dr = DbTable.ExecuteSelectRow(query);
            int trialid = 0;
            if (dr == null)
            {
                Response.Redirect("~/trial/add.aspx?enquiryid=" + GetId());
            }
            else
            {
                trialid = GlobalUtilities.ConvertToInt(dr["trial_trialid"]);
                Response.Redirect("~/trial/add.aspx?id=" + trialid);
            }
            lblMessage.Text = "Status has been updated to Trial";
            lblMessage.Visible = true;
        }
	}

    protected void btnUpdateMobile_Click(object sender, EventArgs e)
    {
        Hashtable hstbl = new Hashtable();
        InsertUpdate obj = new InsertUpdate();
        hstbl.Add("mobileno", txtmobileno.Text);
        obj.UpdateData(hstbl, "tbl_enquiry", GetId());
        InsertUpdate obj2 = new InsertUpdate();
        hstbl = new Hashtable();
        hstbl.Add("mobileno", txtmobileno.Text);
        obj2.UpdateData(hstbl, "tbl_client", GlobalUtilities.ConvertToInt(txtclientid.Text));
        lblMessage.Text = "Mobile No. updated successfully!";
        lblMessage.Visible = true;
    }
}
