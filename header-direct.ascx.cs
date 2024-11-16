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
using System.Text;
using WebComponent;
using System.IO;

public partial class header_direct : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindOfferCategory();
        }
        BindLogin();
    }
    private void BindOfferCategory()
    {
        string query = "select * from tbl_offercategory";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<div class='cssmenu'><ul><li><a href='#'><span>OFFER ZONE</span></a><ul>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string catName = GlobalUtilities.ConvertToString(dttbl.Rows[i]["offercategory_offercategory"]);
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["offercategory_offercategoryid"]);
            html.Append("<li><a href='../offer-zone/default.aspx?id=" + id + "'>" + catName + "</a></li>");
        }
        html.Append("</ul></li></ul></div>");
        ltOfferCategory.Text = html.ToString();
    }
    private void BindLogin()
    {
        StringBuilder html = new StringBuilder();
        bool isLoggedIn = GlobalUtilities.ConvertToBool(CustomSession.Session("Customer_IsLogin"));
        if (isLoggedIn)
        {
            string customerName = CustomSession.Session("Customer_FullName");
            html.Append("<div class='cssmenu'><ul><li style='float:right'><a href='#'><span class='btnmyaccount'>Hi " + customerName + " !</span></a><ul>");
            html.Append("<li><a href='../account/track-order-status.aspx'>Track My Order</a></li>");
            html.Append("<li><a href='../account/my-orders.aspx'>My Orders</a></li>");
            html.Append("<li><a href='../account/wishlist.aspx'>My Wishlist</a></li>");
            html.Append("<li><a href='../account/myprofile.aspx'>My Profile</a></li>");
            html.Append("<li><a href='../account/change-password.aspx'>Change Password</a></li>");
            html.Append("<li><a href='../account/userlogout.aspx'>Logout</a></li>");
            html.Append("</ul></li></ul></div>");
        }
        else
        {
            html.Append("<table class='tbllogin' cellspacing=5><tr><td><a href='../account/registration.aspx'>Sign Up</td><td><div class='toplinksep'>&nbsp;</div></td><td><a href='../account/userlogin.aspx'>Login</td></tr></table>");
        }
        ltLogin.Text = html.ToString();
    }
}
