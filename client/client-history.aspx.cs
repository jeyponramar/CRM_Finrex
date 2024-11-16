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
using System.Text;

public partial class client_client_history : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDetail();
        }
    }
    private void BindDetail()
    {
        if (Request.QueryString["id"] == null) return;
        int id = Common.GetId();
        //get first amc detail
        string query = "select top 1 * from tbl_client where client_clientid=" + id ;
        DataRow drclientdetail = DbTable.ExecuteSelectRow(query);
        if (drclientdetail == null) return;

        lblClientName.Text = GlobalUtilities.ConvertToString(drclientdetail["client_customername"]);
        lblAmcDate.Text = GlobalUtilities.ConvertToDate(drclientdetail["client_createddate"]);
        ArrayList arr = Common.GetClientContactDetail(id);
        lblContactPersons.Text = arr[0].ToString();
        lblLandlineNos.Text = arr[1].ToString();
        lblModileNos.Text = arr[2].ToString();
        lblEmails.Text = arr[3].ToString();
        lblSiteLocation.Text = GlobalUtilities.ConvertToString(drclientdetail["client_address"]);


        BindEnquiry(id);
        BindWorkReport(id);
    }
    private void BindEnquiry(int id)
    {
        string query = "select *,(select COUNT(*) from tbl_followups "+
                        "join tbl_enquiry on enquiry_enquiryid=followups_mid "+
                        "WHERE followups_mid=t.enquiry_enquiryid and followups_followupactionid=9 ) as NoOfVisits, "+
                        "(select top 1 share_message from tbl_share "+
                        "WHERE share_targetid=t.enquiry_enquiryid order by share_shareid Desc )as PostCommand "+
                        "from tbl_enquiry t "+
                        "LEFT JOIN tbl_client ON client_clientid=enquiry_clientid "+
                        "LEFT JOIN tbl_campaign ON campaign_campaignid=enquiry_campaignid "+
                        "join tbl_enquiryfor On enquiryfor_enquiryforid=enquiry_enquiryforid "+
                        "left join tbl_priority on priority_priorityid=enquiry_priorityid "+
                        "left join tbl_enquirystage On enquirystage_enquirystageid=enquiry_enquirystageid "+
                        "LEFT JOIN tbl_product ON product_productid=enquiry_productid "+
                        "LEFT JOIN tbl_city ON city_cityid=enquiry_cityid "+
                        "LEFT JOIN tbl_area ON area_areaid=enquiry_areaid "+
                        "LEFT JOIN tbl_communicationsource ON communicationsource_communicationsourceid=enquiry_communicationsourceid "+
                        "LEFT JOIN tbl_employee ON employee_employeeid=enquiry_employeeid "+
                        "LEFT JOIN tbl_enquirystatus ON enquirystatus_enquirystatusid=enquiry_enquirystatusid " +
                       "WHERE enquiry_clientid=" + id;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html=new StringBuilder();
        html.Append("<table class='grid-print' cellspacing=0 cellpadding=5><tr><td width='50' class='first'>Sr. No.</td><td width='100'>Enquiry No</td>" +
                    "<td width='100'>Enquiry Date</td><td width='300'>Invoice For</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string lastrow = "";
            if (i == dttbl.Rows.Count - 1) lastrow = " class='lastrow'";
            html.Append("<tr" + lastrow + "><td class='first'>" + (i + 1) + "</td><td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["enquiry_enquiryno"]) + "</td>" +
                    "<td>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["enquiry_enquirydate"]) + "</td>" +
                    "<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["enquiry_subject"]) + "</td></tr>");
        }
        html.Append("</table>");
        ltInvoices.Text = html.ToString();
    }
    private void BindWorkReport(int id)
    {
        string query = "select * from( "+
                        "select complaint_ticketno as code,complaint_complaintdate as workdate, " +
                        "complaint_location as location, complaint_intime as intime, complaint_outtime outtime, "+
                        "complaint_manpower as manpower,complaint_manhours as manhours,complaint_remarks as remarks from tbl_complaint  " +
                        "where complaint_clientid= "+id+
                        " union all "+
                        "select '' as code,amcservice_servicedate as workdate, "+
                        "amcservice_location as location, amcservice_intime as intime, amcservice_outtime outtime, "+
                        "amcservice_manpower as manpower,amcservice_manhours as manhours,amcservice_remarks as remarks from tbl_amcservice  " +
                        "where amcservice_clientid=" + id +
                        " union all "+
                        "select handover_handovercode as code,handover_date as workdate, "+
                        "handover_location as location, handover_intime as intime, handover_outtime outtime, "+
                        "handover_manpower as manpower,handover_manhours as manhours,handover_remarks as remarks from tbl_handover  " +
                        "where handover_clientid=" + id +
                        ")r order by r.workdate";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table class='grid-print' cellspacing=0 cellpadding=5><tr><td width='50' class='first'>Sr. No.</td><td width='100'>Work Report No</td>" +
                    "<td width='80'>Date</td><td width='100'>Floor No</td><td>Remarks</td><td>In-Time</td><td>Out Time</td><td>Man Power</td><td>Man Hours</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string lastrow = "";
            if (i == dttbl.Rows.Count - 1) lastrow = " class='lastrow'";
            html.Append("<tr" + lastrow + "><td class='first'>" + (i + 1) + "</td><td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["code"]) + "</td>" +
                    "<td>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["workdate"]) + "</td>" +
                    "<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["location"]) + "</td>"+
                    "<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["remarks"]) + "</td>" +
                    "<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["intime"]) + "</td>" +
                    "<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["outtime"]) + "</td>" +
                    "<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["manpower"]) + "</td>" +
                    "<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["manhours"]) + "</td>" +
                    "</tr>");
        }
        html.Append("</table>");
        ltWorkReport.Text = html.ToString();
    }
}
