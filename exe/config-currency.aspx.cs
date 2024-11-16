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
using System.Text;
using WebComponent;

public partial class exe_config_currency : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Finstation obj = new Finstation();
            Array arrcurrenytype = "1,2,3,4,5,6".Split(',');
            int clientUserId = FinstationExe.ClientUserId;
            StringBuilder html = new StringBuilder();
            string colconfig = obj.GetLiverateUserConfig_Columns(clientUserId, Enum_AppType.Finwatch);
            html.Append(colconfig);
            for (int i = 0; i < arrcurrenytype.Length; i++)
            {
                int currencyTypeId = Convert.ToInt32(arrcurrenytype.GetValue(i));
                string strhtml = obj.GetLiverateUserConfig(clientUserId, Enum_AppType.Finwatch, currencyTypeId);
                html.Append(strhtml);
            }
            ltcurrencies.Text = html.ToString();
        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        int clientUserId = FinstationExe.ClientUserId;
        Finstation obj = new Finstation();
        obj.SaveLiverateUserConfig(clientUserId, (int)Enum_AppType.Finwatch, hdncurrencies.Text, hdncolumns.Text);
        Response.Redirect("~/exe/default.aspx?sid=" + FinstationExe.SessionId);
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/exe/default.aspx?sid=" + FinstationExe.SessionId);
    }
}
