using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class whatsapptrialreport : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!(Common.RoleId == 1 || Common.RoleId == 8))//Research
        {
            global.PageTitle = "Permission Denied";
            global.Message = "You do not have enough access rights to perform this operation, please contact Administrator to assign rights to you!";
            Response.Redirect("~/message/message.aspx");
        }
		//ChartEvent_START
		
		//ChartEvent_END
        if (!IsPostBack)
        {
            BindData();    
        }

        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        BindData();
    }
    private void BindData()
    {
        grid.Report();
    }
    private void BindDataOnLoad()
    {
        BindData();
    }
    
}