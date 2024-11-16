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

public partial class User_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_user", "userid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (GlobalUtilities.ConvertToInt(CustomSession.Session("Login_RoleId")) != 1)
        {
            Session["S_Error"] = "You do not have access rights to perform this operation!";
            Response.Redirect("../error.aspx");
        }
        if (!IsPostBack)
        {
            //FillDropDown_START
			
			gblData.FillDropdown(ddlroleid, "tbl_role", "role_rolename", "role_roleid", "", "role_rolename");
			EnableControlsOnEdit();
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
                //SetDefault_START//SetDefault_END
            }
            else
            {
                trpassword.Visible = false;
                txtpassword.Attributes["issave"] = "false";
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
            lblPageTitle.Text = "Add User";
        }
        else
        {
            lblPageTitle.Text = "Edit User";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    private void EnableControlsOnEdit()
    {
    }
    //PopulateSubGrid_START
			
			//PopulateSubGrid_END
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (gblData.IsAlreadyExists("user_username", txtusername.Text))
        {
            lblMessage.Visible = true;
            lblMessage.Text = "User Name Is Already Exists!";
            return;
        } 
        SaveData(true);
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        if (GlobalUtilities.ConvertToInt(txtemployeeid.Text) == 0)
        {
            lblMessage.Text = "Please select employee!";
            lblMessage.Visible = true;
            return 0;
        }
        int id = 0;
        string validateAgentQuery = "select count(*) as c from tbl_user where isnull(user_agentid,0) > 0";
        if (GetId() == 0)
        {
            int totaluser = WebComponent.AppConstants.TotalUsers;
            string query = @"select COUNT(*) from tbl_user ";
            DataTable dttbl = DbTable.ExecuteSelect(query);
            int count = GlobalUtilities.ConvertToInt(dttbl.Rows[0][0]);
            if (count >= totaluser)
            {
                lblMessage.Text = "You can't create more than " + totaluser + " Users. Please Contact Vendor.";
                lblMessage.Visible = true;
                return 0;
            }
        }
        else
        {
            validateAgentQuery += " AND user_userid<>" + GetId();
        }
        //DataRow drAgent = DbTable.ExecuteSelectRow(validateAgentQuery);
        //int agentCount = GlobalUtilities.ConvertToInt(drAgent["c"]);
        //if (GlobalUtilities.ConvertToInt(txtagentid.Text) > 0)
        //{
        //    agentCount = agentCount + 1;
        //}
        //if (agentCount > AppConstants.TotalChatAgents)
        //{
        //    lblMessage.Text = "You can not have more than " + AppConstants.TotalChatAgents + " agents for live chat";
        //    lblMessage.Visible = true;
        //    return 0;
        //}
        if (Request.QueryString["id"] == null)
        {
            gblData.AddExtraValues("isactive", 1);
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
            string guid = Guid.NewGuid().ToString();
            string uniqueid = Encription.Encrypt(guid + "#" + id + "#0");
            string query = "update tbl_user set user_guid='" + guid + "',user_uniqueId='" + 
                            uniqueid + "' where user_userid=" + id;
            InsertUpdate obj = new InsertUpdate();
            Hashtable hstbl = new Hashtable();
            hstbl.Add("guid", guid);
            hstbl.Add("uniqueid", uniqueid);
            obj.UpdateData(hstbl, "tbl_user", id);

            //SaveSubTable_START
			
			//SaveSubTable_END
            //SaveFile_START
			mfuphoto.Save(id);
			//SaveFile_END
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
