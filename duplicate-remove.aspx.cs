using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WebComponent;
using System.Text;

public partial class fixduplicate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CustomSession.Session("Login_IsRefuxLoggedIn") == null)
        {
            Response.Redirect("../adminlogin.aspx");
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string columnName = txtColumnName.Text;
        if (!columnName.Contains("_")) columnName = txtModule.Text + "_" + columnName;
        string query = "select count(*) c," + columnName + " from tbl_" + txtModule.Text +
                        " group by "+columnName+" having count(*)>1";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        ltDuplicateItems.Text = Common.BindGrid(dttbl, columnName + ",c", "Item,Count");

        query = "select module_modulename,columns_submoduleid from tbl_columns " +
                "join tbl_module ON module_moduleid=columns_moduleid " +
                "where columns_dropdowncolumn='" + columnName + "'";
        DataTable dttblModules = DbTable.ExecuteSelect(query);

        DataTable dttblTables = new DataTable();
        dttblTables.Columns.Add("TableName");
        for (int i = 0; i < dttblModules.Rows.Count; i++)
        {
            int submoduleId = GlobalUtilities.ConvertToInt(dttblModules.Rows[i]["columns_submoduleid"]);
            string tableName = "";
            if (submoduleId > 0)
            {
                query = "select * from tbl_columns WHERE columns_columnsid=" + submoduleId;
                DataRow dr = DbTable.ExecuteSelectRow(query);
                string col = GlobalUtilities.ConvertToString(dr["columns_columnname"]);
                col = Common.GetColumnName(col);
                tableName = "tbl_" + col;
            }
            else
            {
                tableName = "tbl_" + GlobalUtilities.ConvertToString(dttblModules.Rows[i]["module_modulename"]).Replace(" ", "").ToLower();
            }
            DataRow drTable = dttblTables.NewRow();
            drTable["TableName"] = tableName;
            dttblTables.Rows.Add(drTable);
        }
        ltDuplicateTables.Text = Common.BindGrid(dttblTables, "TableName", "Table Name");
        StringBuilder modules = new StringBuilder();
        for (int i = 0; i < dttblTables.Rows.Count; i++)
        {
            if (i == 0)
            {
                modules.Append(dttblTables.Rows[i]["TableName"]);
            }
            else
            {
                modules.Append("," + dttblTables.Rows[i]["TableName"]);
            }
        }
        txtRemoveDuplicateModule.Text = modules.ToString();

    }
    protected void btnRemoveDuplicates_Click(object sender, EventArgs e)
    {
        Array arrRemoveModules = txtRemoveDuplicateModule.Text.Split(',');
        string module = txtModule.Text;
        string columnName = txtColumnName.Text;
        if (!columnName.Contains("_")) columnName = txtModule.Text + "_" + columnName;
        string query = "select count(*) c," + columnName + " from tbl_" + module +
                        " group by " + columnName + " having count(*)>1";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        string idcolumn = module + "_" + module + "id";
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string duplicatedata = global.CheckData(GlobalUtilities.ConvertToString(dttbl.Rows[i][columnName]));
            query = "select * from tbl_" + module + " WHERE " + columnName + "='" + duplicatedata + "'";
            DataTable dttblParent = DbTable.ExecuteSelect(query);

            //keep the first value and remove all other values
            int acutualId = GlobalUtilities.ConvertToInt(dttblParent.Rows[0][idcolumn]);
            StringBuilder removeIds = new StringBuilder();
            for (int j = 1; j < dttblParent.Rows.Count; j++)
            {
                if (j == 1)
                {
                    removeIds.Append(GlobalUtilities.ConvertToInt(dttblParent.Rows[j][idcolumn]));
                }
                else
                {
                    removeIds.Append("," + GlobalUtilities.ConvertToInt(dttblParent.Rows[j][idcolumn]));
                }
            }
            for (int j = 0; j < arrRemoveModules.Length; j++)
            {
                string removeModule = arrRemoveModules.GetValue(j).ToString().ToLower();
                if (removeModule.Contains("tbl_")) removeModule = removeModule.Replace("tbl_", "");
                query = "update tbl_" + removeModule + " set " + removeModule + "_" + module + "id=" + acutualId +
                        " WHERE " + removeModule + "_" + module + "id IN(" + removeIds.ToString() + ")";
                bool success = DbTable.ExecuteQuery(query);
            }
            //update the parent module
            query = "delete from tbl_" + module + " WHERE " + module + "_" + module + "id IN(" + removeIds.ToString() + ")";
            bool isdeleted = DbTable.ExecuteQuery(query);
        }
        lblMessage.Text = "Duplicates successfully removed!";
    }
}
