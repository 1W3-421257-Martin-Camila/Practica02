using Practica01.Data.Helpers;
using Practica01.Data.Interfaces;
using Practica01.Domain;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public InvoiceDetail? GetById(int id)
        {
            throw new NotImplementedException();
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
            exito = DataHelper.GetInstance().ExecuteSpDML("SP_SAVE_INVOICE_DETAIL", parameters);

            return exito;
        }
    }
}
