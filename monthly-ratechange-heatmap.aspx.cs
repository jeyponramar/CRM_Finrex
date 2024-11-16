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
using System.Data;
using System.Text;
using WebComponent;

public partial class monthly_ratechange_heatmap : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Finstation.CheckFullFinstationAccess();
        if (!IsPostBack)
        {
            txtfromyear.Text = Convert.ToString(DateTime.Now.Year - 10);
            txttoyear.Text = DateTime.Now.Year.ToString();
            string title = "";
            string currency = DbTable.GetOneColumnData("tbl_currencymaster", "currencymaster_currency", Currency);
            title = currency + " Monthly Heatmap %";
            lbltitle.Text = title;
            Page.Title = title;
            ltnote.Text = "<b>Positive %</b> means " + currency + " has gained over previous month and <b>Negative %</b> means " + currency + " has lost over previous month";
            BindData();
        }
    }
    private int Currency
    {
        get
        {
            return Common.GetQueryStringValue("currency");
        }
    }
    protected void btnSubmit_Click(object s, EventArgs e)
    {
        BindData();
    }
    private void BindData()
    {
        StringBuilder html = new StringBuilder();
        string query = "";
        query = "select * from tbl_month order by month_month";
        DataTable dttblm = DbTable.ExecuteSelect(query);
        int fromYear = GlobalUtilities.ConvertToInt(txtfromyear.Text);
        int toYear = GlobalUtilities.ConvertToInt(txttoyear.Text);
        query = "select * from tbl_monthlyratechange";
        query += " where monthlyratechange_currencymasterid=" + Currency;
        if (fromYear > 0) query += " and monthlyratechange_year>=" + fromYear;
        if (toYear > 0) query += " and monthlyratechange_year<=" + toYear;
        query += " order by monthlyratechange_year,monthlyratechange_monthid";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        html.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        html.Append("<tr class='grid-ui-header'><td>Month/Year</td>");
        for (int i = fromYear; i <= toYear; i++)
        {
            html.Append("<td style='min-width:50px;'>" + i + "</td>");
        }
        for (int i = 0; i < 12; i++)
        {
            int month = i + 1;
            html.Append("<tr><td class='bold'>" + GlobalUtilities.GetMonthName(month) + "</td>");
            for (int j = fromYear; j <= toYear; j++)
            {
                double rate = GetRateChange(dttbl, month, j);
                string tdcss = "heatmap-green3";//0-1
                if (rate > 2)
                {
                    tdcss = "heatmap-green1";
                }
                else if (rate > 1 && rate <= 2)
                {
                    tdcss = "heatmap-green2";
                }
                else if (rate < -2)
                {
                    tdcss = "heatmap-red1";
                }
                else if (rate < -1 && rate >= -2)
                {
                    tdcss = "heatmap-red2";
                }
                else if (rate < 0 && rate >= -1)
                {
                    tdcss = "heatmap-red3";
                }
                html.Append("<td class='" + tdcss + "'>" + GlobalUtilities.FormatAmount(rate) + "%</td>");
            }
            html.Append("</tr>");
        }
        html.Append("</tr>");
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
    private double GetRateChange(DataTable dttbl, int month, int year)
    {
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (GlobalUtilities.ConvertToInt(dttbl.Rows[i]["monthlyratechange_year"]) == year &&
                GlobalUtilities.ConvertToInt(dttbl.Rows[i]["monthlyratechange_monthid"]) == month)
            {
                return GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["monthlyratechange_changepercentage"]);
            }
        }
        return 0;
    }
}
