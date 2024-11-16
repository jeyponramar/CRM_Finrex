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
public partial class tradefinanceenquiry_supplier_credit : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_suppliercreditenquiry", "suppliercreditenquiryid");
    protected void Page_Load(object sender, EventArgs e)
    {
        Finstation.CheckFullFinstationAccess();
        if (!IsPostBack)
        {
            gblData.FillDropdown(ddlgoodstypeid, "tbl_goodstype", "goodstype_goodstype", "goodstype_goodstypeid", "", "goodstype_goodstype");
            gblData.FillDropdown(ddllcstatusid, "tbl_lcstatus", "lcstatus_lcstatus", "lcstatus_lcstatusid", "", "lcstatus_lcstatus");
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
        query = @"select * from tbl_suppliercreditenquiry
                     LEFT JOIN tbl_goodstype ON goodstype_goodstypeid=suppliercreditenquiry_goodstypeid
                     LEFT JOIN tbl_lcstatus ON lcstatus_lcstatusid=suppliercreditenquiry_lcstatusid
                     LEFT JOIN tbl_enquirymodeofpayment ON enquirymodeofpayment_enquirymodeofpaymentid=suppliercreditenquiry_enquirymodeofpaymentid";

        query += " where suppliercreditenquiry_suppliercreditenquiryid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        string body = Common.GetFormattedSettingForEmail("tradefinanceenquiry-supplier-credit", dr);
        Response.Write(body);
        Response.End();
        BulkEmail.SendMail_Alert("tradefinance@finrex.in", "", "", "Import Finance - Supplier Credit Enquiry", body, "");
    }
    public string VersionNo
    {
        get
        {
            return ConfigurationManager.AppSettings["VersionNo"].ToString();
        }
    }
}
