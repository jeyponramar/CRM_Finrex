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

public partial class CP_config_bulkedit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindColumnDetail();
        }
    }
    private void BindColumnDetail()
    {
        StringBuilder html = new StringBuilder();
        int moduleid = GlobalUtilities.ConvertToInt(Request.QueryString["mid"]);
        string query = "select * from tbl_columns where columns_moduleid=" + moduleid + " AND columns_subsectionid=0 AND columns_submoduleid=0 order by columns_sequence";
        DataTable dttblColumns = DbTable.ExecuteSelect(query);
        html.Append("<table class='repeater' width='100%' border=1><tr class='repeater-header'>" +
                    "<td width='85'>Control</td><td width='85'>Label</td><td width='105'>Column Name</td><td width='85'>DropDownModule</td><td width='30'>Sequence</td>" +
                    "<td width='85'>Section</td><td width='85'>GridModule</td>"+
                    "<td title='Row Attributes'>Row Attr</td><td>CSS</td><td>Attributes</td><td width='170'>Settings</td><td width='25' title='ColSpan'>Colspan</td>" +
                    "<td width='20' title='Is Left'>IL</td><td width='20' title='Is Required'>IR</td><td width='20' title='Is Unique'>IU</td>" +
                    "<td width='20' title='View'>V</td><td width='20' title='Search'>S</td></tr>");
        int count = 0;
        string sectionHtml = GetSection(dttblColumns);
        string subGridHtml = GetGrid(dttblColumns);
        string tblname = Common.GetOneColumnData("tbl_module", GlobalUtilities.ConvertToInt(Request.QueryString["mid"]), "tablename");
        txtPrefix.Text = tblname.Replace("tbl_", "") + "_";
        for (int i = 0; i < dttblColumns.Rows.Count; i++)
        {
            DataRow dr = dttblColumns.Rows[i];
            string control = Convert.ToString(dr["columns_control"]);
            string label = Convert.ToString(dr["columns_lbl"]);
            int columId = GlobalUtilities.ConvertToInt(dr["columns_columnsid"]);

            if (control == "Section" || control == "Sub Grid")
            {
                if (control == "Section")
                {
                    query = "select * from tbl_columns where columns_subsectionid=" + columId + " order by columns_sequence";
                }
                else
                {
                    query = "select * from tbl_columns where columns_submoduleid=" + columId + " order by columns_sequence";
                }
                html.Append("<tr><td class='bold' colspan='12'>" + label + "</td></tr>");
                
                DataTable dttblSubColumns = DbTable.ExecuteSelect(query);
                for (int j = 0; j < dttblSubColumns.Rows.Count; j++)
                {
                    dr = dttblSubColumns.Rows[j];
                    GetRowHtml(dr, count, html, sectionHtml, subGridHtml);
                    count++;
                }
            }
            else
            {
                GetRowHtml(dr, count, html, sectionHtml, subGridHtml);
            }

            count++;
        }
        //for add
        for (int i = 0; i < 10; i++)
        {
            html.Append("<tr><td>" + GetControlList(count, "").Replace("$AddUpdate$", "@ColInsert").Replace("~id~", "~0~") + "</td>" +
                            "<td><input type='text' title='Label' name='@ColInsert_label_" + count + "' class='collabel'/></td>" +
                            "<td><input type='text' title='Column Name' name='@ColInsert_colname_" + count + "' class='colname'/></td>" +
                            "<td><input type='text' title='DropDown Module' class='ac dropdownmoduleid' m='module' cn='modulename' name='@ColInsert_dropdownmodule_" + count + "'/><input name='@ColInsert_dropdownmoduleid_" + count + "' type='text' class='hdn hdnac'/><input name='@ColInsert_dropdownmodulecolumn_" + count + "' type='text' class='hdn dropdowncolumn'/></td>" +
                            "<td><input type='text' title='Sequence' name='@ColInsert_sequence_" + count + "' class='sequence'/></td>" +
                            "<td><select title='Section' name='@ColInsert_section_" + count + "' class='section'>"+sectionHtml+"</select></td>" +
                            "<td><select title='GridModule' name='@ColInsert_gridmodule_" + count + "' class='gridmodule'>" + subGridHtml + "</select></td>" +

                            "<td><input type='text' title='Row Attributes' name='@ColInsert_trattributes_" + count + "' class='rowattr'/></td>" +
                            "<td><input type='text' title='CSS' name='@ColInsert_css_" + count + "' class='css'/></td>" +
                            "<td><input type='text' title='Attributes' name='@ColInsert_attributes_" + count + "' class='attributies'/></td>" +
                            "<td><textarea title='Settings' name='@ColInsert_settings_" + count + "' class='settings' ></textarea ></td>" +
                            "<td><input type='text' title='Col Span' name='@ColInsert_colspan_" + count + "' class='rowattr'/></td>" +

                            "<td><input type='checkbox' title='IsLeft' name='@ColInsert_isleft_" + count + "' class='isleft'/></td>" +
                            "<td><input type='checkbox' title='IsReq' name='@ColInsert_isrequired_" + count + "' class='isrequired'/></td>" +
                            "<td><input type='checkbox' title='IsUniq' name='@ColInsert_isunique_" + count + "' class='isunique'/></td>" +
                            "<td><input type='checkbox' title='View' name='@ColInsert_isview_" + count + "' class='isview'/></td>" +
                            "<td><input type='checkbox' title='Search' name='@ColInsert_issearch_"+count+"' class='issearch'/></td>" +
                        "</tr>");
            count++;
        }
        html.Append("</table>");
        ltColumnDetail.Text = html.ToString();
    }

    private void GetRowHtml(DataRow dr, int index, StringBuilder html, string sectionHtml, string subGridHtml)
    {
        string columnName = Convert.ToString(dr["columns_columnname"]);
        int columnId = GlobalUtilities.ConvertToInt(dr["columns_columnsid"]);
        string control = Convert.ToString(dr["columns_control"]);
        string label = Convert.ToString(dr["columns_lbl"]);
        int sequence = GlobalUtilities.ConvertToInt(dr["columns_sequence"]);
        bool isrequired = Convert.ToBoolean(dr["columns_isrequired"]);
        bool isunique = Convert.ToBoolean(dr["columns_isunique"]);
        int subsectionid = GlobalUtilities.ConvertToInt(dr["columns_subsectionid"]);
        int submoduleid = GlobalUtilities.ConvertToInt(dr["columns_submoduleid"]);
        string dropdownColumnName = Convert.ToString(dr["columns_dropdowncolumn"]);
        int dropdownmoduleid = GlobalUtilities.ConvertToInt(dr["columns_dropdownmoduleid"]);
        string dropdownmodulename = Common.GetOneColumnData("tbl_module", dropdownmoduleid, "modulename");
        bool isLeft = GlobalUtilities.ConvertToBool(dr["columns_isleftcolumn"]);
        bool isViewPage = GlobalUtilities.ConvertToBool(dr["columns_isviewpage"]);
        bool isSearchfield = GlobalUtilities.ConvertToBool(dr["columns_issearchfield"]);
        string css = GlobalUtilities.ConvertToString(dr["columns_css"]);
        string attr =GlobalUtilities.ConvertToString(dr["columns_attributes"]);
        string rowattr = GlobalUtilities.ConvertToString(dr["columns_trattributes"]);
        string settings = GlobalUtilities.ConvertToString(dr["columns_settings"]);
        if (columnName.Contains("_"))
        {
            columnName = columnName.Substring(columnName.IndexOf("_") + 1);
        }
        string strIsLeft = "";
        string strIsRequired = "";
        string strIsUnique = "";
        string strIsView = "";
        string strIsSearch = "";

        if (isLeft) strIsLeft = " checked='checked'";
        if (isrequired) strIsRequired = " checked='checked'";
        if (isunique) strIsUnique = " checked='checked'";
        if (isViewPage) strIsView = " checked='checked'";
        if (isSearchfield) strIsSearch = " checked='checked'";



        html.Append("<tr><td>" + GetControlList(index, control).Replace("$AddUpdate$", "@ColUpdate").Replace("~id~", "~"+columnId.ToString()+"~") + "</td>" +
                        "<td><input type='text' title='Label:"+label+"' name='@ColUpdate_label_" + index + "' class='collabel' value='" + label + "'/></td>" +
                        "<td><input type='text' title='Column Name:"+columnName+"' name='@ColUpdate_colname_" + index + "' class='colname' value='" + columnName + "'/></td>" +
                        "<td><input type='text' title='DropDown Module' class='ac dropdownmoduleid' m='module' cn='modulename' name='@ColUpdate_dropdownmodule_" + index + "' value='" + dropdownmodulename + "'/><input name='@ColUpdate_dropdownmoduleid_" + index + "' type='text' class='hdn hdnac' value='" + dropdownmoduleid + "' /><input name='@ColUpdate_dropdownmodulecolumn_" + index + "' type='text' class='hdn dropdowncolumn' value='" + dropdownColumnName + "'/></td>" +
                        "<td><input type='text' title='Sequence' name='@ColUpdate_sequence_" + index + "' class='sequence' value='" + sequence + "'/></td>" +
                        "<td><select name='@ColUpdate_section_" + index + "' title='Section' class='section ddlselectedval' selectedval='" + subsectionid + "'>" + sectionHtml + "/select></td>" +
                        "<td><select name='@ColUpdate_gridmodule_" + index + "' title='GridModule' class='gridmodule ddlselectedval' selectedval='" + submoduleid + "'>" + subGridHtml + "</select></td>" +


                        "<td><input type='text' title='Row Attributes:"+rowattr+"' name='@ColUpdate_trattributes_" + index + "' class='rowattr' value='" + rowattr + "'/></td>" +
                        "<td><input type='text' title='CSS:"+css+"' name='@ColUpdate_css_" + index + "' class='css' value='" + css + "'/></td>" +
                        "<td><input type='text' title='Attributes:"+attr+"' name='@ColUpdate_attributes_" + index + "' class='attributies' value='" + attr + "'/></td>" +
                        "<td><textarea title='Settings:" + settings + "' name='@ColUpdate_settings_" + index + "' class='settings' value='" + settings + "'></textarea></td>" +
                        "<td><input type='text' title='Col Span' name='@ColUpdate_colspan_" + index + "' class='colspan' value='" + GlobalUtilities.ConvertToString(dr["columns_colspan"]) + "'/></td>" +

                        "<td><input type='checkbox' name='@ColUpdate_isleft_"+index+"' title='IsLeft' class='isleft'" + strIsLeft + "/></td>" +
                        "<td><input type='checkbox' name='@ColUpdate_isrequired_"+index+"' title='IsReq' class='isrequired'" + strIsRequired + "/></td>" +
                        "<td><input type='checkbox' name='@ColUpdate_isunique_"+index+"' title='IsUniq' class='isunique'" + strIsUnique + "/></td>" +
                        "<td><input type='checkbox' name='@ColUpdate_isview_"+index+"' title='View' class='isview'" + strIsView + "/></td>" +
                        "<td><input type='checkbox' name='@ColUpdate_issearch_"+index+"' title='Search' class='issearch'" + strIsSearch + "/></td>" +
                    "</tr>");
    }
    private string GetControlList(int index, string selectedControl)
    {

        string controls = "<select title='Control:"+selectedControl+"' name=\"$AddUpdate$_Control_"+index+"~id~\" class=\"ddlcontrol\" selectedval='" + selectedControl + "'>" +
                                "<option value=\"Text Box\">Text Box</option>" +
                                "<option value=\"Label\">Label</option>" +
                                "<option value=\"Auto Complete\">Auto Complete</option>" +
                                "<option value=\"Email Id\">Email Id</option>" +
                                "<option value=\"Phone No\">Phone No</option>" +
                                "<option value=\"Amount\">Amount</option>" +
                                "<option value=\"Number\">Number</option>" +
                                "<option value=\"Date\">Date</option>" +
                                "<option value=\"Date Time\">Date Time</option>" +
                                "<option value=\"Multi Line\">Multi Line</option>" +
                                "<option value=\"Html Editor\">Html Editor</option>" +
                                "<option value=\"Dropdown\">Dropdown</option>" +
                                "<option value=\"Checkbox\">Checkbox</option>" +
                                "<option value=\"File Upload\">File Upload</option>" +
                                "<option value=\"Section\">Section</option>" +
                                "<option value=\"Sub Grid\">Sub Grid</option>" +
                                "<option value=\"Non Editable Grid\">Non Editable Grid</option>" +
                                "<option value=\"Updateable Grid\">Updateable Grid</option>" +
                                "<option value=\"Button\">Button</option>" +
                                "<option value=\"Literal\">Literal</option>" +
                                "<option value=\"Row HTML\">Row HTML</option>" +
                        "</select>";
        return controls;

    }
    private string GetSection(DataTable dttblColumns)
    {
        StringBuilder html = new StringBuilder();
        html.Append("<option value='0'>Select</option>");
        for (int i = 0; i < dttblColumns.Rows.Count; i++)
        {
            DataRow dr = dttblColumns.Rows[i];
            string label = Convert.ToString(dr["columns_lbl"]);
            string control = Convert.ToString(dr["columns_control"]);
            int sectionId = GlobalUtilities.ConvertToInt(dr["columns_columnsid"]);
            string selected = "";
            if (control == "Section")
            {
                html.Append("<option value='" + sectionId + "'>" + label + "</option>");
            }
        }
        return html.ToString();
    }
    private string GetGrid(DataTable dttblColumns)
    {
        StringBuilder html = new StringBuilder();
        html.Append("<option value='0'>Select</option>");
        for (int i = 0; i < dttblColumns.Rows.Count; i++)
        {
            DataRow dr = dttblColumns.Rows[i];
            string label = Convert.ToString(dr["columns_lbl"]);
            string control = Convert.ToString(dr["columns_control"]);
            int sectionId = GlobalUtilities.ConvertToInt(dr["columns_columnsid"]);
            if (control == "Sub Grid")
            {
                html.Append("<option value='" + sectionId + "'>" + label + "</option>");
            }
        }
        return html.ToString();
    }
    protected void btn_CreateColumnClick(object sender, EventArgs e)
    {
        int prevRowindex = -1;
        int rowIndex = 0;        
        int moduleid =GlobalUtilities.ConvertToInt(Request.QueryString["mid"]);
        var _Form = HttpContext.Current.Request.Form;
        string prefix = "";
        DataRow drm = DbTable.GetOneRow("tbl_module", moduleid);
        string moduleName = GlobalUtilities.ConvertToString(drm["module_modulename"]).Replace(" ", "").ToLower();
        prefix = moduleName + "_";
        for (int i = 0; i < Request.Form.Keys.Count; i++)
        {
            string KeyName = Request.Form.Keys[i];
            if (KeyName.StartsWith("@ColUpdate") || KeyName.StartsWith("@ColInsert"))
            {
                string _key = (KeyName.StartsWith("@ColUpdate")) ? "@ColUpdate" : "@ColInsert";
                Array arr = KeyName.Split('_');
                int _id = 0;
                string RowIndex_val = arr.GetValue(2).ToString();
                if (RowIndex_val.Contains('~'))
                    rowIndex = GlobalUtilities.ConvertToInt(RowIndex_val.Substring(0, RowIndex_val.IndexOf('~')));
                if (prevRowindex != rowIndex)
                {
                    if (KeyName.Contains("~"))
                    {
                        _id = GlobalUtilities.ConvertToInt(KeyName.Split('~').GetValue(1));
                    }
                    string control = GlobalUtilities.ConvertToString(_Form[_key + "_Control_" + rowIndex + "~" + _id + "~"]);
                    string label = GlobalUtilities.ConvertToString(_Form[_key + "_label_" + rowIndex]);
                    string colname = GlobalUtilities.ConvertToString(_Form[_key + "_colname_" + rowIndex]);
                    string dropdownmodulecolumn = GlobalUtilities.ConvertToString(_Form[_key + "_dropdownmodulecolumn_" + rowIndex]);
                    int dropdownmoduleid = GlobalUtilities.ConvertToInt(_Form[_key + "_dropdownmoduleid_" + rowIndex]);
                    int sequence = GlobalUtilities.ConvertToInt(_Form[_key + "_sequence_" + rowIndex]);
                    string section = GlobalUtilities.ConvertToString(_Form[_key + "_section_" + rowIndex]);
                    string gridmodule = GlobalUtilities.ConvertToString(_Form[_key + "_gridmodule_" + rowIndex]);
                    bool isleft = GlobalUtilities.ConvertToBool(_Form[_key + "_isleft_" + rowIndex]);
                    bool isrequired = GlobalUtilities.ConvertToBool(_Form[_key + "_isrequired_" + rowIndex]);
                    bool isunique = GlobalUtilities.ConvertToBool(_Form[_key + "_isunique_" + rowIndex]);
                    bool isview = GlobalUtilities.ConvertToBool(_Form[_key + "_isview_" + rowIndex]);
                    bool issearch = GlobalUtilities.ConvertToBool(_Form[_key + "_issearch_" + rowIndex]);

                    if (!colname.Contains("_")) colname = prefix + colname;
                    Hashtable hstbl = new Hashtable();
                    hstbl.Add("control", control);
                    hstbl.Add("lbl", label);
                    hstbl.Add("columnname", colname);
                    hstbl.Add("dropdowncolumn", dropdownmodulecolumn);
                    hstbl.Add("dropdownmoduleid", dropdownmoduleid);
                    hstbl.Add("sequence", sequence);
                    //hstbl.Add("gridmodule", gridmodule);
                    hstbl.Add("subsectionid", section);
                    hstbl.Add("submoduleid", gridmodule);
                    hstbl.Add("isleftcolumn", isleft);
                    hstbl.Add("isrequired", isrequired);
                    hstbl.Add("isunique", isunique);
                    hstbl.Add("isviewpage", isview);
                    hstbl.Add("issearchfield", issearch);
                    InsertUpdate obj = new InsertUpdate();
                    if (_id > 0)
                     {
                        updateColumnSequence(sequence, moduleid, _id);
                        obj.UpdateData(hstbl, "tbl_columns", _id);
                    }
                    else
                    {
                        hstbl.Add("moduleid", moduleid);
                        if (label != "" && control != "" && colname != "")
                        {
                            updateColumnSequence(sequence, moduleid, 0);
                            obj.InsertData(hstbl, "tbl_columns");
                        }
                    }
                }
                prevRowindex = rowIndex;
            }
        }
        GenerateDesignCode();
        BindColumnDetail();
        lblMessage.Text = "Update's are done Sucessfully!!!";
        lblMessage.Visible = true;
        
    }
    private void GenerateDesignCode()
    {
        int moduleid = Convert.ToInt32(Request.QueryString["mid"]);
        InsertUpdate obj = new InsertUpdate();
        string query = "select * from tbl_module where module_moduleid=" + moduleid;
        DataRow dr = obj.ExecuteSelectRow(query);
        string moduleName = GlobalUtilities.ConvertToString(dr["module_modulename"]);
        int menuId = GlobalUtilities.ConvertToInt(dr["module_menuid"]);
        string tableName = Convert.ToString(dr["module_tablename"]);

        CP objCp = new CP();
        //database changes
        objCp.CheckAndCreateTable(tableName);
        objCp.CreateTableColumns(tableName, moduleid);
        objCp.AddCreatedDate(tableName);

        //code changes

        objCp.GenerateAddDesignPageV2_0(dr, moduleid);
        objCp.GenerateAddCodePage(dr, moduleid, true);
        objCp.GenerateViewPage(dr, moduleid);
        objCp.CreateSubMenu(menuId, moduleName);
        //objCp.BindColumns(ltContainer);
        //menu
        objCp.GenerateViewXml(true);
    }
    protected void btnResetSequence_Click(object sender, EventArgs e)
    {
        int moduleid = GlobalUtilities.ConvertToInt(Request.QueryString["mid"]);
        string query = "select * from tbl_columns where columns_moduleid=" + moduleid + " AND columns_subsectionid=0 AND columns_submoduleid=0 order by columns_columnsid";
        DataTable dttblColumns = DbTable.ExecuteSelect(query);

        int count = 0;
        int sectionColcount = 0;
        int gridColcount = 0;
        for (int i = 0; i < dttblColumns.Rows.Count; i++)
        {
            DataRow dr = dttblColumns.Rows[i];
            string control = Convert.ToString(dr["columns_control"]);
            string label = Convert.ToString(dr["columns_lbl"]);
            int columId = GlobalUtilities.ConvertToInt(dr["columns_columnsid"]);

            if (control == "Section" || control == "Sub Grid")
            {
                if (control == "Section")
                {
                    query = "select * from tbl_columns where columns_subsectionid=" + columId + " order by columns_sequence";
                }
                else
                {
                    query = "select * from tbl_columns where columns_submoduleid=" + columId + " order by columns_sequence";
                }

                DataTable dttblSubColumns = DbTable.ExecuteSelect(query);
                for (int j = 0; j < dttblSubColumns.Rows.Count; j++)
                {
                    int _count = (control == "Section") ? ++sectionColcount : ++gridColcount;
                    dr = dttblSubColumns.Rows[j];
                    resetColumnSequence(_count,columId);
                }
            }
            else
            {
                count++;
                resetColumnSequence(count, columId);
            }
        }

        BindColumnDetail();
        lblMessage.Text = "Sequence's are reseted sucessfully!!!!!!!!";
        lblMessage.Visible = true;
    }
    private void resetColumnSequence(int sequence, int columnid)
    {
        string updatequery = "UPDATE tbl_columns SET columns_sequence =" + sequence + " WHERE columns_columnsid=" + columnid;
        DbTable.ExecuteQuery(updatequery);
    }
    private void updateColumnSequence(int SequenceNo, int ModuleId,int columnid)
    {
        if (columnid > 0)
        {
            string query = @"IF NOT EXISTS
                      ( SELECT * FROM tbl_columns WHERE columns_moduleid=" + ModuleId + " AND columns_columnsid="+columnid+" AND columns_sequence=" + SequenceNo +
                           @")      
                      BEGIN
		                UPDATE tbl_columns SET columns_sequence = columns_sequence+1 WHERE columns_moduleid=" + ModuleId + " AND ISNULL(columns_sequence,0)>=" + SequenceNo +
                           " END";
            DbTable.ExecuteQuery(query);
        }
        else
        {
            string query = @"IF EXISTS
                      ( SELECT * FROM tbl_columns WHERE columns_moduleid=" + ModuleId + " AND columns_sequence=" + SequenceNo +
                           @")      
                      BEGIN
		                UPDATE tbl_columns SET columns_sequence = columns_sequence+1 WHERE columns_moduleid=" + ModuleId + " AND ISNULL(columns_sequence,0)>=" + SequenceNo +
                           " END";
            DbTable.ExecuteQuery(query);
        }
    }
}
