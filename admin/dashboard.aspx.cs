using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.Collections;
using System.Text;

public partial class admin_dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindNotes();
    }
    private void BindNotes()
    {
        string query = "select top 10 * from tbl_note where note_createdby=" + CustomSession.Session("Login_UserId") + " order by note_noteid desc";
        InsertUpdate obj = new InsertUpdate();
        DataTable dttbl = obj.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string color = "";
            int priority = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["note_priorityid"]);
            if (priority == 1)
            {
                color = "blue";
            }
            else if (priority == 2)
            {
                color = "green";
            }
            else
            {
                color = "red";
            }
            string note = Convert.ToString(dttbl.Rows[i]["note_note"]);
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["note_noteid"]);
            string createdDatetime = GlobalUtilities.ConvertToDateTime(dttbl.Rows[i]["note_createddate"]);
            html.Append("<tr class='noterow'><td><table width='100%'><tr><td class='note_" + color + "'><table width='100%'><tr><td width='90%' class='note'>" + note
                                    + "</td><td rowspan='2'><img src='../images/icon/pin_" + color + ".png' class='delete-note hand' inner='1' title='Delete this note' nid='" +
                                    id + "'/></td></tr><tr><td colspan='2' align='right' class='stext'>" + createdDatetime + "</td></tr></table></td></tr></table></td></tr>");
        }
        ltNotes.Text = html.ToString();
    }
    
}
