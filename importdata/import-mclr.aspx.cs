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

public partial class importdata_importmclr : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lnkDownloadTemplate.NavigateUrl = "~/upload/exceltemplate/mclr.xlsx";
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
        dttblInvalidData.Columns.Add("Bank Name");
        dttblInvalidData.Columns.Add("Effective Date");
        dttblInvalidData.Columns.Add("Base Rate");
        dttblInvalidData.Columns.Add("Error Remarks");
        string errorRemarks = "";
        int totalImported = 0;
        int totalRecords = 0;
        for (int i = 0; i < dttblData.Rows.Count; i++)
        {
            try
            {
                Hashtable hstbl = new Hashtable();
                DateTime dt = Convert.ToDateTime(dttblData.Rows[i]["mclr_effectivedate"]);
                string effectiveDate = GlobalUtilities.ConvertToDate(dttblData.Rows[i]["mclr_effectivedate"]);
                string bankName = GlobalUtilities.ConvertToString(dttblData.Rows[i]["mclrbank_bankname"]);
                string baseRate = GlobalUtilities.ConvertToString(dttblData.Rows[i]["mclr_baserate"]);
                if (effectiveDate == "" && bankName == "" && baseRate == "") continue;
                bool isvalid = true;
                int oldmclrId = 0;
                int bankId = 0;
                totalRecords++;
                effectiveDate = effectiveDate.Replace(" 12:00:00 AM", "").Replace("/", "-");
                if (effectiveDate == "")
                {
                    isvalid = false;
                    errorRemarks = "Date is required";
                }
                //Array arr = effectiveDate.Split('-');
                //string day = arr.GetValue(0).ToString();
                //string month = arr.GetValue(1).ToString();
                //if (day.Length == 1) day = "0" + day;
                //if (month.Length == 1) month = "0" + month;
                //effectiveDate = day + "-" + month + "-" + arr.GetValue(2).ToString();
                if (bankName == "")
                {
                    isvalid = false;
                    errorRemarks = "Bank Name is required";
                }

                if (isvalid)
                {
                    if (isvalid)
                    {
                        for (int j = 0; j < dttblData.Columns.Count; j++)
                        {
                            string data = GlobalUtilities.ConvertToString(dttblData.Rows[i][j]);
                            string columnName = dttblData.Columns[j].ColumnName.ToLower();
                            if (columnName == "mclrbank_bankname")
                            {
                                bankId = SaveMCLRBank(data);
                                hstbl.Add("mclrbankid", bankId);
                            }
                            else if (columnName == "mclr_effectivedate")
                            {
                                hstbl.Add("effectivedate", effectiveDate);
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
                    int mclrId = 0;
                    string query = "select * from tbl_mclr where mclr_mclrbankid=" + bankId;
                    DataRow dr = DbTable.ExecuteSelectRow(query);
                    if (dr != null)
                    {
                        oldmclrId = GlobalUtilities.ConvertToInt(dr["mclr_mclrid"]);
                    }
                    if (oldmclrId > 0)
                    {
                        if (GlobalUtilities.ConvertToDate(dr["mclr_effectivedate"]) != effectiveDate)
                        {
                            query = @"insert into tbl_mclrhistory(mclrhistory_mclrid,mclrhistory_mclrbankid,mclrhistory_1month,mclrhistory_3months,
                                  mclrhistory_6months,mclrhistory_1year,mclrhistory_effectivedate,mclrhistory_baserate)
                                  select mclr_mclrid,mclr_mclrbankid,mclr_1month,mclr_3months,
                                  mclr_6months,mclr_1year,mclr_effectivedate,mclr_baserate
                                  from tbl_mclr where mclr_mclrid=" + oldmclrId;
                            DbTable.ExecuteQuery(query);
                        }
                        mclrId = obj.UpdateData(hstbl, "tbl_mclr", oldmclrId);
                    }
                    else
                    {
                        mclrId = obj.InsertData(hstbl, "tbl_mclr");
                    }
                    if (mclrId > 0)
                    {
                        totalImported++;
                    }
                }
                else
                {
                    DataRow drInvalid = dttblInvalidData.NewRow();
                    drInvalid["Bank Name"] = bankName;
                    drInvalid["Effective Date"] = effectiveDate;
                    drInvalid["Base Rate"] = baseRate;
                    drInvalid["Error Remarks"] = errorRemarks;
                    dttblInvalidData.Rows.Add(drInvalid);
                }
            }
            catch (Exception ex)
            {
                ex = ex;
            }
        }
        try
        {
            File.Delete(fileName);
        }
        catch { }
        lblMessage.Text = "Data uploaded successfully!";
        trResult.Visible = true;
        lblTotalData.Text = totalRecords.ToString();
        lblInvaliddatas.Text = dttblInvalidData.Rows.Count.ToString();
        lblValiddatas.Text = totalImported.ToString();// Convert.ToString(dttblData.Rows.Count - dttblInvalidData.Rows.Count);
        lblDataImported.Text = totalImported.ToString();

        ltResult.Text = Common.BindGrid(dttblInvalidData, "Bank Name,Effective Date,Base Rate,Error Remarks",
                                                          "Bank Name,Effective Date,Base Rate,Error Remarks");
    }

    private int SaveMCLRBank(string mclrbankName)
    {
        if (mclrbankName.Trim() == "") return 0;
        string query = "select * from tbl_mclrbank WHERE mclrbank_bankname='" + mclrbankName + "'";
        DataRow drmclrbank = DbTable.ExecuteSelectRow(query);
        Hashtable hstbl = new Hashtable();
        int mclrbankId = 0;
        if (drmclrbank == null)
        {
            hstbl.Add("bankname", mclrbankName);
            InsertUpdate obj = new InsertUpdate();
            mclrbankId = obj.InsertData(hstbl, "tbl_mclrbank");
        }
        else
        {
            mclrbankId = GlobalUtilities.ConvertToInt(drmclrbank["mclrbank_mclrbankid"]);
        }
        return mclrbankId;
    }
}
