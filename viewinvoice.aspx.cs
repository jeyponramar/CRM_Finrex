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
using System.Text;
using WebComponent;

public partial class viewinvoice : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int tab = Common.GetQueryStringValue("t");
            if (tab == 0)
            {
                BindInvoice();
            }
            else if (tab == 1)
            {
                BindProformaInvoice();
            }
            BindTabs();
        }
    }
    private void BindInvoice()
    {
        string query = "";
        int clientId = Common.ClientId;
        query = @"select * from tbl_invoice
                LEFT JOIN tbl_invoicestatus ON invoicestatus_invoicestatusid=invoice_invoicestatusid
                where invoice_clientid=" + clientId;
        query += " order by invoice_invoiceid desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%' cellpadding=7 class='grid-ui' border=1>");
        html.Append(@"<tr class='grid-ui-header'><td>Invoice No</td><td>Invoice Date</td><td>Period from</td><td>Period to</td><td>Amount</td><td>Status</td><td>Download</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["invoice_invoiceid"]);
            string css = "grid-ui-row-alt";
            string bgcolor = GlobalUtilities.ConvertToString(dttbl.Rows[i]["invoicestatus_backgroundcolor"]);
            string color = GlobalUtilities.ConvertToString(dttbl.Rows[i]["invoicestatus_textcolor"]);
            if (i % 2 == 0)
            {
                css = "grid-ui-row";
            }
            html.Append("<tr class='" + css + "'>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["invoice_invoiceno"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["invoice_invoicedate"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["invoice_periodfrom"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["invoice_periodto"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["invoice_totalamount"]) + "</td>");
            html.Append("<td><div class='grid-status' style='background-color:#" + bgcolor + ";color:#" + color + ";'>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["invoicestatus_status"]) + "</div></td>");
            html.Append(@"<td><a href='download-pdf.aspx?m=invoice&id=" + id + "' target='_blank'>Download</a></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
    private void BindProformaInvoice()
    {
        string query = "";
        int clientId = Common.ClientId;
        query = @"select * from tbl_proformainvoice
                LEFT JOIN tbl_salesstatus ON salesstatus_salesstatusid=proformainvoice_salesstatusid
                where proformainvoice_clientid=" + clientId;
        query += " order by proformainvoice_proformainvoiceid desc";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table width='100%' cellpadding=7 class='grid-ui' border=1>");
        html.Append(@"<tr class='grid-ui-header'><td>Proforma No</td><td>Proforma Date</td><td>Period from</td><td>Period to</td><td>Amount</td><td>Status</td><td>Download</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            int id = GlobalUtilities.ConvertToInt(dttbl.Rows[i]["proformainvoice_proformainvoiceid"]);
            string css = "grid-ui-row-alt";
            string bgcolor = GlobalUtilities.ConvertToString(dttbl.Rows[i]["salesstatus_backgroundcolor"]);
            string color = GlobalUtilities.ConvertToString(dttbl.Rows[i]["salesstatus_textcolor"]);
            if (i % 2 == 0)
            {
                css = "grid-ui-row";
            }
            html.Append("<tr class='" + css + "'>");
            html.Append("<td>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["proformainvoice_proformainvoiceno"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["proformainvoice_date"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["proformainvoice_periodfrom"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToDate(dttbl.Rows[i]["proformainvoice_periodto"]) + "</td>");
            html.Append("<td>" + GlobalUtilities.ConvertToDouble(dttbl.Rows[i]["proformainvoice_totalamount"]) + "</td>");
            html.Append("<td><div class='grid-status' style='background-color:#" + bgcolor + ";color:#" + color + ";'>" + GlobalUtilities.ConvertToString(dttbl.Rows[i]["salesstatus_status"]) + "</div></td>");
            html.Append(@"<td><a href='download-pdf.aspx?m=proformainvoice&id=" + id + "' target='_blank'>Download</a></td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
    private void BindTabs()
    {
        int tab = Common.GetQueryStringValue("t");
        StringBuilder html = new StringBuilder();
        html.Append("<ul class='line-tab'>");
        html.Append("<li" + (tab == 0 ? " class='line-tab-active'" : "") + "><a href='viewinvoice.aspx?t=0'>Invoice History</a></li>");
        html.Append("<li" + (tab == 1 ? " class='line-tab-active'" : "") + "><a href='viewinvoice.aspx?t=1'>Proforma Invoice Due</a></li>");
        html.Append("</ul>");
        lttab.Text = html.ToString();
    }
}
