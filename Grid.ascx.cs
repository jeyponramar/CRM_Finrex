using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using WebComponent;
using System.IO;
using System.Xml;

public delegate bool GridDataRowBind(DataTable dttbl, int i, StringBuilder html);
public delegate void BindChartUsingGrid(DataTable dttbl);

public partial class Grid : System.Web.UI.UserControl
{
    public GridSettings gridSettings = new GridSettings();
    public event GridDataRowBind gridDataRowBind;
    public event BindChartUsingGrid bindChartUsingGrid;
    public DataTable Data = null;
    int _index = 0;
    public event SetGridData setGridData;
    public string GridSummaryHtml = "";
    private string _queryWithoutPaging = "";

    public string SearchHolderId
    {
        set
        {
            ViewState["SearchHolder"] = value;
        }
        get
        {
            return Convert.ToString(ViewState["SearchHolder"]);
        }
    }
    public bool IsSearch
    {
        set
        {
            ViewState["IsSearch"] = value;
        }
        get
        {
            if (ViewState["IsSearch"] == null) return false;
            return Convert.ToBoolean(ViewState["IsSearch"]);
        }
    }
    public bool EnableAddLink
    {
        set
        {
            imgCreateNew.Visible = GlobalUtilities.ConvertToBool(value);
        }
    }
    public bool IsCheckAccess
    {
        set
        {
            ViewState["IsCheckAccess"] = value;
        }
        get
        {
            if (ViewState["IsCheckAccess"] == null) return false;
            return Convert.ToBoolean(ViewState["IsCheckAccess"]);
        }
    }
    private PlaceHolder SearchHolder
    {
        get
        {
            if (SearchHolderId == "") return null;

            return (PlaceHolder)this.Parent.FindControl(SearchHolderId);
        }
    }
    public string AddUrl
    {
        set
        {
            ViewState["AddUrl"] = value;
        }
        get
        {
            return Convert.ToString(ViewState["AddUrl"]);
        }
    }
    public string ExtraWhere
    {
        set
        {
            ViewState["ExtraWhere"] = value;
        }
        get
        {
            return Convert.ToString(ViewState["ExtraWhere"]);
        }
    }
    public string Rights
    {
        set
        {
            ViewState["Rights"] = value;
        }
        get
        {
            return Convert.ToString(ViewState["Rights"]);
        }
    }
    public string GridQuery
    {
        set
        {
            ViewState["GridQuery"] = value;
        }
        get
        {
            return Convert.ToString(ViewState["GridQuery"]);
        }
    }
    public string GridSumColumns
    {
        set
        {
            ViewState["GridSumColumns"] = value;
        }
        get
        {
            return Convert.ToString(ViewState["GridSumColumns"]);
        }
    }
    public string QueryWithoutPaging
    {
        set
        {
            ViewState["QueryWithoutPaging"] = value;
            //if (gridSettings.IsMobileView)
            {
                Session["s_gridquery"] = value;//used in mobile apps
            }
        }
        get
        {
            return Convert.ToString(ViewState["QueryWithoutPaging"]);
        }
    }
    public string ReportWhere
    {
        set
        {
            ViewState["ReportWhere"] = value;
        }
        get
        {
            return Convert.ToString(ViewState["ReportWhere"]);
        }
    }
    public string SumValues
    {
        set
        {
            ViewState["SumValues"] = value;
        }
        get
        {
            return Convert.ToString(ViewState["SumValues"]);
        }
    }
    public bool IsAdvancedSearch
    {
        set
        {
            ViewState["IsAdvancedSearch"] = value;
        }
        get
        {
            return GlobalUtilities.ConvertToBool(ViewState["IsAdvancedSearch"]);
        }
    }
    public void SetConstant(string variable, string value)
    {
        ViewState["C_" + variable] = variable;
    }
    public string GetConstant(string variable, string value)
    {
        return Convert.ToString(ViewState["C_" + variable]);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Visible == false) return;
        if (Module == "") return;
        if (Module.ToLower() == "subscription")
        {
            tdFEMheader.Visible = true;
        }
        InitGrid();
        if (!IsPostBack)
        {
            if (HttpContext.Current.Request.Url.ToString().Contains("default.aspx"))
            {
            }
            else
            {
                //Common.CheckRights("view");
                //Common.CheckDataVerification();
                //if (!Common.GetModuleName().Contains("report"))
                //{
                //    btnExport.Visible = true;
                //}
            }
            LoadGrid();
        }
    }
    private bool IsRightsExists(string action)
    {
        Array arr = Rights.Split(',');
        for (int i = 0; i < arr.Length; i++)
        {
            if (Convert.ToString(arr.GetValue(i)) == action)
            {
                return true;
            }
        }
        return false;
    }
    public void BindReport()
    {
        InitGrid();
        LoadGrid();
        BindData();

    }
    public void InitGrid()
    {
        gridSettings = new GridSettings();
        gridSettings.BindXml(Module);
        string m = gridSettings.Module.ToLower();
        if (m == "") m = Module.ToLower();
        if (m.StartsWith("view")) m = m.Substring(4);
        if (m == "invoice" || m == "proformainvoice")
        {
            IsCheckAccess = true;
        }
        if (Request.Url.ToString().ToLower().Contains("default.aspx") || GlobalUtilities.ConvertToInt(CustomSession.Session("Login_RoleId")) == 1)
        {
            IsCheckAccess = false;
        }
        if (IsCheckAccess)
        {
            int userid = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId"));
            Rights = Common.GetRights(m);
            //if (!IsRightsExists("view"))
            //{
            //    Response.Redirect("~/access/noaccess.aspx");
            //}
            if (IsRightsExists("2"))//"export"))
            {
                btnExport.Visible = true;
            }
        }
        if (AddUrl == "")
        {
            imgCreateNew.Attributes.Add("href", "#/" + m + "/a/add");
        }
        else
        {
            imgCreateNew.Attributes.Add("href", AddUrl);
        }
        imgCreateNew.Attributes.Add("pt", "Add " + m);
        if (gridSettings.PageSize == 0)
        {
            tblPaging.Visible = false;
        }
        if (gridSettings.IsReport)
        {
            txtKeyword.Visible = false;
            btnSearch.Visible = false;
            ddlSearchBy.Visible = false;
            imgCreateNew.Visible = false;
            //tdEdit.Visible = false;
        }
        imgGridSetting.Visible = gridSettings.EnableGridSetting;
        if (GlobalUtilities.ConvertToInt(CustomSession.Session("Login_RoleId")) == 1)
        {
            btnExport.Visible = true;
        }
        if (AppConstants.IsLive)
        {
            imgGridSetting.Visible = false;
        }
    }
    public void LoadGrid()
    {
        CurrentSort = gridSettings.DefaultSort;
        if (CurrentSort.ToLower().Contains(" desc"))
        {
            IsAscending = false;
            CurrentSort = CurrentSort.Replace(" desc", "").Replace(" DESC", "");
        }
        else
        {
            CurrentSort = CurrentSort.Replace(" asc", "").Replace(" ASC", "");
            IsAscending = true;
        }
        if (gridSettings.EnableSearch == false)
        {
            ddlSearchBy.Visible = false;
            txtKeyword.Visible = false;
            btnSearch.Visible = false;
        }
        if (IsMobileView)
        {
            this.Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>$(document).ready(function() {initPaging($('.tblgrid-m'));});</script>");
        }
        if (!gridSettings.IsReport)
        {
            BindData();
        }
    }
    public string Module
    {
        get
        {
            return Convert.ToString(ViewState["Module"]);
        }
        set { ViewState["Module"] = value; }
    }
    public int CurrentPageIndex
    {
        get { return Convert.ToInt32(ViewState["CurrentPageIndex"]); }
        set { ViewState["CurrentPageIndex"] = value; }
    }
    public bool IsAscending
    {
        get
        {
            if (ViewState["IsAscending"] == null) return false;
            return Convert.ToBoolean(ViewState["IsAscending"]);
        }
        set { ViewState["IsAscending"] = value; }
    }
    public string PreviousSortColumn
    {
        get { return Convert.ToString(ViewState["PreviousSortColumn"]); }
        set { ViewState["PreviousSortColumn"] = value; }
    }
    public string QueryStringVariables
    {
        get { return Convert.ToString(ViewState["QueryStringVariables"]); }
        set { ViewState["QueryStringVariables"] = value; }
    }
    public string CurrentSort
    {
        get
        {
            string sb = Convert.ToString(ViewState["CurrentSort"]);

            return sb;
        }
        set
        {
            ViewState["CurrentSort"] = value;
        }
    }
    private void BindGridSetting()
    {
        if (gridSettings.IsReport)
        {
            txtKeyword.Visible = false;
            ddlSearchBy.Visible = false;
        }
        imgGridSetting.Attributes.Add("m", Module.ToLower().Replace("view", ""));
        if (IsMobileView)
        {
            ddlSearchBy_m.Items.Clear();
        }
        else
        {
            ddlSearchBy.Items.Clear();
        }
        if (gridSettings.arrSearchByCols != null)
        {
            for (int i = 0; i < gridSettings.arrSearchByCols.Length; i++)
            {
                if (IsMobileView)
                {
                    ddlSearchBy_m.Items.Add(new ListItem(gridSettings.arrSearchByLabels.GetValue(i).ToString(), gridSettings.arrSearchByCols.GetValue(i).ToString()));
                }
                else
                {
                    ddlSearchBy.Items.Add(new ListItem(gridSettings.arrSearchByLabels.GetValue(i).ToString(), gridSettings.arrSearchByCols.GetValue(i).ToString()));
                }
            }
        }
    }
    public void SetCreateUrl(string url)
    {
        //lnkAddPage.NavigateUrl = url;
    }
    public void BindData()
    {
        if (IsMobileView)
        {
            tblMobile.Visible = true;
        }
        else
        {
            tblNonMobile.Visible = true;
        }

        string m = gridSettings.Module;
        if (m == "") m = Module.ToLower().Replace("view", "");
        tblData.Attributes.Add("m", m);
        tblMobile.Attributes.Add("m", m);
        if (!gridSettings.IsEnableEdit)// && gridSettings.IsReport == false)
        {
            tblData.Attributes.Add("enableedit", "false");
        }
        if (gridSettings.GridType != "grid")
        {
            tdEdit.Visible = false;
        }
        BindGridSetting();
        CurrentPageIndex = 0;
        lnkPrevPage.Enabled = false;
        this.Visible = true;
        if (gridSettings.EditUrl == "")
        {
            //lnkAddPage.Visible = false;
        }
        else
        {
            //lnkAddPage.NavigateUrl = gridSettings.EditUrl;
            if (gridSettings.IsCreateRequired)
            {
                string AddUrl = GetLabel(gridSettings.EditUrl.Replace("Add", "Create New ").Replace(".aspx", ""));
                if (AddUrl.Contains("?"))
                {
                    AddUrl = AddUrl.Substring(0, AddUrl.IndexOf("?"));
                }
                //lnkAddPage.Text = AddUrl;
            }
            else
            {
                //lnkAddPage.Visible = false;
            }
        }
        GetData();
        if (gridSettings.EnablePaging)
        {
            //trPaging.Visible = true;
            if (gridSettings.TotalRecords < 1)
            {
                lblError.Visible = true;
                //return;
            }
            else
            {
                lblError.Visible = false;
                if (gridSettings.TotalPages > 1)
                {
                    lnkNextPage.Enabled = true;
                }
                else
                {
                    lnkNextPage.Enabled = false;
                }
            }
            BindGridData();

        }
        else
        {
            tblPaging.Visible = false;
            if (Data != null)
            {
                lblError.Visible = false;
                BindGridData();
            }
            else
            {
                tblData.Visible = false;
                lblError.Visible = true;
            }
        }
    }
    private void SetTotal()
    {
        if (gridSettings.TotalRecords == 0)
        {
            lblTotalRecords.Visible = false;
        }
        else
        {
            lblTotalRecords.Visible = true;
        }
        string TotalRecordSummary = "";
        if (ddlPageSize.SelectedValue == "All")
        {
            TotalRecordSummary = "Total Records : <b>" + gridSettings.TotalRecords + "</b>";
        }
        else
        {
            TotalRecordSummary = "Showing <b>" + Convert.ToString(CurrentPageIndex * gridSettings.PageSize + 1);

            if ((CurrentPageIndex + 1) * gridSettings.PageSize > gridSettings.TotalRecords)
            {
                TotalRecordSummary += "</b> to <b>" + Convert.ToString(gridSettings.TotalRecords) + "</b>";
            }
            else
            {
                TotalRecordSummary += "</b> to <b>" + Convert.ToString((CurrentPageIndex + 1) * gridSettings.PageSize) + "</b>";
            }
            TotalRecordSummary += " of <b>" + gridSettings.TotalRecords + "</b>";
        }
        lblTotalRecords.Text = TotalRecordSummary;

    }
    private bool IsMobileView
    {
        get
        {
            //if (gridSettings.IsMobileView && AppConstants.IsMobile)
            if (AppConstants.IsMobile)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public void BindMobileGridData()
    {
        tblMobile.Visible = true;
        _index = 0;
        if (!gridSettings.IsEnableEdit && gridSettings.IsReport == false)
        {
            //tdEdit.Visible = false;
        }
        //if (gridSettings.GridType == "grid")
        //{
        //    BindHead();
        //}

        trNodata.Visible = false;
        if (Data != null)
        {
            if (!GlobalUtilities.IsValidaTable(Data))
            {
                ltMobile.Text = "<div class='error'>No data found!</div>";

                return;
            }
            trNodata.Visible = true;
            StringBuilder html = new StringBuilder();
            string m = gridSettings.Module.ToLower();
            if (m == "") m = Module.ToLower();
            if (m.StartsWith("view")) m = m.Substring(4);

            tblMobile.Attributes.Add("m", m);
            tblMobile.Attributes.Add("title", gridSettings.Title.Replace("View ", ""));

            string idcol = "";
            if (gridSettings.IdentityColumn == "")
            {
                idcol = m + "_" + m + "id";
            }
            else
            {
                idcol = gridSettings.IdentityColumn.Replace("\n", "").Replace("\t", "").Trim();
            }
            if (idcol == "none") idcol = "";
            m = gridSettings.TableName.Replace("tbl_", "");

            bool isStatusColumnExists = false;
            string statusColorColumn_bg = "";
            string statusColorColumn_text = "";
            string statusColor_bg = "";
            string statusColor_text = "";

            if (!Module.ToLower().Contains("status") || gridSettings.IsReport)
            {
                for (int i = 0; i < Data.Columns.Count; i++)
                {
                    if (Data.Columns[i].ColumnName.ToLower().EndsWith("statusid"))
                    {
                        string statusColumnName = Data.Columns[i].ColumnName.ToLower();
                        if (statusColumnName.IndexOf("_") > 0)
                        {
                            statusColumnName = statusColumnName.Substring(statusColumnName.IndexOf("_") + 1);
                            statusColumnName = statusColumnName.Substring(0, statusColumnName.Length - 2);
                            if (Data.Columns.Contains(statusColumnName + "_textcolor"))
                            {
                                statusColorColumn_text = statusColumnName + "_textcolor";
                                statusColorColumn_bg = statusColumnName + "_backgroundcolor";
                                isStatusColumnExists = true;
                            }
                            break;
                        }
                    }
                }
            }
            DataTable dttblMobColumns = new DataTable();
            dttblMobColumns.Columns.Add("columnname");
            dttblMobColumns.Columns.Add("colspan", typeof(int));
            dttblMobColumns.Columns.Add("class");
            dttblMobColumns.Columns.Add("format");

            for (int j = 0; j < gridSettings.xmlGridColumns.Count; j++)
            {
                string colName = gridSettings.xmlGridColumns[j].InnerText;
                XmlNode xmlNode = gridSettings.xmlGridColumns[j];

                if (IsMobileColumn(xmlNode))
                {
                    string dataFormat = gridSettings.xmlGridColumnFormat[j].InnerText.ToLower();
                    int colspan = 4;
                    string customClass = "";

                    for (int k = 0; k < xmlNode.ParentNode.Attributes.Count; k++)
                    {
                        if (xmlNode.ParentNode.Attributes[k].Name == "colspan")
                        {
                            colspan = Convert.ToInt32(xmlNode.ParentNode.Attributes[k].Value);
                        }
                        else if (xmlNode.ParentNode.Attributes[k].Name == "class")
                        {
                            customClass = xmlNode.ParentNode.Attributes[k].Value;
                        }
                    }
                    DataRow dr = dttblMobColumns.NewRow();
                    dr["columnname"] = colName;
                    dr["colspan"] = colspan;
                    dr["class"] = customClass;
                    dr["format"] = dataFormat;
                    dttblMobColumns.Rows.Add(dr);
                }
            }
            for (int i = 0; i < Data.Rows.Count; i++)
            {
                if (gridDataRowBind == null)
                {
                    string rptClass = "repeater-row-m";
                    string strId = "";
                    int row = 0;
                    int prevRow = 0;
                    int id = 0;
                    string idval = "";

                    idval = "idval='" + Convert.ToString(Data.Rows[i][idcol]) + "'";
                    if (gridSettings.HiddenColumns.Trim() != "")
                    {
                        Array arrhidden = gridSettings.HiddenColumns.Split(',');
                        for (int k = 0; k < arrhidden.Length; k++)
                        {
                            idval += " " + arrhidden.GetValue(k).ToString() + "='" + Data.Rows[i][arrhidden.GetValue(k).ToString()] + "'";
                        }
                    }
                    if (i % 2 == 0)
                    {
                        rptClass = "repeater-alt-m";
                    }

                    if (gridSettings.IsEnableEdit && gridSettings.IsReport == false)
                    {
                        rptClass += " repeater-row-mob";
                    }
                    html.Append("<tr class='" + rptClass + "' " + idval + ">");


                    html.Append("<td class='repeater-td-mob'>");
                    html.Append("<table width='100%' cellpadding=3><tr>");
                    int totalColspan = 0;
                    for (int j = 0; j < dttblMobColumns.Rows.Count; j++)
                    {
                        string colName = Convert.ToString(dttblMobColumns.Rows[j]["columnname"]);
                        int colspan = Convert.ToInt32(dttblMobColumns.Rows[j]["colspan"]);
                        string customClass = Convert.ToString(dttblMobColumns.Rows[j]["class"]);
                        string dataFormat = Convert.ToString(dttblMobColumns.Rows[j]["format"]);
                        string css = "";

                        string val = Convert.ToString(Data.Rows[i][colName]);

                        if (val == "01-01-2000 12:00:00 AM" || val == "01-01-1900 12:00:00 AM")
                        {
                            val = "";
                        }

                        if (dataFormat == "Amount" || dataFormat == "Quantity")
                        {
                            css = " rightalign ";
                        }
                        else if (dataFormat == "date")
                        {
                            if (val.Contains("1900") || val.Contains("2000") || val == "")
                            {
                                val = "";
                            }
                            else
                            {
                                val = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(val));
                            }
                        }
                        else if (dataFormat == "datetime")
                        {
                            if (val.Contains("1900") || val.Contains("2000") || val == "")
                            {
                                val = "";
                            }
                            else
                            {
                                val = GlobalUtilities.ConvertToDateTime(val);
                            }
                        }
                        if (colName.EndsWith("cramount") || colName.EndsWith("dramount"))
                        {
                            if (GlobalUtilities.ConvertToDouble(val) == 0) val = "";
                        }
                        if (isStatusColumnExists && colName.ToLower().Contains("status"))
                        {
                            statusColor_bg = GlobalUtilities.ConvertToString(Data.Rows[i][statusColorColumn_bg]);
                            statusColor_text = GlobalUtilities.ConvertToString(Data.Rows[i][statusColorColumn_text]);
                            if (statusColor_text == "")
                            {
                                statusColor_text = "#000000";
                            }
                            else
                            {
                                statusColor_text = "#" + statusColor_text;
                            }
                            statusColor_bg = "#" + statusColor_bg;

                            val = "<div class='grid-status' style='background-color:" + statusColor_bg + ";color:" + statusColor_text + "'>" + val + "</div>";
                        }
                        if (val == "") val = "&nbsp;";

                        totalColspan += colspan;

                        if (totalColspan == 4)
                        {
                            if (colspan < 4)
                            {
                                customClass += " rightalign";
                            }
                        }

                        html.Append("<td colspan='" + colspan + "' class='" + customClass + "'>" + val + "</td>");

                        if (totalColspan == 4)
                        {
                            totalColspan = 0;
                            html.Append("</tr>");
                        }
                    }
                    if (totalColspan != 4)
                    {
                        html.Append("</tr>");
                    }
                    html.Append("</table></td></tr>");
                    //bool showColumn = true;
                    //bool isMobileColumn = false;
                    //bool isFirstCol = true;
                    //for (int j = 0; j < gridSettings.xmlGridColumns.Count; j++)
                    //{
                    //    string colName = gridSettings.xmlGridColumns[j].InnerText;

                    //    XmlNode xmlNode = gridSettings.xmlGridColumns[j];
                    //    if (isMobile)
                    //    {
                    //        showColumn = IsMobileColumn(xmlNode);
                    //        isMobileColumn = showColumn;
                    //    }
                    //    if (showColumn)
                    //    {
                    //        if (colName.Contains(" "))
                    //        {
                    //            colName = colName.Substring(colName.LastIndexOf(" ") + 1);
                    //        }
                    //        if (colName.Contains("."))
                    //        {
                    //            colName = colName.Substring(colName.IndexOf(".") + 1);
                    //        }
                    //        string headerText = gridSettings.xmlGridHeaders[j].InnerText;
                    //        string columnWidth = gridSettings.xmlGridColumnWidth[j].InnerText;
                    //        string val = Convert.ToString(Data.Rows[i][colName]);
                    //        string dataFormat = gridSettings.xmlGridColumnFormat[j].InnerText.ToLower();
                    //        string width = "";
                    //        string css = "";
                    //        string idval = "";
                    //        if (val == "01-01-2000 12:00:00 AM" || val == "01-01-1900 12:00:00 AM")
                    //        {
                    //            val = "";
                    //        }
                    //        if (j == 0 && idcol != "")
                    //        {
                    //            idval = "idval='" + Convert.ToString(Data.Rows[i][idcol]) + "'";
                    //            if (gridSettings.HiddenColumns.Trim() != "")
                    //            {
                    //                Array arrhidden = gridSettings.HiddenColumns.Split(',');
                    //                for (int k = 0; k < arrhidden.Length; k++)
                    //                {
                    //                    idval += " " + arrhidden.GetValue(k).ToString() + "='" + Data.Rows[i][arrhidden.GetValue(k).ToString()] + "'";
                    //                }
                    //            }
                    //        }
                    //        if (dataFormat == "Amount" || dataFormat == "Quantity")
                    //        {
                    //            css = " right ";
                    //        }
                    //        else if (dataFormat == "date")
                    //        {
                    //            if (val.Contains("1900") || val.Contains("2000") || val == "")
                    //            {
                    //                val = "";
                    //            }
                    //            else
                    //            {
                    //                val = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(val));
                    //            }
                    //        }
                    //        else if (dataFormat == "datetime")
                    //        {
                    //            if (val.Contains("1900") || val.Contains("2000") || val == "")
                    //            {
                    //                val = "";
                    //            }
                    //            else
                    //            {
                    //                val = GlobalUtilities.ConvertToDateTime(val);
                    //            }
                    //        }
                    //        if (colName.EndsWith("cramount") || colName.EndsWith("dramount"))
                    //        {
                    //            if (GlobalUtilities.ConvertToDouble(val) == 0) val = "";
                    //        }
                    //        if (columnWidth != "")
                    //        {
                    //            width = " width='" + columnWidth + "'";
                    //        }
                    //        if (isStatusColumnExists && colName.ToLower().Contains("status"))
                    //        {
                    //            statusColor_bg = GlobalUtilities.ConvertToString(Data.Rows[i][statusColorColumn_bg]);
                    //            statusColor_text = GlobalUtilities.ConvertToString(Data.Rows[i][statusColorColumn_text]);
                    //            if (statusColor_text == "")
                    //            {
                    //                statusColor_text = "#000000";
                    //            }
                    //            else
                    //            {
                    //                statusColor_text = "#" + statusColor_text;
                    //            }
                    //            statusColor_bg = "#" + statusColor_bg;
                    //            //if (!statusColor_bg.Contains("#")) statusColor_bg = "#" + statusColor_bg;
                    //            //if (!statusColor_text.Contains("#")) statusColor_text = "#" + statusColor_text;

                    //            val = "<div class='grid-status' style='background-color:" + statusColor_bg + ";color:" + statusColor_text + "'>" + val + "</div>";
                    //        }

                    //        if (dataFormat == "image")
                    //        {
                    //            string imgPath = GlobalUtilities.ConvertToString(gridSettings.xmlGridColumnFormat[j].Attributes["imgpath"].Value);

                    //            if (imgPath != "")
                    //            {
                    //                string fileName = "default.jpg";
                    //                if (val.Trim() != "")
                    //                {
                    //                    if (File.Exists(Server.MapPath("../" + imgPath + "/" + id + "." + val.Trim())))
                    //                    {
                    //                        fileName = id + "." + val.Trim();
                    //                    }
                    //                }
                    //                val = "<img src='../" + imgPath + "/" + fileName + "' height='30'/>";
                    //            }
                    //        }
                    //        if (val == "") val = "&nbsp;";
                    //        //row = Convert.ToInt32(gridSettings.xmlGridColumnRow[j].InnerText);
                    //        if (gridSettings.GridType == "list" && (j == 0 || row != prevRow))
                    //        {
                    //            html.Append("<tr>");
                    //        }
                    //        if (gridSettings.GridType == "list")
                    //        {
                    //            html.Append("<td>" + headerText + " : </td><td><b>" + val + "</b></td>");
                    //        }
                    //        else
                    //        {
                    //            if (j == 0) css = " idval";
                    //            string attrs = "";


                    //            if (isMobileColumn)
                    //            {
                    //                int colspan = 4;
                    //                string customClass = "";
                    //                for (int k = 0; k < xmlNode.ParentNode.Attributes.Count; k++)
                    //                {
                    //                    if (xmlNode.ParentNode.Attributes[k].Name == "colspan")
                    //                    {
                    //                        colspan = Convert.ToInt32(xmlNode.ParentNode.Attributes[k].Value);
                    //                    }
                    //                    else if (xmlNode.ParentNode.Attributes[k].Name == "class")
                    //                    {
                    //                        customClass = xmlNode.ParentNode.Attributes[k].Value;
                    //                    }
                    //                }
                    //                if (customClass != "") css = css + " " + customClass;

                    //                if (colspan == 2)
                    //                {
                    //                    html.Append("<td colspan='2' class='" + css + "' " + idval + ">" + val + "</td></tr><tr>");
                    //                }
                    //                else
                    //                {
                    //                    if (isFirstCol == false)
                    //                    {
                    //                        css += " right";
                    //                    }
                    //                    html.Append("<td class='" + css + "' " + idval + ">" + val + "</td>");
                    //                    if (isFirstCol == false)
                    //                    {
                    //                        html.Append("</tr><tr>");
                    //                    }
                    //                    isFirstCol = !isFirstCol;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                if (css != "") css = " class='" + css + "'";
                    //                html.Append("<td" + width + css + idval + ">" + val + "</td>");
                    //            }
                    //        }
                    //        if (gridSettings.GridType == "list" && row != prevRow && j > 0)
                    //        {
                    //            html.Append("</tr>");
                    //        }
                    //        prevRow = row;
                    //    }
                    //}
                    //if (isMobile)
                    //{
                    //    html.Append("</tr></table></td><td class='grid-arrow' align='right'>&nbsp;</td>");
                    //    html.Append("</tr></table></td>");
                    //}
                    //if (gridSettings.GridType != "list")
                    //{
                    //    html.Append("</tr>");
                    //}
                    //else
                    //{
                    //    if (row != prevRow)
                    //    {
                    //        html.Append("</tr>");
                    //    }
                    //}
                }
                else
                {

                    gridDataRowBind(Data, i, html);
                }
            }

            html.Append("<tr><td colspan='10' align='center' class='paging-count'>Showing 1 to " + Data.Rows.Count + " of " + gridSettings.TotalRecords);

            ////bind footer summary
            //if (GridSumColumns != "")
            //{
            //    string querySummary = _queryWithoutPaging;
            //    Array arrSumColumns = GridSumColumns.Split(',');
            //    string sumQuery = "";
            //    int count = 0;
            //    string sumValues = "";
            //    for (int i = 0; i < arrSumColumns.Length; i++)
            //    {
            //        bool isexists = false;
            //        string sumColumnName = Convert.ToString(arrSumColumns.GetValue(i));
            //        for (int j = 0; j < gridSettings.arrColumns_ColumnName.Length; j++)
            //        {
            //            if (Convert.ToString(gridSettings.arrColumns_ColumnName.GetValue(j)) == sumColumnName)
            //            {
            //                isexists = true;
            //                break;
            //            }
            //        }
            //        if (isexists)
            //        {
            //            if (count == 0)
            //            {
            //                sumQuery = "SUM(" + sumColumnName + ") AS " + sumColumnName + "_SUM";
            //            }
            //            else
            //            {
            //                sumQuery += ",SUM(" + sumColumnName + ") AS " + sumColumnName + "_SUM";
            //            }
            //            count++;
            //        }
            //    }
            //    if (count > 0)
            //    {
            //        //querySummary = querySummary.Replace("*", sumQuery);
            //        //int starIndex = querySummary.IndexOf("*");
            //        //if (starIndex > 0)
            //        //{
            //        //    querySummary = querySummary.Remove(starIndex, 1);
            //        //    querySummary = querySummary.Insert(starIndex, sumQuery);
            //        //}
            //        querySummary = "SELECT " + sumQuery + " FROM (" + querySummary + ")summresult";

            //        DataTable dttblSum = DbTable.ExecuteSelect(querySummary);
            //        if (GlobalUtilities.IsValidaTable(dttblSum))
            //        {
            //            ArrayList arrSumColumnHtml = new ArrayList();
            //            for (int i = 0; i < gridSettings.arrColumns_ColumnName.Length; i++)
            //            {
            //                string gridColumnName = Convert.ToString(gridSettings.arrColumns_ColumnName.GetValue(i));
            //                bool isexists = false;
            //                for (int j = 0; j < arrSumColumns.Length; j++)
            //                {
            //                    if (gridColumnName == Convert.ToString(arrSumColumns.GetValue(j)))
            //                    {
            //                        isexists = true;
            //                    }
            //                }
            //                if (isexists)
            //                {
            //                    string sumVal = Convert.ToString(dttblSum.Rows[0][gridColumnName + "_SUM"]);
            //                    if (gridColumnName.Contains("amount"))
            //                    {
            //                        sumVal = GlobalUtilities.FormatAmount(sumVal);
            //                    }
            //                    arrSumColumnHtml.Add("<td align='right'>" + sumVal + "</td>");
            //                    if (sumValues == "")
            //                    {
            //                        sumValues = sumVal;
            //                    }
            //                    else
            //                    {
            //                        sumValues += "," + sumVal;
            //                    }
            //                }
            //                else
            //                {
            //                    arrSumColumnHtml.Add("");
            //                }
            //            }
            //            int colspan = 1;
            //            int startColIndex = 0;
            //            for (int i = 0; i < arrSumColumnHtml.Count; i++)
            //            {
            //                if (Convert.ToString(arrSumColumnHtml[i]) != "")
            //                {
            //                    startColIndex = i;
            //                    break;
            //                }
            //            }
            //            colspan = startColIndex;
            //            if (gridSettings.IsEnableEdit && gridSettings.IsReport == false) colspan++;
            //            string colspanHtml = "";
            //            if (colspan > 1) colspanHtml = " colspan='" + colspan + "'";
            //            html.Append("<tr class='repeater-footer'><td" + colspanHtml + " align='right'>Total</td>");
            //            for (int i = startColIndex; i < arrSumColumnHtml.Count; i++)
            //            {
            //                if (Convert.ToString(arrSumColumnHtml[i]) == "")
            //                {
            //                    html.Append("<td>&nbsp;</td>");
            //                }
            //                else
            //                {
            //                    html.Append(Convert.ToString(arrSumColumnHtml[i]));
            //                }
            //            }
            //            html.Append("</tr>");
            //        }
            //        SumValues = sumValues;
            //    }
            //}
            ltMobile.Text = html.ToString();
            return;
        }
        if (Data == null)
        {
            trNodata.Visible = true;
        }
        else
        {
            if (Data.Rows.Count == 0)
            {
                trNodata.Visible = true;
            }
            else
            {
                trNodata.Visible = false;
            }
            //bind chart if exists
            if (bindChartUsingGrid != null) bindChartUsingGrid(Data);
        }
        if (gridSettings.EnablePaging) BindPaging();

    }
    public void BindGridData()
    {
        bool isMobile = IsMobileView;
        if (isMobile)
        {
            tblMobile.Visible = true;
        }
        else
        {
            tblNonMobile.Visible = true;
        }
        if (isMobile)
        {
            BindMobileGridData();
            return;
        }

        _index = 0;
        if (!gridSettings.IsEnableEdit)// && gridSettings.IsReport == false)
        {
            tdEdit.Visible = false;
        }
        if (gridSettings.GridType == "grid")
        {
            BindHead();
        }

        //rpt.DataSource = Data;
        //rpt.DataBind();
        trNodata.Visible = false;
        if (Data != null)
        {
            if (!GlobalUtilities.IsValidaTable(Data) && isMobile)
            {
                ltMobile.Text = "<div class='error'>No data found!</div>";

                return;
            }
            trNodata.Visible = true;
            StringBuilder html = new StringBuilder();
            string m = gridSettings.Module.ToLower();
            if (m == "") m = Module.ToLower();
            if (m.StartsWith("view")) m = m.Substring(4);

            tblMobile.Attributes.Add("m", m);
            tblData.Attributes.Add("m", m);

            string idcol = "";
            if (gridSettings.IdentityColumn == "")
            {
                idcol = m + "_" + m + "id";
            }
            else
            {
                idcol = gridSettings.IdentityColumn.Replace("\n", "").Replace("\t", "").Trim();
            }
            if (idcol == "none") idcol = "";
            m = gridSettings.TableName.Replace("tbl_", "");
            //string idcol = gridSettings.TableName.Replace("tbl_", "");
            //idcol = idcol + "_" + idcol + "id";

            bool isStatusColumnExists = false;
            string statusColorColumn_bg = "";
            string statusColorColumn_text = "";
            string statusColor_bg = "";
            string statusColor_text = "";

            if (!Module.ToLower().Contains("status") || gridSettings.IsReport)
            {
                for (int i = 0; i < Data.Columns.Count; i++)
                {
                    if (Data.Columns[i].ColumnName.ToLower().EndsWith("statusid"))
                    {
                        string statusColumnName = Data.Columns[i].ColumnName.ToLower();
                        if (statusColumnName.IndexOf("_") > 0)
                        {
                            statusColumnName = statusColumnName.Substring(statusColumnName.IndexOf("_") + 1);
                            statusColumnName = statusColumnName.Substring(0, statusColumnName.Length - 2);
                            if (Data.Columns.Contains(statusColumnName + "_textcolor"))
                            {
                                statusColorColumn_text = statusColumnName + "_textcolor";
                                statusColorColumn_bg = statusColumnName + "_backgroundcolor";
                                isStatusColumnExists = true;
                            }
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < Data.Rows.Count; i++)
            {
                if (gridDataRowBind == null)
                {
                    string rptClass = "repeater-row";
                    if (i % 2 == 0)
                    {
                        rptClass = "repeater-alt";
                    }
                    if (isMobile) rptClass = rptClass + "-m";
                    if (gridSettings.GridType != "list")
                    {
                        if (isMobile)
                        {
                            html.Append("<tr>");
                        }
                        else
                        {
                            html.Append("<tr class='" + rptClass + "'>");
                        }
                    }
                    int row = 0;
                    int prevRow = 0;
                    int id = 0;

                    if (idcol != "" && isMobile == false)
                    {
                        id = GlobalUtilities.ConvertToInt(Data.Rows[i][idcol]);
                        if (gridSettings.IsEnableEdit)// && gridSettings.IsReport == false)
                        {
                            html.Append("<td class='gedit'>&nbsp;</td>");
                        }
                    }
                    if (isMobile)
                    {
                        html.Append("<td class='" + rptClass + "'><table width='100%'><tr>");
                        html.Append("<td><table width='100%'><tr>");
                    }
                    bool showColumn = true;
                    bool isMobileColumn = false;
                    bool isFirstCol = true;
                    for (int j = 0; j < gridSettings.xmlGridColumns.Count; j++)
                    {
                        string colName = gridSettings.xmlGridColumns[j].InnerText;
                        XmlNode xmlNode = gridSettings.xmlGridColumns[j];
                        if (isMobile)
                        {
                            showColumn = IsMobileColumn(xmlNode);
                            isMobileColumn = showColumn;
                        }
                        if (showColumn)
                        {
                            if (colName.Contains(" "))
                            {
                                colName = colName.Substring(colName.LastIndexOf(" ") + 1);
                            }
                            if (colName.Contains("."))
                            {
                                colName = colName.Substring(colName.IndexOf(".") + 1);
                            }
                            if (colName == "bankaudit_download")
                            {
                                int bankAuditStatusId = GlobalUtilities.ConvertToInt(Data.Rows[i]["bankaudit_bankauditstatusid"]);
                                if (bankAuditStatusId == 4)
                                {
                                    string guid = GlobalUtilities.ConvertToString(Data.Rows[i]["bankaudit_guid"]);
                                    html.Append(@"<td><a href='../download-file.aspx?f=bankaudit/doc/" + id + "/AuditReport-" + guid + ".pdf'" + " target='_blank'>Download</a></td>");
                                }
                                else
                                {
                                    html.Append(@"<td></td>");
                                }
                            }
                            else if (Data.Columns.Contains(colName))
                            {
                                string headerText = gridSettings.xmlGridHeaders[j].InnerText;
                                string columnWidth = gridSettings.xmlGridColumnWidth[j].InnerText;
                                string val = Convert.ToString(Data.Rows[i][colName]);
                                string dataFormat = gridSettings.xmlGridColumnFormat[j].InnerText.ToLower();
                                string width = "";
                                string css = "";
                                string idval = "";
                                if (val == "01-01-2000 12:00:00 AM" || val == "01-01-1900 12:00:00 AM")
                                {
                                    val = "";
                                }
                                if (j == 0 && idcol != "")
                                {
                                    idval = "idval='" + Convert.ToString(Data.Rows[i][idcol]) + "'";
                                    if (gridSettings.HiddenColumns.Trim() != "")
                                    {
                                        Array arrhidden = gridSettings.HiddenColumns.Split(',');
                                        for (int k = 0; k < arrhidden.Length; k++)
                                        {
                                            idval += " " + arrhidden.GetValue(k).ToString() + "='" + Data.Rows[i][arrhidden.GetValue(k).ToString()] + "'";
                                        }
                                    }
                                }
                                if (dataFormat == "Amount" || dataFormat == "Quantity")
                                {
                                    css = " right ";
                                }
                                else if (dataFormat == "date")
                                {
                                    if (val.Contains("1900") || val.Contains("2000") || val == "")
                                    {
                                        val = "";
                                    }
                                    else
                                    {
                                        val = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(val));
                                    }
                                }
                                else if (dataFormat == "datetime")
                                {
                                    if (val.Contains("1900") || val.Contains("2000") || val == "")
                                    {
                                        val = "";
                                    }
                                    else
                                    {
                                        val = GlobalUtilities.ConvertToDateTime(Data.Rows[i][colName]);
                                    }
                                }
                                if (colName.EndsWith("cramount") || colName.EndsWith("dramount"))
                                {
                                    if (GlobalUtilities.ConvertToDouble(val) == 0) val = "";
                                }
                                if (columnWidth != "")
                                {
                                    width = " width='" + columnWidth + "'";
                                }
                                if (isStatusColumnExists && colName.ToLower().Contains("status"))
                                {
                                    statusColor_bg = GlobalUtilities.ConvertToString(Data.Rows[i][statusColorColumn_bg]);
                                    statusColor_text = GlobalUtilities.ConvertToString(Data.Rows[i][statusColorColumn_text]);
                                    if (statusColor_text == "")
                                    {
                                        statusColor_text = "#000000";
                                    }
                                    else
                                    {
                                        statusColor_text = "#" + statusColor_text;
                                    }
                                    statusColor_bg = "#" + statusColor_bg;
                                    //if (!statusColor_bg.Contains("#")) statusColor_bg = "#" + statusColor_bg;
                                    //if (!statusColor_text.Contains("#")) statusColor_text = "#" + statusColor_text;

                                    val = "<div class='grid-status' style='background-color:" + statusColor_bg + ";color:" + statusColor_text + "'>" + val + "</div>";
                                }

                                if (dataFormat == "image")
                                {
                                    string imgPath = GlobalUtilities.ConvertToString(gridSettings.xmlGridColumnFormat[j].Attributes["imgpath"].Value);

                                    if (imgPath != "")
                                    {
                                        string fileName = "default.jpg";
                                        if (val.Trim() != "")
                                        {
                                            if (File.Exists(Server.MapPath("../" + imgPath + "/" + id + "." + val.Trim())))
                                            {
                                                fileName = id + "." + val.Trim();
                                            }
                                        }
                                        val = "<img src='../" + imgPath + "/" + fileName + "' height='30'/>";
                                    }
                                }
                                if (val == "") val = "&nbsp;";
                                //row = Convert.ToInt32(gridSettings.xmlGridColumnRow[j].InnerText);
                                if (gridSettings.GridType == "list" && (j == 0 || row != prevRow))
                                {
                                    html.Append("<tr>");
                                }
                                if (gridSettings.GridType == "list")
                                {
                                    html.Append("<td>" + headerText + " : </td><td><b>" + val + "</b></td>");
                                }
                                else
                                {
                                    if (j == 0) css = " idval";
                                    string attrs = "";
                                    //if (colName.StartsWith(m.ToLower() + "_"))
                                    //{
                                    //}
                                    //else
                                    //{
                                    //    if (colName.Contains("_"))
                                    //    {
                                    //        string submodule = colName.Substring(0, colName.IndexOf("_"));
                                    //        if (submodule != "")
                                    //        {
                                    //            int sid = GlobalUtilities.ConvertToInt(Data.Rows[i][submodule + "_" + submodule + "id"]);
                                    //            attrs = " href='#" + submodule + "/add.aspx?id=" + sid + "' ";
                                    //        }
                                    //    }
                                    //}

                                    if (isMobileColumn)
                                    {
                                        int colspan = 4;
                                        string customClass = "";
                                        for (int k = 0; k < xmlNode.ParentNode.Attributes.Count; k++)
                                        {
                                            if (xmlNode.ParentNode.Attributes[k].Name == "colspan")
                                            {
                                                colspan = Convert.ToInt32(xmlNode.ParentNode.Attributes[k].Value);
                                            }
                                            else if (xmlNode.ParentNode.Attributes[k].Name == "class")
                                            {
                                                customClass = xmlNode.ParentNode.Attributes[k].Value;
                                            }
                                        }
                                        if (customClass != "") css = css + " " + customClass;

                                        if (colspan == 2)
                                        {
                                            html.Append("<td colspan='2' class='" + css + "' " + idval + ">" + val + "</td></tr><tr>");
                                        }
                                        else
                                        {
                                            if (isFirstCol == false)
                                            {
                                                css += " right";
                                            }
                                            html.Append("<td class='" + css + "' " + idval + ">" + val + "</td>");
                                            if (isFirstCol == false)
                                            {
                                                html.Append("</tr><tr>");
                                            }
                                            isFirstCol = !isFirstCol;
                                        }
                                    }
                                    else
                                    {
                                        if (css != "") css = " class='" + css + "'";
                                        html.Append("<td" + width + css + idval + ">" + val + "</td>");
                                    }
                                }
                                if (gridSettings.GridType == "list" && row != prevRow && j > 0)
                                {
                                    html.Append("</tr>");
                                }
                                prevRow = row;
                            }
                        }
                    }
                    if (isMobile)
                    {
                        //html.Append("<td class='grid-arrow' align='right'>&nbsp;</td>");
                        html.Append("</tr></table></td><td class='grid-arrow' align='right'>&nbsp;</td>");
                        html.Append("</tr></table></td>");
                    }
                    if (gridSettings.GridType != "list")
                    {
                        if (Module.ToLower() == "subscription")
                        {
                            html.Append("<td width='40'><input type='button' class='view-button jq-viewfemportal' value='FEM'/></td>");
                        }
                        html.Append("</tr>");
                    }
                    else
                    {
                        if (row != prevRow)
                        {
                            html.Append("</tr>");
                        }
                    }
                }
                else
                {

                    gridDataRowBind(Data, i, html);
                }
            }
            if (isMobile)
            {
                html.Append("<tr><td colspan='10' align='center' class='paging-count'>Showing 1 to " + Data.Rows.Count + " of " + gridSettings.TotalRecords);
            }
            //bind footer summary
            if (GridSumColumns != "")
            {
                string querySummary = _queryWithoutPaging;
                Array arrSumColumns = GridSumColumns.Split(',');
                string sumQuery = "";
                int count = 0;
                string sumValues = "";
                for (int i = 0; i < arrSumColumns.Length; i++)
                {
                    bool isexists = false;
                    string sumColumnName = Convert.ToString(arrSumColumns.GetValue(i));
                    for (int j = 0; j < gridSettings.arrColumns_ColumnName.Length; j++)
                    {
                        if (Convert.ToString(gridSettings.arrColumns_ColumnName.GetValue(j)) == sumColumnName)
                        {
                            isexists = true;
                            break;
                        }
                    }
                    if (isexists)
                    {
                        if (count == 0)
                        {
                            sumQuery = "SUM(" + sumColumnName + ") AS " + sumColumnName + "_SUM";
                        }
                        else
                        {
                            sumQuery += ",SUM(" + sumColumnName + ") AS " + sumColumnName + "_SUM";
                        }
                        count++;
                    }
                }
                if (count > 0)
                {
                    //querySummary = querySummary.Replace("*", sumQuery);
                    //int starIndex = querySummary.IndexOf("*");
                    //if (starIndex > 0)
                    //{
                    //    querySummary = querySummary.Remove(starIndex, 1);
                    //    querySummary = querySummary.Insert(starIndex, sumQuery);
                    //}
                    querySummary = "SELECT " + sumQuery + " FROM (" + querySummary + ")summresult";

                    DataTable dttblSum = DbTable.ExecuteSelect(querySummary);
                    if (GlobalUtilities.IsValidaTable(dttblSum))
                    {
                        ArrayList arrSumColumnHtml = new ArrayList();
                        for (int i = 0; i < gridSettings.arrColumns_ColumnName.Length; i++)
                        {
                            string gridColumnName = Convert.ToString(gridSettings.arrColumns_ColumnName.GetValue(i));
                            bool isexists = false;
                            for (int j = 0; j < arrSumColumns.Length; j++)
                            {
                                if (gridColumnName == Convert.ToString(arrSumColumns.GetValue(j)))
                                {
                                    isexists = true;
                                }
                            }
                            if (isexists)
                            {
                                string sumVal = Convert.ToString(dttblSum.Rows[0][gridColumnName + "_SUM"]);
                                if (gridColumnName.Contains("amount"))
                                {
                                    sumVal = GlobalUtilities.FormatAmount(sumVal);
                                }
                                arrSumColumnHtml.Add("<td align='right'>" + sumVal + "</td>");
                                if (sumValues == "")
                                {
                                    sumValues = sumVal;
                                }
                                else
                                {
                                    sumValues += "," + sumVal;
                                }
                            }
                            else
                            {
                                arrSumColumnHtml.Add("");
                            }
                        }
                        int colspan = 1;
                        int startColIndex = 0;
                        for (int i = 0; i < arrSumColumnHtml.Count; i++)
                        {
                            if (Convert.ToString(arrSumColumnHtml[i]) != "")
                            {
                                startColIndex = i;
                                break;
                            }
                        }
                        colspan = startColIndex;
                        if (gridSettings.IsEnableEdit && gridSettings.IsReport == false) colspan++;
                        string colspanHtml = "";
                        if (colspan > 1) colspanHtml = " colspan='" + colspan + "'";
                        html.Append("<tr class='repeater-footer'><td" + colspanHtml + " align='right'>Total</td>");
                        for (int i = startColIndex; i < arrSumColumnHtml.Count; i++)
                        {
                            if (Convert.ToString(arrSumColumnHtml[i]) == "")
                            {
                                html.Append("<td>&nbsp;</td>");
                            }
                            else
                            {
                                html.Append(Convert.ToString(arrSumColumnHtml[i]));
                            }
                        }
                        html.Append("</tr>");
                    }
                    SumValues = sumValues;
                }
            }
            if (isMobile)
            {
                ltMobile.Text = html.ToString();
                return;
            }
            else
            {
                ltGrid.Text = html.ToString();
            }
        }
        if (Data == null)
        {
            trNodata.Visible = true;
        }
        else
        {
            if (Data.Rows.Count == 0)
            {
                trNodata.Visible = true;
            }
            else
            {
                trNodata.Visible = false;
            }
            //bind chart if exists
            if (bindChartUsingGrid != null) bindChartUsingGrid(Data);
        }
        if (gridSettings.EnablePaging) BindPaging();

    }
    private bool IsMobileColumn(XmlNode xmlNode)
    {
        xmlNode = xmlNode.ParentNode;
        for (int i = 0; i < xmlNode.Attributes.Count; i++)
        {
            if (xmlNode.Attributes[i].Name == "ismobile")
            {
                return true;
            }
        }
        return false;
    }
    private void BindHead()
    {
        rptHead.DataSource = gridSettings.arrColumns_HeaderText;
        rptHead.DataBind();

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        CurrentPageIndex = 0;
        ddlPageSize.SelectedIndex = 0;
        IsSearch = true;
        GetData();
        BindGridData();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (Common.RoleId > 1)
        {
            if (!Common.IsExportRights(Module))
            {
                Response.Redirect("~/access/noaccess.aspx");
                return;
            }
        }
        DataTable dttblExcelData = DbTable.ExecuteSelect(QueryWithoutPaging);
        ArrayList extraColumns = new ArrayList();
        if (Module.ToLower() == "invoice")
        {
            string subquery = QueryWithoutPaging;
            int index = subquery.IndexOf("*");
            subquery = subquery.Remove(index, 1);
            subquery = subquery.Insert(index, "invoice_invoiceid");
            string query = @"select * from tbl_invoiceprofitsharingdetail
                            join tbl_employee on employee_employeeid=invoiceprofitsharingdetail_employeeid
                            where invoiceprofitsharingdetail_invoiceid in(" + subquery + @")
                            order by invoiceprofitsharingdetail_invoiceprofitsharingdetailid";
            DataTable dttblProfit = DbTable.ExecuteSelect(query);
            if (GlobalUtilities.IsValidaTable(dttblProfit))
            {

                for (int i = 0; i < dttblExcelData.Rows.Count; i++)
                {
                    int invoiceId = GlobalUtilities.ConvertToInt(dttblExcelData.Rows[i]["invoice_invoiceid"]);
                    DataRow[] drs = dttblProfit.Select("invoiceprofitsharingdetail_invoiceid=" + invoiceId);
                    for (int j = 0; j < drs.Length; j++)
                    {
                        DataRow dr1 = (DataRow)drs.GetValue(j);
                        string empName = GlobalUtilities.ConvertToString(dr1["employee_employeename"]);
                        double sharingPer = GlobalUtilities.ConvertToDouble(dr1["invoiceprofitsharingdetail_profitpercentage"]);
                        double sharingAmount = GlobalUtilities.ConvertToDouble(dr1["invoiceprofitsharingdetail_profitamount"]);
                        if (!dttblExcelData.Columns.Contains("Employee Name " + (j + 1)))
                        {
                            dttblExcelData.Columns.Add("Employee Name " + (j + 1));
                            extraColumns.Add("Employee Name " + (j + 1));
                        }
                        if (!dttblExcelData.Columns.Contains("Sharing Percentage " + (j + 1)))
                        {
                            dttblExcelData.Columns.Add("Sharing Percentage " + (j + 1));
                            extraColumns.Add("Sharing Percentage " + (j + 1));
                        }
                        if (!dttblExcelData.Columns.Contains("Sharing Amount " + (j + 1)))
                        {
                            dttblExcelData.Columns.Add("Sharing Amount " + (j + 1));
                            extraColumns.Add("Sharing Amount " + (j + 1));
                        }
                        DataRow dr = dttblExcelData.Rows[i];
                        dr["Employee Name " + (j + 1)] = empName;
                        dr["Sharing Percentage " + (j + 1)] = sharingPer;
                        dr["Sharing Amount " + (j + 1)] = sharingAmount;
                    }
                }
            }
        }
        ExportToExcel(dttblExcelData, extraColumns, extraColumns);
    }
    public void ExportToExcel(DataTable dt, ArrayList arrExtraHeader, ArrayList arrExtraCol)
    {
        //if (dt.Rows.Count > 0)
        {
            string filename = "data.xls";
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            DataGrid dgGrid = new DataGrid();
            DataTable dttblData = new DataTable();
            bool issrno = false;
            if (!Request.Url.ToString().ToLower().Contains("/whatsapp")) issrno = true;
            if (issrno)
            {
                dttblData.Columns.Add("Sr No");
            }
            ArrayList arrHeader = new ArrayList();
            ArrayList arrColumn = new ArrayList();
            for (int i = 0; i < gridSettings.arrColumns_HeaderText.Length; i++)
            {
                arrHeader.Add(Convert.ToString(gridSettings.arrColumns_HeaderText.GetValue(i)));
            }
            for (int i = 0; i < arrExtraHeader.Count; i++)
            {
                arrHeader.Add(Convert.ToString(arrExtraHeader[i]));
            }
            for (int i = 0; i < gridSettings.arrColumns_ColumnName.Length; i++)
            {
                arrColumn.Add(Convert.ToString(gridSettings.arrColumns_ColumnName.GetValue(i)));
            }
            for (int i = 0; i < arrExtraCol.Count; i++)
            {
                arrColumn.Add(Convert.ToString(arrExtraCol[i]));
            }

            //for (int i = 0; i < arrColumn.Count; i++)
            //{
            //    dttblData.Columns.Add(Convert.ToString(arrColumn[i]));
            //}
            for (int i = 0; i < arrHeader.Count; i++)
            {
                dttblData.Columns.Add(Convert.ToString(arrHeader[i]));
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dttblData.NewRow();
                if (issrno)
                {
                    dr["Sr No"] = i + 1;
                }
                for (int j = 0; j < arrColumn.Count; j++)
                {
                    string columnName = Convert.ToString(arrColumn[j]);
                    if (dt.Columns[columnName].DataType == typeof(DateTime))
                    {
                        dr[arrHeader[j].ToString()] = GlobalUtilities.ConvertToDate(dt.Rows[i][columnName]);
                    }
                    else
                    {
                        string val = Convert.ToString(dt.Rows[i][columnName]);
                        dr[arrHeader[j].ToString()] = val;
                    }
                }
                dttblData.Rows.Add(dr);
            }
            if (GridSumColumns != "")
            {
                //add sum values
                if (SumValues != "")
                {
                    Array arrSumValues = SumValues.Split(',');
                    Array arrSumColumns = GridSumColumns.Split(',');
                    DataRow dr = dttblData.NewRow();
                    dr[0] = "Total";
                    int sumColumnCounter = 0;
                    for (int i = 0; i < arrHeader.Count; i++)
                    {
                        string gridColumn = Convert.ToString(arrColumn[i]);
                        for (int j = 0; j < arrSumColumns.Length; j++)
                        {
                            if (gridColumn == Convert.ToString(arrSumColumns.GetValue(j)))
                            {
                                dr[Convert.ToString(arrColumn[i])] = Convert.ToString(arrSumValues.GetValue(sumColumnCounter));
                                sumColumnCounter++;
                            }
                        }

                    }

                    dttblData.Rows.Add(dr);
                }
            }
            //dgGrid.DataSource = dttblData;
            //dgGrid.DataBind();
            GridView gv = new GridView();
            gv.DataSource = dttblData;
            gv.DataBind();

            for (int i = 0; i < gv.Rows.Count; i++)
            {
                GridViewRow row = gv.Rows[i];
                //Apply text style to each Row
                for (int j = 0; j < gv.Rows[i].Cells.Count; j++)
                {
                    if (gv.HeaderRow.Cells[j].Text.ToLower().Replace(" ", "").Contains("mobileno"))
                    {
                        gv.Rows[i].Cells[j].Attributes.Add("class", "textmode");
                    }
                }
            }

            //Get the HTML for the control.
            gv.RenderControl(hw);

            //Write the HTML back to the browser.
            //Response.ContentType = application/vnd.ms-excel;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
            //Page.EnableViewState = false;
            //format the excel cells to text format
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.ContentType = "application/text";

            HttpContext.Current.Response.Write(tw.ToString());
            HttpContext.Current.Response.End();
        }
    }
    public string Keyword
    {
        get
        {
            return txtKeyword.Text;
        }
        set
        {
            txtKeyword.Text = value;
        }
    }
    protected void rptHead_OnItemDataBound(object source, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            LinkButton lnkHead = (LinkButton)e.Item.FindControl("lnkHead");
            Label lblHead = (Label)e.Item.FindControl("lblHead");

            string headerText = Convert.ToString(gridSettings.arrColumns_HeaderText.GetValue(e.Item.ItemIndex));
            string columnName = Convert.ToString(gridSettings.arrColumns_ColumnName.GetValue(e.Item.ItemIndex));
            if (gridSettings.EnableSorting)
            {
                lblHead.Visible = false;
                lnkHead.CommandArgument = columnName;
                lnkHead.Attributes.Add("col", columnName);
                lnkHead.Text = headerText;
                if (columnName.Substring(columnName.IndexOf("_") + 1).ToLower() == gridSettings.Namefield.ToLower())
                {
                    lnkHead.Attributes.Add("nf", columnName.Substring(columnName.IndexOf("_") + 1));
                }
                string dataFormat = gridSettings.xmlGridColumnFormat[e.Item.ItemIndex].InnerText.ToLower();
                if (dataFormat == "image")
                {
                    string imgPath = GlobalUtilities.ConvertToString(gridSettings.xmlGridColumnFormat[e.Item.ItemIndex].Attributes["imgpath"].Value);
                    lnkHead.Attributes.Add("img", imgPath);
                }
            }
            else
            {
                lnkHead.Visible = false;

                lblHead.Text = headerText;
            }
            if (gridSettings.xmlGridColumnWidth[e.Item.ItemIndex] != null)
            {
                HtmlControl tdHeader = (HtmlControl)e.Item.FindControl("tdHeader");
                string columnWidth = gridSettings.xmlGridColumnWidth[e.Item.ItemIndex].InnerText;
                if (columnWidth != "")
                {
                    tdHeader.Attributes.Add("width", columnWidth);
                }
            }
        }
    }
    protected void lnkHead_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "ColumnName")
        {
            string ColumnName = e.CommandArgument.ToString();
            //int index = SortBy.IndexOf(ColumnName + "=");
            //Array arrSortBy = SortBy.Split(',');
            //int OrderBy = 0;
            //for (int i = 0; i < arrSortBy.Length; i++)
            //{
            //    Array arr = Convert.ToString(arrSortBy.GetValue(i)).Split('=');
            //    if (Convert.ToString(arr.GetValue(0)) == ColumnName)
            //    {
            //        OrderBy = Convert.ToInt32(arr.GetValue(1));
            //        break;
            //    }
            //}
            if (PreviousSortColumn == ColumnName)
            {
                if (IsAscending)
                {
                    IsAscending = false;
                }
                else
                {
                    IsAscending = true;
                    //OrderBy = OrderBy + 1;
                }
            }
            else
            {
                IsAscending = true;
                //OrderBy = OrderBy + 1;
            }
            //CurrentSort = OrderBy;
            CurrentSort = ColumnName;
            PreviousSortColumn = ColumnName;
            //CurrentPageIndex = Convert.ToInt32(e.CommandArgument);
            GetData();
            BindGridData();
        }
    }
    protected void rpt_OnItemDataBound(object source, RepeaterItemEventArgs e)
    {
        StringBuilder html = new StringBuilder();
        Literal ltItem = new Literal();
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ltItem = (Literal)e.Item.FindControl("ltItem");
            if (_index % 2 == 0)
                html.Append("<tr class='repeater-row'>");
            else
                html.Append("<tr class='repeater-alt'>");
            string columnName = "";
            string data = "";
            int N = gridSettings.xmlGridColumns.Count;
            if (gridSettings.EditUrl != "")
            {
                N = N - 1;
            }
            int Start = 0;

            Array arrhidden;
            arrhidden = gridSettings.HiddenColumns.Split(',');

            for (int j = 0; j < arrhidden.Length; j++)
            {
                columnName = arrhidden.GetValue(j).ToString();
                string shortColName = columnName.Substring(columnName.IndexOf("_") + 1);
                html.Append("<td class='hidden' cn='" + shortColName + "'>" + Convert.ToString(Data.Rows[_index][columnName]) + "</td>");

            }
            Start = 0;
            for (int i = Start; i < N; i++)
            {
                columnName = gridSettings.xmlGridColumns[i].InnerText;
                columnName = columnName.Replace(" ", "");
                if (columnName.IndexOf('.') > 0)
                {
                    columnName = columnName.Substring(columnName.IndexOf('.') + 1);
                }
                data = Convert.ToString(Data.Rows[_index][columnName]);
                if (data.Trim() == "") data = "&nbsp;";

                if (columnName.ToLower().Contains("date") || columnName.ToLower().Contains("dob"))
                {
                    if (data != "&nbsp;")
                    {
                        data = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(data));
                    }
                }
                if (i == 0 && Convert.ToString(gridSettings.ViewUrl) != "")
                {
                    string newWindow = "";
                    if (gridSettings.ViewInNewWindow)
                    {
                        newWindow = "target='_blank'";
                    }
                    data = "<a " + newWindow + " href='" + gridSettings.ViewUrl + "?" + Convert.ToString(gridSettings.arrColumns_ColumnName.GetValue(0)) + "=" + Data.Rows[_index][Convert.ToString(gridSettings.arrColumns_ColumnName.GetValue(0))] + "'>" + data + "</a>";
                }
                string hiddenvalues = "";
                if (i == 0)
                {

                    for (int j = 0; j < arrhidden.Length; j++)
                    {
                        if (j == 0)
                        {
                            hiddenvalues = " hv=" + Convert.ToString(arrhidden.GetValue(j)) + "~" + Convert.ToString(Data.Rows[_index][Convert.ToString(arrhidden.GetValue(j))]);
                        }
                        else
                        {
                            hiddenvalues = "~" + Convert.ToString(arrhidden.GetValue(j)) + "~" + Convert.ToString(Data.Rows[_index][Convert.ToString(arrhidden.GetValue(j))]);
                        }
                    }
                }
                string shortColName = columnName.Substring(columnName.IndexOf("_") + 1);
                if (columnName.ToLower().Contains("amount"))
                {
                    html.Append("<td class='right' cn='" + shortColName + "'>" + data + "</td>");
                }
                else
                {
                    html.Append("<td cn='" + shortColName + "'>" + data + "</td>");
                }
            }
            if (gridSettings.EditUrl != "")
            {
                string queryString = "";
                if (QueryStringVariables == "")
                {
                    queryString = Convert.ToString(gridSettings.arrColumns_ColumnName.GetValue(0)) + "="
                    + Data.Rows[_index][Convert.ToString(gridSettings.arrColumns_ColumnName.GetValue(0))];
                }
                else
                {
                    Array arrQueryStringVar = QueryStringVariables.Split(',');
                    for (int i = 0; i < arrQueryStringVar.Length; i++)
                    {
                        string queryVar = arrQueryStringVar.GetValue(i).ToString();
                        if (!gridSettings.EditUrl.Contains(queryVar + "="))
                        {
                            if (queryString == "")
                            {
                                queryString = queryVar + "=" + Data.Rows[_index][queryVar];
                            }
                            else
                            {
                                queryString += "&" + queryVar + "=" + Data.Rows[_index][queryVar];
                            }
                        }
                    }
                }
                if (gridSettings.IsEnableEdit)// && gridSettings.IsReport == false)
                {
                    if (gridSettings.EditUrl.Contains("?"))
                    {
                        html.Append("<td><a href='" + gridSettings.EditUrl + "&" + queryString + "'>Edit</a></td>");
                    }
                    else
                    {
                        html.Append("<td><a href='" + gridSettings.EditUrl + "?" + queryString + "'>Edit</a></td>");
                    }
                }
            }
            html.Append("</tr>");
            ltItem.Text = html.ToString();
            _index++;
        }
    }
    private void BindPaging()
    {
        int startIndex = 1;// (CurrentPageIndex - 1) * PageSize + 1;
        int endIndex = gridSettings.TotalPages;//CurrentPageIndex * PageSize;
        SetTotal();
        for (int i = 0; i < gridSettings.TotalPages; i++)
        {
            int PageBoxStart = gridSettings.MaxPages * i;
            int PageBoxEnd = gridSettings.MaxPages * (i + 1);
            if (CurrentPageIndex >= PageBoxStart && CurrentPageIndex <= PageBoxEnd)
            {
                startIndex = PageBoxStart + 1;
                endIndex = startIndex + gridSettings.MaxPages - 1;
                if (endIndex > gridSettings.TotalPages)
                {
                    endIndex = gridSettings.TotalPages;
                    break;
                }
            }
        }

        //if (endIndex > TotalRecords)
        //{
        //    endIndex = TotalRecords;
        //}
        ArrayList arr = new ArrayList();
        for (int i = startIndex; i <= endIndex; i++)
        {
            arr.Add(i);
        }
        rptPaging.DataSource = arr;
        rptPaging.DataBind();
        int row = 0;

        for (int i = startIndex; i <= endIndex; i++)
        {
            LinkButton lnk = (LinkButton)rptPaging.Items[row].FindControl("lnkPage");
            lnk.Text = "&nbsp;" + i.ToString() + "&nbsp;";
            lnk.CommandArgument = Convert.ToString(i - 1);
            if (i - 1 == CurrentPageIndex)
            {
                //lnk.Attributes.Add("class", "paging-active");
                HtmlControl td = (HtmlControl)rptPaging.Items[row].FindControl("page_td");
                td.Style.Add("background-color", "#0f65f1;");
                lnk.Style.Add("color", "#ffffff");
            }
            row++;
        }
    }
    protected void lnkPage_OnCommand(object sender, CommandEventArgs e)
    {
        CurrentPageIndex = Convert.ToInt32(e.CommandArgument);
        if (CurrentPageIndex + 1 == gridSettings.TotalPages)
        {
            lnkNextPage.Enabled = false;
        }
        else
        {
            lnkNextPage.Enabled = true;
        }
        if (CurrentPageIndex == 0)
        {
            lnkPrevPage.Enabled = false;
        }
        else
        {
            lnkPrevPage.Enabled = true;
        }
        GetData();
        BindGridData();
    }
    public void Report()
    {
        gridSettings = new GridSettings();
        gridSettings.BindXml(Module);
        IsReport = gridSettings.IsReport;
        imgGridSetting.Attributes.Add("m", Module.ToLower());
        GetData();
        BindGridData();
    }
    public bool IsReport
    {
        get
        {
            if (ViewState["IsReport"] == null) return gridSettings.IsReport;
            return Convert.ToBoolean(ViewState["IsReport"]);
        }
        set
        {
            ViewState["IsReport"] = value;
        }
    }

    private string GetWhere(string whereQuery)
    {
        if (SearchHolder == null) return whereQuery;
        ArrayList arrControls = new ArrayList();
        ArrayList arrControls_tmp = new ArrayList();
        GlobalUtilities.GetControls(SearchHolder, arrControls);
        //for (int i = 0; i < arrControls_tmp.Count; i++)
        //{
        //    Control control = (Control)arrControls_tmp[i];
        //    if (control.ClientID.Contains("reportfield_"))
        //    {
        //        ArrayList arrControls_tmp2 = new ArrayList();
        //        GlobalUtilities.GetControls(SearchHolder, arrControls_tmp2);
        //        for (int j = 0; j < arrControls_tmp2.Count; j++)
        //        {
        //            arrControls.Add(arrControls_tmp2[j]);
        //        }
        //    }
        //}
        for (int i = 0; i < arrControls.Count; i++)
        {
            string id = "";
            string val = "";
            bool isinclude = true;
            Control control = (Control)arrControls[i];
            Control parentControl = control.Parent;

            //if (parentControl.Visible)
            {
                if (control is DropDownList)
                {
                    DropDownList ddl = (DropDownList)arrControls[i];
                    id = ddl.ID.ToLower().Replace("ddl", "");
                    val = ddl.SelectedValue;
                }
                if (control is TextBox)
                {
                    TextBox txt = (TextBox)arrControls[i];
                    
                    id = txt.ID.ToLower().Replace("txt", "");
                    if (whereQuery.Contains("$txt" + id))
                    {
                        whereQuery = whereQuery.Replace("$txt" + txt.ID.ToLower() + "$", txt.Text);
                    }
                    if (txt.Attributes["Include"] == "0")
                    {
                        isinclude = false;
                    }
                    else
                    {
                        string DataFormat = txt.Attributes["Format"];

                        if (DataFormat == "Date")
                        {
                            if (txt.ID.Contains("_from"))
                            {
                                val = GlobalUtilities.ConvertToSqlMinDateFormat(txt.Text);
                            }
                            else if (txt.ID.Contains("_to"))
                            {
                                val = GlobalUtilities.ConvertToSqlMaxDateFormat(txt.Text);
                            }
                        }
                        else if (DataFormat == "Int")
                        {
                            if (txt.Text.Trim() == "")
                            {
                                val = "0";
                            }
                            else
                            {
                                val = txt.Text;
                            }
                        }
                        else
                        {
                            val = txt.Text;
                        }
                    }
                }
                if (id != "" && isinclude)
                {
                    whereQuery = whereQuery.Replace("$" + id + "$", val);
                }
            }
        }
        ReportWhere = whereQuery;
        return whereQuery;
    }
    public void GetData()
    {
        DataTable dttbl = new DataTable();
        int start = CurrentPageIndex * gridSettings.PageSize;
        int end = CurrentPageIndex * gridSettings.PageSize + gridSettings.PageSize;
        if (ddlPageSize.SelectedValue == "All")
        {
            end = 0;
        }
        if (setGridData == null)
        {
            if (GridQuery == "")
            {
                getGridData(start, end, CurrentSort, IsSearch);
            }
            else
            {
                string countQuery = "select count(*) from(" + GridQuery + ")s_count";
                InsertUpdate obj1 = new InsertUpdate();
                DataTable dttblCount = obj1.ExecuteSelect(countQuery);
                gridSettings.TotalRecords = Convert.ToInt32(dttblCount.Rows[0][0]);
                InsertUpdate obj = new InsertUpdate();
                string sorting = CurrentSort;
                if (sorting == "") sorting = gridSettings.DefaultSort.Replace(" desc", "");
                if (sorting.Contains(" "))
                {
                    sorting = sorting.Substring(sorting.LastIndexOf(" ") + 1);
                }
                if (sorting.Contains("."))
                {
                    sorting = sorting.Substring(sorting.IndexOf(".") + 1);
                }

                if (IsAscending == false)
                {
                    sorting = sorting + " DESC";
                }
                string query = GridQuery + " order by " + sorting;
                _queryWithoutPaging = GridQuery;
                QueryWithoutPaging = _queryWithoutPaging;
                start = start + 1;
                string pagingQuery = "";

                if (end > 0)
                {
                    pagingQuery = "select * from (select *,ROW_NUMBER() OVER(ORDER BY " + sorting + ") AS RowNumber from(" + GridQuery
                        + ")paging1)paging2 where RowNumber between " + start + " and " + end;
                }
                else
                {
                    pagingQuery = GridQuery + " order by " + sorting;
                }
                Data = obj.ExecuteSelect(pagingQuery);
            }
        }
        else
        {
            setGridData(start, end, CurrentSort, IsSearch);
        }
    }
    private string ReplaceConstants(string query)
    {
        query = query.Replace("$LoggedInUser$", CustomSession.Session("Login_UserId"));
        return query;
    }
    private string GetKeyword()
    {
        if (IsMobileView)
        {
            return txtKeyword_m.Text;
        }
        else
        {
            return txtKeyword.Text;
        }
    }
    private string GetSearchBy()
    {
        if (IsMobileView)
        {
            return ddlSearchBy_m.SelectedValue;
        }
        else
        {
            return ddlSearchBy.SelectedValue;
        }
    }
    private string m
    {
        get
        {
            string strm = gridSettings.Module.ToLower();
            if (strm == "") strm = Module.ToLower();
            if (strm.StartsWith("view")) strm = strm.Substring(4);
            return strm;
        }
    }
    private string GetSubReportExtraWhere()
    {
        if (Request.QueryString["sr"] == null) return "";
        string query = "select * from tbl_subreport where subreport_subreportid=" + Request.QueryString["sr"];
        DataRow dr = DbTable.ExecuteSelectRow(query);
        return GlobalUtilities.ConvertToString(dr["subreport_extrawhere"]);
    }
    public void getGridData(int start, int end, string SortBy, bool IsSearch)
    {
        DataTable dttbl = new DataTable();
        InsertUpdate obj = new InsertUpdate();
        string query = gridSettings.Query;
        if (query == "") query = GridQuery;
        string topRec = "";
        string sorting = SortBy;
        string extraWhere = ExtraWhere;
        string strKeyword = GetKeyword();
        string strSearchBy = GetSearchBy();
        string subReportExtraWhere = GetSubReportExtraWhere();
        bool isCheckPermission = false;
        if (query.Contains("$CheckPermission$"))
        {
            isCheckPermission = true;
            query = query.Replace("$CheckPermission$", "");
        }
        if (subReportExtraWhere != "")
        {
            if (extraWhere == "")
            {
                extraWhere = subReportExtraWhere;
            }
            else
            {
                extraWhere += " AND " + subReportExtraWhere;
            }
        }

        //apply view access rights
        //apply view rights to all user
        //if (gridSettings.IsApplyViewRights)
        if (m != "clientuser")
        {
            if (Common.RoleId > 1 && Common.RoleId != 9)//Non Admin and Accounts
            {
                int userId = Common.UserId;
                int employeeId = Common.EmployeeId;
                int managerId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ManagerId"));
                int backupPersondId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_BackupPersonId"));
                string rightsWhere = "";

                if (Module == "advancedsearchloginhistory" || isCheckPermission)
                {
                    rightsWhere += "(client_employeeid=" + employeeId;
                    string childEmployees = CustomSession.Session("Login_ChildEmployees");
                    if (childEmployees != "" && childEmployees != null)
                    {
                        rightsWhere += " OR client_employeeid IN(" + childEmployees + ")";
                    }
                    rightsWhere += ")";
                }
                else if (Common.RoleId == 8 //Research team
                        && (Module == "whatsapptrialreport" || Module == "whatsappsubscriptionreport"))
                {
                    btnExport.Visible = true;
                }
                else
                {
                    for (int i = 0; i < gridSettings.arrColumns.Length; i++)
                    {
                        if (gridSettings.arrColumns.GetValue(i).ToString().Contains("_employeename"))
                        {
                            rightsWhere += " OR " + m + "_employeeid=" + employeeId;
                            //finrex changes
                            string childEmployees = CustomSession.Session("Login_ChildEmployees");
                            if (childEmployees != "" && childEmployees != null)
                            {
                                rightsWhere += " OR " + m + "_employeeid IN(" + childEmployees + ")";
                            }
                            break;
                        }
                    }
                    if (m == "calllog" || m == "proformainvoice" || m == "invoice" || m == "subscription")
                    {
                        rightsWhere += " OR client_employeeid=" + employeeId;
                    }
                    if (rightsWhere != "")
                    {
                        rightsWhere = "(" + m + "_createdby=" + userId + rightsWhere + ")";
                    }
                }
                if (rightsWhere != "")
                {
                    if (extraWhere == "")
                    {
                        extraWhere = rightsWhere;
                    }
                    else
                    {
                        extraWhere += " AND " + rightsWhere;
                    }
                }
            }
        }
        if (extraWhere != "")
        {
            int searchIndex = query.IndexOf("$Search$");
            extraWhere = extraWhere.Trim();
            if (extraWhere.ToLower().StartsWith("where"))
            {
                extraWhere = extraWhere.Substring(5);
            }
            if (query.Replace("$Search$", "").Trim().EndsWith(" where") || query.Contains(" where"))
            {
                if (searchIndex > 0)
                {
                    query = query.Insert(searchIndex, " AND " + extraWhere);
                }
                else
                {
                    query += " AND " + extraWhere;
                }
            }
            else
            {
                if (searchIndex > 0)
                {
                    query = query.Insert(searchIndex, " where " + extraWhere);
                }
                else
                {
                    query += " where " + extraWhere;
                }
            }
        }
        if (sorting.Contains(" "))
        {
            sorting = sorting.Substring(sorting.LastIndexOf(" ") + 1);
        }
        if (sorting.Contains("."))
        {
            sorting = sorting.Substring(sorting.IndexOf(".") + 1);
        }
        if (sorting.Trim() == "")
        {
            sorting = gridSettings.DefaultSort.ToLower();
            if (sorting.Contains(" "))
            {
                sorting = sorting.Substring(0, sorting.LastIndexOf(" ") + 1);
            }
        }
        if (IsAscending == false)
        {
            sorting = sorting + " DESC";
        }
        tblMobile.Attributes.Add("sb", sorting);
        if (IsReport)
        {
            //query = query.Replace("$Search$", "");
            if (query.ToLower().Contains("group by"))
            {
                string swhere = "";
                if (gridSettings.Where != "")
                {
                    if (extraWhere == "")
                    {
                        swhere = " WHERE " + GetWhere(gridSettings.Where);
                    }
                    else
                    {
                        string w = GetWhere(gridSettings.Where);
                        if (w.Trim() != "")
                        {
                            swhere = " AND " + GetWhere(gridSettings.Where);
                        }
                    }
                }
                query = query.Replace("$Search$", swhere);
            }
            else
            {
                if (gridSettings.Where != "")
                {
                    if (extraWhere == "")
                    {
                        query = query + " WHERE " + GetWhere(gridSettings.Where);
                    }
                    else
                    {
                        string w = GetWhere(gridSettings.Where);
                        if (w.Trim() != "")
                        {
                            query = query + " AND " + GetWhere(gridSettings.Where);
                        }
                    }
                }
            }
            query = query.Replace("$Search$", "");
        }
        else if (IsSearch)
        {
            if (IsAdvancedSearch)
            {
                string searchCriteria = "";
                for (int i = 0; i < Request.Form.Count; i++)
                {
                    string key = Request.Form.Keys[i].ToLower();
                    if (key.StartsWith("as_"))//advanced search
                    {
                        Array arrColumn = key.Split('_');
                        string columnName = Convert.ToString(arrColumn.GetValue(2)) + "_" + Convert.ToString(arrColumn.GetValue(3));
                        string value = global.CheckData(GlobalUtilities.ConvertToString(Request.Form[i]));
                        string searchCriteriatmp = "";
                        string control = Convert.ToString(arrColumn.GetValue(4));
                        string tovalue = "";
                        if (!control.EndsWith("-to"))
                        {
                            if (value.StartsWith("="))
                            {
                                searchCriteriatmp = columnName + "='" + value + "'";
                            }
                            else if (control.EndsWith("-ac"))
                            {
                                columnName = columnName.Replace("-ac", "");
                                if (GlobalUtilities.ConvertToInt(value) != 0)
                                {
                                    searchCriteriatmp = columnName + "='" + value + "'";
                                }
                            }
                            else
                            {
                                if (control == "date")
                                {
                                    if (value != "" || tovalue != "")
                                    {
                                        if (value == "") value = "01/01/1900";
                                        if (tovalue == "") tovalue = "01/01/1900";
                                        tovalue = global.CheckData(Request.Form[key + "-to"]);
                                        searchCriteriatmp = columnName + " BETWEEN '" + value + "' AND '" + tovalue + "'";
                                    }
                                }
                                else if (control == "amount")
                                {
                                    if (GlobalUtilities.ConvertToInt(value) != 0 && GlobalUtilities.ConvertToInt(tovalue) != 0)
                                    {
                                        tovalue = global.CheckData(Request.Form[key + "-to"]);
                                        searchCriteriatmp = columnName + " BETWEEN " + GlobalUtilities.ConvertToDouble(value) + " AND " + GlobalUtilities.ConvertToDouble(tovalue);
                                    }
                                }
                                else if (control == "number")
                                {
                                    if (GlobalUtilities.ConvertToInt(value) != 0 || GlobalUtilities.ConvertToInt(tovalue) != 0)
                                    {
                                        tovalue = global.CheckData(Request.Form[key + "-to"]);
                                        searchCriteriatmp = columnName + " BETWEEN '" + value + "' AND '" + tovalue + "'";
                                    }
                                }
                                else
                                {
                                    if (value.Trim() != "")
                                    {
                                        searchCriteriatmp = "ISNULL(" + columnName + ",'') LIKE '%" + value + "%'";
                                    }
                                }
                            }
                            if (searchCriteriatmp != "")
                            {
                                if (searchCriteria == "")
                                {
                                    searchCriteria = searchCriteriatmp;
                                }
                                else
                                {
                                    searchCriteria += " AND " + searchCriteriatmp;
                                }
                            }
                        }
                    }
                }
                if (query.Contains(" where"))
                {
                    query = query.Replace("$Search$", " AND " + searchCriteria);
                }
                else
                {
                    searchCriteria = " WHERE " + searchCriteria;
                    query = query.Replace("$Search$", searchCriteria);
                }
            }
            else
            {
                if (query.Contains(" where"))
                {
                    if (strKeyword.Contains("="))
                    {
                        query = query.Replace("$Search$", " AND " + strSearchBy + strKeyword.Replace("=", "='") + "'");
                    }
                    else
                    {
                        query = query.Replace("$Search$", " AND " + strSearchBy + " LIKE '%" + strKeyword + "%'");
                    }
                }
                else
                {
                    if (strKeyword.Contains("="))
                    {
                        query = query.Replace("$Search$", " WHERE " + strSearchBy + strKeyword.Replace("=", "='") + "'");
                    }
                    else
                    {
                        query = query.Replace("$Search$", " WHERE " + strSearchBy + " LIKE '%" + strKeyword + "%'");
                    }
                }
            }
        }
        else
        {
            query = query.Replace("$Search$", "");
        }
        if (IsCheckAccess)
        {
            //show only created by logged in user
            //show only data under logged in user
            if (IsRightsExists("under me"))
            {
                if (query.ToLower().Contains(" where"))
                {
                    query = query + " AND (" + Module.ToLower() + "_createdby=" + CustomSession.Session("Login_UserId") + " OR " +
                                        Module.ToLower() + "_createdby in(select user_userid from tbl_user where user_reportingtoid=" +
                                                            CustomSession.Session("Login_UserId") + ") OR " +
                                        Module.ToLower() + "_assignedtoid in(select user_userid from tbl_user where user_reportingtoid=" +
                                                            CustomSession.Session("Login_UserId") + "))";
                }
                else
                {
                    query = query + " WHERE (" + Module.ToLower() + "_createdby=" + CustomSession.Session("Login_UserId") + " OR " +
                                        Module.ToLower() + "_createdby in(select user_userid from tbl_user where user_reportingtoid=" +
                                                            CustomSession.Session("Login_UserId") + ") OR " +
                                        Module.ToLower() + "_assignedtoid in(select user_userid from tbl_user where user_reportingtoid=" +
                                                            CustomSession.Session("Login_UserId") + "))";
                }
            }
            else if (IsRightsExists("owned by me"))//show only assigned to or created by logged in user
            {
                // if (query.ToLower().Contains(" where"))
                if (query.Contains(" where"))
                {
                    query = query + " AND (" + Module.ToLower() + "_createdby=" + CustomSession.Session("Login_UserId") + " OR " +
                                    Module.ToLower() + "_assignedtoid=" + CustomSession.Session("Login_UserId") + ")";
                }
                else
                {
                    query = query + " WHERE (" + Module.ToLower() + "_createdby=" + CustomSession.Session("Login_UserId") + " OR " +
                                    Module.ToLower() + "_assignedtoid=" + CustomSession.Session("Login_UserId") + ")";
                }
            }
        }

        query = ReplaceConstants(query);

        //query = query + extraWhere;

        query = query.Trim();
        if (query.ToLower().EndsWith("where"))
        {   
            query = query.Substring(0, query.Length - 5);
        }
        if (Request.QueryString["debug"] == "true")
        {
            ErrorLog.WriteLog(query);
        }
        //Response.Write(query);
        bool isSessionVarExists = true;
        int stindex = 0;
        int enindex = 0;
        while (isSessionVarExists)
        {
            stindex = query.IndexOf("$SESSION_");
            if (stindex > 0)
            {
                enindex = query.IndexOf("$", stindex + 1);
                string variable = query.Substring(stindex + 9, enindex - stindex - 9);
                query = query.Replace("$SESSION_" + variable + "$", CustomSession.Session(variable));
            }
            else
            {
                isSessionVarExists = false;
            }
        }

        bool isDateVarExists = true;
        stindex = 0;
        enindex = 0;
        while (isDateVarExists)
        {
            //format=> DATEBETWEEN(stock_date,txtFromDate,txtToDate)
            stindex = query.IndexOf("DATEBETWEEN");
            if (stindex > 0)
            {
                enindex = query.IndexOf(")", stindex + 1);
                if (enindex > 0)
                {
                    enindex = enindex + 1;
                    string datecondition = query.Substring(stindex, enindex - stindex);
                    Array arrDateCondn = datecondition.Split(',');
                    string dateCol = Convert.ToString(arrDateCondn.GetValue(0)).Replace("(", "").Replace(")", "").ToLower().Replace("datebetween", "").Trim();
                    string txtFromDate = Convert.ToString(arrDateCondn.GetValue(1)).Trim().Replace("(", "").Replace(")", "");
                    string txtToDate = Convert.ToString(arrDateCondn.GetValue(2)).Trim().Replace("(", "").Replace(")", "");
                    string strFromDate = GetFormVarData(txtFromDate);
                    string strToDate = GetFormVarData(txtToDate);
                    if (strFromDate != "") strFromDate = GlobalUtilities.ConvertMMDateToDD(strFromDate).Replace('-', '/');
                    if (strToDate != "") strToDate = GlobalUtilities.ConvertMMDateToDD(strToDate).Replace('-', '/');
                    string dateWhere = "";
                    if (strFromDate != "")
                    {
                        dateWhere = " AND CAST(" + dateCol + " AS DATE) >=CAST('" + strFromDate + "' AS DATE)";
                    }
                    if (strToDate != "")
                    {
                        dateWhere += " AND CAST(" + dateCol + " AS DATE) <=CAST('" + strToDate + "' AS DATE)";
                    }

                    //if (dateWhere == "") dateWhere = "1=1";
                    query = query.Remove(stindex, enindex - stindex);
                    query = query.Insert(stindex, dateWhere);
                    query = query.Replace("WHERE  AND", "WHERE ").Replace("WHERE AND", "WHERE ");
                }
                else
                {
                    isDateVarExists = false;
                }
            }
            else
            {
                isDateVarExists = false;
            }
        }

        isSessionVarExists = true;
        stindex = 0;
        enindex = 0;
        while (isSessionVarExists)
        {
            stindex = query.IndexOf("$FORM_");
            if (stindex > 0)
            {
                enindex = query.IndexOf("$", stindex + 1);
                string variable = query.Substring(stindex + 6, enindex - stindex - 6);
                query = query.Replace("$FORM_" + variable + "$", Request.Form["ctl00$ContentPlaceHolder1$" + variable]);
            }
            else
            {
                isSessionVarExists = false;
            }
        }

        bool isConstantExists = true;
        stindex = 0;
        enindex = 0;
        while (isConstantExists)
        {
            stindex = query.IndexOf("$CONSTANT_");
            if (stindex > 0)
            {
                enindex = query.IndexOf("$", stindex + 1);
                string variable = query.Substring(stindex + 10, enindex - stindex - 10);
                query = query.Replace("$SESSION_" + variable + "$", CustomSession.Session(variable));
            }
            else
            {
                isConstantExists = false;
            }
        }
        bool isQueryStringExists = true;
        stindex = 0;
        enindex = 0;
        while (isQueryStringExists)
        {
            stindex = query.IndexOf("$QUERYSTRING_");
            if (stindex > 0)
            {
                enindex = query.IndexOf("$", stindex + 1);
                string variable = query.Substring(stindex + 13, enindex - stindex - 13);
                query = query.Replace("$QUERYSTRING_" + variable + "$", Request.QueryString[variable]);
            }
            else
            {
                isQueryStringExists = false;
            }
        }
        if (gridSettings.TopRecords == 0)
        {
            query = query.Trim();
            if (query.EndsWith(" OR"))
            {
                query = query.Substring(0, query.Length - 3);
            }
            if (query.ToLower().EndsWith(" where"))
            {
                query = query.Substring(0, query.Length - 6);
            }
            string countQuery = "select count(*) from(" + query + ")s_count";
            DataTable dttblCount = obj.ExecuteSelect(countQuery);
            gridSettings.TotalRecords = Convert.ToInt32(dttblCount.Rows[0][0]);
        }
        else
        {
            topRec = " top " + gridSettings.TopRecords + " ";
        }
        query = query.Replace("AND AND", "AND");
        start = start + 1;
        string pagingQuery = "";
        _queryWithoutPaging = query;
        QueryWithoutPaging = _queryWithoutPaging;
        Session["QueryWithoutPaging"] = QueryWithoutPaging;
        if (end > 0)// && trPaging.Visible)
        {
            pagingQuery = "select " + topRec + "* from (select *,ROW_NUMBER() OVER(ORDER BY " + sorting + ") AS RowNumber from(" + query
                + ")paging1)paging2 where RowNumber between " + start + " and " + end;
        }
        else
        {
            pagingQuery = query.Replace("select ", "select " + topRec) + " order by " + sorting;
        }

        Data = obj.ExecuteSelect(pagingQuery);
    }
    private string GetFormVarData(string variable)
    {
        variable = variable.Replace("$FORM_", "").Replace("$", "");
        return GlobalUtilities.ConvertToString(Request.Form["ctl00$ContentPlaceHolder1$" + variable]);
    }
    protected void ddlPageSize_Changed(object sender, EventArgs e)
    {
        if (ddlPageSize.SelectedValue == "All")
        {
            gridSettings.PageSize = Int32.MaxValue;
        }
        else
        {
            gridSettings.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        }
        CurrentPageIndex = 0;
        GetData();
        BindGridData();
    }
    protected void lnkPrevPage_Click(object sender, EventArgs e)
    {
        if (CurrentPageIndex != 0)
        {
            CurrentPageIndex = CurrentPageIndex - 1;
            GetData();
            BindGridData();
            if (CurrentPageIndex == 0)
            {
                lnkPrevPage.Enabled = false;
            }
            else
            {
                lnkPrevPage.Enabled = true;
            }
        }
        else
        {
            lnkPrevPage.Enabled = false;
        }
        lnkNextPage.Enabled = true;
    }
    protected void lnkNextPage_Click(object sender, EventArgs e)
    {
        if (CurrentPageIndex + 1 != gridSettings.TotalPages)
        {
            CurrentPageIndex = CurrentPageIndex + 1;

            //if (CurrentPageIndex == 1)
            {
                //GetData(1);
            }
            //else
            {
                GetData();
            }
            BindGridData();

            if (CurrentPageIndex + 1 == gridSettings.TotalPages)
            {
                lnkNextPage.Enabled = false;
            }
            else
            {
                lnkNextPage.Enabled = true;
            }
        }
        else
        {
            lnkNextPage.Enabled = false;
        }
        if (CurrentPageIndex > 0)
        {
            lnkPrevPage.Enabled = true;
        }

    }
    private string GetLabel(string ColumnName)
    {
        string Label = "";
        byte[] ASCIIValues = Encoding.ASCII.GetBytes(ColumnName);
        bool first = true;
        int prev = 0;
        foreach (byte b in ASCIIValues)
        {
            if (b >= 65 && b <= 90 && first == false && !(prev >= 65 && prev <= 90))//caps
            {
                Label += " " + ((char)b).ToString();
            }
            else
            {
                Label += ((char)b).ToString();
            }
            first = false;
            prev = (int)b;
        }

        return Label;
    }
    public void SearchBy(string text, string value)
    {
        ddlSearchBy.Items.Add(new ListItem(text, value));
    }
    public string SearchBySelected
    {
        get
        {
            return ddlSearchBy.SelectedValue;
        }
    }
    protected void imgRefresh_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void btnAdvancedSearch_Click(object sender, EventArgs e)
    {
        IsSearch = true;
        IsAdvancedSearch = true;
        GetData();
        BindGridData();

    }
    public void HidePaging()
    {
        trPaging.Visible = false;
    }
}
