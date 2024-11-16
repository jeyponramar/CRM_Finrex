<%@ WebHandler Language="C#" Class="reminder" %>

using System;
using System.Web;
using WebComponent;
using System.Data;

public class reminder : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        SendWishes();
    }
    private void SendWishes()
    {
        //send birthday reminder
        DataTable dttblBirthay = Reminder.GetBirthdayReminders();
        string birthayWishSubject = Common.GetSetting("Birthday Wishes Mail Subject");
        string birthayWishMessage = Common.GetSetting("Birthday Wishes Mail Message");
        string anniversaryWishSubject = Common.GetSetting("Anniversary Wishes Mail Subject");
        string anniversaryWishMessage = Common.GetSetting("Anniversary Wishes Mail Message");
        string birthayWishSMS = Common.GetSetting("Birthday Wishes SMS");
        string anniversaryWishesSMS = Common.GetSetting("Anniversary Wishes SMS");
        for (int i = 0; i < dttblBirthay.Rows.Count; i++)
        {
            string emailId = GlobalUtilities.ConvertToString(dttblBirthay.Rows[i]["contacts_emailid"]);
            string mobileNo = GlobalUtilities.ConvertToString(dttblBirthay.Rows[i]["contacts_mobileno"]);
            int contactId = GlobalUtilities.ConvertToInt(dttblBirthay.Rows[i]["contacts_contactsid"]);
            if (emailId != "")
            {
                string body = Common.GetFormattedSettingForEmail(birthayWishMessage, dttblBirthay.Rows[i], false);
                bool isemailed = BulkEmail.SendMail(emailId, birthayWishSubject, body, "");

                string sms = Common.GetFormattedSettingForEmail(birthayWishSMS, dttblBirthay.Rows[i], false);
                bool issmssent = SMS.SendSMS(mobileNo, sms);

                Reminder.UpdateBirthdayEmailSMSYear(isemailed, issmssent, contactId);
            }

        }
        //send anniversary reminder
        DataTable dttblAnniversary = Reminder.GetAnniversaryReminders();
        for (int i = 0; i < dttblAnniversary.Rows.Count; i++)
        {
            string emailId = GlobalUtilities.ConvertToString(dttblAnniversary.Rows[i]["contacts_emailid"]);
            string mobileNo = GlobalUtilities.ConvertToString(dttblAnniversary.Rows[i]["contacts_mobileno"]);
            int contactId = GlobalUtilities.ConvertToInt(dttblBirthay.Rows[i]["contacts_contactsid"]);
            if (emailId != "")
            {
                string body = Common.GetFormattedSettingForEmail(anniversaryWishSubject, dttblAnniversary.Rows[i], false);
                bool isemailed = BulkEmail.SendMail(emailId, anniversaryWishMessage, body, "");

                string sms = Common.GetFormattedSettingForEmail(anniversaryWishesSMS, dttblAnniversary.Rows[i], false);
                bool issmssent = SMS.SendSMS(mobileNo, sms);

                Reminder.UpdateAnniversaryEmailSMSYear(isemailed, issmssent, contactId);
            }

        }
    }
    public bool IsReusable
    {
        get {
            return false;
        }
    }

}