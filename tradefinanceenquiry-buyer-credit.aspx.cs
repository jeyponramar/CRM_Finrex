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
public partial class tradefinanceenquiry_buyer_credit : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_buyercreditenquiry", "buyercreditenquiryid");
    protected void Page_Load(object sender, EventArgs e)
    {
        Finstation.CheckFullFinstationAccess();
        if (!IsPostBack)
        {
            gblData.FillDropdown(ddlgoodstypeid, "tbl_goodstype", "goodstype_goodstype", "goodstype_goodstypeid", "", "goodstype_goodstype");
            gblData.FillDropdown(ddltypeofbuyerscreditid, "tbl_typeofbuyerscredit", "typeofbuyerscredit_typeofbuyerscredit", "typeofbuyerscredit_typeofbuyerscreditid", "", "typeofbuyerscredit_typeofbuyerscredit");
            gblData.FillDropdown(ddlenquirymodeofpaymentid, "tbl_enquirymodeofpayment", "enquirymodeofpayment_modeofpayment", "enquirymodeofpayment_enquirymodeofpaymentid", "", "enquirymodeofpayment_modeofpayment");
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
            string query = "";
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
        query = @"select * from tbl_buyercreditenquiry
 LEFT JOIN tbl_goodstype ON goodstype_goodstypeid=buyercreditenquiry_goodstypeid
 LEFT JOIN tbl_typeofbuyerscredit ON typeofbuyerscredit_typeofbuyerscreditid=buyercreditenquiry_typeofbuyerscreditid
 LEFT JOIN tbl_enquirymodeofpayment ON enquirymodeofpayment_enquirymodeofpaymentid=buyercreditenquiry_enquirymodeofpaymentid";

        query += " where buyercreditenquiry_buyercreditenquiryid=" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        string body = Common.GetFormattedSettingForEmail("tradefinanceenquiry-buyer-credit", dr);
        //Response.Write(body);
        //Response.End();
        BulkEmail.SendMail_Alert("tradefinance@finrex.in", "", "", "Import Finance - Buyer Credit Enquiry", body, "");
    }
    public string VersionNo
    {
        get
        {
            return ConfigurationManager.AppSettings["VersionNo"].ToString();
        }
    }
}
