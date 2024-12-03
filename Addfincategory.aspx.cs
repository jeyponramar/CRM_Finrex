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
public partial class Addfincategory : System.Web.UI.Page
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
        
        GlobalData objGlobalData = new GlobalData("tbl_findoccategory","findoccategoryid");
       
        objGlobalData.PopulateForm(form);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int clientId = Common.ClientId;
        int clientUserId = Common.ClientUserId;
        int id = Common.GetQueryStringValue("id");
        string query = "";
        query = "select * from tbl_findoccategory where findoccategory_clientid=" + Common.ClientId +
               "and findoccategory_categoryname=@categoryname";
        if (id > 0)
        {
            query += " and findoccategory_findoccategoryid<>" + id;
        }
        Hashtable hstblP = new Hashtable();
        hstblP.Add("categoryname", txtcategoryname.Text);
        DataRow dr = DbTable.ExecuteSelectRow(query, hstblP);
        if (dr != null)
        {
            lblmessage.Text = "Data already exists.";
            return;
        }
        Hashtable hstbl = new Hashtable();
        hstbl.Add("categoryname", txtcategoryname.Text);
        hstbl.Add("clientid", clientId);
        hstbl.Add("clientuserid", clientUserId);
        InsertUpdate obj = new InsertUpdate();

        if (id == 0)
        {
            int findoccategoryId = obj.InsertData(hstbl, "tbl_findoccategory");
            if (findoccategoryId > 0)
            {
                Response.Redirect("~/viewfincategory.aspx");
            }
        }
        else
        {
            int findoccategoryId = obj.UpdateData(hstbl, "tbl_findoccategory",id);
            if (findoccategoryId > 0)
            {
                Response.Redirect("~/viewfincategory.aspx");
            }
        }
       
    }
}
