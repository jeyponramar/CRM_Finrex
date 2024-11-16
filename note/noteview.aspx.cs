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
using System.Text;

public partial class note_noteview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindNotes();
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('My Notes')</script>");
    }
    private void BindNotes()
    {
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%' cellspacing=20>");
        string query = "select * from tbl_note LEFT JOIN tbl_priority ON priority_priorityid=note_priorityid " +
                     "where note_createdby = " + CustomSession.Session("Login_UserId") + " order by note_noteid desc";
        InsertUpdate obj = new InsertUpdate();
        DataTable dttbl = obj.ExecuteSelect(query);

        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int priority = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["note_priorityid"]);
            int noteid = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["note_noteid"]);
            string date = GlobalUtilities.ConvertToDateTime(dttbl.Rows[i]["note_createddate"]);
            string note = Convert.ToString(dttbl.Rows[i]["note_note"]);
            string strPriority = Convert.ToString(dttbl.Rows[i]["priority_priority"]);
            string priorityColor = "";
            if (priority == 1)
            {
                priorityColor = "blue";
            }
            else if (priority == 2)
            {
                priorityColor = "green";
            }
            else
            {
                priorityColor = "red";
            }
            string css = "note_" + priorityColor;// +" tilt-note";
            if (i % 3 == 0)
            {
                html.Append("<tr>");
            }
            html.Append("<td class='" + css + " noterow'><table width='100%'>" +
                        "<tr><td class='bold'>" + strPriority + "</td><td class='right'>" + date + "</td>"+
                              "<td><img src='../images/icon/pin_" + priorityColor + ".png' class='delete-note hand' nid='"+noteid+"' title='Delete this note' inner='1'/></td></tr>" +
                        "<tr><td colspan=3>"+note+"</td></tr><tr><td>&nbsp;</td></tr>"+
                        "</table></td>");

            if ((i+1) % 3 == 0 || i + 1 == dttbl.Rows.Count)
            {
                html.Append("</tr>");
            }
        }
        html.Append("</table>");
        ltMyNotes.Text = html.ToString();
    }
}
