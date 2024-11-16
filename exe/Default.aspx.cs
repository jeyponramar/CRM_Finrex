using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using WebComponent;
using System.Collections;
using System.Text;

public partial class exe_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hdnsessionid.Text = Common.GetQueryString("sid");
            lnksetalert.NavigateUrl = "setliveratealert.aspx?sid=" + hdnsessionid.Text;
            //ErrorLog.WriteLog("exe session:" + hdnsessionid.Text);
            BindData();
        }
    }
    private string SessionId
    {
        get
        {
            return Common.GetQueryString("sid");
        }
    }
    private int GetClientUserId()
    {
        string query = "select * from tbl_clientuser WHERE clientuser_isactive=1 and clientuser_exesessionid=@sessionid";//<1 min heart beat
        Hashtable hstbl = new Hashtable();
        hstbl.Add("sessionid", hdnsessionid.Text);
        DataRow dr = DbTable.ExecuteSelectRow(query, hstbl);
        if (dr == null) return 0;
        return GlobalUtilities.ConvertToInt(dr["clientuser_clientuserid"]);
    }
    private void BindData()
    {
        string query = "";
        int clientUserId = GetClientUserId();
        Finstation objfinstation = new Finstation();
        string includeCurrencies = objfinstation.GetUserConfigLiverateCurrenciesOnly(Enum_AppType.Finwatch, 0, clientUserId);
        string includeColumns = objfinstation.GetUserConfigLiverateCurrencyColumns(Enum_AppType.Finwatch, clientUserId);
        string strhtml = "";
        string imgsetting = "<a href='config-currency.aspx?sid=" + SessionId + "'><img width='15' src='images/plus-add-white.png'/></a>";
        Array arrcolumns = includeColumns.Split(',');
        string headerCols = "";
        bool isBidSelected = false;
        for (int i = 0; i < arrcolumns.Length; i++)
        {
            Enum_CurrencyLiveRateColumn col = (Enum_CurrencyLiveRateColumn)Convert.ToInt32(arrcolumns.GetValue(i));
            string colName = "";
            if (col == Enum_CurrencyLiveRateColumn.Bid)
            {
                colName = "Bid";
                isBidSelected = true;
            }
            else if (col == Enum_CurrencyLiveRateColumn.Ask)
            {
                colName = "Ask";
            }
            else if (col == Enum_CurrencyLiveRateColumn.PercentageChg)
            {
                colName = "%Chg";
            }
            else if (col == Enum_CurrencyLiveRateColumn.Open)
            {
                colName = "Open";
            }
            else if (col == Enum_CurrencyLiveRateColumn.High)
            {
                colName = "High";
            }
            else if (col == Enum_CurrencyLiveRateColumn.Low)
            {
                colName = "Low";
            }
            headerCols += "<td>" + colName + "</td>";
        }
//        if (includeCurrencies == "")
//        {
//            query = @"select liverate_liverateid as rid,liverate_currentrate as cr from tbl_liverate 
//                  where liverate_liverateid IN(1,4,5,10,13,14,19,22,23,37,40,41,46,49,50)";
//            DataTable dttbl = DbTable.ExecuteSelect(query);
//            strhtml = @"<table width='100%' cellpadding='0' cellspacing=0>
//                            <tr class='lr-header'><td>"+imgsetting+@"Currency</td><td>Price</td><td>High</td><td>Low</td></tr>
//                            <tr><td class='lr-title'>USDINR</td>" + GetTdHtml(dttbl, 1) + GetTdHtml(dttbl, 4) + GetTdHtml(dttbl, 5) + @"</tr>
//                            <tr><td class='lr-title'>EURINR</td>" + GetTdHtml(dttbl, 10) + GetTdHtml(dttbl, 13) + GetTdHtml(dttbl, 14) + @"</tr>
//                            <tr><td class='lr-title'>GBPINR</td>" + GetTdHtml(dttbl, 19) + GetTdHtml(dttbl, 22) + GetTdHtml(dttbl, 23) + @"</tr>
//                            <tr><td class='lr-title'>EURUSD</td>" + GetTdHtml(dttbl, 37) + GetTdHtml(dttbl, 40) + GetTdHtml(dttbl, 41) + @"</tr>
//                            <tr><td class='lr-title'>GBPUSD</td>" + GetTdHtml(dttbl, 46) + GetTdHtml(dttbl, 49) + GetTdHtml(dttbl, 50) + @"</tr>
//                        </table>";
//        }
//        else
        {
            string sectionIds = "1,2,3,50,4,59,60,61,62";
            Array arrsectionIds = sectionIds.Split(',');
            StringBuilder html = new StringBuilder();
            html.Append(@"<table width='100%' cellpadding='0' cellspacing=0>
                            <tr class='lr-header'><td>");
//            html.Append(@"<div style='float:left;position: relative;' class='jq-push-notify-panel'>
//                                <i class='icon ion-ios-bell header-bell jq-header-bell'></i>");
//            html.Append(@"<div class='push-notify-msg-count'></div>
//                         <div class='push-notify-msg-list'></div>");
//            html.Append("</div>");
            html.Append(imgsetting + @" Currency</td>" + headerCols + "</tr>");
            
            for (int s = 0; s < arrsectionIds.Length; s++)
            {
                int currencyTypeId = 0;
                int currentSectionId = Convert.ToInt32(arrsectionIds.GetValue(s));
                currencyTypeId = Finstation.GetCurrencyTypeBySectionId(currentSectionId);
                includeCurrencies = objfinstation.GetUserConfigLiverateCurrenciesOnly(Enum_AppType.Finwatch, currencyTypeId, clientUserId);
                if (includeCurrencies == "") continue;
                
                query = @"select liverate_row,liverate_column,liverate_currentrate,liverate_prevrate,liverate_liverateid,liverate_rtdcode,
                liverate_calculation,liverate_istick,liverate_liveratesectionid,liverate_decimalplaces,
                currencymaster_currencymasterid,currencymaster_currency,currencytype_currencytypeid
                from tbl_liverate
                join tbl_currencymaster on currencymaster_currencymasterid=liverate_currencymasterid
                join tbl_currencytype on currencytype_currencytypeid=currencymaster_currencytypeid
                WHERE liverate_liveratesectionid =" + currentSectionId + " and currencymaster_currencymasterid in(" + includeCurrencies + ")";
                query += " order by liverate_liveratesectionid,liverate_row,liverate_column";
                //ErrorLog.WriteLog(query);
                DataTable dttbl = DbTable.ExecuteSelect(query);
                dttbl = Finstation.CorrectLiveRateValues(dttbl);
               
                int prevRowId = 0;
                int prevSectionId = 0;
                for (int i = 0; i < dttbl.Rows.Count; i++)
                {
                    int row = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liverate_row"]);
                    int sectionId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liverate_liveratesectionid"]);
                    if (row != prevRowId || sectionId != prevSectionId)
                    {
                        html.Append("<tr>");
                        string currency = GlobalUtilities.ConvertToString(dttbl.Rows[i]["currencymaster_currency"]);
                        if (sectionId == 50)
                        {
                            html.Append("<td class='lr-title' title='OFFSHORE'>" + currency + " (O)</td>");
                        }
                        else
                        {
                            html.Append("<td class='lr-title'>" + currency + "</td>");
                        }
                        int bidLiveRateId = 0;
                        string bidLiveRate = "";
                        for (int j = 0; j < dttbl.Rows.Count; j++)
                        {
                            int row2 = GlobalUtilities.ConvertToInt(dttbl.Rows[j]["liverate_row"]);
                            int sectionId2 = GlobalUtilities.ConvertToInt(dttbl.Rows[j]["liverate_liveratesectionid"]);
                            int column = GlobalUtilities.ConvertToInt(dttbl.Rows[j]["liverate_column"]);
                            int lid = GlobalUtilities.ConvertToInt(dttbl.Rows[j]["liverate_liverateid"]);
                            string liveRate = GlobalUtilities.ConvertToString(dttbl.Rows[j]["liverate_currentrate"]);
                            if (sectionId == 50)
                            {
                            }
                            if (sectionId == sectionId2 && row == row2)
                            {
                                bool isvalidcolumn = false;
                                Enum_CurrencyLiveRateColumn currentColumn = Enum_CurrencyLiveRateColumn.None;
                                if ((Enum_CurrencyType)currencyTypeId == Enum_CurrencyType.GlobalIndicesFutures
                                    || (Enum_CurrencyType)currencyTypeId == Enum_CurrencyType.GovernmentBondYield)
                                {
                                    if (column == 2)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Bid;
                                        bidLiveRateId = lid;
                                        bidLiveRate = liveRate;
                                    }
                                    else if (column == 3)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Ask;
                                        if (isBidSelected)
                                        {
                                            liveRate = "-";
                                        }
                                        else//if no Bid selected then take Bid value
                                        {
                                            lid = bidLiveRateId;
                                            liveRate = bidLiveRate;
                                        }
                                    }
                                    else if (column == 4)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.PercentageChg;
                                    }
                                    else if (column == 5)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Open;
                                    }
                                    else if (column == 6)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.High;
                                    }
                                    else if (column == 7)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Low;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else if (sectionId == 59 || sectionId == 60)//Commodity entries in Indices and Commodities
                                //sectionId == 60 for India Vix, Bank Nifty
                                {
                                    if (column == 2)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Bid;
                                        bidLiveRateId = lid;
                                        bidLiveRate = liveRate;
                                    }
                                    else if (column == 3)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Ask;
                                        if (isBidSelected)
                                        {
                                            liveRate = "-";
                                        }
                                        else//if no Bid selected then take Bid value
                                        {
                                            lid = bidLiveRateId;
                                            liveRate = bidLiveRate;
                                        }
                                    }
                                    else if (column == 4)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.PercentageChg;
                                    }
                                    else if (column == 5)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Open;
                                    }
                                    else if (column == 6)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.High;
                                    }
                                    else if (column == 7)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Low;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else if ((Enum_CurrencyType)currencyTypeId == Enum_CurrencyType.IndicesAndCommodities)
                                {
                                    if (column == 1)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Bid;
                                        bidLiveRateId = lid;
                                        bidLiveRate = liveRate;
                                    }
                                    else if (column == 2)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Ask;
                                        if (isBidSelected)
                                        {
                                            liveRate = "-";
                                        }
                                        else//if no Bid selected then take Bid value
                                        {
                                            lid = bidLiveRateId;
                                            liveRate = bidLiveRate;
                                        }
                                    }
                                    else if (column == 3)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.PercentageChg;
                                    }
                                    else if (column == 4)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Open;
                                    }
                                    else if (column == 5)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.High;
                                    }
                                    else if (column == 6)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Low;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else if (sectionId == 50 && lid >= 1214 && lid <= 1220)//Offshore USDINR
                                {
                                    if (column == 1)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Bid;
                                    }
                                    else if (column == 2)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Ask;
                                    }
                                    else if (column == 3)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.PercentageChg;
                                    }
                                    else if (column == 4)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Open;
                                    }
                                    else if (column == 5)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.High;
                                    }
                                    else if (column == 6)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Low;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                    
                                }
                                else // spot rate
                                {
                                    if (column == 1)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Bid;
                                    }
                                    else if (column == 2)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Ask;
                                    }
                                    else if (column == 3)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.PercentageChg;
                                    }
                                    else if (column == 6)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Open;
                                    }
                                    else if (column == 4)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.High;
                                    }
                                    else if (column == 5)
                                    {
                                        currentColumn = Enum_CurrencyLiveRateColumn.Low;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                    
                                }
                                for (int k = 0; k < arrcolumns.Length; k++)
                                {
                                    if ((Enum_CurrencyLiveRateColumn)Convert.ToInt32(arrcolumns.GetValue(k)) == currentColumn)
                                    {
                                        isvalidcolumn = true;
                                        break;
                                    }
                                }
                                if (isvalidcolumn)
                                {
                                    if (liveRate == "-")
                                    {
                                        html.Append("<td align='center'>-</td>");
                                    }
                                    else
                                    {
                                        html.Append("<td><div class='lr-rate' id='lr" + lid + "' lid='" + lid + "' sid='"+sectionId2+"'>" + liveRate + "</div></td>");
                                    }
                                }
                            }
                        }
                        html.Append("<tr>");
                    }
                    prevRowId = row;
                    prevSectionId = sectionId;
                }
            }
            html.Append("</table>");
            strhtml = html.ToString();
        }
        ltliverate.Text = strhtml;
    }
    private string GetLiverateRow(DataTable dttbl)
    {
        return "";
    }
    private string GetTdHtml(DataTable dttbl, int liverateId)
    {
        double liveRate = 0; double prevRate = 0;
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (GlobalUtilities.ConvertToInt(dttbl.Rows[i]["rid"]) == liverateId)
            {
                liveRate = GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["cr"]);
                break;
            }
        }
        string html = "<td><div class='lr-rate' id='lr" + liverateId + "' lid='"+liverateId+"'>" + liveRate + "</div></td>";
        return html;
    }
    public string VersionNo
    {
        get
        {
            return ConfigurationManager.AppSettings["VersionNo"].ToString();
        }
    }
}
