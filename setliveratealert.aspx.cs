using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class setliveratealert : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_liveratealert", "liveratealertid");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gblData.FillDropdown(ddlcurrencymasterid, "tbl_currencymaster", "currencymaster_currency", "currencymaster_currencymasterid",
                            "currencymaster_currencymasterid <= 4", "currencymaster_currency");
            gblData.FillDropdown(ddlcovertypeid, "tbl_covertype", "covertype_covertype", "covertype_covertypeid", "", "covertype_covertype");
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["rid"] != null)
                {
                    int liverateId = Common.GetQueryStringValue("rid");
                    if (liverateId == 1)
                    {
                        ddlcurrencymasterid.SelectedValue = "1";
                        ddlcovertypeid.SelectedValue = "2";
                    }
                    else if (liverateId == 2)
                    {
                        ddlcurrencymasterid.SelectedValue = "1";
                        ddlcovertypeid.SelectedValue = "1";
                    }
                    else if (liverateId == 10)
                    {
                        ddlcurrencymasterid.SelectedValue = "2";
                        ddlcovertypeid.SelectedValue = "2";
                    }
                    else if (liverateId == 11)
                    {
                        ddlcurrencymasterid.SelectedValue = "2";
                        ddlcovertypeid.SelectedValue = "1";
                    }
                    else if (liverateId == 19)
                    {
                        ddlcurrencymasterid.SelectedValue = "3";
                        ddlcovertypeid.SelectedValue = "2";
                    }
                    else if (liverateId == 20)
                    {
                        ddlcurrencymasterid.SelectedValue = "3";
                        ddlcovertypeid.SelectedValue = "1";
                    }
                    else if (liverateId == 28)
                    {
                        ddlcurrencymasterid.SelectedValue = "4";
                        ddlcovertypeid.SelectedValue = "2";
                    }
                    else if (liverateId == 29)
                    {
                        ddlcurrencymasterid.SelectedValue = "4";
                        ddlcovertypeid.SelectedValue = "1";
                    }
                }
            }
            else
            {
                //Populate_START
                gblData.PopulateForm(form, GetId());
                //Populate_END
                txttarget.Text = txttarget.Text.Replace(".0000", "");
                txtstoploss.Text = txtstoploss.Text.Replace(".0000", "");
            }
            BindContacts();
        }
        
    }
    private void BindContacts()
    {
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        string query = "select * from tbl_contacts where contacts_clientid="+clientId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder emailhtml = new StringBuilder();
        StringBuilder mobilehtml = new StringBuilder();
        emailhtml.Append("<table class='tblemailids'>");
        mobilehtml.Append("<table class='tblmobilenos'>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string emailId = GlobalUtilities.ConvertToString(dttbl.Rows[i]["contacts_emailid"]);
            string mobileNo = GlobalUtilities.ConvertToString(dttbl.Rows[i]["contacts_mobileno"]);
            string contactPerson = GlobalUtilities.ConvertToString(dttbl.Rows[i]["contacts_contactperson"]);
            int contactId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["contacts_contactsid"]);
            string strchecked = "";
            Array arr = txtemailid.Text.Split(',');
            for (int j = 0; j < arr.Length; j++)
            {
                if (GlobalUtilities.ConvertToInt(arr.GetValue(j)) == contactId)
                {
                    strchecked = "checked='checked'"; break;
                }
            }
            if (emailId.Trim() != "")
            {
                emailhtml.Append("<tr><td width='20'><input type='checkbox' class='chktwoselect' value='" + contactId + "' " + strchecked + "/></td>" +
                                      "<td>" + contactPerson + " (" + emailId + ")</td></tr>");
            }
            strchecked = "";
            arr = txtmobileno.Text.Split(',');
            for (int j = 0; j < arr.Length; j++)
            {
                if (GlobalUtilities.ConvertToInt(arr.GetValue(j)) == contactId)
                {
                    strchecked = "checked='checked'"; break;
                }
            }
            if (mobileNo != "")
            {
                mobilehtml.Append("<tr><td width='20'><input type='checkbox' class='chktwoselect' value='" + contactId + "' " + strchecked + "/></td>" +
                                      "<td>" + contactPerson + " (" + mobileNo + ")</td></tr>");
            }
        }
        emailhtml.Append("</table>");
        mobilehtml.Append("</table>");

        ltemailids.Text = emailhtml.ToString();
        ltmobilenos.Text = mobilehtml.ToString();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData(true);
    }
    private int SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        int id = 0;
        int clientUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientUserId"));
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        gblData.AddExtraValues("clientid", clientId);
        gblData.AddExtraValues("clientuserid", clientUserId);
        int liveRateId = 0;
        int currenyId = GlobalUtilities.ConvertToInt(ddlcurrencymasterid.SelectedValue);
        int converTypeId = GlobalUtilities.ConvertToInt(ddlcovertypeid.SelectedValue);

        if (converTypeId == 1)//import
        {
            if (currenyId == 1)
            {
                liveRateId = 2;
            }
            else if (currenyId == 2)
            {
                liveRateId = 11;
            }
            else if (currenyId == 3)
            {
                liveRateId = 20;
            }
            else if (currenyId == 4)
            {
                liveRateId = 29;
            }
        }
        else//export
        {
            if (currenyId == 1)
            {
                liveRateId = 1;
            }
            else if (currenyId == 2)
            {
                liveRateId = 10;
            }
            else if (currenyId == 3)
            {
                liveRateId = 19;
            }
            else if (currenyId == 4)
            {
                liveRateId = 28;
            }
        }
        gblData.AddExtraValues("liverateid", liveRateId);
        id = gblData.SaveForm(form, GetId());

        if (id > 0)
        {
            lblMessage.Text = "Live rate alert has been set successfully!";
            lblMessage.Visible = true;
            string script = "<script>alert('" + lblMessage.Text + "');window.parent.hideLiveRateAlert();</script>";
            Page.RegisterClientScriptBlock("closepop", script);
        }
        else
        {
            lblMessage.Text = "Error occurred while saving data</br>Error : " + gblData._error;
            lblMessage.Visible = true;
        }
        return id;
    }
   
    private int GetId()
    {
        return GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
    }
}
