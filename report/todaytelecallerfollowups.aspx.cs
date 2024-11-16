using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class todaytelecallerfollowups : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
		//ChartEvent_START
		
		//ChartEvent_END
        if (!IsPostBack)
        {
			//FillDropDown_START
			
			//FillDropDown_END
            //BindDataOnLoad_START
			
			//BindDataOnLoad_END
            //grid.GridQuery = getQuery();
            grid.Report();
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        BindData();
    }
    private void BindData()
    {
        grid.SearchHolderId = plSearch.ID;
        grid.Report();
    }
    private void BindDataOnLoad()
    {
        BindData();
    }
    //BindChart_START
	
	//BindChart_END
    private string getQuery()
    {
        string query="";
        query = @"select * from tbl_employee where telecaller_userid=" + GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId"));
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            int telecallerId = GlobalUtilities.ConvertToInt(dr["employee_employeeid"]);
            query = @"select * from tbl_telecalling where telecalling_employeeid="+telecallerId+@" and 
                    telecalling_telecallingid in (select followups_mid from tbl_followups 
                    where cast(followups_date as date) =cast(getdate() as date))";
        }
        return query;
    }
}