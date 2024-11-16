using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WebComponent;

public partial class clientfeedback : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        if (txtfeedback.Text.Trim() == "")
        {
            lblMessage.Text = "Please enter feedback";
            return;
        }
        string toEmailId = "info@finrex.in";
        string subject = "Customer Feedback";
        string query = "";
        query = @"select * from tbl_clientuser 
                  join tbl_client on client_clientid=clientuser_clientid
                  left join tbl_contacts on contacts_contactsid=clientuser_contactsid
                  where clientuser_clientuserid=" + Common.ClientUserId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        dr.Table.Columns.Add("feedback");
        dr["feedback"] = txtfeedback.Text;
        string body = Common.GetFormattedSettingForEmail("Customer_Feedback_Email", dr, true);
        BulkEmail.SendMail_Alert(toEmailId, subject, body, "");
        lblMessage.Text = "Thanks for submitting your feedback!";
        trfeedback.Visible = false;
    }
}
