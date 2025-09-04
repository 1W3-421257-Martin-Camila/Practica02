using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Practica01.Domain;

namespace Practica01.Data.Helpers
{
    public class DataHelper
    {
        private static DataHelper? _instance;
        private SqlConnection _connection;
        SqlTransaction _transaction; //Esto es para el UnitOfWork

        private DataHelper() //Patrón Singleton --> Constructor privado
        {
            _connection = new SqlConnection(Properties.Resources.ConnectionString);
        }

        public static DataHelper GetInstance()//Patrón Singleton --> Método de creación estático
        {
            if (_instance == null)
            {
                _instance = new DataHelper();
            }
            return _instance;
        }

        public DataTable ExecuteSPQuery(string sp, List<Parameter>? parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                _connection.Open();
                var cmd = new SqlCommand(sp, _connection); //"sp" --> nombre del Procedimiento Almacenado
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp;

                // Agregamos parámetros si los hay
                if (parameters != null)
                {
                    foreach (Parameter param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Name, param.Value);
                    }
                }

                dt.Load(cmd.ExecuteReader()); //Carga en un DataTable los resultados que lee del SP
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error en ExecuteSPQuery: " + ex.Message);
                dt = null;

            }
            finally
            {
                _connection.Close();//En cualquier caso cierra la conexión
            }

            return dt;
        }


        public bool ExecuteSpDML(string sp, List<Parameter>? parameters = null)
        {
            bool result;
            try
            {
                // Abrimos la conexión
                _connection.Open();
                var cmd = new SqlCommand(sp, _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Agregamos parámetros si los hay
                if (parameters != null)
                {
                    foreach (Parameter p in parameters)
                    {
                        cmd.Parameters.AddWithValue(p.Name, p.Value);
                    }
                }

                int affectedRows = cmd.ExecuteNonQuery();

                result = affectedRows > 0;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error en ExecuteSpDML: " + ex.Message);
                result = false;
            }
            finally
            {
                // Cerramos la conexión
                _connection.Close();
            }

            return result;

        }

        public bool ExecuteTransaction(Invoice invoice)
        {
            _connection.Open();

            // Obtener transaccion a partir de conexion
            SqlTransaction transaction = _connection.BeginTransaction();
            var cmd = new SqlCommand("SELECT ", _connection, transaction);

            // Ejecutar los comandos que hagan falta
            // Primero tenemos que crear el maestro INVOICES
            cmd.CommandText = "SP_SAVE_INVOICE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@InvoiceDate", invoice.Date);
            cmd.Parameters.AddWithValue("@Customer", invoice.Customer);
            cmd.Parameters.AddWithValue("@PaymentMethodId", invoice.PaymentMethod.Id);

            int affectedRows = cmd.ExecuteNonQuery();
            if (affectedRows <= 0)
            {
                transaction.Rollback();
                return false;
            }
            else
            {
                // Luego tenemos que crear el INVOICE DETAILS
                foreach (InvoiceDetail i in invoice.Details)
                {
                    SqlCommand cmdDetails = new SqlCommand("", _connection, transaction);
                    cmdDetails.CommandText = "SP_SAVE_INVOICE_DETAIL";
                    cmdDetails.CommandType = CommandType.StoredProcedure;

                    int invoiceNumber = 1;
                    cmdDetails.Parameters.AddWithValue("@InvoiceNumber", invoiceNumber);
                    cmdDetails.Parameters.AddWithValue("@ArticleId", i.Article.Id);
                    cmdDetails.Parameters.AddWithValue("@Quantity", i.Quantity);

                    int affectedRowsDetail = cmdDetails.ExecuteNonQuery();
                    if (affectedRowsDetail <= 0)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }

                transaction.Commit();
                return true;
            }
        }
    }
}
