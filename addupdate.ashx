<%@ WebHandler Language="C#" Class="add" %>

using System;
using System.Web;
using System.Collections;
using System.Data;
using WebComponent;

public class add : IHttpHandler,System.Web.SessionState.IRequiresSessionState  {
    public void ProcessRequest(HttpContext context)
    {
        if (CustomSession.Session("Login_UserId") == null)
        {
            context.Response.Write("session expired");
            return;
        }
        context.Response.ContentType = "text/plain";
        string m = context.Request.QueryString["m"].Trim().ToLower();
        int oldid = 0;
        int noofnotemoduleColumns = 0;
        string tableName = "";
        if (m.StartsWith("tbl_"))
        {
            tableName = m;
        }
        else
        {
            tableName = "tbl_" + m;
        }
        string module = m.Replace("tbl_", "");
        Hashtable hstbl = new Hashtable();
        
        for (int i = 0; i < HttpContext.Current.Request.Form.Keys.Count; i++)
        {
            string key = HttpContext.Current.Request.Form.Keys[i].ToLower();
            string colname = "";
            if (key.Contains("$"))
            {
                colname = key.Substring(key.LastIndexOf("$") + 1);
                if (colname.StartsWith("txt") || colname.StartsWith("hdn") || colname.StartsWith("ddl") ||
                    colname.StartsWith("chk"))
                {
                    if (colname.StartsWith("hdn") && colname == "hdn" + tableName.Replace("tbl_", "") + "id")
                    {
                        //get identity value
                        oldid = Convert.ToInt32(HttpContext.Current.Request.Form[key]);
                        colname = "_";//set _ to ignore for add
                    }
                    else
                    {
                        colname = colname.Substring(3);
                    }
                }
                else
                {
                    colname = "";
                }
            }
            else
            {
                //colname = key.Replace("txt", "").Replace("hdn", "").Replace("ddl", "").Replace("chk", "");
                if (key.StartsWith("txt") || key.StartsWith("hdn") || key.StartsWith("ddl") ||
                    key.StartsWith("chk"))
                {
                    if (key.StartsWith("hdn") && key == "hdn" + tableName.Replace("tbl_", "") + "id")
                    {
                        //get identity value
                        oldid = Convert.ToInt32(HttpContext.Current.Request.Form[key]);
                        colname = "_";//set _ to ignore for add
                    }
                    else
                    {
                        colname = key.Substring(3);
                        if (colname == "note" || colname == "priorityid")
                        {
                            noofnotemoduleColumns++;
                        }
                    }
                }
            }
            if (!colname.Contains("_") && colname != "")
            {
                string val = HttpContext.Current.Request.Form[key];
                if (colname.StartsWith("is") && val == "on") val = "1";
                hstbl.Add(colname, val);
                if (noofnotemoduleColumns == 2) break;
            }
        }
        //get non checked checkbox columns
        string nc = context.Request.QueryString["nc"];
        if (nc != null && nc!="")
        {
            Array arr = nc.Split(',');
            for (int i = 0; i < arr.Length; i++)
            {
                string controlName = Convert.ToString(arr.GetValue(i)).ToLower();
                controlName = controlName.Substring(controlName.IndexOf("chk") + 3);
                if (!hstbl.Contains(controlName))
                {
                    hstbl.Add(controlName, 0);
                }
            }
        }
        InsertUpdate obj = new InsertUpdate();
        int id = 0;
        string query = "";
        if (oldid == 0)
        {
            if (m == "columns")
            {
                int puid = GlobalUtilities.ConvertToInt(context.Request.QueryString["puid"]);
                int sequence = 1;
                if (puid > 0)
                {
                    InsertUpdate objs = new InsertUpdate();
                    query = "select columns_sequence from tbl_columns where columns_columnsid=" + puid;
                    DataRow drs = objs.ExecuteSelectRow(query);
                    if (drs != null)
                    {
                        sequence = GlobalUtilities.ConvertToInt(drs["columns_sequence"]) + 1;
                    }
                    hstbl.Add("sequence", sequence);
                }
                //increment all seqnences
                InsertUpdate objinc = new InsertUpdate();
                int isleft = GlobalUtilities.ConvertToInt(hstbl["isleftcolumn"]);
                int mid = GlobalUtilities.ConvertToInt(hstbl["moduleid"]);
                query = "update tbl_columns set columns_sequence=columns_sequence+1 where columns_moduleid=" + mid + " and columns_isleftcolumn=" + isleft +
                         " and columns_sequence>=" + sequence;
                objinc.ExecuteQuery(query);
            }
            id = obj.InsertData(hstbl, tableName);
        }
        else
        {
            string idcolname = tableName.Replace("tbl_", "");
            idcolname = idcolname + "_" + idcolname + "id";
            
            id = obj.UpdateData(hstbl, tableName, idcolname, oldid);
        }
        context.Response.Write(id.ToString());
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}