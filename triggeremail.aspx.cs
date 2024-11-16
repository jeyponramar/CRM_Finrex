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
using System.Web.Caching;
using WebComponent;

public partial class triggeremail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (CustomSession.Session("Login_IsRefuxLoggedIn") == null || GlobalUtilities.ConvertToBool(Session["IsDemoSample"]))
        //{
        //    Response.Redirect("~/adminlogin.aspx");
        //}
        //if (!IsPostBack)
        //{
        //    Timer();
        //}
        //if (Cache["EmailReminder"] == null)
        //{
        //    Cache.Insert("EmailReminder", "reminder", null, DateTime.Now.AddSeconds(5),
        //                System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High,
        //                new CacheItemRemovedCallback(CacheRemovedCallback));
        //}
    }
    //public void CacheRemovedCallback(String key, object value, System.Web.Caching.CacheItemRemovedReason removedReason)
    //{
    //    try
    //    {
    //        Timer();
    //    }
    //    catch { }
    //    Cache.Insert("EmailReminder", "reminder", null, DateTime.Now.AddSeconds(5),
    //                System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High,
    //                new CacheItemRemovedCallback(CacheRemovedCallback));
    //}
    //private void Timer()
    //{
    //    EmailReminder.TriggerEmail();
    //}
}
