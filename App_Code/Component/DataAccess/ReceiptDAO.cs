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
/// Summary description for ReceiptDAO
/// </summary>
public class ReceiptDAO
{
    private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    private static readonly DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
    public ReceiptDAO()
	{		
    }
    public DataRow Updateinvoicereceipt(double balanceamount, int invoiceid)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usb_Updateinvoicepaymentdetail";

                SqlParameter pinvoiceid = new SqlParameter("@invoiceid", SqlDbType.Int);
                pinvoiceid.Value = invoiceid;
                command.Parameters.Add(pinvoiceid);             

                SqlParameter pbalanceamount = new SqlParameter("@balanceamount", SqlDbType.Decimal);
                pbalanceamount.Value = balanceamount;
                command.Parameters.Add(pbalanceamount);

                using (DbDataAdapter adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (GlobalUtilities.IsValidaTable(dt))
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
    public DataRow GetInvoiceReceiptdetail(int invoiceid)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usb_Getinvoicedetail";

                SqlParameter pinvoiceid = new SqlParameter("@invoiceid", SqlDbType.Int);
                pinvoiceid.Value = invoiceid;
                command.Parameters.Add(pinvoiceid);

                using (DbDataAdapter adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (GlobalUtilities.IsValidaTable(dt))
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
    public DataTable GetInvoiceReceiptdetail(int invoiceid,int receiptid)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usb_Getinvoicedetail";

                SqlParameter pinvoiceid = new SqlParameter("@invoiceid", SqlDbType.Int);
                pinvoiceid.Value = invoiceid;
                command.Parameters.Add(pinvoiceid);

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
    public DataRow Getinvoicedetail_by_receipt(int receiptid)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_getsinvoicedetail_by_receipt";

                SqlParameter preceiptid = new SqlParameter("@receiptid", SqlDbType.Int);
                preceiptid.Value = receiptid;
                command.Parameters.Add(preceiptid);

                using (DbDataAdapter adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (GlobalUtilities.IsValidaTable(dt))
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
