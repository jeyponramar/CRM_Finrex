using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GridReport : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
        }
    }
    public string ModuleName
    {
        set
        {
            gridReport.Module = value.Replace(" ","");
        }
    }
    public void Bind()
    {
        gridReport.Report();
    }
}
