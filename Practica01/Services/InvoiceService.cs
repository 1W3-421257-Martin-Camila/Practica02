using CommerceBack.Data;
using CommerceBack.Data.Implementations;
using CommerceBack.Data.Interfaces;
using CommerceBack.Domain;
using CommerceBack.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceBack.Servicios
{
    public class InvoiceService : IInvoiceService
    {
        private IInvoiceRepository _invoiceRepository;
        public InvoiceService()
        {
            _invoiceRepository = new InvoiceRepository();
        }

        public List<Invoice> GetAll()
        {
            return _invoiceRepository.GetAll();
        }

        public Invoice? GetInvoice(int id)
        {
            return _invoiceRepository.GetById(id);
        }

        public bool SaveInvoice(Invoice invoice)
        {
            return _invoiceRepository.Save(invoice);
        }

        public bool DeleteInvoice(int id)
        {
            // Verifico que exista un producto con el mismo código
            var invoiceEnDb = _invoiceRepository.GetById(id);

            // Si existe, lo elimino
            return invoiceEnDb != null ? _invoiceRepository.Delete(id) : false;
        }
    }
}
