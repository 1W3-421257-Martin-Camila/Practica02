using Practica01.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica01.Domain
{
    public class InvoiceDetail
    {
        public int Id { get; set; }
        public Article Article { get; set; }
        public int Quantity { get; set; }
        public Invoice Invoice { get; set; }
    }
}
