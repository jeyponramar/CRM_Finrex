using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class viewalerts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["did"] != null)
            {
                int clientUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
                string query = "delete from tbl_liveratealert where liveratealert_liveratealertid=" + 
                            Common.GetQueryStringValue("did") + " and liveratealert_clientuserid=" + clientUserId;
                DbTable.ExecuteQuery(query);
                Response.Redirect("~/viewalerts.aspx");
            }
            BindAlerts();
        }
    }
    private void BindAlerts()
    {
        int clientUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
        string query = @"select * from tbl_liveratealert
                        join tbl_liverate on liverate_liverateid=liveratealert_liverateid
                        join tbl_currencymaster on currencymaster_currencymasterid=liveratealert_currencymasterid                        
                        join tbl_covertype on covertype_covertypeid=liveratealert_covertypeid   
                        join tbl_alertstatus on alertstatus_alertstatusid=liveratealert_alertstatusid                     
                        where liveratealert_clientuserid=" + clientUserId + " order by liveratealert_liveratealertid desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        html.Append("<tr class='grid-ui-header'>");
        html.Append("<td>Currency</td><td>Cover Type</td><td>Target</td><td>Stop Loss</td><td>Expiry Date</td><td>Email Id</td><td>Mobile No</td><td>Status</td><td>Edit</td>");
        html.Append("</tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            double liverate = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["liverate_currentrate"]);
            double target = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["liveratealert_target"]);
            double stoploss = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["liveratealert_stoploss"]);
            string emailId = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liveratealert_emailid"]);
            emailId=GetContactDetail(emailId,"contacts_emailid");
            emailId = emailId.Replace(",", "<br/>");
            string mobileNo = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liveratealert_mobileno"]);
            mobileNo=GetContactDetail(mobileNo,"contacts_mobileno");
            mobileNo = mobileNo.Replace(",", "<br/>");
            string currency = GlobalUtilities.ConvertToString(dttbl.Rows[i]["currencymaster_currency"]);
            string covertype = GlobalUtilities.ConvertToString(dttbl.Rows[i]["covertype_covertype"]);
            string status = GlobalUtilities.ConvertToString(dttbl.Rows[i]["alertstatus_status"]);
            string expiryDate = GlobalUtilities.ConvertToDate(dttbl.Rows[i]["liveratealert_expirydate"]);
            int id=GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liveratealert_liveratealertid"]);
            int statusId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liveratealert_alertstatusid"]);

            if (statusId == 1)
            {
                status = "<div style='background-color:#08f060;color:#fff;padding:5px;text-align:center;width:50px;'>"+status+"</div>";
            }
            else
            {
                status = "<div style='background-color:#c10000;color:#fff;padding:5px;text-align:center;width:50px;'>" + status + "</div>";
            }
            html.Append("<tr><td>" + currency + "</td><td>" + covertype + "</td><td>" + target.ToString().Replace(".0000", "") + "</td>" +
                        "<td>" + stoploss.ToString().Replace(".0000", "") + "</td><td>" + expiryDate + "</td><td>" + emailId + "</td><td>" + mobileNo + "</td>");
            html.Append("<td>" + status + "</td><td><table><tr><td><a href='#' class='set-alert' aid='"+id+"'>Edit</a></td><td><a href='viewalerts.aspx?did="+id+"'>Delete</a></td></tr></table></td></tr>");
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
    private string GetContactDetail(string contactIds, string column)
    {
        if (contactIds == "") return "";
        string query = "select * from tbl_contacts where contacts_contactsid in(" + contactIds + ")";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder contacts = new StringBuilder();
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string val = GlobalUtilities.ConvertToString(dttbl.Rows[i][column]);
            if (val.Trim() != "")
            {
                if (contacts.ToString() == "")
                {
                    contacts.Append(val);
                }
                else
                {
                    contacts.Append(","+val);
                }
            }
        }
        return contacts.ToString();
    }
}
