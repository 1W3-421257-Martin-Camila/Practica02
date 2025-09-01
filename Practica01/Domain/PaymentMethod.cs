using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica01.Domain
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public override string ToString()
        {
            return "Método de pago: " + Name;
        }
    }
}
