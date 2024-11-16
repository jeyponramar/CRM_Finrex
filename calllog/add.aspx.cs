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

public partial class CallLog_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_calllog", "calllogid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSubmit.Text = "Send";
        if (!IsPostBack)
        {
            //FillDropDown_START
			
			gblData.FillDropdown(ddlbulksmstemplateid, "tbl_bulksmstemplate", "bulksmstemplate_templatename", "bulksmstemplate_bulksmstemplateid", "", "bulksmstemplate_templatename");
			gblData.FillDropdown(ddlbulkemailtemplateid, "tbl_bulkemailtemplate", "bulkemailtemplate_templatename", "bulkemailtemplate_bulkemailtemplateid", "", "bulkemailtemplate_templatename");
			EnableControlsOnEdit();
			//FillDropDown_END
            gblData.FillDropdown(ddlwhatsapptemplateid, "tbl_whatsapptemplate", "whatsapptemplate_templatename", "whatsapptemplate_whatsappmessagecategoryid=1");
            if (Request.QueryString["id"] == null)
            {
       			//PopulateAcOnAdd_START
			
			//PopulateAcOnAdd_END
				//PopulateOnAdd_START
				DataRow drpop = CommonPage.PopulateOnAdd(form);//pop=true&popm=&popjoin=&popid=
				DataRow drsubscription = CommonPage.PopulateOnAdd(form, "subscriptionid");
				DataRow drtrial = CommonPage.PopulateOnAdd(form, "trialid");
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
                txtnotificationtypeid.Text = Common.GetQueryString("ntype");
                if (NotificationTypeId == 1)
                {
                    txtnotificationtypeid.Text = "1";
                    trattachment.Visible = true;
                    txtemailid.Text = GetAdditionalContact(true);
                }
                else if (NotificationTypeId == 2)
                {
                    txtnotificationtypeid.Text = "2";
                    trattachment.Visible = false;
                    txtmobileno.Text = GetAdditionalContact(false);
                }
                else if (NotificationTypeId == 3)
                {
                    txtnotificationtypeid.Text = "3";
                    trattachment.Visible = false;
                    txtmobileno.Text = GetAdditionalContact(false);
                    txtemailid.Text = "";
                }
               
                CheckIsAlreadySent();                
                BindSentBy();
                if (NotificationTypeId == 1)
                {
                    string signature = Common.GetUserSignature();
                    txtmessage.Text = "<br/><br/>" + signature;

                    //txtemailid.Text = Contact.GetCommaSepEmailIds(GlobalUtilities.ConvertToInt(txtclientid.Text));
                    tremailid.Visible = true;
                }
                else
                {
                    //txtmobileno.Text = Contact.GetCommaSepMobileNos(GlobalUtilities.ConvertToInt(txtclientid.Text));
                    txtmessage.Width = 400;
                    txtmessage.Height = 150;
                    trmobileno.Visible = true;
                    //txtmobileno.MaxLength = 160;
                }
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
            EnableDisableControls();
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Call Log";
        }
        else
        {
            lblPageTitle.Text = "Edit Call Log";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    private int NotificationTypeId
    {
        get
        {
            return GlobalUtilities.ConvertToInt(txtnotificationtypeid.Text);
        }
    }
    //PopulateSubGrid_START
			
			//PopulateSubGrid_END
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData(true);
    }
    private bool IsVaild()
    {
        int notificationType = GlobalUtilities.ConvertToInt(txtnotificationtypeid.Text);
        if (notificationType == 1)
        {
            if (txtsubject.Text == "")
            {
                lblMessage.Text = "Please enter subject";
                lblMessage.Visible = true;
                return false;
            }
        }
        else if (notificationType == 3)//whatsapp
        {
            string templateMessage = txtwhatsapptemplatemessage.Text;
            int totalvariables = GlobalUtilities.GetStringOccurencesCount(templateMessage, "{{");
            int totalvals = GlobalUtilities.GetStringOccurencesCount(txtwhatsappvariables.Text, "|");
            if (totalvariables != totalvals)
            {
                lblMessage.Text = "WhatsApp template values not matching.";
                lblMessage.Visible = true;
                return false;
            }
        }
        if (notificationType != 3)
        {
            if (txtmessage.Text == "")
            {
                lblMessage.Text = "Please enter message";
                lblMessage.Visible = true;
                return false;
            }
        }
        if (notificationType == 1)
        {
            if (Common.IsValidCommaSepEmailId(txtemailid.Text) == false)
            {
                lblMessage.Text = "Invalid Email Id!";
                lblMessage.Visible = true;
                return false;
            }
        }
        else if (notificationType == 2)
        {
            if (Common.IsValidCommaSepMobileNo(txtmobileno.Text) == false)
            {
                lblMessage.Text = "Invalid Mobile Number!";
                lblMessage.Visible = true;
                return false;
            }
        }
        return true;
    }
    private int SaveData(bool isclose)
    {
        if (!IsVaild()) return 0;
        lblMessage.Visible = false;
        //SetCode_START//SetCode_END
        //ExtraValues_START//ExtraValues_END
        int id = 0;
        gblData.AddExtraValues("sentdate", "dbo.GetDate()");
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            
            //SaveSubTable_START
			
			//SaveSubTable_END
            //SaveFile_START
			mfuattachment.Save(id);
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END
            int issentStatus = 3;
            int notificationType = GlobalUtilities.ConvertToInt(txtnotificationtypeid.Text);
            if (notificationType == 1)
            {
                if (SendEmail(id))
                {
                    issentStatus = 2;
                }
            }
            else if (notificationType == 2)
            {
                if (SendSMS())
                {
                    issentStatus = 2;
                }
            }
            else if (notificationType == 3)
            {
                if (SendWhatsApp())
                {
                    issentStatus = 2;
                }
            }
            string query = "update tbl_calllog set calllog_emailsmssentstatusid=" + issentStatus + " where calllog_calllogid=" + id;
            DbTable.ExecuteQuery(query);

            if (issentStatus == 2)
            {
                query = "update tbl_subscription set subscription_lastcalllogsent=getdate() where subscription_subscriptionid=" + txtsubscriptionid.Text;
                DbTable.ExecuteQuery(query);
            }
            if (issentStatus == 2)
            {
                if (notificationType == 1)
                {
                    lblMessage.Text = "Email sent successfully!";
                }
                else if (notificationType == 2)
                {
                    lblMessage.Text = "SMS sent successfully!";
                }
                else if (notificationType == 3)
                {
                    lblMessage.Text = "WhatsApp message sent successfully!";
                }
            }
            else
            {
                if (notificationType == 1)
                {
                    lblMessage.Text = "Unable to send Email!";
                }
                else if (notificationType == 2)
                {
                    lblMessage.Text = "Unable to send SMS!";
                }
                else if (notificationType == 3)
                {
                    lblMessage.Text = "Unable to send WhatsApp message!";
                }
            }
            
            lblMessage.Visible = true;
            //CommonPage.CloseQuickAddEditWindow(Page, form, id);
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
    private void EnableDisableControls()
    {
        if (NotificationTypeId == 1)
        {
            trbulkemailtemplateid.Visible = true;
            notificationtype.Text = "Email";
            txtnotificationtypeid.Text = "1";
            trsubject.Visible = true;
            txtmessage.CssClass = "htmleditor";
            txtmessage.Height = 220;
            trccemailid.Visible = true;
            trbccemailid.Visible = true;
            trmessage.Visible = true;
        }
        else if (NotificationTypeId == 2)
        {
            trbulksmstemplateid.Visible = true;
            notificationtype.Text = "SMS";
            txtnotificationtypeid.Text = "2";
            trsubject.Visible = false;
            trmessage.Visible = true;
        }
        else if (NotificationTypeId == 3)
        {
            notificationtype.Text = "WhatsApp";
            txtnotificationtypeid.Text = "3";
            trsubject.Visible = false;
            trwhatsappvariables.Visible = true;
            trwhatsapptemplatemessage.Visible = true;
            trwhatsapptemplate.Visible = true;
            trmobileno.Visible = true;
        }
    }
    protected void ddlemailtemplate_changed(object sender, EventArgs e)
    {
        DataRow dr = DbTable.GetOneRow("tbl_bulkemailtemplate", GlobalUtilities.ConvertToInt(ddlbulkemailtemplateid.SelectedValue));
        if (dr == null) return;
        txtsubject.Text = GlobalUtilities.ConvertToString(dr["bulkemailtemplate_subject"]);
        string signature = Common.GetUserSignature();
        string message = GlobalUtilities.ConvertToString(dr["bulkemailtemplate_message"]);
        string query = "select * from tbl_client WHERE client_clientid=" + txtclientid.Text;
        DataRow drClient = DbTable.ExecuteSelectRow(query);
        message = Common.GetFormattedSettingForEmail(message, drClient, false);
        txtmessage.Text = message + "<br/><br/>" + signature;
    }
    protected void ddlsmstemplate_changed(object sender, EventArgs e)
    {
        DataRow dr = DbTable.GetOneRow("tbl_bulksmstemplate", GlobalUtilities.ConvertToInt(ddlbulksmstemplateid.SelectedValue));
        if (dr == null) return;
        txtmessage.Text = GlobalUtilities.ConvertToString(dr["bulksmstemplate_message"]);
    }
    private bool SendEmail(int id)
    {
        string backuppersonEmailId = "";
        string managerEmailId = "";
        int employeeId = Common.EmployeeId;
        string query = "select * from tbl_employee where employee_employeeid=(select employee_backuppersonid from tbl_employee where employee_employeeid=" + employeeId + ")";
        DataRow drbackup = DbTable.ExecuteSelectRow(query);
        if (drbackup != null)
        {
            backuppersonEmailId = GlobalUtilities.ConvertToString(drbackup["employee_emailid"]);
        }
        query = "select * from tbl_employee where employee_employeeid=(select employee_managerid from tbl_employee where employee_employeeid=" + employeeId + ")";
        DataRow drManager= DbTable.ExecuteSelectRow(query);
        if (drManager != null)
        {
            managerEmailId = GlobalUtilities.ConvertToString(drManager["employee_emailid"]);
        }
        string cc = "";
        if (backuppersonEmailId != "")
        {
            cc = backuppersonEmailId;
        }
        if (managerEmailId != "")
        {
            if (cc == "")
            {
                cc = managerEmailId;
            }
            else
            {
                cc += "," + managerEmailId;
            }
        }
        string attachment = "";
        query = "select * from tbl_calllog where calllog_calllogid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        attachment = GlobalUtilities.ConvertToString(dr["calllog_attachment"]);
        attachment = Email.GetCommaSepAttachment(attachment, "calllog");
        bool issent = BulkEmail.SendMailFromLoginUser(txtemailid.Text, txtsubject.Text, txtmessage.Text, attachment, txtccemailid.Text, txtbccemailid.Text);
        return issent;
    }
    private bool SendSMS()
    {
        bool issent = SMS.SendSMS(txtmobileno.Text, txtmessage.Text);
        return issent;
    }
    private bool SendWhatsApp()
    {
        bool issent = RPlusWhatsAppAPI.SendMessage("", false, "calllog", GetId(), null, txtmobileno.Text, EnumWhatsAppMessageType.Text,
                ddlwhatsapptemplateid.SelectedItem.Text, "", "", "", "", "", txtwhatsappvariables.Text);

        return issent;
    }
    private void BindSentBy()
    {
        int employeeId=Common.EmployeeId;
        DataRow dr = DbTable.GetOneRow("tbl_employee", employeeId);
        txtemployeeid.Text = employeeId.ToString();
        if (dr != null)
        {
            employee.Text = GlobalUtilities.ConvertToString(dr["employee_employeename"]);
        }
    }
    private void CheckIsAlreadySent()
    {
        int clientId = GlobalUtilities.ConvertToInt(txtclientid.Text);
        string query = "select * from tbl_calllog where cast(calllog_sentdate as date)=cast(getdate() as date) AND " +
                       " calllog_clientid=" + clientId;
        if (txtnotificationtypeid.Text != "")
        {
            query += " AND calllog_notificationtypeid=" + txtnotificationtypeid.Text;
        }
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            int status = GlobalUtilities.ConvertToInt(dr["calllog_emailsmssentstatusid"]);
            if (status == 2)
            {
                if (GlobalUtilities.ConvertToInt(txtnotificationtypeid.Text) == 1)
                {
                    lblMessage.Text = "You have already sent call log EMAIL for this client today, please check in call log history";
                }
                else
                {
                    lblMessage.Text = "You have already sent call log SMS for this client today, please check in call log history";
                }
                lblMessage.Visible = true;
            }
        }
    }
    private string GetAdditionalContact(bool isEmail)
    {
        int clientId = GlobalUtilities.ConvertToInt(txtclientid.Text);
        if (clientId == 0) return "";
        string emailsms = "";

        string query = "select * from tbl_client where client_clientid="+clientId;
        DataRow drclient = DbTable.ExecuteSelectRow(query);
        if (isEmail)
        {
            emailsms = GlobalUtilities.ConvertToString(drclient["client_emailid"]);
        }
        else
        {
            emailsms = GlobalUtilities.ConvertToString(drclient["client_mobileno"]);
        }

        query = "select * from tbl_contacts where contacts_clientid=" + clientId;
        //if (isEmail)
        //{
        //    query += " and isnull(contacts_isemailcommunication,0)=1";
        //    emailsms=emailId;
        //}
        //else
        //{
        //    query += " and isnull(contacts_issmscommunication,0)=1";
        //    emailsms = mobileNo;
        //}
        DataTable dttbl = DbTable.ExecuteSelect(query);

        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (isEmail)
            {
                string email = GlobalUtilities.ConvertToString(dttbl.Rows[i]["contacts_emailid"]);
                if (email != txtemailid.Text)
                {
                    if (emailsms == "")
                    {
                        emailsms = email;
                    }
                    else
                    {
                        emailsms += "," + email;
                    }
                }
            }
            else
            {
                string mobile = GlobalUtilities.ConvertToString(dttbl.Rows[i]["contacts_mobileno"]);
                if (mobile != txtmobileno.Text)
                {
                    if (emailsms == "")
                    {
                        emailsms = mobile;
                    }
                    else
                    {
                        emailsms += "," + mobile;
                    }
                }
            }
        }
        return emailsms;
    }
    protected void btnGetJSON_Click(object sender, EventArgs e)
    {
        int id = 0;
        gblData.AddExtraValues("sentdate", "dbo.GetDate()");
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            mfuattachment.Save(id);
            string attachment = Email.GetCommaSepAttachment(id);
            bool issent = BulkEmailUtility.SendMail(Common.FullName, Common.EmailId, txtemailid.Text, txtsubject.Text, txtmessage.Text, attachment, txtccemailid.Text,txtbccemailid.Text);
            Session["LogMessage"] = BulkEmail.apiRequestData;
            Response.Redirect("~/log.aspx");
        }
    }
    protected void ddlwhatsapptemplate_changed(object sender, EventArgs e)
    {
        int templateId = GlobalUtilities.ConvertToInt(ddlwhatsapptemplateid.SelectedValue);
        string query = "";
        query = "select * from tbl_whatsapptemplate where whatsapptemplate_whatsapptemplateid=" + templateId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return;
        string templateMessage = GlobalUtilities.ConvertToString(dr["whatsapptemplate_message"]);
        txtwhatsapptemplatemessage.Text = templateMessage;//.Replace("\n", "<br/>");
    }
}
