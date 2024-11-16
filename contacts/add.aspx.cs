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

public partial class Contacts_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_contacts", "contactsid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillDropDown_START
			
			EnableControlsOnEdit();
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
                lblMessage.Text = "User login will be created only if email id is entered!";
                lblMessage.Visible = true;
       			//PopulateAcOnAdd_START
			
			//PopulateAcOnAdd_END
				//PopulateOnAdd_START
				DataRow drpop = CommonPage.PopulateOnAdd(form);//pop=true&popm=&popjoin=&popid=
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
                Common.PopulateAutoComplete(client, txtclientid, Common.GetQueryStringValue("clientid"));
                chkisactive.Checked = true;
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                if (txtemailid.Text.Trim() == "")
                {
                    lblMessage.Text = "User login will be created only if email id is entered!";
                    lblMessage.Visible = true;
                }
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Contacts";
        }
        else
        {
            lblPageTitle.Text = "Edit Contacts";
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
    protected void btnSaveAndView_Click(object sender, EventArgs e)
    {
        if (SaveData(false) > 0)
		{
        	Response.Redirect("~/Contacts/view.aspx");
		}
    }
    private int SaveData(bool isclose)
    {
        if (txtmobileno.Text.Trim() == "" && txtemailid.Text.Trim() == "")
        {
            lblMessage.Text = "Please enter email id OR mobile number!";
            lblMessage.Visible = true;
            return 0;
        }
        if (Common.IsValidateMobileNo(txtmobileno.Text) == false)
        {
            lblMessage.Text = "Invalid mobile number!";
            lblMessage.Visible = true;
            return 0;
        }
        if (txtemailid.Text.Trim()!="" && !txtemailid.Text.Contains("@") && !txtemailid.Text.Contains("."))
        {
            lblMessage.Text = "Invalid Email Id number!";
            lblMessage.Visible = true;
            return 0;
        }
        lblMessage.Visible = false;
        //SetCode_START//SetCode_END
        //ExtraValues_START//ExtraValues_END
        int id = 0;
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            SaveClientUser(id);
            //SaveSubTable_START
			
			//SaveSubTable_END
            //SaveFile_START
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END
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
        DeleteClientUser();
        Common.Delete();
    }
    private void DeleteClientUser()
    {
        string query = "delete from tbl_clientuser where clientuser_contactsid=" + GetId();
        DbTable.ExecuteQuery(query);
    }
    private int SaveClientUser(int id)
    {
        if (id <= 0) return 0;
        string query = "";
        query = "select * from tbl_clientuser where clientuser_contactsid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if(dr == null)
        {
            query = "select * from tbl_clientuser where clientuser_clientid=" + txtclientid.Text + 
                    " and clientuser_username='" + global.CheckInputData(txtemailid.Text) + "'";
            dr = DbTable.ExecuteSelectRow(query);
            if (dr != null)
            {
                query = "update tbl_clientuser set clientuser_contactsid=" + id + " where clientuser_clientuserid=" + GlobalUtilities.ConvertToInt(dr["clientuser_clientuserid"]);
                DbTable.ExecuteQuery(query);
            }
        }
        if (dr != null && txtemailid.Text.Trim() == "")
        {
            //DeleteClientUser();
            return GlobalUtilities.ConvertToInt(dr["clientuser_clientuserid"]);
        }
        if (txtemailid.Text.Trim() == "") return 0;// GlobalUtilities.ConvertToInt(dr["clientuser_clientuserid"]);
        int clientuserId = 0;
        if (dr != null)
        {
            Hashtable hstbl = new Hashtable();
            hstbl.Add("username", txtemailid.Text);
            hstbl.Add("name", txtcontactperson.Text);
            hstbl.Add("isexeuser", chkisexeuser.Checked ? 1 : 0);
            hstbl.Add("iswebuser", chkiswebuser.Checked ? 1 : 0);
            hstbl.Add("ismobileuser", chkismobileuser.Checked ? 1 : 0);
            hstbl.Add("isfinmessenger", chkisfinmessenger.Checked ? 1 : 0);
            hstbl.Add("isactive", chkisactive.Checked ? 1 : 0);
            InsertUpdate obj = new InsertUpdate();
            clientuserId = GlobalUtilities.ConvertToInt(dr["clientuser_clientuserid"]);
            obj.UpdateData(hstbl, "tbl_clientuser", clientuserId);
        }
        else
        {
            DataRow drclient = DbTable.GetOneRow("tbl_client", GlobalUtilities.ConvertToInt(txtclientid.Text));
            Hashtable hstbl = new Hashtable();
            hstbl.Add("contactsid", id);
            hstbl.Add("clientid", txtclientid.Text);
            hstbl.Add("username", txtemailid.Text);
            hstbl.Add("name", txtcontactperson.Text);
            hstbl.Add("isexeuser", chkisexeuser.Checked ? 1 : 0);
            hstbl.Add("iswebuser", chkiswebuser.Checked ? 1 : 0);
            hstbl.Add("ismobileuser", chkismobileuser.Checked ? 1 : 0);
            hstbl.Add("isfinmessenger", chkisfinmessenger.Checked ? 1 : 0);
            hstbl.Add("isactive", chkisactive.Checked ? 1 : 0);
            InsertUpdate obj = new InsertUpdate();
            clientuserId = obj.InsertData(hstbl, "tbl_clientuser");
            if (clientuserId > 0)
            {
                SendPassword(clientuserId);
            }
        }
        return clientuserId;
    }
    private void SendPassword(int id)
    {
        string password = Common.GeneratePassword();
        string query = "update tbl_clientuser set clientuser_password='" + password + "',clientuser_isfirstlogin=1 WHERE clientuser_clientuserid=" + id;
        DbTable.ExecuteQuery(query);
        query = "select * from tbl_clientuser " +
               "JOIN tbl_client ON client_clientid=clientuser_clientid " +
               "WHERE clientuser_clientuserid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        string subjectTemplate = "Trial Email Subject";
        string bodyTemplate = "Trial Email Body";
        if (GlobalUtilities.ConvertToInt(dr["client_subscriptionstatusid"]) == 2)
        {
            subjectTemplate = "SubscriptionPasswordMailSubject";
            bodyTemplate = "SubscriptionPasswordMail";
        }

        string body = Common.GetFormattedSettingForEmail(bodyTemplate, dr, true);
        string subject = Common.GetSetting(subjectTemplate);
        string emailid = GlobalUtilities.ConvertToString(txtemailid.Text);

        //RPlusWhatsAppAPI.SendMessage("", false, "contacts", GetId(), dr, txtmobileno.Text, EnumWhatsAppMessageType.Text,
        //        "User Login Details", "", "", "", "", "");

        Email.SaveEmailAndRedirect(emailid, subject, body, "Send Password", true);
    }
	protected void btnsendpassword_Click(object sender, EventArgs e)
	{
        if (txtemailid.Text.Trim() == "")
        {
            lblMessage.Text = "Please enter email id.";
            lblMessage.Visible = true;
            return;
        }
        int clientUserId = SaveClientUser(GetId());
        if (clientUserId > 0)
        {
            SendPassword(clientUserId);
            lblMessage.Text = "Password has been emailed to the user.";
            lblMessage.Visible = true;
        }
        else
        {
            lblMessage.Text = "Error occured while creating user ("+clientUserId+")!";
            lblMessage.Visible = true;
        }
	}
}
