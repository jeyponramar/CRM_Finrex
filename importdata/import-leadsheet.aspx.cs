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

public partial class importdata_importleadsheet : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lnkDownloadTemplate.NavigateUrl = "~/upload/exceltemplate/leadsheet.xlsx";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
    }
    protected void btnImportData_Click(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
        if (!flExcel.HasFile)
        {
            lblMessage.Text = "Please select excel file";
            lblMessage.Visible = true;
            return;
        }
        string fileName = Server.MapPath("~/upload/temp/" + Guid.NewGuid() + ".xlsx");
        flExcel.SaveAs(fileName);
        DataTable dttblData = new DataTable();
        using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName
                                                            + ";Extended Properties='Excel 12.0;HDR=YES'"))
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            System.Data.DataTable dtExcelSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string sheetName = Convert.ToString(dtExcelSchema.Rows[0]["Table_Name"]);
            dttblData = Common.GetExcelSheetData(fileName, sheetName);
            conn.Close();
        }
        DataTable dttblInvalidData = new DataTable();
        dttblInvalidData.Columns.Add("Date");
        dttblInvalidData.Columns.Add("Company Name");
        dttblInvalidData.Columns.Add("MD Name");
        dttblInvalidData.Columns.Add("MD Mobile");
        dttblInvalidData.Columns.Add("MD Email");
        dttblInvalidData.Columns.Add("Error Remarks");
        string errorRemarks = "";
        int totalImported = 0;
        int loggedInEmployeeId = Common.EmployeeId;
        for (int i = 0; i < dttblData.Rows.Count; i++)
        {
            Hashtable hstbl = new Hashtable();
            int cityId = 0;
            string date = GlobalUtilities.ConvertToString(dttblData.Rows[i]["leadsheet_date"]);
            string mdmobileno = GlobalUtilities.ConvertToString(dttblData.Rows[i]["leadsheet_mdmobile"]);
            string mdemailid = GlobalUtilities.ConvertToString(dttblData.Rows[i]["leadsheet_mdemail"]);
            string companyName = GlobalUtilities.ConvertToString(dttblData.Rows[i]["leadsheet_companyname"]);
            string mdname = GlobalUtilities.ConvertToString(dttblData.Rows[i]["leadsheet_mdname"]);
            string exportimport = GlobalUtilities.ConvertToString(dttblData.Rows[i]["business_business"]);
            string currency = GlobalUtilities.ConvertToString(dttblData.Rows[i]["currency_currency"]);
            string empName = GlobalUtilities.ConvertToString(dttblData.Rows[i]["employee_employeename"]);
            string meetingdone = GlobalUtilities.ConvertToString(dttblData.Rows[i]["leadsheet_ismeetingdone"]).ToLower().Trim();
            string whatsappseen = GlobalUtilities.ConvertToString(dttblData.Rows[i]["leadsheet_iswhatsappseen"]).ToLower().Trim();
            bool isvalid = true;
            int oldleadsheetId = 0;
            int currencyId = 0;
            int businessId = 0;
            int employeeId = 0;
            bool alreadyExists = false;

            if (date == "")
            {
                isvalid = false;
                errorRemarks = "Date is required";
            }
            if (companyName == "")
            {
                isvalid = false;
                errorRemarks = "Company Name is required";
            }
            if (exportimport != "")
            {
                string query = "select * from tbl_business where business_business='" + exportimport + "'";
                DataRow dr1 = DbTable.ExecuteSelectRow(query);
                if (dr1 == null)
                {
                    isvalid = false;
                    errorRemarks = "Invalid business, it should be like Import, Export, Both, Domestic etc";
                }
                else
                {
                    businessId = GlobalUtilities.ConvertToInt(dr1["business_businessid"]);
                }
            }
            if (empName == "")
            {
                employeeId = loggedInEmployeeId;
            }
            else
            {
                string query = "select * from tbl_employee where employee_employeename='" + empName + "'";
                DataRow dr1 = DbTable.ExecuteSelectRow(query);
                if (dr1 == null)
                {
                    isvalid = false;
                    errorRemarks = "Invalid Employee";
                }
                else
                {
                    employeeId = GlobalUtilities.ConvertToInt(dr1["employee_employeeid"]);
                }
            }
            if (currency != "")
            {
                string query = "select * from tbl_currency where currency_currency='" + currency + "'";
                DataRow dr1 = DbTable.ExecuteSelectRow(query);
                if (dr1 == null)
                {
                    isvalid = false;
                    errorRemarks = "Invalid currency";
                }
                else
                {
                    currencyId = GlobalUtilities.ConvertToInt(dr1["currency_currencyid"]);
                }
            }
            if (meetingdone != "")
            {
                if (meetingdone == "yes" || meetingdone == "no")
                {
                }
                else
                {
                    isvalid = false;
                    errorRemarks = "Invalid Is Meeting Done";
                }
            }
            if (whatsappseen != "")
            {
                if (whatsappseen == "yes" || whatsappseen == "no")
                {
                }
                else
                {
                    isvalid = false;
                    errorRemarks = "Invalid Is Whatsapp Seen";
                }
            }
            if (isvalid)
            {
                alreadyExists = IsAlreadyExists(companyName, mdmobileno, mdemailid, ref oldleadsheetId);
                if (alreadyExists)
                {
                    if (chkIsUpdate.Checked == false)
                    {
                        errorRemarks = "Lead Sheet Already Exists";
                        isvalid = false;
                    }
                }
                if (isvalid)
                {
                    for (int j = 0; j < dttblData.Columns.Count; j++)
                    {
                        string data = GlobalUtilities.ConvertToString(dttblData.Rows[i][j]);
                        string columnName = dttblData.Columns[j].ColumnName.ToLower();
                        if (columnName == "city_cityname")
                        {
                            cityId = SaveCity(data);
                            hstbl.Add("cityid", cityId);
                        }
                        else if (columnName == "leadsheet_date")
                        {
                            string date1 = data.Replace(" 12:00:00 AM", "");
                            hstbl.Add("date", date1);
                        }
                        else if (columnName == "business_business")
                        {
                            hstbl.Add("businessid", businessId);
                        }
                        else if (columnName == "currency_currency")
                        {
                            hstbl.Add("currencyid", currencyId);
                        }
                        else if (columnName == "employee_employeename")
                        {
                            hstbl.Add("employeeid", employeeId);
                        }
                        else if (columnName == "leadsheet_ismeetingdone")
                        {
                            if (meetingdone.Contains("y"))
                            {
                                hstbl.Add("ismeetingdone", 1);
                            }
                            else
                            {
                                hstbl.Add("ismeetingdone", 0);
                            }
                        }
                        else if (columnName == "leadsheet_iswhatsappseen")
                        {
                            if (whatsappseen.Contains("y"))
                            {
                                hstbl.Add("iswhatsappseen", 1);
                            }
                            else
                            {
                                hstbl.Add("iswhatsappseen", 0);
                            }
                        }
                        else
                        {
                            columnName = Common.GetColumnName(columnName);
                            hstbl.Add(columnName, data);
                        }
                    }
                }
            }
            if (isvalid)
            {
                InsertUpdate obj = new InsertUpdate();
                
                if (!alreadyExists)
                {
                    hstbl.Add("leadsheetstatusid", 1);//active
                }
                int leadsheetId = 0;
                if (alreadyExists)
                {
                    leadsheetId = obj.UpdateData(hstbl, "tbl_leadsheet", oldleadsheetId);
                }
                else
                {
                    leadsheetId = obj.InsertData(hstbl, "tbl_leadsheet");
                }
                if (leadsheetId > 0)
                {
                    totalImported++;
                }
            }
            else
            {
                DataRow drInvalid = dttblInvalidData.NewRow();
                drInvalid["Date"] = date;
                drInvalid["Company Name"] = companyName;
                drInvalid["MD Name"] = mdname;
                drInvalid["MD Mobile"] = mdmobileno;
                drInvalid["MD Email"] = mdemailid;
                drInvalid["Error Remarks"] = errorRemarks;
                dttblInvalidData.Rows.Add(drInvalid);
            }
        }
        try
        {
            File.Delete(fileName);
        }
        catch { }
        lblMessage.Text = "Data uploaded successfully!";
        trResult.Visible = true;
        lblTotalData.Text = dttblData.Rows.Count.ToString();
        lblInvaliddatas.Text = dttblInvalidData.Rows.Count.ToString();
        lblValiddatas.Text = Convert.ToString(dttblData.Rows.Count - dttblInvalidData.Rows.Count);
        lblDataImported.Text = totalImported.ToString();

        ltResult.Text = Common.BindGrid(dttblInvalidData, "Date,Company Name,MD Name,MD Mobile,MD Email,Error Remarks",
                                                          "Date,Company Name,MD Name,MD Mobile,MD Email,Error Remarks");
    }

    private int SaveCity(string cityName)
    {
        if (cityName.Trim() == "") return 0;
        string query = "select * from tbl_city WHERE city_cityname='" + cityName + "'";
        DataRow drCity = DbTable.ExecuteSelectRow(query);
        Hashtable hstbl = new Hashtable();
        int cityId = 0;
        if (drCity == null)
        {
            hstbl.Add("cityname", cityName);
            InsertUpdate obj = new InsertUpdate();
            cityId = obj.InsertData(hstbl, "tbl_city");
        }
        else
        {
            cityId = GlobalUtilities.ConvertToInt(drCity["city_cityid"]);
        }
        return cityId;
    }
    private int SaveState(string stateName)
    {
        if (stateName.Trim() == "") return 0;
        string query = "select * from tbl_state WHERE state_state='" + stateName + "'";
        DataRow drstate = DbTable.ExecuteSelectRow(query);
        Hashtable hstbl = new Hashtable();
        int stateId = 0;
        if (drstate == null)
        {
            hstbl.Add("state", stateName);
            InsertUpdate obj = new InsertUpdate();
            stateId = obj.InsertData(hstbl, "tbl_state");
        }
        else
        {
            stateId = GlobalUtilities.ConvertToInt(drstate["state_stateid"]);
        }
        return stateId;
    }
    private int SaveArea(int stateId, string areaName)
    {
        if (areaName.Trim() == "") return 0;
        string query = "select * from tbl_area WHERE area_areaname='" + areaName + "'";
        DataRow drarea = DbTable.ExecuteSelectRow(query);
        Hashtable hstbl = new Hashtable();
        int areaId = 0;
        if (drarea == null)
        {
            hstbl.Add("stateid", stateId);
            hstbl.Add("areaname", areaName);
            InsertUpdate obj = new InsertUpdate();
            areaId = obj.InsertData(hstbl, "tbl_area");
        }
        else
        {
            areaId = GlobalUtilities.ConvertToInt(drarea["area_areaid"]);
        }
        return areaId;
    }
    protected void btnDownloadTemplate_Click(object sender, EventArgs e)
    {
        string query = "select * from tbl_columns " +
                       "JOIN tbl_module ON module_moduleid=columns_moduleid " +
                       "WHERE module_modulename='Tele Call'";
        DataTable dttblColumns = DbTable.ExecuteSelect(query);
        DataTable dttblImportCols = new DataTable();
        for (int i = 0; i < dttblColumns.Rows.Count; i++)
        {
            string columnName = GlobalUtilities.ConvertToString(dttblColumns.Rows[i]["columns_columnname"]);
            string dropdownColumnName = GlobalUtilities.ConvertToString(dttblColumns.Rows[i]["columns_dropdowncolumn"]);
            if (dropdownColumnName.Trim() != "") columnName = dropdownColumnName;
            dttblImportCols.Columns.Add(columnName);
        }
        GlobalUtilities.ExportToExcel(dttblImportCols);
        Response.Redirect("~/importdata/import-leadsheet.aspx");
    }
    private bool IsAlreadyExists(string customerName, string mobileNo, string emailId, ref int leadsheetId)
    {
        leadsheetId = 0;
        string query = "select * from tbl_leadsheet WHERE leadsheet_leadsheetstatusid=1 AND (leadsheet_companyname='" + global.CheckInputData(customerName) + "' ";
        if (mobileNo != "")
        {
            query += " OR leadsheet_mdmobile='" + global.CheckInputData(mobileNo) + "'";
        }
        if (emailId != "")
        {
            query += " OR leadsheet_mdemail='" + global.CheckInputData(emailId) + "'";
        }
        query += ")";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            return false;
        }
        else
        {
            leadsheetId = GlobalUtilities.ConvertToInt(dr["leadsheet_leadsheetid"]);
            return true;
        }
    }
}
