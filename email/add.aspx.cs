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

public partial class Email_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_email", "emailid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnSubmit.Text = "Send";
            
            //FillDropDown_START
			
			EnableControlsOnEdit();
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["type"] == null)
                {
                    txtemailtype.Text = "Email";
                }
                else
                {
                    txtemailtype.Text = Request.QueryString["type"];
                }
                //PopulateAcOnAdd_START
			
			//PopulateAcOnAdd_END
                //PopulateOnAdd_START
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                int status = GlobalUtilities.ConvertToInt(txtemailsmssentstatusid.Text);
                if (status == 2)
                {
                    lblMessage.Text = "This email has been already sent, continue if you want to resend again!";
                    lblMessage.Visible = true;
                }
                else if (status == 3)
                {
                    lblMessage.Text = "Software has already tried to send this email but it was failed last time, continue if you want to resend again!";
                    lblMessage.Visible = true;
                }
            }
            
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        lblPageTitle.Text = "Send Email";
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
        lblMessage.Visible = false;
        //SetCode_START//SetCode_END
        //ExtraValues_START//ExtraValues_END
        int id = 0;
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

            string attachment = Email.GetCommaSepAttachment(id);
            int iswhatsappsent = SendWhatsAppMessage(id);

            bool issent = BulkEmail.SendMail(Common.FullName, txtfromemailid.Text, txttoemailid.Text, txtsubject.Text, txtmessage.Text, attachment);
            UpdateEmailStatus(id, issent);
            lblMessage.Text = "";
            if (iswhatsappsent == 1)
            {
                lblMessage.Text = "WhatsApp message sent successfully!";
            }
            else if (iswhatsappsent == 2)
            {
                lblMessage.Text = "Unable to send WhatsApp message!";
            }
            if (issent)
            {
                lblMessage.Text += "<br/>Email sent to server successfully!";
            }
            else
            {
                lblMessage.Text = "Error occurred while sending mail to server!";
            }
            lblMessage.Visible = true;

            //lblMessage.Text = "Data saved successfully!";
            //lblMessage.Visible = true;
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
    private void UpdateEmailStatus(int id, bool issent)
    {
        int statusId = 3;//failed
        if (issent) statusId = 2;//success
        string query = "update tbl_email set email_emailsmssentstatusid=" + statusId + ",email_isdraft=0 WHERE email_emailid=" + id;
        DbTable.ExecuteQuery(query);
        string module = txtmodule.Text.ToLower();
        int moduleId = GlobalUtilities.ConvertToInt(txtmoduleid.Text);
        query = "";
        if (module != "" && moduleId > 0)
        {
            string emailtype = txtemailtype.Text.Trim().ToLower().Replace(" ","");
            if (module == "followups")
            {
                if (issent)
                {
                    if (emailtype == "mom")
                    {
                        query = "update tbl_followups set followups_momsent=1 WHERE followups_followupsid=" + moduleId;
                    }
                    else
                    {
                        query = "update tbl_followups set followups_meetingrequestsent=1 WHERE followups_followupsid=" + moduleId;
                    }
                    DbTable.ExecuteQuery(query);
                }
            }
            else
            {
                query = "select * from tbl_" + module + " WHERE 1=2";
                DataTable dttblModule = DbTable.ExecuteSelect(query);
                string colName = module + "_" + emailtype + "sentstatusid";
                if (dttblModule !=null && dttblModule.Columns.Contains(colName))
                {
                    query = "update tbl_" + module + " set " + colName + "=" + statusId + " where " + module + "_" + module + "id=" + moduleId;
                    try
                    {
                        DbTable.ExecuteQuery(query);
                    }
                    catch { }
                }
            }
        }
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
    private int SendWhatsAppMessage(int id)
    {
        string query = "";
        query = "select * from tbl_email where email_emailid=" + id;
        DataRow dremail = DbTable.ExecuteSelectRow(query);
        string emailType = GlobalUtilities.ConvertToString(dremail["email_emailtype"]);
        string module = GlobalUtilities.ConvertToString(dremail["email_module"]);
        int moduleId = GlobalUtilities.ConvertToInt(dremail["email_moduleid"]);
        DataRow dr = null;
        int clientId = 0;
        string attachment = "";
        string attachmentName = "";
        string mobileNos = "";
        string templateName = emailType;
        string fileName = GlobalUtilities.ConvertToString(dremail["email_attachment"]);
        if (emailType == "Company Profile")
        {
            dr = Custom.GetEnquiryData(moduleId);
            clientId = GlobalUtilities.ConvertToInt(dr[module + "_clientid"]);
            attachment = "https://finstation.in/upload/companyprofile/companyprofile.pdf";
            attachmentName = "CompanyProfile-Finrex.pdf";
            mobileNos = Common.GetAllContactMobileNos(clientId);
        }
        else if (emailType == "Trial - Company Profile")
        {
            templateName = "Company Profile";
            dr = Custom.GetTrialData(moduleId);
            clientId = GlobalUtilities.ConvertToInt(dr[module + "_clientid"]);
            attachment = "https://finstation.in/upload/companyprofile/companyprofile.pdf";
            attachmentName = "CompanyProfile-Finrex.pdf";
            mobileNos = Common.GetAllContactMobileNos(clientId);
        }
        else if (emailType == "proformainvoice")
        {
            dr = Custom.GetProformaInvoiceData(moduleId);
            clientId = GlobalUtilities.ConvertToInt(dr[module + "_clientid"]);
            attachment = "https://finstation.in/upload/email/" + fileName;
            attachmentName = "ProformaInvoice.pdf";
            mobileNos = GetAccountsMobileNos(module, dr);
            templateName = "Proforma Invoice To Customer"; 
        }
        else if (emailType == "invoice")
        {
            dr = Custom.GetInvoiceData(moduleId);
            clientId = GlobalUtilities.ConvertToInt(dr[module + "_clientid"]);
            attachment = "https://finstation.in/upload/email/" + fileName;
            attachmentName = "Invoice.pdf";
            mobileNos = GetAccountsMobileNos(module, dr);
            templateName = "Invoice To Customer";
        }
        else if (emailType == "Send Password")
        {
            //RPlusWhatsAppAPI.SendMessage("", false, "contacts", GetId(), dr, txtmobileno.Text, EnumWhatsAppMessageType.Text,
            //   "User Login Details", "", "", "", "", "");

            dr = Custom.GetContactsClientUserData(moduleId);
            clientId = GlobalUtilities.ConvertToInt(dr[module + "_clientid"]);
            mobileNos = GlobalUtilities.ConvertToString(dr["contacts_mobileno"]);
            templateName = "User Login Details";
        }
        else
        {
            return 0;
        }
        if (dr == null) return 0;
        bool issent = RPlusWhatsAppAPI.SendMessage("", false, module, moduleId, dr, mobileNos, EnumWhatsAppMessageType.Text,
                templateName, "", "", "", attachment, attachmentName);

        if (issent)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
    private string GetAccountsMobileNos(string module, DataRow dr)
    {
        string directormobile = GlobalUtilities.ConvertToString(dr[module + "_directormobile"]);
        string financemobile = GlobalUtilities.ConvertToString(dr[module + "_financemobile"]);
        string mobileNos = "";
        if (directormobile != "")
        {
            mobileNos = GlobalUtilities.CommaSeperator(mobileNos, directormobile);
        }
        if (financemobile != "")
        {
            mobileNos = GlobalUtilities.CommaSeperator(mobileNos, financemobile);
        }
        return mobileNos;
    }
}
