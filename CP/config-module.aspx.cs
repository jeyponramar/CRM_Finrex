using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;
using System.IO;

public partial class CP_config_module : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_columns","columnsid");
    bool _isMobile = true;
    protected void Page_Load(object sender, EventArgs e)
    {
        SetModuleName();
        if (!IsPostBack)
        {
            //GlobalData gbl = new GlobalData();
            //gbl.FillDropdown(ddlDropdownmoduleid, "tbl_module", "module_modulename", "module_moduleid", "", "module_modulename");
            InsertUpdate obj = new InsertUpdate();
            DataRow dr = obj.ExecuteSelectRow("select * from tbl_module where module_moduleid=" + Request.QueryString["mid"]);
            if (dr == null)
            {
                Response.Redirect("~/module/view.aspx");
            }
            gblData.FillDropdown(module, "tbl_module", "module_modulename", "module_moduleid", "", "module_modulename");
            string prefix = Convert.ToString(dr["module_tablename"]);
            h_prefix.Text = prefix.Replace("tbl_", "") + "_";
            lblTitle.Text = Convert.ToString(dr["module_modulename"]) + " - Configure Page";

            PopulateSubmodule();
            PopulateSection();
            if (Request.QueryString["id"] == null)
            {
                SetSequence();
                Reset();
            }
            else
            {
                //txtcolumnname.Enabled = false;
                //txtlbl.Enabled = false;
                gblData.PopulateForm(form, GlobalUtilities.ConvertToInt(Request.QueryString["id"]));
                txtlbl.Text = Convert.ToString(gblData._CurrentRow["columns_lbl"]);
                PopulateDropdownModule();
                lnkConvert.NavigateUrl = "~/cp/convert.aspx?mid=" + Request.QueryString["mid"] + "&id=" + Request.QueryString["id"];
            }
            SetWarning();
        }
    }
    private void Reset()
    {
        //chkisvisibleinadd.Checked = true;
        //chkisvisibleinedit.Checked = true;
        chkisleftcolumn.Checked = true;
        //chkisviewpage.Checked = true;
        chkisgeneratelabel.Visible = true;
        //chkissearchfield.Checked = true;
    }
    private void PopulateDropdownModule()
    {
        DataRow drColum = DbTable.GetOneRow("tbl_columns", GlobalUtilities.ConvertToInt(Request.QueryString["id"]));

        int dropDownModuleId = GlobalUtilities.ConvertToInt(drColum["columns_dropdownmoduleid"]);
        if (dropDownModuleId == 0) return;
        DataRow dr = DbTable.GetOneRow("tbl_module", dropDownModuleId);
        if (dr != null)
        {
            dropdownmodule.Text = GlobalUtilities.ConvertToString(dr["module_modulename"]);
            txtdropdownmoduleid.Text = dropDownModuleId.ToString();
        }
    }
    private void PopulateSubmodule()
    {
        string query = "select * from tbl_columns where columns_moduleid=" + Request.QueryString["mid"] + " AND columns_control='Sub Grid'";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        ddlsubmoduleid.DataSource = dttbl;
        ddlsubmoduleid.DataTextField = "columns_lbl";
        ddlsubmoduleid.DataValueField = "columns_columnsid";
        ddlsubmoduleid.DataBind();
        ddlsubmoduleid.Items.Insert(0, new ListItem("Select", "0"));
    }
    private void PopulateSection()
    {
        string query = "select * from tbl_columns where columns_moduleid=" + Request.QueryString["mid"] + " AND columns_control='Section'";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        ddlsubsectionid.DataSource = dttbl;
        ddlsubsectionid.DataTextField = "columns_lbl";
        ddlsubsectionid.DataValueField = "columns_columnsid";
        ddlsubsectionid.DataBind();
        ddlsubsectionid.Items.Insert(0, new ListItem("Select", "0"));
    }
    private string GetSubSectionId(string section)
    {
        return section.Replace(" ", "").Replace("&", "").Replace("/", "");
    }
    
    private void PopulateSubSection()
    {
        string query = "select * from tbl_columns where columns_moduleid=" + Request.QueryString["mid"] + " AND columns_control='Section'";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        ddlsubmoduleid.DataSource = dttbl;
        ddlsubmoduleid.DataTextField = "columns_lbl";
        ddlsubmoduleid.DataValueField = "columns_columnsid";
        ddlsubmoduleid.Items.Insert(0, new ListItem("Select", "0"));
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] == null) return;

        int columnId = GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
        CP objCP = new CP();
        objCP.DeleteProjectTableColumn(columnId);
        Response.Redirect("config-module.aspx?mid=" + Request.QueryString["mid"]);
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GenerateDesignCode();
    }
    private void GenerateDesignCode()
    {
        int moduleid = Convert.ToInt32(Request.QueryString["mid"]);
        InsertUpdate obj = new InsertUpdate();
        string query = "select * from tbl_module where module_moduleid=" + moduleid;
        DataRow dr = obj.ExecuteSelectRow(query);
        string moduleName = GlobalUtilities.ConvertToString(dr["module_modulename"]);
        int menuId = GlobalUtilities.ConvertToInt(dr["module_menuid"]);
        CP objCp = new CP();
        //code changes
        objCp.GenerateAddDesignPageV2_0(dr, moduleid);
        //if (_isMobile)
        //{
        //    objCp.GenerateAddDesignPageV2_0(dr, moduleid, true);
        //}
        objCp.GenerateAddCodePage(dr, moduleid, true);
        objCp.GenerateViewPage(dr, moduleid);
        if (_isMobile)
        {
            objCp.GenerateViewPage(dr, moduleid, true);
        }
        objCp.CreateSubMenu(menuId, moduleName);
        //objCp.BindColumns(ltContainer);
        //menu
        DateTime dtcreateddate = Convert.ToDateTime(dr["module_createddate"]);
        TimeSpan ts = DateTime.Now - dtcreateddate;
        if (moduleid == 0 || ts.Days < 7 || chisoverwritexml.Checked)
        {
            objCp.GenerateViewXml(true);
        }
    }
    protected void btnSaveAndGenerate_Click(object sender, EventArgs e)
    {
        if (!IsValid()) return;
        if (txtlbl.Text == "") return;
        Save();
        GenerateDesignCode();
        Response.Redirect("config-module.aspx?mid=" + Request.QueryString["mid"]);

    }

    private bool IsValid()
    {
        string columnName = txtcolumnname.Text;
        if (!columnName.Contains("_")) columnName = h_prefix.Text + columnName;
        string query = "select * from tbl_columns where columns_submoduleid=0 AND columns_moduleid=" + Request.QueryString["mid"] + " AND columns_columnname='" + columnName + "'";
        if (Request.QueryString["id"] != null)
        {
            query += " AND columns_columnsid<>" + Request.QueryString["id"];
        }
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            return true;
        }
        else
        {
            lblMessage.Text = "Column already exists";
            lblMessage.Visible = true;
            return false;
        }
    }
    private void SetWarning()
    {
        string query = "select * from tbl_columns where columns_moduleid=" + Request.QueryString["mid"] + " order by columns_sequence";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        if (!GlobalUtilities.IsValidaTable(dttbl)) return;
        int viewCount = 0;
        int searchCount = 0;
        int requiredCount = 0;
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (GlobalUtilities.ConvertToBool(dttbl.Rows[i]["columns_isviewpage"]))
            {
                viewCount++;
            }
            if (GlobalUtilities.ConvertToBool(dttbl.Rows[i]["columns_issearchfield"]))
            {
                searchCount++;
            }
            if (GlobalUtilities.ConvertToBool(dttbl.Rows[i]["columns_isrequired"]))
            {
                requiredCount++;
            }
        }
        lblWarning.Text = "WARNING :";
        lblWarning.Visible = false;
        if (viewCount < 1)
        {
            lblWarning.Text += " No View Page Column.";
        }
        if (searchCount < 1)
        {
            lblWarning.Text += " No Search Column.";
        }
        if (requiredCount < 1)
        {
            lblWarning.Text += " No Required Field found.";
        }
        if (GlobalUtilities.ConvertToBool(dttbl.Rows[0]["columns_isautocomplete"]))
        {
            lblWarning.Text += " No Required Field found.";
        }

        if (lblWarning.Text == "WARNING :")
        {
            lblWarning.Visible = false;
        }
        else
        {
            lblWarning.Visible = true;
        }

    }
    private void Save()
    {
        string query = "select top 1 * from tbl_columns where columns_moduleid=" + Request.QueryString["mid"] + " and columns_sequence=" + txtsequence.Text;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            query = "update tbl_columns set columns_sequence=columns_sequence+1 where columns_moduleid=" + Request.QueryString["mid"] + " and columns_sequence>=" + txtsequence.Text;
            DbTable.ExecuteQuery(query);
        }
        if (txtsequence.Text == "") txtsequence.Text = "0";

        gblData.AddExtraValues("moduleid", Request.QueryString["mid"]);
        gblData.SaveForm(form);

        int moduleid = Convert.ToInt32(Request.QueryString["mid"]);
        InsertUpdate obj = new InsertUpdate();
        query = "select * from tbl_module where module_moduleid=" + moduleid;
        dr = obj.ExecuteSelectRow(query);
        string moduleName = Convert.ToString(dr["module_modulename"]);
        string tableName = Convert.ToString(dr["module_tablename"]);

        //database changes
        CP objCp = new CP();
        objCp.CheckAndCreateTable(tableName);
        objCp.CreateTableColumns(tableName, moduleid);
        objCp.AddCreatedDate(tableName);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!IsValid()) return;
        if (txtlbl.Text == "") return;
        Save();
        Response.Redirect("config-module.aspx?mid=" + Request.QueryString["mid"]);
    }
    private void SetSequence()
    {
        string query = "select top 1 * from tbl_columns where columns_moduleid=" + Request.QueryString["mid"] + " and columns_submoduleid=0 and columns_subsectionid=0 order by columns_sequence desc";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            txtsequence.Text = "1";
        }
        else
        {
            txtsequence.Text = Convert.ToString(GlobalUtilities.ConvertToInt(dr["columns_sequence"]) + 1);
        }
    }
    protected void btnDeleteModule_Click(object sender, EventArgs e)
    {
        int moduleId = GlobalUtilities.ConvertToInt(Request.QueryString["mid"]);
        CP cp = new CP();
        cp.DeleteModule(moduleId);
        Response.Redirect("~/module/view.aspx");
    }
    protected void module_Changed(object sender, EventArgs e)
    {
        if (module.SelectedIndex == 0) return;
        Response.Redirect("~/cp/config-module.aspx?mid=" + module.SelectedValue); 
    }
    protected void ddlGotoConfigPageSetting_changed(object sender, EventArgs e)
    {
        int moduleid = GlobalUtilities.ConvertToInt(Request.QueryString["mid"]);
        string modulename =Common.GetOneColumnData("tbl_module", moduleid, "modulename");
        if (ddlGotoConfigPageSetting.SelectedValue == "0") return;
        string strPage = ddlGotoConfigPageSetting.SelectedValue;

        if (strPage == "configure-viewpage") strPage = "configure-viewpage.aspx?mname=" + modulename + "&mid=" + moduleid + "&isSubGrid=true";
        else if (strPage == "controlpanelview-setting") strPage = "controlpanelview-setting.aspx?mid=" + moduleid;
        else if (strPage == "Configure-UpdatableGrid") strPage = "configure-UpdatableGrid.aspx?mid=" + moduleid + "&mname=" + modulename + "&isUpdatebleGrid=true";
        else if (strPage == "configure-viewpage-view") strPage = "configure-viewpage.aspx?mname=" + modulename + "&mid=" + moduleid;
        else
        {
            strPage = strPage + ".aspx?mid=" + moduleid;
            if (Request.QueryString["id"] != null)
            {
                strPage = strPage + "&cid=" + Request.QueryString["id"];
            }
        }
        Response.Redirect("~/cp/" + strPage);
    }
    private void SetModuleName()
    {
        string modulename = Common.GetOneColumnData("tbl_module", GlobalUtilities.ConvertToInt(Request.QueryString["mid"]), "modulename");
        h_modulename.Text = modulename;
    }
}
