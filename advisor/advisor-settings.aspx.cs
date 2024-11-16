using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class advisor_settings : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateEmailId();
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Common.UpdateSettings("Advisor_EximToEmailId", txteximemailid.Text);
        Common.UpdateSettings("Advisor_EximBccEmailId", txteximbccemailid.Text);
        Common.UpdateSettings("Advisor_FemaToEmailId", txtfemaemailid.Text);
        Common.UpdateSettings("Advisor_FemaBccEmailId", txtfemabccemailid.Text);
        lblMessage.Text = "Settings updated successfully";
        lblMessage.Visible = true;
    }
    private void PopulateEmailId()
    {
        txteximemailid.Text = Common.GetSetting("Advisor_EximToEmailId");
        txteximbccemailid.Text = Common.GetSetting("Advisor_EximBccEmailId");
        txtfemaemailid.Text = Common.GetSetting("Advisor_FemaToEmailId");
        txtfemabccemailid.Text = Common.GetSetting("Advisor_FemaBccEmailId");
    }
}
