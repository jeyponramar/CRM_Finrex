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

public partial class Followups_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_followups", "followupsid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillDropDown_START
			
			gblData.FillDropdown(ddlfollowupactionid, "tbl_followupaction", "followupaction_action", "followupaction_followupactionid", "", "followupaction_action");
			gblData.FillDropdown(ddlfollowupstatusid, "tbl_followupstatus", "followupstatus_status", "followupstatus_followupstatusid", "", "followupstatus_status");
			EnableControlsOnEdit();
			//FillDropDown_END
            if (Request.QueryString["clientid"] != null)
            {
                int intClientId = GlobalUtilities.ConvertToInt(Request.QueryString["clientid"]);
                setClientName(intClientId);

            }
            if (Request.QueryString["id"] == null)
            {
                ddlfollowupstatusid.SelectedValue = "1";
                txtmodule.Text = Request.QueryString["module"];
                txtmid.Text = Request.QueryString["mid"];
       			//PopulateAcOnAdd_START
			
			//PopulateAcOnAdd_END
				//PopulateOnAdd_START
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
                setEmployee();
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                EnableDisable();
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Followups";
        }
        else
        {
            lblPageTitle.Text = "Edit Followups";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    //PopulateSubGrid_START
			
			//PopulateSubGrid_END
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData(true, true);
    }
    private void setClientName(int clientId)
    {
        txtclientid.Text = clientId.ToString();
        client.Text = Common.GetOneColumnData("tbl_client", clientId, "customername");
    }
    private int SaveData(bool isclose, bool isRedirect)
    {
        lblMessage.Visible = false;
        //SetCode_START//SetCode_END
        //ExtraValues_START//ExtraValues_END
        int id = 0;
        gblData.AddExtraValues("userid", Common.UserId);
        if (Request.QueryString["id"] == null)
        {
            gblData.AddExtraValues("isremoved", 0);
            if (Request.QueryString["clientid"] != null)
            {
                gblData.AddExtraValues("clientid", Request.QueryString["clientid"]);
            }

            if (Request.QueryString["mid"] != null)
            {
                gblData.AddExtraValues("mid", Request.QueryString["mid"]);
                gblData.AddExtraValues("module", Request.QueryString["m"]);
            }
        }
        if (GlobalUtilities.ConvertToInt(ddlfollowupstatusid.SelectedValue) == 2)//Activity complete
        {
            gblData.AddExtraValues("isremoved", 1);
        }
        if (txtreminderdate.Text != "")
        {
            gblData.AddExtraValues("isreminder", 1);
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
            ClosePrevTasks(id);
            ////meeting
            //if (GlobalUtilities.ConvertToInt(ddlfollowupactionid.SelectedValue) == 6 && GlobalUtilities.ConvertToInt(ddlfollowupstatusid.SelectedValue) == 1)
            //{
            //    SendEmail(id);
            //}
            SaveLastActivity(id);

            lblMessage.Text = "Data saved successfully!";
            lblMessage.Visible = true;
            //redirect only for meeting
            if (isRedirect)
            {
                if (Request.QueryString["id"] == null && Convert.ToInt32(ddlfollowupactionid.SelectedValue) == 6)//redirect only for meeting
                {
                    Response.Redirect("~/followups/add.aspx?id=" + id);
                }
                else
                {
                    CommonPage.CloseQuickAddEditWindow(Page, form, id);
                }
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
    private void SaveLastActivity()
    {

    }
    private void ClosePrevTasks(int id)
    {
        if (!chkcloseallprevioustasks.Checked) return;
        int mid = 0;
        string module = "";

        if (Request.QueryString["id"] == null)
        {
            mid = GlobalUtilities.ConvertToInt(Request.QueryString["mid"]);
            module = GlobalUtilities.ConvertToString(Request.QueryString["m"]);
        }
        else
        {
            DataRow dr = DbTable.GetOneRow("tbl_followups", id);
            mid = GlobalUtilities.ConvertToInt(dr["followups_mid"]);
            module = GlobalUtilities.ConvertToString(dr["followups_module"]);
        }
        string query = "update tbl_followups set followups_followupstatusid=2,followups_isreminder=0 " +
                       "WHERE followups_followupsid < " + id + " AND followups_mid=" + mid + " AND followups_module='" + module + "'";
        DbTable.ExecuteQuery(query);
    }
    //EnableControlsOnEdit_START
	private void EnableControlsOnEdit()
	{
		if (Request.QueryString["id"] == null) return;
		btnsendmeetingrequest.Visible = true;
		btnsendmom.Visible = true;
		btnthanksmail.Visible = true;
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
    private void setEmployee()
    {
        DataRow dr = Common.GetOneRowData("tbl_employee", Common.EmployeeId);
        if (dr == null) return;
        txtemployeeid.Text = Common.EmployeeId.ToString();
        employee.Text = GlobalUtilities.ConvertToString(dr["employee_employeename"]);
    }
    
    private void SaveLastActivity(int id)
    {
        string m = txtmodule.Text;
        if (m == "enquiry" || m == "opportunity")
        {
            string lastActivity = ddlfollowupactionid.SelectedItem.Text;
            if (txtremarks.Text != "")
            {
                lastActivity += "-" + txtremarks.Text;
            }
            Hashtable hstbl = new Hashtable();
            hstbl.Add("lastactivity", lastActivity);
            hstbl.Add("followupsdate", txtdate.Text);
            InsertUpdate obj = new InsertUpdate();
            obj.UpdateData(hstbl, "tbl_" + m, id);
        }
    }
	protected void btnsendmeetingrequest_Click(object sender, EventArgs e)
	{
        int id = SaveData(false, false);
        if (id > 0)
        {
            SendMail(false, id);
        }
	}
	protected void btnsendmom_Click(object sender, EventArgs e)
	{
        int id = SaveData(false, false);
        if (id > 0)
        {
            SendMail(true, id);
        }
	}
    private void SendMail(bool isMOM, int id)
    {
        string query = "select * from tbl_followups " +
                       "JOIN tbl_client ON client_clientid=followups_clientid " +
                       "WHERE followups_followupsid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        string subjectTemplate = "Meeting Schedule Email Subject";
        string bodyTemplate = "Meeting Schedule Email Body";
        if (isMOM)
        {
            subjectTemplate = "Minutes Of Meeting Subject";
            bodyTemplate = "Minutes Of Meeting Mail";
        }
        string subject = Common.GetSetting(subjectTemplate);
        string body = Common.GetFormattedSettingForEmail(bodyTemplate, dr, true);
        string clientEmailId = Common.GetOneColumnData("tbl_client", GlobalUtilities.ConvertToInt(txtclientid.Text), "emailid");

        string emailType = "";
        if (isMOM)
        {
            emailType = "MOM";
        }
        else
        {
            emailType = "Meeting Request";
        }
        Email.SaveEmailAndRedirect(clientEmailId, subject, body, emailType, id);

    }
    private void EnableDisable()
    {
        if (GlobalUtilities.ConvertToInt(ddlfollowupactionid.SelectedValue) == 6)//meeting
        {
            btnsendmeetingrequest.Visible = true;
            btnsendmom.Visible = true;
            btnthanksmail.Visible = true;
        }
        else
        {
            btnsendmeetingrequest.Visible = false;
            btnsendmom.Visible = false;
            btnthanksmail.Visible = false;
        }
    }
    protected void ddlfollowupactionid_Changed(object sender, EventArgs e)
    {
        EnableDisable();
    }
	protected void btnthanksmail_Click(object sender, EventArgs e)
	{
        int id = SaveData(false, false);
        if (id > 0)
        {
            SendThanksMail(id);
        }
	}
    private void SendThanksMail(int id)
    {
        string query = "select * from tbl_followups " +
                       "JOIN tbl_client ON client_clientid=followups_clientid " +
                       "WHERE followups_followupsid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        
        string subject = Common.GetSetting("Thanks Mail After Meeting Subject");
        string body = Common.GetFormattedSettingForEmail("Thanks Mail After Meeting Body", dr, true);
        string clientEmailId = GlobalUtilities.ConvertToString(dr["client_emailid"]);

        string emailType = "Meeting Thanks";
        Email.SaveEmailAndRedirect(clientEmailId, subject, body, emailType, id);
    }
}
