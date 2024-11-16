using System;
using System.Collections.Generic;
using WebComponent;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Configuration;

public partial class InnerMaster : System.Web.UI.MasterPage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        AppConstants.CustomSettings = new CustomSettings();
        Common.ValidateAjaxRequest();
        string url = Request.Url.ToString().ToLower();

        if (url.Contains("adminlogin.aspx") || url.Contains("error.aspx") || url.Contains("message.aspx"))
        {
        }
        else
        {
            if (CustomSession.Session("Login_IsRefuxLoggedIn") == null)
            {
                if (Request.QueryString["url"] == null)
                {
                    Response.Redirect("~/adminlogin.aspx");
                }
                else
                {
                    Response.Redirect("~/adminlogin.aspx?url=" + Request.QueryString["url"]);
                }
            }
            else
            {
                if (Common.RoleId == 10 || Common.RoleId == 11)
                {
                    if (Common.GetModuleName() != "advisor")
                    {
                        Response.Redirect("~/adminlogin.aspx");
                    }
                }
            }
            WriteLoginInfo();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        WriteLoginInfo();
        if (!IsPostBack)
        {
            //CheckRights();
            SetCurrentPageOnCookie();
            PopulateHistory();
        }
    }
    private void CheckRights()
    {
        string url = Request.Url.ToString().ToLower();
        if (url.Contains("/add.aspx"))
        {
            Common.CheckRightsForCreate();
        }
        else if (url.Contains("/view.aspx"))
        {
            Common.CheckRightsForView();
        }

    }
    private void SetCurrentPageOnCookie()
    {
        string url = Common.GetPageUrl();
        string script = "<script>$(document).ready(function(){parent.setCurrentPage('" + url + "');});</script>";
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "currentpage", script);
    }
    private void WriteLoginInfo()
    {
        string url = Request.Url.ToString().ToLower();
        string module = "";
        if (url.Contains("add.aspx"))
        {
            Array arr = url.Split('/');
            module = Convert.ToString(arr.GetValue(arr.Length - 2));
        }
        int mid = GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
        string script = "<script>";
        script += "var __LOGIN_FIRSTNAME='" + CustomSession.Session("Login_FirstName") + "';";
        script += " __LOGIN_USERID='" + CustomSession.Session("Login_UserId") + "';";
        script += "var __Login_RoleId='" + CustomSession.Session("Login_RoleId") + "';";
        script += "_module = '"+module+"';";
        script += "_moduleId = '" + mid + "';";
        script += "</script>";
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "loginInfo", script);
    }
    private void PopulateHistory()
    {

        trHistory.Visible = false;
        try
        {
            if (Request.QueryString["id"] == null)
            {
                trHistory.Visible = false;
                return;
            }
            string url = Request.Url.ToString().ToLower();
            if (url.Contains("add.aspx"))
            {
                Array arr = url.Split('/');
                string module = Convert.ToString(arr.GetValue(arr.Length - 2));
                int id = GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
                string query = "select u." + module + "_createddate,u." + module + "_modifieddate,u1.user_fullname as cname,u2.user_fullname as mname,u." +
                               module + "_createdby,u." + module + "_modifiedby from tbl_" + module +
                               " u left join tbl_user u1 on u1.user_userid=u." + module + "_createdby " +
                               " left join tbl_user u2 on u2.user_userid=u." + module + "_modifiedby " +
                               " where u." + module + "_" + module + "id=" + id;
                InsertUpdate obj = new InsertUpdate();
                DataRow dr = obj.ExecuteSelectRow(query);
                if (dr != null)
                {
                    StringBuilder html = new StringBuilder();
                    html.Append("<table cellspacing=5><tr><td>Created By : </td>");
                    int userid = GlobalUtilities.ConvertToInt(dr[module + "_createdby"]);
                    if (userid > 0)
                    {
                        html.Append("<td class='spage val' href='#user/add.aspx?id=" + userid + "'>" + Convert.ToString(dr["cname"]) + "</td>" +
                                "<td><img src='../images/user/thumb/" + userid + ".jpg' height='30' class='imguser'/></td>");
                    }
                    else
                    {
                        html.Append("<td>&nbsp;</td>");
                    }
                    html.Append("<td>Created Date : </td><td class='val'>" + GlobalUtilities.ConvertToDateTime(dr[module + "_createddate"]) + "</td>");
                    userid = GlobalUtilities.ConvertToInt(dr[module + "_modifiedby"]);
                    if (userid > 0)
                    {
                        html.Append("<td style='padding-left:20px'>Last Modified By : </td><td class='spage val' href='#user/add.aspx?id=" + userid + "'>" + Convert.ToString(dr["mname"]) + "</td>" +
                                "<td><img src='../images/user/thumb/" + userid + ".jpg' class='imguser' height='30'/></td>");
                    }
                    else
                    {
                        html.Append("<td style='padding-left:20px'>Last Modified By : </td><td>&nbsp;</td>");
                    }
                    html.Append("<td>Last Modified Date : </td><td class='val'>" + GlobalUtilities.ConvertToDateTime(dr[module + "_modifieddate"]) + "</td>");
                    html.Append("</tr></table>");
                    ltDocumentHistory.Text = html.ToString();
                    trHistory.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
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
