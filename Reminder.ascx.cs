using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class reminder : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindReminders();
    }
    private void BindReminders()
    {
        StringBuilder html = new StringBuilder();
        DataTable dttbl = new DataTable();
        string query="select * from tbl_followup where followup_date='2013-04-24' and followup_isreminder=1 "+
                        "and followup_userid=1 and isnull(followup_isremoved,0)=0";
        InsertUpdate obj = new InsertUpdate();
        dttbl = obj.ExecuteSelect(query);
        html.Append("<table class='reminder'>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            html.Append("<tr><td class='r-subject'>"+Convert.ToString(dttbl.Rows[i]["followup_subject"])+"</td>"+
                            "<td class='r-id'>"+Convert.ToString(dttbl.Rows[i]["followup_followupid"])+"</td>"+
                            "<td class='r-time'>"+Convert.ToString(dttbl.Rows[i]["followup_remindertime"])
                            + " " + Convert.ToString(dttbl.Rows[i]["followup_reminderampm"]) + "</td>" +
                       "</tr>");
        }
        html.Append("</table>");
        ltReminder.Text = html.ToString();
    }
}
