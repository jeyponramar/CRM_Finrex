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

public partial class CP_configure_viewpage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        if(!IsPostBack)
        {
            if (GlobalUtilities.ConvertToBool(Request.QueryString["isSubGrid"]))
            {
                NonEditableGridSettings.Visible = true;
                tdSetings.Visible = false;
                btnUpdateSettings.Attributes.Add("ValidationGroup", "vg");
                bindAllNonEditableGridName();
                //BindColumns(getModulepath().Replace(" ", "").ToLower().Trim(),"",ddlNonEditableGridName.SelectedItem.Text);
            }
            else
            {
                BindColumns(getModulepath().Replace(" ", "").ToLower().Trim());
                btnUpdateSettings.Attributes.Remove("ValidationGroup");
            }
        }
    }    
    protected void btnUpdateSettings_Click(object sender, EventArgs e)
    {
        ViewState["query"] = txtquery.Text;
        if (GlobalUtilities.ConvertToBool(Request.QueryString["isSubGrid"]))
        {
            //BindColumns(getModulepath().Replace(" ", "").ToLower().Trim());
            if (txtGridName.Text == "")
            {
                lblmessage.Text="Please enter grid name";
                lblmessage.Visible =true;
                txtGridName.Focus();
                return;
            }
            saveXMLSubGridSettings();
        }
        else
        {
            saveXMLSettings();
        }
    }
    private string getModulepath()
    {
        string strModuleName = "";
        string strType = "";
        int reportid = GlobalUtilities.ConvertToInt(Request.QueryString["id"]);//Report Id
        int moduleid = GlobalUtilities.ConvertToInt(Request.QueryString["mid"]);//Module Id
        hdnModuleId.Text = moduleid.ToString();
        hdnReportId.Text = reportid.ToString();
        //ViewState["module"] = "mid=" + moduleid + "|rid=" + reportid + "";
        if (GlobalUtilities.ConvertToBool(Request.QueryString["isSubGrid"]))
        {
            return "add/" + GlobalUtilities.ConvertToString(Request.QueryString["mname"]).ToLower().Trim().Replace(" ","");
        }
        else
        {
            if (reportid > 0)
            {
                strType = "Report";
                DataRow dr = Common.GetOneRowData("tbl_report", reportid);
                if (dr != null)
                {
                    strModuleName = GlobalUtilities.ConvertToString(dr["report_reportname"]);
                    return "report/" + strModuleName;
                }
            }
            else if (moduleid > 0)
            {
                strType = "Module";
                strModuleName = GlobalUtilities.ConvertToString(Request.QueryString["mname"]);
                hdnModuleName.Text = strModuleName;
                return "view/" + strModuleName;
            }
        }
        lblmessage.Visible = true;
        lblmessage.Text = "Invalid "+strType;
        return strModuleName;

    }
    private void bindAllNonEditableGridName()
    {
        XmlDocument doc = XMLNodeBinder.getXMLDocument("add", GlobalUtilities.ConvertToString(Request.QueryString["mname"]).Trim().ToLower().Replace(" ",""));
        string strNonEditGridname = XMLNodeBinder.getSingleNodeText("noneditablegridname", doc);
        ViewState["NonEditGridname"] = strNonEditGridname;
        Array arr = strNonEditGridname.Split(',');
        ddlNonEditableGridName.Items.Add(new ListItem("Select grid Name", "0"));
        for (int i = 0; i < arr.Length; i++)
        {
            ddlNonEditableGridName.Items.Add(new ListItem(arr.GetValue(i).ToString(), (i + 1).ToString()));
        }

    }
    protected void btnBindSettings_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        if (btn.Text == "Create New Settings")
        {
            if (GlobalUtilities.ConvertToInt(ddlNonEditableGridName.SelectedValue) > 0)
            {
                lblmessage.Text = "Don't Select Grid Name";
                lblmessage.Visible = true;
                return;
            }
            BindColumns("add/" + GlobalUtilities.ConvertToString(Request.QueryString["mname"]).ToLower().Trim().Replace(" ",""), "", "");
        }
        else
        {
            BindColumns("add/" + GlobalUtilities.ConvertToString(Request.QueryString["mname"]).ToLower().Trim().Replace(" ",""), "", ddlNonEditableGridName.SelectedItem.Text.ToLower().Trim().Replace(" ",""));
            txtGridName.Text = ddlNonEditableGridName.SelectedItem.Text;
        }
        tdSetings.Visible = true;
        tdNonEditSettings.Visible = true;

    }
    private void BindColumns(string strModulepath,string strTempQuery,string gridname)
    {
        bool isSubGridBuilding = GlobalUtilities.ConvertToBool(Request.QueryString["isSubGrid"]);
        string strGridSettingName = GlobalUtilities.ConvertToString(Request.QueryString["name"]);
        lblmessage.Visible = false;
        DataTable dtColumn = new DataTable();
        string targetFile = Server.MapPath("~/xml/" + strModulepath + ".xml");
        txttargetFile.Text = targetFile;
        if (!System.IO.File.Exists(targetFile))
        {
            Response.Write("File not found");
            Response.End();
        }
        //
        GridSettings gridSettings = new GridSettings();        
        string str_modulename= strModulepath.Substring(strModulepath.IndexOf('/')+1);
        gridSettings.BindXml(str_modulename);
        //Grid Settings

        //
        XmlDocument doc = new XmlDocument();
        doc.Load(txttargetFile.Text);
        //Root Node 
        gridname = gridname.Trim().Replace(" ", "").ToLower();
        string strRootNode = (isSubGridBuilding) ? "/setting/noneditablegrid[@name='"+gridname+"']" : "/setting";
        if (isSubGridBuilding)
        {
            ViewState["NonEditGridname"] = XMLNodeBinder.getSingleNodeText("/setting/" + "/noneditablegridname", doc); 
        }
        //
        //

        string query = (strTempQuery != "") ? strTempQuery :XMLNodeBinder.getSingleNodeText(strRootNode+"/query",doc);
        txtquery.Text = query;
        query = query.Replace("$COLUMNS$", "*").Replace("$Search$", "").Replace("WHERE", "--").Replace("where", "--").Replace("*", " TOP 0 * ");
        query = XMLQueryBuilder.bulidXMLQuery(query);
        if (query!="")
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
        int totalSettingsPerRow =5;
        int int_counter = 0;
        gridname = gridname.ToLower().Trim().Replace(" ","");
        int m_sequence = 0;
        for (int i = 0; i < dtColumn.Rows.Count; i++) 
        {                   
            string strColumnName =GlobalUtilities.ConvertToString(dtColumn.Rows[i]["ColumnName"]);
            //string isColumnincluded = (isdataExists(gridSettings.arrColumns,strColumnName)) ? "checked" : "";
            //string isSearchIncluded = (isdataExists(gridSettings.arrSearchByCols,strColumnName)) ? "checked" : "";
            //string isAdvSearchIncluded = (fn_isAdvSearchIncluded(strColumnName, GlobalUtilities.ConvertToInt(hdnModuleId.Text))) ? "checked" : "";
            //string strColumnLbl =GlobalUtilities.ConvertToString(dtColumn.Rows[i]["Columnlabel"]);

            //string isColumnincluded = (isdataExists(XMLNodeBinder.getMultipleNode(strRootNode+"/columns",doc), strColumnName)) ? "checked" : "";
            string isColumnincluded =""; //(isdataExists(XMLNodeBinder..getMultipleNode(strRootNode + "/gridcolumn/column/name", doc), strColumnName)) ? "checked" : "";
            string _rootNode = (isSubGridBuilding) ? "/setting/noneditablegrid[@name='"+gridname+"']/columns" : "/setting/gridcolumn/column/name";
            XmlNodeList xmlGridColumns = doc.SelectNodes("/setting/gridcolumn/column/name");
            string strgridColumns="";
            if (isSubGridBuilding)
            {
                XmlNode xml_strgridColumns = doc.SelectSingleNode("/setting/noneditablegrid[@name='" + gridname + "']/columns");
                if(xml_strgridColumns!=null)
                strgridColumns = xml_strgridColumns.InnerText; 
                Array arr = strgridColumns.Split(',');
                for (int k = 0; k < arr.Length; k++)
                {
                    if (strColumnName == GlobalUtilities.ConvertToString(arr.GetValue(k)))
                    {
                        isColumnincluded = "checked";
                        m_sequence++;
                        break;
                    }
                }
                XmlNode xmltitle = doc.SelectSingleNode("/setting/noneditablegrid[@name='" + gridname + "']/title");
                txtGridName.Text = (xmltitle != null) ? xmltitle.InnerText : "";
            }
            else
            {

                for (int j = 0; j < xmlGridColumns.Count; j++)
                {
                    if (xmlGridColumns[j].InnerText == strColumnName)
                    {
                        isColumnincluded = "checked";
                        break;
                    }
                }
            }

            string isSearchIncluded = (isdataExists(XMLNodeBinder.getSingleNode(strRootNode + "/searchbycolumns", doc), strColumnName)) ? "checked" : "";
            string isAdvSearchIncluded =(isSubGridBuilding)?"":(fn_isAdvSearchIncluded(strColumnName, GlobalUtilities.ConvertToInt(hdnModuleId.Text))) ? "checked" : "";
            string strColumnLbl = GlobalUtilities.ConvertToString(dtColumn.Rows[i]["Columnlabel"]);

            string strcolor_SearchIncluded = "";
            string strcolor_ColumnIncluded = "";
            string strcolor_ColumnAdvSearchIncluded = "";
            string strTitle = (strColumnLbl.Length > 18) ? "title='" + strColumnLbl + "'" : "";
            int NewmoduleStart = GlobalUtilities.ConvertToInt(dtColumn.Rows[i]["IsNewModuleStart"]);
            string str_Module_Name = GlobalUtilities.ConvertToString(dtColumn.Rows[i]["ModuleName"]);
            if (NewmoduleStart==1)
            {
                string M_name = (str_Module_Name != "") ? GlobalUtilities.UppercaseFirst(str_Module_Name) : "Others";
                int TotNoOfRow = (isSubGridBuilding) ? 2 : 4;
                html.Append("<tr><td style='font-size:19px;border-bottom:1px dotted white;color:#07FA48' colspan='" + (totalSettingsPerRow * TotNoOfRow) + "' align='center'><b>" + M_name + "</b></td></tr>");
                if ((int_counter) % totalSettingsPerRow != 0)
                {
                    html.Append("</tr>");
                }
                int_counter = 0;
            }
            if (isColumnincluded != "" || isSearchIncluded != "")
            {

            }
            if (int_counter % totalSettingsPerRow == 0)
            {
                html.Append("<tr>");
            }
            string strBackColor = (isColumnincluded != "" || isSearchIncluded != ""||isAdvSearchIncluded!="") ? "#00FF00" : "#ffffff";
            strBackColor = "style='width:130px;background-color:" + strBackColor + "'";
            strcolor_SearchIncluded = (isSearchIncluded != "") ? "style='color:#FF384C'" : "";
            strcolor_ColumnIncluded = (isColumnincluded != "") ? "style='color:#07FA48'" : "";
            strcolor_ColumnAdvSearchIncluded = (isAdvSearchIncluded != "") ? "style='color:#FF384C'" : "";
            html.Append("<td><input " + isColumnincluded + " type='checkbox' columnlbl='" + strColumnLbl + "' columnname='" + strColumnName + "' class='chkincludeview chkview_" + strColumnName + "' name='chk_" + strColumnName + "' id=chk_includecolumn_'" + strColumnName + "'/><span class='chkview_" + strColumnName + "' " + strcolor_ColumnIncluded + ">View</span></td>");
            if (!isSubGridBuilding)
            {
                html.Append("<td><input " + isSearchIncluded + " type='checkbox' class='chkincludesearch chksearch_" + strColumnName + "' columnlbl='" + strColumnLbl + "' columnname='" + strColumnName + "' name='chk_" + strColumnName + "' id=chk_includesearch_'" + strColumnName + "'/><span class='chksearch_" + strColumnName + "'  " + strcolor_SearchIncluded + "'>Search</span></td>");
                html.Append("<td><input " + isAdvSearchIncluded + " type='checkbox' class='chkincludeadvsearch chkadvsearch_" + strColumnName + "' columnlbl='" + strColumnLbl + "' columnname='" + strColumnName + "' name='chk_" + strColumnName + "' id=chk_includeadvsearch_'" + strColumnName + "'/><span class='chkadvsearch_" + strColumnName + "'  " + strcolor_ColumnAdvSearchIncluded + "'>AdvS</span></td>");
            }
            html.Append("<td><input " + strBackColor + " type='text' class='txt_" + strColumnName + "' " + strTitle + " value='" + strColumnLbl + "' placeholder='" + strColumnLbl + "' name=txt_'" + strColumnName + "' id='" + i + "_" + strColumnName + "'/>     <input style='width:25px;' type='text' class='txtsequence_" + strColumnName + "' value='' placeholder='Sequence' name=txtsequence_'" + strColumnName + "' id='" + i + "_" + strColumnName + "'/> </td>");
            if ((int_counter + 1) % totalSettingsPerRow == 0)
            {
                html.Append("</tr>");
            }            
            int_counter++;             
        }
        html.Append("</table></tr>");
        ltSettings.Text = html.ToString();
    }    
    private void saveXMLSubGridSettings() 
    {
        string gridsequence = hdnIncludeView_Sequence.Text;
        string gridactualsequence = "";
        string gridColumns = txtincludeview.Text;
        string gridColumnsHeader = hdnincludeviewHeader.Text;
        Array arrgridSequence = gridsequence.Split(',');
        for (int i = 0; i < arrgridSequence.Length; i++)
        {
            //gridactualsequence+=(gridactualsequence=="")?
        }
        string gridName = GlobalUtilities.ConvertToString(ViewState["NonEditGridname"]);
        gridName = (gridName == "") ? txtGridName.Text : (gridName.Contains(txtGridName.Text)) ? gridName : gridName + "," + txtGridName.Text;
        ViewState["NonEditGridname"] = gridName;
        Array arrGridColumns = gridColumns.Split(',');
        Array arrGridColumnHeader = gridColumnsHeader.Split(',');

        XmlDocument doc = new XmlDocument();
        doc.Load(txttargetFile.Text);
        string strRootNode = "/setting/noneditablegrid[@name='"+txtGridName.Text.ToLower().Trim().Replace(" ","")+"']";
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
            
            XmlNode NonEdit = doc.CreateNode(XmlNodeType.Element, "noneditablegrid", null);
            XmlAttribute name = doc.CreateAttribute("name");
            name.Value = txtGridName.Text.ToLower().Trim().Replace(" ","");
            NonEdit.Attributes.Append(name);
            RootNode.AppendChild(NonEdit);

            //XmlNode NonEditEnd = doc.CreateNode(XmlNodeType.EndElement, "noneditablegrid", null);
            //RootNode.AppendChild(NonEditEnd);
            XmlNode columns = doc.CreateNode(XmlNodeType.Element, "columns", null);
            columns.InnerText = gridColumns;
            XmlNode title = doc.CreateNode(XmlNodeType.Element, "title", null);
            title.InnerText = txtGridName.Text;
            XmlNode columnlabels = doc.CreateNode(XmlNodeType.Element, "columnlabels", null);
            columnlabels.InnerText = gridColumnsHeader;
            XmlNode query = doc.CreateNode(XmlNodeType.Element, "query", null);
            query.InnerText = txtquery.Text;           
            NonEdit.AppendChild(columns);
            NonEdit.AppendChild(columnlabels);
            NonEdit.AppendChild(query);
            NonEdit.AppendChild(title);
            if (doc.SelectSingleNode("/setting/noneditablegridname") != null)
            {
                bindXmlNodeText(doc.SelectSingleNode("/setting" + "/noneditablegridname"), GlobalUtilities.ConvertToString(ViewState["NonEditGridname"]));
            }
            else
            {
                XmlNode Nodegridname = doc.CreateNode(XmlNodeType.Element, "noneditablegridname", null);
                Nodegridname.InnerText = GlobalUtilities.ConvertToString(ViewState["NonEditGridname"]);
                RootNode.AppendChild(Nodegridname);
            }
            doc.AppendChild(RootNode);

        }
        else
        {
            bindXmlNodeText(doc.SelectSingleNode("/setting" + "/noneditablegridname"), GlobalUtilities.ConvertToString(ViewState["NonEditGridname"]));
            //bindXmlNodeText(doc.SelectSingleNode(strRootNode + "/title"), txtGridName.Text);
            bindXmlNodeText(doc.SelectSingleNode(strRootNode + "/columns"), gridColumns);
            bindXmlNodeText(doc.SelectSingleNode(strRootNode + "/columnlabels"), gridColumnsHeader);
            bindXmlNodeText(doc.SelectSingleNode(strRootNode + "/title"), txtGridName.Text);
            bindXmlNodeText(doc.SelectSingleNode(strRootNode + "/query"), GlobalUtilities.ConvertToString(ViewState["query"]));
        }
        
        doc.Save(txttargetFile.Text);
        BindColumns(getModulepath().Replace(" ", "").ToLower().Trim(),"",txtGridName.Text.ToLower().Trim().Replace(" ",""));
        lblmessage.Text = "Settings updated sucessfully!!!!!!!!";
        lblmessage.Visible = true;
    }
    private void saveXMLSettings()
    {
        string gridColumns = txtincludeview.Text;
        string gridSearchColumns = txtincludesearch.Text;
        string gridAdvSearchColumns = hdnIncludeAdvSearch.Text;
        string gridColumnsHeader = hdnincludeviewHeader.Text;
        string gridSearchColumnsHeader = hdnincludesearchHeader.Text;

        Array arrGridColumns = gridColumns.Split(',');
        Array arrGridColumnHeader = gridColumnsHeader.Split(',');
        Array arrSearchColumns = gridSearchColumns.Split(',');
        Array arrSearchColumnHeader = gridSearchColumnsHeader.Split(',');
        Array arrAdvSearchColumn= gridAdvSearchColumns.Split(',');
        
        //Update Column table
        if (GlobalUtilities.ConvertToInt(hdnReportId.Text) == 0)//only Module
        {
            //Array arrFilteredArrayColumns = arrGetFilteredArray(arrGridColumns, hdnModuleName.Text);
            string query = "DELETE FROM tbl_columns WHERE columns_moduleid=" + hdnModuleId.Text + " AND ISNULL(columns_isextracolumns,0)=1";
            DbTable.ExecuteQuery(query);
            query = "UPDATE tbl_columns SET columns_isadvancedsearch=0,columns_isviewpage=0 WHERE columns_isupdatedbyconfigsettings=1 AND columns_moduleid=" + hdnModuleId.Text;
            DbTable.ExecuteQuery(query);
            for(int i=0; i<arrGridColumns.Length;i++)
            {
                string strColumnsName = GlobalUtilities.ConvertToString(arrGridColumns.GetValue(i));
                string strColumnsLbl = GlobalUtilities.ConvertToString(arrGridColumnHeader.GetValue(i));
                bool isAdvSearch = isdataExists(arrAdvSearchColumn,strColumnsName);
                bool isView = isdataExists(arrSearchColumns, strColumnsName);
                if (hdnModuleName.Text != "")
                {
                    string strmodule_name_ = hdnModuleName.Text.Trim().ToLower();
                    if (strColumnsName.StartsWith(strmodule_name_+"_"))
                    {
                        int moduleid = GlobalUtilities.ConvertToInt(hdnModuleId.Text);
                        string str_isAdv = (isAdvSearch) ? "1" : "0";
                        string str_isView = (isView) ? "1" : "0";
                        query = @"IF EXISTS(SELECT * FROM tbl_columns WHERE columns_moduleid=" + moduleid + " AND columns_columnname='" + strColumnsName + "')" + @"
	                            BEGIN
		                            UPDATE tbl_columns SET columns_lbl='" + strColumnsLbl + "',columns_isadvancedsearch=" + str_isAdv + ",columns_isviewpage=" + str_isView + ",columns_isupdatedbyconfigsettings=1 WHERE columns_moduleid=" + moduleid + " AND columns_columnname='" + strColumnsName + "'" +
                                    @"END
                            ELSE
	                            BEGIN
		                            INSERT INTO tbl_columns(columns_lbl,columns_isadvancedsearch,columns_isviewpage,columns_moduleid,columns_columnname,columns_isextracolumns,columns_isupdatedbyconfigsettings)  VALUES('" + strColumnsLbl + "'," + str_isAdv + "," + str_isView + "," + moduleid + ",'" + strColumnsName + "',1,1)" +
                                    "END";
                        DbTable.ExecuteQuery(query);
                    }
                }
            }
        }
        //End


        XmlDocument doc = new XmlDocument();
        doc.Load(txttargetFile.Text);
        XmlNode node = doc.SelectSingleNode("/setting/gridcolumns");
        bindXmlNodeText(doc.SelectSingleNode("/setting/advancedsearchcolumns"),gridAdvSearchColumns);
        bindXmlNodeText(doc.SelectSingleNode("/setting/gridcolumns"),gridColumns);
        bindXmlNodeText(doc.SelectSingleNode("/setting/gridheadercolumns"),gridColumnsHeader);
        bindXmlNodeText(doc.SelectSingleNode("/setting/query"), txtquery.Text);
        bindXmlNodeText(doc.SelectSingleNode("/setting/searchbycolumns"),gridSearchColumns);
        bindXmlNodeText(doc.SelectSingleNode("/setting/searchbylabels"),gridSearchColumnsHeader);
        bindXmlNodeText(doc.SelectSingleNode("/setting/enableadvancedsearch"), (hdnIncludeAdvSearch.Text != "") ? "true" : "false");
        XmlNodeList xmlGridColumnFormat = doc.SelectNodes("/setting/gridcolumn/column/format");
        XmlNodeList xmlGridColumns = doc.SelectNodes("/setting/gridcolumn/column/name");

        string columns = "";
        for (int i = 0; i < arrGridColumns.Length; i++)
        {
            string columnName = arrGridColumns.GetValue(i).ToString();
            string dataFormat = "";
            if (xmlGridColumnFormat != null && xmlGridColumnFormat.Count > 0)
            {
                for (int j = 0; j < xmlGridColumns.Count; j++)
                {
                    if (xmlGridColumns[j].InnerText == columnName)
                    {
                        dataFormat = xmlGridColumnFormat[j].InnerText;
                    }
                }
            }
            columns += "<column>" +
                        "<name>" + columnName + "</name>" +
                        "<headertext>" + arrGridColumnHeader.GetValue(i).ToString() + "</headertext>" +
                        "<row>1</row>" +
                        "<width>10%</width>" +
                        "<format>"+dataFormat+"</format>"+
                    "</column>";
        }
        bindXmlNodeText(node = doc.SelectSingleNode("/setting/gridcolumn"),columns,true);
        doc.Save(txttargetFile.Text);
        BindColumns(getModulepath().Replace(" ", "").ToLower().Trim());
        lblmessage.Text = "Settings updated sucessfully!!!!!!!!";
        lblmessage.Visible = true;
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
    private bool isdataExists(ArrayList arr, string value)
    {
        bool isdataExists = false;
        for (int j = 0; j < arr.Count; j++)
        {            
            if (GlobalUtilities.ConvertToString(arr[j]).ToLower() == value.ToLower())
            {
                isdataExists = true;
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
        ViewState["query"] = txtquery.Text;
        BindColumns(getModulepath().Replace(" ", "").ToLower().Trim(),txtquery.Text,"");
    }
    private void BindColumns(string p)
    {
        BindColumns(p, "","");
    }
}
