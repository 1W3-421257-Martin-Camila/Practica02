using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Practica01.Data;

namespace Practica01.Datos
{
    public class DataHelper
    {
        private static DataHelper _instance;
        private SqlConnection _connection;

        private DataHelper() //Patrón Singleton --> Constructor privado
        {
            _connection = new SqlConnection(Properties.Resources.ConnectionString);
        }

        public static DataHelper GetInstance()//Patrón Singleton --> Método de creación estático
        {
            if(_instance == null)
            {
                _instance = new DataHelper();
            }
            return _instance;
        }

        public DataTable ExecuteSPQuery(string sp, List<Parameter> parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                _connection.Open();
                var cmd = new SqlCommand(sp, _connection); //"sp" --> nombre del Procedimiento Almacenado
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp;

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
                dt = null;
            }
            finally
            {
                _connection.Close();//En cualquier caso cierra la conexión
            }

            return dt;
        }


        public int ExecuteSPDML(string sp, List<Parameter> parameters = null)
        {
            int rowsAffected = 0;
            try
            {
                _connection.Open();
                var cmd = new SqlCommand(sp, _connection);
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (Parameter param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Name, param.Value);
                    }
                }

                rowsAffected = cmd.ExecuteNonQuery(); // ejecuta INSERT/UPDATE/DELETE
            }
            catch (SqlException ex)
            {
                rowsAffected = -1;
            }
            finally
            {
                _connection.Close();
            }

            return rowsAffected;
        }

    }
}
