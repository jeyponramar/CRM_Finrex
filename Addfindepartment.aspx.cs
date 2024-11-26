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
public partial class Addfindepartment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateData();
        }
    }
    private void PopulateData()
    {
        int id = Common.GetQueryStringValue("id");
        if (id == 0) return;
        GlobalData objGlobalData = new GlobalData("tbl_findocdepartment", "findocdepartmentid");
        string query = "";
        query = "select * from tbl_findocdepartment where findocdepartment_findocdepartmentid=" + id +
                " and findocdepartment_clientid=" + Common.ClientId;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        if (dr == null)
        {
            Response.End();
        }
        objGlobalData.PopulateForm(dr, form);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int clientId = Common.ClientId;
        int clientUserId = Common.ClientUserId;
        int id = Common.GetQueryStringValue("id");
        string query = "";
        query = "select * from tbl_findocdepartment where findocdepartment_clientid=" + Common.ClientId + 
                "and findocdepartment_departmentname=@departmentname";
        if (id > 0)
        {
            query += " and findocdepartment_findocdepartmentid<>" + id;
        }
        Hashtable hstblp = new Hashtable();
        hstblp.Add("departmentname", txtdepartmentname.Text);
        DataRow dr = DbTable.ExecuteSelectRow(query, hstblp);
        if (dr != null)
        {
            lblmessage.Text = "Data already exists.";
            return;
        }
        Hashtable hstbl = new Hashtable();
        hstbl.Add("departmentname", txtdepartmentname.Text);
        hstbl.Add("clientid", clientId);
        hstbl.Add("clientuserid", clientUserId);
        InsertUpdate obj = new InsertUpdate();
        
        if (id == 0)
        {
            int findocdepartmentId = obj.InsertData(hstbl, "tbl_findocdepartment");
            if (findocdepartmentId > 0)
            {
                Response.Redirect("~/viewfindepartment.aspx");
            }
        }
        else
        {
            int findocdepartmentId = obj.UpdateData(hstbl, "tbl_findocdepartment", id);
            if (findocdepartmentId > 0)
            {
                Response.Redirect("~/viewfindepartment.aspx");
            }
        }
    }

}

