<%@ WebHandler Language="C#" Class="bulkemailsms" %>

using System;
using System.Web;
using System.Data;
using WebComponent;

public class bulkemailsms : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        try
        {
            int bulksmsid = GlobalUtilities.ConvertToInt(context.Request.QueryString["id"]);
            string query = "select top 10 * from tbl_bulksmsdetail WHERE bulksmsdetail_emailsmssentstatusid=1 AND bulksmsdetail_bulksmsid=" + bulksmsid;
            DataTable dttbl = DbTable.ExecuteSelect(query);

            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                string mobileNo = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bulksmsdetail_mobileno"]);
                string sms = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bulksmsdetail_message"]);
                int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["bulksmsdetail_bulksmsdetailid"]);
                int clientId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["bulksmsdetail_clientid"]);
                int sentstatusid = 3;//2 - sent, 3 - failed

                if (sms.Contains("$"))
                {
                    DataRow drClient = DbTable.GetOneRow("tbl_client", clientId);
                    sms = Common.GetFormattedSettingForEmail(sms, drClient, false);
                }
                bool issent = SMS.SendSMS(mobileNo, sms);
                if (issent) sentstatusid = 2;
                
                query = "update tbl_bulksmsdetail set bulksmsdetail_date=GETDATE(),bulksmsdetail_emailsmssentstatusid=" + sentstatusid +
                        " WHERE bulksmsdetail_bulksmsdetailid=" + id;
                DbTable.ExecuteQuery(query);
            }
            query = "update tbl_bulksms set bulksms_totalsent=(select count(*) from tbl_bulksmsdetail WHERE bulksmsdetail_emailsmssentstatusid=2 AND bulksmsdetail_bulksmsid=" + bulksmsid + ")," +
                    "bulksms_totalfailed=(select count(*) from tbl_bulksmsdetail WHERE bulksmsdetail_emailsmssentstatusid=3 AND bulksmsdetail_bulksmsid=" + bulksmsid + ")";
            DbTable.ExecuteQuery(query);
            query = "select bulksms_totalsent as totalsent,bulksms_totalfailed as totalfailed,bulksms_balance as balance from tbl_bulksms WHERE bulksms_bulksmsid=" + bulksmsid;
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (GlobalUtilities.ConvertToInt(dr["balance"]) == 0)
            {
                query = "update tbl_bulksms set bulksms_emailsmsstatusid=4 where bulksms_bulksmsid=" + bulksmsid;
                DbTable.ExecuteQuery(query);
            }
            context.Response.Write(JSON.Convert(dr));
        }
        catch (Exception ex)
        {
            context.Response.Write("");
        }
    }

    public bool IsReusable
    {
        get {
            return false;
        }
    }

}