using Practica01.Datos;
using Practica01.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Practica01.Data
{
    public class InvoiceRepository : IInvoiceRepository
    {
        public bool Delete(int id)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
            new Parameter() { Name = "@Number", Value = id }
            };

            int rows = DataHelper.GetInstance().ExecuteSPDML("SP_SOFTDELETE_INVOICE", parameters);

            return rows > 0;
        }

        public List<Invoice> GetAll()
        {
            List<Invoice> list = new List<Invoice>();

            var dt = DataHelper.GetInstance().ExecuteSPQuery("SP_GET_ALL_INVOICES");
            // --> GetInstance() e _instance son static, por lo que
            // puedo obtener la única instancia de la clase, luego
            // llamo a ExecuteSPQuery() sobre esa instancia.


            foreach (DataRow row in dt.Rows)
            {
                Invoice i = new Invoice();
                i.Number = (int)row["Number"];
                i.Date = (DateTime)row["Date"];
                i.Customer = row["Customer"].ToString();
                i.PaymentMethod = new PaymentMethod
                {
                    Id = (int)row["PaymentMethodId"],
                    Name = row["PaymentMethod"].ToString()
                };
                list.Add(i);
            }


            return list;
        }

        /*public Invoice GetById(int id)
        {
            List<Parameter> paramaters = new List<Parameter>()
            {
                //Dentro de esa lista se agrega un único parámetro:
                //Nombre: "@Number"(el parámetro que espera el procedimiento almacenado en SQL)
                //Valor: id(el valor que entró por parámetro al método).
                new Parameter()
                {
                    Name = "@Number",
                    Value = id
                }
            };

            var dt = DataHelper.GetInstance().ExecuteSPQuery("SP_GET_INVOICE", paramaters); //singleton
            //le paso el nombre del SP y la lista de arriba
            
            //var no cambia el tipo real, dt es un DataTable porque
            //el método ExecuteSPQuery está declarado para devolver
            //un DataTable.

            if (dt != null && dt.Rows.Count>0)
                //dt != null --> valida que el objeto dt se creó correctamente.(Si ExecuteSPQuery falló
                //devolvió null(por ejemplo: error en la conexión, SP mal escrito, etc.), entonces dt no existe.

                //dt.Rows.Count > 0 --> valida que el DataTable tiene filas cargadas.
                //Puede pasar que la consulta a la BD se ejecute bien(entonces dt no es null), pero no encuentre resultados.
            {
                Invoice i = new Invoice()
                {
                    Number = (int)dt.Rows[0]["number"],
                    Date = (DateTime)dt.Rows[0]["date"],
                    PaymentMethod = new PaymentMethod()
                    {
                        Id = (int)dt.Rows[0]["paymentMethod"]
                    },
                    Customer = dt.Rows[0]["customer"].ToString()
                };

                return i;
            }

            return null;
        }*/

        public Invoice GetById(int id)
        {
            List<Parameter> parameters = new List<Parameter>()
    {
        new Parameter() { Name = "@Number", Value = id }
    };

            var dt = DataHelper.GetInstance().ExecuteSPQuery("SP_GET_INVOICE", parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                Invoice i = new Invoice()
                {
                    Number = (int)dt.Rows[0]["Number"],                 // exacto alias del SP
                    Date = (DateTime)dt.Rows[0]["Date"],
                    Customer = dt.Rows[0]["Customer"].ToString(),
                    PaymentMethod = new PaymentMethod()
                    {
                        Id = (int)dt.Rows[0]["PaymentMethodId"],       // id correcto
                        Name = dt.Rows[0]["PaymentMethod"].ToString()  // nombre del método
                    },
                    Details = new List<InvoiceDetail>()
                };

                // cargar detalles
                foreach (DataRow row in dt.Rows)
                {
                    InvoiceDetail detail = new InvoiceDetail()
                    {
                        Quantity = (int)row["Quantity"],
                        Article = new Article()
                        {
                            Id = (int)row["ArticleId"],
                            Name = row["Article"].ToString(),
                            UnitPrice = (decimal)row["UnitPrice"]
                        }
                    };

                    i.Details.Add(detail);
                }

                return i;
            }

            return null;
        }


        public bool Save(Invoice invoice)
        {
            List<Parameter> parameters = new List<Parameter>()
{
            new Parameter() { Name = "@Date", Value = invoice.Date },
            new Parameter() { Name = "@PaymentMethodId", Value = invoice.PaymentMethod.Id },
            new Parameter() { Name = "@Customer", Value = invoice.Customer }
            };

            // 2. Ejecuto SP (con el helper que hicimos antes)
            int rows = DataHelper.GetInstance().ExecuteSPDML("SP_SAVE_INVOICE", parameters);

            // 3. Devuelvo true si afectó al menos una fila
            return rows > 0;
        }
    }
}
