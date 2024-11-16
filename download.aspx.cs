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
using System.Net;
using System.IO;

public partial class download : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string filePath = Request.QueryString["f"];
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            Array arr = filePath.Split('/');
            if (arr.Length == 0) arr = filePath.Split('\\');
            string fileName = arr.GetValue(arr.Length - 1).ToString();
            Array arr1 = fileName.Split('_');
            fileName = arr1.GetValue(arr1.Length - 1).ToString();
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
            byte[] data = req.DownloadData(Server.MapPath(filePath));
            response.BinaryWrite(data);
            response.End();
        }
        catch (Exception ex)
        {
        }
    }
}
