using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;
using System.Collections;

public partial class viewbankaudit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Finstation.CheckBankScanFinstationAccess();
    }
    protected void btnaddbankaudit_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/addbankaudit.aspx");
    }
}
