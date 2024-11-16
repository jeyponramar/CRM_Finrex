using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using WebComponent;
using System.Data;

public partial class topic : System.Web.UI.Page
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
        int submenuId = Common.GetQueryStringValue("smid");
        string query = "select * from tbl_topicsubmenu where topicsubmenu_topicsubmenuid="+submenuId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        string title = GlobalUtilities.ConvertToString(dr["topicsubmenu_submenu"]);
        string message = GlobalUtilities.ConvertToString(dr["topicsubmenu_message"]);
        lbltitle.Text = title;
        this.Title = title;
        lblmessage.Text = message;
    }
}
