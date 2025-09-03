using Practica01.Data.Implementations;
using Practica01.Data.Interfaces;
using Practica01.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica01.Services
{
    public class InvoiceDetailService
    {
        

        private IInvoiceDetailRepository _invoiceDetailRepository;

        public InvoiceDetailService()
        {
            _invoiceDetailRepository = new InvoiceDetailRepository();
        }


        // ---- LOGICA DE NEGOCIO ----
        //Deberá controlar que, si un mismo artículo se agrega más de una vez,
        //se deberá incrementar las cantidades del mismo detalle.
        //(Si alguien intenta agregar el mismo artículo más de una vez a
        //la misma factura, en lugar de insertar un nuevo registro, se debe sumar
        //la cantidad al registro existente.)

        public List<InvoiceDetail> GetArticles()
        {
            return _invoiceDetailRepository.GetAll();
        }

        public InvoiceDetail? GetInvoiceDetail(int id)
        {
            return _invoiceDetailRepository.GetById(id);
        }

        public bool SaveArticle(InvoiceDetail article)
        {
            return _invoiceDetailRepository.Save(article);
        }

        public bool DeleteArticle(int id)
        {

            var articleInDB = _invoiceDetailRepository.GetById(id);

            return articleInDB != null ? _invoiceDetailRepository.Delete(id) : false;
        }

    }
}
