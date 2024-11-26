using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WebComponent;
public partial class Addfincategory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        int clientUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
        Hashtable hstbl = new Hashtable();
        hstbl.Add("categoryname", txtcategoryname.Text);
        hstbl.Add("clientid", clientId);
        hstbl.Add("clientuserid", clientUserId);
        InsertUpdate obj = new InsertUpdate();
        int findoccategoryId = obj.InsertData(hstbl, "tbl_findoccategory");
        if (findoccategoryId > 0)
        {
            lblmessage.Text = "Data saved successfully";
        }
    }
}
