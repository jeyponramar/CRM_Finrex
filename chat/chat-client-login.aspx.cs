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

public partial class chat_chat_client : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Hashtable hstbl = new Hashtable();
        hstbl.Add("name", txtName.Text);
        //hstbl.Add("companyname", txtCompanyName.Text);
        //hstbl.Add("mobileno", txtMobileNo.Text);
        //hstbl.Add("emailid", txtEmailId.Text);
        InsertUpdate obj = new InsertUpdate();
        int chatId = obj.InsertData(hstbl, "tbl_chat");
        

    }
}
