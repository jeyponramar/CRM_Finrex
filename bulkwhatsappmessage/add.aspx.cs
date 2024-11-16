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

public partial class BulkWhatsAppMessage_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_bulkwhatsappmessage", "bulkwhatsappmessageid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillDropDown_START
			
			gblData.FillDropdown(ddlwhatsapptemplateid, "tbl_whatsapptemplate", "whatsapptemplate_templatename", "whatsapptemplate_whatsapptemplateid", "", "whatsapptemplate_templatename");
			EnableControlsOnEdit();
			//FillDropDown_END
            gblData.FillDropdown(ddlwhatsapptemplateid, "tbl_whatsapptemplate", "whatsapptemplate_templatename", "whatsapptemplate_whatsapptemplateid", "whatsapptemplate_whatsappmessagecategoryid=1", "whatsapptemplate_templatename");
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
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Bulk WhatsApp Message";
        }
        else
        {
            lblPageTitle.Text = "Edit Bulk WhatsApp Message";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_END
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
        if (Request.QueryString["id"] == null)
        {
            gblData.AddExtraValues("emailsmsstatusid", "1");
        }
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            //SaveSubTable_START
			
			mcbulkwhatsappmessageclientgroups.Save(id);
			//SaveSubTable_END
            //SaveFile_START
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END
            SaveMessageDetail(id);
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
    private void SaveMessageDetail(int id)
    {
        string query = "delete from tbl_bulkwhatsappmessagedetail WHERE bulkwhatsappmessagedetail_bulkwhatsappmessageid=" + id;
        DbTable.ExecuteQuery(query);

        query = "";
        Array arrIds = mcbulkwhatsappmessageclientgroups.Ids.Split(',');
        for (int i = 0; i < arrIds.Length; i++)
        {
            int groupId = Convert.ToInt32(arrIds.GetValue(i));
            int statusId = 0;
            if (i > 0)
            {
                query += Environment.NewLine + " union " + Environment.NewLine;
            }
            query += Environment.NewLine + "select distinct " + id + ",getdate(),getdate()," + Common.UserId + ",client_clientid,contacts_mobileno,'" + 
                            global.CheckInputData(txtwhatsapptemplatemessage.Text) + "','"+global.CheckInputData(txtwhatsappvariables.Text)+"',1 from tbl_client ";
            if (groupId == 1 || groupId == 4 || groupId == 5)
            {
                query += "JOIN tbl_subscription ON subscription_clientid=client_clientid " +
                         "JOIN tbl_contacts ON contacts_clientid=client_clientid " +
                         "WHERE contacts_iswhatsappcommunication = 1";
            }
            else
            {
                query += "JOIN tbl_trial ON trial_clientid=client_clientid " +
                         "JOIN tbl_contacts ON contacts_clientid=client_clientid " +
                         "WHERE contacts_iswhatsappcommunication = 1";
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
        query = @"insert into tbl_bulkwhatsappmessagedetail(bulkwhatsappmessagedetail_bulkwhatsappmessageid,bulkwhatsappmessagedetail_date,
                        bulkwhatsappmessagedetail_createddate,bulkwhatsappmessagedetail_createdby,bulkwhatsappmessagedetail_clientid,
                        bulkwhatsappmessagedetail_mobileno,bulkwhatsappmessagedetail_message,
                        bulkwhatsappmessagedetail_whatsappvariables,bulkwhatsappmessagedetail_emailsmssentstatusid)" + query;

        DbTable.ExecuteQuery(query);
        query = "update tbl_bulkwhatsappmessage set bulkwhatsappmessage_totalsent=(select count(*) from tbl_bulkwhatsappmessagedetail WHERE bulkwhatsappmessagedetail_emailsmssentstatusid=2 AND bulkwhatsappmessagedetail_bulkwhatsappmessageid=" + id + ")," +
                    "bulkwhatsappmessage_totalmessages=(select count(*) from tbl_bulkwhatsappmessagedetail WHERE bulkwhatsappmessagedetail_bulkwhatsappmessageid=" + id + ")," +
                    "bulkwhatsappmessage_totalfailed=(select count(*) from tbl_bulkwhatsappmessagedetail WHERE bulkwhatsappmessagedetail_emailsmssentstatusid=3 AND bulkwhatsappmessagedetail_bulkwhatsappmessageid=" + id + ") " +
                "WHERE bulkwhatsappmessage_bulkwhatsappmessageid=" + id;
        DbTable.ExecuteQuery(query);
    }
}
