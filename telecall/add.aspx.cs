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

public partial class TeleCall_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_telecall", "telecallid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            NextPrevDetail.Visible = false;
            //FillDropDown_START
			
			EnableControlsOnEdit();
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
       			//PopulateAcOnAdd_START
			
			//PopulateAcOnAdd_END
				//PopulateOnAdd_START
				//PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
            }
            else
            {
                SetId(GlobalUtilities.ConvertToInt(Request.QueryString["id"]));
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                BindTelecallHistory();
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Tele Call";
        }
        else
        {
            lblPageTitle.Text = "Edit Tele Call";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    //PopulateSubGrid_START
			
			//PopulateSubGrid_END
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        gblData.AddExtraValues("telecallstatusid", 2);
        SaveData(true);
    }
    protected void btnSaveAndView_Click(object sender, EventArgs e)
    {
        SaveData(false);
        Response.Redirect("~/TeleCall/view.aspx");
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        if (txtmobileno.Text == "" && txtlandlineno.Text == "")
        {
            lblMessage.Text = "Please enter mobile number OR telephone number!";
            lblMessage.Visible = true;
            return 0;
        }
        //SetCode_START//SetCode_END
        //ExtraValues_START//ExtraValues_END
        if (Request.QueryString["id"] == null)
        {
            gblData.AddExtraValues("telecallstatusid", 1);
        }
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

            //save call history
            SaveCallHistory();

            lblMessage.Text = "Data saved successfully!";
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
		btncompanyprofile.Visible = true;
		btnnotinterested.Visible = true;
		btnconverttoenquiry.Visible = true;
		btneditlastcall.Visible = true;
		btnDelete.Visible = true;
	}//EnableControlsOnEdit_END
    private int GetId()
    {
        return GlobalUtilities.ConvertToInt(ViewState["id"]);
    }
    private void SetId(int id)
    {
        ViewState["id"] = id;
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Common.Delete();
    }    
	protected void btncompanyprofile_Click(object sender, EventArgs e)
	{
	}
	protected void btnnotinterested_Click(object sender, EventArgs e)
	{
        txttelecallstatusid.Text = "3";
        telecallstatus.Text = "Not Interested";
        SaveData(false);
        OpenNextTelecall();
	}
	protected void btnconverttoenquiry_Click(object sender, EventArgs e)
	{
        txttelecallstatusid.Text = "4";
        telecallstatus.Text = "Converted to Enquiry";
        SaveData(false);
        OpenNextTelecall();
	}
    private void OpenNextTelecall()
    {
    }
	protected void btneditlastcall_Click(object sender, EventArgs e)
	{
	}
    private void BindTelecallHistory()
    {
        string query = "select * from tbl_telecallhistory " +
                       "JOIN tbl_employee ON employee_employeeid=telecallhistory_employeeid " +
                       "JOIN tbl_telecallstatus ON telecallstatus_telecallstatusid=telecallhistory_telecallstatusid " +
                       "WHERE telecallhistory_telecallid=" + GetId() + 
                       " ORDER BY telecallhistory_telecallhistoryid DESC";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        lttelecallinghistory.Text = Common.BindGrid(dttbl, "telecallhistory_calldate,employee_employeename,telecallhistory_remarks,telecallstatus_status",
                        "Call Date,Called By,Remarks,Status");
    }
    private void SaveCallHistory()
    {
        Hashtable hstbl = new Hashtable();
        hstbl.Add("telecallid", GetId());
        hstbl.Add("calldate", "getdate()");
        hstbl.Add("employeeid", EmployeeId);
        hstbl.Add("remarks", txtremarks.Text);
        hstbl.Add("telecallstatusid", txttelecallstatusid.Text);
        InsertUpdate obj = new InsertUpdate();
        obj.InsertData(hstbl, "tbl_telecallhistory");
    }
    private int EmployeeId
    {
        get
        {
            return GlobalUtilities.ConvertToInt(CustomSession.Session("Login_EmployeeId"));
        }
    }
}
