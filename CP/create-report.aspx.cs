using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Collections;
using System.Data;
using System.IO;
using System.Xml;
using System.Text;

public partial class create_report : System.Web.UI.Page
{
    GlobalData gbldata = new GlobalData("tbl_report");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gbldata.FillDropdown(ddlReport);
            gbldata.FillDropdown(ddlModule, "tbl_module", "module_modulename", "module_moduleid", "", "module_modulename");
            gbldata.FillDropdown(ddlMenu);
        }
    }
    protected void ddlReport_Changed(object sender, EventArgs e)
    {
        Populate();
    }
    private void Populate()
    {
        if (ddlReport.SelectedIndex == 0)
        {
            txtReportName.Text = "";
            ddlModule.SelectedIndex = 0;
            ddlMenu.SelectedIndex = 0;
            btnDelete.Visible = false;
            lnkConfigReport.Visible = false;
            return;
        }
        lnkConfigReport.Visible = true;
        int id = Convert.ToInt32(ddlReport.SelectedValue);        
        InsertUpdate obj = new InsertUpdate();
        DataRow dr = obj.ExecuteSelectRow("select * from tbl_report where report_reportid=" + ddlReport.SelectedValue);
        int mid = GlobalUtilities.ConvertToInt(dr["report_moduleid"]);
        ddlModule.SelectedValue = mid.ToString();
        txtReportName.Text = Convert.ToString(dr["report_reportname"]);
        ddlMenu.SelectedValue = GlobalUtilities.ConvertToInt(dr["report_mainmenuid"]).ToString();
        chkIsBindOnLoad.Checked = GlobalUtilities.ConvertToBool(dr["report_isbindonload"]);
        chkIsDisplayChartBelowGrid.Checked = GlobalUtilities.ConvertToBool(dr["report_isdisplaychartbelowgrid"]);
        ddlChartType.SelectedValue = GlobalUtilities.ConvertToInt(dr["report_charttype"]).ToString();
        txtGridTitle.Text = GlobalUtilities.ConvertToString(dr["report_gridtitle"]);
        txtChartHeaderColumns.Text = GlobalUtilities.ConvertToString(dr["report_chartheadercolumns"]);
        txtChartColumns.Text = GlobalUtilities.ConvertToString(dr["report_chartcolumns"]);
        txtChartColors.Text = GlobalUtilities.ConvertToString(dr["report_chartcolors"]);
        chkapplyviewrights.Checked = GlobalUtilities.ConvertToBool(dr["report_applyviewrights"]);
        btnDelete.Visible = true;
        lnkConfigReport.NavigateUrl = "build-report.aspx?id=" + id + "&mid=" + mid;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Save())
        {
            int id = Convert.ToInt32(ddlReport.SelectedValue);
            int mid = GlobalUtilities.ConvertToInt(DbTable.GetOneColumnData("tbl_report", "report_moduleid", id));
            //Response.Redirect("build-report.aspx?isbulkreport=true&uid=" + ddlMenu.SelectedValue + "&mid=" + ddlModule.SelectedValue);
            DataTable dt = getSuggestedReportName(GlobalUtilities.ConvertToInt(ddlModule.SelectedValue));
            btngenerateBulkReport.Visible = true;
            BindSuggestedReport(dt, mid);
            lnkConfigReport.Visible = true;
            lnkConfigReport.NavigateUrl = "build-report.aspx?id=" + id + "&mid=" + mid;
            lblMessage.Text = "Report saved successfully!";
            //Response.Redirect("build-report.aspx?id=" + id + "&mid=" + mid);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (ddlReport.SelectedIndex > 0)
        {
            int reportId = GlobalUtilities.ConvertToInt(ddlReport.SelectedValue);
            string reportName = txtReportName.Text.Replace(" ","");
            string query = "delete from tbl_report where report_reportid=" + reportId + ";" +
                           "delete from tbl_reportdetail WHERE reportdetail_reportid=" + reportId +
                           "delete from tbl_submenu WHERE submenu_submenuname='" + global.CheckInputData(txtReportName.Text) + "'";
            DbTable.ExecuteQuery(query);
            GlobalUtilities.DeleteFile(Server.MapPath("~/report/" + reportName + ".aspx"));
            GlobalUtilities.DeleteFile(Server.MapPath("~/report/" + reportName + ".aspx.cs"));
            GlobalUtilities.DeleteFile(Server.MapPath("~/xml/report/" + reportName + ".xml"));
            gbldata.FillDropdown(ddlReport);
            lblMessage.Text = "Report deleted successfully!";
        }
    }
    private bool Save()
    {
        int id = 0;
        int mid = 0;
        if (ddlReport.SelectedIndex == 0)
        {
            string query = "select * from tbl_report WHERE report_reportname='" + global.CheckInputData(txtReportName.Text) + "'";
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (dr != null)
            {
                lblMessage.Text = "Report already exists!";
                return false;
            }
        }
        Hashtable hstblReport = new Hashtable();
        hstblReport.Add("reportname", txtReportName.Text);
        hstblReport.Add("moduleid", ddlModule.SelectedValue);
        hstblReport.Add("mainmenuid", ddlMenu.SelectedValue);
        hstblReport.Add("gridtitle", txtGridTitle.Text);
        hstblReport.Add("isbindonload", chkIsBindOnLoad.Checked);
        hstblReport.Add("isdisplaychartbelowgrid", chkIsDisplayChartBelowGrid.Checked);
        hstblReport.Add("charttype", ddlChartType.SelectedValue);
        hstblReport.Add("chartheadercolumns", txtChartHeaderColumns.Text);
        hstblReport.Add("chartcolumns", txtChartColumns.Text);
        hstblReport.Add("chartcolors", txtChartColors.Text);
        hstblReport.Add("chartattributes", txtChartAttributes.Text);
        hstblReport.Add("applyviewrights", chkapplyviewrights.Checked);
        
        if (ddlReport.SelectedIndex > 0)
        {
            id = Convert.ToInt32(ddlReport.SelectedValue);
            string query = "select * from tbl_report WHERE report_reportname='" + global.CheckInputData(txtReportName.Text) + "' AND report_reportid<>" + id;
            DataRow dr1 = DbTable.ExecuteSelectRow(query);
            if (dr1 != null)
            {
                lblMessage.Text = "Report already exists!";
                return false;
            }
            InsertUpdate obj = new InsertUpdate();
            DataRow dr = obj.ExecuteSelectRow("select * from tbl_report where report_reportid=" + id);
            mid = Convert.ToInt32(dr["report_moduleid"]);
            if (GlobalUtilities.ConvertToBool(dr["report_iseditable"]) == false)
            {
                lblMessage.Text = "This report can not be edited";
                return false;
            }
            InsertUpdate obj1 = new InsertUpdate();
            obj1.UpdateData(hstblReport, "tbl_report", id);
        }
        else
        {
            if (Common.IsDemoVersion())
            {
                lblMessage.Text = "Please select report";
                return false;
            }
            if (txtReportName.Text == "")
            {
                lblMessage.Text = "Please enter report name";
                return false;
            }
            if (ddlModule.SelectedIndex == 0)
            {
                lblMessage.Text = "Please select a module";
                return false;
            }
            InsertUpdate obj = new InsertUpdate();
            hstblReport.Add("iseditable", 1);
            id = obj.InsertData(hstblReport, "tbl_report");
            mid = Convert.ToInt32(ddlModule.SelectedValue);
            gbldata.FillDropdown(ddlReport);
            ddlReport.SelectedValue = id.ToString();
            Populate();
        }
        if (id > 0)
        {
            int mainMenuId = GlobalUtilities.ConvertToInt(ddlMenu.SelectedValue);
            InsertUpdateSubMenu(mainMenuId, txtReportName.Text);
            UpdateXml();
            return true;
        }
        return false;
    }
    private void InsertUpdateSubMenu(int mainMenuId,string ReportName)
    {
        string query = "select * from tbl_submenu where submenu_menuid=" + mainMenuId + " and submenu_submenuname='" +global.CheckInputData(ReportName) + "'";
        InsertUpdate objs = new InsertUpdate();
        DataRow drs = objs.ExecuteSelectRow(query);
        if (drs == null)
        {
            Hashtable hstbls = new Hashtable();
            hstbls = GetSubMenuData(mainMenuId, false, ReportName);
            InsertUpdate objss = new InsertUpdate();
            int submenuId = objss.InsertData(hstbls, "tbl_submenu");
        }
        else
        {
            Hashtable hstbls = new Hashtable();
            hstbls = GetSubMenuData(mainMenuId, true, ReportName);
            InsertUpdate objss = new InsertUpdate();
            int submenuId = objss.UpdateData(hstbls, "tbl_submenu", Convert.ToInt32(drs["submenu_submenuid"]));
        }
    }
    private void UpdateXml()
    {
        string filePath = Server.MapPath("~/xml/report/" + txtReportName.Text.ToLower().Replace(" ", "") + ".xml");
        if (!File.Exists(filePath)) return;
        XmlDocument doc = new XmlDocument();
        doc.Load(filePath);
        CP cp = new CP();
        cp.AddUpdateXmlSetting_Text(doc, "isbindonload", chkIsBindOnLoad.Checked.ToString());
        cp.AddUpdateXmlSetting_Text(doc, "isdisplaychartbelowgrid", chkIsDisplayChartBelowGrid.Checked.ToString());
        cp.AddUpdateXmlSetting_Text(doc, "charttype", ddlChartType.SelectedValue.ToString());
        cp.AddUpdateXmlSetting_Text(doc, "chartheadercolumns", txtChartHeaderColumns.Text);
        cp.AddUpdateXmlSetting_Text(doc, "chartcolumns", txtChartColumns.Text);
        cp.AddUpdateXmlSetting_Text(doc, "chartcolors", txtChartColors.Text);
        cp.AddUpdateXmlSetting_Text(doc, "chartattributes", txtChartAttributes.Text);
        doc.Save(filePath);
    }
    private Hashtable GetData(int mainMenuId, bool isUpdate)
    {
        Hashtable hstbls = new Hashtable();
        if (isUpdate == false)//add
        {
            hstbls.Add("isvisible", 1);
            hstbls.Add("sequence", GetSequence(mainMenuId));
            hstbls.Add("menutype", "report");
        }
        hstbls.Add("submenuname", txtReportName.Text);
        hstbls.Add("url", "#/report/a/" + txtReportName.Text.ToLower().Replace(" ", ""));
        hstbls.Add("menuid", mainMenuId);
        hstbls.Add("gridtitle", txtGridTitle.Text);

        return hstbls;
    }
    private Hashtable GetSubMenuData(int mainMenuId, bool isUpdate,string ReportName)
    {
        Hashtable hstbls = new Hashtable();
        if (isUpdate == false)//add
        {
            hstbls.Add("isvisible", 1);
            hstbls.Add("sequence", GetSequence(mainMenuId));
            hstbls.Add("menutype", "report");
        }
        hstbls.Add("submenuname", ReportName);
        hstbls.Add("url", "#/report/a/" +ReportName.ToLower().Replace(" ", ""));
        hstbls.Add("menuid", mainMenuId);
        
        return hstbls;
    }
    private int GetSequence(int menuId)
    { 
        string query = "select top 1 * from tbl_submenu where submenu_menuid=" + menuId + " order by submenu_sequence";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return 1;
        return GlobalUtilities.ConvertToInt(dr["submenu_sequence"]);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Save();
        lblMessage.Text = "Report saved successfully";
    }
    private void BindSuggestedReport(DataTable dtSuggestedReport, int ModuleId)
    {
        ViewState["SuggestedReport"] = dtSuggestedReport;
        StringBuilder html = new StringBuilder();
        DataTable dt = new DataTable();
        dt = DbTable.ExecuteSelect(@"SELECT * FROM tbl_report WHERE report_moduleid=" + ModuleId);
        if (GlobalUtilities.IsValidaTable(dtSuggestedReport))
        {
            for (int i = 0; i < dtSuggestedReport.Rows.Count; i++)
            {
                string strReportName = GlobalUtilities.ConvertToString(dtSuggestedReport.Rows[i]["ReportName"]);
                bool ISreportExists = false;
                int ReportId = 0;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (strReportName.ToLower().Replace(" ", "").Trim() == GlobalUtilities.ConvertToString(dt.Rows[j]["report_reportname"]).ToLower().Replace(" ", "").Trim())
                    {
                        ReportId = GlobalUtilities.ConvertToInt(dt.Rows[j]["report_reportid"]);
                        ISreportExists = true;
                        break;
                    }
                }
                string rcls = "repeater-alt";
                if (i % 2 == 0)
                {
                    rcls = "repeater-row";
                    //ISreportExists = true;
                }
                string strEditHtml = (ISreportExists) ? "<td><a href='build-report.aspx?id=" + ReportId + "&mid=" + ModuleId + "' target='_blank' title='Click Here to Update " + strReportName + "' >Edit Report</a></td>" : "<td>&nbsp;</td>";
                int TempreportId = GlobalUtilities.ConvertToInt(dtSuggestedReport.Rows[i]["TempReportId"]);
                html.Append("<tr class='" + rcls + "'><td width='30'>" + Convert.ToString((ISreportExists) ? "&nbsp;</td>" : "<input type='checkbox' ReportId=" + TempreportId + " class='chk_ReportName' name='chk_" + TempreportId + "' id='" + TempreportId + "'/>") + "</td><td style='" + Convert.ToString((ISreportExists) ? "color:red;" : "") + "'>" + strReportName + "</td>"+
                    Convert.ToString((!ISreportExists)?" <td><input width='400' value='"+strReportName+"' type='text' name='txt_"+TempreportId+"' id='txt_"+TempreportId+"' /></td>":"<td>&nbsp;</td>") + strEditHtml + "</tr>");
            }
        }
        ltSuggestedReport.Text = html.ToString();
        ltSuggestedReport.Visible = true;
    }
    private DataTable getSuggestedReportName(int ModuleId)
    {
        string query = @"SELECT * FROM tbl_columns 
                        JOIN tbl_module ON module_moduleid = columns_moduleid
                        WHERE ISNULL(columns_submoduleid,0)=0 AND columns_moduleid =" + ModuleId;
        DataTable dt = DbTable.ExecuteSelect(query);
        DataTable dtSuggestedReport = new DataTable();
        dtSuggestedReport.Columns.Add("ReportName");
        dtSuggestedReport.Columns.Add("ColumnName");
        dtSuggestedReport.Columns.Add("ColumnHeader");
        dtSuggestedReport.Columns.Add("ControlName");
        dtSuggestedReport.Columns.Add("TempReportId");
        dtSuggestedReport.Columns.Add("WhereCondition");

        if (GlobalUtilities.IsValidaTable(dt))
        {
            string AllControlName = "";
            string AllColumnName = "";
            string AllheaderName = "";
            string AllWhereCondition = "";
            string tablename = GlobalUtilities.ConvertToString(dt.Rows[0]["module_tablename"]);
            string ModuleName = GlobalUtilities.ConvertToString(dt.Rows[0]["module_modulename"]);
            string strSuggestedReport_Name = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ColumnHeaderName = GlobalUtilities.ConvertToString(dt.Rows[i]["columns_gridcolumnname"]).Replace("ID", "").Replace("Id", "").Replace("id", "");

                bool isDateRangeExists = false;
                string strControlName = "";
                string strWhereCondition ="";
                string strColumnName = GlobalUtilities.ConvertToString(dt.Rows[i]["columns_columnname"]);
                string strInnerTableName = (strColumnName.IndexOf("emailid") < 0) ? (strColumnName.IndexOf("id") > 0) ? strColumnName.Substring(strColumnName.IndexOf('_') + 1, strColumnName.Length - strColumnName.IndexOf('_') - 1).Replace("id", "") : "" : "";

                string strReportName = "";
                if (strColumnName.IndexOf("date") > 0)
                {
                    if (!strColumnName.Contains("createddate") || !strColumnName.Contains("modifieddate"))
                    {
                        isDateRangeExists = true;
                        strReportName = ModuleName + " by " + ColumnHeaderName;
                        strControlName = "Date Range";
                        strWhereCondition = " " + strColumnName + " BETWEEN '$" + strColumnName + "_from$' AND '$" + strColumnName + "_to$'";
                    }
                }
                else if (strInnerTableName != "" && strInnerTableName != tablename)
                {
                    string strInnerModuleName = "";
                    DataRow dr11 = DbTable.ExecuteSelectRow("SELECT * FROM tbl_module WHERE module_tablename='tbl_" + strInnerTableName + "'");
                    if (dr11 != null)
                        strInnerModuleName = GlobalUtilities.ConvertToString(dr11["module_modulename"]);
                    strReportName = ModuleName + " by " + strInnerModuleName;
                    strControlName = (strColumnName.ToLower().Contains("status")) ? "Dropdown" : "Autocomplete";
                    strWhereCondition = " ('$" + strColumnName + "$' = '0' OR " + strColumnName + " = '$" + strColumnName + "$')";
                }
                else
                {
                    continue;
                }
                DataRow dr = dtSuggestedReport.NewRow();
                dr["ReportName"] = strReportName;
                dr["ColumnName"] = strColumnName;
                dr["ColumnHeader"] = ColumnHeaderName;
                dr["ControlName"] = strControlName;
                dr["WhereCondition"] = strWhereCondition;
                dr["TempReportId"] = GlobalUtilities.ConvertToInt(dt.Rows[i]["columns_columnsid"]);
                AllControlName += (AllControlName == "") ? strControlName : "," + strControlName;
                AllColumnName += (AllColumnName == "") ? strColumnName : "," + strColumnName;
                AllheaderName += (AllheaderName == "") ? ColumnHeaderName : "," + ColumnHeaderName;
                AllWhereCondition += (AllWhereCondition == "") ? strWhereCondition : " , " + strWhereCondition;
                strSuggestedReport_Name += (strSuggestedReport_Name == "") ? strReportName : "," + strReportName;
                dtSuggestedReport.Rows.Add(dr);
            }
            DataRow d1r = dtSuggestedReport.NewRow();
            d1r["ReportName"] = ModuleName + " Advanced Search";
            d1r["ColumnName"] = AllColumnName;
            d1r["ColumnHeader"] = AllheaderName;
            d1r["ControlName"] = AllControlName;
            d1r["WhereCondition"] = AllWhereCondition;
            d1r["TempReportId"] = "0";
            dtSuggestedReport.Rows.Add(d1r);
            return dtSuggestedReport;
        }
        return null;
    }
    private bool IsReportAlreadyExists()
    {
        StringBuilder ReportName = new StringBuilder();
        bool IsReportExists = false;
         string strBulkReportId = h_BulkreportId.Text;
            Array arrBulkReport = h_BulkreportId.Text.Split(',');
            if (arrBulkReport.Length > 0)
            {
                ReportName.Append("<table border='1' width='500'>");
                for (int i = 0; i < arrBulkReport.Length; i++)
                {
                    int _ReportId = GlobalUtilities.ConvertToInt(arrBulkReport.GetValue(i));
                    string NewReportName = HttpContext.Current.Request.Form.Get("txt_" + _ReportId);
                    if (NewReportName != null && NewReportName != "")
                    {
                        string query = "select * from tbl_report WHERE report_reportname='" + global.CheckInputData(NewReportName) + "'";
                        DataRow dr = DbTable.ExecuteSelectRow(query);
                        if (dr != null)
                        {
                            ReportName.Append("<tr><td class='error'>" + NewReportName + " - Report already exists!</td></tr>");
                            lblMessage.Text = "Report already exists!";
                            // return false;
                            IsReportExists = true;
                            //HttpContext.Current.Request.Form.Set("txt_" + _ReportId, NewReportName);
                        }
                    }
                }
                ReportName.Append("</table>");
                ltAlreadyExistsReportNames.Text = ReportName.ToString();
            }
            if (IsReportExists) ltAlreadyExistsReportNames.Visible = true;
            return IsReportExists;
    }
    protected void btngenerateBulkReport_Click(object sender, EventArgs e)
    {
        if (GlobalUtilities.ConvertToInt(ddlModule.SelectedValue) > 0 && GlobalUtilities.ConvertToInt(ddlMenu.SelectedValue) > 0)
        {
            if (IsReportAlreadyExists()) return;
            string strBulkReportId = h_BulkreportId.Text;
            Array arrBulkReport = h_BulkreportId.Text.Split(',');
            if (arrBulkReport.Length > 0)
            {
                DataTable dtSuggestedReport = (DataTable)ViewState["SuggestedReport"];
                for (int i = 0; i < arrBulkReport.Length; i++)
                {
                    int _ReportId = GlobalUtilities.ConvertToInt(arrBulkReport.GetValue(i));
                    string keyValue = HttpContext.Current.Request.Form.Get("txt_" + _ReportId);
                    string NewReportName = "";
                    string strColumnName = "";
                    string strWhereCondition = "";
                    string strControlName = "";
                    string ColumnHeaderName = "";
                    bool isReportSelected = false;
                    if (keyValue != null)
                    {
                        NewReportName = keyValue;
                    }
                    for (int j = 0; j < dtSuggestedReport.Rows.Count; j++)
                    {
                        int tempReportId = GlobalUtilities.ConvertToInt(dtSuggestedReport.Rows[j]["TempReportId"]);
                        if (tempReportId == _ReportId)
                        {
                            isReportSelected = true;
                            if (NewReportName == "")
                            {
                                NewReportName = GlobalUtilities.ConvertToString(dtSuggestedReport.Rows[j]["ReportName"]);
                            }
                            ColumnHeaderName = GlobalUtilities.ConvertToString(dtSuggestedReport.Rows[j]["ColumnHeader"]);
                            strControlName = GlobalUtilities.ConvertToString(dtSuggestedReport.Rows[j]["ControlName"]);
                            strWhereCondition = GlobalUtilities.ConvertToString(dtSuggestedReport.Rows[j]["WhereCondition"]);
                            strColumnName = GlobalUtilities.ConvertToString(dtSuggestedReport.Rows[j]["ColumnName"]);
                            break;
                        }
                    }
                    if (isReportSelected)
                    {                       
                        Hashtable hstblReport = new Hashtable();
                        hstblReport.Add("reportname", NewReportName);
                        hstblReport.Add("moduleid", ddlModule.SelectedValue);
                        hstblReport.Add("mainmenuid", ddlMenu.SelectedValue);
                        hstblReport.Add("gridtitle", txtGridTitle.Text);
                        hstblReport.Add("iseditable", 1);
                        hstblReport.Add("isdisplaychartbelowgrid", chkIsDisplayChartBelowGrid.Checked);
                        hstblReport.Add("charttype", ddlChartType.SelectedValue);
                        hstblReport.Add("chartheadercolumns", txtChartHeaderColumns.Text);
                        hstblReport.Add("chartcolumns", txtChartColumns.Text);
                        hstblReport.Add("chartcolors", txtChartColors.Text);
                        hstblReport.Add("chartattributes", txtChartAttributes.Text);


                        InsertUpdate obj = new InsertUpdate();
                        int id = obj.InsertData(hstblReport, "tbl_report");
                        int mid = Convert.ToInt32(ddlModule.SelectedValue);

                        //Add Sub Table Data
                        Array arrColumns = strColumnName.Split(',');
                        Array arrControlName = strControlName.Split(',');
                        Array arrWhereValue = strWhereCondition.Split(',');
                        string strFulWhereCondition = strWhereCondition;
                        for (int k = 0; k < arrColumns.Length; k++)
                        {
                            strColumnName = GlobalUtilities.ConvertToString(arrColumns.GetValue(k));
                            strControlName = GlobalUtilities.ConvertToString(arrControlName.GetValue(k));
                            strWhereCondition = GlobalUtilities.ConvertToString(arrWhereValue.GetValue(0));
                            string strOperator = (strControlName.ToLower().Trim().Replace(" ", "") == "daterange") ? "Between" : "Equal to";
                            Hashtable hstbl = new Hashtable();
                            hstbl.Add("reportid", id);
                            hstbl.Add("columnname", strColumnName);
                            hstbl.Add("control", strControlName);
                            hstbl.Add("operator", strOperator);
                            hstbl.Add("andor", (_ReportId == 0) ? "AND" : "OR");
                            hstbl.Add("columnvalue", (_ReportId == 0) ? "" : strWhereCondition);
                            hstbl.Add("betweenfrom", (strControlName == "Date Range") ? "$" + strColumnName + "_from$" : "");
                            hstbl.Add("betweento", (strControlName == "Date Range") ? "$" + strColumnName + "_to$" : "");
                            hstbl.Add("where", strWhereCondition);

                            obj = new InsertUpdate();
                            if (Request.QueryString["rdid"] == null)
                            {
                                obj.InsertData(hstbl, "tbl_reportdetail");
                            }
                        }
                        string strJoinTables = Common.GetJoinTables_Script(GlobalUtilities.ConvertToInt(ddlModule.SelectedValue));
                        Common.GenerateReportPage(GlobalUtilities.ConvertToInt(ddlModule.SelectedValue), id, "(" + strFulWhereCondition.Replace(",", "AND") + ")", strJoinTables, false, false);

                        obj = new InsertUpdate();
                        Hashtable _hstblReport = new Hashtable();
                        _hstblReport.Add("jointables", strJoinTables);
                        _hstblReport.Add("wherequery", strFulWhereCondition.Replace(",", "AND"));
                        obj.UpdateData(_hstblReport, "tbl_report", id);
                        InsertUpdateSubMenu(GlobalUtilities.ConvertToInt(ddlMenu.SelectedValue), NewReportName);
                    }
                }
                
                BindBulkReport();
                lblMessage.Text = "Report generated sucessfully";
                lblMessage.Visible = true;
            }
        }
        else
        {
            lblMessage.Text = "Please select any one module and menu";
            lblMessage.Visible = true;
        }
    }                                              
    protected void btnBulkReportGenerater_Click(object sender, EventArgs e)
    {
        //Response.Redirect("build-report.aspx?isbulkreport=true&uid=" + ddlMenu.SelectedValue + "&mid=" + ddlModule.SelectedValue);
        BindBulkReport();
    }
    private void BindBulkReport()
    {
        int Moduled = GlobalUtilities.ConvertToInt(ddlModule.SelectedValue);
        DataTable dt = getSuggestedReportName(Moduled);
        btngenerateBulkReport.Visible = true;
        BindSuggestedReport(dt, Moduled);
    }
}
