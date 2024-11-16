using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Xml;
using System.Text;
using System.Data;

public partial class controlpanelview_setting : System.Web.UI.Page
{
    GlobalData globalData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Request.QueryString["m"] == null) Response.End();
        if (!IsPostBack)
        {
            //string query = "select * from tbl_columns join tbl_module module_moduleid=columns_moduleid where replace(module_modulename,' ','')='" + Request.QueryString["m"] + "'";
            //InsertUpdate obj = new InsertUpdate();
            //DataTable dttbl = obj.ExecuteSelect(query);
            //globalData.FillDropdown(ddlSort, "tbl_columns", "columns_lbl", "replace(module_modulename,' ','')='"+Request.QueryString["m"]+"'");
            BindColumns();
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('Grid Setting -Quick Page Alignment Settings " + Request.QueryString["m"] + "')</script>");
    }
    private void BindSortColumns()
    {

    }
    private void BindColumns()
    {
        int int_moduleId = GlobalUtilities.ConvertToInt(Request.QueryString["mid"]);
        string str_ModuleName = GlobalUtilities.ConvertToString(Request.QueryString["m"]);
        str_ModuleName = (str_ModuleName == "") ? Common.GetOneColumnData("tbl_module", int_moduleId, "modulename") : str_ModuleName;
        ViewState["ModuleId"] = int_moduleId;
        ViewState["ModuleName"] = str_ModuleName;
        string query = @"SELECT * FROM tbl_columns WHERE columns_moduleid =" + int_moduleId + " AND (columns_sequence <(SELECT TOP 1 ISNULL(columns_sequence,0) FROM tbl_columns WHERE columns_submoduleid>0 ORDER BY columns_sequence) OR 0=ISNULL(columns_submoduleid,0)) ORDER BY columns_sequence";
        DataTable dtColumns = DbTable.ExecuteSelect(query);
        if (GlobalUtilities.IsValidaTable(dtColumns))
        {
            for (int i = 0; i < dtColumns.Rows.Count; i++)
            {
                int intColumnId = GlobalUtilities.ConvertToInt(dtColumns.Rows[i]["columns_columnsid"]);
                string strColumnsLbl = GlobalUtilities.ConvertToString(dtColumns.Rows[i]["columns_lbl"]);

                if (GlobalUtilities.ConvertToBool(dtColumns.Rows[i]["columns_isleftcolumn"]))
                {
                    lstLeft.Items.Add(new ListItem(strColumnsLbl,intColumnId.ToString()));
                }
                else
                {
                    lstRight.Items.Add(new ListItem(strColumnsLbl, intColumnId.ToString()));
                }
            }
        }  
    }
    private void updateSequence(string Columns)
    {
        Array arr = Columns.Split(',');
        for (int i = 0; i < arr.Length; i++)
        {
            int ColumnId = GlobalUtilities.ConvertToInt(arr.GetValue(i));
            DbTable.ExecuteQuery("UPDATE tbl_columns SET columns_sequence=" + i + 1 + " WHERE columns_columnsid=" + ColumnId);
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string strQuery = "UPDATE tbl_columns SET columns_isleftcolumn=1 WHERE columns_columnsid IN("+hdnGridLeftColumns.Text+")";
        DbTable.ExecuteQuery(strQuery);
        strQuery = "UPDATE tbl_columns SET columns_isleftcolumn=0 WHERE columns_columnsid IN(" + hdnGridRightColumns.Text + ")";
        DbTable.ExecuteQuery(strQuery);
        updateSequence(hdnGridLeftColumns.Text);
        updateSequence(hdnGridRightColumns.Text);

        int moduleid = GlobalUtilities.ConvertToInt(Request.QueryString["mid"]);
        string query = "select * from tbl_module where module_moduleid=" + moduleid;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        string moduleName = GlobalUtilities.ConvertToString(dr["module_modulename"]);
        int menuId = GlobalUtilities.ConvertToInt(dr["module_menuid"]);
        string tableName = Convert.ToString(dr["module_tablename"]);

        CP objCp = new CP();
        
        objCp.GenerateAddDesignPageV2_0(dr, moduleid);
        //objCp.GenerateAddCodePage(dr, moduleid, true);


        lstLeft.Items.Clear();
        lstRight.Items.Clear();
        BindColumns();
        lblMessage.Text = "Page Settings Updated sucessfully!!!";
        lblMessage.Visible = true;
    }
}
