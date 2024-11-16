using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;
using System.IO;

public partial class view_documents : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGridData();
            string title = "";
            if (Id == 1)
            {
                title = "SBI Card Rate";
            }
            else if (Id == 2)
            {
                title = "Media";
            }
            if (Id == 1)
            {
            }
            else
            {
                Finstation.CheckFullFinstationAccess();
            }
            lbltitle.Text = title;
            Page.Title = title;
        }
    }
    private int Id
    {
        get
        {
            return Common.GetQueryStringValue("id");
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindGridData();
    }
    private void BindGridData()
    {

        string query = "";
        query = @"select * from tbl_document where document_finstationdocumenttypeid=" + Id;
        if(txtfromdate.Text!="") query += " AND cast(document_date as date)>=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(txtfromdate.Text)) + "' as date)";
        if(txttodate.Text!="")query += " AND cast(document_date as date)>=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(txttodate.Text)) + "' as date)";
        query += " order by document_date desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);

        StringBuilder html = new StringBuilder();
        html.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        html.Append("<tr class='grid-ui-header'>");
        html.Append("<td>Date</td><td>Particular</td><td>URL</td><td>Remark</td><td>Document</td>");
        html.Append("</tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["document_documentid"]);
            string particular = GlobalUtilities.ConvertToString(dttbl.Rows[i]["document_particular"]);
            string remark = GlobalUtilities.ConvertToString(dttbl.Rows[i]["document_remark"]);
            string url = GlobalUtilities.ConvertToString(dttbl.Rows[i]["document_url"]);
            string ext = GlobalUtilities.ConvertToString(dttbl.Rows[i]["document_document"]);
            string css = "grid-ui-alt";
            if (i % 2 == 0) css = "grid-ui-row";
            html.Append("<tr class='" + css + "'>");
            html.Append("<td>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["document_date"]) + "</td>");
            string url1 = url;
            string file = "&nbsp;";
            
            //if (url == "")
            //{
                html.Append("<td>" + particular + "</td>");
            //}
            //else
            //{
                if (url.StartsWith("http://") || url.StartsWith("https://"))
                {
                }
                else
                {
                    if (!url.StartsWith("http://")) url = "http://" + url;
                }
                html.Append("<td><a href='" + url + "' target='_blank'>" + url1 + "</a></td>");
            //}
            html.Append("<td>" + remark + "</td>");
            if (ext != "")
            {
                if (File.Exists(Server.MapPath("~/upload/documents/" + id + "." + ext)))
                {
                    file = "<a href='upload/documents/" + id + "." + ext + "' target='_blank'>Download</a>";
                }
            }
            html.Append("<td>" + file + "</td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
}
