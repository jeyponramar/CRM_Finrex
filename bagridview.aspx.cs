using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Text;
using System.Data;
using System.Collections;
using System.Web.UI.HtmlControls;

public partial class exportexposureportal_bagridview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string module = Common.GetQueryString("m");
            if (module == "exportorder")
            {
                tdbank.Visible = false;
            }
            else if (module == "bank")
            {
                //tdlnkadvsearch.Visible = false;
            }
            BindGridData();
        }
    }
    private void BindGridData()
    {
        string module = Common.GetQueryString("m");
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        string query = @"select * from tbl_columns 
                         join tbl_module on module_moduleid=columns_moduleid
                         where columns_isgenerate=1 AND REPLACE(module_modulename,' ','')='" + module +"' "+
                         "AND (ISNULL(columns_isenableinedit,0) = 0 OR (ISNULL(columns_isenableinedit,0) = 1 " +
                                  "AND columns_columnsid IN(select customercustomfields_columnsid from tbl_customercustomfields WHERE customercustomfields_clientid=" + clientId + "))) " +
                         @" order by columns_sequence";
        DataTable dttblCol = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table class='bagrid' cellspacing='0' cellpadding='5' width='100%' m='"+module+"'><tr class='bagrid-header'>");
        for (int i = 0; i < dttblCol.Rows.Count; i++)
        {
            string label = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_lbl"]);
            string control = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_control"]);
            if (control == "Date")
            {
                label = label + " (dd-mm-yyyy)";
            }
            html.Append("<td>" + label + "</td>");
        }
        if (module != "bank")
        {
            html.Append("<td>Delete</td>");
        }
        html.Append("</tr>");
        
        html.Append("<tr class='bagrid-template'>");
        StringBuilder joinTables = new StringBuilder();
        ArrayList arrSummaryColumns = new ArrayList();
        ArrayList arrSummaryColumnsIndex = new ArrayList();
        for (int i = 0; i < dttblCol.Rows.Count; i++)
        {
            string label = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_lbl"]);
            string columnname = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_columnname"]);
            string dropdowncolumn = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_dropdowncolumn"]);
            string control = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_control"]);
            string attributes = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_attributes"]);
            bool isrequired = GlobalUtilities.ConvertToBool(dttblCol.Rows[i]["columns_isrequired"]);
            bool iscalculationField = GlobalUtilities.ConvertToBool(dttblCol.Rows[i]["columns_isvisibleinedit"]);//calculation field
            string controlHtml = "";
            Array arr1 = columnname.Split('_');
            string shortColumnName = arr1.GetValue(1).ToString();
            string css = "";
            string required = "";
            if (isrequired) required = " ir='1'";
            if (attributes.Contains("IsTotal"))
            {
                arrSummaryColumns.Add("SUM(" + columnname + ")");
                arrSummaryColumnsIndex.Add(i);
            }
            else if (attributes.Contains("IsAvg"))
            {
                if (columnname == "exportorder_costing")
                {
                    arrSummaryColumns.Add(@"CASE WHEN ISNULL(SUM(exportorder_netamount),0) = 0 THEN 0 
                                            ELSE SUM(exportorder_value)/SUM(exportorder_netamount) END AS " + columnname);
                }
                else if (columnname == "forwardcontract_rate")
                {
                    arrSummaryColumns.Add(@"CASE WHEN ISNULL(SUM(forwardcontract_balancesold),0) = 0 THEN 0 
                                            ELSE SUM(forwardcontract_soldamountinrs)/SUM(forwardcontract_balancesold) END AS " + columnname);
                }
                else if (columnname == "pcfc_spotrate")
                {
                    arrSummaryColumns.Add(@"CASE WHEN ISNULL(SUM(pcfc_fcamountbalance),0) = 0 THEN 0 
                                            ELSE SUM(pcfc_product)/SUM(pcfc_fcamountbalance) END AS " + columnname);
                }
                else
                {
                    arrSummaryColumns.Add("AVG(" + columnname + ")");
                }
                arrSummaryColumnsIndex.Add(i);
            }
            if (iscalculationField)
            {
                controlHtml = "<label type='text' id='lblba_" + shortColumnName + "' class='lblba'/>";
            }
            else if (control == "Auto Complete")
            {
                string ddlm = "";
                Array arr = dropdowncolumn.Split('_');
                ddlm = arr.GetValue(0).ToString();
                controlHtml = "<input type='text' id='txtba_" + shortColumnName + "' class='hdnac hidden txtba'/>";
                controlHtml += "<input type='text' m='" + ddlm + "' cn='" + dropdowncolumn + "' id='ba_" + shortColumnName + "' class='ac txtba'"+required+"/>";
                joinTables.Append(" LEFT JOIN tbl_" + ddlm + " ON " + ddlm + "_" + ddlm + "id=" + module + "_" + ddlm + "id");
            }
            else
            {
                string controlExt = "";
                if (control == "Amount")
                {
                    css = "val-dbl";
                    shortColumnName = shortColumnName + "-dbl";
                }
                else if (control == "Number")
                {
                    css = "val-i";
                    shortColumnName = shortColumnName + "-i";
                }
                else if (control == "Date")
                {
                    css = "val-dt";
                    shortColumnName = shortColumnName + "-dt";
                }
                controlHtml = "<input type='text' id='txtba_" + shortColumnName + "' class='txtba "+css+"'" + required + "/>";
            }

            if (i == 0)
            {
                html.Append("<td><input type='text' id='txtba_hdnid' class='hdnbaid' value='0'/>" + controlHtml + "</td>");
            }
            else
            {
                html.Append("<td>" + controlHtml + "</td>");
            }
        }
        if (module != "bank")
        {
            html.Append("<td class='ba-delete'>Delete</td>");
        }
        html.Append("</tr>");
        //get where from search panel
        string where = "";
        if (GlobalUtilities.ConvertToInt(txtcurrencyid.Text) > 0)
        {
            if (where == "")
                where = module + "_exposurecurrencymasterid=" + txtcurrencyid.Text;
            else
                where += " AND " + module + "_exposurecurrencymasterid=" + txtcurrencyid.Text;
        }
        if (GlobalUtilities.ConvertToInt(txtbankid.Text) > 0)
        {
            if (where == "")
                where = module + "_bankid=" + txtbankid.Text;
            else
                where += " AND " + module + "_bankid=" + txtbankid.Text;
        }
        if (ddloutstanding.SelectedValue == "0")
        {
            string outstandwhere = "";
            if (module == "exportorder")
            {
                outstandwhere = "exportorder_netamount > 0";
            }
            else if (module == "forwardcontract")
            {
                outstandwhere = "forwardcontract_balancesold > 0";
            }
            else if (module == "pcfc")
            {
                outstandwhere = "pcfc_fcamountbalance > 0";
            }
            if (where == "")
            {
                where = outstandwhere;
            }
            else
            {
                where += " AND " + outstandwhere;
            }
        }
        
        string keywoardwhere = "";
        if (txtkeyword.Text.Trim() != "")
        {
            if (module == "exportorder")
            {
                keywoardwhere = "(exportorder_exportorderno LIKE '%" + txtkeyword.Text + "%' OR exportorder_customername LIKE '%" + txtkeyword.Text + "%'" +
                                " OR exportorder_invoiceno LIKE '%" + txtkeyword.Text + "%')";
            }
            else if (module == "forwardcontract")
            {
                keywoardwhere = "(forwardcontract_exportorderno LIKE '%" + txtkeyword.Text + "%' OR forwardcontract_bookingno LIKE '%" + txtkeyword.Text + "%'" +
                                " OR forwardcontract_invoiceno LIKE '%" + txtkeyword.Text + "%')";
            }
            else if (module == "pcfc")
            {
                keywoardwhere = "(pcfc_exportorderno LIKE '%" + txtkeyword.Text + "%' OR pcfc_pcfcno LIKE '%" + txtkeyword.Text + "%')";
            }
        }
        if (keywoardwhere != "")
        {
            if (where == "")
            {
                where = keywoardwhere;
            }
            else
            {
                where += " AND " + keywoardwhere;
            }
        }
        if (where == "")
        {
            where = module + "_clientid=" + clientId;
        }
        else
        {
            where += " AND " + module + "_clientid=" + clientId;
        }
        if (where != "") where = " where " + where;
        //get record count
        TotalRecords = 0;
        query = "select count(*) as c from tbl_" + module + joinTables.ToString()+where;
        DataTable dttblDataCount = DbTable.ExecuteSelect(query);
        TotalRecords = GlobalUtilities.ConvertToInt(dttblDataCount.Rows[0][0]);

        //populate the data
        int start = CurrentPageIndex * PageSize;
        int end = CurrentPageIndex * PageSize + PageSize;
        if (ddlPageSize.SelectedValue == "All")
        {
            end = 0;
        }
        query = "select * from tbl_" + module + joinTables.ToString() + where;
        start = start + 1;
        if (end > 0)
        {
            query = "select * from (select *,ROW_NUMBER() OVER(ORDER BY " + module + "_" + module + "id DESC) AS RowNumber from(" + query
                + ")paging1)paging2 where RowNumber between " + start + " and " + end;
        }

        DataTable dttblData = DbTable.ExecuteSelect(query);
        BindPaging();

        for (int k = 0; k < dttblData.Rows.Count; k++)
        {
            html.Append("<tr class='bagrid-row'>");
            for (int i = 0; i < dttblCol.Rows.Count; i++)
            {
                string label = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_lbl"]);
                string columnname = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_columnname"]);
                string dropdowncolumn = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_dropdowncolumn"]);
                string control = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_control"]);
                bool isrequired = GlobalUtilities.ConvertToBool(dttblCol.Rows[i]["columns_isrequired"]);
                bool iscalculationField = GlobalUtilities.ConvertToBool(dttblCol.Rows[i]["columns_isvisibleinedit"]);//calculation field
                string controlHtml = "";
                Array arr1 = columnname.Split('_');
                string shortColumnName = arr1.GetValue(1).ToString();
                string css = "";
                string required = "";
                if (isrequired) required = " ir='1'";

                if (iscalculationField)
                {
                    string val = GlobalUtilities.ConvertToString(dttblData.Rows[k][columnname]);
                    if (control == "Date")
                    {
                        val = GlobalUtilities.ConvertToDate(dttblData.Rows[k][columnname]);
                    }
                    else
                    {
                        val = Common.FormatAmountComma(val);
                    }
                    controlHtml = "<label type='text' id='lblba_" + shortColumnName + "' class='lblba'>" + val + "</label>";
                }
                else if (control == "Auto Complete")
                {
                    string ddlm = "";
                    
                    Array arr = dropdowncolumn.Split('_');
                    ddlm = arr.GetValue(0).ToString();
                    string text = GlobalUtilities.ConvertToString(dttblData.Rows[k][dropdowncolumn]);
                    string ddlid = GlobalUtilities.ConvertToString(dttblData.Rows[k][ddlm + "_" + ddlm + "id"]);
                    controlHtml = "<input type='text' id='txtba_" + shortColumnName + "' class='hdnac hidden txtba' value='"+ddlid+"'/>";
                    controlHtml += "<input type='text' m='" + ddlm + "' cn='" + dropdowncolumn + "' id='ba_" + shortColumnName + "' "+
                                    "value='" + text + "' class='ac txtba'" + required + "/>";
                }
                else
                {
                    string val = "";
                    if (control == "Amount")
                    {
                        css = "val-dbl";
                        shortColumnName = shortColumnName + "-dbl";
                        if (GlobalUtilities.ConvertToDouble(dttblData.Rows[k][columnname]) > 0)
                        {
                            val = GlobalUtilities.ConvertToString(dttblData.Rows[k][columnname]);
                            val = Common.FormatAmountComma(val);
                        }
                    }
                    else if (control == "Number")
                    {
                        css = "val-i";
                        shortColumnName = shortColumnName + "-i";
                        if (GlobalUtilities.ConvertToInt(dttblData.Rows[k][columnname]) > 0)
                        {
                            val = GlobalUtilities.ConvertToString(dttblData.Rows[k][columnname]);
                        }
                    }
                    else if (control == "Date")
                    {
                        css = "val-dt";
                        shortColumnName = shortColumnName + "-dt";
                        val = GlobalUtilities.ConvertToDate(dttblData.Rows[k][columnname]);
                    }
                    else
                    {
                        val = GlobalUtilities.ConvertToString(dttblData.Rows[k][columnname]);
                    }
                    controlHtml = "<input type='text' id='txtba_" + shortColumnName + "' class='txtba "+css+"' value='" + val + "'" + required + "/>";
                }

                if (i == 0)
                {
                    int uniqueId = GlobalUtilities.ConvertToInt(dttblData.Rows[k][module + "_" + module + "id"]);
                    html.Append("<td><input type='text' id='txtba_hdnid' class='hdnbaid' value='" + uniqueId + "'/>" + controlHtml + "</td>");
                }
                else
                {
                    html.Append("<td>" + controlHtml + "</td>");
                }
            }
            if (module != "bank")
            {
                html.Append("<td class='ba-delete'>Delete</td>");
            }
            html.Append("</tr>");
        }
        if (arrSummaryColumns.Count > 0)
        {
            query = "select ";
            for (int i = 0; i < arrSummaryColumns.Count; i++)
            {
                if (i == 0)
                {
                    query += arrSummaryColumns[i].ToString();
                }
                else
                {
                    query += "," + arrSummaryColumns[i].ToString();
                }
            }
            if (where == "")
            {
                query += " from tbl_" + module;
            }
            else
            {
                query += " from(select * from tbl_" + module + where + " )r";
            }
            DataTable dttblSummary = DbTable.ExecuteSelect(query);
            if (GlobalUtilities.IsValidaTable(dttblSummary))
            {
                html.Append("<tr><td>&nbsp;</td></tr><tr class='bold'><td>Gross Total</td>");
                int index = 0;
                for (int i = 1; i < dttblCol.Rows.Count; i++)
                {
                    
                    if (arrSummaryColumnsIndex.Contains(i))
                    {
                        html.Append("<td>" + Common.FormatAmountComma(GlobalUtilities.FormatAmount(dttblSummary.Rows[0][index])) + "</td>");
                        index++;
                    }
                    else
                    {
                        html.Append("<td>&nbsp;</td>");
                    }
                }
                html.Append("<tr>");
            }
        }
        //html.Append(@"<tr class='bagrid-footer'>");
        //for (int i = 0; i < dttblCol.Rows.Count; i++)
        //{
        //    if (i == 0)
        //    {
        //        html.Append("<td style='color:#035081;height:20px;cursor:pointer;'>Add New</td>");
        //    }
        //    else
        //    {
        //        html.Append("<td>&nbsp;</td>");
        //    }
        //}
        //html.Append("</tr>");
        ltbagrid.Text = html.ToString();

        
    }
    protected void lnkPrevPage_Click(object sender, EventArgs e)
    {
        if (CurrentPageIndex != 0)
        {
            CurrentPageIndex = CurrentPageIndex - 1;
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
            BindGridData();

            if (CurrentPageIndex + 1 == TotalPages)
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
    protected void lnkPage_OnCommand(object sender, CommandEventArgs e)
    {
        CurrentPageIndex = Convert.ToInt32(e.CommandArgument);
        if (CurrentPageIndex + 1 == TotalPages)
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
        BindGridData();
    }
    protected void ddlPageSize_Changed(object sender, EventArgs e)
    {
        CurrentPageIndex = 0;
        BindGridData();
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
    private void BindPaging()
    {
        int startIndex = 1;
        int endIndex = TotalPages;
        int maxPages = 5;
        for (int i = 0; i < TotalPages; i++)
        {
            int PageBoxStart = maxPages * i;
            int PageBoxEnd = maxPages * (i + 1);
            if (CurrentPageIndex >= PageBoxStart && CurrentPageIndex <= PageBoxEnd)
            {
                startIndex = PageBoxStart + 1;
                endIndex = startIndex + maxPages - 1;
                if (endIndex > TotalPages)
                {
                    endIndex = TotalPages;
                    break;
                }
            }
        }
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
                HtmlControl td = (HtmlControl)rptPaging.Items[row].FindControl("page_td");
                td.Style.Add("background-color", "#0f65f1;");
                lnk.Style.Add("color", "#ffffff");
            }
            row++;
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
    public int CurrentPageIndex
    {
        get { return Convert.ToInt32(ViewState["CurrentPageIndex"]); }
        set { ViewState["CurrentPageIndex"] = value; }
    }
    public int PageSize
    {
        get
        {
            if (ddlPageSize.SelectedValue == "All")
            {
                return Int32.MaxValue;
            }
            else
            {
                return Convert.ToInt32(ddlPageSize.SelectedValue);
            }
        }
    }
    public int TotalRecords
    {
        get { return Convert.ToInt32(ViewState["TotalRecords"]); }
        set { ViewState["TotalRecords"] = value; }
    }
    protected void btnsearch_click(object sender, EventArgs e)
    {
        BindGridData();
    }
}
