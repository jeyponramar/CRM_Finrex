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
using System.Xml.Linq;
using System.Text;
using WebComponent;

public partial class mobile_phonegap_static_html : System.Web.UI.Page
{
    DataTable _dttblMetal = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        string device = Request.QueryString["device"];
        string onesignalid = Request.QueryString["onesignalid"];
        string isnotification = Request.QueryString["isnotification"];
        string extraQueryString = "device=" + device+"&onesignalid="+onesignalid+"&isnotification="+isnotification;
        //ErrorLog.WriteLog("extraQueryString=" + extraQueryString);

        int clientUserId = CheckLogin();

        MobileApp_PhoneGap objmobile = new MobileApp_PhoneGap();
        ltspotrate.Text = "";// objmobile.BindSpotrate(clientUserId);

        objmobile.BindMetals();
        //HttpContext.Current.Response.Headers.Remove("Access-Control-Allow-Origin");
        //HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", "*");
        objmobile.BindForwardRate(ltforwardrate_usdinr, 195, 208, 221, 234, 248, 207, 220, 2, false);
        //BindForwardRate(ltforwardrate_eurinr, 305, 318, 331, 344, 357, 317, 330, 2, false);//EURINR for Monthwise
        objmobile.BindForwardRate(ltforwardrate_eurinr, 1253, 1266, 1279, 1292, 1305, 1265, 1278, 2, false);//EURINR for Monthend
        //BindForwardRate(ltforwardrate_gbpinr, 410, 423, 436, 449, 462, 422, 435, 2, true);//GBPINR for Monthwise
        objmobile.BindForwardRate(ltforwardrate_gbpinr, 1357, 1370, 1383, 1396, 1409, 1369, 1382, 2, false);//GBPINR for Monthend
        objmobile.BindForwardRate(ltforwardrate_eurusd, 618, 631, 644, 657, 670, 630, 643, 4, false);
        objmobile.BindForwardRate(ltforwardrate_gbpusd, 722, 735, 748, 761, 774, 734, 747, 4, false);
        objmobile.BindEssentialReading(ltessentialreading);
        objmobile.BindSpotrate(clientUserId);
        objmobile.BindLiverateLabel(lblspotdate, 1048);
        objmobile.BindHiddenLiveRates(lthiddenliverates);

        //bind hidden liverates used for calculation
        objmobile.BindCurrency(ltUSDINR, 30);
        objmobile.BindCurrency(ltEURINR, 32);
        objmobile.BindCurrency(ltGBPINR, 34);
        objmobile.BindCurrency(ltJPYINR, 36);
        objmobile.BindCurrency(ltEURUSD, 38);
        objmobile.BindCurrency(ltGBPUSD, 40);
        objmobile.BindCurrency(ltUSDJPY, 42);

        txtexpirydate.Text = GlobalUtilities.GetCurrentDateDDMMYYYY().Replace("/", "-");

        GlobalData gblData = new GlobalData();
        gblData.FillDropdown(ddlcurrencymasterid, "tbl_currencymaster", "currencymaster_currency", "currencymaster_currencymasterid",
                            "currencymaster_currencymasterid <= 4", "currencymaster_currency");
        gblData.FillDropdown(ddlcovertypeid, "tbl_covertype", "covertype_covertype", "covertype_covertypeid", "", "covertype_covertype");

        //metal
        BindMetalCommodity();
        objmobile.BindOffshore(ltoffshore);
    }
    public void BindMetalCommodity()
    {
        MobileApp_PhoneGap objmobile = new MobileApp_PhoneGap();
        objmobile.BindMetalLiverate(ltlmeforward, lt3mcashspread);
        objmobile.BindLMESettlementRate(ltmetallmesettlementrate);
        objmobile.BindMetalStock(ltmetalstock);
        objmobile.BindMetalIndicesAndCommodities(ltmetalindicesandcommodities);
        objmobile.BindMetalCurrencies(ltmetalCurrencies);
    }
    public int CheckLogin()
    {
        string query = "";
        string sessionid = Common.GetQueryString("sessionid");
        int clientUserId = 0;
        //ErrorLog.WriteLog("CheckLogin1-" + sessionid);
        if (sessionid != "" && sessionid != "undefined")
        {
            query = "select * from tbl_clientuser where clientuser_isactive=1 and clientuser_ismobileuser=1 and clientuser_mobilesessionid=@sessionid";
            Hashtable hstblp = new Hashtable();
            hstblp.Add("sessionid", sessionid);
            DataRow druser = DbTable.ExecuteSelectRow(query, hstblp);
            //ErrorLog.WriteLog("CheckLogin2-" + sessionid);
            //ErrorLog.WriteLog("qry:" + query);
            if (druser != null)
            {
                //ErrorLog.WriteLog("CheckLogin3-" + sessionid);
                MobileApp_PhoneGap objMob = new MobileApp_PhoneGap();
                int clientId = GlobalUtilities.ConvertToInt(druser["clientuser_clientid"]);
                if (objMob.IsClientActive(clientId))
                {
                    //ErrorLog.WriteLog("CheckLogin4-" + sessionid);
                    txtserversessionid.Text = sessionid;
                    txtisfinstationenabled.Text = Finstation.IsFinstationEnabled(clientId).ToString().ToLower();
                    txtismetalcommodityenabled.Text = Commodity.IsCommodityEnabled(clientId).ToString().ToLower();
                }
                clientUserId = GlobalUtilities.ConvertToInt(druser["clientuser_clientuserid"]);
                Common.SaveClientUserHistory(clientUserId, 3);
            }
        }
        return clientUserId;
    }
    
}
