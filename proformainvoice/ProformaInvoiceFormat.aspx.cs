using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class ProformaInvoice_ProformaInvoiceFormat : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int ProforId = GlobalUtilities.ConvertToInt(Request.QueryString["piid"]);
        FinrexProformaInvoicePdf obj = new FinrexProformaInvoicePdf();
        string fileName = obj.GenerateInvoice(ProforId);
        if (fileName != "")
        {
            Response.Redirect("~/upload/temp/" + fileName);
        }

        SettingsDAO daos = new SettingsDAO();
        lblCompanyName1.Text = CustomSettings.CompanyName;
        lblCompanyName.Text = CustomSettings.CompanyName;
        lblAddress.Text = Common.GetSetting("CompanyAddress").Replace("\n", "</br>"); 
        lblemail.Text = Common.GetSetting("Company EmailId");
        lblphone.Text = Common.GetSetting("Company Telephone No");
        lblpanno.Text = Common.GetSetting("Pan card No");
        lblwebsite.Text = Common.GetSetting("Company Website");
        lblstate.Text = Common.GetSetting("state");
        lblstatecode.Text = Common.GetSetting("state Code");
        lblgstno.Text = Common.GetSetting("GST IN");
        
        int j = 0;
        string query = @"SELECT * FROM tbl_proformainvoice t1
			              LEFT JOIN tbl_client t2 ON t1.proformainvoice_clientid=t2.client_clientid 
                          Left Join tbl_setupfortermsandcondition t5 ON t5.setupfortermsandcondition_setupfortermsandconditionid=t1.proformainvoice_setupfortermsandconditionid 	
                          WHERE t1.proformainvoice_proformainvoiceid =" + ProforId;

        DataRow dr = DbTable.ExecuteSelectRow(query);

        if (dr != null)
        {
            lblquotationno.Text = GlobalUtilities.ConvertToString(dr["proformainvoice_proformainvoiceno"]);
            lblTotalAmount.Text = GlobalUtilities.ConvertToString(dr["proformainvoice_totalamount"]);
            lblClientName.Text = GlobalUtilities.ConvertToString(dr["client_customername"]);
            //lblclientemailid.Text = GlobalUtilities.ConvertToString(dr["proformainvoice_emailid"]);
            //lblTelephone.Text = GlobalUtilities.ConvertToString(dr["client_landlineno"]);
            //lblMobileNo.Text = GlobalUtilities.ConvertToString(dr["proformainvoice_mobileno"]);
            lblClientAddress.Text = GlobalUtilities.ConvertToString(dr["proformainvoice_billingaddress"]).Replace("\n", "</br>");
            string amountword = GlobalUtilities.AmountInWords(dr["proformainvoice_totalamount"]);
            lblBankdetail.Text = GlobalUtilities.ConvertToString(dr["setupfortermsandcondition_description"]).Replace("\n", "</br>");
            lblgstin.Text = GlobalUtilities.ConvertToString(dr["proformainvoice_gstin"]);
            lbldirectoremailid.Text = GlobalUtilities.ConvertToString(dr["proformainvoice_directoremailid"]);
            lbldirectormobileno.Text = GlobalUtilities.ConvertToString(dr["proformainvoice_directormobile"]);
            lbldirectorname.Text = GlobalUtilities.ConvertToString(dr["proformainvoice_directorname"]);
            lblfinancename.Text = GlobalUtilities.ConvertToString(dr["proformainvoice_financename"]);
            lblfinancemobileno.Text = GlobalUtilities.ConvertToString(dr["proformainvoice_financemobile"]);
            lblfinanceemailid.Text = GlobalUtilities.ConvertToString(dr["proformainvoice_financeemailid"]);
            lblofficetelno.Text = GlobalUtilities.ConvertToString(dr["proformainvoice_officetelno"]);
            lblAmountinWord.Text = amountword;
            if (dr["proformainvoice_date"] != DBNull.Value)
            {
                lblDate.Text = GlobalUtilities.ConvertToDate(dr["proformainvoice_date"]);

            }
            DataTable categoryDttb = new DataTable();
            double dblTotalAmount = 0;
            query = @"select * FROM tbl_proformainvoicedetail t1
			            JOIN tbl_proformainvoice t2 ON t1.proformainvoicedetail_proformainvoiceid=t2.proformainvoice_proformainvoiceid
			            LEFT JOIN tbl_Product t4 ON t1.proformainvoicedetail_productid=t4.product_productid	
                        LEFT JOIN tbl_servicetype ON proformainvoicedetail_servicetypeid=servicetype_servicetypeid	
			            left join tbl_unit on unit_unitid=product_unitid
			            left join tbl_tax on tax_taxid=proformainvoicedetail_productid		
                        WHERE t1.proformainvoicedetail_proformainvoiceid = " + ProforId;
            categoryDttb = DbTable.ExecuteSelect(query);
            query = @"select * from tbl_proformainvoicedetail
                            left join tbl_product on product_productid=proformainvoicedetail_productid
                            LEFT JOIN tbl_servicetype ON proformainvoicedetail_servicetypeid=servicetype_servicetypeid	
                            join tbl_proformainvoice on proformainvoice_proformainvoiceid=proformainvoicedetail_proformainvoiceid
                            where proformainvoicedetail_proformainvoiceid=" + ProforId;
            categoryDttb = DbTable.ExecuteSelect(query);
            bool isGst = false;
            string tableHtml = GetGSTTableFormat(ProforId, dr, categoryDttb, "proformainvoice", "proformainvoicedetail", out isGst);
            if (isGst)
            {
                trOldProforInvoiceFormat.Visible = false;
                ltgstTable.Text = tableHtml;
            }
            else
            {
                if (GlobalUtilities.IsValidaTable(categoryDttb))
                {
                    rptinvoice.DataSource = categoryDttb;
                    rptinvoice.DataBind();
                    for (int i = 0; i < categoryDttb.Rows.Count; i++)
                    {
                        if (!GlobalUtilities.ConvertToBool(categoryDttb.Rows[i]["proformainvoicedetail_istax"]))
                        {
                            j = j + 1;

                            Label lblSerialno = (Label)rptinvoice.Items[i].FindControl("lblSerialno");
                            lblSerialno.Text = GlobalUtilities.ConvertToString(j);
                        }
                        Label lblDescription = (Label)rptinvoice.Items[i].FindControl("lblDescription");
                        lblDescription.Text = GlobalUtilities.ConvertToString(categoryDttb.Rows[i]["product_productname"]);
                        string desc = GlobalUtilities.ConvertToString(categoryDttb.Rows[i]["proformainvoicedetail_productdescription"]);
                        if (desc != "")
                        {
                            lblDescription.Text = lblDescription.Text + "<br/>" + desc;
                        }

                        Label lblquantity = (Label)rptinvoice.Items[i].FindControl("lblQuantity");
                        if (GlobalUtilities.ConvertToInt(categoryDttb.Rows[i]["proformainvoicedetail_quantity"]) != 0)
                            lblquantity.Text = GlobalUtilities.ConvertToString(categoryDttb.Rows[i]["proformainvoicedetail_quantity"]);
                        else
                            lblquantity.Text = "";

                        Label lblrate = (Label)rptinvoice.Items[i].FindControl("lblrate");
                        if (GlobalUtilities.ConvertToDouble(categoryDttb.Rows[i]["proformainvoicedetail_rate"]) != 0.00)
                            lblrate.Text = GlobalUtilities.ConvertToString(categoryDttb.Rows[i]["proformainvoicedetail_rate"]);
                        else
                            lblrate.Text = "";
                        if (GlobalUtilities.ConvertToBool(categoryDttb.Rows[i]["proformainvoicedetail_istax"]))
                            lblDescription.Text = GlobalUtilities.ConvertToString(categoryDttb.Rows[i]["tax_tax"]);

                        Label lblamount = (Label)rptinvoice.Items[i].FindControl("lblAmount");
                        if (GlobalUtilities.ConvertToDouble(categoryDttb.Rows[i]["proformainvoicedetail_amount"]) != 0.00)
                            lblamount.Text = GlobalUtilities.ConvertToString(categoryDttb.Rows[i]["proformainvoicedetail_amount"]);
                        else
                            lblamount.Text = "";
                        dblTotalAmount += GlobalUtilities.ConvertToDouble(categoryDttb.Rows[i]["proformainvoicedetail_amount"]);
                        StringBuilder htmltax = new StringBuilder();
                        htmltax.Append(@"<tr>
                        <td colspan='4' align='right' style='border-bottom:1px solid black;border-top:1px solid black;border-left:1px solid black;padding-right:20px;height:30px;font-size:15px;'><b>Sub Total</b></td>" +
                                       "<td align='right' style='border-bottom:1px solid black;border-top:1px solid black;border-left:1px solid black;padding-right:10px;border-right:1px solid black;text-align:right;height:30px;font-size:15px;'><b>" + GlobalUtilities.FormatAmount(dblTotalAmount) + "</b></td>" +
                                   "</tr>");
                        ltsubtotal.Text = htmltax.ToString();
                        ltsubtotal.Visible = true;
                        bindTaxDetails(ProforId);
                    }

                }
                SetCustomizeHeightOnFormat();
            }
        }

    }
    private void SetCustomizeHeightOnFormat()
    {
        string height = Common.GetSetting("ProformaFormatHeight");
        StringBuilder html = new StringBuilder();
        ltsetCustomHeight.Text = "<tr h='" + height + "' class='blankheight'></tr>";
        
    }
    private void bindTaxDetails(int ProforId)
    {
        StringBuilder htmltax = new StringBuilder();
        DataTable dt = DbTable.ExecuteSelect(@"SELECT SUM(proformainvoicedetail_amount)As TaxAmount,MIN(tax_tax) AS Tax FROM tbl_proformainvoicedetail
                                                JOIN tbl_tax ON tax_taxid = proformainvoicedetail_productid
                                                WHERE ISNULL(proformainvoicedetail_istax,0)=1 AND proformainvoicedetail_proformainvoiceid=" + ProforId +
                                               " GROUP BY proformainvoicedetail_productid");
        if (GlobalUtilities.IsValidaTable(dt))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string strTax = GlobalUtilities.ConvertToString(dt.Rows[i]["Tax"]);
                double dblTaxAmount = GlobalUtilities.ConvertToDouble(dt.Rows[i]["TaxAmount"]);
                htmltax.Append(@"<tr>
                        <td colspan='4' align='right' style='padding-right:20px;border-left:1px solid black;border-bottom:1px solid black;font-size:14px;padding-top:5px;padding-bottom:5px;'>" + strTax + "</td>" +
                            "<td align='right' class='border-r' style='border-bottom:1px solid black;border-left:1px solid black;border-right:1px solid black;padding-right:10px;font-size:14px;'>" + GlobalUtilities.FormatAmount(dblTaxAmount) + "</td>" +
                        "</tr>");
            }
            ltTax.Text = htmltax.ToString();
            ltTax.Visible = true;
        }

    }
    private string GetGSTTableFormat(int id, DataRow dr, DataTable dttbl, string mainTable, string subtable, out bool isGST)
    {
        isGST = true;
        StringBuilder html = new StringBuilder();
        int i = 0;
        bool isDiscountAvailable = false;
        string query = "";
        double totalcgst = 0;
        double totalsgst = 0;
        double totaligst = 0;
        int invoiceFormatVersion = GlobalUtilities.ConvertToInt(dr["proformainvoice_invoiceformatversion"]);

        html.Append("<table border='1' style='border-collapse:collapse;border-color:#000;' cellpadding='5' width='100%' cellspacing='0'>");
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
        totalcgst = GlobalUtilities.ConvertToDouble(dttbl.Rows[0]["proformainvoice_cgst"]);
        totalsgst = GlobalUtilities.ConvertToDouble(dttbl.Rows[0]["proformainvoice_sgst"]);
        totaligst = GlobalUtilities.ConvertToDouble(dttbl.Rows[0]["proformainvoice_igst"]); 
        double totalDiscount = 0;
        double totalProductAmount = GlobalUtilities.ConvertToDouble(dttbl.Rows[0]["proformainvoice_taxableamount"]);
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
                query = @"select * from tbl_proformainvoiceservices
                        join tbl_service on service_serviceid=proformainvoiceservices_serviceid
                        where proformainvoiceservices_proformainvoiceid=" + id;
                productHtml = "<div><ul>";
                DataTable dttblService= DbTable.ExecuteSelect(query);
                for (int j = 0; j < dttblService.Rows.Count; j++)
                {
                    productHtml += "<li>" + GlobalUtilities.ConvertToString(dttblService.Rows[j]["service_service"]) + "</li>";
                }
                productHtml += "</ul></div>";
                query = "select * from tbl_serviceplan where serviceplan_serviceplanid=" + GlobalUtilities.ConvertToInt(dr["proformainvoice_serviceplanid"]);
                DataRow drsp = DbTable.ExecuteSelectRow(query);
                servicetype = GlobalUtilities.ConvertToString(drsp["serviceplan_planname"]);
            }
            html.Append("<tr>");
            html.Append("<td>" + productHtml + "</td>");
            html.Append("<td>" + servicetype + "</td>");
            html.Append("<td" + alignRight + ">" + FormatAmount_Nbsp(taxableAmount) + "</td>");
            
            html.Append("</tr>");
            srNo++;
            i++;
        }
        html.Append("<tr><td colspan='3'>Period From : " + GlobalUtilities.ConvertToDate(dttbl.Rows[0]["proformainvoice_periodfrom"]) +
                            " To : " + GlobalUtilities.ConvertToDate(dttbl.Rows[0]["proformainvoice_periodto"]) + "</td></tr>");

        html.Append("<tr style='text-align:right;font-weight:bold;'><td colspan='2'>Total</td>");
        html.Append("<td>" + FormatAmount_Nbsp(totalProductAmount) + "</td>");
        //if (isDiscountAvailable)
        //{
        //    html.Append("<td>" + FormatAmount_Nbsp(totalDiscount) + "</td>");
        //    html.Append("<td>" + FormatAmount_Nbsp(totalTaxableVal) + "</td>");
        //}
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
        html.Append("<td colspan='2' style='font-size:20px;'>" + GlobalUtilities.FormatAmount(dttbl.Rows[0][mainTable + "_totalamount"]) + "</td>");
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
