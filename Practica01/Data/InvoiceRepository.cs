using Practica01.Datos;
using Practica01.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica01.Data
{
    public class InvoiceRepository : IInvoiceRepository
    {
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Invoice> GetAll()
        {
            List<Invoice> list = new List<Invoice>();

            var dt = DataHelper.GetInstance().ExecuteSPQuery("");
            // --> GetInstance() e _instance son static, por lo que
            // puedo obtener la única instancia de la clase, luego
            // llamo a ExecuteSPQuery() sobre esa instancia.
        
        
            foreach(DataRow row in dt.Rows)
            {
                Invoice i = new Invoice();
                i.Number = (int)row["number"];
                i.Date = (DateTime)row["date"];
                i.PaymentMethod = new PaymentMethod();
                i.PaymentMethod.Id = (int)row["paymentMethod"];
                i.Customer = row["customer"].ToString();
            }

            return list;
        }

        public Invoice GetById(int id)
        {
            List<Parameter> paramaters = new List<Parameter>()
            {
                new Parameter()
                {
                    Name = "@Number",
                    Value = id
                }
            };

            var dt = DataHelper.GetInstance().ExecuteSPQuery("", paramaters);

            if (dt != null && dt.Rows.Count>0)
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
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }
    }
}
