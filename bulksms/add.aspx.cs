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

public partial class BulkSMS_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_bulksms", "bulksmsid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillDropDown_START
			
			gblData.FillDropdown(ddlbulksmstemplateid, "tbl_bulksmstemplate", "bulksmstemplate_templatename", "bulksmstemplate_bulksmstemplateid", "", "bulksmstemplate_templatename");
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
                UpdateSMSStatus();
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                if (GlobalUtilities.ConvertToInt(txttotalsent.Text) > 0)
                {
                    btnSubmit.Visible = false;
                }
                EnableButton();
                
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Bulk SMS";
        }
        else
        {
            lblPageTitle.Text = "Edit Bulk SMS";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
        btnDelete.Visible = false;
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
        //ExtraValues_START
		if(Request.QueryString["id"] == null)
		{
			gblData.AddExtraValues("emailsmsstatusid", "1");
		}
		//ExtraValues_END
        int id = 0;
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            //SaveSubTable_START
			
			mcclientgroups.Save(id);
			//SaveSubTable_END
            //SaveFile_START
			//SaveFile_END
            //ParentCountUpdate_START
			
			//ParentCountUpdate_END
            SaveMessageDetail(id);
            if (Request.QueryString["id"] == null)
            {
                Response.Redirect("~/bulksms/add.aspx?id=" + id);
            }
            else
            {
                gblData.PopulateForm(form, GetId());
                EnableButton();
            }
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
    //EnableControlsOnEdit_START
	private void EnableControlsOnEdit()
	{
		if (Request.QueryString["id"] == null) return;
		btnstart.Visible = true;
		btnstop.Visible = true;
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
    protected void ddlbulksmstemplate_changed(object sender, EventArgs e)
    {
        if (ddlbulksmstemplateid.SelectedIndex == 0)
        {
            txtmessage.Text = "";
            return;
        }
        string query = "select * from tbl_bulksmstemplate WHERE bulksmstemplate_bulksmstemplateid=" + ddlbulksmstemplateid.SelectedValue;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return;
        txtmessage.Text = GlobalUtilities.ConvertToString(dr["bulksmstemplate_message"]);
    }
	protected void btnstart_Click(object sender, EventArgs e)
	{
        string query = "select * from tbl_bulksms WHERE bulksms_bulksmsid="+GetId();
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (GlobalUtilities.ConvertToInt(dr["bulksms_balance"]) > 0)
        {
            query = "update tbl_bulksms set bulksms_emailsmsstatusid=2 WHERE bulksms_bulksmsid=" + GetId();
            DbTable.ExecuteQuery(query);
            ClientScript.RegisterClientScriptBlock(typeof(Page), "sendBulkSMS", "<script>$(document).ready(function(){startSendBulkSMS();\n});</script>");
        }
        else
        {
            lblMessage.Text = "No more sms found!";
            lblMessage.Visible = true;

        }
	}
	protected void btnstop_Click(object sender, EventArgs e)
	{
        string query = "update tbl_bulksms set bulksms_emailsmsstatusid=3 WHERE bulksms_bulksmsid=" + GetId();
        DbTable.ExecuteQuery(query);
	}
    private void SaveMessageDetail(int id)
    {
        string query = "delete from tbl_bulksmsdetail WHERE bulksmsdetail_bulksmsid=" + id;
        DbTable.ExecuteQuery(query);
        
        query = "";
        Array arrIds = mcclientgroups.Ids.Split(',');
        for (int i = 0; i < arrIds.Length; i++)
        {
            int groupId = Convert.ToInt32(arrIds.GetValue(i));
            int statusId = 0;
            if (i > 0)
            {
                query += Environment.NewLine + " union " + Environment.NewLine;
            }
            query += Environment.NewLine + "select distinct " + id + ",client_clientid,contacts_mobileno,'" + global.CheckInputData(txtmessage.Text) + "',1 from tbl_client ";
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
        query = "insert into tbl_bulksmsdetail(bulksmsdetail_bulksmsid,bulksmsdetail_clientid,bulksmsdetail_mobileno,bulksmsdetail_message,bulksmsdetail_emailsmssentstatusid)" + query;

        DbTable.ExecuteQuery(query);
        query = "update tbl_bulksms set bulksms_totalsent=(select count(*) from tbl_bulksmsdetail WHERE bulksmsdetail_emailsmssentstatusid=2 AND bulksmsdetail_bulksmsid=" + id + ")," +
                    "bulksms_totalsms=(select count(*) from tbl_bulksmsdetail WHERE bulksmsdetail_bulksmsid=" + id + ")," +
                    "bulksms_totalfailed=(select count(*) from tbl_bulksmsdetail WHERE bulksmsdetail_emailsmssentstatusid=3 AND bulksmsdetail_bulksmsid=" + id + ") " +
                "WHERE bulksms_bulksmsid=" + id;
        DbTable.ExecuteQuery(query);
    }
    private void EnableButton()
    {
        if (GlobalUtilities.ConvertToInt(txtbalance.Text) == 0)
        {
            btnstart.Visible = false;
            btnstop.Visible = false;
            btnSubmit.Visible = false;
            btnDelete.Visible = false;
        }
        else
        {
            btnstart.Visible = true;
            btnstop.Visible = true;
            
        }
        if (GlobalUtilities.ConvertToInt(txttotalfailed.Text) > 0)
        {
            btnretry.Visible = true;
        }
        else
        {
            btnretry.Visible = false;
        }
    }
	protected void btnretry_Click(object sender, EventArgs e)
	{
        string query = "select * from tbl_bulksms WHERE bulksms_bulksmsid=" + GetId();
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (GlobalUtilities.ConvertToInt(dr["bulksms_totalfailed"]) > 0)
        {
            query = "update tbl_bulksmsdetail set bulksmsdetail_emailsmssentstatusid=1 "+
                    "WHERE bulksmsdetail_emailsmssentstatusid=3 AND bulksmsdetail_bulksmsid = " + GetId();
            DbTable.ExecuteQuery(query);

            query = "update tbl_bulksms set bulksms_emailsmsstatusid=2,bulksms_totalfailed=0 WHERE bulksms_bulksmsid=" + GetId();
            DbTable.ExecuteQuery(query);

            txttotalfailed.Text = "0";
            txtbalance.Text = Convert.ToString(GlobalUtilities.ConvertToInt(txttotalsms.Text) - GlobalUtilities.ConvertToInt(txttotalsent.Text));
            ClientScript.RegisterClientScriptBlock(typeof(Page), "sendBulkSMS", "<script>$(document).ready(function(){startSendBulkSMS();});</script>");
        }
        else
        {
            lblMessage.Text = "No failed sms found!";
            lblMessage.Visible = true;

        }
	}
    private void UpdateSMSStatus()
    {
        string query = "update tbl_bulksms set bulksms_emailsmsstatusid=4 where bulksms_balance=0 AND bulksms_bulksmsid="+GetId();
        DbTable.ExecuteQuery(query);
    }
}
