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

public partial class LiveRateSection_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_liveratesection", "liveratesectionid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
			EnableControlsOnEdit();
            if (Request.QueryString["id"] == null)
            {
            }
            else
            {
                gblData.PopulateForm(form, GetId());
            }
			
			PopulateSubGrid();
        }
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Live Rate Section";
        }
        else
        {
            lblPageTitle.Text = "Edit Live Rate Section";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
    }
			
	private void PopulateSubGrid()
	{
		SubGrid sgrid = new SubGrid();
		sgrid.Populate(gblData, ltliverate, liverate_param, liverate_setting);
	}
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData(true);
    }
    protected void btnSaveAndView_Click(object sender, EventArgs e)
    {
        if(SaveData(false) > 0)
        {
			Response.Redirect("~/LiveRateSection/view.aspx");
		}
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        int id = 0;
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
			gblData.SaveSubTable(id, gblData, liverate_setting);
            PopulateSubGrid();
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
	private void EnableControlsOnEdit()
	{
		if (Request.QueryString["id"] == null) return;
		btnDelete.Visible = true;
	}
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
