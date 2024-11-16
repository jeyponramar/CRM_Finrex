using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class Charges_view : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            grid.AddUrl = "charges/add.aspx";
            lblPageTitle.Text = "View Charges";
            //RightPanel_START
            //RightPanel_END
        }
    }
    
}