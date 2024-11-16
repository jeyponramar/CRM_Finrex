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

public partial class Module_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_module", "moduleid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillDropDown_START
			
			gblData.FillDropdown(extravaluetypeid, "tbl_extravaluetype", "extravaluetype_extravaluetype", "extravaluetype_extravaluetypeid", "", "extravaluetype_extravaluetype");
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
                chkenablegridedit.Checked = true;
                //SetDefault_START//SetDefault_END
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
            }
            //CallPopulateSubGrid_START
			
			PopulateSubGrid();
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Module";
        }
        else
        {
            lblPageTitle.Text = "Edit Module";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    //PopulateSubGrid_START
			
	private void PopulateSubGrid()
	{
		SubGrid sgrid = new SubGrid();
		sgrid.Populate(gblData, ltextravalues, extravalues_param, extravalues_setting);
	}
			//PopulateSubGrid_END
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!IsValid()) return;
        SaveData(true);

    }
    protected void btnSaveAndConfig_Click(object sender, EventArgs e)
    {
        if (!IsValid()) return;
        int id = SaveData(true);
        Response.Redirect("~/cp/config-module.aspx?mid=" + id);
    }
    protected void btnSaveAndBulkConfig_Click(object sender, EventArgs e)
    {
        if (!IsValid()) return;
        int id = SaveData(true);
        Response.Redirect("~/cp/config-bulkedit.aspx?mid=" + id);
    }

    private bool IsValid()
    {
        string query = "select * from tbl_module where module_modulename='" + txtmodulename.Text + "'";
        if (Request.QueryString["id"] != null)
        {
            query = "select * from tbl_module where module_modulename='" + txtmodulename.Text + "' and module_moduleid<>" + GetId();
        }
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private int SaveData(bool isclose)
    {
        
        lblMessage.Visible = false;
        //SetCode_START//SetCode_END
        int id = 0;
        id = gblData.SaveForm(form);

        if (id > 0)
        {
            //SaveSubTable_START
			
			gblData.SaveSubTable(id, gblData, extravalues_setting);
			//SaveSubTable_END
            
            //code changes
            if (Request.QueryString["id"] != null)
            {
                InsertUpdate obj = new InsertUpdate();
                string query = "select * from tbl_module where module_moduleid=" + id;
                DataRow dr = obj.ExecuteSelectRow(query);
                CP objCp = new CP();
                objCp.GenerateAddDesignPageV2_0(dr, id);
                objCp.GenerateAddCodePage(dr, id, true);
                objCp.GenerateExtraValuesCode(dr, id);
                objCp.GenerateViewPage(dr, id);
                objCp.CreateSubMenu(GlobalUtilities.ConvertToInt(txtmenuid.Text), txtmodulename.Text);
                //objCp.CreateSubMenuInClientProject(GlobalUtilities.ConvertToInt(txtmenuid.Text), txtmodulename.Text);
            }
            lblMessage.Text = "Data saved successfully!";
            lblMessage.Visible = true;
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
    //EnableControlsOnEdit_END
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
    protected void btnConfig_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/cp/config-module.aspx?mid=" + GetId());
    }
    protected void btnActionManager_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/actionmanager/add.aspx?mid=" + GetId());
    }
    protected void btnConfigBulkUpdate_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/cp/config-bulkedit.aspx?mid=" + GetId());
    }
    protected void GotoConfigPageSetting_changed(object sender, EventArgs e)
    {
        int moduleid = GetId();
        string modulename = Common.GetOneColumnData("tbl_module", moduleid, "modulename");
        if (GotoConfigPageSetting.SelectedValue == "0") return;
        string strPage = GotoConfigPageSetting.SelectedValue;
        if (strPage == "configure-viewpage") strPage = "configure-viewpage.aspx?mname=" + modulename + "&mid=" + moduleid + "&isSubGrid=true";
        else if (strPage == "controlpanelview-setting") strPage = "controlpanelview-setting.aspx?mid=" + moduleid;
        else if (strPage == "Configure-UpdatableGrid") strPage = "configure-UpdatableGrid.aspx?mid=" + moduleid + "&mname=" + modulename + "&isUpdatebleGrid=true";
        else if (strPage == "configure-viewpage-view") strPage = "configure-viewpage.aspx?mname=" + modulename + "&mid=" + moduleid;
        Response.Redirect("~/cp/" + strPage);
    }
}
