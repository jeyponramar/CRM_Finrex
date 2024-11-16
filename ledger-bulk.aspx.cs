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
using System.Text;

public partial class ledger_bulk : System.Web.UI.Page
{
    GlobalData gbldata = new GlobalData("tbl_ledgergroup", "ledgergroupid");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CustomSession.Session("Login_IsRefuxLoggedIn") == null)
        {
            Response.Redirect("../adminlogin.aspx");
        }
        if (!IsPostBack)
        {
            gbldata.FillDropdown(ddlGroup, "tbl_ledgergroup", "ledgergroup_ledgergroupname", "ledgergroup_ledgergroupid", "", "ledgergroup_ledgergroupname");
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtModule.Text == "") return;
        string query = "select * from tbl_" + txtModule.Text;
        DataTable dttbl = DbTable.ExecuteSelect(query);
        int count = 0;
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int mid = GlobalUtilities.ConvertToInt(dttbl.Rows[i][0]);
            string ledgerName = global.CheckData(GlobalUtilities.ConvertToString(dttbl.Rows[i][txtLedgerName.Text]));
            int ledgerId = Accounts.SaveLedger(ledgerName, (LedgerGroup)Convert.ToInt32(ddlGroup.SelectedValue), "", txtModule.Text, mid,
                        (LedgerType)Convert.ToInt32(ddlLedgerType.SelectedValue));
            if (ledgerId > 0) count++;
        }
        lblMessage.Text = "Ledger created successfully!, Total created : " + count.ToString();
    }
}
