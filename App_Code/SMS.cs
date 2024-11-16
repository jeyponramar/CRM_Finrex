using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WebComponent;
using System.Net;
using System.IO;
using System.Collections;
using System.Text;

/// <summary>
/// Summary description for SMS
/// </summary>
public class SMS
{
    public static bool CheckSMSValidity_Count(ref int RemainingSMS)
    {
        bool isValid = false;
        if (CustomSettings.IsSMSPackageEnable)
        {
            int TotalSMS = CustomSettings.m_TotalSMS;
            DataRow dr = DbTable.ExecuteSelectRow(@"SELECT ISNULL(MAX(smsdeliveryhistory_smsdeliveryhistoryid),0)AS TotalSmsSent
                                                        FROM tbl_smsdeliveryhistory");
            if (dr != null)
            {
                int TotalSMSSent = GlobalUtilities.ConvertToInt(dr["TotalSmsSent"]);
                if (TotalSMS > TotalSMSSent)
                {
                    isValid = true;
                }
                RemainingSMS = TotalSMS - TotalSMSSent;
                if (RemainingSMS <= 0)
                {
                    //global.SendMail("SMS Package is Over", "SMS Package is Over for " + CustomSettings.CompanyName, CustomSettings.RefuxSupportEmailId, "", "", "");
                }
            }
        }
        return isValid;
    }
    public static bool CheckSMSValidity_Count()
    {
        if (AppConstants.IsDemoVersion) return true;
        int RemainingSMS = 0;
        return (CheckSMSValidity_Count(ref RemainingSMS));
    }
    public static bool SendSMS(string MobileNo, string Message)
    {
        //if (CheckSMSValidity_Count())
        {
            bool isSent = false;
            //MobileNo = MobileNo.Replace("+91", "").Replace("91+", "").Replace(" ", "").Trim();
            //if (Message.Trim() == "" || MobileNo.Length != 10)
            //{
            //    return false;
            //}
            //MobileNo = "91" + MobileNo;

            StringBuilder mobileNos = new StringBuilder();
            Array arr = MobileNo.Split(',');
            for (int i = 0; i < arr.Length; i++)
            {
                string mob = arr.GetValue(i).ToString();
                mob = mob.Replace("+91", "").Replace("91+", "").Replace(" ", "").Trim();
                if (mob.Length == 10)
                {
                    mob = "91" + mob;
                    if (mobileNos.ToString() == "")
                    {
                        mobileNos.Append(mob);
                    }
                    else
                    {
                        mobileNos.Append("," + mob);
                    }
                }
            }
            if (mobileNos.ToString() == "") return false;

            string url = CustomSettings.SMS_URL.Replace("MobileNo", mobileNos.ToString()).Replace("Message", Message);
            if (url == "")
            {
                return false;
            }
            try
            {

                var webRequest = WebRequest.Create(url);
                using (var response = webRequest.GetResponse())
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    string responseString = sr.ReadToEnd();

                    if (responseString.StartsWith("The Message Id") || responseString.Contains(MobileNo.Replace("91", ""))
                         || responseString.Contains("|"))
                    {
                        isSent = true;
                    }
                }
                //insert Hash Table History
                if (isSent)
                {
                    Hashtable hstbl = new Hashtable();
                    hstbl.Add("mobileno", MobileNo);
                    hstbl.Add("message", Message);
                    hstbl.Add("smsdeliverydate", "GETDATE()");
                    InsertUpdate obj = new InsertUpdate();
                    obj.InsertData(hstbl, "tbl_smsdeliveryhistory");
                }

            }

            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("<p class='error'>Unable to send sms</p>");
            }
            return isSent;
        }
        //else
        {
          //  HttpContext.Current.Response.Write("<p class='error'>SMS validity is expired!</p>");
        }
        return false;
    }
    
}
