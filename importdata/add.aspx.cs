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
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.IO;

public partial class ImportData_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_importdata", "importdataid");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (GlobalUtilities.ConvertToBool(Request.QueryString["isdownloadtemplate"]))
            {
                Response.Redirect("add.aspx");
            }
            CommonPage.DisableOnQuickAddEdit(btnSubmitAndView);
            //FillDropDown_START

            gblData.FillDropdown(ddlimportdatamoduleid, "tbl_importdatamodule", "importdatamodule_modulename", "importdatamodule_importdatamoduleid", "", "importdatamodule_modulename");
            //FillDropDown_END
            if (Request.QueryString["id"] == null)
            {
                //PopulateAcOnAdd_START

                //PopulateAcOnAdd_END
                //PopulateOnAdd_START
                //PopulateOnAdd_END
                //SetDefault_START//SetDefault_END
                int ModuleId = Common.GetQueryStringValue("mid");
                if (ModuleId > 0)
                {
                    trModule.Visible = false;
                    DataRow dr = DbTable.ExecuteSelectRow(" SELECT * FROM tbl_module WHERE module_moduleid=" + ModuleId);
                    if (dr != null)
                    {
                        ViewState["ModuleName"] = GlobalUtilities.ConvertToString(dr["module_modulename"]).ToLower().Trim().Replace(" ", "");
                        ViewState["ModuleId"] = ModuleId;

                        lblPageTitle.Text = "Import " + GlobalUtilities.ConvertToString(dr["module_modulename"]);
                    }
                }
            }
            else
            {
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
            //lblPageTitle.Text = "Add Import Data";
        }
        else
        {
            lblPageTitle.Text = "Edit Import Data";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
        //PageTitle_START
    }
    //PopulateSubGrid_START

    //PopulateSubGrid_END
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData(true);
    }
    protected void btnSaveAndView_Click(object sender, EventArgs e)
    {
        if (SaveData(false) > 0)
		{
        	Response.Redirect("~/ImportData/view.aspx");
		}
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        
        lblMessage.Text = "";
        if (ddlimportdatamoduleid.SelectedIndex == 0 && GlobalUtilities.ConvertToString(ViewState["ModuleName"]) == "")
        {
            lblMessage.Text = "Please select the module";
            return 0;
        }

        //SetCode_START//SetCode_END
        //ExtraValues_START//ExtraValues_END
        int id = 0;
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            //SaveSubTable_START

            //SaveSubTable_END
            //SaveFile_START
            mfuuploadexcelfilexlsxxls.Save(id);
            //SaveFile_END
            //ParentCountUpdate_START

            //ParentCountUpdate_END
            ImportData(id);

            //lblMessage.Text = "Data saved successfully!";
            //lblMessage.Visible = true;
            //CommonPage.CloseQuickAddEditWindow(Page, form, id);
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
    //EnableControlsOnEdit_START
    //EnableControlsOnEdit_END
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
    private bool isValidDate_dd_mm_yyy_fromat(string value)
    {
        string strVal = value;
        return isValidDate_dd_mm_yyy_fromat(ref strVal);
    }
    private bool isValidDate_dd_mm_yyy_fromat(ref string value)
    {
        bool isValidDateFormat = false;
        try
        {
            Array arrDate = (value.Contains("-")) ? value.Split('-') : value.Split('/');
            if (arrDate.Length == 3)
            {
                int day = GlobalUtilities.ConvertToInt(arrDate.GetValue(0));
                object month = (GlobalUtilities.IsInteger(arrDate.GetValue(1).ToString())) ? (object)GlobalUtilities.ConvertToInt(arrDate.GetValue(1)) : GlobalUtilities.ConvertToString(arrDate.GetValue(1));
                Array ar = GlobalUtilities.ConvertToString(arrDate.GetValue(2)).Split(' ');
                int year = GlobalUtilities.ConvertToInt(ar.GetValue(0));
                year = (year.ToString().Length > 2) ? year : 2000 + year;
                if (31 >= day && day > 0)
                {
                    //if(12>=month||GlobalUtilities.()

                    if (year != 0)
                    {
                        value = year + "-" + month + "-" + day;
                        isValidDateFormat = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }


        return isValidDateFormat;
    }
    private bool IsValidData(string strTableName, string strColumnName, ref string strValue, out string Error)
    {
        string ModuleName = (strTableName.Contains("tbl_")) ? strTableName.Replace("tbl_", "") : strTableName;
        strTableName = (strTableName.Contains("tbl_")) ? strTableName : "tbl_" + strTableName;
        StringBuilder errList = new StringBuilder();
        DataRow drColumn = DbTable.ExecuteSelectRow("SELECT * FROM tbl_columns WHERE columns_columnname='" + strColumnName + "'");
        if (drColumn != null)
        {
            string ControlName = GlobalUtilities.ConvertToString(drColumn["columns_control"]);
            bool isRequired = GlobalUtilities.ConvertToBool(drColumn["columns_isrequired"]);
            bool isUnique = GlobalUtilities.ConvertToBool(drColumn["columns_isunique"]);
            string defaultvalue = GlobalUtilities.ConvertToString(drColumn["columns_defaultvalue"]);
            string codeformat = GlobalUtilities.ConvertToString(drColumn["columns_codeformat"]);
            if (codeformat != "")
            {
                strValue = UniqueCode.GetUniqueCode(strTableName, strColumnName, codeformat);
            }
            if (defaultvalue != "")
            {
                if (defaultvalue == "")
                {
                    strValue = defaultvalue;
                    Error = "";
                    return true;
                }
            }
            if (strColumnName.ToLower().Contains("amount") || strColumnName.ToLower().Contains("rate"))
            {
                if (strValue == "") strValue = "0";
            }
            //if(val!="")
            if (ControlName == "Amount")
            {
                if (strValue == "") strValue = "0";
                strValue = strValue.ToLower().Replace(",", "").Replace("rs", "").Replace("amount", "");
                strValue = (strValue.Trim() == "") ? strValue = "0" : strValue;
                if (!GlobalUtilities.IsNumeric(strValue)) errList.Append(" Invalid Data For " + strColumnName);

            }
            if (ControlName == "Number")
            {
                strValue = (strValue.Trim() == "") ? strValue = "0" : strValue;
                if (!GlobalUtilities.IsInteger(strValue)) errList.Append(" Invalid Data For " + strColumnName);
            }
            if (ControlName == "Date" && strValue != "")
            {
                if (!isValidDate_dd_mm_yyy_fromat(ref strValue)) errList.Append(" Invalid Date Format For " + strColumnName + " Date Format Should be this (dd-mm-yyyy) OR (dd-MM-yyyy) format");
            }
            /*if (ControlName == "Email Id")
            {
                if (!Common.IsValidEmail(strValue)) errList.Append(" Invalid Email Format for "+strColumnName+" [ " + strValue + " ]");
            }*/
            if (ControlName == "Checkbox" && strValue != "")
            {
                bool val;
                if (!GlobalUtilities.IsInteger(strValue))
                {
                    try
                    {
                        strValue = (Convert.ToBoolean(strValue)) ? "1" : "0";

                    }
                    catch (Exception ex)
                    {
                        errList.Append(" , Invalid Data " + strValue);
                    }
                }
            }
        }


        Error = errList.ToString();
        if (Error != "") return true;
        return false;
    }

    private void ImportData(int Id)
    {
        int TotalExistingData = 0;
        DataRow dr = DbTable.ExecuteSelectRow("SELECT COUNT(*) AS CNT FROM tbl_" + GlobalUtilities.ConvertToString(ViewState["ModuleName"]));
        if (dr != null)
        {
            TotalExistingData = GlobalUtilities.ConvertToInt(dr["CNT"]);
            ViewState["TotalExistingData"] = TotalExistingData;

        }
        int TotalExcelSheetData = 0;
        int TotalImportedData = 0;
        DataTable dttblTemplateColumns = getTemplateColumns(GlobalUtilities.ConvertToString(ViewState["ModuleName"]));
        string guid = Guid.NewGuid().ToString();
        string newFile = Server.MapPath("~/upload/importdata/" + Id + ".xlsx");
        if (!File.Exists(newFile))
        {
            lblMessage.Text = "Please Upload File a .xlsx";
            lblMessage.Visible = true;
            return;
        }
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
            lblMessage.Visible = true;
            return;
        }
        TotalExcelSheetData = dttblData.Rows.Count;

        string query = "";
        string m = GlobalUtilities.ConvertToString(ViewState["ModuleName"]);
        string tableName = "tbl_" + m;
        string idcolumn = m + "_" + m + "id";
        //if (chkDeletePrevData.Checked)
        //{
        //    query = "truncate table " + tableName;
        //    DbTable.ExecuteQuery(query);
        //}
        StringBuilder columnNames = new StringBuilder();
        StringBuilder htmlSubTableError = new StringBuilder();
        for (int j = 0; j < dttblData.Columns.Count; j++)
        {
            string columnName = dttblData.Columns[j].ColumnName;
            if (!columnName.Contains("_")) continue;

            //Check If Details Table Column Exists
            Regex regex = new Regex(@"_\d+$");
            if (regex.IsMatch(columnName, columnName.IndexOf('_') + 1))
            {
                continue;
            }
            //End
            if (!columnName.StartsWith(m + "_"))
            {
                try
                {

                    columnName = m + "_" + columnName.Substring(0, columnName.IndexOf("_")) + "id";
                }
                catch (Exception ex)
                {
                }
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
                    where module_moduleid='" + GlobalUtilities.ConvertToInt(ViewState["ModuleId"]) + "' and columns_isunique=1";
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
                    if (!columnName.Contains("_")) continue;
                    string value = Convert.ToString(dttblData.Rows[i][j]).Trim();
                    if (columnName.Contains("amount"))
                    {
                        if (value == "") value = "0";
                    }
                    string _error = "";
                    if (GlobalUtilities.ConvertToString(dttblData.Columns[j].ColumnName).IndexOf('_') > 0)
                    {
                        string _mName = GlobalUtilities.ConvertToString(dttblData.Columns[j].ColumnName).Substring(0, GlobalUtilities.ConvertToString(dttblData.Columns[j].ColumnName).IndexOf('_'));
                        IsValidData(_mName, GlobalUtilities.ConvertToString(dttblData.Columns[j].ColumnName), ref value, out _error);
                    }
                    if (_error != "") error_master += (error_master == "") ? _error : "," + _error;
                    if (value == "")
                        if (dttblData.Rows[i][j] == DBNull.Value) value = "";
                    value = global.CheckData(value);
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
                            int NoOfData_Subtable = GlobalUtilities.ConvertToInt(columnName.Substring(columnName.LastIndexOf('_') + 1, columnName.Length - columnName.LastIndexOf('_') - 1));
                            string subtableName = columnName.Substring(0, columnName.IndexOf('_'));
                            string key = regex.Replace(columnName, "");
                            string error_subtable = "";
                            IsValidData(subtableName, key.Replace(subtableName, tableName).Replace("tbl_", ""), ref value, out error_subtable);
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
                            if (error_subtable != "")
                                htmlSubTableError.Append((htmlSubTableError.ToString() == "") ? error_subtable : "," + error_subtable);
                            continue;

                        }
                        //
                        //error_master+=htmlSubTableError.ToString();

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
                if (htmlSubTableError.ToString() != "") error_master += htmlSubTableError.ToString();
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
                        string str_err = obj._error;
                        if (obj._error.StartsWith("Violation of UNIQUE KEY constraint"))
                        {
                            str_err = "Data Already Exists";
                            string strUniqueColumnName = obj._error.Substring(obj._error.IndexOf('\'') + 1, obj._error.IndexOf('.') - obj._error.IndexOf('\'') + 1 - 3);
                            if (strUniqueColumnName.Contains("unique_"))
                            {
                                strUniqueColumnName = strUniqueColumnName.Replace("unique_", "");
                                string strUniqueKeyValue = obj._error.Substring(obj._error.IndexOf('(') + 1, obj._error.IndexOf(')') - obj._error.IndexOf('(') - 1);
                                DataRow drrr = DbTable.ExecuteSelectRow("SELECT * FROM " + tableName + " WHERE " + strUniqueColumnName + "= '" + strUniqueKeyValue + "'");
                                if (drrr != null)
                                {
                                    _MId = GlobalUtilities.ConvertToInt(drrr[m + "_" + m + "id"]);
                                }
                            }
                        }
                        //dttblData.Rows[i]["Import Status"] = "<span class='error'>" + obj._error + "</span>";
                        status = "<span class='error'>" + str_err + "</span>";
                    }

                    string strSubtableName = "";
                    //Add SubTable Data
                    int _TotalAmount = 0;
                    int _TotalQuantity = 0;

                    if (_MId > 0)
                    {
                        for (int k = 0; k < arrhstbl.Length; k++)
                        {
                            string err;
                            Hashtable hstbl = (Hashtable)arrhstbl[k];
                            if (hstbl != null)
                            {
                                //check whether data is empty
                                bool isValidSubTableData = false;
                                foreach (string key in hstbl.Keys)
                                {
                                    string val = Convert.ToString(hstbl[key]).Trim();
                                    strSubtableName = (string)hstbl["SubTableName"];
                                    IsValidData(m, key.Replace(strSubtableName, m), ref val, out err);
                                    if (key == "SubTableName") continue;
                                    if (key == strSubtableName + "_" + m + "id")
                                    {
                                        if (val == "-1")
                                        {
                                            isValidSubTableData = false;
                                            break;
                                        }
                                        continue;
                                    }
                                    if (val == "" || val == "0")
                                    {
                                    }
                                    else
                                    {
                                        isValidSubTableData = true;
                                        break;
                                    }

                                }
                                strSubtableName = "";
                                if (!isValidSubTableData) continue;
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
                                if (isValidSubTableData)
                                {
                                    InsertUpdate obj_1 = new InsertUpdate();
                                    obj_1.InsertData(hstbl, "tbl_" + strSubtableName, false);
                                }
                            }

                            //End
                        }
                        //Update Main Table Total Amount
                        try
                        {
                            DbTable.ExecuteQuery("UPDATE tbl_" + tableName + " SET " + tableName + "_totalamount=" + _TotalAmount + " WHERE " + m + "_" + m + "id=" + _MId);
                        }
                        catch (Exception ex) { }
                        //End
                    }

                }
                else
                {
                    lblMessage.Text = error_master;
                    lblMessage.Visible = true;
                    html.Append("<td class='error'>" + error_master + "</td></tr>");
                    ltImportStatus.Text = html.ToString();
                    return;
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
        ltDetails.Text = htmlDetal.ToString();
    }
    private bool IsDataAlreadyExists(string strColumnName, string tablename, string value)
    {
        bool IsDataExists = false;
        DataRow drColumn = DbTable.ExecuteSelectRow("SELECT * FROM tbl_columns WHERE columns_columnname='" + strColumnName + "'");
        if (drColumn != null)
        {
            string ControlName = GlobalUtilities.ConvertToString(drColumn["columns_control"]);
            bool isRequired = GlobalUtilities.ConvertToBool(drColumn["columns_isrequired"]);
            bool isUnique = GlobalUtilities.ConvertToBool(drColumn["columns_isunique"]);
            if (isUnique)
            {
                DataRow dr = DbTable.ExecuteSelectRow(" SELECT * FROM " + tablename + " WHERE " + strColumnName + "='" + value + "'");
                if (dr != null) IsDataExists = true;
            }
        }
        return IsDataExists;
    }
    private int AddSubTabelData(string subTable, string columnName, string value, ref string error)
    {
        if (value.Trim() == "") return 0;
        int masterId = 0;
        if (!DbTable.IsDataExists("select * from " + subTable + " where " + columnName + "='" + value + "'"))
        {
            string query = "insert into " + subTable + "(" + columnName + ") values(N'" + value + "')";
            InsertUpdate objmaster = new InsertUpdate();
            masterId = objmaster.InsertData(query);
            if (masterId == 0)
            {
                error = objmaster._error;
                if (error != "")
                {
                    Response.Write(error);
                    Response.End();
                }
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
    private bool IsValidColumns(DataTable dttblTemplate, DataTable dttblData)
    {
        for (int i = 0; i < dttblData.Columns.Count; i++)
        {
            bool isExists = false;
            string strDatacolumn = GlobalUtilities.ConvertToString(dttblData.Columns[i].ColumnName.ToLower());
            if (dttblData.Columns[i].ColumnName.ToLower() != "Import Status")
            {
                for (int j = 0; j < dttblTemplate.Columns.Count; j++)
                {
                    Regex regex = new Regex(@"_\d+$");
                    string columnName = dttblTemplate.Columns[j].ColumnName.ToLower();
                    if (regex.IsMatch(columnName, columnName.IndexOf('_') + 1))
                    {
                        isExists = true;
                        break;
                    }
                    if (dttblTemplate.Columns[j].ColumnName.ToLower() == dttblData.Columns[i].ColumnName.ToLower())
                    {
                        isExists = true;
                        break;
                    }
                    else
                    {
                        DataRow dr = DbTable.ExecuteSelectRow("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='tbl_" + GlobalUtilities.ConvertToString(ViewState["ModuleName"]) + "' AND COLUMN_NAME='" + strDatacolumn + "'");
                        if (dr != null)
                        {
                            isExists = true;
                            break;
                        }
                    }
                }
                if (!isExists) return false;
            }
        }
        return true;
    }
    private void BindDataToDataColumns(DataTable dtData, DataTable dtColumn, bool IsSubTable)
    {
        for (int j = 0; j < dtData.Rows.Count; j++)
        {
            string strColumnName = GlobalUtilities.ConvertToString(dtData.Rows[j]["COLUMN_NAME"]);
            bool isColunExists = false;
            for (int i = 0; i < dtColumn.Columns.Count; i++)
            {
                if (strColumnName == GlobalUtilities.ConvertToString(dtColumn.Columns[i]))
                {
                    isColunExists = true;
                    break;
                }
            }
            if (!isColunExists)
            {
                dtColumn.Columns.Add(strColumnName + Convert.ToString((IsSubTable) ? "_1" : ""));
            }
        }
    }
    private void BindDataToDataColumns(DataTable dtData, DataTable dtColumn)
    {
        BindDataToDataColumns(dtData, dtColumn, false);
    }
    protected void btndownloadtemplate_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (ddlimportdatamoduleid.SelectedIndex == 0 && GlobalUtilities.ConvertToString(ViewState["ModuleName"]) == "")
        {
            lblMessage.Text = "Please select the module";
            lblMessage.Visible = true;
            return;
        }
        string tableName = GlobalUtilities.ConvertToString(ViewState["ModuleName"]);
        if (tableName == "enquiry")
        {
            Common.Redirect("~/importdata/template/enquiry.xlsx");
            return;
        }
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
        Response.Redirect("add.aspx");

    }
    private bool IsSubTableExists(string MainTable, ref string SubTableName)
    {
        bool IsSubTableExists = false;
        DataRow dr = DbTable.ExecuteSelectRow(@"
        SELECT TOP 1 * FROM INFORMATION_SCHEMA.TABLES t1
                            JOIN INFORMATION_SCHEMA.COLUMNS t2 ON t1.TABLE_NAME = t2.TABLE_NAME
                            WHERE t1.TABLE_NAME LIKE '%detail%' AND t1.TABLE_NAME<>'" + MainTable + "' AND t2.COLUMN_NAME LIKE '%_" + MainTable + "id'");
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
    private DataTable getTemplateColumns(string tableName, DataTable dtColumnName, bool IsSubTable)
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
                    string MainModule = GlobalUtilities.ConvertToString(ViewState["ModuleName"]);
                    if (MainModule == INnertablename)
                    {
                        //Remove Inner table Id Coumns
                        string strInnerTableIdColumns = tableName + "_" + INnertablename + "id";//Inner Table Id Columns
                        strInnerTableIdColumns = strInnerTableIdColumns + "_1";
                        ReomveColumnsIsExists(dtColumnName, strInnerTableIdColumns);
                        //If Main Table Contains Total Amunt we have to remove ,this will automatically Added
                        ReomveColumnsIsExists(dtColumnName, INnertablename + "_totalamount");//Exclude TotalAmount Columns 
                        continue;
                    }
                }

                if (INnertablename != tableName)
                {
                    string strquery = @"SELECT * FROM INFORMATION_SCHEMA.COLUMNS t1
                                JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE t2 ON t1.COLUMN_NAME = t2.COLUMN_NAME
                                WHERE t1.TABLE_NAME='tbl_" + INnertablename + "' AND t1.ORDINAL_POSITION <>1";
                    BindDataToDataColumns(DbTable.ExecuteSelect(strquery), dtColumnName, IsSubTable);
                    string strInnerTableIdColumns = tableName + "_" + INnertablename + "id";//Inner Table Id Columns
                    //Remove Inner table Id Coumns
                    if (IsSubTable) strInnerTableIdColumns = strInnerTableIdColumns + "_1";
                    ReomveColumnsIsExists(dtColumnName, strInnerTableIdColumns);
                    ReomveColumnsIsExists(dtColumnName, tableName + "_istax_1");//Exclude Tax Columns                     
                }
            }
        }

        return dtColumnName;
    }
    private void ReomveColumnsIsExists(DataTable dtColumn, string strColumnName)
    {
        if (strColumnName.Trim().ToLower().Contains("emailid")) return;
        for (int i = 0; i < dtColumn.Columns.Count; i++)
        {
            if (GlobalUtilities.ConvertToString(dtColumn.Columns[i]).ToLower().Trim() == strColumnName.ToLower().Trim())
            {
                dtColumn.Columns.RemoveAt(i);
                i--;
            }
        }
    }
    private DataTable getTemplateColumns(string tableName)
    {
        DataTable dtColumnName = new DataTable();
        getTemplateColumns(tableName, dtColumnName, false);
        //Bind Sub Table Columns
        string strSubTableName = "";
        if (IsSubTableExists(tableName, ref strSubTableName))
        {
            DataTable dtSubTable = getTemplateColumns(strSubTableName.Replace("tbl_", ""), dtColumnName, true);
            BindDataToDataColumns(dtSubTable, dtColumnName, true);
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
            string strInnerTableName = (strColumnName.IndexOf("id") > 0) ? strColumnName.Substring(strColumnName.IndexOf('_') + 1, strColumnName.Length - strColumnName.IndexOf('_') - 1).Replace("id", "") : "";
            if (strInnerTableName != "" && strInnerTableName != tablename)
            {
                strTable_Name += (strTable_Name == "") ? strInnerTableName : "," + strInnerTableName;
            }
        }
        return strTable_Name.Split(',');

    }
}


