using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class commodity : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FinstationPortal obj = new FinstationPortal();
            obj.BindRate(59, ltcommodity, "~Name,Time,LTP,Change,%Change,Open,High,Low");
            obj.BindRate(60, ltIndices, "");
            obj.BindRate(61, ltIndiceFutures, "");
            obj.BindRate(62, ltGovernmenrBond, "");
        }
    }
}
