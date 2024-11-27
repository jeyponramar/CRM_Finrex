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

public partial class importdata_import_customrate : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lnkDownloadTemplate.NavigateUrl = "~/upload/exceltemplate/customrate.xlsx";
            //PopulateData();
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");

    }
    private int Id
    {
        get
        {
            return Common.GetQueryStringValue("id");
        }
    }
    private void PopulateData()
    {
        if (Id == 0) return;
        string query = "";
        query = @"select * from tbl_customrateuploadhistory
                    join tbl_employee on employee_employeeid=customrateuploadhistory_employeeid
                    where customrateuploadhistory_customrateuploadhistoryid=" + Id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        lbluploaddate.Text = GlobalUtilities.ConvertToDate(dr["customrateuploadhistory_uploaddate"]);
        lbluploadedby.Text = GlobalUtilities.ConvertToString(dr["employee_employeename"]);
        query = @"select * from tbl_customrate
                join tbl_othercurrency on othercurrency_othercurrencyid=customrate_othercurrencyid
                where customrate_customrateuploadhistoryid=" + Id;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        tbledit.Visible = true;

        ltdata.Text = Common.BindGrid(dttbl, "othercurrency_currency,customrate_date~date,customrate_import,customrate_export", "Currancy,Date,Import,Export");
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
        if (dttblData.Columns.Count != 4)
        {
            lblMessage.Text = "Invalid template format.";
            lblMessage.Visible = true;
            return;
        }
        DataTable dttblInvalidData = new DataTable();
        dttblInvalidData.Columns.Add("Column1");
        dttblInvalidData.Columns.Add("Column2");
        dttblInvalidData.Columns.Add("Error Remarks");
        int totalImported = 0;
        int totalRecords = dttblData.Rows.Count;

        string query = "";
        //int historyId = Id;
        //Hashtable hstblhistory = new Hashtable();
        //hstblhistory.Add("employeeid", Common.EmployeeId);
        //hstblhistory.Add("uploaddate", "getdate()");
        //hstblhistory.Add("uploadstatus", "Failed");
        //InsertUpdate obj1 = new InsertUpdate();
        //query = "update tbl_customrate set customrate_isactive=0";
        //DbTable.ExecuteQuery(query);
        //if (historyId > 0)
        //{
        //    query = "delete from tbl_customrate where customrate_customrateuploadhistoryid=" + historyId;
        //    DbTable.ExecuteQuery(query);
        //    obj1.UpdateData(hstblhistory, "tbl_customrateuploadhistory", historyId);
        //}
        //else
        //{
        //    historyId = obj1.InsertData(hstblhistory, "tbl_customrateuploadhistory");
        //}
        query = "select * from tbl_othercurrency";
        DataTable dttblothercurrency = DbTable.ExecuteSelect(query);

        for (int i = 0; i < dttblData.Rows.Count; i++)
        {
            string error = "";
            string date = "";
            DateTime dt = new DateTime();
            double importrate = 0;
            double exportrate = 0;
            int othercurrencyId = 0;
            if (DateTime.TryParse(Convert.ToString(dttblData.Rows[i][0]), out dt))
            {
                date = GlobalUtilities.ConvertToDate(dttblData.Rows[i][0]);
            }
            else
            {
                error = "Invalid date format : " + Convert.ToString(dttblData.Rows[i][0]);
            }
            string othercurrency = GlobalUtilities.ConvertToString(dttblData.Rows[i][1]).Trim().ToLower();

            if (Double.TryParse(Convert.ToString(dttblData.Rows[i][2]), out importrate))
            {
                importrate = GlobalUtilities.ConvertToDouble(dttblData.Rows[i][2]);
            }
            else
            {
                error = "Invalid Import rate format : " + Convert.ToString(dttblData.Rows[i][2]);
            }

            if (Double.TryParse(Convert.ToString(dttblData.Rows[i][3]), out exportrate))
            {
                exportrate = GlobalUtilities.ConvertToDouble(dttblData.Rows[i][3]);
            }
            else
            {
                error = "Invalid Export Rate format : " + Convert.ToString(dttblData.Rows[i][3]);
            }
            bool isexists = false;
            for (int j = 0; j < dttblothercurrency.Rows.Count; j++)
            {
                if (GlobalUtilities.ConvertToString(dttblothercurrency.Rows[j]["othercurrency_currency"]).ToLower().Trim() == othercurrency)
                {
                    othercurrencyId = GlobalUtilities.ConvertToInt(dttblothercurrency.Rows[j]["othercurrency_othercurrencyid"]);
                    isexists = true;
                    break;
                }
            }
            if (!isexists)
            {
                error = "Invalid Currency";
            }
            string customRateDate = GlobalUtilities.ConvertMMDateToDD(date);
            query = "select * from tbl_customrate where customrate_othercurrencyid=" + othercurrencyId + " and cast(customrate_date as date)=cast(@date as date)";
            Hashtable hstblp = new Hashtable();
            hstblp.Add("date", customRateDate);
            DataRow drexistingrate = DbTable.ExecuteSelectRow(query, hstblp);
            if (drexistingrate != null)
            {
                error = "Data already exists";
            }
            if (error == "")
            {
                Hashtable hstbl = new Hashtable();
                //hstbl.Add("customuploadhistoryid", historyId);
                hstbl.Add("othercurrencyid", othercurrencyId);
                hstbl.Add("export", exportrate);
                hstbl.Add("date", date);
                hstbl.Add("import", importrate);
                //hstbl.Add("isactive", 1);
                InsertUpdate obj = new InsertUpdate();
                obj.InsertData(hstbl, "tbl_customrate");
                totalImported++;
            }
            else
            {
                DataRow drerr = dttblInvalidData.NewRow();
                drerr["Column1"] = othercurrency;
                drerr["Column2"] = date;
                drerr["Error Remarks"] = error;
                dttblInvalidData.Rows.Add(drerr);
            }
        }
//        if (dttblData.Rows.Count == totalImported)
//        {
//            query = @"update tbl_customrateuploadhistory set customrateuploadhistory_uploadstatus='Success' 
//                    where customrateuploadhistory_customrateuploadhistoryid=" + historyId;
//            DbTable.ExecuteQuery(query);
//        }
        try
        {
            File.Delete(fileName);
        }
        catch (Exception ce) { }
        lblMessage.Text = "Data uploaded successfully!";
        trResult.Visible = true;
        lblTotalData.Text = totalRecords.ToString();
        lblInvaliddatas.Text = dttblInvalidData.Rows.Count.ToString();
        lblValiddatas.Text = totalImported.ToString();// Convert.ToString(dttblData.Rows.Count - dttblInvalidData.Rows.Count);
        lblDataImported.Text = totalImported.ToString();

        ltResult.Text = Common.BindGrid(dttblInvalidData, "Column1,Column2,Error Remarks",
                                                          "Column1,Column2,Error Remarks");
    }
    
}
