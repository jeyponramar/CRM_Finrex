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
using System.IO;

public partial class BulkEmail_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_bulkemail", "bulkemailid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnSubmit.Text = "Send";
            btnDelete.Visible = false;
            btnSubmit.Visible = false;
            //FillDropDown_START
			
			gblData.FillDropdown(ddlbulkemailtemplateid, "tbl_bulkemailtemplate", "bulkemailtemplate_templatename", "bulkemailtemplate_bulkemailtemplateid", "", "bulkemailtemplate_templatename");
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
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                btnSubmit.Visible = false;
                btnSubmit1.Visible = false;
                if (GlobalUtilities.ConvertToInt(txtemailsmssentstatusid.Text) == 3)
                {
                    btnretry.Visible = true;
                }
                else
                {
                    btnretry.Visible = false;
                }
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Bulk Email";
        }
        else
        {
            lblPageTitle.Text = "Edit Bulk Email";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
        btnDelete.Visible = false;
    }
    //PopulateSubGrid_START
			
			//PopulateSubGrid_END
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData(true, false);
    }
    protected void btnSubmit1_Click(object sender, EventArgs e)
    {
        SaveData(true, true);
    }
    private int SaveData(bool isclose, bool isnewEmail)
    {
        if (txtmessage.Text.Trim() == "")
        {
            lblMessage.Text = "Please enter message";
            lblMessage.Visible = true;
            return 0;
        }
        lblMessage.Visible = false;
        string query = "";
        //string query = "select count(*) as c from (" + GetBulkEmailQuery() + ")r";
        //DataRow drc = DbTable.ExecuteSelectRow(query);
        //if (GlobalUtilities.ConvertToInt(drc["c"]) > 1)
        //{
        //    lblMessage.Text = "You can not send 1000 emails in one request!";
        //    lblMessage.Visible = true;
        //    return 0;
        //}
        //SetCode_START//SetCode_END
        //ExtraValues_START//ExtraValues_END
        if (Request.QueryString["id"] == null)
        {
            gblData.AddExtraValues("emailsmssentstatusid", "1");
        }
        int id = 0;
        
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            //SaveSubTable_START
			
			mcclientgroups.Save(id);
			//SaveSubTable_END
            //SaveFile_START
			mfuattachment.Save(id);
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END
            if (SendBulkEmail(id, isnewEmail))
            {
                lblMessage.Text = "Bulk email has been sent to the server successfully!";
                lblMessage.Visible = true;
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
    private string GetBulkEmailQuery()
    {
        string query = "";
        Array arrIds = mcclientgroups.GetSelectedIds().Split(',');
        for (int i = 0; i < arrIds.Length; i++)
        {
            int groupId = Convert.ToInt32(arrIds.GetValue(i));
            int statusId = 0;
            if (i > 0)
            {
                query += Environment.NewLine + " union " + Environment.NewLine;
            }
            query += Environment.NewLine + " select distinct contacts_emailid from tbl_client ";
            if (groupId == 1 || groupId == 4 || groupId == 5)
            {
                query += "JOIN tbl_subscription ON subscription_clientid=client_clientid " +
                         "JOIN tbl_contacts ON contacts_clientid=client_clientid " +
                         "WHERE contacts_isemailcommunication = 1";
            }
            else
            {
                query += "JOIN tbl_trial ON trial_clientid=client_clientid " +
                         "JOIN tbl_contacts ON contacts_clientid=client_clientid " +
                         "WHERE contacts_isemailcommunication = 1";
            }
            if (groupId == 5)//competitor
            {
                query += " AND ISNULL(subscription_subscriptionid,0) IN(0,3,4) AND client_clientid IN(select clientcompetitor_clientid from tbl_clientcompetitor)";
            }
            else
            {
                if (groupId == 1)
                {
                    statusId = 2;
                }
                else if (groupId == 2)
                {
                    statusId = 1;
                }
                else if (groupId == 3)
                {
                    statusId = 3;
                }
                else if (groupId == 4)
                {
                    statusId = 4;
                }
                if (statusId > 0)
                {
                    query += " AND client_subscriptionstatusid=" + statusId;
                }
            }
        }
        return query;
    }
    private bool SendBulkEmail(int id, bool isnewEmail)
    {
        string query = "";

        query = GetBulkEmailQuery();
        DataTable dttbl = DbTable.ExecuteSelect(query);
        if (!GlobalUtilities.IsValidaTable(dttbl))
        {
            lblMessage.Text = "No client found!";
            lblMessage.Visible = true;
            return false;
        }

        //if (dttbl.Rows.Count > 1)
        //{
        //    lblMessage.Text = "You can not send 1000 emails in one request!";
        //    lblMessage.Visible = true;
        //    return false;
        //}


        StringBuilder emailIds = new StringBuilder();
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string emailId = GlobalUtilities.ConvertToString(dttbl.Rows[i]["contacts_emailid"]);
            if (emailId.Trim() != "")
            {
                if (emailIds.ToString() == "")
                {
                    emailIds.Append(emailId);
                }
                else
                {
                    emailIds.Append("," + emailId);
                }
            }
        }
        if (Request.Url.ToString().ToLower().Contains("localhost"))
        {
            emailIds = new StringBuilder();
            emailIds.Append("jeyponramar@gmail.com,jeyponramar@gmail.com,nadarmuthulaxmi1985@gmail.com,dana.refux@gmail.com");
            string toEmailIds = "";
        }
        DataRow drBulkMail = DbTable.GetOneRow("tbl_bulkemail", id);
        string attachment = GlobalUtilities.ConvertToString(drBulkMail["bulkemail_attachment"]);
        string strAttachment = "";
        if (attachment != "")
        {
            Array arr = attachment.Split('|');
            for (int i = 0; i < arr.Length; i++)
            {
                string fileName = Server.MapPath("~/upload/bulkemail/" + arr.GetValue(i).ToString());
                if (File.Exists(fileName))
                {
                    if (strAttachment == "")
                    {
                        strAttachment = fileName;
                    }
                    else
                    {
                        strAttachment += "," + fileName;
                    }
                }
            }
        }
        bool issent = false;
        //Response.Write(query);
        //bool issent = BulkEmail.SendBulkEmailOnlyFromLoggedInUser(emailIds.ToString(), txtsubject.Text, txtmessage.Text, strAttachment);
        //if (isnewEmail)
        //{
            issent = BulkEmail.SendBulkEmailOnlyFromLoggedInUser(emailIds.ToString(), txtsubject.Text, txtmessage.Text, strAttachment);
        //}
        //else
        //{
        //    issent = BulkEmail.SendMailFromLoginUser(emailIds.ToString(), txtsubject.Text, txtmessage.Text, strAttachment);
        //}
        
        //Response.End();
        int status = 1;
        if (issent)
        {
            status = 2;
        }
        else
        {
            lblMessage.Text = "Error occurred while sending bulk email to server, please try again later!";
            lblMessage.Visible = true;
            status = 3;
        }
        query = "update tbl_bulkemail set bulkemail_emailsmssentstatusid=" + status + " where bulkemail_bulkemailid=" + id;
        DbTable.ExecuteQuery(query);
        return issent;
    }

    //EnableControlsOnEdit_START
	private void EnableControlsOnEdit()
	{
		if (Request.QueryString["id"] == null) return;
		btnretry.Visible = true;
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
	protected void btnretry_Click(object sender, EventArgs e)
	{
        SaveData(false, false);
	}
    private void UpdateEmailStatus()
    {
        string query = "update tbl_bulkemail set bulkemail_emailsmsstatusid=4 where bulkemail_balance=0 AND bulkemail_bulkemailid=" + GetId();
        DbTable.ExecuteQuery(query);
    }
    
	protected void btnstart_Click(object sender, EventArgs e)
	{
	}
	protected void btnstop_Click(object sender, EventArgs e)
	{
	}
    protected void btnTemplate_Changed(object sender, EventArgs e)
    {
        int templateId = GlobalUtilities.ConvertToInt(ddlbulkemailtemplateid.SelectedValue);
        if (templateId == 0) return;
        DataRow dr = DbTable.GetOneRow("tbl_bulkemailtemplate", templateId);
        if (dr == null) return;
        txtsubject.Text = GlobalUtilities.ConvertToString(dr["bulkemailtemplate_subject"]);
        txtmessage.Text = GlobalUtilities.ConvertToString(dr["bulkemailtemplate_message"]);
    }
}
