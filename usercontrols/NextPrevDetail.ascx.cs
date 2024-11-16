using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class NextPrevDetail : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] == null)
            {
                this.Visible = false;
            }
        }
    }
    protected void btnPrev_Click(object sender, EventArgs e)
    {
        int id = GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
        string m = Common.GetModuleName();
        string idcolumn = m + "_" + m + "id";
        string query = "select top 1 " + idcolumn + " from tbl_" + m + " where " + idcolumn + "<" + id + " order by " + idcolumn + " desc";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            string script = "<script>alert('No data found!');</script>";
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "alert", script);
        }
        else
        {
            if (AppConstants.IsMobile)
            {
                Response.Redirect("~/mobile/" + m + "/add.aspx?id=" + GlobalUtilities.ConvertToInt(dr[idcolumn]));
            }
            else
            {
                Response.Redirect("~/" + m + "/add.aspx?id=" + GlobalUtilities.ConvertToInt(dr[idcolumn]));
            }
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        int id = GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
        string m = Common.GetModuleName();
        string idcolumn = m + "_" + m + "id";
        string query = "select top 1 " + idcolumn + " from tbl_" + m + " where " + idcolumn + ">" + id + " order by " + idcolumn;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            string script = "<script>alert('No data found!');</script>";
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "alert", script);
        }
        else
        {
            if (AppConstants.IsMobile)
            {
                Response.Redirect("~/mobile/" + m + "/add.aspx?id=" + GlobalUtilities.ConvertToInt(dr[idcolumn]));
            }
            else
            {
                Response.Redirect("~/" + m + "/add.aspx?id=" + GlobalUtilities.ConvertToInt(dr[idcolumn]));
            }
        }
    }
    private void BindCurrentPage()
    {

    }
}
