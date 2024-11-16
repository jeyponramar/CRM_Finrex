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
using System.Data.OleDb;
using System.IO;
using WebComponent;
using System.Text;
using System.Text.RegularExpressions;

public partial class importdata_import : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gblData.FillDropdown(ddlModule, "tbl_importdatamodule", "importdatamodule_modulename", "importdatamodule_importdatamoduleid", "", "importdatamodule_modulename");

        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
    }
    protected void ddlModule_Click(object sender, EventArgs e)
    {
        DataRow dr = DbTable.ExecuteSelectRow("SELECT COUNT(*) AS CNT FROM tbl_"+ddlModule.SelectedItem.Text.ToLower().Trim());
        if (dr != null)
        {
            ViewState["TotalExistingData"] = GlobalUtilities.ConvertToInt(dr["CNT"]);
            StringBuilder htmlDetal = new StringBuilder();
            htmlDetal.Append("<td>" + GlobalUtilities.ConvertToInt(dr["CNT"]) + "</td><td>0</td><td>0</td><td>0</td>");
            ltDetails.Text = htmlDetal.ToString();
        }
        
    }
   
    private void ImportData()
    {
        //if (!flExcel.HasFile)
        //{
        //    lblMessage.Text = "Please select an excel file";
        //    return;
        //}
        //string ext = Path.GetExtension(flExcel.FileName);
        //if (ext == ".xls" || ext == ".xlsx")
        //{
        //}
        //else
        //{
        //    lblMessage.Text = "Invalid file format";
        //    return;
        //}
        int TotalExistingData = GlobalUtilities.ConvertToInt(ViewState["TotalExistingData"]);
        int TotalExcelSheetData = 0;
        int TotalImportedData = 0;
        DataTable dttblTemplateColumns = getTemplateColumns(ddlModule.SelectedItem.Text.ToLower().Trim());
        string guid = Guid.NewGuid().ToString();
        string ext = ".xlsx";
        string newFile = Server.MapPath("~/temp/" + guid + ext);
        //flExcel.SaveAs(newFile);
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + newFile
                                                            + ";Extended Properties='Excel 12.0;HDR=YES'");
        if (conn.State == ConnectionState.Closed)
            conn.Open();
        System.Data.DataTable dtExcelSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        string sheetName = Convert.ToString(dtExcelSchema.Rows[0]["Table_Name"]);
        DataTable dttblData = Common.GetExcelSheetData(newFile, sheetName);
        if (!IsValidColumns(dttblTemplateColumns, dttblData))
        {
            lblMessage.Text = "There are some invalid columns in the excel sheet, please click on Download Template to get all the column names";
            return;
        }
        TotalExcelSheetData = dttblData.Rows.Count;

        string query = "";
        string m = ddlModule.SelectedItem.Text.ToLower().Replace(" ","");
        string tableName = "tbl_" + m;
        string idcolumn = m + "_" + m + "id";
        if (chkDeletePrevData.Checked)
        {
            query = "truncate table " + tableName;
            DbTable.ExecuteQuery(query);
        }
        StringBuilder columnNames = new StringBuilder();
        for (int j = 0; j < dttblData.Columns.Count; j++)
        {
            string columnName = dttblData.Columns[j].ColumnName;
            //Check If Details Table Column Exists
             Regex regex = new Regex(@"_\d+$");
             if (regex.IsMatch(columnName, columnName.IndexOf('_') + 1))
             {
                 continue;
             }
            //End
            if (!columnName.StartsWith(m + "_"))
            {
                columnName = m + "_" + columnName.Substring(0, columnName.IndexOf("_")) + "id";
            }
            if (columnNames.ToString() == "")
            {
                columnNames.Append(columnName);
            }
            else
            {
                columnNames.Append("," + columnName);
            }
        }
        query = @"select top 1 columns_columnname from tbl_columns  
                    join tbl_module on columns_moduleid=module_moduleid  
                    where module_moduleid='" + ddlModule.SelectedValue + "' and columns_isunique=1";
        //dttblData.Columns.Add("Import Status");
        StringBuilder html = new StringBuilder();
        html.Append("<table class='repeater'><tr class='repeater-header'><td>Row No</td><td>Data</td><td>Status</td></tr>");
        for (int i = 0; i < dttblData.Rows.Count; i++)
        {
            StringBuilder values = new StringBuilder();
            string error_master = "";
            string status = "";            
            bool isdataexists = false;            
            for (int j = 0; j < dttblData.Columns.Count; j++)
            {
                string value = global.CheckData(Convert.ToString(dttblData.Rows[i][j]).Trim());
                if (value != "")
                {
                    isdataexists = true;
                    break;
                }
            }
            if (isdataexists)
            {
                html.Append("<tr><td>" + Convert.ToString(i + 2) + "</td>");
                Hashtable[] arrhstbl = new Hashtable[dttblData.Columns.Count];
                for (int j = 0; j < dttblData.Columns.Count; j++)
                {
                    string columnName = dttblData.Columns[j].ColumnName;
                    string value = global.CheckData(Convert.ToString(dttblData.Rows[i][j]).Trim());
                    if (dttblData.Rows[i][j] == DBNull.Value) value = "";
                    if (j == 0)
                    {
                        html.Append("<td>" + value + "</td>");
                    }
                    if (!columnName.StartsWith(m + "_"))
                    {
                        //
                        Regex regex = new Regex(@"_\d+$");
                        if (regex.IsMatch(columnName, columnName.IndexOf('_') + 1))
                        {
                            int NoOfData_Subtable =GlobalUtilities.ConvertToInt(columnName.Substring(columnName.LastIndexOf('_') + 1, columnName.Length - columnName.LastIndexOf('_') - 1));
                            string subtableName = columnName.Substring(0, columnName.IndexOf('_'));
                            string key =regex.Replace(columnName,"");
                            if (arrhstbl[NoOfData_Subtable - 1] == null)
                            {
                                arrhstbl[NoOfData_Subtable - 1] = new Hashtable();
                            }
                            if (!arrhstbl[NoOfData_Subtable - 1].Contains(key))
                                arrhstbl[NoOfData_Subtable - 1].Add(key, value);

                            if (!arrhstbl[NoOfData_Subtable - 1].Contains("SubTableName"))
                                arrhstbl[NoOfData_Subtable - 1].Add("SubTableName", subtableName);

                            if (m.Contains(columnName))
                            {
                                if (!arrhstbl[NoOfData_Subtable - 1].Contains("SubTableName"))
                                    arrhstbl[NoOfData_Subtable - 1].Add("SubTableName", subtableName);
                            }                            
                            continue;
                        }
                        //


                        //add master data
                        if (value == "")
                        {
                            value = "0";
                        }
                        else
                        {
                            string subTable = "tbl_" + columnName.Substring(0, columnName.IndexOf("_"));

                            //add master Table data
                            value = AddSubTabelData(subTable, columnName, value, ref error_master).ToString();
                            //End 
                        }
                    }

                    if (values.ToString() == "")
                    {
                        values.Append("N'" + value + "'");
                    }
                    else
                    {
                        values.Append(",N'" + value + "'");
                    }
                }
                if (error_master == "")
                {
                    query = "insert into " + tableName + "(" + columnNames + ")values(" + values.ToString() + ")";
                    InsertUpdate obj = new InsertUpdate();
                    obj._throwError = false;
                    int _MId = obj.InsertData(query);
                    if (obj._error == "")
                    {
                        TotalImportedData++;
                        //dttblData.Rows[i]["Import Status"] = "<span>Success</span>";
                        status = "<span class='green'>Success</span>";
                    }
                    else
                    {
                        //dttblData.Rows[i]["Import Status"] = "<span class='error'>" + obj._error + "</span>";
                        status = "<span class='error'>" + obj._error + "</span>";
                    }

                    string strSubtableName = "";
                    //Add SubTable Data
                    for (int k = 0; k < arrhstbl.Length; k++)
                    {
                        Hashtable hstbl = (Hashtable)arrhstbl[k];
                        if (hstbl != null)
                        {
                            strSubtableName = (string)hstbl["SubTableName"];
                            foreach (string key in hstbl.Keys)
                            {
                                if (!key.StartsWith(strSubtableName + "_") && key != "SubTableName")
                                {
                                    string subTable = "tbl_" + key.Substring(0, key.IndexOf("_"));
                                    string strNewValue = AddSubTabelData(subTable, key, (string)hstbl[key], ref error_master).ToString();
                                    string newKey = strSubtableName + "_" + key.Substring(0, key.IndexOf("_")) + "id";
                                    if (hstbl.Contains(newKey))
                                    {
                                        hstbl.Remove(newKey);
                                    }
                                    hstbl.Add(newKey, strNewValue);
                                    hstbl.Remove(key);
                                    break;
                                }
                            }
                            hstbl.Remove("SubTableName");
                            if (hstbl.Contains(strSubtableName + "_" + m + "id"))
                            {
                                hstbl.Remove(strSubtableName + "_" + m + "id");
                            }
                            hstbl.Add(strSubtableName + "_" + m + "id", _MId);
                            InsertUpdate obj_1 = new InsertUpdate();
                            obj_1.InsertData(hstbl, "tbl_" + strSubtableName, false);
                        }

                        //End
                    }
                }
                html.Append("<td>" + status + "</td></tr>");
            }
        }
        ltImportStatus.Text = html.ToString();
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();
            conn.Dispose();
        }
        conn = null;
        //GlobalUtilities.DeleteFile(newFile);
        lblMessage.Text = "Excel Data Import Successfully!";
        StringBuilder htmlDetal = new StringBuilder();
        string strStyle = "style='font-size:20px; color:red'";
        htmlDetal.Append("<td " + strStyle + ">" + TotalExistingData + "</td><td " + strStyle + ">" + TotalExcelSheetData + "</td><td " + strStyle + ">" + TotalImportedData + "</td><td " + strStyle + ">" + (TotalExistingData + TotalImportedData) + "</td>");
        ltDetails.Text =htmlDetal.ToString();
    }
    private int AddSubTabelData(string subTable, string columnName, string value,ref string error)
    {
        int masterId = 0;
        if (!DbTable.IsDataExists("select * from " + subTable + " where " + columnName + "='" + value + "'"))
        {
           string query = "insert into " + subTable + "(" + columnName + ") values(N'" + value + "')";
            InsertUpdate objmaster = new InsertUpdate();
            masterId = objmaster.InsertData(query);
            if (masterId == 0)
            {
                error = objmaster._error;
                //dttblData.Rows[i]["Import Status"] = "<span class='error'>" + objmaster._error + "</span>";
                //status = "<span class='error'>" + objmaster._error + "</span>";
            }
            else
            {
                value = masterId.ToString();
            }
        }
        else
        {
            DataRow drMaster = DbTable.GetOneRow(subTable, columnName + "='" + value + "'");
            value = Convert.ToString(drMaster[0]);//get the id
            masterId = GlobalUtilities.ConvertToInt(value);
        }
        return masterId;
    }
    private bool IsValidColumns(DataTable dttblTemplate,DataTable dttblData)
    {
        for (int i = 0; i < dttblData.Columns.Count; i++)
        {
            bool isExists = false;
            if (dttblData.Columns[i].ColumnName.ToLower() != "Import Status")
            {
                for (int j = 0; j < dttblTemplate.Columns.Count; j++)
                {
                    if (dttblTemplate.Columns[j].ColumnName.ToLower() == dttblData.Columns[i].ColumnName.ToLower())
                    {
                        isExists = true;
                        break;
                    }
                }
                if (!isExists) return false;
            }
        }
        return true;
    }
    private void BindDataToDataColumns(DataTable dtData, DataTable dtColumn,bool IsSubTable)
    {
        for (int j = 0; j < dtData.Rows.Count; j++)
        {
            dtColumn.Columns.Add(GlobalUtilities.ConvertToString(dtData.Rows[j]["COLUMN_NAME"]) + Convert.ToString((IsSubTable) ? "_1" : ""));
        }
    }
    private void BindDataToDataColumns(DataTable dtData, DataTable dtColumn )
    {
        BindDataToDataColumns(dtData, dtColumn, false);
    }
    protected void btnDownloadTemplate_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (ddlModule.SelectedIndex == 0)
        {
            lblMessage.Text = "Please select the module";
            return;
        }
        string tableName = ddlModule.SelectedItem.Text.ToLower().Trim();
        DataTable dttbl = getTemplateColumns(tableName);
        GlobalUtilities.ExportToExcel(dttbl);

        StringBuilder html = new StringBuilder();
        html.Append("<table border=1><tr>");
        for (int i = 0; i < dttbl.Columns.Count; i++)
        {
            string column = Convert.ToString(dttbl.Columns[i].ColumnName);
            html.Append("<td>" + column + "</td>");
        }
        html.Append("</tr></table>");
        ltColumns.Text = html.ToString();

    }
    private bool IsSubTableExists(string MainTable,ref string SubTableName)
    {
        bool IsSubTableExists = false;
        DataRow dr = DbTable.ExecuteSelectRow(@"
        SELECT TOP 1 * FROM INFORMATION_SCHEMA.TABLES t1
                            JOIN INFORMATION_SCHEMA.COLUMNS t2 ON t1.TABLE_NAME = t2.TABLE_NAME
                            WHERE t1.TABLE_NAME LIKE '%detail%' AND t1.TABLE_NAME<>'"+MainTable+"' AND t2.COLUMN_NAME LIKE '%_" + MainTable + "id'");
        if (dr != null)
        {
            SubTableName = GlobalUtilities.ConvertToString(dr["TABLE_NAME"]);
            IsSubTableExists = true;
        }
        return IsSubTableExists;
    }
    private void BindTemplateColumns(ref DataTable dtColumnName, string tableName)
    {
    }
    private DataTable getTemplateColumns(string tableName, DataTable dtColumnName,bool IsSubTable)
    {
        string columnquery = @"SELECT * FROM INFORMATION_SCHEMA.COLUMNS
                            WHERE TABLE_NAME='tbl_" + tableName + "' AND ORDINAL_POSITION<>1 AND (COLUMN_NAME<>'" + tableName + "_createddate' AND COLUMN_NAME<>'" + tableName + "_createdby' AND COLUMN_NAME<>'" + tableName + "_modifieddate' AND COLUMN_NAME<>'" + tableName + "_modifiedby')";
        DataTable dt = DbTable.ExecuteSelect(columnquery);
        BindDataToDataColumns(dt, dtColumnName, IsSubTable);
        Array arrInnerTable = getFirstLevel_table_Data(dt, tableName);
        if (arrInnerTable.Length > 0)
        {            
            for (int i = 0; i < arrInnerTable.Length; i++)
            {
                string INnertablename = GlobalUtilities.ConvertToString(arrInnerTable.GetValue(i));
                if (IsSubTable)
                {
                    string MainModule = ddlModule.SelectedItem.Text.ToLower().Trim();
                    if (MainModule == INnertablename) continue;
                }
                
                if (INnertablename != tableName)
                {
                    string strquery = @"SELECT * FROM INFORMATION_SCHEMA.COLUMNS t1
                                JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE t2 ON t1.COLUMN_NAME = t2.COLUMN_NAME
                                WHERE t1.TABLE_NAME='tbl_" + INnertablename + "' AND t1.ORDINAL_POSITION <>1";
                    BindDataToDataColumns(DbTable.ExecuteSelect(strquery), dtColumnName, IsSubTable);
                    string strTemPcolumnName = tableName + "_" + INnertablename + "id";
                    if (dtColumnName.Columns.IndexOf(strTemPcolumnName) >= 0)
                        dtColumnName.Columns.Remove(strTemPcolumnName);
                }
            }
        }
        
        return dtColumnName;
    }
    private DataTable getTemplateColumns(string tableName)
    {
        DataTable dtColumnName = new DataTable();
        getTemplateColumns(tableName, dtColumnName,false);
            //Bind Sub Table Columns
        string strSubTableName = "";
        if (IsSubTableExists(tableName, ref strSubTableName))
        {
            DataTable dtSubTable = getTemplateColumns(strSubTableName.Replace("tbl_", ""), dtColumnName,true);
            BindDataToDataColumns(dtSubTable, dtColumnName,true);
            return dtColumnName;
        }
        //end
        return dtColumnName;
    }    
    private Array getFirstLevel_table_Data(DataTable dt, string tablename)
    {
        string strTable_Name = "";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string strColumnName = dt.Rows[i]["COLUMN_NAME"].ToString();
            string strInnerTableName = (strColumnName.IndexOf("id") > 0) ? strColumnName.Substring(strColumnName.IndexOf('_')+1, strColumnName.Length - strColumnName.IndexOf('_')-1).Replace("id", "") : "";
            if (strInnerTableName != "" && strInnerTableName!=tablename)
            {
                strTable_Name += (strTable_Name == "") ? strInnerTableName : "," + strInnerTableName;
            }            
        }
        return strTable_Name.Split(',');
    }    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (ddlModule.SelectedIndex == 0)
        {
            lblMessage.Text = "Please select the module";
            return;
        }
        ImportData();
    }
}
