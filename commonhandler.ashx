<%@ WebHandler Language="C#" Class="commonhandler" %>

using System;
using System.Web;
using WebComponent;
using System.Data;
using System.Collections;
using System.Text;

//common handler for client user and admin user
public class commonhandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        Common.ValidateAjaxRequest(); 
        context.Response.ContentType = "text/plain";
        string m = Common.GetQueryString("m");
        if (m == "save-kycclientownercontact")
        {
            int id = Common.GetQueryStringValue("id");
            Hashtable hstbl = new Hashtable();
            hstbl.Add("designationid", "1");
            id = Save("kycclientownercontact", "contacts", id, hstbl);
            HttpContext.Current.Response.Write("{\"status\":\"ok\"}");
        }
        else if (m == "save-kycclientfinancecontact")
        {
            int id = Common.GetQueryStringValue("id");
            Hashtable hstbl = new Hashtable();
            hstbl.Add("designationid", "4");
            id = Save("kycclientfinancecontact", "contacts", id, hstbl);
            HttpContext.Current.Response.Write("{\"status\":\"ok\"}");
        }
        else if (m == "save-kycbankdetail")
        {
            int id = Common.GetQueryStringValue("id");
            id = Save("kycbankdetail", "kycbankdetail", id);
            HttpContext.Current.Response.Write("{\"status\":\"ok\"}");
        }
        else if (m == "kycownercontactgrid")
        {
            GetClientContactsGrid(true);
        }
        else if (m == "kycfinancecontactgrid")
        {
            GetClientContactsGrid(false);
        }
        else if (m == "kycbankdetailgrid")
        {
            GetKYCBankDetailGrid();
        }
    }
    private int ClientId
    {
        get
        {
            if (Common.UserId > 0)
            {
                return Common.GetQueryStringValue("clientid");
            }
            else
            {
                return Common.ClientId;
            }
        }
    }
    private int Save(string prefix, string m, int id)
    {
        return Save(prefix, m, id, null);
    }
    private int Save(string prefix, string m, int id, Hashtable hstbl)
    {
        if (hstbl == null)
        {
            hstbl = new Hashtable();
        }
        hstbl.Add("clientid", ClientId);
        for (int i = 0; i < HttpContext.Current.Request.Form.Keys.Count; i++)
        {
            string key = HttpContext.Current.Request.Form.Keys[i];
            key = key.Substring(3);
            if (key.StartsWith(prefix))
            {
                Array arr = key.Split('_');
                string colname = arr.GetValue(1).ToString();
                string val = HttpContext.Current.Request.Form[i];
                if (val == "on") val = "1";
                hstbl.Add(colname, val);
            }
        }
        InsertUpdate obj = new InsertUpdate();
        if (id == 0)
        {
            if (Common.ClientUserId > 0)
            {
                hstbl.Add("createdbyclientuserid", Common.ClientUserId);
                hstbl.Add("createddateclientuser", "getdate()");
            }
            id = obj.InsertData(hstbl, "tbl_" + m);
        }
        else
        {
            if (Common.ClientUserId > 0)
            {
                hstbl.Add("modifiedbyclientuserid", Common.ClientUserId);
                hstbl.Add("modifieddateclientuser", "getdate()");
            }
            id = obj.UpdateData(hstbl, "tbl_" + m, id);
        }
        return id;
    }
    private void GetClientContactsGrid(bool isownercontact)
    {
        string query = "";
        query = @"select * from tbl_contacts where contacts_clientid=" + ClientId;
        int designationId = 1;
        if (!isownercontact) designationId = 4;
        query += " and contacts_designationid=" + designationId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%' cellpadding=7 class='grid-ui' border=1>");
        html.Append(@"<tr class='grid-ui-header'><td>Contact Person</td><td>Mobile Number</td><td>Email Id</td><td>Edit</td></tr>");
        
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["contacts_contactsid"]);
            string name = GlobalUtilities.ConvertToString(dttbl.Rows[i]["contacts_contactperson"]);
            string mobileNo = GlobalUtilities.ConvertToString(dttbl.Rows[i]["contacts_mobileno"]);
            string emailId = GlobalUtilities.ConvertToString(dttbl.Rows[i]["contacts_emailid"]);
            string editmn = "kycclientfinancecontact";
            if (isownercontact) editmn = "kycclientownercontact";
            html.Append("<tr>");
            html.Append("<td>" + name + "</td><td>" + mobileNo + "</td><td>" + emailId + "</td>"+
                        "<td class='linktext jq-edit jq-common-btneditmodal' did='" + id + "' mn='" + editmn + "' modaltitle='Edit Contact Details'>Edit</td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        HttpContext.Current.Response.Write(html.ToString());
    }
    private void GetKYCBankDetailGrid()
    {
        string query = "";
        query = @"select * from tbl_kycbankdetail 
                  left join tbl_bankauditbank on bankauditbank_bankauditbankid=kycbankdetail_bankauditbankid
                  where kycbankdetail_clientid=" + ClientId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%' cellpadding=7 class='grid-ui' border=1>");
//        html.Append(@"<tr class='grid-ui-header'><td>Bank</td><td>Account Number</td><td>Branch</td><td>Bank Margin</td>
//                    <td>Treasury Contact No</td><td>Branch RM Name</td><td>Branch RM No.</td><td>Branch Head Name</td><td>Branch Head No</td>
//                    <td>Sanctioned Letter Renewal date</td><td>Audit Done?</td><td>Edit</td></tr>");

        html.Append(@"<tr class='grid-ui-header'><td>Bank</td><td>Account Number</td><td>Branch</td><td style='min-width:50px !important'>Bank Margin</td>
                    <td style='min-width:80px !important'>Sanctioned Letter Renewal date</td><td>Treasury Contact Number</td>
                    <td>RM Name</td><td>RM Contact Number</td>                    
                    <td style='min-width:50px !important'>Audit Done?</td><td style='min-width:50px !important'>Edit</td></tr>");

        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["kycbankdetail_kycbankdetailid"]);
            html.Append("<tr>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditbank_bankname"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["kycbankdetail_accountnumber"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["kycbankdetail_branchname"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["kycbankdetail_bankmargininpaisa"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["kycbankdetail_sanctionedletterrenewaldate"]) + "</td>");

            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["kycbankdetail_treasurycontactnumber"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["kycbankdetail_branchrmcontactperson"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["kycbankdetail_branchrmcontactnumber"]) + "</td>");
            
            //html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["kycbankdetail_treasurycontactnumber"]) + "</td>");
            //html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["kycbankdetail_branchrmcontactperson"]) + "</td>");
            //html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["kycbankdetail_branchrmcontactnumber"]) + "</td>");
            //html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["kycbankdetail_branchheadcontactperson"]) + "</td>");
            //html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["kycbankdetail_branchheadcontactnumber"]) + "</td>");
            //html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["kycbankdetail_sanctionedletterrenewaldate"]) + "</td>");
            html.Append("<td>" + (GlobalUtilities.ConvertToBool(dttbl.Rows[i]["kycbankdetail_isauditdone"]) ? "Yes": "No") + "</td>");
            html.Append("<td class='linktext jq-edit jq-common-btneditmodal' did='" + id + "' mn='kycbankdetail' modaltitle='Edit Bank Details' modalwidth='900px' modalheight='500px'>Edit</td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        HttpContext.Current.Response.Write(html.ToString());
    }
    public bool IsReusable
    {
        get {
            return false;
        }
    }

}