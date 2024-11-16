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
using System.Configuration;

public partial class exportexposureportal_bagridview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string module = Common.GetQueryString("m");
            if (module == "exportorder")
            {
                //tdbank.Visible = false;
            }
            else if (module == "bank")
            {
            }
            int portalType = 1;
            if (module == "fimforwardcancellationdetail" || module == "fimforwardutilitsationdetail" || module == "fimforwardcontract"
                 || module == "fimtradecreditdetail" || module == "fimtradecredit" || module == "fimorderinvoice"
                 || module == "fimimportorder")
            {
                portalType = 2;
            }
            if(!ExportExposurePortal.IsFEMPortalEnabled(portalType))
            {
                Response.Redirect("~/customerlogin.aspx"); return;
            }
            BindGridData();
            if (Common.GetQueryStringBool("issubmodule") || module== "bank")
            {
                tradvsearch.Visible = false;
            }
        }
        string script = "<script>$(document).ready(function(){bindSummaryDetail();});</script>";
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "currentpage", script);
    }
    private DataTable GetColumns()
    {
        string module = Common.GetQueryString("m");
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        string query = @"select * from tbl_columns 
                         join tbl_module on module_moduleid=columns_moduleid
                         where columns_isgenerate=1 AND REPLACE(module_modulename,' ','')='" + module + "' " +
                         "AND (ISNULL(columns_isenableinedit,0) = 0 OR (ISNULL(columns_isenableinedit,0) = 1 " +
                                  "AND columns_columnsid IN(select customercustomfields_columnsid from tbl_customercustomfields WHERE customercustomfields_clientid=" + clientId + "))) " +
                         @" order by columns_sequence";
        DataTable dttblCol = DbTable.ExecuteSelect(query);
        return dttblCol;
    }
    private void BindGridData()
    {
        string module = Common.GetQueryString("m");
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        string query = "";
        DataTable dttblCol = GetColumns();
        StringBuilder html = new StringBuilder();
        html.Append("<table class='bagrid' cellspacing='0' cellpadding='5' width='100%' m='" + module + "' "+
                         "pid='" + Common.GetQueryStringValue("pid") + "' pm='"+Common.GetQueryString("pm")+"'>" +
                        "<tr class='bagrid-header'>");
        HtmlTableRow trSearch = new HtmlTableRow();
        tblSearch.Controls.Add(trSearch);
        if (!IsPostBack)
        {
            ddldatewise.Items.Clear();
            ddldatewise.Items.Add(new ListItem("Select Datewise", "0"));
            ddlamountwise.Items.Clear();
            ddlamountwise.Items.Add(new ListItem("Select Amountwise", "0"));
        }
        bool isMoreSearch = false;
        for (int i = 0; i < dttblCol.Rows.Count; i++)
        {
            string label = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_lbl"]);
            string attributes = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_attributes"]);
            string control = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_control"]);
            string columnName = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_columnname"]);
            string dropdownColumn = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_dropdowncolumn"]);
            bool isSearch = GlobalUtilities.ConvertToBool(dttblCol.Rows[i]["columns_issearchfield"]);
            string shortColumn = columnName.Split('_').GetValue(1).ToString();
            string label2 = label;
            if (control == "Date" || control == "Amount")
            {
                if (isSearch && !IsPostBack)
                {
                    if (control == "Date")
                    {
                        ddldatewise.Items.Add(new ListItem(label, columnName));
                    }
                    else if (control == "Amount")
                    {
                        ddlamountwise.Items.Add(new ListItem(label, columnName));
                    }
                }
                isSearch = false;
            }
            if (control == "Date")
            {
                label2 += " (dd-mm-yyyy)";
            }
            string style = "";
            if (attributes.ToLower().Contains("width"))
            {
                style = " style='min-width:" + Common.GetAttributes(attributes, "width") + "px;'";
            }
            html.Append("<td cn='" + shortColumn + "'" + style + ">" + label2 + "</td>");
            if (isSearch)
            {
                tdMoreSearch.Visible = true;
                HtmlTableCell td1 = new HtmlTableCell();
                trSearch.Controls.Add(td1);
                td1.InnerText = label;
                if (control == "Date")
                {
                    HtmlTableCell td2 = new HtmlTableCell();
                    TextBox txtfromdate = new TextBox();
                    txtfromdate.ID = "txtsearch_" + columnName + "_fromdate";
                    txtfromdate.CssClass = "datepicker";
                    txtfromdate.Width = 70;
                    txtfromdate.Text = GetData(txtfromdate.ID);
                    td2.Controls.Add(txtfromdate);
                    TextBox txttodate = new TextBox();
                    txttodate.ID = "txtsearch_" + columnName + "_todate";
                    txttodate.CssClass = "datepicker";
                    txttodate.Width = 70;
                    td2.Controls.Add(txttodate);
                    txttodate.Text = GetData(txttodate.ID);
                    trSearch.Controls.Add(td2);
                }
                else if (control == "Auto Complete")
                {
                    HtmlTableCell td2 = new HtmlTableCell();
                    TextBox txthdnac = new TextBox();
                    txthdnac.ID = "txtsearch_" + columnName + "_ac";
                    txthdnac.CssClass = "hdnac";
                    td2.Controls.Add(txthdnac);
                    txthdnac.Text = GetData(txthdnac.ID);
                    TextBox txtac = new TextBox();
                    txtac.ID = "acsearch_" + columnName;
                    txtac.CssClass = "ac";
                    txtac.Width = 100;
                    txtac.Attributes.Add("m", dropdownColumn.Split('_').GetValue(0).ToString());
                    txtac.Attributes.Add("cn", dropdownColumn);
                    td2.Controls.Add(txtac);
                    txtac.Text = GetData(txtac.ID);
                    trSearch.Controls.Add(td2);
                }
                else if (control == "Amount")
                {
                    HtmlTableCell td2 = new HtmlTableCell();
                    TextBox txtfromamount = new TextBox();
                    txtfromamount.ID = "txtsearch_" + columnName + "_fromamount";
                    txtfromamount.CssClass = "val-dbl";
                    txtfromamount.Width = 50;
                    txtfromamount.Text = GetData(txtfromamount.ID);
                    td2.Controls.Add(txtfromamount);
                    TextBox txttoamount = new TextBox();
                    txttoamount.ID = "txtsearch_" + columnName + "_toamount";
                    txttoamount.CssClass = "val-dbl";
                    txttoamount.Width = 50;
                    td2.Controls.Add(txttoamount);
                    txttoamount.Text = GetData(txttoamount.ID);
                    trSearch.Controls.Add(td2);
                }
                else
                {
                    HtmlTableCell td2 = new HtmlTableCell();
                    TextBox txtsearch = new TextBox();
                    txtsearch.ID = "txtsearch_" + columnName + "_text";
                    txtsearch.CssClass = "textbox";
                    txtsearch.Width = 70;
                    txtsearch.Text = GetData(txtsearch.ID);
                    td2.Controls.Add(txtsearch);
                    trSearch.Controls.Add(td2);
                }
            }
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
        GetColculatedColumns(dttblCol, out arrSummaryColumns, out arrSummaryColumnsIndex);
        for (int i = 0; i < dttblCol.Rows.Count; i++)
        {
            string label = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_lbl"]);
            string columnname = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_columnname"]);
            string dropdowncolumn = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_dropdowncolumn"]);
            string control = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_control"]);
            string attributes = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_attributes"]);
            bool isrequired = GlobalUtilities.ConvertToBool(dttblCol.Rows[i]["columns_isrequired"]);
            bool iscalculationField = GlobalUtilities.ConvertToBool(dttblCol.Rows[i]["columns_isvisibleinedit"]);//calculation field
            string css = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_css"]);
            string controlHtml = "";
            Array arr1 = columnname.Split('_');
            string shortColumnName = arr1.GetValue(1).ToString();
            string required = "";
            if (isrequired) required = " ir='1'";
            string submodue = Common.GetAttributes(attributes, "SubModule");
            
            if (iscalculationField)
            {
                controlHtml = "<label type='text' id='lblba_" + shortColumnName + "' class='lblba "+css+"'/>";
            }
            else if (submodue !="")
            {
                controlHtml = @"<label type='text' id='lblba_" + shortColumnName + @"' class='lblba "+css+@"'></label>
                               <div class='btnfem-submodule' sm='" + submodue + "' lbl='" + label + "'>Detail</div>";
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

        string where = GetWhere();

        //get record count
        TotalRecords = 0;
        query = "select count(*) as c from tbl_" + module + joinTables.ToString() + where;
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
        //ErrorLog.WriteLog(query);
        if (TotalRecords < PageSize)
        {
            trpaging.Visible = false;
            tdPaging_top.Visible = false;
        }
        else
        {
            trpaging.Visible = true;
            tdPaging_top.Visible = true;
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
                string attributes = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_attributes"]);
                string css = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_css"]);
                string controlHtml = "";
                Array arr1 = columnname.Split('_');
                string shortColumnName = arr1.GetValue(1).ToString();
                string required = "";
                if (isrequired) required = " ir='1'";
                string submodue = Common.GetAttributes(attributes, "SubModule");

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
                    if (css.Contains("nonzero") && GlobalUtilities.ConvertToDouble(dttblData.Rows[k][columnname]) == 0) val = "-";
                    controlHtml = "<label type='text' id='lblba_" + shortColumnName + "' class='lblba " + css + "'>" + val + "</label>";
                }
                else if (submodue != "")
                {
                    string val = Common.FormatAmountComma(GlobalUtilities.ConvertToString(dttblData.Rows[k][columnname]));
                    controlHtml = @"<label type='text' id='lblba_" + shortColumnName + @"' class='lblba " + css + "'>" + val + @"</label>
                                    <div class='btnfem-submodule' sm='" + submodue + "' lbl='" + label + "'>Detail</div>";
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
                        css += " val-dbl";
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
            //html.Append("<tr><td>&nbsp;</td></tr><tr class='bold bagrid-footer'>");
            html.Append("<tr class='bold bagrid-footer'>");
            for (int i = 0; i < dttblCol.Rows.Count; i++)
            {
                html.Append("<td class='bold right' cn='" + GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_columnname"]).Split('_').GetValue(1).ToString() + "'>&nbsp;</td>");
            }
            html.Append("</tr>");
          
        }

        ltbagrid.Text = html.ToString();

        
    }
    private string GetWhere()
    {
        string module = Common.GetQueryString("m");
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        //get where from search panel
        string where = "";
        string keyword = global.CheckInputData(txtkeyword.Text);
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
            else if (module == "fimimportorder")
            {
                outstandwhere = "fimimportorder_netimportorderamountpayable > 0";
            }
            else if (module == "fimtradecredit")
            {
                outstandwhere = "fimtradecredit_outstandingtradecreditamount > 0";
            }
            else if (module == "fimforwardcontract")
            {
                outstandwhere = "fimforwardcontract_forwardbalanceamount > 0";
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
        if (keyword.Trim() != "")
        {
            if (module == "exportorder")
            {
                keywoardwhere = "(exportorder_exportorderno LIKE '%" + keyword + "%' OR exportorder_customername LIKE '%" + keyword + "%'" +
                                " OR exportorder_invoiceno LIKE '%" + keyword + "%')";
            }
            else if (module == "forwardcontract")
            {
                keywoardwhere = "(forwardcontract_exportorderno LIKE '%" + keyword + "%' OR forwardcontract_bookingno LIKE '%" + keyword + "%'" +
                                " OR forwardcontract_invoiceno LIKE '%" + keyword + "%')";
            }
            else if (module == "pcfc")
            {
                keywoardwhere = "(pcfc_exportorderno LIKE '%" + keyword + "%' OR pcfc_pcfcno LIKE '%" + keyword + "%')";
            }
            else if (module == "fimimportorder")
            {
                keywoardwhere = "(fimimportorder_importorderinvoiceno LIKE '%" + keyword + "%' OR fimimportorder_bankrefno LIKE '%" + keyword + "%')";
            }
            else if (module == "fimtradecredit")
            {
                keywoardwhere = "(fimimportorder_importorderinvoiceno LIKE '%" + keyword + "%' OR fimtradecredit_tradecreditbankrefno LIKE '%" + keyword + "%')";
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
        //search
        for (int i = 0; i < Request.Form.Keys.Count; i++)
        {
            string key = Request.Form.Keys[i];
            string val = global.CheckInputData(Request.Form[i]).Trim();
            if (key.StartsWith("ctl00$ContentPlaceHolder1$txtsearch_"))
            {
                key = key.Replace("ctl00$ContentPlaceHolder1$txtsearch_", "");
                Array arr = key.Split('_');
                string colName = arr.GetValue(0).ToString() + "_" + arr.GetValue(1).ToString();
                string controlType = arr.GetValue(2).ToString();
                string searchWhere = "";
                if (val != "")
                {
                    if (controlType == "fromdate")
                    {
                        searchWhere = "cast(" + colName + " as date)" + ">=cast('" + GlobalUtilities.ConvertMMDateToDD(val) + "' as date)";
                    }
                    else if (controlType == "todate")
                    {
                        searchWhere = "cast(" + colName + " as date)" + "<=cast('" + GlobalUtilities.ConvertMMDateToDD(val) + "' as date)";
                    }
                    else if (controlType == "ac")
                    {
                        if (GlobalUtilities.ConvertToInt(val) > 0)
                        {
                            searchWhere = colName + "=" + val;
                        }
                    }
                    else if (controlType == "fromamount")
                    {
                        searchWhere = colName + ">=" + GlobalUtilities.ConvertToDouble(val);
                    }
                    else if (controlType == "toamount")
                    {
                        searchWhere = colName + "<=" + GlobalUtilities.ConvertToDouble(val);
                    }
                    else if (controlType == "text")
                    {
                        searchWhere = colName + "='" + val + "'";
                    }
                }
                if (searchWhere != "") where += " AND " + searchWhere;
            }
        }
        if (ddldatewise.SelectedValue != "0")
        {
            if (txtfromdatewise.Text != "")
            {
                where += " AND cast(" + ddldatewise.SelectedValue + " as date)" + ">=cast('" + GlobalUtilities.ConvertMMDateToDD(txtfromdatewise.Text) + "' as date)";
            }
            if (txttodatewise.Text != "")
            {
                where += " AND cast(" + ddldatewise.SelectedValue + " as date)" + "<=cast('" + GlobalUtilities.ConvertMMDateToDD(txttodatewise.Text) + "' as date)";
            }
        }
        if (ddlamountwise.SelectedValue != "0")
        {
            if (txtamountwise_from.Text != "")
            {
                where += " AND " + ddlamountwise.SelectedValue + ">=" + GlobalUtilities.ConvertToDouble(txtamountwise_from.Text);
            }
            if (txtamountwise_to.Text != "")
            {
                where += " AND " + ddlamountwise.SelectedValue + "<=" + GlobalUtilities.ConvertToDouble(txtamountwise_to.Text);
            }
        }
        if (Common.GetQueryString("pm") != "")
        {
            string parentModWhere = module + "_" + Common.GetQueryString("pm") + "id=" + Common.GetQueryStringValue("pid");
            if (where == "")
            {
                where = parentModWhere;
            }
            else
            {
                where += " AND " + parentModWhere;
            }
        }
        txtwhere.Text = where.Replace("=", "~");
        if (where != "") where = " where " + where;
        return where;
    }
    private string GetData(string controlId)
    {
        return Request.Form["ctl00$ContentPlaceHolder1$" + controlId];
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
        ddlPageSize.SelectedValue = ((DropDownList)sender).SelectedValue;
        ddlPageSize_top.SelectedValue = ((DropDownList)sender).SelectedValue;
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
        rptPaging_top.DataSource = arr;
        rptPaging_top.DataBind();
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
            lnk = (LinkButton)rptPaging_top.Items[row].FindControl("lnkPage");
            lnk.Text = "&nbsp;" + i.ToString() + "&nbsp;";
            lnk.CommandArgument = Convert.ToString(i - 1);
            if (i - 1 == CurrentPageIndex)
            {
                HtmlControl td = (HtmlControl)rptPaging_top.Items[row].FindControl("page_td");
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
        lblTotalRecords_top.Text = TotalRecordSummary;
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
    protected void btnexport_click(object sender, EventArgs e)
    {
        string module = Common.GetQueryString("m");
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        DataTable dttblCol = GetColumns();
        StringBuilder joinTables = new StringBuilder();
        DataTable dttblExcel = new DataTable();
        dttblExcel.Columns.Add("SrNo");
        for (int i = 0; i < dttblCol.Rows.Count; i++)
        {
            string label = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_lbl"]);
            string columnname = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_columnname"]);
            string dropdowncolumn = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_dropdowncolumn"]);
            string control = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_control"]);
            if (control == "Auto Complete")
            {
                string ddlm = "";
                Array arr = dropdowncolumn.Split('_');
                ddlm = arr.GetValue(0).ToString();
                joinTables.Append(" LEFT JOIN tbl_" + ddlm + " ON " + ddlm + "_" + ddlm + "id=" + module + "_" + ddlm + "id");
                columnname = dropdowncolumn;
            }
            dttblExcel.Columns.Add(label);
            if (control == "Date")
            {
                dttblExcel.Columns[label].Caption = columnname + "-date";
            }
            else
            {
                dttblExcel.Columns[label].Caption = columnname;
            }
        }
        string where = GetWhere();
        string query = "select * from tbl_" + module + joinTables.ToString() + where;
        DataTable dttblData = DbTable.ExecuteSelect(query);
        for (int i = 0; i < dttblData.Rows.Count; i++)
        {
            DataRow dr = dttblExcel.NewRow();
            dr["SrNo"] = i + 1;
            for (int j = 1; j < dttblExcel.Columns.Count; j++)
            {
                string colName = dttblExcel.Columns[j].Caption;
                if (colName.Contains("-date"))
                {
                    colName = colName.Replace("-date", "");
                    dr[dttblExcel.Columns[j].ColumnName] = GlobalUtilities.ConvertToDate(dttblData.Rows[i][colName]);
                }
                else
                {
                    dr[dttblExcel.Columns[j].ColumnName] = GlobalUtilities.ConvertToString(dttblData.Rows[i][colName]);
                }
            }
            dttblExcel.Rows.Add(dr);
        }
        ExportExposurePortal obj = new ExportExposurePortal(clientId);
        DataRow drSummary = obj.GetSummaryDataRow(module, txtwhere.Text);
        DataRow drExcelSummary = dttblExcel.NewRow();
        drExcelSummary["SrNo"] = "Gross Total";
        for (int i = 0; i < dttblExcel.Columns.Count; i++)
        {
            string colName = dttblExcel.Columns[i].Caption;
            for (int j = 0; j < drSummary.Table.Columns.Count; j++)
            {
                if (colName.EndsWith("_" + drSummary.Table.Columns[j]))
                {
                    drExcelSummary[dttblExcel.Columns[i].ColumnName] = drSummary[j];
                    break;
                }
            }
        }
        dttblExcel.Rows.Add(drExcelSummary);
        GlobalUtilities.ExportToExcel(dttblExcel);
    }

    private void GetColculatedColumns(DataTable dttblCol, out ArrayList arrSummaryColumns, out ArrayList arrSummaryColumnsIndex)
    {
        arrSummaryColumns = new ArrayList();
        arrSummaryColumnsIndex = new ArrayList();
        for (int i = 0; i < dttblCol.Rows.Count; i++)
        {
            string columnname = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_columnname"]);
            string attributes = GlobalUtilities.ConvertToString(dttblCol.Rows[i]["columns_attributes"]);
            
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
        }
    }

    public string VersionNo
    {
        get
        {
            return ConfigurationManager.AppSettings["VersionNo"].ToString();
        }
    }
}
