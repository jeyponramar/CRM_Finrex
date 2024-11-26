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

public partial class AddFindoc : System.Web.UI.Page
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
        GlobalData objGlobalData = new GlobalData("tbl_findocdocument","findocdocumentid");
        string query = "";
        query = @"select * from tbl_findocdocument 
                  left join tbl_findocdepartment on findocdepartment_findocdepartmentid=findocdocument_findocdepartmentid
                  left join tbl_findoccategory On findoccategory_findoccategoryid=findocdocument_findoccategoryid
                  left join tbl_findocsubcategory on findocsubcategory_findocsubcategoryid=findocdocument_findocsubcategoryid
                  left join tbl_findocdocumenttype on findocdocumenttype_findocdocumenttypeid=findocdocument_findocdocumenttypeid
                  left join tbl_clientuser on clientuser_clientuserid=findocdocument_clientuserid
                where findocdocument_findocdocumentid=" + id +
              " and findocdocument_clientid=" + Common.ClientId;
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
       
        Hashtable hstbl = new Hashtable();
        hstbl.Add("findocdepartmentid", txtfindocdepartmentid.Text);
        hstbl.Add("findoccategoryid", txtfindoccategoryid.Text);
        hstbl.Add("findocsubcategoryid", txtfindocsubcategoryid.Text);
        hstbl.Add("findocdocumenttypeid", txtfindocdocumenttypeid.Text);
        hstbl.Add("clientid", clientId);
        hstbl.Add("clientuserid",clientUserId);
        hstbl.Add("subject", txtsubject.Text);
        hstbl.Add("remarks", txtremarks.Text);
        hstbl.Add("uploaddate", "getdate()");
        InsertUpdate obj = new InsertUpdate();
        if (id == 0)
        {
            int findocdocumentId = obj.InsertData(hstbl, "tbl_findocdocument");
            if (findocdocumentId > 0)
            {
                Response.Redirect("~/viewfindoc.aspx");
            }
        }
        else
        {
            int findocdocumentId = obj.UpdateData(hstbl, "tbl_findocdocument", id);
            if (findocdocumentId > 0)
            {
                Response.Redirect("~/viewfindoc.aspx");
            }
        }

    }
}
