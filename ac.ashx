<%@ WebHandler Language="C#" Class="ac" %>

using System;
using System.Web;
using System.Data;
using System.Text;
using WebComponent;

public class ac : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    private bool IsValidEmailId(string emailAddress)
    {
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        System.Text.RegularExpressions.Match match = regex.Match(emailAddress);
        return match.Success;
    }
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        Common.ValidateAjaxRequest();
        string q = context.Request.QueryString["q"].Replace(",","");
        q = global.CheckData(q);
        string m = context.Request.QueryString["m"];
        string cm = context.Request.QueryString["cm"];
        string cn = context.Request.QueryString["cn"];
        string ec = context.Request.QueryString["ec"];
        string ev = context.Request.QueryString["ev"];
        string w = GlobalUtilities.ConvertToString(context.Request.QueryString["w"]);
        StringBuilder data = new StringBuilder();
        string tableName = "tbl_" + m.ToLower();
        int index = tableName.IndexOf('_');
        string prefix = tableName.Substring(index + 1);
        //if (Common.UserId == 0)
        //{
        //    //for client user
        //    Array arrm = "state,area".Split(',');
        //    if (!GlobalUtilities.IsDataExistsInArray(arrm, m))
        //    {
        //        return;
        //    }
        //}
        if (m == "client" || cm.StartsWith("client_"))
        {
            cn = "client_customername";
        }
        string columnName = cn;
        if (!columnName.Contains("_"))
        {
            columnName = prefix + "_" + columnName;
        }
        string extraWhere = "";
        if (ec != "" && ec != null)
        {
            Array arrEc = ec.Split(',');
            Array arrEv = ev.Split(',');
            if (ev == "")
            {
                context.Response.Write("");
                context.Response.End();
            }
            for (int i = 0; i < arrEc.Length; i++)
            {
                extraWhere += " AND " + prefix + "_" + arrEc.GetValue(i).ToString() + "='" + arrEv.GetValue(i).ToString() + "'";
            }
        }
        if (w != "")
        {
            extraWhere = extraWhere + " AND " + w;
        }
        if (m == "fimimportorder")
        {
            int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
            extraWhere = " and fimimportorder_clientid=" + clientId;
        }
        //string query = "SELECT TOP 20 * FROM tbl_" + m + " WHERE " + columnName + " LIKE '%" + q + "%'";
        string query = "SELECT TOP 20 * FROM tbl_" + m + " WHERE " + columnName + " LIKE '" + q + "%'" + extraWhere;
        if (cm == "contacts")
        {
            Array arr = q.Split(';');
            for (int i = 0; i < arr.Length; i++)
            {
                string EmailId = arr.GetValue(i).ToString();
                bool isValid = IsValidEmailId(EmailId);
                if (!isValid)
                {
                    q = arr.GetValue(i).ToString();
                    break;
                }
            }
            query = "select top 20 * from tbl_contacts " +
                    "left join tbl_client on client_clientid=contacts_clientid" +
                    " where contacts_name like '%" + q + "%' or client_clientname like '%" + q + "%' " +
                        "or contacts_emailid like '%" + q + "%' or contacts_secondaryemailid like '" + q + "'";
        }
        else if (cm == "taxproduct")
        {
            query = "SELECT TOP 20 cast(product_productid as varchar)+'-'+cast(ISNULL(product_price,0) as varchar)+'-0-0' as product_productid,product_productname FROM tbl_product where product_productname LIKE '%" + q + "%' " +
                    "UNION SELECT cast(tax_taxid as varchar)+'-'+cast(ISNULL(tax_percentage,0) as varchar)+'-1-'+cast(tax_taxtypeid as varchar) as product_productid,tax_tax as productname FROM tbl_tax WHERE tax_tax LIKE '%" + q + "%'";
            columnName = "product_productname";
        }
        else if (m == "serialno" || cm == "serialno")
        {
            //query = "SELECT TOP 10 amcdetail_amcid as id,amcdetail_serialno as serialno FROM tbl_amcdetail WHERE ISNULL(amcdetail_serialno,'')<>'' AND amcdetail_serialno LIKE '%"+q+"%' " +
            //        "UNION SELECT TOP 10 salesdetail_salesid as id,salesdetail_serialno as serialno FROM tbl_salesdetail WHERE ISNULL(salesdetail_serialno,'')<>'' AND salesdetail_serialno LIKE '%" + q + "%' ";
            //columnName = "serialno";
            query = "SELECT TOP 20 MIN(serialno_serialnoid) AS serialno_serialnoid,serialno_serialno FROM tbl_serialno " +
                    "WHERE serialno_serialno LIKE '%" + q + "%' group by serialno_serialno";
            columnName = "serialno_serialno";
        }
        else if (cm == "marketingexecutive")
        {
            query = "SELECT TOP 20 employee_employeeid,employee_employeename FROM tbl_employee WHERE employee_employeetypeid=2 " +
                    "AND employee_employeename LIKE '" + q + "%' ";
            columnName = "employee_employeename";
        }
        else if (cm == "telecaller" || m == "telecaller")
        {
            query = "SELECT TOP 20 employee_employeeid,employee_employeename FROM tbl_employee WHERE employee_employeetypeid=1 " +
                    "AND employee_employeename LIKE '" + q + "%' ";
            columnName = "employee_employeename";
        }
        else if (cm == "serviceengineer")
        {
            query = "SELECT TOP 20 employee_employeeid,employee_employeename FROM tbl_employee WHERE employee_employeetypeid=3 " +
                    "AND employee_employeename LIKE '" + q + "%' ";
            columnName = "employee_employeename";
        }
        else if (m == "tasktype")
        {
            query = "SELECT TOP 20 tasktype_tasktypeid,tasktype_tasktype FROM tbl_tasktype WHERE tasktype_tasktypeid NOT IN(1,2,3,4) AND tasktype_tasktype LIKE '" + q + "%' ";
            columnName = "tasktype_tasktype";
        }
        else if (m == "amc")
        {
            query = "SELECT TOP 20 amc_amcid, " +
                    "amc_amccode + ' ('+dbo.fn_ConvertToDate(amc_startdate)+' to ' + dbo.fn_ConvertToDate(amc_enddate)+')'  " +
                    "+ CASE WHEN ISNULL(amc_iswarranty,0)=1 THEN ' WARRANTY' ELSE '' END AS amc_amccode " +
                    "FROM tbl_amc " +
                    "WHERE amc_amccode LIKE '" + q + "%' ";
            if (ev != "")
            {
                query += " AND amc_clientid=" + ev;
            }
            columnName = "amc_amccode";
        }
        else if (cm == "purchaseproduct")
        {
            query = "SELECT TOP 20 cast(product_productid as varchar)+'-'+cast(ISNULL(product_price,0) as varchar)+'-0-0' as product_productid,product_productname " +
                    "FROM tbl_product JOIN tbl_purchaseorderdetail ON purchaseorderdetail_productid=product_productid " +
                    "WHERE ISNULL(purchaseorderdetail_istax,0) = 0 AND purchaseorderdetail_purchaseorderid=" +ev+
                    " AND product_productname LIKE '%" + q + "%'";
            columnName = "product_productname";
        }
        else if (m == "bank")
        {
            int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
            query += " AND bank_clientid=" + clientId;
        }
        else if (m == "month")
        {
            q = "select month_monthid,month_month from tbl_month order by month_monthid";
        }
        else if (cm == "client_byrole")
        {
            Enum_Role role = (Enum_Role)Common.RoleId;
            if (role == Enum_Role.Administrator || role == Enum_Role.Research)
            {
                query = @"select top 100 client_clientid,client_customername from tbl_client 
                      where client_customername LIKE '%" + q + "%'";
            }
            else
            {
                query = @"select top 100 client_clientid,client_customername from tbl_client 
                      where client_employeeid=" + Common.EmployeeId + " and client_customername LIKE '%" + q + "%'";
            }
        }
        if (m == "state")
        {
            query += " and state_isactive=1";
        }
        InsertUpdate obj = new InsertUpdate();
        DataTable dttbl = obj.ExecuteSelect(query);
        if (global.IsValidTable(dttbl))
        {
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                if (cm == "contacts")
                {
                    string id = Convert.ToString(dttbl.Rows[i]["contacts_contactsid"]);
                    string fileId = id;
                    if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/images/user/thumb/" + id + ".jpg")))
                    {
                        fileId = "default";
                    }
                    data.Append(Convert.ToString(dttbl.Rows[i]["contacts_name"]) + "#" +
                        Convert.ToString(dttbl.Rows[i]["contacts_emailid"]) +"#" +
                        Convert.ToString(dttbl.Rows[i]["client_clientname"]) + "#" + fileId +
                        "|" + Convert.ToString(dttbl.Rows[i]["contacts_contactsid"]) + "\n");
                }
                else
                {
                    data.Append(Convert.ToString(dttbl.Rows[i][columnName]) + "|" + Convert.ToString(dttbl.Rows[i][0]) + "\n");
                }
            }
        }
        context.Response.Write(data.ToString());
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}