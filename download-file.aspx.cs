using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using WebComponent;

public partial class download_file : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string filePath = @"upload\" + Request.QueryString["f"];
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
            string fileActualPath = Server.MapPath(filePath);
            if (!System.IO.File.Exists(fileActualPath))
            {
                ErrorLog.WriteLog("download_file not exists: fileActualPath=" + fileActualPath);
            }
            byte[] data = req.DownloadData(fileActualPath);
            response.BinaryWrite(data);
            response.End();
        }
        catch (Exception ex)
        {
        }
    }
}
