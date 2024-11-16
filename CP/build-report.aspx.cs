using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.Text;
using System.Collections;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class build_report : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_report");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblTitle.Text = "Build Report";

                
            Delete();
           Common.GetReportColumns(Common.GetQueryStringValue("id"));
            BindColumns();
            BindReportDetail();
            if (txtJoinTables.Text == "")
            {
               txtJoinTables.Text = Common.GetJoinTables_Script(GlobalUtilities.ConvertToInt(Request.QueryString["mid"]));
            }
            BindWhereConditions();
        }
        if (AppConstants.IsLive)
        {
            tdResetGridSetting.Visible = false;
            tdJoinTables.Visible = false;
        }
        else
        {
            tdResetGridSetting.Visible = true;
            tdJoinTables.Visible = true;
        }
    }
    private void BindColumns()
    {
        //gblData.FillDropdown(ddlColumns, "tbl_columns", "columns_columnname", "columns_columnsid",
        //                    "columns_moduleid=" + Request.QueryString["mid"] + " and columns_subsectionid=0 and columns_control<>'Sub Grid' " +
        //                    "and columns_control<>'Section'", "columns_columnname");
        string query = "select * from tbl_columns WHERE columns_moduleid=" + Request.QueryString["mid"] + " and columns_subsectionid=0 and columns_control<>'Sub Grid' " +
                       "and columns_control<>'Section'";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        DataTable dttblColumns = new DataTable();
        dttblColumns.Columns.Add("columns_columnsid");
        dttblColumns.Columns.Add("columns_columnname");
        DataRow dr = dttblColumns.NewRow();
        dr["columns_columnsid"] = 0;
        dr["columns_columnname"] = "Select";
        dttblColumns.Rows.Add(dr);
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            dr = dttblColumns.NewRow();
            string columnName = GlobalUtilities.ConvertToString(dttbl.Rows[i]["columns_columnname"]);
            dr["columns_columnsid"] = dttbl.Rows[i]["columns_columnsid"];
            dr["columns_columnname"] = columnName;
            dttblColumns.Rows.Add(dr);
            if(columnName.EndsWith("id"))
            {
                columnName = columnName.Substring(0, columnName.Length - 2);
                Array arr = columnName.Split('_');
                string subModule = Convert.ToString(arr.GetValue(1));
                query = "select * from tbl_columns JOIN tbl_module ON module_moduleid=columns_moduleid WHERE REPLACE(module_modulename,' ','')='" + subModule + "'";
                DataTable dttblSubColumns = DbTable.ExecuteSelect(query);
                for (int j = 0; j < dttblSubColumns.Rows.Count; j++)
                {
                    dr = dttblColumns.NewRow();
                    columnName = GlobalUtilities.ConvertToString(dttblSubColumns.Rows[j]["columns_columnname"]);
                    dr["columns_columnsid"] = dttblSubColumns.Rows[j]["columns_columnsid"];
                    dr["columns_columnname"] = columnName;
                    dttblColumns.Rows.Add(dr);
                }
            }
        }
        ddlColumns.DataSource = dttblColumns;
        ddlColumns.DataTextField = "columns_columnname";
        ddlColumns.DataValueField = "columns_columnsid";
        ddlColumns.DataBind();
    }
    private void BindReportDetail()
    {
        InsertUpdate obj = new InsertUpdate();
        DataRow dr = obj.ExecuteSelectRow("select * from tbl_report where report_reportid=" + Request.QueryString["id"]);
        txtJoinTables.Text = Convert.ToString(dr["report_jointables"]);
        txtWhere.Text = Convert.ToString(dr["report_wherequery"]);
        lblTitle.Text = lblTitle.Text + " - <b>" + Convert.ToString(dr["report_reportname"]) + "</b>";
    }    
   
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveReport();
        Common.GenerateReportPage(Common.GetQueryStringValue("mid"), GlobalUtilities.ConvertToInt(Request.QueryString["id"]), txtWhere.Text.Trim(), txtJoinTables.Text, chkResetGridSetting.Checked, chkResetColumns.Checked);
    }
    private void SaveReport()
    {
        Hashtable hstbl = new Hashtable();
        hstbl.Add("jointables", txtJoinTables.Text);
        hstbl.Add("wherequery", txtWhere.Text);
        InsertUpdate obj = new InsertUpdate();
        obj.UpdateData(hstbl, "tbl_report", GlobalUtilities.ConvertToInt(Request.QueryString["id"]));
        lblMessage.Text = "Report saved successfully";
    }
    
    protected void btnGotoViewPageSetting_Click(object sender, EventArgs e)
    {
        int ReportId =GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
        int ModuleId = GlobalUtilities.ConvertToInt(Request.QueryString["mid"]);
        Button btn = (Button)sender;
        if (btn.Text == "Go to Align View Page Columns Settings")
        {
            //Response.Redirect("controlpanelview-setting.aspx?mid=" + moduleid;
        }
        else
        {
            Response.Redirect("configure-viewpage.aspx?mid=" + ModuleId + "&id=" + ReportId);
        }
    }    
    protected void btnAddWhere_Click(object sender, EventArgs e)
    {
        string where = txtWhereCondition.Text;
        int id = Common.GetQueryStringValue("id");
        DataRow drReport = DbTable.GetOneRow("tbl_report", id);
        string reportName = GlobalUtilities.ConvertToString(drReport["report_reportname"]);
        string module = reportName.ToLower().Replace(" ", "");
        XmlDocument doc = new XmlDocument();
        string xmlPath = HttpContext.Current.Server.MapPath("~/xml/report/" + reportName + ".xml");
        string tableName = "tbl_" + module;
        string idcolumn = module + "_" + module + "id";
        string WHERE = txtWhere.Text.Trim();
        if (WHERE.ToLower().EndsWith(" or") || WHERE.ToLower().EndsWith(" and"))
        {
            WHERE = WHERE.Substring(0, WHERE.Length - 3).Trim();
        }
        Hashtable hstbl = new Hashtable();
        hstbl.Add("reportid", Request.QueryString["id"]);
        hstbl.Add("columnname", ddlColumns.SelectedItem.Text);
        hstbl.Add("control", ddlControlType.SelectedValue);
        hstbl.Add("operator", ddlOperator.SelectedValue);
        hstbl.Add("andor", ddlJOIN.SelectedValue);
        hstbl.Add("columnvalue", txtValue.Text);
        hstbl.Add("betweenfrom", txtFrom.Text);
        hstbl.Add("betweento", txtTo.Text);
        hstbl.Add("where", txtWhereCondition.Text);
        hstbl.Add("css", txtCss.Text);

        InsertUpdate obj = new InsertUpdate();
        if (Request.QueryString["rdid"] == null)
        {
            obj.InsertData(hstbl, "tbl_reportdetail");
        }
        else
        {
            obj.UpdateData(hstbl, "tbl_reportdetails", GlobalUtilities.ConvertToInt(Request.QueryString["cid"]));
        }
        if (txtWhere.Text == "")
        {
            txtWhere.Text = txtWhereCondition.Text;
        }
        else
        {
            txtWhere.Text = txtWhere.Text + "\n" + txtWhereCondition.Text;
        }
        SaveReport();
        txtWhereCondition.Text = "";
        ddlColumns.SelectedIndex = 0;
        ddlControlType.SelectedIndex = 0;
        ddlOperator.SelectedIndex = 0;
        ddlVariable.SelectedIndex = 0;
       Common.GenerateReportPage(Common.GetQueryStringValue("mid"), id,txtWhere.Text.Trim(),txtJoinTables.Text,chkResetGridSetting.Checked,chkResetColumns.Checked);
        BindWhereConditions();
        
    }
    protected void btnRemoveWhere_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["rdid"] != null)
        {
            string query = "delete from tbl_reportdetail WHERE reportdetail_reportdetailid=" + Request.QueryString["rdid"];
            DbTable.ExecuteQuery(query);
            string url = "build-report.aspx?id=" + Request.QueryString["id"] + "&mid=" + Request.QueryString["mid"];
            Response.Redirect(url);
        }
    }
    private void BindWhereConditions()
    {
        StringBuilder html = new StringBuilder();
        string query = "select * from tbl_reportdetail WHERE reportdetail_reportid=" + Request.QueryString["id"];
        DataTable dttbl = DbTable.ExecuteSelect(query);
        string qs = "build-report.aspx?id=" + Request.QueryString["id"] + "&mid=" + Request.QueryString["mid"];
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string rcls = "repeater-alt";
            if (i % 2 == 0) rcls = "repeater-row";
            int rdid = GlobalUtilities.ConvertToInt(Convert.ToString(dttbl.Rows[i]["reportdetail_reportdetailid"]));
            html.Append("<tr class='" + rcls + "'><td>" + Convert.ToString(dttbl.Rows[i]["reportdetail_columnname"]) + "</td><td>" +
                            Convert.ToString(dttbl.Rows[i]["reportdetail_control"]) + "</td>" +
                        "<td><a href='" + qs + "&rdid=" + rdid + "&action=remove'>Remove</a></td></tr>");
            
        }
        ltQueryBuilder.Text = html.ToString();

        
    }
    private void Delete()
    {
        if (Request.QueryString["rdid"] != null && Request.QueryString["action"]=="remove")
        {
            string query = "delete from tbl_reportdetail WHERE reportdetail_reportdetailid=" + Request.QueryString["rdid"];
            DbTable.ExecuteQuery(query);
            string url = "build-report.aspx?id=" + Request.QueryString["id"] + "&mid=" + Request.QueryString["mid"];
            Response.Redirect(url);
        }
    }
    
}

