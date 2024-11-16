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

public partial class importdata_importmonthlycommodity : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lnkDownloadTemplate.NavigateUrl = "~/upload/exceltemplate/monthlycommodityrate.xlsx";
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
        if (!Directory.Exists(Server.MapPath("~/upload/temp/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/upload/temp/"));
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
        dttblInvalidData.Columns.Add("Column1");
        dttblInvalidData.Columns.Add("Column2");
        dttblInvalidData.Columns.Add("Error Remarks");
        int totalImported = 0;
        int totalRecords = 0;

        string query = "";

        query = "select * from tbl_metal";
        DataTable dttblmetal = DbTable.ExecuteSelect(query);
        double usdcloseRate = GlobalUtilities.ConvertToDouble(txtusdclosingrate.Text);
        double usdrbirefRate = GlobalUtilities.ConvertToDouble(txtrbirefrate.Text);
        StringBuilder ids = new StringBuilder();
        for (int i = 0; i < dttblData.Rows.Count; i++)
        {
            InsertUpdate obj = new InsertUpdate();
            //string monthyear = Convert.ToString(dttblData.Rows[i][0]).Trim();
            if ((dttblData.Rows[i][0] == DBNull.Value || GlobalUtilities.ConvertToString(dttblData.Rows[i][0]) == "")
                    && (dttblData.Rows[i][1] == DBNull.Value || GlobalUtilities.ConvertToString(dttblData.Rows[i][1]) == ""))
            {
                continue;
            }
            DateTime dt = Convert.ToDateTime(dttblData.Rows[i][0]);
            string metal = Convert.ToString(dttblData.Rows[i][1]).Trim();
            double cash = GlobalUtilities.ConvertToDouble(dttblData.Rows[i][2]);
            //if (metal.ToLower().Replace(" ", "") == "closingrate")
            //{
            //    usdcloseRate = cash;
            //    continue;
            //}
            //if (metal.ToLower().Replace(" ", "") == "rbirefrate")
            //{
            //    usdrbirefRate = cash;
            //    continue;
            //}
            double threemonths = GlobalUtilities.ConvertToDouble(dttblData.Rows[i][3]);
            //if (!monthyear.Contains("-")) continue;
            //Array arr = monthyear.Split('-');
            //int month = Convert.ToInt32(arr.GetValue(0));
            //int year = Convert.ToInt32(arr.GetValue(1));
            //string m = month.ToString();
            //if (m.Length == 1) m = "0" + m;
            //string sqldate = year.ToString() + "-" + m + "-01";
            //string date = "01-" + m + "-" + year;
            string sqldate = GlobalUtilities.ConvertToSqlDate(dt);
            string date = GlobalUtilities.ConvertToDate(dt);
            int metalId = GetIdFromTable(dttblmetal, metal);
            if (metalId == 0) continue;
            query = "select * from tbl_MonthlyLMEMetalRate where MonthlyLMEMetalRate_date='" + sqldate + 
                        "' and MonthlyLMEMetalRate_metalid=" + metalId;
            DataRow dr = DbTable.ExecuteSelectRow(query);
            Hashtable hstbl = new Hashtable();
            hstbl.Add("date", date);
            hstbl.Add("metalid", metalId);
            hstbl.Add("cash", cash);
            hstbl.Add("threemonths", threemonths);
            int id = 0;
            if (dr == null)
            {
                id = obj.InsertData(hstbl, "tbl_MonthlyLMEMetalRate");
            }
            else
            {
                id = obj.UpdateData(hstbl, "tbl_MonthlyLMEMetalRate", GlobalUtilities.ConvertToInt(dr["MonthlyLMEMetalRate_MonthlyLMEMetalRateid"]));
            }
            if (id > 0)
            {
                if (ids.ToString() == "")
                {
                    ids.Append(id.ToString());
                }
                else
                {
                    ids.Append("," + id.ToString());
                }
                totalImported++;
            }
            else
            {

            }
        }
        if (ids.ToString() != "")
        {
            query = @"update tbl_MonthlyLMEMetalRate set monthlylmemetalrate_usdinrclose=" + usdcloseRate + "," +
                      "monthlylmemetalrate_usdinrrbirefrate=" + usdrbirefRate + @"
                     where MonthlyLMEMetalRate_MonthlyLMEMetalRateid in(" + ids.ToString() + ")";
            DbTable.ExecuteQuery(query);
        }
        try
        {
            File.Delete(fileName);
        }
        catch(Exception ce) { }
        lblMessage.Text = "Data uploaded successfully!";
        trResult.Visible = true;
        lblTotalData.Text = totalRecords.ToString();
        lblInvaliddatas.Text = dttblInvalidData.Rows.Count.ToString();
        lblValiddatas.Text = totalImported.ToString();// Convert.ToString(dttblData.Rows.Count - dttblInvalidData.Rows.Count);
        lblDataImported.Text = totalImported.ToString();

        ltResult.Text = Common.BindGrid(dttblInvalidData, "Column1,Column2,Error Remarks",
                                                          "Column1,Column2,Error Remarks");
    }
    private int GetIdFromTable(DataTable dttbl, string data)
    {
        data = data.ToLower();
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (dttbl.Rows[i][1].ToString().Trim().ToLower() == data) return Convert.ToInt32(dttbl.Rows[i][0]);
        }
        return 0;
    }

  
}
