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
public partial class Addfinsubcategory : System.Web.UI.Page
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
        GlobalData objGlobalData = new GlobalData("tbl_findocsubcategory", "findocsubcategoryid");
        string query = "";
        query = @"select * from tbl_findocsubcategory 
                  join tbl_findoccategory on findoccategory_findoccategoryid=findocsubcategory_findoccategoryid
                where findocsubcategory_findocsubcategoryid=" + id +
               " and findocsubcategory_clientid=" + Common.ClientId;
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
        query = @"select * from tbl_findocsubcategory 
                where findocsubcategory_clientid=" + clientId +
               " and findocsubcategory_subcategoryname=@subcategoryname";
        if (id > 0)
        {
            query += " and findocsubcategory_findocsubcategoryid<>" + id;
        }
        Hashtable hstblP = new Hashtable();
        hstblP.Add("subcategoryname", txtsubcategoryname.Text);
        DataRow dr = DbTable.ExecuteSelectRow(query,hstblP);
        if (dr != null)
        {
            lblmessage.Text = "Data already exists.";
            return;
        }
        Hashtable hstbl = new Hashtable();
        hstbl.Add("subcategoryname", txtsubcategoryname.Text);
        hstbl.Add("findoccategoryid", txtfindoccategoryid.Text);
        hstbl.Add("clientid", clientId);
        hstbl.Add("clientuserid", clientUserId);
        InsertUpdate obj = new InsertUpdate();
        if (id == 0)
        {
            int findocsubcategoryId = obj.InsertData(hstbl, "tbl_findocsubcategory");
            if (findocsubcategoryId > 0)
            {
                Response.Redirect("~/viewfinsubcategory.aspx");
            }
        }
        else
        {
            int findocsubcategoryId = obj.UpdateData(hstbl, "tbl_findocsubcategory", id);
            if (findocsubcategoryId > 0)
            {
                Response.Redirect("~/viewfinsubcategory.aspx");
            }
        }
    }
}
