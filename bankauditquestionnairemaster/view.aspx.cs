using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class BankAuditQuestionnaireMaster_view : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblPageTitle.Text = "View Bank Audit Questionnaire Master";
            //RightPanel_START
            //RightPanel_END
        }
    }
    
}