using Practica01.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica01.Data.Interfaces
{
    public interface IInvoiceDetailRepository
    {
        List<InvoiceDetail> GetAll();
        InvoiceDetail? GetById(int id);
        bool Save(InvoiceDetail invoiceDetail);
        bool Delete(int id);
    }
}
