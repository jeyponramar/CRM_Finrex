using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using WebComponent;

/// <summary>
/// Summary description for CommonDb
/// </summary>
public static class CommonDb
{
    public static void UpdateCountInParent(string setting)
    {
        string m = Common.GetModuleName();
        Array arrParentModules = setting.Split(',');
        DataRow dr = Common.GetCurrentModuleRow();
        if (dr == null) return;
        //bool isActiveColumnExists = IsColumnExistsInRow(dr, "isactive");
        for (int i = 0; i < arrParentModules.Length; i++)
        {
            string parentModule = Convert.ToString(arrParentModules.GetValue(i));
            int parentId = GlobalUtilities.ConvertToInt(dr[m + "_" + parentModule + "id"]);
            if (parentId > 0)
            {
                string parentColumn = parentModule + "_" + parentModule + "id";
                string query = "UPDATE tbl_" + parentModule + " " +
                            "SET " + parentModule + "_" + m + "count = (SELECT COUNT(*) FROM tbl_" + m + " WHERE " + m + "_" + parentModule + "id=" + parentId + ") " +
                            "FROM tbl_" + parentModule + " WHERE " + parentModule + "_" + parentModule + "id = " + parentId;
                DbTable.ExecuteQuery(query);
            }
        }

    }
    private static bool IsColumnExistsInRow(DataRow dr, string columnName)
    {
        for (int i = 0; i < dr.Table.Columns.Count; i++)
        {
            if (dr.Table.Columns[i].ColumnName == columnName)
            {
                return true;
            }
        }
        return false;
    }
}
