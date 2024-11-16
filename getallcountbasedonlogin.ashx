<%@ WebHandler Language="C#" Class="getallcountbasedonlogin" %>

using System;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;
using WebComponent;

public class getallcountbasedonlogin : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "application/plain";
        CommonDAO obj = new CommonDAO();
        DataTable dttbl = new DataTable();

        int openEnquiry = GetEnquiryCountByStatus(1);
        int assignedEnq = GetEnquiryCountByStatus(8);
        int oppEnq = GetEnquiryCountByStatus(4);
        int holdEnq = GetEnquiryCountByStatus(10);
        int quoteEnq = GetEnquiryCountByStatus(9);
        int wonEnq = GetEnquiryCountByStatus(5);

        string result = openEnquiry + "," + assignedEnq + "," + oppEnq + "," + holdEnq + "," + quoteEnq + "," + wonEnq;
        
        context.Response.Write(result);
    }
    private int GetEnquiryCountByStatus(int statusId)
    {
        int employeeId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_EmployeeId"));
        int RoleId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_RoleId"));
        string query = "select count(*) as c from tbl_enquiry WHERE ";
        if (RoleId == 1)
        {
            query += " enquiry_enquirystatusid="+statusId;
        }
        else
        {
            query += " enquiry_employeeid=" + employeeId + " AND enquiry_enquirystatusid=" + statusId;
        }
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            return GlobalUtilities.ConvertToInt(dr["c"]);
        }
        return 0;
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}