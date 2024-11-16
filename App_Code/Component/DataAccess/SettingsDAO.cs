using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;
using WebComponent;

public class SettingsDAO
{
    private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    private static readonly DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
    public int RecordCount = 0;
    
    public SettingsDAO()
    {
    }
    public DataTable GetAllSettingss()
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_GetAllSettingss";

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
    public DataTable GetAllSettingss(int PageNo, int SortBy)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_GetAllSettingss_Paging";

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
    public DataTable SearchSettings(int PageNo, int SortBy, string Keyword)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_SearchSettings_Paging";

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
    public DataRow GetSettingsDetail(int SettingsId)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_GetSettingsDetail";

                SqlParameter pSettingsId = new SqlParameter("SettingsId", SqlDbType.Int);
                pSettingsId.Value = SettingsId;
                command.Parameters.Add(pSettingsId);

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
}
