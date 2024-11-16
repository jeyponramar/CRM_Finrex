using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WebComponent;

public partial class view_pushnotification : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindNotifications();
        }
    }
    private void BindNotifications()
    {
        FinrexPushNotification obj = new FinrexPushNotification();
        ltpushnotifications.Text = obj.GetAllNotifications(Common.GetQueryStringValue("id"), Enum_PushnotificationApplicationType.Finstation);
    }
}
