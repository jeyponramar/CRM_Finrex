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
using System.IO;
using System.Data.OleDb;
using System.Text;
using WebComponent;

public partial class generatequote : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    private void ExtractQuoteData(DataTable dttblFrom, DataTable dttblToData)
    {
        int headerRow = 0;
        bool foundHeader = false;
        int productColumn = 0;
        for (int i = 0; i < dttblFrom.Rows.Count; i++)
        {
            for (int j = 0; j < dttblFrom.Columns.Count; j++)
            {
                string data = Convert.ToString(dttblFrom.Rows[i][j]);
                if (data.ToUpper().Contains("GADGETS SUMMARY"))
                {
                    headerRow = i + 1;
                    foundHeader = true;
                    productColumn = j;
                    break;
                }
            }
            if (foundHeader) break;
        }
        if (foundHeader == false) return;
        int quantityColumn = 0;
        for (int i = productColumn + 1; i < dttblFrom.Columns.Count; i++)
        {
            if (Convert.ToString(dttblFrom.Rows[headerRow][i]).Trim() != "")
            {
                quantityColumn = i;
                break;
            }
        }
        for (int i = headerRow; i < dttblFrom.Rows.Count; i++)
        {
            string strProduct = Convert.ToString(dttblFrom.Rows[i][productColumn]);
            string strQty = Convert.ToString(dttblFrom.Rows[i][quantityColumn]);
            if (strQty.Trim() != "")
            {
                int quantity = Convert.ToInt32(strQty.ToLower().Replace(".", "").Replace("nos", "").Replace("no", ""));
                bool productExists = false;
                for (int j = 0; j < dttblToData.Rows.Count; j++)
                {
                    if (Convert.ToString(dttblToData.Rows[j]["ProductName"]) == strProduct)
                    {
                        dttblToData.Rows[j]["Quantity"] = quantity + Convert.ToInt32(dttblToData.Rows[j]["Quantity"]);
                        productExists = true;
                        break;
                    }
                }
                if (!productExists)
                {
                    DataRow dr = dttblToData.NewRow();
                    dr["ProductName"] = strProduct;
                    dr["Quantity"] = quantity;
                    dttblToData.Rows.Add(dr);
                }
            }
            else
            {
                if (strProduct.ToLower().Contains("note"))
                {
                    string strNote = strProduct;
                }
            }
        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (flLayout.HasFile)
        {
            string fileName = flLayout.FileName.ToLower();
            int l = fileName.LastIndexOf(".");
            string ext = fileName.Substring(l);
            if (fileName.EndsWith(".xls") || fileName.EndsWith(".xlsx"))
            {
                string newFile = Server.MapPath("~/temp/" + Guid.NewGuid().ToString() + ext);
                flLayout.SaveAs(newFile);

                OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + newFile
                                                            + ";Extended Properties='Excel 12.0;HDR=YES'");
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                System.Data.DataTable dtExcelSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                DataTable dttblQuoteData = new DataTable();
                dttblQuoteData.Columns.Add("ProductName");
                dttblQuoteData.Columns.Add("Quantity");

                for (int i = 0; i < dtExcelSchema.Rows.Count; i++)
                {
                    string sheetName = Convert.ToString(dtExcelSchema.Rows[i]["Table_Name"]);
                    if (sheetName.EndsWith("$'"))
                    {
                        DataTable dttblData = Common.GetExcelSheetData(newFile, sheetName);
                        ExtractQuoteData(dttblData, dttblQuoteData);
                    }
                }
                StringBuilder html = new StringBuilder();
                html.Append("<table border=1><tr><td><b>Product</b></td><td><b>Qty</b></td></tr>");
                for (int i = 0; i < dttblQuoteData.Rows.Count; i++)
                {
                    html.Append("<tr>");
                    html.Append("<td>" + Convert.ToString(dttblQuoteData.Rows[i]["ProductName"]) + "</td>");
                    html.Append("<td>" + Convert.ToString(dttblQuoteData.Rows[i]["Quantity"]) + "</td>");
                    html.Append("</tr>");
                }
                html.Append("<table>");
                ltData.Text = html.ToString();
                conn.Close();

                
                //Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();

                //Microsoft.Office.Interop.Excel.Workbook wb = excelApp.Workbooks.Open(newFile, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                //                                                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                //                                                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                //                                                Type.Missing, Type.Missing);

                //Worksheet sheet = (Worksheet)wb.Sheets["GROUND FLOOR"];
                //int rows = sheet.Rows.Count;
                //if (rows > 1000) rows = 1000;
                //int cols = sheet.Columns.Count;
                //if (cols > 100) cols = 100;


                //for (int i = 1; i < rows; i++)
                //{
                //    for (int j = 1; j < cols; j++)
                //    {
                //        var val = (Range)sheet.Cells[i, j];
                //        //string v = Convert.ToString(val.Value2);
                //    }
                //}

                //Range excelRange = sheet.UsedRange;

                //excelApp.Workbooks.Close();
                //excelApp.Quit();
                //excelApp = null;

                File.Delete(newFile);
            }
        }
    }
}
