using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Practica01.Domain
{
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsActive {  get; set; }

        public Article() { }

        public Article(int id, string name, decimal unitPrice, bool isActive)
        {
            Id = id;
            Name = name;
            UnitPrice = unitPrice;
            IsActive = isActive;
        }

        public override string ToString()
        {
            return "Artículo: " + Name + " - Precio: $" + UnitPrice;
        }
    }
}
