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

public partial class client_apiconfig : System.Web.UI.Page
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
        GlobalData objGlobalData = new GlobalData("tbl_client", "clientid");
        string query = "";
        query = "select * from tbl_client where client_clientid=" + id;                         
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
        int id = Common.GetQueryStringValue("id");
        string query = "";
        query = "select * from tbl_client where client_clientid=" + Common.ClientId + 
                "and client_apiusername=@apiusername";
        if (id > 0)
        {
            query += " and client_clientid<>" + id;
        }
        Hashtable hstblP = new Hashtable();
        hstblP.Add("apiusername", txtapiusername.Text);
        DataRow dr = DbTable.ExecuteSelectRow(query, hstblP);
        if (dr != null)
        {
            lblmessage.Text = "Data already exists";
        }
        Hashtable hstbl = new Hashtable();
        hstbl.Add("apiusername", txtapiusername.Text);
        hstbl.Add("maxapicallsperday", txtmaxapicallsperday.Text);
        InsertUpdate obj = new InsertUpdate();
        if (id > 0)
        {
            id = obj.UpdateData(hstbl, "tbl_client", id);
            lblmessage.Text = "Data update successfully";
        }

    }
}
