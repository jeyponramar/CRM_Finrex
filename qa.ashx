<%@ WebHandler Language="C#" Class="qa" %>

using System;
using System.Web;
using WebComponent;
using System.Data;
using System.Data.SqlClient;

public class qa : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    private string InsertSpecialChars(string data)
    {
        data = data.Replace("SC__AND", "&");
        return data;
    }
    public void ProcessRequest(HttpContext context)
    {

        Common.ValidateAjaxRequest();

        context.Response.ContentType = "text/plain";
        string m = context.Request.QueryString["m"].ToString().ToLower();
        string name = context.Request.QueryString["cn"].ToString();
        string val = InsertSpecialChars(context.Request.QueryString["v"].ToString());
        string evals = Convert.ToString(context.Request.QueryString["ev"]);
        string ecols = Convert.ToString(context.Request.QueryString["ec"]);
        string tableName = "tbl_" + m;
        string columnName = m + "_" + name;

        if (m == "expensetype")
        {
            int ledgerId = Accounts.SaveLedger(val, LedgerGroup.IndirectExpense, "", "expensetype", LedgerType.Expense);
            if (ledgerId == 0)
            {
                return;
            }
            else if (ledgerId == -1)
            {
                context.Response.Write("-1");
                return;
            }

        }
        
        string query = "select * from " + tableName + " where " + columnName + "='" + global.CheckData(val) + "'";
        InsertUpdate objIsExists = new InsertUpdate();
        DataRow drExists = objIsExists.ExecuteSelectRow(query);
        if (drExists != null)
        {
            context.Response.Write("-1");
            return;
        }
        InsertUpdate obj = new InsertUpdate();
        System.Collections.Hashtable hstbl = new System.Collections.Hashtable();
        hstbl.Add(name, global.CheckData(val));
        if (name.Contains("customername"))
        {
            hstbl.Add("billingname", global.CheckData(val));
        }
        if (ecols != "")
        {
            Array arrCols = ecols.Split(',');
            Array arrVals = evals.Split(',');
            for (int i = 0; i < arrCols.Length; i++)
            {
                hstbl.Add(Convert.ToString(arrCols.GetValue(i)), GlobalUtilities.ConvertToInt(arrVals.GetValue(i)));
            }
        }
        int id = obj.InsertData(hstbl, tableName);
        context.Response.Write(id);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}