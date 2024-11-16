using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.IO;

public partial class CP_copymodule : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private void CopyModule()
    {
        string shortModule = txtModuleName.Text.Replace(" ", "");
        string shortCopyModule = txtCopyModule.Text.Replace(" ", "");
        string query = "select * from tbl_module where replace(module_modulename,' ','')='" + txtModuleName.Text + "'";
        DataRow drModule = ExecuteSelect(query).Rows[0];
        Hashtable hstblModule=new Hashtable();
        int moduleId = CopyRow(drModule, hstblModule, "module");
        
        query = "select * from tbl_columns where columns_moduleid="+GlobalUtilities.ConvertToInt(drModule["module_moduleid"]);
        DataTable dttblColumn = ExecuteSelect(query);
        for (int i = 0; i < dttblColumn.Rows.Count; i++)
        {
            Hashtable hstblColumn = new Hashtable();
            hstblColumn.Add("moduleid", moduleId);
            int columnId = CopyRow(dttblColumn.Rows[i], hstblColumn, "column");
        }
        
        string copyFolder = "";
        if (txtCopyProjectFolder.Text == "")
        {
            copyFolder = Server.MapPath("~/");
        }
        else
        {
            copyFolder = txtCopyProjectFolder.Text;
        }
        if (!copyFolder.EndsWith("/"))
        {
            copyFolder = copyFolder + "/";
        }
        GlobalUtilities.CopyFolder(copyFolder + shortCopyModule, Server.MapPath("~/" + shortModule));
        File.Copy(copyFolder + "xml/view/" + shortCopyModule + ".xml", Server.MapPath("~/xml/view/" + shortModule + ".xml"), true);

    }
    private int CopyRow(DataRow drSource, Hashtable hstbl, string module)
    {
        for (int i = 1; i < drSource.Table.Columns.Count; i++)
        {
            string columnName = drSource.Table.Columns[i].ColumnName;
            if (columnName.Contains("_createddate") || columnName.Contains("_modifieddate") ||
                columnName.Contains("_createdby") || columnName.Contains("_modifiedby"))
            {
            }
            else
            {
                columnName = Common.GetColumnName(columnName);
                hstbl.Add(columnName, GlobalUtilities.ConvertToString(drSource.Table.Columns[i].ColumnName));
            }
        }
        InsertUpdate obj = new InsertUpdate();
        int id = obj.InsertData(hstbl, "tbl_"+module);
        return id;
    }
    private bool IsValid()
    {
        if (txtCopyProjectConnectionString.Text != "")
        {
            if (txtCopyProjectFolder.Text == "")
            {
                lblMessage.Text = "Please enter copy folder";
                lblMessage.Visible = true;
                return false;
            }
        }
        if (txtCopyProjectFolder.Text != "")
        {
            if (txtCopyProjectConnectionString.Text == "")
            {
                lblMessage.Text = "Please enter copy connection string";
                lblMessage.Visible = true;
                return false;
            }
        }
        string query = "select * from tbl_module where replace(module,' ','')='" + txtCopyModule.Text.Replace(" ","") + "'";
        DataTable dttblCopyModule = ExecuteSelect(query);
        if (!GlobalUtilities.IsValidaTable(dttblCopyModule))
        {
            lblMessage.Text = "Invalid copy module";
            lblMessage.Visible = true;
            return false;
        }
        query = "select * from tbl_module where replace(module,' ','')='" + txtCopyModule.Text.Replace(" ", "") + "'";
        DataTable dttblModule = DbTable.ExecuteSelect(query);
        if (GlobalUtilities.IsValidaTable(dttblModule))
        {
            lblMessage.Text = "Module table data already exists";
            lblMessage.Visible = true;
            return false;
        }
        return true;
    }
    private DataTable ExecuteSelect(string query)
    {
        DataTable dttblSelectData = new DataTable();
        if (txtCopyProjectConnectionString.Text == "")
        {
            dttblSelectData = DbTable.ExecuteSelect(query);
        }
        else
        {
            dttblSelectData = ExecuteSelect_CopyDb(query);
        }
        return dttblSelectData;
    }
    private DataTable ExecuteSelect_CopyDb(string query)
    {
        DataTable dttblSelectData = new DataTable();
        using (SqlConnection con = new SqlConnection(txtCopyProjectConnectionString.Text))
        {
            SqlDataAdapter daSelectData_SQL = null;
            daSelectData_SQL = new SqlDataAdapter(query, con);
            daSelectData_SQL.SelectCommand.CommandType = CommandType.Text;
            daSelectData_SQL.Fill(dttblSelectData);
        }
        return dttblSelectData;
    }
    protected void btnCopy_Click(object sender, EventArgs e)
    {
        if (IsValid())
        {
            CopyModule();
        }
    }
    
}
