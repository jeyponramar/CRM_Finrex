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

public partial class importdata_importdailycommodity : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lnkDownloadTemplate.NavigateUrl = "~/upload/exceltemplate/dailycommodityrate.xlsx";
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

        string date = GlobalUtilities.ConvertToDate(Convert.ToDateTime(dttblData.Columns[1].ColumnName));
        string sqldate = GlobalUtilities.ConvertToSqlDate(Convert.ToDateTime(dttblData.Columns[1].ColumnName));
        string query = "";
        //query = "select * from tbl_DailyLMEMetalRate where cast(DailyLMEMetalRate_date as date)=cast('" + sqldate + "' as date)";
        //DataTable dttblold = DbTable.ExecuteSelect(query);
        //query = "select * from tbl_MetalCurrencyRate where cast(MetalCurrencyRate_date as date)=cast('" + sqldate + "' as date)";
        //DataTable dttblmetalcurold = DbTable.ExecuteSelect(query);

        query = "select * from tbl_metal";
        DataTable dttblmetal = DbTable.ExecuteSelect(query);
        Hashtable hstbl = new Hashtable();
        query = "select * from tbl_metalcurrency";
        DataTable dttblcurrency = DbTable.ExecuteSelect(query);
        int i = 0;
        while(i < dttblData.Rows.Count)
        {
            try
            {
                if ((dttblData.Rows[i][0] == DBNull.Value || GlobalUtilities.ConvertToString(dttblData.Rows[i][0]) == "")
                    && (dttblData.Rows[i][1] == DBNull.Value || GlobalUtilities.ConvertToString(dttblData.Rows[i][1]) == ""))
                {
                    i++;
                    continue;
                }

                InsertUpdate obj = new InsertUpdate();
                string column1 = Convert.ToString(dttblData.Rows[i][0]).Trim().ToLower();
                string column2 = Convert.ToString(dttblData.Rows[i][1]).Trim().ToLower();
                if (column2 == "contract")
                {
                    i++;
                    int j = i;
                    while (j < dttblData.Rows.Count)
                    {
                        column1 = Convert.ToString(dttblData.Rows[j][0]).Trim().ToLower();
                        if (column1 == "")
                        {
                            i++;
                            break;
                        }
                        column2 = Convert.ToString(dttblData.Rows[j][1]).Trim().ToLower();
                        string metal = column1;
                        int metalid = GetIdFromTable(dttblmetal, metal);
                        if (metalid == 0)
                        {
                            i++;
                            j++;
                            continue;
                        }
                        column1 = Convert.ToString(dttblData.Rows[j][0]).Trim();
                        double cash = GlobalUtilities.ConvertToDouble(dttblData.Rows[j][2]);
                        double cashoffer = GlobalUtilities.ConvertToDouble(dttblData.Rows[j][3]);
                        double monthlyrate = GlobalUtilities.ConvertToDouble(dttblData.Rows[j+1][2]);
                        double monthoffer = GlobalUtilities.ConvertToDouble(dttblData.Rows[j+1][3]);
                        double yearlyrate = GlobalUtilities.ConvertToDouble(dttblData.Rows[j+2][2]);
                        double yearoffer = GlobalUtilities.ConvertToDouble(dttblData.Rows[j+2][3]);
                        hstbl = new Hashtable();
                        hstbl.Add("metalid", metalid);
                        hstbl.Add("date", date);
                        hstbl.Add("cash", cash);
                        hstbl.Add("cashoffer", cashoffer);
                        hstbl.Add("threemonths", monthlyrate);
                        hstbl.Add("threemonthsoffer", monthoffer);
                        hstbl.Add("oneyear", yearlyrate);
                        hstbl.Add("oneyearoffer", yearoffer);
                        obj = new InsertUpdate();
                        int id = 0;
                        int oldId = GetDailyRateId(metalid, sqldate);
                        if (oldId == 0)
                        {
                            id = obj.InsertData(hstbl, "tbl_DailyLMEMetalRate");
                        }
                        else
                        {
                            id = obj.UpdateData(hstbl, "tbl_DailyLMEMetalRate", oldId);
                        }
                        j = j + 3;
                        i = i + 3;
                        if (id > 0)
                        {
                            totalImported++;
                        }
                    }
                }
                else if (column2 == "stocks")
                {
                    i++;
                    int j = i;
                    while (j < dttblData.Rows.Count)
                    {
                        string metal = Convert.ToString(dttblData.Rows[j][0]).Trim().ToLower();
                        if (metal == "")
                        {
                            i++;
                            break;
                        }
                        string currency = Convert.ToString(dttblData.Rows[j][1]).Trim().ToLower();
                        int metalid = GetIdFromTable(dttblmetal, metal);
                        if (metalid == 0)
                        {
                            i++;
                            j++;
                            continue;
                        }
                        double openingstock = GlobalUtilities.ConvertToDouble(dttblData.Rows[j][2]);
                        double livewarrants = GlobalUtilities.ConvertToDouble(dttblData.Rows[j+1][2]);
                        double cancelledwarrants = GlobalUtilities.ConvertToDouble(dttblData.Rows[j+2][2]);
                        hstbl = new Hashtable();
                        hstbl.Add("metalid", metalid);
                        hstbl.Add("date", date);
                        hstbl.Add("openingstock", openingstock);
                        hstbl.Add("livewarrants", livewarrants);
                        hstbl.Add("cancelledwarrants", cancelledwarrants);
                        int id = 0;
                        int oldId = GetDailyRateId(metalid, sqldate);
                        obj = new InsertUpdate();
                        if (oldId == 0)
                        {
                            id = obj.InsertData(hstbl, "tbl_DailyLMEMetalRate");
                        }
                        else
                        {
                            id = obj.UpdateData(hstbl, "tbl_DailyLMEMetalRate", oldId);
                        }
                        i = i + 3;
                        j = j + 3;
                        if (id > 0)
                        {
                            totalImported++;
                        }

                    }
                }
                else if (column2 == "currency")
                {
                    i++;
                    int j = i;
                    while (j < dttblData.Rows.Count)
                    {
                        string metal = Convert.ToString(dttblData.Rows[j][0]).Trim().ToLower();
                        if (metal == "")
                        {
                            i++;
                            break;
                        }
                        string currency = Convert.ToString(dttblData.Rows[j][1]).Trim().ToLower();
                        int metalid = GetIdFromTable(dttblmetal, metal);
                        int currencyid = GetIdFromTable(dttblcurrency, currency);
                        if (metalid == 0 || currencyid == 0)
                        {
                            i++;
                            j++;
                            continue;
                        }
                        double rate = GlobalUtilities.ConvertToDouble(dttblData.Rows[j][2]);
                        hstbl = new Hashtable();
                        hstbl.Add("metalid", metalid);
                        hstbl.Add("metalcurrencyid", currencyid);
                        hstbl.Add("date", date);
                        hstbl.Add("rate", rate);
                        obj = new InsertUpdate();
                        int oldid = GetDailyMetalCurrencyRateId(metalid, sqldate, currencyid);
                        int id = 0;
                        if (oldid == 0)
                        {
                            id = obj.InsertData(hstbl, "tbl_MetalCurrencyRate");
                        }
                        else
                        {
                            id = obj.UpdateData(hstbl, "tbl_MetalCurrencyRate", oldid);
                        }
                        
                        i++;
                        j++;
                        if (id > 0)
                        {
                            totalImported++;
                        }

                    }
                }
                else if (column2 == "metal")
                {
                    i++;
                    int j = i;
                    while (j < dttblData.Rows.Count)
                    {
                        string metal = Convert.ToString(dttblData.Rows[j][0]).Trim().ToLower();
                        if (metal == "")
                        {
                            i++;
                            break;
                        }
                        int metalid = GetIdFromTable(dttblmetal, metal);
                        if (metalid == 0)
                        {
                            i++;
                            j++;
                            continue;
                        }
                        double rate = GlobalUtilities.ConvertToDouble(dttblData.Rows[j][2]);
                        string contract = GlobalUtilities.ConvertToString(dttblData.Rows[j][1]);
                        hstbl = new Hashtable();
                        hstbl.Add("metalid", metalid);
                        hstbl.Add("date", date);
                        hstbl.Add("asianrate", rate);
                        hstbl.Add("asiancontract", contract);
                        obj = new InsertUpdate();
                        int id = 0;
                        int oldId = GetDailyRateId(metalid, sqldate);
                        if (oldId == 0)
                        {
                            id = obj.InsertData(hstbl, "tbl_DailyLMEMetalRate");
                        }
                        else
                        {
                            id = obj.UpdateData(hstbl, "tbl_DailyLMEMetalRate", oldId);
                        }

                        i++;
                        j++;
                        if (id > 0)
                        {
                            totalImported++;
                        }

                    }
                }
                else
                {
                    i++;
                }
                
            }
            catch (Exception ex)
            {
            }
        }
        double usdclosingrate = GlobalUtilities.ConvertToDouble(txtusdclosingrate.Text);
        double rbirefrate = GlobalUtilities.ConvertToDouble(txtrbirefrate.Text);
        query = @"update d1 set DailyLMEMetalRate_isactive=1,DailyLMEMetalRate_usdinrclose="+usdclosingrate+@" 
                ,DailyLMEMetalRate_usdinrrbirefrate="+rbirefrate+@",
                DailyLMEMetalRate_prevdayopeningstock=(select top 1 DailyLMEMetalRate_openingstock from tbl_DailyLMEMetalRate d2 where d1.DailyLMEMetalRate_metalid=d2.DailyLMEMetalRate_metalid
                and cast(DailyLMEMetalRate_date as date)<'"+sqldate+@"' order by DailyLMEMetalRate_date desc)
                from tbl_DailyLMEMetalRate d1
                where cast(DailyLMEMetalRate_date as date)=cast('"+sqldate+"' as date)";
        DbTable.ExecuteQuery(query);
        
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
