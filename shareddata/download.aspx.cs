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
using WebComponent;

public partial class shareddata_download : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DownloadData();
        }
    }
    private void DownloadData()
    {
        string type = Common.GetQueryString("t");
        string clientUniqueId = Common.GetQueryString("cuid");
        if (type != "inofinexportdata" || clientUniqueId == "")
        {
            Response.Write("Invalid");
            return;
        }
        string query = "select * from tbl_client where client_uniqueid=@uniqueid";
        Hashtable hstblp=new Hashtable();
        hstblp.Add("uniqueid",clientUniqueId);
        DataRow dr = DbTable.ExecuteSelectRow(query, hstblp);
        if (dr == null)
        {
            Response.Write("Invalid.");
            return;
        }
        try
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=\"InoFinExportData.csv");
            string fileActualPath = Server.MapPath("~/sharedfiles/InoFinExportData.csv");
            if (!System.IO.File.Exists(fileActualPath))
            {
                Response.Write("Invalid file");
                return;
            }
            response.TransmitFile(fileActualPath);
            response.Flush();
            response.End();
        }
        catch (Exception ex)
        {
        }
    }
}
