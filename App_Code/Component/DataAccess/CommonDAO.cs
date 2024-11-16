using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;
using WebComponent;

public class CommonDAO
{
    private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    private static readonly DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
    public int RecordCount = 0;
    
    public CommonDAO()
    {
    }
    public DataTable GetAllCounts(int LastNotificationId)
    {
        int loggedinUserId = GlobalUtilities.ConvertToInt(CustomSession.Session("Login_UserId"));
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_GetAllCounts";

                SqlParameter pUserId = new SqlParameter("@UserId", SqlDbType.Int);
                pUserId.Value = loggedinUserId;
                command.Parameters.Add(pUserId);

                SqlParameter pLastNotificationId = new SqlParameter("@LastNotificationId", SqlDbType.Int);
                pLastNotificationId.Value = LastNotificationId;
                command.Parameters.Add(pLastNotificationId);

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
    public DataTable GetClientAmcInfo(int ClientId,int AmcId)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                SqlParameter pClientId = new SqlParameter("@ClientId", SqlDbType.Int);
                pClientId.Value = ClientId;
                command.Parameters.Add(pClientId);

                SqlParameter pAmcId = new SqlParameter("@AmcId", SqlDbType.Int);
                pAmcId.Value = AmcId;
                command.Parameters.Add(pAmcId);

                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_GetClientAmcInfo";

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
    public DataTable GetAllCounts()
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_GetAllCounts";

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
    public void SetApprovalNotification(string ModuleName, string message, int ActionId, int ModuleId, int FromUserId)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_SetApprovalNotification";

                SqlParameter pModuleName = new SqlParameter("@ModuleName", SqlDbType.VarChar);
                pModuleName.Value = ModuleName;
                command.Parameters.Add(pModuleName);

                SqlParameter pMessage = new SqlParameter("@Message", SqlDbType.VarChar);
                pMessage.Value = message;
                command.Parameters.Add(pMessage);

                SqlParameter pModuleId  = new SqlParameter("@ModuleId", SqlDbType.Int);
                pModuleId.Value = ModuleId;
                command.Parameters.Add(pModuleId);

                SqlParameter pActionId = new SqlParameter("@ActionId", SqlDbType.Int);
                pActionId.Value = ActionId;
                command.Parameters.Add(pActionId);

                SqlParameter pFromUserId = new SqlParameter("@FromUserId", SqlDbType.Int);
                pFromUserId.Value = FromUserId;
                command.Parameters.Add(pFromUserId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
    
    
}
