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

public partial class trackclient_add : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gridopportunity.Visible = false;
            gridSales.Visible = false;
            gridenquiry.Visible = false;
            gridQuotation.Visible = false;
            gridinvoice.Visible = false;
            gridenquiryfollowups.Visible = false;
            gridfollowup.Visible = false;
            gridopportunityfollowups.Visible = false;
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('Track Client')</script>");
    }
    protected void btnTrack_Click(object sender, EventArgs e)
    {
        int clientId = GlobalUtilities.ConvertToInt(txtclientid.Text);
        if (clientId == 0)
        {
            lblMessage.Text = "Invalid Client";
            return;
        }

        gridenquiry.ExtraWhere = " where enquiry_enquirystatusid in (1,10) And enquiry_clientid=" + clientId;
        gridenquiry.InitGrid();
        gridenquiry.LoadGrid();
        gridenquiry.EnableAddLink = false;

        gridopportunity.ExtraWhere = " where opportunity_opportunitystatusid=1 And opportunity_clientid=" + clientId;
        gridopportunity.InitGrid();
        gridopportunity.LoadGrid();
        gridopportunity.EnableAddLink = false;

        gridQuotation.ExtraWhere = " where quotation_quotationstatusid=1 And quotation_clientid=" + clientId;
        gridQuotation.InitGrid();
        gridQuotation.LoadGrid();
        gridQuotation.EnableAddLink = false;

        gridSales.ExtraWhere = "  where sales_salesstatusid=1 And sales_clientid=" + clientId;
        gridSales.InitGrid();
        gridSales.LoadGrid();
        gridSales.EnableAddLink = false;

        gridinvoice.ExtraWhere = "  where invoice_balanceamount&gt;0 And invoice_clientid=" + clientId;
        gridinvoice.InitGrid();
        gridinvoice.LoadGrid();
        gridinvoice.EnableAddLink = false;

        gridenquiryfollowups.ExtraWhere = "  where followups_module='enquiry' And followups_clientid=" + clientId;
        gridenquiryfollowups.InitGrid();
        gridenquiryfollowups.LoadGrid();
        gridenquiryfollowups.EnableAddLink = false;

        gridopportunityfollowups.ExtraWhere = "  where followups_module='opportunity' And followups_clientid=" + clientId;
        gridopportunityfollowups.InitGrid();
        gridopportunityfollowups.LoadGrid();
        gridopportunityfollowups.EnableAddLink = false;

        gridfollowup.ExtraWhere = "  where followups_module=' ' And followups_clientid=" + clientId;
        gridfollowup.InitGrid();
        gridfollowup.LoadGrid();
        gridfollowup.EnableAddLink = false;
    }
}
