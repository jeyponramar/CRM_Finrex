using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Text;
using System.Data;
using System.Collections;

public partial class exportexposureportal_importexposuresettings : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_importexposurealerts", "importexposurealertsid");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gblData.PopulateForm(form, GetId());
            Bind();
        }
    }
    private int GetId()
    {
        int id = 0;
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        string query = "select importexposurealerts_importexposurealertsid from tbl_importexposurealerts where importexposurealerts_clientid="+clientId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            Hashtable hstbl = new Hashtable();
            hstbl.Add("clientid", clientId);
            InsertUpdate obj = new InsertUpdate();
            id = obj.InsertData(hstbl, "tbl_importexposurealerts");
        }
        else
        {
            id = GlobalUtilities.ConvertToInt(dr["importexposurealerts_importexposurealertsid"]);
        }
        return id;
    }
    private void Bind()
    {
        BindOptionalFields();
        BindCurrency();
    }
    private void BindOptionalFields()
    {
        string query = @"select * from tbl_columns 
                        join tbl_module on module_moduleid=columns_moduleid
                        where columns_isgenerate=1 AND isnull(columns_isenableinedit,0)=1 AND module_modulename IN('FIM Import Order','FIM Trade Credit','FIM Forward Contract')
                        order by module_modulename";
        DataTable dttblcol = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        if (!GlobalUtilities.IsValidaTable(dttblcol)) return;
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        string prevModule = "";
        string moduleName = "";
        query = "select * from tbl_customercustomfields where customercustomfields_clientid="+clientId;
        DataTable dttblCustom = DbTable.ExecuteSelect(query);
        for (int i = 0; i < dttblcol.Rows.Count; i++)
        {
            moduleName = GlobalUtilities.ConvertToString(dttblcol.Rows[i]["module_modulename"]).Replace("FIM ","");
            string lbl = GlobalUtilities.ConvertToString(dttblcol.Rows[i]["columns_lbl"]);
            int columnId = GlobalUtilities.ConvertToInt(dttblcol.Rows[i]["columns_columnsid"]);
            string strchecked = "";
            for (int j = 0; j < dttblCustom.Rows.Count; j++)
            {
                if (GlobalUtilities.ConvertToInt(dttblCustom.Rows[j]["customercustomfields_columnsid"]) == columnId)
                {
                    strchecked = "checked='checked'";
                }
            }
            if (moduleName != prevModule)
            {
                html.Append("<tr><td class='bold' colspan='10'>" + moduleName + "</td></tr>");
            }
            if (i % 5 == 0) html.Append("<tr>");
            html.Append("<td><input type='checkbox' name='chcc_" + columnId + "' "+strchecked+"/>" + lbl + "</td>");
            if ((i+1) % 5 == 0) html.Append("</tr>");
            prevModule = moduleName;
        }
        ltOptionalFields.Text = html.ToString();
    }
    private void BindCurrency()
    {
        string query = "select * from tbl_exposurecurrencymaster";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table>");
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        query = "select * from tbl_customercurrency where customercurrency_clientid=" + clientId;
        DataTable dttblc = DbTable.ExecuteSelect(query);
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string strchecked = "";
            for (int j = 0; j < dttblc.Rows.Count; j++)
            {
                if (GlobalUtilities.ConvertToInt(dttblc.Rows[j]["customercurrency_exposurecurrencymasterid"]) ==
                    GlobalUtilities.ConvertToInt(dttbl.Rows[i]["exposurecurrencymaster_exposurecurrencymasterid"]))
                {
                    strchecked = "checked='checked'";
                    break;
                }
            }
            html.Append("<tr><td><input type='checkbox' name='che_" + 
                GlobalUtilities.ConvertToInt(dttbl.Rows[i]["exposurecurrencymaster_exposurecurrencymasterid"]) + " '" +
                        " "+strchecked+"/>" + dttbl.Rows[i]["exposurecurrencymaster_currency"] + "</td></tr>");
        }
        html.Append("</table>");
        ltCurrency.Text = html.ToString();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        int clientId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_ClientId"));
        string query = "delete from tbl_customercurrency where customercurrency_clientid="+clientId;
        DbTable.ExecuteQuery(query);
        query = @"delete from tbl_customercustomfields where customercustomfields_clientid=" + clientId + @"
                  and customercustomfields_columnsid in(select columns_columnsid from tbl_columns
                    join tbl_module on module_moduleid=columns_moduleid where module_modulename IN('FIM Import Order','FIM Trade Credit','FIM Forward Contract'))";
        DbTable.ExecuteQuery(query);
        for (int i = 0; i < Request.Form.Keys.Count; i++)
        {
            string key = Request.Form.Keys[i];
            if (key.StartsWith("che_"))
            {
                Hashtable hstbl = new Hashtable();
                hstbl.Add("clientid", clientId);
                hstbl.Add("exposurecurrencymasterid", key.Replace("che_", ""));
                InsertUpdate obj = new InsertUpdate();
                obj.InsertData(hstbl, "tbl_customercurrency");
            }
            else if (key.StartsWith("chcc_"))
            {
                Hashtable hstbl = new Hashtable();
                hstbl.Add("clientid", clientId);
                hstbl.Add("columnsid", key.Replace("chcc_", ""));
                InsertUpdate obj = new InsertUpdate();
                obj.InsertData(hstbl, "tbl_customercustomfields");
            }
        }
        gblData.SaveForm(form, GetId());
        Bind();
        lblMessage.Text = "Settings saved successfully!";
    }
}
