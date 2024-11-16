<%@ WebHandler Language="C#" Class="savetask" %>

using System;
using System.Web;
using WebComponent;
using System.Collections;
using System.Data;

public class savetask : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        try
        {
            int taskId = GlobalUtilities.ConvertToInt(context.Request["taskid"]);
            int engineerId = GlobalUtilities.ConvertToInt(context.Request["engid"]);
            int taskstatusid = GlobalUtilities.ConvertToInt(context.Request["statusid"]);
            string assignDate = global.CheckData(context.Request["assigndate"], false);
            string closedDate = global.CheckData(context.Request["closeddate"], false);
            string remarks = global.CheckData(context.Request["remarks"]);

            string query = "SELECT * FROM tbl_task JOIN tbl_tasktype ON tasktype_tasktypeid=task_tasktypeid " +
                        "LEFT JOIN tbl_employee ON employee_employeeid=task_employeeid " +
                        "JOIN tbl_status ON status_statusid=task_statusid " +
                        "LEFT JOIN tbl_client ON client_clientid=task_clientid " +
                        "WHERE task_taskid=" + taskId;

            DataRow drTaskDetail = DbTable.ExecuteSelectRow(query);
            int oldEngineerId = GlobalUtilities.ConvertToInt(drTaskDetail["task_employeeid"]);
            int taskTypeId = GlobalUtilities.ConvertToInt(drTaskDetail["task_tasktypeid"]);

            InsertUpdate objUpdate = new InsertUpdate();
            Hashtable hstbl = new Hashtable();
            hstbl.Add("employeeid", engineerId);
            hstbl.Add("assigneddate", assignDate);
            hstbl.Add("remarks", remarks);
            if (taskTypeId == 4)
            {
                hstbl.Add("enquirystatusid", 10);//In progress
            }
            else
            {
                hstbl.Add("statusid", taskstatusid);
            }
            int id = objUpdate.UpdateData(hstbl, "tbl_task", taskId);

            TaskDAO task = new TaskDAO();
            task.UpdateTaskParentModule(taskId, taskstatusid);

            if (engineerId == 0)
            {
                context.Response.Write(id);
                return;
            }
            string taskModule = Convert.ToString(drTaskDetail["task_module"]).ToLower().Replace(" ", "");
            DataRow drEngineer = DbTable.GetOneRow("tbl_employee", engineerId);

            bool issent = false;
            string mobileNo = "";
            string sms = "";

            string newEngineer = Convert.ToString(drEngineer["employee_employeename"]);

            if (taskModule == "complaint")
            {
                //assign task to engineer

                mobileNo = Convert.ToString(drEngineer["employee_mobileno"]);
                sms = Common.GetFormattedSettingForEmail("ComplaintSMSFormat_Assign_Task", drTaskDetail);
                sms = sms.Replace("$employee_employeename$", newEngineer);
                issent = SMS.SendSMS(mobileNo, sms);
            }
            if (oldEngineerId > 0)
            {
                //unassign task from engineer
                if (taskModule == "complaint")
                {
                    DataRow drOldEngineer = DbTable.GetOneRow("tbl_employee", oldEngineerId);

                    string oldEngineerName = Convert.ToString(drOldEngineer["employee_employeename"]);
                    mobileNo = Convert.ToString(drOldEngineer["employee_mobileno"]);
                    sms = Common.GetFormattedSettingForEmail("ComplaintSMSFormat_UnAssign_Task", drTaskDetail);
                    sms = sms.Replace("$oldengineer_name$", oldEngineerName);
                    sms = sms.Replace("$employee_employeename$", newEngineer);
                    issent = SMS.SendSMS(mobileNo, sms);
                }
            }
            context.Response.Write(id);
        }
        catch (Exception ex)
        {
            string s = ex.Message;
            context.Response.Write("");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
