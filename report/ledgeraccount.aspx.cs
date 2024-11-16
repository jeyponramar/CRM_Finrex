using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;

public partial class ledgeraccountreport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int ledgerType = GlobalUtilities.ConvertToInt(Request.QueryString["ltype"]);
            LedgerType ltype = (LedgerType)ledgerType;
            string title = ltype.ToString() + " Account";
            lblPageTitle.Text = title;
            lblLedgerType.Text = ltype.ToString();
            if (ledgerType > 0)
            {
                ledger.Attributes.Add("w", "ledger_ledgertype=" + ledgerType);
            }
            //RightPanel_START
            //RightPanel_END
        }
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        grid.Module = "ledgeraccount";//to load grid only on click
        grid.BindReport();
        int ledgerId = GlobalUtilities.ConvertToInt(txtledgerid.Text);
        lblOpeningBal.Text = "Opening Balance : <b>" + Accounts.GetOpeningBal(ledgerId, txtVoucherDate_From.Text) + "</b> &nbsp;" +
                             "Closing Balance : <b>" + Accounts.GetAccountBal(ledgerId) + "<b>";
    }
    
}