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

public partial class Role_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_role", "roleid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillDropDown_START
			
			//FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
                //SetDefault_START//SetDefault_END
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                BindRights();
                BindActionRights();
            }
            //CallPopulateSubGrid_START
			
			//CallPopulateSubGrid_END
        }
        //PageTitle_START
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Role";
        }
        else
        {
            lblPageTitle.Text = "Edit Role";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    //PopulateSubGrid_START
			
			//PopulateSubGrid_END
    private void BindRights()
    {
        string strquery = "select * from tbl_menu where menu_isvisible=1";
        DataTable dttblMenu = DbTable.ExecuteSelect(strquery);
        
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%'>");
        StringBuilder sbSubmenuCount = new StringBuilder();
        sbSubmenuCount.Append(dttblMenu.Rows.Count);
        int roleId=GetId();
        for (int i = 0; i < dttblMenu.Rows.Count; i++)
        {
            int menuId = GlobalUtilities.ConvertToInt(dttblMenu.Rows[i]["menu_menuid"]);
            strquery = "select * from tbl_submenu where submenu_isvisible=1 and submenu_menuid=" + menuId;
            DataTable dttblSubMenu = DbTable.ExecuteSelect(strquery);
            sbSubmenuCount.Append("," + dttblSubMenu.Rows.Count);
            bool isMenuRights = IsMenuHasRights(roleId, menuId, 0);
            string strchecked = "";
            if (isMenuRights)
            {
                strchecked = "checked";
            }
            html.Append("<tr>" +
                        "<td width=20>" +
                        "<input type='checkbox' " + strchecked + " class='chkmenu' name='rights_" + i + "' value='" + Convert.ToString(dttblMenu.Rows[i]["menu_menuid"]) + "'/></td><td class='subtitle'>" +
                        Convert.ToString(dttblMenu.Rows[i]["menu_menuname"]) + "</td></tr>" +
                        "<tr style='display:none' class='trsubmenu'><td><td><table width'100%'>");
            
            for (int j = 0; j < dttblSubMenu.Rows.Count; j++)
            {
                int subMenuId = GlobalUtilities.ConvertToInt(dttblSubMenu.Rows[j]["submenu_submenuid"]);
                if (isMenuRights)
                {
                    strchecked = "checked";
                }
                else
                {
                    bool isSubMenuRights = IsMenuHasRights(roleId, 0,subMenuId);
                    if (isSubMenuRights)
                    {
                        strchecked = "checked";
                    }
                    else
                    {
                        strchecked = "";
                    }
                }
                html.Append("<tr><td><input " + strchecked + " class='chksubmenu' type='checkbox' name='rights_" + i + "_" + j + "' value='" + subMenuId + "'/></td>" +
                            "<td>" + Convert.ToString(dttblSubMenu.Rows[j]["submenu_submenuname"]) + "</td></tr>");
            }
            html.Append("</table></td></tr>");
        }
        ViewState["VS_MenuCount"] = sbSubmenuCount.ToString();
        html.Append("</table>");
        ltRights.Text = html.ToString();
    }
    private bool IsMenuHasRights(int roleId, int menuId, int subMenuId)
    {
        string query = "";
        if (menuId > 0)
        {
            query = "select count(*) c from tbl_menurights where menurights_roleid=" + roleId + " and menurights_menuid=" + menuId;
        }
        else
        {
            query = "select count(*) c from tbl_menurights where menurights_roleid=" + roleId + " and menurights_submenuid=" + subMenuId;
        }
        InsertUpdate obj = new InsertUpdate();
        DataRow dr =obj.ExecuteSelectRow(query);
        if (GlobalUtilities.ConvertToInt(dr[0]) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData(true);
    }
    private void SaveActionRights(int roleId)
    {
        string query = "delete from tbl_actionrights where actionrights_roleid=" + roleId;
        DbTable.ExecuteQuery(query);
        for (int i = 0; i < Request.Form.Keys.Count; i++)
        {
            string key = Request.Form.Keys[i];
            if (key.StartsWith("actionrights_"))
            {
                Array arr = key.Split('_');
                int moduleid = GlobalUtilities.ConvertToInt(arr.GetValue(1));
                int action = GlobalUtilities.ConvertToInt(arr.GetValue(2));
                Hashtable hstbl = new Hashtable();
                hstbl.Add("moduleid", moduleid);
                hstbl.Add("roleid", roleId);
                hstbl.Add("action", action);
                InsertUpdate obj = new InsertUpdate();
                obj.InsertData(hstbl, "tbl_actionrights");
            }
        }
    }
    private void SaveRights(int roleId)
    {
        string strMenuCounts = Convert.ToString(ViewState["VS_MenuCount"]);
        Array arrMenuCounts = strMenuCounts.Split(',');
        int mainMenuCount = GlobalUtilities.ConvertToInt(arrMenuCounts.GetValue(0));
        string query = "delete from tbl_menurights where menurights_roleid=" + roleId;
        DbTable.ExecuteQuery(query);
        for (int i = 0; i < mainMenuCount; i++)
        {
            int menuId = GlobalUtilities.ConvertToInt(Request.Form["rights_" + i]);
            if (menuId > 0)
            {
                Hashtable hstbl = new Hashtable();
                hstbl.Add("roleid", roleId);
                hstbl.Add("menuid", menuId);
                hstbl.Add("submenuid", 0);
                InsertUpdate obj = new InsertUpdate();
                obj.InsertData(hstbl, "tbl_menurights");
            }
            else
            {
                int subMenuCount = GlobalUtilities.ConvertToInt(arrMenuCounts.GetValue(i+1));
                for (int j = 0; j < subMenuCount; j++)
                {
                    int subMenuId = GlobalUtilities.ConvertToInt(Request.Form["rights_" + i + "_" + j]);
                    if (subMenuId > 0)
                    {
                        Hashtable hstbl = new Hashtable();
                        hstbl.Add("roleid", roleId);
                        hstbl.Add("menuid", 0);
                        hstbl.Add("submenuid", subMenuId);
                        InsertUpdate obj = new InsertUpdate();
                        obj.InsertData(hstbl, "tbl_menurights");
                    }
                }
            }
        }
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
            SaveRights(id);
            SaveActionRights(id);

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
    private void BindActionRights()
    {
        string query = "select * from tbl_module WHERE module_menuid<>2 OR module_modulename in('Lead Sheet')";//NOT REFUX MENU
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table border=1><tr class='repeater-header'><td>Module</td><td>Delete</td><td>Excel Export</td></tr>");
        query = "select * from tbl_actionrights where actionrights_roleid=" + GetId();
        DataTable dttblAction = DbTable.ExecuteSelect(query);
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string modulename = GlobalUtilities.ConvertToString(dttbl.Rows[i]["module_modulename"]);
            int moduleid = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["module_moduleid"]);
            string ctrlid = "actionrights_" + moduleid;
            string deleterights = "";
            string exportrights = "";
            if (IsActionrights(dttblAction, moduleid, 1))
            {
                deleterights = " checked='checked'";
            }
            if (IsActionrights(dttblAction, moduleid, 2))
            {
                exportrights = " checked='checked'";
            }
            html.Append("<tr><td>" + modulename + "</td><td><input type='checkbox' name='" + ctrlid + "_1'" + deleterights + "/></td>" +
                              "<td><input type='checkbox' name='" + ctrlid + "_2'"+exportrights+"/></td>" +
                        "</tr>");
        }
        html.Append("</table>");
        ltActionRights.Text = html.ToString();
    }
    private bool IsActionrights(DataTable dttbl,int moduleid, int action)
    {
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (GlobalUtilities.ConvertToInt(dttbl.Rows[i]["actionrights_moduleid"]) == moduleid
                && GlobalUtilities.ConvertToInt(dttbl.Rows[i]["actionrights_action"]) == action)
            {
                return true;
            }
        }
        return false;
    }
}
