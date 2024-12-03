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
        if (fleupload.HasFile)
        {
            string extension = System.IO.Path.GetExtension(fleupload.FileName);
            if (extension == ".csv")
            {
                fleupload.SaveAs(Server.MapPath("~/upload/sharedfiles/InoFinExportData.csv"));
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
}
