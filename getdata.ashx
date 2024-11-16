<%@ WebHandler Language="C#" Class="chat" %>

using System;
using System.Web;
using WebComponent;
using System.Data;
using System.Collections;
using System.Text;

public class chat : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "application/json";
        string m = context.Request.QueryString["m"];
        int id =GlobalUtilities.ConvertToInt(context.Request.QueryString["invid"]);
        if (m == "") return;
        int bid = GlobalUtilities.ConvertToInt(context.Request.QueryString["bid"]);
        int zid = GlobalUtilities.ConvertToInt(context.Request.QueryString["zid"]);
        int szid = GlobalUtilities.ConvertToInt(context.Request.QueryString["szid"]);
        DataTable dttbl = new DataTable();

        if (m == "cbranch")
        {
            //CustomerBranchDAO dao = new CustomerBranchDAO();
            //dttbl = dao.GetCustomerBranches(bid, zid, szid);
        }
        else if (m == "invoiceamount")
        {
            ReceiptDAO dao = new ReceiptDAO();
            dttbl = dao.GetInvoiceReceiptdetail(id, 0);            
        }
        
        StringBuilder json = new StringBuilder();
        json = SetJson(dttbl);
        context.Response.Write(json);
    }
    private StringBuilder SetJson(DataTable dttbl)
    {
        StringBuilder json = new StringBuilder();
        if (dttbl.Rows.Count > 0)
        {
            if (json.ToString() == "")
            {
                json.Append("[");
            }
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                json.Append("{");
                for (int j = 0; j < dttbl.Columns.Count; j++)
                {
                    string val = Convert.ToString(dttbl.Rows[i][j]).Replace("\r\n", "<br/>");
                    json.Append("\"" + dttbl.Columns[j].ColumnName + "\": \"" + val + "\"");
                    if (j != dttbl.Columns.Count - 1)
                    {
                        json.Append(",");
                    }
                }
                json.Append("}");
                if (i != dttbl.Rows.Count - 1)
                {
                    json.Append(",");
                }
            }
            if (json.ToString() != "")
            {
                json.Append("]");
            }
        }
        return json;
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}