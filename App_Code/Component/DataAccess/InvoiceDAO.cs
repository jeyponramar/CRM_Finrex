using System;
using System.Data;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;
using WebComponent;

/// <summary>
/// Summary description for InvoiceDAO
/// </summary>
public class InvoiceDAO
{
    private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    private static readonly DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
    public InvoiceDAO()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public DataRow GetInvoiceProductDetail(int IId)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_GetInvoiceProductDetail";

                SqlParameter pIId = new SqlParameter("@IId", SqlDbType.Int);
                pIId.Value = IId;
                command.Parameters.Add(pIId);

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
    public DataTable GetInvoiceProductDetailDetail(int IId)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_GetInvoiceProductDetailDetail";

                SqlParameter pIId = new SqlParameter("@IId", SqlDbType.Int);
                pIId.Value = IId;
                command.Parameters.Add(pIId);

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
