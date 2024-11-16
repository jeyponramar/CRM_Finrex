using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class exportexposureportal_ExportPortalMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!GlobalUtilities.ConvertToBool(CustomSession.Session("Login_IsLoggedIn")))
        {
            Response.Redirect("~/customerlogin.aspx");
        }
        if (!IsPostBack)
        {
            if (Finstation.IsExportPortalEnabled() || Finstation.IsImportPortalEnabled())
            {
            }
            else
            {
                Response.Redirect("~/customerlogin.aspx");
            }
        }
    }
    public string VersionNo
    {
        get
        {
            return ConfigurationManager.AppSettings["VersionNo"].ToString();
        }
    }
}
