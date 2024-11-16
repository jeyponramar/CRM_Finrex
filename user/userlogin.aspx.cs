using System.Collections.Generic;
using WebComponent;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Data;
using System.Data.SqlClient;

public partial class configure_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            CustomSession.InitSession(8);
        }
        if (Convert.ToString(Request.UrlReferrer).Contains("default.aspx") || Convert.ToString(Request.UrlReferrer).EndsWith("/"))
        {
            ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>window.parent.location = '../userlogin.aspx'</script>");
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string url1 = Request.Url.AbsoluteUri;
        if (url1.Contains("&review=true"))
        {
            url1 = "&review=true";
        }
        else if (url1.Contains("&comment=true"))
        {
            url1 = "&comment=true";
        }
        else
        { url1 = ""; }

        InsertUpdate obj = new InsertUpdate();
        string query = "select customer_firstname+' '+customer_lastname AS FullName,* from tbl_customer where customer_emailid='" + global.CheckData(txtUserName.Text) + "' and customer_password='" + global.CheckData(txtPassword.Text) + "'";
        DataRow dr = obj.ExecuteSelectRow(query);
        if (dr != null)
        {
            string url = Request.QueryString["url"];
            int customertid = GlobalUtilities.ConvertToInt(dr["customer_customerid"]);
            string strFullName = Convert.ToString(dr["FullName"]);
            string strEmailId = Convert.ToString(dr["customer_emailid"]);
            CustomSession.Session("Login_Customerid", customertid);
            CustomSession.Session("Customer_FullName", strFullName);
            CustomSession.Session("Customer_EmailId", strEmailId);
            CustomSession.Session("Customer_IsLogin", true);
            global.SetUser(Convert.ToInt32(dr["customer_customerid"]), strFullName, strEmailId);
            if (GlobalUtilities.IsQueryStringBlank("url"))
            {
                Response.Redirect("~/home/default.aspx");
            }
            else
            {
                HttpContext.Current.Response.Redirect("~/" + url + url1);
            }            
            CartDAO cart = new CartDAO();
            cart.UpdateCart(global.SessionId, customertid);
            cart = null;           
        }
        else
        {
            lblError.Text = "Invalid User Name/Password";
        }
    }
}

