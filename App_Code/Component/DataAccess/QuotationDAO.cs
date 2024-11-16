using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;
using WebComponent;
public class QuotationDAO
{
    private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    private static readonly DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
    public int RecordCount = 0;

	public QuotationDAO()
	{
	}
    public DataRow GetQuotationDetail(int QId)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_GetQuotationDetail";

                SqlParameter pQId = new SqlParameter("@QId", SqlDbType.Int);
                pQId.Value = QId;
                command.Parameters.Add(pQId);

                using (DbDataAdapter adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (global.IsValidTable(dt))
                    {
                        return dt.Rows[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
    public DataTable GetQuotationProductDetail(int QId)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_GetQuotationProductDetail";

                SqlParameter pQId = new SqlParameter("@QId", SqlDbType.Int);
                pQId.Value = QId;
                command.Parameters.Add(pQId);

                using (DbDataAdapter adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (global.IsValidTable(dt))
                    {
                        //return dt.Rows[0];
                        return dt;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
