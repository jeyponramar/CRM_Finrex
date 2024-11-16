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
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
public partial class emailSignature : System.Web.UI.Page
{
    GlobalData gbl = new GlobalData("tbl_user", "userid");
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateSignature();
            lblPageTitle.Text = "Signature";

            ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
        }
    }
    protected void btn_SubmitClick(object sender, EventArgs e)
    {
        Hashtable hstbl= new Hashtable();
        hstbl.Add("textsignature",tmessage.Text);
        hstbl.Add("htmlsignature", Email_Body.Text);       
        int id = gbl.UpdateForm(form1, hstbl, Convert.ToInt32(CustomSession.Session("Login_UserId")));
        if (id > 0)
        {
            message.Visible = true;
            message.Text = "Signature saved Sucessfully";
        }
        //gbl.SaveForm(form1);
    }
    private void PopulateSignature()
    {
        InsertUpdate obj = new InsertUpdate();
        string query = "";
        query = "SELECT * FROM tbl_user WHERE user_userid="+Convert.ToInt32(CustomSession.Session("Login_UserId"));
        DataRow dr = obj.ExecuteSelectRow(query);
        if (dr != null)
        {
            try
            {
                Email_Body.Text = Convert.ToString(dr["user_htmlsignature"]);
                tmessage.Text = Convert.ToString(dr["user_textsignature"]);
            }
            catch (Exception ex) { }
        }
    }
}
