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

public partial class EmailTemplate_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_emailtemplate", "emailtemplateid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        txtbody.CssClass = "htmleditor";
        if (!IsPostBack)
        {
            //FillDropDown_START
					
			//FillDropDown_END
           // multifileUploader.folderPath = "../upload/temp/";
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
                //multifileUploader.PopulateFiles("../upload/emailtemplate/" + GetId());
                if (gblData._CurrentRow != null)
                {
                    if (Convert.ToString(gblData._CurrentRow["emailtemplate_mailformat"]) == "Plain Text")
                    {
                        txtbody.CssClass = "textarea";
                    }
                    else
                    {
                        txtbody.CssClass = "htmleditor";
                    }
                }
            }
            //CallPopulateSubGrid_START
					
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add EmailTemplate";
        }
        else
        {
            lblPageTitle.Text = "Edit EmailTemplate";
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

        if (!gblData.IsDataAlreadyExists("emailtemplate_templatename", txttemplatename.Text,
            GlobalUtilities.ConvertToInt(Request.QueryString["id"])))
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
                //multifileUploader.SaveFiles("../upload/emailtemplate/" + id);
                //SaveSubTable_START

                //SaveSubTable_END
                mfuuploadfile.Save(id);
                mfuuploadfile.Clear();
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
        else
        {
            lblMessage.Text = "Data already exists, duplicate entry not allowed!";
            lblMessage.Visible = true;
            return -1;
        }
 
    }
    protected void ddlMailType_Changed(object sender, EventArgs e)
    {

        if (ddlmailformat.SelectedValue == "Plain Text")//txt format
        {
            txtbody.CssClass = "textarea";
        }
        else
        {
            txtbody.CssClass = "htmleditor";
        }

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
