using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data;
using WebComponent;
using System.Collections;
using System.Text;

/// <summary>
/// Summary description for MobileApp_PhoneGap
/// </summary>
public class MobileApp_PhoneGap
{
    int _clientId = 0;
    int _clientUserId = 0;
    DataTable _dttblMetal = new DataTable();

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string m = Common.GetQueryString("m");
            string action = Common.GetQueryString("action");
            string query = "";
            string error = ""; string data = ""; string redirect = ""; string msg = "";
            if (m != "login")
            {
                string sessionId = Common.GetQueryString("sessionid");
                query = "select * from tbl_clientuser where clientuser_isactive=1 and clientuser_mobilesessionid=@sessionid";
                Hashtable hstbl = new Hashtable();
                hstbl.Add("sessionid", sessionId);
                DataRow drclientuser = DbTable.ExecuteSelectRow(query, hstbl);
                if (drclientuser != null)
                {
                    _clientId = GlobalUtilities.ConvertToInt(drclientuser["clientuser_clientid"]);
                    _clientUserId = GlobalUtilities.ConvertToInt(drclientuser["clientuser_clientuserid"]);
                }
                if (m == "checkloginstatus")
                {
                    if (drclientuser != null)
                    {
                        HttpContext.Current.Response.Write("ok");
                    }
                    return;
                }
            }
            if (m == "login")
            {
                string username = global.CheckInputData(context.Request["username"]);
                string password = global.CheckInputData(context.Request["password"]);

                query = "select * from tbl_clientuser where clientuser_isactive=1 and clientuser_ismobileuser=1 and " +
                        "clientuser_username='" + username + "' and clientuser_password='" + password + "'";
                DataRow dr = DbTable.ExecuteSelectRow(query);
                if (dr == null)
                {
                    error = "Invalid User Name / Password";
                }
                else
                {
                    if (IsClientActive(GlobalUtilities.ConvertToInt(dr["clientuser_clientid"])))
                    {
                        redirect = "dashboard.html,FinIcon";
                        int clientUserId = GlobalUtilities.ConvertToInt(dr["clientuser_clientuserid"]);
                        string newsession = Guid.NewGuid().ToString();
                        query = "update tbl_clientuser set clientuser_mobilesessionid='" + newsession + "' where clientuser_clientuserid=" + clientUserId;
                        DbTable.ExecuteQuery(query);
                        data = "\"sessionid\":\"" + newsession + "\"";
                        //data = json.Replace("\n", "__NEWLINE__").Replace("\r", "__NEWLINER__");
                    }
                    else
                    {
                        error = "Your subscription has expired!";
                    }
                }
            }
            else if (m == "setalert")
            {
                if (action == "save")
                {
                    int currencyId = GlobalUtilities.ConvertToInt(context.Request["ddlcurrencymasterid"]);
                    int covertypeId = GlobalUtilities.ConvertToInt(context.Request["ddlcovertypeid"]);
                    double target = GlobalUtilities.ConvertToDouble(context.Request["txttarget"]);
                    double stopLoss = GlobalUtilities.ConvertToDouble(context.Request["txtstoploss"]);
                    string expirydate = context.Request["txtexpirydate"];
                    string emailId = context.Request["txtemailid"];
                    string mobileNo = context.Request["txtmobileno"];
                    //ErrorLog.WriteLog("currencyId=" + currencyId+Environment.NewLine);
                    //for (int i = 0; i < context.Request.Form.Keys.Count; i++)
                    //{
                    //    ErrorLog.WriteLog("F_"+context.Request.Form.Keys[i] + "=" + context.Request[context.Request.Form.Keys[i]]+Environment.NewLine);
                    //}
                    //for (int i = 0; i < context.Request.QueryString.Keys.Count; i++)
                    //{
                    //    ErrorLog.WriteLog("QS_"+context.Request.QueryString.Keys[i] + "=" + context.Request.QueryString[i] + Environment.NewLine);
                    //}
                    int liveRateId = 0;

                    if (covertypeId == 1)//import
                    {
                        if (currencyId == 1)
                        {
                            liveRateId = 2;
                        }
                        else if (currencyId == 2)
                        {
                            liveRateId = 11;
                        }
                        else if (currencyId == 3)
                        {
                            liveRateId = 20;
                        }
                        else if (currencyId == 4)
                        {
                            liveRateId = 29;
                        }
                    }
                    else//export
                    {
                        if (currencyId == 1)
                        {
                            liveRateId = 1;
                        }
                        else if (currencyId == 2)
                        {
                            liveRateId = 10;
                        }
                        else if (currencyId == 3)
                        {
                            liveRateId = 19;
                        }
                        else if (currencyId == 4)
                        {
                            liveRateId = 28;
                        }
                    }

                    Hashtable hstbl = new Hashtable();
                    hstbl.Add("clientid", _clientId);
                    hstbl.Add("clientuserid", _clientUserId);
                    hstbl.Add("currencymasterid", currencyId);
                    hstbl.Add("covertypeid", covertypeId);
                    hstbl.Add("target", target);
                    hstbl.Add("stoploss", stopLoss);
                    hstbl.Add("expirydate", expirydate);
                    hstbl.Add("emailid", emailId);
                    hstbl.Add("mobileno", mobileNo);
                    hstbl.Add("liverateid", liveRateId);
                    InsertUpdate obj = new InsertUpdate();
                    int alertId = obj.InsertData(hstbl, "tbl_liveratealert");
                    if (alertId > 0)
                    {
                        data = "\"alertid\":\"" + alertId + "\"";
                        msg = "Live rate alert has been set successfully!";
                    }
                    else
                    {
                        error = "Error occurred, please try again!";
                    }
                }
            }
            else if (m == "bindalertemailmobile")
            {
                data = GetAlertContacts();
            }
            else if (m == "getliverateuserconfig")
            {
                Finstation obj = new Finstation();
                string html = obj.GetLiverateUserConfig(_clientUserId, Enum_AppType.FinIcon, 0);
                HttpContext.Current.Response.Write(html);
                return;
            }
            else if (m == "saveliverateuserconfig")
            {
                Finstation obj = new Finstation();
                string currencies = Common.GetQueryString("c");
                obj.SaveLiverateUserConfig(_clientUserId, (int)Enum_AppType.FinIcon, currencies);
                HttpContext.Current.Response.Write("ok");
                return;
            }
            else if (m == "getrate")
            {
                if (Common.GetQueryString("ratetype") == "spot")
                {
                    string html = BindSpotrate(_clientUserId);
                    HttpContext.Current.Response.Write(html);
                    return;
                }
            }
            else if (m == "pushnotification")
            {
                FinrexPushNotification obj = new FinrexPushNotification();
                obj.ProcessRequest();
                return;
            }
            string response = "{\"error\":\"" + error + "\",\"data\":{" + data + "},\"redirect\":\"" + redirect + "\",\"msg\":\"" + msg + "\"}";
            HttpContext.Current.Response.Write(response);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("Error: unable to load.");
        }
    }
    public bool IsClientActive(int clientId)
    {
        string query = "";
        query = "select * from tbl_client where client_clientid=" + clientId;
        DataRow drClient = DbTable.ExecuteSelectRow(query);
        int subscriptionStatus = 0;
        if (drClient != null)
        {
            subscriptionStatus = GlobalUtilities.ConvertToInt(drClient["client_subscriptionstatusid"]);
            if (subscriptionStatus == 1 || subscriptionStatus == 2)//1-trial,2-subscribed
            {
                if (Convert.ToDateTime(drClient["client_enddate"]) < DateTime.Today)
                {
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }
    private string GetAlertContacts()
    {
        int clientId = _clientId;
        string query = "select * from tbl_contacts where contacts_clientid > 0 and contacts_clientid=" + clientId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder emailhtml = new StringBuilder();
        StringBuilder mobilehtml = new StringBuilder();
        emailhtml.Append("<table class='tblemailids'>");
        mobilehtml.Append("<table class='tblmobilenos'>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string emailId = GlobalUtilities.ConvertToString(dttbl.Rows[i]["contacts_emailid"]);
            string mobileNo = GlobalUtilities.ConvertToString(dttbl.Rows[i]["contacts_mobileno"]);
            string contactPerson = GlobalUtilities.ConvertToString(dttbl.Rows[i]["contacts_contactperson"]);
            int contactId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["contacts_contactsid"]);
            string strchecked = "";
            //Array arr = txtemailid.Text.Split(',');
            //for (int j = 0; j < arr.Length; j++)
            //{
            //    if (GlobalUtilities.ConvertToInt(arr.GetValue(j)) == contactId)
            //    {
            //        strchecked = "checked='checked'"; break;
            //    }
            //}
            if (emailId.Trim() != "")
            {
                emailhtml.Append("<tr><td width='20'><input type='checkbox' class='chktwoselect' value='" + contactId + "' " + strchecked + "/></td>" +
                                      "<td>" + contactPerson + " (" + emailId + ")</td></tr>");
            }
            strchecked = "";
            //arr = txtmobileno.Text.Split(',');
            //for (int j = 0; j < arr.Length; j++)
            //{
            //    if (GlobalUtilities.ConvertToInt(arr.GetValue(j)) == contactId)
            //    {
            //        strchecked = "checked='checked'"; break;
            //    }
            //}
            if (mobileNo != "")
            {
                mobilehtml.Append("<tr><td width='20'><input type='checkbox' class='chktwoselect' value='" + contactId + "' " + strchecked + "/></td>" +
                                      "<td>" + contactPerson + " (" + mobileNo + ")</td></tr>");
            }
        }
        emailhtml.Append("</table>");
        mobilehtml.Append("</table>");

        string data = "\"emailhtml\":\"" + emailhtml.ToString() + "\",\"mobilehtml\":\"" + mobilehtml.ToString() + "\"";
        return data;
    }
    public string BindSpotrate(int clientUserId)
    {
        string query = "";
        Finstation objfinstation = new Finstation();
        string includeCurrencies = objfinstation.GetUserConfigLiverateCurrenciesOnly(Enum_AppType.FinIcon, 0, clientUserId);
        if (includeCurrencies == "")
        {
            return BindSpotrateBasic();
        }
        else
        {
            Array arrsectionIds = "1,2,3,4,59,60,61,62".Split(',');
            StringBuilder html = new StringBuilder();
            StringBuilder html_offshore = new StringBuilder();
            html.Append(@"<table width='150%' cellpadding='0' cellspacing=0>
                            <tr class='lr-header'><td>&nbsp;</td><td>Bid (Export)</td><td>Ask (Import)</td><td>%Chg</td><td>High</td><td>Low</td></tr>");
            for (int s = 0; s < arrsectionIds.Length; s++)
            {
                int currencyTypeId = 0;
                int currentSectionId = Convert.ToInt32(arrsectionIds.GetValue(s));
                currencyTypeId = Finstation.GetCurrencyTypeBySectionId(currentSectionId);
                includeCurrencies = objfinstation.GetUserConfigLiverateCurrenciesOnly(Enum_AppType.FinIcon, currencyTypeId, clientUserId);
                if (includeCurrencies == "") continue;

                query = @"select liverate_row,liverate_column,liverate_currentrate,liverate_prevrate,liverate_liverateid,liverate_rtdcode,
                liverate_calculation,liverate_istick,liverate_liveratesectionid,liverate_decimalplaces,
                currencymaster_currencymasterid,currencymaster_currency,currencytype_currencytypeid
                from tbl_liverate
                join tbl_currencymaster on currencymaster_currencymasterid=liverate_currencymasterid
                join tbl_currencytype on currencytype_currencytypeid=currencymaster_currencytypeid
                WHERE liverate_liveratesectionid =" + currentSectionId + " and currencymaster_currencymasterid in(" + includeCurrencies + ")";
                query += " order by liverate_liveratesectionid,liverate_row,liverate_column";
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
                        html.Append("<td class='lr-title'>" + currency + "</td>");
                        for (int j = 0; j < dttbl.Rows.Count; j++)
                        {
                            int row2 = GlobalUtilities.ConvertToInt(dttbl.Rows[j]["liverate_row"]);
                            int sectionId2 = GlobalUtilities.ConvertToInt(dttbl.Rows[j]["liverate_liveratesectionid"]);
                            int column = GlobalUtilities.ConvertToInt(dttbl.Rows[j]["liverate_column"]);
                            string liveRate = GlobalUtilities.ConvertToString(dttbl.Rows[j]["liverate_currentrate"]);
                            int lid = GlobalUtilities.ConvertToInt(dttbl.Rows[j]["liverate_liverateid"]);
                            if (lid == 1506)
                            {
                                lid = lid;
                            }
                            if (sectionId == sectionId2 && row == row2)
                            {
                                bool isvalidcolumn = false;
                                if ((Enum_CurrencyType)currencyTypeId == Enum_CurrencyType.GlobalIndicesFutures)
                                {
                                    if (column == 2 || column == 3 || column == 4 || column == 6 || column == 7)
                                    {
                                        isvalidcolumn = true;
                                        if (column == 3)
                                        {
                                            liveRate = "-";
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else if (sectionId == 59)//Commodity entries in Indices and Commodities
                                {
                                    if (column == 2 || column == 3 || column == 4 || column == 6 || column == 7)
                                    {
                                        isvalidcolumn = true;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else if ((Enum_CurrencyType)currencyTypeId == Enum_CurrencyType.GovernmentBondYield)
                                {
                                    if (column == 2 || column == 3 || column == 4 || column == 6 || column == 7)
                                    {
                                        isvalidcolumn = true;
                                        if (column == 3)
                                        {
                                            liveRate = "-";
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else if ((Enum_CurrencyType)currencyTypeId == Enum_CurrencyType.IndicesAndCommodities)
                                {
                                    if (column == 1 || column == 2 || column == 3 || column == 5 || column == 6)
                                    {
                                        isvalidcolumn = true;
                                        //if (column == 2)
                                        //{
                                        //    liveRate = "-";
                                        //}
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (column == 1 || column == 2 || column == 3 || column == 4 || column == 5)
                                    {
                                        isvalidcolumn = true;
                                    }
                                }
                                if (isvalidcolumn)
                                {
                                    int decPlaces = 4;
                                    //if (currency == "EURUSD" || currency == "GBPUSD" || currency == "AUDUSD" || currency == "USDCNY")
                                    //{
                                    //    decPlaces = 5;
                                    //}
                                    html.Append("<td class='decimalplaces' decimalplaces='" + decPlaces + "'><div class='lr-rate' lrid='" + lid + "'>" + liveRate + "</div></td>");
                                    //html.Append("<td class='decimalplaces" + colorChange + "' decimalplaces='" + decPlaces + "'><div class='lr-rate' lrid='" + i + "'>" + GetLiverate(dttbl, i) + "</div></td>");
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
            return html.ToString();
        }
    }
    public string BindSpotrateBasic()
    {
        string html = @"<table width='150%' cellpadding='0' cellspacing=0>
                <tr class='lr-header'><td></td><td>Bid<br />(Export)</td><td>Ask<br />(Import)</td><td>% Chg</td><td>High</td><td>Low</td></tr>
                <tr>
                    <td class='lr-title'>USDINR</td>
                    " + GetSpotrateHtml(1, 5, 4) + @"
                </tr>
                <tr>
                    <td class='lr-title'>EURINR</td>
                    " + GetSpotrateHtml(10, 14, 4) + @"
                </tr>
                <tr>
                    <td class='lr-title'>GBPINR</td>
                    " + GetSpotrateHtml(19, 23, 4) + @"
                </tr>
                <tr>
                    <td class='lr-title'>JPYINR</td>
                    " + GetSpotrateHtml(28, 32, 4) + @"
                </tr>
                <tr>
                    <td class='lr-title'>EURUSD</td>
                    " + GetSpotrateHtml(37, 41, 5) + @"
                </tr>
                <tr>
                    <td class='lr-title'>GBPUSD</td>
                    " + GetSpotrateHtml(46, 50, 5) + @"
                </tr>
                <tr>
                    <td class='lr-title'>USDJPY</td>
                   " + GetSpotrateHtml(55, 59, 4) + @"
                </tr>
                <tr>
                    <td class='lr-title'>AUDUSD</td>
                    " + GetSpotrateHtml(64, 68, 5) + @"
                </tr>
                <tr>
                    <td class='lr-title'>USDCNY</td>
                    " + GetSpotrateHtml(136, 140, 5) + @"
                </tr>
                <tr>
                    <td class='lr-title'>AUDINR</td>
                    " + GetSpotrateHtml(1196, 1200, 4) + @"
                </tr>

                <tr>
                    <td class='lr-title'>CADINR</td>
                    " + GetSpotrateHtml(1205, 1209, 4) + @"
                </tr>
                <tr>
                    <td class='lr-title'>CNYINR</td>
                    " + GetSpotrateHtml(1221, 1225, 4) + @"
                </tr>
                <tr>
                    <td class='lr-title'>AEDINR</td>
                    " + GetSpotrateHtml(1230, 1234, 4) + @"
                </tr>
            </table>";
        return html;
    }
    public string GetLiverate(int liverateId, bool isDivideBy100)
    {
        string rate = GetLiverate(liverateId);
        if (isDivideBy100)
        {
            rate = GlobalUtilities.ConvertToString(GlobalUtilities.ConvertToDouble(rate) / 100.0);
        }
        return rate;
    }
    public string GetLiverate(int liverateId)
    {
        string query = "select * from tbl_liverate where liverate_liverateid=" + liverateId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            return GlobalUtilities.ConvertToString(dr["liverate_currentrate"]);
        }
        return "";
    }
    public void BindHiddenLiveRates(Literal lthiddenliverates)
    {
        string liverates = "1029,1032,1035,1038,1041,1044,1054";
        string query = "select * from tbl_liverate where liverate_liverateid in(" + liverates + ")";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int liverateId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liverate_liverateid"]);
            html.Append("<div class='lr-rate liverate_" + liverateId + "' lrid='" + liverateId + "'>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_currentrate"]) + "</div>");
        }
        lthiddenliverates.Text = html.ToString();
    }

    public void BindForwardRate(Literal lt, int liverateId1, int liverateId2, int liverateId3, int liverateId4, int liverateId5,
        int cashSpotId1, int cashSpotId2, int decimalPlaces, bool isDivideBy100)
    {
        string lblcashspot = "Cash/Spot";
        StringBuilder html = new StringBuilder();
        if (lt.ID != "ltforwardrate_usdinr")
        {
            //lblcashspot = "TN";
        }
        html.Append("<div>");
        html.Append(@"<table width='100%' cellspacing='0' style='font-size:12px;'>    
                        <tr><td class='bold'>" + lblcashspot + @"</td>
                            <td width='100'>BID (Export)</td><td class='nocolorchange'>" + GetLiveRateHtml(null, cashSpotId1) + @"</td>
                            <td width='100'>ASK (Import)</td><td class='nocolorchange'>" + GetLiveRateHtml(null, cashSpotId2) + @"</td></tr>
                    </table>");

        html.Append("</div>");
        string paisaLabel = "Premium in Paisa";
        if (lt.ID == "ltforwardrate_eurusd" || lt.ID == "ltforwardrate_gbpusd")//EURUSD AND GBPUSD
        {
            paisaLabel = "Premium in Pips";
        }
        html.Append(@"<div><table width='100%' cellpadding='0' cellspacing='0'>
		            <tr class='lr-header'><td>Month End Date</td><td colspan='2'>" + paisaLabel + @"</td><td colspan='2'>Outright Rate</td></tr>
                    <tr class='lr-header'><td></td><td>Bid<br />(Export)</td><td>Ask<br />(Import)</td><td>Bid<br />(Export)</td><td>Ask<br />(Import)</td></tr>");
        StringBuilder liverateIds = new StringBuilder();
        for (int i = 0; i < 12; i++)
        {
            if (i == 0)
            {
                liverateIds.Append(liverateId1);
                liverateIds.Append("," + liverateId2);
                liverateIds.Append("," + liverateId3);
                liverateIds.Append("," + liverateId4);
                liverateIds.Append("," + liverateId5);
            }
            else
            {
                liverateIds.Append("," + (liverateId1 + i));
                liverateIds.Append("," + (liverateId2 + i));
                liverateIds.Append("," + (liverateId3 + i));
                liverateIds.Append("," + (liverateId4 + i));
                liverateIds.Append("," + (liverateId5 + i));
            }
        }
        string query = @"select * from tbl_liverate 
                        join tbl_liveratesection on liveratesection_liveratesectionid=liverate_liveratesectionid
                        where liverate_liverateid in(" + liverateIds.ToString() + ")";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        string divideBy100 = "";
        if (isDivideBy100)
        {
            divideBy100 = " divideby100";
        }
        for (int i = 0; i < 12; i++)
        {
            html.Append(@"<tr>
                            <td class='nocolorchange'>" + GetLiveRateHtml(dttbl, liverateId1) + @"</td>
                            <td class='nocolorchange" + divideBy100 + "'>" + GetLiveRateHtml(dttbl, liverateId2) + @"</td>
                            <td class='nocolorchange" + divideBy100 + "'>" + GetLiveRateHtml(dttbl, liverateId3) + @"</td>
                            <td class='nocolorchange decimalplaces' decimalplaces='" + decimalPlaces + @"'>" + GetLiveRateHtml(dttbl, liverateId4) + @"</td>
                            <td class='nocolorchange decimalplaces' decimalplaces='" + decimalPlaces + @"'>" + GetLiveRateHtml(dttbl, liverateId5) + @"</td>
                        </tr>");
            liverateId1++; liverateId2++; liverateId3++; liverateId4++; liverateId5++;
        }
        html.Append("</table></div>");
        lt.Text = html.ToString();
    }
    public string GetLiveRateHtml(DataTable dttbl, int liverateId)
    {
        string rate = "";
        string rtdCode = "";
        string calculation = "";
        int rowNo = 0;
        int colNo = 0;
        string sectionCode = "";
        int sectionId = 0;
        string calc = "";
        string cls = "";
        if (dttbl == null)
        {
            string query = "select * from tbl_liverate " +
                       "JOIN tbl_liveratesection ON liveratesection_liveratesectionid=liverate_liveratesectionid " +
                       "where liverate_liverateid=" + liverateId;
            dttbl = DbTable.ExecuteSelect(query);
        }
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liverate_liverateid"]) == liverateId)
            {
                rate = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_currentrate"]);
                rtdCode = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_rtdcode"]);
                calculation = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_calculation"]);
                rowNo = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liverate_row"]);
                colNo = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liverate_column"]);
                sectionCode = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liveratesection_code"]);
                sectionId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liverate_liveratesectionid"]);
            }
        }
        string code = sectionCode + "_" + rowNo + "_" + colNo;
        if (calculation != "")
        {
            cls += " calc";
            calc = " calc='" + calculation + "'";
        }
        cls += " " + code;

        string html = "<div sc='" + sectionCode + "' sid='" + sectionId + "' row='" + rowNo + "' class='lr-rate " + cls + "' lrid='" + liverateId + "' rid='" + liverateId + "' rc='" + rtdCode + "'" + calc + " c='" + code + "'>" + rate + "</div>";

        return html;
    }

    public string GetLiverate(DataTable dttbl, int liverateId, bool isDivideBy100)
    {
        string rate = GetLiverate(dttbl, liverateId);
        if (isDivideBy100)
        {
            rate = GlobalUtilities.ConvertToString(GlobalUtilities.ConvertToDouble(rate) / 100.0);
        }
        rate = String.Format(rate, "{0:#.00}");
        return rate;
    }
    public string GetLiverate(DataTable dttbl, int liverateId, int decimalPlaces, bool isDivideBy100)
    {
        string rate = GetLiverate(dttbl, liverateId);
        if (isDivideBy100)
        {
            rate = (GlobalUtilities.ConvertToDouble(rate) / 100.0).ToString();
        }
        if (decimalPlaces == 2)
        {
            rate = String.Format(rate, "{0:#.00}");
        }
        else if (decimalPlaces == 4)
        {
            rate = String.Format(rate, "{0:#.0000}");
        }
        return rate;
    }
    public string GetLiverate(DataTable dttbl, int liverateId)
    {
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liverate_liverateid"]) == liverateId)
            {
                return GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_currentrate"]);
            }
        }
        return "";
    }
    public void BindEssentialReading(Literal ltessentialreading)
    {
        StringBuilder html = new StringBuilder();
        string query = "select * from tbl_essentialreading order by essentialreading_essentialreadingid desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        html.Append("<ons-list style='background-color:#fff;color:#000;'>");
        string url = HttpContext.Current.Request.Url.ToString();
        Array arr = url.Split('/');
        string applicationURL = arr.GetValue(0).ToString() + "//" + arr.GetValue(2).ToString();
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string downloadLink = "";
            url = "";
            if (GlobalUtilities.ConvertToString(dttbl.Rows[i]["essentialreading_document"]).Trim() != "")
            {
                string docPath = "/upload/essentialreading/" +
                                GlobalUtilities.ConvertToString(dttbl.Rows[i]["essentialreading_essentialreadingid"]) + "." +
                                GlobalUtilities.ConvertToString(dttbl.Rows[i]["essentialreading_document"]).Trim();
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("~" + docPath)))
                {
                    downloadLink = "<a href='" + applicationURL + docPath + "' class='lnk'>Download</a>";
                }
            }
            if (GlobalUtilities.ConvertToString(dttbl.Rows[i]["essentialreading_url"]) != "")
            {
                url = "<a href='" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["essentialreading_url"]) + "' class='lnk'>Detail</a>";
            }
            html.Append(@"<ons-list-item style='border-bottom:solid 1px #eee;'>
                        <ons-row>
                            <ons-col>
                                <div class='col-medium'>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["essentialreading_articles"]) + @"</div>
                            </ons-col>
                        </ons-row>
                        <ons-row>
                            <ons-col>
                                <div>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["essentialreading_date"]) + @"</div>
                            </ons-col>
                            <ons-col></ons-col>
                            <ons-col>
                                <div class='col-right'>" + downloadLink + @"</div>
                            </ons-col>
                        </ons-row>
                        <ons-row>
                            <ons-col>
                                <div>" + url + @"</div>
                            </ons-col>
                        </ons-row>
                     </ons-list-item>");
        }
        html.Append("</ons-list>");
        ltessentialreading.Text = html.ToString();
    }


    public void BindOffshore(Literal ltoffshore)
    {
        StringBuilder html = new StringBuilder();
        string query = "";

        html.Append(@"<table width='100%' cellpadding='0' cellspacing=0>
                        <tr>
                            <td colspan='6' style='background-color:#222;height:32px;font-size:14px;padding:10px;'>OFFSHORE</td>
                        </tr>
                        <tr class='lr-header'><td>&nbsp;</td><td>Bid</td><td>Ask</td><td>%Chg</td><td>High</td><td>Low</td></tr>");

        query = @"select liverate_row,liverate_column,liverate_currentrate,liverate_prevrate,liverate_liverateid,liverate_rtdcode,
                    liverate_calculation,liverate_istick,liverate_liveratesectionid,liverate_decimalplaces,
                    currencymaster_currencymasterid,currencymaster_currency,currencytype_currencytypeid
                    from tbl_liverate
                    join tbl_currencymaster on currencymaster_currencymasterid=liverate_currencymasterid
                    join tbl_currencytype on currencytype_currencytypeid=currencymaster_currencytypeid
                    WHERE liverate_liveratesectionid =50
                    and ((liverate_currencymasterid = 1 AND liverate_column in(1,2,3,5,6)) OR (liverate_currencymasterid > 1 AND liverate_column in(1,2,3,4,5)))
                    order by liverate_row,liverate_column";
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
                html.Append("<td class='lr-title'>" + currency + "</td>");
                for (int j = 0; j < dttbl.Rows.Count; j++)
                {
                    int row2 = GlobalUtilities.ConvertToInt(dttbl.Rows[j]["liverate_row"]);
                    int sectionId2 = GlobalUtilities.ConvertToInt(dttbl.Rows[j]["liverate_liveratesectionid"]);
                    int column = GlobalUtilities.ConvertToInt(dttbl.Rows[j]["liverate_column"]);
                    string liveRate = GlobalUtilities.ConvertToString(dttbl.Rows[j]["liverate_currentrate"]);
                    int lid = GlobalUtilities.ConvertToInt(dttbl.Rows[j]["liverate_liverateid"]);
                    if (row == row2)
                    {
                        //bool isvalidcolumn = false;
                        //if (column == 1 || column == 2 || column == 3 || column == 5 || column == 6)
                        //{
                        //    isvalidcolumn = true;
                        //}
                        //if (isvalidcolumn)
                        {
                            int decPlaces = 2;
                            html.Append("<td decimalplaces='" + decPlaces + "'><div class='lr-rate' lrid='" + lid + "'>" + liveRate + "</div></td>");
                        }
                    }
                }
                html.Append("<tr>");
            }
            prevRowId = row;
            prevSectionId = sectionId;
        }

        html.Append("</table>");
        ltoffshore.Text = html.ToString();
    }


    public void BindLiverateLabel(HtmlGenericControl lbl, int liverateId)
    {
        string query = "select * from tbl_liverate where liverate_liverateid=" + liverateId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            lbl.InnerText = GlobalUtilities.ConvertToString(dr["liverate_currentrate"]);
        }
    }

    public void BindCurrency(Literal ltCurrency, int sectionId)
    {
        string query = "select * from tbl_liverate " +
                       "JOIN tbl_liveratesection ON liveratesection_liveratesectionid=liverate_liveratesectionid " +
                       "where liverate_liveratesectionid=" + sectionId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int liverateId = Convert.ToInt32(dttbl.Rows[i]["liverate_liverateid"]);
            html.Append(GetLiveRateHtml(dttbl, liverateId));
        }
        ltCurrency.Text = html.ToString();
    }
    public void BindMetalLiverate(Literal ltlmeforward, Literal lt3mcashspread)
    {
        string query = "";
        query = @"select *
                 from tbl_MetalLiveRate 
                 order by MetalLiveRate_metalid";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        StringBuilder lme3mSpreadHtml = new StringBuilder();
        html.Append("<table width='100%' cellspacing=0 cellpadding=0 class='jq-tbl'>");
        html.Append(@"<tr class='lr-header'><td>METAL</td><td>BID</td><td>ASK</td>
                        <td>CHANGE% <img src='https://finstation.in/images/arrow_expand_right.png' class='jq-btn-more-data hand'></td>
                        <td class='jq-more-data hidden'>OPEN</td><td class='jq-more-data hidden'>HIGH</td>
                        <td class='jq-more-data hidden'>LOW</td><td class='jq-more-data hidden'>P.CLOSE</td>
                        <td class='jq-more-data hidden'>CHANGE</td>
                    </tr>");
        lme3mSpreadHtml.Append("<table width='100%' cellspacing=0 cellpadding=0>");
        lme3mSpreadHtml.Append(@"<tr class='lr-header'><td>METAL</td><td>LTP</td></tr>");

        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            DataRow dr = dttbl.Rows[i];
            int metalId = GlobalUtilities.ConvertToInt(dr["MetalLiveRate_metalid"]);
            if (metalId == 7) continue;//ALUMINALLOY
            string metalName = GetMetalName(metalId);
            string time = GlobalUtilities.ConvertToDateTime(dr["metalliverate_date"]);
            double bid = GlobalUtilities.ConvertToDouble(dr["metalliverate_bid"]);
            double ask = GlobalUtilities.ConvertToDouble(dr["metalliverate_ask"]);
            double open = GlobalUtilities.ConvertToDouble(dr["metalliverate_open"]);
            double high = GlobalUtilities.ConvertToDouble(dr["metalliverate_high"]);
            double low = GlobalUtilities.ConvertToDouble(dr["metalliverate_low"]);
            double prevclose = GlobalUtilities.ConvertToDouble(dr["metalliverate_prevclose"]);
            double change = GlobalUtilities.ConvertToDouble(dr["metalliverate_change"]);
            double changeper = GlobalUtilities.ConvertToDouble(dr["metalliverate_changeper"]);
            double tradingvolume = GlobalUtilities.ConvertToDouble(dr["metalliverate_tradingvolume"]);
            double tradingvalue = GlobalUtilities.ConvertToDouble(dr["metalliverate_tradingvalue"]);

            double threemcashspead_bid = GlobalUtilities.ConvertToDouble(dr["metalliverate_3mcashspreadbid"]);
            double threemcashspead_ask = GlobalUtilities.ConvertToDouble(dr["metalliverate_3mcashspreadask"]);
            double threemcashspead_close = GlobalUtilities.ConvertToDouble(dr["metalliverate_3mcashspreadclose"]);
            double threemcashspead_ltp = GlobalUtilities.ConvertToDouble(dr["metalliverate_3mcashspreadltp"]);

            html.Append("<tr>");
            html.Append("<td class='lr-title'>" + metalName + "</td>");
            html.Append("<td class='rate-padding comm-liverate nowrap hidden' metalid='" + metalId + "' col='date'>" + time + "</td>");
            html.Append("<td><div class='lr-rate comm-liverate' col='bid' metalid='" + metalId + "'>" + bid + "</div></td>");
            html.Append("<td><div class='lr-rate comm-liverate' col='ask' metalid='" + metalId + "'>" + ask + "</div></td>");
            html.Append("<td><div class='lr-rate comm-liverate' col='changeper' metalid='" + metalId + "'>" + changeper + "</div></td>");

            html.Append("<td><div class='lr-rate comm-liverate jq-more-data hidden' col='open' metalid='" + metalId + "'>" + open + "</div></td>");
            html.Append("<td><div class='lr-rate comm-liverate jq-more-data hidden' col='high' metalid='" + metalId + "'>" + high + "</div></td>");
            html.Append("<td><div class='lr-rate comm-liverate jq-more-data hidden' col='low' metalid='" + metalId + "'>" + low + "</div></td>");
            html.Append("<td><div class='lr-rate comm-liverate jq-more-data hidden' col='prevclose' metalid='" + metalId + "'>" + prevclose + "</div></td>");
            html.Append("<td><div class='lr-rate comm-liverate jq-more-data hidden' col='change' metalid='" + metalId + "'>" + change + "</div></td>");


            //html.Append("<td><div class='rate comm-liverate' col='tradingvolume' metalid='" + metalId + "'>" + tradingvolume + "</div></td>");
            //html.Append("<td><div class='rate comm-liverate' col='tradingvalue' metalid='" + metalId + "'>" + tradingvalue + "</div></td>");
            html.Append("</tr>");

            lme3mSpreadHtml.Append("<tr>");
            lme3mSpreadHtml.Append("<td class='lr-title'>" + metalName + "</td>");
            lme3mSpreadHtml.Append("<td><div class='lr-rate comm-liverate' col='3mcashspreadltp' metalid='" + metalId + "'>" + threemcashspead_ltp + "</div></td>");
            lme3mSpreadHtml.Append("</tr>");

        }
        html.Append("</table>");
        ltlmeforward.Text = html.ToString();
        lt3mcashspread.Text = lme3mSpreadHtml.ToString();
    }
    public void BindLMESettlementRate(Literal ltmetallmesettlementrate)
    {
        string query = "";
        query = @"select * from tbl_lmesettlementmetalrate";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        string date = GlobalUtilities.ConvertToDateMMM(Convert.ToDateTime(dttbl.Rows[0]["lmesettlementmetalrate_date"] == DBNull.Value ? DateTime.Now : dttbl.Rows[0]["lmesettlementmetalrate_date"]));
        StringBuilder html = new StringBuilder();
        string nextYear = "Dec-" + Convert.ToString(DateTime.Now.Year + 1).Substring(2);
        string nextYear2 = "Dec-" + Convert.ToString(DateTime.Now.Year + 2).Substring(2);
        string nextYear3 = "Dec-" + Convert.ToString(DateTime.Now.Year + 3).Substring(2);
        html.Append("<table width='100%' cellspacing=0 cellpadding=0>");
        html.Append("<tr><td class='jq-lmesettlement-date'>" + date + "</td></tr>");
        html.Append("<tr><td>");
        html.Append("<table width='100%' cellspacing=0 cellpadding=0>");
        html.Append(@"<tr class='lr-header'><td>METAL</td><td colspan=2>Cash Settlement</td><td class='nowrap' colspan=2>3M-Settlement</td>
                    </tr>");
        html.Append(@"<tr class='lr-header'><td><td>BID</td><td>ASK</td><td>BID</td><td>ASK</td>
                    </tr>");
        BindMetals();
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            DataRow dr = dttbl.Rows[i];
            int id = GlobalUtilities.ConvertToInt(dr["lmesettlementmetalrate_lmesettlementmetalrateid"]);
            int metalId = GlobalUtilities.ConvertToInt(dr["lmesettlementmetalrate_metalid"]);
            string metalName = GetMetalName(metalId);
            double cashAsk = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_cashask"]);
            double cashbid = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_cashbid"]);
            double threemonthsAsk = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_threemonthsask"]);
            double threemonthsBid = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_threemonthsbid"]);
            //double oneyearask = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_oneyearask"]);
            //double oneyearbid = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_oneyearbid"]);
            //double twoyearask = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_twoyearsask"]);
            //double twoyearbid = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_twoyearsbid"]);
            //double threeyearask = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_threeyearsask"]);
            //double threeyearbid = GlobalUtilities.ConvertToDouble(dr["lmesettlementmetalrate_threeyearsbid"]);

            html.Append("<tr>");
            html.Append("<td class='lr-title'>" + metalName + "</td>");

            html.Append("<td><div class='lr-rate'>" + ExportExposurePortal.DecimalPoint(cashAsk, 2) + "</div></td>");
            html.Append("<td><div class='lr-rate'>" + ExportExposurePortal.DecimalPoint(cashbid, 2) + "</div></td>");

            html.Append("<td><div class='lr-rate'>" + ExportExposurePortal.DecimalPoint(threemonthsAsk, 2) + "</div></td>");
            html.Append("<td><div class='lr-rate'>" + ExportExposurePortal.DecimalPoint(threemonthsBid, 2) + "</div></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        html.Append("</td></tr></table>");
        ltmetallmesettlementrate.Text = html.ToString();
    }

    public void BindMetalStock(Literal ltmetalstock)
    {
        string query = "";
        query = @"select * from tbl_lmestockmetalrate";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        string date = GlobalUtilities.ConvertToDateMMM(Convert.ToDateTime(dttbl.Rows[0]["lmestockmetalrate_date"] == DBNull.Value ? DateTime.Now : dttbl.Rows[0]["lmestockmetalrate_date"]));
        StringBuilder html = new StringBuilder();
        string nextYear = "Dec-" + Convert.ToString(DateTime.Now.Year + 1).Substring(2);
        html.Append("<table width='100%' cellspacing=0 cellpadding=0>");
        html.Append("<tr><td class='jq-lmestock-date'>" + date + "</td></tr>");
        html.Append("<tr><td>");
        html.Append("<table width='100%' cellspacing=0 cellpadding=0>");
        html.Append(@"<tr class='lr-header'><td>METAL</td><td>Opening Stock(Total)</td>
                        <td>CHANGE (+ /-)</td>
                      </tr>");

        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            DataRow dr = dttbl.Rows[i];
            int metalId = GlobalUtilities.ConvertToInt(dr["lmestockmetalrate_metalid"]);
            string metalName = GetMetalName(metalId);
            double livewarrants = GlobalUtilities.ConvertToDouble(dr["lmestockmetalrate_livewarrants"]);
            double cancelledwarrants = GlobalUtilities.ConvertToDouble(dr["lmestockmetalrate_cancelledwarrants"]);
            double openingstock = GlobalUtilities.ConvertToDouble(dr["lmestockmetalrate_openingstock"]);
            double prevdayopeningstock = GlobalUtilities.ConvertToDouble(dr["lmestockmetalrate_prevdayopeningstock"]);
            double totalopeningstock = livewarrants + cancelledwarrants;
            double change = GlobalUtilities.ConvertToDouble(dr["lmestockmetalrate_stocknetchange"]);//totalopeningstock - prevdayopeningstock;
            html.Append("<tr>");
            html.Append("<td class='lr-title'>" + metalName + "</td>");
            html.Append("<td><div class='lr-rate'>" + totalopeningstock + "</div></td>");
            html.Append("<td><div class='lr-rate'>" + change + "</div></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        html.Append("</td></tr></table>");
        ltmetalstock.Text = html.ToString();
    }
    public void BindMetalIndicesAndCommodities(Literal ltmetalindicesandcommodities)
    {
        StringBuilder html = new StringBuilder();
        html.Append(@"<table width='100%' cellpadding='0' cellspacing=0>
                    <tr class='lr-header'><td></td><td>LTP</td><td>Change</td><td>%Change</td></tr>");
        html.Append(GetMetalIndicesAndCommodities("$ Index", 1193));
        html.Append(GetMetalIndicesAndCommodities("SENSEX", 163));
        html.Append(GetMetalIndicesAndCommodities("NIFTY", 1107));
        html.Append(GetMetalIndicesAndCommodities("Bank Nifty", 1471));
        html.Append(GetMetalIndicesAndCommodities("India Vix", 1474));
        html.Append(GetMetalIndicesAndCommodities("Brent Oil", 1480));
        html.Append(GetMetalIndicesAndCommodities("WTI Crude", 1483));
        html.Append(GetMetalIndicesAndCommodities("Gold Spot", 1477));
        html.Append(GetMetalIndicesAndCommodities("Silver", 1486));
        html.Append("</table>");
        ltmetalindicesandcommodities.Text = html.ToString();
    }
    public string GetMetalIndicesAndCommodities(string commodityName, int startId)
    {
        StringBuilder html = new StringBuilder();
        html.Append("<tr><td class='lr-title'>" + commodityName + "</td>");
        html.Append("<td><div class='lr-rate' lrid='" + startId + "'>" + GetLiverate(startId) + "</div></td>");
        html.Append("<td><div class='lr-rate' lrid='" + (startId + 1) + "'>" + GetLiverate(startId + 1) + "</div></td>");
        if (startId == 163)
        {
            html.Append("<td><div class='lr-rate' lrid='1106'>" + GetLiverate(1106) + "</div></td>");
        }
        else
        {
            html.Append("<td><div class='lr-rate' lrid='" + (startId + 2) + "'>" + GetLiverate(startId + 2) + "</div></td>");
        }
        html.Append("</tr>");
        return html.ToString();
    }
    public void BindMetalCurrencies(Literal ltmetalCurrencies)
    {
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%' cellspacing=0 cellpadding=0>");
        html.Append(@"<table width='150%' cellpadding='0' cellspacing=0>
                <tr class='lr-header'><td></td><td>Bid</td><td>Ask</td><td>Open</td><td>High</td><td>Low</td><td>P.CLOSE</td></tr>");
        html.Append(@"
                <tr>
                    <td class='lr-title'>USDINR</td>
                    " + GetSpotrateHtml_Metal(1, 7, 4) + @"
                </tr>
                <tr>
                    <td class='lr-title'>EURINR</td>
                    " + GetSpotrateHtml_Metal(10, 16, 4) + @"
                </tr>
                <tr>
                    <td class='lr-title'>GBPINR</td>
                    " + GetSpotrateHtml_Metal(19, 25, 4) + @"
                </tr>
                <tr>
                    <td class='lr-title'>JPYINR</td>
                    " + GetSpotrateHtml_Metal(28, 34, 4) + @"
                </tr>
                <tr>
                    <td class='lr-title'>EURUSD</td>
                    " + GetSpotrateHtml_Metal(37, 43, 5) + @"
                </tr>
                <tr>
                    <td class='lr-title'>GBPUSD</td>
                    " + GetSpotrateHtml_Metal(46, 52, 5) + @"
                </tr>
                <tr>
                    <td class='lr-title'>USDJPY</td>
                   " + GetSpotrateHtml_Metal(55, 61, 4) + @"
                </tr>
                <tr>
                    <td class='lr-title'>AUDUSD</td>
                    " + GetSpotrateHtml_Metal(64, 70, 5) + @"
                </tr>
                <tr>
                    <td class='lr-title'>USDCNY</td>
                    " + GetSpotrateHtml_Metal(136, 142, 5) + @"
                </tr>
                <tr>
                    <td class='lr-title'>AUDINR</td>
                    " + GetSpotrateHtml_Metal(1196, 1202, 4) + @"
                </tr>

                <tr>
                    <td class='lr-title'>CADINR</td>
                    " + GetSpotrateHtml_Metal(1205, 1211, 4) + @"
                </tr>
                <tr>
                    <td class='lr-title'>CNYINR</td>
                    " + GetSpotrateHtml_Metal(1221, 1227, 4) + @"
                </tr>
                <tr>
                    <td class='lr-title'>AEDINR</td>
                    " + GetSpotrateHtml_Metal(1230, 1236, 4) + @"
                </tr>
            </table>");
        ltmetalCurrencies.Text = html.ToString();
    }
    public string GetSpotrateHtml_Metal(int fromLiverateId, int toLiverateId, int decimalplaces)
    {
        string query = "select * from tbl_liverate where liverate_liverateid>=" + fromLiverateId + " and liverate_liverateid<=" + toLiverateId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        int index = 0;
        for (int i = fromLiverateId; i <= toLiverateId; i++)
        {
            //Bid (Export)	Ask (Import)	% Chg	Today's High	Today's Low	Today's Open Show more	Prv. Close
            if (i == fromLiverateId + 2) continue;
            int decPlaces = decimalplaces;
            if (index == 2) decPlaces = 2;
            string colorChange = "";
            if (i == fromLiverateId || i == fromLiverateId + 1 || i == fromLiverateId + 2)
            {
                //show color change only for Bid,Ask & % Change
            }
            else
            {
                colorChange = " nocolorchange";
            }
            html.Append("<td class='decimalplaces" + colorChange + "' decimalplaces='" + decPlaces + "'>" +
                        "<div class='lr-rate' lrid='" + i + "'>" + GetLiverate(dttbl, i) + "</div></td>");
            index++;
        }
        return html.ToString();
    }
    public void BindMetals()
    {
        string query = "select * from tbl_metal";
        _dttblMetal = DbTable.ExecuteSelect(query);
    }
    public string GetMetalName(int metalId)
    {
        for (int i = 0; i < _dttblMetal.Rows.Count; i++)
        {
            if (Convert.ToInt32(_dttblMetal.Rows[i]["metal_metalid"]) == metalId)
            {
                return Convert.ToString(_dttblMetal.Rows[i]["metal_metalname"]);
            }
        }
        return "";
    }
    private string GetSpotrateHtml(int fromLiverateId, int toLiverateId, int decimalplaces)
    {
        string query = "select * from tbl_liverate where liverate_liverateid>=" + fromLiverateId + " and liverate_liverateid<=" + toLiverateId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        int index = 0;
        for (int i = fromLiverateId; i <= toLiverateId; i++)
        {
            int decPlaces = decimalplaces;
            if (index == 2) decPlaces = 2;
            string colorChange = "";
            if (i == fromLiverateId || i == fromLiverateId + 1 || i == fromLiverateId + 2)
            {
                //show color change only for Bid,Ask & % Change
            }
            else
            {
                colorChange = " nocolorchange";
            }
            html.Append("<td class='decimalplaces" + colorChange + "' decimalplaces='" + decPlaces + "'><div class='lr-rate' lrid='" + i + "'>" + GetLiverate(dttbl, i) + "</div></td>");
            index++;
        }
        return html.ToString();
    }
}
