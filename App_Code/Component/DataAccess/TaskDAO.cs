using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebComponent;
using System.Data;
using System.Collections;

/// <summary>
/// Summary description for TaskDAO
/// </summary>
public enum TaskType
{
    Complaint = 1,
    AmcService = 2,
    WarrantyService = 3,
    Enquiry = 4,
    PaymentCollection = 5,
    Others = 6
}
public class TaskDAO
{
	public TaskDAO()
	{
	}
    public int UpdateTaskParentModule(int taskId, int taskTypdId)
    {
        if (taskTypdId > 4) return 0;
        string query = "select * from tbl_task WHERE task_taskid=" + taskId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        int taskType = GlobalUtilities.ConvertToInt(dr["task_tasktypeid"]);
        int statusid = GlobalUtilities.ConvertToInt(dr["task_statusid"]);
        int employeeid = GlobalUtilities.ConvertToInt(dr["task_employeeid"]);
        int mid = GlobalUtilities.ConvertToInt(dr["task_mid"]);
        int clientid = GlobalUtilities.ConvertToInt(dr["task_clientid"]);
        string module = GlobalUtilities.ConvertToString(dr["task_module"]);
        string remarks = GlobalUtilities.ConvertToString(dr["task_remarks"]);
        string assignedDate = GlobalUtilities.ConvertToDateTime(dr["task_assigneddate"]);
        string subject = GlobalUtilities.ConvertToString(dr["task_subject"]);
        string description = GlobalUtilities.ConvertToString(dr["task_description"]);

        Hashtable hstbl = new Hashtable();
        hstbl.Add("clientid", clientid);
        hstbl.Add("employeeid", employeeid);
        hstbl.Add("statusid", statusid);
        hstbl.Add("assigneddate", assignedDate);
        hstbl.Add("remarks", remarks);
        hstbl.Add("subject", subject);
        hstbl.Add("description", description);
        
        InsertUpdate obj = new InsertUpdate();
        mid = obj.UpdateData(hstbl, "tbl_" + module, mid);
        return mid;
    }
    public int CreateTask(string module, int mid, TaskType taskType)
    {
        module = module.ToLower();
        string query = "select * from tbl_task WHERE task_module='" + module + "' AND task_mid=" + mid;
        DataRow drTask = DbTable.ExecuteSelectRow(query);

        query = "select * from tbl_" + module + " WHERE " + module + "_" + module + "id=" + mid;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        string remarks = GlobalUtilities.ConvertToString(dr[module+"_remarks"]);
        string assignedDate = GlobalUtilities.ConvertToDateTime(dr[module + "_assigneddate"]);
        //string subject = GlobalUtilities.ConvertToString(dr[module + "_subject"]);
        //string description = GlobalUtilities.ConvertToString(dr[module + "_description"]);
        int clientid = GlobalUtilities.ConvertToInt(dr[module + "_clientid"]);

        int employeeid = GlobalUtilities.ConvertToInt(dr[module + "_employeeid"]);
        int statusid = 0;

        if (module == "enquiry")
        {
            int enquiryStatusId = GlobalUtilities.ConvertToInt(dr[module + "_enquirystatusid"]);
            if (enquiryStatusId == 1)
            {
                statusid = 1;
            }
            else if (enquiryStatusId == 3 || enquiryStatusId == 4 || enquiryStatusId == 5)//won
            {
                statusid = 4;//closed
            }
            else if (enquiryStatusId == 9)//hold
            {
                statusid = 2;
            }
            else if (enquiryStatusId == 2 || enquiryStatusId == 8 || enquiryStatusId == 10)
            {
                statusid = 3;//inprogress
            }
        }
        else
        {
            statusid = GlobalUtilities.ConvertToInt(dr[module + "_statusid"]);
        }
        Hashtable hstbl = new Hashtable();
        hstbl.Add("clientid", clientid);
        hstbl.Add("employeeid", employeeid);
        hstbl.Add("statusid", statusid);
        hstbl.Add("tasktypeid", Convert.ToInt32(taskType));
        hstbl.Add("assigneddate", assignedDate);
        hstbl.Add("remarks", remarks);
        hstbl.Add("description", "");
        hstbl.Add("subject", "");
        hstbl.Add("module", module);
        hstbl.Add("mid", mid);

        InsertUpdate obj = new InsertUpdate();
        int taskId = 0;
        if (drTask == null)
        {
            taskId = obj.InsertData(hstbl, "tbl_task");
        }
        else
        {
            taskId = GlobalUtilities.ConvertToInt(drTask["task_taskid"]);
            obj.UpdateData(hstbl, "tbl_task", taskId);
        }
        return taskId;
    }
}
