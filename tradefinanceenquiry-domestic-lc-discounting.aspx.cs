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
public partial class tradefinanceenquiry_domestic_lc_discounting : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_domesticlcdiscountingenquiry", "domesticlcdiscountingenquiryid");
    protected void Page_Load(object sender, EventArgs e)
    {
        Finstation.CheckFullFinstationAccess();
        if (!IsPostBack)
        {
            gblData.FillDropdown(ddlenquirymodeofpaymentid, "tbl_enquirymodeofpayment", "enquirymodeofpayment_modeofpayment",
                "enquirymodeofpayment_enquirymodeofpaymentid", "enquirymodeofpayment_enquirymodeofpaymentid in(1,2)", "enquirymodeofpayment_modeofpayment");
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
        int id = 0;
        if (!Common.IsValidEmail_Single(txtemail.Text))
        {
            lblMessage.Text = "Please enter valid email id";
            lblMessage.Visible = true;
            return;
        }
        if (!Common.IsValidMobileNo(txtmobilenumber.Text))
        {
            lblMessage.Text = "Please enter valid mobile number";
            lblMessage.Visible = true;
            return;
        }
        id = gblData.SaveForm(form, 0);

        if (id > 0)
        {
            //mfuuploaddocuments.Save(id);
            SendEmail(id);
            form.Visible = false;
            lblMessage.Text = "Thanks for submitting the details!";
            lblMessage.Visible = true;
        }
        else
        {
            lblMessage.Text = "Error occurred while saving data";
            lblMessage.Visible = true;
        }
    }
    private void SendEmail(int id)
    {
        string query = "";
        query = @"select * from tbl_domesticlcdiscountingenquiry
                LEFT JOIN tbl_enquirymodeofpayment ON enquirymodeofpayment_enquirymodeofpaymentid=domesticlcdiscountingenquiry_enquirymodeofpaymentid";

        query += " where domesticlcdiscountingenquiry_domesticlcdiscountingenquiryid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        string body = Common.GetFormattedSettingForEmail("tradefinanceenquiry-domestic-lc-discounting", dr);
        BulkEmail.SendMail_Alert("tradefinance@finrex.in", "", "", "Export Finance - Domestic LC Discounting Enquiry", body, "");
    }
    public string VersionNo
    {
        get
        {
            return ConfigurationManager.AppSettings["VersionNo"].ToString();
        }
    }
}
