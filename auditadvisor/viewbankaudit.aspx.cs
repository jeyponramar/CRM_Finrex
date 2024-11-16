using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class auditadvisor_viewbankaudit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblPageTitle.Text = "View Audit";
            GlobalData gblData = new GlobalData();
            gblData.FillDropdown(bankaudit_bankauditstatusid, "tbl_bankauditstatus", "bankauditstatus_status", "");
            grid.SearchHolderId = "plSearch";
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
        if (type == 2)//open
        {
            where = "bankaudit_bankauditstatusid=1";
            lblPageTitle.Text = "Open Audits";
        }
        else if (type == 3)//Waiting Your Response
        {
            where = "bankaudit_bankauditstatusid=3";
            lblPageTitle.Text = "Audits Waiting Your Response";
        }
        else if (type == 4)//Pending Audits
        {
            where = "bankaudit_bankauditstatusid<>4";
            lblPageTitle.Text = "Pending Audits";
        }
        else if (type == 5)//Closed Audits
        {
            where = "bankaudit_bankauditstatusid=4";
            lblPageTitle.Text = "Closed Audits";
        }
        return where;
    }
}