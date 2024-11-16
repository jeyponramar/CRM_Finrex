using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Text;

public partial class viewfpiinvestment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Custom.CheckSubscriptionAccess();
        Finstation.CheckFullFinstationAccess();
        if (!IsPostBack)
        {
            GlobalData gbldata = new GlobalData();
            //gbldata.FillDropdown(ddlcurrency, "tbl_currency", "currency_currency", "");
            BindYear();
        }
    }
    private void BindYear()
    {
        string query = "select distinct year(fpiinvestment_date) as yr from tbl_fpiinvestment order by year(fpiinvestment_date) desc";
        DataTable dttbl=DbTable.ExecuteSelect(query);
        ddlyear.DataSource = dttbl;
        ddlyear.DataTextField = "yr";
        ddlyear.DataValueField = "yr";
        ddlyear.DataBind();
    }
    protected void ddlreporttype_changed(object sender, EventArgs e)
    {
        if (ddlreporttype.SelectedValue == "0")
        {
            lbltitle.Text = "FPI Net Investments";
            tryear.Visible = false;
            trdate.Visible = true;
        }
        else
        {
            lbltitle.Text = "Monthly FPI Net Investments";
            tryear.Visible = true;
            trdate.Visible = false;
        }

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //if (ddlcurrency.SelectedIndex == 0)
        //{
        //    ltdata.Text = "<span class='error'>Please select currency!</span>";
        //    return;
        //}
        int type = Convert.ToInt32(ddlreporttype.SelectedValue);
        string query = "";
        if (type == 0)
        {
            query = @"select top 1000 * from tbl_fpiinvestment
                      where 1=1";
            //query += " where fpiinvestment_currencyid=" + ddlcurrency.SelectedValue;
            if (txtfromdate.Text != "") query += " AND cast(fpiinvestment_date as date)>=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(txtfromdate.Text)) + "' as date)";
            if (txttodate.Text != "") query += " AND cast(fpiinvestment_date as date)<=cast('" + global.CheckInputData(GlobalUtilities.ConvertMMDateToDD(txttodate.Text)) + "' as date)";
            query += " order by fpiinvestment_date desc";
        }
        else
        {
            query = @"select top 1000 month(fpiinvestment_date) as m, sum(fpiinvestment_equity) as fpiinvestment_equity,sum(fpiinvestment_debt) as fpiinvestment_debt,
                        sum(fpiinvestment_debtvrr) as fpiinvestment_debtvrr,sum(fpiinvestment_hybrid) as fpiinvestment_hybrid,
                        sum(fpiinvestment_debtfar) as fpiinvestment_debtfar, sum(fpiinvestment_mutualfund) as fpiinvestment_mutualfund,
                        sum(fpiinvestment_aifs) as fpiinvestment_aifs
                        from tbl_fpiinvestment ";
            query += " where year(fpiinvestment_date)=" + ddlyear.SelectedValue +" group by month(fpiinvestment_date)";
        }
        DataTable dttbl = DbTable.ExecuteSelect(query);
        StringBuilder html = new StringBuilder();
        html.Append("<table class='grid-ui' cellpadding='7' border='1'>");
        html.Append(@"<tr class='grid-ui-header'><td>Date</td><td>Equity</td><td>Debt</td><td>Debt VRR</td><td>Debt-FAR</td><td>Hybrid</td>
                        <td>Total Debt</td><td>Mutual Fund</td><td>AIFS</td><td>Total</td></tr>");
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            string css = "grid-ui-alt";
            if (i % 2 == 0) css = "grid-ui-row";
            string date = "";
            if (type == 0)
            {
                date = GlobalUtilities.ConvertToDate(dttbl.Rows[i]["fpiinvestment_date"]);
            }
            else
            {
                date = GlobalUtilities.GetMonthName(Convert.ToInt32(dttbl.Rows[i]["m"]));
            }
            string equity = ExportExposurePortal.DecimalPoint(dttbl.Rows[i]["fpiinvestment_equity"], 2);
            string debt = ExportExposurePortal.DecimalPoint(dttbl.Rows[i]["fpiinvestment_debt"], 2);
            string debtvrr = ExportExposurePortal.DecimalPoint(dttbl.Rows[i]["fpiinvestment_debtvrr"], 2);
            string hybrid = ExportExposurePortal.DecimalPoint(dttbl.Rows[i]["fpiinvestment_hybrid"], 2);
            string debtfar = ExportExposurePortal.DecimalPoint(dttbl.Rows[i]["fpiinvestment_debtfar"], 2);
            string mutualfund = ExportExposurePortal.DecimalPoint(dttbl.Rows[i]["fpiinvestment_mutualfund"], 2);
            string aifs = ExportExposurePortal.DecimalPoint(dttbl.Rows[i]["fpiinvestment_aifs"], 2);
            string totaldebt = ExportExposurePortal.DecimalPoint(Convert.ToDouble(debt) + Convert.ToDouble(debtvrr) + Convert.ToDouble(debtfar) 
                                + Convert.ToDouble(hybrid), 2);
            string total = ExportExposurePortal.DecimalPoint(Convert.ToDouble(equity) + Convert.ToDouble(totaldebt) + Convert.ToDouble(mutualfund)
                            + Convert.ToDouble(aifs), 2);
            html.Append("<tr class='" + css + "'>");
            html.Append("<td>" + date + "</td>");
            html.Append("<td>" + equity + "</td>");
            html.Append("<td>" + debt + "</td>");
            html.Append("<td>" + debtvrr + "</td>");
            html.Append("<td>" + debtfar + "</td>");
            html.Append("<td>" + hybrid + "</td>");
            html.Append("<td>" + totaldebt + "</td>");
            html.Append("<td>" + mutualfund + "</td>");
            html.Append("<td>" + aifs + "</td>");
            html.Append("<td>" + total + "</td>");
            html.Append("</tr>");
        }
        html.Append("</table>");
        ltdata.Text = html.ToString();
    }
}
