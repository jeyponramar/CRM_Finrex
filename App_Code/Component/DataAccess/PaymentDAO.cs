using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for PaymentDAO
/// </summary>
public class PaymentDAO
{
    private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    private static readonly DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
	public PaymentDAO()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public DataTable GetPurchaseOrderPaymentdetail(int PurchaseOrderId,int payment)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usb_GetPurchaseOrderPaymentDetail";

                SqlParameter pPurchaseOrderId = new SqlParameter("@Poid", SqlDbType.Int);
                pPurchaseOrderId.Value = PurchaseOrderId;
                command.Parameters.Add(pPurchaseOrderId);

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
    public DataRow GetPurchaseOrderPaymentdetail(int PurchaseOrderId)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usb_GetPurchaseOrderPaymentDetail";

                SqlParameter pPurchaseOrderId = new SqlParameter("@Poid", SqlDbType.Int);
                pPurchaseOrderId.Value = PurchaseOrderId;
                command.Parameters.Add(pPurchaseOrderId);

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
    public DataRow UpdatePurchaseOrderForPayment(double balanceamount, int purchaseOrderid)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usb_UpdatePurchaseOrderpaymentdetail";

                SqlParameter ppurchaseOrderid = new SqlParameter("@purchaseOrderid", SqlDbType.Int);
                ppurchaseOrderid.Value = purchaseOrderid;
                command.Parameters.Add(ppurchaseOrderid);

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
    public DataTable GetExpensePaymentdetail(int expensesId, int payment)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usb_GetExpensePaymentDetail";

                SqlParameter pexpensesId = new SqlParameter("@expensesid", SqlDbType.Int);
                pexpensesId.Value = expensesId;
                command.Parameters.Add(pexpensesId);

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
    public DataRow GetExpensePaymentdetail(int expensesId)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usb_GetExpensePaymentDetail";

                SqlParameter pexpensesId = new SqlParameter("@expensesid", SqlDbType.Int);
                pexpensesId.Value = expensesId;
                command.Parameters.Add(pexpensesId);

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
    public DataRow UpdateExpenseForPayment(double balanceamount, int expenseId)
    {
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usb_UpdateExpensepaymentdetail";

                SqlParameter pexpenseId = new SqlParameter("@Expensesid", SqlDbType.Int);
                pexpenseId.Value = expenseId;
                command.Parameters.Add(pexpenseId);

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
}
