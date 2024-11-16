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

public partial class ClientUser_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_clientuser", "clientuserid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.Text = "You can not add/update client user, please use contacts module instead.";
            //FillDropDown_START
			
			EnableControlsOnEdit();
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
       			//PopulateAcOnAdd_START
			
			//PopulateAcOnAdd_END
				//PopulateOnAdd_START
				DataRow drpop = CommonPage.PopulateOnAdd(form);//pop=true&popm=&popjoin=&popid=
				DataRow drtrial = CommonPage.PopulateOnAdd(form, "trialid");
				DataRow drsubscription = CommonPage.PopulateOnAdd(form, "subscriptionid");
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
                PopulateClientDetail();
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
            lblPageTitle.Text = "Add Client User";
        }
        else
        {
            lblPageTitle.Text = "Edit Client User";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    //PopulateSubGrid_START
			
			//PopulateSubGrid_END
    protected void btnSubmit_Click_NOTINUSE(object sender, EventArgs e)
    {
        SaveData(true);
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        //SetCode_START//SetCode_END
        //ExtraValues_START
		if(Request.QueryString["id"] == null)
		{
			Common.AddExtraQueryStringVal(gblData, "subscriptionid");
			Common.AddExtraQueryStringVal(gblData, "trialid");
			gblData.AddExtraValues("isfirstlogin", "1");
		}
		//ExtraValues_END
        int id = 0;
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            //SaveSubTable_START
			
			//SaveSubTable_END
            //SaveFile_START
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END
            lblMessage.Text = "Data saved successfully!";
            lblMessage.Visible = true;
            if (Request.QueryString["id"] == null)
            {
                //always send password
                SendPassword(id);
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
	private void EnableControlsOnEdit()
	{
		if (Request.QueryString["id"] == null) return;
		btnsendpassword.Visible = true;
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
    
	protected void btnsendpassword_Click(object sender, EventArgs e)
	{
        SendPassword(GetId());
	}
    private void SendPassword(int id)
    {
        if (txtusername.Text.Trim().Length < 6)
        {
            lblMessage.Text = "Length of user name should be greater than 6!";
            lblMessage.Visible = true;
            return;
        }
        string password = Common.GeneratePassword();
        string query = "update tbl_clientuser set clientuser_password='" + password + "',clientuser_isfirstlogin=1 WHERE clientuser_clientuserid=" + id;
        DbTable.ExecuteQuery(query);
        query = "select * from tbl_clientuser " +
               "JOIN tbl_client ON client_clientid=clientuser_clientid " +
               "WHERE clientuser_clientuserid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        string subjectTemplate = "Trial Email Subject";
        string bodyTemplate = "Trial Email Body";
        if (GlobalUtilities.ConvertToInt(txtsubscriptionid.Text) > 0)
        {
            subjectTemplate = "SubscriptionPasswordMailSubject";
            bodyTemplate = "SubscriptionPasswordMail";
        }

        string body = Common.GetFormattedSettingForEmail(bodyTemplate, dr, true);
        string subject = Common.GetSetting(subjectTemplate);
        string emailid = GlobalUtilities.ConvertToString(txtusername.Text);

        Email.SaveEmailAndRedirect(emailid, subject, body, "Send Password", true);
        //if (BulkEmail.SendMailFromLoginUser(emailid, subject, body, ""))
        //{
        //    lblMessage.Text = "Login detail has been sent to the customer!";
        //    lblMessage.Visible = true;
        //}
        //else
        //{
        //    lblMessage.Text = "Unable to send email to customer, please try again later!";
        //    lblMessage.Visible = true;
        //}
    }
    private void PopulateClientDetail()
    {
        int clientId = GlobalUtilities.ConvertToInt(txtclientid.Text);
        string query = "select * from tbl_clientuser where clientuser_clientid=" + clientId;
        DataRow drTrialUser = DbTable.ExecuteSelectRow(query);
        if (drTrialUser != null) return;
        //populate automatically only if no user created for this client
        DataRow dr = DbTable.GetOneRow("tbl_client", clientId);
        if (dr == null) return;
        txtname.Text = GlobalUtilities.ConvertToString(dr["client_contactperson"]);
        txtusername.Text = GlobalUtilities.ConvertToString(dr["client_emailid"]);
    }
}
