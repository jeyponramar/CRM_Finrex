using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WebComponent;

public partial class buy_sell_scrips_enquiry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GlobalData gbldata = new GlobalData();
            gbldata.FillDropdown(ddlbuysellscripttypeid, "tbl_buysellscripttype", "buysellscripttype_scriptname", "");
            BindUserDetails();
        }
    }
    private void BindUserDetails()
    {
        DataRow drc = DbTable.GetOneRow("tbl_client", Common.ClientId);
        lblcompanyname.Text = GlobalUtilities.ConvertToString(drc["client_customername"]);
        string query = @"select * from tbl_contacts
                         join tbl_clientuser on clientuser_contactsid=contacts_contactsid
                         where clientuser_clientuserid=" + Common.ClientUserId;
        DataRow dru = DbTable.ExecuteSelectRow(query);
        if (dru == null) return;
        txtpersonname.Text = GlobalUtilities.ConvertToString(dru["contacts_contactperson"]);
        txtmobileno.Text = GlobalUtilities.ConvertToString(dru["contacts_mobileno"]);
        txtemailid.Text = GlobalUtilities.ConvertToString(dru["contacts_emailid"]);
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        if (rbtnisexporter.SelectedValue == "")
        {
            lblMessage.Text = "Please choose Exporter/Importer";
            return;
        }
        if (rbtnisbuy.SelectedValue == "")
        {
            lblMessage.Text = "Please choose Buy/Sell";
            return;
        }
        GlobalData gbldata = new GlobalData("tbl_buysellscripsenquiry", "buysellscripsenquiryid");
        gbldata.AddExtraValues("clientid", Common.ClientId);
        gbldata.AddExtraValues("clientuserid", Common.ClientUserId);
        gbldata.AddExtraValues("isexporter", Convert.ToInt32(rbtnisexporter.SelectedValue));
        gbldata.AddExtraValues("isbuy", Convert.ToInt32(rbtnisbuy.SelectedValue));
        int id = gbldata.SaveForm(form);
        if (id > 0)
        {
            string toEmailId = "Scrips@finrex.in";
            string subject = "Buy - Sell Scrips RODTEP/ROSCTL";
            string query = @"select * from tbl_buysellscripsenquiry
                             join tbl_client on client_clientid=buysellscripsenquiry_clientid
                             left join tbl_buysellscripttype on buysellscripttype_buysellscripttypeid=buysellscripsenquiry_buysellscripttypeid
                             where buysellscripsenquiry_buysellscripsenquiryid=" + id;
            DataRow dr = DbTable.ExecuteSelectRow(query);
            dr.Table.Columns.Add("buysell");
            dr.Table.Columns.Add("exporterimporter");
            dr["buysell"] = rbtnisbuy.SelectedValue == "1" ? "Buy" : "Sell";
            dr["exporterimporter"] = rbtnisexporter.SelectedValue == "1" ? "Exporter" : "Importer";
            string body = Common.GetFormattedSettingForEmail("Buy_Sell_Scrips_Enquiry", dr, true);
            BulkEmail.SendMail_Alert(toEmailId, subject, body, "");

            trform.Visible = false;
            lblMessage.Text = "Your request has been submitted successfully!";
        }
        else
        {
            lblMessage.Text = "Error occurred while submitting your request.";
        }
        
    }
}
