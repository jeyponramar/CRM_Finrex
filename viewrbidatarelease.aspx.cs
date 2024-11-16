using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class viewrbidatarelease : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Custom.CheckSubscriptionAccess();
        Finstation.CheckFullFinstationAccess();
        if (!IsPostBack)
        {
            Bind();
        }
    }
    private void Bind()
    {
        string query = "";
        int id = Common.GetQueryStringValue("id");
        query = "select * from tbl_rbidatarelease order by rbidatarelease_referencedate desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        html.Append("<tr class='grid-ui-header'><td>Reference Date</td><td>Particular</td><td>Title</td><td>Download</td><td>Remark</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string css = "grid-ui-alt";
            if (i % 2 == 0) css = "grid-ui-row";
            string documenthref = "&nbsp;";
            string document = GlobalUtilities.ConvertToString(dttbl.Rows[i]["rbidatarelease_document"]);
            if (document != "")
            {
                documenthref = "<a href='download-file.aspx?f=rbidatarelease\\" + document + "' target='_blank'>Download</a>";
            }
            html.Append("<tr class='" + css + "'>");
            html.Append("<td>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["rbidatarelease_referencedate"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["rbidatarelease_particular"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["rbidatarelease_title"]) + "</td>");
            html.Append("<td>" + documenthref + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["rbidatarelease_remark"]) + "</td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
}
