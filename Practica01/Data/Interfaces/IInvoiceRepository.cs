using CommerceBack.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceBack.Data.Interfaces
{
    public interface IInvoiceRepository
    {
        List<Invoice> GetAll();
        Invoice? GetById(int id);
        bool Save(Invoice invoice);
        bool Delete(int id);
    }
}
