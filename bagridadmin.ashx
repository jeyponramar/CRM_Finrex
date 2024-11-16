<%@ WebHandler Language="C#" Class="bagridexportexposure" %>

using System;
using System.Web;
using WebComponent;
using System.Data;
using System.Collections;

public class bagridexportexposure : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        //try
        {
            int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
            string m = Common.GetQueryString("m");
            if (context.Request.QueryString["a"] == "d")
            {
                DeleteData(m, clientId);
                return;
            }
            int id = GlobalUtilities.ConvertToInt(context.Request.Form["txtba_hdnid"]);
            //if (!IsValid(m, id, clientId)) return;
            Hashtable hstbl = new Hashtable();
            
            for (int i = 0; i < context.Request.Form.Keys.Count; i++)
            {
                string key = context.Request.Form.Keys[i];
                if (key.StartsWith("txtba_") && key != "txtba_hdnid")
                {
                    string columnName = key.Replace("txtba_", "");
                    string val = global.CheckInputData(context.Request.Form[key]);
                    if (columnName.EndsWith("-dbl"))
                    {
                        columnName = columnName.Replace("-dbl", "");
                        val = GlobalUtilities.ConvertToDouble(val).ToString();
                    }
                    else if (columnName.EndsWith("-i"))
                    {
                        columnName = columnName.Replace("-i", "");
                        val = GlobalUtilities.ConvertToInt(val).ToString();
                    }
                    else if (columnName.EndsWith("-dt"))
                    {
                        columnName = columnName.Replace("-dt", "");
                    }
                    if (!hstbl.Contains(columnName))
                    {
                        hstbl.Add(columnName, val);
                    }
                }
            }
            hstbl.Add("employeeid", Common.EmployeeId);
            InsertUpdate obj = new InsertUpdate();
            int newid = 0;
            if (id == 0)
            {
                newid = obj.InsertData(hstbl, "tbl_" + m);
            }
            else
            {
                newid = obj.UpdateData(hstbl, "tbl_" + m, id);
            }
            if (newid > 0)
            {
                m = Common.GetQueryString("m");
                
                string query = "select " + m + "_" + m + "id as id,* from tbl_" + m + " where " + m + "_" + m + "id=" + newid;
                DataRow dr = DbTable.ExecuteSelectRow(query);
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    if (dr.Table.Columns[i].DataType == typeof(System.Decimal))
                    {
                        string val = GlobalUtilities.ConvertToString(dr[i]);
                        if (val == "")
                        {
                            dr[i] = "0.00";
                        }
                        else
                        {
                            dr[i] = Common.FormatAmountComma(val);
                        }
                    }
                }
                string data = JSON.ConvertAmountComma(dr, "", true);
                context.Response.Write(data);
            }
            else
            {
                context.Response.Write("Error");
            }
        }
        //catch (Exception ex)
        {
            //context.Response.Write("Error : " + ex.Message);
        }
    }
    private void DeleteData(string m, int clientId)
    {
        string query = "";
        int id = Common.GetQueryStringValue("id");
        query = "select * from tbl_" + m + " where " + m + "_" + m + "id=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return;
        
        query = "delete from tbl_" + m + " where " + m + "_" + m + "id=" + id + " AND " + m + "_clientid=" + clientId;

	if (m == "leadsheet")
        {
            query = "delete from tbl_" + m + " where " + m + "_" + m + "id=" + id;
        }
        DbTable.ExecuteQuery(query);
        
        HttpContext.Current.Response.Write("Ok");
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}