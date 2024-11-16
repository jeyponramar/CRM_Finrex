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

public partial class importdata_importenquiry : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lnkDownloadTemplate.NavigateUrl = "~/upload/exceltemplate/enquiry.xlsx";
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
            int stateId = 0;
            string mobileno = GlobalUtilities.ConvertToString(dttblData.Rows[i]["enquiry_mobileno"]);
            string landlineno = GlobalUtilities.ConvertToString(dttblData.Rows[i]["enquiry_landlineno"]);
            string emailid = GlobalUtilities.ConvertToString(dttblData.Rows[i]["enquiry_emailid"]);
            string customerName = GlobalUtilities.ConvertToString(dttblData.Rows[i]["enquiry_companyname"]);
            string contactPerson = GlobalUtilities.ConvertToString(dttblData.Rows[i]["enquiry_contactperson"]);
            bool isvalid = true;
            int oldenquiryId = 0;
            bool alreadyExists = false;

            if (mobileno == "")
            {
                isvalid = false;
                errorRemarks = "Mobile No required";
            }
            if (emailid == "")
            {
                isvalid = false;
                errorRemarks = "Email Id required";
            }
            if (customerName == "")
            {
                isvalid = false;
                errorRemarks = "Customer Name required";
            }
            if (contactPerson == "")
            {
                isvalid = false;
                errorRemarks = "Contact Person required";
            }
            if (isvalid)
            {
                alreadyExists = IsAlreadyExists(customerName, mobileno, emailid, ref oldenquiryId);
                if (alreadyExists)
                {
                    if (chkIsUpdate.Checked == false)
                    {
                        errorRemarks = "Enquiry Already Exists";
                        isvalid = false;
                    }
                }
                if (isvalid)
                {
                    string website = "";
                    for (int j = 0; j < dttblData.Columns.Count; j++)
                    {
                        string data = GlobalUtilities.ConvertToString(dttblData.Rows[i][j]);
                        string columnName = dttblData.Columns[j].ColumnName.ToLower();
                        if (columnName == "state_statename")
                        {
                            stateId = SaveState(data);
                            hstbl.Add("stateid", stateId);
                        }
                        //else if (columnName == "city_cityname")
                        //{
                        //    cityId = SaveCity(data);
                        //    hstbl.Add("cityid", cityId);
                        //}
                        else if (columnName == "area_areaname")
                        {
                            areaId = SaveArea(stateId, data);
                            hstbl.Add("areaid", areaId);
                        }
                        else if (columnName == "area_website")
                        {
                        }
                        else
                        {
                            columnName = Common.GetColumnName(columnName);
                            hstbl.Add(columnName, data);
                        }
                    }
                    int designationId = 0;
                    int campaignId = 0;
                    int exposureid = 0;
                    int businessid = 0;
                    int industrytypesid = 0;
                    int clientId = Custom.SaveClient(customerName, emailid, contactPerson, landlineno, mobileno, website, designationId,
                                 campaignId, stateId, areaId,exposureid,businessid,industrytypesid);
                }
            }
            if (isvalid)
            {
                InsertUpdate obj = new InsertUpdate();
                
                if (!alreadyExists)
                {
                    hstbl.Add("enquirystatusid", 8);//assigned
                    hstbl.Add("assigneddate", "getdate()");
                    hstbl.Add("employeeid", Common.EmployeeId);
                    hstbl.Add("enquirydate", "getdate()");
                    GlobalData gblData = new GlobalData("tbl_enquiry", "enquiry_enquiryid");
                    string enquiryCode = UniqueCode.GetUniqueCode(gblData, "enquiryno", "E-", 1);
                    hstbl.Add("enquiryno", enquiryCode);
                }
                int enquiryId = 0;
                if (alreadyExists)
                {
                    enquiryId = obj.UpdateData(hstbl, "tbl_enquiry", oldenquiryId);
                }
                else
                {
                    enquiryId = obj.InsertData(hstbl, "tbl_enquiry");
                }
                if (enquiryId > 0)
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
        Response.Redirect("~/importdata/import-enquiry.aspx");
    }
    private bool IsAlreadyExists(string customerName, string mobileNo, string emailId, ref int enquiryId)
    {
        enquiryId = 0;
        string query = "select * from tbl_enquiry WHERE (enquiry_companyname='" + global.CheckInputData(customerName) + "' " +
                        "OR enquiry_mobileno='" + global.CheckInputData(mobileNo) + "' OR enquiry_emailid='" + global.CheckInputData(emailId) + "') "+
                        "AND enquiry_enquirystatusid IN(1,8)";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            return false;
        }
        else
        {
            enquiryId = GlobalUtilities.ConvertToInt(dr["enquiry_enquiryid"]);
            return true;
        }
    }
}
