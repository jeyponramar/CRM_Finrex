using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;
using WebComponent;

public class UtilitiesDAO
{
    private static readonly string connectionString = AppConstants.ConnectionString;
    private static readonly DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
    public int RecordCount = 0;

    public UtilitiesDAO()
    {
    }

    public void UpdateTenderDocSequence(int TenderId, int TemplateId, int TargetTemplateId)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_UpdateTenderDocSequence";

                SqlParameter pTenderId = new SqlParameter("@TenderId", SqlDbType.Int);
                pTenderId.Value = TenderId;
                command.Parameters.Add(pTenderId);

                SqlParameter pTemplateId = new SqlParameter("@TemplateId", SqlDbType.Int);
                pTemplateId.Value = TemplateId;
                command.Parameters.Add(pTemplateId);

                SqlParameter pTargetTemplateId = new SqlParameter("@TargetTemplateId", SqlDbType.Int);
                pTargetTemplateId.Value = TargetTemplateId;
                command.Parameters.Add(pTargetTemplateId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
    public void UpdateEnquiryStatus(int EnqId, int StatusId)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_UpdateEnquiryStatus";

                SqlParameter pEnqId = new SqlParameter("@EnqId", SqlDbType.Int);
                pEnqId.Value = EnqId;
                command.Parameters.Add(pEnqId);

                SqlParameter pStatusId = new SqlParameter("@StatusId", SqlDbType.Int);
                pStatusId.Value = StatusId;
                command.Parameters.Add(pStatusId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
    public void UpdateTenderStatus(int TenderId, int StatusId)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_UpdateTenderStatus";

                SqlParameter pTenderId = new SqlParameter("@TenderId", SqlDbType.Int);
                pTenderId.Value = TenderId;
                command.Parameters.Add(pTenderId);

                SqlParameter pStatusId = new SqlParameter("@StatusId", SqlDbType.Int);
                pStatusId.Value = StatusId;
                command.Parameters.Add(pStatusId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
    public void AddDataVeification(string modulename, int Moduleid)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_AddDataverification";

                SqlParameter pmodulename = new SqlParameter("@modulename", SqlDbType.VarChar);
                pmodulename.Value = modulename;
                command.Parameters.Add(pmodulename);

                SqlParameter pModuleid = new SqlParameter("@Moduleid", SqlDbType.Int);
                pModuleid.Value = Moduleid;
                command.Parameters.Add(pModuleid);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
    //public DataTable GetMenuForDDL()
    //{
    //    using (DbConnection connection = factory.CreateConnection())
    //    {
    //        connection.ConnectionString = connectionString;

    //        using (DbCommand command = factory.CreateCommand())
    //        {
    //            command.Connection = connection;
    //            command.CommandType = CommandType.StoredProcedure;
    //            command.CommandText = "usp_GetInvoiceProductDetail";

    //            SqlParameter pIId = new SqlParameter("@IId", SqlDbType.Int);
    //            pIId.Value = IId;
    //            command.Parameters.Add(pIId);

    //            using (DbDataAdapter adapter = factory.CreateDataAdapter())
    //            {
    //                adapter.SelectCommand = command;

    //                DataTable dt = new DataTable();
    //                adapter.Fill(dt);

    //                if (global.IsValidTable(dt))
    //                {
    //                    //return dt.Rows[0];
    //                    return dt;
    //                }
    //                else
    //                {
    //                    return null;
    //                }
    //            }
    //        }
    //    }
    //}

}

