using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.Text;

public partial class exportexposureportal_portal_dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindCurrency();
            BindData(GlobalUtilities.ConvertToInt(ddlcurrency.SelectedValue), 0);
        }
    }
    private void BindCurrency()
    {
        string query = "select * from tbl_exposurecurrencymaster order by exposurecurrencymaster_exposurecurrencymasterid";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        ddlcurrency.DataSource = dttbl;
        ddlcurrency.DataTextField = "exposurecurrencymaster_currency";
        ddlcurrency.DataValueField = "exposurecurrencymaster_exposurecurrencymasterid";
        ddlcurrency.DataBind();
    }
    protected void ddlcurrency_changed(object sender, EventArgs e)
    {
        int currency=GlobalUtilities.ConvertToInt(ddlcurrency.SelectedValue);
        BindData(currency, 0);
        lnknextyear.Visible = false;
        lnkprevyear.Visible = false;
        if (currency == 1 || currency == 2 || currency == 3)
        {
            lnknextyear.Visible = true;
            ViewState["year"] = "0";
        }
    }
    private void BindData(int currency, int year)
    {
        //bind spot rate
        int liverateid = 0;
        if (PortalType == 1)//export
        {
            if (currency == 1)
            {
                liverateid = 1;
            }
            else if (currency == 2)
            {
                liverateid = 10;
            }
            else if (currency == 3)
            {
                liverateid = 19;
            }
            else if (currency == 4)
            {
                liverateid = 28;
            }
        }
        else//import
        {
            if (currency == 1)
            {
                liverateid = 2;
            }
            else if (currency == 2)
            {
                liverateid = 11;
            }
            else if (currency == 3)
            {
                liverateid = 20;
            }
            else if (currency == 4)
            {
                liverateid = 29;
            }
        }
        string query = "select * from tbl_liverate where liverate_liverateid=" + liverateid;
        DataRow dr1 = DbTable.ExecuteSelectRow(query);
        lblspotrate.Text = GlobalUtilities.ConvertToString(dr1["liverate_currentrate"]);

        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        ExportExposurePortal obj = new ExportExposurePortal(clientId);
        DataTable dttbl = new DataTable();
        ltDashboard.Text = obj.GetDashboardData(PortalType, currency, false, year, out dttbl);
        
    }
    protected void lnkprevyear_Click(object sender, EventArgs e)
    {
        int year = GlobalUtilities.ConvertToInt(ViewState["year"]);
        lnknextyear.Visible = true;
        if (year <= 1)
        {
            year = 0;
            lnkprevyear.Visible = false;
        }
        else
        {
            year--;
        }
        ViewState["year"] = year;
        BindBindDataByYear(year);
    }
    protected void lnknextyear_Click(object sender, EventArgs e)
    {
        int year = GlobalUtilities.ConvertToInt(ViewState["year"]);
        lnkprevyear.Visible = true;
        if (year >= 4)
        {
            year = 4;
            lnknextyear.Visible = false;
        }
        else
        {
            year++;
        }
        ViewState["year"] = year;
        BindBindDataByYear(year);
    }
    private void BindBindDataByYear(int year)
    {
        BindData(GlobalUtilities.ConvertToInt(ddlcurrency.SelectedValue), year);
    }
    private int PortalType
    {
        get
        {
            int pt = Common.GetQueryStringValue("pt");
            return pt;
        }
    }
}
