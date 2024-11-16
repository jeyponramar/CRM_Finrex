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

public partial class DailyHistoricalLiveRate_add : System.Web.UI.Page
{
    GlobalData gblData = new GlobalData("tbl_dailyhistoricalliverate", "dailyhistoricalliverateid");
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gblData.FillDropdown(ddlcurrencyid, "tbl_currency", "currency_currency", "");
            if (Request.QueryString["id"] != null)
            {
                Populate();
            }
        }
        if (Request.QueryString["id"] == null)
        {
            lblPageTitle.Text = "Add Daily Historical Live Rate";
        }
        else
        {
            lblPageTitle.Text = "Edit Daily Historical Live Rate";
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>setTitle('" + lblPageTitle.Text + "')</script>");
    }
    private void Populate()
    {
        string query = "";
        query = @"select * from tbl_dailyhistoricalliverate
                  join tbl_liverate on liverate_liverateid=dailyhistoricalliverate_liverateid
                   where dailyhistoricalliverate_dailyhistoricalliverateid=" + GetId();
        DataRow dr = DbTable.ExecuteSelectRow(query);
        ddlcurrencyid.SelectedValue = GlobalUtilities.ConvertToString(dr["liverate_currencyid"]);
        txtdate.Text = GlobalUtilities.ConvertToDate(dr["dailyhistoricalliverate_date"]);
        BindRates();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveData(true);
    }

    private void SaveData(bool isclose)
    {
        lblMessage.Visible = false;
        int currencyId = GlobalUtilities.ConvertToInt(ddlcurrencyid.SelectedValue);
        string query = "";
        string date = GlobalUtilities.ConvertMMDateToDD(global.CheckInputData(txtdate.Text));
        for (int i = 0; i < Request.Form.Keys.Count; i++)
        {
            string key = Request.Form.Keys[i];
            if (key.StartsWith("dhrate_"))
            {
                int liverateId = GlobalUtilities.ConvertToInt(key.Split('_').GetValue(1));
                query = @"select * from tbl_dailyhistoricalliverate 
                          where cast(dailyhistoricalliverate_date as date)=cast('" + date + @"' as date)
                          and dailyhistoricalliverate_liverateid=" + liverateId;
                DataRow dr = DbTable.ExecuteSelectRow(query);
                Hashtable hstbl = new Hashtable();
                hstbl.Add("liverateid", liverateId);
                hstbl.Add("currentrate", global.CheckInputData(Request.Form[key]));
                hstbl.Add("date", txtdate.Text);
                InsertUpdate obj = new InsertUpdate();
                if (dr == null)
                {
                    obj.InsertData(hstbl, "tbl_dailyhistoricalliverate");
                }
                else
                {
                    obj.UpdateData(hstbl, "tbl_dailyhistoricalliverate", GlobalUtilities.ConvertToInt(dr["dailyhistoricalliverate_dailyhistoricalliverateid"]));
                }
            }
        }
        BindRates();
        lblMessage.Text = "Data saved successfully!";
        lblMessage.Visible = true;
    
    }
    protected void ddlcurrencyid_Changed(object sender, EventArgs e)
    {
        BindRates();
    }
    private void BindRates()
    {
        string query = "";
        if (txtdate.Text == "") return;
        string date = GlobalUtilities.ConvertMMDateToDD(global.CheckInputData(txtdate.Text));
        int currencyId = GlobalUtilities.ConvertToInt(ddlcurrencyid.SelectedValue);
        query = @"select * from tbl_dailyhistoricalliverate where cast(dailyhistoricalliverate_date as date)=cast('" + date + "' as date)";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table class='repeater' border='1'>");
        html.Append("<tr><td class='bold'>Description</td><td>CASH/SPOT</td>");
        int startIndex1 = 182;
        int endIndex1 = 193;
        int startIndex2 = 194;
        int endIndex2 = 206;
        int startIndex3 = 207;
        int endIndex3 = 219;
        int startIndex4 = 220;
        int endIndex4 = 232;
        for (int i = startIndex1; i <= endIndex1; i++)
        {
            html.Append("<td><input type='text' class='mbox' name='dhrate_" + i + "' value='" + GetRate(dttbl, i) + "'/></td>");
        }
        html.Append("</tr>");
        html.Append("<tr><td class='bold'>Month End Date</td>");
        for (int i = startIndex2; i <= endIndex2; i++)
        {
            html.Append("<td><input type='text' class='mbox' name='dhrate_" + i + "' value='" + GetRate(dttbl, i) + "'/></td>");
        }
        html.Append("</tr>");
        html.Append("<tr><td class='bold'>BID (Export)</td>");
        for (int i = startIndex3; i <= endIndex3; i++)
        {
            html.Append("<td><input type='text' class='mbox' name='dhrate_" + i + "' value='" + GetRate(dttbl, i) + "'/></td>");
        }
        html.Append("</tr>");
        html.Append("<tr><td class='bold'>ASK (Import)</td>");
        for (int i = startIndex4; i <= endIndex4; i++)
        {
            html.Append("<td><input type='text' class='mbox' name='dhrate_" + i + "' value='" + GetRate(dttbl, i) + "'/></td>");
        }
        html.Append("</tr>");
        html.Append("</table>");
        ltrate.Text = html.ToString();
    }
    private string GetRate(DataTable dttbl, int rateId)
    {
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            if (GlobalUtilities.ConvertToInt(dttbl.Rows[i]["dailyhistoricalliverate_liverateid"]) == rateId)
            {
                return GlobalUtilities.ConvertToString(dttbl.Rows[i]["dailyhistoricalliverate_currentrate"]);
            }
        }
        return "";
    }
    private void EnableControlsOnEdit()
	{
		if (Request.QueryString["id"] == null) return;
		btnDelete.Visible = true;
	}
    private int GetId()
    {
        if (h_IsCopy.Text == "1")
        {
            return 0;
        }
        else
        {
            return GlobalUtilities.ConvertToInt(Request.QueryString["id"]);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Common.Delete();
    }  
    
}
