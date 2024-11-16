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

public partial class correctdata : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btncorrectsubscriptionprospects_Click(object sender, EventArgs e)
    {
        string query = "";
        query = "select * from tbl_client where client_subscriptionstatusid=2";
        DataTable dttblclient = DbTable.ExecuteSelect(query);
        query = @"if object_id('tbl_clientprospects_bk') is not null 
                    drop table tbl_clientprospects_bk
                    select * into tbl_clientprospects_bk from tbl_clientprospects
                    if object_id('tbl_clientservices_bk') is not null
                    drop table tbl_clientservices_bk
                    select * into tbl_clientservices_bk from tbl_clientservices
  
                    if object_id('tbl_subscriptionprospects_bk') is not null 
                    drop table tbl_subscriptionprospects_bk
                    select * into tbl_subscriptionprospects_bk from tbl_subscriptionprospects
                    if object_id('tbl_subscriptionservices_bk') is not null
                    drop table tbl_subscriptionservices_bk
                    select * into tbl_subscriptionservices_bk from tbl_subscriptionservices";
        DbTable.ExecuteQuery(query);
        for (int i = 0; i < dttblclient.Rows.Count; i++)
        {
            int clientId = GlobalUtilities.ConvertToInt(dttblclient.Rows[i]["client_clientid"]);
            query = "select top 1 * from tbl_invoice where invoice_clientid=" + clientId +
                    " order by invoice_invoiceid desc";
            DataRow drinvoice = DbTable.ExecuteSelectRow(query);
            if (drinvoice == null) continue;
            int invoiceId = GlobalUtilities.ConvertToInt(drinvoice["invoice_invoiceid"]);
            int servicePlanId = GlobalUtilities.ConvertToInt(drinvoice["invoice_serviceplanid"]);
            Custom.UpdateClientProspects("invoice", invoiceId, clientId, servicePlanId);

            UpdateSubscription(clientId, invoiceId);
        }
        lblMessage.Text = "Data corrected successfully.";
    }
    private void UpdateSubscription(int clientId, int invoiceId)
    {
        string query = "select top 1 * from tbl_subscription where subscription_clientid=" + clientId + " order by 1 desc";
        DataRow drs = DbTable.ExecuteSelectRow(query);
        if (drs == null) return;
        int subscriptionId = GlobalUtilities.ConvertToInt(drs["subscription_subscriptionid"]);
        Hashtable hstbl = new Hashtable();
        query = @"update tbl_subscription set subscription_invoiceperiodfrom=(select invoice_periodfrom from tbl_invoice where invoice_invoiceid=" + invoiceId + @"),
                 subscription_invoiceperiodto=(select invoice_periodto from tbl_invoice where invoice_invoiceid=" + invoiceId + @"),
                 subscription_latestinvoicedate=(select invoice_invoicedate from tbl_invoice where invoice_invoiceid=" + invoiceId + @")
                 where subscription_subscriptionid=" + subscriptionId;
        DbTable.ExecuteQuery(query);
    }
}
