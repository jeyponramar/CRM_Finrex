using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.Text;

public partial class usercontrols_Dashboard_EnquiryStatus : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            iFrame.Attributes.Add("src", Common.BackSlashURL("../graph/leadstatus.aspx"));
            BindEnquiryStatus();
        }
    }
    private void BindEnquiryStatus()
    {
        string query = @"select COUNT(*) as totcount,min(enquirystatus_status) as statusName
                        ,MIN(enquirystatus_enquirystatusid) as enquirystatus_enquirystatusid
                         from tbl_enquirystatus
                        join tbl_enquiry on enquiry_enquirystatusid=enquirystatus_enquirystatusid
                        where 1=1 "+Common.GetViewRightsQuery("enquiry")+@"
                        group by enquirystatus_status
                        order by enquirystatus_enquirystatusid";
        DataTable dttb = DbTable.ExecuteSelect(query);
        if (GlobalUtilities.IsValidaTable(dttb))
        {
            StringBuilder html = new StringBuilder();
            for (int i = 0; i < dttb.Rows.Count; i++)
            {
                string status = Convert.ToString(dttb.Rows[i]["statusName"]);
                int count = GlobalUtilities.ConvertToInt(dttb.Rows[i]["totcount"]);
                bool isOddTdExists = false;
                if (i % 2 == 0)
                {
                    html.Append("<tr>");
                    html.Append("<td width='100'>" + status + "</td><td class='bold'>" + count.ToString() + "</td>");
                }
                else
                {
                    html.Append("<td width='100'>" + status + "</td><td class='bold'>" + count.ToString() + "</td>");
                    isOddTdExists = true;
                    html.Append("</tr>");
                }
                if (!isOddTdExists && i == dttb.Rows.Count)
                {
                    html.Append("<td>&nbsp;</td>");
                    html.Append("</tr>");
                }
            }
            ltEnquiryStatus.Text = html.ToString();
        }
    }

}
