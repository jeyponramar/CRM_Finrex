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
using iTextSharp.text.pdf;
using WebComponent;
using System.Text;

/// <summary>
/// Summary description for FinrexInvoicePdf
/// </summary>
public class FinrexInvoicePdf
{
	public string GenerateInvoice(int id)
	{
        string fileName = "";
        RPlusPdfGenerator_Finrex obj = new RPlusPdfGenerator_Finrex();
        string query = @"SELECT * FROM tbl_invoice t1
			              LEFT JOIN tbl_client t2 ON t1.invoice_clientid=t2.client_clientid 
                          Left Join tbl_setupfortermsandcondition t5 ON t5.setupfortermsandcondition_setupfortermsandconditionid=t1.invoice_setupfortermsandconditionid 	
                          WHERE t1.invoice_invoiceid =" + id;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null) return "";
        try
        {
            fileName = "invoice-" + Guid.NewGuid().ToString() + "_Invoice_" + GlobalUtilities.ConvertToString(dr["invoice_invoiceno"]).Replace("/", "-") + "_" + id + ".pdf";
            string filePath = HttpContext.Current.Server.MapPath("~/upload/temp/" + fileName);
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
            string html = "";
            string amountword = GlobalUtilities.AmountInWords(dr["invoice_totalamount"]);
            string invoiceDate = "";
            if (dr["invoice_invoicedate"] != DBNull.Value)
            {
                invoiceDate = GlobalUtilities.ConvertToDate(dr["invoice_invoicedate"]);
            }

            obj.Generate(filePath);

            AddHeader(obj);
            html = @"<table cellspacing=0 cellpadding=0><tr><td style='text-align:right;padding-bottom:10px;' width='55%'><font style='font-size:12px;color:#2E3092;'>TAX INVOICE</font></td>
                        <td width='45%' style='text-align:right;font-size:9px;font-weight:normal;color:#222;padding-bottom:10px;'>Original For Recipient</td></tr>
                     </table>
                    ";
            obj.AddHTML(html);
            obj.AddSpace(2,2);
            obj.AddLine("#F6933F");
            html = @"<table cellspacing=0 cellpadding=0>
                        <tr>
                        <td width='50%' valign='top'>
                             <table cellspacing=0 cellpadding=0>
                                <tr><td style='vertical-align:top;'><b>Bill To :</b></td></tr>
                                <tr><td style='font-size:10px;'><b>M/s. " + GlobalUtilities.ConvertToString(dr["client_customername"])+@",</b></td></tr>
                                <tr><td>"+GlobalUtilities.ConvertToString(dr["invoice_billingaddress"]).Replace("\n", "</br>")+@"</td></tr>
                             </table>
                        </td>
                        <td>
                            <table cellspacing=2 cellpadding=0>
                                <tr><td><b>Invoice No.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</b> " + GlobalUtilities.ConvertToString(dr["invoice_invoiceno"]) + @"</td></tr>
                                <tr><td><b>Invoice Date&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</b> " + invoiceDate + @"</td></tr>
                                <tr><td><b>PAN No.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</b> " + Common.GetSetting("Pan card No") + @"</td></tr>
                                <tr><td><b>GST No.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</b> " + Common.GetSetting("GST IN") + @"</td></tr>
                                <tr><td><b>SAC No.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</b> 998311</td></tr>
                             </table>
                        </td>
                        </tr>
                     </table>";
            obj.AddHTML(html);
            obj.AddSpace(2,0);
            html = @"<table cellspacing=0 cellpadding=3 border=0.5>
                        <tr>
                            <td><b>Director Name</b><br/>" + GlobalUtilities.ConvertToString(dr["invoice_directorname"]) + @"</td>
                            <td><b>Director Mobile No</b><br/>" + GlobalUtilities.ConvertToString(dr["invoice_directormobile"]) + @"</td>
                            <td><b>Director Email Id</b><br/>" + GlobalUtilities.ConvertToString(dr["invoice_directoremailid"]) + @"</td>
                            <td><b>GSTIN No</b><br/>" + GlobalUtilities.ConvertToString(dr["invoice_gstin"]) + @"</td>
                        </tr>
                        <tr>
                            <td><b>Finance Name</b><br/>" + GlobalUtilities.ConvertToString(dr["invoice_financename"]) + @"</td>
                            <td><b>Finance Mobile No</b><br/>" + GlobalUtilities.ConvertToString(dr["invoice_financemobile"]) + @"</td>
                            <td><b>Finance Email Id</b><br/>" + GlobalUtilities.ConvertToString(dr["invoice_financeemailid"]) + @"</td>
                            <td><b>Office Tel No.</b><br/>" + GlobalUtilities.ConvertToString(dr["invoice_officetelno"]) + @"</td>
                        </tr>
                    </table>";
            obj.AddHTML(html);
            bool isGst = false;
            query = @"select * from tbl_invoicedetail
                            left join tbl_product on product_productid=invoicedetail_productid
                            LEFT JOIN tbl_servicetype ON invoicedetail_servicetypeid=servicetype_servicetypeid	
                            join tbl_invoice on invoice_invoiceid=invoicedetail_invoiceid
                            where invoicedetail_invoiceid=" + id;
            DataTable dttbl = DbTable.ExecuteSelect(query);
            string tableHtml = GetGSTTableFormat(id, dr, dttbl, "invoice", "invoicedetail", out isGst);
            obj.AddHTML(tableHtml);
            string terms = GlobalUtilities.ConvertToString(dr["setupfortermsandcondition_description"]).Replace("\n", "</br>");
            terms = "TDS on Professional Fees u/s 194J @ 10%<br/>" + terms;
            html = @"<table cellspacing=0 cellpadding=3 border='0.5'>
                        <tr><td colspan='2'>" + terms + @"</td></tr>
                        <tr>
                            <td width='70%'>
                                <table cellspacing=0 cellpadding=1 border='0'>
                                     <tr><td colspan='3'><b><u>Bank Detail NEFT/RTGS :</u></b></td></tr>
                                     <tr><td width='30%'>Beneficiary Name</td><td width='5%'>:</td><td width='65%'>FINREX TREASURY ADVISORS LLP</td></tr>
                                     <tr><td width='30%'>Bank Name</td><td width='5%'>:</td><td width='65%'>ICICI BANK LTD.</td></tr>
                                     <tr><td width='30%'>A/c Number</td><td width='5%'>:</td><td width='65%'>697705600488</td></tr>
                                     <tr><td width='30%'>Branch</td><td width='5%'>:</td><td width='65%'>Andheri Kurla Branch, Visal Apartments, Andheri Kurla Road, Mumbai 400069.</td></tr>
                                     <tr><td width='30%'>IFSC Code</td><td width='5%'>:</td><td width='65%'>ICIC0006977</td></tr>
                                     <tr><td width='30%'>MICR Code</td><td width='5%'>:</td><td width='65%'>400229170</td></tr>
                                </table>
                            </td>
                            <td style='border:solid 1px #ff0000;'>
                                <table cellspacing=0 cellpadding=3 border='0'>
                                    <tr><td><b>For M/s. Finrex Treasury Advisors LLP,</b></td></tr>
                                    <tr><td style='padding-left:50px;'><img width='70' src='" + Custom.AppUrl + @"/images/signature.jpg'/></td></tr>
                                    <tr><td style='text-align:center'><b>Authorised Signatory</b></td></tr>
                                </table>
                            </td>
                        </tr>
                     </table>";
            obj.AddHTML(html);
            obj.Close();
        }
        catch (Exception ex)
        {
            obj.Close();
            return "";
        }
        return fileName;
	}
    public void AddHeader(RPlusPdfGenerator_Finrex obj)
    {
        string html = "";
        PdfPTable table = obj.AddTable(2);
        obj.AddTableCellImg(table, HttpContext.Current.Server.MapPath("~/images/finrex_pdf.png"));
        html = @"<div style='text-align:right;'><div><font style='color:#32347e;font-size:16px;'>" + CustomSettings.CompanyName + @"</font><div>
                            <div><font style='font-size:8px;color:#222;'>" + Common.GetSetting("CompanyAddress").Replace("\n", "</br>") + @"<br/>
                            <b>Tel No. :</b> " + Common.GetSetting("Company Telephone No") + @"   <b>Email Id :</b> " + Common.GetSetting("Company EmailId") + @"    
                            <b>Web Site :</b> " + Common.GetSetting("Company Website") + @"<br/>
                            <b>State :</b> " + Common.GetSetting("state") + @"    <b>State Code :</b> " + Common.GetSetting("state Code") + @"<br/>
                            </font></div>";
        obj.AddTableCellHtml(table, html);
        obj.AddTable(table);
        obj.AddLine("#F6933F");
    }
    private string GetGSTTableFormat(int id, DataRow dr, DataTable dttbl, string mainTable, string subtable, out bool isGST)
    {
        isGST = true;
        if (dttbl.Rows.Count == 0) return "";
        StringBuilder html = new StringBuilder();
        int i = 0;
        bool isDiscountAvailable = false;
        string query = "";
        double totalcgst = 0;
        double totalsgst = 0;
        double totaligst = 0;
        int invoiceFormatVersion = GlobalUtilities.ConvertToInt(dr[mainTable + "_invoiceformatversion"]);

        html.Append("<table border='0.5' style='border-collapse:collapse;border-color:#000;' cellpadding='2' width='100%' cellspacing='0'>");
        if (invoiceFormatVersion == 0)
        {
            html.Append(@"<tr style='font-weight:bold;text-align:center;'><td>Product</td><td>Service</td>
                        <td width='150'>Taxable Amount</td></tr>");
        }
        else
        {
            html.Append(@"<tr style='font-weight:bold;text-align:center;'><td>Services</td><td>Service Plan</td>
                        <td width='150'>Taxable Amount</td></tr>");
        }

        int srNo = 1;
        totalcgst = GlobalUtilities.ConvertToDouble(dttbl.Rows[0]["invoice_cgst"]);
        totalsgst = GlobalUtilities.ConvertToDouble(dttbl.Rows[0]["invoice_sgst"]);
        totaligst = GlobalUtilities.ConvertToDouble(dttbl.Rows[0]["invoice_igst"]);
        double totalDiscount = 0;
        double totalProductAmount = GlobalUtilities.ConvertToDouble(dttbl.Rows[0]["invoice_taxableamount"]);
        double totalTaxableVal = 0;
        string alignRight = " style='text-align:right;'";
        while (i < dttbl.Rows.Count)
        {
            string servicetype = GlobalUtilities.ConvertToString(dttbl.Rows[i]["servicetype_servicetype"]);
            int serviceTypeId = GlobalUtilities.ConvertToInt(dttbl.Rows[i][subtable + "_servicetypeid"]);
            double taxableAmount = GlobalUtilities.ConvertToDouble(dttbl.Rows[i][subtable + "_taxableamount"]);
            string productHtml = "";

            if (invoiceFormatVersion == 0)
            {
                query = @"select * from tbl_servicetypeproduct 
                    left join tbl_product on product_productid=servicetypeproduct_productid
                    where servicetypeproduct_servicetypeid=" + serviceTypeId;
                productHtml = "<div><ul>";
                DataTable dttblServiceProd = DbTable.ExecuteSelect(query);
                for (int j = 0; j < dttblServiceProd.Rows.Count; j++)
                {
                    productHtml += "<li>" + GlobalUtilities.ConvertToString(dttblServiceProd.Rows[j]["product_productname"]) + "</li>";
                }
                productHtml += "</ul></div>";
            }
            else
            {
                query = @"select * from tbl_invoiceservices
                        join tbl_service on service_serviceid=invoiceservices_serviceid
                        where invoiceservices_invoiceid=" + id;
                productHtml = "<div><b><u>Services:</u></b></div>";
                productHtml += "<div><ul style='padding-left:5px;'>";

                DataTable dttblService = DbTable.ExecuteSelect(query);
                for (int j = 0; j < dttblService.Rows.Count; j++)
                {
                    productHtml += "<li>" + GlobalUtilities.ConvertToString(dttblService.Rows[j]["service_service"]) + "</li>";
                }
                productHtml += "</ul></div>";

                query = @"select * from tbl_invoiceprospects
                        join tbl_prospect on prospect_prospectid=invoiceprospects_prospectid
                        where invoiceprospects_invoiceid=" + id;
                productHtml += "<div><b><u>Softwares:</u></b></div>";
                productHtml += "<div><ul style='padding-left:5px;'>";
                DataTable dttblSoftware = DbTable.ExecuteSelect(query);
                for (int j = 0; j < dttblSoftware.Rows.Count; j++)
                {
                    productHtml += "<li>" + GlobalUtilities.ConvertToString(dttblSoftware.Rows[j]["prospect_prospect"]) + "</li>";
                }
                productHtml += "</ul></div>";

                query = "select * from tbl_serviceplan where serviceplan_serviceplanid=" + GlobalUtilities.ConvertToInt(dr["invoice_serviceplanid"]);
                DataRow drsp = DbTable.ExecuteSelectRow(query);
                servicetype = GlobalUtilities.ConvertToString(drsp["serviceplan_planname"]);
            }

            html.Append("<td>" + productHtml + "</td>");
            html.Append("<td>" + servicetype + "</td>");
            html.Append("<td" + alignRight + ">" + FormatAmount_Nbsp(taxableAmount) + "</td>");

            html.Append("</tr>");
            srNo++;
            i++;
        }
        html.Append("<tr><td colspan='3'>Period From : " + GlobalUtilities.ConvertToDate(dttbl.Rows[0]["invoice_periodfrom"]) +
                            " To : " + GlobalUtilities.ConvertToDate(dttbl.Rows[0]["invoice_periodto"]) + "</td></tr>");

        html.Append("<tr style='text-align:right;font-weight:bold;'><td colspan='2'>Total</td>");
        html.Append("<td>" + FormatAmount_Nbsp(totalProductAmount) + "</td>");
        if (totalcgst > 0)
        {
            html.Append("<tr style='text-align:right;font-weight:bold;'><td colspan='2'>CGST 9%</td><td>" + FormatAmount_Nbsp(totalcgst) + "</td></tr>");
        }
        if (totalsgst > 0)
        {
            html.Append("<tr style='text-align:right;font-weight:bold;'><td colspan='2'>SGST 9%</td><td>" + FormatAmount_Nbsp(totalsgst) + "</td></tr>");
        }
        if (totaligst > 0)
        {
            html.Append("<tr style='text-align:right;font-weight:bold;'><td colspan='2'>IGST 18%</td><td>" + FormatAmount_Nbsp(totaligst) + "</td></tr>");
        }

        html.Append("<tr style='text-align:right;font-weight:bold;'><td colspan='2'>Net Total</td>");
        html.Append("<td colspan='2' style='font-size:14px;'>" + GlobalUtilities.FormatAmount(dttbl.Rows[0][mainTable + "_totalamount"]) + "</td>");
        html.Append("</tr>");
        html.Append("<tr><td colspan='3'>Amount in words : <b>" + GlobalUtilities.AmountInWords(dttbl.Rows[0][mainTable + "_totalamount"]) + "</b></td></tr>");
        html.Append("</table>");
        return html.ToString();
    }
    private string FormatAmount_Nbsp(double dblAmount)
    {
        if (dblAmount == 0) return "&nbsp;";
        return String.Format("{0:#.00}", dblAmount);
    }
    private string Nbsp(object val)
    {
        if (GlobalUtilities.ConvertToDouble(val) == 0) return "&nbsp;";
        return val.ToString();
    }
}
