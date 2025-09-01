using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Practica01.Domain
{
    public class Invoice
    {
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public List<InvoiceDetail>? Details { get; set; }
        public string? Customer { get; set; }

        public override string ToString()
        {
            string detalles = "";
            if (Details != null)
            {
                foreach (var d in Details)
                {
                    detalles += d.ToString() + " | ";
                }
            }
            return "Factura N°: " + Number + " - Fecha: " + Date + " - Cliente: " + Customer + " - Pago: " + PaymentMethod + " - Detalles: " + detalles;
        }

    }
}
