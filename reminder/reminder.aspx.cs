using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;

public partial class reminder_reminder : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_followups", "followupsid");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] == "undefined") return;
            gblData.PopulateForm(form, Convert.ToInt32(Request.QueryString["id"]));
            if (Request.QueryString["qsv"] != null)
            {
                if (GlobalUtilities.ConvertToString(Request.QueryString["qsv"]) == "telecalling")
                {
                    btnGoto.Text = "Go To TeleCalling Page";
                    trcustomerName.Visible = false;
                }
                else if (GlobalUtilities.ConvertToString(Request.QueryString["qsv"]) == "enquiry")
                {
                    btnGoto.Text = "Go To Enquiry Page";
                }

            }
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('" + lblPageTitle.Text + "')</script>");
    }
    protected void btnGoto_Click(object sender, EventArgs e)
    {
        string query = "update tbl_followups set followups_isremoved=1  where followups_followupsid=" + GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
        InsertUpdate obj = new InsertUpdate();
        obj.ExecuteQuery(query);

        if (Request.QueryString["qsv"] != null)
        {
            int mid = GlobalUtilities.ConvertToInt(Request.QueryString["mid"]);
            if (mid == 0)
            {
                string script = "<script>parent.closeTab();</script>";
                ClientScript.RegisterClientScriptBlock(typeof(Page), "closetab", script);
                return;
            }
            if (GlobalUtilities.ConvertToString(Request.QueryString["qsv"]) == "telecalling")
            {
                Response.Redirect("~/telecalling/add.aspx?qsv=todaysfollowups&id=" + mid);
            }
            else if (GlobalUtilities.ConvertToString(Request.QueryString["qsv"]) == "enquiry")
            {
                Response.Redirect("~/enquiry/add.aspx?id=" + mid);
            }
        }
    }
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        string query = "update tbl_followups set followups_isremoved=1  where followups_followupsid=" + GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
        InsertUpdate obj = new InsertUpdate();
        obj.ExecuteQuery(query);
        string script = "<script>parent.closeTab();</script>";
        ClientScript.RegisterClientScriptBlock(typeof(Page), "closetab", script);
    }
}
