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
using System.Text;

public partial class map : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindMap();
        }
    }
    private void BindMap()
    {
        string query = "select * from tbl_branch";
        DataTable dttbl = new DataTable();
        InsertUpdate obj = new InsertUpdate();
        dttbl = obj.ExecuteSelect(query);
        double dblMinLatitude = 9999;
        double dblMaxLatitude = -9999;
        double dblMinLongitude = 9999;
        double dblMaxLongitude = -9999;
        double dblTotalLatitude = 0;
        double dblTotalLongitude = 0;

        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            dblTotalLatitude += Convert.ToDouble(dttbl.Rows[i]["branch_latitude"]);
            dblTotalLongitude += Convert.ToDouble(dttbl.Rows[i]["branch_longitude"]);
        }
        double dblAvgLatitude = dblTotalLatitude / dttbl.Rows.Count;
        double dblAvgLongitude = dblTotalLongitude / dttbl.Rows.Count;
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            dttbl.Rows[i]["branch_latitude"] = dblAvgLatitude - Convert.ToDouble(dttbl.Rows[i]["branch_latitude"]);
            dttbl.Rows[i]["branch_longitude"] = dblAvgLongitude - Convert.ToDouble(dttbl.Rows[i]["branch_longitude"]);
        }

        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            double dblLatitude_temp = Convert.ToDouble(dttbl.Rows[i]["branch_latitude"]);
            double dblLongitude_temp = Convert.ToDouble(dttbl.Rows[i]["branch_longitude"]);
            //dblTotalLatitude += dblLatitude_temp;
            //dblTotalLongitude += dblLongitude_temp;
            if (dblLatitude_temp < dblMinLatitude)
            {
                dblMinLatitude = dblLatitude_temp;
            }
            if (dblLatitude_temp > dblMaxLatitude)
            {
                dblMaxLatitude = dblLatitude_temp;
            }
            if (dblLongitude_temp < dblMinLongitude)
            {
                dblMinLongitude = dblLongitude_temp;
            }
            if (dblLongitude_temp > dblMaxLongitude)
            {
                dblMaxLongitude = dblLongitude_temp;
            }
        }
        
        int maxMapAreaX = 450;
        int maxMapAreaY = 450;
        StringBuilder script =new StringBuilder();
        script.Append("<script>" + Environment.NewLine);
        double dblZoomFactor1 = maxMapAreaX / dblMaxLatitude;
        double dblZoomFactor2 = maxMapAreaY / dblMaxLongitude;
        double dblZoomFactor = dblZoomFactor2;
        if (dblZoomFactor1 < dblZoomFactor2)
        {
            dblZoomFactor = dblZoomFactor1;
        }
        for (int i = 0; i < dttbl.Rows.Count; i++)
        {
            double dblLatitude = Convert.ToDouble(dttbl.Rows[i]["branch_latitude"]);
            double dblLongitude = Convert.ToDouble(dttbl.Rows[i]["branch_longitude"]);
            string branchName = Convert.ToString(dttbl.Rows[i]["branch_branchname"]);
            int y = Convert.ToInt32(dblLatitude * dblZoomFactor) + 450;
            int x = 450 - Convert.ToInt32(dblLongitude * dblZoomFactor);
            script.Append("$(document).ready(function(){"+Environment.NewLine);
            script.Append("drawMarker("+x+","+y+");" + Environment.NewLine);
            script.Append("displayText(" + x + "," + y + ",\""+branchName+"\");");
            script.Append("});" + Environment.NewLine);
        }
        script.Append("</script>" + Environment.NewLine);
        ClientScript.RegisterClientScriptBlock(typeof(Page), "script", script.ToString());
    }
}
