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

public partial class UserType_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_usertype", "usertypeid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillDropDown_START
					
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
                //SetDefault_START
					
			//SetDefault_END
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
            lblPageTitle.Text = "Add User Type";
        }
        else
        {
            lblPageTitle.Text = "Edit User Type";
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
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        int id = 0;
        if (h_IsCopy.Text == "1")
        {
            id = gblData.SaveForm(form, 0);
        }
        else
        {
            id = gblData.SaveForm(form);
        }

        if (id > 0)
        {
            //SaveSubTable_START
					
			//SaveSubTable_END
            lblMessage.Text = "Data saved successfully!";
            lblMessage.Visible = true;

            string target = "";
            if (Request.QueryString["targettxt"] != null)
            {
                //pass data from this page to previous tab
                //target = "setPassData(" + Request.QueryString["tpage"] + ",'" + Request.QueryString["targettxt"] 
                //    + "','" + Request.QueryString["targethdn"] + "'," + id + ",'" + txtamccode.Text + "');";
            }
            string script = "";
            string close = "";
            if (Request.QueryString["id"] == null)
            {
                gblData.ResetForm(form);
            }
            else
            {
                close = "parent.closeTab();";
            }
            script = target + close;
            if (script != "" && isclose)
            {
                script = "<script>" + script + "</script>";
                ClientScript.RegisterClientScriptBlock(typeof(Page), "closetab", script);
            }

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
    
}
