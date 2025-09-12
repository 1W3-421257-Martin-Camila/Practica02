using Microsoft.Data.SqlClient;
using CommerceBack.Data.Helpers;
using CommerceBack.Data.Implementations;
using CommerceBack.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceBack.Services
{
    internal class UnitOfWork : IDisposable
    {
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;
        private bool _disposed;
       
        public UnitOfWork()
        {
            _connection = new SqlConnection(Properties.Resources.ConnectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public bool ExecuteSpDml(string sp, List<Parameter>? parameters = null)
        {
            try
            {
                using (var cmd = new SqlCommand(sp, _connection, _transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var p in parameters)
                        {
                            cmd.Parameters.AddWithValue(p.Name, p.Value);
                        }
                    }

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error en ExecuteSpDml (UoW): " + ex.Message);
                throw;
            }
        }

        public void Commit()
        {
            _transaction?.Commit();
            _connection?.Close();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _connection?.Close();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
                _disposed = true;
            }
        }
    }
}
