using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;
using System.IO;

public partial class usercontrols_Dashboard_MyEnquiryFollowups : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int telecalllerId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_EmployeeId"));
            int mkpersonid = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_EmployeeId"));
            int RoleId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_RoleId"));

            if (RoleId == 1)
            {
                BindEnquiryFollowups(true, 0);
            }
            else if (mkpersonid > 0)
            {
                BindEnquiryFollowups(false, mkpersonid);
            }
        }
    }
    private void BindEnquiryFollowups(bool isadmin, int mkpersonId)
    {
        string query = "";
        if (isadmin)
        {
            query = @"SELECT top 10 * FROM tbl_followups
                        JOIN tbl_client ON client_clientid= followups_clientid
                        join tbl_enquiry on enquiry_enquiryid=followups_mid
                        left join tbl_employee on employee_employeeid=followups_employeeid
                        WHERE CAST(followups_date AS DATE)<=CAST(GETDATE() AS DATE) 
                        AND followups_module='Enquiry' AND followups_followupstatusid=1";
        }
        else
        {
            query = @"SELECT top 10 * FROM tbl_followups
                        JOIN tbl_client ON client_clientid= followups_clientid
                        join tbl_enquiry on enquiry_enquiryid=followups_mid
                        left join tbl_employee on employee_employeeid=followups_employeeid
                        WHERE CAST(followups_date AS DATE)<=CAST(GETDATE() AS DATE) 
                        AND followups_employeeid=" + mkpersonId +
                    @" AND followups_module='Enquiry' AND followups_followupstatusid=1";
        }
        DataTable dttb = DbTable.ExecuteSelect(query);
        if (GlobalUtilities.IsValidaTable(dttb))
        {
            StringBuilder html = new StringBuilder();
            html.Append("<table width='100%'");
            html.Append("<tr><td colspan='3' style='text-align:right;'><a href='#reminder/enquiryview.aspx?qsv=followups' class='spage' title='Enquiy Followups'>View more..</a></td></tr>");
            for (int i = 0; i < dttb.Rows.Count; i++)
            {
                string customerName = GlobalUtilities.ConvertToString(dttb.Rows[i]["client_customername"]);
                string followupsDate = GlobalUtilities.ConvertToDate(dttb.Rows[i]["followups_date"]);
                int customerId = GlobalUtilities.ConvertToInt(dttb.Rows[i]["client_clientid"]);
                string MkPersonName = GlobalUtilities.ConvertToString(dttb.Rows[i]["employee_employeename"]);
                string enquiryNo = GlobalUtilities.ConvertToString(dttb.Rows[i]["enquiry_enquiryno"]);
                string contactperson = GlobalUtilities.ConvertToString(dttb.Rows[i]["enquiry_contactperson"]);
                string mobileno = GlobalUtilities.ConvertToString(dttb.Rows[i]["enquiry_mobileno"]);
                if (contactperson == "") contactperson = "&nbsp;";
                if (mobileno == "") mobileno = "&nbsp;";
                int enquiryId = GlobalUtilities.ConvertToInt(dttb.Rows[i]["enquiry_enquiryid"]);
                string rowcss = "dashboard-alt";
                string imgLogoName = customerId.ToString();
                if (!File.Exists(Server.MapPath("~/upload/client-logo/" + customerId + ".jpg")))
                {
                    imgLogoName = "default";
                }
                if (i % 2 == 0) rowcss = "dashboard-row";
                html.Append("<tr class='" + rowcss + "'>");

                //html.Append("<td valign='middle' width='40'><div><img src='../upload/client-logo/" + imgLogoName + ".jpg' width='40' class='circle'/></div></td>" +
                html.Append("<td><table width='100%'><tr><td class='bold' width='40%'>" + customerName + "</td>");
                html.Append("<td align='left' width='20%'>" + contactperson + "</td>");

                html.Append("<td align='right' width='40%'><a href='#enquiry/add.aspx?id=" + enquiryId + "' title='Edit Enquiry' class='spage'>" + enquiryNo + "</a></td>");
                html.Append("</tr>");
                html.Append("<tr><td>" + followupsDate + "</td>");
                html.Append("<td align='left'>" + mobileno + "</td>");
                if (isadmin)
                    html.Append("<td align='right'>" + MkPersonName + "</td>");
                else
                {
                    html.Append("<td>&nbsp;</td>");
                }
                html.Append("</tr>");
                html.Append("</table></td>");
                html.Append("</td></tr>");
            }
            html.Append("</table>");
            ltmyenquiryfollowups.Text = html.ToString();
        }
        else
        {
            ltmyenquiryfollowups.Text = "<htm><body><p class='error'>No Followups Found</p></body></html>";
        }
    }
}
