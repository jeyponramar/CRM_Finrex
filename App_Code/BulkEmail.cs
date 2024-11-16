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
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using WebComponent;

/// <summary>
/// Summary description for Email
/// </summary>
public class BulkEmail
{
    public static string apiRequestData = "";
    private static int _MaxBulkEmail = 990;
    public static bool SendMail(string ToEmailId, string subject, string message, string attachment)
    {
        //InitBulkEmail();
        //return BulkEmailUtility.SendMail("Finrex", "info@finrex.in", ToEmailId, subject, message, attachment);
        return RPlusEmailUtility.SendEmail("Finrex", "info@finrex.in", ToEmailId, "","", subject, message, attachment);
    }
    public static bool SendMail_Alert(string ToEmailId, string subject, string message, string attachment)
    {
        //InitBulkEmail();
        //return BulkEmailUtility.SendMail("Finrex", "alert@finrex.in", ToEmailId, subject, message, attachment);
        return RPlusEmailUtility.SendEmail("Finrex", "alert@finrex.in", ToEmailId, "", "", subject, message, attachment);
    }
    public static bool SendMail_Alert(string ToEmailId, string ccEmailId, string bccEmailId, string subject, string message, string attachment)
    {
        //InitBulkEmail();
        //return BulkEmailUtility.SendMail("Finrex", "alert@finrex.in", ToEmailId, subject, message, attachment, ccEmailId, bccEmailId);
        return RPlusEmailUtility.SendEmail("Finrex", "alert@finrex.in", ToEmailId, ccEmailId, bccEmailId, subject, message, attachment);
    }
    private static void InitBulkEmail()
    {
        //we are not using falconide api as price is high, as per discussion with Himesh on 30-11-23
        //BulkEmailUtility._apiKey = "a0baca018b34101eac0513345ed88f1e";
        //BulkEmailUtility._emailApiUrl = "https://api.falconide.com/falconapi/web.send.json";
    }
    public static bool SendMailFromLoginUser(string ToEmailId, string subject, string message, string attachment)
    {
        //InitBulkEmail();
        //return BulkEmailUtility.SendMail(Common.FullName, Common.EmailId, ToEmailId, subject, message, attachment);
        return RPlusEmailUtility.SendEmail(Common.FullName, Common.EmailId, ToEmailId, "", "", subject, message, attachment);
    }
    public static bool SendMailFromLoginUser(string ToEmailId, string subject, string message, string attachment, string cc, string bcc)
    {
        //InitBulkEmail();
        //return BulkEmailUtility.SendMail(Common.FullName, Common.EmailId, ToEmailId, subject, message, attachment, cc, bcc);
        return RPlusEmailUtility.SendEmail(Common.FullName, Common.EmailId, ToEmailId, cc, bcc, subject, message, attachment);
    }
    public static bool SendMail(string fromName, string fromEmailId, string ToEmailId, string subject, string message, string attachment)
    {
        //InitBulkEmail();
        //return BulkEmailUtility.SendMail(fromName, fromEmailId, ToEmailId, subject, message, attachment, "", "");
        return RPlusEmailUtility.SendEmail(fromName, fromEmailId, ToEmailId, "", "", subject, message, attachment);
    }
    public static bool SendBulkEmailOnlyFromLoggedInUser(string ToEmailId, string subject, string message, string attachment)
    {
        //return SendBulkEmailOnly(Common.FullName, Common.EmailId, ToEmailId, subject, message, attachment);
        return RPlusEmailUtility.SendBulkEmail(Common.FullName, Common.EmailId, ToEmailId, "", "", subject, message, attachment);
    }
    public static bool SendBulkEmailOnly(string ToEmailId, string subject, string message, string attachment)
    {
        //return SendBulkEmailOnly("Finrex", "info@finrex.in", ToEmailId, subject, message, attachment);
        return RPlusEmailUtility.SendBulkEmail("Finrex", "info@finrex.in", ToEmailId, "", "", subject, message, attachment);
    }
    public static bool SendBulkEmailOnly(string fromName, string fromEmailId, string ToEmailId, string subject, string message, string attachment)
    {
        return RPlusEmailUtility.SendBulkEmail(fromName, fromEmailId, ToEmailId, "", "", subject, message, attachment);
        
        //bool issucess = false;
        //string[] arr = ToEmailId.ToLower().Split(',');
        //arr = arr.Distinct().ToArray();
        //StringBuilder toEmailIds = new StringBuilder();
        //for (int i = 0; i < arr.Length; i++)
        //{
        //    if (arr.GetValue(i).ToString() != "")
        //    {
        //        if (toEmailIds.ToString() == "")
        //        {
        //            toEmailIds.Append(arr.GetValue(i).ToString());
        //        }
        //        else
        //        {
        //            toEmailIds.Append("," + arr.GetValue(i).ToString());
        //        }
        //    }
        //    if (i % _MaxBulkEmail == 0)
        //    {
        //        issucess = SendBulkEmailOnly_API(fromName, fromEmailId, toEmailIds.ToString(), subject, message, attachment);
        //        toEmailIds = new StringBuilder();
        //    }
        //}
        //if (toEmailIds.ToString() != "")
        //{
        //    issucess = SendBulkEmailOnly_API(fromName, fromEmailId, toEmailIds.ToString(), subject, message, attachment);
        //}
        //return issucess;
    }
    public static bool SendBulkEmailOnly_API(string fromName, string fromEmailId, string ToEmailId, string subject, string message, string attachment)
    {
        //return SendBulkEmailOnly_API(fromName, fromEmailId, ToEmailId, "", "", subject, message, attachment);
        return RPlusEmailUtility.SendBulkEmail(fromName, fromEmailId, ToEmailId, "", "", subject, message, attachment);
    }
    public static bool SendBulkEmailOnly_API(string fromName, string fromEmailId, string ToEmailId, string ccEmailId, string bccEmailId, 
        string subject, string message, string attachment)
    {
        return RPlusEmailUtility.SendBulkEmail(fromName, fromEmailId, ToEmailId, ccEmailId, bccEmailId, subject, message, attachment);

        //try
        //{
        //    message = ReplaceURIString(message);
        //    string siteUrl = HttpContext.Current.Request.Url.ToString();
        //    Array arru = siteUrl.Split('/');
        //    string websiteUrl = "";
        //    for (int i = 0; i < arru.Length; i++)
        //    {
        //        if (arru.GetValue(i).ToString().Contains(".aspx")) break;
        //        if (i == 0)
        //        {
        //            websiteUrl = arru.GetValue(i).ToString();
        //        }
        //        else
        //        {
        //            websiteUrl += "/" + arru.GetValue(i).ToString();
        //        }
        //    }
        //    int lastIndex = websiteUrl.LastIndexOf('/');
        //    websiteUrl = websiteUrl.Substring(0, lastIndex);
        //    message = message.Replace("../", websiteUrl + "/");

        //    StringBuilder recipients = new StringBuilder();
        //    string[] arr = ToEmailId.ToLower().Split(',');
        //    arr = arr.Distinct().ToArray();
        //    for (int i = 0; i < arr.Length; i++)
        //    {
        //        if (i > 0)
        //        {
        //            recipients.Append(",");
        //        }
        //        recipients.Append("{\"to\": [" +
        //                            "{" +
        //                              "\"email\": \"" + arr.GetValue(i).ToString() + "\"" +
        //                            "}" +
        //                          "]}"
        //                        );
        //    }
        //    StringBuilder attachments = new StringBuilder();
        //    string attachmentData = "";
        //    if (attachment != "")
        //    {
        //        attachments.Append(",\"attachments\": [");
        //        Array arrFiles = attachment.Split(',');
        //        for (int i = 0; i < arrFiles.Length; i++)
        //        {
        //            string fileFullName = arrFiles.GetValue(i).ToString();
        //            Array arrf = fileFullName.Split('/');
        //            if (arrf.Length <= 1) arrf = fileFullName.Split('\\');
        //            string fileName = arrf.GetValue(arrf.Length - 1).ToString();
        //            if (fileName.Contains("_"))
        //            {
        //                Array arrt = fileName.Split('_');
        //                fileName = arrt.GetValue(arrt.Length - 1).ToString();
        //            }
        //            if (fileFullName.StartsWith("~"))
        //            {
        //                fileFullName = HttpContext.Current.Server.MapPath(fileFullName);
        //            }
        //            FileStream fs = new FileStream(fileFullName, FileMode.Open, FileAccess.Read);
        //            byte[] filebytes = new byte[fs.Length];
        //            fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
        //            string filedata = Convert.ToBase64String(filebytes);

        //            if (i > 0) attachments.Append(",");
        //            attachments.Append("{" +
        //                                    "\"filename\": \"" + fileName + "\"," +
        //                                    "\"content\": \"" + filedata + "\"" +
        //                                "}");
        //        }
        //        attachments.Append("]");
        //    }

        //    string body = "{" +
        //                     "\"personalizations\": [" +
        //                        recipients.ToString() +
        //                     "]," +
        //                     "\"from\": {" +
        //                       "\"email\": \"" + fromEmailId + "\"," +
        //                       "\"name\": \"" + fromName + "\"" +
        //                     "}," +
        //                     "\"subject\": \"" + subject + "\"," +
        //                     "\"content\": [" +
        //                       "{" +
        //                         "\"type\": \"text/html\"," +
        //                         "\"value\": \"" + message + "\"" +
        //                       "}" +
        //                     "]" +
        //        //"\"attachments\": [" +
        //                        attachments.ToString() +
        //        //"]" +
        //                   "}";

        //    if (!File.Exists(HttpContext.Current.Server.MapPath("~/doc/log/bulkemailonly.txt")))
        //    {
        //        File.WriteAllText(HttpContext.Current.Server.MapPath("~/doc/log/bulkemailonly.txt"), body);
        //    }
        //    if (!File.Exists(HttpContext.Current.Server.MapPath("~/doc/log/bulkemailonlyids.txt")))
        //    {
        //        File.WriteAllText(HttpContext.Current.Server.MapPath("~/doc/log/bulkemailonlyids.txt"), ToEmailId);
        //    }
        //    var request = (HttpWebRequest)WebRequest.Create("https://api.us1-mta1.sendclean.net/v2/mail/send");
        //    request.Headers.Add("Authorization", "SC.8d64bcb1db3d27cc.b53e6e4a025336a703f6f982");
        //    request.ContentType = "application/json";
        //    request.Method = "POST";
        //    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        //    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        //    {
        //        streamWriter.Write(body);
        //    }
        //    var httpResponse = (HttpWebResponse)request.GetResponse();
        //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //    {
        //        var result = streamReader.ReadToEnd();
        //        HttpContext.Current.Response.Write(result);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ErrorLog.WriteLog("SendBulkEmailOnly:" + ex.Message);
        //    return false;
        //}
        //return true;
    }
    private static string ReplaceURIString(string data)
    {
        data = data.Replace("\"", "'").Replace("\n", "").Replace("\r", "").Replace("\t", "");
        return data;
    }
}
