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

public partial class importdata_importtelecall : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lnkDownloadTemplate.NavigateUrl = "~/upload/exceltemplate/telecall.xls";
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
        string fileName = Server.MapPath("~/upload/temp/" + Guid.NewGuid() + ".xls");
        flExcel.SaveAs(fileName);

        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName
                                                            + ";Extended Properties='Excel 12.0;HDR=YES'");
        if (conn.State == ConnectionState.Closed)
            conn.Open();
        System.Data.DataTable dtExcelSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        string sheetName = Convert.ToString(dtExcelSchema.Rows[0]["Table_Name"]);
        DataTable dttblData = Common.GetExcelSheetData(fileName, sheetName);
        DataTable dttblInvalidData = new DataTable();
        dttblInvalidData.Columns.Add("Customer Name");
        dttblInvalidData.Columns.Add("Contact Person");
        dttblInvalidData.Columns.Add("Mobile No");
        dttblInvalidData.Columns.Add("Landline No");
        dttblInvalidData.Columns.Add("Error Remarks");
        string errorRemarks = "";
        int totalImported = 0;
        for (int i = 0; i < dttblData.Rows.Count; i++)
        {
            Hashtable hstbl = new Hashtable();
            int cityId = 0;
            int areaId = 0;
            string mobileno = GlobalUtilities.ConvertToString(dttblData.Rows[i]["telecall_mobileno"]);
            string landlineno = GlobalUtilities.ConvertToString(dttblData.Rows[i]["telecall_landlineno"]);
            string customerName = GlobalUtilities.ConvertToString(dttblData.Rows[i]["telecall_customername"]);
            string contactPerson = GlobalUtilities.ConvertToString(dttblData.Rows[i]["telecall_contactperson"]);
            bool isvalid = true;
            int oldTelecallId = 0;
            bool alreadyExists = false;

            if (mobileno == "" && landlineno == "")
            {
                isvalid = false;
                errorRemarks = "Mobile No/Landline No required";
            }
            if (isvalid)
            {
                
                alreadyExists = IsAlreadyExists(mobileno, landlineno, ref oldTelecallId);
                if (alreadyExists)
                {
                    if (chkIsUpdate.Checked == false)
                    {
                        errorRemarks = "Already exists";
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
                        else if (columnName == "area_areaname")
                        {
                            areaId = SaveArea(cityId, data);
                            hstbl.Add("areaid", areaId);
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
                hstbl.Add("telecallstatusid", 5);//not assigned
                int telecallId = 0;
                if (alreadyExists)
                {
                    telecallId = obj.UpdateData(hstbl, "tbl_telecall", oldTelecallId);
                }
                else
                {
                    telecallId = obj.InsertData(hstbl, "tbl_telecall");
                }
                if (telecallId > 0)
                {
                    totalImported++;
                }
            }
            else
            {
                DataRow drInvalid = dttblInvalidData.NewRow();
                drInvalid["Customer Name"] = customerName;
                drInvalid["Contact Person"] = contactPerson;
                drInvalid["Mobile No"] = mobileno;
                drInvalid["Landline No"] = landlineno;
                drInvalid["Error Remarks"] = errorRemarks;
                dttblInvalidData.Rows.Add(drInvalid);
            }
        }
        //try
        //{
        //    File.Delete(fileName);
        //}
        //catch { }
        lblMessage.Text = "Data uploaded successfully!";
        trResult.Visible = true;
        lblTotalData.Text = dttblData.Rows.Count.ToString();
        lblInvaliddatas.Text = dttblInvalidData.Rows.Count.ToString();
        lblValiddatas.Text = Convert.ToString(dttblData.Rows.Count - dttblInvalidData.Rows.Count);
        lblDataImported.Text = totalImported.ToString();

        ltResult.Text = Common.BindGrid(dttblInvalidData, "Customer Name,Contact Person,Mobile No,Landline No,Error Remarks", "Customer Name,Contact Person,Mobile No,Landline No,Error Remarks");
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
    private int SaveArea(int cityId, string areaName)
    {
        if (areaName.Trim() == "") return 0;
        string query = "select * from tbl_area WHERE area_areaname='" + areaName + "'";
        DataRow drarea = DbTable.ExecuteSelectRow(query);
        Hashtable hstbl = new Hashtable();
        int areaId = 0;
        if (drarea == null)
        {
            hstbl.Add("cityid", cityId);
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
        Response.Redirect("~/importdata/import-telecall.aspx");
    }
    private bool IsAlreadyExists(string mobileNo, string landlineNo, ref int telecallId)
    {
        telecallId = 0;
        string query = "select * from tbl_telecall WHERE telecall_mobileno='" + mobileNo + "' OR telecall_landlineno='" + landlineNo + "'";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            return false;
        }
        else
        {
            telecallId = GlobalUtilities.ConvertToInt(dr["telecall_telecallid"]);
            return true;
        }
    }
}
