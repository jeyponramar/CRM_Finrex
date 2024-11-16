using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Text;

public partial class _Default : System.Web.UI.Page 
{
    GlobalData gblData = new GlobalData("tbl_expenses", "expensesid");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string query1 = @"SELECT * FROM tbl_expenses 
                                    join tbl_employee on employee_employeeid=expenses_employeeid
                                    WHERE expenses_expensesid=" + GlobalUtilities.ConvertToInt(Request.QueryString["expensesid"]);
            DataTable dta = new DataTable();
            InsertUpdate obj1 = new InsertUpdate();
            dta = obj1.ExecuteSelect(query1);
            if (dta.Rows.Count > 0)
            {

                DataRow dr = (DataRow)dta.Rows[0];
                gblData.PopulateForm(dr, form1);


                double dblTotalAmount = GlobalUtilities.ConvertToDouble(dta.Rows[0]["expenses_totalamount"]);
                totalamount.Text = GlobalUtilities.ConvertToString(dblTotalAmount);
                lblamtinwords.Text = GlobalUtilities.AmountInWords(dblTotalAmount);
                lblvocherdate.Text = GlobalUtilities.ConvertMMDateToDD(Convert.ToString(dta.Rows[0]["expenses_expensedate"]));
                lblvoucherno.Text = GlobalUtilities.ConvertToString(dta.Rows[0]["expenses_voucherno"]);
                lblemployeename.Text = GlobalUtilities.ConvertToString(dta.Rows[0]["employee_employeename"]);
            }

            if (GlobalUtilities.ConvertToInt(Request.QueryString["expensesid"]) > 0)
            {
                string query = @"SELECT * FROM tbl_expensesdetail 
                                    left join tbl_expensetype on expensesdetail_expensetypeid= expensetype_expensetypeid
                                    WHERE expensesdetail_expensesid=" + GlobalUtilities.ConvertToInt(Request.QueryString["expensesid"]);
                DataTable dt = new DataTable();
                InsertUpdate obj = new InsertUpdate();
                dt = obj.ExecuteSelect(query);
                if (dt.Rows.Count > 0)
                {
                                       
                    DataRow dr = (DataRow)dt.Rows[0];
                    gblData.PopulateForm(dr, form1);
                    

                    StringBuilder html = new StringBuilder();
                    html.Append(@"<tr>
                                    <td style='text-align:center;width:70%;' class='border-tl'>Particulars</td>
                                    <td style='text-align:center;width:15%;' class='border-trl'>Rs.</td>
                                </tr>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string strParticulars = Convert.ToString(dt.Rows[i]["expensetype_expensetype"]);
                        string stramount = Convert.ToString(dt.Rows[i]["expensesdetail_amount"]);
                        html.Append(@"<tr>"+
                                    "<td style='text-align:left;width:70%;padding-left:10px;' class='border-tl'>"+ strParticulars +"</td>"+
                                    "<td style='text-align:right;width:15%;padding-right:10px;' class='border-trl'>" + stramount + "</td>" +
                                    //"<td style='text-align:center;width:15%;' class='border-trl'></td>"+
                                "</tr>");
                    }
                    ltvoucher.Text = html.ToString();
                }

            }
            lblTelephoneno.Text = Convert.ToString(Common.GetSetting("Company Telephone No"));
            lblCompanyName.Text = CustomSettings.CompanyName;
            lblEmailId.Text = Convert.ToString(Common.GetSetting("Company EmailId"));
            lblWebsite.Text = Convert.ToString(Common.GetSetting("Company Website"));
            string strCompanyAddress = Convert.ToString(Common.GetSetting("CompanyAddress"));
            strCompanyAddress = strCompanyAddress.Replace("\n", "<br />");
            lblCompamyAddress.Text = strCompanyAddress;            
        }
    }
}
