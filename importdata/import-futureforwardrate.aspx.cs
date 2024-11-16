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

public partial class importdata_import_futureforwardrate : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lnkDownloadTemplate.NavigateUrl = "~/upload/exceltemplate/futureforwardrate.xlsx";
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
        if (txtdate.Text.Trim()=="")
        {
            lblMessage.Text = "Please enter date";
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
        if (dttblData.Rows.Count != 48)//records for 4years
        {
            lblMessage.Text = "Data should have 4 years!";
            lblMessage.Visible = true;
            return;
        }
        string startDate = GlobalUtilities.ConvertToDate(dttblData.Rows[0]["MonthsEnd"]);
        string endDate = GlobalUtilities.ConvertToDate(dttblData.Rows[dttblData.Rows.Count - 1]["MonthsEnd"]);
        string date = txtdate.Text;
        string query = "";
        //query = "select * from tbl_futureforwardratehistory where cast(futureforwardratehistory_monthendstartdate as date)=cast('" + GlobalUtilities.ConvertMMDateToDD(startDate) + "' as date)" +
        //        " and cast(futureforwardratehistory_monthendenddate as date)=cast('" + GlobalUtilities.ConvertMMDateToDD(endDate) + "' as date)";
        query = "select * from tbl_futureforwardratehistory where cast(futureforwardratehistory_date as date)=cast('" + GlobalUtilities.ConvertMMDateToDD(date) + "' as date)";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        InsertUpdate obj = new InsertUpdate();
        Hashtable hstbl = new Hashtable();
        hstbl.Add("monthendstartdate",startDate);
        hstbl.Add("monthendenddate",endDate);
        hstbl.Add("uploaddate","getdate()");
        hstbl.Add("date", date);
        int id = 0;
        if (dr == null)
        {
            id = obj.InsertData(hstbl, "tbl_futureforwardratehistory");
        }
        else
        {
            id = GlobalUtilities.ConvertToInt(dr["futureforwardratehistory_futureforwardratehistoryid"]);
            obj.UpdateData(hstbl, "tbl_futureforwardratehistory", id);
            query = "delete from tbl_futureforwardrate where futureforwardrate_futureforwardratehistoryid=" + id;
            DbTable.ExecuteQuery(query);
        }

        for (int i = 0; i < dttblData.Rows.Count; i++)
        {
            string strmonthenddate = GlobalUtilities.ConvertToDateMMM(dttblData.Rows[i]["MonthsEnd"]);
            DateTime dt = Convert.ToDateTime(dttblData.Rows[i]["MonthsEnd"]);
            string monthenddate = GlobalUtilities.ConvertToDateMMM(dt);
            string shortdate = GlobalUtilities.GetMonthShortName(dt.Month).ToUpper().Substring(0, 3) + "-" + dt.Year.ToString();
            hstbl = new Hashtable();
            string usdinrbid = GlobalUtilities.ConvertToString(dttblData.Rows[i]["USDINR_Bid"]);
            string usdinrask = GlobalUtilities.ConvertToString(dttblData.Rows[i]["USDINR_Ask"]);
            string eurinrbid = GlobalUtilities.ConvertToString(dttblData.Rows[i]["EURINR_Bid"]);
            string eurinrask = GlobalUtilities.ConvertToString(dttblData.Rows[i]["EURINR_Ask"]);
            string gbpinrbid = GlobalUtilities.ConvertToString(dttblData.Rows[i]["GBPINR_Bid"]);
            string gbpinrask = GlobalUtilities.ConvertToString(dttblData.Rows[i]["GBPINR_Ask"]);

            hstbl.Add("futureforwardratehistoryid", id);
            hstbl.Add("monthenddate", monthenddate);
            hstbl.Add("usdinrbid", usdinrbid);
            hstbl.Add("usdinrask", usdinrask);
            hstbl.Add("eurinrbid", eurinrbid);
            hstbl.Add("eurinrask", eurinrask);
            hstbl.Add("gbpinrbid", gbpinrbid);
            hstbl.Add("gbpinrask", gbpinrask);
            InsertUpdate obj1 = new InsertUpdate();
            obj1.InsertData(hstbl, "tbl_futureforwardrate");

            //USDINR
            SaveLiveRate(i + 2655, shortdate);
            SaveLiveRate(i + 2703, strmonthenddate);
            SaveLiveRate(i + 2751, usdinrbid);
            SaveLiveRate(i + 2799, usdinrask);

            //EURINR
            SaveLiveRate(i + 3039, shortdate);
            SaveLiveRate(i + 3087, strmonthenddate);
            SaveLiveRate(i + 3135, eurinrbid);
            SaveLiveRate(i + 3183, eurinrask);

            //GBPINR
            SaveLiveRate(i + 3423, shortdate);
            SaveLiveRate(i + 3471, strmonthenddate);
            SaveLiveRate(i + 3519, gbpinrbid);
            SaveLiveRate(i + 3567, gbpinrask);
        }
      
        try
        {
            File.Delete(fileName);
        }
        catch (Exception ce) { }

        lblMessage.Text = "Data uploaded successfully!";
        lblMessage.Visible = true;
    }
    private void SaveLiveRate(int liverateId, string rate)
    {
        string query = "";
        query = "update tbl_liverate set liverate_prevrate=liverate_currentrate,liverate_currentrate='" + rate + "' where liverate_liverateid=" + liverateId;
        DbTable.ExecuteQuery(query);
    }
    private void SaveFutureForwardRate(DataTable dttbl, string currency)
    {
        int currencyId = 0;
        if (currency == "USDINR")
        {
        }
        else if (currency == "USDINR")
        {
        }
        else if (currency == "USDINR")
        {
        }
        string query = "";
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            query = "select * from tbl_futureforwardrate where futureforwardrate_currencyid=" + currencyId;
            DataRow dr = DbTable.ExecuteSelectRow(query);
            string date = GlobalUtilities.ConvertToDate(dttbl.Rows[i]["MonthEnd"]);
            Hashtable hstbl = new Hashtable();
            hstbl.Add("currencyid", currencyId);
            hstbl.Add("date", GlobalUtilities.ConvertMMDateToDD(date));
            hstbl.Add("bid", GlobalUtilities.ConvertToDouble(dttbl.Rows[i][currency+"_Bid"]));
            hstbl.Add("ask", GlobalUtilities.ConvertToDouble(dttbl.Rows[i][currency + "_Ask"]));
            InsertUpdate obj = new InsertUpdate();
            if (dr == null)
            {
                obj.InsertData(hstbl, "tbl_futureforwardrate");
            }
            else
            {
                obj.UpdateData(hstbl, "tbl_futureforwardrate", GlobalUtilities.ConvertToInt(dr["futureforwardrate_futureforwardrateid"]));
            }
        }
    }
}
