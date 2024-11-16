using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Collections;

public partial class CP_report_settings : System.Web.UI.Page
{
    int _moduleId = 0;
    GlobalData gblData = new GlobalData("tbl_report", "reportid");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindColumns();
        }
    }
    private void BindColumns()
    {
        string query = "";
        DataRow dr = DbTable.GetOneRow("tbl_report", GlobalUtilities.ConvertToInt(Request.QueryString["id"]));
        if (dr == null) return;
        int moduleId = GlobalUtilities.ConvertToInt(dr["report_moduleid"]);
        query = GlobalUtilities.ConvertToString(dr["report_jointables"]);
        query = "select top 1 * from (" + query + ")rep1";
        //query = "select 0 as columns_columnsid,'Select' as columns_columnname union select columns_columnsid,columns_columnname "+
        //                "from tbl_columns where columns_moduleid=" + _moduleId + " and columns_subsectionid=0 and columns_control<>'Sub Grid' " +
                            //"and columns_control<>'Section'";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        DataTable dttblColumns = new DataTable();
        dttblColumns.Columns.Add("columns_columnsid");
        dttblColumns.Columns.Add("columns_columnname");

        DataRow dr1 = dttblColumns.NewRow();
        dr1["columns_columnsid"] = "";
        dr1["columns_columnname"] = "Select";
        dttblColumns.Rows.Add(dr1);

        for (int i = 0; i < dttbl.Columns.Count; i++)
        {
            dr1 = dttblColumns.NewRow();
            dr1["columns_columnsid"] = dttbl.Columns[i].ColumnName;
            dr1["columns_columnname"] = dttbl.Columns[i].ColumnName;
            dttblColumns.Rows.Add(dr1);
        }

        Column1.DataSource = dttblColumns;
        Column1.DataTextField = "columns_columnname";
        Column1.DataValueField = "columns_columnsid";
        Column1.DataBind();

        Column2.DataSource = dttblColumns;
        Column2.DataTextField = "columns_columnname";
        Column2.DataValueField = "columns_columnsid";
        Column2.DataBind();
    }
    protected void ddlColumn1_Changed(object sender, EventArgs e)
    {
        if (Column1.SelectedIndex > 0)
        {
            string column = Column1.SelectedItem.Text;
            if (txtXAxisColumns.Text == "")
            {
                txtXAxisColumns.Text = column;
            }
            else
            {
                txtXAxisColumns.Text += "," + column;
            }
        }
    }
    protected void ddlColumn2_Changed(object sender, EventArgs e)
    {
        if (Column2.SelectedIndex > 0)
        {
            string column = Column2.SelectedItem.Text;
            if (txtYAxisColumns.Text == "")
            {
                txtYAxisColumns.Text = column;
            }
            else
            {
                txtYAxisColumns.Text += "," + column;
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        gblData.SaveForm(form);
    }
}
