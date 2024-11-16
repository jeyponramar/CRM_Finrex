using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using WebComponent;

public partial class communicationsources_Calllogreport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SettingsDAO daos = new SettingsDAO();
        lblCompanyName.Text = CustomSettings.CompanyName;
        lblCompanyAddress.Text = Common.GetSetting("CompanyAddress");
        lblemailid.Text = Common.GetSetting("Company EmailId");
        lblcphoneno.Text = Common.GetSetting("Company Phone No");
        lblFaxNo.Text = Common.GetSetting("Company MobileNo");
        lblwebsite.Text = Common.GetSetting("Company Website");

        int clientid = GlobalUtilities.ConvertToInt(Request.QueryString["cid"]);
        string strFromDate = GlobalUtilities.ConvertMMDateToDD(Request.QueryString["sd"]).Replace('-', '/');
        string strToDate = GlobalUtilities.ConvertMMDateToDD(Request.QueryString["ed"]).Replace('-', '/');

        string query = @"SELECT * FROM tbl_calllog
                    JOIN tbl_client ON calllog_clientid=client_clientid
                    JOIN tbl_notificationtype ON calllog_notificationtypeid=notificationtype_notificationtypeid
                    JOIN tbl_emailsmssentstatus ON calllog_emailsmssentstatusid=emailsmssentstatus_emailsmssentstatusid
                    JOIN tbl_employee ON calllog_employeeid=employee_employeeid 
                    where calllog_emailsmssentstatusid=2 AND calllog_clientid =" + clientid + " AND cast(calllog_sentdate as date) BETWEEN '" + strFromDate + "' AND '" + strToDate + "'";
        InsertUpdate obj = new InsertUpdate();
        DataTable dttbl = DbTable.ExecuteSelect(query);


        StringBuilder html = new StringBuilder();
        int srNo = 1;
        html.Append("<tr><td style='border-top:1px solid black;border-right:1px solid black;border-left:1px solid black;'><table width='100%' cellpadding='0' cellspacing='0'>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            DataRow dr = dttbl.Rows[i];
            html.Append("<tr><td style='border-bottom:1px solid black;'><table width='100%' cellpadding='5' cellspacing='0'>");
            string clientName = GlobalUtilities.ConvertToString(dr["client_customername"]);
            string message = GlobalUtilities.ConvertToString(dr["calllog_message"]);
            string emailid = GlobalUtilities.ConvertToString(dr["calllog_emailid"]);
            string notificationtype = GlobalUtilities.ConvertToString(dr["notificationtype_notificationtype"]);
            string subject = GlobalUtilities.ConvertToString(dr["calllog_subject"]);
            string SendBY = GlobalUtilities.ConvertToString(dr["employee_employeename"]);
            string senddate = GlobalUtilities.ConvertToDate(dr["calllog_sentdate"]);
            string status = GlobalUtilities.ConvertToString(dr["emailsmssentstatus_status"]);

            string strSrNo = "&nbsp;";
            html.Append("<tr>");
               html.Append("<td><b>Date :</b></td><td style='text-align:left'>"+senddate+"</td>");
               html.Append("<td style='text-align:right;'><b>Communicated Through :</b></td><td style='text-align:left'>"+ notificationtype+"</td>");
            html.Append("</tr>");
            html.Append("<tr>");
               html.Append("<td><b>Send To :</b></td><td style='text-align:left'>"+ clientName+"</td>");
               html.Append("<td style='text-align:right;'><b>Send By : </b></td><td style='text-align:left'>"+ SendBY+"</td>");
            html.Append("</tr>");
            html.Append("<tr>");
               html.Append("<td colspan='3'><b>Message :</b></td>");
            html.Append("</tr>");
            html.Append("<tr>");
               html.Append("<td colspan='3'>"+ message+"</td>");
            html.Append("</tr>");

            
            html.Append("</table></td></tr>");
        }
        html.Append("</table></td></tr>");

        ltcalllogdetail.Text = html.ToString();
    }

    private string GetCustomizeHeightOnFormat()
    {
        string height = Common.GetSetting("QuotationFormatHeight");
        StringBuilder html = new StringBuilder();
        html.Append("<tr h='" + height + "' class='blankheight'></tr>");
        return html.ToString();
    }
    
}
