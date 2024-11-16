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

public partial class commonaddmodalpage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            f.Action = "/";
            //if (!GlobalUtilities.ConvertToBool(Session["Login_IsLoggedIn"]))
            if (Common.ClientUserId == 0 && Common.UserId == 0)
            {
                Response.End();
                return;
            }
            if (AppConstants.IsLive)
            {
                if (Request.UrlReferrer == null)
                {
                    Response.End();
                    return;
                }
            }
            BindDetails();
        }
    }
    private void BindDetails()
    {
        string modal = Common.GetQueryString("modal");
        if (modal == "kycclientownercontact")
        {
            plkycclientownercontact.Visible = true;
            BindContactDetails(plkycclientownercontact);
        }
        else if (modal == "kycclientfinancecontact")
        {
            plkycclientfinancecontact.Visible = true;
            BindContactDetails(plkycclientfinancecontact);
        }
        else if (modal == "kycbankdetail")
        {
            plkycbankdetail.Visible = true;
            BindBankDetail();
        }
    }
    private void BindContactDetails(PlaceHolder pl)
    {
        string query = "";
        int id = Common.GetQueryStringValue("id");
        query = @"select * from tbl_contacts where contacts_contactsid=" + id;
        query += " and contacts_clientid=" + ClientId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        GlobalData gbldata = new GlobalData();
        gbldata.PopulateForm(dr, pl);
    }
    private void BindBankDetail()
    {
        string query = "";
        int id = Common.GetQueryStringValue("id");
        if (id == 0) return;
        query = @"select * from tbl_kycbankdetail where kycbankdetail_kycbankdetailid=" + id;
        query += " and kycbankdetail_clientid=" + ClientId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        GlobalData gbldata = new GlobalData();
        gbldata.PopulateForm(dr, plkycbankdetail);
        int bankId = GlobalUtilities.ConvertToInt(dr["kycbankdetail_bankauditbankid"]);
        DataRow drb = DbTable.GetOneRow("tbl_bankauditbank", bankId);
        bankauditbank.Text = GlobalUtilities.ConvertToString(drb["bankauditbank_bankname"]);
    }
    private int ClientId
    {
        get
        {
            if (IsClientLoggedIn)
            {
                return Common.ClientId;
            }
            else
            {
                return Common.GetQueryStringValue("clientid");
            }
        }
    }
    private bool IsClientLoggedIn
    {
        get
        {
            if (LoggedInUserId > 0) return false;
            return true;
        }
    }
    private int LoggedInUserId
    {
        get { return Common.UserId; }
    }
}
