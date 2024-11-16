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

public partial class importdata_import_interestrate : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lnkDownloadTemplate.NavigateUrl = "~/upload/exceltemplate/interestrate.xlsx";
            PopulateData();
            BindInterestType();
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
    private void BindInterestType()
    {
        string query = "select * from tbl_interestratetype";
        StringBuilder html = new StringBuilder();
        DataTable dttbl = DbTable.ExecuteSelect(query);
        html.Append("<table border=1>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            html.Append("<tr><td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["interestratetype_type"])+"</td>");
        }
        html.Append("<table>");
        ltinteresttype.Text = html.ToString();
    }
    private void PopulateData()
    {
        if (Id == 0) return;
        string query = "";
        query = @"select * from tbl_interestrateuploadhistory
                    join tbl_employee on employee_employeeid=interestrateuploadhistory_employeeid
                    where interestrateuploadhistory_interestrateuploadhistoryid=" + Id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        lbluploaddate.Text = GlobalUtilities.ConvertToDate(dr["interestrateuploadhistory_uploaddate"]);
        lbluploadedby.Text = GlobalUtilities.ConvertToString(dr["employee_employeename"]);
        query = @"select * from tbl_interestrate
                join tbl_interestratetype on interestratetype_interestratetypeid=interestrate_interestratetypeid
                where interestrate_interestrateuploadhistoryid=" + Id;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        tbledit.Visible = true;

        ltdata.Text = Common.BindGrid(dttbl, "interestratetype_type,interestrate_date~date,interestrate_particular,interestrate_rate", "Type,Date,Particular,Rate");
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
        int historyId = Id;
        Hashtable hstblhistory = new Hashtable();
        hstblhistory.Add("employeeid", Common.EmployeeId);
        hstblhistory.Add("uploaddate", "getdate()");
        hstblhistory.Add("uploadstatus", "Failed");
        InsertUpdate obj1 = new InsertUpdate();
        query = "update tbl_interestrate set interestrate_isactive=0";
        DbTable.ExecuteQuery(query);
        if (historyId > 0)
        {
            query = "delete from tbl_interestrate where interestrate_interestrateuploadhistoryid=" + historyId;
            DbTable.ExecuteQuery(query);
            obj1.UpdateData(hstblhistory, "tbl_interestrateuploadhistory", historyId);
        }
        else
        {
            historyId = obj1.InsertData(hstblhistory, "tbl_interestrateuploadhistory");
        }
        query = "select * from tbl_interestratetype";
        DataTable dttblinteresttype = DbTable.ExecuteSelect(query);

        for (int i = 0; i < dttblData.Rows.Count; i++)
        {
            string error = "";
            string interestType = GlobalUtilities.ConvertToString(dttblData.Rows[i][0]).Trim().ToLower();
            string date = "";
            string particular = GlobalUtilities.ConvertToString(dttblData.Rows[i][2]);
            DateTime dt = new DateTime();
            double rate = 0;
            int interestTypeId=0;
            if (DateTime.TryParse(Convert.ToString(dttblData.Rows[i][1]), out dt))
            {
                date = GlobalUtilities.ConvertToDate(dttblData.Rows[i][1]);
            }
            else
            {
                error = "Invalid date format : " + Convert.ToString(dttblData.Rows[i][1]);
            }
            if (Double.TryParse(Convert.ToString(dttblData.Rows[i][3]).Replace("%",""), out rate))
            {
                rate = GlobalUtilities.ConvertToDouble(dttblData.Rows[i][3]);
            }
            else
            {
                error = "Invalid rate format : " + Convert.ToString(dttblData.Rows[i][3]);
            }
            bool isexists = false;
            for (int j = 0; j < dttblinteresttype.Rows.Count; j++)
            {
                if (GlobalUtilities.ConvertToString(dttblinteresttype.Rows[j]["interestratetype_type"]).ToLower().Trim() == interestType)
                {
                    interestTypeId = GlobalUtilities.ConvertToInt(dttblinteresttype.Rows[j]["interestratetype_interestratetypeid"]);
                    isexists = true;
                    break;
                }
            }
            if (!isexists)
            {
                error = "Invalid interest type";
            }
            if (error == "")
            {
                Hashtable hstbl = new Hashtable();
                hstbl.Add("interestrateuploadhistoryid", historyId);
                hstbl.Add("interestratetypeid", interestTypeId);
                hstbl.Add("particular", particular);
                hstbl.Add("date", date);
                hstbl.Add("rate", rate);
                hstbl.Add("isactive", 1);
                InsertUpdate obj = new InsertUpdate();
                obj.InsertData(hstbl, "tbl_interestrate");
                totalImported++;
            }
            else
            {
                DataRow drerr = dttblInvalidData.NewRow();
                drerr["Column1"] = interestType;
                drerr["Column2"] = date;
                drerr["Error Remarks"] = error;
                dttblInvalidData.Rows.Add(drerr);
            }
        }
        if (dttblData.Rows.Count == totalImported)
        {
            query = @"update tbl_interestrateuploadhistory set interestrateuploadhistory_uploadstatus='Success' 
                    where interestrateuploadhistory_interestrateuploadhistoryid=" + historyId;
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
    private int GetDailyRateId(int metalId, string sqldate)
    {
        string query = "select * from tbl_DailyLMEMetalRate where cast(DailyLMEMetalRate_date as date)=cast('" + sqldate + "' as date) "+
                        "and DailyLMEMetalRate_metalid=" + metalId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return 0;
        return GlobalUtilities.ConvertToInt(dr["DailyLMEMetalRate_DailyLMEMetalRateid"]);
    }
    private int GetDailyMetalCurrencyRateId(int metalId, string sqldate, int currencyId)
    {
        string query = "select * from tbl_MetalCurrencyRate where cast(MetalCurrencyRate_date as date)=cast('" + sqldate + "' as date) " +
                        "and MetalCurrencyRate_metalid=" + metalId + " and MetalCurrencyRate_metalcurrencyid=" + currencyId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return 0;
        return GlobalUtilities.ConvertToInt(dr["MetalCurrencyRate_MetalCurrencyRateid"]);
    }
    private int GetDailyRate(DataTable dttbl, int metalId)
    {
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (GlobalUtilities.ConvertToInt(dttbl.Rows[i]["DailyLMEMetalRate_metalid"]) == metalId) 
                return GlobalUtilities.ConvertToInt(dttbl.Rows[i]["DailyLMEMetalRate_DailyLMEMetalRateid"]);
        }
        return -1;
    }
    private int GetMetalCurrencyRate(DataTable dttbl, int metalId, int metalCurrencyId)
    {
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (GlobalUtilities.ConvertToInt(dttbl.Rows[i]["MetalCurrencyRate_metalid"]) == metalId &&
                GlobalUtilities.ConvertToInt(dttbl.Rows[i]["MetalCurrencyRate_metalcurrencyid"]) == metalCurrencyId)
                return GlobalUtilities.ConvertToInt(dttbl.Rows[i]["MetalCurrencyRate_MetalCurrencyRateid"]);
        }
        return -1;
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
