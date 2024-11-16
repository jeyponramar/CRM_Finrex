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

public partial class CPGridData : System.Web.UI.UserControl
{
    private DataTable _Data = null;
    Array arrColumns_HeaderText;
    Array arrColumns_ColumnName;
    public DataProvider _DataProvider;
    
    int _index = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }
    }
    public int PageSize
    {
        get 
        {
            if (ViewState["PageSize"] == null) return 20;
            return Convert.ToInt32(ViewState["PageSize"]); 
        }
        set { ViewState["PageSize"] = value; }
    }
    public bool EnablePaging
    {
        get { return Convert.ToBoolean(ViewState["EnablePaging"]); }
        set 
        {
            if (value == false)
            {
                tblPaging.Visible = false;
            }
            ViewState["EnablePaging"] = value; 
        }
    }
    public bool IsMultiSelect
    {
        get 
        {
            if (ViewState["IsMultiSelect"] == null)
            {
                return false;
            }
            return Convert.ToBoolean(ViewState["IsMultiSelect"]); 
        }
        set
        {
            ViewState["IsMultiSelect"] = value;
        }
    }
    public bool EnableSorting
    {
        get { return Convert.ToBoolean(ViewState["EnableSorting"]); }
        set { ViewState["EnableSorting"] = value; }
    }
    public string EditUrl
    {
        get { return Convert.ToString(ViewState["EditUrl"]); }
        set { ViewState["EditUrl"] = value; }
    }
    public string ViewUrl
    {
        get { return Convert.ToString(ViewState["ViewUrl"]); }
        set { ViewState["ViewUrl"] = value; }
    }
    public string Columns
    {
        get 
        {
            return Convert.ToString(ViewState["PagingColumns"]); 
        }
        set 
        { 
            ViewState["PagingColumns"] = value; 
        }
    }
    public string HeaderText
    {
        get
        {
            return Convert.ToString(ViewState["HeaderText"]);
        }
        set
        {
            ViewState["HeaderText"] = value;
        }
    }
    public string SortBy
    {
        get { return Convert.ToString(ViewState["PagingSortBy"]); }
        set { ViewState["PagingSortBy"] = value; }
    }
    public string SearchKeyword
    {
        get { return Convert.ToString(ViewState["SearchKeyword"]); }
        set { ViewState["SearchKeyword"] = value; }
    }
    public bool IsSearch
    {
        get 
        {
            if (ViewState["IsSearch"] == null) return false;
            return Convert.ToBoolean(ViewState["IsSearch"]); 
        }
        set { ViewState["IsSearch"] = value; }
    }
    public bool ViewInNewWindow
    {
        get 
        {
            if (ViewState["ViewInNewWindow"] == null)
            {
                return false;
            }
            else
            {
                return Convert.ToBoolean(ViewState["ViewInNewWindow"]);
            }
        }
        set { ViewState["ViewInNewWindow"] = value; }
    }
    public bool IsCreateRequired
    {
        get 
        {
            if (ViewState["IsCreateRequired"] == null)
            {
                return true;
            }
            else
            {
                return Convert.ToBoolean(ViewState["IsCreateRequired"]); 
            }
            
        }
        set { ViewState["IsCreateRequired"] = value; }
    }
    public int TotalRecords
    {
        get{return Convert.ToInt32(ViewState["TotalRecords"]);}
        set{ViewState["TotalRecords"] = value;}
    }
    public int MaxPages
    {
        get 
        {
            if (ViewState["MaxPages"] == null)
                return 5;
            return Convert.ToInt32(ViewState["MaxPages"]); 
        }
        set { ViewState["MaxPages"] = value; }
    }
    public int CurrentPageIndex
    {
        get{return Convert.ToInt32(ViewState["CurrentPageIndex"]);}
        set{ViewState["CurrentPageIndex"] = value;}
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
    public string HiddenColumns
    {
        get 
        {
            if (ViewState["HiddenColumns"] == null)
            {
                return Module.ToLower() + "_" + Module.ToLower() + "id";
            }
            else
            {
                return Convert.ToString(ViewState["HiddenColumns"]);
            }
        }
        set { ViewState["HiddenColumns"] = value; }
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
   

    public string Module
    {
        get
        {
            return Convert.ToString(ViewState["Module"]);
        }
        set { ViewState["Module"] = value; }
    }
    public string Namefield
    {
        get
        {
            return Convert.ToString(ViewState["nf"]);
        }
        set { ViewState["nf"] = value; }
    }

    public DataTable Data
    {
        get
        {
            return _Data;
        }
        set
        {
            _Data = value;
        }
    }
    public int TotalPages
    {
        get
        {
            int pages = TotalRecords / PageSize;
            if (TotalRecords % PageSize > 0)
            {
                pages = pages + 1;
            }
            return pages;
        }
    }
    private void BindGridSetting()
    {
        string query = "select * from tbl_module " +
                        "join tbl_columns on columns_moduleid = module_moduleid " +
                        "where replace(module_modulename,' ','') = '" + Module.Replace(" ","") + "' AND columns_isviewpage=1";
        DataTable dttbl = new DataTable();
        InsertUpdate obj = new InsertUpdate();
        dttbl = obj.ExecuteSelect(query);
        string columns = "";
        string headertext = "";
        if (dttbl.Rows.Count == 0)
        {
            //query = query.Replace(" AND c.isviewpage=1","");
            InsertUpdate obj2 = new InsertUpdate();
            //dttbl = obj2.ExecuteSelect(query);
            string tableName = "tbl_" + Module.ToLower();
            query = "select c.name as columnName from syscolumns c join sysobjects s on c.id=s.id where s.name='" + tableName + "'";
            dttbl = obj2.ExecuteSelect(query);
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                string colName = Convert.ToString(dttbl.Rows[i]["columnName"]);
                if (i == 0)
                {
                    columns = colName;
                    headertext = colName;
                }
                else
                {
                    columns = columns + "," + colName;
                    headertext = headertext + "," + colName;
                }
            }
        }
        else
        {
            string prefix = "";// Module.Replace(" ", "").ToLower() + "_";
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                string columnName = prefix + Convert.ToString(dttbl.Rows[i]["columns_columnname"]);
                if (Convert.ToBoolean(dttbl.Rows[i]["columns_isnamefield"]) || i == 0)
                {
                    Namefield = Convert.ToString(dttbl.Rows[i]["columns_columnname"]);
                }
                if (GlobalUtilities.ConvertToInt(dttbl.Rows[i]["columns_dropdownmoduleid"]) > 0)
                {
                    columnName = Convert.ToString(dttbl.Rows[i]["columns_dropdowncolumn"]);
                }
                if (i == 0)
                {
                    columns = columnName;
                    headertext = Convert.ToString(dttbl.Rows[i]["columns_gridcolumnname"]);
                }
                else
                {
                    columns = columns + "," + columnName;
                    headertext = headertext + "," + Convert.ToString(dttbl.Rows[i]["columns_gridcolumnname"]);
                }
            }
        }
        Columns = columns;
        HeaderText = headertext;
    }
    private void SetHeaderAndColumn()
    {
        arrColumns_HeaderText = HeaderText.Split(',');
        arrColumns_ColumnName = Columns.Split(',');
        //for (int i = 0; i < arrColumns_HeaderText.Length; i++)
        //{
        //    string headerText = Convert.ToString(arrColumns_HeaderText.GetValue(i));
        //    int index = headerText.IndexOf("|");                                               
        //    if (index > 0)
        //    {
        //        arrColumns_HeaderText.SetValue(headerText.Substring(index + 1), i);
        //        arrColumns_ColumnName.SetValue(headerText.Substring(0, index), i);
        //    }
        //    else
        //    {
        //        if (headerText.IndexOf('.', headerText.Length - 1) > 0) headerText = headerText.Substring(0, headerText.Length - 1);
        //        arrColumns_HeaderText.SetValue(GetLabel(headerText), i);
        //        arrColumns_ColumnName.SetValue(headerText.Replace(" ",""), i);
        //    }
        //}
    }
    public void SetCreateUrl(string url)
    {
        lnkAddPage.NavigateUrl = url;
    }
    public void BindData()
    {
        SortBy = _DataProvider.DefaultSort;
        CurrentSort = _DataProvider.DefaultSort;
        IsAscending = _DataProvider.DefaultSortDirection;
        BindGridSetting();
        CurrentPageIndex = 0;
        lnkPrevPage.Enabled = false;
        this.Visible = true;
        if (EditUrl == "")
        {
            lnkAddPage.Visible = false;
        }
        else
        {
            lnkAddPage.NavigateUrl = EditUrl;
            if (IsCreateRequired)
            {
                string AddUrl = GetLabel(EditUrl.Replace("Add", "Create New ").Replace(".aspx", ""));
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
        SetHeaderAndColumn();
        GetData();
        if (EnablePaging)
        {
            trPaging.Visible = true;
            if (TotalRecords < 1)
            {
                tblData.Visible = false;
                tblPaging.Visible = false;
                trPaging.Visible = false;
                lblError.Visible = true;
                return;
            }
            else
            {
                tblPaging.Visible = true;
                trPaging.Visible = true;
                tblData.Visible = true;
                lblError.Visible = false;
                if (TotalPages > 1)
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
        if (TotalRecords == 0)
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
            TotalRecordSummary = "Total Records : <b>" + TotalRecords + "</b>";
        }
        else
        {
            TotalRecordSummary = "Showing <b>" + Convert.ToString(CurrentPageIndex * PageSize + 1);

            if ((CurrentPageIndex + 1) * PageSize > TotalRecords)
            {
                TotalRecordSummary += "</b> to <b>" + Convert.ToString(TotalRecords) + "</b>";
            }
            else
            {
                TotalRecordSummary += "</b> to <b>" + Convert.ToString((CurrentPageIndex + 1) * PageSize) + "</b>";
            }
            TotalRecordSummary += " of <b>" + TotalRecords + "</b>";
        }
        lblTotalRecords.Text = TotalRecordSummary;

    }
    private void BindGridData()
    {
        _index = 0;
        BindHead();
        rpt.DataSource = Data;
        rpt.DataBind();
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
        if (EnablePaging) BindPaging();

    }
    private void BindHead()
    {
        rptHead.DataSource = arrColumns_HeaderText;
        rptHead.DataBind();
       
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SetHeaderAndColumn();
        GetData();
        BindGridData();
    }
    public string Keyword
    {
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

            string headerText = Convert.ToString(arrColumns_HeaderText.GetValue(e.Item.ItemIndex));
            string columnName = Convert.ToString(arrColumns_ColumnName.GetValue(e.Item.ItemIndex));
            if (EnableSorting)
            {
                lblHead.Visible = false;
                lnkHead.CommandArgument = columnName;
                lnkHead.Text = headerText;
                if (columnName.Substring(columnName.IndexOf("_") + 1).ToLower() == Namefield.ToLower())
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
            Array arrHiddenColumns = HiddenColumns.Split(',');
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
            SetHeaderAndColumn();
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
            int N = arrColumns_ColumnName.Length;
            if (EditUrl != "")
            {
                N = N - 1;
            }
            int Start = 0;
            //if (HiddenColumns != "")
            //{
            //    Array arrHiddenColumns = HiddenColumns.Split(',');
            //    for (int i = 0; i < arrHiddenColumns.Length; i++)
            //    {
            //        columnName = arrHiddenColumns.GetValue(i).ToString();
            //        string shortColName = columnName.Substring(columnName.IndexOf("_") + 1);
            //        html.Append("<td class='hidden' cn='"+shortColName+"'>" + Convert.ToString(Data.Rows[_index][columnName]) + "</td>");
            //        Start++;
            //    }
            //}
            Array arrhidden = _DataProvider.HiddenColumns.Split(',');

            for (int j = 0; j < arrhidden.Length; j++)
            {
                columnName = arrhidden.GetValue(j).ToString();
                string shortColName = columnName.Substring(columnName.IndexOf("_") + 1);
                html.Append("<td class='hidden' cn='" + shortColName + "'>" + Convert.ToString(Data.Rows[_index][columnName]) + "</td>");

                //if (j == 0)
                //{
                //    hiddenvalues = " hv=" + Convert.ToString(arrhidden.GetValue(j)) + "~" + Convert.ToString(Data.Rows[_index][Convert.ToString(arrhidden.GetValue(j))]);
                //}
                //else
                //{
                //    hiddenvalues = "~" + Convert.ToString(arrhidden.GetValue(j)) + "~" + Convert.ToString(Data.Rows[_index][Convert.ToString(arrhidden.GetValue(j))]);
                //}
            }
            Start = 0;
            for (int i = Start; i < N; i++)
            {
                columnName = Convert.ToString(arrColumns_ColumnName.GetValue(i));
                columnName = columnName.Replace(" ", "");
                if (columnName.IndexOf('.') > 0)
                {
                    columnName = columnName.Substring(columnName.IndexOf('.')+1);
                    
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
                if (i == 0 && Convert.ToString(ViewUrl)!="")
                {
                    string newWindow = "";
                    if (ViewInNewWindow)
                    {
                        newWindow = "target='_blank'";
                    }
                    data = "<a " + newWindow + " href='" + ViewUrl + "?" + Convert.ToString(arrColumns_ColumnName.GetValue(0)) + "=" + Data.Rows[_index][Convert.ToString(arrColumns_ColumnName.GetValue(0))] + "'>" + data + "</a>";
                }
                string hiddenvalues = "";
                if (i == 0)
                {
                    //Array arrhidden = _DataProvider.HiddenColumns.Split(',');
                    
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
            if (EditUrl != "")
            {
                string queryString = "";
                if (QueryStringVariables == "")
                {
                    queryString = Convert.ToString(arrColumns_ColumnName.GetValue(0)) + "="
                    + Data.Rows[_index][Convert.ToString(arrColumns_ColumnName.GetValue(0))];
                }
                else
                {
                    Array arrQueryStringVar = QueryStringVariables.Split(',');
                    for (int i = 0; i < arrQueryStringVar.Length; i++)
                    {
                        string queryVar = arrQueryStringVar.GetValue(i).ToString();
                        if (!EditUrl.Contains(queryVar + "="))
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
                if (EditUrl.Contains("?"))
                {
                    html.Append("<td><a href='" + EditUrl + "&" + queryString + "'>Edit</a></td>");
                }
                else
                {
                    html.Append("<td><a href='" + EditUrl + "?" + queryString + "'>Edit</a></td>");
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
        int endIndex = TotalPages;//CurrentPageIndex * PageSize;
        SetTotal();
        for (int i = 0; i < TotalPages; i++)
        {
            int PageBoxStart = MaxPages * i;
            int PageBoxEnd = MaxPages * (i + 1);
            if (CurrentPageIndex >= PageBoxStart && CurrentPageIndex <= PageBoxEnd)
            {
                startIndex = PageBoxStart + 1;
                endIndex = startIndex + MaxPages - 1;
                if (endIndex > TotalPages)
                {
                    endIndex = TotalPages;
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
            lnk.Text = "&nbsp;"+i.ToString()+"&nbsp;";
            lnk.CommandArgument = Convert.ToString(i - 1);
            if (i - 1 == CurrentPageIndex)
            {
                //lnk.Attributes.Add("class", "paging-active");
                HtmlControl td = (HtmlControl)rptPaging.Items[row].FindControl("page_td");
                td.Style.Add("background-color","#0f65f1;");
                lnk.Style.Add("color", "#ffffff");
            }
            row++;
        }
    }
    protected void lnkPage_OnCommand(object sender, CommandEventArgs e)
    {
        CurrentPageIndex = Convert.ToInt32(e.CommandArgument);
        if (CurrentPageIndex+1 == TotalPages)
        {
            lnkNextPage.Enabled = false;
        }
        else
        {
            lnkNextPage.Enabled = true;
        }
        if (CurrentPageIndex  == 0)
        {
            lnkPrevPage.Enabled = false;
        }
        else
        {
            lnkPrevPage.Enabled = true;
        }
        SetHeaderAndColumn();
        GetData();
        BindGridData();
    }
    public void GetData()
    {
        DataTable dttbl = new DataTable();
        int start = CurrentPageIndex * PageSize;
        int end = CurrentPageIndex * PageSize + PageSize;
        int total = 0;
        if (ddlPageSize.SelectedValue == "All")
        {
            end = 0;
        }
        Data = _DataProvider.GetGridData(ref total, start, end, CurrentSort, ddlSearchBy, txtKeyword.Text);
        TotalRecords = total;
    }
    protected void ddlPageSize_Changed(object sender, EventArgs e)
    {
        if (ddlPageSize.SelectedValue == "All")
        {
            PageSize = Int32.MaxValue;
        }
        else
        {
            PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        }
        SetHeaderAndColumn();
        GetData();
        BindGridData();
    }
    protected void lnkPrevPage_Click(object sender, EventArgs e)
    {
        if (CurrentPageIndex != 0)
        {
            CurrentPageIndex = CurrentPageIndex - 1;
            SetHeaderAndColumn();

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
        if (CurrentPageIndex + 1 != TotalPages)
        {
            CurrentPageIndex = CurrentPageIndex + 1;

            SetHeaderAndColumn();
            //if (CurrentPageIndex == 1)
            {
                //GetData(1);
            }
            //else
            {
                GetData();
            }
            BindGridData();

            if (CurrentPageIndex+1 == TotalPages)
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
}
