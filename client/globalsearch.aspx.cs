using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class client_globalsearch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }
        ClientScript.RegisterClientScriptBlock(typeof(Page), "title", "<script>parent.setTitle('Global Search')</script>");
    }
    protected void btnTrack_Click(object sender, EventArgs e)
    {
        //string client = global.CheckInputData(txtclient.Text);
        //if (client.Length < 3)
        //{
        //    lblMessage.Text = " Please enter at least 3 letters";
        //    lblMessage.Visible = true;
        //    return;
        //}
        lblMessage.Visible = false;
        string query = @"select *,
                        case when isnull(client_subscriptionstatusid,0)=0 then 'Enquiry'
                                else subscriptionstatus_status end as status,
                        case when isnull(client_subscriptionstatusid,0)=0 then 
                                            (select top 1 employee_employeename from tbl_enquiry
                                            join tbl_employee on employee_employeeid=enquiry_employeeid
                                            where enquiry_companyname=client_customername) else employee_employeename end as employeename
                        from tbl_client 
                        LEFT JOIN tbl_clientgroup ON clientgroup_clientgroupid=client_clientgroupid
                        LEFT JOIN tbl_employee ON employee_employeeid=client_employeeid
                        LEFT JOIN tbl_subscriptionstatus ON subscriptionstatus_subscriptionstatusid=client_subscriptionstatusid 
                        LEFT JOIN tbl_trial on trial_clientid=client_clientid
                        LEFT JOIN tbl_contacts on contacts_clientid=client_clientid
                        LEFT JOIN tbl_designation ON designation_designationid=client_designationid
                        ";
        DataTable dttbl = new DataTable();
        Hashtable hstbl = new Hashtable();
        string where = "";
        if (txtclient.Text != "")
        {
            hstbl.Add("client", "%" + txtclient.Text + "%");
            where += " OR client_customername like @client";
        }
        if (txtcontactperson.Text != "")
        {
            hstbl.Add("contactperson", "%" + txtcontactperson.Text + "%");
            where += " OR client_contactperson like @contactperson OR contacts_contactperson like @contactperson";
        }
        if (txtmobileno.Text != "")
        {
            hstbl.Add("mobileno", "%" + txtmobileno.Text + "%");
            where += " OR client_mobileno like @mobileno OR contacts_mobileno like @mobileno";
        }
        if (txtemailid.Text != "")
        {
            hstbl.Add("emailid", "%" + txtemailid.Text + "%");
            where += " OR client_emailid like @emailid OR contacts_emailid like @emailid";
        }
        if (where != "")
        {
            query += " where " + where.Substring(4);
        }
        dttbl = DbTable.ExecuteSelect(query, hstbl);
        ltcustomerdetail.Text = Common.BindGrid(dttbl,
            "client_customercode,client_customername,contacts_contactperson,designation_designation,contacts_mobileno,client_landlineno,contacts_emailid,clientgroup_groupname,employeename,status,trial_remarks", 
            "Customer Code,Customer Name,Contact Person,Designation,Mobile No,Landline No,Email Id,Client Group,Assign To,Subscription Status,Remarks");



    }
}
