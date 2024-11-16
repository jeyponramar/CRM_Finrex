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

public partial class Setting_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_setting", "settingid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Url.ToString().ToLower().Contains("demoonline.in"))
        {
            if (GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId")) > 1)
            {
                Session["S_Error"] = "This feature is not available in demo version!";
                Response.Redirect("../error.aspx");
                return;
            }
        }
        if (!IsPostBack)
        {
            if (AppConstants.IsLive)
            {
                trtype.Style.Add("display", "none");
            }
            //FillDropDown_START
			
			//gblData.FillDropdown(ddltypeid, "tbl_type", "type_type", "type_typeid", "", "type_type");
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
                //SetDefault_START//SetDefault_END
                settingvalue_plain.Visible = true;
                settingvalue_html.Visible = false;
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                txtsettingname.Enabled = false;
                if(gblData._CurrentRow!=null)
                {
                     if (GlobalUtilities.ConvertToBool(gblData._CurrentRow["setting_ishtml"]))
                    {
                        settingvalue_html.Text = Convert.ToString(gblData._CurrentRow["setting_settingvalue"]);
                        settingvalue_plain.Visible = false;
                        settingvalue_html.Visible = true;
                    }
                    else
                    {
                        settingvalue_plain.Text = Convert.ToString(gblData._CurrentRow["setting_settingvalue"]);
                        settingvalue_plain.Visible = true;
                        settingvalue_html.Visible = false;
                    }
                }
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
            
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Setting";
        }
        else
        {
            lblPageTitle.Text = "Edit Setting";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    //PopulateSubGrid_START
			
			//PopulateSubGrid_END
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (gblData.IsAlreadyExists("setting_settingname", txtsettingname.Text))
        {
            lblMessage.Text = "Seting Name Is Already Exists!";
            lblMessage.Visible = true;
            return;
        }
        SaveData(true);
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        int id = 0;
        if (ddlishtml.SelectedIndex == 0)
        {
            gblData.AddExtraValues("settingvalue", settingvalue_plain.Text);
        }
        else
        {
            gblData.AddExtraValues("settingvalue", settingvalue_html.Text);
        }
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
    protected void ddlishtml_Changed(object sender, EventArgs e)
    {
        if (ddlishtml.SelectedIndex == 0)
        {
            settingvalue_plain.Visible = true;
            settingvalue_html.Visible = false;
        }
        else
        {
            settingvalue_plain.Visible = false;
            settingvalue_html.Visible = true;
        }
        settingvalue_html.Text = "";
        settingvalue_plain.Text = "";
    }
}
