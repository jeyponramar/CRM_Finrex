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

public partial class task_allocatetask : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gblData.FillDropdown(ddlTaskType, "tbl_tasktype", "tasktype_tasktype", "tasktype_tasktypeid", "", "tasktype_tasktype");
            gblData.FillDropdown(ddlEmployeeType, "tbl_employeetype", "employeetype_employeetype", "employeetype_employeetypeid", "employeetype_employeetypeid<>1", "employeetype_employeetype");
            SetStartDate();
            BindTask();
            BindOpenTask();
        }
        else
        {
            if (GlobalUtilities.ConvertToInt(hdnIsLeft.Text) > 0)
            {
                MoveDateLeftRight();
            }
        }
    }
    private void BindTask()
    {
        string date1 = GetTaskDate(0);
        string date2 = GetTaskDate(1);
        string date3 = GetTaskDate(2);
        string date4 = GetTaskDate(3);
        string date5 = GetTaskDate(4);
        string date6 = GetTaskDate(5);

        string startDate = GlobalUtilities.ConvertToSqlMinDateFormat(date1);
        string endDate = GlobalUtilities.ConvertToSqlMaxDateFormat(date6);

        date1 = GetSundayStyle(date1);
        date2 = GetSundayStyle(date2);
        date3 = GetSundayStyle(date3);
        date4 = GetSundayStyle(date4);
        date5 = GetSundayStyle(date5);
        date6 = GetSundayStyle(date6);
        
        string query = "SELECT * FROM tbl_task JOIN tbl_tasktype ON tasktype_tasktypeid=task_tasktypeid " +
                       "JOIN tbl_status ON status_statusid=task_statusid "+
                       "LEFT JOIN tbl_client ON client_clientid=task_clientid WHERE task_statusid > 1 AND task_assigneddate>='" + startDate
                        + "' AND task_assigneddate<='" + endDate + "'";
        DataTable dttblThisWeekTask = DbTable.ExecuteSelect(query);

        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%' cellpadding=2 cellspacing=0>" +
                        "<tr class='task-dates'><td class='d1'>" + date1 + "</td><td class='d2'>" + date2 + "</td><td class='d3'>" +
                        date3 + "</td><td class='d4'>" + date4 + "</td><td class='d5'>" + date5 + "</td><td class='d6'>" + date6 + "</td></tr>");

        query = "select * from tbl_employee JOIN tbl_employeetype ON employeetype_employeetypeid=employee_employeetypeid WHERE ";
        if (ddlEmployeeType.SelectedIndex > 0)
        {
            query += "employee_employeetypeid=" + ddlEmployeeType.SelectedValue;
        }
        else
        {
            query += "employee_employeetypeid<>1";
        }
        DataTable dttblEmployee = DbTable.ExecuteSelect(query);

        for (int i = 0; i < dttblEmployee.Rows.Count; i++)
        {
            int employeeId = GlobalUtilities.ConvertToInt(dttblEmployee.Rows[i]["employee_employeeid"]);
            string engineerName = GlobalUtilities.ConvertToString(dttblEmployee.Rows[i]["employee_employeename"]);
            string engineerEmail = GlobalUtilities.ConvertToString(dttblEmployee.Rows[i]["employee_emailid"]);
            string engPhoneNo = GlobalUtilities.ConvertToString(dttblEmployee.Rows[i]["employee_mobileno"]);
            string employeeType = GlobalUtilities.ConvertToString(dttblEmployee.Rows[i]["employeetype_employeetype"]);
            string task = "";
            string engPhoto = employeeId + ".jpg";
            if (!File.Exists(Server.MapPath("~/upload/employee/thumb/" + employeeId + ".jpg")))
            {
                engPhoto = "default.jpg";
            }
            html.Append("<tr><td colspan=6 style='background-color:#034c5e;color:#ffffff'><table><tr><td><img class='imguser' src='../upload/employee/thumb/" + engPhoto + "' " +
                        "title='" + engineerName + " (" + engPhoneNo + ")'/></td>" +
                        "<td><b>" + engineerName + "</b> (" + employeeType +" " + engPhoneNo + ", " + engineerEmail + ") "+"</td>" +
                        "</tr></table>" +
                    "</td></tr>");
            html.Append("<tr class='maintr' eid='" + employeeId + "' nm='" + engineerName + "'>");

            for (int j = 0; j < 6; j++)
            {
                html.Append("<td valign='top' class='task-box-outer' colid='" + (j + 1) + "'>" + GetTaskByDateAndEng(dttblThisWeekTask, employeeId, j) + "</td>");
            }
            html.Append("</tr>");
           
        }

        html.Append("</table>");
        ltTaskByDate.Text = html.ToString();
    }
    private string GetTaskByDateAndEng(DataTable dttblThisWeekTask, int employeeId, int index)
    {
        index = index + CurrentDayIndex;
        DateTime dt = GetStartDate();
        dt = dt.AddDays(index);
        string date = "";
        date = GlobalUtilities.ConvertToDate(dt);
        string html = "";
        StringBuilder htmlTask = new StringBuilder();

        for (int i = 0; i < dttblThisWeekTask.Rows.Count; i++)
        {
            string taskAssignedDate = GlobalUtilities.ConvertToDate(dttblThisWeekTask.Rows[i]["task_assigneddate"]);
            int assignedToEng = GlobalUtilities.ConvertToInt(dttblThisWeekTask.Rows[i]["task_employeeid"]);
            if (taskAssignedDate == date && assignedToEng == employeeId)
            {
                string task = GlobalUtilities.ConvertToString(dttblThisWeekTask.Rows[i]["task_description"]);
                string subject = GlobalUtilities.ConvertToString(dttblThisWeekTask.Rows[i]["task_subject"]);
                string createdDate = GlobalUtilities.ConvertToDate(dttblThisWeekTask.Rows[i]["task_createddate"]);
                string taskType = GlobalUtilities.ConvertToString(dttblThisWeekTask.Rows[i]["tasktype_tasktype"]);
                string clientName = GlobalUtilities.ConvertToString(dttblThisWeekTask.Rows[i]["client_customername"]);
                string status = GlobalUtilities.ConvertToString(dttblThisWeekTask.Rows[i]["status_status"]);
                string remarks = GlobalUtilities.ConvertToString(dttblThisWeekTask.Rows[i]["task_remarks"]);
                int taskid = GlobalUtilities.ConvertToInt(dttblThisWeekTask.Rows[i]["task_taskid"]);
                int statusid = GlobalUtilities.ConvertToInt(dttblThisWeekTask.Rows[i]["task_statusid"]);
                string module = GlobalUtilities.ConvertToString(dttblThisWeekTask.Rows[i]["task_module"]);
                int mid = GlobalUtilities.ConvertToInt(dttblThisWeekTask.Rows[i]["task_mid"]);
                string statusIcon = "";
                if (statusid == 4)
                {
                    statusIcon = "<div class='right' style='margin-top:-10px;'><img src='../images/tick.png' title='Closed'/></div>";
                }
                htmlTask.Append("<tr class='trtask' tid='" + taskid + "' ttype='" + taskType + "' ts='" + statusid + "' m='"+module+"' mid='"+mid+"'>" +
                                    "<td class='task-" + taskType.Replace(" ","").ToLower() + " taskbox'>" +
                                    "<table class='taskbox-task task-" + taskType.ToLower().Replace(" ", "") + "' width='100%' title='" + remarks + "'>" +
                                        "<tr><td colspan='2'><div class='left'>" + clientName + "</div>" + statusIcon + "</td></tr>" +
                                        "<tr><td class='bold'>" + taskType + "</td><td class='right'>" + createdDate + "</td></tr>" +
                                        "<tr><td colspan='2'>" + subject + "</td></tr>" +
                                    "</table>" +
                                "</td></tr>");
            }
        }
        if (htmlTask.ToString() == "")
        {
            html = "<table width='100%' class='tbltask'><tr class='trnotask trtask'><td>No Task</td></tr></table>";
        }
        else
        {
            html = "<table width='100%' class='tbltask'>" + htmlTask.ToString() + "</table>";
        }
        return html;
    }
    private string GetTaskDate(int index)
    {
        index = index + CurrentDayIndex;
        DateTime dt = GetStartDate();
        dt = dt.AddDays(index);
        string date = "";
        date = GlobalUtilities.ConvertToDate(dt);

        return date;
    }
    private string GetSundayStyle(string date)
    {
        Array arr = date.Split('-');
        DateTime dt = new DateTime(GlobalUtilities.ConvertToInt(arr.GetValue(2)), GlobalUtilities.ConvertToInt(arr.GetValue(1)), GlobalUtilities.ConvertToInt(arr.GetValue(0)));
        if (dt.DayOfWeek == DayOfWeek.Sunday)
        {
            date = "<div style='background-color:#ff0000;color:#ffffff;'>" + date + "</div>";
        }
        return date;
    }
    private DateTime GetStartDate()
    {
        string startDate = GlobalUtilities.ConvertToString(ViewState["StartDate"]);
        Array arr = startDate.Split('-');
        DateTime dt = new DateTime(GlobalUtilities.ConvertToInt(arr.GetValue(2)), GlobalUtilities.ConvertToInt(arr.GetValue(1)), GlobalUtilities.ConvertToInt(arr.GetValue(0)));
        return dt;
    }
    private int CurrentDayIndex
    {
        get
        {
            if (ViewState["CurrentDayIndex"] == null) return 0;
            return Convert.ToInt32(ViewState["CurrentDayIndex"]);
        }
        set
        {
            ViewState["CurrentDayIndex"] = value;
        }
    }
    private void SetStartDate()
    {
        ViewState["StartDate"] = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
    }
    protected void ddlTaskType_Changed(object sender, EventArgs e)
    {
        int taskType = Convert.ToInt32(ddlTaskType.SelectedValue);
        if (taskType == 1 || taskType == 2 || taskType == 3)
        {
            ddlEmployeeType.SelectedValue = "3";
        }
        else if (taskType == 4)
        {
            ddlEmployeeType.SelectedValue = "2";
        }
        else
        {
            ddlEmployeeType.SelectedValue = "0";
        }
        BindOpenTask();
        BindTask();
    }
    private void BindOpenTask()
    {
        string query = "SELECT * FROM tbl_task JOIN tbl_tasktype ON tasktype_tasktypeid=task_tasktypeid "+
                       "LEFT JOIN tbl_client ON client_clientid=task_clientid WHERE task_statusid = 1";
        if (ddlTaskType.SelectedIndex > 0)
        {
            query += " AND task_tasktypeid=" + ddlTaskType.SelectedValue;
        }
        query += " ORDER by task_taskid";
        DataTable dttblTask = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table cellpadding=0 cellspacing=0 width='100%'>");
        for (int i = 0; i < dttblTask.Rows.Count; i++)
        {
            string task = GlobalUtilities.ConvertToString(dttblTask.Rows[i]["task_description"]);
            string subject = GlobalUtilities.ConvertToString(dttblTask.Rows[i]["task_subject"]);
            string createdDate = GlobalUtilities.ConvertToDate(dttblTask.Rows[i]["task_createddate"]);
            string taskType = GlobalUtilities.ConvertToString(dttblTask.Rows[i]["tasktype_tasktype"]);
            string clientName = GlobalUtilities.ConvertToString(dttblTask.Rows[i]["client_customername"]);
            string strCss = "";
            int taskid = GlobalUtilities.ConvertToInt(dttblTask.Rows[i]["task_taskid"]);
            string date = GlobalUtilities.ConvertToDate(dttblTask.Rows[i]["task_createddate"]);
            int statusid = GlobalUtilities.ConvertToInt(dttblTask.Rows[i]["task_statusid"]);
            string module = GlobalUtilities.ConvertToString(dttblTask.Rows[i]["task_module"]);
            int mid = GlobalUtilities.ConvertToInt(dttblTask.Rows[i]["task_mid"]);

            html.Append("<tr tid='" + taskid + "' class='trtask' ts='" + statusid + "' ttype='" + taskType + "' m='" + module + "' mid='" + mid + "'><td style='padding-bottom:5px;'>" +
                            "<table class='task-box-loc' width='100%'>" +
                                "<tr><td class='task-sub-box-loc'>" +
                                    "<table class='taskbox-task task-" + taskType.ToLower().Replace(" ","") + strCss + "' width='100%'>" +
                                        "<tr><td colspan='2'>"+clientName+"</td></tr>"+
                                        "<tr><td class='bold'>" + taskType + "</td><td class='right'>" + createdDate + "</td></tr>" +
                                        "<tr><td colspan='2'>" + subject + "</td></tr>" +
                                    "</table>" +
                                "</td></tr>" +
                            "</table>" +
                        "</td></tr>");
        }
        html.Append("</table>");
        ltOpenTask.Text = html.ToString();
    }
    private void MoveDateLeftRight()
    {
        int left = GlobalUtilities.ConvertToInt(hdnIsLeft.Text);
        if (left == 1)
        {
            CurrentDayIndex = CurrentDayIndex - 1;
        }
        else
        {
            CurrentDayIndex = CurrentDayIndex + 1;
        }
        BindTask();
    }
    protected void ddlEmployeeType_Changed(object sender, EventArgs e)
    {
        BindTask();
    }
}
