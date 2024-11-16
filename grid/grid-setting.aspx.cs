using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Xml;
using System.Text;
using System.Data;

public partial class grid_setting : System.Web.UI.Page
{
    GlobalData globalData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["m"] == null) Response.End();
        if (!IsPostBack)
        {
            //string query = "select * from tbl_columns join tbl_module module_moduleid=columns_moduleid where replace(module_modulename,' ','')='" + Request.QueryString["m"] + "'";
            //InsertUpdate obj = new InsertUpdate();
            //DataTable dttbl = obj.ExecuteSelect(query);
            //globalData.FillDropdown(ddlSort, "tbl_columns", "columns_lbl", "replace(module_modulename,' ','')='"+Request.QueryString["m"]+"'");
            BindColumns();
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('Grid Setting - " + Request.QueryString["m"] + "')</script>");
    }
    private void BindSortColumns()
    {

    }
    private void BindColumns()
    {
        string targetFile = Server.MapPath("~/xml/view/" + Request.QueryString["m"] + ".xml");
        if (!System.IO.File.Exists(targetFile))
        {
            targetFile = Server.MapPath("~/xml/report/" + Request.QueryString["m"] + ".xml");
            if (!System.IO.File.Exists(targetFile))
            {
                Response.Write("Error occurred");
                Response.End();
            }
        }
        GridSettings gridSettings = new GridSettings(); 
        gridSettings.BindXml(Request.QueryString["m"]);

        lstLeft.Items.Clear();
        lstRight.Items.Clear();
        for (int i = 0; i < gridSettings.arrColumns.Length; i++)
        {
            bool exists = false;
            for (int j = 0; j < gridSettings.xmlGridColumns.Count; j++)
            {
                if (gridSettings.xmlGridColumns[j].InnerText == gridSettings.arrColumns.GetValue(i).ToString())
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                lstLeft.Items.Add(new ListItem(gridSettings.arrColumnLabels.GetValue(i).ToString(), gridSettings.arrColumns.GetValue(i).ToString()));
            }
        }
        for (int i = 0; i < gridSettings.xmlGridColumns.Count; i++)
        {
            lstRight.Items.Add(new ListItem(gridSettings.xmlGridHeaders[i].InnerText,gridSettings.xmlGridColumns[i].InnerText));
        }
        BindGridColumns(gridSettings.xmlGridHeaders,gridSettings.xmlGridColumnWidth);
        ddlSort.Items.Add(new ListItem(gridSettings.HiddenColumns, gridSettings.HiddenColumns));
        for (int i = 0; i < gridSettings.arrColumns.Length; i++)
        {
            ddlSort.Items.Add(new ListItem(gridSettings.arrColumnLabels.GetValue(i).ToString(), gridSettings.arrColumns.GetValue(i).ToString()));
        }

        //populate advanced settings
        txtTopRecords.Text = gridSettings.TopRecords.ToString();
        txtPageSize.Text = gridSettings.PageSize.ToString();
        chkEnablePaging.Checked = gridSettings.EnablePaging;
        chkEnableSorting.Checked = gridSettings.EnableSorting;
        chkEnableSearch.Checked = gridSettings.EnableSearch;
        string defaultSort = gridSettings.DefaultSort;
        string sortDirection = "ASC";
        if (defaultSort.ToLower().Contains(" desc"))
        {
            sortDirection = "DESC";
        }
        ddlSortDirection.SelectedValue = sortDirection;
        ddlSort.SelectedValue = defaultSort.ToLower().Replace(" asc", "").Replace(" desc", "");
    }
    private void BindGridColumns(XmlNodeList xmlGridHeaders, XmlNodeList xmlGridColumnWidth)
    {
        StringBuilder html = new StringBuilder();
        html.Append("<table class='repeater' cellpadding='2' cellspacing='0'><tr class='repeater-header'>");
        for (int i = 0; i < xmlGridHeaders.Count; i++)
        {
            string width = "";
            if (xmlGridColumnWidth != null)
            {
                width = " style='width:" + xmlGridColumnWidth[i].InnerText + "'";
            }
            html.Append("<td" + width + ">" + xmlGridHeaders[i].InnerText + "</td>");
        }
        html.Append("</tr><tr class='grid-set-row'>");
        for (int i = 0; i < xmlGridHeaders.Count; i++)
        {
            html.Append("<td style='border-left:1px solid #eeeeee;'>Data</td>");
        }
        html.Append("</tr></table>");
        ltGrid.Text = html.ToString();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string gridColumns = hdnGridColumns.Text;
        string gridlabels = hdnGridColumnLabels.Text;
        string targetFile = Server.MapPath("~/xml/view/" + Request.QueryString["m"] + ".xml");
        if (!System.IO.File.Exists(targetFile))
        {
            targetFile = Server.MapPath("~/xml/report/" + Request.QueryString["m"] + ".xml");
        }
        if (txtTopRecords.Text.Trim() == "")
        {
            txtTopRecords.Text = "0";
            if (txtPageSize.Text.Trim() == "")
            {
                txtPageSize.Text = "20";
            }
        }
        else
        {
            if (GlobalUtilities.ConvertToInt(txtTopRecords.Text) > 0)
            {
                txtPageSize.Text = "0";
            }
        }
        XmlDocument doc = new XmlDocument();
        doc.Load(targetFile);
        XmlNode node = doc.SelectSingleNode("/setting/gridcolumns");
        if (node != null)
            node.InnerText = gridColumns;
        node = doc.SelectSingleNode("/setting/gridheadercolumns");
        if (node != null)
            node.InnerText = gridlabels;

        node = doc.SelectSingleNode("/setting/enablepaging");
        if (node != null)
            node.InnerText = chkEnablePaging.Checked.ToString().ToLower();
        node = doc.SelectSingleNode("/setting/enablesorting");
        if (node != null)
            node.InnerText = chkEnableSorting.Checked.ToString().ToLower();

        node = doc.SelectSingleNode("/setting/enablesearch");
        if (node != null)
            node.InnerText = chkEnableSearch.Checked.ToString().ToLower();

        node = doc.SelectSingleNode("/setting/pagesize");
        if (node != null)
            node.InnerText = txtPageSize.Text.ToString();
        node = doc.SelectSingleNode("/setting/toprecords");
        if (node != null)
            node.InnerText = txtTopRecords.Text.ToString();
        node = doc.SelectSingleNode("/setting/defaultsort");
        if (node != null)
            node.InnerText = ddlSort.SelectedValue + " " + ddlSortDirection.SelectedValue;

        node = doc.SelectSingleNode("/setting/gridcolumn");
        Array arrGridCols = gridColumns.Split(',');
        Array arrGridHeaders = gridlabels.Split(',');
        Array arrGridColWidth = h_GridColumnWidth.Text.Split(',');
        XmlNodeList xmlGridColumnFormat = doc.SelectNodes("/setting/gridcolumn/column/format");
        XmlNodeList xmlGridColumns = doc.SelectNodes("/setting/gridcolumn/column/name");

        string columns = "";
        string query = "";
        for (int i = 0; i < arrGridCols.Length; i++)
        {
            string columnName = arrGridCols.GetValue(i).ToString();
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
            string imgPath = "";
            string colSetting = "";
            query = "select * from tbl_columns where columns_columnname='" + columnName + "'";
            DataRow drColumn = DbTable.ExecuteSelectRow(query);

            if (dataFormat.ToLower() == "image")
            {
                string attributes = GlobalUtilities.ConvertToString(drColumn["columns_attributes"]);
                CP cp = new CP();
                string folderPath = cp.GetAttribute(attributes, "FolderPath");
                imgPath = " imgpath='" + folderPath + "'";
            }
            else
            {
                if (drColumn != null)
                {
                    colSetting = " " + GlobalUtilities.ConvertToString(drColumn["columns_mobilesetting"]);
                }
            }
            columns += "<column" + colSetting + ">" +
                        "<name>" + columnName + "</name>" +
                        "<headertext>" + arrGridHeaders.GetValue(i).ToString() + "</headertext>" +
                        //"<row>1</row>" +
                        "<width>" + arrGridColWidth.GetValue(i).ToString() + "%</width>" +
                        "<format" + imgPath + ">" + dataFormat + "</format>" +
                    "</column>";
        }
        if (node != null)
            node.InnerXml = columns;
        doc.Save(targetFile);
        query = "update tbl_columns set columns_isviewpage=1 where columns_columnname in('" + gridColumns.Replace(",","','") + "')";
        DbTable.ExecuteQuery(query);
        //query = "select * from tbl_module WHERE REPLACE(module_modulename,' ','')='" + Request.QueryString["m"].Replace(" ","") + "'";
        //DataRow drModule = DbTable.ExecuteSelectRow(query);
        //int moduleId = GlobalUtilities.ConvertToInt(drModule["module_moduleid"]);
        //CP cp = new CP();
        //cp.GenerateViewXml(true, moduleId);
        string script = "<script>parent.closeTab();</script>";
        ClientScript.RegisterClientScriptBlock(typeof(Page), "closetab", script);
    }
}
