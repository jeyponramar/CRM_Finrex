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


public partial class RepeaterGrid : System.Web.UI.UserControl
{
    GridSettings gridSettings = new GridSettings();
    public DataTable Data = null;
    int _index = 0;
    public SetGridData setGridData;
    protected void Page_Load(object sender, EventArgs e)
    {
        gridSettings = new GridSettings();
        gridSettings.BindXml(Module);
        BindSettingsFromXml();
        if (!IsPostBack)
        {
            CurrentSort = gridSettings.DefaultSort;
            if (CurrentSort.ToLower().Contains(" desc"))
            {
                IsAscending = false;
                CurrentSort = CurrentSort.Replace(" desc", "").Replace(" DESC", "");
            }
            else
            {
                IsAscending = true;
            }
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
            if (!IsAscending) sb = sb + " DESC";
            return sb;
        }
        set { ViewState["CurrentSort"] = value; }
    }
    private void BindSettingsFromXml()
    {
        
    }
    private void BindGridSetting()
    {
        if (gridSettings.IsReport)
        {
            txtKeyword.Visible = false;
            ddlSearchBy.Visible = false;
        }
        imgGridSetting.Attributes.Add("m", Module);
        for (int i = 0; i < gridSettings.arrSearchByCols.Length; i++)
        {
            ddlSearchBy.Items.Add(new ListItem(gridSettings.arrSearchByLabels.GetValue(i).ToString(), gridSettings.arrSearchByCols.GetValue(i).ToString()));
        }

        
    }
    public void SetCreateUrl(string url)
    {
        lnkAddPage.NavigateUrl = url;
    }
    public void BindData()
    {
        tblData.Attributes.Add("m", Module);
        BindGridSetting();
        CurrentPageIndex = 0;
        lnkPrevPage.Enabled = false;
        this.Visible = true;
        if (gridSettings.EditUrl == "")
        {
            lnkAddPage.Visible = false;
        }
        else
        {
            lnkAddPage.NavigateUrl = gridSettings.EditUrl;
            if (gridSettings.IsCreateRequired)
            {
                string AddUrl = GetLabel(gridSettings.EditUrl.Replace("Add", "Create New ").Replace(".aspx", ""));
                if (AddUrl.Contains("?"))
                {
                    AddUrl = AddUrl.Substring(0, AddUrl.IndexOf("?"));
                }
                lnkAddPage.Text = AddUrl;
            }
            else
            {
                lnkAddPage.Visible = false;
            }
        }
        GetData();
        if (gridSettings.EnablePaging)
        {
            trPaging.Visible = true;
            if (gridSettings.TotalRecords < 1)
            {
                lblError.Visible = true;
                return;
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
    private void BindGridData()
    {
        _index = 0;
        BindHead();
        //rpt.DataSource = Data;
        //rpt.DataBind();
        if (Data != null)
        {
            StringBuilder html = new StringBuilder();
            for (int i = 0; i < Data.Rows.Count; i++)
            {
                string rptClass = "repeater-row";
                if (i % 2 == 0)
                {
                    rptClass = "repeater-alt";
                }
                if (gridSettings.GridType != "list")
                {
                    html.Append("<tr class='" + rptClass + "'>");
                }
                int row = 0;
                int prevRow = 0;
                for (int j = 0; j < gridSettings.xmlGridColumns.Count; j++)
                {
                    string colName = gridSettings.xmlGridColumns[j].InnerText;
                    string headerText = gridSettings.xmlGridHeaders[j].InnerText;
                    string val = Convert.ToString(Data.Rows[i][colName]);
                    row = Convert.ToInt32(gridSettings.xmlGridColumnRow[j].InnerText);
                    if (gridSettings.GridType == "list" && (j == 0 || row != prevRow))
                    {
                        html.Append("<tr>");
                    }
                    html.Append("<td>" + headerText + " : </td><td><b>" + val + "</b></td>");
                    if (gridSettings.GridType == "list" && row != prevRow && j > 0)
                    {
                        html.Append("</tr>");
                    }
                    prevRow = row;
                }
                if (gridSettings.GridType != "list")
                {
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
            ltGrid.Text = html.ToString();
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
        }
        if (gridSettings.EnablePaging) BindPaging();

    }
    private void BindHead()
    {
        rptHead.DataSource = gridSettings.arrColumns_HeaderText;
        rptHead.DataBind();

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        gridSettings.IsSearch = true;
        GetData();
        BindGridData();
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
                lnkHead.Text = headerText;
                if (columnName.Substring(columnName.IndexOf("_") + 1).ToLower() == gridSettings.Namefield.ToLower())
                {
                    lnkHead.Attributes.Add("nf", columnName.Substring(columnName.IndexOf("_") + 1));
                }
            }
            else
            {
                lnkHead.Visible = false;

                lblHead.Text = headerText;
            }

            HtmlControl tdHeader = (HtmlControl)e.Item.FindControl("tdHeader");
            Array arrHiddenColumns = gridSettings.HiddenColumns.Split(',');
            for (int i = 0; i < arrHiddenColumns.Length; i++)
            {
                if (columnName.ToLower().Replace(" ", "") == arrHiddenColumns.GetValue(i).ToString().ToLower().Replace(" ", ""))
                {
                    //tdHeader.Attributes.Add("class", "hidden");
                    break;
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
                if (gridSettings.EditUrl.Contains("?"))
                {
                    html.Append("<td><a href='" + gridSettings.EditUrl + "&" + queryString + "'>Edit</a></td>");
                }
                else
                {
                    html.Append("<td><a href='" + gridSettings.EditUrl + "?" + queryString + "'>Edit</a></td>");
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
            getGridData(start, end, CurrentSort, gridSettings.IsSearch); 
        }
        else
        {
            setGridData(start, end, CurrentSort, gridSettings.IsSearch);
        }
        //InsertUpdate objCount = new InsertUpdate();
        //DataTable dttbCount = objCount.ExecuteSelect("select count(*) from(" + Query + ")s_count");
        //InsertUpdate obj = new InsertUpdate();
        //Data = obj.ExecuteSelect(Query);
        //TotalRecords = Convert.ToInt32(dttbCount.Rows[0][0]);
        
    }
    public void getGridData(int start, int end, string SortBy, bool IsSearch)
    {
        DataTable dttbl = new DataTable();
        InsertUpdate obj = new InsertUpdate();
        string query = gridSettings.Query;
        string topRec = "";
        query = query.Replace("$Search$", "");
        if (gridSettings.TopRecords == 0)
        {
            string countQuery = "select count(*) from(" + query + ")s_count";
            DataTable dttblCount = obj.ExecuteSelect(countQuery);
            gridSettings.TotalRecords = Convert.ToInt32(dttblCount.Rows[0][0]);
        }
        else
        {
            topRec = " top " + gridSettings.TopRecords + " ";
        }
        start = start + 1;
        string pagingQuery = "";
        
        if (end > 0)
        {
            pagingQuery = "select "+topRec+"* from (select *,ROW_NUMBER() OVER(ORDER BY " + SortBy + ") AS RowNumber from(" + query
                + ")paging1)paging2 where RowNumber between " + start + " and " + end;
        }
        else
        {
            pagingQuery = query + " order by " + SortBy;
        }
        Data = obj.ExecuteSelect(pagingQuery);
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
}
