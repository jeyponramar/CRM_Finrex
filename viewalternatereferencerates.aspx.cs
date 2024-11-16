using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class viewalternatereferencerates : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Custom.CheckSubscriptionAccess();
        Finstation.CheckFullFinstationAccess();
        if (!IsPostBack)
        {
            GlobalData gbldata = new GlobalData();
            gbldata.FillDropdown(ddlcurrency, "tbl_currency", "currency_currency", "");
            gbldata.FillDropdown(ddlarr, "tbl_arrmaster", "arrmaster_name", "");
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string query = @"select top 1000 * from tbl_alternativereferencerate
                        join tbl_arrmaster on arrmaster_arrmasterid=alternativereferencerate_arrmasterid
                        join tbl_currency on currency_currencyid=alternativereferencerate_currencyid";
        query += " where 1=1";
        if (Convert.ToInt32(ddlcurrency.SelectedValue) > 0) query += " and alternativereferencerate_currencyid=" + ddlcurrency.SelectedValue;
        if (Convert.ToInt32(ddlarr.SelectedValue) > 0) query += " and alternativereferencerate_arrmasterid=" + ddlarr.SelectedValue;

        if (txtfromdate.Text != "") query += " AND cast(alternativereferencerate_date as date)>=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(txtfromdate.Text)) + "' as date)";
        if (txttodate.Text != "") query += " AND cast(alternativereferencerate_date as date)<=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(txttodate.Text)) + "' as date)";
        query += " order by alternativereferencerate_date";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table class='grid-ui' cellpadding='7' border='1' style='font-size:14px;'>");
        html.Append(@"<tr class='grid-ui-header'><td>Currency</td><td>ARR</td><td>Date</td><td>O/n</td>
                <td>1Week</td><td>1-month TERM / Average</td><td>3-month TERM / Average</td><td>6-month TERM / Average</td><td>12-month TERM / Average</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string css = "grid-ui-alt";
            if (i % 2 == 0) css = "grid-ui-row";
            html.Append("<tr class='" + css + "'>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["currency_currency"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["arrmaster_name"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["alternativereferencerate_date"]) + "</td>");
            html.Append("<td>" + FormatData(dttbl.Rows[i]["alternativereferencerate_on"]) + "</td>");
            html.Append("<td>" + FormatData(dttbl.Rows[i]["alternativereferencerate_1week"]) + "</td>");
            html.Append("<td>" + FormatData(dttbl.Rows[i]["alternativereferencerate_1monthtermaverage"]) + "</td>");
            html.Append("<td>" + FormatData(dttbl.Rows[i]["alternativereferencerate_3monthtermaverage"]) + "</td>");
            html.Append("<td>" + FormatData(dttbl.Rows[i]["alternativereferencerate_6monthtermaverage"]) + "</td>");
            html.Append("<td>" + FormatData(dttbl.Rows[i]["alternativereferencerate_12monthtermaverage"]) + "</td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
    private string FormatData(object val)
    {
        if (GlobalUtilities.ConvertToDouble(val) == 0) return "-";
        string data = ExportExposurePortal.DecimalPoint(val, 4);
        return data;
    }
}
