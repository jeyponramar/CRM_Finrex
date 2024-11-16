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

public partial class SubMenu_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_submenu", "submenuid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
			CommonPage.DisableOnQuickAddEdit(btnSubmitAndView);
            //FillDropDown_START
			
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
       			//PopulateOnAdd_START
			
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
            lblPageTitle.Text = "Add Sub Menu";
        }
        else
        {
            lblPageTitle.Text = "Edit Sub Menu";
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
        	Response.Redirect("~/SubMenu/view.aspx");
		}
    }
    private void UpdateSequence()
    {
        int id = GetId();
        int seq = GlobalUtilities.ConvertToInt(txtsequence.Text);
        string query = "select top 1 * from tbl_submenu WHERE submenu_sequence=" + seq + " AND submenu_menuid=" + txtmenuid.Text + " AND submenu_submenuid<>" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            query = "update tbl_submenu set submenu_sequence=submenu_sequence+1 WHERE submenu_sequence>=" + seq + " AND submenu_menuid=" + txtmenuid.Text + " AND submenu_submenuid<>" + id;
            DbTable.ExecuteQuery(query);
        }
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        //SetCode_START//SetCode_END
        int id = 0;
        UpdateSequence();
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            //CP cp = new CP();
            //cp.CreateSubMenuInClientProjectByMenuName(GlobalUtilities.ConvertToInt(txtmenuid.Text), txtsubmenuname.Text, txturl.Text, "setting");
            //SaveSubTable_START

            //SaveSubTable_END
            lblMessage.Text = "Data saved successfully!";
            lblMessage.Visible = true;
            chkisvisible.Checked = true;
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
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DbTable.ExecuteQuery("DELETE FROM tbl_submenu WHERE submenu_submenuid=" + GetId());
        Response.Redirect("view.aspx");
    }
    
}
