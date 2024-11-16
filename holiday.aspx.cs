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
using System.Text;
using WebComponent;

public partial class holiday : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Custom.CheckSubscriptionAccess();
        Finstation.CheckFullFinstationAccess();
        if (!IsPostBack)
        {
            BindData();
        }
    }
    private void BindData()
    {
        StringBuilder html = new StringBuilder();
        string query = "select * from tbl_holiday where holiday_currencyid=" + GlobalUtilities.ConvertToInt(ddlcurrency.SelectedValue) +
                        " ORDER BY holiday_date";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        html.Append("<table class='tbl' width='500' border='1'><tr class='tbl-header'><td>Sr. No.</td><td>Date</td><td>Day</td><td>Description</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string css = "tbl-alt";
            if (i % 2 == 0) css = "tbl-row";
            string day = "";
            DateTime dt = Convert.ToDateTime(dttbl.Rows[i]["holiday_date"]);
            day = dt.DayOfWeek.ToString();

            html.Append("<tr class='" + css + "'><td>" + (i + 1) + "</td><td>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["holiday_date"]) + "</td>" +
                        "<td>" + day + "</td><td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["holiday_description"]) + "</td></tr>");
        }
        ltHoliday.Text = html.ToString();
    }
    protected void ddlcurrency_Changed(object sender, EventArgs e)
    {
        BindData();
    }
}
