<%@ WebHandler Language="C#" Class="liveratealert" %>

using System;
using System.Web;
using WebComponent;
using System.Data;
using System.Text;

public class liveratealert : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        DateTime dt = DateTime.Now;
        int h = dt.Hour;
        if (h < 9 || h >= 17) return;
        if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday) return;
        string query = @"select * from tbl_liveratealert
                        join tbl_liverate on liverate_liverateid=liveratealert_liverateid
                        join tbl_currencymaster on currencymaster_currencymasterid=liveratealert_currencymasterid                        
                        join tbl_covertype on covertype_covertypeid=liveratealert_covertypeid                        
                        where cast(liverate_currentrate as numeric(19,8)) > 0 AND liveratealert_alertstatusid=1 AND
                        (
	                        (
		                        liveratealert_covertypeid = 2 --export
		                        AND
		                        (
			                        (
				                        liveratealert_target > 0
				                        AND
				                        (liveratealert_targetsentdate is null or cast(liveratealert_targetsentdate as date)<cast(getdate() as date))
				                        AND
				                        liverate_currentrate >= liveratealert_target 
			                        )
			                        OR
			                        (
				                        liveratealert_stoploss > 0
				                        AND
				                        (liveratealert_stoplosssentdate is null or cast(liveratealert_stoplosssentdate as date)<cast(getdate() as date))
				                        AND
				                        liverate_currentrate <= liveratealert_stoploss
			                        )
		                        )
	                        )
	                        OR
	                        (
		                        liveratealert_covertypeid = 1 --import
		                        AND
		                        (
			                        (
				                        liveratealert_target > 0
				                        AND
				                        (liveratealert_targetsentdate is null or cast(liveratealert_targetsentdate as date)<cast(getdate() as date))
				                        AND
				                        liverate_currentrate <= liveratealert_target 
			                        )
			                        OR
			                        (
				                        liveratealert_stoploss > 0
				                        AND
				                        (liveratealert_stoplosssentdate is null or cast(liveratealert_stoplosssentdate as date)<cast(getdate() as date))
				                        AND
				                        liverate_currentrate >= liveratealert_stoploss
			                        )
		                        )
	                        )
                        )              
                ";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        dttbl.Columns.Add("triggertime");
        dttbl.Columns.Add("alertlevel");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            double liverate = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["liverate_currentrate"]);
            double target = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["liveratealert_target"]);
            double stoploss = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["liveratealert_stoploss"]);
            string emailId = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liveratealert_emailid"]);//comma sep contacts id
            int coverType = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liveratealert_covertypeid"]);
            emailId = GetContactDetail(emailId, "contacts_emailid");
            string mobileNo = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liveratealert_mobileno"]);//comma sep contacts id
            mobileNo = GetContactDetail(mobileNo, "contacts_mobileno");
            string currency = GlobalUtilities.ConvertToString(dttbl.Rows[i]["currencymaster_currency"]);
            int liveratealertId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liveratealert_liveratealertid"]);
            DateTime dtTargetSentDate = new DateTime();
            if (dttbl.Rows[i]["liveratealert_targetsentdate"] != DBNull.Value) dtTargetSentDate = Convert.ToDateTime(dttbl.Rows[i]["liveratealert_targetsentdate"]);
            DateTime dtStopLossSentDate = new DateTime();
            if (dttbl.Rows[i]["liveratealert_stoplosssentdate"] != DBNull.Value) dtStopLossSentDate = Convert.ToDateTime(dttbl.Rows[i]["liveratealert_stoplosssentdate"]);
            
            double triggerLevel = 0;
            bool targetSent = false;
            bool stopLossSent = false;
            bool isTarget = false;
            
            if (dtTargetSentDate.Day == DateTime.Now.Day && dtTargetSentDate.Month == DateTime.Now.Month && dtTargetSentDate.Year == DateTime.Now.Year)
            {
                targetSent = true;
            }
            if (dtStopLossSentDate.Day == DateTime.Now.Day && dtStopLossSentDate.Month == DateTime.Now.Month && dtStopLossSentDate.Year == DateTime.Now.Year)
            {
                stopLossSent = true;
            }
            if (coverType == 1)//Import
            {
                if (liverate <= target && target > 0)
                {
                    triggerLevel = target;
                    if (targetSent) continue;
                    isTarget = true;
                }
                else
                {
                    triggerLevel = stoploss;
                    if (stopLossSent) continue;
                }
            }
            else //Export
            {
                if (liverate >= target && target > 0)
                {
                    triggerLevel = target;
                    if (targetSent) continue;
                    isTarget = true;
                }
                else
                {
                    triggerLevel = stoploss;
                    if (stopLossSent) continue;
                }
            }
            //context.Response.Write(dtTargetSentDate);
            //context.Response.Write(targetSent);
            //context.Response.End();
            string message = Common.GetSetting("Finstation Live Rate Alert");
            string subject = "Finstation Live Rate Alert - " + currency;

            dttbl.Rows[i]["triggertime"] = GlobalUtilities.ConvertToDateTimeMMM(DateTime.Now);
            dttbl.Rows[i]["alertlevel"] = triggerLevel.ToString();
            
            //message = message.Replace("$triggertime$", GlobalUtilities.ConvertToDateTimeMMM(DateTime.Now));
            //message = message.Replace("$alertlevel$", triggerLevel.ToString());
            message = Common.GetFormattedSettingForEmail(message, dttbl.Rows[i], false);
            bool isemailsent = false;
            try
            {
                isemailsent = BulkEmail.SendMail_Alert(emailId, subject, message, "");
            }
            catch { }
            string smsmessage = Common.GetSetting("Finstation Live Rate Alert SMS");
            //smsmessage = smsmessage.Replace("$triggertime$", GlobalUtilities.ConvertToDateTimeMMM(DateTime.Now));
            //smsmessage = smsmessage.Replace("$alertlevel$", triggerLevel.ToString());
            smsmessage = Common.GetFormattedSettingForEmail(smsmessage, dttbl.Rows[i], false);
            bool issmssent = false;
            bool iswhatsappsent = false;
            try
            {
                issmssent = SMS.SendSMS(mobileNo, smsmessage);
            }
            catch { }
            try
            {
                RPlusWhatsAppAPI.SendMessage("", false, "liveratealert", 0, dttbl.Rows[i], mobileNo, EnumWhatsAppMessageType.Text,
                                "Live Rate Alert", "", "", "", "", "");
            }
            catch { }
            if (issmssent || isemailsent)
            {
                string sentdateColumn = "liveratealert_stoplosssentdate";
                if (isTarget)
                {
                    sentdateColumn = "liveratealert_targetsentdate";
                }
                query = "update tbl_liveratealert set " + sentdateColumn + "=getdate() where liveratealert_liveratealertid=" + liveratealertId;
                DbTable.ExecuteQuery(query);
            }
        }
        context.Response.Write("Ok");
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
                    contacts.Append("," + val);
                }
            }
        }
        return contacts.ToString();
    }
    public bool IsReusable
    {
        get {
            return false;
        }
    }

}