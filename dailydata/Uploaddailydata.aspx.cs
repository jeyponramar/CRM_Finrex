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
    private void SendEmail()
    {
        string message = Common.GetSetting("InoFinExportData File Upload Notification Email");
        string url = "https://finstation.in/shareddata/download.aspx?t=inofinexportdata&cuid=abc";
        string toEmailId = "jeyponramar@gmail.com";
        string subject = "InoFinExportData File Upload Notification";
        string error = "";
        message = message.Replace("$fileurl$", url);
        BulkEmail.SendMail(toEmailId, subject, message, "");
    }
}
