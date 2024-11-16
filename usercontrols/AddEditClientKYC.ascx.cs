using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class usercontrols_AddEditClientKYC : System.Web.UI.UserControl
{
    GlobalData gblData = new GlobalData("tbl_client", "clientid");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gblData.FillDropdown(ddlindustrytypesid, "tbl_industrytypes", "industrytypes_industrytypes", "industrytypes_industrytypesid", "", "industrytypes_industrytypes");
            //gblData.FillDropdown(ddlcontacttypeid, "tbl_contacttype", "contacttype_contacttype", "contacttype_contacttypeid", "", "contacttype_contacttype");
            gblData.FillDropdown(ddlexportannualturnoverid, "tbl_annualturnover", "annualturnover_turnover", "annualturnover_annualturnoverid", "", "annualturnover_annualturnoverid");
            gblData.FillDropdown(ddlimportannualturnoverid, "tbl_annualturnover", "annualturnover_turnover", "annualturnover_annualturnoverid", "", "annualturnover_annualturnoverid");
            gblData.FillDropdown(importingexportingcountry, "tbl_country", "country_country", "country_countryid", "", "country_country");
            BindCurrencies();
            PopulateDetails();
            hdnclientid.Text = ClientId.ToString();
            BindMultiCheckBox();
        }
        BindTab();
    }
    
    private void PopulateDetails()
    {
        string query = "";
        query = @"select * from tbl_client 
                  left join tbl_state on state_stateid=client_stateid
                  left join tbl_city on city_cityid=client_cityid
                  where client_clientid=" + ClientId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        gblData.PopulateForm(dr, form);
        //lblcustomername.Text = GlobalUtilities.ConvertToString(dr["client_customername"]);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        mcbusinessinto.AddSelectedIds_CommaSep(gblData);
        mcbusinesstype.AddSelectedIds_CommaSep(gblData);
        mcenterprisetype.AddSelectedIds_CommaSep(gblData);
        mchedgingpolicy.AddSelectedIds_CommaSep(gblData);
        mcexposuresheetmaintained.AddSelectedIds_CommaSep(gblData);
        mcforwardlimits.AddSelectedIds_CommaSep(gblData);
        mcforwardcontractbooking.AddSelectedIds_CommaSep(gblData);
        mchedgingperiod.AddSelectedIds_CommaSep(gblData);
        mctypeofbooking.AddSelectedIds_CommaSep(gblData);
        mcfrequencyofadvisory.AddSelectedIds_CommaSep(gblData);
        mctypeoffunding.AddSelectedIds_CommaSep(gblData);
        mcclientbusinesspaymentcycle.AddSelectedIds_CommaSep(gblData);

        if (IsClientLoggedIn)
        {
            gblData.AddExtraValues("modifiedbyclientuserid", Common.ClientUserId);
            gblData.AddExtraValues("modifieddateclientuser", "getdate()");
        }
        gblData.AddExtraValues("iskycupdated", "1");
        gblData.SaveForm(form, ClientId);
        BindMultiCheckBox();

        lblmessage.Text = "Data saved successfully!";
    }
    private void BindMultiCheckBox()
    {
        if (IsClientLoggedIn)
        {
            mcbusinessinto.Bind(ClientId, "", "", "", "client");
            mcbusinesstype.Bind(ClientId, "", "", "", "client");
            mcenterprisetype.Bind(ClientId, "", "", "", "client");
            mchedgingpolicy.Bind(ClientId, "", "", "", "client");
            mcexposuresheetmaintained.Bind(ClientId, "", "", "", "client");
            mcforwardlimits.Bind(ClientId, "", "", "", "client");
            mcforwardcontractbooking.Bind(ClientId, "", "", "", "client");
            mchedgingperiod.Bind(ClientId, "", "", "", "client");
            mctypeofbooking.Bind(ClientId, "", "", "", "client");
            mcfrequencyofadvisory.Bind(ClientId, "", "", "", "client");
            mctypeoffunding.Bind(ClientId, "", "", "", "client");
            mcclientbusinesspaymentcycle.Bind(ClientId, "", "", "", "client");
        }
        else
        {
            mcbusinessinto.Bind();
            mcbusinesstype.Bind();
            mcenterprisetype.Bind();
            mchedgingpolicy.Bind();
            mcexposuresheetmaintained.Bind();
            mcforwardlimits.Bind();
            mcforwardcontractbooking.Bind();
            mchedgingperiod.Bind();
            mctypeofbooking.Bind();
            mcfrequencyofadvisory.Bind();
            mctypeoffunding.Bind();
            mcclientbusinesspaymentcycle.Bind();
        }
    }
    private void BindCurrencies()
    {
        string query = "select * from tbl_bankauditcurrency";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        eefcaccountcurrencies.DataTextField = "bankauditcurrency_currency";
        eefcaccountcurrencies.DataValueField = "bankauditcurrency_bankauditcurrencyid";
        currencydealingin.DataTextField = "bankauditcurrency_currency";
        currencydealingin.DataValueField = "bankauditcurrency_bankauditcurrencyid";

        eefcaccountcurrencies.DataSource = dttbl;
        currencydealingin.DataSource = dttbl;
        eefcaccountcurrencies.DataBind();
        currencydealingin.DataBind();
        eefcaccountcurrencies.Items.Insert(0, new ListItem("Select", "0"));
        currencydealingin.Items.Insert(0, new ListItem("Select", "0"));
    }
    private void BindTab()
    {
        int tabIndex = GlobalUtilities.ConvertToInt(hdnmaintabid.Text);
        string html = @"<ul class='curve-tab jq-maintab'>
                        <li class='" + (tabIndex == 0 ? "curve-tab-active" : "") + @"' target='jq-trbacisinfo'>Basic Business Information</li>
                        <li target='jq-trownercontact' class='" + (tabIndex == 1 ? " curve-tab-active" : "") + @"'>Owner's Details</li>
                        <li target='jq-trfinancecontact' class='" + (tabIndex == 2 ? " curve-tab-active" : "") + @"'>Finance Person's Details</li>
                        <li target='jq-trexposure' class='" + (tabIndex == 3 ? " curve-tab-active" : "") + @"'>Exposure Details</li>
                        <li target='jq-trbankdetail' class='" + (tabIndex == 4 ? " curve-tab-active" : "") + @"'>Bank Details</li>
                    </ul>";
        lttab.Text = html;
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
                return Common.GetQueryStringValue("id");
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
