using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WebComponent;
using System.IO;
using System.Data.OleDb;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

/// <summary>
/// Summary description for Common
/// </summary>
public class CustomFunction
{
    public static void ExportToExcel(DataTable dt, DataTable dttblcolumns)
    {
        string filename = "data.xls";
        System.IO.StringWriter tw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        DataGrid dgGrid = new DataGrid();
        DataTable dttblData = new DataTable();
        dttblData.Columns.Add("Sr No");
        for (int i = 0; i < dttblcolumns.Rows.Count; i++)
        {
            dttblData.Columns.Add(Convert.ToString(dttblcolumns.Rows[i]["columns_lbl"]));
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow dr = dttblData.NewRow();
            dr["Sr No"] = i + 1;
            for (int j = 0; j < dttblcolumns.Rows.Count; j++)
            {
                string columnName = Convert.ToString(dttblcolumns.Rows[j]["columns_columnname"]);
                string control = Convert.ToString(dttblcolumns.Rows[j]["columns_control"]);
                string dropdowncolumnName = Convert.ToString(dttblcolumns.Rows[j]["columns_dropdowncolumn"]);
                string val = "";
                if (dropdowncolumnName == "yesno_yesno")
                {
                    if (GlobalUtilities.ConvertToInt(dt.Rows[i][columnName]) == 1)
                    {
                        val = "Yes";
                    }
                    else
                    {
                        val = "No";
                    }
                }
                else
                {
                    if (dropdowncolumnName != "")
                    {
                        columnName = dropdowncolumnName;
                    }
                    if (control == "Date")
                    {
                        val = GlobalUtilities.ConvertToDate(dt.Rows[i][columnName]);
                    }
                    else
                    {
                        val = Convert.ToString(dt.Rows[i][columnName]);
                    }
                }
                dr[dttblcolumns.Rows[j]["columns_lbl"].ToString()] = val;
            }
            dttblData.Rows.Add(dr);
        }
        dgGrid.DataSource = dttblData;
        dgGrid.DataBind();

        //Get the HTML for the control.
        dgGrid.RenderControl(hw);
        //Write the HTML back to the browser.
        //Response.ContentType = application/vnd.ms-excel;
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
        //Page.EnableViewState = false;
        HttpContext.Current.Response.Write(tw.ToString());
        HttpContext.Current.Response.End();
    }
}