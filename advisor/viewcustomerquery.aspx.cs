using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class viewcustomerquery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblPageTitle.Text = "View Customer Query";
            //RightPanel_START
            //RightPanel_END
            int type = Common.GetQueryStringValue("t");
            if (type == 0)
            {
                Response.End();
            }
            GlobalData gblData = new GlobalData();
            string w = "";
            int roleId = Common.RoleId;
            if (roleId == 10)
            {
                w = "querytopic_querytypeid=1";
            }
            else if (roleId == 11)
            {
                w = "querytopic_querytypeid=2";
            }
            gblData.FillDropdown(ddlquerytopicid, "tbl_querytopic", "querytopic_topicname", w);
            gblData.FillDropdown(ddlstatusid, "tbl_querystatus", "querystatus_status", "");
            grid.ExtraWhere = GetWhere();
            grid.Report();
        }
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        grid.ExtraWhere = GetWhere();
        grid.Report();
    }
    private string GetWhere()
    {
        int type = Common.GetQueryStringValue("t");
        string where = "";
        if (type == 1)//all
        {
            where = "1=1";
        }
        else if (type == 2)//open
        {
            where += "customerquery_querystatusid in(1,6)";
            lblPageTitle.Text = "Open Queries";
        }
        else if (type == 3)//pending your response
        {
            where += "customerquery_querystatusid=3";
            lblPageTitle.Text = "Waiting your response";
        }
        else if (type == 4)//pending 
        {
            where += "customerquery_querystatusid in(1,3,6)";
            lblPageTitle.Text = "Pending Queries";
        }
        else if (type == 5)//closed
        {
            where += "customerquery_querystatusid=5";
            lblPageTitle.Text = "Closed Queries";
        }
        int topicId = GlobalUtilities.ConvertToInt(ddlquerytopicid.SelectedValue);
        int statusId = GlobalUtilities.ConvertToInt(ddlstatusid.SelectedValue);
        if (topicId > 0) where += " and customerquery_querytopicid=" + topicId;
        if (statusId > 0) where += " and customerquery_querystatusid=" + statusId;
        if (query_date_from.Text != "") where += " and cast(customerquery_date as date)>=cast('" + GlobalUtilities.ConvertMMDateToDD(global.CheckInputData(query_date_from.Text)) + "' as date)";
        if (query_date_to.Text != "") where += " and cast(customerquery_date as date)<=cast('" + GlobalUtilities.ConvertMMDateToDD(global.CheckInputData(query_date_to.Text)) + "' as date)";
        int roleId = Common.RoleId;
        if (roleId == 10)
        {
            where += " and querytopic_querytypeid=1";
        }
        else if (roleId == 11)
        {
            where += " and querytopic_querytypeid=2";
        }
        return where;
    }
}