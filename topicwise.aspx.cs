using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.Text;

public partial class topicwise : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Custom.CheckSubscriptionAccess();
        Finstation.CheckFullFinstationAccess();
        if (!IsPostBack)
        {
            BindTopicWiseMenu();
        }
    }
    private void BindTopicWiseMenu()
    {
        StringBuilder html = new StringBuilder();
        string query = "select * from tbl_topicmenu";
        DataTable dttblmenu = DbTable.ExecuteSelect(query);
        html.Append("<ul class='topicwise-list'>");
        for (int i = 0; i < dttblmenu.Rows.Count; i++)
        {
            int menuid = GlobalUtilities.ConvertToInt(dttblmenu.Rows[i]["topicmenu_topicmenuid"]);
            string menu = GlobalUtilities.ConvertToString(dttblmenu.Rows[i]["topicmenu_menu"]);
            html.Append("<li>" + menu);
            query = "select * from tbl_topicsubmenu where topicsubmenu_topicmenuid=" + menuid;
            DataTable dttblsubmenu = DbTable.ExecuteSelect(query);
            if (dttblsubmenu.Rows.Count > 0)
            {
                html.Append("<ul>");
                for (int j = 0; j < dttblsubmenu.Rows.Count; j++)
                {
                    int submenuId = GlobalUtilities.ConvertToInt(dttblsubmenu.Rows[j]["topicsubmenu_topicsubmenuid"]);
                    string submenu = GlobalUtilities.ConvertToString(dttblsubmenu.Rows[j]["topicsubmenu_submenu"]);
                    html.Append("<li><a href='topic.aspx?smid=" + submenuId + "'>" + submenu + "</a></li>");
                }
                html.Append("</ul>");
            }
            html.Append("</li>");
        }
        html.Append("</ul>");
        ltMenu.Text = html.ToString();
    }
}
