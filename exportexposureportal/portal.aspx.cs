using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;

public partial class exportexposureportal_portal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!GlobalUtilities.ConvertToBool(CustomSession.Session("Login_IsLoggedIn")))// || !ExportExposurePortal.IsFEMPortalEnabled(PortalType))
        {
            Response.Redirect("~/customerlogin.aspx");
        }
        bool isExportEnabled = ExportExposurePortal.IsFEMPortalEnabled(1);
        bool isImportEnabled = ExportExposurePortal.IsFEMPortalEnabled(2);
        if (!isExportEnabled && !isImportEnabled)
        {
            Response.Redirect("~/customerlogin.aspx");
        }
        if (!IsPostBack)
        {
            ddlportaltype.SelectedValue = PortalType.ToString();
            if (isImportEnabled && isExportEnabled)
            {
                ddlportaltype.Visible = true;
            }
            
            if (PortalType == 1)//1-Export
            {
                divexportmenu.Visible = true;
                if (!isExportEnabled)
                {
                    Response.Redirect("portal.aspx?pt=2");
                }
            }
            else//Import
            {
                divimportmenu.Visible = true;
                if (!isImportEnabled)
                {
                    Response.Redirect("portal.aspx?pt=1");
                }
            }
            ltdashboardlink.Text="<iframe src='portal-dashboard.aspx?pt="+PortalType+"' style='border:solid 0px;width:100%;height:500px;' id='ifr-dashboard'></iframe>";
            BindCompanyDetail();
            CreateMasterTable();
        }
    }
    
    private void BindCompanyDetail()
    {
        int clientUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
        string query = @"select * from tbl_clientuser 
                         join tbl_client on client_clientid=clientuser_clientid
                         where clientuser_clientuserid=" + clientUserId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        lblCompanyName.Text = GlobalUtilities.ConvertToString(dr["client_customername"]);
        lblusername.Text = GlobalUtilities.ConvertToString(dr["clientuser_name"]);
    }
    private void CreateMasterTable()
    {
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        string query = "select count(*) c from tbl_customercurrency where customercurrency_clientid=" + clientId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (GlobalUtilities.ConvertToInt(dr[0]) > 0) return;
        query = "insert into tbl_customercurrency(customercurrency_exposurecurrencymasterid,customercurrency_clientid)" +
                "select exposurecurrencymaster_exposurecurrencymasterid," + clientId + " from tbl_exposurecurrencymaster";
        DbTable.ExecuteQuery(query);
    }
    protected void ddlportaltype_Changed(object sender, EventArgs e)
    {
        Response.Redirect("~/exportexposureportal/portal.aspx?pt="+ddlportaltype.SelectedValue);
    }
    private int PortalType
    {
        get
        {
            int pt = Common.GetQueryStringValue("pt");
            if (pt == 0) pt = 1;
            return pt;
        }
    }
}
