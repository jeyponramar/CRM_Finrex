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
using System.IO;

public partial class dailydata_Uploaddailydata : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ltdownloadurl.Text = "<a href='" + DownloadUrl + "' target='_blank'>" + DownloadUrl + "</a>";
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (fleinofinupload.HasFile)
        {
            string extension = System.IO.Path.GetExtension(fleinofinupload.FileName);
            if (extension == ".csv")
            {
                fleinofinupload.SaveAs(Server.MapPath("~/upload/sharedfiles/InoFinExportData.csv"));
                SendEmail();
                lblmessage.Text = "File has been uploaded successfully";
            }
            else
            {
                lblmessage.Text = "Please select the csv file";
            }
        }
        else
        {
            lblmessage.Text = "Please choose the file";
        }
    }
    private string DownloadUrl
    {
        get
        {
            string appUrl = Common.GetApplicationURL();
            return appUrl + "/shareddata/download.aspx?t=inofinexportdata&cuid=5653012E-877D-4D68-8FF0-BBB0CDEF7120";
        }
    }
    private void SendEmail()
    {
        string message = Common.GetSetting("InoFinExportData File Upload Notification Email");
        string toEmailId = Common.GetSetting("InoFinExportData Notification Email Id");
        if (toEmailId == "") return;
        string subject = "InoFinExportData File Upload Notification";
        string error = "";
        message = message.Replace("$fileurl$", DownloadUrl);
        BulkEmail.SendMail(toEmailId, subject, message, "");
    }
}
