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
using System.Net;

public partial class download_pdf : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GeneratePdf();
    }
    private void GeneratePdf()
    {
        string m = Common.GetQueryString("m");
        int id = Common.GetQueryStringValue("id");
        string filePath = "";
        if (m == "invoice")
        {
            FinrexInvoicePdf obj = new FinrexInvoicePdf();
            filePath = obj.GenerateInvoice(id);
        }
        else if (m == "proformainvoice")
        {
            FinrexProformaInvoicePdf obj = new FinrexProformaInvoicePdf();
            filePath = obj.GenerateInvoice(id);
        }
        if (filePath != "")
        {
            filePath = "~/upload/temp/" + filePath;
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            Array arr = filePath.Split('/');
            if (arr.Length == 0) arr = filePath.Split('\\');
            string fileName = arr.GetValue(arr.Length - 1).ToString();
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
    }
    
}
