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
using System.Data;
using System.Text;
public partial class Viewfincategory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }

    }
    private void BindData()
    {
        string query = "";
        int clientId = Common.ClientId;
        query = "select * from tbl_findoccategory where findoccategory_clientid=" + clientId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%' cellpadding=7  border=1 class='grid-ui'>");
        html.Append("<tr class='grid-ui-header'><td>Category Name</td><td>Edit</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["findoccategory_findoccategoryid"]);

            html.Append("<tr>");
            html.Append("<td>" +GlobalUtilities.ConvertToString(dttbl.Rows[i]["findoccategory_categoryname"]) + "</td>");
            html.Append("<td><a href='addfincategory.aspx?id=" + id + "'>Detail</a></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
}
