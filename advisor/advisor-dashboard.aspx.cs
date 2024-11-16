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

public partial class advisor_dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindCharts();
        }
    }
    private void BindCharts()
    {
        string query = "";
        string extrawhere = "";
        int queryTypeId = Finstation.QueryTypeId;
        if (queryTypeId > 0)
        {
            extrawhere = " where querytopic_querytypeid=" + queryTypeId;
        }
        query = @"select querystatus_status as lbl,count(*) as val from tbl_customerquery
                  join tbl_querystatus on querystatus_querystatusid=customerquery_querystatusid
                  join tbl_querytopic on querytopic_querytopicid=customerquery_querytopicid
                  ";
        query += extrawhere;
        query += " group by querystatus_status";
        DataTable dttbl = DbTable.ExecuteSelect(query);

        StringBuilder html = new StringBuilder();
        StringBuilder lbls = new StringBuilder();
        StringBuilder vals = new StringBuilder();
        Finstation.GetChartData(dttbl, 0, ref lbls, ref vals);
        html.Append("<div class='db-chartjs-panel' ct='5'  xaxislabel='Count' data='" + vals + "' labels='" + lbls + "' pointradius='0'></div>");
        ltquerystatus.Text = html.ToString();

        query = @"select top 100 count(*) as val, substring(DATENAME(mm,customerquery_date),1,3)+' ' +cast(year(customerquery_date) as varchar) as lbl from tbl_customerquery
                    join tbl_querytopic on querytopic_querytopicid=customerquery_querytopicid
                    " + extrawhere + @"
                    group by year(customerquery_date),DATENAME(mm,customerquery_date)";

        dttbl = DbTable.ExecuteSelect(query);

        html = new StringBuilder();
        lbls = new StringBuilder();
        vals = new StringBuilder();
        Finstation.GetChartData(dttbl, 0, ref lbls, ref vals);
        html.Append("<div class='db-chartjs-panel' ct='3'  xaxislabel='Count' data='" + vals + "' labels='" + lbls + "' pointradius='5'></div>");
        ltmonthlyquery.Text = html.ToString();

        query = @"select querytopic_topicname as lbl,count(*) as val from tbl_customerquery
                  join tbl_querytopic on querytopic_querytopicid=customerquery_querytopicid
                   " + extrawhere + @"
                  group by querytopic_topicname";
        dttbl = DbTable.ExecuteSelect(query);

        html = new StringBuilder();
        lbls = new StringBuilder();
        vals = new StringBuilder();
        Finstation.GetChartData(dttbl, 0, ref lbls, ref vals);
        html.Append("<div class='db-chartjs-panel' ct='1' colors='red' xaxislabel='Count' data='" + vals + "' labels='" + lbls + "'></div>");
        ltquerybytopic.Text = html.ToString();

        string countwhere = "";
        if (extrawhere == "")
        {
            countwhere = " where 1=1";
        }
        else
        {
            countwhere = extrawhere;
        }
        SetCount(countwhere, lbltotalcount);
        SetCount(countwhere + " and customerquery_querystatusid in(1,6)", lblopencount);
        SetCount(countwhere + " and customerquery_querystatusid=5", lblclosedcount);
        SetCount(countwhere + " and customerquery_querystatusid in(1,6,3)", lblpendingcount);
        SetCount(countwhere + " and customerquery_querystatusid=3", lblpendingresponsecount);
    }
    private void SetCount(string extrawhere, Label lbl)
    {
        string query = "";
        query = @"select count(*) as c from tbl_customerquery
                join tbl_querytopic on querytopic_querytopicid=customerquery_querytopicid";
        query += extrawhere;
        DataRow dr = DbTable.ExecuteSelectRow(query);
        lbl.Text = Convert.ToString(dr["c"]);
    }
}
