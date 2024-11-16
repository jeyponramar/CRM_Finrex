<%@ WebHandler Language="C#" Class="grid" %>

using System;
using System.Web;
using WebComponent;
using System.Data;
using System.Text;
using System.Xml;

public class grid : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string gridQuery = GlobalUtilities.ConvertToString(context.Session["s_gridquery"]);
        int pageNo = GlobalUtilities.ConvertToInt(context.Request.QueryString["p"]);
        string sorting = GlobalUtilities.ConvertToString(context.Request.QueryString["sb"]);
        string module = GlobalUtilities.ConvertToString(context.Request.QueryString["m"]);
        module = module.Replace(" ", "").ToLower();
        GridSettings gridSettings = new GridSettings();
        gridSettings.BindXml(module);
        
        int pageSize = 20;
        int start = pageNo * pageSize + 1;
        int end = pageNo * pageSize + pageSize;
        
        string pagingQuery = "select * from (select *,ROW_NUMBER() OVER(ORDER BY " + sorting + ") AS RowNumber from(" + gridQuery
                             + ")paging1)paging2 where RowNumber between " + start + " and " + end;
        DataTable dttbl = DbTable.ExecuteSelect(pagingQuery);
        context.Response.Write(GetGridHtml(dttbl, gridSettings, module, start, end));
    }
    private string GetGridHtml(DataTable Data, GridSettings gridSettings, string module, int start, int end)
    {
        StringBuilder html = new StringBuilder();
        if (!GlobalUtilities.IsValidaTable(Data))
        {
            return "";
        }
        string idcol = "";
        if (gridSettings.IdentityColumn == "")
        {
            idcol = module + "_" + module + "id";
        }
        else
        {
            idcol = gridSettings.IdentityColumn.Replace("\n", "").Replace("\t", "").Trim();
        }
        if (idcol == "none") idcol = "";

        bool isStatusColumnExists = false;
        string statusColorColumn_bg = "";
        string statusColorColumn_text = "";
        string statusColor_bg = "";
        string statusColor_text = "";

        if (!module.ToLower().Contains("status") || gridSettings.IsReport)
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
            string rptClass = "repeater-alt-m";
            if (i % 2 == 0)
            {
                rptClass = "repeater-row-m";
            }

            html.Append("<tr>");
            int row = 0;
            int prevRow = 0;
            int id = 0;

            html.Append("<td class='" + rptClass + "'><table width='100%'><tr>");
            html.Append("<td><table width='100%'><tr>");
            bool showColumn = true;
            bool isMobileColumn = false;
            bool isFirstCol = true;
            for (int j = 0; j < gridSettings.xmlGridColumns.Count; j++)
            {
                string colName = gridSettings.xmlGridColumns[j].InnerText;
                XmlNode xmlNode = gridSettings.xmlGridColumns[j];
                showColumn = IsMobileColumn(xmlNode);
                isMobileColumn = showColumn;

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
                    string headerText = gridSettings.xmlGridHeaders[j].InnerText;
                    string columnWidth = gridSettings.xmlGridColumnWidth[j].InnerText;
                    string val = Convert.ToString(Data.Rows[i][colName]);
                    string dataFormat = gridSettings.xmlGridColumnFormat[j].InnerText.ToLower();
                    string width = "";
                    string css = "";
                    string idval = "";

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
                            val = GlobalUtilities.ConvertToDateTime(val);
                        }
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
                                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + imgPath + "/" + id + "." + val.Trim())))
                                {
                                    fileName = id + "." + val.Trim();
                                }
                            }
                            val = "<img src='../../" + imgPath + "/" + fileName + "' height='30'/>";
                        }
                    }
                    if (val == "") val = "&nbsp;";
                    row = Convert.ToInt32(gridSettings.xmlGridColumnRow[j].InnerText);

                    if (j == 0) css = " idval";
                    int colspan = 0;
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
                        isFirstCol = !isFirstCol;
                    }
                    prevRow = row;
                }
            }
            html.Append("</tr></table></td><td class='grid-arrow' align='right'>&nbsp;</td>");
            html.Append("</tr></table></td>");
            html.Append("</tr>");
        }
        int totalRecs=GetRecordCount();
        if (end > totalRecs) end = totalRecs;
        html.Append("<tr><td colspan='10' align='center' class='paging-count'>Showing " + start + " to " + end + " of " + totalRecs + "</td></tr>");
        return html.ToString();
    }
    private int GetRecordCount()
    {
        string gridQuery = GlobalUtilities.ConvertToString(HttpContext.Current.Session["s_gridquery"]);
        string countQuery = "select count(*) from(" + gridQuery + ")s_count";
        InsertUpdate obj1 = new InsertUpdate();
        DataTable dttblCount = obj1.ExecuteSelect(countQuery);
        return Convert.ToInt32(dttblCount.Rows[0][0]);
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
    public bool IsReusable {
        get {
            return false;
        }
    }

}