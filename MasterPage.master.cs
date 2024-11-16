using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.Text;
using System.Configuration;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        bool isLoggedIn = GlobalUtilities.ConvertToBool(CustomSession.Session("Login_IsLoggedIn"));
        string url = Request.Url.ToString().ToLower();
        bool isFinstationEnabled = false;
        if (Finstation.IsFinstationEnabled() || Finstation.IsMiniFinstationEnabled())
        {
            isFinstationEnabled = true;
        }
        if (!isLoggedIn)
        {
            if (url == Custom.AppUrl.ToLower() + "/trade-finance-enquiry.aspx" ||
                url.StartsWith(Custom.AppUrl.ToLower() + "/tradefinanceenquiry-"))
            {
            }
            else
            {
                Array arr = url.Split('/');
                string redirectUrl="";
                int startIndex=3;
                if(url.ToLower().Contains("localhost"))startIndex++;
                for (int i = startIndex; i < arr.Length; i++)
                {
                    if (redirectUrl.ToString() == "")
                    {
                        redirectUrl = arr.GetValue(i).ToString();
                    }
                    else
                    {
                        redirectUrl += "/" + arr.GetValue(i).ToString();
                    }
                }
                if (redirectUrl != "") redirectUrl = "?url=" + redirectUrl;
                Response.Redirect("~/customerlogin.aspx" + redirectUrl);
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        bool isLoggedIn = GlobalUtilities.ConvertToBool(Session["Login_IsLoggedIn"]);
        bool isFinstationEnabled = false;
        if (Finstation.IsFinstationEnabled() || Finstation.IsMiniFinstationEnabled())
        {
            isFinstationEnabled = true;
        }
        string url = Request.Url.ToString().ToLower();

        if (!IsPostBack && isFinstationEnabled)
        {
            if (isLoggedIn)
            {
                BindExpiryDate();
                BindAllLiveRates();
                BindCurrencies();
            }
            
            if (url.IndexOf("/index.aspx") > 0)
            {
                //tblinner.Style.Add("width", "1024px");
            }
            else if (url.IndexOf("/cross-currency.aspx") > 0)
            {
                tblinner.Style.Add("width", "1500px");
            }
            else
            {
                //tblinner.Style.Add("width", "1024px");
                tblinner.Style.Add("min-width", "1024px");
            }
            BindQueryMenu();
            BindFAQMenu();
            BindLiborRatesFromAlternateRefRates();
            //BindTopicWiseMenu();
            //InitBrokenDateCalc();
        }
        if (isLoggedIn && isFinstationEnabled)
        {
            //EnableExportPortal(4);
            //EnableExportPortal(5);
            //if (Finstation.IsExportPortalEnabled() || Finstation.IsImportPortalEnabled())
            //{
            //    tdExposurePortal.Visible = true;
            //}
            //if (Commodity.IsCommodityEnabled())
            //{
            //    tdmetal.Visible = true;
            //}
        }
        if (!isFinstationEnabled)
        {
            trfinstationmenu.Visible = false;
        }
        if (!isFinstationEnabled)
        {
            if (!IsPostBack)
            {
                if (Finstation.IsBankScanEnabled())
                {
                    if (url.StartsWith(Custom.AppUrl.ToLower() + "/viewbankaudit.aspx") || url.StartsWith(Custom.AppUrl.ToLower() + "/addbankaudit.aspx"))
                    {
                    }
                    else
                    {
                        Response.Redirect("~/customerlogin.aspx");
                    }
                }
            }
        }
        if (!IsPostBack)
        {
            lblUserName.Text = GlobalUtilities.ConvertToString(Session["Login_Name"]);
            if (lblUserName.Text != "")
            {
                lblUserInitial.Text = lblUserName.Text.Substring(0, 1).ToUpper();
            }
        }
        InitFinstationScript();
    }
    private void EnableExportPortal(int type)
    {
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        string query = "select * from tbl_subscriptionprospects " +
                        "join tbl_subscription ON subscription_subscriptionid=subscriptionprospects_subscriptionid " +
                        "WHERE subscriptionprospects_prospectid = "+type+" AND subscription_clientid=" + clientId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            query = "select * from tbl_trialprospect " +
                        "join tbl_trial ON trial_trialid=trialprospect_trialid " +
                        "WHERE trialprospect_prospectid = "+type+" AND trial_clientid=" + clientId;
            dr = DbTable.ExecuteSelectRow(query);
            if (dr == null) return;
        }
        if (type == 4 || type == 5)
        {
            //tdExposurePortal.Visible = true;
        }
        //else
        //{
        //    tdImposurePortal.Visible = true;
        //}
    }
    
    private void BindAllLiveRates()
    {
        for (int i = 0; i < pnlCalculate.Controls.Count; i++)
        {
            Control ctrl = pnlCalculate.Controls[i];
            if (ctrl is Label)
            {
                Label lbl = (Label)ctrl;
                if (GlobalUtilities.ConvertToString(lbl.Attributes["rc"]) != "")
                {
                    string rc = global.CheckInputData(lbl.Attributes["rc"]);
                    string query = "select * from tbl_liverate WHERE liverate_rtdcode='" + rc + "'";
                    DataRow dr = DbTable.ExecuteSelectRow(query);
                    if (dr != null)
                    {
                        int liverateId = GlobalUtilities.ConvertToInt(dr["liverate_liverateid"]);
                        lbl.Attributes.Add("rid", liverateId.ToString());
                        lbl.Text = GlobalUtilities.ConvertToString(dr["liverate_currentrate"]);
                        lbl.Attributes.Add("istick", GlobalUtilities.ConvertToInt(dr["liverate_istick"]).ToString());
                    }
                }
                else if (GlobalUtilities.ConvertToString(lbl.Attributes["rid"]) != "")
                {
                    int rateId = GlobalUtilities.ConvertToInt(lbl.Attributes["rid"]);
                    string query = "select * from tbl_liverate WHERE liverate_liverateid=" + rateId;
                    DataRow dr = DbTable.ExecuteSelectRow(query);
                    if (dr != null)
                    {
                        int liverateId = GlobalUtilities.ConvertToInt(dr["liverate_liverateid"]);
                        lbl.Attributes.Add("rc", GlobalUtilities.ConvertToString(dr["liverate_rtdcode"]));
                        lbl.Text = GlobalUtilities.ConvertToString(dr["liverate_currentrate"]);
                        lbl.Attributes.Add("istick", GlobalUtilities.ConvertToInt(dr["liverate_istick"]).ToString());
                        if (GlobalUtilities.ConvertToString(lbl.Attributes["divideby"]) != "")
                        {
                            double dblDivideBy = GlobalUtilities.ConvertToDouble(lbl.Attributes["divideby"]);
                            lbl.Text = Convert.ToString(GlobalUtilities.ConvertToDouble(lbl.Text) / dblDivideBy);
                        }
                    }
                }
            }
        }
    }
    private void BindTopicWiseMenu()
    {
        StringBuilder html = new StringBuilder();
        string query = "select * from tbl_topicmenu";
        DataTable dttblmenu = DbTable.ExecuteSelect(query);
        html.Append("<ul>");
        for (int i = 0; i < dttblmenu.Rows.Count; i++)
        {
            int menuid = GlobalUtilities.ConvertToInt(dttblmenu.Rows[i]["topicmenu_topicmenuid"]);
            string menu = GlobalUtilities.ConvertToString(dttblmenu.Rows[i]["topicmenu_menu"]);
            html.Append("<li style='width:190px;'><a href='#'>" + menu + "</a>");
            query = "select * from tbl_topicsubmenu where topicsubmenu_topicmenuid=" + menuid;
            DataTable dttblsubmenu = DbTable.ExecuteSelect(query);
            if (dttblsubmenu.Rows.Count > 0)
            {
                html.Append("<ul>");
                for (int j = 0; j < dttblsubmenu.Rows.Count; j++)
                {
                    int submenuId = GlobalUtilities.ConvertToInt(dttblsubmenu.Rows[j]["topicsubmenu_topicsubmenuid"]);
                    string submenu = GlobalUtilities.ConvertToString(dttblsubmenu.Rows[j]["topicsubmenu_submenu"]);
                    html.Append("<li style='width:190px;'><a dynamic='true' href='topic.aspx?smid=" + submenuId + "'>" + submenu + "</a></li>");
                }
                html.Append("</ul>");
            }
            html.Append("</li>");
        }
        html.Append("</ul>");
        //ltstaticmenu.Text = html.ToString();
    }
    private void BindExpiryDate()
    {
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        string query = "select * from tbl_client where client_clientid=" + clientId;
        DataRow drClient = DbTable.ExecuteSelectRow(query);
        if (drClient == null) Response.Redirect("~/customerlogin.aspx");
        DateTime endDate = Convert.ToDateTime(drClient["client_enddate"]);
        ltExpiryMessage.Text = "<div class='subscribe-expired-msg'><marquee>Your Subscription is valid till " + GlobalUtilities.ConvertToDate(endDate) + "</marquee></div>";

    }
    private void BindQueryMenu()
    {
        StringBuilder html = new StringBuilder();
        string query = "select * from tbl_querytopic order by querytopic_querytypeid";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        //EXIM
        html.Append("<ul>");
        html.Append("<li><a href='viewcustomerquery.aspx'>View My Queries</a></li>");
        BindQuerySubMenu(html, dttbl, 1);
        html.Append("</ul>");
        lteximhelpdeskment.Text = html.ToString();

        //fema
        html = new StringBuilder();
        html.Append("<ul>");
        html.Append("<li><a href='viewcustomerquery.aspx'>View My Queries</a></li>");
        BindQuerySubMenu(html, dttbl, 2);
        html.Append("</ul>");
        ltfemahelpdeskment.Text = html.ToString();

        //html.Append("<ul>");
        //html.Append("<li><a href='viewcustomerquery.aspx'>View My Queries</a></li>");
        //html.Append("<li class='submenu-title'><a href='#'>EXIM</a></li>");
        //BindQuerySubMenu(html, dttbl, 1);
        //html.Append("<li class='submenu-title'><a href='#'>FEMA/RBI</a></li>");
        //BindQuerySubMenu(html, dttbl, 2);
        //html.Append("</ul>");
        //ltquerymenu.Text = html.ToString();
    }
    private void BindQuerySubMenu(StringBuilder html, DataTable dttbl, int type)
    {
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (type == GlobalUtilities.ConvertToInt(dttbl.Rows[i]["querytopic_querytypeid"]))
            {
                int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["querytopic_querytopicid"]);
                string topicName = GlobalUtilities.ConvertToString(dttbl.Rows[i]["querytopic_topicname"]);
                html.Append("<li><a href='customerquery.aspx?id=" + id + "'>" + topicName + "</a></li>");
            }
        }
    }
    private void BindFAQMenu()
    {
        StringBuilder html = new StringBuilder();
        string query = "select * from tbl_faqcategory";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        html.Append("<ul>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["faqcategory_faqcategoryid"]);
            string catName = GlobalUtilities.ConvertToString(dttbl.Rows[i]["faqcategory_categoryname"]);
            html.Append("<li><a href='viewfaq.aspx?id=" + id + "'>" + catName + "</a></li>");
        }
        html.Append("</ul>");
        ltfaqmenu.Text = html.ToString();
    }
    private void BindLiborRatesFromAlternateRefRates()
    {
        string query = "";
        //currency - USD, ARR - CME SOFR TERM
        query = @"select top 1 * from tbl_alternativereferencerate
                  where alternativereferencerate_currencyid=2 and alternativereferencerate_arrmasterid=1
                  order by alternativereferencerate_date desc";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        string rate1 = "";
        string rate2 = "";
        string rate3 = "";
        string rate4 = "";
        if (dr != null)
        {
            rate1 = GlobalUtilities.ConvertToString(dr["alternativereferencerate_1monthtermaverage"]);
            rate2 = GlobalUtilities.ConvertToString(dr["alternativereferencerate_3monthtermaverage"]);
            rate3 = GlobalUtilities.ConvertToString(dr["alternativereferencerate_6monthtermaverage"]);
            rate4 = GlobalUtilities.ConvertToString(dr["alternativereferencerate_12monthtermaverage"]);
            lbl_altrefrate_cme_usd_11.Text = rate1;
            lbl_altrefrate_cme_usd_12.Text = rate2;
            lbl_altrefrate_cme_usd_13.Text = rate3;
            lbl_altrefrate_cme_usd_14.Text = rate4;

            lbl_altrefrate_cme_usd_21.Text = rate1;
            lbl_altrefrate_cme_usd_22.Text = rate2;
            lbl_altrefrate_cme_usd_23.Text = rate3;
            lbl_altrefrate_cme_usd_24.Text = rate4;

            lbl_altrefrate_cme_usd_31.Text = rate1;
            lbl_altrefrate_cme_usd_32.Text = rate2;
            lbl_altrefrate_cme_usd_33.Text = rate3;
            lbl_altrefrate_cme_usd_34.Text = rate4;
        }
        //currency - EUR, ARR - EURIBOR
        query = @"select top 1 * from tbl_alternativereferencerate
                  where alternativereferencerate_currencyid=3 and alternativereferencerate_arrmasterid=3
                  order by alternativereferencerate_date desc";
        dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            rate1 = GlobalUtilities.ConvertToString(dr["alternativereferencerate_1monthtermaverage"]);
            rate2 = GlobalUtilities.ConvertToString(dr["alternativereferencerate_3monthtermaverage"]);
            rate3 = GlobalUtilities.ConvertToString(dr["alternativereferencerate_6monthtermaverage"]);
            rate4 = GlobalUtilities.ConvertToString(dr["alternativereferencerate_12monthtermaverage"]);
            lbl_altrefrate_EURIBOR_EUR_11.Text = rate1;
            lbl_altrefrate_EURIBOR_EUR_12.Text = rate2;
            lbl_altrefrate_EURIBOR_EUR_13.Text = rate3;
            lbl_altrefrate_EURIBOR_EUR_14.Text = rate4;

            lbl_altrefrate_EURIBOR_EUR_21.Text = rate1;
            lbl_altrefrate_EURIBOR_EUR_22.Text = rate2;
            lbl_altrefrate_EURIBOR_EUR_23.Text = rate3;
            lbl_altrefrate_EURIBOR_EUR_24.Text = rate4;

            lbl_altrefrate_EURIBOR_EUR_31.Text = rate1;
            lbl_altrefrate_EURIBOR_EUR_32.Text = rate2;
            lbl_altrefrate_EURIBOR_EUR_33.Text = rate3;
            lbl_altrefrate_EURIBOR_EUR_34.Text = rate4;
        }
        //currency - GBP, ARR - SONIA
        query = @"select top 1 * from tbl_alternativereferencerate
                  where alternativereferencerate_currencyid=4 and alternativereferencerate_arrmasterid=6
                  order by alternativereferencerate_date desc";
        dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            rate1 = GlobalUtilities.ConvertToString(dr["alternativereferencerate_1monthtermaverage"]);
            rate2 = GlobalUtilities.ConvertToString(dr["alternativereferencerate_3monthtermaverage"]);
            rate3 = GlobalUtilities.ConvertToString(dr["alternativereferencerate_6monthtermaverage"]);
            rate4 = GlobalUtilities.ConvertToString(dr["alternativereferencerate_12monthtermaverage"]);
            lbl_altrefrate_SONIA_GBP_11.Text = rate1;
            lbl_altrefrate_SONIA_GBP_12.Text = rate2;
            lbl_altrefrate_SONIA_GBP_13.Text = rate3;
            lbl_altrefrate_SONIA_GBP_14.Text = rate4;

            lbl_altrefrate_SONIA_GBP_21.Text = rate1;
            lbl_altrefrate_SONIA_GBP_22.Text = rate2;
            lbl_altrefrate_SONIA_GBP_23.Text = rate3;
            lbl_altrefrate_SONIA_GBP_24.Text = rate4;

            lbl_altrefrate_SONIA_GBP_31.Text = rate1;
            lbl_altrefrate_SONIA_GBP_32.Text = rate2;
            lbl_altrefrate_SONIA_GBP_33.Text = rate3;
            lbl_altrefrate_SONIA_GBP_34.Text = rate4;
        }
    }
    private void InitBrokenDateCalc()
    {
        int clientUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
        string query = "select * from tbl_usercurrencymargin where usercurrencymargin_clientuserid=" + clientUserId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return;
        double margin = GlobalUtilities.ConvertToDouble(dr["usercurrencymargin_margin"]);
        string script = "<script>$(document).ready(function(){$('.jq-currency-margin').val(" + margin + ");});</script>";
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "currencymargin", script);
    }
    private void InitFinstationScript()
    {
        string onesignalid = DbTable.GetOneColumnData("tbl_clientuser", "clientuser_onesignalid", Common.ClientUserId);
        string script = "<script>$(document).ready(function(){_isFinstationEnabled = " + IsFinstationEnabled.ToString().ToLower() + 
                        ";__LOGIN_USERID='" + Common.UserId.ToString() + "';"+Environment.NewLine;
        script += "initOneSignal('" + Common.GetSetting("OneSignalAppId") + "', '" + onesignalid + "');";
        if (GlobalUtilities.ConvertToBool(CustomSession.Session("Login_IsAdmin")))
        {
            script += "initLiverateAdmin();";
        }
        script += Environment.NewLine + "});</script>";
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "finstation", script);
    }
    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        CustomSession.Delete();
        Session.Abandon();
        Response.Redirect("~/customerlogin.aspx");
    }
    public bool IsFinstationEnabled
    {
        get
        {
            return Finstation.IsFinstationEnabled();
        }
    }

    public string VersionNo
    {
        get
        {
            return ConfigurationManager.AppSettings["VersionNo"].ToString();
        }
    }
    private void BindCurrencies()
    {
        Finstation.BindCurrency(ltUSDINR, 30);
        Finstation.BindCurrency(ltEURINR, 32);
        Finstation.BindCurrency(ltGBPINR, 34);
        Finstation.BindCurrency(ltJPYINR, 36);
        Finstation.BindCurrency(ltEURUSD, 38);
        Finstation.BindCurrency(ltGBPUSD, 40);
        Finstation.BindCurrency(ltUSDJPY, 42);
        if (Request.QueryString["debug"] != "true")
        {
            divcurrencies.Attributes.Add("style", "display:none;");
        }
    }
}

