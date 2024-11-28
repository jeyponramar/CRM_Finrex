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
using WebComponent;
using System.Text;
using System.IO;
using WebComponent;
using iTextSharp.text.pdf;
using System.Text;

public partial class usercontrols_AddEditAuditControl : System.Web.UI.UserControl
{
    DataRow _dr = null;
    private bool _IsAdminPage = false;
    int _LoggedInClientId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Id == 0 && Common.ClientId > 0)//dont allow client to add
        {
            Response.End();
            return;
        }
        BindAuditDetails();
        if (!IsPostBack)
        {
            BindAllAuditDetails();
            SetSteps();
            hdnstage.Text = Step.ToString();
            if (Step == 1)
            {
                GlobalData gbldata = new GlobalData();
                gbldata.FillDropdown(ddlbank, "tbl_bankauditbank", "bankauditbank_bankname", "");
                gbldata.FillDropdown(ddlcurrencies, "tbl_bankauditcurrency", "bankauditcurrency_currency", "");
                if (Id > 0)
                {
                    ddlbank.SelectedValue = GlobalUtilities.ConvertToString(_dr["bankaudit_bankauditbankid"]);
                }
                if (IsClientLoggedIn)
                {
                    UpdateCustomerVisited();
                }
            }
            if (Id == 0)
            {
                trcustomer.Visible = true;
                ShowTabGuidelineModal();
            }
            else
            {
                if (!IsClientLoggedIn)
                {
                    trclient.Visible = true;
                }
            }
            BindBasicDetails();
        }
    }
    private void ShowTabGuidelineModal()
    {
        string script = "<script>$(document).ready(function(){showDialog($('#divbankscantabdialog'),'Understanding Tab');});</script>";
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "bankscantabdialog", script);
    }
    private int Id
    {
        get
        {
            return Common.GetQueryStringValue("id");
        }
    }
    private int Step
    {
        get
        {
            int s = Common.GetQueryStringValue("s");
            if (s == 0) return 1;
            return s;
        }
    }
    private bool IsClientLoggedIn
    {
        get
        {
            if (Common.ClientId > 0) return true;
            return false;
        }
    }
    private int LoggedInClientId
    {
        get 
        {
            return _LoggedInClientId;
        }
    }
    private int LoggedInClientUserId
    {
        get { return Common.ClientUserId; }
    }
    private int LoggedInUserId
    {
        get { return Common.UserId; }
    }
    private void BindAuditDetails()
    {
        hdnid.Text = Id.ToString();
        if (Id == 0) return;
        string query = "";
        query = @"select * from tbl_bankaudit
                    join tbl_client on client_clientid=bankaudit_clientid 
                    join tbl_bankauditstatus on bankauditstatus_bankauditstatusid=bankaudit_bankauditstatusid
                    left join tbl_clientuser on clientuser_clientuserid=bankaudit_clientuserid
                    left join tbl_bankauditbank on bankauditbank_bankauditbankid=bankaudit_bankauditbankid
                    where bankaudit_bankauditid=" + Id;
        if (IsClientLoggedIn) query += " and bankaudit_clientid=" + Common.ClientId;
        _dr = DbTable.ExecuteSelectRow(query);
        if (_dr == null)
        {
            Response.Write("Error occurred");
            Response.End();
        }
        _LoggedInClientId = GlobalUtilities.ConvertToInt(_dr["bankaudit_clientid"]);
    }
    private void UpdateCustomerVisited()
    {
        string query = "";
        query = "update tbl_bankaudit set bankaudit_iscustomervisited=1 where bankaudit_bankauditid=" + Id;
        DbTable.ExecuteQuery(query);
    }
    private int StatusId
    {
        get
        {
            if (_dr == null) return 0;
            return GlobalUtilities.ConvertToInt(_dr["bankaudit_bankauditstatusid"]);
        }
    }
    private int GetYear(int yearIndex)
    {
        return GlobalUtilities.ConvertToInt(_dr["bankaudit_year" + yearIndex]);
    }
    private void BindBasicDetails()
    {
        if (_dr == null) return;
        int statusId = StatusId;
        txtbankbranch.Text = GlobalUtilities.ConvertToString(_dr["bankaudit_bankbranch"]);
        txtindustry.Text = GlobalUtilities.ConvertToString(_dr["bankaudit_industry"]);
        hdncurrencyids.Text = GlobalUtilities.ConvertToString(_dr["bankaudit_currencyids"]);
        lbldate.Text = GlobalUtilities.ConvertToDateTime(_dr["bankaudit_date"]);
        lblCode.Text = GlobalUtilities.ConvertToString(_dr["bankaudit_code"]);
        trcode.Visible = true;
        BindBankLetterSentDetail();
        trstatus.Visible = true;
        if (statusId == 4)
        {
            trclosedon.Visible = true;
            lblclosedon.Text = GlobalUtilities.ConvertToDateTime(_dr["bankaudit_closeddate"]);
            btnReopen.Visible = true;
        }
        string statusHtml = "<div class='grid-status' style='background-color:#"+GlobalUtilities.ConvertToString(_dr["bankauditstatus_backgroundcolor"])
            + ";color:#" + GlobalUtilities.ConvertToString(_dr["bankauditstatus_textcolor"]) + "'>"+
            GlobalUtilities.ConvertToString(_dr["bankauditstatus_status"]) + "</div>";
        ltstatus.Text = statusHtml;
        
        if (!IsClientLoggedIn)
        {
            lblcompany.Text = GlobalUtilities.ConvertToString(_dr["client_customername"]);
            trclient.Visible = true;
        }
        if (statusId > 1)
        {
            lbltitle.Text = "Edit BankScan";
        }
        if (IsClientLoggedIn)
        {
            hdnissaveenabled.Text = "0";
            //if (statusId == 0 || statusId == 1 || statusId == 2)
            //{
            //    //btnsendforreview.Visible = true;
            //    btnsendforreview_documents.Visible = true;
            //}
            //else
            {
                btnfinalsave.Visible = false;
                //btnsendforreview.Visible = false;
                btnsave2.Visible = false;
                btnsave3.Visible = false;
                btnsave4.Visible = false;
                btnprevstep2.Visible = false;
                btnprevstep3.Visible = false;
                btnprevstep4.Visible = false;
                btnprevstep5.Visible = false;
                btnprevstep6.Visible = false;
                btnprevstep7.Visible = false;

                btnnextstep2.Visible = false;
                btnnextstep3.Visible = false;
                btnnextstep4.Visible = false;
                btnnextstep5.Visible = false;
                btnnextstep6.Visible = false;
                btnReopen.Visible = false;
                btnnextstep2.Text = "Next";
                btnnextstep3.Text = "Next";
                btnnextstep4.Text = "Next";
                ddlcurrencies.CssClass = "ddl ddlmultiselect disabled";
            }
        }
        else
        {
            if (statusId != 1)
            {
                btnsendbankletter.Visible = true;
            }
            if (statusId == 1)//open
            {
                btncompleteaudit.Visible = true;
                //btnsubmitreview.Visible = true;
            }
            else
            {
                hdnissaveenabled.Text = "0";
                btnfinalsave.Visible = false;
                //btnsendforreview.Visible = false;
                btncompleteaudit.Visible = false;
                btnsave2.Visible = false;
                btnsave3.Visible = false;
                btnsave4.Visible = false;
                btnnextstep2.Text = "Next";
                btnnextstep3.Text = "Next";
                btnnextstep4.Text = "Next";
                ddlcurrencies.CssClass = "ddl ddlmultiselect disabled";
            }
        }
    }
    private void BindBankLetterSentDetail()
    {
        if (GlobalUtilities.ConvertToBool(_dr["bankaudit_isbanklettersent"]))
        {
            trisbanklettersent.Visible = true;
            lblbanklettersentdate.Text = GlobalUtilities.ConvertToDateTime(_dr["bankaudit_banklettersentdate"]);
        }
    }
    private bool IsEditable()
    {
        int statusId = StatusId;
        if (IsClientLoggedIn)
        {
            if (statusId == 0 || statusId == 1 || statusId == 2)
            {
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (statusId != 3)
            {
                return false;
            }
        }
        return true;
    }
    private void BindAllAuditDetails()
    {
        int step = Step;
        if (step == 0 || step == 1)
        {
            plstep1.Visible = true;
        }
        else if (step == 2)
        {
            plstep2.Visible = true;
            BindYearlyTurnOver();
            BindTurnOverCurrencywise();
        }
        else if (step == 3)
        {
            plstep3.Visible = true;
            BindYearlyBankingCost();
            BindBankingMargin();
            BindBankingDetails();
        }
        else if (step == 4)
        {
            plstep4.Visible = true;
            BindQuestinnaire();
            BindQuestionCategory();
        }
        else if (step == 5)
        {
            plstep5.Visible = true;
            BindDocuments(ltdocuments, 0);
        }
        else if (step == 6)
        {
            plstep6.Visible = true;
            BindDocuments(ltbankletter, 1);
        }
        else if (step == 7)
        {
            plstep7.Visible = true;
            if (IsClientLoggedIn)
            {
                trclientramrks.Visible = true;
            }
            else
            {
                trfinrexremarks.Visible = true;
            }
            BindSummaryDetails();
            BindComments();
        }
    }
    private string GetCurrencies()
    {
        if (hdncurrencyids.Text == "") return "";
        Array arrids = hdncurrencyids.Text.Split(',');
        StringBuilder vals = new StringBuilder();
        for (int i = 0; i < arrids.Length; i++)
        {
            string val = "";
            for (int j = 0; j < ddlcurrencies.Items.Count; j++)
            {
                if (ddlcurrencies.Items[j].Value == arrids.GetValue(i).ToString())
                {
                    val = ddlcurrencies.Items[j].Text;
                    break;
                }
            }
            if (vals.ToString() == "")
            {
                vals.Append(val);
            }
            else
            {
                vals.Append("," + val);
            }
        }
        return vals.ToString();
    }
    private string GetCode()
    {
        string query = "";
        int clientId = GlobalUtilities.ConvertToInt(txtclientid.Text);
        query = @"select top 1 * from tbl_subscription where subscription_clientid=" + clientId +
                    @" and subscription_subscriptionstatusid=2 order by subscription_subscriptionid desc";
        DataRow drs = DbTable.ExecuteSelectRow(query);
        if (drs == null)
        {
            Response.Write("No active subscription found.");
            Response.End();
        }
        string subscriptionCode = GlobalUtilities.ConvertToString(drs["subscription_subscriptioncode"]);
        query = @"select top 1 * from tbl_bankaudit 
                    where bankaudit_clientid=" + clientId + //" and bankaudit_code like '" + subscriptionCode + @"-%'
                    " order by bankaudit_bankauditid desc";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        int count = 1000;
        if (dr != null)
        {
            string c = GlobalUtilities.ConvertToString(dr["bankaudit_code"]);
            Array arr = c.Split('-');
            count = GlobalUtilities.ConvertToInt(arr.GetValue(arr.Length - 1)) + 1;
        }
        string code = subscriptionCode + "-A-" + count;
        return code;
    }
    protected void btnaddaudit_Click(object sender, EventArgs e)
    {
        int id = Id;
        if (StatusId != 4)
        {
            Hashtable hstbl = new Hashtable();
            string currencies = GetCurrencies();
            hstbl.Add("bankauditbankid", ddlbank.SelectedValue);
            hstbl.Add("bankbranch", txtbankbranch.Text);
            hstbl.Add("currencies", currencies);
            hstbl.Add("currencyids", hdncurrencyids.Text);
            //hstbl.Add("businessinto", txtbuesinessinto.Text);
            InsertUpdate obj = new InsertUpdate();
            if (Id == 0)
            {
                int clientId = GlobalUtilities.ConvertToInt(txtclientid.Text);
                hstbl.Add("code", GetCode());
                hstbl.Add("guid", Guid.NewGuid().ToString());
                hstbl.Add("date", "getdate()");
                hstbl.Add("clientid", clientId);
                hstbl.Add("clientuserid", 0);
                hstbl.Add("bankauditstatusid", "1");
                hstbl.Add("completedsteps", "1");
                int year = DateTime.Now.Year;
                hstbl.Add("year1", year);
                hstbl.Add("year2", year - 1);
                hstbl.Add("year3", year - 2);
                id = obj.InsertData(hstbl, "tbl_bankaudit");
                if (id > 0)
                {
                    _LoggedInClientId = clientId;
                    AddLastThreeYearTurnOver(id);
                    AddCurrencywiseTurnOver(id);
                    AddLastThreeYearBankingCost(id);
                    AddBankAuditCurrency(id);
                    AddQuestionaires(id);
                }
            }
            else
            {
                if (IsPageModified)
                {
                    id = obj.UpdateData(hstbl, "tbl_bankaudit", id);
                }
            }
        }
        Response.Redirect("addbankaudit.aspx?id=" + id + "&s=2");
    }
    private bool IsPageModified
    {
        get
        {
            if (hdnispagemodified.Text == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    private void AddLastThreeYearTurnOver(int auditId)
    {
        AddLastThreeYearTurnOver(auditId, "Domestic", "Domestic Rs.");
        AddLastThreeYearTurnOver(auditId, "Export", "Export Rs.");
        AddLastThreeYearTurnOver(auditId, "Import", "Import Rs.");
    }
    private void AddCurrencywiseTurnOver(int auditId)
    {
        for (int i = 0; i < 5; i++)
        {
            int currencyId = i + 1;
            Hashtable hstbl = new Hashtable();
            hstbl.Add("bankauditid", auditId);
            hstbl.Add("clientid", LoggedInClientId);
            hstbl.Add("clientuserid", LoggedInClientUserId);
            hstbl.Add("bankauditcurrencyid", currencyId);
            InsertUpdate obj = new InsertUpdate();
            int id = obj.InsertData(hstbl, "tbl_BankAuditAnnualCurrencyTurnover");
        }
    }
    private int AddLastThreeYearTurnOver(int auditId, string name, string title)
    {
        Hashtable hstbl = new Hashtable();
        hstbl.Add("bankauditid", auditId);
        hstbl.Add("clientid", LoggedInClientId);
        hstbl.Add("clientuserid", LoggedInClientUserId);
        hstbl.Add("name", name);
        hstbl.Add("title", title);
        InsertUpdate obj = new InsertUpdate();
        int id = obj.InsertData(hstbl, "tbl_BankAuditYearlyTurnOver");
        return id;
    }
    private void AddBankAuditCurrency(int auditId)
    {
        string query = "";
        query = @"insert into tbl_bankauditconversionmargin(bankauditconversionmargin_bankauditid,bankauditconversionmargin_createddate,
                    bankauditconversionmargin_bankauditcurrencyid,bankauditconversionmargin_clientid,bankauditconversionmargin_clientuserid)
                  select " + auditId + ",getdate(),bankauditcurrency_bankauditcurrencyid," + LoggedInClientId + "," + LoggedInClientUserId +
                                " from tbl_bankauditcurrency";
        DbTable.ExecuteQuery(query);
    }
    private void AddLastThreeYearBankingCost(int auditId)
    {
        AddLastThreeYearBankingCost(auditId, "Bank Charges Paid");
        AddLastThreeYearBankingCost(auditId, "Interest Paid");
    }
    private int AddLastThreeYearBankingCost(int auditId, string title)
    {
        Hashtable hstbl = new Hashtable();
        hstbl.Add("bankauditid", auditId);
        hstbl.Add("clientid", LoggedInClientId);
        hstbl.Add("clientuserid", LoggedInClientUserId);
        hstbl.Add("title", title);
        InsertUpdate obj = new InsertUpdate();
        int id = obj.InsertData(hstbl, "tbl_BankAuditYearlyBankingCost");
        return id;
    }
    private void AddQuestionaires(int auditId)
    {
        string query = "";
        query = @"insert into tbl_bankauditquestionnaire(bankauditquestionnaire_bankauditid,bankauditquestionnaire_createddate,
                    bankauditquestionnaire_bankauditquestiontypeid,bankauditquestionnaire_bankauditquestioncategoryid,
                    bankauditquestionnaire_bankauditquestionnairemasterid,bankauditquestionnaire_question,bankauditquestionnaire_clientid,
                    bankauditquestionnaire_clientuserid)
                    select " + auditId + @",getdate(),bankauditquestionnairemaster_bankauditquestiontypeid,bankauditquestionnairemaster_bankauditquestioncategoryid,
                    bankauditquestionnairemaster_bankauditquestionnairemasterid,bankauditquestionnairemaster_question,
                    " + LoggedInClientId + @"," + LoggedInClientUserId + @"
                    from tbl_bankauditquestionnairemaster
                    join tbl_bankauditquestiontype on bankauditquestiontype_bankauditquestiontypeid = bankauditquestionnairemaster_bankauditquestiontypeid
                    join tbl_bankauditquestioncategory on bankauditquestioncategory_bankauditquestioncategoryid = bankauditquestionnairemaster_bankauditquestioncategoryid
                    ";
        DbTable.ExecuteQuery(query);
    }
    private string GetYearText(int yearIndex)
    {
        int year = GetYear(yearIndex);
        return (year - 1) + "-" + year.ToString().Substring(2);
    }
    private void BindYearlyTurnOver()
    {
        string query = "";
        StringBuilder html = new StringBuilder();
        int clientId = LoggedInClientId;
        string year1 = GetYearText(1);
        string year2 = GetYearText(2);
        string year3 = GetYearText(3);
        html.Append("<table class='grid-ui' border='1' cellpadding='7' style='width:auto;'><tr class='grid-ui-header'><td>FY</td>");
        html.Append("<td>" + year3 + "</td><td>" + year2 + "</td><td>" + year1 + "</td></tr>");
        query = "select * from tbl_BankAuditYearlyTurnOver where BankAuditYearlyTurnOver_bankauditid=" + Id;
        if (IsClientLoggedIn) query += " and BankAuditYearlyTurnOver_clientid=" + LoggedInClientId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string title = GlobalUtilities.ConvertToString(dttbl.Rows[i]["BankAuditYearlyTurnOver_title"]);
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["bankaudityearlyturnover_bankaudityearlyturnoverid"]);
            string val1 = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankaudityearlyturnover_yearlyturnoveramount1"]);
            string val2 = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankaudityearlyturnover_yearlyturnoveramount2"]);
            string val3 = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankaudityearlyturnover_yearlyturnoveramount3"]);
            html.Append("<tr><td>" + title + "<input type='text' class='hidden' name='txtbankaudityearlyturnover_id_" + (i) + "' value='" + id + "'/></td>");
            html.Append("<td><input type='text' class='mbox' name='txtbankaudityearlyturnover_yearlyturnoveramount1_" + id + "' value='" + val1 + "'/></td>");
            html.Append("<td><input type='text' class='mbox' name='txtbankaudityearlyturnover_yearlyturnoveramount2_" + id + "' value='" + val2 + "'/></td>");
            html.Append("<td><input type='text' class='mbox' name='txtbankaudityearlyturnover_yearlyturnoveramount3_" + id + "' value='" + val3 + "'/></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        ltyearlyturnover.Text = html.ToString();
    }
    private void BindTurnOverCurrencywise()
    {
        string query = "";
        StringBuilder html = new StringBuilder();
        int clientId = LoggedInClientId;
        
        query = @"select * from tbl_BankAuditAnnualCurrencyTurnover
                  join tbl_bankauditcurrency on bankauditcurrency_bankauditcurrencyid=BankAuditAnnualCurrencyTurnover_bankauditcurrencyid 
                  where BankAuditAnnualCurrencyTurnover_bankauditid=" + Id;
        if (IsClientLoggedIn) query += " and BankAuditAnnualCurrencyTurnover_clientid=" + LoggedInClientId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        html.Append("<table class='grid-ui jq-tbl-annualturnover-currencywise' border='1' cellpadding='7' style='width:auto;'><tr class='grid-ui-header'><td>Currency-wise</td>");
        html.Append("<td>Export</td><td>Import</td><td>FCY Loan</td>");
        query = @"select * from tbl_bankauditcustomlabel where bankauditcustomlabel_type='annualcurrencyturnover' and bankauditcustomlabel_bankauditid=" + Id;
        DataTable dttblcustomlabel = DbTable.ExecuteSelect(query);
        for (int i = 0; i < dttblcustomlabel.Rows.Count; i++)
        {
            string label = GlobalUtilities.ConvertToString(dttblcustomlabel.Rows[i]["bankauditcustomlabel_label"]);
            int index = GlobalUtilities.ConvertToInt(dttblcustomlabel.Rows[i]["bankauditcustomlabel_index"]);
            
            html.Append("<td class='jq-customlabel-header'>"+label+"</td>");
        }
        if (IsEditable() && dttblcustomlabel.Rows.Count < BankAudit._MaxCustomColumns)
        {
            html.Append("<td style='min-width:50px;text-align:center;'><span class='jq-addcustomlabel-header cursor' title='Add Custom Column'>+</span></td>");
        }
        html.Append("</tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string currency = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditcurrency_currency"]);
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["bankauditannualcurrencyturnover_bankauditannualcurrencyturnoverid"]);
            string exportamount = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditannualcurrencyturnover_exportamount"]);
            string importamount = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditannualcurrencyturnover_importamount"]);
            string fcyloanamount = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditannualcurrencyturnover_fcyloanamount"]);
            html.Append("<tr><td>" + currency + "<input type='text' class='hidden jq-rowid' name='txtbankauditannualcurrencyturnover_id_" + (i) + "' value='" + id + "'/></td>");
            html.Append("<td><input type='text' class='mbox' name='txtbankauditannualcurrencyturnover_exportamount_" + id + "' value='" + exportamount + "'/></td>");
            html.Append("<td><input type='text' class='mbox' name='txtbankauditannualcurrencyturnover_importamount_" + id + "' value='" + importamount + "'/></td>");
            html.Append("<td><input type='text' class='mbox' name='txtbankauditannualcurrencyturnover_fcyloanamount_" + id + "' value='" + fcyloanamount + "'/></td>");
            for (int j = 0; j < dttblcustomlabel.Rows.Count; j++)
            {
                string customval = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditannualcurrencyturnover_custom" + (j + 1)]);
                html.Append("<td><input type='text' class='mbox' name='txtbankauditannualcurrencyturnover_custom_"+(j+1)+"_" + id + "' value='" + customval + "'/></td>");
            }
            
            html.Append("</tr>");
        }
        html.Append("</table>");
        ltyearlyrunovercurrency.Text = html.ToString();
    }
    private void BindYearlyBankingCost()
    {
        string query = "";
        StringBuilder html = new StringBuilder();
        int clientId = LoggedInClientId;
        html.Append("<table class='grid-ui jq-totalbankingcostgrid' border='1' cellpadding='7' style='width:auto;'><tr class='grid-ui-header'><td>Currency-wise</td>");
        string year1 = GetYearText(1);
        string year2 = GetYearText(2);
        string year3 = GetYearText(3);
        html.Append("<td>" + year3 + "</td><td>" + year2 + "</td><td>" + year1 + "</td></tr>");
        query = @"select * from tbl_BankAuditYearlyBankingCost
                  where BankAuditYearlyBankingCost_bankauditid=" + Id;
        if (IsClientLoggedIn) query += " and BankAuditYearlyBankingCost_clientid=" + LoggedInClientId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string title = GlobalUtilities.ConvertToString(dttbl.Rows[i]["BankAuditYearlyBankingCost_title"]);
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["bankaudityearlybankingcost_bankaudityearlybankingcostid"]);
            string cost1 = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankaudityearlybankingcost_yearlybankcost1"]);
            string cost2 = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankaudityearlybankingcost_yearlybankcost2"]);
            string cost3 = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankaudityearlybankingcost_yearlybankcost3"]);
            html.Append("<tr><td>" + title + "<input type='text' class='hidden' name='txtbankaudityearlybankingcost_id_" + id + "' value='" + id + "'/></td>");
            html.Append("<td><input type='text' class='mbox' name='txtbankaudityearlybankingcost_yearlybankcost1_" + id + "' value='" + cost1 + "'/></td>");
            html.Append("<td><input type='text' class='mbox' name='txtbankaudityearlybankingcost_yearlybankcost2_" + id + "' value='" + cost2 + "'/></td>");
            html.Append("<td><input type='text' class='mbox' name='txtbankaudityearlybankingcost_yearlybankcost3_" + id + "' value='" + cost3 + "'/></td>");
            html.Append("</tr>");
        }
        if (IsEditable())
        {
            html.Append("<tr><td colspan='20'><div class='linktext jq-addbankingcostlabel'>Add</div></td></tr>");
        }
        html.Append("</table>");
        ltyearlybankingcost.Text = html.ToString();
    }
    private void BindBankingDetails()
    {
        StringBuilder html = new StringBuilder();
        html.Append(@"<table>
                        <tr>
                            <td>No. of Shipment/Invoice Per Month</td>
                            <td><input type='text' class='mbox val-i' name='txtbankaudit_invoicecountpermonth' value='" + GlobalUtilities.ConvertToString(_dr["bankaudit_invoicecountpermonth"]) + @"'/></td>
                        </tr>
                        <tr>
                            <td>Value of shipment : Per approx. range between :  $</td>
                            <td><input type='text' class='mbox' name='txtbankaudit_valueofshipment' value='" + GlobalUtilities.ConvertToString(_dr["bankaudit_valueofshipment"]) + @"'/></td>
                        </tr>
                        <tr>
                            <td>No. & Avg Amount of PCFC/ EPC Availed in a month</td>
                            <td><input type='text' class='mbox' name='txtbankaudit_avgpcfcamount' value='" + GlobalUtilities.ConvertToString(_dr["bankaudit_avgpcfcamount"]) + @"'/></td>
                        </tr>
                    </table>");

        ltbankingdetails.Text = html.ToString();
    }
    
    private void BindBankingMargin()
    {
        string query = "";
        StringBuilder html = new StringBuilder();
        int clientId = LoggedInClientId;
        html.Append("<table cellpadding='7'>");
        query = @"select * from tbl_bankauditconversionmargin
                 join tbl_bankauditcurrency on bankauditcurrency_bankauditcurrencyid=bankauditconversionmargin_bankauditcurrencyid
                 where bankauditconversionmargin_bankauditid=" + Id;
        if (IsClientLoggedIn) query += " and bankauditconversionmargin_clientid=" + LoggedInClientId;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string currency = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditcurrency_currency"]);
            string paisalabel = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditcurrency_paisa"]);
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["bankauditconversionmargin_bankauditconversionmarginid"]);
            string amount = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditconversionmargin_marginamount"]);
            if (i % 5 == 0) html.Append("<tr>");
            html.Append("<td>" + currency + " (" + paisalabel + ")<input type='text' class='hidden' name='txtbankauditconversionmargin_id_" + id + "' value='" + id + "'/></td>");
            html.Append("<td><input type='text' class='mbox' name='txtbankauditconversionmargin_marginamount_" + id + "' value='" + amount + "'/></td>");
            if (i + 1 % 5 == 0 || i == dttbl.Rows.Count - 1) html.Append("</tr>");
        }
        html.Append("</table>");
        ltbankingmargin.Text = html.ToString();
    }
    private int QuestionTypeId
    {
        get
        {
            return Common.GetQueryStringValue("qtid");
        }
    }
    private void BindQuestionCategory()
    {
        string query = @"select * from tbl_bankauditquestiontype order by bankauditquestiontype_bankauditquestiontypeid";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<ul class='curve-tab'>");
        int ctQuestionTypeId = QuestionTypeId;
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int catId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["bankauditquestiontype_bankauditquestiontypeid"]);
            string url = "addbankaudit.aspx?id=" + Id + "&s=4&qtid=" + catId;
            html.Append("<li" + (catId == ctQuestionTypeId ? " class='curve-tab-active'" : "") +
                    "><a href='" + url + "'>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditquestiontype_questiontype"]) + "</a></li>");
        }
        html.Append("</ul>");
        ltquestionairetabs.Text = html.ToString();
    }
    private void BindQuestinnaire()
    {
        string query = "";
        int qtypeid = QuestionTypeId;
        if (qtypeid == 0) qtypeid = GetFirstQuestionCatId();
        query = @"select * from tbl_bankauditquestiontype where bankauditquestiontype_bankauditquestiontypeid=" + qtypeid;
        DataRow drtype = DbTable.ExecuteSelectRow(query);
        if (drtype == null) return;
        int clientId = LoggedInClientId;
        lblquestiontitle.Text = GlobalUtilities.ConvertToString(drtype["bankauditquestiontype_questiontype"]);
        int questionTypeId = GlobalUtilities.ConvertToInt(drtype["bankauditquestiontype_bankauditquestiontypeid"]);
        query = @"select * from tbl_bankauditquestionnaire
                join tbl_bankauditquestioncategory on bankauditquestioncategory_bankauditquestioncategoryid=bankauditquestionnaire_bankauditquestioncategoryid
                join tbl_bankauditquestionnairemaster on bankauditquestionnairemaster_bankauditquestionnairemasterid=bankauditquestionnaire_bankauditquestionnairemasterid
                where bankauditquestionnaire_bankauditid=" + Id + " and bankauditquestionnaire_bankauditquestiontypeid=" + questionTypeId;
        if (IsClientLoggedIn) query += " and bankauditquestionnaire_clientid=" + LoggedInClientId;
        query += " order by bankauditquestioncategory_bankauditquestioncategoryid";
        DataTable dttblquestioncategory = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append(@"<table class='grid-ui' border='1' cellpadding='7' style='width:auto;'>
                        <tr class='grid-ui-header'><td colspan='2'>Company Inputs</td><td style='min-width:200px;'>Finrex Suggestion</td><td>Saving Benefit</td><td>Remarks</td></tr>");
        int prevQuestionCatId = 0;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        for (int j = 0; j < dttbl.Rows.Count; j++)
        {
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[j]["bankauditquestionnaire_bankauditquestionnaireid"]);
            int controlId = GlobalUtilities.ConvertToInt(dttbl.Rows[j]["bankauditquestionnairemaster_clientcontrolid"]);
            string question = GlobalUtilities.ConvertToString(dttbl.Rows[j]["bankauditquestionnaire_question"]);
            string answer = GlobalUtilities.ConvertToString(dttbl.Rows[j]["bankauditquestionnaire_answer"]);
            string suggestion = GlobalUtilities.ConvertToString(dttbl.Rows[j]["bankauditquestionnaire_suggestion"]);
            string remarks = GlobalUtilities.ConvertToString(dttbl.Rows[j]["bankauditquestionnaire_remarks"]);
            string savingBenefit = GlobalUtilities.ConvertToString(dttbl.Rows[j]["bankauditquestionnaire_savingbenefit"]);
            bool isqueryresolved = GlobalUtilities.ConvertToBool(dttbl.Rows[j]["bankauditquestionnaire_isqueryresolved"]);
            int questionCatId = GlobalUtilities.ConvertToInt(dttblquestioncategory.Rows[j]["bankauditquestioncategory_bankauditquestioncategoryid"]);
            if (prevQuestionCatId != questionCatId)
            {
                string category = GlobalUtilities.ConvertToString(dttblquestioncategory.Rows[j]["bankauditquestioncategory_category"]);
                html.Append("<tr><td colspan='5' class='audit-question-cat'>" + category + "</td></tr>");
            }
            html.Append("<tr>");
            html.Append("<td style='min-width:200px;'>" + question + "<input type='text' class='hidden' name='txtbankauditquestionnaire" + (IsClientLoggedIn ? "" : "_advisor") + "_id_" + id + "' value='" + id + "'/></td>");
            bool iseditable = false;
            if(IsClientLoggedIn)
            {
                if(StatusId == 1 || StatusId == 2)
                {
                    iseditable = true;
                }
            }
            string controlHtml = GetControlHtml(controlId, id, answer, iseditable);
            if (IsClientLoggedIn)
            {
                html.Append("<td>" + controlHtml + "</td>");
                html.Append("<td>" + suggestion + "</td>");
                html.Append("<td>" + savingBenefit + "</td>");
            }
            else
            {
                suggestion = SanitizeData(suggestion);
                html.Append("<td style='min-width:200px;'>" + controlHtml + "</td>");
                html.Append("<td><textarea class='textarea' name='txtbankauditquestionnaire_suggestion_" + id + "'>" + suggestion + "</textarea></td>");
                html.Append("<td><textarea class='textarea' name='txtbankauditquestionnaire_savingbenefit_" + id + "'>" + savingBenefit + "</textarea></td>");
            }
            string remarkscss = "";
            if (IsClientLoggedIn)
            {
                if (isqueryresolved)
                {
                }
                else
                {
                    remarkscss = " bankaudit-remarks";
                }
            }
            else
            {
                remarkscss += " jq-questinnarie-remarks-advisor";    
            }
            
            html.Append("<td><textarea class='textarea" + remarkscss + "' name='txtbankauditquestionnaire_remarks_" + id + "'>" + remarks + "</textarea>");
            html.Append("<div>");
            if (IsClientLoggedIn && iseditable)
            {
                if (!isqueryresolved && remarks.Trim() != "")
                {
                    html.Append("<div><input type='checkbox' id='chkqueryresolved_" + id + "' class='jq-chkqueryresolved'/><label for='chkqueryresolved_" + id + "'>Is resolved?</label>");
                }
            }
            html.Append("</div>");
            html.Append("<input type='text' class='hidden jq-hdnisqueryresolved' name='txtbankauditquestionnaire_isqueryresolved_" + id + "' value='" + (isqueryresolved ? "1" : "0") + "'/>");
            html.Append("</td>");
            html.Append("</tr>");
            prevQuestionCatId = questionCatId;
        }
        html.Append("</table>");
        ltquestionnarie.Text = html.ToString();
    }
    private void BindDocuments(Literal ltdoc, int isbankletter)
    {
        string query = "";
        StringBuilder html = new StringBuilder();
        int clientId = LoggedInClientId;
        bool isUploadEnabled = false;
        int statusId = StatusId;
        if (IsClientLoggedIn == false)
        {
            isUploadEnabled = true;
        }
        //if (isbankletter == 1)//only for finrex
        //{
        //    if (!IsClientLoggedIn)
        //    { 
        //        if(statusId == 3)
        //        {
        //            isUploadEnabled = true;
        //        }
        //    }
        //}
        //else
        //{
        //    if (statusId == 1 || statusId == 2)
        //    {
        //        isUploadEnabled = true;
        //    }
        //}
        html.Append("<table class='grid-ui' border='1' cellpadding='7' style='width:auto;'><tr class='grid-ui-header'><td style='min-width:200px;'>Document Name</td>");
        html.Append("<td>Upload</td></tr>");
        query = @"select * from tbl_bankauditdocumentlist 
                join tbl_bankauditdocumentcategory on bankauditdocumentcategory_bankauditdocumentcategoryid=bankauditdocumentlist_bankauditdocumentcategoryid
                left join tbl_bankauditdocument on bankauditdocument_bankauditdocumentlistid=bankauditdocumentlist_bankauditdocumentlistid 
                                                   and bankauditdocument_bankauditid='"+Id+@"'
                where bankauditdocumentcategory_isbankletter=" + isbankletter +
                @" order by bankauditdocumentlist_bankauditdocumentcategoryid";

        DataTable dttbl = DbTable.ExecuteSelect(query);
        int prevcategoryId = 0;
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int categoryId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["bankauditdocumentcategory_bankauditdocumentcategoryid"]);
            int docId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["bankauditdocumentlist_bankauditdocumentlistid"]);
            string docName = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditdocumentlist_documentname"]);
            string fileNames = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditdocument_filenames"]);
            int docListId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["bankauditdocumentlist_bankauditdocumentlistid"]);
            if (prevcategoryId != categoryId)
            {
                string cateogoryName = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditdocumentcategory_categoryname"]);
                html.Append("<tr><td colspan='2' class='audit-question-cat'>" + cateogoryName + "</td></tr>");
            }
            html.Append("<tr>");
            html.Append("<td>" + docName + "</td>");
            html.Append("<td>" + GetUploadControl(Id, docListId, fileNames, isUploadEnabled) + "</td>");
            html.Append("</tr>");
            prevcategoryId = categoryId;
        }
        html.Append("</table>");
        ltdoc.Text = html.ToString();
    }
    private string GetUploadControl(int auditId, int docId, string fileNames, bool isUploadEnabled)
    {
        string folder = "upload/bankaudit/doc/" + auditId + "/";
        string filesHtml = GetUploadFileHtml(folder, fileNames, isUploadEnabled);
        string html = "";
        if (isUploadEnabled)
        {
            html = @"<table class='tblmiltiupload'>
                        <tr>
			                <td>
                                <div resize='' isguid='True' class='filediv' title='Click here to upload file' filetype='any' size='' 
                                    ismultiple='True' fileprefix='' folder='" + folder + @"' m='bankaudit' mid='" + auditId + @"' cm='bankauditdocument' cmid='" + docId + @"'>
                                    <input type='file' name='file'>
                                    <button>Upload</button>
                                    <div>Upload File</div>                     
                                </div>
                            </td>
		                </tr>
                        <tr>
                            <td>
                                <input type='text' class='hdnfiles hidden' name='txt'>
                                <table class='tblfiles' cellspacing='5' cellpadding='0'>
                                    " + filesHtml + @"
                                </table>
                        </td>
                        </tr>
                    </table>";
        }
        else
        {
            html = "<table>" + filesHtml + "</table>";
        }
        return html;
    }
    private string GetUploadFileHtml(string folderPath, string fileNames, bool isDeleteEnabled)
    {
        if (fileNames == "") return "";
        StringBuilder html = new StringBuilder();
        Array arrFiles = fileNames.Split('|');
        string uploadPrefix = "";
        if (IsAdminPage) uploadPrefix = "../";
        for (int i = 0; i < arrFiles.Length; i++)
        {
            string path = Convert.ToString(arrFiles.GetValue(i));
            string filename = Path.GetFileName(path);
            if (filename.Contains("."))
            {
                string fileext = filename.Substring(filename.LastIndexOf('.')).ToLower();
                string imgsrc = "";
                string deleteimgsrc = "";
                string fullFileUrl = folderPath.Replace("~", "..");

                if (fullFileUrl.EndsWith("/"))
                {
                    fullFileUrl = fullFileUrl + filename;
                }
                else
                {
                    fullFileUrl = fullFileUrl + "/" + filename;
                }

                if (folderPath.EndsWith("/"))
                {
                    deleteimgsrc = folderPath + filename;
                }
                else
                {
                    deleteimgsrc = folderPath + "/" + filename;
                }

                if (fileext == ".jpg" || fileext == ".png" || fileext == ".gif" || fileext == ".bmp")
                {
                    imgsrc = fullFileUrl;
                }
                else if (fileext == ".mp3" || fileext == ".wav")
                {
                    imgsrc = uploadPrefix + "images/song-icon.png";
                }
                else if (fileext == ".avi" || fileext == ".wmv" || fileext == ".mov" || fileext == ".mpg" || fileext == ".vob" || fileext == ".3g2")
                {
                    imgsrc = uploadPrefix + "images/video-icon.png";
                }
                else if (fileext == ".doc" || fileext == ".docx")
                {
                    imgsrc = uploadPrefix + "images/doc-icon.png";
                }
                else if (fileext == ".txt")
                {
                    imgsrc = uploadPrefix + "images/txt-icon.png";
                }
                else if (fileext == ".pdf")
                {
                    imgsrc = uploadPrefix + "images/pdf-icon.png";
                }
                else if (fileext == ".zip")
                {
                    imgsrc = uploadPrefix + "images/icon/zip.png";
                }
                else if (fileext == ".xls" || fileext == ".xlsx")
                {
                    imgsrc = uploadPrefix + "images/xl-icon.png";
                }
                else if (fileext == ".ppt")
                {
                    imgsrc = uploadPrefix + "images/ppt-icon.png";
                }
                else
                {
                    imgsrc = uploadPrefix + "images/unknown.png";
                }
                Array arr1 = filename.Split('_');
                string displayFileName = arr1.GetValue(arr1.Length - 1).ToString();
                html.Append("<tr><td align='center'><img src=\'" + imgsrc + "' width='25px'/>" +
                    "</td><td><a href='" + fullFileUrl + "' target='_blank'>" + displayFileName + "</a></td>");
                if (isDeleteEnabled)
                {
                    html.Append("<td><img src='" + uploadPrefix + "images/delete.png' class='deletefile hand' val='" + 
                                    deleteimgsrc + "' fn='" + filename + "' title='Delete'></td>");
                }
                html.Append("</tr>");
            }
        }
        return html.ToString();
    }
    private string GetControlHtml(int controlId, int bankauditquestionnaireId, string answer, bool iseditable)
    {
        /*
        1.Text Box
        2.Multiline
        3.YesNo
        4.Dropdown
        5.Number
        6.Amount
         */
        if (controlId == 0) controlId = 1;
        StringBuilder html = new StringBuilder();
        answer = SanitizeData(answer);
        if (!iseditable)
        {
            if (controlId != 3)//YesNo
            {
                return answer;
            }
        }
        if (controlId == 3)//YesNo
        {
            if (iseditable)
            {
                html.Append("<select class='ddl' name='txtbankauditquestionnaire_" + bankauditquestionnaireId +
                    "'><option></option><option value='1'" + (answer == "1" ? " selected" : "") +
                    ">Yes</option><option value='0'" + (answer == "0" ? " selected" : "") + ">No</option></select>");
            }
            else
            {
                if (answer == "1")
                {
                    html.Append("Yes");
                }
                else if (answer == "0")
                {
                    html.Append("No");
                }
            }
        }
        else if (controlId == 2)//Multiline
        {
            html.Append("<textarea class='textarea' name='txtbankauditquestionnaire_" + bankauditquestionnaireId + "'>" + answer + "</textarea>");
        }
        else
        {
            string css = "textbox";
            if (controlId == 5)//Number
            {
                css = "mbox val-i";
            }
            else if (controlId == 6)//Amount
            {
                css = "mbox";
            }
            html.Append("<input type='text' class='" + css + "' name='txtbankauditquestionnaire_" + bankauditquestionnaireId + "' value=\"" + answer + "\"/>");
        }
        return html.ToString();
    }
    private string SanitizeData(string data)
    {
        if (data == null) return "";
        data = data.Replace("\"", "");
        return data;
    }
    protected void btnprevstep2_Click(object sender, EventArgs e)
    {
        Response.Redirect("addbankaudit.aspx?id=" + Id + "&s=1");
    }
    protected void btnnextstep2_Click(object sender, EventArgs e)
    {
        Response.Redirect("addbankaudit.aspx?id=" + Id + "&s=3");
    }
    protected void btnprevstep3_Click(object sender, EventArgs e)
    {
        Response.Redirect("addbankaudit.aspx?id=" + Id + "&s=2");
    }
    protected void btnnextstep3_Click(object sender, EventArgs e)
    {
        Response.Redirect("addbankaudit.aspx?id=" + Id + "&s=4&qtid="+GetFirstQuestionCatId());
    }
    protected void btnprevstep4_Click(object sender, EventArgs e)
    {
        int qtypeid = QuestionTypeId;
        string query = "";
        query = @"select top 1 bankauditquestiontype_bankauditquestiontypeid from tbl_bankauditquestiontype 
                    where bankauditquestiontype_bankauditquestiontypeid<"+qtypeid+
                    @"order by bankauditquestiontype_bankauditquestiontypeid desc";

        DataRow drtype = DbTable.ExecuteSelectRow(query);

        if (drtype == null)
        {
            Response.Redirect("addbankaudit.aspx?id=" + Id + "&s=3");
        }
        else
        {
            qtypeid = GlobalUtilities.ConvertToInt(drtype["bankauditquestiontype_bankauditquestiontypeid"]);
            Response.Redirect("addbankaudit.aspx?id=" + Id + "&s=4&qtid=" + qtypeid);
        }
    }
    protected void btnnextstep4_Click(object sender, EventArgs e)
    {
        int qtypeid = QuestionTypeId;
        string query = "";
        query = @"select top 1 bankauditquestiontype_bankauditquestiontypeid from tbl_bankauditquestiontype 
                    where bankauditquestiontype_bankauditquestiontypeid>"+qtypeid+
                    @"order by bankauditquestiontype_bankauditquestiontypeid";

        DataRow drtype = DbTable.ExecuteSelectRow(query);
        if (drtype == null)
        {
            Response.Redirect("addbankaudit.aspx?id=" + Id + "&s=5&qtid=" + qtypeid);
        }
        else
        {
            qtypeid = GlobalUtilities.ConvertToInt(drtype["bankauditquestiontype_bankauditquestiontypeid"]);
            Response.Redirect("addbankaudit.aspx?id=" + Id + "&s=4&qtid=" + qtypeid);
        }
    }
    private int GetFirstQuestionCatId()
    {
        string query="select top 1 bankauditquestiontype_bankauditquestiontypeid from tbl_bankauditquestiontype order by bankauditquestiontype_bankauditquestiontypeid";
        DataRow dr=DbTable.ExecuteSelectRow(query);
        return GlobalUtilities.ConvertToInt(dr["bankauditquestiontype_bankauditquestiontypeid"]);
    }
    private int GetLastQuestionCatId()
    {
        string query = "select top 1 bankauditquestiontype_bankauditquestiontypeid from tbl_bankauditquestiontype order by bankauditquestiontype_bankauditquestiontypeid desc";
        DataRow dr = DbTable.ExecuteSelectRow(query);
        return GlobalUtilities.ConvertToInt(dr["bankauditquestiontype_bankauditquestiontypeid"]);
    }
    protected void btnprevstep5_Click(object sender, EventArgs e)
    {
        int qtypeid = GetLastQuestionCatId();
        Response.Redirect("addbankaudit.aspx?id=" + Id + "&s=4&qtid=" + qtypeid);
    }
    protected void btnnextstep5_Click(object sender, EventArgs e)
    {
        Response.Redirect("addbankaudit.aspx?id=" + Id + "&s=6&qtid=" + QuestionTypeId);
    }
    protected void btnprevstep6_Click(object sender, EventArgs e)
    {
        Response.Redirect("addbankaudit.aspx?id=" + Id + "&s=5&qtid=" + QuestionTypeId);
    }
    protected void btnnextstep6_Click(object sender, EventArgs e)
    {
        Response.Redirect("addbankaudit.aspx?id=" + Id + "&s=7&qtid=" + QuestionTypeId);
    }
    protected void btnprevstep7_Click(object sender, EventArgs e)
    {
        Response.Redirect("addbankaudit.aspx?id=" + Id + "&s=6&qtid=" + QuestionTypeId);
    }

    protected void btnfinalsave_Click(object sender, EventArgs e)
    {
        SaveFinalBankAudit(false, 0);
        BankAudit obj = new BankAudit();
        obj.UpdateCompletedSteps(Id, _dr, BankAuditSteps.Summary);
    }
    private void SaveFinalBankAudit(bool isSendToReview, int statusId)
    {
        Hashtable hstbl = new Hashtable();
        hstbl.Add("forexexpectedsaving", GlobalUtilities.ConvertToString(txtforexexpectedsaving.Text));
        hstbl.Add("forexremarks", txtforexremarks.Text);
        hstbl.Add("forwardcontractexpectedsaving", GlobalUtilities.ConvertToString(txtforwardcontractexpectedsaving.Text));
        hstbl.Add("forwardcontractremarks", txtforwardcontractremarks.Text);
        if(IsAdminPage)
        {
            hstbl.Add("finrexremarks", txtfinrexremarks.Text);
            if (isSendToReview)
            {
                hstbl.Add("clientremarks", "");
                hstbl.Add("lastremarks", txtfinrexremarks.Text);
            }
        }
        else
        {
            //hstbl.Add("clientremarks", txtclientremarks.Text);
            if (isSendToReview)
            {
                hstbl.Add("finrexremarks", "");
                //hstbl.Add("lastremarks", txtclientremarks.Text);
            }
        }
        if (statusId > 0)
        {
            hstbl.Add("bankauditstatusid", statusId.ToString());
            if (statusId == 4)//close
            {
                hstbl.Add("closeddate", "getdate()");
            }
        }
        if (isSendToReview)
        {
            hstbl.Add("lastupdateddate", "getdate()");
        }
        InsertUpdate obj = new InsertUpdate();
        int id = obj.UpdateData(hstbl, "tbl_bankaudit", Id);
        lblMessage.Text = "Data saved successfully!";
    }
    //protected void btnsendforreview_fromdoc_Click(object sender, EventArgs e)
    //{
    //    btnsendforreview_Click(sender, e);
    //    BankAudit obj = new BankAudit();
    //    obj.UpdateCompletedSteps(Id, _dr, BankAuditSteps.Documents);
    //}
    //protected void btnsendforreview_Click(object sender, EventArgs e)
    //{
    //    SaveFinalBankAudit(true, 3);
    //    SavebankAuditCommentHistory();
    //    EmailToAdvisor();
    //    lblMessage.Text = "We have received your request, our Finrex Audit team will respond to you soon.";
    //    trhomelink.Visible = true;
    //    lblMessage.Visible = true;
    //    plstep6.Visible = false;
    //    trpagecontent.Visible = false;
    //    trsteps.Visible = false;
    //}
    //protected void btnsubmitreview_Click(object sender, EventArgs e)
    //{
    //    SaveFinalBankAudit(true, 2);
    //    SavebankAuditCommentHistory();
    //    EmailToClient();
    //    lblMessage.Text = "Your response has been submitted to client.";
    //    lblMessage.Visible = true;
    //    plstep6.Visible = false;
    //    trpagecontent.Visible = false;
    //    trsteps.Visible = false;
    //}
    protected void btncompleteaudit_Click(object sender, EventArgs e)
    {
        bool isgenaretd = GenerateCloseAuditReport();
        if (!isgenaretd)
        {
            lblMessage.Text = "Unable to generated audit report.";
            lblMessage.Visible = true;
            return;
        }
        SaveFinalBankAudit(true, 4);
        SavebankAuditCommentHistory();
        
        EmailAuditReportToClient();
        lblMessage.Text = "Bank Audit has been closed successfully.";
        lblMessage.Visible = true;
        plstep6.Visible = false;
        trpagecontent.Visible = false;
        trsteps.Visible = false;
        btnReopen.Visible = true;
    }
    protected void btnsendbankletter_Click(object sender, EventArgs e)
    {
        bool issent = EmailBankLetterToClient();
        if (issent)
        {
            Hashtable hstbl = new Hashtable();
            hstbl.Add("isbanklettersent", "1");
            hstbl.Add("banklettersentdate", "getdate()");
            InsertUpdate obj = new InsertUpdate();
            obj.UpdateData(hstbl, "tbl_bankaudit", Id);
            BindAuditDetails();
            BindBankLetterSentDetail();
            lblMessage.Text = "Bank letter sent successfully.";
        }
        else
        {
            lblMessage.Text = "Unable to send bank letter!";
        }
        BindDocuments(ltbankletter, 1);
        lblMessage.Visible = true;
    }
    private bool GenerateCloseAuditReport()
    {
        if (!Directory.Exists(Server.MapPath("~/upload/bankaudit/doc/" + Id)))
        {
            Directory.CreateDirectory(Server.MapPath("~/upload/bankaudit/doc/" + Id));
        }
        string fileName = Server.MapPath("~/upload/bankaudit/doc/" + Id + "/AuditReport-" + GlobalUtilities.ConvertToString(_dr["bankaudit_guid"]) + ".pdf");
        RPlusPdfGeneratorV1 obj = new RPlusPdfGeneratorV1();
        try
        {
            string html = "";
            obj.Generate(fileName);
            PdfPTable table = obj.AddTable(2);
            obj.AddTableCellImg(table, Server.MapPath("~/images/finrex.png"));
            html = @"<div style='text-align:right;'><div><font style='color:#2E3092;font-size:16px;'>Finrex Treasury Advisors LLP</font><div>
                                    <div><font style='font-size:9px;color:#444444;'>Empress Nucleus, 1st Floor, Andheri Kurla Road,<br/>
                                    Next to Little Flower Education School, Andheri East,<br/>
                                    Mumbai 400069.</font></div>";
            obj.AddTableCellHtml(table, html);
            obj.AddTable(table);
            obj.AddLine("#F57D17");
            html = @"<table cellspacing=0 cellpadding=0><tr><td style='text-align:center;'><font style='font-size:14px;color:#2E3092;'>BANKSCAN AUDIT REPORT</font></td></tr>
                        <tr><td style='text-align:right;'>" + GlobalUtilities.ConvertToDate(_dr["bankaudit_closeddate"]) + @"</td></tr>
                     </table>
                    ";
            obj.AddHTML(html);
            html = @"<table cellspacing=0 cellpadding=0>
                    <tr><td width='20%'>Company</td><td><b>" + GlobalUtilities.ConvertToString(_dr["client_customername"]) + @"</b></td></tr>
                    <tr><td>Audit Code</td><td>" + GlobalUtilities.ConvertToString(_dr["bankaudit_code"]) + @"</td></tr>
                    <tr><td>Bank Name</td><td>" + GlobalUtilities.ConvertToString(_dr["bankauditbank_bankname"]) + @"</td></tr>
                    <tr><td>Branch</td><td>" + GlobalUtilities.ConvertToString(_dr["bankaudit_bankbranch"]) + @"</td></tr>
                    <tr><td>Currencies</td><td>" + GlobalUtilities.ConvertToString(_dr["bankaudit_currencies"]) + @"</td></tr>
                    <tr><td>Submitted On</td><td>" + GlobalUtilities.ConvertToDate(_dr["bankaudit_date"]) + @"</td></tr>
                 </table>";
            obj.AddHTML(html);
            AddTitle(obj, "TURNOVER DETAILS");
            html = "<div><b>Last 3 Years Turnover in Rs. in Cr</b></div>" +
                   "<div><b>Industry : </b>" + GlobalUtilities.ConvertToString(_dr["bankaudit_industry"]) + "</div>";
            
            obj.AddHTML(html);

            string year1 = GetYearText(1);
            string year2 = GetYearText(2);
            string year3 = GetYearText(3);
            string header = "FY," + year3 + "," + year2 + "," + year1;
            DataTable dttbl = GetYearlyTurnOverData();
            string columns = "BankAuditYearlyTurnOver_title,bankaudityearlyturnover_yearlyturnoveramount1,bankaudityearlyturnover_yearlyturnoveramount2,bankaudityearlyturnover_yearlyturnoveramount3";
            obj.AddTable(dttbl, header, columns, "#ff9372", "#ffd0c1");

            dttbl = GetTurnOverCurrencywise();

            html = "<div><b>Last Year Annual Turnover Currency-wise</b></div>";
            obj.AddHTML(html);
            header = "Currency-wise,Export,Import,FCY Loan";
            columns = "bankauditcurrency_currency,bankauditannualcurrencyturnover_exportamount,bankauditannualcurrencyturnover_importamount,bankauditannualcurrencyturnover_fcyloanamount";
            obj.AddTable(dttbl, header, columns, "#ff9372", "#ffd0c1");

            AddTitle(obj, "BANKING DETAILS");
            html = "<div><b>Total Banking Cost Last Year</b></div>";
            obj.AddHTML(html);
            dttbl = GetYearlyBankingCost();
            header = "Currency-wise," + year3 + "," + year2 + "," + year1;
            columns = "BankAuditYearlyBankingCost_title,bankaudityearlybankingcost_yearlybankcost1,bankaudityearlybankingcost_yearlybankcost2,bankaudityearlybankingcost_yearlybankcost3";

            html = "<table cellpadding=0 cellspacing=0>" +
                        "<tr><td width='50%'>No. of Shipment/Invoice Per Month</td><td>" + GlobalUtilities.ConvertToString(_dr["bankaudit_invoicecountpermonth"]) + "</td></tr>" +
                        "<tr><td>Value of shipment : Per approx. range between : </td><td>" + GlobalUtilities.ConvertToString(_dr["bankaudit_avgpcfcamount"]) + "</td></tr>" +
                        "<tr><td>No. & Avg Amount of PCFC/ EPC Availed in a month</td><td>" + GlobalUtilities.ConvertToString(_dr["bankaudit_forexexpectedsaving"]) + "</td></tr>" +
                    "</table>";
            obj.AddHTML(html);
            html = "<div><b>Bank Conversion Margin/Charges:</b></div>";
            obj.AddHTML(html);
            dttbl = GetBankingMargin();
            header = "Currency,Margin/Charges";
            columns = "currency,bankauditconversionmargin_marginamount";
            obj.AddTable(dttbl, header, columns, "#ff9372", "#ffd0c1");

            AddTitle(obj, "QUESTIONNAIRE");

            string query = @"select * from tbl_bankauditquestiontype order by bankauditquestiontype_bankauditquestiontypeid";
            DataTable dttblquestiontype = DbTable.ExecuteSelect(query);
            for (int i = 0; i < dttblquestiontype.Rows.Count; i++)
            {
                int questionTypeId = GlobalUtilities.ConvertToInt(dttblquestiontype.Rows[i]["bankauditquestiontype_bankauditquestiontypeid"]);
                string questionType = GlobalUtilities.ConvertToString(dttblquestiontype.Rows[i]["bankauditquestiontype_questiontype"]);
                html = GetQuestinnarieHtml(questionTypeId);
                if (html != "")
                {
                    AddTitle(obj, questionType);
                    obj.AddTableByHtml(html);
                }
            }
            AddTitle(obj, "SUMMARY");

            html = GetSummaryHtml();
            obj.AddTableByHtml(html);

            obj.Close();
            return true;
        }
        catch (Exception ex)
        {
            ErrorLog.WriteLog("GenerateCloseAuditReport:Error:" + ex);
            obj.Close();
            return false;
        }
    }
    private void AddTitle(RPlusPdfGeneratorV1 pdf, string title)
    {
        pdf.AddTitle(title, "#D54715", "#D54715");
    }
    private DataTable GetYearlyTurnOverData()
    {
        string query = "select * from tbl_BankAuditYearlyTurnOver where BankAuditYearlyTurnOver_bankauditid=" + Id;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        return dttbl;
    }
    private DataTable GetTurnOverCurrencywise()
    {
        string query = @"select * from tbl_BankAuditAnnualCurrencyTurnover
                      join tbl_bankauditcurrency on bankauditcurrency_bankauditcurrencyid=BankAuditAnnualCurrencyTurnover_bankauditcurrencyid 
                      where BankAuditAnnualCurrencyTurnover_bankauditid=" + Id;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        return dttbl;
    }
    private DataTable GetYearlyBankingCost()
    {
        string query = @"select * from tbl_BankAuditYearlyBankingCost
                  where BankAuditYearlyBankingCost_bankauditid=" + Id;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        return dttbl;
    }
    private DataTable GetBankingMargin()
    {
        string query = @"select bankauditcurrency_currency+' ('+bankauditcurrency_paisa+')' as currency,bankauditconversionmargin_marginamount from tbl_bankauditconversionmargin
                 join tbl_bankauditcurrency on bankauditcurrency_bankauditcurrencyid=bankauditconversionmargin_bankauditcurrencyid
                 where bankauditconversionmargin_bankauditid=" + Id;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        return dttbl;
    }
    private string GetQuestinnarieHtml(int questionTypeId)
    {
        string query = "";
        query = @"select * from tbl_bankauditquestionnaire
            join tbl_bankauditquestioncategory on bankauditquestioncategory_bankauditquestioncategoryid=bankauditquestionnaire_bankauditquestioncategoryid
            join tbl_bankauditquestionnairemaster on bankauditquestionnairemaster_bankauditquestionnairemasterid=bankauditquestionnaire_bankauditquestionnairemasterid
            where bankauditquestionnaire_bankauditid=" + Id + " and bankauditquestionnaire_bankauditquestiontypeid=" + questionTypeId +
            " order by bankauditquestioncategory_bankauditquestioncategoryid";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        if (dttbl.Rows.Count == 0) return "";
        StringBuilder html = new StringBuilder();
        html.Append("<table border='0.1' bordercolor='#ff9372'>");
        html.Append(@"<tr><td colspan='2' font-weight='bold' bgcolor='#ffd0c1'>Company Inputs</td>
                        <td font-weight='bold' bgcolor='#ffd0c1'>Finrex Suggestion</td>
                        <td font-weight='bold' bgcolor='#ffd0c1'>Saving Benefit</td>
                        <td font-weight='bold' bgcolor='#ffd0c1'>Remarks</td>
                    </tr>");
        int prevQuestionCatId = 0;
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string question = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditquestionnaire_question"]);
            string answer = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditquestionnaire_answer"]);
            string suggestion = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditquestionnaire_suggestion"]);
            int questionCatId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["bankauditquestioncategory_bankauditquestioncategoryid"]);
            string savingbenefit = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditquestionnaire_savingbenefit"]);
            string remarks = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditquestionnaire_remarks"]);
            if (prevQuestionCatId != questionCatId)
            {
                string category = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditquestioncategory_category"]);
                html.Append("<tr><td colspan='5' bgcolor='#E0EAFF' font-weight='bold'>" + category + "</td></tr>");
            }
            html.Append("<tr>");
            html.Append("<td>" + question + "</td><td>" + answer + "</td><td>" + suggestion + "</td><td>" + savingbenefit + "</td><td>" + remarks + "</td>");
            html.Append("</tr>");
            prevQuestionCatId = questionCatId;
        }
        html.Append("</table>");
        return html.ToString();
    }
    private string GetSummaryHtml()
    {
        StringBuilder html = new StringBuilder();
        html.Append(@"<table border='1' bordercolor='#f9734a'><tr><td bgcolor='#ffc9b7' font-weight='bold'>SUMMARY</td>
                    <td bgcolor='#ffc9b7' font-weight='bold'>Expected Savings</td><td bgcolor='#ffc9b7' font-weight='bold'>Remarks</td></tr>");
        html.Append("<tr><td>Forex Conversion</td><td>" + GlobalUtilities.ConvertToString(_dr["bankaudit_forexexpectedsaving"]) + "</td>");
        html.Append("<td>" + GlobalUtilities.ConvertToString(_dr["bankaudit_forexremarks"]) + "</td></tr>");
        html.Append("<tr><td>Forward Contract</td><td>" + GlobalUtilities.ConvertToString(_dr["bankaudit_forwardcontractexpectedsaving"]) + "</td>");
        html.Append("<td>" + GlobalUtilities.ConvertToString(_dr["bankaudit_forwardcontractremarks"]) + "</td></tr>");
        html.Append("</table>");
        return html.ToString();
    }
    private void EmailToClient()
    {
        string toEmailId = GlobalUtilities.ConvertToString(_dr["client_emailid"]);
        string subject = "Queries raised by Finrex Audit Team";
        string body = Common.GetFormattedSettingForEmail("BankAudit_Email_Audit_Review_To_Client", _dr, true);
        BulkEmail.SendMail_Alert(toEmailId, subject, body, "");
    }
    private void EmailAuditReportToClient()
    {
        string toEmailId = GlobalUtilities.ConvertToString(_dr["client_emailid"]);
        string subject = "Audit Report - " + GlobalUtilities.ConvertToString(_dr["client_customername"]);
        string body = Common.GetFormattedSettingForEmail("BankAudit_Email_Audit_Report_To_Client", _dr, true);
        string attachment = Server.MapPath("~/upload/bankaudit/doc/" + Id + "/AuditReport-" + GlobalUtilities.ConvertToString(_dr["bankaudit_guid"]) + ".pdf");
        BulkEmail.SendMail_Alert(toEmailId, subject, body, attachment);
    }
    private bool EmailBankLetterToClient()
    {
        try
        {
            string toEmailId = GlobalUtilities.ConvertToString(_dr["client_emailid"]);
            string subject = "Bank Letter - " + GlobalUtilities.ConvertToString(_dr["client_customername"]);
            string body = Common.GetFormattedSettingForEmail("BankAudit_Email_BankLetter_To_Client", _dr, true);
            StringBuilder attachment = new StringBuilder();
            string query = "";
            query = @"select * from tbl_bankauditdocument 
                    join tbl_bankauditdocumentlist on bankauditdocumentlist_bankauditdocumentlistid=bankauditdocument_bankauditdocumentlistid 
                    join tbl_bankauditdocumentcategory on bankauditdocumentcategory_bankauditdocumentcategoryid=bankauditdocumentlist_bankauditdocumentcategoryid
                    where bankauditdocument_bankauditid=" + Id + @" and bankauditdocumentcategory_isbankletter=1 
                    order by bankauditdocumentlist_bankauditdocumentcategoryid";
            DataTable dttbl = DbTable.ExecuteSelect(query);
            string folderPath = Server.MapPath("~/upload/bankaudit/doc/"+Id);
            for (int i = 0; i < dttbl.Rows.Count; i++)
            {
                string fileNames = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditdocument_filenames"]);
                if (fileNames != "")
                {
                    Array arr = fileNames.Split('|');
                    for (int j = 0; j < arr.Length; j++)
                    {
                        string filepath = folderPath + "/" + arr.GetValue(j).ToString();
                        if (attachment.ToString() == "")
                        {
                            attachment.Append(filepath);
                        }
                        else
                        {
                            attachment.Append("," + filepath);
                        }
                    }
                }
            }
            if (attachment.ToString() == "")
            {
                return false;
            }
            else
            {
                BulkEmail.SendMail_Alert(toEmailId, subject, body, attachment.ToString());
            }
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }
    private string AdvisorEmailId
    {
        get
        {
            return Common.GetSetting("AuditAdvisor_EmailId");
        }
    }
    private void EmailToAdvisor()
    {
        bool isfirsttimeReview = false;
        if (StatusId == 1) isfirsttimeReview = true;
        string subject = "";
        string body = "";
        if (isfirsttimeReview)
        {
            subject = "Audit Questionnaire filled by " + GlobalUtilities.ConvertToString(_dr["client_customername"]);
            body = Common.GetFormattedSettingForEmail("BankAudit_Email_Audit_Submitted_To_Advisor", _dr, true);
        }
        else
        {
            subject = "Queries resolved by (" + GlobalUtilities.ConvertToString(_dr["client_customername"]) + ")";
            body = Common.GetFormattedSettingForEmail("BankAudit_Email_Query_Resolved_To_Advisor", _dr, true);
        }
        BulkEmail.SendMail_Alert(AdvisorEmailId, subject, body, "");
    }
    private void SavebankAuditCommentHistory()
    {
        Hashtable hstbl = new Hashtable();
        hstbl.Add("bankauditid", Id);
        //if (IsClientLoggedIn)
        //{
        //    if (txtclientremarks.Text.Trim() == "") return;
        //    hstbl.Add("clientid", LoggedInClientId);
        //    hstbl.Add("clientuserid", LoggedInClientUserId);
        //    hstbl.Add("isclientcomment", "1");
        //    hstbl.Add("comment", txtclientremarks.Text);
        //}
        //else
        {
            if (txtfinrexremarks.Text.Trim() == "") return;
            hstbl.Add("isclientcomment", "0");
            hstbl.Add("userid", LoggedInUserId);
            hstbl.Add("comment", txtfinrexremarks.Text);
        }
        
        InsertUpdate obj = new InsertUpdate();
        obj.InsertData(hstbl, "tbl_bankauditcomment");
    }
    private void BindComments()
    {
        StringBuilder html = new StringBuilder();
        string query = "";
        query = @"select * from tbl_bankauditcomment
                    left join tbl_user on user_userid=bankauditcomment_userid
                    left join tbl_clientuser on clientuser_clientuserid=bankauditcomment_clientuserid 
                    where bankauditcomment_bankauditid=" + Id + " order by bankauditcomment_bankauditcommentid desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        html.Append("<table><tr><td><b>Comments</b></td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            bool isclientcomment = GlobalUtilities.ConvertToBool(dttbl.Rows[i]["bankauditcomment_isclientcomment"]);
            string comment = GlobalUtilities.ConvertToString(dttbl.Rows[i]["bankauditcomment_comment"]);
            string date = GlobalUtilities.ConvertToDateTime(dttbl.Rows[i]["bankauditcomment_createddate"]);
            string user = GlobalUtilities.ConvertToString(dttbl.Rows[i]["user_fullname"]);
            string divcss = "bankaudit-comment-advisor";
            if (isclientcomment)
            {
                divcss = "bankaudit-comment-client";
                user = GlobalUtilities.ConvertToString(dttbl.Rows[i]["clientuser_name"]);
            }
            html.Append("<tr><td><div class='" + divcss + "'>");
            html.Append("<table width='100%'><tr class='bold'><td style='padding-bottom:20px;'>" + user + "</td><td align='right'>" + date + "</td></tr>");
            html.Append("<tr><td colspan='2'>" + comment + "</td></tr>");
            html.Append("</table></div></td></tr>");
        }
        html.Append("</table>");
        ltcomment.Text = html.ToString();
    }
    private void SetSteps()
    {
        int step = Step;
        if (step == 0) step = 1;
        int id = Id;
        int qtypeid = QuestionTypeId;
        if (qtypeid == 0) qtypeid = GetFirstQuestionCatId();
//        string html = @"<div style='height:30px;'>
//                        <div class='wizard small'>
//                            <a" + (step == 1 ? " class='current'" : "") + @" href='addbankaudit.aspx?id="+id+@"&s=1'>BASIC DETAILS</a>
//                            <a" + (step == 2 ? " class='current'" : "") + @" href='addbankaudit.aspx?id=" + id + @"&s=2' class='jq-bankaudit-tab'>TURNOVER DETAILS</a>
//                            <a" + (step == 3 ? " class='current'" : "") + @" href='addbankaudit.aspx?id=" + id + @"&s=3' class='jq-bankaudit-tab'>BANKING DETAILS</a>
//                            <a" + (step == 4 ? " class='current'" : "") + @" href='addbankaudit.aspx?id=" + id + @"&s=4&qtid=" + qtypeid + @"' class='jq-bankaudit-tab'>QUESTIONNAIRE</a>
//                            <a" + (step == 5 ? " class='current'" : "") + @" href='addbankaudit.aspx?id=" + id + @"&s=5&qtid=" + qtypeid + @"' class='jq-bankaudit-tab'>DOCUMENTS</a>
//                            <a" + (step == 6 ? " class='current'" : "") + @" href='addbankaudit.aspx?id=" + id + @"&s=6&qtid=" + qtypeid + @"' class='jq-bankaudit-tab'>BANK LETTER</a>
//                            <a" + (step == 7 ? " class='current'" : "") + @" href='addbankaudit.aspx?id=" + id + @"&s=7&qtid=" + qtypeid + @"' class='jq-bankaudit-tab'>SUMMARY</a>
//                        </div>
//                      </div>";
//        string html = @"<ul class='bankaudit-leftmenu'>
//                            <li" + (step == 1 ? " class='bankaudit-left-current'" : "") + @">" + GetLeftMenuIcon(1) + @"<a href='addbankaudit.aspx?id=" + id + @"&s=1'>BASIC DETAILS</a></li>
//                            <li" + (step == 2 ? " class='bankaudit-left-current'" : "") + @">" + GetLeftMenuIcon(2) + @"<a href='addbankaudit.aspx?id=" + id + @"&s=2' class='jq-bankaudit-tab'>TURNOVER DETAILS</a></li>
//                            <li" + (step == 3 ? " class='bankaudit-left-current'" : "") + @">" + GetLeftMenuIcon(3) + @"<a href='addbankaudit.aspx?id=" + id + @"&s=3' class='jq-bankaudit-tab'>BANKING DETAILS</a></li>
//                            <li" + (step == 4 ? " class='bankaudit-left-current'" : "") + @">" + GetLeftMenuIcon(4) + @"<a href='addbankaudit.aspx?id=" + id + @"&s=4&qtid=" + qtypeid + @"' class='jq-bankaudit-tab'>QUESTIONNAIRE</a></li>
//                            <li" + (step == 5 ? " class='bankaudit-left-current'" : "") + @">" + GetLeftMenuIcon(5) + @"<a href='addbankaudit.aspx?id=" + id + @"&s=5&qtid=" + qtypeid + @"' class='jq-bankaudit-tab'>DOCUMENTS</a></li>
//                            <li" + (step == 6 ? " class='bankaudit-left-current'" : "") + @">" + GetLeftMenuIcon(6) + @"<a href='addbankaudit.aspx?id=" + id + @"&s=6&qtid=" + qtypeid + @"' class='jq-bankaudit-tab'>BANK LETTER</a></li>
//                            <li" + (step == 7 ? " class='bankaudit-left-current'" : "") + @">" + GetLeftMenuIcon(7) + @"<a href='addbankaudit.aspx?id=" + id + @"&s=7&qtid=" + qtypeid + @"' class='jq-bankaudit-tab'>SUMMARY</a></li>
//                        </ul>
//                      ";
        string html = "<ul class='bankaudit-leftmenu'>" +
                      GetLeftMenu(1, "addbankaudit.aspx?id=" + id + @"&s=1", "BASIC DETAILS", "") +
                      GetLeftMenu(2, "addbankaudit.aspx?id=" + id + @"&s=2", "TURNOVER DETAILS", "jq-bankaudit-tab") +
                      GetLeftMenu(3, "addbankaudit.aspx?id=" + id + @"&s=3", "BANKING DETAILS", "jq-bankaudit-tab") +
                      GetLeftMenu(4, "addbankaudit.aspx?id=" + id + @"&s=4&qtid=" + qtypeid, "QUESTIONNAIRE", "jq-bankaudit-tab") +
                      GetLeftMenu(5, "addbankaudit.aspx?id=" + id + @"&s=5&qtid=" + qtypeid, "DOCUMENTS", "jq-bankaudit-tab") +
                      GetLeftMenu(6, "addbankaudit.aspx?id=" + id + @"&s=6&qtid=" + qtypeid, "BANK LETTER", "jq-bankaudit-tab") +
                      GetLeftMenu(7, "addbankaudit.aspx?id=" + id + @"&s=7&qtid=" + qtypeid, "SUMMARY", "jq-bankaudit-tab") +
                      "</ul>";
                        
        ltsteps.Text = html;
    }
    private string GetLeftMenu(int step, string url, string menu, string css)
    {
        string html = "";
        string completedSteps = GlobalUtilities.ConvertToString(_dr, "bankaudit_completedsteps");
        Array arr = completedSteps.Split(',');
        bool iscompleted = false;
        if (completedSteps != "")
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (Convert.ToInt32(arr.GetValue(i)) == step)
                {
                    iscompleted = true;
                }
            }
        }
        string licss = "";
        string srnoHtml = "<span class='bankaudit-leftmenu-srno'>" + step + "</span>";
        string imgPrefix = "";
        if (IsAdminPage) imgPrefix = "../";
        if (step == Step)
        {
            licss = "bankaudit-left-current";
        }
        if (iscompleted)
        {
            srnoHtml = "<span style='padding-right: 20px;position: relative;'><img src='" + imgPrefix + "images/tick2.png' width='15' style='margin-top:-1px;position: absolute;'/></span>";
            licss += " bankaudit-leftmenu-completed";
        }
        
        html = "<li class='" + licss + "'>" + srnoHtml + @"<a href='" + url + "' class='" + css + "'>" + menu + "</a></li>";
        return html;
    }
    private void BindSummaryDetails()
    {
        txtforexexpectedsaving.Text = GlobalUtilities.ConvertToString(_dr["bankaudit_forexexpectedsaving"]);
        txtforexremarks.Text = GlobalUtilities.ConvertToString(_dr["bankaudit_forexremarks"]);
        txtforwardcontractexpectedsaving.Text = GlobalUtilities.ConvertToString(_dr["bankaudit_forwardcontractexpectedsaving"]);
        txtforwardcontractremarks.Text = GlobalUtilities.ConvertToString(_dr["bankaudit_forwardcontractremarks"]);
        if (IsClientLoggedIn)
        {
            //txtclientremarks.Text = GlobalUtilities.ConvertToString(_dr["bankaudit_clientremarks"]);
            lblfinrexremarks.Text = GlobalUtilities.ConvertToString(_dr["bankaudit_finrexremarks"]);
        }
        else
        {
            txtfinrexremarks.Text = GlobalUtilities.ConvertToString(_dr["bankaudit_finrexremarks"]);
            //lblclientremarks.Text = GlobalUtilities.ConvertToString(_dr["bankaudit_clientremarks"]);
        }
    }
    protected void btnreopenaudit_Click(object sender, EventArgs e)
    {
        Hashtable hstbl = new Hashtable();
        hstbl.Add("bankauditstatusid", "1");
        InsertUpdate obj = new InsertUpdate();

    }
    public bool IsAdminPage
    {
        set { _IsAdminPage = value; }
        get { return _IsAdminPage; }
    }
    
    public string VersionNo
    {
        get
        {
            return ConfigurationManager.AppSettings["VersionNo"].ToString();
        }
    }
}
