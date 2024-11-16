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

public partial class CommodityMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GlobalUtilities.ConvertToBool(Session["Login_IsLoggedIn"]) || !Finstation.IsMetalCommodityEnabled())
        {
            Response.Redirect("~/customerlogin.aspx");
        }
        SetMenu();
        string url = Request.Url.ToString().ToLower();
        if (url.Contains("/commodity-metal.aspx") || url.Contains("/commodity.aspx") || url.Contains("/commodity-historicaldata.aspx"))
        {
            tblpage.Style.Add("width", "98%");
        }
        else
        {
            tblpage.Style.Add("width", "80%");
        }
    }
    private void SetMenu()
    {
        string script = "<script>$(document).ready(function(){setMenu('" + Common.GetQueryString("m") + "');});</script>";
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "setmenu", script);
    }
    public string VersionNo
    {
        get
        {
            return ConfigurationManager.AppSettings["VersionNo"].ToString();
        }
    }
}
