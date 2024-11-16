using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class CP_bulk_module_delete : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindModules();
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
    }
    private void BindModules()
    {
        DataTable dttbl = DbTable.ExecuteSelect("select * from tbl_module order by module_modulename");
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%' class='repeater'>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int mid = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["module_moduleid"]);
            string css = "repeater-alt";
            if (i % 2 == 0) css = "repeater-row";
            if (i % 4 == 0)
            {
                html.Append("<tr class='" + css + "'>");
            }
            html.Append("<td><input type='checkbox' name='chkd_" + mid + "'/>" + Convert.ToString(dttbl.Rows[i]["module_modulename"]) + "</td>");
            if ((i + 1) % 4 == 0)
            {
                html.Append("</tr>");
            }
        }
        html.Append("</table>");
        ltModules.Text = html.ToString();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < Request.Form.Keys.Count; i++)
        {
            string key = Request.Form.Keys[i];
            if (key.StartsWith("chkd_"))
            {
                Array arr = key.Split('_');
                int mid = GlobalUtilities.ConvertToInt(arr.GetValue(1));
                CP cp = new CP();
                cp.DeleteModule(mid);
            }
        }
        BindModules();
        lblMessage.Text = "Modules delete successfully";
    }
}
