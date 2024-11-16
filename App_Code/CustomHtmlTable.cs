using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using System.Text;

/// <summary>
/// Summary description for CustomHtmlTable
/// </summary>
public class CustomHtmlTable
{
    private ArrayList _arrColWidth = new ArrayList();

	public CustomHtmlTable()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string BindGrid(DataTable dttbl, string headerNames, string columnNames)
    {
        return BindGrid(dttbl, headerNames, columnNames, "", "");
    }
    public string BindGrid(DataTable dttbl,string headerNames, string columnNames,string editUrl,string identityColumn)
    {
        StringBuilder html = new StringBuilder();
            html.Append(@"<table cellpadding='0' cellspacing='0' width='100%' style='background-color:#afddf8;border-radius:9px;'>
                                <tr class='title'>");

            string strTempCssClass = "br-bt-bb";
            string strCssClass = "";
            string strBackgroundcssClass = "style='background-color:#afddf8;'";
            Array _arrColumns = columnNames.Split(',');
            Array _arrHeaderColumns = headerNames.Split(',');
            ArrayList arrTableColumns = new ArrayList();
            for (int i = 0; i < _arrColumns.Length; i++)
            {
                string colName = Convert.ToString(_arrColumns.GetValue(i)).ToLower();
                if (colName.IndexOf("_") > 0)
                {
                    arrTableColumns.Add(colName);
                }
                else
                {
                    for (int j = 0; j < dttbl.Columns.Count; j++)
                    {
                        if (dttbl.Columns[j].ColumnName.ToLower() == colName || dttbl.Columns[j].ColumnName.ToLower().EndsWith("_" + colName.ToLower()))
                        {
                            arrTableColumns.Add(dttbl.Columns[j].ColumnName);
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < _arrHeaderColumns.Length; i++)
            {
                if (i == 0) strCssClass = "br-full";
                else strCssClass = strTempCssClass;
                html.Append("<td class='" + strCssClass + "' style='padding-left:5px;'>" + _arrHeaderColumns.GetValue(i).ToString() + "</td>");
            }
            if (editUrl != "")
            {
                html.Append("<td class='br-full'>Edit</td>");
            }
            html.Append("</tr>");            
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                html.Append("<tr>");         
                for (int j = 0; j < arrTableColumns.Count; j++)
                {                    
                    string strColumnValue = Convert.ToString(dttbl.Rows[i][arrTableColumns[j].ToString()]);
                    string strColumnName = Convert.ToString(_arrColumns.GetValue(j).ToString());
                    if (strColumnName.ToLower().Contains("date"))
                    {
                        try
                        {
                            strColumnValue = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(strColumnValue));
                        }
                        catch { }                        
                    }
                    html.Append("<td class='br-bt-bb' style='padding-left:5px;'>" + strColumnValue + "</td>");                    
                }
                if (editUrl != "")
                {

                    html.Append("<td class='br-bt-bb'><a href='" + editUrl.Replace("$id$", Convert.ToString(dttbl.Rows[i][identityColumn])) + "'>Edit</a></td>");
                }
                html.Append("</tr>");
            }
            if (!GlobalUtilities.IsValidaTable(dttbl))
            {
                html.Append("<tr><td align='center' colspan='10' class='error'>No data found!</td></tr>");
            }
            //html.Append("</tr>");
            html.Append("</table>");   
        
        return html.ToString();
    }
}
