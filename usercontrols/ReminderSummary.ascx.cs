using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class usercontrols_ReminderSummary : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindReminder();
        }
    }
    private void BindReminder()
    {
        CommonDAO obj = new CommonDAO();
        DataTable dttbl = obj.GetAllCounts();
        if (!GlobalUtilities.IsValidaTable(dttbl)) return;
        DataRow dr = dttbl.Rows[0];
        int AmcRemindersCount = GlobalUtilities.ConvertToInt(dr["AmcRemindersCount"]);
        lblAmcReminderCount.Text = AmcRemindersCount.ToString();
    }
}
