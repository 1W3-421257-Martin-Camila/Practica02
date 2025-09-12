using CommerceBack.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceBack.Services
{
    internal interface IInvoiceService
    {
        public List<Invoice> GetAll();
        public Invoice? GetInvoice(int id);
        public bool SaveInvoice(Invoice invoice);
        public bool DeleteInvoice(int id);
    }
}
