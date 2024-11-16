using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebComponent;
using System.Data;
using System.Data.SqlClient;

public partial class query : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //if (txtPassword.Text != "jeyapaul12") return;

        InsertUpdate obj = new InsertUpdate();
        try
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(txtQuery.Text, con);
            cmd.ExecuteNonQuery();

            lblError.Text = "Success";
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        InsertUpdate obj = new InsertUpdate();
        try
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            DataTable dttblSelectData = new DataTable();
            SqlDataAdapter daSelectData_SQL = null;
            daSelectData_SQL = new SqlDataAdapter(txtQuery.Text, con);
            daSelectData_SQL.SelectCommand.CommandType = CommandType.Text;
            daSelectData_SQL.Fill(dttblSelectData);

            gv.AutoGenerateColumns = true;
            gv.DataSource = dttblSelectData;
            gv.DataBind();
            lblError.Text = "Total : " + dttblSelectData.Rows.Count.ToString();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
    private string ConnectionString
    {
        get
        {
          return   System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }
    }
}
