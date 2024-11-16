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
using System.Xml.Linq;
using System.Collections;
using WebComponent;

/// <summary>
/// Summary description for Invoice
/// </summary>
public enum Chargeable
{
    Chargeable = 1,
    NonChargeble = 2
}
public class Invoice
{
    public static void SaveInvoicePending(string invoicefor, int mid, string subject, int clientId, string referenceNo, string date, int intChargeable)
    {
        Chargeable chargeable = Chargeable.NonChargeble;
        if (intChargeable > 0)
        {
            chargeable = (Chargeable)intChargeable;
        }
        string module = Common.GetModuleName(); 
        if (Common.GetQueryStringValue("id") == 0)//add
        {
            if (chargeable == Chargeable.NonChargeble)
            {
                return;
            }
        }
        else
        {
            if (chargeable == Chargeable.NonChargeble)
            {
                DeleteInvicePending(module, mid);
                return;
            }
        }
        //check data already exists
        string query = "select * from tbl_invoicepending WHERE invoicepending_module='" + module + "' AND invoicepending_moduleid=" + mid;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        
        Hashtable hstbl = new Hashtable();
        hstbl.Add("module", module);
        hstbl.Add("moduleid", mid);
        hstbl.Add("subject", subject);
        hstbl.Add("clientid", clientId);
        hstbl.Add("date", date);
        hstbl.Add("invoicefor", invoicefor);
        hstbl.Add("referenceno", referenceNo);
        InsertUpdate obj = new InsertUpdate();
        if (dr == null)
        {
            obj.InsertData(hstbl, "tbl_invoicepending");
        }
        else
        {
            obj.UpdateData(hstbl, "tbl_invoicepending", GlobalUtilities.ConvertToInt(dr["invoicepending_invoicependingid"]));
        }
    }
    public static void DeleteInvicePending(string module, int mid)
    {
        string query = "delete from tbl_invoicepending WHERE invoicepending_module='" + module + "' AND invoicepending_moduleid=" + mid;
        DbTable.ExecuteQuery(query);
    }
}
