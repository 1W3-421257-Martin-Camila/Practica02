using Practica01.Data;
using Practica01.Data.Interfaces;
using Practica01.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica01.Servicios
{
    public class InvoiceService
    {
        //el service guarda la lógica del negocio: qué se puede hacer, cómo, con qué límites,
        //y de qué forma se conectan la interfaz y los datos.
        private IInvoiceRepository _invoiceRepository;

        public InvoiceService()
        {
            _invoiceRepository = new InvoiceRepository();
        }

        public Invoice GetInvoiceById(int id)
        {
            return _invoiceRepository.GetById(id);
        }

        public List<Invoice> GetAll()
        {
            return _invoiceRepository.GetAll();
        }

        /*
        public bool SaveInvoice(Invoice invoice)
        {
            bool saved;

            var SavedInvoice = _invoiceRepository.GetById(invoice.Number);

            if (SavedInvoice == null)
            {
                saved = _invoiceRepository.Save(invoice);
            }
            else
            {
                saved = false;
            }
            return saved;
        }
        */

        public bool SaveInvoice(Invoice invoice)
        {
            // Guardamos la factura directamente, sin chequear Number
            return _invoiceRepository.Save(invoice);
        }
    }
}
