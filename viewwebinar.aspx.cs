using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class viewwebinar : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Custom.CheckSubscriptionAccess();
        Finstation.CheckFullFinstationAccess();
        if (!IsPostBack)
        {
            BindWebinar();
        }
    }
    private void BindWebinar()
    {
        string query = "";
        query = "select * from tbl_webinar order by webinar_recordeddate desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%'>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["webinar_webinarid"]);
            string url = GlobalUtilities.ConvertToString(dttbl.Rows[i]["webinar_recordingurl"]);
            html.Append("<tr><td><div class='webinar-row'><table width='100%' cellpadding=8>");
            html.Append("<tr><td class='webinar-title'>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["webinar_title"]) + "</td></tr>");
            html.Append("<tr><td><span class='bold'>Presenters: </span>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["webinar_presenters"]) + "</td></tr>");
            html.Append("<tr><td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["webinar_description"]) + "</td></tr>");
            html.Append("<tr><td style='border-top:solid 1px #ddd;'><table width='100%'>");
            html.Append("<tr>");
            html.Append("<td class='bold'>Recorded: " + Convert.ToDateTime(dttbl.Rows[i]["webinar_recordeddate"]).ToLongDateString() + "</td>");
            if (url != "")
            {
                html.Append("<td align='right'><a href='" + url + "' target='_blank'>View Recording ></a></td>");
            }
            html.Append("</tr>");
            string attachment = GlobalUtilities.ConvertToString(dttbl.Rows[i]["webinar_attachment"]);
            if (attachment != "")
            {
                Array arr = attachment.Split('|');
                html.Append("<tr><td><table>");
                for (int j = 0; j < arr.Length; j++)
                {
                    string filePath = arr.GetValue(j).ToString();
                    string fileName = filePath.Split('_').GetValue(1).ToString();
                    html.Append("<tr><td><a href='download-file.aspx?f=webinar/" + filePath + "' target='_blank'>" + fileName + "</a></td></tr>");
                }
                html.Append("</table></td></tr>");
            }
            html.Append("</table></td></tr>");
            html.Append("</table></td></div></tr>");
            html.Append("<tr><td>&nbsp;</td></tr>");
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
}
