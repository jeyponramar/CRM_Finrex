using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class utilities_view : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Module = Request.QueryString["m"];
            grid.Module = Module;
            
            if (Request.QueryString["title"] == null)
            {
                GridSettings gridSettings = new GridSettings();
                gridSettings.BindXml(Module);
                lblPageTitle.Text = gridSettings.Title;
            }
            else
            {
                lblPageTitle.Text = Request.QueryString["title"];
                
            }
            string ew = GlobalUtilities.ConvertToString(Request.QueryString["ew"]);
            if (ew == "")
            {
                if (Session["ew"] != null)
                {
                    ew = GlobalUtilities.ConvertToString(Session["ew"]);
                    ViewState["ew"] = ew;
                    Session["ew"] = null;
                }
                else if (ViewState["ew"] != null)
                {
                    ew = GlobalUtilities.ConvertToString(ViewState["ew"]);
                }
            }
            //Response.Write(ew);
            if (ew != "")
            {
                string extraWhere = ew.Replace("~", "=");
                grid.ExtraWhere = extraWhere;
            }
            
            if (GlobalUtilities.ConvertToBool(Request.QueryString["hidepaging"]))
            {
                grid.HidePaging();
            }
            
            //RightPanel_START
            //RightPanel_END
        }
    }
    
}