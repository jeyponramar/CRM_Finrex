using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;
using System.IO;

public partial class task_mydailytask : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFromDate.Text = GlobalUtilities.GetCurrentDateDDMMYYYY().Replace("/", "-");
            txtToDate.Text = txtFromDate.Text;

            gblData.FillDropdown(ddlStatus, "tbl_status", "status_status", "status_statusid", "status_statusid<6", "status_statusid");
            BindTask();
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
    }
    private void BindTask()
    {
        string query = "SELECT * FROM tbl_task JOIN tbl_tasktype ON tasktype_tasktypeid=task_tasktypeid " +
                       "JOIN tbl_status ON status_statusid=task_statusid " +
                       "JOIN tbl_client ON client_clientid=task_clientid " +
                       "JOIN tbl_employee ON employee_employeeid=task_employeeid ";
        string where = "";
        if (txtFromDate.Text != "")
        {
            where += " cast(task_assigneddate as date)>=cast('" + GlobalUtilities.ConvertMMDateToDD(txtFromDate.Text) + "' as date)";
        }
        if (txtToDate.Text != "")
        {
            if (where != "") where += " AND";
            where += " cast(task_assigneddate as date)<=cast('" + GlobalUtilities.ConvertMMDateToDD(txtToDate.Text) + "' as date)";
        }
        if (ddlStatus.SelectedIndex > 0)
        {
            if (where != "") where += " AND";
            where += "task_statusid=" + ddlStatus.SelectedValue;
        }
        
        int roleId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_RoleId"));
        if (roleId > 1)//if not admin
        {
            int employeeId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_EmployeeId"));
            if (where != "") where += " AND";
            where += " task_employeeid=" + employeeId;
        }
        if (where != "")
        {
            query += " WHERE " + where;
        }
        DataTable dttblTask = DbTable.ExecuteSelect(query);
        if (dttblTask.Rows.Count == 0)
        {
            ltTask.Text = "<div class='error'>No data found.</div>";
            return;
        }
        StringBuilder html = new StringBuilder();
        for (int i = 0; i < dttblTask.Rows.Count; i++)
        {
            string task = GlobalUtilities.ConvertToString(dttblTask.Rows[i]["task_description"]);
            string subject = GlobalUtilities.ConvertToString(dttblTask.Rows[i]["task_subject"]);
            string createdDate = GlobalUtilities.ConvertToDate(dttblTask.Rows[i]["task_createddate"]);
            string assignedDate = GlobalUtilities.ConvertToDate(dttblTask.Rows[i]["task_assigneddate"]);
            string taskType = GlobalUtilities.ConvertToString(dttblTask.Rows[i]["tasktype_tasktype"]);
            string clientName = GlobalUtilities.ConvertToString(dttblTask.Rows[i]["client_customername"]);
            string status = GlobalUtilities.ConvertToString(dttblTask.Rows[i]["status_status"]);
            string remarks = GlobalUtilities.ConvertToString(dttblTask.Rows[i]["task_remarks"]);
            int taskid = GlobalUtilities.ConvertToInt(dttblTask.Rows[i]["task_taskid"]);
            int statusid = GlobalUtilities.ConvertToInt(dttblTask.Rows[i]["task_statusid"]);
            string module = GlobalUtilities.ConvertToString(dttblTask.Rows[i]["task_module"]);
            string empName = GlobalUtilities.ConvertToString(dttblTask.Rows[i]["employee_employeename"]);
            int mid = GlobalUtilities.ConvertToInt(dttblTask.Rows[i]["task_mid"]);

            string bgcolor = Common.GetColorAZ(clientName);
            string url = "";
            if (module == "")
            {
                url = "task/add.aspx?id=" + taskid;
            }
            else
            {
                url = module + "/add.aspx?id=" + mid;
            }

            string circle = "<div class='circletext' style='background-color:#" + bgcolor + "'>" + clientName.Substring(0, 2) + "</div>";
            html.Append("<div class='dailytaskbox spage' href='" + url + "' style='color:#000' title='Edit Task'><table width='100%'><tr><td width='70' style='vertical-align:top;padding-top:15px;'>" + circle + "</td>" +
                        "<td class='valign'><table width='100%'><tr><td colspan='2'><table width='100%'><tr><td class='dailytaskbox-title'>" + clientName +
                                            "</td><td align='right'><div title='" + status + "' class='status-" + status.ToLower().Replace(" ", "") + "'></div></td></tr></table></td></tr>" +
                                            "<tr><td class='bold'>" + module + "</td><td class='right' width='65'>" + assignedDate + "</td></tr>" +
                                            "<tr><td>" + subject + "</td><td align='right'>" + empName + "</td></tr>" +
                                            "<tr><td colspan='2'>" + remarks + "</td></tr>" +
                                            "</table></td>" +
                        "</tr></table></div>");

        }
        ltTask.Text = html.ToString();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindTask();
    }
}