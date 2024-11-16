using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;

public partial class UC_UpdatableGrid : System.Web.UI.UserControl
{
    string _Module;
    string _Name;
    public string Module
    {
        set
        {
            _Module = value;
        }
        get
        {
            return _Module;
        }
    }
    public string Name
    {
        set
        {
            _Name = value;
        }
        get
        {
            return _Name; 
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UpdatableGrid.bindGrid(this.Parent,ltUpdatableGrid,Name, ((_Module==null||Module=="")?Common.GetModuleName():_Module));  
        }
    }
    
}
