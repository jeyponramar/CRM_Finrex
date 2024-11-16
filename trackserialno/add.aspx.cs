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

public partial class trackserialno_add : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('Track Serial Number')</script>");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindSerialNo();
        BindAmcWarrantyDetail();
    }
    private void BindSerialNo()
    {
        string query = "select * from tbl_serialno "+
                       "LEFT JOIN tbl_product ON product_productid=serialno_productid "+
                       "LEFT JOIN tbl_employee ON employee_employeeid=serialno_employeeid "+
                       "WHERE serialno_serialno ='" + SerialNo.Text + "' ORDER BY serialno_date";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        ltSerialNo.Text = Common.BindGrid(dttbl, "serialno_serialno,serialno_date~date,product_productname,serialno_clientname,employee_employeename,serialno_module,serialno_inout,serialno_referenceno,serialno_remarks",
                               "Serial No,Date,Product,Client Name,Employee,Module,In/Out,Reference No,Remarks");
    }
    private void BindAmcWarrantyDetail()
    {
        string query = "SELECT *,CASE WHEN iswarranty = 1 THEN '<span class=\"btngreen\">Under '+amcstatus +'</span>' ELSE '<span class=\"btnred\">'+amcstatus+' Expired</span>' END AS warrantystatus FROM " +
                       "(select *,CASE WHEN CAST(GETDATE() AS DATE) BETWEEN CAST(amc_startdate AS DATE) AND CAST(amc_enddate AS DATE) THEN 1 ELSE 0 END iswarranty,"+
                       "CASE WHEN ISNULL(amc_iswarranty,0)=0 THEN 'AMC Warranty' ELSE 'Sales Warranty' END AS amcstatus " +
                       "from tbl_amc " +
                       "JOIN tbl_client ON client_clientid=amc_clientid " +
                       "LEFT JOIN tbl_amctype ON amctype_amctypeid=amc_amctypeid " +
                       "JOIN tbl_amcstatus ON amcstatus_amcstatusid=amc_amcstatusid " +
                       "JOIN tbl_amcdetail ON amcdetail_amcid=amc_amcid " +
                       "JOIN tbl_product ON product_productid=amcdetail_productid " +
                       "WHERE amcdetail_serialno = '" + SerialNo.Text + "')r";
        DataTable dttbl = DbTable.ExecuteSelect(query);
        
        ltWarrantyDetail.Text = Common.BindGrid(dttbl, "client_customername,amc_amccode,amc_startdate~date,amc_enddate~date,amctype_amctype,amcdetail_serialno,product_productname,amcstatus_status,warrantystatus",
                            "Client Name,AMC Code,Start Date,End Date,AMC Type,Serial No,Product,AMC Status,Warranty Status");

    }
}
