using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Text;
using System.Data;

public partial class Followups : System.Web.UI.UserControl
{
    GlobalData gblData = new GlobalData("tbl_followup", "followupid");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] == null)
        {
            this.Visible = false;
            grid.Visible = false;
            return;
        }
        //grid.gridDataRowBind += new GridDataRowBind(grid_gridDataRowBind);
        

        if (!IsPostBack)
        {
            TextBox txtClientId = (TextBox)Parent.FindControl("txtclientid");
            string strClientId = "0";
            if (txtClientId != null)
            {
                strClientId = txtClientId.Text;
            }
            TextBox txtmarketingpersonid = (TextBox)Parent.FindControl("txtemployeeid");
            string strMarketingPersonId = "0";
            if (txtmarketingpersonid != null)
            {
                strMarketingPersonId = txtmarketingpersonid.Text;
            }
            grid.AddUrl = "#followups/add.aspx?m=" + Module.Replace("followups_", "") + "&mid=" + Request.QueryString["id"] + "&clientid=" + strClientId + "&marketingpersonid=" + strMarketingPersonId;

            if (Request.QueryString["id"] != null)
            {
                string module = Common.GetModuleName().ToLower();
                int id=GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
                int enquiryId = 0;
                if (module == "enquiry")
                {
                    enquiryId = id;   
                }
                else if (module == "opportunity")
                {
                    enquiryId = GlobalUtilities.ConvertToInt(DbTable.GetOneColumnData("tbl_opportunity", "opportunity_enquiryid", id));

                }
                grid.ExtraWhere = "followups_module='enquiry' and followups_mid=" + enquiryId;
                grid.AddUrl = "#followups/add.aspx?m=enquiry&mid=" + enquiryId + "&clientid=" + strClientId + "&marketingpersonid=" + strMarketingPersonId;
            }
        }
    }
    public bool EnableAddlink
    {
        set
        {
            grid.EnableAddLink = value;
        }
    }
    public string Module
    {
       
        set
        {
            ViewState["FollowupsModule"] = value;
        }
        get
        {
            return Convert.ToString(ViewState["FollowupsModule"]);
        }
    }
    public bool grid_gridDataRowBind(DataTable dttbl, int i, StringBuilder html)
    {
        string imgUser = GlobalUtilities.CreatedByUserThumbImage(GlobalUtilities.ConvertToInt(dttbl.Rows[i]["followups_createdby"]));

        string action = Convert.ToString(dttbl.Rows[i]["followupaction_action"]);
        string icon = "<img src='../images/" + action + ".png' title='" + action + "'/>";
        string employeeName = "";

        int employeeId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["followups_employeeid"]);
        if (employeeId > 0)
        {
            employeeName = Common.GetOneColumnData("tbl_employee", employeeId, "employeename");
        }
        html.Append("<tr><td><table width='100%'>" +
                    "<tr><td style='background-color:#fcfbfb;border:solid 1px #e8e6e6;' class='corner'>" +
                    "<table width='100%'>" +
                        "<tr>" +
                        "<td valign='top' width='50'>" + imgUser + "</td>" +
                        "<td><table width='100%'>" +
                            "<tr><td colspan='5'><a class='page follow-title fol-page' href='#followups/add.aspx?id=" + GlobalUtilities.ConvertToInt(dttbl.Rows[i]["followups_followupsid"]) + "'>" + Convert.ToString(dttbl.Rows[i]["followups_subject"]) + "</a></td>" +
                                 "<td align='right'>" + icon + "</td>" +
                            "</tr>" +
                            "<tr><td><table><tr>" +
                               "<td>Action :</td><td class='bold'>" + Convert.ToString(dttbl.Rows[i]["followupaction_action"]) + "</td>" +
                               "<td style='padding-left:20px;'>Date :</td><td class='bold'>" +
                                                GlobalUtilities.ConvertToDateTime(dttbl.Rows[i]["followups_date"]) + "</td>" +
                               "<td style='padding-left:20px;'>Status :</td><td class='bold'>" + Convert.ToString(dttbl.Rows[i]["followupstatus_status"]) + "</td>" +
                               "<td style='padding-left:20px;'>Assigned To :</td><td class='bold'>" + employeeName + "</td>" +
                               "</tr></table></td>" +
                            "</tr>" +
                            "<tr><td colspan='10'>" + Convert.ToString(dttbl.Rows[i]["followups_remarks"]) + "</td></tr>" +
                        "</table></td>" +
                    "</table>" +
                    "</td></tr>" +
                   "</table></td></tr>");
        return true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

    }
}
