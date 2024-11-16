using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Web.UI;
using System.Xml;
using WebComponent;
using System.Collections;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for NonEditableSubGrid
/// </summary>
public static class NonEditableJSGrid
{
    public static string _RootNode { get { return "/setting/noneditablegrid"; } }
    public static void bindSubGridDetails(Literal lt, string Stored_Procedure_Name)
    {        
       // ExecStoredProcedureDAO dao = new ExecStoredProcedureDAO(Stored_Procedure_Name,
    }
    public static void bindSubGridDetails(string Stored_Procedure_Name)
    {
       // ExecStoredProcedureDAO dao = new ExecStoredProcedureDAO(Stored_Procedure_Name
    }
    public static void bindGrid(string strNodeModuleName)
    {
        bindGrid(strNodeModuleName, Common.GetModuleName());
    }    
    public static string bindGrid(string strNodename, string strModuleName)
    {        
        string strRootNode = _RootNode + "[@name='" + strModuleName + "']";
        Array arrGridColumns = XMLNodeBinder.getSingleNode(strRootNode + "/columns");
        Array arrGridColumnsHeader = XMLNodeBinder.getSingleNode(strRootNode + "/columnlabels");
        string strQuery = XMLNodeBinder.getSingleNodeText(strRootNode + "/query");
        strQuery = XMLQueryBuilder.bulidXMLQuery(strQuery);
        if (strQuery != "")
        {
           return BindGrid(DbTable.ExecuteSelect(strQuery), arrGridColumns, arrGridColumnsHeader);
        }
        else
        {
            return "<div class='error'>No Data Found!</div>";
        }

    }
    public static string BindGrid(DataTable dttbl, Array _arrColumns, Array _arrHeaderColumns)
    {
        StringBuilder html = new StringBuilder();
        if (GlobalUtilities.IsValidaTable(dttbl))
        {
            html.Append(@"<table cellpadding='0' cellspacing='0' width='100%'>");
            html.Append(@"<tr><td class='sub-title'>Product Details</td></tr>
                            <tr><td>
                                <table cellpadding='4' cellspacing='0' class='grid' width='100%' >
                                    <tr class='repeater-header'>");

            string strCssClass = "";            
            for (int i = 0; i < _arrHeaderColumns.Length; i++)
            {
                html.Append("<td class='" + strCssClass + "' style='padding-left:5px;'>" + _arrHeaderColumns.GetValue(i).ToString() + "</td>");
            }
            html.Append("</tr>");


            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                strCssClass = (i % 2 == 0) ? "repeater-row" : "repeater-alt";
                html.Append("<tr class='" + strCssClass + "'>");
                for (int j = 0; j < _arrColumns.Length&&_arrColumns.Length==_arrHeaderColumns.Length; j++)
                {
                    string strColumnValue = GlobalUtilities.ConvertToString(dttbl.Rows[i][_arrColumns.GetValue(j).ToString()]);
                    string strColumnName = GlobalUtilities.ConvertToString(_arrColumns.GetValue(j).ToString());
                    if (strColumnName.ToLower().Contains("date"))
                    {
                       strColumnValue = Formatter.ConvertToMonthDate(strColumnValue);                        
                    }                    
                    html.Append("<td>" + strColumnValue + "</td>");
                }
                html.Append("</tr>");
            }
            html.Append("</table>");
        }
        else
        {
            html.Append("<div class='error'>No data found</div>");
        }
        return html.ToString();
    }
}
