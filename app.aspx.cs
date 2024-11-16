using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;

public partial class app : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string app = "";
        app = "p=" + AppConstants.ProjectName + "<br/>";
        app += "c=" + AppConstants.CompanyName + "<br/>";
        app += "d=" + GlobalUtilities.ConvertToDate(AppConstants.RenewDate) + "<br/>";
        app += "u=" + AppConstants.TotalUsers + "<br/>";
        Response.Write(app);
    }
}
