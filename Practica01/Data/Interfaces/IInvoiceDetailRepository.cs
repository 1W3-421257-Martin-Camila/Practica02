using CommerceBack.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceBack.Data.Interfaces
{
    public interface IInvoiceDetailRepository
    {
        List<InvoiceDetail> GetAll();
        InvoiceDetail? GetById(int id);
        bool Save(InvoiceDetail invoiceDetail);
        bool Delete(int id);
    }
}
