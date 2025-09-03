using Practica01.Data.Helpers;
using Practica01.Data.Interfaces;
using Practica01.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica01.Data.Implementations
{
    public class InvoiceDetailRepository : IInvoiceDetailRepository
    {
        //Este SOLO guarda y recupera registros, pero siempre ligados a una factura.
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<InvoiceDetail> GetAll()
        {
            List<InvoiceDetail> list = new List<InvoiceDetail>();

            // Traer registros de la BD
            var dt = DataHelper.GetInstance().ExecuteSPQuery("SP_GET_INVOICEDETAILS");

            // Mapear cada DataRow a un Ingredient
            foreach (DataRow row in dt.Rows)
            {
                InvoiceDetail i = new InvoiceDetail();
                i.Invoice.Number = (int)row["InvoiceNumber"];
                i.Article.Id = (int)row["ArticleId"];
                i.Quantity = (int)row["Quantity"];
                i.Id = (int)row["Id"];

                list.Add(i);
            }

            return list;
        }

        public InvoiceDetail? GetById(int id)
        {
            // Preparar parámetros
            List<Parameter> parameters = new List<Parameter>()
    {
        new Parameter()
        {
            Name = "@Id",
            Value = id
        }
    };

            // Ejecutar SP que filtra por Id
            var dt = DataHelper.GetInstance().ExecuteSPQuery("SP_GET_INVOICE_DETAIL_BY_ID", parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                InvoiceDetail detail = new InvoiceDetail()
                {
                    Id = (int)row["Id"],
                    Quantity = (int)row["Quantity"],
                    Article = new Article()
                    {
                        Id = (int)row["ArticleId"],
                        Name = (string)row["ArticleName"],
                        UnitPrice = (decimal)row["UnitPrice"],
                        IsActive = true // opcional
                    },
                    Invoice = new Invoice()
                    {
                        Number = (int)row["InvoiceNumber"],
                        Date = (DateTime)row["InvoiceDate"],
                        Customer = (string)row["Customer"],
                        PaymentMethod = new PaymentMethod()
                        {
                            Id = (int)row["PaymentMethodId"]
                        },
                        IsActive = true // opcional
                    }
                };

                return detail;
            }

            return null;
        }

        public bool Save(InvoiceDetail invoiceDetail)
        {
            bool exito;
            List<Parameter> parameters = new List<Parameter>()
            {
                    new Parameter()
                    {
                        Name = "@InvoiceNumber",
                        Value = invoiceDetail.Invoice
                    },
                    new Parameter()
                    {
                        Name = "@ArticleId",
                        Value = invoiceDetail.Article
                    },
                    new Parameter()
                    {
                        Name = "@Quantity",
                        Value = invoiceDetail.Quantity
                    }
            };
            exito = DataHelper.GetInstance().ExecuteSpDML("SP_ADD_OR_UPDATE_INVOICE_DETAIL", parameters);

            return exito;
        }
    }
}
