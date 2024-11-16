using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class historical_data_interestrate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Custom.CheckSubscriptionAccess();
        Finstation.CheckFullFinstationAccess();
        if (!IsPostBack)
        {
            BindData(true);
        }
    }
    private void BindData(bool isactive)
    {
        string query = "";
        string fromDate = txtfromdate.Text;
        string toDate = txttodate.Text;
        query = @"select * from tbl_interestrate
                join tbl_interestratetype on interestratetype_interestratetypeid=interestrate_interestratetypeid
                where interestrate_interestratetypeid=" + Common.GetQueryStringValue("type");
        if (isactive) query += " and interestrate_isactive=1";
        if (fromDate != "") query += " AND cast(interestrate_date as date)>=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(fromDate)) + "' as date)";
        if (toDate != "") query += " AND cast(interestrate_date as date)<=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(toDate)) + "' as date)";

        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        html.Append("<tr class='grid-ui-header'><td>Type</td><td>Date</td><td>Particular / Tenor</td><td>Rate</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string css = "grid-ui-alt";
            if (i % 2 == 0) css = "grid-ui-row";
            html.Append("<tr class='" + css + "'>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["interestratetype_type"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["interestrate_date"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["interestrate_particular"]) + "</td>");
            html.Append("<td>" + String.Format("{0:0.0000}", GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["interestrate_rate"])) + "</td>");
            html.Append("</tr>");
        }
        html.Append("</tabl>");
        ltdata.Text = html.ToString();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindData(false);
    }
    
}
