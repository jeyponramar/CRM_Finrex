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

public partial class MCLR_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_mclr", "mclrid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillDropDown_START
			
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
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add MCLR";
        }
        else
        {
            lblPageTitle.Text = "Edit MCLR";
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
        int oldmclrId = 0;
        int bankId = GlobalUtilities.ConvertToInt(txtmclrbankid.Text);
        string query = "select * from tbl_mclr where mclr_mclrbankid=" + bankId;
        DataRow drmclr = DbTable.ExecuteSelectRow(query);
        if (drmclr != null)
        {
            id = GlobalUtilities.ConvertToInt(drmclr["mclr_mclrid"]);
            oldmclrId = id;
            if (oldmclrId > 0)
            {
                if (txteffectivedate.Text != GlobalUtilities.ConvertToDate(drmclr["mclr_effectivedate"]))
                {
                    query = @"insert into tbl_mclrhistory(mclrhistory_mclrid,mclrhistory_mclrbankid,mclrhistory_1month,mclrhistory_3months,
                                  mclrhistory_6months,mclrhistory_1year,mclrhistory_effectivedate,mclrhistory_baserate)
                                  select mclr_mclrid,mclr_mclrbankid,mclr_1month,mclr_3months,
                                  mclr_6months,mclr_1year,mclr_effectivedate,mclr_baserate
                                  from tbl_mclr where mclr_mclrid=" + oldmclrId;
                    DbTable.ExecuteQuery(query);
                }
            }
        }
        id = gblData.SaveForm(form, id);

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
    
}
