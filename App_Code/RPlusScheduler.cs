using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebComponent;
using System.Data;
using System.Text;

/// <summary>
/// Summary description for RPlusScheduler
/// </summary>
public static class RPlusScheduler
{
    public enum RPlusSchedulerType
    {

    }
	public static void Init()
	{
        System.Timers.Timer timer = new System.Timers.Timer();
        timer.Enabled = true;
        timer.Interval = 60000;
        timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        timer.Start();
	}

    static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        string query = "select * from tbl_scheduler where scheduler_isenabled=1";
        DataTable dttblsch = DbTable.ExecuteSelect(query);

    }
}
