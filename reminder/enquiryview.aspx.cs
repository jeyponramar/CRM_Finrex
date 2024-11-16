using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class reminder_enquiryview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblPageTitle.Text = "View Enquiry";
            grid.EnableAddLink = false;
            if (Request.QueryString["qsv"] != null)
            {
                string qsv = GlobalUtilities.ConvertToString(Request.QueryString["qsv"]);
                int employeeId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_EmployeeId"));
                int RoleId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_RoleId"));
                if (RoleId == 1)
                {
                    displayEnquiryDetails(qsv, true, 0);
                }
                else if (employeeId > 0)
                {
                    displayEnquiryDetails(qsv, false, employeeId);
                }
            }
            grid.Module = "enquiryview";
            grid.InitGrid();
            grid.BindData();

        }
    }
    private void displayEnquiryDetails(string qsv, bool isadmin, int employeeId)
    {
        string extraWhere = "";
        if (qsv == "open")
        {
            extraWhere = " where enquiry_enquirystatusid=1";
            lblPageTitle.Text = "View Open Enquiry";
        }
        else if (qsv == "assigned")
        {
            extraWhere = " where enquiry_enquirystatusid=8";
            lblPageTitle.Text = "View Assigned Enquiry";
        }
        else if (qsv == "opportunity")
        {
            extraWhere = " where enquiry_enquirystatusid in (4)";
            lblPageTitle.Text = "View Opportunity Enquiry";
        }
        else if (qsv == "createquote")
        {
            lblPageTitle.Text = "View Quotation Created Enquiry";
            extraWhere = " where enquiry_enquirystatusid =9";
        }
        else if (qsv == "won")
        {
            lblPageTitle.Text = "View Won Enquiry ";
            extraWhere = " where enquiry_enquirystatusid =5";
        }
        else if (qsv == "cold")
        {
            lblPageTitle.Text = "View Cold Enquiry";
            //extraWhere = " where enquiry_enquirystatusid =7";
        }
        else if (qsv == "hold")
        {
            lblPageTitle.Text = "View Hold Enquiry";
            extraWhere = " where enquiry_enquirystatusid =10";
        }
        else if (qsv == "followups")
        {
            lblPageTitle.Text = "Todays Followups Enquiry";
            extraWhere = @" where enquiry_enquiryid in (select followups_mid from tbl_followups
                                    where CAST(followups_date AS DATE)<=CAST(GETDATE() AS DATE) 
                                    AND followups_module='Enquiry' 
                                    AND followups_followupstatusid=1";
        }
        if (isadmin == false)
        {
            if (qsv == "followups")
            {
                extraWhere += " AND enquiry_employeeid=" + employeeId + ")";
            }
            else
            {
                extraWhere += " AND enquiry_employeeid=" + employeeId;
            }
        }
        else
        {
            if (qsv == "followups")
            {
                extraWhere += ")";
            }
        }
        grid.ExtraWhere = extraWhere;
    }

}
