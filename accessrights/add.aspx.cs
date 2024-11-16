using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using WebComponent;
using System.Collections;

public partial class accessrights : System.Web.UI.Page
{
    ArrayList dummy_arrForBlank = new ArrayList();          //Dummy array for function Bindrights
    GlobalData gblData = new GlobalData("tbl_rights","rightsid");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gblData.FillDropdown(ddlRole);
            gblData.FillDropdown(ddlModule);
            gblData.FillDropdown(ddlMenu);
            gblData.FillDropdown(ddlRole_From, "tbl_role", "role_rolename", "role_roleid", "", "role_rolename");
            lblPageTitle.Text = "Access Rights";
            ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
        }
    }
    protected void ddlModule_Change(object sender, EventArgs e)
    {
        if (ddlModule.SelectedIndex > 0)
        {
            //BindAccessRights();
        }
    }    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblMessage.Visible = true;
        if (ddlRole.SelectedValue == "1")
        {
            lblMessage.Text = "Administrator rights can not be changed.";
            return;
        }
        string query = "";
        if (GlobalUtilities.ConvertToInt(ddlModule.SelectedValue) > 0 && GlobalUtilities.ConvertToInt(ddlRole.SelectedValue) > 0)
        {
            query = "delete from tbl_rights where rights_module='" +(ddlModule.SelectedItem.Text) +"'"+
                            " and rights_roleid=" + GlobalUtilities.ConvertToInt(ddlRole.SelectedValue);
        }
        else if (GlobalUtilities.ConvertToInt(ddlRole.SelectedValue) > 0)
        {
            query = "delete from tbl_rights where"+
                           " rights_roleid=" + GlobalUtilities.ConvertToInt(ddlRole.SelectedValue);
        }
        else
        {
            lblMessage.Text = "Invalid Selection";
            return;
        }
        InsertUpdate obj = new InsertUpdate();
        obj.ExecuteQuery(query);
        int count = GlobalUtilities.ConvertToInt(ViewState["modulescount"]);
       
        int moduleid = 0;
        string modulename = "";
        for (int i = 0; i < HttpContext.Current.Request.Form.Keys.Count; i++)
        {
            string key = HttpContext.Current.Request.Form.Keys[i].ToLower();
            
            if (key.StartsWith("txtmodulename_"))
            {
                modulename = Convert.ToString(key.Replace("txtmodulename_", ""));
            }           
            if (key.StartsWith("ar_" + modulename + "_"))
            {
                string val = key.Replace("ar_"+ modulename +"_","");
                Hashtable hstbl = new Hashtable();
                hstbl.Add("roleid", ddlRole.SelectedValue);
                hstbl.Add("module", modulename);
                hstbl.Add("action", val);
                InsertUpdate obj2 = new InsertUpdate();
                int id = obj2.InsertData(hstbl, "tbl_rights");                
            }
        }
        BindRights(dummy_arrForBlank);
        lblMessage.Text = "Access rights have been set successfully!";
        
    }
    private void BindRights(ArrayList arrModulename)
    {
        DataTable dttbl = new DataTable();
        InsertUpdate obj = new InsertUpdate();
        string query = "";
        //string query = "select * from tbl_moduleaction where module_moduleid=" + GlobalUtilities.ConvertToInt(ddlModule.SelectedValue);
        //dttbl = obj.ExecuteSelect(query);
        //obj = new InsertUpdate();
       
        StringBuilder html = new StringBuilder();
        StringBuilder html_report = new StringBuilder();
        html.Append("<table width='100%'" +
                    "style='' cellpadding=0 cellspacing=0>" +
                    "<tr><td class='handovertitle' style='width:100%' >ModuleName</td>" +
                    "<td class='handovertitle' style='width:100%' >Action/Rights</td></tr>");

        DataTable dtModules = new DataTable();
        query = "SELECT * FROM tbl_moduleaction ORDER BY moduleaction_modulename ";
        if (GlobalUtilities.ConvertToInt(ddlModule.SelectedValue) > 0)
        {
            query = "select * from tbl_moduleaction where moduleaction_modulename='" +Convert.ToString(Convert.ToString(ddlModule.SelectedItem.Text).Replace(" ","").ToLower()) + "' ORDER BY moduleaction_modulename ";
        }

        obj = new InsertUpdate();
        dtModules = obj.ExecuteSelect(query);
        ViewState["modulescount"] = dtModules.Rows.Count;
        bool isreporthtmladded = false;
        for (int k = 0; k < dtModules.Rows.Count; k++)
        {
            string chked = "";
            string strtdAction = "";
            string strModuleName = Convert.ToString(dtModules.Rows[k]["moduleaction_modulename"]);
            bool isreport = GlobalUtilities.ConvertToBool(dtModules.Rows[k]["moduleaction_isreport"]);
            
            //int intModuleId = GlobalUtilities.ConvertToInt(dtModules.Rows[k]["module_moduleid"]);
            obj = new InsertUpdate();
            string actions = Convert.ToString(dtModules.Rows[k]["moduleaction_rightsaction"]);
            
            DataTable dttblRights = new DataTable();
            obj = new InsertUpdate();
            query = "SELECT * FROM tbl_rights WHERE rights_roleid=" + GlobalUtilities.ConvertToInt(ddlRole.SelectedValue) + " AND replace(rights_module,' ','')='" + strModuleName.Replace(" ","") + "'";
            //Roshan Code For Copy Access
            if (GlobalUtilities.ConvertToInt(ddlRole_From.SelectedValue) > 0)
            {
                query = "SELECT * FROM tbl_rights WHERE rights_roleid=" + GlobalUtilities.ConvertToInt(ddlRole_From.SelectedValue) + " AND replace(rights_module,' ','')='" + strModuleName.Replace(" ", "") + "'";
            }
            //End of Roshan Code For Copy Access
            dttblRights = obj.ExecuteSelect(query);
            Array arrActions = actions.Split(',');            

            string strBackgroundColor = "#d7d7d7";
            if (k % 2 > 0)
            {
                strBackgroundColor = "#eaecec";
            }
            string Css = "style='font-size:small;background-color:" + strBackgroundColor + ";border-bottom:1px solid #21618a;padding-left:10px;'";
            for (int i = 0; i < arrActions.Length; i++)
            {
                chked = "";
                string action = arrActions.GetValue(i).ToString();

                if (GlobalUtilities.ConvertToInt(ddlMenu.SelectedValue) > 0)
                {
                    for (int x = 0; x < arrModulename.Count; x++)
                    {
                        if (strModuleName.Equals(arrModulename[x]))
                        {
                            chked = "checked";
                            break;
                        }
                    }
                }

                for (int j = 0; j < dttblRights.Rows.Count; j++)
                {
                    if (Convert.ToString(dttblRights.Rows[j]["rights_action"]).ToLower() == action.ToLower())
                    {
                        chked = "checked";
                        break;
                    }
                }
                if (action != "" || actions == "")
                {
                    strtdAction += "<td><input " + chked + " type='checkbox' class='chk' name='ar_" + strModuleName.Replace(" ","") + "_" + action + "'/></td>" +
                                "</td>" +
                                "<td>" + action +
                                "</td>";
                }

            }
            if (actions != "" || actions == "")
            {
                
                if ((isreport)&&(!isreporthtmladded))
                {
                    isreporthtmladded = true;
                    html_report.Append("<tr><td class='title' colspan='2'>Report</td></tr>");
                }
                string strTempHtml =@"<tr><td style='font-size:small;background-color:" + strBackgroundColor + "; border-bottom:1px solid #77bffc;border-left:1px solid #21618a;border-right:1px solid #21618a;border-bottom:1px solid #21618a;padding-left:10px;'>" +
                                    "<b>" + strModuleName + "</b><input class='hdn hidden' name=txtmodulename_" + strModuleName.Replace(" ", "") + " type='text' text='" + strModuleName + "'" + "/></td>" +
                                    " <td style='border-bottom:1px solid #21618a;background-color:" + strBackgroundColor + "'><table><tr>" + strtdAction + "</tr></table></td>" +
                                 "</tr>";
                if (isreport)
                {
                    html_report.Append(strTempHtml);
                }
                else
                {
                    html.Append(strTempHtml);
                }
            }
                    
        }
        html.Append(html_report.ToString());// For Printing the Report Last
        html.Append("</table>");
        ltRights.Text = html.ToString();
    }
    protected void btn_GetdetailClick(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
        BindRights(dummy_arrForBlank);
    }
    protected void Btn_fullaccess_click(object sender, EventArgs e)
    {
        string temp_menu ="";
        ArrayList arrModulenames = new ArrayList();
        int menuid = GlobalUtilities.ConvertToInt(ddlMenu.SelectedValue);
        string query = "SELECT * FROM tbl_submenu where submenu_menuid = " + menuid;
        InsertUpdate obj = new InsertUpdate();
        DataTable dt = obj.ExecuteSelect(query);
        if (dt != null)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string menu = Convert.ToString(dt.Rows[i]["submenu_submenuname"]);
                if (menu.Contains("Add"))
                {
                    temp_menu = menu.Replace("Add", "");
                    temp_menu = temp_menu.Replace(" ","");
                }
                else if (menu.Contains("View"))
                {
                    temp_menu = menu.Replace("View", "");
                    temp_menu = temp_menu.Replace(" ","");
                }
                else
                {
                    temp_menu = menu;
                    temp_menu = temp_menu.Replace(" ","");
                    arrModulenames.Add(temp_menu);
                }
                if (!arrModulenames.Contains(temp_menu))
                {
                    arrModulenames.Add(temp_menu);
                }
            }
        }
        BindRights(arrModulenames);
    }
    protected void Btn_copyaccess_click(object sender,EventArgs e)
    {
        BindRights(dummy_arrForBlank);
    }
}
