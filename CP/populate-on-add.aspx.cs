using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Data;
using WebComponent;
using System.Collections;

public partial class CP_populate_on_add : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateSettingColumns();
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DataRow drModule = DbTable.GetOneRow("tbl_module", Common.GetQueryStringValue("mid"));
        string moduleName = Convert.ToString(drModule["module_modulename"]).Replace(" ", "").ToLower();
        string xmlPath = Server.MapPath("~/xml/add/" + moduleName + ".xml");
        string xml = "";
        if (!File.Exists(xmlPath))
        {
            xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                         "<setting>" +
                         "\n</setting>";
            GlobalUtilities.WriteFile(xmlPath, xml);
        }
        string popModule = txtQueryStringColumn.Text;
        if (popModule.EndsWith("id")) popModule = popModule.Substring(0, popModule.Length - 2);
        XmlDocument doc = new XmlDocument();
        doc.Load(xmlPath);
        XmlNode xmlpopulateonadd = doc.SelectSingleNode("/setting/populateonadd/" + popModule);
        string query = txtQuery.Text.Replace("<", "&lt;").Replace(">", "&gt;");

        string newSetting = "\n\t\t\t<querystringcolumn>" + txtQueryStringColumn.Text + "</querystringcolumn>" +
                            "\n\t\t\t<query>" + query + "</query>";
        if (xmlpopulateonadd == null)
        {
            xml = "\n\t\t<" + popModule + ">" +
                  newSetting +
                  "\n\t\t</" + popModule + ">\n";
            XmlNode newNode = doc.CreateNode(XmlNodeType.Element, "populateonadd", null);
            newNode.InnerXml = xml;
            XmlNode xmlNode = doc.SelectSingleNode("/setting");
            xmlNode.AppendChild(newNode);
        }
        else
        {
            newSetting += "\n\t\t";
            xmlpopulateonadd.InnerXml = newSetting;
        }
        doc.Save(xmlPath);
        UpdateModuleSettings();
        lblMessage.Text = "Setting updated successfully!";
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        if (txtQueryStringColumn.Text == "")
        {
            lblMessage.Text = "Please enter query string column";
            return;
        }
        string popModule = txtQueryStringColumn.Text;
        if (popModule.EndsWith("id")) popModule = popModule.Substring(0, popModule.Length - 2);

        DataRow drModule = DbTable.GetOneRow("tbl_module", Common.GetQueryStringValue("mid"));
        string moduleName = Convert.ToString(drModule["module_modulename"]).Replace(" ", "").ToLower();
        string xmlPath = Server.MapPath("~/xml/add/" + moduleName + ".xml");
        bool ispopulated = false;
        if (File.Exists(xmlPath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode xmlpopulateonadd = doc.SelectSingleNode("/setting/populateonadd/" + popModule + "/query");
            if (xmlpopulateonadd != null)
            {
                txtQuery.Text = xmlpopulateonadd.InnerText;
                ispopulated = true;
            }
        }
        if(!ispopulated)
        {
            string query = "SELECT * FROM tbl_" + popModule;
            if (txtJoinModules.Text != "")
            {
                Array arr = txtJoinModules.Text.Split(',');
                for (int i = 0; i < arr.Length; i++)
                {
                    string m = Convert.ToString(arr.GetValue(i));
                    query += " \nJOIN tbl_" + m + " ON " + m + "_" + m + "id = " + popModule + "_" + m + "id";
                }
            }
            txtQuery.Text = query;
        }
    }
    private void UpdateModuleSettings()
    {
        string setting = DbTable.GetOneColumnData("tbl_module", "module_settings", Common.GetQueryStringValue("id"));
        if (!setting.Contains("PopulateOnAdd"))
        {
            if (setting == "")
            {
                setting = "PopulateOnAdd=" + txtQueryStringColumn.Text;
            }
            else
            {
                setting += "\nPopulateOnAdd=" + txtQueryStringColumn.Text;
            }
        }
        string query = "update tbl_module set module_settings='" + setting + "' WHERE module_moduleid=" + Common.GetQueryStringValue("mid");
        DbTable.ExecuteQuery(query);
    }
    
    private void PopulateSettingColumns()
    {
        DataRow drModule = DbTable.GetOneRow("tbl_module", Common.GetQueryStringValue("mid"));
        string moduleName = Convert.ToString(drModule["module_modulename"]).Replace(" ", "").ToLower();
        string xmlPath = Server.MapPath("~/xml/add/" + moduleName + ".xml");

        if (File.Exists(xmlPath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNodeList xmlpopulateonadd = doc.SelectNodes("/setting/populateonadd");
            if (xmlpopulateonadd != null)
            {
                //btnGo.Enabled = false;
                ArrayList arr = new ArrayList();
                arr.Add("Select");
                for (int i = 0; i < xmlpopulateonadd.Count; i++)
                {
                    XmlNode setting = xmlpopulateonadd[i].FirstChild;
                    //txtQueryStringColumn.Text = setting.SelectSingleNode("querystringcolumn").InnerText;
                    //txtQuery.Text = setting.SelectSingleNode("query").InnerText;
                    
                    arr.Add(setting.SelectSingleNode("querystringcolumn").InnerText);
                }
                ddlQueryStringColumn.DataSource = arr;
                ddlQueryStringColumn.DataBind();
            }
        }

    }
    protected void ddlQueryStringColumn_Changed(object sender, EventArgs e)
    {
        if (ddlQueryStringColumn.SelectedIndex == 0) return;
        DataRow drModule = DbTable.GetOneRow("tbl_module", Common.GetQueryStringValue("mid"));
        string moduleName = Convert.ToString(drModule["module_modulename"]).Replace(" ", "").ToLower();
        string xmlPath = Server.MapPath("~/xml/add/" + moduleName + ".xml");
        
        if (File.Exists(xmlPath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            string popModule = ddlQueryStringColumn.SelectedValue;
            if (popModule.EndsWith("id")) popModule = popModule.Substring(0, popModule.Length - 2);
            XmlNode xmlpopulateonadd = doc.SelectSingleNode("/setting/populateonadd/" + popModule);
            if (xmlpopulateonadd != null)
            {
                txtQuery.Text = xmlpopulateonadd.SelectSingleNode("query").InnerText;
                txtQueryStringColumn.Text = ddlQueryStringColumn.SelectedValue;
            }
        }
    }
}
