using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class historical_forwardrate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Custom.CheckSubscriptionAccess();
        Finstation.CheckFullFinstationAccess();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ddlcurrency.SelectedIndex == 0)
        {
            ltdata.Text = "<span class='error'>Please select currency!</span>";
            return;
        }
        int currencyId = Convert.ToInt32(ddlcurrency.SelectedValue);
        StringBuilder html = new StringBuilder();
        FinstationPortal obj = new FinstationPortal();
        html.Append("<table width='100%'><tr><td class='bold'>Spot Rate:</td></tr>");
        html.Append("<tr><td>" + Finstation.GetHistoricalData(currencyId, txtdate.Text, txtdate.Text, EnumFinstationHistoryType.SpotRate) + "</td></tr>");
        html.Append("<tr><td class='bold'>Forward Rate:</td></tr>");
        html.Append("<tr><td>");
        html.Append("<table width='100%'>");
        if (currencyId == 2)//usdinr
        {
            ltspotrate.Text = Finstation.GetCurrency_History(30, txtdate.Text);

            html.Append("<tr><td>" + obj.GetRateHtml_History(5, "", "", txtdate.Text) + "</td></tr>");
            html.Append("<tr><td class='stitle'>USDINR Outright Rate</td></tr>");
            html.Append("<tr><td>" + obj.GetRateHtml_History(6, "", "", txtdate.Text) + "</td></tr>");
            html.Append("<tr><td class='stitle'>USDINR Annualised Premium %</td></tr>");
            html.Append("<tr><td>" + obj.GetRateHtml_History(7, "", "", txtdate.Text) + "</td></tr>");
        }
        else if (currencyId == 3)//eurinr
        {
            ltspotrate.Text = Finstation.GetCurrency_History(32, txtdate.Text);

            html.Append("<tr><td class='stitle'>EURINR Premium in Paisa</td></tr>");
            html.Append("<tr><td>" + obj.GetRateHtml_History(51, "", "", txtdate.Text) + "</td></tr>");
            html.Append("<tr><td class='stitle'>EURINR Outright Rate</td></tr>");
            html.Append("<tr><td>" + obj.GetRateHtml_History(52, "", "", txtdate.Text) + "</td></tr>");
            html.Append("<tr><td class='stitle'>EURINR Annualised Premium %</td></tr>");
            html.Append("<tr><td>" + obj.GetRateHtml_History(53, "", "", txtdate.Text) + "</td></tr>");
        }
        else if (currencyId == 4)//gbpinr
        {
            ltspotrate.Text = Finstation.GetCurrency_History(34, txtdate.Text);

            html.Append("<tr><td class='stitle'>GBPINR Premium in Paisa</td></tr>");
            html.Append("<tr><td>" + obj.GetRateHtml_History(54, "", "", txtdate.Text) + "</td></tr>");
            html.Append("<tr><td class='stitle'>GBPINR Outright Rate</td></tr>");
            html.Append("<tr><td>" + obj.GetRateHtml_History(55, "", "", txtdate.Text) + "</td></tr>");
            html.Append("<tr><td class='stitle'>GBPINR Annualised Premium %</td></tr>");
            html.Append("<tr><td>" + obj.GetRateHtml_History(56, "", "", txtdate.Text) + "</td></tr>");
        }
        html.Append("</table>");
        html.Append("</td></tr></table>");
        if (html.ToString().Contains("<div") || html.ToString().Contains("grid"))
        {
            ltdata.Text = html.ToString();
        }
        else
        {
            ltdata.Text = "No data found.";
        }
    }
    private string GetRateHtml(int sectionId, string columns)
    {
        string query = "select * from tbl_liveratesection where liveratesection_liveratesectionid=" + sectionId;
        DataRow drSection = DbTable.ExecuteSelectRow(query);
        string rows = GlobalUtilities.ConvertToString(drSection["liveratesection_rows"]);
        string cols = GlobalUtilities.ConvertToString(drSection["liveratesection_columns"]);
        string sectionCode = GlobalUtilities.ConvertToString(drSection["liveratesection_code"]);
        if (columns != "") cols = columns;

        Array arrCols = cols.Split(',');
        int colCount = arrCols.Length;
        int tempCount = 0;
        bool isColFound = true;
        if (Int32.TryParse(cols, out tempCount))
        {
            colCount = tempCount;
            isColFound = false;
        }
        StringBuilder html = new StringBuilder();
        html.Append("<table class='repeater' border=1 style='width:100%;height:100%;' cellpadding=5>");
        if (isColFound)
        {
            html.Append("<tr class='repeater-header'>");
            if (!columns.StartsWith("~"))
            {
                html.Append("<td>&nbsp;</td>");
            }

            int colLength = arrCols.Length;
            for (int i = 0; i < colLength; i++)
            {
                string name = arrCols.GetValue(i).ToString().Replace("~", "");
                html.Append("<td align='center'>" + name + "</td>");
            }
            html.Append("</tr>");
        }
        query = @"select * from tbl_dailyhistoricalliverate
                join tbl_liverate on liverate_liverateid=dailyhistoricalliverate_liverateid";
        query += " where liverate_liveratesectionid = " + sectionId;
        query += " AND cast(dailyhistoricalliverate_date as date)=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(txtdate.Text)) + "' as date)";
        query += " order by liverate_row";
        Array arrRows = rows.Split(',');
        DataTable dttbl = DbTable.ExecuteSelect(query);
        int rowCounter = 0;
        int prevRow = 0;
        int startIndex = 0;
        for (int i = 0; i < arrRows.Length; i++)
        {
            string liverateName = arrRows.GetValue(i).ToString();
            liverateName = liverateName.Replace(" ", "&nbsp;");

            for (int k = startIndex; k < dttbl.Rows.Count; k++)
            {
                int r = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_row"]);
                if (prevRow != r)
                {
                    rowCounter = r;
                    prevRow = r;
                    startIndex = k;
                    break;
                }
            }
            html.Append("<tr><td>" + liverateName + "</td>");
            for (int j = 0; j < colCount; j++)
            {
                int liverateId = 0;
                string currentRate = "";
                for (int k = startIndex; k < dttbl.Rows.Count; k++)
                {
                    if (GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_row"]) == rowCounter && //(i + 1) &&
                        GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_column"]) == (j + 1))
                    {
                        currentRate = GlobalUtilities.ConvertToString(dttbl.Rows[k]["dailyhistoricalliverate_currentrate"]);
                        liverateId = GlobalUtilities.ConvertToInt(dttbl.Rows[k]["liverate_liverateid"]);
                        break;
                    }
                }
                if (sectionId == 5 && liverateId == 181)
                {
                    html.Append("<td>CASH/SPOT</td>");
                }
                else
                {
                    html.Append("<td><div>" + currentRate + "</div></td>");
                }
            }
            html.Append("</tr>");
        }

        html.Append("</table>");

        return html.ToString();
    }
}
