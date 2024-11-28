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
public partial class Addfindocumenttype : System.Web.UI.Page
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
        GlobalData objGlobalData = new GlobalData("tbl_findocdocumenttype", "findocdocumenttypeid");
        string query = "";
        query = "select * from tbl_findocdocumenttype where findocdocumenttype_findocdocumenttypeid=" + id +
               "and findocdocumenttype_clientid=" + Common.ClientId;
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
        query="select * from tbl_findocdocumenttype where findocdocumenttype_clientid=" + Common.ClientId +
                "and findocdocumenttype_documenttype=@documenttype";
        if (id > 0)
        {
            query += " and findocdocumenttype_findocdocumenttypeid<>" + id;
        }
        Hashtable hstblP = new Hashtable();
        hstblP.Add("documenttype", txtdocumenttype.Text);
        DataRow dr = DbTable.ExecuteSelectRow(query, hstblP);
        if (dr != null)
        {
            lblmessage.Text = "Data already exists.";
            return;
        }
        Hashtable hstbl = new Hashtable();
        hstbl.Add("documenttype", txtdocumenttype.Text);
        hstbl.Add("clientid", clientId);
        hstbl.Add("clientuserid", clientUserId);
        InsertUpdate obj = new InsertUpdate();
        if (id == 0)
        {
            int findocdocumenttypeId = obj.InsertData(hstbl, "tbl_findocdocumenttype");
            if (findocdocumenttypeId > 0)
            {
                Response.Redirect("~/viewfindocumenttype.aspx");
            }
        }
        else
        {
            int findocdocumenttypeId = obj.UpdateData(hstbl, "tbl_findocdocumenttype",id);
            if (findocdocumenttypeId > 0)
            {
                Response.Redirect("~/viewfindocumenttype.aspx");
            }
        }
       
    }
}
