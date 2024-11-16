using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.Text;

public partial class commodity_metaldetails : System.Web.UI.Page
{
    int _metalId = 0;
    DataRow _drmetal;
    protected void Page_Load(object sender, EventArgs e)
    {
        _metalId = Common.GetQueryStringValue("id");
        _drmetal = DbTable.GetOneRow("tbl_metal", _metalId);
        if (_drmetal == null) return;
        hdnmetalid.Text = _metalId.ToString();
        if (!IsPostBack)
        {
            BindLiveRate();
            //BindTradingSummary();
            BindPriceGraph();
            BindMonth();
            //BindMonthSummary();
        }
    }
    private void BindLiveRate()
    {
        string query = "";
        StringBuilder html = new StringBuilder();
        query = "select top 1 * from tbl_MetalLiveRateHistory order by MetalLiveRateHistory_date desc";
        DataRow dr1 = DbTable.ExecuteSelectRow(query);
        if (dr1 == null) return;
        string sqldate = GlobalUtilities.ConvertToSqlDate(Convert.ToDateTime(dr1["MetalLiveRateHistory_date"]));
        query = "delete from tbl_MetalLiveRateHistory where cast(MetalLiveRateHistory_date as date)<cast('" + sqldate + @"' as date)";
        DbTable.ExecuteQuery(query);
//        query = @"select top 100 MetalLiveRateHistory_date as lbl,MetalLiveRateHistory_close as val from tbl_MetalLiveRateHistory 
//                    where MetalLiveRateHistory_metalid=" + _metalId + @" and cast(MetalLiveRateHistory_date as date)=cast('"+sqldate+@"' as date)
//                    order by MetalLiveRateHistory_MetalLiveRateHistoryid";
//        DataTable dttbl = DbTable.ExecuteSelect(query);

        query = "select * from tbl_MetalLiveRate where MetalLiveRate_metalid="+_metalId;
        DataRow dr = DbTable.ExecuteSelectRow(query);

        string metalName = GlobalUtilities.ConvertToString(_drmetal["metal_metalname"]);
        lblpagetitle.Text = metalName;
        lblmetalname.Text = metalName;
        if (dr == null) return;
        double bid = GlobalUtilities.ConvertToDouble(dr["MetalLiveRate_bid"]);
        double ask = GlobalUtilities.ConvertToDouble(dr["MetalLiveRate_ask"]);
        double open = GlobalUtilities.ConvertToDouble(dr["MetalLiveRate_open"]);
        double prevclose = GlobalUtilities.ConvertToDouble(dr["MetalLiveRate_prevclose"]);
        double high = GlobalUtilities.ConvertToDouble(dr["MetalLiveRate_high"]);
        double low = GlobalUtilities.ConvertToDouble(dr["MetalLiveRate_low"]);
        double ctweeklow = GlobalUtilities.ConvertToDouble(dr["MetalLiveRate_ctweeklow"]);
        double ctweekhigh = GlobalUtilities.ConvertToDouble(dr["MetalLiveRate_ctweekhigh"]); 
        double ctmonthlow = GlobalUtilities.ConvertToDouble(dr["MetalLiveRate_ctmonthlow"]);
        double ctmonthhigh = GlobalUtilities.ConvertToDouble(dr["MetalLiveRate_ctmonthhigh"]);
        double weeklow52 = GlobalUtilities.ConvertToDouble(dr["MetalLiveRate_fiftytwoweeklow"]);
        double weekhigh52 = GlobalUtilities.ConvertToDouble(dr["MetalLiveRate_fiftytwoweekhigh"]);
        string date = GlobalUtilities.ConvertToDateTime(dr["MetalLiveRate_date"]);

        html.Append("<table width='100%'>");
        html.Append("<tr><td colspan='2' class='bigtext'><span class='comm-liverate' metalid='" + _metalId + "' col='bid'>" + bid + "</span> / <span class='comm-liverate' metalid='" + _metalId + "' col='ask'>" + ask + "</span></td></tr>");
        html.Append("<tr><td metalid='" + _metalId + "' col='date'>" + date + "</td></tr>");
        html.Append("<tr><td class='line' colspan='2'><hr/></td></tr>");
        html.Append("<tr><td>O : <span metalid='" + _metalId + "' col='open'>" + open + "</span></td><td align='right'>C : <span metalid='" + _metalId + "' col='prevclose'>" + prevclose + "</span></td></tr>");
        html.Append("<tr><td>H : <span metalid='" + _metalId + "' col='high'>" + high + "</span></td><td align='right'>L : <span metalid='" + _metalId + "' col='low'>" + low + "</span></td></tr>");
        html.Append("<tr><td class='line' colspan='2'><hr/></td></tr>");
        //html.Append("<tr><td>Cur. Week High</td><td align='right' metalid='" + _metalId + "' col='ctweekhigh'>" + ctweekhigh + "</span></td></tr>");
        //html.Append("<tr><td>Cur. Week Low</td><td align='right' metalid='" + _metalId + "' col='ctweeklow'>" + ctweeklow + "</td></tr>");
        //html.Append("<tr><td colspan='2'>&nbsp;</td></tr>");
        //html.Append("<tr><td>Cur. Month High</td><td align='right' metalid='" + _metalId + "' col='ctmonthhigh'>" + ctmonthhigh + "</td></tr>");
        //html.Append("<tr><td>Cur. Month Low</td><td align='right' metalid='" + _metalId + "' col='ctmonthlow'>" + ctmonthlow + "</td></tr>");
        //html.Append("<tr><td colspan='2'>&nbsp;</td></tr>");
        //html.Append("<tr><td>52 Week High</td><td align='right' metalid='" + _metalId + "' col='fiftytwoweekhigh'>" + weekhigh52 + "</td></tr>");
        //html.Append("<tr><td>52 Week Low</td><td align='right' metalid='" + _metalId + "' col='fiftytwoweeklow'>" + weeklow52 + "</td></tr>");
        html.Append("</table>");

        ltliverate.Text = html.ToString();
    }
    private void BindPriceGraph()
    {
        DateTime dtfrom = DateTime.Now.AddMonths(-3);
        dtfrom = new DateTime(dtfrom.Year, dtfrom.Month, 1);
        DateTime dtto = DateTime.Now;
        txtpricegraphdatefrom.Text = GlobalUtilities.ConvertToDate(dtfrom);
        txtpricegraphdateto.Text = GlobalUtilities.ConvertToDate(dtto);
        //ltpricegraph.Text = com.GetPriceGraph(txtpricegraphdatefrom.Text, txtpricegraphdateto.Text, 1);
    }
  
    private void BindMonth()
    {
        string query = @"select month(MonthlyLMEMetalRate_date) as m,year(MonthlyLMEMetalRate_date) as yr from tbl_MonthlyLMEMetalRate
                          where year(MonthlyLMEMetalRate_date)=" + DateTime.Now.Year + " and MonthlyLMEMetalRate_metalid=" + _metalId +
                          " group by (MonthlyLMEMetalRate_date),year(MonthlyLMEMetalRate_date)";
        StringBuilder html = new StringBuilder();
        DataTable dttbl = DbTable.ExecuteSelect(query);
        html.Append("<select class='ddlmonth ddl'>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int m = Convert.ToInt32(dttbl.Rows[i]["m"]);
            int year = Convert.ToInt32(dttbl.Rows[i]["yr"]);
            string month = GlobalUtilities.GetMonthShortName(m) + " " + year;
            string selected = "";
            if (i == dttbl.Rows.Count - 1) selected = " selected";
            string val = m + "-" + year;
            html.Append("<option value='" + val + "' " + selected + ">" + month + "</option>");
        }
        html.Append("</select>");
        ltmonth.Text = html.ToString();
    }

}
