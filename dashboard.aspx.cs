using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class home_dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        addExtraWhereOnGrid();
    }
    private void addExtraWhereOnGrid()
    {
        if (GlobalUtilities.ConvertToInt(CustomSession.Session("Login_RoleId")) != 1)
        {
            neworder.ExtraWhere = " and order_merchantid=$SESSION_Login_MerchantId$";
            pendingorder.ExtraWhere = " and order_merchantid=$SESSION_Login_MerchantId$";
        }
        else
        {
            trminstock.Visible = false;
            trminstockreport.Visible = false;
        }
    }
}
