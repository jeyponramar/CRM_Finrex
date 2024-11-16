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
using iTextSharp.text.pdf;
using System.Text;

public partial class testpdf : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
//        string html = @"<table>
//                            <tr><td>c1</td><td><table><tr><td>c2</td></tr></table></td><td>c3</td>
//                            </tr>
//                            <tr><td>d1</td><td>d2</td><td>d3</td></tr>
//                       </table>";
        GeneratePdf();
    }
    private void GeneratePdf()
    {
        //FinrexInvoicePdf obj = new FinrexInvoicePdf();
        //obj.GenerateInvoice(12);
        FinrexProformaInvoicePdf obj = new FinrexProformaInvoicePdf();
        obj.GenerateInvoice(17);
    }
    /*
    public void GeneratePdf1()
    {
        string fileName = Server.MapPath("~/upload/temp/test1.pdf");
        RPlusPdfGenerator obj = new RPlusPdfGenerator();
        obj.Generate(fileName);
        try
        {
            string html = "";
            PdfPTable table = obj.AddTable(2);
            obj.AddTableCellImg(table, "E:\\Disk02\\Ram\\Websites\\CRM_Finrex\\images\\finrex.png");
            html = @"<div style='text-align:right;'><div><font style='color:#2E3092;font-size:16px;'>Finrex Treasury Advisors LLP</font><div>
                                    <div><font style='font-size:9px;color:#444444;'>Empress Nucleus, 1st Floor, Andheri Kurla Road,<br/>
                                    Next to Little Flower Education School, Andheri East,<br/>
                                    Mumbai 400069.</font></div>";
            obj.AddTableCellHtml(table, html);
            obj.AddTable(table);

            html = @"<table border='0.5' style='border-collapse:collapse;border-color:#0070c0;' bordercolor='#0070c0' cellpadding='5' width='100%' cellspacing='0'>
                        <tr><td colspan='6'></td><td style='text-align:right;font-weight:bold;' colspan='2' bgcolor='#ffffff'>Supply</td>
                            <td style='text-align:right;font-weight:bold;' colspan='2' bgcolor='#ffffff'>Installation</td>
                        </tr>
                        <tr style='font-weight:bold;text-align:center;background-color:#dce6f1;'><td bgcolor='#dce6f1'>S.NO</td>
                                <td bgcolor='#dce6f1'>Make</td><td bgcolor='#dce6f1'>Model No</td><td bgcolor='#dce6f1'>PARTICULARS</td>
                                <td bgcolor='#dce6f1'>HSN</td><td bgcolor='#dce6f1'>UNIT</td><td bgcolor='#dce6f1'>QTY</td><td bgcolor='#dce6f1'>RATE</td>
                                <td bgcolor='#dce6f1'>TOTAL COST</td><td bgcolor='#dce6f1'>RATE</td><td bgcolor='#dce6f1'>TOTAL COST</td>
                        </tr>
                        <tr><td>1000</td><td>Hikvision</td><td>HIK-005</td>
                            <td>Camera Hikvision HIK-005<br><div>Shutter Speed : 1/3s ~ 1/100,000s</div><div>Lens:4.7 - 94mm 
                                    @ F1.4 angle of view:53.8°~3.1°</div><div>Lens Mount : AF automatic focusing and motorized zoom lens</div>
                                <div>Day&amp; Night : IR Cut</div>
                            </td><td></td><td>No</td><td style='text-align:right;'>3</td><td style='text-align:right;'>1,300.00</td>
                            <td style='text-align:right;'>3,900.00</td><td style='text-align:right;'>&nbsp;</td><td style='text-align:right;'>&nbsp;</td>
                         </tr>
                         <tr><td style='text-align:right;font-weight:bold;' colspan='7'>TOTAL</td>
                            <td style='text-align:right;font-weight:bold;' colspan='2'>3,900.00</td>
                            <td style='text-align:right;font-weight:bold;' colspan='2'>0.00</td>
                        </tr>
                        <tr><td style='text-align:right;font-weight:bold;' colspan='7'>TOTAL (SUPPLY + INSTALLATION)</td>
                            <td colspan='4' style='text-align:right;font-weight:bold;'>3,900.00</td>        
                        </tr>
                        <tr><td colspan='7' style='text-align:right;font-weight:bold;'>GRAND TOTAL</td>
                            <td style='text-align:right;font-weight:bold;font-size:16px;' colspan='4'>3,900.00</td>
                        </tr></tbody>
                        </table>";

            html = @"<table border='0.5' style='border-collapse:collapse;border-color:#0070c0;' bordercolor='#0070c0' cellpadding='5' width='100%' cellspacing='0'>
                        <tr>
                            <td colspan='6'></td><td style='text-align:right;font-weight:bold;' colspan='2' bgcolor='#ffffff'>Supply</td>
                            <td style='text-align:right;font-weight:bold;' colspan='2' bgcolor='#ffffff'>Installation</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr style='font-weight:bold;text-align:center;background-color:#dce6f1;'>
                                <td bgcolor='#dce6f1'>S.NO</td>
                                <td bgcolor='#dce6f1'>Make</td><td bgcolor='#dce6f1'>Model No</td><td bgcolor='#dce6f1'>PARTICULARS</td>
                                <td bgcolor='#dce6f1'>HSN</td><td bgcolor='#dce6f1'>UNIT</td><td bgcolor='#dce6f1'>QTY</td><td bgcolor='#dce6f1'>RATE</td>
                                <td bgcolor='#dce6f1'>TOTAL COST</td><td bgcolor='#dce6f1'>RATE</td><td bgcolor='#dce6f1'>TOTAL COST</td>
                        </tr>
                        <tr><td>1000</td><td>LENOVA</td><td>LEN-005</td>
                            <td>Camera LENOVA LEN-005 Shutter Speed : 1/3s ~ 1/100,000s Lens:4.7 - 94mm 
                                    @ F1.4 angle of view:53.8°~3.1° Lens Mount : AF automatic focusing and motorized zoom lens Day & Night : IR Cut</td>
                            <td>1000</td><td>1000</td><td>1000</td><td>1000</td><td>1000</td><td>1000</td><td>1000.00</td>
                        </tr></tbody>
                        </table>";

            obj.AddTableByHtml(html, new float[] { 7, 10, 10, 25, 7, 7, 7, 6, 12, 6, 12 });

            obj.Close();
        }
        catch (Exception ex)
        {
            obj.Close();
        }
    }
    */
}
