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

public partial class test2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            string extension = System.IO.Path.GetExtension(FileUpload1.FileName);
            if (extension== ".jpg")
            {
                FileUpload1.SaveAs(Server.MapPath("~/fileupload/" + FileUpload1.FileName));
                lblmessage.Text = "file uploaded";
            }
            else
            {
                lblmessage.Text = "Please select jpg file name";
            }
        }
        else
        {
            lblmessage.Text = "Please choose filename";
        }

    }
}
