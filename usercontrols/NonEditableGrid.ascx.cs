using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NonEditableGrid : System.Web.UI.UserControl
{
    string _Module;
    string _SubGridName;
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
    public string SubGridName
    {
        set
        {
            _SubGridName = value;
        }
        get
        {
            return _SubGridName;
        }
    }  

    protected void Page_Load(object sender, EventArgs e)
    {
        //XMLNodeBinder.loadXMLDocument();
        //NonEditableSubGrid.bindGrid(this.Parent, ltNonEditableGrid, _SubGridName);        
        ltNonEditableGrid.Text = "";
        if (!IsPostBack)
        {
            //NonEditableSubGrid._SubGridName = _SubGridName;
            XMLNodeBinder.loadXMLDocument();            
            NonEditableSubGrid.bindGrid(this.Parent, ltNonEditableGrid, _SubGridName);
        }
        

    }
    
}
