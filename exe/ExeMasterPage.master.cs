using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using WebComponent;
public partial class exe_ExeMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = Request.Url.ToString().ToLower();
        if (!IsPostBack)
        {
            if (!url.Contains("/exe/login.aspx"))
            {
                string sessionId = global.CheckInputData(Common.GetQueryString("sid"));
                hsessionid.Text = sessionId;
                string query = "select * from tbl_clientuser WHERE clientuser_isactive=1 and clientuser_exesessionid='" + sessionId + "'";//<1 min heart beat
                DataRow dr = DbTable.ExecuteSelectRow(query);
                if (sessionId == "" || dr == null)
                {
                    Response.Redirect("~/exe/login.aspx?header=" + Request.QueryString["header"]);
                }
                else
                {
                    int clientUserId = GlobalUtilities.ConvertToInt(dr["clientuser_clientuserid"]);
                    if (Session["IsClientLoginSaved"] == null)
                    {
                        Common.SaveClientUserHistory(clientUserId, 2);
                        Session["IsClientLoginSaved"] = true;
                    }
                }
                hdnsessionid.Text = sessionId;
                //lnkconfig.NavigateUrl = "~/exe/config-currency.aspx?sid=" + sessionId;
            }
            //if (url.ToLower().Contains("/exe/default.aspx"))
            //{
            //    tdsetting.Visible = true;
            //}
            if (Request.QueryString["header"] == "false")
            {
                trheader.Style.Add("visibility", "hidden");
            }
        }
    }
    public string VersionNo
    {
        get
        {
            return ConfigurationManager.AppSettings["VersionNo"].ToString();
        }
    }
}
