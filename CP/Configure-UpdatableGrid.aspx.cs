using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections;

public partial class CP_Configure_UpdatableGrid : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            ViewState["viewcolumn"] = "";
            ViewState["hiddencolumn"] = "";
            ViewState["viewcolumnheader"] = "";
            bindAllUpdatabelgridName();
            //BindColumns("add/"+GlobalUtilities.ConvertToString(Request.QueryString["mname"]));
        }
    }    
    protected void btnUpdateSettings_Click(object sender, EventArgs e)
    {
        if (GlobalUtilities.ConvertToBool(Request.QueryString["isUpdatebleGrid"]))
        {
            saveXMLSubGridSettings();
        }        
    }
    private void BindColumns(string strModulepath, string strTempQuery,string gridname)
    {
        lblmessage.Visible = false;
        DataTable dtColumn = new DataTable();
        string targetFile = Server.MapPath("~/xml/" + strModulepath + ".xml");
        txttargetFile.Text = targetFile;
        if (!System.IO.File.Exists(targetFile))
        {
            Response.Write("Error occurred");
            Response.End();
        }
        XmlDocument doc = new XmlDocument();
        doc.Load(txttargetFile.Text);
        //Root Node 
        string strRootNode = "/setting/updatablegrid[@name='" + gridname + "']";
        //        
        string query = (strTempQuery != "") ? strTempQuery : XMLNodeBinder.getSingleNodeText(strRootNode + "/query", doc);
        txtquery.Text = query;
        query = query.Replace("$COLUMNS$", "").Replace("$Search$", "").Replace("*", " TOP 0 * ");
        query = XMLQueryBuilder.bulidXMLQuery(query);
        txtTableName.Text = XMLNodeBinder.getSingleNodeText(strRootNode + "/tablename", doc);
        txtGridName.Text = gridname;
        txtHtmlScript.Text = XMLNodeBinder.getSingleNodeText(strRootNode + "/htmlscript", doc);
        ViewState["viewcolumn"] = XMLNodeBinder.getSingleNodeText(strRootNode + "/columns", doc);
        ViewState["hiddencolumn"] = XMLNodeBinder.getSingleNodeText(strRootNode + "/hiddencolumns", doc);
        ViewState["viewcolumnheader"] = XMLNodeBinder.getSingleNodeText(strRootNode + "/columnlabels", doc);
        if (query != "")
        {
            DataTable dt = new DataTable();
            dt = DbTable.ExecuteSelect(query);

            dtColumn.Columns.Add("ColumnName");
            dtColumn.Columns.Add("Columnlabel");
            dtColumn.Columns.Add("IsNewModuleStart");
            dtColumn.Columns.Add("ModuleName");
            string strPrev_Module_Name = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string strColumnName = GlobalUtilities.ConvertToString(dt.Columns[i]);
                string strModule_Name = strColumnName.Substring(0, (strColumnName.IndexOf('_') >= 0) ? strColumnName.IndexOf('_') : 0);
                string strIdentityColumn = strModule_Name + "_" + strModule_Name + "id";
                if (strIdentityColumn == strColumnName) continue;
                string strColumnlbl = getColumnLbl(strColumnName);
                DataRow dr = dtColumn.NewRow();
                if (strColumnName.Contains("_createddate"))
                {
                    strColumnlbl = "Created On";
                }
                dr["ColumnName"] = strColumnName;
                dr["ModuleName"] = strModule_Name;
                dr["Columnlabel"] = (strColumnlbl == "") ? strColumnName : strColumnlbl;
                if (strPrev_Module_Name != strModule_Name)
                {
                    dr["IsNewModuleStart"] = (strPrev_Module_Name != strModule_Name) ? 1 : 0;
                }
                else
                {
                    dr["IsNewModuleStart"] = -1;
                }
                strPrev_Module_Name = strModule_Name;
                dtColumn.Rows.Add(dr);
            }
        }
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%'><tr>");
        ddlColumns.DataSource = dtColumn;
        ddlColumns.DataTextField = "Columnlabel";
        ddlColumns.DataValueField = "ColumnName";
        ddlColumns.DataBind();
        ddlColumns.Items.Insert(0, new ListItem(" Select ", "0"));
        ddlColumns.SelectedIndex = 0;
    }
    private void saveXMLSubGridSettings()
    {
        string gridColumns = GlobalUtilities.ConvertToString(ViewState["viewcolumn"]);
        string hiddenColumns = GlobalUtilities.ConvertToString(ViewState["hiddencolumn"]);
        string gridColumnsHeader = GlobalUtilities.ConvertToString(ViewState["viewcolumnheader"]);        
        string gridName =GlobalUtilities.ConvertToString(ViewState["UpdatableGridname"]);
        gridName = (gridName == "") ? txtGridName.Text : (gridName.Contains(txtGridName.Text)) ? gridName : gridName+"," + txtGridName.Text;
        ViewState["UpdatableGridname"] = gridName;
        Array arrGridColumns = gridColumns.Split(',');
        Array arrGridColumnHeader = gridColumnsHeader.Split(',');
        Array arrOptionTypeId = hdnGridTextboxType.Text.Split(',');
        XmlDocument doc = new XmlDocument();
        doc.Load(txttargetFile.Text);
        string strRootNode = "/setting/updatablegrid[@name='"+txtGridName.Text+"']";
        XmlNode node = doc.SelectSingleNode(strRootNode);
        if (node == null)
        {
            if (!File.Exists(txttargetFile.Text))
            {
                File.Create(txttargetFile.Text);
                doc = new XmlDocument();
                doc.Load(txttargetFile.Text);
            }
            XmlNode RootNode = doc.SelectSingleNode("/setting");            
            if (RootNode == null)
            {
                RootNode = doc.CreateNode(XmlNodeType.Element, "setting", null);
            }
            XmlNode Nodegridname = doc.CreateNode(XmlNodeType.Element, "updatablegridname", null);
            Nodegridname.InnerText = GlobalUtilities.ConvertToString(ViewState["UpdatableGridname"]);            

            XmlNode UpdatableGrid = doc.CreateNode(XmlNodeType.Element, "updatablegrid", null);
            XmlAttribute name = doc.CreateAttribute("name");
            name.Value = txtGridName.Text;
            UpdatableGrid.Attributes.Append(name);
            RootNode.AppendChild(UpdatableGrid);           

            //XmlNode NonEditEnd = doc.CreateNode(XmlNodeType.EndElement, "noneditablegrid", null);
            //RootNode.AppendChild(NonEditEnd);
            XmlNode tablename = doc.CreateNode(XmlNodeType.Element, "tablename", null);
            tablename.InnerText = txtTableName.Text;
            XmlNode columns = doc.CreateNode(XmlNodeType.Element, "columns", null);
            columns.InnerText = gridColumns;
            XmlNode hiddencolumns = doc.CreateNode(XmlNodeType.Element, "hiddencolumns", null);
            hiddencolumns.InnerText = hiddenColumns;
            XmlNode columnlabels = doc.CreateNode(XmlNodeType.Element, "columnlabels", null);
            columnlabels.InnerText = gridColumnsHeader;
            XmlNode query = doc.CreateNode(XmlNodeType.Element, "query", null);
            query.InnerText = txtquery.Text;
            XmlNode htmlscript = doc.CreateNode(XmlNodeType.Element, "htmlscript", null);
            htmlscript.InnerText = txtHtmlScript.Text;
            UpdatableGrid.AppendChild(tablename);
            UpdatableGrid.AppendChild(columns);
            UpdatableGrid.AppendChild(hiddencolumns);
            UpdatableGrid.AppendChild(columnlabels);
            UpdatableGrid.AppendChild(query);
            UpdatableGrid.AppendChild(htmlscript);
            
            if (doc.SelectSingleNode("/setting/updatablegridname") != null)
            {
                bindXmlNodeText(doc.SelectSingleNode("/setting" + "/updatablegridname"), GlobalUtilities.ConvertToString(ViewState["UpdatableGridname"]));
            }
            else
            {               
                RootNode.AppendChild(Nodegridname);
            }
            
            doc.AppendChild(RootNode);
            
        }
        else
        {
           // bindXmlNodeText(doc.SelectSingleNode(strRootNode + "/title"), "");
            bindXmlNodeText(doc.SelectSingleNode("/setting" + "/updatablegridname"), GlobalUtilities.ConvertToString(ViewState["UpdatableGridname"]));
            bindXmlNodeText(doc.SelectSingleNode(strRootNode + "/columns"), gridColumns);
            if (bindXmlNodeText(doc.SelectSingleNode(strRootNode + "/hiddencolumns"), hiddenColumns) == null)
            {
                XmlNode hiddencolumns = doc.CreateNode(XmlNodeType.Element, "hiddencolumns", null);
                hiddencolumns.InnerText = hiddenColumns;
                node.AppendChild(hiddencolumns);
            }

            bindXmlNodeText(doc.SelectSingleNode(strRootNode + "/columnlabels"), gridColumnsHeader);
            bindXmlNodeText(doc.SelectSingleNode(strRootNode + "/query"), txtquery.Text);
            bindXmlNodeText(doc.SelectSingleNode(strRootNode + "/htmlscript"), txtHtmlScript.Text, false);
            bindXmlNodeText(doc.SelectSingleNode(strRootNode + "/tablename"), txtTableName.Text, false);
        }
        doc.Save(txttargetFile.Text);
        BindColumns("add/" + GlobalUtilities.ConvertToString(Request.QueryString["mname"]),txtquery.Text,txtGridName.Text);
        lblmessage.Text = "Settings updated sucessfully!!!!!!!!";
        lblmessage.Visible = true;
    }
    private void bindAllUpdatabelgridName()
    {
        XmlDocument doc = XMLNodeBinder.getXMLDocument("add",GlobalUtilities.ConvertToString(Request.QueryString["mname"]));
        string strUpdatableGridname = XMLNodeBinder.getSingleNodeText("updatablegridname", doc);
        ViewState["UpdatableGridname"] = strUpdatableGridname;
        Array arr = strUpdatableGridname.Split(',');
        ddlUpdatableGridName.Items.Add(new ListItem("Select grid Name", "0"));
        for(int i=0;i<arr.Length;i++)
        {
            ddlUpdatableGridName.Items.Add(new ListItem(arr.GetValue(i).ToString(), (i + 1).ToString()));
        }
        
    }
    protected void btnBindSettings_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        if (btn.Text == "Create New Settings")
        {
            BindColumns("add/" + GlobalUtilities.ConvertToString(Request.QueryString["mname"]),"","");
        }
        else
        {
            BindColumns("add/" + GlobalUtilities.ConvertToString(Request.QueryString["mname"]), "", ddlUpdatableGridName.SelectedItem.Text);
        }
        tdSettings.Visible = true;
        
    }
    private XmlNode bindXmlNodeText(XmlNode xmlNode, string strNodevalue, bool isXML)
    {
        if (xmlNode != null)
        {
            if (isXML)
            {
                xmlNode.InnerXml = strNodevalue;
            }
            else
            {
                xmlNode.InnerText = strNodevalue;
            }
        }
        else
        {
            
        }
        return xmlNode;
    }
    private Array arrGetFilteredArray(Array arr, string condition)
    {
        Array arrNewArr=null;
        for (int i = 0; i < arr.Length; i++)
        {
            string strVal =GlobalUtilities.ConvertToString(arr.GetValue(i));
            if (strVal.Contains(condition))
            {
                arrNewArr.SetValue(strVal, i);
            }
        }
        return arrNewArr;
    }
    private void insertNewElement(XmlDocument doc, string ElementName, string Value)
    {
    }
    private XmlNode bindXmlNodeText(XmlNode node, string strNodevalue)
    {
        return bindXmlNodeText(node, strNodevalue, false);
    }
    private bool isdataExists(Array arr, string value)
    {
        int int_position =0;
        return isdataExists(arr, value, ref int_position);
    }
    private bool isdataExists(Array arr, string value,ref int IndexPosition)
    {
        bool isdataExists = false;
        for (int i = 0; i < arr.Length; i++)
        {
            if (value.ToLower() == GlobalUtilities.ConvertToString(arr.GetValue(i)).ToLower())
            {
                isdataExists = true;
                IndexPosition = i;
                break;
            }
        }
        return isdataExists;
    }
    private string getColumnLbl(string strColumnName)
    {
        return getColumnLbl(strColumnName,0);
    }
    private string getColumnLbl(string strColumnName,int moduleId)
    {
        string strColumnlbl = "";
        string strExtraWhere = (moduleId > 0) ? " AND columns_moduleid=" + moduleId : " ";
        string query = "SELECT columns_lbl FROM tbl_columns WHERE columns_columnname='" + strColumnName + "' " + strExtraWhere;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            strColumnlbl = GlobalUtilities.ConvertToString(dr["columns_lbl"]);
        }
        return strColumnlbl;
    }
    private bool fn_isAdvSearchIncluded(string strColumnName, int moduleId)
    {
        string strExtraWhere = (moduleId > 0) ? " AND columns_moduleid=" + moduleId : " ";
        string query = "SELECT columns_isadvancedsearch FROM tbl_columns WHERE columns_columnname='" + strColumnName + "' "+strExtraWhere;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            return GlobalUtilities.ConvertToBool(dr["columns_isadvancedsearch"]);
        }
        return false;
    }
    private bool fn_isAdvSearchIncluded(string strColumnName)
    {
        return fn_isAdvSearchIncluded(strColumnName, 0);
    }
    protected void generateColumns_Click(object sender, EventArgs e)
    {
        BindColumns("add/" + GlobalUtilities.ConvertToString(Request.QueryString["mname"]), txtquery.Text, (GlobalUtilities.ConvertToInt(ddlUpdatableGridName.SelectedValue) > 0) ? ddlUpdatableGridName.SelectedItem.Text : "");
    }   
    protected void btnGenerateScript_Click(object sender, EventArgs e)
    {
        string strColumnName = GlobalUtilities.ConvertToString(ddlColumns.SelectedValue);
        if (strColumnName == "0")
        {
            lblmessage.Text = "Please select column name";
            lblmessage.Visible = true;
            return; 
        }
        string strColumnLbl = GlobalUtilities.ConvertToString(txtColumnLbl.Text);
        string strViewCol = GlobalUtilities.ConvertToString(ViewState["viewcolumn"]);
        string strViewColHeader = GlobalUtilities.ConvertToString(ViewState["viewcolumnheader"]);
        int optiontypeid = GlobalUtilities.ConvertToInt(ddlScriptInputOptionId.SelectedValue);
        if (optiontypeid == 5)//Hidden Field
        {
            string hiddencol = GlobalUtilities.ConvertToString(ViewState["hiddencolumn"]);
            ViewState["hiddencolumn"] = (hiddencol == "") ? strColumnName : hiddencol + "," + strColumnName;
        }
        else
        {
            strViewCol += (strViewCol != "") ? "," + ddlColumns.SelectedValue : ddlColumns.SelectedValue;
            strViewColHeader += (strViewColHeader != "") ? "," + strColumnLbl : strColumnLbl;
            ViewState["viewcolumn"] = strViewCol;

            ViewState["viewcolumnheader"] = strViewColHeader;
        }
        generateHTMLScript(GlobalUtilities.ConvertToInt(ddlScriptInputOptionId.SelectedValue), strColumnName);
    }
    protected void ddlColumns_ChangeCommitted(object sender, EventArgs e)
    {
        if (GlobalUtilities.ConvertToString(ddlColumns.SelectedValue)!="0")
        {
            txtColumnLbl.Text = ddlColumns.SelectedItem.Text;
        }
    }
    private void generateHTMLScript(int typeid, string strColumnName)
    {
        StringBuilder html = new StringBuilder();
        string strGridName = txtGridName.Text;
        if (typeid == 1)
            html.Append("<td>$" + strColumnName + "$</td>");
        else if (typeid == 2)
            html.Append("<td><input type='text' class='sbox val-i' name='@" + strGridName + "_" + strColumnName + "_$Count$' /></td>");
        else if (typeid == 3)
            html.Append("<td><input type='text' class='sbox' name='@" + strGridName + "_" + strColumnName + "_$Count$' /></td>");
        else if (typeid == 4)
            html.Append("<td><textarea name='@" + strGridName + "_" + strColumnName + "_$Count$' class='textarea'></textarea></td>");
        else if (typeid == 5)
        {
            html.Append("<td><input type='hidden' name='@" + strGridName + "_" + strColumnName + "_$Count$' /></td>");
            if (txtHtmlScript.Text.Trim() == "")
            {
                txtHtmlScript.Text = html.ToString();
            }
            else
            {
                txtHtmlScript.Text = Environment.NewLine + txtHtmlScript.Text.Insert(4, html.ToString().Replace("<td>", "").Replace("</td>", "") + Environment.NewLine + "");
            }
            return;
        }
        else if (typeid == 6)
            html.Append("<td><input type='text' class='sbox val-dbl' name='@" + strGridName + "_" + strColumnName + "_$Count$' /></td>");
        else if (typeid == 7)//Auto Complete
        {
            string strModule_Name = strColumnName.Substring(0, (strColumnName.IndexOf('_') >= 0) ? strColumnName.IndexOf('_') : 0);
            string strShortColoName = strColumnName.Replace(strModule_Name + "_", "");
            string strIdentityCol = strModule_Name + "_" + strModule_Name + "id";
            html.Append("<td><input type='text' name='@" + strGridName + "_" + strColumnName + "_$Count$'  id='" + strIdentityCol + "'  class=' hdnac " + strModule_Name + "id'/><input type='text' id='" + strShortColoName + "'  cm='" + strShortColoName + "' class='textbox  ac '  m='" + strModule_Name + "' cn='" + strShortColoName + "' include='0' /><img src='../images/down-arrow.png' class='epage'/></td>");
        }
        txtHtmlScript.Text += Environment.NewLine + html.ToString();
    }
        
    
}
