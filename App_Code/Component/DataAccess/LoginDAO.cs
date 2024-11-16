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
/// Summary description for LoginDAO
/// </summary>
public class LoginDAO
{
    private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    private static readonly DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");

    public LoginDAO()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public DataRow ClientLoginDetails(string username,string password)
    {
        //int id = 0;
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_Login";

                SqlParameter Username = new SqlParameter("@username", SqlDbType.VarChar);
                Username.Value = username;
                command.Parameters.Add(Username);

                SqlParameter Password = new SqlParameter("@password", SqlDbType.VarChar);
                Password.Value = password;
                command.Parameters.Add(Password);


                //SqlParameter clientid = new SqlParameter("@id", SqlDbType.Int);
                //clientid.Direction = ParameterDirection.Output;
                //clientid.Value = 0;
                //command.Parameters.Add(clientid);


                using (DbDataAdapter adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (global.IsValidTable(dt))
                    {
                        return dt.Rows[0];
                        //return dt;
                    }
                    else
                    {
                        return null;
                    }
                //    id = GlobalUtilities.ConvertToInt(command.Parameters["@id"].Value);
                //    return id;

                }
            }
        }
    }
}
