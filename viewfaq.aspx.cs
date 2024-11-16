using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class viewfaq : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Custom.CheckSubscriptionAccess();
        Finstation.CheckFullFinstationAccess();
        if (!IsPostBack)
        {
            BindFAQ();
        }
    }
    private void BindFAQ()
    {
        string query = "";
        int id = Common.GetQueryStringValue("id");
        query = "select * from tbl_faq where faq_faqcategoryid=" + id;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='50%'>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string question = GlobalUtilities.ConvertToString(dttbl.Rows[i]["faq_question"]);
            string answer = GlobalUtilities.ConvertToString(dttbl.Rows[i]["faq_answer"]);
            html.Append("<tr class='faq-row'><td><div class='jq-faq'>" + question + "<span class='faq-arr faq-arr-down'></span></div><div class='faq-ans'>" + answer + "</div></td></tr>");
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
}
