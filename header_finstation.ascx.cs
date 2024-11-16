using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class header_finstation : System.Web.UI.UserControl
{
    bool _isCommodity = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        bool isLoggedIn = GlobalUtilities.ConvertToBool(Session["Login_IsLoggedIn"]);
        if (!IsPostBack)
        {
            if (isLoggedIn)
            {
                BindMarqueNews();
                lblUserName.Text = GlobalUtilities.ConvertToString(Session["Login_Name"]);

                if (lblUserName.Text != "")
                {
                    lbluserinitial2.Text = lblUserName.Text.Substring(0, 1).ToUpper();
                }
                //lbluserinitial2.Text = lblUserName.Text.Substring(0, 1);

                tblLoginSection.Visible = true;
                string html = "<div class='jq-currentdate'></div>";
                if (GlobalUtilities.ConvertToBool(CustomSession.Session("Login_IsAdmin")))
                {
                    html = "<div class='jq-currentdate' title='API - Yellow, XL - Green, Calc - Blue'></div>";
                }
                ltctdate.Text = html;
            }
            if (!_isCommodity)
            {
                if (Finstation.IsFinstationEnabled() || Finstation.IsMiniFinstationEnabled())
                {
                    ltappname.Text = @"<a href='index.aspx' style='color:#32347e;text-decoration:none;'>FinStation</a><div class='header-poweredby'>powered by <img src='images/finrex.png' height='15'/></div>";
                }
                else
                {
                    ltappname.Text = @"<a href='viewbankaudit.aspx' style='color:#32347e;text-decoration:none;'>BankScan</a>";
                }
            }
        }
    }
    public bool IsCommodity
    {
        set
        {
            if (value)
            {
                _isCommodity = true;
                //div_setalert.Visible = false;
                div_finwatch.Visible = false;
                ltappname.Text = @"<a href='commodity-metal.aspx' style='color:#32347e;text-decoration:none;white-space:nowrap;'>MetStation</a>";
            }
        }
    }
    private void BindMarqueNews()
    {
        string news = Common.GetSetting("Scrolling News");
        if (news != "")
        {
            ltMarque.Text = "<div class='marque-news1' style='height:30px;padding-bottom:5px;'><marquee style='height:30px;'>" + news + "</marquee></div>";
        }
    }
    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        CustomSession.Delete();
        Session.Abandon();
        Response.Redirect("~/customerlogin.aspx");
    }
   
}
