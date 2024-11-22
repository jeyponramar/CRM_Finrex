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
public partial class AddFindoc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        int clientUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
        Hashtable hstbl = new Hashtable();
        hstbl.Add("findocdepartmentid", txtfindocdepartmentid.Text);
        hstbl.Add("findoccategoryid", txtfindoccategoryid.Text);
        hstbl.Add("findocsubcategoryid", txtfindocsubcategoryid.Text);
        hstbl.Add("findocdocumenttypeid", txtfindocdocumenttypeid.Text);
        hstbl.Add("clientid",clientId);
        hstbl.Add("clientuserid",clientUserId);
        hstbl.Add("subject", txtsubject.Text);
        hstbl.Add("uploaddate", "getdate()");
        InsertUpdate obj = new InsertUpdate();
        int findocdocumentId = obj.InsertData(hstbl, "tbl_findocdocument");
        if (findocdocumentId > 0)
        {
            lblmessage.Text = "Data saved successfully";
        }

    }
}
