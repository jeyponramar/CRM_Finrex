using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.Text;
using System.Xml;
using System.IO;

public partial class CP_convert : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_module", "module_moduleid");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateModule();
            PopulateToModule();
            PopulateFromChildModule();
            PopulateConvertSetting();
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblTitle.Text + "')</script>");
    }
    private void PopulateModule()
    {
        //DataRow drColumn = DbTable.GetOneRow("tbl_columns", Common.GetQueryStringValue("id"));
        //lblName.Text = GlobalUtilities.ConvertToString(drColumn["columns_lbl"]);

        DataRow drModule = DbTable.GetOneRow("tbl_module", Common.GetQueryStringValue("mid"));
        lblModuleName.Text = GlobalUtilities.ConvertToString(drModule["module_modulename"]);
    }
    private void PopulateToModule()
    {
        gblData.FillDropdown(ddlToModule, "tbl_module", "module_modulename", "module_moduleid<>" + Common.GetQueryStringValue("mid"));
    }
    private void PopulateFromChildModule()
    {
        gblData.FillDropdown(ddlFromChildModule, "tbl_columns", "columns_lbl",
                             "columns_control='Sub Grid' AND columns_moduleid=" + Common.GetQueryStringValue("mid"));

    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        BindMapping();
        BindChildMapping();
    }
    private void BindMapping()
    {
        StringBuilder html = new StringBuilder();
        string fromTableName = "tbl_" + lblModuleName.Text.ToLower().Replace(" ", "");
        DataRow drToModule = DbTable.GetOneRow("tbl_module", GlobalUtilities.ConvertToInt(ddlToModule.SelectedValue));
        string toModule = Convert.ToString(drToModule["module_modulename"]).ToLower().Replace(" ", "");
        string toTableName = "tbl_" + toModule;

        html.Append("<table class='repeater' width='100%'><tr class='repeater-header'><td>To Column</td><td>Value</td></tr>");
        string query = "select * from " + toTableName + " where 1=2";
        DataTable dttblTo = DbTable.ExecuteSelect(query);
        query = "select * from " + fromTableName + " where 1=2";
        DataTable dttblFrom = DbTable.ExecuteSelect(query);
        int count = 1;
        XmlDocument doc = new XmlDocument();
        bool isXmlExists = false;
        string xmlPath = Server.MapPath("~/xml/convert/" + lblName.Text + ".xml");
        XmlNodeList xmlColumns = null;
        XmlNodeList xmlValues = null;
        if (File.Exists(xmlPath)) isXmlExists = true;
        if (isXmlExists)
        {
            doc.Load(xmlPath);
            xmlColumns = doc.SelectNodes("/setting/mapping/name");
            xmlValues = doc.SelectNodes("/setting/mapping/value");
            txtStatusColumn.Text = doc.SelectSingleNode("/setting/statuscolumn").InnerText;
            txtStatusValue.Text = doc.SelectSingleNode("/setting/statusvalue").InnerText;
        }
        for (int i = 0; i < dttblTo.Columns.Count; i++)
        {
            string columnName = dttblTo.Columns[i].ColumnName;
            int index = columnName.IndexOf('_');
            string shortColumnName = columnName.Substring(index + 1).ToLower();

            if (shortColumnName == "createddate" || shortColumnName == "modifieddate" || shortColumnName == "createdby" || shortColumnName == "modifiedby" ||
                shortColumnName == toModule + "id")
            {
            }
            else
            {
                string rowCss = "repeater-row";
                if (count % 2 == 0) rowCss = "repeater-alt";
                string val = "";
                if (isXmlExists)
                {
                    for (int j = 0; j < xmlColumns.Count; j++)
                    {
                        if (xmlColumns[j].InnerText == columnName)
                        {
                            val = xmlValues[j].InnerText;
                            break;
                        }
                    }
                }
                if (val == "")
                {
                    for (int j = 0; j < dttblFrom.Columns.Count; j++)
                    {
                        string fromColumn = dttblFrom.Columns[j].ColumnName;
                        if (shortColumnName == Common.GetColumnName(fromColumn))
                        {
                            val = fromColumn;
                            break;
                        }
                    }
                }
                if (val == "")
                {
                    //set default value
                    if (val == "") val = GetDetaultVal(columnName);
                }
                html.Append("<tr class='" + rowCss + "'><td>" + columnName + "</td><td>" +
                            "<input type='text' value='" + val + "' name='colval_" + columnName + "' style='width:400px;'/></td></tr>");
                count++;
            }
        }
        html.Append("</table>");
        ltColumnMapping.Text = html.ToString();
    }
    private string GetDetaultVal(string columnName)
    {
        string val = "";
        string query = "select * from information_schema.COLUMNS WHERE COLUMN_NAME='" + columnName + "'";
        DataRow drColumn = DbTable.ExecuteSelectRow(query);
        if (drColumn != null)
        {
            string datatype = Convert.ToString(drColumn["DATA_TYPE"]);
            if (datatype == "int" || datatype == "bit" || datatype == "numeric")
            {
                val = "0";
            }
            else if (datatype == "datetime")
            {
                val = "NULL";
            }
        }
        return val;
    }
    private void BindChildMapping()
    {
        if (ddlFromChildModule.SelectedIndex == 0 | ddlToChildModule.SelectedIndex == 0) return;
        StringBuilder html = new StringBuilder();
        string fromTableName = "tbl_" + ddlFromChildModule.SelectedItem.Text.ToLower().Replace(" ", "");
        string toModule = ddlToChildModule.SelectedItem.Text.ToLower().Replace(" ", "");
        string toTableName = "tbl_" + toModule;

        html.Append("<table class='repeater' width='100%'><tr class='repeater-header'><td>To Column</td><td>Value</td></tr>");
        string query = "select * from " + toTableName + " where 1=2";
        DataTable dttblTo = DbTable.ExecuteSelect(query);
        query = "select * from " + fromTableName + " where 1=2";
        DataTable dttblFrom = DbTable.ExecuteSelect(query);
        int count = 1;
        XmlDocument doc = new XmlDocument();
        bool isXmlExists = false;
        string xmlPath = Server.MapPath("~/xml/convert/" + lblName.Text + "_child.xml");
        XmlNodeList xmlColumns = null;
        XmlNodeList xmlValues = null;
        if (File.Exists(xmlPath)) isXmlExists = true;
        if (isXmlExists)
        {
            doc.Load(xmlPath);
            xmlColumns = doc.SelectNodes("/setting/mapping/name");
            xmlValues = doc.SelectNodes("/setting/mapping/value");
        }
        string masterColumn = ddlToModule.SelectedItem.Text.Replace(" ", "").ToLower() + "id";
        for (int i = 0; i < dttblTo.Columns.Count; i++)
        {
            string columnName = dttblTo.Columns[i].ColumnName;
            int index = columnName.IndexOf('_');
            string shortColumnName = columnName.Substring(index + 1).ToLower();

            if (shortColumnName == "createddate" || shortColumnName == "modifieddate" || shortColumnName == "createdby" || shortColumnName == "modifiedby" ||
                shortColumnName == toModule + "id" || shortColumnName == masterColumn)
            {
            }
            else
            {
                string rowCss = "repeater-row";
                if (count % 2 == 0) rowCss = "repeater-alt";
                string val = "";
                if (isXmlExists)
                {
                    for (int j = 0; j < xmlColumns.Count; j++)
                    {
                        if (xmlColumns[j].InnerText == columnName)
                        {
                            val = xmlValues[j].InnerText;
                            break;
                        }
                    }
                }
                if (val == "")
                {
                    for (int j = 0; j < dttblFrom.Columns.Count; j++)
                    {
                        string fromColumn = dttblFrom.Columns[j].ColumnName;
                        if (shortColumnName == Common.GetColumnName(fromColumn))
                        {
                            val = fromColumn;
                            break;
                        }
                    }
                    //set default value
                    if (val == "") val = GetDetaultVal(columnName);
                }
                html.Append("<tr class='" + rowCss + "'><td>" + columnName + "</td><td>" +
                            "<input type='text' value='" + val + "' name='colvalchild_" + columnName + "' style='width:400px;'/></td></tr>");
                count++;
            }
        }
        html.Append("</table>");
        ltChildColumnMapping.Text = html.ToString();
    }
    
    private void SaveMapping()
    {
        StringBuilder xml = new StringBuilder();
        xml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n\t<setting name='" + lblName.Text + "'>");
        xml.Append("\n\t\t<fromtable>tbl_" + lblModuleName.Text.Replace(" ", "").ToLower() + "</fromtable>");
        xml.Append("\n\t\t<totable>tbl_" + ddlToModule.SelectedItem.Text.Replace(" ", "").ToLower() + "</totable>");
        xml.Append("\n\t\t<frommoduleid>" + Request.QueryString["mid"] + "</frommoduleid>");
        xml.Append("\n\t\t<tomoduleid>" + ddlToModule.SelectedValue + "</tomoduleid>");
        xml.Append("\n\t\t<statuscolumn>" + txtStatusColumn.Text + "</statuscolumn>");
        xml.Append("\n\t\t<statusvalue>" + txtStatusValue.Text + "</statusvalue>");
        for (int i = 0; i < Request.Form.Keys.Count; i++)
        {
            string key = Request.Form.Keys[i];
            if (key.StartsWith("colval_"))
            {
                string columnName = Common.GetColumnName(key);
                string val = Request.Form[key];
                xml.Append("\n\t\t<mapping><name>" + columnName + "</name><value>" + val + "</value></mapping>");
            }
        }
        xml.Append("\n\t</setting>");
        GlobalUtilities.WriteFile(Server.MapPath("~/xml/convert/" + lblName.Text + ".xml"), xml.ToString());
    }
    private void SaveChildMapping()
    {
        StringBuilder xml = new StringBuilder();
        xml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n\t<setting name='" + lblName.Text + "_child'>");
        xml.Append("\n\t\t<fromtable>tbl_" + ddlFromChildModule.SelectedItem.Text.Replace(" ", "").ToLower() + "</fromtable>");
        xml.Append("\n\t\t<totable>tbl_" + ddlToChildModule.SelectedItem.Text.Replace(" ", "").ToLower() + "</totable>");
        xml.Append("\n\t\t<frommoduleid>" + ddlFromChildModule.SelectedValue + "</frommoduleid>");
        xml.Append("\n\t\t<tomoduleid>" + ddlToChildModule.SelectedValue + "</tomoduleid>");
        for (int i = 0; i < Request.Form.Keys.Count; i++)
        {
            string key = Request.Form.Keys[i];
            if (key.StartsWith("colvalchild_"))
            {
                string columnName = Common.GetColumnName(key);
                string val = Request.Form[key];
                xml.Append("\n\t\t<mapping><name>" + columnName + "</name><value>" + val + "</value></mapping>");
            }
        }
        xml.Append("\n\t</setting>");
        GlobalUtilities.WriteFile(Server.MapPath("~/xml/convert/" + lblName.Text + "_child.xml"), xml.ToString());
    }
    protected void ddlToModule_Changed(object sender, EventArgs e)
    {
        gblData.FillDropdown(ddlToChildModule, "tbl_columns", "columns_lbl",
                                     "columns_control='Sub Grid' AND columns_moduleid=" + ddlToModule.SelectedValue);
        lblName.Text = "Convert" + lblModuleName.Text.Replace(" ", "") + "To" + ddlToModule.SelectedItem.Text.Replace(" ","");

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveMapping();
        SaveChildMapping();
        UpdateColumnSetting();
        lblMessage.Text = "Convert setting generated successfully!";
    }
    private void UpdateColumnSetting()
    {
        string setting = DbTable.GetOneColumnData("tbl_columns", "columns_settings", Common.GetQueryStringValue("id"));
        if (!setting.Contains("ConvertModule"))
        {
            if (setting == "")
            {
                setting = "ConvertModule=" + lblName.Text;
            }
            else
            {
                setting += "\n\rConvertModule=" + lblName.Text;
            }
        }
        string query = "update tbl_columns set columns_settings='" + setting + "' WHERE columns_columnsid=" + Common.GetQueryStringValue("id");
        DbTable.ExecuteQuery(query);
    }
    private void PopulateConvertSetting()
    {
        string setting = DbTable.GetOneColumnData("tbl_columns", "columns_settings", Common.GetQueryStringValue("id"));
        if (!setting.Contains("ConvertModule"))return;
        CP cp = new CP();
        string convertModule = cp.GetSetting(setting, "ConvertModule");
        if (convertModule == "") return;

        XmlDocument doc = new XmlDocument();
        string xmlPath = HttpContext.Current.Server.MapPath("~/xml/convert/" + convertModule + ".xml");
        if (!File.Exists(xmlPath)) return;
        doc.Load(xmlPath);
        int fromModuleId = GlobalUtilities.ConvertToInt(doc.SelectSingleNode("/setting/frommoduleid").InnerText);
        int toModuleId = GlobalUtilities.ConvertToInt(doc.SelectSingleNode("/setting/tomoduleid").InnerText);
        ddlToModule.SelectedValue = toModuleId.ToString();
        gblData.FillDropdown(ddlToChildModule, "tbl_columns", "columns_lbl",
                                     "columns_control='Sub Grid' AND columns_moduleid=" + ddlToModule.SelectedValue);
        lblName.Text = "Convert" + lblModuleName.Text.Replace(" ", "") + "To" + ddlToModule.SelectedItem.Text;

        BindMapping();

        //for child    
        xmlPath = HttpContext.Current.Server.MapPath("~/xml/convert/" + convertModule + "_child.xml");
        if (!File.Exists(xmlPath)) return;
        doc.Load(xmlPath);
        int fromModuleId_Child = GlobalUtilities.ConvertToInt(doc.SelectSingleNode("/setting/frommoduleid").InnerText);
        int toModuleId_Child = GlobalUtilities.ConvertToInt(doc.SelectSingleNode("/setting/tomoduleid").InnerText);
        ddlFromChildModule.SelectedValue = fromModuleId_Child.ToString();
        ddlToChildModule.SelectedValue = toModuleId_Child.ToString();
        BindChildMapping();
        
    }
}
