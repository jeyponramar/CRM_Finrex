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

public partial class addreport : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_report");
    DataTable _dttbl = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblTitle.Text = "Build Report";
            GetReportColumns();
            BindQueryBuilder();
            BindColumns();
            BindReportDetail();
            if (txtJoinTables.Text == "")
            {
                BindJoinTables();
            }
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
    private void BindJoinTables()
    {
        InsertUpdate obj = new InsertUpdate();
        DataTable dttblm = obj.ExecuteSelect("select * from tbl_module where module_moduleid=" + Request.QueryString["mid"]);
        string tableName = Convert.ToString(dttblm.Rows[0]["module_tablename"]);
        string joinTables = "";
        InsertUpdate objcolumns = new InsertUpdate();
        DataTable dttblColumns = objcolumns.ExecuteSelect("select * from tbl_columns where columns_subsectionid=0 and columns_submoduleid=0 and columns_control<>'Sub Grid' and columns_control<>'Section' and columns_moduleid=" + Request.QueryString["mid"]);
        for (int i = 1; i < dttblColumns.Rows.Count; i++)
        {
            string columnName = Convert.ToString(dttblColumns.Rows[i]["columns_columnname"]);
            string gridColumnLabel = Convert.ToString(dttblColumns.Rows[i]["columns_gridcolumnname"]);
            int dropdownmoduleid = Convert.ToInt32(dttblColumns.Rows[i]["columns_dropdownmoduleid"]);
            string dropdowncolumnname = Convert.ToString(dttblColumns.Rows[i]["columns_dropdowncolumn"]);
            if (dropdownmoduleid > 0)
            {
                InsertUpdate objm = new InsertUpdate();
                string query = "select * from tbl_module where module_moduleid=" + dropdownmoduleid;
                DataRow dr = objm.ExecuteSelectRow(query);
                if (dr != null)
                {
                    string subTableName = Convert.ToString(dr["module_tablename"]);
                    string rightColumn = subTableName.Replace("tbl_", "");
                    rightColumn = rightColumn + "_" + rightColumn + "id";
                    joinTables += "\nJOIN " + subTableName + " ON " + columnName + "=" + rightColumn;
                }
            }
        }
        txtJoinTables.Text = "SELECT * FROM " + tableName + joinTables;
    }
    private string IsSelected(string ddlval,string selectedVal)
    {
        if (ddlval == selectedVal)
        {
            return " selected";
        }
        return "";
    }
    private void BindQueryBuilder()
    {
        if (Request.QueryString["id"] == null) return;
        string prefix = "@sm_";
        StringBuilder html = new StringBuilder();

        for (int i = 0; i < _dttbl.Rows.Count; i++)
        {
            string id=Convert.ToString(_dttbl.Rows[i]["reportdetail_reportdetailid"]);
            string columnname = Convert.ToString(_dttbl.Rows[i]["reportdetail_columnname"]);
            string control = Convert.ToString(_dttbl.Rows[i]["reportdetail_control"]);
            string stroperator = Convert.ToString(_dttbl.Rows[i]["reportdetail_operator"]);
            string columnvalue = Convert.ToString(_dttbl.Rows[i]["reportdetail_columnvalue"]);
            string constant = Convert.ToString(_dttbl.Rows[i]["reportdetail_constant"]);
            string betweenfrom = Convert.ToString(_dttbl.Rows[i]["reportdetail_betweenfrom"]);
            string betweento = Convert.ToString(_dttbl.Rows[i]["reportdetail_betweento"]);
            string andor = Convert.ToString(_dttbl.Rows[i]["reportdetail_andor"]);
            int rowIndex = i + 1;

            string tr = "<tr class='where-row'><td class='label rowcol'>" +
                    "<input type='text' name='"+prefix+"id-"+rowIndex+"' value='"+id+"' class='hidden'/>" +
                    "<input type='text' value='" + columnname + "' name='" + prefix + "columnname-" + rowIndex +
                    "' class='hidden colname'/>" + columnname + "</td>";
            tr += "<td><select name='" + prefix + "control-" + rowIndex + "' style='width:100px' class='control'>" +
                   "<option value='Default'" + IsSelected("Default", control) + ">Default</option>" +
                   "<option value='Text Box'" + IsSelected("Text Box", control) + ">Text Box</option>" +
                   "<option value='Date Range'" + IsSelected("Date Range", control) + ">Date Range</option>" +
                   "<option value='Dropdown'" + IsSelected("Dropdown", control) + ">Dropdown</option>" +
                   "<option value='Autocomplete'" + IsSelected("Autocomplete", control) + ">Autocomplete</option></select></td>";
            tr += "<td><select name='" + prefix + "operator-" + rowIndex + "' style='width:100px' class='operator'>" +
                      "<option value='Equal to'" + IsSelected("Autocomplete", stroperator) + ">Equal to</option>" +
                      "<option value='NOT Equal to'" + IsSelected("NOT Equal to", stroperator) + ">NOT Equal to</option>" +
                      "<option value='Greater than'" + IsSelected("Greater than", stroperator) + ">Greater than</option>" +
                      "<option value='Less than'" + IsSelected("Less than", stroperator) + ">Less than</option>" +
                      "<option value='LIKE'" + IsSelected("LIKE", stroperator) + ">LIKE</option>" +
                      "<option value='Between'" + IsSelected("Between", stroperator) + ">Between</option>" +
                  "</select></td>";
            tr += "<td><input type='text' value='"+columnvalue+"' name='" + prefix + "columnvalue-" + rowIndex + "' style='width:100px' class='value'/></td>";
            tr += "<td><select name='" + prefix + "constant-" + rowIndex + "' style='width:120px' class='constant'>" +
                        "<option value=''>Select</option>" +
                        "<option value='LoggedInUser'" + IsSelected("LoggedInUser", constant) + ">Logged In User</option>" +
                        "<option value='LoggedInRole'" + IsSelected("LoggedInRole", constant) + ">Logged In Role</option>" +
                        "<option value='Today'" + IsSelected("Today", constant) + ">Today</option>" +
                        "<option value='ThisWeek'" + IsSelected("ThisWeek", constant) + ">This Week</option>" +
                        "<option value='ThisMonth'" + IsSelected("ThisMonth", constant) + ">This Month</option>" +
                        "<option value='ThisYear'" + IsSelected("ThisYear", constant) + ">This Year</option>" +
                    "</select></td>";
            tr += "<td><input type='text' name='" + prefix + "BetweenFrom-" + rowIndex + "' style='width:100px' class='from' value='"+betweenfrom+"'></td>";
            tr += "<td><input type='text' name='" + prefix + "BetweenTo-" + rowIndex + "' style='width:100px' class='to' value='" + betweento + "'></td>";
            tr += "<td><select name='" + prefix + "AndOr-" + rowIndex + "' class='andor'>"+
                   "<option value='OR'" + IsSelected("OR", andor) + ">OR</option><option value='AND'" + IsSelected("AND", andor) + ">AND</option></select>";
            tr += "<td><img src='img/close-red.png' class='delete hand'/></td>";
            tr += "</tr>";
            html.Append(tr);
        }
        h_rowIndex.Text = Convert.ToString(_dttbl.Rows.Count + 1);
        ltQueryBuilder.Text = html.ToString();
    }
    private void BindHtml()
    {
        string script = "$(document).ready(function() {" +
            "$(\".column\").change(function() {" +
            "if ($(this).val() == \"0\") return;"+
            "var tr;"+
            "var column = $(this).find(\"option:selected\").text();"+
            "var rowIndex = parseInt($(\".rowindex\").val());"+
            "$(\".rowindex\").val(rowIndex + 1);"+
            "var prefix = \"@sm_\";"+
            "tr = \"<tr class='where-row'><td class='label rowcol'><input type='text' value='\" + $(this).val() + \"' id='\" + prefix + \"txtcolumnid-\" + rowIndex + \"' class='hidden columnid'/>\" + column + \"</td>\";"+
            "tr += \"<td><select id='\" + prefix + \"ddlcontrol-\" + rowIndex + \"' style='width:100px' class='control'>\" +"+
                   "\"<option value='Default'>Default</option>\" +" +
                   "\"<option value='Text Box'>Text Box</option>\" +" +
                   "\"<option value='Date Range'>Date Range</option>\" +" +
                   "\"<option value='Dropdown'>Dropdown</option>\" +" +
                   "\"<option value='Date Range'>Date Range</option>\" +" +
                   "\"<option value='Autocomplete'>Autocomplete</option></select></td>\";" +
            "tr += \"<td><select id='\" + prefix + \"ddloperator-\" + rowIndex + \"' style='width:100px' class='operator'>\" +"+
                      "\"<option value='Equal to'>Equal to</option>\" +" +
                      "\"<option value='NOT Equal to'>NOT Equal to</option>\" +" +
                      "\"<option value='Greater than'>Greater than</option>\" +" +
                      "\"<option value='Less than'>Less than</option>\" +" +
                      "\"<option value='LIKE'>LIKE</option>\" +" +
                      "\"<option value='Between'>Between</option>\" +" +
                  "\"</select></td>\";"+
            "tr += \"<td><input type='text' value='' id='\" + prefix + \"txtcolumnvalue-\" + rowIndex + \"' style='width:100px' class='value'/></td>\";"+
            "tr += \"<td><select id='\" + prefix + \"ddlconstant-\" + rowIndex + \"' style='width:120px' class='constant'>\" +"+
                        "\"<option value=''>Select</option>\" +"+
                        "\"<option value='LoggedInUser'>Logged In User</option>\" +"+
                        "\"<option value='LoggedInRole'>Logged In Role</option>\" +"+
                        "\"<option value='Today'>Today</option>\" +"+
                        "\"<option value='ThisWeek'>This Week</option>\" +"+
                        "\"<option value='ThisMonth'>This Month</option>\" +"+
                        "\"<option value='ThisYear'>This Year</option>\" +"+
                    "\"</select></td>\";"+
            "tr += \"<td><input type='text' id='\" + prefix + \"txtBetweenFrom-\" + rowIndex + \"' style='width:100px' class='from'></td>\";"+
            "tr += \"<td><input type='text' id='\" + prefix + \"txtBetweenTo-\" + rowIndex + \"' style='width:100px' class='to'></td>\";"+
            "tr += \"<td><select id='\" + prefix + \"ddlAndOr-\" + rowIndex + \"' class='andor'><option value='OR'>OR</option><option value='AND'>AND</option></select>\";"+
            "tr += \"<td><img src='img/close-red.png' class='delete hand'/></td>\";"+
            "tr += \"</tr>\";"+
            "$(\".submodule\").append(tr);"+
            "buildQuery();"+
        "});});";
        ClientScript.RegisterClientScriptBlock(typeof(Page), "querybuilder", script);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Common.CheckDemoVersion();
        string where = txtWhere.Text.Trim();
        if (where.EndsWith(" OR") || where.EndsWith(" AND"))
        {
            where = where.Substring(0, where.Length - 3);
        }
        txtWhere.Text = where.Replace("<","&lt;").Replace(">","&gt;");
        int id = Convert.ToInt32(Request.QueryString["id"]);
        HtmlInputHidden setting = new HtmlInputHidden();
        setting.Value = "smprefix~@sm_~m~reportdetail";
        //gblData.SaveSubTable(id, "reportdetail_reportid", "tbl_reportdetail", "@sm_");
        gblData.SaveSubTable(id, gblData, setting);

        Hashtable hstbl = new Hashtable();
        hstbl.Add("jointables", txtJoinTables.Text);
        hstbl.Add("wherequery", txtWhere.Text);
        InsertUpdate obj = new InsertUpdate();
        obj.UpdateData(hstbl, "tbl_report","report_reportid",Convert.ToInt32(Request.QueryString["id"]));
        GetReportColumns();
        BindQueryBuilder();
        GenerateReportPage();
    }
    private void GetReportColumns()
    {
        string query = "select * from tbl_reportdetail where reportdetail_reportid=" + Request.QueryString["id"];
        InsertUpdate obj = new InsertUpdate();
        _dttbl = obj.ExecuteSelect(query);
    }
    private void GenerateReportPage()
    {
        StringBuilder controls = new StringBuilder();
        StringBuilder fillDropdwn = new StringBuilder();
        controls.Append("\n\t\t\t\t<asp:PlaceHolder ID=\"plSearch\" runat=\"server\">");
        controls.Append("\n\t\t\t\t<table>");
        int controlCount = 0;
        bool controlExists = false;
        for (int i = 0; i < _dttbl.Rows.Count; i++)
        {
            string id = Convert.ToString(_dttbl.Rows[i]["reportdetail_reportdetailid"]);
            string columnname = Convert.ToString(_dttbl.Rows[i]["reportdetail_columnname"]);
            string control = Convert.ToString(_dttbl.Rows[i]["reportdetail_control"]);
            string stroperator = Convert.ToString(_dttbl.Rows[i]["reportdetail_operator"]);
            string columnvalue = Convert.ToString(_dttbl.Rows[i]["reportdetail_columnvalue"]);
            string constant = Convert.ToString(_dttbl.Rows[i]["reportdetail_constant"]);
            string betweenfrom = Convert.ToString(_dttbl.Rows[i]["reportdetail_betweenfrom"]);
            string betweento = Convert.ToString(_dttbl.Rows[i]["reportdetail_betweento"]);
            string andor = Convert.ToString(_dttbl.Rows[i]["reportdetail_andor"]);
            bool iscontrol = false;
            string controlhtml = "";
            string query = "";
            string btnReport = "";
            if (i == _dttbl.Rows.Count - 1)
            {
                btnReport = "\n\t\t\t\t\t\t<asp:Button ID=\"btnReport\" runat=\"server\" Text=\"Report\" CssClass=\"button\" OnClick=\"btnReport_Click\" />";
            }
            InsertUpdate objc = new InsertUpdate();
            query = "select * from tbl_columns where columns_columnname='" + columnname + "'";
            DataRow drc = objc.ExecuteSelectRow(query);
            string label = Convert.ToString(drc["columns_lbl"]);
            int dropdownmoduleid = Convert.ToInt32(drc["columns_dropdownmoduleid"]);
            if (controlCount % 2 == 0)
            {
                if (controlCount != 0) controlhtml += "\n\t\t\t\t\t</tr>";
                controlhtml += "\n\t\t\t\t\t<tr>";
            }
            if (control == "Dropdown")
            {
                controlExists = true;
                string dropdowncolumnName = Convert.ToString(drc["columns_dropdowncolumn"]);
                controlhtml += "\n\t\t\t\t\t<td>" + label + "</td><td><asp:DropDownList ID=\"" + columnname + "\" runat=\"server\"></asp:DropDownList>" + btnReport + "</td>";
                fillDropdwn.Append("\n\t\t\tgblData.FillDropdown(" + columnname + ", \"tbl_" + dropdowncolumnName.Substring(0, dropdowncolumnName.IndexOf("_")) + "\", \"" + dropdowncolumnName + "\", \"\");");
                iscontrol = true;
                controlCount++;
            }
            else if (control == "Date Range")
            {
                controlhtml += "\n\t\t\t\t\t<td>" + label + "</td>";
                controlhtml += "\n\t\t\t\t\t<td>From : <asp:TextBox ID=\"" + betweenfrom.Replace("$", "") + "\" CssClass=\"datepicker\" runat=\"server\" Format=\"Date\"/>" +
                                "\n\t\t\t\t\t\tTo : <asp:TextBox ID=\"" + betweento.Replace("$", "") + "\" CssClass=\"datepicker\" runat=\"server\" Format=\"Date\"/>" + btnReport + "</td>";
                iscontrol = true;
                controlExists = true;
                controlCount++;
            }
            else if (control == "Text Box")
            {
                controlExists = true;
                iscontrol = true;
                controlCount++;
            }
            else if (control == "Autocomplete")
            {
                string txtac = " ac txtac";
                string hdnac = " hdnac";
                controlExists = true;
                string cn = GlobalUtilities.GetDropdownColumn(dropdownmoduleid);
                if (cn.Contains("_")) cn = cn.Substring(cn.IndexOf("_") + 1);
                string autoCompletemodule = GlobalUtilities.GetModuleName(dropdownmoduleid).ToLower().Replace(" ", "");
                string modulesetting = "m=\"" + autoCompletemodule + "\" cn=\"" + cn + "\" ";
                controlhtml += "\n\t\t\t\t\t<td>" + label + "</td>";
                controlhtml += "\n\t\t\t\t\t<td><asp:TextBox ID=\"" + columnname + "\" runat=\"server\" " + modulesetting + "CssClass=\"textbox" + txtac + "\" Include=\"0\"></asp:TextBox>";
                controlhtml += "\n\t\t\t\t\t\t<asp:TextBox id=\"txt" + columnname + "\" Text=\"0\" runat=\"server\" class=\"" + hdnac + "\" Format=\"Int\" />" + btnReport + "</td>";
                iscontrol = true;
                controlCount++;
            }
            if (iscontrol)
            {
                controls.Append("\n\t\t\t\t\t" + controlhtml);
            }
        }
        if (controls.ToString().EndsWith("</tr>"))
        {
            controls.Append("\n\t\t\t\t\t</tr");
        }
        //controls.Append("\n\t\t\t\t<tr>");
        //controls.Append("\n\t\t\t\t\t<td colspan=4><asp:Button ID=\"btnReport\" runat=\"server\" Text=\"Report\" CssClass=\"button\" OnClick=\"btnReport_Click\" /></td>");
        controls.Append("\n\t\t\t\t</tr>");
        controls.Append("\n\t\t\t\t</table>");
        controls.Append("\n\t\t\t\t</asp:PlaceHolder>");
        if (!controlExists)
        {
            controls = new StringBuilder();
            controls.Append("<asp:PlaceHolder ID=\"plSearch\" runat=\"server\"></asp:PlaceHolder>");
        }
        InsertUpdate obj = new InsertUpdate();
        DataRow dr = obj.ExecuteSelectRow("select * from tbl_report where report_reportid=" + Request.QueryString["id"]);
        string reportName = Convert.ToString(dr["report_reportname"]);
        //write design page
        string projectName = CustomSession.Session("S_ProjectName");
        string projectPath = AppConstants.ProjectPath + "/" + projectName;
        string className = reportName.ToLower().Replace(" ", "");
        string designPath = projectPath + "/report/" + className + ".aspx";
        string code = GlobalUtilities.ReadFile(Server.MapPath("template/design/report.aspx.txt"));
        code = code.Replace("$CLASSNAME$", className);
        code = code.Replace("$TITLE$", reportName);
        code = code.Replace("$SEARCH$", controls.ToString());
        GlobalUtilities.WriteFile(designPath, code);

        //write code page
        string codePath = projectPath + "/report/" + className + ".aspx.cs";
        code = GlobalUtilities.ReadFile(Server.MapPath("template/design/report.aspx.cs.txt"));
        code = code.Replace("$CLASSNAME$", className);
        code = code.Replace("$FILLDROPDOWN$", fillDropdwn.ToString());
        GlobalUtilities.WriteFile(codePath, code);

        string module = reportName.Replace(" ", "").ToLower();
        string where = txtWhere.Text.Trim();
        //create xml file
        string filePath = projectPath + "/xml/report/" + module + ".xml";

        DataTable dttblm = obj.ExecuteSelect("select * from tbl_module where module_moduleid=" + Request.QueryString["mid"]);
        string tableName = Convert.ToString(dttblm.Rows[0]["module_tablename"]);
        module = Convert.ToString(dttblm.Rows[0]["module_modulename"]);
        string idcolumn = tableName.Replace("tbl_", "");
        idcolumn = idcolumn + "_" + idcolumn + "id";

        DataTable dttblColumns = new DataTable();
        StringBuilder Columns = new StringBuilder();
        StringBuilder ColumnLabels = new StringBuilder();
        StringBuilder gridColumnsxml = new StringBuilder();
        InsertUpdate objcolumns = new InsertUpdate();
        string query1 = "select * from tbl_columns where columns_subsectionid=0 and columns_submoduleid=0 and columns_control<>'Sub Grid' and columns_control<>'Section' and columns_moduleid=" + Request.QueryString["mid"];
        dttblColumns = objcolumns.ExecuteSelect(query1);
        int width = 100 / dttblColumns.Rows.Count;

        for (int i = 1; i < dttblColumns.Rows.Count; i++)
        {
            string columnName = Convert.ToString(dttblColumns.Rows[i]["columns_columnname"]);
            string control = Convert.ToString(dttblColumns.Rows[i]["columns_control"]);
            string gridColumnLabel = Convert.ToString(dttblColumns.Rows[i]["columns_gridcolumnname"]);
            int dropdownmoduleid = Convert.ToInt32(dttblColumns.Rows[i]["columns_dropdownmoduleid"]);
            string dropdowncolumnname = Convert.ToString(dttblColumns.Rows[i]["columns_dropdowncolumn"]);
            string dataFormat = "";
            if (control == "Date")
            {
                dataFormat = "Date";
            }
            else if (control == "Amount")
            {
                dataFormat = "Amount";
            }
            if (dropdownmoduleid > 0)
            {
                columnName = dropdowncolumnname;
            }
            if (i == 1)
            {
                Columns.Append(columnName);
                ColumnLabels.Append(gridColumnLabel);
            }
            else
            {
                Columns.Append("," + columnName);
                ColumnLabels.Append("," + gridColumnLabel);
            }
            gridColumnsxml.Append("\n\t\t\t<column><name>" + columnName + "</name><headertext>" +
                gridColumnLabel + "</headertext><row>1</row><width>" + width + "%</width>" +
                "<format>" + dataFormat + "</format></column>");
        }
        if (chkResetGridSetting.Checked || !System.IO.File.Exists(filePath))
        {
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "\n\t<setting>" +
                      "\n\t\t<title>" + module + "</title>  " +
                      "\n\t\t<module>" + module.Replace(" ","") + "</module>" +
                      "\n\t\t<table>" + tableName + "</table>" +
                      "\n\t\t<isreport>true</isreport>" +
                      "\n\t\t<enablepaging>true</enablepaging>" +
                      "\n\t\t<pagesize>20</pagesize>" +
                      "\n\t\t<toprecords>0</toprecords>" +
                      "\n\t\t<enablesorting>true</enablesorting>" +
                      "\n\t\t<enablesearch>true</enablesearch>" +
                      "\n\t\t<defaultsort>" + idcolumn + " desc</defaultsort>" +
                      "\n\t\t<columns>" + Columns.ToString() + "</columns>" +
                      "\n\t\t<columnlabels>" + ColumnLabels.ToString() + "</columnlabels>" +
                      "\n\t\t<gridtype>grid</gridtype>" +
                      "\n\t\t<gridcolumn>" +
                        gridColumnsxml.ToString() +
                      "\n\t\t</gridcolumn>" +
                      "\n\t\t<hiddencolumns>" + idcolumn + "</hiddencolumns>" +
                      "\n\t\t<query>" + txtJoinTables.Text + " $Search$</query>" +
                      "\n\t\t<where>" + where + "</where>" +
                    "\n\t</setting>";

            GlobalUtilities.WriteFile(filePath, xml);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode node = doc.SelectSingleNode("/setting/query");
            if (node != null)
                node.InnerText = txtJoinTables.Text;

            node = doc.SelectSingleNode("/setting/where");
            if (node != null)
                node.InnerText = txtWhere.Text;

            if (chkResetColumns.Checked)
            {
                node = doc.SelectSingleNode("/setting/columns");
                if (node != null)
                    node.InnerText = Columns.ToString();

                node = doc.SelectSingleNode("/setting/columnlabels");
                if (node != null)
                    node.InnerText = ColumnLabels.ToString();
            }

            doc.Save(filePath);
        }
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
}

