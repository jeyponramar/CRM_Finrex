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

public partial class City_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_city", "cityid");
    
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
        CommonPage.SetPageTitle(Page, lblPageTitle, "City");
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
    private void EnableControlsOnEdit()
    {
        if (Request.QueryString["id"] == null) return;
    }
    private int GetId()
    {
        return GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
    }
    
}
