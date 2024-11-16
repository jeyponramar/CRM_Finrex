using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;
using WebComponent;

public class UserDAO
{
    private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    private static readonly DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
    public int RecordCount = 0;

    public UserDAO()
    {
    }
    public DataTable CheckUserLogin(string EmailId, string Password)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_CheckUser";

                SqlParameter pEmailId = new SqlParameter("@EmailId", SqlDbType.VarChar);
                pEmailId.Value = EmailId;
                command.Parameters.Add(pEmailId);
                SqlParameter pPassword = new SqlParameter("@Password", SqlDbType.VarChar);
                pPassword.Value = Password;
                command.Parameters.Add(pPassword);

                using (DbDataAdapter adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
    public DataTable RegisterUser(string FirstName, string LastName, string EmailId, string Password)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_RegisterUser";

                SqlParameter pFirstName = new SqlParameter("@FirstName", SqlDbType.VarChar);
                pFirstName.Value = FirstName;
                command.Parameters.Add(pFirstName);
                SqlParameter pLastName = new SqlParameter("@LastName", SqlDbType.VarChar);
                pLastName.Value = LastName;
                command.Parameters.Add(pLastName);

                SqlParameter pEmailId = new SqlParameter("@EmailId", SqlDbType.VarChar);
                pEmailId.Value = EmailId;
                command.Parameters.Add(pEmailId);
                SqlParameter pPassword = new SqlParameter("@Password", SqlDbType.VarChar);
                pPassword.Value = Password;
                command.Parameters.Add(pPassword);

                using (DbDataAdapter adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
    public DataTable GetAllUsers()
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_GetAllUsers";

                using (DbDataAdapter adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    RecordCount = dt.Rows.Count;
                    return dt;
                }
            }
        }
    }
    public DataTable GetAllUsers(int PageNo, int SortBy)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_GetAllUsers_Paging";

                SqlParameter pPageNo = new SqlParameter("@PageNo", SqlDbType.Int);
                pPageNo.Value = PageNo;
                command.Parameters.Add(pPageNo);
                SqlParameter pSortBy = new SqlParameter("@SortBy", SqlDbType.Int);
                pSortBy.Value = SortBy;
                command.Parameters.Add(pSortBy);

                SqlParameter pRecordCount = new SqlParameter("@RecordCount", SqlDbType.Int);
                pRecordCount.Direction = ParameterDirection.Output;
                pRecordCount.Value = 0;
                command.Parameters.Add(pRecordCount);

                using (DbDataAdapter adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    RecordCount = Convert.ToInt32(command.Parameters["@RecordCount"].Value);
                    return dt;
                }
            }
        }
    }
    public DataTable SearchUser(int PageNo, int SortBy, string Keyword)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_SearchUser_Paging";

                SqlParameter pKeyword = new SqlParameter("@Keyword", SqlDbType.VarChar);
                pKeyword.Value = Keyword;
                command.Parameters.Add(pKeyword);
                SqlParameter pPageNo = new SqlParameter("@PageNo", SqlDbType.Int);
                pPageNo.Value = PageNo;
                command.Parameters.Add(pPageNo);
                SqlParameter pSortBy = new SqlParameter("@SortBy", SqlDbType.Int);
                pSortBy.Value = SortBy;
                command.Parameters.Add(pSortBy);

                SqlParameter pRecordCount = new SqlParameter("@RecordCount", SqlDbType.Int);
                pRecordCount.Direction = ParameterDirection.Output;
                pRecordCount.Value = 0;
                command.Parameters.Add(pRecordCount);

                using (DbDataAdapter adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    RecordCount = Convert.ToInt32(command.Parameters["@RecordCount"].Value);
                    return dt;
                }
            }
        }
    }
    public DataRow GetUserDetail(int UserId)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_GetUserDetail";

                SqlParameter pUserId = new SqlParameter("UserId", SqlDbType.Int);
                pUserId.Value = UserId;
                command.Parameters.Add(pUserId);

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
    public DataTable AdminUserLogin(string EmailId, string Password)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_CheckAdminUser";

                SqlParameter pEmailId = new SqlParameter("@EmailId", SqlDbType.VarChar);
                pEmailId.Value = EmailId;
                command.Parameters.Add(pEmailId);

                SqlParameter pPassword = new SqlParameter("@Password", SqlDbType.VarChar);
                pPassword.Value = Password;
                command.Parameters.Add(pPassword);

                using (DbDataAdapter adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    RecordCount = dt.Rows.Count;
                    return dt;
                }
            }
        }
    }
    public DataTable GetForgotPassword(string EmailId)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_GetForgotPassword";

                SqlParameter pEmailId = new SqlParameter("@EmailId", SqlDbType.VarChar);
                pEmailId.Value = EmailId;
                command.Parameters.Add(pEmailId);

                using (DbDataAdapter adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    RecordCount = dt.Rows.Count;
                    return dt;
                }
            }
        }
    }
    public int ChangePassword(string NewPassword, int UserId)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;
            connection.Open();

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_ChangePassword";

                SqlParameter pNewPassword = new SqlParameter("@NewPassword", SqlDbType.VarChar);
                pNewPassword.Value = NewPassword;
                command.Parameters.Add(pNewPassword);

                SqlParameter pUserId = new SqlParameter("@UserId", SqlDbType.VarChar);
                pUserId.Value = UserId;
                command.Parameters.Add(pUserId);

                int result = 0;
                result = command.ExecuteNonQuery();
                return result;

            }
        }
    }
}
