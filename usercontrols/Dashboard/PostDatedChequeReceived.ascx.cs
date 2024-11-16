using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebComponent;
using System.Collections;
using System.Text;
using System.IO;

public partial class usercontrols_Dashboard_PostDatedChequeReceived : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindPostDatedChequeReceivedreminder();
    }
    private void BindPostDatedChequeReceivedreminder()
    {
        
        string query = @"SELECT * FROM tbl_ledgervoucher
                JOIN tbl_ledger ON ledgervoucher_ledgerid=ledger_ledgerid
                JOIN tbl_accountvouchertype ON ledgervoucher_accountvouchertypeid=accountvouchertype_accountvouchertypeid
                JOIN tbl_accountadjustmentmethod ON ledgervoucher_accountadjustmentmethodid=accountadjustmentmethod_accountadjustmentmethodid
                JOIN tbl_paymentmode ON ledgervoucher_paymentmodeid=paymentmode_paymentmodeid
        			WHERE ledgervoucher_accountvouchertypeid = 5
                AND DATEDIFF(day,GETDATE(),ledgervoucher_voucherdate) BETWEEN 0 AND 2";
        DataTable dttblAmc = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%'>");
        for (int i = 0; i < dttblAmc.Rows.Count; i++)
        {
            DataRow dr = dttblAmc.Rows[i];
            string ledgerName = GlobalUtilities.ConvertToString(dr["ledger_ledgername"]);
            string date = GlobalUtilities.ConvertToDate(dr["ledgervoucher_voucherdate"]);
            string chequeno = GlobalUtilities.ConvertToString(dr["ledgervoucher_chequeno"]);
            string referanceno = GlobalUtilities.ConvertToString(dr["ledgervoucher_referenceno"]);
            string rowcss = "dashboard-alt";
            if (i % 2 == 0) rowcss = "dashboard-row";
            html.Append("<tr class='" + rowcss + "'>");
            html.Append("<td><table width='100%'><tr><td class='bold'>" + referanceno + "</td><td class='bold'>" + ledgerName + "</td><td>" + chequeno + "</td><td align='right'>" + date + "</td></tr>");
            html.Append("</table>");
            html.Append("</td></tr>");
        }
        html.Append("</table>");
        ltPostDatedChequeReceived.Text = html.ToString();
    }
}
