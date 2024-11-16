<%@ WebHandler Language="C#" Class="data" %>

using System;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;
using WebComponent;
using System.Text;

public class data : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "application/json";
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string m = context.Request.QueryString["m"];
        string query = "";
        var results = new System.Collections.Generic.List<object>();
        StringBuilder json = new StringBuilder();
        bool isSetFlag = false;
        if (m == "reminder")
        {
            int userid = Common.UserId;
            int employeeId = Common.EmployeeId;
            if (userid == 0)
            {
                isSetFlag = true;
            }
            else
            {
                query = @"select followups_followupsid as id,followups_subject as subject,followups_module as module, followups_mid as mid            
                        from tbl_followups where followups_date <= getdate() and followups_isreminder=1 and followups_followupstatusid=1
                        and (followups_userid=" + userid + " OR followups_employeeid=" + employeeId + ") and isnull(followups_isremoved,0)=0";
                DataTable dttb = new DataTable();
                try
                {
                    dttb = SelectData(query);
                    if (GlobalUtilities.IsValidaTable(dttb))
                    {
                        SetJson(dttb, "reminder", json);
                    }
                }
                catch (Exception ex)
                {
                    isSetFlag = true;
                }
            }
        }
        else if (m == "reminder-count")
        {
            int userid = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId"));
            if (userid == 0)
            {
                context.Response.Write("");
            }
            else
            {
                query = @"select count(*) c from tbl_followups 
                          where followups_date <= getdate() and followups_isreminder=1 and followups_followupstatusid=1
                          and followups_userid=" + userid + " and isnull(followups_isremoved,0)=0";
                DataRow dr = DbTable.ExecuteSelectRow(query);
                context.Response.Write(Convert.ToString(dr["c"]));
                //context.Response.Write("5");
                return;
            }
        }
        
        if (json.ToString() != "")
        {
            json.Append("]");
        }
        if (isSetFlag)
        {
            context.Response.Write("-1");
        }
        else
        {
            context.Response.Write(json);
        }
    }
    private DataTable SelectData(string query)
    {
        InsertUpdate obj = new InsertUpdate();
        DataTable dttbl = obj.ExecuteSelect(query);
        return dttbl;
    }
    private void SetJson(DataTable dttbl, string module, StringBuilder json)
    {

       
            
            if (dttbl.Rows.Count > 0 )
            {
                if (json.ToString() == "")
                {
                    json.Append("[");
                }
                for (int i = 0; i < dttbl.Rows.Count; i++)
                {
                    json.Append("{\"mn\":\"" + module + "\",");
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
            }
       
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}