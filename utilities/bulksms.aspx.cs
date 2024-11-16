using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using WebComponent;

public partial class utilities_bulksms : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDetail();
        }
    }
    private void BindDetail()
    {
        
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        Hashtable hstbl = new Hashtable();
        InsertUpdate obj = new InsertUpdate();
        int id = obj.InsertData(hstbl, "tbl_bulksms");
        if (id == 0)
        {
            lblMessage.Text = "Unable to save bulk sms detail!";
            lblMessage.Visible = true;
            return;
        }
        txtbulksmsid.Text = id.ToString();

    }
    protected void btnReSend_Click(object sender, EventArgs e)
    {
        int id = Common.GetQueryStringValue("id");
        if (id == 0)
        {
            id = GlobalUtilities.ConvertToInt(ViewState["id"]);
        }
        if (id == 0) return;
        string query = "update tbl_bulksmsdetail set bulksmsdetail_isfailed=0 WHERE bulksmsdetail_isfailed=1 AND bulksmsdetail_bulksmsid=" + id;
        DbTable.ExecuteQuery(query);


    }
}
