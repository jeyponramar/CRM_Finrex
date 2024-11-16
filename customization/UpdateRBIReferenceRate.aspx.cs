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

public partial class customization_UpdateRBIReferenceRate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateRates();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        UpdateRate(txtrate1, 1098);
        UpdateRate(txtdate1, 1099);
        UpdateRate(txtrate2, 1100);
        UpdateRate(txtdate2, 1101);
        UpdateRate(txtrate3, 1102);
        UpdateRate(txtdate3, 1103);
        UpdateRate(txtrate4, 1104);
        UpdateRate(txtdate4, 1105);
        lblMessage.Text = "Data saved successfully!";
    }
    private void PopulateRates()
    {
        string query = "select * from tbl_liverate where liverate_liverateid between 1098 and 1105";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int liveRateId = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["liverate_liverateid"]);
            if (liveRateId == 1098)
            {
                txtrate1.Text = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_currentrate"]);
            }
            else if (liveRateId == 1099)
            {
                txtdate1.Text = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_currentrate"]);
            }
            else if (liveRateId == 1100)
            {
                txtrate2.Text = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_currentrate"]);
            }
            else if (liveRateId == 1101)
            {
                txtdate2.Text = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_currentrate"]);
            }
            else if (liveRateId == 1102)
            {
                txtrate3.Text = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_currentrate"]);
            }
            else if (liveRateId == 1103)
            {
                txtdate3.Text = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_currentrate"]);
            }
            else if (liveRateId == 1104)
            {
                txtrate4.Text = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_currentrate"]);
            }
            else if (liveRateId == 1105)
            {
                txtdate4.Text = GlobalUtilities.ConvertToString(dttbl.Rows[i]["liverate_currentrate"]);
            }
        }
    }
    private void UpdateRate(TextBox txt, int liveRateId)
    {
        string rate = txt.Text;
        if (rate == "") return;
        //if (txt.ID.Contains("date")) rate =  global.ConvertMMDateToDD(rate);
        rate = global.CheckInputData(rate);
        string query = "UPDATE tbl_liverate SET liverate_prevrate=liverate_currentrate,liverate_currentrate='" + rate + "',liverate_modifieddate=GETDATE() WHERE liverate_liverateid=" + liveRateId + "";
        DbTable.ExecuteQuery(query);
    }
}
