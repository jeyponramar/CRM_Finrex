using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;
using System.Xml;
using System.IO;

public partial class CP_config_mobile : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gblData.FillDropdown(ddlModule, "tbl_module", "module_modulename", "module_moduleid", "", "module_modulename");
            gblData.FillDropdown(ddlReport, "tbl_report", "report_reportname", "report_reportid", "", "report_reportname");
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindSetting();   
    }
    private void BindSetting()
    {
        StringBuilder html = new StringBuilder();
        XmlDocument doc = new XmlDocument();
        string module = ddlModule.SelectedItem.Text.Replace(" ", "");
        string xmlFile = Server.MapPath("~/xml/view/" + module) + ".xml";
        if (ddlReport.SelectedIndex > 0)
        {
            module = ddlReport.SelectedItem.Text.Replace(" ", "");
            xmlFile = Server.MapPath("~/xml/report/" + module) + ".xml";
        }
        
        if (!File.Exists(xmlFile))
        {
            ltColumns.Text = "";
            return;
        }
        doc.Load(xmlFile);
        html.Append("<table><tr class='repeater-header'><td>Column</td><td>Setting</td><td>Is Mobile</td><td>Is Single Column</td></tr>");

        //update the settings
        XmlNodeList xmlOldColumns = doc.SelectNodes("/setting/gridcolumn/column");
        if (xmlOldColumns != null)
        {
            for (int i = 0; i < xmlOldColumns.Count; i++)
            {
                XmlNode xmlNode = xmlOldColumns[i].SelectSingleNode("name");
                string columnName = xmlNode.InnerText;
                string mobSetting = "";
                XmlNode xmlCol = xmlOldColumns[i];
                for (int j = 0; j < xmlCol.Attributes.Count; j++)
                {
                    string attr = xmlCol.Attributes[j].Name.ToLower();
                    string val = xmlCol.Attributes[j].Value;
                    if (attr == "ismobile")
                    {
                        mobSetting += " ismobile=\"true\"";
                    }
                    else if (attr == "colspan")
                    {
                        mobSetting += " colspan=\"" + val + "\"";
                    }
                    if (attr == "class")
                    {
                        mobSetting += " class=\"" + val + "\"";
                    }
                }
                mobSetting = mobSetting.Trim();
                string qry = "update tbl_columns set columns_mobilesetting='" + mobSetting + "' WHERE columns_columnname='" + columnName + "'";
                DbTable.ExecuteQuery(qry);

                string strIsMobile = "";
                if (mobSetting != "")
                {
                    strIsMobile = " checked";
                }
                html.Append("<tr><td>" + columnName + "</td><td><input style='width:300px' type='text' name='txtcol_" + columnName + "' class='txtcol' value='" + mobSetting + "'/></td>" +
                                "<td><input type='checkbox'" + strIsMobile + " name='chkcol_" + columnName + "' class='chkcol'/></td>" +
                                "<td><input type='checkbox' name='chkissingle_" + columnName + "' class='chkissingle'/></td>" +
                                "</tr>");
            }
        }
        ltColumns.Text = html.ToString();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        XmlDocument doc = new XmlDocument();
        string module = ddlModule.SelectedItem.Text.Replace(" ", "");
        string xmlFile = Server.MapPath("~/xml/view/" + module) + ".xml";
        if (ddlReport.SelectedIndex > 0)
        {
            module = ddlReport.SelectedItem.Text.Replace(" ", "");
            xmlFile = Server.MapPath("~/xml/report/" + module) + ".xml";
        }
        doc.Load(xmlFile);
        //update the settings
        XmlNodeList xmlOldColumns = doc.SelectNodes("/setting/gridcolumn/column");
        bool isMobile = false;
        if (xmlOldColumns != null)
        {
            for (int i = 0; i < xmlOldColumns.Count; i++)
            {
                XmlNode xmlNode = xmlOldColumns[i].SelectSingleNode("name");
                string columnName = xmlNode.InnerText;
                string setting = Request.Form["txtcol_" + columnName].Trim();
                XmlNode xmlCol = xmlOldColumns[i];
                xmlCol.Attributes.RemoveAll();
                if (setting != "")
                {
                    isMobile = true;
                    Array arrSetting = setting.Split(' ');
                    for (int j = 0; j < arrSetting.Length; j++)
                    {
                        Array arr = Convert.ToString(arrSetting.GetValue(j)).Split('=');
                        string name = Convert.ToString(arr.GetValue(0));
                        string val = Convert.ToString(arr.GetValue(1)).Replace("\"", "");
                        XmlAttribute attr = doc.CreateAttribute(name);
                        attr.Value = val;
                        xmlCol.Attributes.Append(attr);
                    }
                }
            }
            XmlNode xmlIsMobile = doc.SelectSingleNode("/setting/ismobile");
            if (xmlIsMobile == null)
            {
                XmlNode xmlNewIsMobile = doc.CreateNode(XmlNodeType.Element, "ismobile", null);
                xmlNewIsMobile.InnerText = isMobile.ToString();
                XmlNode xmlSetting = doc.SelectSingleNode("/setting");
                xmlSetting.AppendChild(xmlNewIsMobile);
            }
            else
            {
                xmlIsMobile.InnerText = isMobile.ToString();
            }

            doc.Save(xmlFile);
            BindSetting();
        }
        
        lblMessage.Text = "Mobile settings saved successfully!";
    }
    
}
