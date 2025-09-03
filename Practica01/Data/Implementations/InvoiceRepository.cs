using Practica01.Data.Helpers;
using Practica01.Data.Interfaces;
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

namespace Practica01.Data.Implementations
{
    public class InvoiceRepository : IInvoiceRepository
    {
        public bool Delete(int id)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter() 
                {
                    Name = "@Number",
                    Value = id 
                }
            };

            return DataHelper.GetInstance().ExecuteSpDML("SP_DEACTIVATE_INVOICE", parameters);
        }

        public List<Invoice> GetAll()
        {
            List<Invoice> list = new List<Invoice>();

            var dt = DataHelper.GetInstance().ExecuteSPQuery("SP_GET_INVOICES");
            // --> GetInstance() e _instance son static, por lo que
            // puedo obtener la única instancia de la clase, luego
            // llamo a ExecuteSPQuery() sobre esa instancia.


            foreach (DataRow row in dt.Rows)
            {
                Invoice i = new Invoice();
                i.Number = (int)row["Number"];
                i.Date = (DateTime)row["InvoiceDate"];
                i.Customer = row["Customer"].ToString();
                i.PaymentMethod = new PaymentMethod
                {
                    Id = (int)row["PaymentMethodId"]
                };
                i.IsActive = (bool)row["IsActive"];
                list.Add(i);
            }

            return list;
        }

        public Invoice? GetById(int id)
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

            var dt = DataHelper.GetInstance().ExecuteSPQuery("SP_GET_INVOICE_BY_NUMBER", paramaters); //singleton
                                                                                            //le paso el nombre del SP y la lista de arriba

            //var no cambia el tipo real, dt es un DataTable porque
            //el método ExecuteSPQuery está declarado para devolver
            //un DataTable.

            if (dt != null && dt.Rows.Count > 0)
            //dt != null --> valida que el objeto dt se creó correctamente.(Si ExecuteSPQuery falló
            //devolvió null(por ejemplo: error en la conexión, SP mal escrito, etc.), entonces dt no existe.

            //dt.Rows.Count > 0 --> valida que el DataTable tiene filas cargadas.
            //Puede pasar que la consulta a la BD se ejecute bien(entonces dt no es null), pero no encuentre resultados.
            {
                Invoice i = new Invoice()
                {
                    Number = (int)dt.Rows[0]["Number"], 
                    Date = (DateTime)dt.Rows[0]["InvoiceDate"],
                    Customer = dt.Rows[0]["Customer"].ToString(),
                    PaymentMethod = new PaymentMethod()
                    {
                        Id = (int)dt.Rows[0]["PaymentMethodId"]
                    },
                    IsActive = (bool)dt.Rows[0]["IsActive"]
                };
                return i;
            }

            return null;
        }
        public bool Save(Invoice invoice)
        {
            List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter("@Date", invoice.Date),
                new Parameter("@PaymentMethodId", invoice.PaymentMethod.Id),
                new Parameter("@Customer", invoice.Customer)
            };
            return DataHelper.GetInstance().ExecuteSpDML("SP_SAVE_INVOICE", parameters);
        }
    }
}
