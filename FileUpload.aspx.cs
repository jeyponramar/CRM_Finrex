using System;
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

public partial class FileUpload : System.Web.UI.Page 
{
    public string FileType = "";
    public string FolderPath = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.Enctype = "multipart/form-data";
        
        if(Request.QueryString["type"]=="image")
        {
            FileType = "image";
            FolderPath = "upload/photos/" + Request.QueryString["id"] + "/";
        }
        else if (Request.QueryString["type"] == "song")
        {
            FileType = "song";
            FolderPath = "upload/songs/" + Request.QueryString["id"] + "/";
        }
        else if (Request.QueryString["type"] == "videos")
        {
            FileType = "video";
            FolderPath = "upload/videos/" + Request.QueryString["id"] + "/";
        }
    }
}
