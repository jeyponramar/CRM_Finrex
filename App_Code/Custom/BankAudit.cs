using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WebComponent;
using System.Text;
using System.Collections;


public enum BankAuditSteps
{
    BasicDetails = 1,
    TurnOverDetails = 2,
    BankDetails = 3,
    Questionnaire = 4,
    Documents = 5,
    BankLetter = 6,
    Summary = 7
}
public class BankAudit
{
    public const int _MaxCustomColumns = 5;
    public void Process()
    {
        string cm = Common.GetQueryString("cm");
        if (cm == "save-customlabel")
        {
            SaveCustomLabel();
        }
        else if (cm == "save-bankcostlabel")
        {
            SaveBankCostLabel();
        }
    }
	public void Save()
	{
        int stageId = GlobalUtilities.ConvertToInt(GetFormData("ctl00$ContentPlaceHolder1$addeditaudit$hdnstage"));
        int bankAuditId = GlobalUtilities.ConvertToInt(GetFormData("ctl00$ContentPlaceHolder1$addeditaudit$hdnid"));
        int clientId = ClientId;
        int clientUserId = Common.ClientUserId;
        try
        {
            if (stageId == 2)//Turnover details
            {
                SaveBankAuditInTurnOverDetails(bankAuditId, clientId, clientUserId);
                SaveTurnOverDetails(bankAuditId, clientId, clientUserId);
                SaveTurnOverCurrencywise(bankAuditId, clientId, clientUserId);
                UpdateCompletedSteps(bankAuditId, null, BankAuditSteps.TurnOverDetails);
            }
            else if (stageId == 3)//bank details
            {
                SaveBankAuditYearlyBankingCost(bankAuditId, clientId, clientUserId);
                SaveBankingMargin(bankAuditId, clientId, clientUserId);
                SaveBankDetail(bankAuditId, clientId, clientUserId);
                UpdateCompletedSteps(bankAuditId, null, BankAuditSteps.BankDetails);
            }
            else if (stageId == 4)//questionaire
            {
                SaveQuestionaire(bankAuditId, clientId, clientUserId);
                UpdateCompletedSteps(bankAuditId, null, BankAuditSteps.Questionnaire);
            }
            HttpContext.Current.Response.Write("{\"status\":\"ok\"}");
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write("{\"status\":\"error\"}");
        }
	}
    private void SaveBankAuditInTurnOverDetails(int bankAuditId, int clientId, int clientUserId)
    {
        DataRow dr = GetBankAuditDetails(bankAuditId, clientId);
        string existingIndustry = GlobalUtilities.ConvertToString(dr["bankaudit_industry"]);
        string industry = GetFormData("ctl00$ContentPlaceHolder1$addeditaudit$txtindustry");
        if (industry == existingIndustry) return;
        Hashtable hstbl = new Hashtable();
        hstbl.Add("industry", industry);
        hstbl.Add("modifiedclientuserid", clientUserId);
        InsertUpdate obj = new InsertUpdate();
        string where = "bankaudit_bankauditid=" + bankAuditId;
        if (IsClientLoggedIn) where += " and bankaudit_clientid=" + clientId;
        obj.UpdateData(hstbl, "tbl_bankaudit", where);
    }
    private DataRow GetBankAuditDetails(int bankAuditId, int clientId)
    {
        string query = "select * from tbl_bankaudit where bankaudit_bankauditid=" + bankAuditId + " and bankaudit_clientid=" + clientId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        return dr;
    }
    private int ClientId
    {
        get
        {
            if (IsClientLoggedIn) return Common.ClientId;
            int bankAuditId = GlobalUtilities.ConvertToInt(GetFormData("ctl00$ContentPlaceHolder1$addeditaudit$hdnid"));
            DataRow dr = DbTable.GetOneRow("tbl_bankaudit", bankAuditId);
            return GlobalUtilities.ConvertToInt(dr["bankaudit_clientid"]);
        }
    }
    private void SaveTurnOverDetails(int bankAuditId, int clientId, int clientUserId)
    {
        for (int i = 0; i < HttpContext.Current.Request.Form.Keys.Count; i++)
        {
            string key = HttpContext.Current.Request.Form.Keys[i];
            if (key.StartsWith("txtbankaudityearlyturnover_id_"))
            {
                int id = GlobalUtilities.ConvertToInt(HttpContext.Current.Request.Form[i]);
                string year1 = GetFormData("txtbankaudityearlyturnover_yearlyturnoveramount1_" + id);
                string year2 = GetFormData("txtbankaudityearlyturnover_yearlyturnoveramount2_" + id);
                string year3 = GetFormData("txtbankaudityearlyturnover_yearlyturnoveramount3_" + id);
                Hashtable hstbl = new Hashtable();
                hstbl.Add("yearlyturnoveramount1", year1);
                hstbl.Add("yearlyturnoveramount2", year2);
                hstbl.Add("yearlyturnoveramount3", year3);
                InsertUpdate obj = new InsertUpdate();
                string where = "bankaudityearlyturnover_bankaudityearlyturnoverid=" + id + " and bankaudityearlyturnover_bankauditid=" + bankAuditId;
                if (IsClientLoggedIn) where += " and bankaudityearlyturnover_clientid=" + clientId;
                obj.UpdateData(hstbl, "tbl_bankaudityearlyturnover", where);
            }
        }
    }
    private void SaveTurnOverCurrencywise(int bankAuditId, int clientId, int clientUserId)
    {
        for (int i = 0; i < HttpContext.Current.Request.Form.Keys.Count; i++)
        {
            string key = HttpContext.Current.Request.Form.Keys[i];
            if (key.StartsWith("txtbankauditannualcurrencyturnover_id_"))
            {
                int id = GlobalUtilities.ConvertToInt(HttpContext.Current.Request.Form[i]);
                string year1 = GetFormData("txtbankauditannualcurrencyturnover_exportamount_" + id);
                string year2 = GetFormData("txtbankauditannualcurrencyturnover_importamount_" + id);
                string year3 = GetFormData("txtbankauditannualcurrencyturnover_fcyloanamount_" + id);
                Hashtable hstbl = new Hashtable();
                hstbl.Add("exportamount", year1);
                hstbl.Add("importamount", year2);
                hstbl.Add("fcyloanamount", year3);
                for (int j = 1; j <= 5; j++)
                {
                    if (HttpContext.Current.Request["txtbankauditannualcurrencyturnover_custom_" + j + "_" + id] == null)
                    {
                        break;
                    }
                    else
                    {
                        hstbl.Add("custom" + j, GlobalUtilities.ConvertToString(HttpContext.Current.Request["txtbankauditannualcurrencyturnover_custom_" + j + "_" + id]));
                    }
                }
                InsertUpdate obj = new InsertUpdate();
                string where = "bankauditannualcurrencyturnover_bankauditannualcurrencyturnoverid=" + id + 
                                " and bankauditannualcurrencyturnover_bankauditid=" + bankAuditId;
                if (IsClientLoggedIn) where += " and bankauditannualcurrencyturnover_clientid=" + clientId;
                obj.UpdateData(hstbl, "tbl_bankauditannualcurrencyturnover", where);
            }
        }
    }
    private void SaveBankAuditYearlyBankingCost(int bankAuditId, int clientId, int clientUserId)
    {
        for (int i = 0; i < HttpContext.Current.Request.Form.Keys.Count; i++)
        {
            string key = HttpContext.Current.Request.Form.Keys[i];
            if (key.StartsWith("txtbankaudityearlybankingcost_id_"))
            {
                int id = GlobalUtilities.ConvertToInt(HttpContext.Current.Request.Form[i]);
                string year1 = GetFormData("txtbankaudityearlybankingcost_yearlybankcost1_" + id);
                string year2 = GetFormData("txtbankaudityearlybankingcost_yearlybankcost2_" + id);
                string year3 = GetFormData("txtbankaudityearlybankingcost_yearlybankcost3_" + id);
                Hashtable hstbl = new Hashtable();
                hstbl.Add("yearlybankcost1", year1);
                hstbl.Add("yearlybankcost2", year2);
                hstbl.Add("yearlybankcost3", year3);
                InsertUpdate obj = new InsertUpdate();
                string where = "bankaudityearlybankingcost_bankaudityearlybankingcostid=" + id +
                                " and bankaudityearlybankingcost_bankauditid=" + bankAuditId;
                if (IsClientLoggedIn) where += " and bankaudityearlybankingcost_clientid=" + clientId;
                obj.UpdateData(hstbl, "tbl_bankaudityearlybankingcost", where);
            }
        }
    }
    private void SaveBankingMargin(int bankAuditId, int clientId, int clientUserId)
    {
        for (int i = 0; i < HttpContext.Current.Request.Form.Keys.Count; i++)
        {
            string key = HttpContext.Current.Request.Form.Keys[i];
            if (key.StartsWith("txtbankauditconversionmargin_id_"))
            {
                int id = GlobalUtilities.ConvertToInt(HttpContext.Current.Request.Form[i]);
                string marginamount = GetFormData("txtbankauditconversionmargin_marginamount_" + id);
                Hashtable hstbl = new Hashtable();
                hstbl.Add("marginamount", marginamount);
                InsertUpdate obj = new InsertUpdate();
                string where = "bankauditconversionmargin_bankauditconversionmarginid=" + id +
                                " and bankauditconversionmargin_bankauditid=" + bankAuditId;
                if (IsClientLoggedIn) where += " and bankauditconversionmargin_clientid=" + clientId;
                obj.UpdateData(hstbl, "tbl_bankauditconversionmargin", where);
            }
        }
    }
    private void SaveBankDetail(int bankAuditId, int clientId, int clientUserId)
    {
        int invoicecountpermonth = GetFormDataInt("txtbankaudit_invoicecountpermonth");
        string avgpcfcamount = GetFormData("txtbankaudit_avgpcfcamount");
        string valueofshipment = GetFormData("txtbankaudit_valueofshipment");
        Hashtable hstbl = new Hashtable();
        hstbl.Add("invoicecountpermonth", invoicecountpermonth);
        hstbl.Add("avgpcfcamount", avgpcfcamount);
        hstbl.Add("valueofshipment", valueofshipment);
        InsertUpdate obj = new InsertUpdate();
        string where = "bankaudit_bankauditid=" + bankAuditId;
        if (IsClientLoggedIn) where += " and bankauditconversionmargin_clientid=" + clientId;
        obj.UpdateData(hstbl, "tbl_bankaudit", bankAuditId);
    }
    private void SaveQuestionaire(int bankAuditId, int clientId, int clientUserId)
    {
        int userId = Common.UserId;
        for (int i = 0; i < HttpContext.Current.Request.Form.Keys.Count; i++)
        {
            string key = HttpContext.Current.Request.Form.Keys[i];
            if (key.StartsWith("txtbankauditquestionnaire_id"))
            {
                int id = GlobalUtilities.ConvertToInt(key.Split('_').GetValue(2));
                string answer = GlobalUtilities.ConvertToString(HttpContext.Current.Request.Form["txtbankauditquestionnaire_" + id]);
                string remarks = GlobalUtilities.ConvertToString(HttpContext.Current.Request.Form["txtbankauditquestionnaire_remarks_" + id]);
                int isqueryresolved = GlobalUtilities.ConvertToInt(HttpContext.Current.Request.Form["txtbankauditquestionnaire_isqueryresolved_" + id]);
                if (remarks.Trim() == "") isqueryresolved = 1;
                Hashtable hstbl = new Hashtable();
                hstbl.Add("answer", answer);
                hstbl.Add("remarks", remarks);
                hstbl.Add("isqueryresolved", isqueryresolved.ToString());
                InsertUpdate obj = new InsertUpdate();
                string where = "bankauditquestionnaire_bankauditquestionnaireid=" + id +
                                " and bankauditquestionnaire_bankauditid=" + bankAuditId;
                if (IsClientLoggedIn) where += " and bankauditquestionnaire_clientid=" + clientId;
                obj.UpdateData(hstbl, "tbl_bankauditquestionnaire", where);
            }
            else if (userId > 0 && key.StartsWith("txtbankauditquestionnaire_advisor_id"))
            {
                int id = GlobalUtilities.ConvertToInt(key.Split('_').GetValue(3));
                string suggestion = GlobalUtilities.ConvertToString(HttpContext.Current.Request.Form["txtbankauditquestionnaire_suggestion_" + id]);
                string savingBenefit = GlobalUtilities.ConvertToString(HttpContext.Current.Request.Form["txtbankauditquestionnaire_savingbenefit_" + id]);
                string remarks = GlobalUtilities.ConvertToString(HttpContext.Current.Request.Form["txtbankauditquestionnaire_remarks_" + id]);
                int isqueryresolved = GlobalUtilities.ConvertToInt(HttpContext.Current.Request.Form["txtbankauditquestionnaire_isqueryresolved_" + id]);
                if (remarks.Trim() == "") isqueryresolved = 1;
                Hashtable hstbl = new Hashtable();
                hstbl.Add("suggestion", suggestion);
                hstbl.Add("savingbenefit", savingBenefit);
                hstbl.Add("remarks", remarks);
                hstbl.Add("isqueryresolved", isqueryresolved.ToString());
                InsertUpdate obj = new InsertUpdate();
                string where = "bankauditquestionnaire_bankauditquestionnaireid=" + id +
                                " and bankauditquestionnaire_bankauditid=" + bankAuditId;
                if (IsClientLoggedIn) where += " and bankauditquestionnaire_clientid=" + clientId;
                obj.UpdateData(hstbl, "tbl_bankauditquestionnaire", where);
            }
        }
    }
    private bool IsClientLoggedIn
    {
        get
        {
            if (Common.UserId > 0) return false;
            return true;
        }
    }
    private int GetFormDataInt(string id)
    {
        return GlobalUtilities.ConvertToInt(GetFormData(id));
    }
    private double GetFormDataDbl(string id)
    {
        return GlobalUtilities.ConvertToDouble(GetFormData(id));
    }
    private string GetFormData(string id)
    {
        return HttpContext.Current.Request.Form[id];
    }
    public bool SaveDocument(int bankAuditId, int docListId, string folderName, string fileName)
    {
        string query = "";
        Hashtable hstbl = new Hashtable();
        InsertUpdate obj = new InsertUpdate();
        bool issuccess = true;
        int id = 0;
        try
        {
            query = @"select * from tbl_bankauditdocumentlist
                    join tbl_bankauditdocumentcategory on bankauditdocumentcategory_bankauditdocumentcategoryid=bankauditdocumentlist_bankauditdocumentcategoryid
                    where bankauditdocumentlist_bankauditdocumentlistid=" + docListId;
            DataRow drdoclist = DbTable.ExecuteSelectRow(query);
            bool isBankLetter = GlobalUtilities.ConvertToBool(drdoclist["bankauditdocumentcategory_isbankletter"]);
            query = "select * from tbl_bankauditdocument where bankauditdocument_bankauditdocumentlistid=" + docListId +
                            " and bankauditdocument_bankauditid=" + bankAuditId;
            DataRow dr = DbTable.ExecuteSelectRow(query);
            if (dr == null)
            {
                hstbl.Add("bankauditid", bankAuditId);
                hstbl.Add("filenames", fileName);
                hstbl.Add("clientuserid", Common.ClientUserId);
                hstbl.Add("clientid", ClientId);
                hstbl.Add("userid", Common.UserId);
                hstbl.Add("bankauditdocumentlistid", docListId);
                id = obj.InsertData(hstbl, "tbl_bankauditdocument");
            }
            else
            {
                string filenames = GlobalUtilities.ConvertToString(dr["bankauditdocument_filenames"]);
                id = GlobalUtilities.ConvertToInt(dr["bankauditdocument_bankauditdocumentid"]);
                filenames += "|" + fileName;
                hstbl.Add("filenames", filenames);
                if (Common.ClientUserId > 0) hstbl.Add("modifiedclientuserid", Common.ClientUserId);
                id = obj.UpdateData(hstbl, "tbl_bankauditdocument", id);
            }
            if (isBankLetter)
            {
                UpdateCompletedSteps(bankAuditId, null, BankAuditSteps.BankLetter);
            }
            else
            {
                UpdateCompletedSteps(bankAuditId, null, BankAuditSteps.Documents);
            }
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }
    public bool DeleteDocument()
    {
        int bankAuditId = Common.GetQueryStringValue("mid");
        int docId = Common.GetQueryStringValue("cmid");
        string fileName = Common.GetQueryString("fn");
        //dont delete the file
        string query = "select * from tbl_bankauditdocument where bankauditdocument_bankauditdocumentid=" + docId +
                            " and bankauditdocument_bankauditid=" + bankAuditId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        Hashtable hstbl = new Hashtable();
        if (dr == null) return true;
        string oldfilenames = GlobalUtilities.ConvertToString(dr["bankauditdocument_filenames"]);
        string filenames = "";
        Array arr = oldfilenames.Split('|');
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr.GetValue(i).ToString() != fileName)
            {
                if (filenames == "")
                {
                    filenames = arr.GetValue(i).ToString();
                }
                else
                {
                    filenames += "|" + arr.GetValue(i).ToString();
                }
            }
        }
        InsertUpdate obj = new InsertUpdate();
        hstbl.Add("filenames", filenames);
        docId = obj.UpdateData(hstbl, "tbl_bankauditdocument", docId);
        return true;
    }
    public static string ConvertToDoubleText(object val)
    {
        double dbl = GlobalUtilities.ConvertToDouble(val);
        if (dbl == 0) return "";
        return dbl.ToString();
    }
    private void SaveCustomLabel()
    {
        string query = "";
        int bankAuditId = Common.GetQueryStringValue("bankauditid");
        string label = GetFormData("txtcustomlabel-annualturnover");
        string type = Common.GetQueryString("type");
        Hashtable hstbl = new Hashtable();
        Hashtable hstblp = new Hashtable();
        hstblp.Add("label", label);
        query = "select top 1 * from tbl_bankauditcustomlabel where bankauditcustomlabel_bankauditid=" + bankAuditId +
                " and bankauditcustomlabel_label=@label";
        DataRow dre = DbTable.ExecuteSelectRow(query, hstblp);
        if (dre != null)
        {
            HttpContext.Current.Response.Write("{\"status\":\"validation\",\"msg\":\"Data already exists!\"}");
            return;
        }
        query = "select top 1 * from tbl_bankauditcustomlabel where bankauditcustomlabel_bankauditid=" + bankAuditId+
                " order by bankauditcustomlabel_bankauditcustomlabelid desc";
        int index = 1;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr != null)
        {
            index = GlobalUtilities.ConvertToInt(dr["bankauditcustomlabel_index"]) + 1;
        }
        hstbl = new Hashtable();
        hstbl.Add("bankauditid", bankAuditId);
        hstbl.Add("label", label);
        hstbl.Add("index", index);
        hstbl.Add("type", type);
        InsertUpdate obj = new InsertUpdate();
        int id = obj.InsertData(hstbl, "tbl_bankauditcustomlabel");
        HttpContext.Current.Response.Write("{\"status\":\"ok\",\"id\":\"" + id + "\",\"index\":\""+index+"\"}");
    }
    private void SaveBankCostLabel()
    {
        string query = "";
        int clientId = ClientId;
        int clientUserId = Common.ClientUserId;
        int bankAuditId = Common.GetQueryStringValue("bankauditid");
        string label = GetFormData("txtcurrencywiselabel-bankcostdetails");
        query = "select * from tbl_BankAuditYearlyBankingCost where BankAuditYearlyBankingCost_title=@title";
        Hashtable hstblp = new Hashtable();
        hstblp.Add("title", label);
        DataRow drexisting = DbTable.ExecuteSelectRow(query, hstblp);
        if (drexisting != null)
        {
            HttpContext.Current.Response.Write("{\"status\":\"validation\",\"msg\":\"Data already exists!\"}");
            return;
        }
        Hashtable hstbl = new Hashtable();
        hstbl.Add("bankauditid", bankAuditId);
        hstbl.Add("clientid", clientId);
        hstbl.Add("clientuserid", clientUserId);
        hstbl.Add("title", label);
        InsertUpdate obj = new InsertUpdate();
        int id = obj.InsertData(hstbl, "tbl_BankAuditYearlyBankingCost");
        HttpContext.Current.Response.Write("{\"status\":\"ok\",\"id\":\"" + id + "\"}");
    }
    public void UpdateCompletedSteps(int id, DataRow dr, BankAuditSteps stage)
    {
        if (dr == null)
        {
            dr = DbTable.GetOneRow("tbl_bankaudit", id);
        }
        string completedSteps = GlobalUtilities.ConvertToString(dr["bankaudit_completedsteps"]);
        int stageId = (int)stage;
        Array arr = completedSteps.Split(',');
        bool isexists = false;
        if (completedSteps != "")
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (Convert.ToInt32(arr.GetValue(i)) == stageId)
                {
                    isexists = true;
                }
            }
        }
        if (isexists) return;
        if (completedSteps == "")
        {
            completedSteps = stageId.ToString();
        }
        else
        {
            completedSteps += "," + stageId.ToString();
        }
        string query = "";
        query = "update tbl_bankaudit set bankaudit_completedsteps='" + completedSteps + "' where bankaudit_bankauditid=" + id;
        DbTable.ExecuteQuery(query);
    }
}
